using MicroSign.Core.Models.PanelConfigs;
using System.Xml.Linq;

namespace MicroSign.Core.ViewModels
{
    partial class MainWindowViewModel
    {
        /// <summary>
        /// アニメーション変換結果
        /// </summary>
        /// <remarks>
        /// 2025.08.12:CS)杉原:パレット処理の流れを変更
        /// </remarks>
        public struct ConvertAnimationResult
        {
            /// <summary>
            /// 成功フラグ
            /// </summary>
            public readonly bool IsSuccess;

            /// <summary>
            /// メッセージ
            /// </summary>
            public readonly string? Message;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="isSuccess"></param>
            /// <param name="message"></param>
            private ConvertAnimationResult(bool isSuccess, string? message)
            {
                this.IsSuccess = isSuccess;
                this.Message = message;
            }

            /// <summary>
            /// 失敗
            /// </summary>
            /// <param name="message"></param>
            /// <returns></returns>
            public static ConvertAnimationResult Failed(string message)
            {
                ConvertAnimationResult result = new ConvertAnimationResult(false, message);
                return result;
            }

            /// <summary>
            /// 成功
            /// </summary>
            /// <returns></returns>
            public static ConvertAnimationResult Success()
            {
                ConvertAnimationResult result = new ConvertAnimationResult(true, null);
                return result;
            }
        }

        /// <summary>
        /// アニメーション変換
        /// </summary>
        /// <returns>アニメーション変換結果</returns>
        public ConvertAnimationResult ConvertAnimation()
        {
            //デフォルトのマトリクスLED画像パスに保存
            return this.ConvertAnimation(MicroSignConsts.Path.MatrixLedImagePath);
        }

        /// <summary>
        /// アニメーション変換
        /// </summary>
        /// <param name="savePath">保存先(2025.10.03:CS)土田:変換結果の保存先を選択できるように引数追加)</param>
        /// <returns>アニメーション変換結果</returns>
        public ConvertAnimationResult ConvertAnimation(string savePath)
        {
            //2025.08.12:CS)杉原:パレット処理の流れを変更 >>>>> ここから
            ////クラス名
            //string? name = this.Name;
            //
            ////閾値取得
            //int redThreshold = this.RedThreshold;
            //int greenThreshold = this.GreenThreshold;
            //int blueThreshold = this.BlueThreshold;
            //----------
            // >> 不要なパラメータを削除
            //2025.08.12:CS)杉原:パレット処理の流れを変更 <<<<< ここまで

            //フォーマット
            FormatKinds formatKind = this.FormatKind;

            //マトリクスLEDの設定値取得
            int matrixLedWidth = this.MatrixLedWidth;
            int matrixLedHeight = this.MatrixLedHeight;
            int matrixLedBrightness = this.MatrixLedBrightness;
            //2025.08.18:CS)土田:ガンマ補正対応で追加 >>>>> ここから
            //-----
            int gammaCorrection = this.GammaCorrection;
            // >> 小数に変換
            double gamma = gammaCorrection / CommonConsts.Gammas.Magnification;
            //2025.08.18:CS)土田:ガンマ補正対応で追加 <<<<< ここまで
            //2025.08.21:CS)土田:残像軽減対応で追加 >>>>> ここから
            //-----
            int motionBlurReduction = this.MotionBlurReduction;
            //2025.08.21:CS)土田:残像軽減対応で追加 <<<<< ここまで

            //アニメーション画像コレクション
            // >> コンストラクタで生成しているのでnullチェック不要
            AnimationImageItemCollection animationImages = this.AnimationImages;

            //2026.05.22:CS)杉原:サウンド機能追加 >>>>> ここから
            //----------
            // PCM8データ取得
            byte[]? pcm8 = this.SoundPcm8Data;
            //2026.05.22:CS)杉原:サウンド機能追加 <<<<< ここまで

            //変換
            //2026.05.22:CS)杉原:サウンド機能追加 >>>>> ここから
            ////2025.08.12:CS)杉原:パレット処理の流れを変更 >>>>> ここから
            ////Models.Model.ConvertResult ret = this.Model.ConvertAnimation(name, formatKind, animationImages, redThreshold, greenThreshold, blueThreshold, matrixLedWidth, matrixLedHeight, matrixLedBrightness);
            ////this.ConvertImage = ret.ConvertImage;
            ////this.Code = ret.Code;
            ////----------
            //// >> 変換コードは無くなりました
            ////2025.08.18:CS)土田:ガンマ補正対応で引数を追加 >>>>> ここから
            ////Models.Model.ConvertResult ret = this.Model.ConvertAnimation(formatKind, animationImages, matrixLedWidth, matrixLedHeight, matrixLedBrightness);
            ////-----
            ////2025.08.21:CS)土田:残像軽減対応で引数を追加 >>>>> ここから
            ////Models.Model.ConvertResult ret = this.Model.ConvertAnimation(formatKind, animationImages, matrixLedWidth, matrixLedHeight, matrixLedBrightness, gamma);
            ////-----
            ////2025.10.03:CS)土田:変換結果の保存先を選択できるように引数追加 >>>>> ここから
            ////Models.Model.ConvertResult ret = this.Model.ConvertAnimation(formatKind, animationImages, matrixLedWidth, matrixLedHeight, matrixLedBrightness, gamma, motionBlurReduction);
            ////-----
            //Models.Model.ConvertResult ret = this.Model.ConvertAnimation(formatKind, animationImages, matrixLedWidth, matrixLedHeight, matrixLedBrightness, gamma, motionBlurReduction, savePath);
            ////2025.10.03:CS)土田:変換結果の保存先を選択できるように引数追加 <<<<< ここまで
            ////2025.08.21:CS)土田:残像軽減対応で引数を追加 <<<<< ここまで
            ////2025.08.18:CS)土田:ガンマ補正対応で引数を追加 <<<<< ここまで
            //this.AnimationMergedBitmap = ret.AnimationMergedBitmap;
            //this.AnimationDatas = ret.AnimationDatas;
            ////2025.08.12:CS)杉原:パレット処理の流れを変更 <<<<< ここまで
            //-----
            Models.Model.ConvertResult ret = this.Model.ConvertAnimation(formatKind, animationImages, matrixLedWidth, matrixLedHeight, matrixLedBrightness, gamma, motionBlurReduction, savePath, pcm8);
            this.AnimationMergedBitmap = ret.AnimationMergedBitmap;
            this.AnimationDatas = ret.AnimationDatas;
            //2026.05.22:CS)杉原:サウンド機能追加 <<<<< ここまで


            //終了
            //2025.08.12:CS)杉原:パレット処理の流れを変更 >>>>> ここから
            //return (ret.IsSuccess, ret.Code);
            //----------
            if (ret.IsSuccess)
            {
                //成功の場合は処理続行
            }
            else
            {
                //失敗の場合は失敗で終了する
                string msg = ret.Message ?? "不明なエラー";
                return ConvertAnimationResult.Failed(msg);
            }

            //ここまで来たら成功で終了
            return ConvertAnimationResult.Success();
            //2025.08.12:CS)杉原:パレット処理の流れを変更 <<<<< ここまで
        }
    }
}
