using System;
using System.Windows.Threading;
using LOGGER = MicroSign.Core.MicroSignLogger;

#nullable disable //Variousから移植したコードなので、Null非許容警告を無効化します

namespace MicroSign.Core.Navigations
{
    public class NavigationFinishWait
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
        /// 完了フラグ
        /// </summary>
        private volatile bool _IsFinish = false;

        /// <summary>
        /// 対象のナビゲーションコントローラー
        /// </summary>
        private volatile NavigationController _Navi;

        /// <summary>
        /// 終了コールバック
        /// </summary>
        private volatile System.Action<object> _ReturnCallback = null;

        /// <summary>
        /// 結果
        /// </summary>
        private volatile object _Result = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="navi">ナビゲーションコントローラー</param>
        public NavigationFinishWait(NavigationController navi) : this(navi, null)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="navi">ナビゲーションコントローラー</param>
        /// <param name="returnCallback">コールバック</param>
        public NavigationFinishWait(NavigationController navi, System.Action<object> returnCallback)
        {
            this._Navi = navi;
            this._ReturnCallback = returnCallback;
        }

        /// <summary>
        /// ループ判定
        /// </summary>
        /// <returns></returns>
        private bool IsLoop()
        {
            //完了フラグ
            {
                bool isFinish = this._IsFinish;
                if(isFinish)
                {
                    //完了している場合はループしない
                    return false;
                }
                else
                {
                    //完了していない場合は処理続行
                }
            }

            //ウインドウクローズ判定
            {
                NavigationController navi = this._Navi;
                if(navi == null)
                {
                    //無効の場合は処理続行
                }
                else
                {
                    //有効の場合はウインドウクローズフラグを確認
                    bool isClosed = navi.IsWindowClosed;
                    if (isClosed)
                    {
                        //クローズしている場合はループしない
                        return false;
                    }
                    else
                    {
                        //クローズしていない場合は処理続行
                    }
                }
            }

            //ここまで来たらループするで終了
            return true;
        }

        /// <summary>
        /// 待ち
        /// </summary>
        public void Wait()
        {
            bool isLoop = this.IsLoop();

            while (isLoop)
            {
                //高速でループしないよう少し待つ
                System.Threading.Thread.Sleep(CommonConsts.Intervals.Zero);

                //DoEvent
                try
                {
                    this.DoEvents();
                }
                catch (Exception ex)
                {
                    //例外は握りつぶす
                    LOGGER.WarnEx("DoEventで例外発生", ex);
                }

                //ループ判定
                isLoop = this.IsLoop();
            }
        }

        /// <summary>
        /// 戻りコールバック処理
        /// </summary>
        /// <param name="o"></param>
        public void ReturnCallback(object o)
        {
            System.Action<object> returnCallback = this._ReturnCallback;
            if (returnCallback == null)
            {
                //コールバックが無効の場合は何もしない
            }
            else
            {
                //コールバックが有効の場合は呼び出す
                try
                {
                    returnCallback(o);
                }
                catch (Exception ex)
                {
                    //例外は握りつぶす
                    LOGGER.WarnEx("戻りコールバック呼出で例外発生", ex);
                }
            }

            //結果を設定
            this._Result = o;


            //終了を発行
            this._IsFinish = true;
        }

        /// <summary>
        /// 戻り値を取得
        /// </summary>
        /// <returns></returns>
        public object GetResult()
        {
            object result = this._Result;
            return result;
        }

        /// <summary>
        /// 禁断のDoEvent
        /// </summary>
        /// <remarks>
        /// [MSDN]Dispatcher.PushFrame(DispatcherFrame)
        /// https://learn.microsoft.com/en-us/dotnet/api/system.windows.threading.dispatcher.pushframe?view=windowsdesktop-9.0&redirectedfrom=MSDN#System_Windows_Threading_Dispatcher_PushFrame_System_Windows_Threading_DispatcherFrame_
        /// </remarks>
        private void DoEvents()
        {
            DispatcherFrame frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
                new DispatcherOperationCallback(NavigationFinishWait.ExitFrame), frame);
            Dispatcher.PushFrame(frame);
        }

        /// <summary>
        /// ExitFrame
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        /// <remarks>
        /// [MSDN]Dispatcher.PushFrame(DispatcherFrame)
        /// https://learn.microsoft.com/en-us/dotnet/api/system.windows.threading.dispatcher.pushframe?view=windowsdesktop-9.0&redirectedfrom=MSDN#System_Windows_Threading_Dispatcher_PushFrame_System_Windows_Threading_DispatcherFrame_
        /// </remarks>
        private static object ExitFrame(object f)
        {
            ((DispatcherFrame)f).Continue = false;

            return null;
        }

    }
}
