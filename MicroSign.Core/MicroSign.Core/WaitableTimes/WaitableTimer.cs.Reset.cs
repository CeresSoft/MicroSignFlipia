using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;
using LOGGER = MicroSign.Core.MicroSignLogger;

namespace MicroSign.Core.WaitableTimes
{
    partial class WaitableTimer
    {
        /// <summary>
        /// シグナルリセット
        /// </summary>
        public void Reset()
        {
            this.Reset(true);
        }

        /// <summary>
        /// シグナルリセット
        /// </summary>
        /// <param name="isContinue">True=前回のシグナル時間からの継続した待ち / False=Reset呼び出ししたタイミングからの待ち</param>
        public void Reset(bool isContinue)
        {
            //シグナルになる間隔を取得
            long signalInterval = this.SignalInterval;
            this.Reset(true, signalInterval);
        }

        /// <summary>
        /// シグナルリセット
        /// </summary>
        /// <param name="signalInterval">待ち時間</param>
        public void Reset(long signalInterval)
        {
            this.Reset(true, signalInterval);
        }

        /// <summary>
        /// シグナルリセット
        /// </summary>
        /// <param name="isContinue">True=前回のシグナル時間からの継続した待ち / False=Reset呼び出ししたタイミングからの待ち</param>
        /// <param name="signalInterval">待ち時間</param>
        public void Reset(bool isContinue, long signalInterval)
        {
            try
            {
                //実行中判定
                {
                    bool isRunning = this.IsRunning;
                    if (isRunning)
                    {
                        //実行中の場合は処理続行
                    }
                    else
                    {
                        //未実行の場合は処理できないので終了
                        return;
                    }
                }

                //ハンドル取得
                SafeWaitHandle handle = this.SafeWaitHandle;

                //シグナルになる時間を取得
                long nowTime = DateTime.Now.ToFileTimeUtc();
                long signalTime = this.SignalTime;
                if (isContinue)
                {
                    //継続の場合は何もしない
                }
                else
                {
                    //継続以外の場合は現在時刻に書き換える
                    signalTime = nowTime;
                }

                //次のシグナル時間を設定
                long dueTime = signalTime + signalInterval;
                while (dueTime < nowTime)
                {
                    //現在時間よりも次のシグナル時間が小さい場合は
                    //現在時間までスキップする
                    dueTime += signalInterval;
                }

                //リセットする
                bool ret = WaitableTimer.Win32Api.SetWaitableTimer(handle, ref dueTime, 0, IntPtr.Zero, IntPtr.Zero, false);
                if (ret)
                {
                    //成功の場合シグナル時間を変更
                    this.SignalTime = dueTime;
                }
                else
                {
                    //エラーの場合
                    LOGGER.Warn($"RESET ERROR - {Marshal.GetLastWin32Error()}");
                }
            }
            catch (Exception)
            {
                //例外は握りつぶす
            }
        }

    }
}
