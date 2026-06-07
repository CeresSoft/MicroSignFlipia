namespace MicroSign.Core.ViewModels
{
    partial class MainWindowViewModel
    {
        /// <summary>
        /// 先頭のアニメーション画像か判定結果
        /// </summary>
        public enum TestFirstAnimationImageResult
        {
            /// <summary>
            /// 先頭
            /// </summary>
            First,

            /// <summary>
            /// 先頭以外
            /// </summary>
            NoFirst,

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
        public TestFirstAnimationImageResult TestFirstAnimationImageItem(AnimationImageItem? item)
        {
            //指定されたアニメーション画像が有効か判定
            if(item == null)
            {
                //無効の場合は先頭ではないで終了
                return TestFirstAnimationImageResult.InvalidItem;
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
                return TestFirstAnimationImageResult.NoCollection;
            }


            //先頭の要素を取得
            // >> nullでもよい(itemはnull以外なので判定がfalseとなる)
            AnimationImageItem? firstItem = animationImages[CommonConsts.Index.First];
            if(item == firstItem)
            {
                //一致した場合先頭を返す
                return TestFirstAnimationImageResult.First;
            }
            else
            {
                //一致しない場合は先頭以外を返す
                return TestFirstAnimationImageResult.NoFirst;
            }
        }

    }
}
