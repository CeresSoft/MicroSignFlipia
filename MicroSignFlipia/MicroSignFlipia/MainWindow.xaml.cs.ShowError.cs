using MicroSign.Core.Navigations;
using System;

namespace MicroSignFlipia
{
    partial class MainWindow
    {
        /// <summary>
        /// エラー表示
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="error">例外</param>
        private void ShowError(string message, Exception error)
        {
            //例外はnull許容型ではないのでnullを渡すとコンパイル時に警告になるが
            //コンパイルエラーではないので無視すれば渡せる
            //よってnullチェックは行う
            if (error == null)
            {
                //例外が無効の場合は直接呼出し
                this.ShowError(message);
            }
            else
            {
                //例外が有効の場合はメッセージの後ろに連結
                string msg = $"{message}\n{error}";
                this.ShowError(msg);
            }
        }

        /// <summary>
        /// エラー表示
        /// </summary>
        /// <param name="message"></param>
        private void ShowError(string message)
        {
            this.MsgGrid.NavigationOverwrap(new MicroSign.Core.Views.Overlaps.WarnMessageBox(message, this.Title));
        }
    }
}
