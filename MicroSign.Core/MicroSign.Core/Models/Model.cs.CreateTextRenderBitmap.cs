using MicroSign.Core.Views.Pages;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MicroSign.Core.Models
{
    /// <summary>
    /// モデル
    /// </summary>
    partial class Model
    {
        /// <summary>
        /// 文字レンダリングビットマップ生成結果
        /// </summary>
        public struct CreateTextRenderBitmapResult
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
            private CreateTextRenderBitmapResult(bool isSuccess, string? message, RenderTargetBitmap? renderBitmap)
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
            public static CreateTextRenderBitmapResult Failed(string message)
            {
                CreateTextRenderBitmapResult result = new CreateTextRenderBitmapResult(false, message, null);
                return result;
            }

            /// <summary>
            /// 成功
            /// </summary>
            /// <param name="renderBitmap">レンダービットマップ</param>
            /// <returns></returns>
            public static CreateTextRenderBitmapResult Success(RenderTargetBitmap? renderBitmap)
            {
                CreateTextRenderBitmapResult result = new CreateTextRenderBitmapResult(true, null, renderBitmap);
                return result;
            }
        }


        /// <summary>
        /// 文字レンダリングビットマップ生成
        /// </summary>
        /// <param name="fontSize">フォントサイズ</param>
        /// <param name="fontColor">フォント色</param>
        /// <param name="displayText">表示文字</param>
        /// <param name="minWidth">横幅</param>
        /// <param name="minHeight">縦幅</param>
        /// <returns></returns>
        public CreateTextRenderBitmapResult CreateTextRenderBitmap(int fontSize, int fontColor, string? displayText, double minWidth, double minHeight)
        {
            //余白なしで生成
            Thickness padding = new Thickness();
            return this.CreateTextRenderBitmap(fontSize, fontColor, displayText, minWidth, minHeight, padding);
        }

        /// <summary>
        /// 文字レンダリングビットマップ生成
        /// </summary>
        /// <param name="fontSize">フォントサイズ</param>
        /// <param name="fontColor">フォント色</param>
        /// <param name="displayText">表示文字</param>
        /// <param name="width">横幅</param>
        /// <param name="height">縦幅</param>
        /// <param name="padding">余白</param>
        /// <returns></returns>
        public CreateTextRenderBitmapResult CreateTextRenderBitmap(int fontSize, int fontColor, string? displayText, double width, double height, Thickness padding)
        {
            //表示文字色取得
            Brush brush = Brushes.White;
            {
                switch (fontColor)
                {
                    case 0: //黒
                        brush = Brushes.Black;
                        break;

                    case 1: //赤
                        brush = Brushes.Red;
                        break;

                    case 2: //緑
                        brush = Brushes.Green;
                        break;

                    case 3: //黄
                        brush = Brushes.Yellow;
                        break;

                    case 4: //青
                        brush = Brushes.Blue;
                        break;

                    case 5: //紫
                        brush = Brushes.Magenta;
                        break;

                    case 6: //水
                        brush = Brushes.Cyan;
                        break;

                    default:
                        //不明の場合は白(=初期値)とする
                        break;
                }
            }

            //文字列コントロールを生成
            AnimationTextRenderUserControl? animationText = null;
            try
            {
                //表示内容反映
                animationText = new AnimationTextRenderUserControl();
                animationText.Text = displayText ?? string.Empty;
                animationText.DisplayTextFontSize = fontSize;
                animationText.DisplayTextColor = brush;
                animationText.Width = width;
                animationText.Height = height;
                animationText.Padding = padding;

                //表示内容から必要サイズを計算
                animationText.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                Size desiredSize = animationText.DesiredSize;

                //コントロールサイズを確定
                animationText.Arrange(new Rect(CommonConsts.Points.Zero.X, CommonConsts.Points.Zero.Y, desiredSize.Width, desiredSize.Height));
            }
            catch (Exception ex)
            {
                string msg = $"文字列コントロールの生成に失敗しました (FontSize={fontSize}, FontColor={fontColor}, DisplayText='{displayText}')";
                LOGGER.WarnEx(msg, ex);
                return CreateTextRenderBitmapResult.Failed(msg);
            }

            //文字列コントロールが収まるビットマップのサイズを計算
            // >> コントロールのWidth/Heightは小数の場合があるので、繰り上げた整数を使用する
            int textWidth = (int)Math.Ceiling(animationText.ActualWidth);
            int textHeight = (int)Math.Ceiling(animationText.ActualHeight);

            //レンダリングターゲットビットマップを生成
            RenderTargetBitmap? renderBitmap = null;
            {
                //レンダリングターゲットビットマップを生成
                var ret = this.CreateRenderTargetBitmap(textWidth, textHeight);
                if (ret.IsSuccess)
                {
                    //成功の場合はレンダリングターゲットビットマップを取得
                    renderBitmap = ret.RenderBitmap;
                }
                else
                {
                    //失敗の場合
                    string msg = $"{ret.Message}";
                    LOGGER.Warn($"レンダリングターゲットビットマップの生成に失敗しました (Width={textWidth}, Height={textHeight}) {msg}");
                    return CreateTextRenderBitmapResult.Failed(msg);
                }
            }

            //レンダリングターゲットビットマップ有効判定
            if (renderBitmap == null)
            {
                //無効の場合は失敗で終了
                string msg = "レンダリングターゲットビットマップが無効です";
                LOGGER.Warn(msg);
                return CreateTextRenderBitmapResult.Failed(msg);
            }
            else
            {
                //有効の場合は続行
            }

            //描写
            try
            {
                //ビットマップに描写
                renderBitmap.Render(animationText);
            }
            catch (Exception ex)
            {
                string msg = $"レンダリングターゲットビットマップの描写に失敗しました";
                LOGGER.WarnEx(msg, ex);
                return CreateTextRenderBitmapResult.Failed(msg);
            }

            //ここまで来たら成功で終了
            return CreateTextRenderBitmapResult.Success(renderBitmap);
        }

    }
}
