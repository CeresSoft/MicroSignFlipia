using MediaFoundation;
using System;

namespace MicroSign.Core.MediaFoundations
{
    partial class MP4StreamRender
    {
        /// <summary>
        /// ビデオサイズ取得結果
        /// </summary>
        public struct GetVideoSizeResult
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
            /// ビデオ横幅
            /// </summary>
            public readonly int Width;

            /// <summary>
            /// ビデオ縦幅
            /// </summary>
            public readonly int Height;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="isSuccess"></param>
            /// <param name="errorMessage"></param>
            /// <param name="width"></param>
            /// <param name="height"></param>
            private GetVideoSizeResult(bool isSuccess, string? errorMessage, int width, int height)
            {
                this.IsSuccess  = isSuccess;
                this.ErrorMessage = errorMessage;
                this.Width = width;
                this.Height = height;
            }


            /// <summary>
            /// 失敗
            /// </summary>
            /// <param name="message"></param>
            /// <returns></returns>
            public static GetVideoSizeResult Failed(string message)
            {
                GetVideoSizeResult result = new GetVideoSizeResult(false, message, (int)System.Windows.Size.Empty.Width, (int)System.Windows.Size.Empty.Height);
                return result;
            }

            /// <summary>
            /// 成功
            /// </summary>
            /// <param name="width"></param>
            /// <param name="height"></param>
            /// <returns></returns>
            public static GetVideoSizeResult Success(int width, int height)
            {
                GetVideoSizeResult result = new GetVideoSizeResult(true, null, width, height);
                return result;
            }
        }


        /// <summary>
        /// ビデオサイズ取得
        /// </summary>
        /// <returns></returns>
        public GetVideoSizeResult GetVideoSize()
        {
            //SourceReader取得
            MediaFoundation.ReadWrite.IMFSourceReader? sourceReader = this.SourceReader;
            if (sourceReader == null)
            {
                //無効の場合は空で終了
                string msg = "ビデオサイズ取得 - SourceReader無効";
                LOGGER.Warn(msg);
                return GetVideoSizeResult.Failed(msg);
            }
            else
            {
                //有効の場合は処理続行
                LOGGER.Debug("ビデオサイズ取得 - SourceReader有効");
            }

            //ビデオストリームインデックス取得
            int videoStreamIndex = this.TargetVideoStreamIndex;

            //ビデオサイズ取得
            MediaFoundation.IMFMediaType? mediaType = null;
            try
            {
                //メディアタイプ取得
                try
                {
                    LOGGER.Debug("ビデオサイズ取得 - MediaType取得");
                    sourceReader.GetCurrentMediaType(videoStreamIndex, out mediaType);

                    //メディアタイプ有効判定
                    if (mediaType == null)
                    {
                        //無効の場合は終了
                        string msg = "ビデオサイズ取得 - MediaType無効";
                        LOGGER.Warn(msg);
                        return GetVideoSizeResult.Failed(msg);
                    }
                    else
                    {
                        //有効の場合は処理続行
                        LOGGER.Debug("ビデオサイズ取得 - MediaType有効");
                    }
                }
                catch (Exception ex)
                {
                    string msg = $"ビデオサイズ取得 - MediaType取得で例外発生";
                    LOGGER.WarnEx(msg, ex);
                    return GetVideoSizeResult.Failed(msg);
                }

                //ビデオの縦横サイズ
                uint videoWidth = CommonConsts.Values.Zero.I;
                uint videoHeight = CommonConsts.Values.Zero.I;

                //ビデオの縦横サイズ取得
                try
                {
                    LOGGER.Debug("ビデオサイズ取得 - サイズ取得 - 開始");
                    mediaType.GetSize(MediaFoundation.MFAttributesClsid.MF_MT_FRAME_SIZE, out videoWidth, out videoHeight);
                    LOGGER.Debug($"ビデオサイズ取得 - サイズ取得 - 完了 (width={videoWidth}, height={videoHeight}");
                }
                catch (Exception ex)
                {
                    string msg = $"ビデオサイズ取得 - サイズ取得で例外発生";
                    LOGGER.WarnEx(msg, ex);
                    return GetVideoSizeResult.Failed(msg);
                }

                //成功で終了
                LOGGER.Info($"ビデオサイズ取得 - 完了 (width={videoWidth}, height={videoHeight})");
                return GetVideoSizeResult.Success((int)videoWidth, (int)videoHeight);
            }
            finally
            {
                LOGGER.Debug($"ビデオサイズ取得 - MediaType破棄");
                CommonUtils.SafeComRelease(mediaType);
            }
        }

    }
}
