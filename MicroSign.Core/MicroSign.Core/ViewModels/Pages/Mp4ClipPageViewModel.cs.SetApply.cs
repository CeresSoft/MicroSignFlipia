namespace MicroSign.Core.ViewModels.Pages
{
    partial class Mp4ClipPageViewModel
    {
        /// <summary>
        /// 確定設定
        /// </summary>
        public void SetAppry()
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

            //確定設定
            {
                double scale = this.SelectScale;

                //クリップの位置は表示側の都合で縮小化前(=videoWidth, videoHeight)の
                //座標で入っているので縮小後の座標に変換する
                double x = this.ClipFrameX;
                double y = this.ClipFrameY;
                double nx = x * scale;
                double ny = y * scale;
                int nxi = (int)nx;
                int nyi = (int)ny;

                //設定する
                args.SetApply(scale, nxi, nyi);
            }
        }
    }
}
