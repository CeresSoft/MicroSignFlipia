using MicroSign.Core.Models.AnimationDatas;
using MicroSign.Core.Models.IndexColors;
using MicroSign.Core.ViewModels;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MicroSign.Core.Models
{
    partial class Model
    {
        /// <summary>
        /// インデックスカラーアニメーション変換
        /// </summary>
        /// <param name="animationImages">アニメーション画像</param>
        /// <param name="matrixLedWidth">マトリクスLED横ドット数</param>
        /// <param name="matrixLedHeight">マトリクスLED縦ドット数</param>
        /// <param name="matrixLedBrightness">マトリクスLED明るさ</param>
        /// <param name="gamma">ガンマ値(2025.08.18:CS)土田:ガンマ補正対応で追加)</param>
        /// <param name="motionBlurReduction">残像軽減(2025.08.21:CS)土田:残像軽減対応で追加)</param>
        /// <param name="savePath">保存先(2025.10.03:CS)土田:変換結果の保存先を選択できるように引数追加)</param>
        /// <param name="pcm8">PCM(=16ビット)の上位8bitだけにしたサウンドデータ(2026.05.22:CS)杉原:サウンド機能追加)</param>
        /// <returns></returns>
        private ConvertResult ConvertAnimationIndexColor(AnimationImageItemCollection animationImages, int matrixLedWidth, int matrixLedHeight, int matrixLedBrightness, double gamma, int motionBlurReduction, string savePath, byte[]? pcm8)
        {
            //2025.08.12:CS)杉原:パレット処理の流れを変更 >>>>> ここから
            ////アニメーション画像からマージしたアニメーション用画像を生成
            //CreateAnimationBitmapResult ret = this.CreateAnimationBitmap(
            //    animationImages,
            //    OutputColorFormatKind.IndexColor,
            //    MicroSignConsts.RGB.Bit8,
            //    MicroSignConsts.RGB.Bit8,
            //    MicroSignConsts.RGB.Bit8);
            //if (ret.IsSuccess)
            //{
            //    //成功した場合は処理続行
            //}
            //else
            //{
            //    //失敗した場合は終了
            //    return ConvertResult.Failed(ret.Message);
            //}
            //
            ////インデックスカラーに変換
            //{
            //    BitmapSource? margeImage = ret.MargeImage;
            //    AnimationDataCollection? animationDatas = ret.AnimationDatas;
            //    ConvertResult result = this.ConvertColor(
            //        OutputColorFormatKind.IndexColor,
            //        margeImage,
            //        name,
            //        MicroSignConsts.RGB.IndexColorMemberName,
            //        MicroSignConsts.RGB.Bit8,
            //        MicroSignConsts.RGB.Bit8,
            //        MicroSignConsts.RGB.Bit8,
            //        animationDatas, matrixLedWidth, matrixLedHeight, matrixLedBrightness);
            //    return result;
            //}
            //----------
            // >> 全面的に作成しなおし

            //アニメーション用マージ画像生成
            BitmapSource? margeImage = null;
            AnimationDataCollection? animationDatas = null;
            {
                //アニメーション用マージ画像生成
                CreateAnimationMergedBitmapResult ret = this.CreateAnimationMergedBitmap(animationImages, matrixLedWidth, matrixLedHeight);
                if (ret.IsSuccess)
                {
                    //成功した場合は処理続行
                }
                else
                {
                    //失敗した場合は終了
                    string msg = $"マージ画像の生成で失敗 ({ret.Message})";
                    return ConvertResult.Failed(msg);
                }

                //マージ画像取得
                margeImage = ret.MargeImage;
                if (margeImage == null)
                {
                    //無効の場合はエラーで終了
                    return ConvertResult.Failed("マージ画像が無効");
                }
                else
                {
                    //有効の場合は処理続行
                }

                //アニメーションデータ取得
                animationDatas = ret.AnimationDatas;
                if (animationDatas == null)
                {
                    //無効の場合はエラーで終了
                    return ConvertResult.Failed("アニメーションデータが無効");
                }
                else
                {
                    //有効の場合は処理続行
                }
            }

            //2025.08.18:CS)土田:ガンマ補正対応で追加 >>>>> ここから
            //----------
            BitmapSource? correctedImage = null;  //CorrectInvertGammaで使用した画像
            {
                CorrectImageGammaResult ret = this.CorrectImageGamma(margeImage, gamma);

                if (ret.IsSuccess)
                {
                    //成功した場合は処理続行
                }
                else
                {
                    //失敗した場合は終了
                    string msg = $"補正画像の生成で失敗 ({ret.Message})";
                    return ConvertResult.Failed(msg);
                }

                //補正画像取得
                correctedImage = ret.CorrectedImage;
                if (correctedImage == null)
                {
                    //無効の場合はエラーで終了
                    return ConvertResult.Failed("補正画像が無効");
                }
                else
                {
                    //有効の場合は処理続行
                }
            }
            //2025.08.18:CS)土田:ガンマ補正対応で追加 <<<<< ここまで

            //ファイルに保存する
            BitmapSource? usedImage = null;  //ConvertAnimationIndexColor_SaveToFileで使用した画像
            {
                //2026.05.22:CS)杉原:サウンド機能追加 >>>>> ここから
                ////2025.08.18:CS)土田:ガンマ補正対応で使用する画像を変更 >>>>> ここから
                ////ConvertAnimationIndexColor_SaveToFileResult ret = this.ConvertAnimationIndexColor_SaveToFile(margeImage, animationDatas, matrixLedWidth, matrixLedHeight, matrixLedBrightness);
                ////----------
                ////2025.08.21:CS)土田:残像軽減対応で引数を追加 >>>>> ここから
                ////ConvertAnimationIndexColor_SaveToFileResult ret = this.ConvertAnimationIndexColor_SaveToFile(correctedImage, animationDatas, matrixLedWidth, matrixLedHeight, matrixLedBrightness);
                ////----------
                ////2025.10.03:CS)土田:変換結果の保存先を選択できるように引数追加 >>>>> ここから
                ////ConvertAnimationIndexColor_SaveToFileResult ret = this.ConvertAnimationIndexColor_SaveToFile(correctedImage, animationDatas, matrixLedWidth, matrixLedHeight, matrixLedBrightness, motionBlurReduction);
                ////----------
                //ConvertAnimationIndexColor_SaveToFileResult ret = this.ConvertAnimationIndexColor_SaveToFile(correctedImage, animationDatas, matrixLedWidth, matrixLedHeight, matrixLedBrightness, motionBlurReduction, savePath);
                ////2025.10.03:CS)土田:変換結果の保存先を選択できるように引数追加 <<<<< ここまで
                ////2025.08.21:CS)土田:残像軽減対応で引数を追加 <<<<< ここまで
                ////2025.08.18:CS)土田:ガンマ補正対応で使用する画像を変更 <<<<< ここまで
                //-----
                ConvertAnimationIndexColor_SaveToFileResult ret = this.ConvertAnimationIndexColor_SaveToFile(correctedImage, animationDatas, matrixLedWidth, matrixLedHeight, matrixLedBrightness, motionBlurReduction, savePath, pcm8);
                //2026.05.22:CS)杉原:サウンド機能追加 <<<<< ここまで

                if (ret.IsSuccess)
                {
                    //成功した場合は処理続行
                    usedImage = ret.UsedImage;
                }
                else
                {
                    //失敗した場合は終了
                    string msg = $"ファイル出力に失敗 ({ret.Message})";
                    return ConvertResult.Failed(msg);
                }

            }

            //ここまで来たら成功で終了
            // >> ConvertAnimationIndexColor_SaveToFileで使用した画像とアニメーションデータを返す
            return ConvertResult.Sucess(usedImage, animationDatas);
            //2025.08.12:CS)杉原:パレット処理の流れを変更 <<<<< ここまで
        }

        /// <summary>
        /// インデックスカラーアニメーション変換 - ファイルに保存
        /// </summary>
        /// <remarks>
        /// 2025.08.12:CS)杉原:パレット処理の流れを変更で追加
        /// </remarks>
        public struct ConvertAnimationIndexColor_SaveToFileResult
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
            /// 使用画像
            /// </summary>
            /// <remarks>減色等される場合があるので内部で使用した画像を返す</remarks>
            public readonly BitmapSource? UsedImage;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="isSuccess">成功フラグ</param>
            /// <param name="message">メッセージ</param>
            /// <param name="usedImage">使用画像</param>
            private ConvertAnimationIndexColor_SaveToFileResult(bool isSuccess, string? message, BitmapSource? usedImage)
            {
                this.IsSuccess = isSuccess;
                this.Message = message;
                this.UsedImage = usedImage;
            }

            /// <summary>
            /// 失敗
            /// </summary>
            /// <param name="message">メッセージ</param>
            /// <returns></returns>
            public static ConvertAnimationIndexColor_SaveToFileResult Failed(string message)
            {
                ConvertAnimationIndexColor_SaveToFileResult result = new ConvertAnimationIndexColor_SaveToFileResult(false, message, null);
                return result;
            }

            /// <summary>
            /// 成功
            /// </summary>
            /// <param name="usedImage">使用画像</param>
            /// <returns></returns>
            public static ConvertAnimationIndexColor_SaveToFileResult Success(BitmapSource? usedImage)
            {
                ConvertAnimationIndexColor_SaveToFileResult result = new ConvertAnimationIndexColor_SaveToFileResult(true, null, usedImage);
                return result;
            }
        }

        /// <summary>
        /// インデックスカラーアニメーション変換 - ファイルに保存
        /// </summary>
        /// <param name="image">入力画像</param>
        /// <param name="animationDatas">アニメーションデータコレクション</param>
        /// <param name="matrixLedWidth">マトリクスLED横ドット数</param>
        /// <param name="matrixLedHeight">マトリクスLED縦ドット数</param>
        /// <param name="matrixLedBrightness">マトリクスLED明るさ</param>
        /// <param name="motionBlurReduction">残像軽減(2025.08.21:CS)土田:残像軽減対応で追加)</param>
        /// <param name="fname">保存先(2025.10.03:CS)土田:変換結果の保存先を選択できるように引数追加)</param>
        /// <param name="pcm8">PCM(=16ビット)の上位8bitだけにしたサウンドデータ(2026.05.22:CS)杉原:サウンド機能追加)</param>
        /// <returns></returns>
        /// <remarks>
        /// 2025.08.12:CS)杉原:パレット処理の流れを変更で追加
        /// ConvertColorFile()を移植(=ConvertColorFile()は削除)
        /// </remarks>
        public ConvertAnimationIndexColor_SaveToFileResult ConvertAnimationIndexColor_SaveToFile(BitmapSource? image, AnimationDataCollection? animationDatas, int matrixLedWidth, int matrixLedHeight, int matrixLedBrightness, int motionBlurReduction, string fname, byte[]? pcm8)
        {
            //画像有効判定
            if (image == null)
            {
                //画像無しは失敗
                return ConvertAnimationIndexColor_SaveToFileResult.Failed("画像無し");
            }
            else
            {
                //画像有りの場合は処理続行
            }

            //アニメーションデータ数取得
            int animationDataCount = CommonConsts.Collection.Empty;
            if (animationDatas == null)
            {
                //アニメーションデータ無しは失敗
                return ConvertAnimationIndexColor_SaveToFileResult.Failed("アニメーションデータ無効");
            }
            else
            {
                animationDataCount = animationDatas.Count;
                if (CommonConsts.Collection.Empty < animationDataCount)
                {
                    //アニメーションデータ有りの場合は処理続行
                }
                else
                {
                    //アニメーションデータ無しは失敗
                    return ConvertAnimationIndexColor_SaveToFileResult.Failed("アニメーションデータ無し");
                }
            }

            //画像ピクセル取得
            // >> 検証済の値を取得
            int imagePixelWidth = image.PixelWidth;
            int imagePixelHeight = image.PixelHeight;

            //画像サイズ検証
            ValidateImageSizeResult validateRet = this.ValidateImageSize(imagePixelWidth, imagePixelHeight);
            if (validateRet.IsValid)
            {
                //検証OKの場合は続行
            }
            else
            {
                //検証NGの場合は終了
                string msg = $"画像サイズ検証に失敗 ({validateRet.ErrorMessage})";
                return ConvertAnimationIndexColor_SaveToFileResult.Failed(msg);
            }

            //画像ビット数取得
            // >> 検証済の値を取得
            int imageWidthBits = validateRet.ImageWidthBits;
            int imageHeightBits = validateRet.ImageHeightBits;

            //画像データを変換
            byte[]? outputData = null;
            //2025.08.11:CS)杉原:インデックスカラー対応修正 >>>>> ここから
            //byte[]? outputImage = null;
            //int outputImageStride = CommonConsts.Collection.Empty;
            //----------
            BitmapSource? usedImage = null;
            //2025.08.11:CS)杉原:インデックスカラー対応修正 <<<<< ここまで
            //2025.08.05:CS)土田:インデックスカラー対応 >>>>> ここから
            //----------
            IndexColorCollection? colors = null;
            //2025.08.05:CS)土田:インデックスカラー対応 <<<<< ここまで
            {
                //画像データを出力データに変換
                //2025.08.05:CS)土田:インデックスカラー対応 >>>>> ここから
                //var convertColorImplResult = this.ConvertColorImpl(image, redBits, greenBits, blueBits);
                //----------
                ConvertAnimationIndexColor_GetColorDataResult convertColorImplResult = this.ConvertAnimationIndexColor_GetColorData(image);
                //2025.08.05:CS)土田:インデックスカラー対応 <<<<< ここまで
                switch(convertColorImplResult.ResultCode)
                {
                    case ConvertAnimationIndexColor_GetColorDataResultCodes.Success:
                        //成功の場合は処理続行
                        break;

                    default:
                        //それ以外は失敗
                        {
                            string msg = $"色データ取得失敗 ({convertColorImplResult.Message})";
                            return ConvertAnimationIndexColor_SaveToFileResult.Failed(msg);
                        }
                }

                //出力データ取得
                outputData = convertColorImplResult.OutputData;
                if (outputData == null)
                {
                    //無効の場合は終了
                    return ConvertAnimationIndexColor_SaveToFileResult.Failed("出力データ無効");
                }
                else
                {
                    //有効の場合は処理続行
                }

                //2025.08.05:CS)土田:インデックスカラー対応 >>>>> ここから
                //----------
                //カラーパレット取得
                colors = convertColorImplResult.Colors;
                if (colors == null)
                {
                    //無効の場合は終了
                    return ConvertAnimationIndexColor_SaveToFileResult.Failed("カラーパレット無効");
                }
                else
                {
                    //有効の場合は処理続行
                }
                //2025.08.05:CS)土田:インデックスカラー対応 <<<<< ここまで
                //2025.08.11:CS)杉原:インデックスカラー対応修正 >>>>> ここから
                //----------
                //色数判定
                {
                    //色数取得
                    int colorCount = colors.Count;

                    //最小値判定
                    if (colorCount < CommonConsts.Palettes.Count.Min)
                    {
                        //最小値未満の場合はエラー
                        return ConvertAnimationIndexColor_SaveToFileResult.Failed($"カラーパレット数が最小値未満です (count={colorCount})");
                    }
                    else
                    {
                        //最小値以上の場合は処理続行
                    }

                    //最大値判定
                    if (CommonConsts.Palettes.Count.Max < colorCount)
                    {
                        //最大値超過の場合はエラー
                        return ConvertAnimationIndexColor_SaveToFileResult.Failed($"カラーパレット数が最大値超過です (count={colorCount})");
                    }
                    else
                    {
                        //最大値以下の場合は処理続行
                    }
                }

                //使用画像取得
                usedImage = convertColorImplResult.UsedImage;
                if (usedImage == null)
                {
                    //無効の場合は終了
                    return ConvertAnimationIndexColor_SaveToFileResult.Failed("使用画像無効");
                }
                else
                {
                    //有効の場合は処理続行
                }
                //2025.08.11:CS)杉原:インデックスカラー対応修正 <<<<< ここまで
            }

            //2026.05.22:CS)杉原:サウンド機能追加 >>>>> ここから
            //-----
            // >> サウンドデータサイズ
            int pcm8Size = CommonUtils.GetCount(pcm8);
            //2026.05.22:CS)杉原:サウンド機能追加 <<<<< ここまで

            //2023.10.16:CS)杉原バイナリデータとして保存
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            using (System.IO.BinaryWriter bw = new System.IO.BinaryWriter(ms))
            {
                //@@ ヘッダ(uint16 x 8のサイズ)
                //バージョン(uint16)
                //2026.05.22:CS)杉原:サウンド機能追加 >>>>> ここから
                //bw.Write((UInt16)MicroSignConsts.Versions.V110);
                //----------
                bw.Write((UInt16)MicroSignConsts.Versions.V120);
                //2026.05.22:CS)杉原:サウンド機能追加 <<<<< ここまで

                //マトリクスLED
                {
                    // >> 横幅(uint16)
                    bw.Write((UInt16)matrixLedWidth);

                    // >> 縦幅(uint16)
                    bw.Write((UInt16)matrixLedHeight);

                    // >> 明るさ
                    bw.Write((UInt16)matrixLedBrightness);

                    //2025.08.21:CS)土田:残像軽減対応で値を追加 >>>>> ここから
                    ////空き
                    //bw.Write((UInt16)MicroSignConsts.FileData.Reserve);
                    //-----
                    //残像軽減
                    //2026.05.22:CS)杉原:サウンド機能追加 >>>>> ここから
                    //bw.Write((UInt16)motionBlurReduction);
                    //-----
                    // >> 常に0を書込
                    bw.Write((UInt16)MicroSignConsts.FileData.Reserve);
                    //2026.05.22:CS)杉原:サウンド機能追加 <<<<< ここまで
                    //2025.08.21:CS)土田:残像軽減対応で値を追加 <<<<< ここまで
                }

                //2025.08.11:CS)杉原:インデックスカラー対応修正 >>>>> ここから
                ////2025.08.05:CS)土田:インデックスカラー対応 >>>>> ここから
                ////----------
                ////パレット
                //{
                //    // >> パレット数(uint16)
                //    int palleteCount = colors.Count;
                //    bw.Write((UInt16)palleteCount);
                //
                //    // >> パレット
                //    for (int i = CommonConsts.Index.First; i < palleteCount; i += CommonConsts.Index.Step)
                //    {
                //        //インデックスカラー取得
                //        int b = MicroSignConsts.RGB.Black;
                //        int g = MicroSignConsts.RGB.Black;
                //        int r = MicroSignConsts.RGB.Black;
                //        int a = MicroSignConsts.RGB.Black;
                //        IndexColor color = colors[i];
                //        if (color == null)
                //        {
                //            //無効の場合初期値のままとする
                //        }
                //        else
                //        {
                //            //有効の場合
                //            b = color.B;
                //            g = color.G;
                //            r = color.R;
                //            a = color.A;
                //        }
                //
                //        // >> B(8bit)
                //        bw.Write((Byte)b);
                //
                //        // >> G(8bit)
                //        bw.Write((Byte)g);
                //
                //        // >> R(8bit)
                //        bw.Write((Byte)r);
                //
                //        // >> A(8bit)
                //        bw.Write((Byte)a);
                //    }
                //}
                ////2025.08.05:CS)土田:インデックスカラー対応 <<<<< ここまで
                //----------
                //2025.08.11:CS)杉原:インデックスカラー対応修正 <<<<< ここまで

                //画像
                {
                    // >> 横ピクセルビット数(uint16)
                    bw.Write((UInt16)imageWidthBits);

                    // >> 横ピクセルビット数(uint16)
                    bw.Write((UInt16)imageHeightBits);

                    // >> フォーマット(uint16)
                    //2025.08.11:CS)杉原:インデックスカラー対応修正 >>>>> ここから
                    //switch (formatKind)
                    //{
                    //    case OutputColorFormatKind.Color64:
                    //        bw.Write((UInt16)OutputColorFormatKind.Color64);
                    //        break;
                    //
                    //    //2025.08.05:CS)土田:インデックスカラー対応 >>>>> ここから
                    //    //----------
                    //    case OutputColorFormatKind.IndexColor:
                    //        bw.Write((UInt16)OutputColorFormatKind.IndexColor);
                    //        //2025.08.11:CS)杉原:インデックスカラー対応修正 >>>>> ここから
                    //        //----------
                    //        paletteCount = colors.Count;
                    //        //2025.08.11:CS)杉原:インデックスカラー対応修正 <<<<< ここまで
                    //        break;
                    //    //2025.08.05:CS)土田:インデックスカラー対応 <<<<< ここまで
                    //
                    //    default:
                    //        //それ以外はすべて256にする
                    //        bw.Write((UInt16)OutputColorFormatKind.Color256);
                    //        break;
                    //}
                    //----------
                    bw.Write((UInt16)OutputColorFormatKind.IndexColor);
                    //2025.08.11:CS)杉原:インデックスカラー対応修正 <<<<< ここまで

                    //2025.08.11:CS)杉原:インデックスカラー対応修正 >>>>> ここから
                    ////空き
                    //bw.Write((UInt16)0);
                    //----------
                    // >> パレット数
                    int paletteCount = colors.Count;
                    bw.Write((UInt16)paletteCount);
                    //2025.08.11:CS)杉原:インデックスカラー対応修正 <<<<< ここまで
                }

                //アニメーション
                {
                    // >> アニメーション数(uint16)
                    bw.Write((UInt16)animationDataCount);

                    // >> アニメーションセルサイズ(uint16) 現在は(+0=X, +1=Y, +2=表示期間の3のみ)
                    // >> 将来の拡張で高度なアニメーションを作るときは増えると思います
                    bw.Write((UInt16)MicroSignConsts.Animations.CellSize.FixedSize);

                    //空き
                    bw.Write((UInt16)MicroSignConsts.FileData.Reserve);

                    //空き
                    bw.Write((UInt16)MicroSignConsts.FileData.Reserve);
                }

                //2026.05.22:CS)杉原:サウンド機能追加 >>>>> ここから
                ////空き領域
                //{
                //    //空き
                //    bw.Write((UInt16)MicroSignConsts.FileData.Reserve);
                //
                //    //空き
                //    bw.Write((UInt16)MicroSignConsts.FileData.Reserve);
                //
                //    //空き
                //    bw.Write((UInt16)MicroSignConsts.FileData.Reserve);
                //
                //    //空き
                //    bw.Write((UInt16)MicroSignConsts.FileData.Reserve);
                //}
                //-----
                //サウンド領域
                {
                    //サウンドサイズ
                    // >> 16bitだと65535byteまでと小さいので、int(=32bit)にします
                    bw.Write(pcm8Size);

                    //空き
                    bw.Write((UInt16)MicroSignConsts.FileData.Reserve);

                    //空き
                    bw.Write((UInt16)MicroSignConsts.FileData.Reserve);

                }
                //2026.05.22:CS)杉原:サウンド機能追加 <<<<< ここまで

                //2025.08.11:CS)杉原:インデックスカラー対応修正 >>>>> ここから
                //----------
                //パレット書込
                {
                    //パレット数(uint16)
                    int paletteCount = colors.Count;

                    //パレット書き込みループ
                    for (int i = CommonConsts.Index.First; i < paletteCount; i += CommonConsts.Index.Step)
                    {
                        //インデックスカラー取得
                        int b = MicroSignConsts.RGB.Black;
                        int g = MicroSignConsts.RGB.Black;
                        int r = MicroSignConsts.RGB.Black;
                        int a = MicroSignConsts.RGB.Black;
                        IndexColor color = colors[i];
                        if (color == null)
                        {
                            //無効の場合初期値のままとする
                        }
                        else
                        {
                            //有効の場合
                            b = color.B;
                            g = color.G;
                            r = color.R;
                            a = color.A;
                        }

                        //エンディアンが気になるのでバイト単位に書込
                        // >> B(8bit)
                        bw.Write((Byte)b);

                        // >> G(8bit)
                        bw.Write((Byte)g);

                        // >> R(8bit)
                        bw.Write((Byte)r);

                        // >> A(8bit)
                        bw.Write((Byte)a);
                    }
                }
                //2025.08.11:CS)杉原:インデックスカラー対応修正 <<<<< ここまで

                //出力データを書込み
                bw.Write(outputData);

                //@@ アニメーションデータ (uint16 * animationDataCount * 3のサイズ)
                for (int i = CommonConsts.Index.First; i < animationDataCount; i += CommonConsts.Index.Step)
                {
                    //アニメーションデータ取得
                    UInt16 x = (UInt16)CommonConsts.Points.Zero.X;
                    UInt16 y = (UInt16)CommonConsts.Points.Zero.Y;
                    UInt16 t = (UInt16)CommonConsts.Intervals.Zero;
                    AnimationData? animationData = animationDatas[i];
                    if (animationData == null)
                    {
                        //無効の場合初期値のままとする
                    }
                    else
                    {
                        //有効の場合
                        x = (UInt16)animationData.X;
                        y = (UInt16)animationData.Y;
                        t = (UInt16)(animationData.DisplayPeriodMillisecond);
                    }

                    //出力
                    // >> X
                    bw.Write(x);

                    // >> Y
                    bw.Write(y);

                    // >> 表示期間
                    bw.Write(t);
                }

                //2026.05.22:CS)杉原:サウンド機能追加 >>>>> ここから
                //-----
                // >> サウンドデータ書込
                if(CommonConsts.Collection.Empty < pcm8Size)
                {
                    //サウンドデータ書き込み
                    bw.Write(pcm8!);
                }
                else
                {
                    //サウンドデータがない場合は何もしない
                }
                //2026.05.22:CS)杉原:サウンド機能追加 <<<<< ここまで

                //@@ 書込みを完了する
                bw.Flush();

                //@@ ファイルに書き込み
                try
                {
                    //2025.10.03:CS)土田:変換結果の保存先を選択できるように引数追加 >>>>> ここから
                    ////ESP32側のファイル名は固定なので、あえて名前は変更しない
                    //string fname = MicroSignConsts.Path.MatrixLedImagePath;
                    //----------
                    //2025.10.03:CS)土田:変換結果の保存先を選択できるように引数追加 <<<<< ここまで
                    string path = CommonUtils.GetFullPath(fname);
                    CommonLogger.Debug($"アニメーションファイルパス='{path}'");

                    //ディレクトリ取得
                    string? dir = System.IO.Path.GetDirectoryName(path);
                    if (dir == null)
                    {
                        //ディレクトリがnullの場合は何もしない
                        CommonLogger.Debug("アニメーションファイルディレクトリ無し");
                    }
                    else
                    {
                        //ディレクトリ有効判定
                        bool isNull = string.IsNullOrEmpty(dir);
                        if (isNull)
                        {
                            //無効の場合は何もしない
                            CommonLogger.Debug("アニメーションファイルディレクトリ無効");
                        }
                        else
                        {
                            //有効の場合はディレクトリを作成
                            CommonLogger.Debug($"アニメーションファイルディレクトリ='{dir}'");
                            System.IO.Directory.CreateDirectory(dir);
                        }
                    }

                    //ファイルを出力
                    CommonLogger.Debug("アニメーションファイル書込み - 開始");
                    using (System.IO.FileStream fs = new System.IO.FileStream(path, FileMode.Create))
                    {
                        ms.WriteTo(fs);
                    }
                    CommonLogger.Debug("アニメーションファイル書込み - 完了");

                    //出力先のフォルダをエクスプローラーで表示
                    if (dir == null)
                    {
                        //ディレクトリがnullの場合は何もしない
                    }
                    else
                    {
                        //ディレクトリ有効判定
                        bool isNull = string.IsNullOrEmpty(dir);
                        if (isNull)
                        {
                            //無効の場合は何もしない
                        }
                        else
                        {
                            //有効の場合はディレクトリを表示
                            System.Diagnostics.Process.Start("explorer.exe", dir);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //例外発生時は終了
                    return ConvertAnimationIndexColor_SaveToFileResult.Failed(CommonLogger.Warn($"アニメーションファイル書込み失敗", ex));
                }
            }

            //終了
            return ConvertAnimationIndexColor_SaveToFileResult.Success(usedImage);
        }

        /// <summary>
        /// インデックスカラーアニメーション変換 - 色データ取得結果コード
        /// </summary>
        private enum ConvertAnimationIndexColor_GetColorDataResultCodes
        {
            /// <summary>
            /// 成功
            /// </summary>
            Success,

            /// <summary>
            /// 画像無効
            /// </summary>
            InvalidImage,

            /// <summary>
            /// 色インデックス無効
            /// </summary>
            InvalidColorIndex,

            /// <summary>
            /// 色数オーバー
            /// </summary>
            ColorOver,

            /// <summary>
            /// 例外発生
            /// </summary>
            CatchException,

            /// <summary>
            /// 減色処理失敗
            /// </summary>
            FailedColorReduction,

            /// <summary>
            /// 減色画像無効
            /// </summary>
            InvalidColorReductionImage,
        }

        /// <summary>
        /// インデックスカラーアニメーション変換 - 色データ取得結果
        /// </summary>
        /// <remarks>
        /// 2025.08.12:CS)杉原:パレット処理の流れを変更で追加
        /// </remarks>
        private struct ConvertAnimationIndexColor_GetColorDataResult
        {
            /// <summary>
            /// 結果コード
            /// </summary>
            public readonly ConvertAnimationIndexColor_GetColorDataResultCodes ResultCode;

            /// <summary>
            /// メッセージ
            /// </summary>
            public readonly string? Message;

            /// <summary>
            /// 出力データ
            /// </summary>
            /// <remarks>
            /// ファイルに書込する変換した出力データ
            /// </remarks>
            public readonly byte[]? OutputData;

            /// <summary>
            /// カラーインデックス
            /// </summary>
            public readonly IndexColorCollection? Colors;

            /// <summary>
            /// 使用画像
            /// </summary>
            public readonly BitmapSource? UsedImage;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="resultCode">結果コード</param>
            /// <param name="message">メッセージ</param>
            /// <param name="outputData">出力データ</param>
            /// <param name="colors">色コレクション</param>
            /// <param name="usedImage">使用画像</param>
            private ConvertAnimationIndexColor_GetColorDataResult(ConvertAnimationIndexColor_GetColorDataResultCodes resultCode, string? message, byte[]? outputData, IndexColorCollection? colors, BitmapSource? usedImage)
            {
                this.ResultCode = resultCode;
                this.Message = message;
                this.OutputData = outputData;
                this.Colors = colors;
                this.UsedImage = usedImage;
            }

            /// <summary>
            ///  変換失敗
            /// </summary>
            /// <param name="resultCode">結果コード</param>
            /// <param name="message">メッセージ</param>
            /// <returns></returns>
            public static ConvertAnimationIndexColor_GetColorDataResult Failed(ConvertAnimationIndexColor_GetColorDataResultCodes resultCode, string message)
            {
                ConvertAnimationIndexColor_GetColorDataResult result = new ConvertAnimationIndexColor_GetColorDataResult(resultCode, message, null, null, null);
                return result;
            }

            /// <summary>
            /// 成功
            /// </summary>
            /// <param name="usedImage">使用画像</param>
            /// <param name="outputData">出力データ</param>
            /// <param name="colors">色コレクション</param>
            /// <returns></returns>
            public static ConvertAnimationIndexColor_GetColorDataResult Success(BitmapSource? usedImage, byte[]? outputData, IndexColorCollection? colors)
            {
                ConvertAnimationIndexColor_GetColorDataResult result = new ConvertAnimationIndexColor_GetColorDataResult(ConvertAnimationIndexColor_GetColorDataResultCodes.Success, null, outputData, colors, usedImage);
                return result;
            }
        }

        /// <summary>
        /// インデックスカラーアニメーション変換 - 色データ取得
        /// </summary>
        /// <param name="image">変換する画像</param>
        /// <returns>色変換実装結果</returns>
        /// <remarks>
        /// 2025.08.12:CS)杉原:パレット処理の流れを変更で追加
        /// </remarks>
        private ConvertAnimationIndexColor_GetColorDataResult ConvertAnimationIndexColor_GetColorData(BitmapSource? image)
        {
            //連結された画像を保存
            if(image == null)
            {
                //画像が無効の場合は何もしない
            }
            else
            {
                //画像が有効の場合はログフォルダに連結したアニメーション画像を保存
                string userProfile = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);
                string path = System.IO.Path.Combine(userProfile, MicroSignConsts.Path.LogAnimationImagePath);

                //PNGで保存する
                try
                {
                    using (System.IO.Stream st = System.IO.File.Create(path))
                    {
                        PngBitmapEncoder encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(image));
                        encoder.Save(st);
                    }
                }
                catch (Exception ex)
                {
                    //例外は握りつぶして処理続行
                    CommonLogger.Warn($"連結したアニメーション画像の保存に失敗 (path='{path}')", ex);
                }
            }

            //指定された画像のまま色データ取得実装呼出
            {
                ConvertAnimationIndexColor_GetColorDataResult ret = this.ConvertAnimationIndexColor_GetColorDataImpl(image);
                //2025.08.05:CS)土田:インデックスカラー対応 <<<<< ここまで
                switch (ret.ResultCode)
                {
                    case ConvertAnimationIndexColor_GetColorDataResultCodes.ColorOver:
                        //色数がオーバーした場合は処理続行
                        break;

                    default:
                        //それ以外はそのまま終了
                        return ret;
                }
            }

            //色数を256色に減色する
            // >> マージ画像が256色超過の場合、「エラーにする」か「減色する」必要があるが
            // >> 初心者ユーザーが256色に減色する作業はまあまあ面倒なので減色したビットマップを作成する
            // >> ただし色見が変わるなどの弊害がある
            BitmapSource? image256 = null;
            {
                ConvertBitmapColorReductionResult ret = this.ConvertBitmapColorReduction(image);
                if (ret.IsSuccess)
                {
                    //成功した場合は処理続行
                }
                else
                {
                    //失敗した場合は終了
                    string msg = $"減色画像の生成に失敗 ({ret.Message})";
                    return ConvertAnimationIndexColor_GetColorDataResult.Failed(ConvertAnimationIndexColor_GetColorDataResultCodes.FailedColorReduction, msg);
                }

                //減色画像を取得
                image256 = ret.ColorReductionImage;
                if (image256 == null)
                {
                    //無効の場合はエラーで終了
                    return ConvertAnimationIndexColor_GetColorDataResult.Failed(ConvertAnimationIndexColor_GetColorDataResultCodes.InvalidColorReductionImage, "減色画像が無効");
                }
                else
                {
                    //有効の場合は処理続行
                }
            }

            //減色画像で色データ取得実装呼出
            {
                ConvertAnimationIndexColor_GetColorDataResult ret = this.ConvertAnimationIndexColor_GetColorDataImpl(image256);

                //無条件にそのまま終了
                return ret;
            }
        }

        /// <summary>
        /// インデックスカラーアニメーション変換 - 色データ取得実装
        /// </summary>
        /// <param name="image">変換する画像</param>
        /// <returns>色変換実装結果</returns>
        /// <remarks>
        /// 2025.08.05:CS)土田:インデックスカラー対応
        /// 
        /// 2025.08.12:CS)杉原:パレット処理の流れを変更で追加
        /// ConvertColorImpl()を移植(=ConvertColorImpl()は削除)
        /// </remarks>
        private ConvertAnimationIndexColor_GetColorDataResult ConvertAnimationIndexColor_GetColorDataImpl(BitmapSource? image)
        {
            //変換する画像の有効判定
            if (image == null)
            {
                //無効の場合は何もせずに終了
                return ConvertAnimationIndexColor_GetColorDataResult.Failed(ConvertAnimationIndexColor_GetColorDataResultCodes.InvalidImage, "画像が無効");
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

                //変換後の画像生成先
                // >> インデックスカラー版では元画像をそのまま使う
                byte[] outputImage = bgra32;

                //出力データ生成先
                int outputSize = imagePixelHeight * imagePixelWidth;
                byte[] outputData = new byte[outputSize];

                //パレット生成先
                IndexColorCollection colors = new IndexColorCollection();

                //Y座標ループ
                for (int y = CommonConsts.Index.First; y < imagePixelHeight; y += CommonConsts.Index.Step)
                {
                    //X軸ループ
                    for (int x = CommonConsts.Index.First; x < imagePixelWidth; x += CommonConsts.Index.Step)
                    {
                        //インデックス計算
                        int index = (y * imagePixelStride) + (x * byteParPixel);
                        int blueIndex = index;
                        int greenIndex = blueIndex + CommonConsts.Index.Step;
                        int redIndex = greenIndex + CommonConsts.Index.Step;
                        int alphaIndex = redIndex + CommonConsts.Index.Step;

                        //透明値
                        int alphaValue = bgra32[alphaIndex];
                        int redValue = bgra32[redIndex];
                        int greenValue = bgra32[greenIndex];
                        int blueValue = bgra32[blueIndex];

                        //色値
                        // >> アルファが効いた状態にする
                        int B = this.CalcColor(blueValue, alphaValue);
                        int G = this.CalcColor(greenValue, alphaValue);
                        int R = this.CalcColor(redValue, alphaValue);
                        int A = CommonConsts.Palettes.Colors.AlphaMax;  //アルファは常に最大値(=無効)にする

                        //インデックスカラー生成
                        IndexColor color = new IndexColor(A, R, G, B);

                        //パレット登録
                        // >> 色が重複する場合、先に登録されている色のインデックスが返る
                        int colorIndex = colors.AddColor(color);
                        if (colorIndex < CommonConsts.Index.First)
                        {
                            //パレット登録失敗の場合は変換失敗
                            return ConvertAnimationIndexColor_GetColorDataResult.Failed(ConvertAnimationIndexColor_GetColorDataResultCodes.InvalidColorIndex, $"色インデックスが無効 (index={colorIndex})");
                        }
                        else
                        {
                            //パレット登録成功の場合は続行
                        }

                        //インデックス最大値判定
                        if (colorIndex < CommonConsts.Palettes.Count.Max)
                        {
                            //パレット最大値未満の場合は処理続行
                        }
                        else
                        {
                            //パレット最大値以上の場合はエラー
                            return ConvertAnimationIndexColor_GetColorDataResult.Failed(ConvertAnimationIndexColor_GetColorDataResultCodes.ColorOver, $"色インデックスが最大値超過です (index={colorIndex})");
                        }

                        //パレット番号を変換後データ設定先に設定
                        {
                            //データの位置を計算
                            int i = (y * imagePixelWidth) + x;

                            //インデックス設定
                            outputData[i] = (byte)colorIndex;
                        }
                    }
                }

                //ここまで来たら成功で終了
                return ConvertAnimationIndexColor_GetColorDataResult.Success(image, outputData, colors);
            }
            catch (Exception ex)
            {
                //例外は握りつぶす
                return ConvertAnimationIndexColor_GetColorDataResult.Failed(ConvertAnimationIndexColor_GetColorDataResultCodes.CatchException, CommonLogger.Warn("色インデックス作成で例外発生", ex));
            }
        }
    }
}
