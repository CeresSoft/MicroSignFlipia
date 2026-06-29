using MicroSign.Core;
using MicroSign.Core.Navigations;
using MicroSign.Core.Navigations.Enums;
using MicroSign.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;
using static MicroSign.Core.Views.Pages.AnimationClipPage;

namespace MicroSignFlipia
{
    partial class MainWindow
    {
        /// <summary>
        /// アニメーション画像追加
        /// </summary>
        /// <param name="imagePaths">追加するアニメーション画像一覧</param>
        protected void AddAnimationImages(string[]? imagePaths)
        {
            //デフォルトの表示期間を取得
            double defaultDisplayPeriod = this.ViewModel.DefaultDisplayPeriod;

            //デフォルトの表示期間で追加
            this.AddAnimationImages(imagePaths, defaultDisplayPeriod);
        }

        /// <summary>
        /// アニメーション画像追加
        /// </summary>
        /// <param name="imagePaths">追加するアニメーション画像一覧</param>
        /// <param name="displayPeriod">表示期間</param>
        protected void AddAnimationImages(List<string>? imagePaths, double displayPeriod)
        {
            if (imagePaths == null)
            {
                //無効の場合は何もしない
                string msg = "追加するアニメーション画像一覧が無効です";
                LOGGER.Warn(msg);
                this.ShowWarning(msg);
                return;
            }
            else
            {
                //有効の場合は続行
                LOGGER.Debug("追加するアニメーション画像一覧が有効");
            }

            //配列に変換
            string[] imagePathsArray = imagePaths.ToArray();

            //アニメーション画像追加
            this.AddAnimationImages(imagePathsArray, displayPeriod);
        }

        /// <summary>
        /// アニメーション画像追加
        /// </summary>
        /// <param name="imagePaths">追加するアニメーション画像一覧</param>
        /// <param name="displayPeriod">表示期間</param>
        protected void AddAnimationImages(string[]? imagePaths, double displayPeriod)
        {
            //アニメーション画像一覧有効判定
            if (imagePaths == null)
            {
                //無効の場合は終了
                LOGGER.Warn("追加するアニメーション画像一覧が無効です");
                return;
            }
            else
            {
                //有効の場合は処理続行
            }

            //ファイル数を取得
            int count = imagePaths.Length;
            if (CommonConsts.Collection.Empty < count)
            {
                //1件以上場合は処理続行
            }
            else
            {
                //0件の場合は終了
                LOGGER.Warn("追加するアニメーション画像一覧が空です");
                return;
            }

            ////2024.04.30:CS)杉原:リリース向けの機能追加 >>>>> ここから
            ////----------
            //// >> そのうちスクロール機能を入れようと思って画像サイズは2の累乗であればなんでも良い仕様にしていましたが
            //// >> パネルが128x32の設定なのに64x64の画像を追加して変換できてしまうのは少々わかりにくい
            //// >> とりあえずパネルのサイズと異なるサイズの画像が有った場合は変換できないようにします
            ////設定されているパネルサイズを取得
            //int panelWidth = this.ViewModel.MatrixLedWidth;
            //int panelHeight = this.ViewModel.MatrixLedHeight;
            ////2024.04.30:CS)杉原:リリース向けの機能追加 <<<<< ここまで

            //ファイル名でソートしながら読込
            //2026.04.14:CS)杉原:自然順ソートに変更 >>>>> ここから
            //IOrderedEnumerable<string> sortedImagePaths = imagePaths.OrderBy(x => x);
            //----------
            // >> ファイルパスを画像ファイルパスクラスに変換する
            // >> >> 先頭または最後の数値部分を比較できるようにクラスにしました
            List<ImageFilePath> imageFilePaths = new List<ImageFilePath>();
            {
                int n = CommonUtils.GetCount(imagePaths);
                for (int i = CommonConsts.Index.First; i < n; i += CommonConsts.Index.Step)
                {
                    string path = imagePaths[i];
                    ImageFilePath? ret = ImageFilePath.Create(path);
                    if (ret == null)
                    {
                        //変換できない場合は無視
                    }
                    else
                    {
                        //変換できた場合は追加
                        imageFilePaths.Add(ret);
                    }
                }
            }
            // >> ソートしたパスを得る
            IEnumerable<string> sortedImagePaths = imageFilePaths.OrderBy(x => x).Select(x => x.Path);
            //2026.04.14:CS)杉原:自然順ソートに変更 <<<<< ここまで
            foreach (string imagePath in sortedImagePaths)
            {
                //拡張子を取得
                string? ext = System.IO.Path.GetExtension(imagePath);
                // >> 比較しやすいように小文字化
                string? extLower = CommonUtils.ToSafeLower(ext);

                //画像拡張子判定
                switch (extLower)
                {
                    case CommonConsts.File.GifExtension:
                        //GIFアニメーション追加
                        this.ViewModel.LoadGifAnimation(imagePath);
                        break;

                    //2026.05.31:CS)杉原:MP4対応 >>>>> ここから
                    //----------
                    case CommonConsts.File.MP4Extension:
                        //MP4アニメーション追加
                        this.ViewModel.LoadMp4Animation(imagePath);
                        break;
                    //2026.05.31:CS)杉原:MP4対応 >>>>> ここから

                    default:
                        //通常のアニメーション画像追加
                        this.AddAnimationImagesImpl(imagePath, displayPeriod /*,panelWidth, panelHeight*/);
                        break;
                }
            }
        }

        /// <summary>
        /// アニメーション画像追加
        /// </summary>
        /// <param name="imagePath"></param>
        /// <param name="defaultDisplayPeriod"></param>
        private void AddAnimationImagesImpl(string imagePath, double defaultDisplayPeriod)
        {
            //画像を読込
            BitmapSource? image = this.ViewModel.GetImage(imagePath);
            if (image == null)
            {
                //取得出来なかった場合は終了
                string msg = $"画像ファイルの読込に失敗しました\npath='{imagePath}'";
                LOGGER.Error(msg);
                this.ShowError(msg);
                return;
            }
            else
            {
                //有効の場合は処理続行
                LOGGER.Info($"画像ファイル読込\npath='{imagePath}'");
            }

            //2024.04.30:CS)杉原:リリース向けの機能追加 >>>>> ここから
            //----------
            // >> そのうちスクロール機能を入れようと思って画像サイズは2の累乗であればなんでも良い仕様にしていましたが
            // >> パネルが128x32の設定なのに64x64の画像を追加して変換できてしまうのは少々わかりにくい
            // >> とりあえずパネルのサイズと異なるサイズの画像が有った場合は変換できないようにします
            //設定されているパネルサイズを取得
            int panelWidth = this.ViewModel.MatrixLedWidth;
            int panelHeight = this.ViewModel.MatrixLedHeight;
            //2024.04.30:CS)杉原:リリース向けの機能追加 <<<<< ここまで

            //2025.08.12:CS)杉原:パレット処理の流れを変更 >>>>> ここから
            ////2023.11.30:CS)土田:プレビュー表示の改造 >>>>> ここから
            ////----------
            ////読み込みと同時に変換する
            //MicroSign.Core.Models.Model.ConvertImageResult convertImageResult = this.ViewModel.ConvertAnimationImage(image);
            //if (convertImageResult.IsSuccess)
            //{
            //    //成功の場合は続行
            //    CommonLogger.Debug($"画像ファイル変換成功");
            //}
            //else
            //{
            //    //変換失敗の場合は終了
            //    this.ShowError(CommonLogger.Error($"画像ファイルの変換に失敗しました\npath='{imagePath}'"));
            //    return;
            //}
            ////2023.11.30:CS)土田:プレビュー表示の改造 <<<<< ここまで
            //
            ////アニメーション画像アイテムを生成
            ////2023.12.24:CS)杉原:アニメーション画像アイテム種類追加 >>>>> ここから
            ////AnimationImageItem animationImageItem = new AnimationImageItem()
            ////{
            ////    Name = System.IO.Path.GetFileName(imagePath),
            ////    Path = imagePath,
            ////    Image = image,
            ////    DisplayPeriod = defaultDisplayPeriod,
            ////    //2023.11.30:CS)土田:プレビュー表示の改造 >>>>> ここから
            ////    //----------
            ////    OutputData = convertImageResult.OutputData,
            ////    PreviewImage = convertImageResult.PreviewImage,
            ////    //2023.11.30:CS)土田:プレビュー表示の改造 <<<<< ここまで
            ////};
            ////----------
            //AnimationImageItem animationImageItem = AnimationImageItem.FromImageFile(
            //    defaultDisplayPeriod,
            //    imagePath,
            //    image,
            //    convertImageResult.OutputData,
            //    convertImageResult.PreviewImage
            //    );
            ////2023.12.24:CS)杉原:アニメーション画像アイテム種類追加 <<<<< ここまで
            //----------
            // >> プレビュー画像が不要になったので変換した画像も不要となりました
            //アニメーション画像アイテムを生成
            AnimationImageItem animationImageItem = AnimationImageItem.FromImageFile(
                defaultDisplayPeriod,
                imagePath,
                image
                );
            //2025.08.12:CS)杉原:パレット処理の流れを変更 <<<<< ここまで

            //2024.04.30:CS)杉原:リリース向けの機能追加 >>>>> ここから
            //----------
            // >> そのうちスクロール機能を入れようと思って画像サイズは2の累乗であればなんでも良い仕様にしていましたが
            // >> パネルが128x32の設定なのに64x64の画像を追加して変換できてしまうのは少々わかりにくい
            // >> とりあえずパネルのサイズと異なるサイズの画像が有った場合は変換できないようにします
            {
                //アニメーション画像が有効の場合適合するか判定
                bool isFit = animationImageItem.IsFit(panelWidth, panelHeight);
                if (isFit)
                {
                    //適合した場合は処理続行
                }
                else
                {
                    //2025.09.22:CS)土田:パネルサイズより大きい場合は切り抜く機能追加 >>>>> ここから
                    ////適合しない場合は失敗にする
                    //this.ShowWarning(CommonLogger.Warn($"パネルサイズに適合しない画像です\npath='{imagePath}'"));
                    //----------
                    //アニメーション切り抜きを行う
                    this.ClipAnimationImage(image, imagePath, panelWidth, panelHeight);
                    //2025.09.22:CS)土田:パネルサイズより大きい場合は切り抜く機能追加 <<<<< ここまで
                    return;
                }
            }
            //2024.04.30:CS)杉原:リリース向けの機能追加 <<<<< ここまで

            //リストに追加
            this.ViewModel.AddAnimationImage(animationImageItem);
        }

        /// <summary>
        /// アニメーション切り抜き
        /// </summary>
        /// <param name="image"></param>
        /// <param name="imagePath"></param>
        /// <param name="panelWidth"></param>
        /// <param name="panelHeight"></param>
        private void ClipAnimationImage(BitmapSource image, string imagePath, int panelWidth, int panelHeight)
        {
            //切り抜きページを表示
            MicroSign.Core.Views.Pages.AnimationClipPage page = new MicroSign.Core.Views.Pages.AnimationClipPage(panelWidth, panelHeight, image, imagePath);
            // >> 画面からの戻りを待つ
            object result = this.NaviPanel.NavigationCallWait(page, null);
            if (result == null)
            {
                //戻り値がnullの場合、成功でも失敗でもないので何もしない
                LOGGER.Debug("アニメーション切り抜きページの戻り値なし");
                return;
            }
            else
            {
                //戻り値が有効の場合は続行
            }

            //アニメーション切り抜き結果を取得
            AnimationClipPageResult? pageResult = result as AnimationClipPageResult;
            if (pageResult == null)
            {
                //結果無効の場合は失敗
                this.ShowWarning("アニメーション切り抜きに失敗しました");
                return;
            }
            else
            {
                //結果有効の場合は続行
            }

            //結果種類を取得
            NavigationResultKind resultKind = pageResult.ResultKind;
            switch(resultKind)
            {
                case NavigationResultKind.Success:
                    //成功の場合は処理続行
                    LOGGER.Debug("アニメーション切り抜き追加成功");
                    break;

                case NavigationResultKind.Cancel:
                    //キャンセルの場合は何もせずに終了
                    LOGGER.Info("アニメーション切り抜きキャンセル");
                    return;

                default:
                    //それ以外は失敗
                    {
                        string msg = $"アニメーション切り抜きに失敗しました (理由={resultKind}')";
                        LOGGER.Warn(msg);
                        this.ShowWarning(msg);
                    }
                    return;
            }

            //出力結果を取得
            List<string>? outputPaths = pageResult.OutputPaths;
            double displayPeriod = pageResult.DisplayPeriod;

            //アニメーション画像追加
            this.AddAnimationImages(outputPaths, displayPeriod);
        }
    }
}
