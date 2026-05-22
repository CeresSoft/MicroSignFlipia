using MicroSign.Core.Models.AnimationSaveSettings;
using MicroSign.Core.SerializerUtils;
using MicroSign.Core.ViewModels;

namespace MicroSign.Core.Models
{
    partial class Model
    {
        /// <summary>
        /// アニメーション保存結果
        /// </summary>
        /// <remarks>
        /// 2025.08.12:CS)杉原:パレット処理の流れを変更で追加
        /// </remarks>
        public struct SaveAnimationResult
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
            private SaveAnimationResult(bool isSuccess, string? message)
            {
                this.IsSuccess = isSuccess;
            }

            /// <summary>
            ///  変換失敗
            /// </summary>
            /// <param name="message">メッセージ</param>
            /// <returns></returns>
            public static SaveAnimationResult Failed(string message)
            {
                SaveAnimationResult result = new SaveAnimationResult(false, message);
                return result;
            }

            /// <summary>
            /// 成功
            /// </summary>
            /// <returns></returns>
            public static SaveAnimationResult Success()
            {
                SaveAnimationResult result = new SaveAnimationResult(true, null);
                return result;
            }
        }

        /// <summary>
        /// アニメーション保存
        /// </summary>
        /// <param name="path">保存先パス</param>
        /// <param name="name">アニメーション名</param>
        /// <param name="animationDatas">アニメーション画像コレクション</param>
        /// <param name="matrixLedWidth">マトリクスLED横幅</param>
        /// <param name="matrixLedHeight">マトリクスLED縦幅</param>
        /// <param name="matrixLedBrightness">マトリクスLED明るさ</param>
        /// <param name="defaultDisplayPeriod">デフォルト表示期間(秒)</param>
        /// <param name="soundFilePath">サウンドファイルパス(2026.05.22:CS)杉原:サウンド機能追加)</param>
        /// <returns>アニメーション保存結果</returns>
        public SaveAnimationResult SaveAnimation(string path, string? name, AnimationImageItemCollection animationDatas, int matrixLedWidth, int matrixLedHeight, int matrixLedBrightness, double defaultDisplayPeriod, string? soundFilePath)
        {
            //保存先パス有効判定
            {
                bool isNull = string.IsNullOrEmpty(path);
                if(isNull)
                {
                    //無効の場合は終了
                    return SaveAnimationResult.Failed("保存先パスが無効です");
                }
                else
                {
                    //有効の場合は処理続行
                }
            }

            //アニメーション画像データ数取得
            int animationDataCount = CommonUtils.GetCount(animationDatas);
            if(CommonConsts.Collection.Empty < animationDataCount)
            {
                //有効の場合は処理続行
            }
            else
            {
                //無効の場合は終了
                return SaveAnimationResult.Failed("アニメーション画像が空です");
            }

            //保存パスのディレクト入り部分を取得
            // >> 基準パスとします
            string? baseDir = System.IO.Path.GetDirectoryName(path);

            //保存用のデータを作成
            AnimationSaveSetting animationSaveSetting = new AnimationSaveSetting();
            if(name == null)
            {
                //無効の場合は何もしない(=デフォルト値)
            }
            else
            {
                bool isNull = string.IsNullOrEmpty (path);
                if(isNull)
                {
                    //無効の場合は何もしない(=デフォルト値)
                }
                else
                {
                    //有効の場合は設定
                    animationSaveSetting.Name = name;
                }
            }

            //マトリクスLEDの情報を設定
            animationSaveSetting.MatrixLedWidth = matrixLedWidth;
            animationSaveSetting.MatrixLedHeight = matrixLedHeight;
            animationSaveSetting.MatrixLedBrightness = matrixLedBrightness;

            //デフォルトの表示期間を設定
            animationSaveSetting.DefaultDisplayPeriod = defaultDisplayPeriod;

            //2026.05.22:CS)杉原:サウンド機能追加 >>>>> ここから
            //-----
            {
                bool isNull = string.IsNullOrEmpty(soundFilePath);
                if(isNull)
                {
                    //サウンドファイルが指定されない場合は何もしない
                }
                else
                {
                    //サウンドファイルが指定されている場合
                    // >> 相対パスに変換する
                    string? relativePath = CommonUtils.GetRelativePath(baseDir, soundFilePath);
                    // >> 設定する
                    animationSaveSetting.SoundFilePath = relativePath;
                }
            }
            //2026.05.22:CS)杉原:サウンド機能追加 <<<<< ここまで

            //アニメーション画像アイテムコレクションを変換
            AnimationSaveDataCollection animationSaveDatas = animationSaveSetting.AnimationDatas;
            for (int i = CommonConsts.Index.First; i < animationDataCount; i += CommonConsts.Index.Step)
            {
                AnimationImageItem item = animationDatas[i];
                if(item == null)
                {
                    //無効の場合は無視する
                }
                else
                {
                    //有効の場合はコピーする
                    // >> 画像パスを取得
                    string? imagePath = item.Path;

                    // >> 相対パスに変換する
                    string? relativePath = CommonUtils.GetRelativePath(baseDir, imagePath);

                    // >> 保存
                    AnimationSaveData saveDatsa = new AnimationSaveData()
                    {
                        ImageType = item.ImageType,
                        ImagePath = relativePath,
                        DisplayPeriod = item.DisplayPeriod,
                        Kind = item.Kind,
                        RectX = item.RectX,
                        RectY = item.RectY,
                        RectWidth = item.RectWidth,
                        RectHeight = item.RectHeight,
                        OffsetX = item.OffsetX,
                        OffsetY = item.OffsetY,
                        TransparentRGB = item.TransparentRGB,
                        SelectFontSize = item.SelectFontSize,
                        SelectFontColor = item.SelectFontColor,
                        DisplayText = item.DisplayText,
                    };

                    // >> リストに追加
                    animationSaveDatas.Add(saveDatsa);
                }
            }

            //保存する
            {
                bool ret = JsonUtil.Save(path, animationSaveSetting);
                if(ret)
                {
                    //成功した場合は処理続行
                }
                else
                {
                    //失敗した場合は終了
                    return SaveAnimationResult.Failed("保存に失敗しました");
                }
            }


            //ここまで来たら成功
            return SaveAnimationResult.Success();
        }
    }
}
