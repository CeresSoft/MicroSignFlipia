namespace MicroSign.Core.MediaFoundations
{
    partial class MP4StreamRender
    {
        /// <summary>
        /// パス
        /// </summary>
        /// <remarks>コンストラクタで設定</remarks>
        public string Path { get; protected set; } = string.Empty;

        /// <summary>
        /// StreamRender
        /// </summary>
        /// <remarks>コンストラクタで設定</remarks>
        public MediaFoundation.ReadWrite.IMFSourceReader? SourceReader { get; protected set; } = null;

        /// <summary>
        /// 対象のビデオストリームインデックス
        /// </summary>
        /// <remarks>コンストラクタで設定</remarks>
        public int TargetVideoStreamIndex { get; protected set; } = CommonConsts.Index.First;
    }
}
