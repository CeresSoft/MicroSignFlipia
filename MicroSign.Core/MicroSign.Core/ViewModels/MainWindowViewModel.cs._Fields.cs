using System.Threading;
using System.Threading.Tasks;

namespace MicroSign.Core.ViewModels
{
    partial class MainWindowViewModel
    {
        /// <summary>
        /// アニメーションタスクs
        /// </summary>
        private Task? _AnimationTask = null;

        /// <summary>
        /// アニメーションキャンセル
        /// </summary>
        private CancellationTokenSource? _AnimationCancel = null;
    }
}
