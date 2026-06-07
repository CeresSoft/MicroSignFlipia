using MicroSign.Core.Navigations.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MicroSign.Core.ViewModels.Pages
{
    /// <summary>
    /// アニメーションテキストページViewModel
    /// </summary>
    public partial class AnimationTextPageViewModel : OkCancelNavigationViewModelBase
    {
        //2026.06.07:CS)杉原:LOGGERを修正 >>>>> ここから
        ///// <summary>
        ///// LOG4NETのロガー
        ///// </summary>
        //private static readonly log4net.ILog LOGGER = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType!);
        //----------
        /// <summary>
        /// LOG4NETのロガー
        /// </summary>
        private static readonly MicroSignLogger LOGGER = MicroSignLogger.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType!);
        //2026.06.07:CS)杉原:LOGGERを修正 <<<<< ここまで

        /// <summary>
        /// モデル
        /// </summary>
        /// <remarks>
        /// 2023.11.23:CS)土田:Core.dllに分離するにあたり、ViewModelからModelへの参照を追加
        ///  >> モデルインスタンスの実装を変えるかもしれないので、プロパティを経由する形にしています
        /// </remarks>
        public Models.Model Model
        {
            get
            {
                return Models.Model.Instance;
            }
        }
    }
}
