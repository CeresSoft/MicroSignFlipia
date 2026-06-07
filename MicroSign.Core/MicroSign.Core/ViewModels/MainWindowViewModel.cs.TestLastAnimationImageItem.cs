namespace MicroSign.Core.ViewModels
{
    partial class MainWindowViewModel
    {
        /// <summary>
        /// 最後のアニメーション画像か判定結果
        /// </summary>
        public enum TestLastAnimationImageResult
        {
            /// <summary>
            /// 最後
            /// </summary>
            Last,

            /// <summary>
            /// 最後以外
            /// </summary>
            NoLast,

            /// <summary>
            /// アニメーション画像アイテムが無効
            /// </summary>
            InvalidItem,

            /// <summary>
            /// コレクションなし
            /// </summary>
            NoCollection,
        }

        /// <summary>
        /// 先頭のアニメーション画像か判定
        /// </summary>
        /// <param name="item"></param>
        public TestLastAnimationImageResult TestLastAnimationImageItem(AnimationImageItem? item)
        {
            //指定されたアニメーション画像が有効か判定
            if(item == null)
            {
                //無効の場合は先頭ではないで終了
                return TestLastAnimationImageResult.InvalidItem;
            }
            else
            {
                //有効の場合は
            }


            //アニメーション画像コレクション
            // >> コンストラクタで生成しているのでnullチェック不要
            AnimationImageItemCollection animationImages = this.AnimationImages;

            int n= CommonUtils.GetCount(animationImages);
            if(CommonConsts.Collection.Empty < n)
            {
                //要素がある場合は処理続行
            }
            else
            {
                //要素が無い場合はコレクションなしを返す
                return TestLastAnimationImageResult.NoCollection;
            }


            //最後の要素を取得
            // >> nullでもよい(itemはnull以外なので判定がfalseとなる)
            int i = CommonConsts.Collection.ToIndex(n);
            AnimationImageItem? firstItem = animationImages[i];
            if(item == firstItem)
            {
                //一致した場合先頭を返す
                return TestLastAnimationImageResult.Last;
            }
            else
            {
                //一致しない場合は先頭以外を返す
                return TestLastAnimationImageResult.NoLast;
            }
        }

    }
}
