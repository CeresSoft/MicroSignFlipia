using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MicroSign.Core.ViewModels.Pages
{
    partial class AnimationTextPageViewModel
    {
        /// <summary>
        /// 画像保存結果
        /// </summary>
        public struct SaveImageResult
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
            /// レンダリング結果
            /// </summary>
            public readonly RenderTargetBitmap? RenderBitmap;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="isSuccess">成功フラグ</param>
            /// <param name="message">エラーメッセージ</param>
            /// <param name="renderBitmap">レンダリング結果</param>
            private SaveImageResult(bool isSuccess, string? message, RenderTargetBitmap? renderBitmap)
            {
                this.IsSuccess = isSuccess;
                this.ErrorMessage = message;
                this.RenderBitmap = renderBitmap;
            }

            /// <summary>
            /// 失敗
            /// </summary>
            /// <param name="message">エラーメッセージ</param>
            /// <returns></returns>
            public static SaveImageResult Failed(string message)
            {
                SaveImageResult result = new SaveImageResult(false, message, null);
                return result;
            }

            /// <summary>
            /// 成功
            /// </summary>
            /// <param name="renderBitmap">レンダリング結果</param>
            /// <returns></returns>
            public static SaveImageResult Success(RenderTargetBitmap? renderBitmap)
            {
                SaveImageResult result = new SaveImageResult(true, null, renderBitmap);
                return result;
            }
        }

        /// <summary>
        /// 画像を保存
        /// </summary>
        /// <param name="savePath"></param>
        /// <returns></returns>
        public SaveImageResult SaveImage(string savePath)
        {
            //設定値を取得
            bool isScrollEnabled = this.IsScrollEnabled;

            //保存する画像を取得
            RenderTargetBitmap? renderBitmap = this.GetSaveImage(isScrollEnabled);

            //PNGで保存
            try
            {
                PngBitmapEncoder png = new PngBitmapEncoder();
                BitmapFrame frame = BitmapFrame.Create(renderBitmap);
                png.Frames.Add(frame);
                using (System.IO.Stream stm = System.IO.File.Create(savePath))
                {
                    png.Save(stm);
                }
            }
            catch (Exception ex)
            {
                //例外は握りつぶす
                string msg = $"PNG保存で例外が発生しました (path='{savePath}') ({ex})";
                LOGGER.WarnEx(msg, ex);
                return SaveImageResult.Failed(msg);
            }

            //ここまできたら成功
            return SaveImageResult.Success(renderBitmap);
        }

        /// <summary>
        /// 保存する画像を取得
        /// </summary>
        /// <param name="isScrollEnabled"></param>
        /// <returns></returns>
        protected RenderTargetBitmap? GetSaveImage(bool isScrollEnabled)
        {
            if (isScrollEnabled)
            {
                //スクロール有効の場合は続行
            }
            else
            {
                //スクロール無効の場合、プレビュー画像を出力する
                RenderTargetBitmap? bmp = this.RenderBitmap;
                return bmp;
            }

            //プロパティ取得
            int fontSize = this.SelectFontSize;
            int fontColor = this.SelectFontColor;
            string? displayText = this.DisplayText;
            int matrixLedWidth = this.MatrixLedWidth;
            int matrixLedHeight = this.MatrixLedHeight;

            //スクロール用テキスト画像を生成
            //移動方向の前後にマトリクスLEDと同じ幅の余白を追加する
            Thickness padding = new Thickness(matrixLedWidth, CommonConsts.Values.Zero.D, matrixLedWidth, CommonConsts.Values.Zero.D);

            //横幅=未指定、縦幅=マトリクスLED縦幅、スクロール分の余白ありで生成する
            var ret = this.Model.CreateTextRenderBitmap(fontSize, fontColor, displayText, double.NaN, matrixLedHeight, padding);
            if (ret.IsSuccess)
            {
                //成功の場合は続行
                LOGGER.Debug("スクロール用テキスト画像を生成に成功");
            }
            else
            {
                //失敗の場合は終了
                LOGGER.Warn($"スクロール用テキスト画像に失敗 ({ret.Message})");
                return null;
            }

            //レンダリングターゲットビットマップを取得
            RenderTargetBitmap? renderBitmap = ret.RenderBitmap;
            return renderBitmap;
        }
    }
}
