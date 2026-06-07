namespace MicroSign.Core.ViewModels
{
    partial class MainWindowViewModel
    {
        /// <summary>
        /// アニメーション画像アイテムを追加
        /// </summary>
        /// <param name="animationImageItem"></param>
        public void AddAnimationImage(AnimationImageItem animationImageItem)
        {
            //追加するアニメーション画像アイテムの有効判定
            if(animationImageItem == null)
            {
                //無効の場合は終了
                return;
            }
            else
            {
                //有効の場合は処理続行
            }

            //アニメーション画像アイテムコレクション取得
            // >> コンストラクタで生成しているのでnullチェック不要
            AnimationImageItemCollection anims = this.AnimationImages;

            //選択されているアニメーション画像アイテムを取得
            int selectedIndex = CommonConsts.Index.Invalid;
            {
                AnimationImageItem? selectAmimationItem = this.GetSelectAnimationImage();
                if (selectAmimationItem == null)
                {
                    //無効の場合は何もしない
                }
                else
                {
                    //有効の場合はインデックスを取得
                    selectedIndex = this.GetAnimationImageIndex(selectAmimationItem);
                }
            }

            //追加
            if(selectedIndex < CommonConsts.Index.First)
            {
                //インデックスが無効の場合は最後に追加
                anims.Add(animationImageItem);
            }
            else
            {
                //有効の場合はインサート
                // >> 追加するのは現在選択されているアニメーション画像の次にする
                int index = selectedIndex + CommonConsts.Index.Step;
                // >> 追加
                anims.Insert(index, animationImageItem);
            }

            //追加したアニメーション画像アイテムを選択する
            this.SetSelectAnimationImage(animationImageItem);
        }
    }
}
