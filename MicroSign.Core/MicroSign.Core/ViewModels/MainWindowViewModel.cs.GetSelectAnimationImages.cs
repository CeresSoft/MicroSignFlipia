namespace MicroSign.Core.ViewModels
{
    partial class MainWindowViewModel
    {
        /// <summary>
        /// 複数選択アニメーション画像を取得
        /// </summary>
        /// <returns></returns>
        public AnimationImageItemCollection GetSelectAnimationImages()
        {
            AnimationImageItemCollection items = this.AnimationImages;
            int n = CommonUtils.GetCount(items);

            //選択されている項目を抽出
            AnimationImageItemCollection result = new AnimationImageItemCollection();
            for (int i = CommonConsts.Index.First; i < n; i+= CommonConsts.Index.Step)
            {
                AnimationImageItem? item = items[i];
                if(item == null)
                {
                    //無効の場合は無視する
                }
                else
                {
                    //有効の場合は選択判定
                    bool isSelect = item.IsSelected;
                    if(isSelect)
                    {
                        //選択していたらリストに追加
                        result.Add(item);
                    }
                    else
                    {
                        //選択されていない場合は無視して処理続行
                    }
                }
            }

            //終了
            return result;
        }
    }
}
