using MediaFoundation;
using MicroSign.Core.MediaFoundations;
using MicroSign.Core.Models.AnimationSaveSettings;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Xml.Linq;
using static MicroSign.Core.CommonConsts;
using static MicroSign.Core.MediaFoundations.MP4StreamRender;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MicroSign.Core.ViewModels
{
    partial class MainWindowViewModel
    {
        /// <summary>
        /// MP4アニメーション読込
        /// </summary>
        public struct LoadMp4AnimationResult
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
            /// アニメーションイメージ
            /// </summary>
            public readonly AnimationImageItemCollection? AnimationImages;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="isSuccess"></param>
            /// <param name="message"></param>
            /// <param name="animationImages"></param>
            private LoadMp4AnimationResult(bool isSuccess, string? message, AnimationImageItemCollection? animationImages)
            {
                this.IsSuccess = isSuccess;
                this.ErrorMessage = message;
                this.AnimationImages = animationImages;
            }

            /// <summary>
            /// 失敗
            /// </summary>
            /// <param name="message"></param>
            /// <returns></returns>
            public static LoadMp4AnimationResult Failed(string message)
            {
                LoadMp4AnimationResult result = new LoadMp4AnimationResult(false, message, null);
                return result;
            }

            /// <summary>
            /// 成功
            /// </summary>
            /// <param name="animationImages"></param>
            /// <returns></returns>
            public static LoadMp4AnimationResult Success(AnimationImageItemCollection? animationImages)
            {
                LoadMp4AnimationResult result = new LoadMp4AnimationResult(true, null, animationImages);
                return result;
            }
        }

        /// <summary>
        /// MP4読込
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public LoadMp4AnimationResult LoadMp4Animation(string? path)
        {
            //読込先パス有効判定
            {
                bool isNull = string.IsNullOrEmpty(path);
                if (isNull)
                {
                    //無効の場合は終了
                    string msg = "読込先パスが無効です";
                    CommonLogger.Warn(msg);
                    return LoadMp4AnimationResult.Failed(msg);
                }
                else
                {
                    //有効の場合は処理続行
                    CommonLogger.Debug($"読込先パス有効  (path='{path}')");
                }
            }

            //アニメーション画像コレクション
            // >> コンストラクタで生成しているのでnullチェック不要
            AnimationImageItemCollection animationImages = this.AnimationImages;

            try
            {
                //MediaFoundation開始
                {
                    CommonLogger.Debug($"MediaFoundationスタート");
                    MediaFoundation.MF.Startup();
                }
                try
                {
                    //MP4読込
                    LoadMp4AnimationResult ret = this.LoadMp4AnimationImplFileLoad(path!, animationImages);
                    return ret;
                }
                finally
                {
                    //シャットダウン
                    CommonLogger.Debug($"MediaFoundationシャットダウン");
                    MediaFoundation.MF.Shutdown();
                }
            }
            catch (Exception ex)
            {
                string msg = $"MP4ファイルの解析に失敗しました ({ex})";
                CommonLogger.Warn(msg, ex);
                return LoadMp4AnimationResult.Failed(msg);
            }
        }

        /// <summary>
        /// MP4読込実装 - ファイル読込
        /// </summary>
        /// <param name="path"></param>
        /// <param name="animationImages"></param>
        /// <returns></returns>
        private LoadMp4AnimationResult LoadMp4AnimationImplFileLoad(string path, AnimationImageItemCollection animationImages)
        {
            //属性コンテナを作成
            CommonLogger.Debug($"属性コンテナ生成");
            MFExtern.MFCreateAttributes(out MediaFoundation.IMFAttributes attributes, CommonConsts.Collection.One);
            try
            {
                //属性コンテナにビデオ処理（色変換・リサイズ）を有効にする
                CommonLogger.Debug($"属性コンテナにビデオ処理有効を設定");
                attributes.SetUINT32(MediaFoundation.MFAttributesClsid.MF_SOURCE_READER_ENABLE_VIDEO_PROCESSING, CommonConsts.FlagValue.TRUE); // 1 = TRUE

                //MP4読込
                MediaFoundation.ReadWrite.IMFSourceReader? sourceReader = null;
                try
                {
                    try
                    {
                        CommonLogger.Debug($"MP4ファイルオープン (path={path})");
                        MediaFoundation.HResult ret = MediaFoundation.MF.CreateSourceReaderFromURL(path, attributes, out sourceReader);
                        if (ret == MediaFoundation.HResult.S_OK)
                        {
                            //成功した場合処理続行
                            CommonLogger.Info($"MP4ファイルオープン成功 (path={path})");
                        }
                        else
                        {
                            //失敗した場合は終了
                            string msg = $"MP4ファイルオープン失敗 (path={path}) {ret}";
                            CommonLogger.Warn(msg);
                            return LoadMp4AnimationResult.Failed(msg);
                        }
                    }
                    catch(Exception ex)
                    {
                        string msg = $"MP4ファイルオープンで例外発生 (path={path})";
                        CommonLogger.Warn(msg, ex);
                        return LoadMp4AnimationResult.Failed(msg);
                    }

                    //SourceReader有効判定
                    if (sourceReader == null)
                    {
                        //無効の場合は終了
                        string msg = $"SourceReader無効";
                        CommonLogger.Warn(msg);
                        return LoadMp4AnimationResult.Failed(msg);
                    }
                    else
                    {
                        //有効の場合は処理続行
                        CommonLogger.Debug($"SourceReader有効");
                    }

                    //解析開始
                    using (MP4StreamRender mp4 = new MP4StreamRender(path, sourceReader))
                    {
                        LoadMp4AnimationResult result = this.LoadMp4AnimationImplCheckSize(mp4, animationImages);
                        return result;
                    }
                }
                finally
                {
                    //終了
                    CommonLogger.Debug($"MP4ファイルクローズ (path={path})");
                    CommonUtils.SafeComRelease(sourceReader);
                }
            }
            finally
            {
                //
                CommonLogger.Debug($"MP4ファイルクローズ (path={path})");
                CommonUtils.SafeComRelease(attributes);
            }

        }


        /// <summary>
        /// MP4読込実装 - サイズチェック
        /// </summary>
        /// <param name="mp4">MP4StreamRender</param>
        /// <returns></returns>
        private LoadMp4AnimationResult LoadMp4AnimationImplCheckSize(MP4StreamRender mp4, AnimationImageItemCollection animationImages)
        {
            //MP4StreamRender有効判定
            if (mp4 == null)
            {
                //無効の場合は終了
                string msg = $"SourceReader無効";
                CommonLogger.Warn(msg);
                return LoadMp4AnimationResult.Failed(msg);
            }
            else
            {
                //有効の場合は処理続行
                CommonLogger.Debug($"SourceReader有効");
            }

            //ビデオの長さを取得
            TimeSpan duration = TimeSpan.Zero;
            {
                MicroSign.Core.MediaFoundations.MP4StreamRender.GetDurationResult ret = mp4.GetDuration();
                bool isSuccess = ret.IsSuccess;
                if (isSuccess)
                {
                    //成功の場合は処理続行
                    CommonLogger.Debug($"ビデオ長さ取得 - 成功");
                    duration = TimeSpan.FromTicks(ret.DurationTicks);
                }
                else
                {
                    //失敗した場合は終了
                    string? msg = $"ビデオ長さ取得 - 失敗({ret.ErrorMessage})";
                    CommonLogger.Warn(msg);
                    return LoadMp4AnimationResult.Failed(msg);
                }
            }

            //ビデオの縦横サイズ取得
            int videoWidth = (int)System.Windows.Size.Empty.Width;
            int videoHeight = (int)System.Windows.Size.Empty.Height;
            {
                CommonLogger.Debug($"ビデオサイズ取得 - 開始");
                MicroSign.Core.MediaFoundations.MP4StreamRender.GetVideoSizeResult ret = mp4.GetVideoSize();
                bool isSuccess = ret.IsSuccess;
                if(isSuccess)
                {
                    //成功の場合は処理続行
                    CommonLogger.Debug($"ビデオサイズ取得 - 成功");
                    videoWidth = ret.Width;
                    videoHeight = ret.Height;
                }
                else
                {
                    //失敗した場合は終了
                    string? msg = $"ビデオサイズ取得 - 失敗({ret.ErrorMessage})";
                    CommonLogger.Warn(msg);
                    return LoadMp4AnimationResult.Failed(msg);
                }
            }

            //ビデオ縦横サイズ有効判定
            {
                //横サイズ判定
                if (CommonConsts.Values.Zero.I < videoWidth)
                {
                    //横サイズが有効の場合は処理続行
                    CommonLogger.Info($"ビデオ横サイズ={videoWidth}");
                }
                else
                {
                    //横サイズが無効の場合は終了
                    string msg = $"ビデオの横サイズが0";
                    CommonLogger.Warn(msg);
                    return LoadMp4AnimationResult.Failed(msg);
                }

                //縦サイズ判定
                if (CommonConsts.Values.Zero.I < videoHeight)
                {
                    //横サイズが有効の場合は処理続行
                    CommonLogger.Info($"ビデオ縦サイズ={videoHeight}");
                }
                else
                {
                    //横サイズが無効の場合は終了
                    string msg = $"ビデオの縦サイズが0";
                    CommonLogger.Warn(msg);
                    return LoadMp4AnimationResult.Failed(msg);
                }
            }

            //メディアタイプをRGB32に設定
            {
                SetMediaTypeRGB32Result ret = mp4.SetMediaTypeRGB32(videoWidth, videoHeight);
                bool isSuccess = ret.IsSuccess;
                if(isSuccess)
                {
                    CommonLogger.Info("ビデオの出力をRGB32に設定 - 成功");
                }
                else
                {
                    string msg = $"ビデオの出力をRGB32に設定 - 失敗 ({ret.ErrorMessage})";
                    CommonLogger.Warn(msg);
                    return LoadMp4AnimationResult.Failed(msg);
                }
            }

            //パネルサイズ取得
            int panelWidth = this.MatrixLedWidth;
            int panelHeight = this.MatrixLedHeight;

            //スケール及び位置
            double clipScale = MicroSignConsts.Clip.DefaultScale;
            int clipX = MicroSignConsts.Clip.DefaultX;
            int clipY = MicroSignConsts.Clip.DefaultY;

            //パネルサイズが一致するか判定
            bool isFit = AnimationImageItem.IsFit(panelWidth, panelHeight, (int)videoWidth, (int)videoHeight);
            if (isFit)
            {
                //適合した場合は等倍設定(=デフォルト値)のまま処理続行
                CommonLogger.Info($"パネルサイズに適合 (panel=[{panelWidth},{panelHeight}] == pixel=[{videoWidth}, {videoHeight}])");
            }
            else
            {
                //異なる場合は切り抜きページを表示
                Mp4ClipRequestEventArgs ret = this.RaiseMp4ClipRequest(panelWidth, panelHeight, mp4);
                Mp4ClipRequestState status = ret?.Status ?? Mp4ClipRequestState.Failed; //戻り値が無効の場合は失敗にする
                clipScale = ret?.ClipScale ?? MicroSignConsts.Clip.DefaultScale;
                clipX = ret?.ClipX ?? MicroSignConsts.Clip.DefaultX;
                clipY = ret?.ClipY ?? MicroSignConsts.Clip.DefaultY;
                switch (status)
                {
                    case Mp4ClipRequestState.Cancel:
                        //キャンセルの場合は処理終了
                        {
                            string msg = $"ビデオクリップキャンセル";
                            CommonLogger.Warn(msg);
                            return LoadMp4AnimationResult.Failed(msg);
                        }

                    case Mp4ClipRequestState.Apply:
                        //適用の場合は処理続行
                        CommonLogger.Info($"ビデオクリップ (scale={clipScale}, X={clipX}, Y={clipY})");
                        break;

                    default:
                        //それ以外はすべてエラーにする
                        {
                            string msg = $"ビデオクリップエラー ({status})";
                            CommonLogger.Warn(msg);
                            return LoadMp4AnimationResult.Failed(msg);
                        }
                }
            }

            //アニメーション画像コレクション有効判定
            if (animationImages == null)
            {
                //無効の場合は終了
                string msg = "アニメーション画像コレクション無効";
                CommonLogger.Warn(msg);
                return LoadMp4AnimationResult.Failed(msg);
            }
            else
            {
                //有効の場合は処理続行
                CommonLogger.Debug("アニメーション画像コレクション有効");
            }

            //動画のパスから出力先フォルダーを生成
            string? outputFolder = null;
            string? outputFilename = null;
            string? path = mp4.Path;
            try
            {
                //拡張子をのぞいたファイル名を取得
                outputFilename = System.IO.Path.GetFileNameWithoutExtension(path);
                {
                    bool isNull = string.IsNullOrEmpty(outputFilename);
                    if (isNull)
                    {
                        //無効の場合は修了
                        string msg = $"ファイル名の取得に失敗しました (path='{path}')";
                        CommonLogger.Warn(msg);
                        return LoadMp4AnimationResult.Failed(msg);
                    }
                    else
                    {
                        //取得できた場合は処理続行
                        CommonLogger.Debug($"ファイル名の取得成功  (path='{path}' -> '{outputFilename}')");
                    }
                }

                //ディレクトリ部を取得
                string? dir = System.IO.Path.GetDirectoryName(path);
                {
                    bool isNull = string.IsNullOrEmpty(dir);
                    if (isNull)
                    {
                        //無効の場合は修了
                        string msg = $"ディレクトリの取得に失敗しました (path='{path}')";
                        CommonLogger.Warn(msg);
                        return LoadMp4AnimationResult.Failed(msg);
                    }
                    else
                    {
                        //取得できた場合は処理続行
                        CommonLogger.Debug($"ディレクトリの取得成功  (path='{path}' -> '{dir}')");
                    }
                }

                //出力フォルダ名を生成
                outputFolder = System.IO.Path.Combine(dir!, outputFilename!);
                CommonLogger.Debug($"出力先フォルダ ('{dir}' + '{outputFilename}' -> '{outputFolder}')");

                //出力フォルダを作成
                CommonLogger.Debug($"出力先フォルダの生成  ('{outputFolder}')");
                System.IO.Directory.CreateDirectory(outputFolder);
            }
            catch (Exception ex)
            {
                //例外は握りつぶす
                string msg = $"出力フォルダの作成で例外が発生しました (path='{path}' / outputFolder='{outputFolder}') ({ex})";
                CommonLogger.Warn(msg, ex);
                return LoadMp4AnimationResult.Failed(msg);
            }

            //動画を抽出する
            try
            {
                //画像を取得先生成
                int imageStride = videoWidth * MicroSignConsts.Clip.BitmapStride;
                byte[] rgb32buffer = CommonUtils.CreateBgr32Buff(videoWidth, videoHeight);

                //WritableBitmap生成
                WriteableBitmap wbitmap = CommonUtils.CreateBgr32Bitmap(videoWidth, videoHeight);

                //ビデオ映像の先頭に移動する
                mp4.SetCurrentPosition(TimeSpan.Zero.Ticks);

                // >> TransformedBitmap を使って高品質にリサイズ
                ScaleTransform scaleTransform = new ScaleTransform(clipScale, clipScale);
                TransformedBitmap resizedBitmap = new TransformedBitmap(wbitmap, scaleTransform);

                //切抜画像
                //　>> 切り抜きたい領域(X, Y, Width, Height)
                Int32Rect cropRect = new Int32Rect(clipX, clipY, panelWidth, panelHeight);
                CroppedBitmap croppedBitmap = new CroppedBitmap(resizedBitmap, cropRect);

                //ループしながら画像を抽出
                int frameNo = CommonConsts.Index.First;
                bool isLoop = true;
                long lastMilliseconds = (long)TimeSpan.Zero.TotalMilliseconds;
                string? lastPngPath = null;
                BitmapSource? lastImage = null;
                while (isLoop)
                {
                    //画像を抽出
                    MP4StreamRender.GetVideoImageResult ret = mp4.GetVideoImage(rgb32buffer);
                    MP4StreamRender.GetVideoImageState status = ret.Status;
                    switch (status)
                    {
                        case MP4StreamRender.GetVideoImageState.Success:
                            //成功の場合処理続行
                            CommonLogger.Debug($"ビデオ映像取得成功");
                            break;

                        case MP4StreamRender.GetVideoImageState.EndOfStream:
                            //終了の場合はループ終了
                            CommonLogger.Debug($"ビデオ映像取得終了");
                            isLoop = false;
                            break;

                        default:
                            //それ以外は失敗で終了
                            string msg = $"ビデオ画像の抽出に失敗 ({ret.ErrorMessage})";
                            CommonLogger.Warn(msg);
                            return LoadMp4AnimationResult.Failed(msg);
                    }

                    //ループが有効の場合画像を処理する
                    if(isLoop)
                    {
                        //映像の時間
                        TimeSpan tsNow = TimeSpan.FromTicks(ret.TimestampTicks);
                        long nowMilliseconds = (long)tsNow.TotalMilliseconds;

                        //前回の画像をアニメーション画像コレクションに登録
                        // >> 映像の表示期間は次の映像を取得したときの時間が必要なので
                        // >> このタイミングで登録する
                        {
                            bool isNull = string.IsNullOrEmpty(lastPngPath);
                            if(isNull)
                            {
                                //無効の場合は登録できないので何もしない
                            }
                            else
                            {
                                //有効の場合は登録する
                                // >> 前映像との時間差を計算
                                // >> ミリ秒以下は設定したくないので
                                // >> ミリ秒に統一して計算
                                long deltaMilliseconds = nowMilliseconds - lastMilliseconds;
                                double displayPeriod = TimeSpan.FromMilliseconds(deltaMilliseconds).TotalSeconds;
                                //アニメーション画像アイテム生成
                                AnimationImageItem animationImageItem = AnimationImageItem.FromImageFile
                                (
                                    displayPeriod,
                                    lastPngPath!,
                                    lastImage
                                );
                                //2025.08.12:CS)杉原:パレット処理の流れを変更 <<<<< ここまで

                                //リストに追加
                                this.AddAnimationImage(animationImageItem);
                            }
                        }

                        //ビットマップにコピーする
                        wbitmap.Lock();
                        try
                        {
                            //コピーする矩形領域（画像全体）を定義
                            Int32Rect sourceRect = new Int32Rect((int)CommonConsts.Points.Zero.X, (int)CommonConsts.Points.Zero.Y, videoWidth, videoHeight);

                            //コピー実行
                            wbitmap.WritePixels(sourceRect, rgb32buffer, imageStride, CommonConsts.Index.First);
                        }
                        finally
                        {
                            wbitmap.Unlock();
                        }

                        //縮小画像(=croppedBitmap)をファイルに保存
                        {
                            // 1. PNGエンコーダーのインスタンスを作成
                            PngBitmapEncoder encoder = new PngBitmapEncoder();

                            // 2. WriteableBitmapからフレームを作成し、エンコーダーに追加
                            encoder.Frames.Add(BitmapFrame.Create(croppedBitmap));

                            // 3. ファイルストリームを開いて、画像データを書き出す
                            string saveName = string.Format(CommonConsts.File.PngFileFormat, outputFilename, frameNo);
                            string savePath = System.IO.Path.Combine(outputFolder, saveName);

                            // 4. 保存
                            using (FileStream fs = new FileStream(savePath, FileMode.Create, FileAccess.Write))
                            {
                                encoder.Save(fs);
                            }

                            //保存した画像を読込
                            BitmapImage? image = this.GetImage(savePath);
                            if (image == null)
                            {
                                //取得出来なかった場合は終了
                                string msg = $"画像ファイルの読込に失敗しました (path='{savePath}')";
                                CommonLogger.Warn(msg);
                                return LoadMp4AnimationResult.Failed(msg);
                            }
                            else
                            {
                                //有効の場合は処理続行
                                CommonLogger.Debug($"画像ファイル読込成功  (path='{savePath}')");
                            }

                            // 5. ファイル番号を1進めておく
                            frameNo += CommonConsts.Index.Step;

                            // 6. 保存しておく
                            lastMilliseconds = nowMilliseconds;
                            lastPngPath = savePath;
                            lastImage = image;
                        }
                    }
                    else
                    {
                        //ループが終了する場合は最後の画像を登録する
                        //映像の最後の時間
                        long nowMilliseconds = (long)duration.TotalMilliseconds;

                        //前回の画像をアニメーション画像コレクションに登録
                        // >> 映像の表示期間は次の映像を取得したときの時間が必要なので
                        // >> このタイミングで登録する
                        {
                            bool isNull = string.IsNullOrEmpty(lastPngPath);
                            if (isNull)
                            {
                                //無効の場合は登録できないので何もしない
                            }
                            else
                            {
                                //有効の場合は登録する
                                // >> 前映像との時間差を計算
                                // >> ミリ秒以下は設定したくないので
                                // >> ミリ秒に統一して計算
                                long deltaMilliseconds = nowMilliseconds - lastMilliseconds;
                                double displayPeriod = TimeSpan.FromMilliseconds(deltaMilliseconds).TotalSeconds;
                                //アニメーション画像アイテム生成
                                AnimationImageItem animationImageItem = AnimationImageItem.FromImageFile
                                (
                                    displayPeriod,
                                    lastPngPath!,
                                    lastImage
                                );
                                //2025.08.12:CS)杉原:パレット処理の流れを変更 <<<<< ここまで

                                //リストに追加
                                this.AddAnimationImage(animationImageItem);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //例外は握りつぶす
                string msg = $"ビデオ画像の抽出で例外が発生しました (path='{path}' / outputFolder='{outputFolder}') ({ex})";
                CommonLogger.Warn(msg, ex);
                return LoadMp4AnimationResult.Failed(msg);
            }

            //ここまで来たら成功
            return LoadMp4AnimationResult.Success(animationImages);
        }


    }
}
