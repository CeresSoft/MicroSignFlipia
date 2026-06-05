using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MicroSign.Core.ViewModels.Pages
{
    partial class Mp4ClipPageViewModel
    {
        /// <summary>
        /// ビデオ画像を更新
        /// </summary>
        private void UpdateVideoImage()
        {
            //MP4クリップ要求引数取得
            MicroSign.Core.ViewModels.Mp4ClipRequestEventArgs? args = this._Args;
            if (args == null)
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

            //MP4取得
            MicroSign.Core.MediaFoundations.MP4StreamRender? mp4 = args.MP4;
            if (mp4 == null)
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

            //ビデオ長さ取得
            long maxDurationTicks = this.MaxDurationTicks;
            if (TimeSpan.Zero.Ticks < maxDurationTicks)
            {
                //有効の場合は処理続行
                CommonLogger.Debug($"ビデオ長さ有効 ({maxDurationTicks}ticks)");
            }
            else
            {
                //無効の場合は終了
                string? msg = $"ビデオ長さ無効 ({maxDurationTicks}ticks)";
                CommonLogger.Warn(msg);
                this.SetWarnMessage(msg);
                return;
            }

            //現在位置取得
            long selectVideoPosition = this.SelectVideoPosition;
            if (selectVideoPosition < TimeSpan.Zero.Ticks)
            {
                //範囲外の場合は範囲内にする
                CommonLogger.Debug($"再生位置が小さすぎる ({selectVideoPosition} < {TimeSpan.Zero.Ticks})");
                selectVideoPosition = TimeSpan.Zero.Ticks;
            }
            else
            {
                //範囲内の場合最大値と比較
                if (maxDurationTicks < selectVideoPosition)
                {
                    //範囲外の場合は範囲内にする
                    CommonLogger.Debug($"再生位置が大きすぎる ({maxDurationTicks} < {selectVideoPosition})");
                    selectVideoPosition = maxDurationTicks;
                }
                else
                {
                    //範囲内の場合は処理続行
                    CommonLogger.Debug($"再生位置有効 ({TimeSpan.Zero.Ticks} <= {selectVideoPosition} <= {maxDurationTicks})");
                }
            }

            //取得するフレームの位置にする
            {
                MicroSign.Core.MediaFoundations.MP4StreamRender.SetCurrentPositionResult ret = mp4.SetCurrentPosition(selectVideoPosition);
                bool isSuccess = ret.IsSuccess;
                if(isSuccess)
                {
                    //成功の場合は処理続行
                    CommonLogger.Debug($"再生位置設定成功 ({selectVideoPosition}ticks)");
                }
                else
                {
                    //失敗の場合は終了
                    string? msg = $"再生位置設定失敗 ({maxDurationTicks}ticks) {ret.ErrorMessage}";
                    CommonLogger.Warn(msg);
                    this.SetWarnMessage(msg);
                    return;
                }
            }

            //映像取得先
            byte[]? rgb32buffer = this.Rgb32Buffer;

            //ビデオフレーム取得
            {
                MicroSign.Core.MediaFoundations.MP4StreamRender.GetVideoImageResult ret = mp4.GetVideoImage(rgb32buffer);
                MicroSign.Core.MediaFoundations.MP4StreamRender.GetVideoImageState status = ret.Status;
                switch(status)
                {
                    case MicroSign.Core.MediaFoundations.MP4StreamRender.GetVideoImageState.Success:
                        //成功した場合は処理続行
                        CommonLogger.Debug($"ビデオ映像取得成功");
                        break;

                    case MicroSign.Core.MediaFoundations.MP4StreamRender.GetVideoImageState.EndOfStream:
                        //動画終了時は終了
                        CommonLogger.Debug($"ビデオ映像終了");
                        return;

                    default:
                        //それ以外は失敗で終了
                        string? msg = $"ビデオ映像取得失敗 ({maxDurationTicks}ticks) {ret.ErrorMessage}";
                        CommonLogger.Warn(msg);
                        this.SetWarnMessage(msg);
                        return;
                }
            }

            //ビットマップに書き込み
            WriteableBitmap? wbitmap = this.VideoImage;
            if (wbitmap == null)
            {
                //ビットマップが無効の場合は何もしない
                string? msg = $"ビットマップ無効";
                CommonLogger.Warn(msg);
                this.SetWarnMessage(msg);
                return;
            }
            else
            {
                //ビットマップが有効の場合は処理続行
                CommonLogger.Debug($"ビットマップ有効");
            }

            //WriteableBitmapに書込
            try
            {
                wbitmap.Lock();
                try
                {
                    //コピーする矩形領域（画像全体）を定義
                    int x = (int)CommonConsts.Points.Zero.X;
                    int y = (int)CommonConsts.Points.Zero.Y;
                    int width = wbitmap.PixelWidth;
                    int height = wbitmap.PixelHeight;
                    Int32Rect sourceRect = new Int32Rect(x, y, width, height);

                    //ストライドを計算
                    int stride = width * (wbitmap.Format.BitsPerPixel / CommonConsts.BitCount.BYTE);

                    //コピー
                    wbitmap.WritePixels(sourceRect, rgb32buffer, stride, CommonConsts.Index.First);
                }
                finally
                {
                    wbitmap.Unlock();
                }
            }
            catch (Exception ex)
            {
                string? msg = $"ビデオ映像取得で例外発生";
                CommonLogger.Warn(msg, ex);
                this.SetWarnMessage(msg);
                return;
            }

        }

    }
}
