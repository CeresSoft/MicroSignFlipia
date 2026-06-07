using System;
using System.Threading;

namespace MicroSign.Core.WaitableTimes
{
    /// <summary>
    /// 待ちができるタイマー
    /// </summary>
    /// <remarks>
    /// [参考] Analog to waitable timers in .NET
    /// https://stackoverflow.com/questions/15858751/analog-to-waitable-timers-in-net
    /// と
    /// [参考]GoaLitiuM/Usleep.cs
    /// https://gist.github.com/GoaLitiuM/5413bfa971d1910ed753
    /// [2025.01.03:CS)杉原]ONVIF.dllから移植
    /// </remarks>
    public partial class WaitableTimer : WaitHandle
    {
        //2026.06.07:CS)杉原:LOGGERを修正 >>>>> ここから
        ///// <summary>
        ///// LOG4NETのロガー
        ///// </summary>
        //private static readonly log4net.ILog LOGGER = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //----------
        /// <summary>
        /// LOG4NETのロガー
        /// </summary>
        private static readonly MicroSignLogger LOGGER = MicroSignLogger.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType!);
        //2026.06.07:CS)杉原:LOGGERを修正 <<<<< ここまで

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="manualReset"></param>
        /// <param name="timerName"></param>
        public WaitableTimer(bool manualReset = true, string? timerName = null)
        {
            this.IsManualReset = manualReset;
            this.TimerName = timerName;
        }

        /// <summary>
        /// 破棄
        /// </summary>
        /// <param name="explicitDisposing"></param>
        protected override void Dispose(bool explicitDisposing)
        {
            //ベースの処理を呼び出す
            base.Dispose(explicitDisposing);

            //ハンドルを破棄
            IDisposable obj = this.SafeWaitHandle;
            if (obj == null)
            {
                //無効の場合は何もしない
            }
            else
            {
                //有効の場合はDisposeする
                try
                {
                    obj.Dispose();
                }
                catch (Exception)
                {
                    //例外は握りつぶす
                }
            }
        }
    }
}
