using System.Runtime.CompilerServices;

namespace MicroSign.Core.ViewModels
{
    public partial class MainWindowViewModel : NotifyPropertyChangedObject
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
