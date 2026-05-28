using ImageMagick;
using ImageMagick.Drawing;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MicroSign.Core.Models
{
    partial class Model
    {
        /// <summary>
        /// BitmapSource減色結果
        /// </summary>
        /// <remarks>
        /// 2025.08.12:CS)杉原:パレット処理の流れを変更で追加
        /// </remarks>
        public struct ConvertBitmapColorReductionResult
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
            /// 減色画像
            /// </summary>
            /// <remarks>
            /// 指定画像のBgra32データ
            /// </remarks>
            public readonly BitmapSource? ColorReductionImage;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="isSuccess">成功フラグ</param>
            /// <param name="message">メッセージ</param>
            /// <param name="colorReductionImage">減色画像</param>
            private ConvertBitmapColorReductionResult(bool isSuccess, string? message, BitmapSource? colorReductionImage)
            {
                this.IsSuccess = isSuccess;
                this.Message = message;
                this.ColorReductionImage = colorReductionImage;
            }

            /// <summary>
            ///  変換失敗
            /// </summary>
            /// <param name="message">メッセージ</param>
            /// <returns></returns>
            public static ConvertBitmapColorReductionResult Failed(string? message)
            {
                ConvertBitmapColorReductionResult result = new ConvertBitmapColorReductionResult(false, message, null);
                return result;
            }

            /// <summary>
            /// 成功
            /// </summary>
            /// <param name="colorReductionImage">減色画像</param>
            /// <returns></returns>
            public static ConvertBitmapColorReductionResult Success(BitmapSource? colorReductionImage)
            {
                ConvertBitmapColorReductionResult result = new ConvertBitmapColorReductionResult(true, null, colorReductionImage);
                return result;
            }
        }

        /// <summary>
        /// BitmapSource減色
        /// </summary>
        /// <param name="image">減色する画像</param>
        /// <returns>BitmapSource減色結果</returns>
        /// <remarks>
        /// 2025.08.12:CS)杉原:パレット処理の流れを変更で追加
        /// お手軽にGIFで保存して読込することで256色の画像に変換します
        /// https://learn.microsoft.com/ja-jp/dotnet/api/system.windows.media.imaging.gifbitmapencoder.-ctor?view=windowsdesktop-9.0&devlangs=csharp&f1url=%3FappId%3DDev17IDEF1%26l%3DJA-JP%26k%3Dk(System.Windows.Media.Imaging.GifBitmapEncoder.%23ctor)%3Bk(DevLang-csharp)%26rd%3Dtrue
        /// >> 2025.08.13:CS)杉原:FormatConvertedBitmapでDestinationFormatをPixelFormats.Indexed8と
        /// >> 指定する方法を試してみましたが、パレットの指定が必要(=Webセーフカラーなパレットで代用できるが)なことと
        /// >> Freeze()でで以外が発生したので諦めました
        /// 2026.05.27:CS)杉原:OpenCVのK-means減色に変更
        /// >> GIF保存ではWebセーフカラーになるようで色合いがかなり変わってしまうので、よりよいK-means減色に変更
        /// </remarks>
        public ConvertBitmapColorReductionResult ConvertBitmapColorReduction(BitmapSource? image)
        {
            //減色する画像の有効判定
            if (image == null)
            {
                //無効の場合は何もせずに終了
                return ConvertBitmapColorReductionResult.Failed("減色する画像が無効");
            }
            else
            {
                //有効の場合は処理続行
            }

            try
            {
                //2026.06.28:CS)杉原:Magick.NETでの減色に変更 >>>>> ここから
                ////2026.05.27:CS)杉原:OpenCVのK-means減色に変更 >>>>> ここから
                //////GifBitmapEncoderを作成する
                ////GifBitmapEncoder encoder = new GifBitmapEncoder();
                ////
                //////ビットマップフレームを作成
                ////BitmapFrame bmpFrame = BitmapFrame.Create(image);
                ////
                //////GifBitmapEncoderにビットマップフレームを追加
                ////encoder.Frames.Add(bmpFrame);
                ////
                //////変換
                ////using (MemoryStream ms = new MemoryStream())
                ////{
                ////    //GifBitmapEncoderをメモリーストリームに保存
                ////    encoder.Save(ms);
                ////
                ////    //メモリーストリームを先頭に移動
                ////    ms.Seek(CommonConsts.Index.First, SeekOrigin.Begin);
                ////
                ////    //メモリーストリームからBitmapSourceを生成
                ////    BitmapImage colorReductionimage = new BitmapImage();
                ////
                ////    // >> https://pierre3.hatenablog.com/entry/2015/10/25/001207
                ////    // >> BitmapImage.StreamSourceに渡したStreamは解除できない(=nullを渡しても解除できない)ので
                ////    // >>  Streamをラップし、ラップしたStreamのDispose
                ////    using (ImageDataStream ids = new ImageDataStream(ms))
                ////    {
                ////        colorReductionimage.BeginInit();
                ////        colorReductionimage.CacheOption = BitmapCacheOption.OnLoad;
                ////        colorReductionimage.CreateOptions = BitmapCreateOptions.None;
                ////        colorReductionimage.StreamSource = ids;
                ////
                ////        colorReductionimage.EndInit();
                ////        colorReductionimage.Freeze();
                ////    }
                ////
                ////    //終了
                ////    return ConvertBitmapColorReductionResult.Success(colorReductionimage);
                ////}
                ////----------
                ////画像をBgr24に変換
                //FormatConvertedBitmap newFormatedBitmapSource = new FormatConvertedBitmap();
                //newFormatedBitmapSource.BeginInit();
                //newFormatedBitmapSource.Source = image;
                //newFormatedBitmapSource.DestinationFormat = PixelFormats.Bgr24; //OpenCV標準のBGRになるように変換
                //newFormatedBitmapSource.EndInit();
                //
                ////カラーパレット数
                //int paletteCount = CommonConsts.Palettes.Count.Max;
                //OpenCvSharp.Size paletteSize = new OpenCvSharp.Size(paletteCount, CommonConsts.Collection.One);
                //
                ////MATに変換
                //using (OpenCvSharp.Mat srcMat = newFormatedBitmapSource.ToMat())
                //{
                //    //MATサイズを計算
                //    int imageWidth = srcMat.Width;
                //    int imageHeight = srcMat.Height;
                //    int imageSize = imageWidth * imageHeight;
                //    OpenCvSharp.Size matSize = new OpenCvSharp.Size(imageSize, CommonConsts.Collection.One);
                //
                //    //デバッグ用に画像を保存
                //    {
                //        // 色の集合（重複なし）
                //        HashSet<OpenCvSharp.Vec3b> colors = new HashSet<OpenCvSharp.Vec3b>();
                //
                //        for (int y = CommonConsts.Index.First; y < imageHeight; y += CommonConsts.Index.Step)
                //        {
                //            for (int x = CommonConsts.Index.First; x < imageWidth; x += CommonConsts.Index.Step)
                //            {
                //                OpenCvSharp.Vec3b pixel = srcMat.At<OpenCvSharp.Vec3b>(y, x);
                //                colors.Add(pixel);
                //            }
                //        }
                //
                //        int colorCount = colors.Count;
                //        System.Diagnostics.Trace.WriteLine($"Reduction Before Colors={colorCount}");
                //    }
                //
                //
                //    //画像の色(BGR)をFloat[3]の1次元配列に変換する
                //    // >> OpenCVの仕様でBGRの順に入る
                //    using (OpenCvSharp.Mat pixelArray = new OpenCvSharp.Mat(matSize, OpenCvSharp.MatType.CV_32FC3))
                //    {
                //        //画像の色をFloat[3]の1次元配列に設定
                //        {
                //            int i = CommonConsts.Index.First;
                //            for (int y = CommonConsts.Index.First; y < imageHeight; y += CommonConsts.Index.Step)
                //            {
                //                for (int x = CommonConsts.Index.First; x < imageWidth; x += CommonConsts.Index.Step)
                //                {
                //                    OpenCvSharp.Vec3b srcPixel = srcMat.At<OpenCvSharp.Vec3b>(y, x);
                //                    byte b = srcPixel.Item0;
                //                    byte g = srcPixel.Item1;
                //                    byte r = srcPixel.Item2;
                //                    OpenCvSharp.Vec3f pixel = new OpenCvSharp.Vec3f(b, g, r);
                //                    pixelArray.Set<OpenCvSharp.Vec3f>(CommonConsts.Index.First, i, pixel);
                //
                //                    i += CommonConsts.Index.Step;
                //                }
                //            }
                //        }
                //
                //        //クラスタリング
                //        using (OpenCvSharp.Mat clusters = new OpenCvSharp.Mat(imageSize, CommonConsts.Collection.One, OpenCvSharp.MatType.CV_32FC1))
                //        {
                //            //K-means++を実行
                //            OpenCvSharp.Cv2.Kmeans(
                //                pixelArray,                             //画像の色
                //                CommonConsts.Palettes.Count.Max,        //クラスター数(=パレット数)
                //                clusters,                               //クラスター(=グループ化)の結果
                //                OpenCvSharp.TermCriteria.Both(10, 1.0), //K-meansの繰り返し条件(10回または精度が1.0で終了)
                //                10,                                     //初期値を変えて10回実行し、最も良い結果を採用する(=標準敵な値)
                //                OpenCvSharp.KMeansFlags.PpCenters       //K-means++を指定
                //                );
                //
                //            //各クラスタの平均値(=パレット値)を計算
                //            using (OpenCvSharp.Mat colorSums = new OpenCvSharp.Mat(paletteSize, OpenCvSharp.MatType.CV_32FC3))
                //            using (OpenCvSharp.Mat colorCounts = new OpenCvSharp.Mat(paletteSize, OpenCvSharp.MatType.CV_32SC1))
                //            {
                //                //ゼロに初期化
                //                colorSums.SetTo(OpenCvSharp.Scalar.All(CommonConsts.Values.Zero.D));
                //                colorCounts.SetTo(OpenCvSharp.Scalar.All(CommonConsts.Collection.Empty));
                //
                //                //クラスター毎に色を合計する
                //                {
                //                    int i = CommonConsts.Index.First;
                //                    for (int y = CommonConsts.Index.First; y < imageHeight; y += CommonConsts.Index.Step)
                //                    {
                //                        for (int x = CommonConsts.Index.First; x < imageWidth; x += CommonConsts.Index.Step)
                //                        {
                //                            //色を取得
                //                            OpenCvSharp.Vec3b srcPixel = srcMat.At<OpenCvSharp.Vec3b>(y, x);
                //                            byte b = srcPixel.Item0;
                //                            byte g = srcPixel.Item1;
                //                            byte r = srcPixel.Item2;
                //
                //                            //クラスター値(=パレット番号)を取得
                //                            int cluster = clusters.At<int>(i);
                //
                //                            //クラスターの色合計値を取得
                //                            OpenCvSharp.Vec3f v = colorSums.At<OpenCvSharp.Vec3f>(CommonConsts.Index.First, cluster);
                //
                //                            //クラスターの要素数を取得
                //                            int c = colorCounts.At<int>(CommonConsts.Index.First, cluster);
                //
                //                            //計算
                //                            v.Item0 += (float)b;
                //                            v.Item1 += (float)g;
                //                            v.Item2 += (float)r;
                //
                //                            //要素数をカウントアップ
                //                            c += CommonConsts.Collection.Step;
                //
                //                            //計算値を戻す
                //                            colorSums.Set<OpenCvSharp.Vec3f>(CommonConsts.Index.First, cluster, v);
                //                            colorCounts.Set<int>(CommonConsts.Index.First, cluster, c);
                //
                //                            //クラスター配列インデックスを進める
                //                            i += CommonConsts.Index.Step;
                //                        }
                //                    }
                //                }
                //
                //                //パレット計算
                //                using (OpenCvSharp.Mat paletts = new OpenCvSharp.Mat(paletteSize, OpenCvSharp.MatType.CV_8SC3))
                //                {
                //                    //パレットをクリア
                //                    paletts.SetTo(OpenCvSharp.Scalar.All(CommonConsts.Values.Zero.I));
                //
                //                    //色の平均値(=パレット色)にする
                //                    for (int i = CommonConsts.Index.First; i < paletteCount; i += CommonConsts.Index.Step)
                //                    {
                //                        //クラスターに対応する現在値を取得
                //                        OpenCvSharp.Vec3f v = colorSums.At<OpenCvSharp.Vec3f>(CommonConsts.Index.First, i);
                //
                //                        //クラスターに対応する要素数を取得
                //                        float c = (float)colorCounts.At<int>(CommonConsts.Index.First, i);
                //
                //                        //平均にする
                //                        float v0 = v.Item0 / c;
                //                        float v1 = v.Item0 / c;
                //                        float v2 = v.Item2 / c;
                //
                //                        //0～255に制限する
                //                        byte w0 = (byte)Math.Min(Math.Max(v0, byte.MinValue), byte.MaxValue);
                //                        byte w1 = (byte)Math.Min(Math.Max(v1, byte.MinValue), byte.MaxValue);
                //                        byte w2 = (byte)Math.Min(Math.Max(v2, byte.MinValue), byte.MaxValue);
                //
                //                        //パレットに設定
                //                        OpenCvSharp.Vec3b p = new OpenCvSharp.Vec3b(w0, w1, w2);
                //                        paletts.Set<OpenCvSharp.Vec3b>(CommonConsts.Index.First, i, p);
                //                    }
                //
                //                    //ソース画像の色をパレット色に変更する
                //                    {
                //                        int i = CommonConsts.Index.First;
                //                        for (int y = CommonConsts.Index.First; y < imageHeight; y += CommonConsts.Index.Step)
                //                        {
                //                            for (int x = CommonConsts.Index.First; x < imageWidth; x += CommonConsts.Index.Step)
                //                            {
                //                                //クラスター値(=パレット番号)を取得
                //                                int cluster = clusters.At<int>(CommonConsts.Index.First, i);
                //
                //                                //パレット色を取得
                //                                OpenCvSharp.Vec3b pixel = paletts.At<OpenCvSharp.Vec3b>(CommonConsts.Index.First, cluster);
                //
                //                                //設定
                //                                srcMat.Set<OpenCvSharp.Vec3b>(y, x, pixel);
                //
                //                                //インデックスを進める
                //                                i += CommonConsts.Index.Step;
                //                            }
                //                        }
                //                    }
                //
                //
                //                }//パレット計算-end
                //
                //            }//各クラスタの平均値(=パレット値)を計算-end
                //
                //        }//クラスタリング-end
                //
                //    }//画像の色(BGR)をFloat[3]の1次元配列に変換する - end
                //
                //
                //    //MATをBitmapSourceに変換して終了
                //    {
                //        BitmapSource colorReductionimage = BitmapSourceConverter.ToBitmapSource(srcMat);
                //
                //        //デバッグ用に画像を保存
                //        {
                //            //GifBitmapEncoderを作成する
                //            PngBitmapEncoder encoder = new PngBitmapEncoder();
                //
                //            //ビットマップフレームを作成
                //            BitmapFrame bmpFrame = BitmapFrame.Create(colorReductionimage);
                //
                //            //GifBitmapEncoderにビットマップフレームを追加
                //            encoder.Frames.Add(bmpFrame);
                //
                //            //変換
                //            using (System.IO.Stream stm = System.IO.File.Create(@"colorReductionimage.png"))
                //            {
                //                //GifBitmapEncoderをメモリーストリームに保存
                //                encoder.Save(stm);
                //            }
                //
                //            // 色の集合（重複なし）
                //            HashSet<OpenCvSharp.Vec3b> colors = new HashSet<OpenCvSharp.Vec3b>();
                //
                //            for (int y = CommonConsts.Index.First; y < imageHeight; y += CommonConsts.Index.Step)
                //            {
                //                for (int x = CommonConsts.Index.First; x < imageWidth; x += CommonConsts.Index.Step)
                //                {
                //                    OpenCvSharp.Vec3b pixel = srcMat.At<OpenCvSharp.Vec3b>(y, x);
                //                    colors.Add(pixel);
                //                }
                //            }
                //
                //            int colorCount = colors.Count;
                //            System.Diagnostics.Trace.WriteLine($"Reduction After Colors={colorCount}");
                //        }
                //
                //        return ConvertBitmapColorReductionResult.Success(colorReductionimage);
                //    }
                //}
                ////2026.05.27:CS)杉原:OpenCVのK-means減色に変更 <<<<< ここまで
                //----------
                // >> OpenCVのK-means法での減色はグループ化する初期色がランダムで決定されるため結果が安定しないし良くない
                // >> このためMagick.NETを使うことにした
                // >> >> ImageSharpという手もあるのだが、こちらは商用利用不可(=有料)となったので仕様できなかった
                ImageMagick.MagickImage? mImage = null;
                BitmapImage? colorReductionimage = null;
                try
                {
                    //BitmapSourceをIMagickImageに変換
                    using (MemoryStream ms = new MemoryStream())
                    {
                        //PNGで保存する
                        PngBitmapEncoder encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(image));
                        encoder.Save(ms);

                        ////デバッグ用に減色前画像を保存
                        //using (System.IO.Stream stm = System.IO.File.Create(@"ColorReductionImage.png"))
                        //{
                        //    //位置を先頭に戻す
                        //    ms.Position = CommonConsts.Index.First;
                        //
                        //    //ストリームに書込
                        //    ms.WriteTo(stm);
                        //}

                        //位置を先頭に戻す
                        ms.Position = CommonConsts.Index.First;


                        //MagicImageに変換
                        mImage = new ImageMagick.MagickImage(ms);
                    }

                    //8bit（256色）に設定
                    {
                        ImageMagick.QuantizeSettings quantizeSettings = new ImageMagick.QuantizeSettings();
                        {
                            //色数
                            quantizeSettings.Colors = (uint)CommonConsts.Palettes.Count.Max;
                            //設定すると色がおかしくなるのでそのままにする quantizeSettings.ColorSpace = ImageMagick.ColorSpace.RGB;
                            quantizeSettings.DitherMethod = ImageMagick.DitherMethod.No; // 輪郭をクッキリさせたい場合はNo、滑らかにしたい場合はRiemersmaなど
                        }
                        mImage.Quantize(quantizeSettings);
                    }

                    //フォーマットをGIFに設定
                    mImage.Format = MagickFormat.Gif;

                    //結果取得
                    using(MemoryStream ms = new MemoryStream())
                    {
                        //結果をメモリストリームに保存
                        mImage.Write(ms);

                        //デバッグ用に減色後の画像を保存
                        //using (System.IO.Stream stm = System.IO.File.Create(@"ColorReductionImage.gif"))
                        //{
                        //    //位置を先頭に戻す
                        //    ms.Position = CommonConsts.Index.First;
                        //
                        //    //ストリームに書込
                        //    ms.WriteTo(stm);
                        //}

                        //位置を先頭に戻す
                        ms.Position = CommonConsts.Index.First;

                        //BitmapSourceに戻す
                        using(ImageDataStream ids = new ImageDataStream(ms))
                        {
                            colorReductionimage = new BitmapImage();
                            colorReductionimage.BeginInit();
                            colorReductionimage.CacheOption = BitmapCacheOption.OnLoad;
                            colorReductionimage.CreateOptions = BitmapCreateOptions.None;
                            colorReductionimage.StreamSource = ids;

                            colorReductionimage.EndInit();
                            colorReductionimage.Freeze();
                        }
                    }

                    //終了
                    return ConvertBitmapColorReductionResult.Success(colorReductionimage);
                }
                finally
                {
                    //MagicImageを破棄
                    CommonUtils.SafeDispose(mImage);
                }
                //2026.06.28:CS)杉原:Magick.NETでの減色に変更 <<<<< ここまで
            }
            catch (Exception ex)
            {
                //例外は握りつぶす
                return ConvertBitmapColorReductionResult.Failed(CommonLogger.Warn("減色処理で例外発生", ex));
            }

        }
    }
}
