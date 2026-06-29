using MicroSign.Core.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;

namespace MicroSign.Core.Models
{
    partial class Model
    {
        public struct ClipAnimationResult
        {
            /// <summary>
            /// 成功フラグ
            /// </summary>
            public readonly bool IsSuccess;

            /// <summary>
            /// エラーメッセージ
            /// </summary>
            public readonly string? ErrorMessage;

            /// <summary>
            /// 出力パス一覧
            /// </summary>
            public readonly List<string>? OutputPaths;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="isSuccess"></param>
            /// <param name="message"></param>
            /// <param name="outputFolder"></param>
            private ClipAnimationResult(bool isSuccess, string? message, List<string>? outputFolder)
            {
                this.IsSuccess = isSuccess;
                this.ErrorMessage = message;
                this.OutputPaths = outputFolder;
            }

            /// <summary>
            /// 失敗
            /// </summary>
            /// <param name="message"></param>
            /// <returns></returns>
            public static ClipAnimationResult Failed(string message)
            {
                ClipAnimationResult result = new ClipAnimationResult(false, message, null);
                return result;
            }

            /// <summary>
            /// 成功
            /// </summary>
            /// <param name="outputPaths"></param>
            /// <returns></returns>
            public static ClipAnimationResult Success(List<string>? outputPaths)
            {
                ClipAnimationResult result = new ClipAnimationResult(true, null, outputPaths);
                return result;
            }
        }

        /// <summary>
        /// アニメーション切り抜き
        /// </summary>
        /// <param name="outputFolder"></param>
        /// <param name="filename"></param>
        /// <param name="image"></param>
        /// <param name="moveDirection"></param>
        /// <param name="moveSpeed"></param>
        /// <param name="matrixLedWidth"></param>
        /// <param name="matrixLedHeight"></param>
        /// <returns></returns>
        public ClipAnimationResult ClipAnimation(string outputFolder, string filename, BitmapSource? image, AnimationMoveDirection moveDirection, int moveSpeed, int matrixLedWidth, int matrixLedHeight)
        {
            //画像有効判定
            if (image == null)
            {
                //画像が無効の場合は処理できないので終了
                string msg = $"切り抜き対象の画像が無効です";
                LOGGER.Warn(msg);
                return ClipAnimationResult.Failed(msg);
            }
            else
            {
                //画像が有効の場合は処理続行
                LOGGER.Debug("切り抜き対象の画像が有効");
            }

            //出力フォルダを生成
            try
            {
                //出力フォルダを作成
                LOGGER.Debug($"出力先フォルダの生成  ('{outputFolder}')");
                System.IO.Directory.CreateDirectory(outputFolder);
            }
            catch (Exception ex)
            {
                //例外は握りつぶす
                string msg = $"出力フォルダの作成で例外が発生しました (outputFolder='{outputFolder}') ({ex})";
                LOGGER.WarnEx(msg, ex);
                return ClipAnimationResult.Failed(msg);
            }

            //画像サイズを取得
            int imageWidth = image.PixelWidth;
            int imageHeight = image.PixelHeight;

            //移動方向から、開始位置と移動量を計算
            int moveDistance = CommonConsts.Values.Zero.I;
            int startX = (int)CommonConsts.Points.Zero.X;
            int startY = (int)CommonConsts.Points.Zero.Y;
            int moveX = CommonConsts.Values.Zero.I;
            int moveY = CommonConsts.Values.Zero.I;
            switch (moveDirection)
            {
                case AnimationMoveDirection.Up:
                    //下から上へ移動
                    {
                        //移動量
                        // >> 画像縦幅とLED縦幅の差を移動
                        moveDistance = imageHeight - matrixLedHeight;

                        //X軸
                        // >> 変化しない

                        //Y軸
                        // >> 開始位置は上端
                        //startY = 0;
                        // >> 画像を上に動かすので、負方向に移動する
                        moveY = moveSpeed * CommonConsts.Values.NegativeOne.I;
                    }
                    break;

                case AnimationMoveDirection.Down:
                    //上から下へ移動
                    {
                        //移動量
                        // >> 画像縦幅とLED縦幅の差を移動
                        moveDistance = imageHeight - matrixLedHeight;

                        //X軸
                        // >> 変化しない

                        //Y軸
                        // >> 開始位置は下端
                        startY = (int)CommonConsts.Points.Zero.Y - moveDistance;
                        // >> 画像を下に動かすので、正方向に移動する
                        moveY = moveSpeed;
                    }
                    break;

                case AnimationMoveDirection.Left:
                    //右から左へ移動
                    {
                        //移動量
                        // >> 画像横幅とLED横幅の差を移動
                        moveDistance = imageWidth - matrixLedWidth;

                        //X軸
                        // >> 開始位置は左端
                        //startX = 0;
                        // >> 画像を左に動かすので、負方向に移動する
                        moveX = moveSpeed * CommonConsts.Values.NegativeOne.I;

                        //Y軸
                        // >> 変化しない
                    }
                    break;

                case AnimationMoveDirection.Right:
                    //左から右へ移動
                    {
                        //移動量
                        // >> 画像横幅とLED横幅の差を移動
                        moveDistance = imageWidth - matrixLedWidth;

                        //X軸
                        // >> 開始位置は右端
                        startX = (int)CommonConsts.Points.Zero.X - moveDistance;
                        // >> 画像を右に動かすので、正方向に移動する
                        moveX = moveSpeed;

                        //Y軸
                        // >> 変化しない
                    }
                    break;

                default:
                    //上記以外の方向には未対応
                    {
                        string msg = $"未対応の移動方向です(MoveDirection='{moveDirection}')";
                        LOGGER.Warn(msg);
                        return ClipAnimationResult.Failed(msg);
                    }
            }

            //移動量から必要なフレーム数を計算
            // >> フレーム数 = (画像縦幅 - LED縦幅) / 移動速度
            // >> ただし余りが出る場合はコマ数を+1する必要がある
            // >> 画像縦幅は必ず整数なので、1を引くことで必ず[必要なコマ数-1]にしてから、最後に+1する
            int frameCount = ((moveDistance - CommonConsts.Values.One.I) / moveSpeed) + CommonConsts.Values.One.I;
            LOGGER.Debug($"フレーム数='{frameCount}'");

            //ファイル番号フォーマットを取得
            //2026.06.29:CS)杉原:GetNumberFormat()追加 >>>>> ここから
            //string fileNumberFormat = string.Empty;
            //{
            //    int log = (int)Math.Log10(frameCount);
            //    int m = log + CommonConsts.Collection.Step;
            //    StringBuilder sb = new StringBuilder();
            //    for (int i = CommonConsts.Index.First; i < m; i += CommonConsts.Index.Step)
            //    {
            //        sb.Append(CommonConsts.File.ZeroPrace);
            //    }
            //    fileNumberFormat = sb.ToString();
            //}
            //----------
            string fileNumberFormat = this.GetNumberFormat(frameCount);
            //2026.06.29:CS)杉原:GetNumberFormat()追加 <<<<< ここまで

            //出力画像パスを保存するリスト
            List<string> outputPaths = new List<string>();

            //レンダーターゲットビットマップを生成
            RenderTargetBitmap? renderBitmap = null;
            {
                var ret = this.CreateRenderTargetBitmap(matrixLedWidth, matrixLedHeight);
                if (ret.IsSuccess)
                {
                    //成功の場合は続行
                    LOGGER.Debug("レンダーターゲットビットマップの生成に成功");
                }
                else
                {
                    //失敗の場合は終了
                    string msg = $"レンダーターゲットビットマップの生成に失敗しました (Width={matrixLedWidth}, Height={matrixLedHeight})";
                    LOGGER.Warn(msg);
                    return ClipAnimationResult.Failed(msg);
                }

                //生成したレンダーターゲットビットマップを取得
                renderBitmap = ret.RenderBitmap;
            }

            //アニメーション切り抜き
            for (int i = CommonConsts.Index.First; i < frameCount; i += CommonConsts.Index.Step)
            {
                //切り抜き座標を計算
                // >> 移動しない方向のmoveは0なので、開始位置から変化しない
                int x = startX + (moveX * i);
                int y = startY + (moveY * i);

                //切り抜いた画像を描写
                {
                    var ret = this.RenderImage(renderBitmap, image, x, y, imageWidth, imageHeight);
                    if (ret.IsSuccess)
                    {
                        //成功の場合は続行
                        //CommonLogger.Debug("レンダーターゲットビットマップの描写に成功");
                    }
                    else
                    {
                        //失敗の場合は終了
                        string msg = $"レンダリングターゲットビットマップの描写に失敗しました (X={x}, Y={y}, ImageWidth={imageWidth}, ImageHeight={imageHeight})";
                        LOGGER.Warn(msg);
                        return ClipAnimationResult.Failed(msg);
                    }
                }

                //切り抜いた画像を連番で保存
                string? savePath = null;
                try
                {
                    //ファイル名を決定
                    string fileNumberText = i.ToString(fileNumberFormat);
                    string saveName = string.Format(CommonConsts.File.PngFileFormat, filename, fileNumberText);
                    savePath = System.IO.Path.Combine(outputFolder, saveName);

                    //PNGで保存
                    PngBitmapEncoder png = new PngBitmapEncoder();
                    BitmapFrame frame = BitmapFrame.Create(renderBitmap);
                    png.Frames.Add(frame);
                    using (System.IO.Stream stm = System.IO.File.Create(savePath))
                    {
                        png.Save(stm);
                    }

                    //保存したファイルのパスをリストに追加
                    outputPaths.Add(savePath);
                }
                catch (Exception ex)
                {
                    //例外は握りつぶす
                    string msg = $"PNG保存で例外が発生しました (path='{savePath}') ({ex})";
                    LOGGER.WarnEx(msg, ex);
                    return ClipAnimationResult.Failed(msg);
                }
            }

            //ここまできたら成功
            return ClipAnimationResult.Success(outputPaths);
        }
    }
}
