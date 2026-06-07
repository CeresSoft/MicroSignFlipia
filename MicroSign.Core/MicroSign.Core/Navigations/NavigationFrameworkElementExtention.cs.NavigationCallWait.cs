using System.Windows;
using LOGGER = MicroSign.Core.MicroSignLogger;

#nullable disable //Variousから移植したコードなので、Null非許容警告を無効化します

namespace MicroSign.Core.Navigations
{
    partial class NavigationFrameworkElementExtention
    {
        /// <summary>
        /// 呼出して完了を待つ
        /// </summary>
        /// <param name="currentElement">現在のエレメント</param>
        /// <param name="callElement">呼出先のエレメント</param>
        /// <returns>NavigationReturnで渡された値</returns>
        /// <remarks>現在表示しているエレメントを非表示時にし、新しいエレメントを表示します</remarks>
        public static object NavigationCallWait(this FrameworkElement currentElement, FrameworkElement callElement)
        {
            //現在のエレメントからナビゲーションコントローラーを取得
            NavigationController navi = NavigationController.GetNavigationController(currentElement);
            if (navi == null)
            {
                //取得できない場合は何もしない
                LOGGER.Warn("ナビゲーションコントローラーが無効です");
                return false;
            }
            else
            {
                //呼出処理
                NavigationFinishWait fin = new NavigationFinishWait(navi);
                navi.Call(callElement, null, fin.ReturnCallback);

                //完了するまで待つ
                fin.Wait();

                //戻り値を返す
                return fin.GetResult();
            }
        }

        /// <summary>
        /// 呼出して完了を待つ
        /// </summary>
        /// <param name="currentElement">現在のエレメント</param>
        /// <param name="callElement">呼出先のエレメント</param>
        /// <param name="arg">呼出先に渡すパラメータ</param>
        /// <returns>NavigationReturnで渡された値</returns>
        /// <remarks>現在表示しているエレメントを非表示時にし、新しいエレメントを表示します</remarks>
        public static object NavigationCallWait(this FrameworkElement currentElement, FrameworkElement callElement, object arg)
        {
            //現在のエレメントからナビゲーションコントローラーを取得
            NavigationController navi = NavigationController.GetNavigationController(currentElement);
            if (navi == null)
            {
                //取得できない場合は何もしない
                LOGGER.Warn("ナビゲーションコントローラーが無効です");
                return null;
            }
            else
            {
                //呼出処理
                NavigationFinishWait fin = new NavigationFinishWait(navi);
                navi.Call(callElement, arg, fin.ReturnCallback);

                //完了するまで待つ
                fin.Wait();

                //戻り値を返す
                return fin.GetResult();
            }
        }

        /// <summary>
        /// 呼出して完了を待つ
        /// </summary>
        /// <param name="currentElement">現在のエレメント</param>
        /// <param name="callElement">呼出先のエレメント</param>
        /// <param name="arg">呼出先に渡すパラメータ</param>
        /// <param name="returnCallback">戻り時に呼び出すコールバック</param>
        /// <returns>NavigationReturnで渡された値</returns>
        /// <remarks>現在表示しているエレメントを非表示時にし、新しいエレメントを表示します</remarks>
        public static object NavigationCallWait(this FrameworkElement currentElement, FrameworkElement callElement, object arg, System.Action<object> returnCallback)
        {
            //現在のエレメントからナビゲーションコントローラーを取得
            NavigationController navi = NavigationController.GetNavigationController(currentElement);
            if (navi == null)
            {
                //取得できない場合は何もしない
                LOGGER.Warn("ナビゲーションコントローラーが無効です");
                return null;
            }
            else
            {
                //UIスレッドで呼出処理
                NavigationFinishWait fin = new NavigationFinishWait(navi, returnCallback);
                navi.Call(callElement, arg, fin.ReturnCallback);

                //完了するまで待つ
                fin.Wait();

                //戻り値を返す
                return fin.GetResult();
            }
        }

        /// <summary>
        /// 呼出して完了を待つ
        /// </summary>
        /// <param name="currentElement">現在のエレメント</param>
        /// <param name="callElement">呼出先のエレメント</param>
        /// <param name="returnCallback">戻り時に呼び出すコールバック</param>
        /// <returns>NavigationReturnで渡された値</returns>
        /// <remarks>現在表示しているエレメントを非表示時にし、新しいエレメントを表示します</remarks>
        public static object NavigationCallWait(this FrameworkElement currentElement, FrameworkElement callElement, System.Action<object> returnCallback)
        {
            //現在のエレメントからナビゲーションコントローラーを取得
            NavigationController navi = NavigationController.GetNavigationController(currentElement);
            if (navi == null)
            {
                //取得できない場合は何もしない
                LOGGER.Warn("ナビゲーションコントローラーが無効です");
                return null;
            }
            else
            {
                //UIスレッドで呼出処理
                NavigationFinishWait fin = new NavigationFinishWait(navi, returnCallback);
                navi.Call(callElement, null, fin.ReturnCallback);

                //完了するまで待つ
                fin.Wait();

                //戻り値を返す
                return fin.GetResult();
            }
        }
    }
}
