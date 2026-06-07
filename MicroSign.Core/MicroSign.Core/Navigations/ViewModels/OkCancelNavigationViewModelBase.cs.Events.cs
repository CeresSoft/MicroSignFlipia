using MicroSign.Core.Navigations.Enums;
using MicroSign.Core.Navigations.Events;
using System;

namespace MicroSign.Core.Navigations.ViewModels
{
    /// <summary>
    /// OK/キャンセルナビゲーションViewModel
    /// </summary>
    partial class OkCancelNavigationViewModelBase
    {
        /// <summary>
        /// OKクリックイベント
        /// </summary>
        public event OkClickEventHandler? OkClick;

        /// <summary>
        /// OKクリックイベント発行結果
        /// </summary>
        protected struct RaiseOkClickResult
        {
            /// <summary>
            /// 処理結果
            /// </summary>
            public readonly NavigationResultKind Result;

            /// <summary>
            /// メッセージ
            /// </summary>
            public readonly string? Message;

            /// <summary>
            /// 例外
            /// </summary>
            public readonly Exception? Error;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="result"></param>
            /// <param name="message"></param>
            /// <param name="error"></param>
            private RaiseOkClickResult(NavigationResultKind result, string? message, Exception? error)
            {
                this.Result = result;
                this.Message = message;
                this.Error = error;
            }

            /// <summary>
            /// 失敗
            /// </summary>
            /// <param name="message"></param>
            /// <param name="error"></param>
            /// <returns></returns>
            public static RaiseOkClickResult Failed(string? message, Exception? error)
            {
                RaiseOkClickResult result = new RaiseOkClickResult(NavigationResultKind.Failed, message, error);
                return result;
            }

            /// <summary>
            /// 成功
            /// </summary>
            /// <returns></returns>
            public static RaiseOkClickResult Success()
            {
                RaiseOkClickResult result = new RaiseOkClickResult(NavigationResultKind.Success, null, null);
                return result;
            }

            /// <summary>
            /// キャンセル
            /// </summary>
            /// <returns></returns>
            public static RaiseOkClickResult Cancel()
            {
                RaiseOkClickResult result = new RaiseOkClickResult(NavigationResultKind.Cancel, null, null);
                return result;
            }
        }

        /// <summary>
        /// OKクリックイベント発行
        /// </summary>
        protected RaiseOkClickResult RaiseOkClick()
        {
            //ハンドラ取得
            OkClickEventHandler? handler = this.OkClick;
            if (handler == null)
            {
                //ハンドラ無効
                string msg = "OKクリックイベントハンドラ無効";
                LOGGER.Warn(msg);
                return RaiseOkClickResult.Failed(msg, null);
            }
            else
            {
                //ハンドラ有効
                LOGGER.Debug("OKクリックイベントハンドラ有効");
            }

            //イベント発行
            OkClickEventArgs arg = new OkClickEventArgs();
            try
            {
                LOGGER.Debug("OKクリックイベントハンドラ発行開始");
                handler(this, arg);
                LOGGER.Debug("OKクリックイベントハンドラ発行完了");
            }
            catch (Exception ex)
            {
                string msg = "OKクリックイベントハンドラ発行で例外発生";
                LOGGER.WarnEx(msg, ex);
                return RaiseOkClickResult.Failed(msg, ex);
            }

            //結果判定
            switch (arg.Result)
            {
                case NavigationResultKind.Success:
                    //成功の場合
                    LOGGER.Debug("OKクリックイベント成功");
                    return RaiseOkClickResult.Success();
                case NavigationResultKind.Cancel:
                    //キャンセルの場合
                    LOGGER.Debug("OKクリックイベントキャンセル");
                    return RaiseOkClickResult.Cancel();
                default:
                    //それ以外は失敗
                    LOGGER.Debug("OKクリックイベント失敗");
                    return RaiseOkClickResult.Failed(arg.Message, arg.Error);
            }
        }

        /// <summary>
        /// キャンセルクリックイベント
        /// </summary>
        public event CancelClickEventHandler? CancelClick;

        /// <summary>
        /// キャンセルクリックイベント発行
        /// </summary>
        protected void RaiseCancelClick()
        {
            //ハンドラ取得
            CancelClickEventHandler? handler = this.CancelClick;
            if (handler == null)
            {
                //ハンドラ無効
                LOGGER.Warn("キャンセルクリックイベントハンドラ無効");
                return;
            }
            else
            {
                //ハンドラ有効
                LOGGER.Debug("キャンセルクリックイベントハンドラ有効");
            }

            //イベント発行
            CancelClickEventArgs arg = new CancelClickEventArgs();
            try
            {
                LOGGER.Debug("キャンセルクリックイベントハンドラ発行開始");
                handler(this, arg);
                LOGGER.Debug("キャンセルクリックイベントハンドラ発行完了");
            }
            catch (Exception ex)
            {
                string msg = "キャンセルクリックイベントハンドラ発行で例外発生";
                LOGGER.WarnEx(msg, ex);
            }
        }
    }
}
