using System;
using System.Windows.Media.Imaging;

namespace MicroSign.Core.Models
{
    /// <summary>
    /// モデル
    /// </summary>
    partial class Model
    {
        /// <summary>
        /// レンダリングターゲットビットマップ生成結果
        /// </summary>
        public struct CreateRenderTargetBitmapResult
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
            /// レンダービットマップ
            /// </summary>
            public readonly RenderTargetBitmap? RenderBitmap;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="isSuccess">成功フラグ</param>
            /// <param name="message">メッセージ</param>
            /// <param name="correctedImage">補正画像</param>
            private CreateRenderTargetBitmapResult(bool isSuccess, string? message, RenderTargetBitmap? renderBitmap)
            {
                this.IsSuccess = isSuccess;
                this.Message = message;
                this.RenderBitmap = renderBitmap;
            }

            /// <summary>
            /// 失敗
            /// </summary>
            /// <param name="message">メッセージ</param>
            /// <returns></returns>
            public static CreateRenderTargetBitmapResult Failed(string message)
            {
                CreateRenderTargetBitmapResult result = new CreateRenderTargetBitmapResult(false, message, null);
                return result;
            }

            /// <summary>
            /// 成功
            /// </summary>
            /// <param name="renderBitmap">レンダービットマップ</param>
            /// <returns></returns>
            public static CreateRenderTargetBitmapResult Success(RenderTargetBitmap? renderBitmap)
            {
                CreateRenderTargetBitmapResult result = new CreateRenderTargetBitmapResult(true, null, renderBitmap);
                return result;
            }
        }


        /// <summary>
        /// レンダリングターゲットビットマップ生成
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public CreateRenderTargetBitmapResult CreateRenderTargetBitmap(int width, int height)
        {
            //横幅有効判定
            if (CommonConsts.Values.Zero.I < width)
            {
                //横幅が0超過なら正常
                LOGGER.Debug($"横幅={width} 有効");
            }
            else
            {
                //横幅が0未満の場合は異常なので、状態を失敗にして終了
                string msg = $"横幅={width} 異常";
                LOGGER.Warn(msg);
                return CreateRenderTargetBitmapResult.Failed(msg);
            }

            //縦幅有効判定
            if (CommonConsts.Values.Zero.I < height)
            {
                //横幅が0超過なら正常
                LOGGER.Debug($"縦幅={height} 有効");
            }
            else
            {
                //横幅が0未満の場合は異常なので、状態を失敗にして終了
                string msg = $"縦幅={height} 異常";
                LOGGER.Warn(msg);
                return CreateRenderTargetBitmapResult.Failed(msg);
            }

            //レンダリングターゲットビットマップを生成
            // >> 2023.12.24:CS)杉原:子のビットマップはRender()を使うのでPixelFormatはPbgra32でなければならない
            try
            {
                LOGGER.Debug("レンダリングターゲットビットマップを生成 - 開始");
                RenderTargetBitmap bmp = new RenderTargetBitmap(width, height, CommonConsts.DPIs.DIP, CommonConsts.DPIs.DIP, System.Windows.Media.PixelFormats.Pbgra32);
                LOGGER.Debug("レンダリングターゲットビットマップを生成 - 完了");
                return CreateRenderTargetBitmapResult.Success(bmp);
            }
            catch(Exception ex)
            {
                string msg = $"レンダリングターゲットビットマップを生成で例外発生 (横幅={width}, 縦幅={height})";
                LOGGER.WarnEx(msg, ex);
                return CreateRenderTargetBitmapResult.Failed(msg);
            }
        }
    }
}
