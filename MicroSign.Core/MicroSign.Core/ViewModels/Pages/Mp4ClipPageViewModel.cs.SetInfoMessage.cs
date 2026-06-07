namespace MicroSign.Core.ViewModels.Pages
{
    partial class Mp4ClipPageViewModel
    {
        /// <summary>
        /// 情報メッセージ設定
        /// </summary>
        /// <param name="msg"></param>
        public string SetInfoMessage(string msg)
        {
            this.IsError = true;
            this.Message = msg;
            return msg;
        }
    }
}
