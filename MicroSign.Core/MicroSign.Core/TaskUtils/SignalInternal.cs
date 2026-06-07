using System;
using System.Threading;

namespace MicroSign.Core.TaskUtils
{
    /// <summary>
    /// シグナル内部クラス
    /// </summary>
    public partial class SignalInternal
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
        /// コンストラクタ
        /// </summary>
        /// <param name="initialSignal">初期シグナル状態(通常はfalse)</param>
        public SignalInternal(bool initialSignal)
        {
            this._Signal = new ManualResetEvent(initialSignal);
        }

        /// <summary>
        /// WaitHandle
        /// </summary>
        public WaitHandle WaitHandle
        {
            get
            {
                return this._Signal;
            }
        }

        /// <summary>
        /// 参照カウンタを増やす
        /// </summary>
        public void AddRef()
        {
            Interlocked.Increment(ref this._ReferenceCount);
        }

        /// <summary>
        /// 参照カウンタを減らす
        /// </summary>
        public void Release()
        {
            int n = Interlocked.Decrement(ref this._ReferenceCount);
            if(n == CommonConsts.Count.Zero)
            {
                //参照カウンタが0になったらシグナルを破棄する
                this._Signal.Dispose();
            }
        }

        /// <summary>
        /// シグナル設定
        /// </summary>
        public void Set()
        {
            try
            {
                this._Signal.Set();
            }
            catch (Exception ex)
            {
                LOGGER.WarnEx("シグナル設定で例外発生", ex);
            }
        }

        /// <summary>
        /// シグナル解除
        /// </summary>
        public void Reset()
        {
            try
            {
                this._Signal.Reset();
            }
            catch (Exception ex)
            {
                LOGGER.WarnEx("シグナル解除で例外発生", ex);
            }
        }

        /// <summary>
        /// シグナル待ち
        /// </summary>
        /// <returns>true=シグナル/false=タイムアウト</returns>
        public bool WaitOne()
        {
            try
            {
                bool ret = this._Signal.WaitOne();
                return ret;
            }
            catch (Exception ex)
            {
                //例外が発生した場合はシグナルで終了する
                LOGGER.WarnEx("シグナル待ちで例外発生", ex);
                return true;
            }
        }

        /// <summary>
        /// シグナル待ち
        /// </summary>
        /// <param name="timeout">タイムアウト値</param>
        /// <returns>true=シグナル/false=タイムアウト</returns>
        public bool WaitOne(int timeout)
        {
            try
            {
                bool ret = this._Signal.WaitOne(timeout);
                return ret;
            }
            catch (Exception ex)
            {
                //例外が発生した場合はシグナルで終了する
                LOGGER.WarnEx($"シグナル待ちで例外発生 (timeout={timeout})", ex);
                return true;
            }
        }
    }
}
