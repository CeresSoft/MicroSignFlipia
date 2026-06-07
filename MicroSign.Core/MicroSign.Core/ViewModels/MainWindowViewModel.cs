using System.Runtime.CompilerServices;

namespace MicroSign.Core.ViewModels
{
    public partial class MainWindowViewModel : NotifyPropertyChangedObject
    {

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
