using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSign.Core.ViewModels.Pages
{
    partial class Mp4ClipPageViewModel
    {
        /// <summary>
        /// MP4クリップ要求引数設定
        /// </summary>
        /// <param name="args"></param>
        public void SetArgs(MicroSign.Core.ViewModels.Mp4ClipRequestEventArgs args)
        {
            this._Args = args;
        }
    }
}
