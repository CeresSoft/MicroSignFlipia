using System;
using System.Windows.Input;

namespace MicroSign.Core.ViewModels
{
    /// <summary>
    /// デリゲートコマンド
    /// </summary>
    public partial class DelegateCommand : ICommand
    {
        //2026.06.07:CS)杉原:LOGGERを修正 >>>>> ここから
        ///// <summary>
        ///// LOG4NETのロガー
        ///// </summary>
        //private static readonly log4net.ILog LOGGER = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType!);
        //----------
        /// <summary>
        /// LOG4NETのロガー
        /// </summary>
        private static readonly MicroSignLogger LOGGER = MicroSignLogger.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType!);
        //2026.06.07:CS)杉原:LOGGERを修正 <<<<< ここまで

        /// <summary>
        /// CanExecute
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object? parameter)
        {
            //実行可能判定アクション取得
            DelegateCommandCanExecuteDelegate? canExecuteAction = this._CanExecuteAction;
            if(canExecuteAction == null)
            {
                //実行可能判定アクションが無い場合は必ずTRUE(=実行可能)を返す
                return true;
            }
            else
            {
                //実行可能判定アクションが有る場合
                try
                {
                    //実行可能判定
                    bool result = canExecuteAction(parameter);
                    return result;
                }
                catch (Exception ex)
                {
                    LOGGER.WarnEx("デリゲートコマンドCanExecuteで例外発生", ex);
                    //実行可能判定アクションが失敗した場合は必ずTRUE(=実行可能)を返す
                    return true;
                }
            }
        }

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object? parameter)
        {
            //実行アクションを取得
            DelegateCommandExecuteDelegate? executeAction = this._ExecuteAction;
            if(executeAction == null)
            {
                //実行アクションが取得できない場合何もできないので終了
                return;
            }
            try
            {
                //実行
                executeAction(parameter);
            }
            catch (Exception ex)
            {
                LOGGER.WarnEx("デリゲートコマンドExecuteで例外発生", ex);
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="executeAction"></param>
        public DelegateCommand(DelegateCommandExecuteDelegate? executeAction)
        {
            this._ExecuteAction = executeAction;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="executeAction"></param>
        /// <param name="canExecuteAction"></param>
        public DelegateCommand(DelegateCommandExecuteDelegate? executeAction, DelegateCommandCanExecuteDelegate? canExecuteAction)
        {
            this._ExecuteAction = executeAction;
            this._CanExecuteAction = canExecuteAction;
        }

        /// <summary>
        /// 実行許可更新
        /// </summary>
        /// <param name="cmd"></param>
        public static void UpdateCanExecute(ICommand cmd)
        {
            //DelegateCommand取得
            DelegateCommand? dcmd = cmd as DelegateCommand;
            if(dcmd == null)
            {
                //取得失敗の場合何もしない
            }
            else
            {
                //取得できた場合CanExecuteChangeイベント発行
                dcmd.RaiseCanExecuteChanged();
            }
        }
    }
}
