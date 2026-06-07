using Microsoft.Win32.SafeHandles;
using System;
using LOGGER = MicroSign.Core.MicroSignLogger;

namespace MicroSign.Core.WaitableTimes
{
    partial class WaitableTimer
    {
        /// <summary>
        /// 開始結果
        /// </summary>
        public enum StartResult
        {
            /// <summary>
            /// 成功
            /// </summary>
            Success,

            /// <summary>
            /// 開始済み
            /// </summary>
            AlreadyStarted,

            /// <summary>
            /// タイマーの解像度を要求で例外発生
            /// </summary>
            TimeBeginPeriodException,

            /// <summary>
            /// WaitableTimer生成失敗
            /// </summary>
            CreateWaitableTimerFailed,

            /// <summary>
            /// WaitableTimer生成で例外発生
            /// </summary>
            CreateWaitableTimerException,

            /// <summary>
            /// WaitableTimer設定失敗
            /// </summary>
            SetWaitableTimerFailed,

            /// <summary>
            /// WaitableTimer設定で例外発生
            /// </summary>
            SetWaitableTimerException,
        }

        /// <summary>
        /// 開始(ミリ秒)
        /// </summary>
        /// <param name="millisecond">シグナル間隔(ミリ秒)</param>
        public StartResult StartByMillisecond(long millisecond)
        {
            //シグナル間隔を設定
            this.SetSignalIntervalByMillisecond(millisecond);

            //開始
            StartResult result = this.Start();
            return result;
        }

        /// <summary>
        /// 開始(マイクロ秒)
        /// </summary>
        /// <param name="microsecond">シグナル間隔(マイクロ秒)</param>
        public StartResult StartByMicrosecond(long microsecond)
        {
            //シグナル間隔を設定
            this.SetSignalIntervalByMicrosecond(microsecond);

            //呼び出し
            StartResult result = this.Start();
            return result;
        }

        /// <summary>
        /// 開始
        /// </summary>
        /// <param name="intervalTime">シグナル間隔(100ナノ秒単位=FILE構造体の時間)</param>
        public StartResult Start(long intervalTime)
        {
            //シグナル間隔を設定
            this.SetSignalInterval(intervalTime);

            //開始
            StartResult result = this.Start();
            return result;
        }

        /// <summary>
        /// 開始
        /// </summary>
        public StartResult Start()
        {
            //タイマー開始判定
            {
                bool isRunning = this.IsRunning;
                if (isRunning)
                {
                    //実行中の場合は開始しているので何もしない
                    return StartResult.AlreadyStarted;
                }
                else
                {
                    //未実行の場合は処理続行
                }
            }

            //現在時間を取得
            DateTime nowDateTime = DateTime.Now;

            //指定する時間を生成
            // >> FILE構造体の時間で指定する(UTCで100ナノ秒単位の経過時間)
            long nowTime = nowDateTime.ToFileTimeUtc();

            //次のシグナル時間を計算
            long intervalTime = this.SignalInterval;
            long dueTime = nowTime + intervalTime;

            //WaitableTimer生成及び開始
            {
                SafeWaitHandle? handle = null;
                try
                {
                    try
                    {
                        //2024.05.10:CS)杉原:非公開APIを使わないようにする >>>>> ここから
                        ////[非API関数]精度を変更するで1ms
                        //WaitableTimer.Win32Api.NtSetTimerResolution(1, true, out int ntCurrentResolution);
                        //----------
                        // >> ネットの情報では精度が1msほど悪化する(1msウエイトすると約2msの待ちになるらしい)が
                        // >> 非公開のAPIを使うのは最終手段なのでやめておきます
                        //1msの解像度を要求する
                        WaitableTimer.Win32Api.timeBeginPeriod(1);
                        //2024.05.10:CS)杉原:非公開APIを使わないようにする <<<<< ここまで
                    }
                    catch (Exception /*ex*/)
                    {
                        return StartResult.TimeBeginPeriodException;
                    }

                    //ハンドルを取得
                    bool manualReset = this.IsManualReset;
                    string? timerName = this.TimerName;
                    try
                    {
                        handle = WaitableTimer.Win32Api.CreateWaitableTimer(IntPtr.Zero, manualReset, timerName);
                        if (handle == null)
                        {
                            //無効の場合は終了
                            return StartResult.CreateWaitableTimerFailed;
                        }
                        else
                        {
                            //有効
                        }
                    }
                    catch (Exception /*ex*/)
                    {
                        return StartResult.CreateWaitableTimerException;
                    }

                    //タイマー設定
                    try
                    {
                        //呼び出し
                        bool ret = WaitableTimer.Win32Api.SetWaitableTimer(handle, ref dueTime, 0, IntPtr.Zero, IntPtr.Zero, false);
                        if (ret)
                        {
                            //成功の場合は処理続行
                        }
                        else
                        {
                            //失敗の場合は終了
                            return StartResult.SetWaitableTimerFailed;
                        }
                    }
                    catch (Exception /*ex*/)
                    {
                        return StartResult.SetWaitableTimerException;
                    }

                    //ここまで来たら保存
                    {
                        //メンバ変数に覚える
                        this.SafeWaitHandle = handle;

                        //解放しないようにハンドルを無効にする
                        handle = null;

                        //実行中にする
                        this.IsRunning = true;

                        //開始時間を保存する
                        this.StartTime = nowTime;

                        //シグナル時間を保存する
                        this.SignalTime = dueTime;
                    }

                    //ここまで来たら成功
                    return StartResult.Success;
                }
                finally
                {
                    if (handle == null)
                    {
                        //無効の場合は何もしない
                    }
                    else
                    {
                        //有効の場合は破棄する
                        try
                        {
                            handle.Close();
                        }
                        catch (Exception)
                        {
                            //例外は握りつぶす
                        }
                        finally
                        {
                            //破棄する
                            try
                            {
                                handle.Dispose();
                            }
                            catch (Exception)
                            {
                                //例外は握りつぶす
                            }
                        }
                    }
                }
            }
        }
    }
}
