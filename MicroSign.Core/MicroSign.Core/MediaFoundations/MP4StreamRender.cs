using MicroSign.Core.Disposables;

namespace MicroSign.Core.MediaFoundations
{
    /// <summary>
    /// MediaFoundation.ReadWrite.IMFSourceReader sourceReaderのラッパークラス
    /// </summary>
    public partial class MP4StreamRender : UnmanageDisposable
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="path"></param>
        /// <param name="sourceReader"></param>
        /// <param name="videoStreamIndex"></param>
        public MP4StreamRender(string path, MediaFoundation.ReadWrite.IMFSourceReader sourceReader, int videoStreamIndex)
        {
            this.Path = path;
            this.SourceReader = sourceReader;
            this.TargetVideoStreamIndex = videoStreamIndex;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="path"></param>
        /// <param name="sourceReader"></param>
        public MP4StreamRender(string path, MediaFoundation.ReadWrite.IMFSourceReader sourceReader)
            :this(path, sourceReader, (int)MediaFoundation.ReadWrite.MF_SOURCE_READER.FirstVideoStream)
        {
        }

        /// <summary>
        /// マネージ破棄
        /// </summary>
        protected override void ManegedDispose()
        {
            //何もなし
        }

        /// <summary>
        /// 巨大メモリ破棄
        /// </summary>
        protected override void HugeManegedSetNull()
        {
            //何もなし
        }

        /// <summary>
        /// アンマネージ破棄
        /// </summary>
        protected override void UnmanegedDispose()
        {
            //sourceReader生成のfinallyで破棄しているので不要
            //CommonUtils.SafeComRelease(this.SourceReader);
        }
    }
}
