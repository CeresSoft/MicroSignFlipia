using System.Windows;
using LOGGER = MicroSign.Core.MicroSignLogger;

#nullable disable //Variousから移植したコードなので、Null非許容警告を無効化します

namespace MicroSign.Core.Navigations
{
    partial class NavigationFrameworkElementExtention
    {
        /// <summary>
        /// オーバーラップ呼び出し
        /// </summary>
        /// <param name="currentElement">現在のエレメント</param>
        /// <param name="callElement">呼出先のエレメント</param>
        /// <remarks>現在表示しているエレメントはそのままで、新しいエレメントをオーバーラップ表示します</remarks>
        public static void NavigationOverwrap(this FrameworkElement currentElement, FrameworkElement callElement)
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
                navi.Overwrap(callElement, null, null);
            }
        }

        /// <summary>
        /// オーバーラップ呼び出し
        /// </summary>
        /// <param name="currentElement">現在のエレメント</param>
        /// <param name="callElement">呼出先のエレメント</param>
        /// <param name="arg">呼出先に渡すパラメータ</param>
        /// <remarks>現在表示しているエレメントはそのままで、新しいエレメントをオーバーラップ表示します</remarks>
        public static void NavigationOverwrap(this FrameworkElement currentElement, FrameworkElement callElement, object arg)
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
                navi.Overwrap(callElement, arg, null);
            }
        }

        /// <summary>
        /// オーバーラップ
        /// </summary>
        /// <param name="currentElement">現在のエレメント</param>
        /// <param name="callElement">呼出先のエレメント</param>
        /// <param name="arg">呼出先に渡すパラメータ</param>
        /// <param name="returnCallback">戻り時に呼び出すコールバック</param>
        /// <remarks>現在表示しているエレメントはそのままで、新しいエレメントをオーバーラップ表示します</remarks>
        public static void NavigationOverwrap(this FrameworkElement currentElement, FrameworkElement callElement, object arg, System.Action<object> returnCallback)
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
                //オーバーラップ処理
                navi.Overwrap(callElement, arg, returnCallback);
            }
        }

        /// <summary>
        /// オーバーラップ
        /// </summary>
        /// <param name="currentElement">現在のエレメント</param>
        /// <param name="callElement">呼出先のエレメント</param>
        /// <param name="returnCallback">戻り時に呼び出すコールバック</param>
        /// <remarks>現在表示しているエレメントはそのままで、新しいエレメントをオーバーラップ表示します</remarks>
        public static void NavigationOverwrap(this FrameworkElement currentElement, FrameworkElement callElement, System.Action<object> returnCallback)
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
                //オーバーラップ処理
                navi.Overwrap(callElement, null, returnCallback);
            }
        }
    }
}
