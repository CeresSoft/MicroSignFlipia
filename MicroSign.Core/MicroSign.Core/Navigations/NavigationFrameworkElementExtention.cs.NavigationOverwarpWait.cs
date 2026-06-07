using System.Windows;
using LOGGER = MicroSign.Core.MicroSignLogger;

#nullable disable //Variousから移植したコードなので、Null非許容警告を無効化します

namespace MicroSign.Core.Navigations
{
    partial class NavigationFrameworkElementExtention
    {
        /// <summary>
        /// オーバーラップ呼出して完了を待つ
        /// </summary>
        /// <param name="currentElement">現在のエレメント</param>
        /// <param name="callElement">呼出先のエレメント</param>
        /// <returns>NavigationReturnで渡された値</returns>
        /// <remarks>現在表示しているエレメントはそのままで、新しいエレメントをオーバーラップ表示します</remarks>
        public static object NavigationOverwrapWait(this FrameworkElement currentElement, FrameworkElement callElement)
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
                navi.Overwrap(callElement, null, fin.ReturnCallback);

                //完了するまで待つ
                fin.Wait();

                //戻り値を返す
                return fin.GetResult();
            }
        }

        /// <summary>
        /// オーバーラップ呼出して完了を待つ
        /// </summary>
        /// <param name="currentElement">現在のエレメント</param>
        /// <param name="callElement">呼出先のエレメント</param>
        /// <param name="arg">呼出先に渡すパラメータ</param>
        /// <returns>NavigationReturnで渡された値</returns>
        /// <remarks>現在表示しているエレメントはそのままで、新しいエレメントをオーバーラップ表示します</remarks>
        public static object NavigationOverwrapWait(this FrameworkElement currentElement, FrameworkElement callElement, object arg)
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
                navi.Overwrap(callElement, arg, fin.ReturnCallback);

                //完了するまで待つ
                fin.Wait();

                //戻り値を返す
                return fin.GetResult();
            }
        }

        /// <summary>
        /// オーバーラップ呼出して完了を待つ
        /// </summary>
        /// <param name="currentElement">現在のエレメント</param>
        /// <param name="callElement">呼出先のエレメント</param>
        /// <param name="arg">呼出先に渡すパラメータ</param>
        /// <param name="returnCallback">戻り時に呼び出すコールバック</param>
        /// <returns>NavigationReturnで渡された値</returns>
        /// <remarks>現在表示しているエレメントはそのままで、新しいエレメントをオーバーラップ表示します</remarks>
        public static object NavigationOverwrapWait(this FrameworkElement currentElement, FrameworkElement callElement, object arg, System.Action<object> returnCallback)
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
                //オーバーラップ処理
                NavigationFinishWait fin = new NavigationFinishWait(navi, returnCallback);
                navi.Overwrap(callElement, arg, fin.ReturnCallback);

                //完了するまで待つ
                fin.Wait();

                //戻り値を返す
                return fin.GetResult();
            }
        }

        /// <summary>
        /// オーバーラップ呼出して完了を待つ
        /// </summary>
        /// <param name="currentElement">現在のエレメント</param>
        /// <param name="callElement">呼出先のエレメント</param>
        /// <param name="returnCallback">戻り時に呼び出すコールバック</param>
        /// <returns>NavigationReturnで渡された値</returns>
        /// <remarks>現在表示しているエレメントはそのままで、新しいエレメントをオーバーラップ表示します</remarks>
        public static object NavigationOverwrapWait(this FrameworkElement currentElement, FrameworkElement callElement, System.Action<object> returnCallback)
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
                //オーバーラップ処理
                NavigationFinishWait fin = new NavigationFinishWait(navi, returnCallback);
                navi.Overwrap(callElement, null, fin.ReturnCallback);

                //完了するまで待つ
                fin.Wait();

                //戻り値を返す
                return fin.GetResult();
            }
        }
    }
}
