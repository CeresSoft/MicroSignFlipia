using MediaFoundation.Misc;
using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MicroSign.Core.Models
{
    partial class Model
    {
        /// <summary>
        /// 画像読込
        /// </summary>
        /// <param name="imagePath">画像パス</param>
        /// <returns></returns>
        public BitmapSource? GetImage(string? imagePath)
        {
            //画像パス有効判定
            if(imagePath == null)
            {
                //無効の場合は即終了
                return null;
            }
            else
            {
                //有効の場合は空チェック
                bool isNull = string.IsNullOrWhiteSpace(imagePath);
                if (isNull)
                {
                    //パスが無効の場合は即終了
                    return null;
                }
                else
                {
                    //パス無効の場合は処理続行
                }
            }

            //画像ファイル読込
            byte[] imageData = imageData = System.IO.File.ReadAllBytes(imagePath);

            //画像データ有効判定
            {
                int n = CommonUtils.GetCount(imageData);
                if (CommonConsts.Collection.Empty < n)
                {
                    //データがある場合は処理続行
                }
                else
                {
                    //データが無い場合は終了
                    return null;
                }
            }

            //20206.06.29:CS)杉原:JPEG画像が回転して取得されることがある >>>>> ここから
            ////画像データに変換
            //BitmapImage image = new BitmapImage();
            //
            //// >> https://pierre3.hatenablog.com/entry/2015/10/25/001207
            //// >> BitmapImage.StreamSourceに渡したStreamは解除できない(=nullを渡しても解除できない)ので
            //// >>  Streamをラップし、ラップしたStreamのDispose
            //using (ImageDataStream ids = new ImageDataStream(imageData))
            //{
            //    image.BeginInit();
            //    image.CacheOption = BitmapCacheOption.OnLoad;
            //    image.CreateOptions = BitmapCreateOptions.None;
            //    image.StreamSource = ids;
            //
            //    image.EndInit();
            //    image.Freeze();
            //}
            //
            ////終了
            //return image;
            //----------
            // Exifのメタデータから回転情報を取り出す
            using (ImageDataStream ids = new ImageDataStream(imageData))
            {
                BitmapDecoder decoder = BitmapDecoder.Create(ids, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                BitmapFrame frame = decoder.Frames[MicroSign.Core.CommonConsts.Index.First];

                //画像の回転
                Transform? transform = new RotateTransform(0);

                BitmapMetadata? metadata = frame.Metadata as BitmapMetadata;
                if (metadata == null)
                {
                    //メタ情報がない場合は何もしない
                }
                else
                {
                    // Orientationのタグは "System.Photo.Orientation" または "/app1/ifd/exif:{uint=274}"
                    try
                    {
                        //回転情報を取得する
                        object orientationObj = metadata.GetQuery(MicroSign.Core.MicroSignConsts.Exif.QueryNames.Orientation);
                        if (orientationObj is ushort orientation)
                        {
                            //回転情報が取得できた場合
                            switch (orientation)
                            {
                                case MicroSign.Core.MicroSignConsts.Exif.Orientation.RIGHT:
                                    // 時計回りに90度
                                    transform = new RotateTransform(MicroSign.Core.MicroSignConsts.Exif.Angle.RIGHT);
                                    break;

                                case MicroSign.Core.MicroSignConsts.Exif.Orientation.DOWN:
                                    // 180度
                                    transform = new RotateTransform(MicroSign.Core.MicroSignConsts.Exif.Angle.DOWN);
                                    break;

                                case MicroSign.Core.MicroSignConsts.Exif.Orientation.LEFT:
                                    // 反時計回りに90度
                                    transform = new RotateTransform(MicroSign.Core.MicroSignConsts.Exif.Angle.LEFT);
                                    break;

                                default:
                                    //それ以外は何もしない
                                    break;
                            }
                        }
                        else
                        {
                            //回転情報がない場合は何もしない
                        }
                    }
                    catch (Exception ex)
                    {
                        //例外は握りつぶす(=エラーにしない)
                        LOGGER.WarnEx("画像のExifのメタデータ取得で例外発生", ex);
                    }
                }

                // 回転させたBitmapSourceを作成して返す
                BitmapSource image = new TransformedBitmap(frame, transform);
                return image;
            }
            //20206.06.29:CS)杉原:JPEG画像が回転して取得されることがある <<<<< ここまで

        }
    }
}
