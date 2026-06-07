using System.Windows;
using LOGGER = MicroSign.Core.MicroSignLogger;

#nullable disable //Variousから移植したコードなので、Null非許容警告を無効化します

namespace MicroSign.Core.Navigations
{
    partial class NavigationFrameworkElementExtention
    {
        /// <summary>
        /// キャンセル
        /// </summary>
        /// <param name="currentElement">現在のエレメント</param>
        /// <returns>
        /// 表示しているエレメントでINavigationCancelインターフェイスを実装していない場合にfalseになります
        /// キャンセル処理の結果ではないので注意してください。
        /// </returns>
        public static bool NavigationCancel(this FrameworkElement currentElement)
        {
            return NavigationFrameworkElementExtention.NavigationCancel(currentElement, null);
        }

        /// <summary>
        /// キャンセル
        /// </summary>
        /// <param name="currentElement">現在のエレメント</param>
        /// <param name="arg">呼出元に渡すパラメータ</param>
        /// <returns>
        /// 表示しているエレメントでINavigationCancelインターフェイスを実装していない場合にfalseになります
        /// キャンセル処理の結果ではないので注意してください。
        /// </returns>
        public static bool NavigationCancel(this FrameworkElement currentElement, object arg)
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
                //キャンセル処理
                bool result = navi.Cancel(arg);
                return result;
            }
        }
    }
}
