using MicroSign.Core.ViewModels;

namespace MicroSign.Core.Models
{
    partial class Model
    {
        //2025.08.12:CS)杉原:パレット処理の流れを変更 >>>>> ここから
        ///// <summary>
        ///// 変換
        ///// </summary>
        ///// <param name="name">クラス名</param>
        ///// <param name="formatKind">フォーマット種類</param>
        ///// <param name="animationImages">アニメーション画像</param>
        ///// <param name="redThreshold">赤閾値</param>
        ///// <param name="greenThreshold">緑閾値</param>
        ///// <param name="blueThreshold">青閾値</param>
        ///// <param name="matrixLedWidth">マトリクスLED横ドット数</param>
        ///// <param name="matrixLedHeight">マトリクスLED縦ドット数</param>
        ///// <param name="matrixLedBrightness">マトリクスLED明るさ</param>
        ///// <returns></returns>
        //public ConvertResult ConvertAnimation(string? name, FormatKinds formatKind, AnimationImageItemCollection animationImages, int redThreshold, int greenThreshold, int blueThreshold, int matrixLedWidth, int matrixLedHeight, int matrixLedBrightness)
        //----------
        /// <summary>
        /// 変換
        /// </summary>
        /// <param name="formatKind">フォーマット種類</param>
        /// <param name="animationImages">アニメーション画像</param>
        /// <param name="matrixLedWidth">マトリクスLED横ドット数</param>
        /// <param name="matrixLedHeight">マトリクスLED縦ドット数</param>
        /// <param name="matrixLedBrightness">マトリクスLED明るさ</param>
        /// <param name="gamma">ガンマ値(2025.08.18:CS)土田:ガンマ補正対応で追加)</param>
        /// <param name="motionBlurReduction">残像軽減(2025.08.21:CS)土田:残像軽減対応で追加)</param>
        /// <param name="savePath">保存先(2025.10.03:CS)土田:変換結果の保存先を選択できるように引数追加)</param>
        /// <param name="pcm8">PCM(=16ビット)の上位8bitだけにしたサウンドデータ(2026.05.22:CS)杉原:サウンド機能追加)</param>
        /// <returns></returns>
        /// <remarks>
        /// 2025.08.12:CS)杉原:パレット処理の流れを変更で不要なパラメータを削除
        /// </remarks>
        public ConvertResult ConvertAnimation(FormatKinds formatKind, AnimationImageItemCollection animationImages, int matrixLedWidth, int matrixLedHeight, int matrixLedBrightness, double gamma, int motionBlurReduction, string savePath, byte[]? pcm8)
        //2025.08.12:CS)杉原:パレット処理の流れを変更 <<<<< ここまで
        {
            switch (formatKind)
            {
                case FormatKinds.HighSpeed:
                case FormatKinds.TestColor64:
                case FormatKinds.TestColor256:
                    return ConvertResult.Failed($"アニメーション画像に変換できない変換フォーマットです ({formatKind})");

                //2025.08.05:CS)土田:インデックスカラー対応 >>>>> ここから
                //case FormatKinds.Color64:
                //    return this.ConvertAnimationColor64(animationImages, name, redThreshold, greenThreshold, blueThreshold, matrixLedWidth, matrixLedHeight, matrixLedBrightness);

                //case FormatKinds.Color256:
                //    return this.ConvertAnimationColor256(animationImages, name, redThreshold, greenThreshold, blueThreshold, matrixLedWidth, matrixLedHeight, matrixLedBrightness);
                //----------
                case FormatKinds.Color64:
                case FormatKinds.Color256:
                    return ConvertResult.Failed($"フォーマット'{formatKind}'は現在のバージョンでは非対応です");

                case FormatKinds.IndexColor:
                    //2026.05.22:CS)杉原:サウンド機能追加 >>>>> ここから
                    ////2025.08.18:CS)土田:ガンマ補正対応で引数を追加 >>>>> ここから
                    ////return this.ConvertAnimationIndexColor(animationImages, matrixLedWidth, matrixLedHeight, matrixLedBrightness);
                    ////-----
                    ////2025.08.21:CS)土田:残像軽減対応で引数を追加 >>>>> ここから
                    ////return this.ConvertAnimationIndexColor(animationImages, matrixLedWidth, matrixLedHeight, matrixLedBrightness, gamma);
                    ////----------
                    ////2025.10.03:CS)土田:変換結果の保存先を選択できるように引数追加 >>>>> ここから
                    ////return this.ConvertAnimationIndexColor(animationImages, matrixLedWidth, matrixLedHeight, matrixLedBrightness, gamma, motionBlurReduction);
                    ////----------
                    //return this.ConvertAnimationIndexColor(animationImages, matrixLedWidth, matrixLedHeight, matrixLedBrightness, gamma, motionBlurReduction, savePath);
                    ////2025.10.03:CS)土田:変換結果の保存先を選択できるように引数追加 <<<<< ここまで
                    ////2025.08.21:CS)土田:残像軽減対応で引数を追加 <<<<< ここまで
                    ////2025.08.18:CS)土田:ガンマ補正対応で引数を追加 <<<<< ここまで
                    ////2025.08.05:CS)土田:インデックスカラー対応 <<<<< ここまで
                    //-----
                    return this.ConvertAnimationIndexColor(animationImages, matrixLedWidth, matrixLedHeight, matrixLedBrightness, gamma, motionBlurReduction, savePath, pcm8);
                    //2026.05.22:CS)杉原:サウンド機能追加 <<<<< ここまで

                default:
                    //不明な形式
                    return ConvertResult.Failed($"不明な変換フォーマット ({formatKind})");
            }
        }
    }
}
