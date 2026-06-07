namespace MicroSign.Core.ViewModels
{
    partial class MainWindowViewModel
    {
        /// <summary>
        /// 選択アニメーション画像を取得
        /// </summary>
        /// <returns></returns>
        public AnimationImageItem? GetSelectAnimationImage()
        {
            //2026.06.05:CS)杉原:複数選択対応 >>>>> ここから
            //return this.SelectedAnimationImageItem;
            //----------
            //選択している項目の一番上を取得する
            AnimationImageItemCollection? selectedAnimationImages = this.GetSelectAnimationImages();
            int n = CommonUtils.GetCount(selectedAnimationImages);
            if (CommonConsts.Collection.Empty < n)
            {
                //選択されている場合は処理続行
            }
            else
            {
                //選択されているアニメーション画像が無い場合はnullで終了
                return null;
            }

            //先頭の項目を返す
            AnimationImageItem? result = selectedAnimationImages[CommonConsts.Index.First];
            return result;
            //2026.06.05:CS)杉原:複数選択対応 <<<<< ここまで
        }
    }
}
