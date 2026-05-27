using MicroSign.Core.Models.AnimationSaveSettings;
using static MicroSign.Core.Models.Model;

namespace MicroSign.Core.ViewModels
{
    partial class MainWindowViewModel
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
            /// コンストラクタ
            /// </summary>
            /// <param name="isSuccess"></param>
            /// <param name="errorMessage"></param>
            private LoadAnimationResult(bool isSuccess, string? errorMessage)
            {
                this.IsSuccess = isSuccess;
                this.ErrorMessage = errorMessage;
            }

            /// <summary>
            /// 失敗
            /// </summary>
            /// <param name="errorMessage"></param>
            /// <returns></returns>
            public static LoadAnimationResult Failed(string? errorMessage)
            {
                LoadAnimationResult result = new LoadAnimationResult(false, errorMessage);
                return result;
            }

            /// <summary>
            /// 成功
            /// </summary>
            /// <returns></returns>
            public static LoadAnimationResult Success()
            {
                LoadAnimationResult result = new LoadAnimationResult(true, null);
                return result;
            }
        }

        /// <summary>
        /// アニメーション設定読込
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public LoadAnimationResult LoadAnimation(string path)
        {
            //読込
            MicroSign.Core.Models.Model.LoadAnimationResult retLoad = this.Model.LoadAnimation(path);
            if(retLoad.IsSuccess)
            {
                //成功の場合は処理続行
            }
            else
            {
                //失敗の場合は終了
                return LoadAnimationResult.Failed(retLoad.ErrorMessage);
            }

            //保存設定を取得
            AnimationSaveSetting? saveSetting = retLoad.SaveSetting;
            if (saveSetting == null)
            {
                //無効の場合は終了
                return LoadAnimationResult.Failed(CommonLogger.Warn("読込したアニメーション設定が無効です"));
            }
            else
            {
                //有効の場合は処理続行
            }

            //読込パスのディレクト入り部分を取得
            // >> 基準パスとします
            string? baseDir = System.IO.Path.GetDirectoryName(path);
            if (baseDir == null)
            {
                //ベースディレクトリが無効の場合は終了
                return LoadAnimationResult.Failed(CommonLogger.Warn($"読込パスのディレクトリが取得出来ません"));
            }
            else
            {
                //有効の場合は処理続行
            }

            //新しいリストを取得
            AnimationImageItemCollection? newList = retLoad.AnimationImages;
            if (newList == null)
            {
                //無効の場合は終了
                return LoadAnimationResult.Failed(CommonLogger.Warn("読込したアニメーションが無効です"));
            }
            else
            {
                //有効の場合は処理続行
            }

            //マトリクスLEDの情報を取得
            int matrixLedWidth = saveSetting.MatrixLedWidth;
            int matrixLedHeight = saveSetting.MatrixLedHeight;
            int matrixLedBrightness = saveSetting.MatrixLedBrightness;
            CommonLogger.Debug($"マトリクスLED (Width={matrixLedWidth}, height={matrixLedHeight}, brightness={matrixLedBrightness})");

            //マトリクスLEDの有効判定
            {
                //画像サイズ検証を流用します
                ValidateImageSizeResult retSize = this.Model.ValidateImageSize(matrixLedWidth, matrixLedHeight);
                if(retSize.IsValid)
                {
                    //有効の場合は処理続行
                    CommonLogger.Debug($"マトリクスLEDサイズ有効 (Width={matrixLedWidth}, height={matrixLedHeight})");
                }
                else
                {
                    //サイズが無効の場合は終了
                    return LoadAnimationResult.Failed(CommonLogger.Warn($"マトリクスLEDサイズ有効 (Width={matrixLedWidth}, height={matrixLedHeight})"));
                }
            }

            //アニメーション名を復元
            this.AnimationName = saveSetting.Name;

            //マトリクスLEDの設定を復元
            this.MatrixLedWidth = matrixLedWidth;
            this.MatrixLedHeight = matrixLedHeight;
            this.MatrixLedBrightness = matrixLedBrightness;

            //デフォルト表示期間(秒)を復元
            this.DefaultDisplayPeriod = saveSetting.DefaultDisplayPeriod;

            //2026.05.22:CS)杉原:サウンド機能追加 >>>>> ここから
            //-----
            {
                string? soundFilePath = saveSetting.SoundFilePath;
                bool isNull = string.IsNullOrEmpty(soundFilePath);
                if (isNull)
                {
                    //サウンドファイルが指定されない場合はサウンドファイルパスをクリアする
                    this.ClearSoundFilePath();
                }
                else
                {
                    //サウンドファイルが指定されている場合設定する
                    CommonLogger.Debug($"サウンドファイル有効 (path={soundFilePath})");
                    this.SetSoundFilePath(soundFilePath);
                }
            }
            //2026.05.22:CS)杉原:サウンド機能追加 <<<<< ここまで

            //アニメーション画像コレクション
            // >> コンストラクタで生成しているのでnullチェック不要
            AnimationImageItemCollection animationImages = this.AnimationImages;

            //アニメーション画像リストをクリア
            // >> イベント処理した場合にRemoveとして扱いたいのでClear()を使わずRemoveAt()で削除します
            {
                int c = CommonUtils.GetCount(animationImages);
                for (int i = CommonConsts.Index.First; i < c; i += CommonConsts.Index.Step)
                {
                    animationImages.RemoveAt(CommonConsts.Index.First);
                }
            }

            //アニメーション画像リストに反映
            {
                int c = CommonUtils.GetCount(newList);
                for (int i = CommonConsts.Index.First; i < c; i += CommonConsts.Index.Step)
                {
                    AnimationImageItem item = newList[i];
                    if(item == null)
                    {
                        //アニメーション画像が無効の場合は追加しない
                    }
                    else
                    {
                        //アニメーション画像が有効の場合は追加する
                        animationImages.Add(item);
                    }
                }
            }

            //先頭を選択する
            {
                int c = animationImages.Count;
                if(CommonConsts.Collection.Empty < c)
                {
                    //用紙がある場合
                    // >> 先頭要素を取得
                    AnimationImageItem firstItem = animationImages[CommonConsts.Index.First];
                    // >> 選択する
                    this.SetSelectAnimationImage(firstItem);
                }
                else
                {
                    //要素が無い場合は何もしない
                }
            }

            //ここまで来たら成功
            return LoadAnimationResult.Success();
        }
    }
}
