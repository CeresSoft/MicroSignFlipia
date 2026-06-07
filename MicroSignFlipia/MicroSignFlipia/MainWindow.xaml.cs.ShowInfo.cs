using MicroSign.Core.Navigations;

namespace MicroSignFlipia
{
    partial class MainWindow
    {
        /// <summary>
        /// 情報表示
        /// </summary>
        /// <param name="message"></param>
        private void ShowInfo(string message)
        {
            this.MsgGrid.NavigationOverwrap(new MicroSign.Core.Views.Overlaps.InfoMessageBox(message, this.Title));
        }
    }
}
