namespace MicroSign.Core.WaitableTimes
{
    partial class WaitableTimer
    {
        /// <summary>
        /// マニュアルリセット
        /// </summary>
        public bool IsManualReset { get; protected set; } = true;

        /// <summary>
        /// タイマー名
        /// </summary>
        public string? TimerName { get; protected set; } = null;

        /// <summary>
        /// 実行中フラグ
        /// </summary>
        public bool IsRunning { get; protected set; } = false;

        /// <summary>
        /// 開始時間
        /// </summary>
        /// <remarks>
        /// FILE構造体の時間単位です(=100ナノ秒単位)
        /// Start()内で設定します
        /// </remarks>
        public long StartTime { get; protected set; } = 0;

        /// <summary>
        /// シグナルになる時間
        /// </summary>
        /// <remarks>
        /// FILE構造体の時間単位です(=100ナノ秒単位)
        /// Start()内で設定します
        /// </remarks>
        public long SignalTime { get; protected set; } = 0;

        /// <summary>
        /// シグナルになる間隔
        /// </summary>
        /// <remarks>
        /// FILE構造体の時間単位です(=100ナノ秒単位)
        /// Start()内で設定します
        /// </remarks>
        public long SignalInterval { get; protected set; } = 0;
    }
}
