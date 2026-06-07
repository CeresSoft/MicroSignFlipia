namespace MicroSign.Core.ViewModels.Pages
{
    partial class Mp4ClipPageViewModel
    {
        /// <summary>
        /// キャンセル設定
        /// </summary>
        public void SetCancel()
        {
            //MP4クリップ要求引数取得
            MicroSign.Core.ViewModels.Mp4ClipRequestEventArgs? args = this._Args;
            if(args == null)
            {
                //無効の場合は何もしない
                return;
            }
            else
            {
                //有効の場合は処理続行
            }

            //キャンセル設定
            args.SetCancel();
        }
    }
}
