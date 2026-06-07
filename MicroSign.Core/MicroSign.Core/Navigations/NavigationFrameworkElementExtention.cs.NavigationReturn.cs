using System.Windows;
using LOGGER = MicroSign.Core.MicroSignLogger;

#nullable disable //Variousから移植したコードなので、Null非許容警告を無効化します

namespace MicroSign.Core.Navigations
{
    partial class NavigationFrameworkElementExtention
    {
        /// <summary>
        /// 戻り
        /// </summary>
        /// <param name="currentElement">現在のエレメント</param>
        public static void NavigationReturn(this FrameworkElement currentElement)
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
                //戻り処理
                navi.Return(null);
            }
        }

        /// <summary>
        /// 戻り
        /// </summary>
        /// <param name="arg">呼出元に渡すパラメータ</param>
        /// <param name="currentElement">現在のエレメント</param>
        public static void NavigationReturn(this FrameworkElement currentElement, object arg)
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
                //戻り処理
                navi.Return(arg);
            }
        }
    }
}
