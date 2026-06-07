using MediaFoundation;
using System;

namespace MicroSign.Core.MediaFoundations
{
    partial class MP4StreamRender
    {
        /// <summary>
        /// 再生位置設定結果
        /// </summary>
        public struct SetCurrentPositionResult
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
            /// コンストラクタ
            /// </summary>
            /// <param name="isSuccess"></param>
            /// <param name="errorMessage"></param>
            private SetCurrentPositionResult(bool isSuccess, string? errorMessage)
            {
                this.IsSuccess = isSuccess;
                this.ErrorMessage = errorMessage;
            }

            /// <summary>
            /// 失敗
            /// </summary>
            /// <param name="message"></param>
            /// <returns></returns>
            public static SetCurrentPositionResult Failed(string message)
            {
                SetCurrentPositionResult result = new SetCurrentPositionResult(false, message);
                return result;
            }

            /// <summary>
            /// 成功
            /// </summary>
            /// <returns></returns>
            public static SetCurrentPositionResult Success()
            {
                SetCurrentPositionResult result = new SetCurrentPositionResult(true, null);
                return result;
            }
        }


        /// <summary>
        /// 再生位置設定
        /// </summary>
        /// <param name="selectVideoPosition">再生位置設定</param>
        /// <returns></returns>
        public SetCurrentPositionResult SetCurrentPosition(long selectVideoPosition)
        {
            //SourceReader取得
            MediaFoundation.ReadWrite.IMFSourceReader? sourceReader = this.SourceReader;
            if (sourceReader == null)
            {
                //無効の場合は空で終了
                string msg = "再生位置設定 - SourceReader無効";
                LOGGER.Warn(msg);
                return SetCurrentPositionResult.Failed(msg);
            }
            else
            {
                //有効の場合は処理続行
                LOGGER.Debug("再生位置設定 - SourceReader有効");
            }

            //再生位置設定
            try
            {
                using (MediaFoundation.Misc.PropVariant pv = new MediaFoundation.Misc.PropVariant((long)selectVideoPosition))
                {
                    LOGGER.Debug("再生位置設定 - 開始");
                    HResult hr = sourceReader.SetCurrentPosition(Guid.Empty, pv);
                    if(hr == HResult.S_OK)
                    {
                        //成功の場合
                        LOGGER.Debug($"再生位置設定 - 完了");
                    }
                    else
                    {
                        //失敗の場合
                        string msg = $"再生位置設定 - 失敗 ({hr})";
                        LOGGER.Warn(msg);
                        return SetCurrentPositionResult.Failed(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = $"再生位置設定で例外発生";
                LOGGER.WarnEx(msg, ex);
                return SetCurrentPositionResult.Failed(msg);
            }

            //ここまで来たら成功で終了
            return SetCurrentPositionResult.Success();
        }


    }
}
