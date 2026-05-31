using System;

namespace MicroSign.Core.ViewModels
{
    partial class MainWindowViewModel
    {
        /// <summary>
        /// MP4クリップ要求イベント
        /// </summary>
        public event Mp4ClipRequestEventHandler? Mp4ClipRequest;

        /// <summary>
        /// MP4クリップ要求イベント発行
        /// </summary>
        /// <param name="panelWidth">パネル横幅</param>
        /// <param name="panelHeight">パネル縦幅</param>
        /// <param name="mp4">MP4</param>
        /// <returns></returns>
        protected Mp4ClipRequestEventArgs RaiseMp4ClipRequest(int panelWidth, int panelHeight, MicroSign.Core.MediaFoundations.MP4StreamRender? mp4)
        {
            //とりあえず引数を生成
            Mp4ClipRequestEventArgs args = new Mp4ClipRequestEventArgs(panelWidth, panelHeight, mp4);

            //ハンドラーを取得
            Mp4ClipRequestEventHandler? handler = this.Mp4ClipRequest;
            if(handler == null)
            {
                //無効の場合は何もせずに終了
                CommonLogger.Warn("MP4クリップ要求イベントハンドラーなし");
                return args;
            }
            else
            {
                //有効の場合は処理続行
            }

            //呼び出し
            try
            {
                handler(this, args);
            }
            catch (Exception ex)
            {
                //例外は握りつぶす
                CommonLogger.Warn("MP4クリップ要求イベント発行で例外発生", ex);
            }

            //そのまま終了
            return args;
        }
    }
}
