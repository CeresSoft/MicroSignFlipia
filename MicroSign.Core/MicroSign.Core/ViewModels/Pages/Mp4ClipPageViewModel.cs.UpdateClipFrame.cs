using System;

namespace MicroSign.Core.ViewModels.Pages
{
    partial class Mp4ClipPageViewModel
    {
        /// <summary>
        /// クリップフレームを更新
        /// </summary>
        private void UpdateClipFrame()
        {
            //準備完了判定
            {
                bool isReady = this.IsReady;
                if(isReady)
                {
                    //準備完了していない場合は何もしない
                    return;
                }
                else
                {
                    //準備完了なら処理続行
                }
            }

            //ビデオサイズ取得
            double videoWidth = this.VideoWidth;
            double videoHeight = this.VideoHeight;

            //パネルサイズ取得
            double panelWidth = this.PanelWidth;
            double panelHeight = this.PanelHeight;

            //スケールを取得
            double minScale = this.MinScale;
            double maxScale = this.MaxScale;
            double selectScale = this.SelectScale;
            double scale = CommonUtils.Clamp(minScale, maxScale, selectScale);

            //クリップするサイズを計算
            double clipWidth = panelWidth / scale;
            double clipHeight = panelHeight / scale;

            //移動範囲
            double minX = this.MinClipX;
            double maxX = videoWidth - clipWidth;
            double minY = this.MinClipY;
            double maxY = videoHeight - clipHeight;
            this.MaxClipX = maxX;
            this.MaxClipY = maxY;

            //現在の座標を範囲内にする
            double selectX = this.ClipFrameX;
            double x = CommonUtils.Clamp(minX, maxX, selectX);

            double selectY = this.ClipFrameY;
            double y = CommonUtils.Clamp(minY, maxY, selectY);

            //クリップ枠を計算
            {
                this.ClipFrameX = x;
                this.ClipFrameY = y;
                this.ClipFrameWidth = clipWidth;
                this.ClipFrameHeight = clipHeight;
            }
        }
    }
}
