using System.Threading;
using System.Threading.Tasks;
using LOGGER = MicroSign.Core.CommonLogger;

namespace MicroSign.Core.ViewModels
{
    partial class MainWindowViewModel
    {
        /// <summary>
        /// アニメーション開始
        /// </summary>
        public struct StartAnimationResult
        {
            /// <summary>
            /// 成功フラグ
            /// </summary>
            public readonly bool IsSuccess;

            /// <summary>
            /// メッセージ
            /// </summary>
            public readonly string? Message;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="isSuccess"></param>
            /// <param name="message"></param>
            private StartAnimationResult(bool isSuccess, string? message)
            {
                this.IsSuccess = isSuccess;
                this.Message = message;
            }

            /// <summary>
            /// 失敗
            /// </summary>
            /// <param name="message"></param>
            public static StartAnimationResult Failed(string message)
            {
                StartAnimationResult result = new StartAnimationResult(false, message);
                return result;
            }

            /// <summary>
            /// 成功
            /// </summary>
            /// <returns></returns>
            public static StartAnimationResult Success()
            {
                StartAnimationResult result = new StartAnimationResult(true, null);
                return result;
            }
        }

        /// <summary>
        /// アニメーション開始
        /// </summary>
        /// <returns></returns>
        public StartAnimationResult StartAnimation()
        {
            //再生中判定
            {
                bool isPlay = this.IsPlayingAnimation;
                if (isPlay)
                {
                    //再生中は無視する
                    return StartAnimationResult.Failed("アニメーション再生中です");
                }
                else
                {
                    //再生していない場合は処理続行
                }
            }

            //選択しているアニメーションを取得
            AnimationImageItem? selectAnimationItem = this.GetSelectAnimationImage();
            if (selectAnimationItem == null)
            {
                //無効の場合は終了
                return StartAnimationResult.Failed("アニメーション画像が選択されていません");
            }
            else
            {
                //有効の場合は処理続行
            }

            //アニメーション一覧を取得
            // >> コンストラクタで生成しているのでnullチェック不要
            AnimationImageItemCollection animationImageItems = this.AnimationImages;

            // 現在の同期コンテキストを取得（UIスレッドなど）
            TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();

            //タスク起動
            LOGGER.Debug("アニメーションタスク開始");
            CancellationTokenSource cancel = new CancellationTokenSource();
            CancellationToken token = cancel.Token;
            Task t = Task.Run(() =>
            {
                this.AnimationTask(token, animationImageItems, selectAnimationItem);
            }).ContinueWith((x) => {
                //アニメーション終了設定
                LOGGER.Debug("アニメーションタスク終了");
                this.IsPlayingAnimation = false;
                this._AnimationTask = null;
                CommonUtils.SafeDispose(this._AnimationCancel);
                this._AnimationCancel = null;
            });

            //アニメーション開始
            this.IsPlayingAnimation = true;
            this._AnimationTask = t;
            this._AnimationCancel = cancel;

            //成功で終了
            LOGGER.Debug("アニメーションタスク開始成功");
            return StartAnimationResult.Success();
        }


    }
}
