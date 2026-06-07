namespace MicroSign.Core.Navigations.ViewModels
{
    /// <summary>
    /// OK/キャンセルナビゲーションViewModel
    /// </summary>
    public partial class OkCancelNavigationViewModelBase : NotifyPropertyChangedObject
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
        /// コンストラクタ
        /// </summary>
        public OkCancelNavigationViewModelBase()
        {
            //OKコマンド
            this.OkCommand = new MicroSign.Core.ViewModels.DelegateCommand(this.OnOk);

            //キャンセルコマンド
            this.CancelCommand = new MicroSign.Core.ViewModels.DelegateCommand(this.OnCancel);
        }

        /// <summary>
        /// OKクリック
        /// </summary>
        /// <param name="parameter"></param>
        private void OnOk(object? parameter)
        {
            //OKを発行
            this.RaiseOkClick();
        }

        /// <summary>
        /// キャンセルクリック
        /// </summary>
        /// <param name="parameter"></param>
        private void OnCancel(object? parameter)
        {
            this.RaiseCancelClick();
        }
    }
}
