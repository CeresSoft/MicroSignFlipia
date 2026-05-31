using MicroSign.Core.ViewModels.Pages;
using System;
using System.Windows.Controls;

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
            this.ViewModel.SetArgs(args);
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
    }
}
