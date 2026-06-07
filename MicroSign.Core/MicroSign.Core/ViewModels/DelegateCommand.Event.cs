using System;

namespace MicroSign.Core.ViewModels
{
    /// <summary>
    /// デリゲートコマンド - イベント
    /// </summary>
    partial class DelegateCommand
    {
        /// <summary>
        /// CanExecuteChangedイベント
        /// </summary>
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// CanExecuteChangedイベント発行
        /// </summary>
        /// <remarks>CanExecuteが変化する場合に呼び出してください</remarks>
        protected void RaiseCanExecuteChanged()
        {
            EventHandler? handler = this.CanExecuteChanged;
            if (handler == null)
            {
                return;
            }
            try
            {
                handler(this, new EventArgs());
            }
            catch (Exception ex)
            {
                LOGGER.WarnEx("デリゲートコマンドCanExecuteChangedイベント発行で例外発生", ex);
            }
        }
    }
}
