using MicroSign.Core.Models.AnimationSaveSettings;
using MicroSign.Core.ViewModels;
using System.Windows.Media.Imaging;

namespace MicroSign.Core.Models
{
    partial class Model
    {
        /// <summary>
        /// アニメーション設定読込結果
        /// </summary>
        public struct LoadAnimationResult
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
            /// アニメーションリスト
            /// </summary>
            public readonly AnimationImageItemCollection? AnimationImages;

            /// <summary>
            /// 読込した保存用アニメーション設定
            /// </summary>
            public readonly AnimationSaveSetting? SaveSetting;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="isSuccess"></param>
            /// <param name="errorMessage"></param>
            /// <param name="animationImages"></param>
            /// <param name="saveSetting"></param>
            private LoadAnimationResult(bool isSuccess, string? errorMessage, AnimationImageItemCollection? animationImages, AnimationSaveSetting? saveSetting)
            {
                this.IsSuccess = isSuccess;
                this.ErrorMessage = errorMessage;
                this.AnimationImages = animationImages;
                this.SaveSetting = saveSetting;
            }

            /// <summary>
            /// 失敗
            /// </summary>
            /// <param name="errorMessage"></param>
            /// <returns></returns>
            public static LoadAnimationResult Failed(string errorMessage)
            {
                LoadAnimationResult result = new LoadAnimationResult(false, errorMessage, null, null);
                return result;
            }

            /// <summary>
            /// 成功
            /// </summary>
            /// <param name="animationImages"></param>
            /// <param name="saveSetting"></param>
            /// <returns></returns>
            public static LoadAnimationResult Success(AnimationImageItemCollection? animationImages, AnimationSaveSetting? saveSetting)
            {
                LoadAnimationResult result = new LoadAnimationResult(true, null, animationImages, saveSetting);
                return result;
            }
        }

        /// <summary>
        /// アニメーション設定読込
        /// </summary>
        /// <param name="path"></param>
        /// <returns>アニメーション設定読込結果</returns>
        public LoadAnimationResult LoadAnimation(string path)
        {
            //読込先パス有効判定
            {
                bool isNull = string.IsNullOrEmpty(path);
                if (isNull)
                {
                    //無効の場合は終了
                    return LoadAnimationResult.Failed("読込先パスが無効です");
                }
                else
                {
                    //有効の場合は処理続行
                }
            }

            //読込
            AnimationSaveSetting? saveSetting = MicroSign.Core.SerializerUtils.JsonUtil.Load<AnimationSaveSetting>(path);
            if(saveSetting == null)
            {
                //読込できなかった場合は終了
                return LoadAnimationResult.Failed("読込に失敗しました");
            }
            else
            {
                //読込できた場合は処理続行
            }

            //マトリクスLEDサイズを取得
            int matrixLedWidth = saveSetting.MatrixLedWidth;
            int matrixLedHeight = saveSetting.MatrixLedHeight;

            //アニメーションのコレクションを取得
            AnimationSaveDataCollection? saveDatas = saveSetting.AnimationDatas;

            //読込パスのディレクト入り部分を取得
            // >> 基準パスとします
            string? baseDir = System.IO.Path.GetDirectoryName(path);
            if(baseDir == null)
            {
                //ベースディレクトリが無効の場合は終了
                return LoadAnimationResult.Failed($"読込パスのディレクトリが取得出来ません\n'{path}'");
            }
            else
            {
                //有効の場合は処理続行
            }

            //アニメーションリストにする
            AnimationImageItemCollection result = new AnimationImageItemCollection();
            {
                int c = CommonUtils.GetCount(saveDatas);
                for (int i = CommonConsts.Index.First; i < c; i += CommonConsts.Index.Step)
                {
                    //アニメーション画像アイテム読込
                    LoadAnimationImplResult ret = this.LoadAnimationImpl(baseDir, matrixLedWidth, matrixLedHeight, i, saveDatas[i]);
                    if(ret.IsSuccess)
                    {
                        //成功の場合は処理続行
                    }
                    else
                    {
                        //失敗の場合は終了
                        string msg = $"アニメーション画像アイテム読込失敗 ({ret.Message})";
                        return LoadAnimationResult.Failed(msg);
                    }

                    // >> アニメーション画像コレクションに追加
                    AnimationImageItem? item = ret.AnimationImage;
                    if(item == null)
                    {
                        //アニメーション画像アイテムが無効の場合は無視する
                    }
                    else
                    {
                        //アニメーション画像アイテムが有効の場合は追加する
                        result.Add(item);
                    }
                }
            }

            //2026.05.22:CS)杉原:サウンド機能追加 >>>>> ここから
            //-----
            // >> サウンドファイルのパスをフルパスにする
            {
                string? soundFilePath = saveSetting.SoundFilePath;
                bool isNull = string.IsNullOrEmpty(soundFilePath);
                if(isNull)
                {
                    //無効の場合は何もしない
                }
                else
                {
                    //有効の場合はフルパスに変換
                    string? soundFullPath = System.IO.Path.Combine(baseDir, soundFilePath!);
                    LOGGER.Debug($"サウンドフルパス (path='{soundFullPath}')");
                    saveSetting.SoundFilePath = soundFullPath;
                }
            }
            //2026.05.22:CS)杉原:サウンド機能追加 <<<<< ここまで

            //ここまで来たら成功
            return LoadAnimationResult.Success(result, saveSetting);
        }

        /// <summary>
        /// アニメーション設定読込実装結果
        /// </summary>
        private struct LoadAnimationImplResult
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
            /// アニメーションアイテム
            /// </summary>
            public readonly AnimationImageItem? AnimationImage;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="isSuccess"></param>
            /// <param name="message"></param>
            /// <param name="animationImage"></param>
            private LoadAnimationImplResult(bool isSuccess, string? message, AnimationImageItem? animationImage)
            {
                this.IsSuccess = isSuccess;
                this.Message = message;
                this.AnimationImage = animationImage;
            }

            /// <summary>
            /// 失敗
            /// </summary>
            /// <param name="message"></param>
            /// <returns></returns>
            public static LoadAnimationImplResult Failed(string message)
            {
                LoadAnimationImplResult result = new LoadAnimationImplResult(false, message, null);
                return result;
            }

            /// <summary>
            /// 成功
            /// </summary>
            /// <param name="animationImage"></param>
            /// <returns></returns>
            public static LoadAnimationImplResult Success(AnimationImageItem? animationImage)
            {
                LoadAnimationImplResult result = new LoadAnimationImplResult(true, null, animationImage);
                return result;
            }

            /// <summary>
            /// 無視
            /// </summary>
            /// <returns></returns>
            public static LoadAnimationImplResult Ignore()
            {
                LoadAnimationImplResult result = new LoadAnimationImplResult(true, null, null);
                return result;
            }
        }

        /// <summary>
        /// アニメーション設定読込実装
        /// </summary>
        /// <param name="baseDir">基準ディレクトリ</param>
        /// <param name="matrixLedWidth">マトリクスLED横幅</param>
        /// <param name="matrixLedHeight">マトリクスLED縦幅</param>
        /// <param name="index">インデックス</param>
        /// <param name="saveData">保存データ</param>
        /// <returns>アニメーション設定読込実装結果</returns>
        private LoadAnimationImplResult LoadAnimationImpl(string baseDir, int matrixLedWidth, int matrixLedHeight, int index, AnimationSaveData? saveData)
        {
            //保存データ有効判定
            if (saveData == null)
            {
                //無効の場合は無視する
                LOGGER.Debug($"[{index}]保存データ無効");
                return LoadAnimationImplResult.Ignore();
            }
            else
            {
                //有効の場合は処理続行
            }

            //アニメーション画像タイプを取得
            AnimationImageType imageType = saveData.ImageType;
            switch (imageType)
            {
                case AnimationImageType.ImageFile:
                    //画像ファイルの場合
                    LOGGER.Debug($"[{index}]アニメーション画像タイプ =　画像ファイル");
                    return this.LoadAnimationImplByImageFile(baseDir, index, saveData);

                case AnimationImageType.Text:
                    //テキストの場合
                    LOGGER.Debug($"[{index}]アニメーション画像タイプ =　テキスト");
                    return this.LoadAnimationImplByText(baseDir, matrixLedWidth, matrixLedHeight, index, saveData);

                default:
                    //それ以外の場合
                    {
                        string msg = $"[{index}]不明なアニメーション画像タイプです ({imageType})";
                        LOGGER.Warn(msg);
                        return LoadAnimationImplResult.Failed(msg);
                    }
            }
        }

        /// <summary>
        /// アニメーション設定読込実装 - 画像ファイル
        /// </summary>
        /// <param name="baseDir">基準ディレクトリ</param>
        /// <param name="index">インデックス</param>
        /// <param name="saveData">保存データ</param>
        /// <returns>アニメーション設定読込実装結果</returns>
        private LoadAnimationImplResult LoadAnimationImplByImageFile(string baseDir, int index, AnimationSaveData? saveData)
        {
            //保存データ有効判定
            if (saveData == null)
            {
                //無効の場合は無視する
                LOGGER.Debug($"[{index}]保存データ無効");
                return LoadAnimationImplResult.Success(null);
            }
            else
            {
                //有効の場合は処理続行
                LOGGER.Debug($"[{index}]保存データ有効");
            }

            //有効の場合はコピーする
            // >> 画像パス取得
            string? imagePath = saveData.ImagePath;
            if (imagePath == null)
            {
                //画像パスが無効の場合は終了
                string msg = $"[{index}]読込した画像パスが無効です";
                LOGGER.Warn(msg);
                return LoadAnimationImplResult.Failed(msg);
            }
            else
            {
                bool isNull = string.IsNullOrEmpty(imagePath);
                if (isNull)
                {
                    //画像パスが無効の場合は終了
                    string msg = $"[{index}]読込した画像パスが空です";
                    LOGGER.Warn(msg);
                    return LoadAnimationImplResult.Failed(msg);
                }
                else
                {
                    //有効の場合は処理続行
                    LOGGER.Debug($"[{index}]読込した画像パス有効 (path='{imagePath}')");
                }
            }

            // >> 画像パスをフルパスに変換
            string? imageFullPath = System.IO.Path.Combine(baseDir, imagePath);
            LOGGER.Debug($"[{index}]読込した画像フルパス (path='{imageFullPath}')");

            // >> 画像を読込
            BitmapSource? bmp = this.GetImage(imageFullPath);
            if (bmp == null)
            {
                //読込出来ない場合は終了
                string msg = $"[{index}]画像が読みできませんでした (path='{imageFullPath}')";
                LOGGER.Warn(msg);
                return LoadAnimationImplResult.Failed(msg);
            }
            else
            {
                //有効の場合は処理続行
                LOGGER.Debug($"[{index}]画像読込成功 (path='{imageFullPath}')");
            }

            //2025.08.12:CS)杉原:パレット処理の流れを変更 >>>>> ここから
            ////フォーマット
            //FormatKinds formatKind = MainWindowViewModel.InitializeValues.FormatKind;
            //Models.Model.ConvertImageResult ret = this.ConvertImage(bmp, formatKind);
            //if(ret.IsSuccess)
            //{
            //    //成功の場合は処理続行
            //    CommonLogger.Debug($"[{index}]画像変換成功");
            //}
            //else
            //{
            //    //失敗の場合は終了
            //    return LoadAnimationImplResult.Failed(CommonLogger.Warn($"[{index}]画像変換に失敗しました (理由='{ret.ErrorMessage}')"));
            //}
            //
            ////表示期間を取得
            //double displayPeriod = saveData.DisplayPeriod;
            //
            //// >> アニメーション画像インスタンス生成
            //AnimationImageItem item = AnimationImageItem.FromImageFile(
            //    displayPeriod,
            //    imageFullPath,
            //    bmp,
            //    ret.OutputData,
            //    ret.PreviewImage
            //    );
            //----------
            // >> プレビュー画像が不要になったので変換した画像も不要となりました
            //表示期間を取得
            double displayPeriod = saveData.DisplayPeriod;

            //アニメーション画像インスタンス生成
            AnimationImageItem item = AnimationImageItem.FromImageFile(
                displayPeriod,
                imageFullPath,
                bmp
                );
            //2025.08.12:CS)杉原:パレット処理の流れを変更 <<<<< ここまで


            //成功で終了
            return LoadAnimationImplResult.Success(item);
        }


        /// <summary>
        /// アニメーション設定読込実装 - テキスト
        /// </summary>
        /// <param name="baseDir">基準ディレクトリ</param>
        /// <param name="matrixLedWidth">マトリクスLED横幅</param>
        /// <param name="matrixLedHeight">マトリクスLED縦幅</param>
        /// <param name="index">インデックス</param>
        /// <param name="saveData">保存データ</param>
        /// <returns>アニメーション設定読込実装結果</returns>
        private LoadAnimationImplResult LoadAnimationImplByText(string baseDir, int matrixLedWidth, int matrixLedHeight, int index, AnimationSaveData? saveData)
        {
            //保存データ有効判定
            if (saveData == null)
            {
                //無効の場合は無視する
                LOGGER.Debug($"[{index}]保存データ無効");
                return LoadAnimationImplResult.Ignore();
            }
            else
            {
                //有効の場合は処理続行
                LOGGER.Debug($"[{index}]保存データ有効");
            }

            //レンダーターゲットビットマップを生成
            RenderTargetBitmap? renderBitmap = null;
            {
                var ret = this.CreateRenderTargetBitmap(matrixLedWidth, matrixLedHeight);
                if (ret.IsSuccess)
                {
                    //成功の場合は処理続行
                }
                else
                {
                    //失敗の場合は終了
                    string msg = $"[{index}]レンダーターゲットビットマップの生成に失敗 (理由='{ret.Message}')";
                    return LoadAnimationImplResult.Failed(msg);
                }

                //生成したレンダーターゲットビットマップを取得
                renderBitmap = ret.RenderBitmap;
            }

            //描写する文字の情報を取得
            int fontSize = saveData.SelectFontSize;
            int fontColor = saveData.SelectFontColor;
            string? displayText = saveData.DisplayText;

            //文字を描写
            {
                var ret = this.RenderText(renderBitmap, fontSize, fontColor, displayText);
                if (ret.IsSuccess)
                {
                    //成功の場合は処理続行
                }
                else
                {
                    //失敗の場合は終了
                    string msg = $"[{index}]文字の描写に失敗 (理由='{ret.Message}')";
                    LOGGER.Warn(msg);
                    return LoadAnimationImplResult.Failed(msg);
                }
            }

            //2025.08.12:CS)杉原:パレット処理の流れを変更 >>>>> ここから
            ////フォーマット
            //FormatKinds formatKind = MainWindowViewModel.InitializeValues.FormatKind;
            //
            ////画像変換
            //Models.Model.ConvertImageResult convertImage = this.ConvertImage(renderBitmap, formatKind);
            //if (convertImage.IsSuccess)
            //{
            //    //成功の場合は処理続行
            //    CommonLogger.Debug($"[{index}]画像変換成功");
            //}
            //else
            //{
            //    //失敗の場合は終了
            //    return LoadAnimationImplResult.Failed(CommonLogger.Warn($"[{index}]画像変換に失敗しました (理由='{convertImage.ErrorMessage}')"));
            //}
            //
            ////表示期間を取得
            //double displayPeriod = saveData.DisplayPeriod;
            //
            //// >> アニメーション画像インスタンス生成
            //AnimationImageItem item = AnimationImageItem.FromText(
            //    displayPeriod,
            //    fontSize,
            //    fontColor,
            //    displayText,
            //    renderBitmap,
            //    convertImage.OutputData,
            //    convertImage.PreviewImage
            //    );
            //----------
            // >> プレビュー画像が不要になったので変換した画像も不要となりました
            //表示期間を取得
            double displayPeriod = saveData.DisplayPeriod;

            //アニメーション画像インスタンス生成
            AnimationImageItem item = AnimationImageItem.FromText(
                displayPeriod,
                fontSize,
                fontColor,
                displayText,
                renderBitmap
                );
            //2025.08.12:CS)杉原:パレット処理の流れを変更 <<<<< ここまで

            //成功で終了
            return LoadAnimationImplResult.Success(item);
        }

    }
}
