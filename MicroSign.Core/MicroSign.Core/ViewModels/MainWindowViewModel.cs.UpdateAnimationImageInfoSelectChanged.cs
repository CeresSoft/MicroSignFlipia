namespace MicroSign.Core.ViewModels
{
    partial class MainWindowViewModel
    {
        /// <summary>
        /// アニメーション画像Itemの選択変更処理
        /// </summary>
        public void UpdateAnimationImageInfoSelectChanged()
        {
            AnimationImageItem? item = this.GetSelectAnimationImage();
            if (item == null)
            {
                //無効の場合は何もしない
            }
            else
            {
                //読込画像に設定し表示する
                this.LoadImage = item.Image;
            }
        }
    }
}
