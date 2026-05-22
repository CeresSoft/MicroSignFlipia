namespace MicroSign.Core.ViewModels
{
    partial class MainWindowViewModel
    {
        /// <summary>
        /// アニメーション設定保存
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Models.Model.SaveAnimationResult SaveAnimation(string path)
        {
            //2026.05.22:CS)杉原:サウンド機能追加 >>>>> ここから
            //return this.Model.SaveAnimation(path, this.AnimationName, this.AnimationImages, this.MatrixLedWidth, this.MatrixLedHeight, this.MatrixLedBrightness, this.DefaultDisplayPeriod);
            //-----
            return this.Model.SaveAnimation(path, this.AnimationName, this.AnimationImages, this.MatrixLedWidth, this.MatrixLedHeight, this.MatrixLedBrightness, this.DefaultDisplayPeriod, this.SoundFilePath);
            //2026.05.22:CS)杉原:サウンド機能追加 <<<<< ここまで
        }
    }
}
