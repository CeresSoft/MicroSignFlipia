using MicroSign.Core.Navigations.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSign.Core.ViewModels.Pages
{
    /// <summary>
    /// MP4クリップ要求ページViewModel
    /// </summary>
    public partial class Mp4ClipPageViewModel : OkCancelNavigationViewModelBase
    {
        /// <summary>
        /// モデル取得
        /// </summary>
        public Models.Model Model
        {
            get
            {
                return Models.Model.Instance;
            }
        }
    }
}
