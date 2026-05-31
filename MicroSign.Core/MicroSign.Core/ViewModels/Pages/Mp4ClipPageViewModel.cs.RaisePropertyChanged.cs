using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MicroSign.Core.ViewModels.Pages
{
    partial class Mp4ClipPageViewModel
    {
        /// <summary>
        /// PropertyChangedイベント発生
        /// </summary>
        /// <param name="propertyName"></param>
        protected override void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.RaisePropertyChanged(propertyName);

            //追加の処理
            switch (propertyName)
            {
                case Mp4ClipPageViewModel.PropertyNames.PanelWidth:
                case Mp4ClipPageViewModel.PropertyNames.PanelHeight:
                case Mp4ClipPageViewModel.PropertyNames.VideoWidth:
                case Mp4ClipPageViewModel.PropertyNames.VideoHeight:
                case Mp4ClipPageViewModel.PropertyNames.SelectScale:
                case Mp4ClipPageViewModel.PropertyNames.SelectClipX:
                case Mp4ClipPageViewModel.PropertyNames.SelectClipY:
                    //枠表示を更新
                    this.UpdateClipFrame();
                    break;

                case Mp4ClipPageViewModel.PropertyNames.MaxDurationTicks:
                case Mp4ClipPageViewModel.PropertyNames.SelectVideoPosition:
                    //表示画像を更新
                    this.UpdateVideoImage();
                    break;

                case Mp4ClipPageViewModel.PropertyNames.IsReady:
                    //準備完了になった場合
                    {
                        //枠表示を更新
                        this.UpdateClipFrame();

                        //表示画像を更新
                        this.UpdateVideoImage();
                    }
                    break;

                default:
                    //それ以外は何もしない
                    break;
            }
        }
    }
}
