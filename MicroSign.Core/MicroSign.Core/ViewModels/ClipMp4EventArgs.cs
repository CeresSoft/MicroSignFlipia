using System;

namespace MicroSign.Core.ViewModels
{
    /// <summary>
    /// MP4クリップ要求イベントハンドラ定義
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void Mp4ClipRequestEventHandler(object sender, Mp4ClipRequestEventArgs e);

    /// <summary>
    /// MP4クリップ要求状態
    /// </summary>
    public enum Mp4ClipRequestState
    {
        /// <summary>
        /// 適用してください
        /// </summary>
        Apply,

        /// <summary>
        /// キャンセルされました
        /// </summary>
        Cancel,

        /// <summary>
        /// 失敗
        /// </summary>
        Failed,
    }

    /// <summary>
    /// MP4クリップ要求イベント引数
    /// </summary>
    public class Mp4ClipRequestEventArgs : EventArgs
    {
        /// <summary>
        /// パネル横幅
        /// </summary>
        public int PanelWidth { get; protected set; } = CommonConsts.Values.Zero.I;

        /// <summary>
        /// パネル縦幅
        /// </summary>
        public int PanelHeight { get; protected set; } = CommonConsts.Values.Zero.I;

        /// <summary>
        /// SourceReader
        /// </summary>
        public MicroSign.Core.MediaFoundations.MP4StreamRender? MP4 { get; protected set; } = null;

        /// <summary>
        /// 倍率
        /// </summary>
        /// <remarks>動画のスケーリング</remarks>
        public double ClipScale { get; protected set; } = MicroSignConsts.Clip.DefaultScale;

        /// <summary>
        /// クリップX始点
        /// </summary>
        public int ClipX { get; protected set; } = MicroSignConsts.Clip.DefaultX;

        /// <summary>
        /// クリップY始点
        /// </summary>
        public int ClipY { get; protected set; } = MicroSignConsts.Clip.DefaultY;

        /// <summary>
        /// 状態
        /// </summary>
        public Mp4ClipRequestState Status { get; protected set; } = Mp4ClipRequestState.Failed;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="panelWidth"></param>
        /// <param name="panelHeight"></param>
        /// <param name="mp4"></param>
        public Mp4ClipRequestEventArgs(int panelWidth, int panelHeight, MicroSign.Core.MediaFoundations.MP4StreamRender? mp4)
        {
            this.PanelWidth = panelWidth;
            this.PanelHeight = panelHeight;
            this.MP4 = mp4;
        }

        /// <summary>
        /// クリップ設定
        /// </summary>
        /// <param name="clipScale"></param>
        /// <param name="clipX"></param>
        /// <param name="clipY"></param>
        public void SetApply(double clipScale, int clipX, int clipY)
        {
            this.ClipScale = clipScale;
            this.ClipX = clipX;
            this.ClipY = clipY;
            this.Status = Mp4ClipRequestState.Apply;
        }

        /// <summary>
        /// キャンセル
        /// </summary>
        public void SetCancel()
        {
            this.Status = Mp4ClipRequestState.Cancel;
        }
    }
}
