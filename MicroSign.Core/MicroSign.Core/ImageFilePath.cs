using System;
using System.Text.RegularExpressions;

namespace MicroSign.Core
{
    /// <summary>
    /// ファイルパスクラス
    /// </summary>
    public partial class ImageFilePath : IComparable<ImageFilePath>
    {
        //2026.06.07:CS)杉原:LOGGERを修正 >>>>> ここから
        ///// <summary>
        ///// LOG4NETのロガー
        ///// </summary>
        //private static readonly log4net.ILog LOGGER = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType!);
        //----------
        /// <summary>
        /// LOG4NETのロガー
        /// </summary>
        private static readonly MicroSignLogger LOGGER = MicroSignLogger.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType!);
        //2026.06.07:CS)杉原:LOGGERを修正 <<<<< ここまで

        /// <summary>
        /// パス
        /// </summary>
        public string Path { get; protected set; } = string.Empty;

        /// <summary>
        /// 数字以外のファイル名
        /// </summary>
        public string? TextWithoutNumbers { get; protected set; } = null;

        /// <summary>
        /// 先頭数字部分
        /// </summary>
        public int? HeaderNumber { get; protected set; } = null;

        /// <summary>
        /// 最後数字部分
        /// </summary>
        public int? FooterNumber { get; protected set; } = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="path"></param>
        /// <param name="textWithoutNumbers"></param>
        /// <param name="headerNumber"></param>
        /// <param name="footerNumber"></param>
        private ImageFilePath(string path, string? textWithoutNumbers, int? headerNumber, int? footerNumber)
        {
            this.Path = path;
            this.TextWithoutNumbers = textWithoutNumbers;
            this.HeaderNumber = headerNumber;
            this.FooterNumber = footerNumber;
        }

        /// <summary>
        /// 正規表現キー
        /// </summary>
        private static class RegexKeys
        {
            /// <summary>
            /// 値部分
            /// </summary>
            public static readonly string Value = "V";

            /// <summary>
            /// 値以外の部分
            /// </summary>
            public static readonly string Text = "T";
        }

        /// <summary>
        /// 先頭部分の数字を抽出する正規表現
        /// </summary>
        /// <returns></returns>
        [GeneratedRegex(@"^(?<V>[0-9]+)(?<T>.*)$")]
        private static partial Regex GetHeaderNumber();

        /// <summary>
        /// 最後部分の数字を抽出する正規表現
        /// </summary>
        /// <returns></returns>
        [GeneratedRegex(@"^(?<T>.*?)(?<V>[0-9]+)$")]
        private static partial Regex GetFooterNumber();

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="path">パス</param>
        public static ImageFilePath? Create(string? path)
        {
            //パス有効判定
            {
                bool isNull = string.IsNullOrEmpty(path);
                if (isNull)
                {
                    //無効の場合はnullで終了する
                    LOGGER.Warn("ImageFilePath生成でパス無効");
                    return null;
                }
                else
                {
                    //有効の場合は処理続行
                    LOGGER.Debug($"ImageFilePath生成でパス有効 = '{path}'");
                }
            }

            //ファイル名の部分(拡張子なし)を取得
            string? filename = System.IO.Path.GetFileNameWithoutExtension(path);
            {
                bool isNull = string.IsNullOrEmpty(path);
                if (isNull)
                {
                    //無効の場合はnullで終了する
                    LOGGER.Warn($"ImageFilePath生成でファイル名取得失敗 '{path}'");
                    return null;
                }
                else
                {
                    //有効の場合は処理続行
                    LOGGER.Debug($"ImageFilePath生成でファイル名取得成功 '{path}' -> '{filename}'");
                }
            }

            //ファイル名の先頭にある数字を取得
            int? headerNumber = null;
            try
            {
                LOGGER.Debug($"ImageFilePath生成で先頭数字取得開始 '{filename}'");
                System.Text.RegularExpressions.Match m = ImageFilePath.GetHeaderNumber().Match(filename!);
                bool isMatch = m.Success;
                if (isMatch)
                {
                    //マッチした場合
                    string t = m.Groups[RegexKeys.Value].Value;
                    LOGGER.Debug($"ImageFilePath生成で先頭数字取得成功 '{filename}' -> '{t}'");
                    bool ret = int.TryParse(t, out var value);
                    if (ret)
                    {
                        //数値に変換できた場合、先頭の数値に設定
                        // >> 変換数字を覚える
                        headerNumber = value;

                        // >> 数字部分をファイル名として取得する
                        filename = m.Groups[RegexKeys.Text].Value;
                        LOGGER.Debug($"ImageFilePath生成で先頭数字の数値化成功 '{t}' -> {value} '{filename}'");
                    }
                    else
                    {
                        //数字に変換できなかった場合は何もしない
                        LOGGER.Debug($"ImageFilePath生成で先頭数字の数値化失敗 '{t}'");
                    }
                }
                else
                {
                    //失敗はそのまま処理続行
                    LOGGER.Debug($"ImageFilePath生成で先頭数字取得失敗 '{filename}'");
                }

            }
            catch(System.Exception ex)
            {
                //例外は握りつぶして無視する
                LOGGER.WarnEx("ファイル名の先頭数字取得で例外発生", ex);
            }

            //最後の数字を取得
            int? footerNumber = null;
            try
            {
                LOGGER.Debug($"ImageFilePath生成で最終数字取得開始 '{filename}'");
                System.Text.RegularExpressions.Match m = ImageFilePath.GetFooterNumber().Match(filename!);
                bool isMatch = m.Success;
                if (isMatch)
                {
                    //マッチした場合
                    string t = m.Groups[RegexKeys.Value].Value;
                    LOGGER.Debug($"ImageFilePath生成で最終数字取得成功 '{filename}' -> '{t}'");
                    bool ret = int.TryParse(t, out var value);
                    if (ret)
                    {
                        //数値に変換できた場合、先頭の数値に設定
                        // >> 変換数字を覚える
                        footerNumber = value;

                        // >> 数字部分をファイル名として取得する
                        filename = m.Groups[RegexKeys.Text].Value;
                        LOGGER.Debug($"ImageFilePath生成で最終数字の数値化成功 '{t}' ->  '{filename}' {value}");
                    }
                    else
                    {
                        //数字に変換できなかった場合は何もしない
                        LOGGER.Debug($"ImageFilePath生成で最終数字の数値化失敗 '{t}'");
                    }
                }
                else
                {
                    //失敗はそのまま処理続行
                    LOGGER.Debug($"ImageFilePath生成で最終数字取得失敗 '{filename}'");
                }

            }
            catch (System.Exception ex)
            {
                //例外は握りつぶして無視する
                LOGGER.WarnEx("ファイル名の最終数字取得で例外発生", ex);
            }

            //戻り値生成して終了
            LOGGER.Info($"ImageFilePath生成('{path}','{filename}', {headerNumber}, {footerNumber})");
            return new ImageFilePath(path!, filename, headerNumber, footerNumber);
        }

        /// <summary>
        /// 比較関数
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(ImageFilePath? other)
        {
            //引数有効判定
            if (other == null)
            {
                //無効の場合は判定できないの小さい(this<otherとなる0より小さい値)を返す
                // >> nullが後ろとなるような値を返す
                return CommonConsts.CompareValue.Less;
            }
            else
            {
                //有効の場合は処理続行
            }

            //テキストを比較する
            {
                string? t1 = this.TextWithoutNumbers;
                string? t2 = other.TextWithoutNumbers;
                int ret = string.Compare(t1, t2, true);//大文字小文字を区別せずに比較
                if (ret == CommonConsts.CompareValue.Same)
                {
                    //一致の場合は処理続行
                }
                else
                {
                    //不一致の場合は比較した値で終了
                    return ret;
                }
            }

            //先頭数字で比較
            {
                int? v1 = this.HeaderNumber;
                int? v2 = other.HeaderNumber;
                if (v1.HasValue)
                {
                    if (v2.HasValue)
                    {
                        //両方の値が有効の場合は比較する
                        int vv1 = v1.Value;
                        int vv2 = v2.Value;
                        int ret = vv1.CompareTo(vv2);
                        if (ret == CommonConsts.CompareValue.Same)
                        {
                            //一致の場合は処理続行
                        }
                        else
                        {
                            //不一致の場合は比較した値で終了
                            return ret;
                        }
                    }
                    else
                    {
                        //引数側が無効の場合小さい(this<otherとなる0より小さい値)を返す
                        // >> nullが後ろとなるような値を返す
                        return CommonConsts.CompareValue.Less;
                    }
                }
                else
                {
                    if (v2.HasValue)
                    {
                        //自分側の値が無効の場合は大きい(other<thisとなる0より大きい値)を返す
                        // >> nullが後ろとなるような値を返す
                        return CommonConsts.CompareValue.Greater;
                    }
                    else
                    {
                        //両方nullの場合は処理続行
                    }
                }
            }

            //最終数字で比較
            {
                int? v1 = this.FooterNumber;
                int? v2 = other.FooterNumber;
                if (v1.HasValue)
                {
                    if (v2.HasValue)
                    {
                        //両方の値が有効の場合は比較する
                        int vv1 = v1.Value;
                        int vv2 = v2.Value;
                        int ret = vv1.CompareTo(vv2);
                        if (ret == CommonConsts.CompareValue.Same)
                        {
                            //一致の場合は処理続行
                        }
                        else
                        {
                            //不一致の場合は比較した値で終了
                            return ret;
                        }
                    }
                    else
                    {
                        //引数側が無効の場合小さい(this<otherとなる0より小さい値)を返す
                        // >> nullが後ろとなるような値を返す
                        return CommonConsts.CompareValue.Less;
                    }
                }
                else
                {
                    if (v2.HasValue)
                    {
                        //自分側の値が無効の場合は大きい(other<thisとなる0より大きい値)を返す
                        // >> nullが後ろとなるような値を返す
                        return CommonConsts.CompareValue.Greater;
                    }
                    else
                    {
                        //両方nullの場合は処理続行
                    }
                }
            }

            //ここまで来たら一致を返す
            return CommonConsts.CompareValue.Same;
        }

    }
}
