namespace MicroSign.Core.Navigations
{
    /// <summary>
    /// ナビゲーションコントローラー用FrameworkElement拡張メソッド
    /// </summary>
    /// <remarks>現在表示しているエレメントを破棄し、新しいエレメントを表示します</remarks>
    public static partial class NavigationFrameworkElementExtention
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
    }
}
