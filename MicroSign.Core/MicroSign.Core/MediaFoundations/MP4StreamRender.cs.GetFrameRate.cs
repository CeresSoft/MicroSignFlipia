using MediaFoundation;
using System;

namespace MicroSign.Core.MediaFoundations
{
    partial class MP4StreamRender
    {
        /// <summary>
        /// ビデオのフレームレート取得
        /// </summary>
        public struct GetFrameRateResult
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
            /// フレームレート分子
            /// </summary>
            public readonly uint Numerator;

            /// <summary>
            /// フレームレート分母
            /// </summary>
            public readonly uint Denominator;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="isSuccess"></param>
            /// <param name="errorMessage"></param>
            /// <param name="durationTicks"></param>
            private GetFrameRateResult(bool isSuccess, string? errorMessage, uint numerator, uint denominator)
            {
                this.IsSuccess = isSuccess;
                this.ErrorMessage = errorMessage;
                this.Numerator = numerator;
                this.Denominator = denominator;
            }

            /// <summary>
            /// 失敗
            /// </summary>
            /// <param name="message"></param>
            /// <returns></returns>
            public static GetFrameRateResult Failed(string message)
            {
                GetFrameRateResult result = new GetFrameRateResult(false, message, CommonConsts.Values.Zero.I, CommonConsts.Values.Zero.I);
                return result;
            }

            /// <summary>
            /// 成功
            /// </summary>
            /// <param name="durationTicks"></param>
            /// <returns></returns>
            public static GetFrameRateResult Success(uint numerator, uint denominator)
            {
                GetFrameRateResult result = new GetFrameRateResult(true, null, numerator, denominator);
                return result;
            }
        }

        /// <summary>
        /// ビデオのフレームレート取得
        /// </summary>
        public GetFrameRateResult GetFrameRate()
        {
            //SourceReader取得
            MediaFoundation.ReadWrite.IMFSourceReader? sourceReader = this.SourceReader;
            if (sourceReader == null)
            {
                //無効の場合は空で終了
                string msg = "ビデオのフレームレート取得 - SourceReader無効";
                LOGGER.Warn(msg);
                return GetFrameRateResult.Failed(msg);
            }
            else
            {
                //有効の場合は処理続行
                LOGGER.Debug("ビデオのフレームレート取得 - SourceReader有効");
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
                    LOGGER.Debug("ビデオのフレームレート取得 - MediaType取得");
                    sourceReader.GetCurrentMediaType(videoStreamIndex, out mediaType);

                    //メディアタイプ有効判定
                    if (mediaType == null)
                    {
                        //無効の場合は終了
                        string msg = "ビデオのフレームレート取得 - MediaType無効";
                        LOGGER.Warn(msg);
                        return GetFrameRateResult.Failed(msg);
                    }
                    else
                    {
                        //有効の場合は処理続行
                        LOGGER.Debug("ビデオのフレームレート取得 - MediaType有効");
                    }
                }
                catch (Exception ex)
                {
                    string msg = $"ビデオのフレームレート取得 - MediaType取得で例外発生";
                    LOGGER.WarnEx(msg, ex);
                    return GetFrameRateResult.Failed(msg);
                }

                //ビデオのフレームレート
                uint numerator = CommonConsts.Values.Zero.I;
                uint denominator = CommonConsts.Values.Zero.I;

                //ビデオのフレームレート取得
                try
                {
                    LOGGER.Debug("ビデオのフレームレート取得 - フレームレート取得 - 開始");
                    HResult hr = mediaType.GetRatio(MediaFoundation.MFAttributesClsid.MF_MT_FRAME_RATE, out numerator, out denominator);
                    if(hr.Succeeded())
                    {
                        //成功した場合   
                        LOGGER.Debug($"ビデオのフレームレート取得 - フレームレート取得 - 完了 (numerator={numerator}, denominator={denominator})");
                    }
                    else
                    {
                        //失敗した場合
                        string msg = $"ビデオのフレームレート取得 - フレームレート取得 - 失敗 ({hr})";
                        LOGGER.Warn(msg);
                        return GetFrameRateResult.Failed(msg);
                    }
                }
                catch (Exception ex)
                {
                    string msg = $"ビデオのフレームレート取得 - フレームレート取得で例外発生";
                    LOGGER.WarnEx(msg, ex);
                    return GetFrameRateResult.Failed(msg);
                }

                //成功で終了
                LOGGER.Info($"ビデオのフレームレート取得 - 完了 (numerator={numerator}, denominator={denominator})");
                return GetFrameRateResult.Success(numerator, denominator);
            }
            finally
            {
                LOGGER.Debug($"ビデオのフレームレート取得 - MediaType破棄");
                CommonUtils.SafeComRelease(mediaType);
            }

        }


    }
}
