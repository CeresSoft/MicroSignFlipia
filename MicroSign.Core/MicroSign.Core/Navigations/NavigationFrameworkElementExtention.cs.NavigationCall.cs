using System.Windows;

#nullable disable //Variousから移植したコードなので、Null非許容警告を無効化します

namespace MicroSign.Core.Navigations
{
    partial class NavigationFrameworkElementExtention
    {
        /// <summary>
        /// 呼出
        /// </summary>
        /// <param name="currentElement">現在のエレメント</param>
        /// <param name="callElement">呼出先のエレメント</param>
        /// <remarks>現在表示しているエレメントを非表示時にし、新しいエレメントを表示します</remarks>
        public static void NavigationCall(this FrameworkElement currentElement, FrameworkElement callElement)
        {
            //現在のエレメントからナビゲーションコントローラーを取得
            NavigationController navi = NavigationController.GetNavigationController(currentElement);
            if (navi == null)
            {
                //取得できない場合は何もしない
                LOGGER.Warn("ナビゲーションコントローラーが無効です");
                return;
            }
            else
            {
                //呼出処理
                navi.Call(callElement, null, null);
            }
        }

        /// <summary>
        /// 呼出
        /// </summary>
        /// <param name="currentElement">現在のエレメント</param>
        /// <param name="callElement">呼出先のエレメント</param>
        /// <param name="arg">呼出先に渡すパラメータ</param>
        /// <remarks>現在表示しているエレメントを非表示時にし、新しいエレメントを表示します</remarks>
        public static void NavigationCall(this FrameworkElement currentElement, FrameworkElement callElement, object arg)
        {
            //現在のエレメントからナビゲーションコントローラーを取得
            NavigationController navi = NavigationController.GetNavigationController(currentElement);
            if (navi == null)
            {
                //取得できない場合は何もしない
                LOGGER.Warn("ナビゲーションコントローラーが無効です");
                return;
            }
            else
            {
                //呼出処理
                navi.Call(callElement, arg, null);
            }
        }

        /// <summary>
        /// 呼出
        /// </summary>
        /// <param name="currentElement">現在のエレメント</param>
        /// <param name="callElement">呼出先のエレメント</param>
        /// <param name="arg">呼出先に渡すパラメータ</param>
        /// <param name="returnCallback">戻り時に呼び出すコールバック</param>
        /// <remarks>現在表示しているエレメントを非表示時にし、新しいエレメントを表示します</remarks>
        public static void NavigationCall(this FrameworkElement currentElement, FrameworkElement callElement, object arg, System.Action<object> returnCallback)
        {
            //現在のエレメントからナビゲーションコントローラーを取得
            NavigationController navi = NavigationController.GetNavigationController(currentElement);
            if (navi == null)
            {
                //取得できない場合は何もしない
                LOGGER.Warn("ナビゲーションコントローラーが無効です");
                return;
            }
            else
            {
                //呼出処理
                navi.Call(callElement, arg, returnCallback);
            }
        }

        /// <summary>
        /// 呼出
        /// </summary>
        /// <param name="currentElement">現在のエレメント</param>
        /// <param name="callElement">呼出先のエレメント</param>
        /// <param name="returnCallback">戻り時に呼び出すコールバック</param>
        /// <remarks>現在表示しているエレメントを非表示時にし、新しいエレメントを表示します</remarks>
        public static void NavigationCall(this FrameworkElement currentElement, FrameworkElement callElement, System.Action<object> returnCallback)
        {
            //現在のエレメントからナビゲーションコントローラーを取得
            NavigationController navi = NavigationController.GetNavigationController(currentElement);
            if (navi == null)
            {
                //取得できない場合は何もしない
                LOGGER.Warn("ナビゲーションコントローラーが無効です");
                return;
            }
            else
            {
                //呼出処理
                navi.Call(callElement, null, returnCallback);
            }
        }
    }
}
