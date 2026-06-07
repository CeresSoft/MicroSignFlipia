using System;
using System.Threading;
using LOGGER = MicroSign.Core.CommonLogger;

namespace MicroSign.Core.ViewModels
{
    partial class MainWindowViewModel
    {
        /// <summary>
        /// アニメーション停止
        /// </summary>
        public void StopAnimation()
        {
            try
            {
                //キャンセルトークン取得
                CancellationTokenSource? cancel = this._AnimationCancel;
                if (cancel == null)
                {
                    //無効の場合は無視する
                    LOGGER.Warn("アニメーションタスク未実行");
                }
                else
                {
                    //キャンセル発行
                    LOGGER.Info("アニメーションタスク停止指示");
                    cancel.Cancel();
                }
            }
            catch(Exception ex)
            {
                LOGGER.Warn("アニメーションタスク終了で例外発生", ex);
            }

        }
    }
}
