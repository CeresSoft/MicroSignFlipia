namespace MicroSign.Core.Models
{
    /// <summary>
    /// モデル
    /// </summary>
    public partial class Model
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
        /// モデルインスタンス
        /// </summary>
        /// <remarks>
        /// 2023.11.23:CS)土田:Core.dllに分離するにあたり、どこからでも参照できる単一インスタンスを追加
        ///  >> 分離前はAppクラスにstaticプロパティを定義していました
        /// </remarks>
        public static MicroSign.Core.Models.Model Instance { get; private set; } = new MicroSign.Core.Models.Model();
    }
}
