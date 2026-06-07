using System.Windows.Media.Imaging;

namespace MicroSign.Core.ViewModels.Pages
{
    /// <summary>
    /// アニメーションテキストページViewModel
    /// </summary>
    partial class AnimationTextPageViewModel
    {
        /// <summary>
        /// 設定されている内容で描写ビットマップを更新する
        /// </summary>
        public void UpdateRenderBitmap()
        {
            //状態判定
            {
                AnimationTextPageStateKind status = this.StatusKind;
                switch (status)
                {
                    case AnimationTextPageStateKind.Initialized:
                        //初期化済みなら処理続行
                        //ログが多いのでコメント化:CommonLogger.Debug("初期化済み");
                        break;

                    //2025.09.30:CS)土田:ビットマップは更新時に毎回生成するように変更 >>>>> ここから
                    //----------
                    case AnimationTextPageStateKind.Ready:
                        //準備完了なら処理続行
                        //ログが多いのでコメント化:CommonLogger.Debug("準備完了");
                        break;

                    case AnimationTextPageStateKind.Failed:
                        //失敗なら処理続行
                        // >> 前回の更新に失敗しても新しくビットマップを生成するので続行する
                        //ログが多いのでコメント化:CommonLogger.Warn("失敗");
                        break;
                    //2025.09.30:CS)土田:ビットマップは更新時に毎回生成するように変更 <<<<< ここまで

                    default:
                        //それ以外の場合は処理できないので終了
                        LOGGER.Warn($"初期化されていないか、初期化失敗 (Status={status})");
                        return;
                }
            }

            //表示文章取得
            string displayText = this.DisplayText ?? string.Empty;

            //フォントサイズ取得
            int fontSize = this.SelectFontSize;

            //フォント色取得
            int fontColor = this.SelectFontColor;

            //文字スクロール有効判定
            bool isScrollEnabled = this.IsScrollEnabled;
            if (isScrollEnabled)
            {
                //スクロール有効のビットマップ更新
                this.UpdateScrollRenderBitmap(displayText, fontSize, fontColor);
            }
            else
            {
                //通常のビットマップ更新
                this.UpdateNormalRenderBitmap(displayText, fontSize, fontColor);
            }
        }

        /// <summary>
        /// スクロール有効のビットマップ更新
        /// </summary>
        /// <param name="displayText"></param>
        /// <param name="fontSize"></param>
        /// <param name="fontColor"></param>
        protected void UpdateScrollRenderBitmap(string displayText, int fontSize, int fontColor)
        {
            //モデル取得(シングルトンで生成してnullにならないためnullチェック省略)
            Models.Model model = this.Model;

            //マトリクスLEDの縦幅を取得
            int matrixLedHeight = this.MatrixLedHeight;

            //文字ビットマップ生成
            RenderTargetBitmap? textBitmap = null;
            {
                //横幅=未指定、縦幅=マトリクスLED縦幅、余白なしで生成する
                // >> 左右方向の余白を除いた内容部分のみ描画する
                var ret = model.CreateTextRenderBitmap(fontSize, fontColor, displayText, double.NaN, matrixLedHeight);
                if (ret.IsSuccess)
                {
                    //成功の場合はレンダリングターゲットビットマップを取得
                    textBitmap = ret.RenderBitmap;
                }
                else
                {
                    //失敗の場合
                    string msg = $"レンダリングターゲットビットマップの生成に失敗しました (理由={ret.Message})";
                    LOGGER.Warn(msg);
                    this.SetStatus(AnimationTextPageStateKind.Failed, msg);
                    return;
                }
            }

            //文字ビットマップ有効判定
            if (textBitmap == null)
            {
                //無効の場合は処理できないので終了
                string msg = "文字ビットマップが無効です";
                LOGGER.Warn(msg);
                this.SetStatus(AnimationTextPageStateKind.Failed, msg);
                return;
            }
            else
            {
                //有効の場合は処理続行
                //ログが多いのでコメント化:CommonLogger.Debug("文字ビットマップが有効");
            }

            //画面に反映
            this.RenderBitmap = textBitmap;

            //ここまできたら更新成功
            this.SetStatus(AnimationTextPageStateKind.Ready, string.Empty);
        }

        /// <summary>
        /// 通常のビットマップ更新
        /// </summary>
        /// <param name="displayText"></param>
        /// <param name="fontSize"></param>
        /// <param name="fontColor"></param>
        protected void UpdateNormalRenderBitmap(string displayText, int fontSize, int fontColor)
        {
            //モデル取得(シングルトンで生成してnullにならないためnullチェック省略)
            Models.Model model = this.Model;

            //マトリクスLEDの横幅を取得
            int matrixLedWidth = this.MatrixLedWidth;

            //マトリクスLEDの縦幅を取得
            int matrixLedHeight = this.MatrixLedHeight;

            //レンダリングターゲットビットマップを生成
            RenderTargetBitmap? renderBitmap = null;
            {
                var ret = model.CreateRenderTargetBitmap(matrixLedWidth, matrixLedHeight);
                if (ret.IsSuccess)
                {
                    //成功の場合はレンダリングターゲットビットマップを取得
                    renderBitmap = ret.RenderBitmap;
                }
                else
                {
                    //失敗の場合
                    string msg = $"レンダリングターゲットビットマップの生成に失敗しました (理由={ret.Message})";
                    LOGGER.Warn(msg);
                    this.SetStatus(AnimationTextPageStateKind.Failed, msg);
                    return;
                }
            }

            //描写
            {
                var ret = model.RenderText(renderBitmap, fontSize, fontColor, displayText);
                if (ret.IsSuccess)
                {
                    //成功の場合そのまま
                    //ログが多いのでコメント化:CommonLogger.Debug("文字描写に成功");
                }
                else
                {
                    //失敗の場合は終了
                    string msg = $"文字描写の生成に失敗しました (理由={ret.Message})";
                    LOGGER.Warn(msg);
                    this.SetStatus(AnimationTextPageStateKind.Failed, msg);
                    return;
                }
            }

            //画面に反映
            this.RenderBitmap = renderBitmap;

            //ここまできたら更新成功
            this.SetStatus(AnimationTextPageStateKind.Ready, string.Empty);
        }
    }
}
