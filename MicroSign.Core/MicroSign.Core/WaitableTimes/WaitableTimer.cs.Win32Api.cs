using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;

namespace MicroSign.Core.WaitableTimes
{
    partial class WaitableTimer
    {
        /// <summary>
        /// Win 32 API
        /// </summary>
        protected static class Win32Api
        {
            /// <summary>
            /// CreateWaitableTimer
            /// </summary>
            /// <param name="lpTimerAttributes"></param>
            /// <param name="bManualReset"></param>
            /// <param name="lpTimerName"></param>
            /// <returns></returns>
            [DllImport("kernel32.dll")]
            public static extern SafeWaitHandle CreateWaitableTimer(IntPtr lpTimerAttributes, bool bManualReset, string? lpTimerName);

            /// <summary>
            /// SetWaitableTimer`
            /// </summary>
            /// <param name="hTimer"></param>
            /// <param name="pDueTime"></param>
            /// <param name="lPeriod"></param>
            /// <param name="pfnCompletionRoutine"></param>
            /// <param name="lpArgToCompletionRoutine"></param>
            /// <param name="fResume"></param>
            /// <returns></returns>
            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool SetWaitableTimer(SafeWaitHandle hTimer, [In] ref long pDueTime, int lPeriod, IntPtr pfnCompletionRoutine, IntPtr lpArgToCompletionRoutine, [MarshalAs(UnmanagedType.Bool)] bool fResume);

            //2024.05.10:CS)杉原:非公開APIを使わないようにする >>>>> ここから
            ///// <summary>
            ///// [非API関数]精度を変更する
            ///// </summary>
            ///// <param name="DesiredResolution"></param>
            ///// <param name="SetResolution"></param>
            ///// <param name="CurrentResolution"></param>
            ///// <returns></returns>
            //[DllImport("ntdll.dll")]
            //public static extern int NtSetTimerResolution(int DesiredResolution, bool SetResolution, out int CurrentResolution);
            //----------
            //2024.05.10:CS)杉原:非公開APIを使わないようにする <<<<< ここまで

            /// <summary>
            /// 定期的なタイマーの最小解像度を要求する
            /// </summary>
            /// <param name="uMilliseconds"></param>
            /// <returns></returns>
            [DllImport("winmm.dll")]
            public static extern uint timeBeginPeriod(uint uMilliseconds);

            /// <summary>
            /// 以前に設定した最小タイマー解像度をクリアする
            /// </summary>
            /// <param name="uMilliseconds"></param>
            /// <returns></returns>
            [DllImport("winmm.dll")]
            public static extern uint timeEndPeriod(uint uMilliseconds);
        }
    }
}
