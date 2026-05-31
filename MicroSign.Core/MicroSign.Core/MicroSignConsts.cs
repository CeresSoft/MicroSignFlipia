using System;

namespace MicroSign.Core
{
    /// <summary>
    /// 定数
    /// </summary>
    public static class MicroSignConsts
    {
        /// <summary>
        /// パス
        /// </summary>
        public static class Path
        {
            /// <summary>
            /// マトリクスLED画像パス
            /// </summary>
            /// <remarks>ESP32のMatrixLedImageSD側のファイル名が「MicroSignImage.bin」で固定なので固定にします</remarks>
            //2025.10.03:CS)土田:変換結果の保存先を選択できるように変更 >>>>> ここから
            //public const string MatrixLedImagePath = @".\Temp\MicroSignImage.bin";
            //----------
            public const string MatrixLedImagePath = @".\Temp\" + MatrixLedImageFileName;

            /// <summary>
            /// マトリクスLED画像ファイル名
            /// </summary>
            public const string MatrixLedImageFileName = @"MicroSignImage.bin";
            //2025.10.03:CS)土田:変換結果の保存先を選択できるように変更 <<<<< ここまで

            /// <summary>
            /// マトリクスLEDパネル設定パス
            /// </summary>
            /// <remarks>
            /// ESP32のMatrixLedImageSD側のファイル名が「PanelConfig.bin」で固定なので固定にします
            /// この後SPIFFSに変換するのでDatasフォルダ配下になるように出力します
            /// </remarks>
            public const string MatrixLedPanelConfigPath = @".\Temp\Datas\PanelConfig.bin";

            /// <summary>
            /// SPIFFS生成パス
            /// </summary>
            public const string SPIFFSPath = @".\Temp\SPIFFS.bin";

            /// <summary>
            /// ログ領域に保存する連結した画像のファイル名
            /// </summary>
            /// <remarks>
            /// log4netのログ出力先が「${USERPROFILE}\MicroSign\Log\log.txt」なので
            /// ユーザプロファイル配下の.\MicroSign\Logに保存します
            /// ログディレクトリはlog4netから取得するのでファイル名だけ指定します
            /// </remarks>
            public const string LogAnimationImageFilename = @"AnimationImage.png";

            /// <summary>
            /// ログ領域に保存する減色した画像のファイル名
            /// </summary>
            /// <remarks>
            /// log4netのログ出力先が「${USERPROFILE}\MicroSign\Log\log.txt」なので
            /// ユーザプロファイル配下の.\MicroSign\Logに保存します
            /// ログディレクトリはlog4netから取得するのでファイル名だけ指定します
            /// </remarks>
            public const string LogColorReductionImageFilename = @"ColorReductionImage.png";
        }

        /// <summary>
        /// 待ち時間
        /// </summary>
        public static class WaitTimes
        {
            /// <summary>
            /// SPIFFS生成の場合最大5分待つ
            /// </summary>
            public static readonly TimeSpan MKSPIFFS = TimeSpan.FromMinutes(5);

            /// <summary>
            /// ESPTOOLの場合最大5分待つ
            /// </summary>
            public static readonly TimeSpan ESPTOOL = TimeSpan.FromMinutes(5);
        }

        /// <summary>
        /// プロセス終了コード
        /// </summary>
        public static class ExitCodes
        {
            /// <summary>
            /// プロセス正常終了
            /// </summary>
            public const int Success = 0;
        }

        /// <summary>
        /// 表示期間
        /// </summary>
        public static class DisplayPeriods
        {
            /// <summary>
            /// 最小値
            /// </summary>
            public const int Min = UInt16.MinValue;

            /// <summary>
            /// 最大値
            /// </summary>
            /// <remarks>データサイズの都合でuint16で表現できる時間までにする</remarks>
            public const int Max = UInt16.MaxValue;
        }

        /// <summary>
        /// RGB
        /// </summary>
        public static class RGB
        {
            /// <summary>
            /// 黒色
            /// </summary>
            public const int Black = 0;

            /// <summary>
            /// 0-3の2ビット色
            /// </summary>
            public const int Bit2 = 2;

            /// <summary>
            /// 0-7の3ビット色
            /// </summary>
            public const int Bit3 = 3;

            /// <summary>
            /// 0-255の8ビット色
            /// </summary>
            public const int Bit8 = 8;

            /// <summary>
            /// 64色カラー変換メンバー名
            /// </summary>
            public const string Color64MemberName = "Data64";

            /// <summary>
            /// 256色カラー変換メンバー名
            /// </summary>
            public const string Color256MemberName = "Data256";

            /// <summary>
            /// インデックスカラー変換メンバー名
            /// </summary>
            public const string IndexColorMemberName = "DataIndex";

            /// <summary>
            /// インデックスカラー最大色数
            /// </summary>
            public const int MaxIndexColorCount = 256;

            /// <summary>
            /// 明るさ
            /// </summary>
            public static class Brightness
            {
                /// <summary>
                /// 明るさ最小値
                /// </summary>
                public const int Min = 0;

                /// <summary>
                /// 明るさ最大値(255)
                /// </summary>
                public const int Max255 = 255;
            }
        }

        public static class MatrixLed
        {
            /// <summary>
            /// HUB75
            /// </summary>
            public static class HUB75
            {
                /// <summary>
                /// 行アドレスサイズ
                /// </summary>
                /// <remarks>
                /// A,B,C,Dの4ビットなので0～15の16行です
                /// </remarks>
                public const int RowSize = 16;

                /// <summary>
                /// 行のパックサイズ
                /// </summary>
                /// <remarks>
                /// 上段と下段の2行
                /// </remarks>
                public const int RowPackSize = 2;
            }

            /// <summary>
            /// HUB75 EX
            /// </summary>
            public static class HUB75_EX
            {
                /// <summary>
                /// 行アドレスサイズ
                /// </summary>
                /// <remarks>
                /// A,B,C,D,Eの5ビットなので0～31の32行です
                /// </remarks>
                public const int RowSize = 32;

                /// <summary>
                /// 行のパックサイズ
                /// </summary>
                /// <remarks>
                /// 上段と下段の2行
                /// </remarks>
                public const int RowPackSize = 2;
            }

        }

        /// <summary>
        /// MicroSignアニメーションファイル向けバージョン
        /// </summary>
        public static class Versions
        {
            /// <summary>
            /// version 1.0.0
            /// </summary>
            public const int V100 = 100;

            /// <summary>
            /// version 1.1.0
            /// </summary>
            public const int V110 = 110;

            /// <summary>
            /// version 1.2.0
            /// </summary>
            /// <remarks>2026.05.22:CS)杉原:サウンド機能追加</remarks>
            public const int V120 = 120;
        }

        /// <summary>
        /// パネル設定
        /// </summary>
        public static class PanelConfig
        {
            /// <summary>
            /// パネル設定向けバージョン
            /// </summary>
            public static class Versions
            {
                /// <summary>
                /// version 1.0.0
                /// </summary>
                public const int V100 = 100;
            }
        }

        /// <summary>
        /// ファイル出力時のデータ
        /// </summary>
        public static class FileData
        {
            /// <summary>
            /// 空き
            /// </summary>
            public static readonly int Reserve = 0;
        }

        /// <summary>
        /// アニメーション
        /// </summary>
        public static class Animations
        {
            /// <summary>
            /// セルサイズ
            /// </summary>
            public static class CellSize
            {
                /// <summary>
                /// サイズ固定セル(+0=X, +1=Y, +2=表示期間 =3)
                /// </summary>
                public const int FixedSize = 3;
            }
        }

        /// <summary>
        /// サウンド
        /// </summary>
        public static class Sounds
        {
            /// <summary>
            /// PCMフォーマット
            /// </summary>
            public static class PcmFormat
            {
                /// <summary>
                /// サンプルレート
                /// </summary>
                public const int SampleRate = 44_100;

                /// <summary>
                /// サンプルビット数
                /// </summary>
                public const int BitsPerSample = 16;

                /// <summary>
                /// チャンネル数
                /// </summary>
                public const int Channels = 2;
            }

            /// <summary>
            /// 再サンプル読込バッファサイズ
            /// </summary>
            public const int ResampleReadBuffSize = 4096;
        }

        /// <summary>
        /// クリップ処理向け定義
        /// </summary>
        public class Clip
        {
            /// <summary>
            /// デフォルト倍率
            /// </summary>
            public const double DefaultScale = 1;

            /// <summary>
            /// デフォルトX始点
            /// </summary>
            public const int DefaultX = 0;

            /// <summary>
            /// デフォルトY始点
            /// </summary>
            public const int DefaultY = 0;
        }
    }
}
