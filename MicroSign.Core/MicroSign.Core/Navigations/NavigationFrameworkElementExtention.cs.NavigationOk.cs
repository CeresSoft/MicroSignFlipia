using System.Windows;
using LOGGER = MicroSign.Core.MicroSignLogger;

#nullable disable //Variousから移植したコードなので、Null非許容警告を無効化します

namespace MicroSign.Core.Navigations
{
    partial class NavigationFrameworkElementExtention
    {
        /// <summary>
        /// OK
        /// </summary>
        /// <param name="currentElement">現在のエレメント</param>
        /// <returns>
        /// 表示しているエレメントでINavigationOkインターフェイスを実装していない場合にfalseになります
        /// Ok処理の結果ではないので注意してください。
        /// </returns>
        public static bool NavigationOK(this FrameworkElement currentElement)
        {
            return NavigationFrameworkElementExtention.NavigationOK(currentElement, null);
        }

        /// <summary>
        /// OK
        /// </summary>
        /// <param name="currentElement">現在のエレメント</param>
        /// <param name="arg">呼出元に渡すパラメータ</param>
        /// <returns>
        /// 表示しているエレメントでINavigationOkインターフェイスを実装していない場合にfalseになります
        /// Ok処理の結果ではないので注意してください。
        /// </returns>
        public static bool NavigationOK(this FrameworkElement currentElement, object arg)
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
                //OK処理
                bool result = navi.Ok(arg);
                return result;
            }
        }
    }
}
