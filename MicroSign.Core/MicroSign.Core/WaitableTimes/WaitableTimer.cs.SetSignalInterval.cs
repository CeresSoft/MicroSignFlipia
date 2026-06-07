namespace MicroSign.Core.WaitableTimes
{
    partial class WaitableTimer
    {
        /// <summary>
        /// シグナル間隔設定
        /// </summary>
        /// <param name="intervalTime">シグナル間隔(100ナノ秒単位=FILE構造体の時間)</param>
        public void SetSignalInterval(long intervalTime)
        {
            this.SignalInterval = intervalTime;
        }

        /// <summary>
        /// シグナル間隔設定(ミリ秒)
        /// </summary>
        /// <param name="millisecond">シグナル間隔(ミリ秒)</param>
        public void SetSignalIntervalByMillisecond(long millisecond)
        {
            //待ち時間をFILE構造体の時間に変換
            // >> FILE構造体の時間は100ナノ秒単位なので x10でマイクロ秒 x1000でミリ秒にする
            long intervalTime = millisecond * WaitableTimer.Consts.OneMillisBy100Nanos;

            //設定
            this.SetSignalInterval(intervalTime);
        }

        /// <summary>
        /// シグナル間隔設定(マイクロ秒)
        /// </summary>
        /// <param name="microsecond">シグナル間隔(マイクロ秒)</param>
        public void SetSignalIntervalByMicrosecond(long microsecond)
        {
            //待ち時間をFILE構造体の時間に変換
            // >> FILE構造体の時間は100ナノ秒単位なので x10でマイクロ秒にする
            long intervalTime = microsecond * WaitableTimer.Consts.OneMicrosBy100Nanos;

            //設定
            this.SetSignalInterval(intervalTime);
        }
    }
}
