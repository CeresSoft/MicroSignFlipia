using System.Windows.Media.Imaging;

namespace MicroSign.Core.ViewModels.Pages
{
    partial class Mp4ClipPageViewModel
    {
        // >>>>> ここから
        #region マトリクスLED横幅
        /// <summary>
        /// 初期値
        /// </summary>
        public static new partial class InitializeValues
        {
            /// <summary>
            /// マトリクスLED横幅初期値
            /// </summary>
            public const int PanelWidth = 128;
        }

        /// <summary>
        /// プロパティ名
        /// </summary>
        public static new partial class PropertyNames
        {
            /// <summary>
            /// マトリクスLED横幅プロパティ名
            /// </summary>
            public const string PanelWidth = "PanelWidth";
        }

        /// <summary>
        /// マトリクスLED横幅保持変数
        /// </summary>
        protected int _PanelWidth = InitializeValues.PanelWidth;

        /// <summary>
        /// マトリクスLED横幅
        /// </summary>
        public int PanelWidth
        {
            get
            {
                return this._PanelWidth;
            }
            set
            {
                int now = this._PanelWidth;
                if (now == value)
                {
                    return;
                }
                this._PanelWidth = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion
        #region マトリクスLED縦幅
        /// <summary>
        /// 初期値
        /// </summary>
        public static new partial class InitializeValues
        {
            /// <summary>
            /// マトリクスLED縦幅初期値
            /// </summary>
            public const int PanelHeight = 32;
        }

        /// <summary>
        /// プロパティ名
        /// </summary>
        public static new partial class PropertyNames
        {
            /// <summary>
            /// マトリクスLED縦幅プロパティ名
            /// </summary>
            public const string PanelHeight = "PanelHeight";
        }

        /// <summary>
        /// マトリクスLED縦幅保持変数
        /// </summary>
        protected int _PanelHeight = InitializeValues.PanelHeight;

        /// <summary>
        /// マトリクスLED縦幅
        /// </summary>
        public int PanelHeight
        {
            get
            {
                return this._PanelHeight;
            }
            set
            {
                int now = this._PanelHeight;
                if (now == value)
                {
                    return;
                }
                this._PanelHeight = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion
        #region ビデオ画像
        /// <summary>
        /// 初期値
        /// </summary>
        public static new partial class InitializeValues
        {
            /// <summary>
            /// ビデオ画像初期値
            /// </summary>
            public const WriteableBitmap? VideoImage = null;
        }

        /// <summary>
        /// プロパティ名
        /// </summary>
        public static new partial class PropertyNames
        {
            /// <summary>
            /// ビデオ画像プロパティ名
            /// </summary>
            public const string VideoImage = "VideoImage";
        }

        /// <summary>
        /// ビデオ画像保持変数
        /// </summary>
        protected WriteableBitmap? _VideoImage = InitializeValues.VideoImage;

        /// <summary>
        /// ビデオ画像
        /// </summary>
        public WriteableBitmap? VideoImage
        {
            get
            {
                return this._VideoImage;
            }
            set
            {
                WriteableBitmap? now = this._VideoImage;
                if (now == value)
                {
                    return;
                }
                this._VideoImage = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion
        #region ビデオ横幅
        /// <summary>
        /// 初期値
        /// </summary>
        public static new partial class InitializeValues
        {
            /// <summary>
            /// ビデオ横幅初期値
            /// </summary>
            public const int VideoWidth = 0;
        }

        /// <summary>
        /// プロパティ名
        /// </summary>
        public static new partial class PropertyNames
        {
            /// <summary>
            /// ビデオ横幅プロパティ名
            /// </summary>
            public const string VideoWidth = "VideoWidth";
        }

        /// <summary>
        /// ビデオ横幅保持変数
        /// </summary>
        protected int _VideoWidth = InitializeValues.VideoWidth;

        /// <summary>
        /// ビデオ横幅
        /// </summary>
        public int VideoWidth
        {
            get
            {
                return this._VideoWidth;
            }
            set
            {
                int now = this._VideoWidth;
                if (now == value)
                {
                    return;
                }
                this._VideoWidth = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion
        #region ビデオ縦幅
        /// <summary>
        /// 初期値
        /// </summary>
        public static new partial class InitializeValues
        {
            /// <summary>
            /// ビデオ縦幅初期値
            /// </summary>
            public const int VideoHeight = 0;
        }

        /// <summary>
        /// プロパティ名
        /// </summary>
        public static new partial class PropertyNames
        {
            /// <summary>
            /// ビデオ縦幅プロパティ名
            /// </summary>
            public const string VideoHeight = "VideoHeight";
        }

        /// <summary>
        /// ビデオ縦幅保持変数
        /// </summary>
        protected int _VideoHeight = InitializeValues.VideoHeight;

        /// <summary>
        /// ビデオ縦幅
        /// </summary>
        public int VideoHeight
        {
            get
            {
                return this._VideoHeight;
            }
            set
            {
                int now = this._VideoHeight;
                if (now == value)
                {
                    return;
                }
                this._VideoHeight = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion

        #region 最小スケール
        /// <summary>
        /// 初期値
        /// </summary>
        public static new partial class InitializeValues
        {
            /// <summary>
            /// 最小スケール初期値
            /// </summary>
            public const double MinScale = 1;
        }

        /// <summary>
        /// プロパティ名
        /// </summary>
        public static new partial class PropertyNames
        {
            /// <summary>
            /// 最小スケールプロパティ名
            /// </summary>
            public const string MinScale = "MinScale";
        }

        /// <summary>
        /// 最小スケール保持変数
        /// </summary>
        protected double _MinScale = InitializeValues.MinScale;

        /// <summary>
        /// 最小スケール
        /// </summary>
        public double MinScale
        {
            get
            {
                return this._MinScale;
            }
            set
            {
                double now = this._MinScale;
                if (now == value)
                {
                    return;
                }
                this._MinScale = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion
        #region 最大スケール
        /// <summary>
        /// 初期値
        /// </summary>
        public static new partial class InitializeValues
        {
            /// <summary>
            /// 最大スケール初期値
            /// </summary>
            public const double MaxScale = 1;
        }

        /// <summary>
        /// プロパティ名
        /// </summary>
        public static new partial class PropertyNames
        {
            /// <summary>
            /// 最大スケールプロパティ名
            /// </summary>
            public const string MaxScale = "MaxScale";
        }

        /// <summary>
        /// 最大スケール保持変数
        /// </summary>
        protected double _MaxScale = InitializeValues.MaxScale;

        /// <summary>
        /// 最大スケール
        /// </summary>
        public double MaxScale
        {
            get
            {
                return this._MaxScale;
            }
            set
            {
                double now = this._MaxScale;
                if (now == value)
                {
                    return;
                }
                this._MaxScale = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion
        #region 選択スケール
        /// <summary>
        /// 初期値
        /// </summary>
        public static new partial class InitializeValues
        {
            /// <summary>
            /// 選択スケール初期値
            /// </summary>
            public const double SelectScale = 1;
        }

        /// <summary>
        /// プロパティ名
        /// </summary>
        public static new partial class PropertyNames
        {
            /// <summary>
            /// 選択スケールプロパティ名
            /// </summary>
            public const string SelectScale = "SelectScale";
        }

        /// <summary>
        /// 選択スケール保持変数
        /// </summary>
        protected double _SelectScale = InitializeValues.SelectScale;

        /// <summary>
        /// 選択スケール
        /// </summary>
        public double SelectScale
        {
            get
            {
                return this._SelectScale;
            }
            set
            {
                double now = this._SelectScale;
                if (now == value)
                {
                    return;
                }
                this._SelectScale = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion

        #region クリップX最小値
        /// <summary>
        /// 初期値
        /// </summary>
        public static new partial class InitializeValues
        {
            /// <summary>
            /// クリップX最小値初期値
            /// </summary>
            public const double MinClipX = 0;
        }

        /// <summary>
        /// プロパティ名
        /// </summary>
        public static new partial class PropertyNames
        {
            /// <summary>
            /// クリップX最小値プロパティ名
            /// </summary>
            public const string MinClipX = "MinClipX";
        }

        /// <summary>
        /// クリップX最小値保持変数
        /// </summary>
        protected double _MinClipX = InitializeValues.MinClipX;

        /// <summary>
        /// クリップX最小値
        /// </summary>
        public double MinClipX
        {
            get
            {
                return this._MinClipX;
            }
            set
            {
                double now = this._MinClipX;
                if (now == value)
                {
                    return;
                }
                this._MinClipX = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion
        #region クリップX最大値
        /// <summary>
        /// 初期値
        /// </summary>
        public static new partial class InitializeValues
        {
            /// <summary>
            /// クリップX最大値初期値
            /// </summary>
            public const double MaxClipX = 0;
        }

        /// <summary>
        /// プロパティ名
        /// </summary>
        public static new partial class PropertyNames
        {
            /// <summary>
            /// クリップX最大値プロパティ名
            /// </summary>
            public const string MaxClipX = "MaxClipX";
        }

        /// <summary>
        /// クリップX最大値保持変数
        /// </summary>
        protected double _MaxClipX = InitializeValues.MaxClipX;

        /// <summary>
        /// クリップX最大値
        /// </summary>
        public double MaxClipX
        {
            get
            {
                return this._MaxClipX;
            }
            set
            {
                double now = this._MaxClipX;
                if (now == value)
                {
                    return;
                }
                this._MaxClipX = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion

        #region クリップY最小値
        /// <summary>
        /// 初期値
        /// </summary>
        public static new partial class InitializeValues
        {
            /// <summary>
            /// クリップY最小値初期値
            /// </summary>
            public const double MinClipY = 0;
        }

        /// <summary>
        /// プロパティ名
        /// </summary>
        public static new partial class PropertyNames
        {
            /// <summary>
            /// クリップY最小値プロパティ名
            /// </summary>
            public const string MinClipY = "MinClipY";
        }

        /// <summary>
        /// クリップY最小値保持変数
        /// </summary>
        protected double _MinClipY = InitializeValues.MinClipY;

        /// <summary>
        /// クリップY最小値
        /// </summary>
        public double MinClipY
        {
            get
            {
                return this._MinClipY;
            }
            set
            {
                double now = this._MinClipY;
                if (now == value)
                {
                    return;
                }
                this._MinClipY = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion
        #region クリップY最大値
        /// <summary>
        /// 初期値
        /// </summary>
        public static new partial class InitializeValues
        {
            /// <summary>
            /// クリップY最大値初期値
            /// </summary>
            public const double MaxClipY = 0;
        }

        /// <summary>
        /// プロパティ名
        /// </summary>
        public static new partial class PropertyNames
        {
            /// <summary>
            /// クリップY最大値プロパティ名
            /// </summary>
            public const string MaxClipY = "MaxClipY";
        }

        /// <summary>
        /// クリップY最大値保持変数
        /// </summary>
        protected double _MaxClipY = InitializeValues.MaxClipY;

        /// <summary>
        /// クリップY最大値
        /// </summary>
        public double MaxClipY
        {
            get
            {
                return this._MaxClipY;
            }
            set
            {
                double now = this._MaxClipY;
                if (now == value)
                {
                    return;
                }
                this._MaxClipY = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion

        #region ビデオの長さ
        /// <summary>
        /// 初期値
        /// </summary>
        public static new partial class InitializeValues
        {
            /// <summary>
            /// ビデオの長さ初期値
            /// </summary>
            public const long MaxDurationTicks = 0;
        }

        /// <summary>
        /// プロパティ名
        /// </summary>
        public static new partial class PropertyNames
        {
            /// <summary>
            /// ビデオの長さプロパティ名
            /// </summary>
            public const string MaxDurationTicks = "MaxDurationTicks";
        }

        /// <summary>
        /// ビデオの長さ保持変数
        /// </summary>
        protected long _MaxDurationTicks = InitializeValues.MaxDurationTicks;

        /// <summary>
        /// ビデオの長さ
        /// </summary>
        /// <remarks>
        /// ticks単位
        /// </remarks>
        public long MaxDurationTicks
        {
            get
            {
                return this._MaxDurationTicks;
            }
            set
            {
                long now = this._MaxDurationTicks;
                if (now == value)
                {
                    return;
                }
                this._MaxDurationTicks = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion
        #region 選択している位置
        /// <summary>
        /// 初期値
        /// </summary>
        public static new partial class InitializeValues
        {
            /// <summary>
            /// 選択している位置初期値
            /// </summary>
            public const long SelectVideoPosition = 0;
        }

        /// <summary>
        /// プロパティ名
        /// </summary>
        public static new partial class PropertyNames
        {
            /// <summary>
            /// 選択している位置プロパティ名
            /// </summary>
            public const string SelectVideoPosition = "SelectVideoPosition";
        }

        /// <summary>
        /// 選択している位置保持変数
        /// </summary>
        protected long _SelectVideoPosition = InitializeValues.SelectVideoPosition;

        /// <summary>
        /// 選択している位置
        /// </summary>
        /// <remarks>
        /// (スライダーで選択する)
        /// </remarks>
        public long SelectVideoPosition
        {
            get
            {
                return this._SelectVideoPosition;
            }
            set
            {
                long now = this._SelectVideoPosition;
                if (now == value)
                {
                    return;
                }
                this._SelectVideoPosition = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion

        #region 状態エラーフラグ
        /// <summary>
        /// 初期値
        /// </summary>
        public static new partial class InitializeValues
        {
            /// <summary>
            /// 状態エラーフラグ初期値
            /// </summary>
            public const bool IsError = false;
        }

        /// <summary>
        /// プロパティ名
        /// </summary>
        public static new partial class PropertyNames
        {
            /// <summary>
            /// 状態エラーフラグプロパティ名
            /// </summary>
            public const string IsError = "IsError";
        }

        /// <summary>
        /// 状態エラーフラグ保持変数
        /// </summary>
        protected bool _IsError = InitializeValues.IsError;

        /// <summary>
        /// 状態エラーフラグ
        /// </summary>
        public bool IsError
        {
            get
            {
                return this._IsError;
            }
            set
            {
                bool now = this._IsError;
                if (now == value)
                {
                    return;
                }
                this._IsError = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion
        #region 状態テキスト
        /// <summary>
        /// 初期値
        /// </summary>
        public static new partial class InitializeValues
        {
            /// <summary>
            /// 状態テキスト初期値
            /// </summary>
            public const string? StatusText = null;
        }

        /// <summary>
        /// プロパティ名
        /// </summary>
        public static new partial class PropertyNames
        {
            /// <summary>
            /// 状態テキストプロパティ名
            /// </summary>
            public const string StatusText = "StatusText";
        }

        /// <summary>
        /// 状態テキスト保持変数
        /// </summary>
        protected string? _StatusText = InitializeValues.StatusText;

        /// <summary>
        /// 状態テキスト
        /// </summary>
        public string? StatusText
        {
            get
            {
                return this._StatusText;
            }
            set
            {
                string? now = this._StatusText;
                if (now == value)
                {
                    return;
                }
                this._StatusText = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion

        #region 準備完了
        /// <summary>
        /// 初期値
        /// </summary>
        public static new partial class InitializeValues
        {
            /// <summary>
            /// 準備完了初期値
            /// </summary>
            public const bool IsReady = false;
        }

        /// <summary>
        /// プロパティ名
        /// </summary>
        public static new partial class PropertyNames
        {
            /// <summary>
            /// 準備完了プロパティ名
            /// </summary>
            public const string IsReady = "IsReady";
        }

        /// <summary>
        /// 準備完了保持変数
        /// </summary>
        protected bool _IsReady = InitializeValues.IsReady;

        /// <summary>
        /// 準備完了
        /// </summary>
        public bool IsReady
        {
            get
            {
                return this._IsReady;
            }
            set
            {
                bool now = this._IsReady;
                if (now == value)
                {
                    return;
                }
                this._IsReady = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion

        #region RGB32取得バッファ
        /// <summary>
        /// 初期値
        /// </summary>
        public static new partial class InitializeValues
        {
            /// <summary>
            /// RGB32取得バッファ初期値
            /// </summary>
            public const byte[]? Rgb32Buffer = null;
        }

        /// <summary>
        /// プロパティ名
        /// </summary>
        public static new partial class PropertyNames
        {
            /// <summary>
            /// RGB32取得バッファプロパティ名
            /// </summary>
            public const string Rgb32Buffer = "Rgb32Buffer";
        }

        /// <summary>
        /// RGB32取得バッファ保持変数
        /// </summary>
        protected byte[]? _Rgb32Buffer = InitializeValues.Rgb32Buffer;

        /// <summary>
        /// RGB32取得バッファ
        /// </summary>
        public byte[]? Rgb32Buffer
        {
            get
            {
                return this._Rgb32Buffer;
            }
            set
            {
                byte[]? now = this._Rgb32Buffer;
                if (now == value)
                {
                    return;
                }
                this._Rgb32Buffer = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion

        #region クリップ枠位置X
        /// <summary>
        /// 初期値
        /// </summary>
        public static new partial class InitializeValues
        {
            /// <summary>
            /// クリップ枠位置X初期値
            /// </summary>
            public const double ClipFrameX = 0;
        }

        /// <summary>
        /// プロパティ名
        /// </summary>
        public static new partial class PropertyNames
        {
            /// <summary>
            /// クリップ枠位置Xプロパティ名
            /// </summary>
            public const string ClipFrameX = "ClipFrameX";
        }

        /// <summary>
        /// クリップ枠位置X保持変数
        /// </summary>
        protected double _ClipFrameX = InitializeValues.ClipFrameX;

        /// <summary>
        /// クリップ枠位置X
        /// </summary>
        public double ClipFrameX
        {
            get
            {
                return this._ClipFrameX;
            }
            set
            {
                double now = this._ClipFrameX;
                if (now == value)
                {
                    return;
                }
                this._ClipFrameX = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion
        #region クリップ枠位置Y
        /// <summary>
        /// 初期値
        /// </summary>
        public static new partial class InitializeValues
        {
            /// <summary>
            /// クリップ枠位置Y初期値
            /// </summary>
            public const double ClipFrameY = 0;
        }

        /// <summary>
        /// プロパティ名
        /// </summary>
        public static new partial class PropertyNames
        {
            /// <summary>
            /// クリップ枠位置Yプロパティ名
            /// </summary>
            public const string ClipFrameY = "ClipFrameY";
        }

        /// <summary>
        /// クリップ枠位置Y保持変数
        /// </summary>
        protected double _ClipFrameY = InitializeValues.ClipFrameY;

        /// <summary>
        /// クリップ枠位置Y
        /// </summary>
        public double ClipFrameY
        {
            get
            {
                return this._ClipFrameY;
            }
            set
            {
                double now = this._ClipFrameY;
                if (now == value)
                {
                    return;
                }
                this._ClipFrameY = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion
        #region クリップ枠位置Width
        /// <summary>
        /// 初期値
        /// </summary>
        public static new partial class InitializeValues
        {
            /// <summary>
            /// クリップ枠位置Width初期値
            /// </summary>
            public const double ClipFrameWidth = 0;
        }

        /// <summary>
        /// プロパティ名
        /// </summary>
        public static new partial class PropertyNames
        {
            /// <summary>
            /// クリップ枠位置Widthプロパティ名
            /// </summary>
            public const string ClipFrameWidth = "ClipFrameWidth";
        }

        /// <summary>
        /// クリップ枠位置Width保持変数
        /// </summary>
        protected double _ClipFrameWidth = InitializeValues.ClipFrameWidth;

        /// <summary>
        /// クリップ枠位置Width
        /// </summary>
        public double ClipFrameWidth
        {
            get
            {
                return this._ClipFrameWidth;
            }
            set
            {
                double now = this._ClipFrameWidth;
                if (now == value)
                {
                    return;
                }
                this._ClipFrameWidth = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion
        #region クリップ枠位置Height
        /// <summary>
        /// 初期値
        /// </summary>
        public static new partial class InitializeValues
        {
            /// <summary>
            /// クリップ枠位置Height初期値
            /// </summary>
            public const double ClipFrameHeight = 0;
        }

        /// <summary>
        /// プロパティ名
        /// </summary>
        public static new partial class PropertyNames
        {
            /// <summary>
            /// クリップ枠位置Heightプロパティ名
            /// </summary>
            public const string ClipFrameHeight = "ClipFrameHeight";
        }

        /// <summary>
        /// クリップ枠位置Height保持変数
        /// </summary>
        protected double _ClipFrameHeight = InitializeValues.ClipFrameHeight;

        /// <summary>
        /// クリップ枠位置Height
        /// </summary>
        public double ClipFrameHeight
        {
            get
            {
                return this._ClipFrameHeight;
            }
            set
            {
                double now = this._ClipFrameHeight;
                if (now == value)
                {
                    return;
                }
                this._ClipFrameHeight = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion

        #region ビデオファイル名
        /// <summary>
        /// 初期値
        /// </summary>
        public static new partial class InitializeValues
        {
            /// <summary>
            /// ビデオファイル名初期値
            /// </summary>
            public const string? VideoFilename = null;
        }

        /// <summary>
        /// プロパティ名
        /// </summary>
        public static new partial class PropertyNames
        {
            /// <summary>
            /// ビデオファイル名プロパティ名
            /// </summary>
            public const string VideoFilename = "VideoFilename";
        }

        /// <summary>
        /// ビデオファイル名保持変数
        /// </summary>
        protected string? _VideoFilename = InitializeValues.VideoFilename;

        /// <summary>
        /// ビデオファイル名
        /// </summary>
        public string? VideoFilename
        {
            get
            {
                return this._VideoFilename;
            }
            set
            {
                string? now = this._VideoFilename;
                if (now == value)
                {
                    return;
                }
                this._VideoFilename = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion

        // <<<<< ここまで
    }
}
