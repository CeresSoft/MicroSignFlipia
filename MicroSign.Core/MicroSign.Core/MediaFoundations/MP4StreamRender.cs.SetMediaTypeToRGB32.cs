using MediaFoundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace MicroSign.Core.MediaFoundations
{
    partial class MP4StreamRender
    {
        /// <summary>
        /// メディアタイプをRGB32に設定結果
        /// </summary>
        public struct SetMediaTypeRGB32Result
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
            private SetMediaTypeRGB32Result(bool isSuccess, string? errorMessage)
            {
                this.IsSuccess = isSuccess;
                this.ErrorMessage = errorMessage;
            }


            /// <summary>
            /// 失敗
            /// </summary>
            /// <param name="message"></param>
            /// <returns></returns>
            public static SetMediaTypeRGB32Result Failed(string message)
            {
                SetMediaTypeRGB32Result result = new SetMediaTypeRGB32Result(false, message);
                return result;
            }

            /// <summary>
            /// 成功
            /// </summary>
            /// <returns></returns>
            public static SetMediaTypeRGB32Result Success()
            {
                SetMediaTypeRGB32Result result = new SetMediaTypeRGB32Result(true, null);
                return result;
            }
        }

        /// <summary>
        /// メディアタイプをRGB32に設定
        /// </summary>
        /// <param name="videoWidth"></param>
        /// <param name="videoHeight"></param>
        /// <returns></returns>
        public SetMediaTypeRGB32Result SetMediaTypeRGB32(int videoWidth, int videoHeight)
        {
            //SourceReader取得
            MediaFoundation.ReadWrite.IMFSourceReader? sourceReader = this.SourceReader;
            if (sourceReader == null)
            {
                //無効の場合は空で終了
                string msg = "メディアタイプをRGB32に設定 - SourceReader無効";
                CommonLogger.Warn(msg);
                return SetMediaTypeRGB32Result.Failed(msg);
            }
            else
            {
                //有効の場合は処理続行
                CommonLogger.Debug("メディアタイプをRGB32に設定 - SourceReader有効");
            }

            //ビデオストリームインデックス取得
            int videoStreamIndex = this.TargetVideoStreamIndex;

            try
            {
                //出力メディアタイプをRGB32bitに設定
                MediaFoundation.IMFMediaType rgb32Type = MediaFoundation.MF.CreateMediaType();
                try
                {
                    //ビデオを設定
                    rgb32Type.SetGUID(MediaFoundation.MFAttributesClsid.MF_MT_MAJOR_TYPE, MediaFoundation.MFMediaType.Video);

                    //RGB32に設定
                    rgb32Type.SetGUID(MediaFoundation.MFAttributesClsid.MF_MT_SUBTYPE, MediaFoundation.MFMediaType.RGB32);

                    //画像サイズは元サイズのままを設定
                    rgb32Type.SetSize(MediaFoundation.MFAttributesClsid.MF_MT_FRAME_SIZE, (uint)videoWidth, (uint)videoHeight);

                    //SourceReader に「この形式で出力せよ」と命令（ここで内部の縮小・色変換が紐づく）
                    MediaFoundation.HResult hr = sourceReader.SetCurrentMediaType(videoStreamIndex, null, rgb32Type);
                    if (hr == MediaFoundation.HResult.S_OK)
                    {
                        //成功の場合は処理続行
                        CommonLogger.Info($"メディアタイプをRGB32に設定 - 成功");
                    }
                    else
                    {
                        //失敗した場合は終了
                        string msg = $"メディアタイプをRGB32に設定 - 失敗 ({hr})";
                        CommonLogger.Warn(msg);
                        return SetMediaTypeRGB32Result.Failed(msg);
                    }

                    // ビデオストリームを明示的に有効化する
                    sourceReader.SetStreamSelection(videoStreamIndex, true);
                }
                finally
                {
                    CommonUtils.SafeComRelease(rgb32Type);
                }
            }
            catch (Exception ex)
            {
                string msg = $"メディアタイプをRGB32に設定 - 例外発生";
                CommonLogger.Warn(msg, ex);
                return SetMediaTypeRGB32Result.Failed(msg);
            }

            //ここまで来たら成功で終了
            CommonLogger.Debug($"メディアタイプをRGB32に設定 - 完了");
            return SetMediaTypeRGB32Result.Success();
        }

    }
}
