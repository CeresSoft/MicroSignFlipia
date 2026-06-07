using MicroSign.Core.Navigations;
using MicroSign.Core.Navigations.Enums;
using MicroSign.Core.ViewModels.Pages;
using MicroSign.Core.Views.Overlaps;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using static MicroSign.Core.ViewModels.Pages.AnimationClipPageViewModel;

namespace MicroSign.Core.Views.Pages
{
    /// <summary>
    /// ImageClipPage.xaml の相互作用ロジック
    /// </summary>
    public partial class AnimationClipPage : UserControl
    {
        //2026.06.07:CS)杉原:LOGGERを修正 >>>>> ここから
        ///// <summary>
        ///// LOG4NETのロガー
        ///// </summary>
        //private static readonly log4net.ILog LOGGER = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType!);
        //----------
        /// <summary>
        /// LOG4NETのロガー
        /// </summary>
        private static readonly MicroSignLogger LOGGER = MicroSignLogger.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType!);
        //2026.06.07:CS)杉原:LOGGERを修正 <<<<< ここまで

        /// <summary>
        /// AnimationClipPage結果
        /// </summary>
        public class AnimationClipPageResult
        {
            /// <summary>
            /// 結果
            /// </summary>
            public readonly NavigationResultKind ResultKind;

            /// <summary>
            /// 出力パス一覧
            /// </summary>
            public readonly List<string>? OutputPaths;

            /// <summary>
            /// 表示期間
            /// </summary>
            /// <remarks>
            /// 出力したすべてのフレームで共通の値
            /// </remarks>
            public readonly double DisplayPeriod;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="resultKind">結果</param>
            /// <param name="outputPaths">出力パス一覧</param>
            /// <param name="displayPeriod">表示期間</param>
            private AnimationClipPageResult(NavigationResultKind resultKind, List<string>? outputPaths, double displayPeriod)
            {
                this.ResultKind = resultKind;
                this.OutputPaths = outputPaths;
                this.DisplayPeriod = displayPeriod;
            }

            /// <summary>
            /// キャンセルの場合
            /// </summary>
            /// <returns></returns>
            public static AnimationClipPageResult Cancel()
            {
                AnimationClipPageResult result = new AnimationClipPageResult(NavigationResultKind.Cancel, null, CommonConsts.Values.Zero.D);
                return result;
            }

            /// <summary>
            /// 成功の場合
            /// </summary>
            /// <param name="outputPaths">出力パス一覧</param>
            /// <param name="displayPeriod">表示期間</param>
            /// <returns></returns>
            public static AnimationClipPageResult Success(List<string>? outputPaths, double displayPeriod)
            {
                AnimationClipPageResult result = new AnimationClipPageResult(NavigationResultKind.Success, outputPaths, displayPeriod);
                return result;
            }

            /// <summary>
            /// 失敗の場合
            /// </summary>
            /// <returns></returns>
            public static AnimationClipPageResult Failed()
            {
                AnimationClipPageResult result = new AnimationClipPageResult(NavigationResultKind.Failed, null, AnimationClipPageViewModel.InitializeValues.DisplayPeriodMillisecond);
                return result;
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AnimationClipPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="matrixLedWidth">マトリクスLED横幅</param>
        /// <param name="matrixLedHeight">マトリクスLED縦幅</param>
        /// <param name="originalImage">オリジナル画像</param>
        /// <param name="originalImagePath">オリジナル画像のパス</param>
        public AnimationClipPage(int matrixLedWidth, int matrixLedHeight, BitmapSource? originalImage, string? originalImagePath)
            : this()
        {
            this.ViewModel.MatrixLedWidth = matrixLedWidth;
            this.ViewModel.MatrixLedHeight = matrixLedHeight;
            this.ViewModel.OriginalImage = originalImage;
            this.ViewModel.OriginalImagePath = originalImagePath;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="moveSpeed">移動速度</param>
        public AnimationClipPage(int matrixLedWidth, int matrixLedHeight, BitmapSource? originalImage, string? originalImagePath, int moveSpeed)
            : this(matrixLedWidth, matrixLedHeight, originalImage, originalImagePath)
        {
            this.ViewModel.MoveSpeed = moveSpeed;
        }

        /// <summary>
        /// OKクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOkClick(object sender, MicroSign.Core.Navigations.Events.OkClickEventArgs e)
        {
            //ViewModelを取得
            AnimationClipPageViewModel vm = this.ViewModel;

            //操作不可
            this.IsEnabled = false;

            //アニメーション切り抜き開始
            vm.ClipAnimation(this.OnClipAnimationFinish);

            //成功で終了
            e.Success();
        }

        /// <summary>
        /// アニメーション切り抜き完了
        /// </summary>
        /// <param name="taskResult"></param>
        private void OnClipAnimationFinish(ClipAnimationResult taskResult)
        {
            //ViewModelを取得
            AnimationClipPageViewModel vm = this.ViewModel;

            //操作可能
            this.IsEnabled = true;

            //画面を閉じる
            {
                //アニメーション切り抜き処理の成否を取得
                bool isSuccess = taskResult.IsSuccess;
                if (isSuccess)
                {
                    //成功の場合は続行
                }
                else
                {
                    //失敗の場合はエラーメッセージを表示して終了
                    this.NavigationOverwrap(new WarnMessageBox($"エラー '{taskResult.ErrorMessage}'"));
                    return;
                }
            }

            //出力結果を取得
            List<string>? outputPaths = taskResult.OutputPaths;

            //アニメーション設定値を取得
            int displayPeriodMs = vm.DisplayPeriodMillisecond;
            // >> ミリ秒を秒に変換
            double displayPeriod = displayPeriodMs / (double)CommonConsts.Intervals.OneSec;

            //成功で終了
            AnimationClipPageResult result = AnimationClipPageResult.Success(outputPaths, displayPeriod);
            this.NavigationReturn(result);
        }

        /// <summary>
        /// キャンセルクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCancelClick(object sender, MicroSign.Core.Navigations.Events.CancelClickEventArgs e)
        {
            //画面を閉じる
            AnimationClipPageResult result = AnimationClipPageResult.Cancel();
            this.NavigationReturn(result);
        }

        /// <summary>
        /// 画面が表示される時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Root_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //ViewModel初期化
                this.ViewModel.Initialize();
            }
            catch (Exception ex)
            {
                string msg = "Loadedで例外が発生しました";
                LOGGER.WarnEx(msg, ex);
                this.ViewModel.SetStatus(AnimationClipPageStateKind.Failed, msg);
            }
        }
    }
}
