using MicroSign.Core.Models.AnimationDatas;
using MicroSign.Core.Models.AnimationImagePoints;
using MicroSign.Core.ViewModels;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MicroSign.Core.Models
{
    partial class Model
    {
        /// <summary>
        /// アニメーション用マージ画像生成結果
        /// </summary>
        private struct CreateAnimationMergedBitmapResult
        {
            /// <summary>
            /// 成功フラグ
            /// </summary>
            public bool IsSuccess;

            /// <summary>
            /// メッセージ
            /// </summary>
            public string? Message;

            /// <summary>
            /// マージした画像
            /// </summary>
            public WriteableBitmap? MargeImage;

            /// <summary>
            /// アニメーションデータ
            /// </summary>
            public AnimationDataCollection? AnimationDatas;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="isSuccess">成功フラグ</param>
            /// <param name="message">メッセージ</param>
            /// <param name="margeImage">マージ画像</param>
            /// <param name="animationDatas">アニメーションデータ</param>
            private CreateAnimationMergedBitmapResult(bool isSuccess, string? message, WriteableBitmap? margeImage, AnimationDataCollection? animationDatas)
            {
                this.IsSuccess = isSuccess;
                this.Message = message;
                this.MargeImage = margeImage;
                this.AnimationDatas = animationDatas;
            }

            /// <summary>
            /// 失敗
            /// </summary>
            /// <param name="message">メッセージ</param>
            /// <returns></returns>
            public static CreateAnimationMergedBitmapResult Failed(string message)
            {
                CreateAnimationMergedBitmapResult result = new CreateAnimationMergedBitmapResult(false, message, null, null);
                return result;
            }

            /// <summary>
            /// 成功
            /// </summary>
            /// <param name="margeImage">マージ画像</param>
            /// <param name="animationDatas">アニメーションデータ</param>
            /// <returns></returns>
            public static CreateAnimationMergedBitmapResult Success(WriteableBitmap? margeImage, AnimationDataCollection? animationDatas)
            {
                CreateAnimationMergedBitmapResult result = new CreateAnimationMergedBitmapResult(true, null, margeImage, animationDatas);
                return result;
            }
        }

        //2025.08.12:CS)杉原:パレット処理の流れを変更 >>>>> ここから
        ///// <summary>
        ///// アニメーション用画像生成
        ///// </summary>
        ///// <param name="animationImages">アニメーション画像コレクション</param>
        ///// <param name="formatKind">出力色形式</param>
        ///// <param name="redBits">Rビット数</param>
        ///// <param name="greenBits">Gビット数</param>
        ///// <param name="blueBits">Bビット数</param>
        ///// <returns></returns>
        //private CreateAnimationBitmapResult CreateAnimationBitmap(AnimationImageItemCollection animationImages, OutputColorFormatKind formatKind, int redBits, int greenBits, int blueBits)
        //----------
        /// <summary>
        /// アニメーション用マージ画像生成
        /// </summary>
        /// <param name="animationImages">アニメーション画像コレクション</param>
        /// <param name="matrixLedWidth">マトリクスLED横ドット数</param>
        /// <param name="matrixLedHeight">マトリクスLED縦ドット数</param>
        /// <returns>アニメーション用画像生成結果</returns>
        /// <remarks>
        /// 重複を除いた全画像をマージしたアニメーション用画像を生成します
        /// 
        /// 2025.08.12:CS)杉原:パレット処理の流れを変更
        /// 不要なパラメータを削除しました
        /// </remarks>
        private CreateAnimationMergedBitmapResult CreateAnimationMergedBitmap(AnimationImageItemCollection animationImages, int matrixLedWidth, int matrixLedHeight)
        //2025.08.12:CS)杉原:パレット処理の流れを変更 <<<<< ここまで
        {
            //アニメーション画像コレクションの全要素数取得
            int allAnimationImageCount = CommonUtils.GetCount(animationImages);
            if (CommonConsts.Collection.Empty < allAnimationImageCount)
            {
                //アニメーション画像数が1つ以上ある場合有効なので処理続行
            }
            else
            {
                //それ以外の場合は無効なので終了
                return CreateAnimationMergedBitmapResult.Failed("アニメーション画像がありません");
            }

            //2025.08.12:CS)杉原:パレット処理の流れを変更 >>>>> ここから
            ////先頭の画像サイズを取得
            //// >> 先頭の画像を基準に残りの画像が同じサイズか判定します
            //int imageWidth = CommonConsts.Values.Zero.I;
            //int imageHeight = CommonConsts.Values.Zero.I;
            //{
            //    //先頭のアニメーション画像を取得
            //    AnimationImageItem animationImage = animationImages[CommonConsts.Index.First];
            //    if (animationImage == null)
            //    {
            //        //無効の場合は即終了
            //        return CreateAnimationMergedBitmapResult.Failed("先頭のアニメーション画像が無効です");
            //    }
            //    else
            //    {
            //        //有効の場合は処理続行
            //    }
            //
            //    //画像を取得
            //    BitmapSource? bmp = animationImage.Image;
            //    if (bmp == null)
            //    {
            //        //無効の場合は即終了
            //        return CreateAnimationMergedBitmapResult.Failed("先頭の画像が無効です");
            //    }
            //    else
            //    {
            //        //有効の場合は処理続行
            //    }
            //
            //    //ピクセルサイズ取得
            //    imageWidth = bmp.PixelWidth;
            //    imageHeight = bmp.PixelHeight;
            //
            //    //画像サイズが有効判定
            //    ValidateImageSizeResult validateImageResult = this.ValidateImageSize(imageWidth, imageHeight);
            //    if (validateImageResult.IsValid)
            //    {
            //        //有効の場合は処理続行
            //    }
            //    else
            //    {
            //        //無効の場合は即終了
            //        return CreateAnimationMergedBitmapResult.Failed(validateImageResult.ErrorMessage);
            //    }
            //}
            //----------
            // >> LEDパネルのサイズを基準にする
            int imageWidth = matrixLedWidth;
            int imageHeight = matrixLedHeight;
            //2025.08.12:CS)杉原:パレット処理の流れを変更 <<<<< ここまで

            //重複画像を除く
            // >> 辞書で作成しようとしたが、辞書だと順番が保持されないのでListにしました
            AnimationImagePointCollection animationImagePoints = new AnimationImagePointCollection();
            for(int i = CommonConsts.Index.First; i< allAnimationImageCount; i+= CommonConsts.Index.Step)
            {
                //アニメーション画像を取得
                AnimationImageItem animationImage = animationImages[i];
                if (animationImage == null)
                {
                    //無効の場合は即終了
                    return CreateAnimationMergedBitmapResult.Failed($"アニメーション画像[No.{CommonConsts.Index.ToCollection(i)}]が無効です");
                }
                else
                {
                    //有効の場合は処理続行
                }

                //2023.12.24:CS)杉原:テキストの場合画像パスが無いので出力データだけで判定する >>>>> ここから
                ////パスを取得
                //string? imagePath = animationImage.Path;
                //{
                //    bool isNull = string.IsNullOrEmpty(imagePath);
                //    if (isNull)
                //    {
                //        //無効の場合は即終了
                //        return CreateAnimationBitmapResult.Failed($"アニメーション画像[No.{CommonConsts.Index.ToCollection(i)}]の画像パスが無効です");
                //    }
                //    else
                //    {
                //        //有効の場合は処理続行
                //    }
                //}
                //----------
                //2023.12.24:CS)杉原:テキストの場合画像パスが無いので出力データだけで判定する <<<<< ここまで

                //画像
                BitmapSource? bmp = animationImage.Image;
                if(bmp == null)
                {
                    //画像が無効の場合は終了
                    return CreateAnimationMergedBitmapResult.Failed($"アニメーション画像[No.{CommonConsts.Index.ToCollection(i)}]の画像が無効です");
                }
                else
                {
                    //有効の場合は処理続行
                }

                //画像のピクセルサイズを確認
                {
                    //画像のピクセルを取得
                    int width = bmp.PixelWidth;
                    int height = bmp.PixelHeight;

                    //基準サイズと同じか判定
                    // >> 横幅
                    if (width == imageWidth)
                    {
                        //同じ場合は処理続行
                    }
                    else
                    {
                        //異なる場合は処理出来ないので失敗で終了
                        return CreateAnimationMergedBitmapResult.Failed($"アニメーション画像[No.{CommonConsts.Index.ToCollection(i)}]が基準横幅と異なります(基準={imageWidth}, 画像={width})");
                    }

                    // >> 縦幅
                    if (height == imageHeight)
                    {
                        //同じ場合は処理続行
                    }
                    else
                    {
                        //異なる場合は処理出来ないので失敗で終了
                        return CreateAnimationMergedBitmapResult.Failed($"アニメーション画像[No.{CommonConsts.Index.ToCollection(i)}]が基準縦幅と異なります(基準={imageHeight}, 画像={height})");
                    }
                }

                //画像データを変換
                byte[]? outputData = null;
                {
                    //2025.08.12:CS)杉原:パレット処理の流れを変更 >>>>> ここから
                    ////2023.11.30:CS)土田:プレビュー表示の改造 >>>>> ここから
                    //////画像データを出力データに変換
                    ////var convertColorImplResult = this.ConvertColorImpl(bmp, redBits, greenBits, blueBits);
                    ////if (convertColorImplResult.IsSuccess)
                    ////{
                    ////    //成功した場合は処理続行
                    ////}
                    ////else
                    ////{
                    ////    //失敗した場合は終了
                    ////    return CreateAnimationBitmapResult.Failed($"アニメーション画像[No.{CommonConsts.Index.ToCollection(i)}]の色変換失敗");
                    ////}
                    ////
                    //////出力データ取得
                    ////outputData = convertColorImplResult.OutputData;
                    ////if (outputData == null)
                    ////{
                    ////    //無効の場合は終了
                    ////    return CreateAnimationBitmapResult.Failed($"アニメーション画像[No.{CommonConsts.Index.ToCollection(i)}]の出力データ無効");
                    ////}
                    ////else
                    ////{
                    ////    //有効の場合は処理続行
                    ////}
                    ////----------
                    ////出力データ取得
                    //outputData = animationImage.OutputData;
                    //if (outputData == null)
                    //{
                    //    //無効の場合は終了
                    //    return CreateAnimationBitmapResult.Failed($"アニメーション画像[No.{CommonConsts.Index.ToCollection(i)}]の出力データ無効");
                    //}
                    //else
                    //{
                    //    //有効の場合は処理続行
                    //}
                    ////2023.11.30:CS)土田:プレビュー表示の改造 <<<<< ここまで
                    //----------
                    ConvertBitmapToBgra32Result ret = this.ConvertBitmapToBgra32(bmp);
                    if(ret.IsSuccess)
                    {
                        //成功の場合は結果を取得
                        // >> BGRA32の場合はストライド=横ピクセル数x4なので不要
                        outputData = ret.OutputImage;
                    }
                    else
                    {
                        //失敗した場合は終了
                        return CreateAnimationMergedBitmapResult.Failed($"アニメーション画像[No.{CommonConsts.Index.ToCollection(i)}]の画像データ取得に失敗 (理由={ret.Message})");
                    }
                    if(outputData == null)
                    {
                        //出力データが無効の場合は終了
                        return CreateAnimationMergedBitmapResult.Failed($"アニメーション画像[No.{CommonConsts.Index.ToCollection(i)}]の画像データ取得に失敗 (理由=出力データ無効)");
                    }
                    else
                    {
                        //出力データ有効の場合は処理続行
                    }
                    //2025.08.12:CS)杉原:パレット処理の流れを変更 <<<<< ここまで
                }

                //アニメーション画像を登録
                //2023.12.24:CS)杉原:テキストの場合画像パスが無いので出力データだけで判定する >>>>> ここから
                //animationImagePoints.AddAnimationImage(imagePath, outputData, bmp);
                //----------
                animationImagePoints.AddAnimationImage(animationImage, outputData, bmp);
                //2023.12.24:CS)杉原:テキストの場合画像パスが無いので出力データだけで判定する <<<<< ここまで
            }

            //重複を除いたアニメーション画像数を取得
            int normalizeAnimationImageCount = animationImagePoints.Count;
            if (CommonConsts.Collection.Empty < normalizeAnimationImageCount)
            {
                //重複を除いたアニメーション画像数が1つ以上ある場合有効なので処理続行
            }
            else
            {
                //それ以外の場合は無効なので終了
                return CreateAnimationMergedBitmapResult.Failed("有効なアニメーション画像がありません");
            }

            //2023.10.17:CS)杉原:2の累乗にする必要がある >>>>> ここから
            ////画像数でマージ後の画像サイズを決める
            //// >> Arduino側の仕様で、行を増やすと上段行の位置から下段行の位置を引くテーブルが大きくなるので
            //// >> なるべく横方向に連結する
            //// >> ただ無条件に横に並べると無駄な領域が多くなるので、なるべく小さなサイズで収まるように計算する
            //int columnCount = CommonConsts.Count.Zero;
            //int rowCount = CommonConsts.Count.Zero;
            //{
            //    //横に並べられる最大列数を計算
            //    int maxColumnCount = Model.Consts.ImageMaxPixels / imageWidth;
            //    if (maxColumnCount < CommonConsts.Collection.One)
            //    {
            //        //1枚未満(=0枚)の場合は処理出来ないので終了
            //        return CreateAnimationBitmapResult.Failed("並べられる画像がありません");
            //    }
            //    else
            //    {
            //        //1枚以上の場合は処理続行
            //    }
            //
            //    //横に並べられる列数を使って必要な行数を計算
            //    rowCount = normalizeAnimationImageCount / maxColumnCount;
            //
            //    //行数値が1未満(=0枚)になっていないか判定
            //    if (columnCount < CommonConsts.Collection.One)
            //    {
            //        //0枚の場合は無条件に1枚にする(=除算で切り捨てされているので切り上げする)
            //        rowCount = CommonConsts.Collection.One;
            //    }
            //    else
            //    {
            //        //1枚以上の場合はそのまま
            //    }
            //
            //    //行数が確定したので列数を計算
            //    columnCount = normalizeAnimationImageCount / rowCount;
            //
            //    //列数の計算で端数が切り捨てされている可能性があるので
            //    //全体数が足りているか判定
            //    int totalCount = columnCount * rowCount;
            //    if (totalCount < normalizeAnimationImageCount)
            //    {
            //        //数が足りていない場合は列数を1つ増やす(=除算で切り捨てされている)
            //        columnCount += CommonConsts.Collection.Step;
            //    }
            //    else
            //    {
            //        //数が足りている場合はそのまま
            //    }
            //}
            //
            ////マージ後の画像サイズを計算
            //int margeImageWicth = imageWidth * columnCount;
            //int margeImageHeight = imageHeight * rowCount;
            //int margeImageSize = margeImageWicth * margeImageHeight;
            //-----------
            // >> 画像サイズを2の累乗にする必要があるので計算方法を変更
            int columnCount = CommonConsts.Count.Zero;
            int rowCount = CommonConsts.Count.Zero;
            //2025.08.06:CS)土田:マージ画像にインデックスカラー以外が残る問題の修正 >>>>> ここから
            //----------
            int maxColumnCount = CommonConsts.Count.Zero;
            //2025.08.06:CS)土田:マージ画像にインデックスカラー以外が残る問題の修正 <<<<< ここまで
            {
                // >> Arduino側の仕様で、行を増やすと上段行の位置から下段行の位置を引くテーブルが大きくなるので
                // >> なるべく横方向に連結する

                //画像のピクセル数から横に並べられる最大列数を計算
                int widthMax = Model.Consts.ImagePixelSizeDic.Keys.Max();
                maxColumnCount = widthMax / imageWidth;
                if (maxColumnCount < CommonConsts.Collection.One)
                {
                    //1枚未満(=0枚)の場合は処理出来ないので終了
                    return CreateAnimationMergedBitmapResult.Failed("並べられる画像がありません");
                }
                else
                {
                    //1枚以上の場合は処理続行
                }

                //横に並べられる最大列数を使って必要な行数を計算
                rowCount = normalizeAnimationImageCount / maxColumnCount;

                //行数と最大列数を掛けて重複を除いたアニメーション画像数以上か判定
                // >> 未満の場合は除算で切り捨てされているので+1して切り上げする
                {
                    int totalCount = rowCount * maxColumnCount;
                    if(totalCount < normalizeAnimationImageCount)
                    {
                        //数が足りない場合は切り捨てされているので+1する
                        rowCount += CommonConsts.Count.Step;
                    }
                    else
                    {
                        //足りている場合は何もしない
                    }
                }

                //行数が確定したので列数を計算
                //2025.08.06:CS)土田:マージ画像にインデックスカラー以外が残る問題の修正 >>>>> ここから
                //columnCount = normalizeAnimationImageCount / rowCount;

                ////列数の計算で端数が切り捨てされている可能性があるので
                ////全体数が足りているか判定
                //{
                //    int totalCount = columnCount * rowCount;
                //    if (totalCount < normalizeAnimationImageCount)
                //    {
                //        //数が足りていない場合は列数を1つ増やす(=除算で切り捨てされている)
                //        columnCount += CommonConsts.Collection.Step;
                //    }
                //    else
                //    {
                //        //数が足りている場合はそのまま
                //    }
                //}
                //----------
                // >> すべての列を使用する
                columnCount = maxColumnCount;
                //2025.08.06:CS)土田:マージ画像にインデックスカラー以外が残る問題の修正 <<<<< ここまで
            }
            //マージ後の画像サイズを計算
            int margeImageWicth = this.NormalizeImageSize(imageWidth * columnCount);
            int margeImageHeight = this.NormalizeImageSize(imageHeight * rowCount);
            int margeImageSize = margeImageWicth * margeImageHeight;
            //2023.10.17:CS)杉原:2の累乗にする必要がある <<<<< ここまで


            //画像最大サイズを超えていないか判定
            if (Model.Consts.MaxImageDataSize < margeImageSize)
            {
                //最大画像データサイズを超える場合は終了
                return CreateAnimationMergedBitmapResult.Failed($"マージ後画像データサイズが最大値{Model.Consts.MaxImageDataSize / CommonConsts.Values.Size.M}Mbytebyteを超えています");
            }
            else
            {
                //最大画像データサイズ以内の場合は続行
            }

            try
            {

                //マージ画像生成先
                WriteableBitmap margeBitmap = new WriteableBitmap(margeImageWicth, margeImageHeight, CommonConsts.DPIs.DIP, CommonConsts.DPIs.DIP, PixelFormats.Bgra32, null);

                //画像をマージ
                {
                    //1ピクセルのバイト数
                    // >> RGBA 32bit固定の書き方 32/8=4になります
                    // >> ほかのピクセルフォーマットに対応する場合はここのコードを変更してください
                    int byteParPixel = PixelFormats.Bgra32.BitsPerPixel / CommonConsts.BitCount.BYTE;

                    //アニメーション画像のバイナリデータ取得先バッファを生成
                    int imagePixelStride = imageWidth * byteParPixel;
                    int bgra32Size = imagePixelStride * imageHeight;
                    byte[] bgra32 = new byte[bgra32Size];

                    //画像の行位置
                    int r = CommonConsts.Index.First;

                    //画像の列位置
                    int c = CommonConsts.Index.First;

                    //アニメーション画像でループ
                    for (int i = CommonConsts.Index.First; i < normalizeAnimationImageCount; i += CommonConsts.Index.Step)
                    {
                        //アニメーション画像位置(=重複を除いたアニメーション画像)を取得
                        AnimationImagePoint animationImagePoinr = animationImagePoints[i];
                        if (animationImagePoinr == null)
                        {
                            //無効の場合は即終了
                            return CreateAnimationMergedBitmapResult.Failed($"アニメーション画像位置(No.{CommonConsts.Index.ToCount(i)})が無効です");
                        }
                        else
                        {
                            //有効の場合は処理続行
                        }

                        //画像を取得
                        BitmapSource? bmp = animationImagePoinr.SourceImage;
                        if (bmp == null)
                        {
                            //無効の場合は即終了
                            return CreateAnimationMergedBitmapResult.Failed($"アニメーション画像位置(No.{CommonConsts.Index.ToCount(i)})の画像が無効です");
                        }
                        else
                        {
                            //有効の場合は処理続行
                        }

                        //画像フォーマットをBgra32に変換
                        // >> https://learn.microsoft.com/ja-jp/dotnet/desktop/wpf/graphics-multimedia/how-to-convert-a-bitmapsource-to-a-different-pixelformat?view=netframeworkdesktop-4.8&viewFallbackFrom=netdesktop-6.0
                        FormatConvertedBitmap newFormatedBitmapSource = new FormatConvertedBitmap();
                        newFormatedBitmapSource.BeginInit();
                        newFormatedBitmapSource.Source = bmp;
                        newFormatedBitmapSource.DestinationFormat = PixelFormats.Bgra32;
                        newFormatedBitmapSource.EndInit();
                        newFormatedBitmapSource.Freeze();

                        //画像取得
                        newFormatedBitmapSource.CopyPixels(bgra32, imagePixelStride, CommonConsts.Index.First);

                        //マージ画像先に設定
                        // >> X位置
                        int x = c * imageWidth;
                        // >> Y位置
                        int y = r * imageHeight;
                        // >> マージ画像に描写
                        {
                            Int32Rect rect = new Int32Rect(x, y, imageWidth, imageHeight);
                            margeBitmap.WritePixels(rect, bgra32, imagePixelStride, CommonConsts.Index.First);
                        }

                        //アニメーション画像位置の画像開始位置を設定
                        animationImagePoinr.SetPoint(x, y);

                        //画像マージ先位置更新
                        c += CommonConsts.Index.Step;
                        if (c < columnCount)
                        {
                            //範囲内の場合はそのまま
                        }
                        else
                        {
                            //範囲外になったら1行下にする
                            c = CommonConsts.Index.First;
                            r += CommonConsts.Index.Step;
                        }
                    }

                    //2025.08.06:CS)土田:マージ画像にインデックスカラー以外が残る問題の修正 >>>>> ここから
                    //----------
                    //余白を最後のフレームで埋める
                    // >> WriteableBitmapは何も書き込まないと透明(ARGBすべて0)のままとなる
                    //    アニメーション画像に透明が含まれない場合、マージ画像全体の色数は
                    //    [アニメーション画像で使用した色] + [WriteableBitmapの初期色]
                    //    となり、不要な1色が増えてしまう
                    //最大行数を計算
                    int maxRowCount = margeImageHeight / imageHeight;

                    //マージ画像の最大フレーム数を計算
                    int maxFrameCount = maxColumnCount * maxRowCount;

                    //描画済みのフレームの次から、残りのフレームを埋める
                    for (int i = normalizeAnimationImageCount; i < maxFrameCount; i += CommonConsts.Index.Step)
                    {
                        //マージ画像先に設定
                        // >> X位置
                        int x = c * imageWidth;
                        // >> Y位置
                        int y = r * imageHeight;
                        // >> マージ画像に描写
                        // >> >> bgra32には最後のフレームのデータが残っている
                        {
                            Int32Rect rect = new Int32Rect(x, y, imageWidth, imageHeight);
                            margeBitmap.WritePixels(rect, bgra32, imagePixelStride, CommonConsts.Index.First);
                        }

                        //画像マージ先位置更新
                        c += CommonConsts.Index.Step;
                        if (c < columnCount)
                        {
                            //範囲内の場合はそのまま
                        }
                        else
                        {
                            //範囲外になったら1行下にする
                            c = CommonConsts.Index.First;
                            r += CommonConsts.Index.Step;
                        }
                    }
                    //2025.08.06:CS)土田:マージ画像にインデックスカラー以外が残る問題の修正 <<<<< ここまで
                }

                //アニメーションデータコレクション生成
                AnimationDataCollection animationDatas = new AnimationDataCollection();
                for (int i = CommonConsts.Index.First; i < allAnimationImageCount; i += CommonConsts.Index.Step)
                {
                    //アニメーション画像を取得
                    AnimationImageItem animationImage = animationImages[i];
                    if (animationImage == null)
                    {
                        //無効の場合は即終了
                        return CreateAnimationMergedBitmapResult.Failed($"アニメーション画像(No.{CommonConsts.Index.ToCount(i)})が無効です");
                    }
                    else
                    {
                        //有効の場合は処理続行
                    }

                    //画像パスを取得
                    // >> nullでも良い。次のアニメーション画像位置の検索でnullになります
                    string? imagePath = animationImage.Path;

                    //対応するアニメーション画像位置を検索
                    //2023.12.24:CS)杉原:テキストの場合画像パスが無いので出力データだけで判定する >>>>> ここから
                    //AnimationImagePoint? animationImagePoint = animationImagePoints.FindAnimationImagePoint(imagePath);
                    //----------
                    AnimationImagePoint? animationImagePoint = animationImagePoints.FindAnimationImagePoint(animationImage);
                    //2023.12.24:CS)杉原:テキストの場合画像パスが無いので出力データだけで判定する <<<<< ここまで

                    if (animationImagePoint == null)
                    {
                        //無効の場合は速終了
                        return CreateAnimationMergedBitmapResult.Failed($"アニメーション画像(No.{CommonConsts.Index.ToCount(i)})に対応する位置が取得出来ませんでした");
                    }
                    else
                    {
                        //有効の場合は処理続行
                    }

                    //アニメーション画像位置を取得
                    int x = animationImagePoint.X;
                    int y = animationImagePoint.Y;

                    //表示秒数をミリ秒で取得
                    TimeSpan ts = TimeSpan.FromSeconds(animationImage.DisplayPeriod);
                    int displayPeriodMilliSecond = (int)ts.TotalMilliseconds;
                    // >> 最小値判定
                    if (displayPeriodMilliSecond < MicroSignConsts.DisplayPeriods.Min)
                    {
                        //最小値未満は最小値にする
                        displayPeriodMilliSecond = MicroSignConsts.DisplayPeriods.Min;
                    }
                    else
                    {
                        //それ以外は処理続行
                    }
                    // >> 最大値判定
                    if (MicroSignConsts.DisplayPeriods.Max < displayPeriodMilliSecond)
                    {
                        //最大値超過は最大値にする
                        displayPeriodMilliSecond = MicroSignConsts.DisplayPeriods.Max;
                    }
                    else
                    {
                        //それ以外は処理続行
                    }

                    //アニメーションデータに追加
                    animationDatas.AddAnimation(x, y, displayPeriodMilliSecond);
                }

                //ここまで来たら成功
                return CreateAnimationMergedBitmapResult.Success(margeBitmap, animationDatas);
            }
            catch (Exception ex)
            {
                //例外は握りつぶす
                string msg = "マージ画像生成で例外発生";
                LOGGER.WarnEx(msg, ex);
                return CreateAnimationMergedBitmapResult.Failed(msg);
            }
        }

        /// <summary>
        /// 画像サイズを正規化
        /// </summary>
        /// <param name="imageSize">正規化する画像サイズ/param>
        /// <returns></returns>
        private int NormalizeImageSize(int imageSize)
        {
            // 画像ピクセル辞書からKeyが指定された画像サイズ「以上」を抽出
            var list1 = Model.Consts.ImagePixelSizeDic.Where(x => imageSize <= x.Key).ToList();

            //抽出した中からKeyが最小の値を取得する
            var list2 = list1.OrderBy(x => x.Key).ToList();
            int result = list2.First().Key;

            //終了
            return result;
        }
    }
}
