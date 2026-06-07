using MicroSign.Core.Navigations;

namespace MicroSignFlipia
{
    partial class MainWindow
    {
        /// <summary>
        /// 警告表示
        /// </summary>
        /// <param name="message"></param>
        private void ShowWarning(string message)
        {
            this.MsgGrid.NavigationOverwrap(new MicroSign.Core.Views.Overlaps.WarnMessageBox(message, this.Title));
        }
    }
}
