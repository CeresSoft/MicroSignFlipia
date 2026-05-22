using MicroSign.Core.ViewModels;

namespace MicroSign.Core.Models.AnimationSaveSettings
{
    /// <summary>
    /// 保存用アニメーション設定
    /// </summary>
    public class AnimationSaveSetting
    {
        /// <summary>
        /// アニメーション名
        /// </summary>
        public string Name { get; set; } = "(no name)";

        /// <summary>
        /// マトリクスLED横幅
        /// </summary>
        public int MatrixLedWidth { get; set; } = MainWindowViewModel.InitializeValues.MatrixLedWidth;

        /// <summary>
        /// マトリクスLED縦幅
        /// </summary>
        public int MatrixLedHeight { get; set; } = MainWindowViewModel.InitializeValues.MatrixLedHeight;

        /// <summary>
        /// マトリクスLED明るさ
        /// </summary>
        public int MatrixLedBrightness { get; set; } = MainWindowViewModel.InitializeValues.MatrixLedBrightness;

        /// <summary>
        /// デフォルト表示期間(秒)
        /// </summary>
        public double DefaultDisplayPeriod { get; set; } = MainWindowViewModel.InitializeValues.DefaultDisplayPeriod;

        /// <summary>
        /// サウンドファイルパス
        /// </summary>
        /// <remarks>
        /// 2026.05.22:CS)杉原:サウンド機能追加で追加
        /// </remarks>
        public string? SoundFilePath { get; set; } = MainWindowViewModel.InitializeValues.SoundFilePath;

        /// <summary>
        ///  保存用アニメーションデータコレクション
        /// </summary>
        public AnimationSaveDataCollection AnimationDatas { get; set; } = new AnimationSaveDataCollection();
    }
}
