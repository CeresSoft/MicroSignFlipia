using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MicroSign.Core.Models
{
    partial class Model
    {
        /// <summary>
        /// 画像レンダリング結果
        /// </summary>
        public struct RenderImageResult
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
            /// コンストラクタ
            /// </summary>
            /// <param name="isSuccess">成功フラグ</param>
            /// <param name="message">メッセージ</param>
            private RenderImageResult(bool isSuccess, string? message)
            {
                this.IsSuccess = isSuccess;
                this.Message = message;
            }

            /// <summary>
            /// 失敗
            /// </summary>
            /// <param name="message">メッセージ</param>
            /// <returns></returns>
            public static RenderImageResult Failed(string message)
            {
                RenderImageResult result = new RenderImageResult(false, message);
                return result;
            }

            /// <summary>
            /// 成功
            /// </summary>
            /// <returns></returns>
            public static RenderImageResult Success()
            {
                RenderImageResult result = new RenderImageResult(true, null);
                return result;
            }
        }

        /// <summary>
        /// 画像レンダリング
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="imageSource"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="imageWidth"></param>
        /// <param name="imageHeight"></param>
        /// <returns></returns>
        /// <remarks>
        /// DrawingVisualを使用するためUIスレッド以外でも実行可能
        /// </remarks>
        public RenderImageResult RenderImage(RenderTargetBitmap? bmp, ImageSource? imageSource, double x, double y, double imageWidth, double imageHeight)
        {
            //ビットマップ有効判定
            if (bmp == null)
            {
                //レンダリングターゲットビットマップが無効の場合は処理できないので終了
                string msg = "レンダリングターゲットビットマップが無効です";
                LOGGER.Warn(msg);
                return RenderImageResult.Failed(msg);
            }
            else
            {
                //レンダリングターゲットビットマップが有効の場合は処理続行
                //ログが多いのでコメント化:CommonLogger.Debug("レンダリングターゲットビットマップが有効");
            }

            //横幅取得
            int width = bmp.PixelWidth;
            if (CommonConsts.Values.Zero.I < width)
            {
                //有効の場合は処理続行
                //ログが多いのでコメント化:CommonLogger.Debug($"横幅有効 ({width})");
            }
            else
            {
                //無効の場合は処理できないので終了
                string msg = $"レンダリングターゲットビットマップの横幅が無効です ({width})";
                LOGGER.Warn(msg);
                return RenderImageResult.Failed(msg);
            }

            //縦幅取得
            int height = bmp.PixelHeight;
            if (CommonConsts.Values.Zero.I < height)
            {
                //有効の場合は処理続行
                //ログが多いのでコメント化:CommonLogger.Debug($"縦幅有効 ({height})");
            }
            else
            {
                //無効の場合は処理できないので終了
                string msg = $"レンダリングターゲットビットマップの縦幅が無効です ({height})";
                LOGGER.Warn(msg);
                return RenderImageResult.Failed(msg);
            }

            //描写
            try
            {
                //ビジュアルを作成
                DrawingVisual visual = new DrawingVisual();

                //描画コンテキストを開く
                using(DrawingContext drawingContext = visual.RenderOpen())
                {
                    //画像を描画
                    drawingContext.DrawImage(imageSource, new Rect(x, y, imageWidth, imageHeight));
                }

                //ビットマップに描画
                bmp.Render(visual);
            }
            catch (Exception ex)
            {
                string msg = $"レンダリングターゲットビットマップの描写に失敗しました (Width={width}, Height={height}, X={x}, Y={y}, ImageWidth={imageWidth}, ImageHeight={imageHeight})";
                LOGGER.WarnEx(msg, ex);
                return RenderImageResult.Failed(msg);
            }

            //ここまで来たら成功で終了
            return RenderImageResult.Success();
        }
    }
}
