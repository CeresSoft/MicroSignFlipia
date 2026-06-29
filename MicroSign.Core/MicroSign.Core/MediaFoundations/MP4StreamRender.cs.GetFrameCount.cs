using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSign.Core.MediaFoundations
{
    partial class MP4StreamRender
    {
        /// <summary>
        /// ビデオのフレーム数取得
        /// </summary>
        public struct GetFrameCountResult
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
            /// ビデオのフレーム数
            /// </summary>
            public readonly long FrameCount;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="isSuccess"></param>
            /// <param name="errorMessage"></param>
            /// <param name="durationTicks"></param>
            private GetFrameCountResult(bool isSuccess, string? errorMessage, long frameCount)
            {
                this.IsSuccess = isSuccess;
                this.ErrorMessage = errorMessage;
                this.FrameCount = frameCount;
            }

            /// <summary>
            /// 失敗
            /// </summary>
            /// <param name="message"></param>
            /// <returns></returns>
            public static GetFrameCountResult Failed(string message)
            {
                GetFrameCountResult result = new GetFrameCountResult(false, message, CommonConsts.Values.Zero.I);
                return result;
            }

            /// <summary>
            /// 成功
            /// </summary>
            /// <param name="durationTicks"></param>
            /// <returns></returns>
            public static GetFrameCountResult Success(long frameCount)
            {
                GetFrameCountResult result = new GetFrameCountResult(true, null, frameCount);
                return result;
            }
        }

        /// <summary>
        /// ビデオのフレーム数取得
        /// </summary>
        public GetFrameCountResult GetFrameCount()
        {
            //SourceReader取得
            MediaFoundation.ReadWrite.IMFSourceReader? sourceReader = this.SourceReader;
            if (sourceReader == null)
            {
                //無効の場合は空で終了
                string msg = "ビデオのフレーム数取得 - SourceReader無効";
                LOGGER.Warn(msg);
                return GetFrameCountResult.Failed(msg);
            }
            else
            {
                //有効の場合は処理続行
                LOGGER.Debug("ビデオのフレーム数取得 - SourceReader有効");
            }

            //ビデオストリームインデックス取得
            int videoStreamIndex = this.TargetVideoStreamIndex;

            try
            {
                //ビデオ映像の先頭に移動する
                this.SetCurrentPosition(TimeSpan.Zero.Ticks);

                //フレーム数カウント
                long result = CommonConsts.Collection.Empty;

                //ループ
                bool isLoop = true;
                while(isLoop)
                {
                    MediaFoundation.HResult hr = sourceReader.ReadSample(
                        videoStreamIndex,
                        MediaFoundation.ReadWrite.MF_SOURCE_READER_CONTROL_FLAG.None,
                        out int actualStreamIndex,
                        out MediaFoundation.ReadWrite.MF_SOURCE_READER_FLAG streamFlags,
                        out long timestamp,
                        out MediaFoundation.IMFSample sample);

                    //サンプル取得成功判定
                    if (hr == MediaFoundation.HResult.S_OK)
                    {
                        //成功の場合は処理続行
                        LOGGER.Debug("ビデオのフレーム数取得 - サンプル取得成功");
                    }
                    else
                    {
                        //失敗の場合は終了
                        string msg = $"ビデオのフレーム数取得 - サンプル取得失敗 ({hr})";
                        LOGGER.Warn(msg);
                        return GetFrameCountResult.Failed(msg);
                    }

                    //ビデオが終了していないか判定
                    {
                        //終了フラグ抽出
                        MediaFoundation.ReadWrite.MF_SOURCE_READER_FLAG f = streamFlags & MediaFoundation.ReadWrite.MF_SOURCE_READER_FLAG.EndOfStream;
                        if (f == MediaFoundation.ReadWrite.MF_SOURCE_READER_FLAG.None)
                        {
                            //終了していない場合は処理続行
                            LOGGER.Debug("ビデオのフレーム数取得 - ビデオ有効");

                            //カウントアップ
                            result += CommonConsts.Collection.Step;
                        }
                        else
                        {
                            //終了している場合は終了
                            string msg = $"ビデオのフレーム数取得 - ビデオ終了";
                            LOGGER.Info(msg);
                            isLoop = false;
                        }
                    }
                }

                //ここまで来たら終了
                return GetFrameCountResult.Success(result);
            }
            catch (Exception ex)
            {
                string msg = $"ビデオのフレーム数取得で例外発生";
                LOGGER.WarnEx(msg, ex);
                return GetFrameCountResult.Failed(msg);
            }

        }
    }
}
