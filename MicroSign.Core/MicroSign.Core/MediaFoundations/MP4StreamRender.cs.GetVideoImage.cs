using System;

namespace MicroSign.Core.MediaFoundations
{
    partial class MP4StreamRender
    {
        /// <summary>
        /// GIFアニメーション読込状態
        /// </summary>
        public enum GetVideoImageState
        {
            /// <summary>
            /// 成功
            /// </summary>
            Success,

            /// <summary>
            /// 終了
            /// </summary>
            EndOfStream,

            /// <summary>
            /// 失敗
            /// </summary>
            Failed,
        }

        /// <summary>
        /// ビデオ画像取得結果
        /// </summary>
        public struct GetVideoImageResult
        {
            /// <summary>
            /// 成功フラグ
            /// </summary>
            public readonly GetVideoImageState Status;

            /// <summary>
            /// エラーメッセージ
            /// </summary>
            public readonly string? ErrorMessage;

            /// <summary>
            /// タイムスタンプ(Ticks単位)
            /// </summary>
            public readonly long TimestampTicks;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="isSuccess"></param>
            /// <param name="errorMessage"></param>
            /// <param name="timespanTicks"></param>
            private GetVideoImageResult(GetVideoImageState status, string? errorMessage, long timespanTicks)
            {
                this.Status = status;
                this.ErrorMessage = errorMessage;
                this.TimestampTicks = timespanTicks;
            }

            /// <summary>
            /// 失敗
            /// </summary>
            /// <param name="message"></param>
            /// <returns></returns>
            public static GetVideoImageResult Failed(string message)
            {
                GetVideoImageResult result = new GetVideoImageResult(GetVideoImageState.Failed, message, TimeSpan.Zero.Ticks);
                return result;
            }

            /// <summary>
            /// 終了
            /// </summary>
            /// <returns></returns>
            public static GetVideoImageResult EndOfStream()
            {
                GetVideoImageResult result = new GetVideoImageResult(GetVideoImageState.EndOfStream, null, TimeSpan.Zero.Ticks);
                return result;
            }

            /// <summary>
            /// 成功
            /// </summary>
            /// <param name="timespanTicks"></param>
            /// <returns></returns>
            public static GetVideoImageResult Success(long timespanTicks)
            {
                GetVideoImageResult result = new GetVideoImageResult(GetVideoImageState.Success, null, timespanTicks);
                return result;
            }
        }

        /// <summary>
        /// ビデオ画像取得結果
        /// </summary>
        /// <param name="rgb32buffer"></param>
        /// <returns></returns>
        public GetVideoImageResult GetVideoImage(byte[]? rgb32buffer)
        {
            //ビデオ画像取得結果
            int rgb32bufferLength = CommonUtils.GetCount(rgb32buffer);
            if (CommonConsts.Collection.Empty < rgb32bufferLength)
            {
                //有効の場合は処理続行
                CommonLogger.Debug("ビデオ画像取得 - rgb32buffer有効");
            }
            else
            {
                //無効の場合は空で終了
                string msg = "ビデオ画像取得 - rgb32buffer無効";
                CommonLogger.Warn(msg);
                return GetVideoImageResult.Failed(msg);
            }

            //SourceReader取得
            MediaFoundation.ReadWrite.IMFSourceReader? sourceReader = this.SourceReader;
            if (sourceReader == null)
            {
                //無効の場合は空で終了
                string msg = "ビデオ画像取得 - SourceReader無効";
                CommonLogger.Warn(msg);
                return GetVideoImageResult.Failed(msg);
            }
            else
            {
                //有効の場合は処理続行
                CommonLogger.Debug("ビデオ画像取得 - SourceReader有効");
            }

            //ビデオストリームインデックス取得
            int videoStreamIndex = this.TargetVideoStreamIndex;

            try
            {
                MediaFoundation.HResult hr = sourceReader.ReadSample(
                    videoStreamIndex,
                    MediaFoundation.ReadWrite.MF_SOURCE_READER_CONTROL_FLAG.None,
                    out int actualStreamIndex,
                    out MediaFoundation.ReadWrite.MF_SOURCE_READER_FLAG streamFlags,
                    out long timestamp,
                    out MediaFoundation.IMFSample sample);
                try
                {
                    //サンプル取得成功判定
                    if (hr == MediaFoundation.HResult.S_OK)
                    {
                        //成功の場合は処理続行
                        CommonLogger.Debug("ビデオ画像取得 - サンプル取得成功");
                    }
                    else
                    {
                        //失敗の場合は終了
                        string msg = $"ビデオ画像取得 - サンプル取得失敗 ({hr})";
                        CommonLogger.Warn(msg);
                        return GetVideoImageResult.Failed(msg);
                    }

                    //ビデオが終了していないか判定
                    {
                        //終了フラグ抽出
                        MediaFoundation.ReadWrite.MF_SOURCE_READER_FLAG f = streamFlags & MediaFoundation.ReadWrite.MF_SOURCE_READER_FLAG.EndOfStream;
                        if (f == MediaFoundation.ReadWrite.MF_SOURCE_READER_FLAG.None)
                        {
                            //終了していない場合は処理続行
                            CommonLogger.Debug("ビデオ画像取得 - ビデオ有効");
                        }
                        else
                        {
                            //終了している場合は終了
                            string msg = $"ビデオ画像取得 - ビデオ終了";
                            CommonLogger.Info(msg);
                            return GetVideoImageResult.EndOfStream();
                        }
                    }

                    //サンプル有効判定
                    if(sample == null)
                    {
                        //無効の場合は処理できないので終了
                        string msg = $"ビデオ画像取得 - サンプル無効";
                        CommonLogger.Warn(msg);
                        return GetVideoImageResult.Failed(msg);
                    }
                    else
                    {
                        //有効の場合は処理続行
                        CommonLogger.Debug("ビデオ画像取得 - サンプル有効有効");
                    }

                    //バッファを取得
                    sample.ConvertToContiguousBuffer(out MediaFoundation.IMFMediaBuffer buffer);
                    try
                    {
                        //バッファロック
                        CommonLogger.Debug("ビデオ画像取得 - バッファロック");
                        buffer.Lock(out IntPtr pBufferData, out int maxLength, out int currentLength);
                        try
                        {
                            //IntPtrをbyte配列にコピー
                            CommonLogger.Debug("ビデオ画像取得 - バッファコピー");
                            System.Runtime.InteropServices.Marshal.Copy(pBufferData, rgb32buffer!, CommonConsts.Index.First, rgb32bufferLength);
                        }
                        finally
                        {
                            CommonLogger.Debug("ビデオ画像取得 - バッファアンロック");
                            buffer.Unlock();
                        }
                    }
                    finally
                    {
                        //バッファを解放
                        CommonLogger.Debug("ビデオ画像取得 - バッファ解放");
                        CommonUtils.SafeComRelease(buffer);
                    }
                }
                finally
                {
                    CommonLogger.Debug("ビデオ画像取得 - サンプル解放");
                    CommonUtils.SafeComRelease(sample);
                }

                //ここまで来たら成功で終了
                CommonLogger.Debug("ビデオ画像取得 - 成功");
                return GetVideoImageResult.Success(timestamp);

            }
            catch (Exception ex)
            {
                string msg = $"ビデオ画像取得で例外発生";
                CommonLogger.Warn(msg, ex);
                return GetVideoImageResult.Failed(msg);
            }

        }


    }
}
