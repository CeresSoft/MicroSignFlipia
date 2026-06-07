using System.Windows;
using LOGGER = MicroSign.Core.MicroSignLogger;

#nullable disable //Variousから移植したコードなので、Null非許容警告を無効化します

namespace MicroSign.Core.Navigations
{
    partial class NavigationFrameworkElementExtention
    {
        /// <summary>
        /// 遷移
        /// </summary>
        /// <param name="currentElement">現在のエレメント</param>
        /// <param name="moveElement">遷移先のエレメント</param>
        public static void NavigationMove(this FrameworkElement currentElement, FrameworkElement moveElement)
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
                //遷移処理
                navi.Move(moveElement, null);
            }
        }

        /// <summary>
        /// 遷移
        /// </summary>
        /// <param name="currentElement">現在のエレメント</param>
        /// <param name="moveElement">遷移先のエレメント</param>
        /// <param name="arg">遷移先に渡すパラメータ</param>
        public static void NavigationMove(this FrameworkElement currentElement, FrameworkElement moveElement, object arg)
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
                //遷移処理
                navi.Move(moveElement, arg);
            }
        }
    }
}
