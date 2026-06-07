using System;
using System.Windows.Media.Imaging;

namespace MicroSign.Core.Models
{
    partial class Model
    {
        /// <summary>
        /// 指定された画像をログフォルダに保存する
        /// </summary>
        /// <param name="filename">ファイル名</param>
        /// <param name="image">保存する画像</param>
        /// <remarks>
        /// log4netのログ出力先が「${USERPROFILE}\MicroSign\Log\log.txt」なので
        /// ユーザプロファイル配下に保存します
        /// </remarks>
        private void ConvertSaveLogImage(string filename, BitmapSource? image)
        {
            //ファイル名有効判定
            {
                bool isNull = string.IsNullOrEmpty(filename);
                if (isNull)
                {
                    //無効の場合は終了
                    LOGGER.Warn("ログ画像保存ファイル名無効");
                    return;
                }
                else
                {
                    //有効の場合は処理続行
                    LOGGER.Debug($"ログ画像保存ファイル名有効 ({filename})");
                }
            }

            //画像有効判定
            if (image == null)
            {
                //画像が無効の場合は何もしない
                LOGGER.Warn("ログ画像保存の府画像が無効");
                return;
            }
            else
            {
                //有効の場合は処理続行
                LOGGER.Debug($"ログ画像保存の画像が有効");
            }

            //ログディレクトリ取得
            string? dir = MicroSignLogger.LogDir;
            {
                bool isNull = string.IsNullOrEmpty(dir);
                if (isNull)
                {
                    //無効の場合は終了
                    LOGGER.Warn("ログ画像保存ディレクトリ無効");
                    return;
                }
                else
                {
                    //有効の場合は処理続行
                    LOGGER.Debug($"ログ画像保存ディレクトリ有効 ({dir})");
                }
            }

            //フルパスにする
            string path = System.IO.Path.Combine(dir!, filename);
            LOGGER.Debug($"ログ画像保存パス ({path})");

            //ディレクトリ生成
            try
            {
                LOGGER.Debug($"ログ画像保存ディレクトリ生成 ({dir})");
                System.IO.Directory.CreateDirectory(dir!);
            }
            catch (Exception ex)
            {
                //例外は握りつぶして終了
                LOGGER.WarnEx($"ログ画像保存のディレクトリ生成で例外発生 ({dir})", ex);
                return;
            }

            //常にPNGで保存する
            try
            {
                //PNGで保存
                LOGGER.Debug($"ログ画像保存開始 ({path})");
                using (System.IO.Stream st = System.IO.File.Create(path))
                {
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(image));
                    encoder.Save(st);
                }
                LOGGER.Debug($"ログ画像保存完了 ({path})");
            }
            catch (Exception ex)
            {
                //例外は握りつぶして終了
                LOGGER.WarnEx($"ログ画像保存で例外発生 ({path})", ex);
                return;
            }
        }
    }
}
