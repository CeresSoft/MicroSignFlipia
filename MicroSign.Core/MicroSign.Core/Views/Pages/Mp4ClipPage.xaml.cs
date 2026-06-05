using MicroSign.Core.Navigations;
using MicroSign.Core.ViewModels.Pages;
using System;
using System.Windows.Controls;
using System.Windows.Media.TextFormatting;

namespace MicroSign.Core.Views.Pages
{
    /// <summary>
    /// Mp4ClipPage.xaml の相互作用ロジック
    /// </summary>
    public partial class Mp4ClipPage : UserControl
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Mp4ClipPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="args"></param>
        public Mp4ClipPage(MicroSign.Core.ViewModels.Mp4ClipRequestEventArgs args)
            :this()
        {
            this.ViewModel.SetMp4ClipRequest(args);
        }


        /// <summary>
        /// 画面表示時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                //ViewModel初期化
                this.ViewModel.Initialize();
            }
            catch (Exception ex)
            {
                string msg = "Loadedで例外が発生しました";
                CommonLogger.Warn(msg, ex);
            }
        }

        /// <summary>
        /// ドラッグ中フラグ
        /// </summary>
        private bool _IsDragging = false;

        /// <summary>
        /// ドラッグ開始位置
        /// </summary>
        private System.Windows.Point _DragStartPoint = new System.Windows.Point();

        /// <summary>
        /// ドラッグ開始時のクリップフレームの位置
        /// </summary>
        private System.Windows.Point _ClipFrameStartPoint = new System.Windows.Point();
        
        /// <summary>
        /// ドラッグ開始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClipFrame_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //ドラッグ開始判定
            if(this._IsDragging)
            {
                //ドラッグ中なら無視する
                return;
            }
            else
            {
                //ドラッグしていない場合は処理続行
            }

            //キャプチャー開始
            this._IsDragging = true;
            this._DragStartPoint = e.GetPosition(this.Video);
            this.ClipFrame.CaptureMouse();
            {
                this._ClipFrameStartPoint.X = this.ViewModel.ClipFrameX;
                this._ClipFrameStartPoint.Y = this.ViewModel.ClipFrameY;
            }
        }

        /// <summary>
        /// 移動
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClipFrame_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //ドラッグ開始判定
            if (this._IsDragging)
            {
                //ドラッグ中なら処理続行
            }
            else
            {
                //ドラッグしていない場合は何もしない
                return;
            }

            //マウス位置を取得
            System.Windows.Point currentPoint = e.GetPosition(this.Video);

            //移動量を計算
            double dx = currentPoint.X - this._DragStartPoint.X;
            double dy = currentPoint.Y - this._DragStartPoint.Y;

            //反映
            {
                double x = this._ClipFrameStartPoint.X + dx;
                double y = this._ClipFrameStartPoint.Y + dy;
                this.ViewModel.UpdateClipFramePoint(x, y);
            }
        }

        /// <summary>
        /// マウスアップ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClipFrame_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //ドラッグ開始判定
            if (this._IsDragging)
            {
                //ドラッグ中なら処理続行
            }
            else
            {
                //ドラッグしていない場合は何もしない
                return;
            }

            //マウス位置を取得
            System.Windows.Point currentPoint = e.GetPosition(this.Video);

            //移動量を計算
            double dx = currentPoint.X - this._DragStartPoint.X;
            double dy = currentPoint.Y - this._DragStartPoint.Y;

            //反映
            {
                double x = this._ClipFrameStartPoint.X + dx;
                double y = this._ClipFrameStartPoint.Y + dy;
                this.ViewModel.UpdateClipFramePoint(x, y);
            }

            //マウスキャプチャー解除
            this._IsDragging = false;
            this.ClipFrame.ReleaseMouseCapture();
        }

        /// <summary>
        /// OKボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //確定の場合
            this.ViewModel.SetAppry();

            //終了
            this.NavigationReturn();
        }

        /// <summary>
        /// キャンセルボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //キャンセルを設定
            this.ViewModel.SetCancel();

            //終了
            this.NavigationReturn();
        }
    }
}
