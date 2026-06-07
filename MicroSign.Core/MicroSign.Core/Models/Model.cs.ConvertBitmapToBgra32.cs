using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MicroSign.Core.Models
{
    partial class Model
    {
        /// <summary>
        /// BitmapSourceをBGRA32のバイト配列に結果
        /// </summary>
        /// <remarks>
        /// 2025.08.12:CS)杉原:パレット処理の流れを変更で追加
        /// </remarks>
        public struct ConvertBitmapToBgra32Result
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
            /// 画像データ
            /// </summary>
            /// <remarks>
            /// 指定画像のBgra32データ
            /// </remarks>
            public readonly byte[]? OutputImage;

            /// <summary>
            /// 画像データのストライド
            /// </summary>
            public readonly int OutputImageStride;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="isSuccess"></param>
            /// <param name="message"></param>
            /// <param name="outputImage"></param>
            /// <param name="outputImageStride"></param>
            private ConvertBitmapToBgra32Result(bool isSuccess, string? message, byte[]? outputImage, int outputImageStride)
            {
                this.IsSuccess = isSuccess;
                this.Message = message;
                this.OutputImage = outputImage;
                this.OutputImageStride = outputImageStride;
            }

            /// <summary>
            ///  変換失敗
            /// </summary>
            /// <param name="message"></param>
            /// <returns></returns>
            public static ConvertBitmapToBgra32Result Failed(string? message)
            {
                ConvertBitmapToBgra32Result result = new ConvertBitmapToBgra32Result(false, message, null, CommonConsts.Collection.Empty);
                return result;
            }

            /// <summary>
            /// 成功
            /// </summary>
            /// <param name="outputImage"></param>
            /// <param name="outputImageStride"></param>
            /// <returns></returns>
            public static ConvertBitmapToBgra32Result Success(byte[]? outputImage, int outputImageStride)
            {
                ConvertBitmapToBgra32Result result = new ConvertBitmapToBgra32Result(true, null, outputImage, outputImageStride);
                return result;
            }
        }

        /// <summary>
        /// BitmapSourceをBGRA32のバイト配列に変換
        /// </summary>
        /// <param name="image">変換する画像</param>
        /// <returns>BitmapSourceをBGRA32のバイト配列に結果</returns>
        /// <remarks>
        /// 2025.08.12:CS)杉原:パレット処理の流れを変更で追加
        /// </remarks>
        public ConvertBitmapToBgra32Result ConvertBitmapToBgra32(BitmapSource? image)
        {
            //変換する画像の有効判定
            if (image == null)
            {
                //無効の場合は何もせずに終了
                return ConvertBitmapToBgra32Result.Failed("変換する画像が無効");
            }
            else
            {
                //有効の場合は処理続行
            }

            try
            {
                //画像ピクセル取得
                // >> 検証済の値を取得
                int imagePixelWidth = image.PixelWidth;
                int imagePixelHeight = image.PixelHeight;

                //画像フォーマットを変換
                // >> https://learn.microsoft.com/ja-jp/dotnet/desktop/wpf/graphics-multimedia/how-to-convert-a-bitmapsource-to-a-different-pixelformat?view=netframeworkdesktop-4.8&viewFallbackFrom=netdesktop-6.0
                FormatConvertedBitmap newFormatedBitmapSource = new FormatConvertedBitmap();
                newFormatedBitmapSource.BeginInit();
                newFormatedBitmapSource.Source = image;
                newFormatedBitmapSource.DestinationFormat = PixelFormats.Bgra32;
                newFormatedBitmapSource.EndInit();
                newFormatedBitmapSource.Freeze();

                //1ピクセルのバイト数
                // >> RGBA 32bit固定の書き方 32/8=4になります
                // >> ほかのピクセルフォーマットに対応する場合はここのコードを変更してください
                int byteParPixel = newFormatedBitmapSource.DestinationFormat.BitsPerPixel / CommonConsts.BitCount.BYTE;

                //画像のストライドを計算
                int imagePixelStride = imagePixelWidth * byteParPixel;

                //画像取得
                int bgra32Size = imagePixelStride * imagePixelHeight;
                byte[] bgra32 = new byte[bgra32Size];
                newFormatedBitmapSource.CopyPixels(bgra32, imagePixelStride, CommonConsts.Index.First);

                //終了
                return ConvertBitmapToBgra32Result.Success(bgra32, imagePixelStride);
            }
            catch(Exception ex)
            {
                //例外は握りつぶす
                string msg = "BGRA32取得で例外発生";
                LOGGER.WarnEx(msg, ex);
                return ConvertBitmapToBgra32Result.Failed(msg);
            }
        }
    }
}
