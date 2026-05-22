namespace MicroSign.Core.ViewModels
{
    partial class MainWindowViewModel
    {
        /// <summary>
        /// サウンドファイルパス設定
        /// </summary>
        public void ClearSoundFilePath()
        {
            this.SoundFilePath = MainWindowViewModel.InitializeValues.SoundFilePath;
        }
    }
}
