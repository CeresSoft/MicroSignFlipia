using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MicroSign.Core.MediaFoundations
{
    partial class MP4StreamRender
    {
        /// <summary>
        /// ビデオの長さ取得
        /// </summary>
        public struct GetDurationResult
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
            /// ビデオの長さ(単位:Tick)
            /// </summary>
            public readonly long DurationTicks;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="isSuccess"></param>
            /// <param name="errorMessage"></param>
            /// <param name="durationTicks"></param>
            private GetDurationResult(bool isSuccess, string? errorMessage, long durationTicks)
            {
                this.IsSuccess = isSuccess;
                this.ErrorMessage = errorMessage;
                this.DurationTicks = durationTicks;
            }

            /// <summary>
            /// 失敗
            /// </summary>
            /// <param name="message"></param>
            /// <returns></returns>
            public static GetDurationResult Failed(string message)
            {
                GetDurationResult result = new GetDurationResult(false, message, TimeSpan.Zero.Ticks);
                return result;
            }

            /// <summary>
            /// 成功
            /// </summary>
            /// <param name="durationTicks"></param>
            /// <returns></returns>
            public static GetDurationResult Success(long durationTicks)
            {
                GetDurationResult result = new GetDurationResult(true, null, durationTicks);
                return result;
            }
        }

        /// <summary>
        /// ビデオの長さ取得
        /// </summary>
        public GetDurationResult GetDuration()
        {
            //SourceReader取得
            MediaFoundation.ReadWrite.IMFSourceReader? sourceReader = this.SourceReader;
            if (sourceReader == null)
            {
                //無効の場合は空で終了
                string msg = "ビデオの長さ取得 - SourceReader無効";
                CommonLogger.Warn(msg);
                return GetDurationResult.Failed(msg);
            }
            else
            {
                //有効の場合は処理続行
                CommonLogger.Debug("ビデオの長さ取得 - SourceReader有効");
            }

            try
            {
                //長さ取得先生成
                using (MediaFoundation.Misc.PropVariant pv = new MediaFoundation.Misc.PropVariant())
                {
                    //長さ取得
                    sourceReader.GetPresentationAttribute((int)MediaFoundation.ReadWrite.MF_SOURCE_READER.MediaSource, MediaFoundation.MFAttributesClsid.MF_PD_DURATION, pv);

                    //データ型を判定して取得
                    long durationTicks = CommonConsts.Values.Zero.I;
                    MediaFoundation.Misc.ConstPropVariant.VariantType vt = pv.GetVariantType();
                    switch(vt)
                    {
                        case MediaFoundation.Misc.ConstPropVariant.VariantType.Int64:
                            //Int64 型だった場合
                            {
                                durationTicks = pv.GetLong();
                                CommonLogger.Debug($"ビデオの長さ取得 - long({durationTicks})");
                            }
                            break;

                        case MediaFoundation.Misc.ConstPropVariant.VariantType.UInt64:
                            //符号なし64ビット整数型（UInt64）として格納されている場合
                            {
                                durationTicks = (long)pv.GetULong();
                                CommonLogger.Debug($"ビデオの長さ取得 - ulong({durationTicks})");
                            }
                            break;

                        case MediaFoundation.Misc.ConstPropVariant.VariantType.None:
                            //無効の場合
                            {
                                string msg = "ビデオの長さ取得 - 長さ無し(ライブ配信などの可能性)";
                                CommonLogger.Warn(msg);
                                return GetDurationResult.Failed(msg);
                            }

                        default:
                            //それ以外の場合
                            //無効の場合
                            {
                                string msg = $"ビデオの長さ取得 - 長さが想定外の型です (type={vt})";
                                CommonLogger.Warn(msg);
                                return GetDurationResult.Failed(msg);
                            }
                    }

                    //終了
                    return GetDurationResult.Success(durationTicks);
                }

            }
            catch (Exception ex)
            {
                string msg = $"ビデオの長さ取得で例外発生";
                CommonLogger.Warn(msg, ex);
                return GetDurationResult.Failed(msg);
            }

        }


    }
}
