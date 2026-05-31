using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSign.Core.ViewModels.Pages
{
    public class Mp4ClipPageViewModel
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
            double selectScale = this.SelectScale;
            if(CommonConsts.Values.Zero.D < selectScale)
            {
                //選択スケールが有効の場合
            }
            else
            {
                //選択スケールが無効の場合は1倍にする
                selectScale = Mp4ClipPageViewModel.InitializeValues.SelectScale
            }

            2026.05.31:時間切れで一旦保留




        }
    }
}
