namespace MicroSign.Core.ViewModels
{
    partial class MainWindowViewModel
    {
        /// <summary>
        /// 選択アニメーション画像に設定
        /// </summary>
        /// <param name="selectAnimationImageItem">選択するアニメーション画像</param>
        public void SetSelectAnimationImage(AnimationImageItem? selectAnimationImageItem)
        {
            AnimationImageItemCollection items = this.AnimationImages;
            int n = CommonUtils.GetCount(items);

            //選択されている項目を抽出
            AnimationImageItemCollection result = new AnimationImageItemCollection();
            for (int i = CommonConsts.Index.First; i < n; i += CommonConsts.Index.Step)
            {
                AnimationImageItem? item = items[i];
                if (item == null)
                {
                    //無効の場合は無視する
                }
                else
                {
                    //有効の場合は選択解除(=初期値にする)
                    if (item == selectAnimationImageItem)
                    {
                        //指定された項目の場合は選択にする
                        item.IsSelected = CommonUtils.Not(AnimationImageItem.InitializeValues.IsSelected);
                    }
                    else
                    {
                        //それ以外は未選択(=初期値)にする
                        item.IsSelected = AnimationImageItem.InitializeValues.IsSelected;
                    }
                }
            }
        }

    }
}
