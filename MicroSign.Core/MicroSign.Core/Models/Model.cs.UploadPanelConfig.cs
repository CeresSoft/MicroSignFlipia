using System;
using System.Diagnostics;
using System.Text;

namespace MicroSign.Core.Models
{
    partial class Model
    {
        /// <summary>
        /// パネル設定をアップロードする結果
        /// </summary>
        public struct UploadPanelConfigResult
        {
            /// <summary>
            /// 成功フラグ
            /// </summary>
            public readonly bool IsSuccess;

            /// <summary>
            /// メッセージ
            /// </summary>
            public readonly string? Message;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="isSuccess">成功フラグ</param>
            /// <param name="message">メッセージ</param>
            private UploadPanelConfigResult(bool isSuccess, string? message)
            {
                this.IsSuccess = isSuccess;
                this.Message = message;
            }

            /// <summary>
            /// 失敗
            /// </summary>
            /// <param name="message">メッセージ</param>
            /// <returns></returns>
            public static UploadPanelConfigResult Failed(string message)
            {
                UploadPanelConfigResult result = new UploadPanelConfigResult(false, message);
                return result;
            }

            /// <summary>
            /// 成功
            /// </summary>
            /// <returns></returns>
            public static UploadPanelConfigResult Success()
            {
                UploadPanelConfigResult result = new UploadPanelConfigResult(true, null);
                return result;
            }
        }


        /// <summary>
        /// パネル設定をアップロードする
        /// </summary>
        /// <param name="mkspiffsPath">MKSPIFFSパス</param>
        /// <param name="esptoolPath">EXPTOOLパス</param>
        /// <param name="com">COMポート</param>
        /// <param name="bardrate">ボーレート</param>
        /// <param name="frequency">フラッシュ動作周波数</param>
        /// <param name="mode">フラッシュ動作モード</param>
        /// <param name="size">SPIFFSサイズ</param>
        /// <param name="offset">SPIFFSオフセット</param>
        /// <returns></returns>
        public UploadPanelConfigResult UploadPanelConfig(string mkspiffsPath, string esptoolPath, string com, int bardrate, string frequency, string mode, string size, string offset)
        {
            //パネル設定のあるディレクトリを取得
            string targetDir = string.Empty;
            {
                //パネル設定ファイル名を取得
                string filename = MicroSignConsts.Path.MatrixLedPanelConfigPath;

                //フルパスに変換
                string fullPath = CommonUtils.GetFullPath(filename);
                LOGGER.Debug($"パネル設定パス='{fullPath}'");

                //ファイルが存在するか判定
                {
                    bool isExists = System.IO.File.Exists(fullPath);
                    if(isExists)
                    {
                        //存在する場合は処理続行
                        LOGGER.Debug($"パネル設定存在 (path='{fullPath}')");
                    }
                    else
                    {
                        //存在しない場合は終了
                        string msg = $"パネル設定がありません (path='{fullPath}')";
                        LOGGER.Warn(msg);
                        return UploadPanelConfigResult.Failed(msg);
                    }
                }

                //ディレクトリ部分だけを取得
                string? dir = System.IO.Path.GetDirectoryName(fullPath);
                if(dir == null)
                {
                    string msg = $"ディレクトリが取得できませんでした (path='{fullPath}')";
                    LOGGER.Warn(msg);
                    return UploadPanelConfigResult.Failed(msg);
                }
                else
                {
                    bool isNull = string.IsNullOrEmpty(dir);
                    if (isNull)
                    {
                        //無効の場合はエラーで終了
                        string msg = $"ディレクトリが空です (path='{fullPath}')";
                        LOGGER.Warn(msg);
                        return UploadPanelConfigResult.Failed(msg);
                    }
                    else
                    {
                        //有効の場合はターゲットディレクトリにする
                        targetDir = dir;
                        LOGGER.Debug($"処理ターゲットディレクトリ (dir='{dir}')");
                    }
                }
            }

            //SPIFFSのバイナリファイルを生成するパスを生成
            string spiffsPath = CommonUtils.GetFullPath(MicroSignConsts.Path.SPIFFSPath);

            //SPIFFSを生成
            {
                //引数を生成
                string arg = $"-c \"{targetDir}\" -s {size} \"{spiffsPath}\"";

                //実行
                var ret = this.ExecuteProcess("SPIFFS生成", mkspiffsPath, arg, MicroSignConsts.WaitTimes.MKSPIFFS);
                if (ret.IsSuccess)
                {
                    //成功の場合処理続行
                    LOGGER.Debug("SPIFFS生成成功");
                }
                else
                {
                    //失敗の場合は終了
                    string msg = $"SPIFFS生成失敗 (理由={ret.Message})";
                    LOGGER.Warn(msg);
                    return UploadPanelConfigResult.Failed(msg);
                }
            }

            //ESPTOOLで書込み
            {
                //引数を生成
                string arg = $"-p {com} -b {bardrate} write_flash -ff {frequency} -fm {mode} {offset} \"{spiffsPath}\"";

                //実行
                var ret = this.ExecuteProcess("SPIFFS書込", esptoolPath, arg, MicroSignConsts.WaitTimes.ESPTOOL);
                if(ret.IsSuccess)
                {
                    //成功の場合処理続行
                    LOGGER.Debug("SPIFFS書込成功");
                }
                else
                {
                    //失敗の場合は終了
                    string msg = $"SPIFFS書込失敗 (理由={ret.Message})";
                    LOGGER.Warn(msg);
                    return UploadPanelConfigResult.Failed(msg);
                }
            }

            //ここまで来たら成功で終了
            {
                string msg = "パネル設定アップロード成功";
                LOGGER.Info(msg);
                return UploadPanelConfigResult.Success();
            }
        }


    }
}
