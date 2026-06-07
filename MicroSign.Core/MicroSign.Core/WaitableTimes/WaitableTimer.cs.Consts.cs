namespace MicroSign.Core.WaitableTimes
{
    partial class WaitableTimer
    {
        /// <summary>
        /// 定数定義
        /// </summary>
        public static class Consts
        {
            /// <summary>
            /// ミリ秒単位の1秒
            /// </summary>
            /// <remarks>10000 x1ミリ秒単位 = 1秒</remarks>
            public static readonly long OneSecByMillis = 1000;

            /// <summary>
            /// マイクロ秒単位の1ミリ秒
            /// </summary>
            /// <remarks>10000 x1マイクロ秒単位 = 1ミリ秒</remarks>
            public static readonly long OneMillisByMicros = 1000;

            /// <summary>
            /// 100ナノ秒単位の1マイクロ秒
            /// </summary>
            /// <remarks>10 x100ナノ単位 = 1マイクロ秒</remarks>
            public static readonly long OneMicrosBy100Nanos = 10;

            /// <summary>
            /// 100ナノ秒単位の1ミリ秒
            /// </summary>
            public static readonly long OneMillisBy100Nanos = OneMillisByMicros * OneMicrosBy100Nanos;

            /// <summary>
            /// 100ナノ秒単位の1秒
            /// </summary>
            public static readonly long OneSecBy100Nanos = OneSecByMillis * OneMillisByMicros * OneMicrosBy100Nanos;
        }
    }
}
