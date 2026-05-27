using MicroSign.Core;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using static MicroSign.App.Consts;

namespace MicroSign
{
    partial class MainWindow
    {
        /// <summary>
        /// ドロップされたファイルの一覧を取得結果
        /// </summary>
        private struct GetDropImageFilesResult
        {
            /// <summary>
            /// 成功フラグ
            /// </summary>
            public readonly bool IsSucess;

            /// <summary>
            /// メッセージ
            /// </summary>
            public readonly string Message;

            /// <summary>
            /// ドロップされた画像ファイルの一覧
            /// </summary>
            public readonly string[]? DropImageFiles;

            /// <summary>
            /// サウンドファイル
            /// 2026.05.22:CS)杉原:サウンド機能追加で追加
            /// </summary>
            public readonly string? SoundFile;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="isSuccess">成功フラグ</param>
            /// <param name="message">メッセージ</param>
            /// <param name="dropImageFiles">ドロップされた画像ファイルの一覧</param>
            /// <param name="soundFile">サウンドファイル(2026.05.22:CS)杉原:サウンド機能追加)</param>
            private GetDropImageFilesResult(bool isSuccess, string message, string[]? dropImageFiles, string? soundFile)
            {
                this.IsSucess = isSuccess;
                this.Message = message;
                this.DropImageFiles = dropImageFiles;
                //2026.05.22:CS)杉原:サウンド機能追加 >>>>> ここから
                //-----
                this.SoundFile = soundFile;
                //2026.05.22:CS)杉原:サウンド機能追加 <<<<< ここまで
            }

            /// <summary>
            /// 失敗
            /// </summary>
            /// <param name="message">メッセージ</param>
            /// <returns></returns>
            public static GetDropImageFilesResult Failed(string message)
            {
                //2026.05.22:CS)杉原:サウンド機能追加 >>>>> ここから
                //GetDropImageFilesResult result = new GetDropImageFilesResult(false, message, null);
                //-----
                GetDropImageFilesResult result = new GetDropImageFilesResult(false, message, null, null);
                //2026.05.22:CS)杉原:サウンド機能追加 <<<<< ここまで
                return result;
            }

            /// <summary>
            /// 成功
            /// </summary>
            /// <param name="dropImageFiles">ドロップされた画像ファイルの一覧</param>
            /// <param name="soundFile">サウンドファイル(2026.05.22:CS)杉原:サウンド機能追加)</param>
            /// <returns></returns>
            public static GetDropImageFilesResult Success(string[]? dropImageFiles, string? soundFile)
            {
                //2026.05.22:CS)杉原:サウンド機能追加 >>>>> ここから
                //GetDropImageFilesResult result = new GetDropImageFilesResult(true, string.Empty, dropImageFiles);
                //-----
                GetDropImageFilesResult result = new GetDropImageFilesResult(true, string.Empty, dropImageFiles, soundFile);
                //2026.05.22:CS)杉原:サウンド機能追加 <<<<< ここまで
                return result;
            }
        }

        /// <summary>
        /// ドロップされたファイルの一覧をプレビュー
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        /// <remarks>
        /// 2026.04.14:CS)杉原:プレビュー用の関数を追加
        /// ListView_PreviewDragOver()の呼び出しは連続する
        /// この時「ドロップファイル数={n}」のログが出力されログをいっぱいにしてしまうのと
        /// 使用しない有効なファイルの一覧を作成するのがもったいないので
        /// 専用の関数を用意しました
        /// </remarks>
        private GetDropImageFilesResult GetDropImageFilesPreview(DragEventArgs e)
        {
            //ファイルのドロップか判定
            {
                bool isFiles = e.Data.GetDataPresent(DataFormats.FileDrop);
                if (isFiles)
                {
                    //ファイルの場合は処理続行
                }
                else
                {
                    //それ以外は処理できないので終了
                    return GetDropImageFilesResult.Failed("ファイル以外のものがドロップされました");
                }
            }

            //ファイルの一覧を取得
            string[]? files = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (files == null)
            {
                //処理すべきファイルがないので終了
                return GetDropImageFilesResult.Failed("ファイル一覧がnullでした");
            }
            else
            {
                //有効の場合は処理続行
            }

            //ファイル数を取得
            int n = files.Length;
            if (CommonConsts.Collection.Empty < n)
            {
                //有効の場合は処理続行
                //大量にログが出るのでコメント化CommonLogger.Debug($"ドロップファイル数={n}");
            }
            else
            {
                //処理すべきファイルがないので終了
                return GetDropImageFilesResult.Failed("ファイル一覧が空でした");
            }

            //画像ファイルを抽出
            //リストは不要List<string> dropImageFiles = new List<string>();
            for (int i = CommonConsts.Index.First; i < n; i += CommonConsts.Index.Step)
            {
                //画像ファイル判定
                // >> ファイル名取得
                string file = files[i];

                // >> 拡張子判定
                try
                {
                    Match m = App.Consts.Files.VaridExtensions.Match(file);
                    if (m.Success)
                    {
                        //成功の場合はリストに追加
                        //dropImageFiles.Add(file);
                        // >> 成功の場合は有効なリストなので成功を返す
                        // >> >> 中身はnull
                        //2026.05.22:CS)杉原:サウンド機能追加 >>>>> ここから
                        //return GetDropImageFilesResult.Success(null);
                        //-----
                        return GetDropImageFilesResult.Success(null, null);
                        //2026.05.22:CS)杉原:サウンド機能追加 <<<<< ここまで
                    }
                    else
                    {
                        //失敗の場合は何もしない
                    }
                }
                catch (Exception ex)
                {
                    //例外は握りつぶす
                    CommonLogger.Warn($"ドロップされたファイルの拡張子判定で例外発生 path='{file}'", ex);
                }

                //2026.05.22:CS)杉原:サウンド機能追加 >>>>> ここから
                //-----
                try
                {
                    Match m = App.Consts.SoundFiles.VaridExtensions.Match(file);
                    if (m.Success)
                    {
                        //成功の場合は成功を返す
                        // >> 中身はnull
                        return GetDropImageFilesResult.Success(null, null);
                    }
                    else
                    {
                        //失敗の場合は何もしない
                    }
                }
                catch (Exception ex)
                {
                    //例外は握りつぶす
                    CommonLogger.Warn($"ドロップされたサウンドファイルの拡張子判定で例外発生 path='{file}'", ex);
                }
                //2026.05.22:CS)杉原:サウンド機能追加 <<<<< ここまで
            }

            //ここまで来たら有効なファイルが存在しないので失敗を返す
            return GetDropImageFilesResult.Failed("画像ファイルが存在しません");
        }

        /// <summary>
        /// ドロップされたファイルの一覧を取得
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private GetDropImageFilesResult GetDropImageFiles(DragEventArgs e)
        {
            //ファイルのドロップか判定
            {
                bool isFiles = e.Data.GetDataPresent(DataFormats.FileDrop);
                if (isFiles)
                {
                    //ファイルの場合は処理続行
                }
                else
                {
                    //それ以外は処理できないので終了
                    return GetDropImageFilesResult.Failed("ファイル以外のものがドロップされました");
                }
            }

            //ファイルの一覧を取得
            string[]? files = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (files == null)
            {
                //処理すべきファイルがないので終了
                return GetDropImageFilesResult.Failed("ファイル一覧がnullでした");
            }
            else
            {
                //有効の場合は処理続行
            }

            //ファイル数を取得
            int n = files.Length;
            if (CommonConsts.Collection.Empty < n)
            {
                //有効の場合は処理続行
                CommonLogger.Debug($"ドロップファイル数={n}");
            }
            else
            {
                //処理すべきファイルがないので終了
                return GetDropImageFilesResult.Failed("ファイル一覧が空でした");
            }

            //2026.05.22:CS)杉原:サウンド機能追加 >>>>> ここから
            //-----
            // >> サウンドファイルは最後に１つだけ覚える
            string? soundFile = null;
            //2026.05.22:CS)杉原:サウンド機能追加 <<<<< ここまで

            //画像ファイルを抽出
            List<string> dropImageFiles = new List<string>();
            for (int i = CommonConsts.Index.First; i < n; i += CommonConsts.Index.Step)
            {
                //画像ファイル判定
                // >> ファイル名取得
                string file = files[i];

                // >> 拡張子判定
                try
                {
                    Match m = App.Consts.Files.VaridExtensions.Match(file);
                    if (m.Success)
                    {
                        //成功の場合はリストに追加
                        dropImageFiles.Add(file);
                    }
                    else
                    {
                        //失敗の場合は何もしない
                    }
                }
                catch (Exception ex)
                {
                    //例外は握りつぶす
                    CommonLogger.Warn($"ドロップされたファイルの拡張子判定で例外発生 path='{file}'", ex);
                }

                //2026.05.22:CS)杉原:サウンド機能追加 >>>>> ここから
                //-----
                try
                {
                    Match m = App.Consts.SoundFiles.VaridExtensions.Match(file);
                    if (m.Success)
                    {
                        //成功の場合は保持する
                        soundFile = file;
                    }
                    else
                    {
                        //失敗の場合は何もしない
                    }
                }
                catch (Exception ex)
                {
                    //例外は握りつぶす
                    CommonLogger.Warn($"ドロップされたサウンドファイルの拡張子判定で例外発生 path='{file}'", ex);
                }
                //2026.05.22:CS)杉原:サウンド機能追加 <<<<< ここまで
            }

            //抽出したファイルが存在するか判定
            {
                //2026.05.22:CS)杉原:サウンド機能追加 >>>>> ここから
                //-----
                bool isSound = string.IsNullOrEmpty(soundFile);
                //2026.05.22:CS)杉原:サウンド機能追加 <<<<< ここまで

                int m = dropImageFiles.Count;
                if (CommonConsts.Collection.Empty < m)
                {
                    //存在する場合は処理続行
                    //2026.05.22:CS)杉原:サウンド機能追加 >>>>> ここから
                    //CommonLogger.Debug($"画像ファイル数={m}");
                    //-----
                    CommonLogger.Debug($"画像ファイル数={m}, サウンドファイル={isSound}");
                    //2026.05.22:CS)杉原:サウンド機能追加 <<<<< ここまで
                }
                else
                {
                    //2026.05.22:CS)杉原:サウンド機能追加 >>>>> ここから
                    ////存在しない場合は失敗を返す
                    //return GetDropImageFilesResult.Failed("画像ファイルが存在しません");
                    //-----
                    // >> サウンドファイルの存在を確認
                    if(isSound)
                    {
                        //存在しない場合は失敗を返す
                        return GetDropImageFilesResult.Failed("対象のファイルが存在しません");
                    }
                    else
                    {
                        //存在する場合は処理続行
                        CommonLogger.Debug($"画像ファイル数=None, サウンドファイル={isSound}");
                    }
                    //2026.05.22:CS)杉原:サウンド機能追加 <<<<< ここまで
                }
            }

            //ここまで来たら成功で返す
            {
                string[] dropImageFileArray = dropImageFiles.ToArray();
                //2026.05.22:CS)杉原:サウンド機能追加 >>>>> ここから
                //return GetDropImageFilesResult.Success(dropImageFileArray);
                //-----
                return GetDropImageFilesResult.Success(dropImageFileArray, soundFile);
                //2026.05.22:CS)杉原:サウンド機能追加 <<<<< ここまで
            }
        }
    }
}
