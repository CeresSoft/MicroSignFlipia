using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace MicroSign.Core
{
    /// <summary>
    /// 共通ユーティリティ
    /// </summary>
    public static class CommonUtils
    {
        /// <summary>
        /// 文字列長さ取得
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static int GetLength(string text)
        {
            if(text == null)
            {
                return CommonConsts.Collection.Empty;
            }
            else
            {
                int result = text.Length;
                return result;
            }
        }

        /// <summary>
        /// 要素数取得
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static int GetCount(ICollection? collection)
        {
            if(collection == null)
            {
                return CommonConsts.Collection.Empty;
            }
            else
            {
                int result = collection.Count;
                return result;
            }
        }

        /// <summary>
        /// 文字列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string? ToObjectString(object obj)
        {
            //オブジェクト有効判定
            if(obj == null)
            {
                //無効の場合はnullを返す
                return null;
            }
            else
            {
                //有効の場合は処理続行
            }

            //型を取得
            Type t = obj.GetType();
            if(t == null)
            {
                //無効の場合はnullを返す
                return null;
            }
            else
            {
                //有効の場合は処理続行
            }

            //プロパティリストを取得
            PropertyInfo[] pis = t.GetProperties();
            if(pis == null)
            {
                //無効の場合はnullを返す
                return null;
            }
            else
            {
                //有効の場合は処理続行
            }

            //全プロパティを文字列化
            StringBuilder sb = new StringBuilder(CommonConsts.Text.STRING_BUILDER_CAPACITY);
            bool isFirst = true;
            foreach(PropertyInfo pi in pis)
            {
                if(isFirst)
                {
                    //初回は何もしない
                    isFirst = false;
                }
                else
                {
                    //初回以外はカンマを追加
                    sb.Append(", ");
                }
                string name = pi.Name;
                sb.Append(name).Append("=");
                object? value = pi.GetValue(obj);
                if(value is string)
                {
                    //値が文字列の場合
                    sb.Append("'").Append(value).Append("'");
                }
                else
                {
                    //値が文字列以外の場合
                    sb.Append(value);
                }
            }

            //終了
            string result = sb.ToString();
            return result;
        }

        /// <summary>
        /// 小文字化
        /// </summary>
        /// <param name="text">小文字化する文字列</param>
        /// <returns></returns>
        public static string? ToSafeLower(string text)
        {
            bool isNull = string.IsNullOrEmpty(text);
            if(isNull)
            {
                //無効の場合はnullを返す
                return null;
            }
            else
            {
                //有効の場合は小文字化して返す
                string result = text.ToLower();
                return result;
            }
        }

        /// <summary>
        /// BYTE文字列化
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ToByteString(byte[] data)
        {
            if(data == null)
            {
                return string.Empty;
            }
            else
            {
                string result = BitConverter.ToString(data);
                return result;
            }
        }

        /// <summary>
        /// 安全に破棄
        /// </summary>
        /// <param name="obj"></param>
        public static void SafeDispose(IDisposable? obj)
        {
            if(obj == null)
            {
                //無効の場合は何もしない
            }
            else
            {
                //有効の場合はDisposeする
                try
                {
                    obj.Dispose();
                }
                catch(Exception)
                {
                    //例外は握りつぶす
                }
            }
        }

        /// <summary>
        /// 安全なシグナルセット
        /// </summary>
        /// <param name="signal"></param>
        public static void SafeSignalSet(ManualResetEvent signal)
        {
            //シグナル有効判定
            if (signal == null)
            {
                //無効の場合は何もしない
                //LOGGER.Warn("シグナル設定 - シグナル無効");
                return;
            }
            else
            {
                //有効の場合は処理続行
            }

            //セット
            try
            {
                signal.Set();
            }
            catch (Exception /*ex*/)
            {
                //例外は握りつぶす
                //LOGGER.Warn("シグナル設定で例外発生", ex);
            }
        }

        /// <summary>
        /// 安全なシグナルリセット
        /// </summary>
        /// <param name="signal"></param>
        public static void SafeSignalReset(ManualResetEvent signal)
        {
            //シグナル有効判定
            if (signal == null)
            {
                //無効の場合は何もしない
                //LOGGER.Warn("シグナル解除 - シグナル無効");
                return;
            }
            else
            {
                //有効の場合は処理続行
            }

            //リセット
            try
            {
                signal.Reset();
            }
            catch (Exception /*ex*/)
            {
                //例外は握りつぶす
                //LOGGER.Warn("シグナル解除で例外発生", ex);
            }
        }

        /// <summary>
        /// フルパスを取得する
        /// </summary>
        /// <param name="path">フルパス化するパス</param>
        /// <returns></returns>
        public static string GetFullPath(string path)
        {
            //パス有効判定
            {
                bool isNull = string.IsNullOrEmpty(path);
                if (isNull)
                {
                    //パスが無効の場合は何もせずに終了
                    return path;
                }
                else
                {
                    //パスが有効の場合は処理続行
                }
            }

            //2021.07.01:CS)杉原:環境変数対応 >>>>> ここから
            //----------
            // >> "%"で囲まれた環境変数を置き換えます
            string path2 = Environment.ExpandEnvironmentVariables(path);
            //2021.07.01:CS)杉原:環境変数対応 <<<<< ここまで

            //フルパス判定
            {
                bool isRoot = System.IO.Path.IsPathRooted(path2);
                if (isRoot)
                {
                    //フルパスの場合は何もせずに終了
                    //2024.01.04:CS)杉原:ドットを抜く対応 >>>>> ここから
                    //return path2;
                    //----------
                    string fullpath2 = System.IO.Path.GetFullPath(path2);
                    return fullpath2;
                    //2024.01.04:CS)杉原:ドットを抜く対応 <<<<< ここまで
                }
                else
                {
                    //フルパス以外の場合は処理続行
                }
            }

            //実行ファイルのパスを取得する
            string dir = System.AppDomain.CurrentDomain.BaseDirectory;

            //パスを結合
            //2021.06.09:CS)杉原:ドットを抜く対応 >>>>> ここから
            //string result = System.IO.Path.Combine(dir, path);
            //----------
            // >> WPFのSaveFileDialogで以下のような途中にドットのディレクトリ(='\.\')が存在すると
            // >> 無効な値とはじかれてしまう(=ディレクトリが認識できないみたいです)物があるので
            // >> ドットのディレクトリを削除するためにフルパス化(=System.IO.Path.GetFullPath(())します
            // >> >> 'C:\Users\sugihara\git\MHIMT_WharfMonitoring\30_PG\ImageFilingSystem\ImageFilingSystem\bin\Debug\.\CsvFolder'
            string fullPath = System.IO.Path.Combine(dir, path2);
            string result = System.IO.Path.GetFullPath(fullPath);
            //2021.06.09:CS)杉原:ドットを抜く対応 <<<<< ここまで

            //終了
            return result;
        }

        /// <summary>
        /// 相対パスを取得
        /// </summary>
        /// <param name="baseDir">基準ディレクトリ</param>
        /// <param name="path">相対パスに変換するパス</param>
        /// <returns>https://kuttsun.blogspot.com/2017/04/c.html</returns>
        /// <remarks></remarks>
        public static string? GetRelativePath(string? baseDir, string? path)
        {
            //基準ディレクトリ有効判定
            if(baseDir == null)
            {
                //基準ディレクトリが無効の場合は何もせずに終了
                return path;
            }
            else
            {
                bool isNull = string.IsNullOrEmpty(baseDir);
                if (isNull)
                {
                    //基準ディレクトリが無効の場合は何もせずに終了
                    return path;
                }
                else
                {
                    //基準ディレクトリが有効の場合は処理続行
                }
            }

            //パス有効判定
            if (path == null)
            {
                //パスが無効の場合は何もせずに終了
                return path;
            }
            else
            {
                bool isNull = string.IsNullOrEmpty(path);
                if (isNull)
                {
                    //パスが無効の場合は何もせずに終了
                    return path;
                }
                else
                {
                    //パスが有効の場合は処理続行
                }
            }

            //基準ディレクトリの後ろにファイルを追加する
            // >> Urlなので最後のディレクトリがファイル名として認識されるので
            // >> ファイル名となる部分を追加する
            string bathPath = System.IO.Path.Combine (baseDir, "---basefile---");

            //相対パスに変換
            Uri u1 = new Uri(bathPath);
            Uri u2 = new Uri(path);
            Uri relativeUri = u1.MakeRelativeUri(u2);
            string relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            //"/"を"\"に置き換え
            string result = relativePath.Replace('/', '\\');

            //終了
            return result;
        }

        /// <summary>
        /// ファイルパスのフォルダを作成
        /// </summary>
        /// <param name="path"></param>
        public static void CreateFilepathFolder(string path)
        {
            //パスの有効判定
            {
                bool isNull = string.IsNullOrEmpty(path);
                if(isNull)
                {
                    //無効の場合は終了
                    return;
                }
                else
                {
                    //有効の場合は処理続行
                }
            }

            //ディレクトリ取得
            string dir = System.IO.Path.GetDirectoryName(path) ?? "";
            {
                bool isNull = string.IsNullOrEmpty(dir);
                if (isNull)
                {
                    //無効の場合は終了
                    return;
                }
                else
                {
                    //有効の場合は処理続行
                }
            }

            //フォルダの存在確認
            {
                bool isExists = System.IO.Directory.Exists(dir);
                if (isExists)
                {
                    //存在する場合は終了
                    return;
                }
            }

            //ディレクトリ生成
            try
            {
                System.IO.Directory.CreateDirectory(dir);
                //LOGGER.Warn($"フォルダ作成成功 [path='{path}', dir='{dir}'");
            }
            catch (Exception /*ex*/)
            {
                //LOGGER.Warn($"フォルダ作成失敗 [path='{path}', dir='{dir}'", ex);
            }
        }

        /// <summary>
        /// 無効な倍率
        /// </summary>
        private const decimal _INVALID_GRA_MAG = 0;

        /// <summary>
        /// 既定倍率
        /// </summary>
        /// <remarks>FormatDisplayUnitText()用</remarks>
        private const decimal _DEFAULT_GRA_MAG = 1m;

        /// <summary>
        /// 値無し時テキスト
        /// </summary>
        /// <remarks>calcValueText()用</remarks>
        private const string _NO_VALUE_TEXT = "";

        ///// <summary>
        ///// Nulable値をフォーマットして返す
        ///// </summary>
        ///// <param name="v">値</param>
        ///// <param name="format">フォーマット</param>
        ///// <returns></returns>
        //public static string ToText(double? v, string format)
        //{
        //    //無効な倍率を渡して、計算が実行されないようにする
        //    return CommonUtils.ToText(v, format, CommonUtils._INVALID_GRA_MAG);
        //}

  //      /// <summary>
  //      /// 値をフォーマットして返す
  //      /// </summary>
  //      /// <param name="v"></param>
  //      /// <param name="format"></param>
  //      /// <param name="mag"></param>
  //      /// <returns></returns>
		//public static string ToText(double? v, string format, decimal mag)
  //      {
  //          decimal? dv = null;
  //          if (v.HasValue)
  //          {
  //              dv = Mathf.ToDecimal(v.Value);
  //          }
  //          string result = CommonUtils.ToText(dv, format, mag);
  //          return result;
  //      }

        /// <summary>
        /// 値をフォーマットして返す
        /// </summary>
        /// <param name="v"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ToText(decimal? v, string format)
        {
            //無効な倍率を渡して、計算が実行されないようにする
            return CommonUtils.ToText(v, format, CommonUtils._INVALID_GRA_MAG);
        }

        /// <summary>
        /// 値をフォーマットして返す
        /// </summary>
        /// <param name="v"></param>
        /// <param name="format"></param>
        /// <param name="mag"></param>
        /// <returns></returns>
        public static string ToText(decimal? v, string format, decimal mag)
        {
            //値有効判定
            if (!v.HasValue)
            {
                return CommonUtils._NO_VALUE_TEXT;
            }

            //値を取得
            decimal magValue = v.Value;

            //0より大きいときだけ計算
            if (CommonUtils._INVALID_GRA_MAG < mag)
            {
                magValue /= mag;
            }

            //文字列を返す
            //[注意]空文字が出せるようにnullだけチェックする
            if (format == null)
            {
                return magValue.ToString();
            }

            //フォーマットした文字列を返す
            string result = string.Format(format, magValue);

            //終了
            return result;
        }

        /// <summary>
        /// 値をフォーマットして返す
        /// </summary>
        /// <param name="v"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ToText(int? v, string format)
        {
            //無効な倍率を渡して、計算が実行されないようにする
            return CommonUtils.ToText(v, format, CommonUtils._INVALID_GRA_MAG);
        }

        /// <summary>
        /// 値をフォーマットして返す
        /// </summary>
        /// <param name="v"></param>
        /// <param name="format"></param>
        /// <param name="mag"></param>
        /// <returns></returns>
		public static string ToText(int? v, string format, decimal mag)
        {
            decimal? dv = null;
            if (v.HasValue)
            {
                dv = v.Value;
            }
            string result = CommonUtils.ToText(dv, format, mag);
            return result;
        }

        /// <summary>
        /// 小数点以下の桁数を取得する名前
        /// </summary>
        private const string _SCALE_TEXT_VALUE = "VALUE";

        /// <summary>
        /// 小数点以下の有効桁数を取得するフォーマット
        /// </summary>
        /// <remarks>
        /// 2020.11.17:CS)杉原:0.05の場合に0桁と返す問題の修正
        /// 0 -> 0桁
        /// 0. -> 0桁
        /// 0.0 -> 0桁
        /// 0.1 -> 1桁
        /// 0.10 -> 1桁
        /// 0.00 -> 0桁
        /// 0.05 -> 2桁
        /// 0.050 -> 2桁
        /// </remarks>
        private readonly static Regex _SCALE_TEXT_FORMAT = new Regex("^-?[0-9]+\\.(?<VALUE>[0-9]*?)(?>0+)$|^-?[0-9]+\\.(?<VALUE>[0-9]*[1-9]?)$");

        /// <summary>
        /// 値テキストの小数点以下が0以外の桁数を取得
        /// </summary>
        /// <param name="valueText">値テキスト</param>
        /// <returns></returns>
        /// <remarks>1.500->1.5となるように小数点以下の不要な0を削除する</remarks>
        public static int GetDecimalPointSize(string valueText)
        {
            //テキスト有効判定
            if (string.IsNullOrEmpty(valueText))
            {
                //無効の場合は0桁を返す
                return CommonConsts.Collection.Empty;
            }

            //正規表現で取得
            Match m = CommonUtils._SCALE_TEXT_FORMAT.Match(valueText);
            bool bSuccess = m.Success;
            if (!bSuccess)
            {
                //失敗の場合は0桁を返す
                return CommonConsts.Collection.Empty;
            }

            //桁数カウント
            string v = m.Groups[CommonUtils._SCALE_TEXT_VALUE].Value;
            int result = v.Length;

            //終了
            return result;
        }

        /// <summary>
        /// フォーマット文字列取得
        /// </summary>
        /// <param name="decimalPlaces"></param>
        /// <returns></returns>
        public static string GetFormatText(int decimalPlaces)
        {
            //小数点以下桁数
            if (decimalPlaces <= CommonConsts.Collection.Empty)
            {
                //0以下の場合は小数点無しのフォーマットを返す
                return ("{0:0}");
            }

            //指定された桁数のフォーマットを生成
            StringBuilder sb = new StringBuilder("{0:0.");
            for (int i = CommonConsts.Index.First ; i < decimalPlaces; i += CommonConsts.Index.Step)
            {
                sb.Append("0");
            }
            sb.Append("}");

            //終了
            string result = sb.ToString();
            return result;
        }

        /// <summary>
        /// 表示用単位文字既定フォーマット
        /// </summary>
        /// <remarks>FormatDisplayUnitText()用</remarks>
        private const string _DISPLAY_UNIT_TEXT_DEFAULT_FORMAT = "{0}";

        /// <summary>
        /// 表示用単位文字フォーマット（表示倍率つき）
        /// </summary>
        /// <remarks>FormatDisplayUnitText()用</remarks>
        private const string _DISPLAY_UNIT_TEXT_MAG_FORMAT = "x{1} {0}";

        /// <summary>
        /// 表示ユニットテキストフォーマット
        /// </summary>
        /// <param name="unitText"></param>
        /// <param name="mag"></param>
        /// <returns></returns>
        public static string FormatDisplayUnitText(string unitText, decimal mag)
        {
            string format = CommonUtils._DISPLAY_UNIT_TEXT_DEFAULT_FORMAT;

            //表示倍率が有効な数値で、かつ等倍でないときは単位文字の前に表示
            if (CommonUtils._INVALID_GRA_MAG < mag)
            {
                if (mag != CommonUtils._DEFAULT_GRA_MAG)
                {
                    format = CommonUtils._DISPLAY_UNIT_TEXT_MAG_FORMAT;
                }
            }

            //フォーマット
            string result = string.Format(format, unitText, mag);

            //終了
            return result;
        }

        /// <summary>
        /// 大文字・小文字を無視して比較
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static bool IsIgnoreCase(string s1, string s2)
        {
            bool result = (CommonUtils.CompareIgnoreCase(s1, s2) == CommonConsts.CompareValue.Same);
            return result;
        }

        /// <summary>
        /// 大文字・小文字を無視して比較
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static int CompareIgnoreCase(string s1, string s2)
        {
            int result = string.Compare(s1, s2, true);
            return result;
        }


        ///// <summary>
        ///// 16進数変換
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //public static string ToHex(int value)
        //{
        //    return CommonUtils.ToHex(value, Mathf.Values.Zero.I);
        //}

        /// <summary>
        /// 16進数変換
        /// </summary>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string ToHex(int value, int size)
        {
            //16進数で変換
            string result = value.ToString("X");

            //先頭を0埋め
            int len = CommonUtils.GetLength(result);
            if (size > len)
            {
                result = result.PadLeft(size, '0');
            }
            //終了
            return result;
        }

        /// <summary>
        /// 16進数フォーマット
        /// </summary>
        /// <remarks>toUInt()用</remarks>
        private static readonly Regex _TO_INT_HEX_FORMAT = new Regex("^0[Xx][0-9a-fA-F]+$");

        /// <summary>
        /// VB6 - 16進数フォーマット
        /// </summary>
        /// <remarks>toUInt()用</remarks>
        private static readonly Regex _TO_INT_VB6_HEX_FORMAT = new Regex("^&[Hh][0-9a-fA-F]+$");

        /// <summary>
        /// VB616進数フォーマットの先頭にある"&h"の文字数
        /// </summary>
        private const int _TO_HEX_VB6_MARK_SIZE = 2;

        /// <summary>
        /// 16進数基底
        /// </summary>
        private const int _HEX_BASE = 16;

        /// <summary>
        /// 16進数変換
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int FromHex(string value)
        {
            int result = Convert.ToInt32(value, CommonUtils._HEX_BASE);
            return result;
        }

        ///// <summary>
        ///// 数値変換
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        ///// <remarks>16進表記と10進表記の両方に対応する</remarks>
        //public static uint ToUInt(string value)
        //{
        //    //値有効判定
        //    if (string.IsNullOrEmpty(value))
        //    {
        //        //無効の場合0を返す
        //        uint result = Mathf.Values.Zero.I;
        //        return result;
        //    }

        //    //VB6 - 16進数フォーマット判定
        //    {
        //        bool bVbHex = CommonUtils._TO_INT_VB6_HEX_FORMAT.IsMatch(value);
        //        if (bVbHex)
        //        {
        //            //VB6-16進数の場合先頭の&H(=2文字)削除してから変換
        //            string str = value.Remove(CommonConsts.Index.First, CommonUtils._TO_HEX_VB6_MARK_SIZE);
        //            uint result = Convert.ToUInt32(str, CommonUtils._HEX_BASE);
        //            return result;
        //        }
        //    }

        //    //16進数フォーマット判定
        //    {
        //        bool bHex = CommonUtils._TO_INT_HEX_FORMAT.IsMatch(value);
        //        if (bHex)
        //        {
        //            //16進数の場合
        //            uint result = Convert.ToUInt32(value, CommonUtils._HEX_BASE);
        //            return result;
        //        }
        //    }

        //    //異なる場合は10進数変換
        //    {
        //        uint result = Convert.ToUInt32(value);
        //        return result;
        //    }
        //}

        /// <summary>
        /// 数値変換
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks>16進表記と10進表記の両方に対応する</remarks>
        public static int ToInt(string value)
        {
            //値有効判定
            if (string.IsNullOrEmpty(value))
            {
                //無効の場合0を返す
                return 0;
            }

            //VB6 - 16進数フォーマット判定
            {
                bool bVbHex = CommonUtils._TO_INT_VB6_HEX_FORMAT.IsMatch(value);
                if (bVbHex)
                {
                    //VB6-16進数の場合先頭の&H(=2文字)削除してから変換
                    string str = value.Remove(CommonConsts.Index.First, CommonUtils._TO_HEX_VB6_MARK_SIZE);
                    int result = Convert.ToInt32(str, CommonUtils._HEX_BASE);
                    return result;
                }
            }

            //16進数フォーマット判定
            {
                bool bHex = CommonUtils._TO_INT_HEX_FORMAT.IsMatch(value);
                if (bHex)
                {
                    //16進数の場合
                    int result = Convert.ToInt32(value, CommonUtils._HEX_BASE);
                    return result;
                }
            }

            //異なる場合は10進数変換
            {
                int result = Convert.ToInt32(value);
                return result;
            }
        }

        /// <summary>
        /// 実数判定
        /// </summary>
        /// <param name="value"></param>
        /// <returns>TRUE=数字/FALSE=数字以外</returns>
        /// <remarks>符号や小数点も有効とする</remarks>
        public static bool IsNumeric(string value)
        {
            {
                bool isNull = string.IsNullOrEmpty(value);
                if (isNull)
                {
                    return false;
                }
            }
            bool result = double.TryParse(value, out _);
            return result;
        }

        /// <summary>
        /// 整数判定
        /// </summary>
        /// <param name="value"></param>
        /// <returns>TRUE=数字/FALSE=数字以外</returns>
        /// <remarks>符号は有効とする。小数点は無効</remarks>
        public static bool IsInteger(string value)
        {
            {
                bool isNull = string.IsNullOrEmpty(value);
                if (isNull)
                {
                    return false;
                }
            }
            bool result = int.TryParse(value, out _);
            return result;
        }

        /// <summary>
        /// 数字判定
        /// </summary>
        /// <remarks>IsNumber()用</remarks>
        private static readonly Regex _IS_NUMBER = new Regex("^[0-9]+$");

        /// <summary>
        /// 数字判定
        /// </summary>
        /// <param name="value"></param>
        /// <returns>TRUE=数字/FALSE=数字以外</returns>
        /// <remarks>数字以外の符号・小数点は無効</remarks>
        public static bool IsNumber(string value)
        {
            {
                bool isNull = string.IsNullOrEmpty(value);
                if (isNull)
                {
                    return false;
                }
            }
            bool result = CommonUtils._IS_NUMBER.IsMatch(value);
            return result;
        }

        /// <summary>
        /// 全角数字を作成
        /// </summary>
        /// <param name="value">数値</param>
        /// <returns></returns>
        public static string ToWideNumberString(int value)
        {
            //文字列化する
            string text = value.ToString();

            //全角化する
            string result = CommonUtils.ToWideNumberString(text);

            //終了
            return result;
        }

        /// <summary>
        /// 全角数字を作成
        /// </summary>
        /// <param name="value">数値</param>
        /// <returns></returns>
        public static string ToWideNumberString(decimal value)
        {
            //文字列化する
            string text = value.ToString();

            //全角化する
            string result = CommonUtils.ToWideNumberString(text);

            //終了
            return result;
        }

        /// <summary>
        /// 全角数字を作成
        /// </summary>
        /// <param name="value">数値</param>
        /// <returns></returns>
        public static string ToWideNumberString(double value)
        {
            //文字列化する
            string text = value.ToString();

            //全角化する
            string result = CommonUtils.ToWideNumberString(text);

            //終了
            return result;
        }

        /// <summary>
        /// 全角数字を作成
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string ToWideNumberString(string text)
        {
            //長さ取得
            int len = CommonUtils.GetLength(text); 

            //全角化
            StringBuilder sb = new StringBuilder(len);
            for (int i = CommonConsts.Index.First; i < len; i += CommonConsts.Index.Step)
            {
                char c = text[i];
                switch (c)
                {
                    case '0':
                        sb.Append("０");
                        break;
                    case '1':
                        sb.Append("１");
                        break;
                    case '2':
                        sb.Append("２");
                        break;
                    case '3':
                        sb.Append("３");
                        break;
                    case '4':
                        sb.Append("４");
                        break;
                    case '5':
                        sb.Append("５");
                        break;
                    case '6':
                        sb.Append("６");
                        break;
                    case '7':
                        sb.Append("７");
                        break;
                    case '8':
                        sb.Append("８");
                        break;
                    case '9':
                        sb.Append("９");
                        break;
                    case '-':
                        sb.Append("－");
                        break;
                    case '+':
                        sb.Append("＋");
                        break;
                    case '.':
                        sb.Append("．");
                        break;
                    default:
                        //ここには来ない
                        sb.Append("？");
                        break;
                }
            }

            //終了
            return sb.ToString();
        }

        /// <summary>
        /// コレクションの中からアイテム取得
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="col"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T? GetItem<T>(IList<T> col, int index)
        {
            //コレクション有効判定
            if (col == null)
            {
                //無効の場合デフォルト値で終了
                return default;
            }

            //コレクション数取得
            int n = col.Count;

            //インデックス有効判定
            if (index < CommonConsts.Index.First)
            {
                //最小値以下の場合デフォルト値で終了
                return default;
            }
            if (n <= index)
            {
                //最大値以下の場合デフォルト値で終了
                return default;
            }

            //値取得
            T result = col[index];

            //終了
            return result;
        }

        /// <summary>
        /// 指定されたインスタンスの指定プロパティ名のプロパティに値を設定
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetPropertyValue(object instance, string propertyName, object value)
        {
            //インスタンス有効判定
            if (instance == null)
            {
                //無効の場合終了
                //LOGGER.Warn("インスタンスが無効です");
                return false;
            }

            //インスタンスの型を取得
            Type t = instance.GetType();

            //プロパティ情報取得
            PropertyInfo? pi = t.GetProperty(propertyName);
            if (pi == null)
            {
                //取得出来ない場合終了
                //LOGGER.Warn($"対応するプロパティが見つかりません (Property Name='{propertyName}')");
                return false;
            }

            //設定
            try
            {
                pi.SetValue(instance, value);
            }
            catch(Exception /*ex*/)
            {
                //LOGGER.Warn($"プロパティへの設定で例外発生 (Property Name='{propertyName}')", ex);
                return false;
            }

            //終了
            return true;
        }

        /// <summary>
        /// コレクション文字列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string ToArrayString<T>(IEnumerable<T> values)
        {
            if(values == null)
            {
                return string.Empty;
            }
            else
            {
                string result = string.Join(",", values);
                return result;
            }
        }

        /// <summary>
        /// コレクション空判定
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        public static bool IsEmpty(System.Collections.ICollection col)
        {
            int n = CommonUtils.GetCount(col);
            bool result = (n <= CommonConsts.Count.Zero);
            return result;
        }

        /// <summary>
        /// 非キャンセル判定
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool IsNotCancel(System.Threading.CancellationToken token)
        {
            bool isCancel = CommonUtils.IsCancel(token);
            if (isCancel)
            {
                //成功(=キャンセル)の場合はfalseを返す
                return false;
            }
            else
            {
                //失敗(=非キャンセル)の場合はtrueを返す
                return true;
            }
        }

        /// <summary>
        /// キャンセル判定
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool IsCancel(System.Threading.CancellationToken token)
        {
            try
            {
                //キャンセル判定
                bool isCancel = token.IsCancellationRequested;
                if (isCancel)
                {
                    //キャンセルならtrueを返す
                    return true;
                }
                else
                {
                    //非キャンセルならfalseを返す
                    return false;
                }
            }
            catch (Exception)
            {
                //例外は握りつぶしてtrue(=キャンセル)を返す
                return true;
            }
        }

        /// <summary>
        /// 非キャンセル待ち
        /// </summary>
        /// <param name="token"></param>
        /// <param name="millisecondsTimeout"></param>
        /// <returns></returns>
        public static bool WaitNotCancel(System.Threading.CancellationToken token, int millisecondsTimeout)
        {
            bool isCancel = CommonUtils.WaitCancel(token, millisecondsTimeout);
            if (isCancel)
            {
                //成功(=キャンセル)の場合はfalseを返す
                return false;
            }
            else
            {
                //失敗(=非キャンセル)の場合はtrueを返す
                return true;
            }
        }

        /// <summary>
        /// キャンセル待ち
        /// </summary>
        /// <param name="token"></param>
        /// <param name="millisecondsTimeout"></param>
        /// <returns></returns>
        public static bool WaitCancel(System.Threading.CancellationToken token, int millisecondsTimeout)
        {
            try
            {
                bool result = token.WaitHandle.WaitOne(millisecondsTimeout);
                return result;
            }
            catch (Exception)
            {
                //例外(=ObjectDisposedException等)は握りつぶしてtrue(=キャンセル)を返す
                return true;
            }
        }

        /// <summary>
        /// キャンセル待ち
        /// </summary>
        /// <param name="token"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static bool WaitCancel(System.Threading.CancellationToken token, TimeSpan timeout)
        {
            try
            {
                bool result = token.WaitHandle.WaitOne(timeout);
                return result;
            }
            catch (Exception)
            {
                //例外(=ObjectDisposedException等)は握りつぶしてtrue(=キャンセル)を返す
                return true;
            }
        }

        /// <summary>
        /// タスク終了
        /// </summary>
        /// <param name="proc">処理名</param>
        /// <param name="task">停止するタスク</param>
        /// <param name="cancel">使用するキャンセルトークンソース</param>
        public static void SafeStopTask(string proc, System.Threading.Tasks.Task task, System.Threading.CancellationTokenSource cancel)
        {
            CommonUtils.SafeStopTask(proc, task, cancel, CommonConsts.Intervals.StandardTaskFinishTimeout);
        }

        /// <summary>
        /// タスク終了
        /// </summary>
        /// <param name="proc">処理名</param>
        /// <param name="task">停止するタスク</param>
        /// <param name="cancel">使用するキャンセルトークンソース</param>
        /// <param name="millisecond">停止待ち時間</param>
        public static void SafeStopTask(string proc, System.Threading.Tasks.Task task, System.Threading.CancellationTokenSource cancel, int millisecond)
        {
            try
            {
                using (cancel)
                {
                    //キャンセル
                    if (cancel == null)
                    {
                        //キャンセル無効の場合は何もしない
                        //LOGGER.Debug($"タスク終了 - {proc} - キャンセル無効");
                    }
                    else
                    {
                        //キャンセル実行
                        try
                        {
                            //LOGGER.Info($"タスク終了 - {proc} - キャンセル発行");
                            cancel.Cancel();
                        }
                        catch (Exception /*ex*/)
                        {
                            //LOGGER.Warn($"タスク終了 - {proc} - キャンセルで例外発生", ex);
                        }
                    }

                    //タスク停止待ち
                    if (task == null)
                    {
                        //キャンセル無効の場合は何もしない
                        //LOGGER.Debug($"タスク終了 - {proc} - タスク無効");
                    }
                    else
                    {
                        //タスク停止待ち
                        try
                        {
                            //LOGGER.Info($"タスク終了 - {proc} - タスク停止待ち - 開始");
                            task.Wait(millisecond);
                            //LOGGER.Info($"タスク終了 - {proc} - タスク停止待ち - 完了");
                        }
                        catch (Exception /*ex*/)
                        {
                            //LOGGER.Warn($"タスク終了 - {proc} - タスク停止待ちで例外発生", ex);
                        }
                    }
                }
            }
            catch (Exception /*ex*/)
            {
                //LOGGER.Warn($"タスク終了 - {proc} - 例外発生", ex);
            }
        }

        /// <summary>
        /// 否定値を返す
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Not(bool value)
        {
            bool result = !value;
            return result;
        }


        /// <summary>
        /// TimeSpanを文字列化
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static string ToSpanText(TimeSpan span)
        {
            int d = span.Days;
            int h = span.Hours;
            int m = span.Minutes;
            int s = span.Seconds;
            int ms = span.Milliseconds;
            int dh = (d * CommonConsts.Intervals.HourOfOneDay) + h;

            //時間のフラグ
            int hf = 0x01 << 3;
            //分のフラグ
            int mf = 0x01 << 2;
            //秒のフラグ
            int sf = 0x01 << 1;
            //ミリのフラグ
            int msf = 0x01 << 0;

            //表示フラグ
            int dispFlag = 0;

            //時間判定
            if (CommonConsts.Intervals.Zero < dh)
            {
                //有効の場合はフラグ加算
                dispFlag += hf;
            }
            else
            {
                //無効の場合は何もしない
            }

            //分判定
            if (CommonConsts.Intervals.Zero < m)
            {
                //有効の場合はフラグ加算
                dispFlag += mf;
            }
            else
            {
                //無効の場合は何もしない
            }

            //秒判定
            if (CommonConsts.Intervals.Zero < s)
            {
                //有効の場合はフラグ加算
                dispFlag += sf;
            }
            else
            {
                //無効の場合は何もしない
            }

            //ミリ秒判定
            if (CommonConsts.Intervals.Zero < ms)
            {
                //有効の場合はフラグ加算
                dispFlag += msf;
            }
            else
            {
                //無効の場合は何もしない
            }

            //文字列化(=最大のフラグから最小のフラグの範囲を表示する)
            // 1... Hpur
            // .1.. Minute
            // ..1. Second
            // ...1 Millisecond
            // 0000= 0 ... 「0秒」と表記
            // 0001= 1 ... xxミリ秒
            // 0010= 2 ... xx秒
            // 0011= 3 ... xx秒 xxミリ秒
            // 0100= 4 ... xx分
            // 0101= 5 ... xx分 xx秒 xxミリ秒
            // 0110= 6 ... xx分 xx秒
            // 0111= 7 ... xx分 xx秒 xxミリ秒
            // 1000= 8 ... xx時間
            // 1001= 9 ... xx時間 xx分 xx秒 xxミリ秒
            // 1010=10 ... xx時間 xx分 xx秒
            // 1011=11 ... xx時間 xx分 xx秒 xxミリ秒
            // 1100=12 ... xx時間 xx分
            // 1101=13 ... xx時間 xx分 xx秒 xxミリ秒
            // 1110=14 ... xx時間 xx分 xx秒
            // 1111=15 ... xx時間 xx分 xx秒 xxミリ秒
            switch (dispFlag)
            {
                case 1:
                    return $"{ms}ミリ秒";

                case 2:
                    return $"{s}秒";

                case 3:
                    return $"{s}秒 {ms:0000}ミリ秒";

                case 4:
                    return $"{m}分";

                case 5:
                case 7:
                    return $"{m}分 {s:00}秒 {ms:0000}ミリ秒";

                case 6:
                    return $"{m}分 {s:00}秒";

                case 8:
                    return $"{dh}時間";

                case 9:
                case 11:
                case 13:
                case 15:
                    return $"{dh}時間 {m:00}分 {s:00}秒 {ms:0000}ミリ秒";

                case 10:
                case 14:
                    return $"{dh}時間 {m:00}分 {s:00}秒";

                case 12:
                    return $"{dh}時間 {m:00}分";

                default:
                    //不明(=0の場合)
                    return "0秒";
            }
        }
    
        /// <summary>
        /// コマンドライン引数を取得
        /// </summary>
        /// <param name="argPrefix">コマンドライン引数の前置詞</param>
        /// <returns></returns>
        public static string? GetCommandLineArgsValue(string argPrefix)
        {
            try
            {
                //正規表現生成
                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex($"^/{argPrefix}:(?<value>.+?)$", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                //コマンドライン引数を取得
                string[] args = Environment.GetCommandLineArgs();
                int n = CommonUtils.GetCount(args);
                // >> 先頭は常に実行ファイルが入っているので2番目から処理する
                for (int i = CommonConsts.Index.Second; i < n; i += CommonConsts.Index.Step)
                {
                    string txt = args[i];
                    bool isNull = string.IsNullOrEmpty(txt);
                    if (isNull)
                    {
                        //空文字の場合は何もしない
                        // >> 正規表現で例外になるので念のためチェック
                    }
                    else
                    {
                        System.Text.RegularExpressions.Match m = reg.Match(txt);
                        if (m.Success)
                        {
                            //成功した場合は結果を返す
                            string result = m.Groups["value"].Value;
                            return result;
                        }
                        else
                        {
                            //失敗した場合は処理続行
                        }
                    }
                }

                //ここまで来たら見つからない
                return null;

            }
            catch (Exception /*ex*/)
            {
                //例外は握りつぶしてnullを返す
                //LOGGER.Warn("コマンドライン引数を取得で例外発生", ex);
                return null;
            }
        }

        /// <summary>
        /// ガンマ補正
        /// </summary>
        /// <param name="valueIn">入力値</param>
        /// <param name="gamma">ガンマ値</param>
        /// <returns></returns>
        public static double CorrectGamma(double valueIn, double gamma)
        {
            //ガンマ補正
            // >> Vout = A * Vin^gamma (A=1)
            double valueOut = Math.Pow(valueIn, gamma);
            return valueOut;
        }

        /// <summary>
        /// ガンマ補正(8bit)
        /// </summary>
        /// <param name="value">入力値</param>
        /// <param name="gamma">ガンマ値</param>
        /// <returns></returns>
        public static int CorrectGamma8bit(int value, double gamma)
        {
            //入力値を0～1に変換
            double valueInClamp = value / (double)CommonConsts.Palettes.Colors.AlphaMax;

            //ガンマ補正
            double valueOutClamp = CommonUtils.CorrectGamma(valueInClamp, gamma);

            //出力値を0～255に変換
            double valueOut = (valueOutClamp * CommonConsts.Palettes.Colors.AlphaMax);

            //出力値を整数に丸める
            double valueOutRound = valueOut + CommonConsts.Gammas.RoundOffValue;
            int result = (int)valueOutRound;

            //出力値有効判定
            // >> 最小値以上判定
            if (result < CommonConsts.Palettes.Colors.Min8bit)
            {
                //最小値未満の場合、最小値に引き上げる
                return CommonConsts.Palettes.Colors.Min8bit;
            }
            else
            {
                //有効の場合は処理続行
            }
            // >> 最大値以下判定
            if (CommonConsts.Palettes.Colors.Max8bit < value)
            {
                //最大値超過の場合、最大値に引き下げる
                return CommonConsts.Palettes.Colors.Max8bit;
            }
            else
            {
                //有効の場合は処理続行
            }

            //ここまできたら0～255の有効な整数
            return result;
        }

        /// <summary>
        /// COM解放処理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <remarks>安全にCOMを解放します</remarks>
        public static void SafeComRelease<T>(T? obj) where T : class
        {
            try
            {
                //インスタンス有効判定
                if (obj == null)
                {
                    //NULLの場合は何もしない
                    return;
                }
                else
                {
                    //非NULLの場合処理続行
                }

                //非NULLの場合COMオブジェクトか判定
                bool isCom = System.Runtime.InteropServices.Marshal.IsComObject(obj);
                if (isCom)
                {
                    //COMならReleaseComObject()する
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                }
                else
                {
                    //非COMの場合Disposableを実装しているか判定
                    // >> COMオブジェクトだと誤認して呼び出している可能性があるので
                    // >> 一応解放します
                    IDisposable? disposable = obj as IDisposable;
                    if (disposable == null)
                    {
                        //Disposableを実装していない場合何もしない
                    }
                    else
                    {
                        //Disposableを実装している場合Disposeを呼ぶ
                        disposable.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                //例外は握りつぶす
                CommonLogger.Warn("COM解放処理で例外発生", ex);
            }
        }

    }
}
