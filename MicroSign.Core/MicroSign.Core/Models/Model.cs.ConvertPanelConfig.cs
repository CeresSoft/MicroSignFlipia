using MediaFoundation.Misc;
using MicroSign.Core.Models.PanelConfigs;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Interop;

namespace MicroSign.Core.Models
{
    partial class Model
    {
        /// <summary>
        /// パネル設定変換結果
        /// </summary>
        public struct ConvertPanelConfigResult
        {
            /// <summary>
            /// 成功フラグ
            /// </summary>
            public readonly bool IsSuccess;

            /// <summary>
            /// エラーメッセージ
            /// </summary>
            public readonly string? ErrorMessage;

            /// <summary>
            /// パネル設定
            /// </summary>
            public readonly PanelConfig? Config;
            
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="isSuccess"></param>
            /// <param name="errorMessage"></param>
            /// <param name="config"></param>
            private ConvertPanelConfigResult(bool isSuccess, string? errorMessage, PanelConfig? config)
            {
                this.IsSuccess = isSuccess;
                this.ErrorMessage = errorMessage;
                this.Config = config;
            }
            
            /// <summary>
            /// 失敗
            /// </summary>
            /// <param name="msg"></param>
            /// <returns></returns>
            public static ConvertPanelConfigResult Failed(string msg)
            {
                ConvertPanelConfigResult result = new ConvertPanelConfigResult(false , msg, null);
                return result;
            }

            /// <summary>
            /// 成功
            /// </summary>
            /// <param name="config"></param>
            /// <returns></returns>
            public static ConvertPanelConfigResult Success(PanelConfig config)
            {
                ConvertPanelConfigResult result = new ConvertPanelConfigResult(true, null, config);
                return result;
            }
        }

        /// <summary>
        /// パネル設定変換
        /// </summary>
        /// <param name="path">パネル設定ファイルパス</param>
        /// <returns></returns>
        public ConvertPanelConfigResult ConvertPanelConfig(string path)
        {
            //パネル設定パス有効判定
            {
                bool isNull = string.IsNullOrEmpty(path);
                if (isNull)
                {
                    //無効の場合は終了
                    string msg = "パネル設定パスが無効です";
                    LOGGER.Warn(msg);
                    return ConvertPanelConfigResult.Failed(msg);
                }
                else
                {
                    //有効の場合は処理続行
                    LOGGER.Debug($"パネル設定パス有効 (path='{path}')");
                }
            }

            //読込
            PanelConfig? config = MicroSign.Core.SerializerUtils.JsonUtil.Load<PanelConfig>(path);
            if (config == null)
            {
                //読込できなかった場合は終了
                string msg = $"パネル設定の読込に失敗しました (path='{path}')";
                LOGGER.Warn(msg);
                return ConvertPanelConfigResult.Failed(msg);
            }
            else
            {
                //読込できた場合は処理続行
                LOGGER.Debug($"パネル設定読込成功 (path='{path}')");
            }

            //パネルの横ドット数取得
            int panelWidth = config.Width;
            {
                //画像ピクセル数のサイズチェックを使ってパネルの横幅が有効か判定
                bool ret = Model.Consts.ImagePixelSizeDic.TryGetValue(panelWidth, out int panelWidthBits);
                if (ret)
                {
                    //取得できた場合は有効なので処理続行
                    LOGGER.Debug($"パネル横ドット数有効 (width={panelWidth})");
                }
                else
                {
                    //取得出来なかった場合は異常
                    string msg = $"パネル横ドット数が無効です (width={panelWidth})";
                    LOGGER.Warn(msg);
                    return ConvertPanelConfigResult.Failed(msg);
                }
            }

            //パネルの縦ドット数取得
            int panelHeight = config.Height;
            {
                //画像ピクセル数のサイズチェックを使ってパネルの横幅が有効か判定
                bool ret = Model.Consts.ImagePixelSizeDic.TryGetValue(panelHeight, out int panelHeightBits);
                if (ret)
                {
                    //取得できた場合は有効なので処理続行
                    LOGGER.Debug($"パネル縦ドット数有効 (width={panelHeight})");
                }
                else
                {
                    //取得出来なかった場合は異常
                    string msg = $"パネル縦ドット数が無効です (width={panelHeight})";
                    LOGGER.Warn(msg);
                    return ConvertPanelConfigResult.Failed(msg);
                }
            }

            //パネルの制御タイプを取得
            // >> 現状エラーなし
            PanelControlTypes controlType = config.ControlType;
            LOGGER.Debug($"パネル制御タイプ (control type={controlType})");

            //マップデータコレクション取得
            MapDataCollection? mapDatas = config.MapDatas;
            if (mapDatas == null)
            {
                //マップデータコレクションが無効の場合終了
                string msg = "マップデータコレクション無効";
                LOGGER.Warn(msg);
                return ConvertPanelConfigResult.Failed(msg);
            }
            else
            {
                //読込できた場合は処理続行
                LOGGER.Debug("マップデータコレクション有効");
            }

            //必要なマップデータ数を計算
            // >> パネルの縦×横の数が必要
            int totalCount = panelWidth * panelHeight;

            //マップデータ数有効判定
            {
                //マップ設定のマップデータ数取得
                int madDataCount = mapDatas.Count;

                //一致しているか判定
                if (totalCount == madDataCount)
                {
                    //一致している場合は正常
                    LOGGER.Debug($"必要マップデータ数有効 (必要数={totalCount}, 定義数={madDataCount})");
                }
                else
                {
                    //不一致の場合は終了
                    string msg = $"必要マップデータ数無効 (必要数={totalCount}, 定義数={madDataCount})";
                    LOGGER.Warn(msg);
                    return ConvertPanelConfigResult.Failed(msg);

                }
            }

            //マップ行数(=最大行 + 1)を取得
            int mapRowCount = CommonConsts.Index.ToCount(mapDatas.Max(x => x.MapRow));

            //マップ列数(=最大列 + 1)を取得
            int mapColumnCount = CommonConsts.Index.ToCount(mapDatas.Max(x => x.MapColumn));

            //定義されているマップのサイズが有効か判定
            {
                //マップサイズを計算
                int mapSize = mapRowCount * mapColumnCount;

                //一致しているか判定
                if (totalCount == mapSize)
                {
                    //一致している場合は正常
                    LOGGER.Debug($"定義マップサイズ有効 (行={mapRowCount}, 列={mapRowCount}, 全数={mapSize}, 必要数={totalCount})");
                }
                else
                {
                    //不一致の場合は終了
                    string msg = $"定義マップサイズ無効 (行={mapRowCount}, 列={mapRowCount}, 全数={mapSize}, 必要数={totalCount})";
                    LOGGER.Warn(msg);
                    return ConvertPanelConfigResult.Failed(msg);
                }
            }

            //マップデーブルを生成
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            using (System.IO.BinaryWriter bw = new System.IO.BinaryWriter(ms))
            {
                //@@ ヘッダ(uint16 x 8のサイズ)
                //バージョン(uint16)
                bw.Write((UInt16)MicroSignConsts.PanelConfig.Versions.V100);

                //マトリクスLED
                {
                    // >> 横幅(uint16)
                    bw.Write((UInt16)panelWidth);

                    // >> 縦幅(uint16)
                    bw.Write((UInt16)panelHeight);

                    // >> 制御タイプ
                    int controlTypeValue = (int)controlType;
                    bw.Write((UInt16)controlTypeValue);

                    // >> 空き
                    bw.Write((UInt16)CommonConsts.Values.Zero.I);
                }

                //マップテーブル
                {
                    // >> 横幅(uint16)
                    bw.Write((UInt16)mapColumnCount);

                    // >> 縦幅(uint16)
                    bw.Write((UInt16)mapRowCount);

                    // >> 空き
                    bw.Write((UInt16)CommonConsts.Values.Zero.I);

                    // >> 空き
                    bw.Write((UInt16)CommonConsts.Values.Zero.I);
                }

                //マップテーブル生成
                for (int r = CommonConsts.Index.First; r < panelHeight; r += CommonConsts.Index.Step)
                {
                    for (int c = CommonConsts.Index.First; c < panelWidth; c += CommonConsts.Index.Step)
                    {
                        //該当するマップ位置のマップデータを取得
                        MapData? mapData = mapDatas.Where(x => ((x.PanelRow == r) && (x.PanelColumn == c))).FirstOrDefault();
                        if (mapData == null)
                        {
                            //マップデータが存在しない場合は終了
                            string msg = $"マップデータなし (パネル行={r}, パネル列={c})";
                            LOGGER.Warn(msg);
                            return ConvertPanelConfigResult.Failed(msg);
                        }
                        else
                        {
                            //マップデータが存在する場合
                            // >> マップ行位置
                            int mapRow = mapData.MapRow;

                            // >> マップ列位置取得
                            int mapColumn = mapData.MapColumn;

                            // >> 設定先のオフセット(=マップ先)を計算
                            // >> オフセットの最大値は128*32=4096なので16bitで足りる
                            // >> もし128*64としても8192なのでuint16で足りる
                            int offset = (mapRow * mapColumnCount) + mapColumn;

                            // >> オフセット出力
                            bw.Write((UInt16)offset);
                        }
                    }
                }

                //書込みを完了する
                bw.Flush();

                //ファイルに書き込み
                try
                {
                    //ESP32側のファイル名は固定なので、あえて名前は変更しない
                    string fname = MicroSignConsts.Path.MatrixLedPanelConfigPath;
                    string writePath = CommonUtils.GetFullPath(fname);
                    LOGGER.Debug($"パネル設定書込パス='{writePath}'");

                    //ディレクトリ取得
                    string? dir = System.IO.Path.GetDirectoryName(writePath);
                    if (dir == null)
                    {
                        //ディレクトリがnullの場合は何もしない
                        LOGGER.Debug("パネル設定書込ディレクトリ無し");
                    }
                    else
                    {
                        //ディレクトリ有効判定
                        bool isNull = string.IsNullOrEmpty(dir);
                        if (isNull)
                        {
                            //無効の場合は何もしない
                            LOGGER.Debug("パネル設定書込ディレクトリ無効");
                        }
                        else
                        {
                            //有効の場合はディレクトリを作成
                            LOGGER.Debug($"パネル設定書込ディレクトリ有効='{dir}'");
                            System.IO.Directory.CreateDirectory(dir);
                        }
                    }

                    //ファイルを出力
                    LOGGER.Debug($"パネル設定書込 - 開始 (path='{writePath}')");
                    using (System.IO.FileStream fs = new System.IO.FileStream(writePath, FileMode.Create))
                    {
                        ms.WriteTo(fs);
                    }
                    LOGGER.Debug($"パネル設定書込 - 完了 (path='{writePath}')");

                    //出力先のフォルダをエクスプローラーで表示
                    if (dir == null)
                    {
                        //ディレクトリがnullの場合は何もしない
                    }
                    else
                    {
                        //ディレクトリ有効判定
                        bool isNull = string.IsNullOrEmpty(dir);
                        if (isNull)
                        {
                            //無効の場合は何もしない
                        }
                        else
                        {
                            //有効の場合はディレクトリを表示
                            System.Diagnostics.Process.Start("explorer.exe", dir);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //例外発生時は終了
                    string msg = $"パネル設定書込失敗";
                    LOGGER.WarnEx(msg, ex);
                    return ConvertPanelConfigResult.Failed(msg);
                }
            }

            //ここまで来たら成功を返す
            return ConvertPanelConfigResult.Success(config);
        }
    }
}
