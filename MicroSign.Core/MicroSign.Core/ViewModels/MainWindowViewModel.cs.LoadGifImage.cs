using System;
using System.Text;
using System.Windows.Media.Imaging;

namespace MicroSign.Core.ViewModels
{
    partial class MainWindowViewModel
    {
        /// <summary>
        /// GIFアニメーション読込
        /// </summary>
        public struct LoadGifAnimationResult
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
            /// アニメーションイメージ
            /// </summary>
            public readonly AnimationImageItemCollection? AnimationImages;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="isSuccess"></param>
            /// <param name="message"></param>
            /// <param name="animationImages"></param>
            private LoadGifAnimationResult(bool isSuccess, string? message, AnimationImageItemCollection? animationImages)
            {
                this.IsSuccess = isSuccess;
                this.ErrorMessage = message;
                this.AnimationImages = animationImages;
            }

            /// <summary>
            /// 失敗
            /// </summary>
            /// <param name="message"></param>
            /// <returns></returns>
            public static LoadGifAnimationResult Failed(string message)
            {
                LoadGifAnimationResult result = new LoadGifAnimationResult(false, message, null);
                return result;
            }

            /// <summary>
            /// 成功
            /// </summary>
            /// <param name="animationImages"></param>
            /// <returns></returns>
            public static LoadGifAnimationResult Success(AnimationImageItemCollection? animationImages)
            {
                LoadGifAnimationResult result = new LoadGifAnimationResult(true, null, animationImages);
                return result;
            }
        }

        /// <summary>
        /// GIF読込
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public LoadGifAnimationResult LoadGifAnimation(string? path)
        {
            //読込先パス有効判定
            {
                bool isNull = string.IsNullOrEmpty(path);
                if (isNull)
                {
                    //無効の場合は終了
                    string msg = "読込先パスが無効です";
                    LOGGER.Warn(msg);
                    return LoadGifAnimationResult.Failed(msg);
                }
                else
                {
                    //有効の場合は処理続行
                    LOGGER.Debug($"読込先パス有効  (path='{path}')");
                }
            }

            //出力フォルダーを生成
            string? outputFolder = null;
            string? outputFilename = null;
            try
            {
                //拡張子をのぞいたファイル名を取得
                outputFilename = System.IO.Path.GetFileNameWithoutExtension(path);
                {
                    bool isNull = string.IsNullOrEmpty(outputFilename);
                    if (isNull)
                    {
                        //無効の場合は修了
                        string msg = $"ファイル名の取得に失敗しました (path='{path}')";
                        LOGGER.Warn(msg);
                        return LoadGifAnimationResult.Failed(msg);
                    }
                    else
                    {
                        //取得できた場合は処理続行
                        LOGGER.Debug($"ファイル名の取得成功  (path='{path}' -> '{outputFilename}')");
                    }
                }

                //ディレクトリ部を取得
                string? dir = System.IO.Path.GetDirectoryName(path);
                {
                    bool isNull = string.IsNullOrEmpty(dir);
                    if (isNull)
                    {
                        //無効の場合は修了
                        string msg = $"ディレクトリの取得に失敗しました (path='{path}')";
                        LOGGER.Warn(msg);
                        return LoadGifAnimationResult.Failed(msg);
                    }
                    else
                    {
                        //取得できた場合は処理続行
                        LOGGER.Debug($"ディレクトリの取得成功  (path='{path}' -> '{dir}')");
                    }
                }

                //出力フォルダ名を生成
                outputFolder = System.IO.Path.Combine(dir!, outputFilename!);
                LOGGER.Debug($"出力先フォルダ ('{dir}' + '{outputFilename}' -> '{outputFolder}')");

                //出力フォルダを作成
                LOGGER.Debug($"出力先フォルダの生成  ('{outputFolder}')");
                System.IO.Directory.CreateDirectory(outputFolder);
            }
            catch (Exception ex)
            {
                //例外は握りつぶす
                string msg = $"出力フォルダの作成で例外が発生しました (path='{path}' / outputFolder='{outputFolder}') ({ex})";
                LOGGER.WarnEx(msg, ex);
                return LoadGifAnimationResult.Failed(msg);
            }

            //アニメーション画像コレクション
            // >> コンストラクタで生成しているのでnullチェック不要
            AnimationImageItemCollection animationImages = this.AnimationImages;

            try
            {
                //GIFファイル読込
                using (System.Drawing.Bitmap gif = new System.Drawing.Bitmap(path!))
                {
                    //フレームの表示時間プロパティ取得
                    System.Drawing.Imaging.PropertyItem? frameDelayTimePropertyItem = gif.GetPropertyItem(20736);
                    if (frameDelayTimePropertyItem == null)
                    {
                        //無効の場合は終了
                        string msg = "フレームの表示時間プロパティ取得失敗";
                        LOGGER.Warn(msg);
                        return LoadGifAnimationResult.Failed(msg);
                    }
                    else
                    {
                        //有効の場合は処理続行
                    }

                    //フレームの表示時間プロパティのバイト配列取得
                    byte[]? frameDelayTimePropertyItemValue = frameDelayTimePropertyItem.Value;
                    if (frameDelayTimePropertyItemValue == null)
                    {
                        //無効の場合は修了
                        string msg = "フレームの表示時間プロパティのValue取得失敗";
                        LOGGER.Warn(msg);
                        return LoadGifAnimationResult.Failed(msg);
                    }
                    else
                    {
                        //有効の場合は処理続行
                    }

                    //FrameDimensionsListが有効か判定
                    int n = gif.FrameDimensionsList.Length;
                    if (CommonConsts.Collection.Empty < n)
                    {
                        //フレームがある場合は処理続行
                    }
                    else
                    {
                        //フレームがない場合は修了
                        string msg = "アニメーションがサポートされていません";
                        LOGGER.Warn(msg);
                        return LoadGifAnimationResult.Failed(msg);
                    }

                    //ファイル番号フォーマットを取得
                    string fileNumberFormat = string.Empty;
                    {
                        int log = (int)Math.Log10(n);
                        int m = log + CommonConsts.Collection.Step;
                        StringBuilder sb = new StringBuilder();
                        for (int i = CommonConsts.Index.First; i < m; i += CommonConsts.Index.Step)
                        {
                            sb.Append(CommonConsts.File.ZeroPrace);
                        }
                        fileNumberFormat = sb.ToString();
                    }

                    //1つめのFrameDimensions GUID取得
                    Guid id = gif.FrameDimensionsList[CommonConsts.Index.First];

                    //FrameDimensionを取得する
                    System.Drawing.Imaging.FrameDimension fd = new System.Drawing.Imaging.FrameDimension(id);

                    //フレーム数を取得
                    int frameCount = gif.GetFrameCount(fd);
                    if (CommonConsts.Collection.Empty < frameCount)
                    {
                        //フレーム数がある場合は処理続行
                    }
                    else
                    {
                        //フレーム数が無い場合は終了
                        string msg = "アニメーション画像ではありません";
                        LOGGER.Warn(msg);
                        return LoadGifAnimationResult.Failed(msg);
                    }


                    //パネルサイズを判定
                    {
                        //GIFサイズを取得
                        int pixelWidth = gif.Width;
                        int pixelHeight = gif.Height;

                        //パネルサイズ取得
                        int panelWidth = this.MatrixLedWidth;
                        int panelHeight = this.MatrixLedHeight;

                        //パネルサイズが一致するか判定
                        bool isFit = AnimationImageItem.IsFit(panelWidth, panelHeight, pixelWidth, pixelHeight);
                        if (isFit)
                        {
                            //適合した場合は処理続行
                            LOGGER.Debug($"パネルサイズに適合 (panel=[{panelWidth},{panelHeight}] == pixel=[{pixelWidth}, {pixelHeight}])");
                        }
                        else
                        {
#warning GIFで画像サイズが異なる場合はクリップする


                            //適合しない場合は失敗にする
                            string msg = $"パネルサイズに適合しない画像です (path='{path}') (panel=[{panelWidth},{panelHeight}] == pixel=[{pixelWidth}, {pixelHeight}])";
                            LOGGER.Warn(msg);
                            return LoadGifAnimationResult.Failed(msg);
                        }
                    }

                    //2025.08.25:CS)土田:複数GIFを同時に読み込めるように、リストクリアのタイミングを変更 >>>>> ここから 
                    ////アニメーション画像リストをクリア
                    //// >> イベント処理した場合にRemoveとして扱いたいのでClear()を使わずRemoveAt()で削除します
                    //{
                    //    int c = CommonUtils.GetCount(animationImages);
                    //    for (int i = CommonConsts.Index.First; i < c; i += CommonConsts.Index.Step)
                    //    {
                    //        animationImages.RemoveAt(CommonConsts.Index.First);
                    //    }
                    //}
                    //----------
                    //2025.08.25:CS)土田:複数GIFを同時に読み込めるように、リストクリアのタイミングを変更 <<<<< ここまで

                    //フレームを処理する
                    for (int i = CommonConsts.Index.First; i < frameCount; i += CommonConsts.Index.Step)
                    {
                        //フレームを選択する
                        gif.SelectActiveFrame(fd, i);

                        //PNGで保存
                        //ファイル名をMP4と同じとなるようにする >>>>> ここから
                        //string fileNumberText = i.ToString(fileNumberFormat);
                        //string saveName = string.Format(CommonConsts.File.PngFileFormat, fileNumberText);
                        //----------
                        string saveName = string.Format(CommonConsts.File.PngFileFormat, outputFilename, i);
                        //ファイル名をMP4と同じとなるようにする <<<<< ここまで
                        string savePath = System.IO.Path.Combine(outputFolder, saveName);
                        gif.Save(savePath, System.Drawing.Imaging.ImageFormat.Png);

                        //表示時間(秒)を取得
                        double displayPeriod = CommonConsts.Time.Zero;
                        {
                            //表示時間(ms)を取得
                            //2025.08.25:CS)土田:表示時間のインデックスがフレームに連動するように修正 >>>>> ここから
                            //int time = BitConverter.ToInt32(frameDelayTimePropertyItemValue, CommonConsts.Index.First) * CommonConsts.Time.GifUnitTime;
                            //----------
                            int timeIndex = i * CommonConsts.ByteCount.INT;
                            int time = BitConverter.ToInt32(frameDelayTimePropertyItemValue, timeIndex) * CommonConsts.Time.GifUnitTime;
                            //2025.08.25:CS)土田:表示時間のインデックスがフレームに連動するように修正 <<<<< ここまで

                            //表示時間を秒に変換
                            TimeSpan ts = TimeSpan.FromMilliseconds(time);
                            displayPeriod = ts.TotalSeconds;
                        }

                        //画像を読込
                        BitmapImage? image = this.GetImage(savePath);
                        if (image == null)
                        {
                            //取得出来なかった場合は終了
                            string msg = $"画像ファイルの読込に失敗しました (path='{savePath}')";
                            LOGGER.Warn(msg);
                            return LoadGifAnimationResult.Failed(msg);
                        }
                        else
                        {
                            //有効の場合は処理続行
                            LOGGER.Debug($"画像ファイル読込成功  (path='{savePath}')");
                        }

                        //2025.08.12:CS)杉原:パレット処理の流れを変更 >>>>> ここから
                        ////2023.11.30:CS)土田:プレビュー表示の改造 >>>>> ここから
                        ////----------
                        ////読み込みと同時に変換する
                        //MicroSign.Core.Models.Model.ConvertImageResult convertImageResult = this.ConvertAnimationImage(image);
                        //if (convertImageResult.IsSuccess)
                        //{
                        //    //成功の場合は続行
                        //    CommonLogger.Debug($"画像ファイル変換成功");
                        //}
                        //else
                        //{
                        //    //変換失敗の場合は終了
                        //    string msg = $"画像ファイルの変換に失敗しました (path='{savePath}')";
                        //    CommonLogger.Warn(msg);
                        //    return LoadGifAnimationResult.Failed(msg);
                        //}
                        ////2023.11.30:CS)土田:プレビュー表示の改造 <<<<< ここまで
                        //
                        ////アニメーション画像アイテム生成
                        //AnimationImageItem animationImageItem = AnimationImageItem.FromImageFile
                        //(
                        //    displayPeriod,
                        //    savePath,
                        //    image,
                        //    convertImageResult.OutputData,
                        //    convertImageResult.PreviewImage
                        //);
                        //----------
                        // >> プレビュー画像が不要になったので変換した画像も不要となりました
                        //アニメーション画像アイテム生成
                        AnimationImageItem animationImageItem = AnimationImageItem.FromImageFile
                        (
                            displayPeriod,
                            savePath,
                            image
                        );
                        //2025.08.12:CS)杉原:パレット処理の流れを変更 <<<<< ここまで

                        //リストに追加
                        this.AddAnimationImage(animationImageItem);
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = $"GIFファイルの解析に失敗しました ({ex})";
                LOGGER.WarnEx(msg, ex);
                return LoadGifAnimationResult.Failed(msg);
            }

            //先頭を選択する
            {
                int c = animationImages.Count;
                if (CommonConsts.Collection.Empty < c)
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
                    //2026.06.05:CS)杉原:複数選択対応 >>>>> ここから
                    //this.SetSelectAnimationImage(MainWindowViewModel.InitializeValues.SelectedAnimationImageItem);
                    //----------
                    // >> 選択解除する
                    this.UnselectAnimationImages();
                    //2026.06.05:CS)杉原:複数選択対応 <<<<< ここまで
                }
            }

            //ここまで来たら成功で終了
            return LoadGifAnimationResult.Success(animationImages);
        }
    }
}
