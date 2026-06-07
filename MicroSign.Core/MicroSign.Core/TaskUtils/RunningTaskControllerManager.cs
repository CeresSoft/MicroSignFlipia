using System.Threading;
using System.Threading.Tasks;

namespace MicroSign.Core.TaskUtils
{
    /// <summary>
    /// 実行中タスク制御管理
    /// </summary>
    /// <remarks>複数の実行中タスク制御を管理します</remarks>
    public partial class RunningTaskControllerManager : MicroSign.Core.Disposables.ManageDisposable
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
        /// 指定したタスクを実行する
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public Task? Run(RunningTaskController task)
        {
            if(task == null)
            {
                //実行するタスクが無いので終了
                return null;
            }
            else
            {
                //実行するタスク有効の場合処理続行
            }

            //キャンセル取得
            CancellationTokenSource cancel = this._CancellationTokenSource;

            //タスク実行開始
            Task? t = task.Run(cancel);
            if(t == null)
            {
                //実行できなかった場合は終了
                return null;
            }
            else
            {
                //実行できた場合は処理続行
            }

            //タスクをリストに追加
            this.AddRunningTask(task);

            //継続処理として実行中タスクからリストを削除して破棄する
            SynchronizationContext? context = SynchronizationContext.Current;
            Task result = t.ContinueWith((r) => { this.RemoveRunningTask(task); task.Dispose(); });

            //終了
            return result;
        }

        /// <summary>
        /// 実行中タスクを追加
        /// </summary>
        /// <param name="task"></param>
        protected virtual void AddRunningTask(RunningTaskController task)
        {
            lock (this._RunningTaskCollectionLockObject)
            {
                LOGGER.Info($"[RunnningTask:ADD]Task={task}, ID={task.RunningTask?.Id}");
                this._RunningTaskCollection.Add(task);
            }
        }

        /// <summary>
        /// 実行中タスクを削除
        /// </summary>
        /// <param name="task"></param>
        protected virtual void RemoveRunningTask(RunningTaskController task)
        {
            lock (this._RunningTaskCollectionLockObject)
            {
                LOGGER.Info($"[RunnningTask:DEL]Task={task}, ID={task.RunningTask?.Id}");
                this._RunningTaskCollection.Remove(task);
            }
        }
    }
}
