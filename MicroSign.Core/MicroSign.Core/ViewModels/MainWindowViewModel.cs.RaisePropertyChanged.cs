namespace MicroSign.Core.ViewModels
{
    partial class MainWindowViewModel
    {
        /// <summary>
        /// PropertyChangedイベント発生
        /// </summary>
        /// <param name="propertyName"></param>
        protected override void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            //デフォルト処理の呼び出し
            base.RaisePropertyChanged(propertyName);

            //追加処理
            switch(propertyName)
            {
                case MainWindowViewModel.PropertyNames.SelectedAnimationImageItem:
                    //アニメーション画像の選択が変化したら画面に反映
                    {
                        AnimationImageItem? item = this.SelectedAnimationImageItem;
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
                    break;

                case MainWindowViewModel.PropertyNames.SoundFilePath:
                    //サウンドファイル」パス変更の場合
                    this.OnSoundFilePathChanged();
                    break;


                default:
                    //それ以外の場合は何もしない
                    break;
            }
        }
    }
}
