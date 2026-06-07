using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MicroSign.Core.ViewModels.Pages
{
    public partial class AnimationClipPageViewModel
    {
        /// <summary>
        /// アニメーション切り抜き結果
        /// </summary>
        public struct ClipAnimationResult
        {
            /// <summary>
            /// 成功フラグ
            /// </summary>
            public readonly bool IsSuccess;

            /// <summary>
            /// エラーメッセージ
            /// </summary>
            public readonly string? ErrorMessage;

            /// <summary>
            /// 出力パス一覧
            /// </summary>
            public readonly List<string>? OutputPaths;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="isSuccess"></param>
            /// <param name="message"></param>
            /// <param name="outputFolder"></param>
            private ClipAnimationResult(bool isSuccess, string? message, List<string>? outputFolder)
            {
                this.IsSuccess = isSuccess;
                this.ErrorMessage = message;
                this.OutputPaths = outputFolder;
            }

            /// <summary>
            /// 失敗
            /// </summary>
            /// <param name="message"></param>
            /// <returns></returns>
            public static ClipAnimationResult Failed(string message)
            {
                ClipAnimationResult result = new ClipAnimationResult(false, message, null);
                return result;
            }

            /// <summary>
            /// 成功
            /// </summary>
            /// <param name="outputPaths"></param>
            /// <returns></returns>
            public static ClipAnimationResult Success(List<string>? outputPaths)
            {
                ClipAnimationResult result = new ClipAnimationResult(true, null, outputPaths);
                return result;
            }
        }

        /// <summary>
        /// アニメーション切り抜きコールバックデリゲート
        /// </summary>
        /// <param name="outputPath"></param>
        public delegate void ClipAnimationCallbackDelegate(ClipAnimationResult result);

        /// <summary>
        /// アニメーション切り抜き
        /// </summary>
        /// <param name="callback"></param>
        public void ClipAnimation(ClipAnimationCallbackDelegate callback)
        {
            //タスクスケジューラを取得
            TaskScheduler taskScheduler = TaskScheduler.Current;
            if (SynchronizationContext.Current == null)
            {
                //同期コンテキストが無効の場合はそのまま(=非UIスレッドからの呼び出し)
            }
            else
            {
                //同期コンテキストが有効の場合はFromCurrentSynchronizationContext()を使う(=UIスレッドからの呼び出し)
                // >> UIスレッドで呼び出しされるようにします
                taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
            }

            //呼び出し
            this.ClipAnimation(callback, taskScheduler);
        }

        /// <summary>
        /// アニメーション切り抜き
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="scheduler"></param>
        public void ClipAnimation(ClipAnimationCallbackDelegate callback, TaskScheduler scheduler)
        {
            try
            {
                //プロパティ取得
                int matrixLedWidth = this.MatrixLedWidth;
                int matrixLedHeight = this.MatrixLedHeight;
                string? originalImagePath = this.OriginalImagePath;
                BitmapSource? image = this.PreviewImage;
                AnimationMoveDirection moveDirection = this.MoveDirection;
                int moveSpeed = this.MoveSpeed;

                //コールバックを保存
                this._Callback = callback;

                //現スレッドの継続タスクでアニメーション切り抜き完了処理を行う
                // >> 完了処理を実行するスケジューラーを決定する
                TaskScheduler taskScheduler = scheduler;
                if (taskScheduler == null)
                {
                    //無効の場合はカレントを使う
                    taskScheduler = TaskScheduler.Current;
                }
                else
                {
                    //有効の場合は指定されたものを使う
                }

                //タスク準備
                Task<ClipAnimationResult> t1 = Task.Run(() => { return this.ClipAnimationTask(image, originalImagePath, matrixLedWidth, matrixLedHeight, moveDirection, moveSpeed); });
                // >> 切り抜き処理の継続タスクに登録
                Task t2 = t1.ContinueWith(this.OnClipAnimationTaskFinish, taskScheduler);
            }
            catch (Exception ex)
            {
                //例外は握りつぶす
                LOGGER.WarnEx("アニメーション切り抜きで例外発生", ex);

                //終了処理を呼ぶ
                this.OnClipAnimationTaskFinish(null);
            }
        }

        /// <summary>
        /// アニメーション切り抜きタスク
        /// </summary>
        /// <param name="image"></param>
        /// <param name="path"></param>
        /// <param name="matrixLedWidth"></param>
        /// <param name="matrixLedHeight"></param>
        /// <param name="moveDirection"></param>
        /// <param name="moveSpeed"></param>
        /// <returns>切り抜きされた画像の出力パス</returns>
        private ClipAnimationResult ClipAnimationTask(BitmapSource? image, string? path, int matrixLedWidth, int matrixLedHeight, AnimationMoveDirection moveDirection, int moveSpeed)
        {
            //出力フォルダーを生成
            string? outputFolder = null;
            try
            {
                //拡張子をのぞいたファイル名を取得
                string? fname = System.IO.Path.GetFileNameWithoutExtension(path);
                {
                    bool isNull = string.IsNullOrEmpty(fname);
                    if (isNull)
                    {
                        //無効の場合は修了
                        string msg = $"ファイル名の取得に失敗しました (path='{path}')";
                        LOGGER.Warn(msg);
                        return ClipAnimationResult.Failed(msg);
                    }
                    else
                    {
                        //取得できた場合は処理続行
                        LOGGER.Debug($"ファイル名の取得成功  (path='{path}' -> '{fname}')");
                    }
                }

                //ディレクトリ部を取得
                string? dir = System.IO.Path.GetDirectoryName(path);
                {
                    bool isNull = string.IsNullOrEmpty(dir);
                    if (isNull)
                    {
                        //無効の場合は修了
                        string msg = $"ディレクトリの取得に失敗しました (path='{path}')";
                        LOGGER.Warn(msg);
                        return ClipAnimationResult.Failed(msg);
                    }
                    else
                    {
                        //取得できた場合は処理続行
                        LOGGER.Debug($"ディレクトリの取得成功  (path='{path}' -> '{dir}')");
                    }
                }

                //出力フォルダ名を生成
                outputFolder = System.IO.Path.Combine(dir!, fname!);
                LOGGER.Debug($"出力先フォルダ ('{dir}' + '{fname}' -> '{outputFolder}')");
            }
            catch (Exception ex)
            {
                //例外は握りつぶす
                string msg = $"出力フォルダの作成で例外が発生しました (path='{path}' / outputFolder='{outputFolder}') ({ex})";
                LOGGER.WarnEx(msg, ex);
                return ClipAnimationResult.Failed(msg);
            }

            //アニメーション切り抜き
            Models.Model.ClipAnimationResult ret = this.Model.ClipAnimation(outputFolder, image, moveDirection, moveSpeed, matrixLedWidth, matrixLedHeight);
            if (ret.IsSuccess)
            {
                //成功の場合は続行
            }
            else
            {
                //失敗の場合は終了
                string msg = ret.ErrorMessage!;
                LOGGER.Warn(msg);
                return ClipAnimationResult.Failed(msg);
            }

            //出力パス一覧を取得
            List<string>? outputPaths = ret.OutputPaths;

            //ここまできたら成功
            return ClipAnimationResult.Success(outputPaths);
        }

        /// <summary>
        /// アニメーション切り抜きタスク完了
        /// </summary>
        /// <param name="task"></param>
        private void OnClipAnimationTaskFinish(Task<ClipAnimationResult>? task)
        {
            //完了コールバック取得
            ClipAnimationCallbackDelegate? callback = this._Callback;
            if (callback == null)
            {
                //コールバック無効の場合は何もできないので終了
                LOGGER.Warn("アニメーション切り抜き完了コールバックが無効です");
                return;
            }
            else
            {
                //有効の場合は続行
                LOGGER.Debug("アニメーション切り抜き完了コールバックが有効");
            }

            //タスク有効判定
            if (task == null)
            {
                //無効の場合はタスク開始できなかったので失敗で終了
                ClipAnimationResult failedResult = ClipAnimationResult.Failed("タスク開始失敗");
                callback(failedResult);
                return;
            }
            else
            {
                //有効の場合は続行
            }

            //タスク実行結果を取得
            ClipAnimationResult result = task.Result;

            //コールバック実行
            callback(result);
        }
    }
}
