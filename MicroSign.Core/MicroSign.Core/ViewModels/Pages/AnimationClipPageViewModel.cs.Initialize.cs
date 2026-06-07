using System.Windows.Media.Imaging;

namespace MicroSign.Core.ViewModels.Pages
{
    partial class AnimationClipPageViewModel
    {
        /// <summary>
        /// ViewModel初期化
        /// </summary>
        public void Initialize()
        {
            //オリジナル画像を取得
            BitmapSource? originalImage = this.OriginalImage;
            if (originalImage == null)
            {
                //オリジナル画像が無効の場合は初期化失敗
                string msg = "オリジナル画像が設定されていません";
                LOGGER.Warn(msg);
                this.SetStatus(AnimationClipPageStateKind.Failed, msg);
                return;
            }
            else
            {
                //有効の場合は続行
            }

            //オリジナル画像の横幅を取得
            int imageWidth = originalImage.PixelWidth;

            //オリジナル画像の縦幅を取得
            int imageHeight = originalImage.PixelHeight;

            //マトリクスLEDの横幅を取得
            int matrixLedWidth = this.MatrixLedWidth;

            //マトリクスLEDの縦幅を取得
            int matrixLedHeight = this.MatrixLedHeight;

            //縮小後の画像サイズを計算
            // >> 初期値はLEDサイズそのまま
            int previewWidth = matrixLedWidth;
            int previewHeight = matrixLedHeight;
            LOGGER.Debug($"縮小後の画像サイズを計算 ImageWidth={imageWidth}, ImageHeight={imageHeight}, MatrixLedWidth={matrixLedWidth}, MatrixLedHeight={matrixLedHeight}");

            // >> 最初に横幅にあわせて計算する
            {
                //横幅のオリジナル:マトリクスLEDの比率を計算
                double ratio = matrixLedWidth / (double)imageWidth;

                //プレビュー画像の縦幅を計算
                double previewHeightD = imageHeight * ratio;
                // >> 整数に丸める
                previewHeight = (int)previewHeightD;
                LOGGER.Debug($"横幅を基準に計算 Width={previewWidth}, Height={previewHeight}");
            }

            // >> 横幅に合わせて縮小した場合に縦幅が足りるか
            if (previewHeight < matrixLedHeight)
            {
                LOGGER.Debug($"縮小後縦幅がマトリクスLED縦幅より小さいため再計算");

                //縦幅のオリジナル:マトリクスLEDの比率を計算
                double ratio = matrixLedHeight / (double)imageHeight;

                //プレビュー画像の横幅を計算
                double previewWidthD = imageWidth * ratio;
                // >> 整数に丸める
                previewWidth = (int)previewWidthD;

                //縦幅はマトリクスLEDの縦幅にあわせる
                previewHeight = matrixLedHeight;
                LOGGER.Debug($"縦幅を基準に計算 Width={previewWidth}, Height={previewHeight}");

                //移動方向の初期値を右から左に変更
                this.MoveDirection = AnimationMoveDirection.Left;
            }
            else
            {
                //縦幅が足りる場合は何もしない
            }

            { 
                //レンダーターゲットビットマップを生成
                RenderTargetBitmap? renderBitmap = null;
                {
                    var ret = this.Model.CreateRenderTargetBitmap(previewWidth, previewHeight);
                    if (ret.IsSuccess)
                    {
                        //成功の場合は続行
                    }
                    else
                    {
                        //失敗の場合は終了
                        string msg = $"レンダリングターゲットビットマップの生成に失敗しました (理由={ret.Message})";
                        LOGGER.Warn(msg);
                        this.SetStatus(AnimationClipPageStateKind.Failed, msg);
                        return;
                    }

                    //生成したレンダーターゲットビットマップを取得
                    renderBitmap = ret.RenderBitmap;
                }

                //プレビュー画像を描画
                {
                    var ret = this.Model.RenderImage(renderBitmap, originalImage, CommonConsts.Points.Zero.X, CommonConsts.Points.Zero.Y, previewWidth, previewHeight);
                    if (ret.IsSuccess)
                    {
                        //成功の場合は処理続行
                    }
                    else
                    {
                        //失敗の場合は終了
                        string msg = $"プレビュー画像を描画に失敗しました (理由={ret.Message})";
                        LOGGER.Warn(msg);
                        this.SetStatus(AnimationClipPageStateKind.Failed, msg);
                        return;
                    }
                }

                //内容が確定したのでフリーズする
                renderBitmap?.Freeze();

                //画像を保存
                this.PreviewImage = renderBitmap;
            }

            //初期化完了にする
            this.SetStatus(AnimationClipPageStateKind.Initialized, string.Empty);
        }
    }
}
