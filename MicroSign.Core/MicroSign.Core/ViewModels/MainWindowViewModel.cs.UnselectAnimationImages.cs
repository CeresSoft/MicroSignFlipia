namespace MicroSign.Core.ViewModels
{
    partial class MainWindowViewModel
    {
        /// <summary>
        /// 全アニメーション画像の選択を解除
        /// </summary>
        /// <returns></returns>
        public void UnselectAnimationImages()
        {
            //項目なしで選択アニメーション画像に設定を呼び出す
            this.SetSelectAnimationImage(null);
        }
    }
}
