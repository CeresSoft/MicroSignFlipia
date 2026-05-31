using System.Configuration.Internal;
using System.Diagnostics.SymbolStore;

namespace MicroSign.Core
{
    /// <summary>
    /// 共通定義
    /// </summary>
    public static class CommonConsts
    {
        /// <summary>
        /// 要素数
        /// </summary>
        public static class Collection
        {
            /// <summary>
            /// 空
            /// </summary>
            public const int Empty = 0;

            /// <summary>
            /// １要素
            /// </summary>
            public const int One = 1;

            /// <summary>
            /// 要素加算
            /// </summary>
            public const int Step = 1;

            /// <summary>
            /// 要素数をインデックスに変換
            /// </summary>
            /// <param name="i"></param>
            /// <returns></returns>
            public static int ToIndex(int i)
            {
                int result = i - 1;
                return result;
            }
        }

        /// <summary>
        /// インデックス
        /// </summary>
        public static class Index
        {
            /// <summary>
            /// 開始インデックス
            /// </summary>
            public const int First = 0;

            /// <summary>
            /// ２番目インデックス
            /// </summary>
            public const int Second = First + Step;

            /// <summary>
            /// インデックスステップ値
            /// </summary>
            public const int Step = 1;

            /// <summary>
            /// 無効値
            /// </summary>
            public const int Invalid = -1;

            /// <summary>
            /// インデックスを要素数に変換
            /// </summary>
            /// <param name="i"></param>
            /// <returns></returns>
            public static int ToCollection(int i)
            {
                int result = i + 1;
                return result;
            }

            /// <summary>
            /// インデックスをカウントに変換
            /// </summary>
            /// <param name="i"></param>
            /// <returns></returns>
            public static int ToCount(int i)
            {
                int result = i + 1;
                return result;
            }
        }

        /// <summary>
        /// カウント
        /// </summary>
        /// <remarks>1から始まるものに使う</remarks>
        public static class Count
        {
            /// <summary>
            /// カウントゼロ
            /// </summary>
            public const int Zero = 0;

            /// <summary>
            /// カウント初期値
            /// </summary>
            public const int First = 1;

            /// <summary>
            /// カウントステップ値
            /// </summary>
            public const int Step = 1;

            /// <summary>
            /// カウントをインデックスに変換
            /// </summary>
            /// <param name="i"></param>
            /// <returns></returns>
            public static int ToIndex(int i)
            {
                int result = i - 1;
                return result;
            }
        }

        /// <summary>
        /// 比較値
        /// </summary>
        public static class CompareValue
        {
            /// <summary>
            /// 小さい
            /// </summary>
            public const int Less = -1;

            /// <summary>
            ///同値
            /// </summary>
            public const int Same = 0;

            /// <summary>
            /// 大きい
            /// </summary>
            public const int Greater = 1;
        }

        /// <summary>
        /// 間隔(ms)
        /// </summary>
        public static class Intervals
        {
            /// <summary>
            /// 0秒/0ms
            /// </summary>
            public const int Zero = 0;

            /// <summary>
            /// 1秒
            /// </summary>
            public const int OneSec = 1000;

            /// <summary>
            /// 間隔なし
            /// </summary>
            public const int Infinity = -1;

            /// <summary>
            /// 標準タスク終了待ち時間
            /// </summary>
            public const int StandardTaskFinishTimeout = 1 * OneSec;

            /// <summary>
            /// 1日の時間
            /// </summary>
            public const int HourOfOneDay = 24;

            /// <summary>
            /// 1時間の分
            /// </summary>
            public const int MinuteOfOneHour = 60;

            /// <summary>
            /// 1時間の分
            /// </summary>
            public const int SecondOfOneMinute = 60;
        }

        /// <summary>
        /// フラグ
        /// </summary>
        public class FlagValue
        {
            /// <summary>
            /// True値
            /// </summary>
            public const int TRUE = 1;

            /// <summary>
            /// False値
            /// </summary>
            public const int FALSE = 0;

            /// <summary>
            /// boolからフラグ値に変換
            /// </summary>
            /// <param name="value">bool値</param>
            /// <returns></returns>
            public static int ToFlagValue(bool value)
            {
                if(value)
                {
                    //TRUEの場合
                    return FlagValue.TRUE;
                }
                else
                {
                    //FALSEの場合
                    return FlagValue.FALSE;
                }
            }

            /// <summary>
            /// フラグ値からboolに変換
            /// </summary>
            /// <param name="value">フラグ値</param>
            /// <returns></returns>
            public static bool FromFalgValue(int value)
            {
                if (value == FlagValue.FALSE)
                {
                    //FALSEの場合
                    return false;
                }
                else
                {
                    //FALSE以外の場合
                    return true;
                }
            }
        }

        /// <summary>
        /// テキスト
        /// </summary>
        public static class Text
        {
            /// <summary>
            /// StringBuilderキャパシティ
            /// </summary>
            public const int STRING_BUILDER_CAPACITY = 4096;

            /// <summary>
            /// シフトJISエンコーディング
            /// </summary>
            public static readonly System.Text.Encoding SJIS = System.Text.Encoding.GetEncoding("Shift_JIS");
        }

        /// <summary>
        /// フォーマット
        /// </summary>
        public static class Format
        {
            /// <summary>
            /// 日付フォーマット
            /// </summary>
            public static readonly string DateTimeFormat = "yyyy/MM/dd HH:mm:ss";

            /// <summary>
            /// 日付フォーマット(ミリ秒あり)
            /// </summary>
            public static readonly string DateTimeLongFormat = "yyyy/MM/dd HH:mm:ss.fff";
        }

        /// <summary>
        /// ビット数
        /// </summary>
        public static class BitCount
        {
            /// <summary>
            /// バイトの半分(16進数1桁)のビット数
            /// </summary>
            public const int HARF = 4;

            /// <summary>
            /// バイトのビット数
            /// </summary>
            public const int BYTE = 8;

            /// <summary>
            /// ワードのビット数
            /// </summary>
            public const int WORD = 16;

            /// <summary>
            /// DWORDのビット数
            /// </summary>
            public const int DWORD = 32;
        }

        /// <summary>
        /// バイト数
        /// </summary>
        public static class ByteCount
        {
            /// <summary>
            /// byteのバイト数
            /// </summary>
            public const int BYTE = 1;

            /// <summary>
            /// shortのバイト数
            /// </summary>
            public const int SHORT = 2;

            /// <summary>
            /// ushortのバイト数
            /// </summary>
            public const int USHORT = 2;

            /// <summary>
            /// intのバイト数
            /// </summary>
            public const int INT = 4;

            /// <summary>
            /// uintのバイト数
            /// </summary>
            public const int UINT = 4;
        }

        /// <summary>
        /// アスキーコード
        /// </summary>
        public static class ASCII
        {
            /// <summary>
            /// NUL文字
            /// </summary>
            public const char NUL = (char)0x00;

            /// <summary>
            /// 改行
            /// </summary>
            public const char LF = (char)0x0A;

            /// <summary>
            /// 
            /// </summary>
            public const char CR = (char)0x0D;

            /// <summary>
            /// STX
            /// </summary>
            public const char STX = '\x02';

            /// <summary>
            /// EOT
            /// </summary>
            public const char EOT = '\x04';

            /// <summary>
            /// ACK
            /// </summary>
            public const char ACK = '\x06';

            /// <summary>
            /// ETB
            /// </summary>
            public const char ETB = '\x17';
        }

        /// <summary>
        /// DPI
        /// </summary>
        public static class DPIs
        {
            /// <summary>
            /// ポイントのサイズ(1/72)
            /// </summary>
            public const double Point = 72;

            /// <summary>
            /// DIP(=WPF:デバイス非依存ピクセル)のサイズ(1/96)
            /// </summary>
            public const double DIP = 96;

            /// <summary>
            /// ポイントをDIPに変換
            /// </summary>
            /// <param name="pt"></param>
            /// <returns></returns>
            public static double FromPoint(double pt)
            {
                double result = pt * CommonConsts.DPIs.DIP;
                result /= CommonConsts.DPIs.Point;
                return result;
            }
        }

        /// <summary>
        /// ソケットポート
        /// </summary>
        public static class SocketPorts
        {
            /// <summary>
            /// 最小値
            /// </summary>
            /// <remarks>
            /// 本来は0も有効な値で、この場合動的にポート割付される
            /// しかしTCPサーバで動的にポート割付されてもクライアント側が
            /// 困るので1からにします
            /// </remarks>
            public const int MinValue = 1;

            /// <summary>
            /// 最大値
            /// </summary>
            public const int MaxValue = 65_535;
        }

        /// <summary>
        /// 値
        /// </summary>
        public static class Values
        {
            /// <summary>
            /// 限りなくゼロ
            /// </summary>
            /// <remarks>
            /// System.Double.Epsilon = 4.94065645841247E-324
            /// System.Single.Epsilon = 1.401298E-45;
            /// と、どちらもあまり使わないような小さな値なので
            /// ここで常用的な最小値を定義します
            /// </remarks>
            public static class Epsilon
            {
                /// <summary>
                /// double ≒ ゼロ
                /// </summary>
                public const double D = 0.0001;

                /// <summary>
                /// float ≒ ゼロ
                /// </summary>
                public const float F = 0.0001F;
            }

            /// <summary>
            /// ゼロ値
            /// </summary>
            public static class Zero
            {
                /// <summary>
                /// int 0
                /// </summary>
                public const int I = 0;

                /// <summary>
                /// double 0
                /// </summary>
                public const double D = 0;
            }

            /// <summary>
            /// 1値
            /// </summary>
            public static class One
            {
                /// <summary>
                /// int 1
                /// </summary>
                public const int I = 1;

                /// <summary>
                /// double 0
                /// </summary>
                public const double D = 1;
            }

            /// <summary>
            /// 10値
            /// </summary>
            public static class Ten
            {
                /// <summary>
                /// int 1
                /// </summary>
                public const int I = 10;

                /// <summary>
                /// double 0
                /// </summary>
                public const double D = 10;
            }

            /// <summary>
            /// サイズ
            /// </summary>
            public static class Size
            {
                /// <summary>
                /// 1024
                /// </summary>
                public const int K = 1024;

                /// <summary>
                /// 1024 * 1024
                /// </summary>
                public const int M = 1024 * 1024;
            }

            /// <summary>
            /// -1
            /// </summary>
            public static class NegativeOne
            {
                /// <summary>
                /// intの-1.0
                /// </summary>
                public const int I = -1;

                /// <summary>
                /// doubeの-1.0
                /// </summary>
                public const double D = I;

                /// <summary>
                /// decimalの-1.0
                /// </summary>
                public const decimal M = I;
            }
        }

        /// <summary>
        /// 頂点
        /// </summary>
        public static class Points
        {
            /// <summary>
            /// ゼロ点
            /// </summary>
            public static System.Windows.Point Zero = new System.Windows.Point(0, 0);
        }

        /// <summary>
        /// 時間関連
        /// </summary>
        public static class Time
        {
            /// <summary>
            /// GIFの遅延時間単位(1単位=10ms)
            /// </summary>
            public const int GifUnitTime = 10;

            /// <summary>
            /// ゼロ値
            /// </summary>
            public const int Zero = 0;
        }

        /// <summary>
        /// ファイル関連
        /// </summary>
        public static class File
        {
            /// <summary>
            /// ゼロプレース
            /// </summary>
            public static readonly string ZeroPrace = "0";

            /// <summary>
            /// PNGファイルフォーマット
            /// </summary>
            public static readonly string PngFileFormat = "{0}.png";

            /// <summary>
            /// GIFファイル拡張子
            /// </summary>
            /// <remarks>
            /// switch分岐で使用するためconstにします
            /// </remarks>
            public const string GifExtension = ".gif";

            /// <summary>
            /// MP4ファイル拡張子
            /// </summary>
            /// <remarks>
            /// 2026.05.31:CS)杉原:MP4対応
            /// switch分岐で使用するためconstにします
            /// </remarks>
            public const string MP4Extension = ".mp4";
        }

        /// <summary>
        /// パレット関連
        /// </summary>
        /// <remarks>
        /// 2025.08.11:CS)杉原:インデックスカラー対応修正
        /// </remarks>
        public static class  Palettes
        {
            /// <summary>
            /// パレット数
            /// </summary>
            public static class Count
            {
                /// <summary>
                /// 空
                /// </summary>
                public static readonly int Zero = 0;

                /// <summary>
                /// 最小値
                /// </summary>
                public static readonly int Min = 1;

                /// <summary>
                /// 最大値
                /// </summary>
                public static readonly int Max = 256;
            }

            /// <summary>
            /// 色関連
            /// </summary>
            public static class Colors
            {
                /// <summary>
                /// アルファの最大値
                /// </summary>
                public static readonly int AlphaMax = 255;

                /// <summary>
                /// 8bitカラーの最小値
                /// </summary>
                public static readonly int Min8bit = 0;

                /// <summary>
                /// 8bitカラーの最大値
                /// </summary>
                public static readonly int Max8bit = 255;
            }

        }

        /// <summary>
        /// ガンマ補正関連
        /// </summary>
        /// <remarks>
        /// 2025.08.18:CS)土田:ガンマ補正対応で追加
        /// </remarks>
        public static class Gammas
        {
            /// <summary>
            /// 倍率
            /// </summary>
            /// <remarks>
            /// 整数から小数へ変換するための係数
            /// </remarks>
            public static readonly double Magnification = 100.0;

            /// <summary>
            /// 四捨五入の値
            /// </summary>
            public static readonly double RoundOffValue = 0.5;
        }
    }
}
