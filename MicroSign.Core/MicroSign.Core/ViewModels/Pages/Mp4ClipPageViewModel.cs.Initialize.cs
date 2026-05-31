using System;

namespace MicroSign.Core.ViewModels.Pages
{
    partial class Mp4ClipPageViewModel
    {
        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            //引数取得
            MicroSign.Core.ViewModels.Mp4ClipRequestEventArgs args = this._Args;
            if(args == null)
            {
                //無効の場合は終了
                string msg = "MP4クリップ要求引数が無効";
                CommonLogger.Warn(msg);
                this.SetWarnMessage(msg);
                return;
            }
            else
            {
                //有効の場合は処理続行
                CommonLogger.Debug("MP4クリップ要求引数有効");
            }

            //パネル横幅取得
            int panelWidth = args.PanelWidth;
            int panelHeight = args.PanelHeight;
            this.PanelWidth = panelWidth;
            this.PanelHeight = panelHeight;

            //MP4取得
            MicroSign.Core.MediaFoundations.MP4StreamRender? mp4 = args.MP4;
            if(mp4 == null)
            {
                //無効の場合は終了
                string msg = "MP4が無効";
                CommonLogger.Warn(msg);
                this.SetWarnMessage(msg);
                return;
            }
            else
            {
                //有効の場合は処理続行
                CommonLogger.Debug("MP4有効");
            }

            //ビデオサイズ取得
            int videoWidth = (int)System.Windows.Size.Empty.Width;
            int videoHeight = (int)System.Windows.Size.Empty.Height;
            {
                CommonLogger.Debug($"ビデオサイズ取得 - 開始");
                MicroSign.Core.MediaFoundations.MP4StreamRender.GetVideoSizeResult ret = mp4.GetVideoSize();
                bool isSuccess = ret.IsSuccess;
                if (isSuccess)
                {
                    //成功の場合は処理続行
                    CommonLogger.Debug($"ビデオサイズ取得 - 成功");
                    videoWidth = ret.Width;
                    videoHeight = ret.Height;
                }
                else
                {
                    //失敗した場合は終了
                    string? msg = $"ビデオサイズ取得 - 失敗({ret.ErrorMessage})";
                    CommonLogger.Warn(msg);
                    this.SetWarnMessage(msg);
                    return;
                }
            }
            this.VideoWidth = videoWidth;
            this.VideoHeight = videoHeight;

            //ビデオ縦横サイズ有効判定
            {
                //横サイズ判定
                if (CommonConsts.Values.Zero.I < videoWidth)
                {
                    //横サイズが有効の場合は処理続行
                    CommonLogger.Info($"ビデオ横サイズ={videoWidth}");
                }
                else
                {
                    //横サイズが無効の場合は終了
                    string msg = $"ビデオの横サイズが0";
                    CommonLogger.Warn(msg);
                    this.SetWarnMessage(msg);
                    return;
                }

                //縦サイズ判定
                if (CommonConsts.Values.Zero.I < videoHeight)
                {
                    //横サイズが有効の場合は処理続行
                    CommonLogger.Info($"ビデオ縦サイズ={videoHeight}");
                }
                else
                {
                    //横サイズが無効の場合は終了
                    string msg = $"ビデオの縦サイズが0";
                    CommonLogger.Warn(msg);
                    this.SetWarnMessage(msg);
                    return;
                }
            }

            //最小スケール/最大スケール計算
            {
                double pw = panelWidth;
                double ph = panelHeight;
                double vw = videoWidth;
                double vh = videoHeight;

                //横比率・縦比率を計算
                double widthRate = pw / vw;
                double heightRate = ph / vh;

                CommonLogger.Debug($"横比率({pw}/{vw}={widthRate}), 縦比率({ph}/{vh}={heightRate})");

                //横比率・縦比率で大きい方を最小倍率にする
                double minScale = CommonConsts.Values.One.D;
                if (widthRate < heightRate)
                {
                    //縦の比率が大きい場合
                    minScale = heightRate;
                }
                else
                {
                    //横の比率が大きい場合
                    minScale = widthRate;
                }

                //最大スケール
                double maxScale = CommonConsts.Values.One.D;
                if (minScale < CommonConsts.Values.One.D)
                {
                    //比率が1倍未満の場合は縮小なので1倍(=初期値)でよい
                }
                else
                {
                    //比率が1倍以上の場合は拡大なので
                    //最大倍率は最小倍率の10倍にする
                    maxScale = minScale * CommonConsts.Values.Ten.D;
                }

                this.MinScale = minScale;
                this.MaxScale = maxScale;
                this.SelectScale = minScale;    //選択は最小のスケールにする
            }

            //表示期間取得
            long durationTicks = TimeSpan.Zero.Ticks;
            {
                CommonLogger.Debug($"ビデオ長さ取得 - 開始");
                MicroSign.Core.MediaFoundations.MP4StreamRender.GetDurationResult ret = mp4.GetDuration();
                bool isSuccess = ret.IsSuccess;
                if (isSuccess)
                {
                    //成功の場合は処理続行
                    CommonLogger.Debug($"ビデオ長さ取得 - 成功");
                    durationTicks = ret.DurationTicks;
                }
                else
                {
                    //失敗した場合は終了
                    string? msg = $"ビデオ長さ取得 - 失敗({ret.ErrorMessage})";
                    CommonLogger.Warn(msg);
                    this.SetWarnMessage(msg);
                    return;
                }
            }

            //長さ有効判定
            if(TimeSpan.Zero.Ticks < durationTicks)
            {
                //有効の場合は処理続行
                CommonLogger.Info($"ビデオ長さ ({durationTicks}ticks)");
            }
            else
            {
                //無効の場合は終了
                string? msg = $"ビデオ長さ無効 ({durationTicks}ticks)";
                CommonLogger.Warn(msg);
                this.SetWarnMessage(msg);
                return;
            }

            this.MaxDurationTicks = durationTicks;


            //準備完了
            {
                string msg = "準備完了";
                CommonLogger.Info(msg);
                this.SetInfoMessage(msg);
                this.IsReady = CommonUtils.Not(Mp4ClipPageViewModel.InitializeValues.IsReady);
            }
        }

    }
}
