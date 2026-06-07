using MicroSign.Core.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        /// テキストレンダリング結果
        /// </summary>
        public struct RenderTextResult
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
            private RenderTextResult(bool isSuccess, string? message)
            {
                this.IsSuccess = isSuccess;
                this.Message = message;
            }

            /// <summary>
            /// 失敗
            /// </summary>
            /// <param name="message">メッセージ</param>
            /// <returns></returns>
            public static RenderTextResult Failed(string message)
            {
                RenderTextResult result = new RenderTextResult(false, message);
                return result;
            }

            /// <summary>
            /// 成功
            /// </summary>
            /// <returns></returns>
            public static RenderTextResult Success()
            {
                RenderTextResult result = new RenderTextResult(true, null);
                return result;
            }
        }


        /// <summary>
        /// テキストをレンダリング
        /// </summary>
        /// <param name="bmp">レンダリング先のビットマップ</param>
        /// <param name="fontSize">フォントサイズ</param>
        /// <param name="fontColor">フォント色</param>
        /// <param name="displayText">表示文字</param>
        /// <returns></returns>
        public RenderTextResult RenderText(RenderTargetBitmap? bmp, int fontSize, int fontColor, string? displayText)
        {
            //ビットマップ有効判定
            if (bmp == null)
            {
                //レンダリングターゲットビットマップが無効の場合は処理できないので終了
                string msg = "レンダリングターゲットビットマップが無効です";
                LOGGER.Warn(msg);
                return RenderTextResult.Failed(msg);
            }
            else
            {
                //レンダリングターゲットビットマップが有効の場合は処理続行
                //ログが多いのでコメント化:CommonLogger.Debug("レンダリングターゲットビットマップが有効");
            }

            //横幅取得
            int width = bmp.PixelWidth;
            if(CommonConsts.Values.Zero.I < width)
            {
                //有効の場合は処理続行
                //ログが多いのでコメント化:CommonLogger.Debug($"横幅有効 ({width})");
            }
            else
            {
                //無効の場合は処理できないので終了
                string msg = $"レンダリングターゲットビットマップの横幅が無効です ({width})";
                LOGGER.Warn(msg);
                return RenderTextResult.Failed(msg);
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
                return RenderTextResult.Failed(msg);
            }

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

            //描写
            try
            {
                //表示内容反映
                AnimationTextRenderUserControl animationText = new AnimationTextRenderUserControl();
                animationText.Width = width;
                animationText.Height = height;
                animationText.Text = displayText ?? string.Empty;
                animationText.DisplayTextFontSize = fontSize;
                animationText.DisplayTextColor = brush;
                animationText.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                animationText.Arrange(new Rect(CommonConsts.Points.Zero.X, CommonConsts.Points.Zero.Y, width, height));

                //ビットマップに描写
                bmp.Render(animationText);
            }
            catch(Exception ex)
            {
                string msg = $"レンダリングターゲットビットマップの描写に失敗しました (Width={width}, Height={height}, FontSize={fontSize}, FontColor={fontColor}, DisplayText='{displayText}')";
                LOGGER.WarnEx(msg, ex);
                return RenderTextResult.Failed(msg);
            }

            //ここまで来たら成功で終了
            return RenderTextResult.Success();
        }
    }
}
