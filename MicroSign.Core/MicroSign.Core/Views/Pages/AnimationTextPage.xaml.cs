using MicroSign.Core.Navigations;
using MicroSign.Core.Navigations.Enums;
using MicroSign.Core.ViewModels.Pages;
using MicroSign.Core.Views.Overlaps;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MicroSign.Core.Views.Pages
{
    /// <summary>
    /// AnimationTextPage.xaml の相互作用ロジック
    /// </summary>
    public partial class AnimationTextPage : UserControl
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
        /// AnimationTextPage結果
        /// </summary>
        public class AnimationTextPageResult
        {
            /// <summary>
            /// 結果
            /// </summary>
            public readonly NavigationResultKind ResultKind;

            //2025.10.02:CS)土田:文字画像追加の流れを変更 >>>>> ここから
            ///// <summary>
            ///// 選択フォントサイズ
            ///// </summary>
            //public readonly int SelectFontSize;

            ///// <summary>
            ///// 選択フォント色
            ///// </summary>
            //public readonly int SelectFontColor;

            ///// <summary>
            ///// 表示文章
            ///// </summary>
            //public readonly string? DisplayText;

            ///// <summary>
            ///// レンダリング結果
            ///// </summary>
            //public readonly RenderTargetBitmap? RenderBitmap;

            ///// <summary>
            ///// コンストラクタ
            ///// </summary>
            ///// <param name="resultKind">結果</param>
            ///// <param name="selectFontSize">選択フォントサイズ</param>
            ///// <param name="selectFontColor">選択フォント色</param>
            ///// <param name="displayText">表示テキスト</param>
            ///// <param name="renderBitmap">レンダリング結果</param>
            //private AnimationTextPageResult(NavigationResultKind resultKind, int selectFontSize, int selectFontColor, string? displayText, RenderTargetBitmap? renderBitmap)
            //{
            //    this.ResultKind = resultKind;
            //    this.SelectFontSize = selectFontSize;
            //    this.SelectFontColor = selectFontColor;
            //    this.DisplayText = displayText;
            //    this.RenderBitmap = renderBitmap;
            //}

            ///// <summary>
            ///// キャンセルの場合
            ///// </summary>
            ///// <returns></returns>
            //public static AnimationTextPageResult Cancel()
            //{
            //    AnimationTextPageResult result = new AnimationTextPageResult(NavigationResultKind.Cancel, CommonConsts.Values.Zero.I, CommonConsts.Values.Zero.I, null, null);
            //    return result;
            //}

            ///// <summary>
            ///// 成功の場合
            ///// </summary>
            ///// <param name="selectFontSize">選択フォントサイズ</param>
            ///// <param name="selectFontColor">選択フォント色</param>
            ///// <param name="displayText">表示テキスト</param>
            ///// <param name="renderBitmap">レンダリング結果</param>
            ///// <returns></returns>
            //public static AnimationTextPageResult Success(int selectFontSize, int selectFontColor, string? displayText, RenderTargetBitmap? renderBitmap)
            //{
            //    AnimationTextPageResult result = new AnimationTextPageResult(NavigationResultKind.Success, selectFontSize, selectFontColor, displayText, renderBitmap);
            //    return result;
            //}
            //----------
            /// <summary>
            /// 出力パス一覧
            /// </summary>
            public readonly List<string>? OutputPaths = null;

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
            private AnimationTextPageResult(NavigationResultKind resultKind, List<string>? outputPaths, double displayPeriod)
            {
                this.ResultKind = resultKind;
                this.OutputPaths = outputPaths;
                this.DisplayPeriod = displayPeriod;
            }

            /// <summary>
            /// キャンセルの場合
            /// </summary>
            /// <returns></returns>
            public static AnimationTextPageResult Cancel()
            {
                AnimationTextPageResult result = new AnimationTextPageResult(NavigationResultKind.Cancel, null, CommonConsts.Values.Zero.D);
                return result;
            }

            /// <summary>
            /// 成功の場合
            /// </summary>
            /// <param name="outputPaths">出力パス一覧</param>
            /// <param name="displayPeriod">表示期間</param>
            /// <returns></returns>
            public static AnimationTextPageResult Success(List<string>? outputPaths, double displayPeriod)
            {
                AnimationTextPageResult result = new AnimationTextPageResult(NavigationResultKind.Success, outputPaths, displayPeriod);
                return result;
            }
            //2025.10.02:CS)土田:文字画像追加の流れを変更 <<<<< ここまで
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AnimationTextPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="matrixLedWidth">マトリクスLED横幅</param>
        /// <param name="matrixLedHeight">マトリクスLED縦幅</param>
        /// <param name="animationName">アニメーション名</param>
        public AnimationTextPage(int matrixLedWidth, int matrixLedHeight, string? animationName)
            :this()
        {
            this.ViewModel.MatrixLedWidth = matrixLedWidth;
            this.ViewModel.MatrixLedHeight = matrixLedHeight;
            this.ViewModel.AnimationName = animationName;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="matrixLedWidth">マトリクスLED横幅</param>
        /// <param name="matrixLedHeight">マトリクスLED縦幅</param>
        /// <param name="animationName">アニメーション名</param>
        /// <param name="selectFontSize">選択フォントサイズ</param>
        /// <param name="selectFontColor">選択フォント色</param>
        /// <param name="displayText">表示文章</param>
        public AnimationTextPage(int matrixLedWidth, int matrixLedHeight, string? animationName, int selectFontSize, int selectFontColor, string? displayText)
            : this(matrixLedWidth, matrixLedHeight, animationName)
        {
            this.ViewModel.SelectFontSize = selectFontSize;
            this.ViewModel.SelectFontColor = selectFontColor;
            this.ViewModel.DisplayText = displayText;
        }


        /// <summary>
        /// OKクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOkClick(object sender, MicroSign.Core.Navigations.Events.OkClickEventArgs e)
        {
            //ViewModelを取得
            AnimationTextPageViewModel vm = this.ViewModel;

            //エラーチェック
            {
                AnimationTextPageStateKind status = vm.StatusKind;
                switch (status)
                {
                    case AnimationTextPageStateKind.Initialized:
                        //初期化済みなら何等かの画面が表示されているはずなので
                        //常に成功として処理続行する
                        break;

                    //2025.09.30:CS)土田:ビットマップは更新時に毎回生成するように変更 >>>>> ここから
                    //----------
                    case AnimationTextPageStateKind.Ready:
                        //準備完了の場合はテキスト描写に成功しているので処理続行する
                        break;
                    //2025.09.30:CS)土田:ビットマップは更新時に毎回生成するように変更 <<<<< ここまで

                    default:
                        //それ以外は何らかのエラーなのでエラーメッセージを表示して終了
                        this.NavigationOverwrap(new WarnMessageBox($"エラー '{vm.StatusText}'"));
                        return;
                }
            }

            //2025.09.30:CS)土田:画像をファイルに書き出すように変更 >>>>> ここから
            ////画面を閉じる
            //{
            //    int fontSize = vm.SelectFontSize;
            //    int fontColor = vm.SelectFontColor;
            //    string? displayText = vm.DisplayText;
            //    RenderTargetBitmap? renderBitmap = vm.RenderBitmap;
            //    AnimationTextPageResult result = AnimationTextPageResult.Success(fontSize, fontColor, displayText, renderBitmap);
            //    this.NavigationReturn(result);
            //}
            //----------
            //保存先取得
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.Title = "テキスト画像を保存します";
            dialog.FileName = ""; // Default file name
            dialog.DefaultExt = ".png"; // Default file extension
            dialog.Filter = "PNG画像(*.png)|*.png|すべてのファイル (*.*)|*.*"; // Filter files by extension               

            //保存ダイアログ表示
            {
                bool ret = dialog.ShowDialog() ?? false;
                if (ret)
                {
                    //選択した場合は処理続行
                }
                else
                {
                    //選択しなかった場合は終了
                    return;
                }
            }

            //保存パスを取得
            string savePath = dialog.FileName;

            //保存
            AnimationTextPageViewModel.SaveImageResult saveResult = vm.SaveImage(savePath);
            if (saveResult.IsSuccess)
            {
                //成功の場合は続行
            }
            else
            {
                //失敗の場合はエラーメッセージを表示して終了
                this.NavigationOverwrap(new WarnMessageBox($"エラー '{saveResult.ErrorMessage}'"));
                return;
            }

            //文字スクロール有効フラグを取得
            bool isScrollEnabled = vm.IsScrollEnabled;

            //デフォルト表示期間を取得
            double displayPeriod = vm.DefaultDisplayPeriod;

            //出力画像の一覧と表示期間を取得
            List<string>? outputPaths = null;
            // >> スクロール有無で出力枚数を変更する
            if (isScrollEnabled)
            {
                //スクロール有効の場合、出力画像でアニメーション切り抜きを行う
                RenderTargetBitmap? image = saveResult.RenderBitmap;

                //マトリクスLED設定を取得
                int matrixLedWidth = vm.MatrixLedWidth;
                int matrixLedHeight = vm.MatrixLedHeight;

                //アニメーション切り抜きページを表示
                LOGGER.Debug("アニメーション切り抜きページを表示");
                MicroSign.Core.Views.Pages.AnimationClipPage page = new MicroSign.Core.Views.Pages.AnimationClipPage(matrixLedWidth, matrixLedHeight, image, savePath, Consts.DefaultMoveSpeed);
                // >> 画面からの戻りを待つ
                object result = this.NavigationCallWait(page, null);
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

                //結果を取得
                if (result is AnimationClipPage.AnimationClipPageResult pageResult)
                {
                    //結果種類を取得
                    NavigationResultKind resultKind = pageResult.ResultKind;
                    switch (resultKind)
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
                            LOGGER.Warn("アニメーション切り抜き失敗 (理由={resultKind}')");
                            this.NavigationOverwrap(new WarnMessageBox($"アニメーション切り抜きに失敗しました (理由={resultKind}')"));
                            return;
                    }

                    //出力パス一覧を取得
                    outputPaths = pageResult.OutputPaths;

                    //表示期間を取得
                    displayPeriod = pageResult.DisplayPeriod;
                }
                else
                {
                    //結果を取得できなかった場合はエラーメッセージを表示して終了
                    this.NavigationOverwrap(new WarnMessageBox($"アニメーション切り抜き結果が確認出来ません"));
                    return;
                }
            }
            else
            {
                //スクロール無効の場合、出力枚数=1とする

                //出力パス一覧を取得
                // >> 保存パスをそのまま使用する
                outputPaths = new List<string> { savePath };

                //表示期間を取得
                // >> デフォルト値をそのまま使用する
            }

            //画面を閉じる
            {
                AnimationTextPageResult result = AnimationTextPageResult.Success(outputPaths, displayPeriod);
                this.NavigationReturn(result);
            }
            //2025.09.30:CS)土田:画像をファイルに書き出すように変更 <<<<< ここまで

            //成功で終了
            e.Success();
        }

        /// <summary>
        /// キャンセルクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCancelClick(object sender, MicroSign.Core.Navigations.Events.CancelClickEventArgs e)
        {
            //画面を閉じる
            AnimationTextPageResult result = AnimationTextPageResult.Cancel();
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
                this.ViewModel.SetStatus(AnimationTextPageStateKind.Failed, msg);
            }
        }
    }
}
