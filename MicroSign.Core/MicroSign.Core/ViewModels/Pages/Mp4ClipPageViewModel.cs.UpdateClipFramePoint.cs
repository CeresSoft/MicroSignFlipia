namespace MicroSign.Core.ViewModels.Pages
{
    partial class Mp4ClipPageViewModel
    {
        /// <summary>
        /// クリップフレームの位置を更新
        /// </summary>
        /// <param name="x">新しい位置X</param>
        /// <param name="y">新しい位置Y</param>
        public void UpdateClipFramePoint(double x, double y)
        {
            //新しい座標を計算
            {
                double minX = this.MinClipX;
                double maxX = this.MaxClipX;
                double normalX = CommonUtils.Clamp(minX, maxX, x);
                this.ClipFrameX = normalX;
            }

            {
                double minY = this.MinClipY;
                double maxY = this.MaxClipY;
                double normalY = CommonUtils.Clamp(minY, maxY, y);
                this.ClipFrameY = normalY;
            }
        }

    }
}
