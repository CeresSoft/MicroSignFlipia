namespace MicroSign.Core.Views.Pages
{
    partial class Mp4ClipPage
    {
        /// <summary>
        /// MP4クリップ要求設定
        /// </summary>
        /// <param name="e">MP4クリップ要求イベント引数</param>
        public void SetMp4ClipRequest(MicroSign.Core.ViewModels.Mp4ClipRequestEventArgs args)
        {
            //Viewにリレーする
            this.ViewModel.SetMp4ClipRequest(args);
        }
    }
}
