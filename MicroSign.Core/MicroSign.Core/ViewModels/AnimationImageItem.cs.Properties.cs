using System.Runtime.Serialization;
using System.Windows.Media.Imaging;

namespace MicroSign.Core.ViewModels
{
    partial class AnimationImageItem
    {
        //「Documents\ViewModelプロパティ作成テンプレート.xlsx」の「AnimationImageItem」をコピー >>>>> ここから
        #region 画像ファイル名
        /// <summary>
        /// 初期値
        /// </summary>
        public static new partial class InitializeValues
        {
            /// <summary>
            /// 画像ファイル名初期値
            /// </summary>
            public const string? Name = null;
        }

        /// <summary>
        /// プロパティ名
        /// </summary>
        public static new partial class PropertyNames
        {
            /// <summary>
            /// 画像ファイル名プロパティ名
            /// </summary>
            public const string Name = "Name";
        }

        /// <summary>
        /// 画像ファイル名保持変数
        /// </summary>
        protected string? _Name = InitializeValues.Name;

        /// <summary>
        /// 画像ファイル名
        /// </summary>
        public string? Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                string? now = this._Name;
                if (now == value)
                {
                    return;
                }
                this._Name = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion
        #region 画像パス
        /// <summary>
        /// 初期値
        /// </summary>
        public static new partial class InitializeValues
        {
            /// <summary>
            /// 画像パス初期値
            /// </summary>
            public const string? Path = null;
        }

        /// <summary>
        /// プロパティ名
        /// </summary>
        public static new partial class PropertyNames
        {
            /// <summary>
            /// 画像パスプロパティ名
            /// </summary>
            public const string Path = "Path";
        }

        /// <summary>
        /// 画像パス保持変数
        /// </summary>
        protected string? _Path = InitializeValues.Path;

        /// <summary>
        /// 画像パス
        /// </summary>
        public string? Path
        {
            get
            {
                return this._Path;
            }
            set
            {
                string? now = this._Path;
                if (now == value)
                {
                    return;
                }
                this._Path = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion
        #region 画像
        /// <summary>
        /// 初期値
        /// </summary>
        public static new partial class InitializeValues
        {
            /// <summary>
            /// 画像初期値
            /// </summary>
            public const BitmapSource? Image = null;
        }

        /// <summary>
        /// プロパティ名
        /// </summary>
        public static new partial class PropertyNames
        {
            /// <summary>
            /// 画像プロパティ名
            /// </summary>
            public const string Image = "Image";
        }

        /// <summary>
        /// 画像保持変数
        /// </summary>
        protected BitmapSource? _Image = InitializeValues.Image;

        /// <summary>
        /// 画像
        /// </summary>
        public BitmapSource? Image
        {
            get
            {
                return this._Image;
            }
            set
            {
                BitmapSource? now = this._Image;
                if (now == value)
                {
                    return;
                }
                this._Image = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion
        #region 表示期間(秒)
        /// <summary>
        /// 初期値
        /// </summary>
        public static new partial class InitializeValues
        {
            /// <summary>
            /// 表示期間(秒)初期値
            /// </summary>
            public const double DisplayPeriod = 1;
        }

        /// <summary>
        /// プロパティ名
        /// </summary>
        public static new partial class PropertyNames
        {
            /// <summary>
            /// 表示期間(秒)プロパティ名
            /// </summary>
            public const string DisplayPeriod = "DisplayPeriod";
        }

        /// <summary>
        /// 表示期間(秒)保持変数
        /// </summary>
        protected double _DisplayPeriod = InitializeValues.DisplayPeriod;

        /// <summary>
        /// 表示期間(秒)
        /// </summary>
        public double DisplayPeriod
        {
            get
            {
                return this._DisplayPeriod;
            }
            set
            {
                double now = this._DisplayPeriod;
                if (now == value)
                {
                    return;
                }
                this._DisplayPeriod = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion
        #region 画像種類
        /// <summary>
        /// 初期値
        /// </summary>
        public static new partial class InitializeValues
        {
            /// <summary>
            /// 画像種類初期値
            /// </summary>
            public const AnimationImageKinds Kind = AnimationImageKinds.Image;
        }

        /// <summary>
        /// プロパティ名
        /// </summary>
        public static new partial class PropertyNames
        {
            /// <summary>
            /// 画像種類プロパティ名
            /// </summary>
            public const string Kind = "Kind";
        }

        /// <summary>
        /// 画像種類保持変数
        /// </summary>
        protected AnimationImageKinds _Kind = InitializeValues.Kind;

        /// <summary>
        /// 画像種類
        /// </summary>
        public AnimationImageKinds Kind
        {
            get
            {
                return this._Kind;
            }
            set
            {
                AnimationImageKinds now = this._Kind;
                if (now == value)
                {
                    return;
                }
                this._Kind = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion
        #region 画像範囲X
        /// <summary>
        /// 初期値
        /// </summary>
        public static new partial class InitializeValues
        {
            /// <summary>
            /// 画像範囲X初期値
            /// </summary>
            public const ushort RectX = 0;
        }

        /// <summary>
        /// プロパティ名
        /// </summary>
        public static new partial class PropertyNames
        {
            /// <summary>
            /// 画像範囲Xプロパティ名
            /// </summary>
            public const string RectX = "RectX";
        }

        /// <summary>
        /// 画像範囲X保持変数
        /// </summary>
        protected ushort _RectX = InitializeValues.RectX;

        /// <summary>
        /// 画像範囲X
        /// </summary>
        /// <remarks>
        /// マトリクスLEDに描写する画像の範囲始点X
        /// </remarks>
        public ushort RectX
        {
            get
            {
                return this._RectX;
            }
            set
            {
                ushort now = this._RectX;
                if (now == value)
                {
                    return;
                }
                this._RectX = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion
        #region 画像範囲Y
        /// <summary>
        /// 初期値
        /// </summary>
        public static new partial class InitializeValues
        {
            /// <summary>
            /// 画像範囲Y初期値
            /// </summary>
            public const ushort RectY = 0;
        }

        /// <summary>
        /// プロパティ名
        /// </summary>
        public static new partial class PropertyNames
        {
            /// <summary>
            /// 画像範囲Yプロパティ名
            /// </summary>
            public const string RectY = "RectY";
        }

        /// <summary>
        /// 画像範囲Y保持変数
        /// </summary>
        protected ushort _RectY = InitializeValues.RectY;

        /// <summary>
        /// 画像範囲Y
        /// </summary>
        /// <remarks>
        /// マトリクスLEDに描写する画像の範囲始点Y
        /// </remarks>
        public ushort RectY
        {
            get
            {
                return this._RectY;
            }
            set
            {
                ushort now = this._RectY;
                if (now == value)
                {
                    return;
                }
                this._RectY = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion
        #region 画像範囲横幅
        /// <summary>
        /// 初期値
        /// </summary>
        public static new partial class InitializeValues
        {
            /// <summary>
            /// 画像範囲横幅初期値
            /// </summary>
            public const ushort RectWidth = 0;
        }

        /// <summary>
        /// プロパティ名
        /// </summary>
        public static new partial class PropertyNames
        {
            /// <summary>
            /// 画像範囲横幅プロパティ名
            /// </summary>
            public const string RectWidth = "RectWidth";
        }

        /// <summary>
        /// 画像範囲横幅保持変数
        /// </summary>
        protected ushort _RectWidth = InitializeValues.RectWidth;

        /// <summary>
        /// 画像範囲横幅
        /// </summary>
        /// <remarks>
        /// マトリクスLEDに描写する画像の範囲横幅。マトリクスLEDの横幅より大きい場合はマトリクスLEDの横幅に制限されます
        /// </remarks>
        public ushort RectWidth
        {
            get
            {
                return this._RectWidth;
            }
            set
            {
                ushort now = this._RectWidth;
                if (now == value)
                {
                    return;
                }
                this._RectWidth = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion
        #region 画像範囲縦幅
        /// <summary>
        /// 初期値
        /// </summary>
        public static new partial class InitializeValues
        {
            /// <summary>
            /// 画像範囲縦幅初期値
            /// </summary>
            public const ushort RectHeight = 0;
        }

        /// <summary>
        /// プロパティ名
        /// </summary>
        public static new partial class PropertyNames
        {
            /// <summary>
            /// 画像範囲縦幅プロパティ名
            /// </summary>
            public const string RectHeight = "RectHeight";
        }

        /// <summary>
        /// 画像範囲縦幅保持変数
        /// </summary>
        protected ushort _RectHeight = InitializeValues.RectHeight;

        /// <summary>
        /// 画像範囲縦幅
        /// </summary>
        /// <remarks>
        /// マトリクスLEDに描写する画像の範囲横幅。マトリクスLEDの横幅より大きい場合はマトリクスLEDの横幅に制限されます
        /// </remarks>
        public ushort RectHeight
        {
            get
            {
                return this._RectHeight;
            }
            set
            {
                ushort now = this._RectHeight;
                if (now == value)
                {
                    return;
                }
                this._RectHeight = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion
        #region 描写先X
        /// <summary>
        /// 初期値
        /// </summary>
        public static new partial class InitializeValues
        {
            /// <summary>
            /// 描写先X初期値
            /// </summary>
            public const ushort OffsetX = 0;
        }

        /// <summary>
        /// プロパティ名
        /// </summary>
        public static new partial class PropertyNames
        {
            /// <summary>
            /// 描写先Xプロパティ名
            /// </summary>
            public const string OffsetX = "OffsetX";
        }

        /// <summary>
        /// 描写先X保持変数
        /// </summary>
        protected ushort _OffsetX = InitializeValues.OffsetX;

        /// <summary>
        /// 描写先X
        /// </summary>
        /// <remarks>
        /// マトリクスLEDの描写先のX
        /// </remarks>
        public ushort OffsetX
        {
            get
            {
                return this._OffsetX;
            }
            set
            {
                ushort now = this._OffsetX;
                if (now == value)
                {
                    return;
                }
                this._OffsetX = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion
        #region 描写先Y
        /// <summary>
        /// 初期値
        /// </summary>
        public static new partial class InitializeValues
        {
            /// <summary>
            /// 描写先Y初期値
            /// </summary>
            public const ushort OffsetY = 0;
        }

        /// <summary>
        /// プロパティ名
        /// </summary>
        public static new partial class PropertyNames
        {
            /// <summary>
            /// 描写先Yプロパティ名
            /// </summary>
            public const string OffsetY = "OffsetY";
        }

        /// <summary>
        /// 描写先Y保持変数
        /// </summary>
        protected ushort _OffsetY = InitializeValues.OffsetY;

        /// <summary>
        /// 描写先Y
        /// </summary>
        /// <remarks>
        /// マトリクスLEDの描写先のY
        /// </remarks>
        public ushort OffsetY
        {
            get
            {
                return this._OffsetY;
            }
            set
            {
                ushort now = this._OffsetY;
                if (now == value)
                {
                    return;
                }
                this._OffsetY = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion
        #region 透明色
        /// <summary>
        /// 初期値
        /// </summary>
        public static new partial class InitializeValues
        {
            /// <summary>
            /// 透明色初期値
            /// </summary>
            public const uint TransparentRGB = 0;
        }

        /// <summary>
        /// プロパティ名
        /// </summary>
        public static new partial class PropertyNames
        {
            /// <summary>
            /// 透明色プロパティ名
            /// </summary>
            public const string TransparentRGB = "TransparentRGB";
        }

        /// <summary>
        /// 透明色保持変数
        /// </summary>
        protected uint _TransparentRGB = InitializeValues.TransparentRGB;

        /// <summary>
        /// 透明色
        /// </summary>
        /// <remarks>
        /// 透明にする画像の色(RGB24で設定)
        /// </remarks>
        public uint TransparentRGB
        {
            get
            {
                return this._TransparentRGB;
            }
            set
            {
                uint now = this._TransparentRGB;
                if (now == value)
                {
                    return;
                }
                this._TransparentRGB = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion






        #region アニメーション画像タイプ
        /// <summary>
        /// 初期値
        /// </summary>
        public static new partial class InitializeValues
        {
            /// <summary>
            /// アニメーション画像タイプ初期値
            /// </summary>
            public const AnimationImageType ImageType = AnimationImageType.ImageFile;
        }

        /// <summary>
        /// プロパティ名
        /// </summary>
        public static new partial class PropertyNames
        {
            /// <summary>
            /// アニメーション画像タイププロパティ名
            /// </summary>
            public const string ImageType = "ImageType";
        }

        /// <summary>
        /// アニメーション画像タイプ保持変数
        /// </summary>
        protected AnimationImageType _ImageType = InitializeValues.ImageType;

        /// <summary>
        /// アニメーション画像タイプ
        /// </summary>
        /// <remarks>
        /// 画像かテキストかを区別する。本来は派生にするべきだが.NET6ではJSONシリアライザーが派生に対応していないのでEnumで区別します
        /// </remarks>
        public AnimationImageType ImageType
        {
            get
            {
                return this._ImageType;
            }
            set
            {
                AnimationImageType now = this._ImageType;
                if (now == value)
                {
                    return;
                }
                this._ImageType = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion
        #region 選択フォントサイズ
        /// <summary>
        /// 初期値
        /// </summary>
        public static new partial class InitializeValues
        {
            /// <summary>
            /// 選択フォントサイズ初期値
            /// </summary>
            public const int SelectFontSize = 16;
        }

        /// <summary>
        /// プロパティ名
        /// </summary>
        public static new partial class PropertyNames
        {
            /// <summary>
            /// 選択フォントサイズプロパティ名
            /// </summary>
            public const string SelectFontSize = "SelectFontSize";
        }

        /// <summary>
        /// 選択フォントサイズ保持変数
        /// </summary>
        protected int _SelectFontSize = InitializeValues.SelectFontSize;

        /// <summary>
        /// 選択フォントサイズ
        /// </summary>
        /// <remarks>
        /// WPFのフォントサイズであるdip単位で入る
        /// </remarks>
        public int SelectFontSize
        {
            get
            {
                return this._SelectFontSize;
            }
            set
            {
                int now = this._SelectFontSize;
                if (now == value)
                {
                    return;
                }
                this._SelectFontSize = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion
        #region 選択フォント色
        /// <summary>
        /// 初期値
        /// </summary>
        public static new partial class InitializeValues
        {
            /// <summary>
            /// 選択フォント色初期値
            /// </summary>
            public const int SelectFontColor = 7;
        }

        /// <summary>
        /// プロパティ名
        /// </summary>
        public static new partial class PropertyNames
        {
            /// <summary>
            /// 選択フォント色プロパティ名
            /// </summary>
            public const string SelectFontColor = "SelectFontColor";
        }

        /// <summary>
        /// 選択フォント色保持変数
        /// </summary>
        protected int _SelectFontColor = InitializeValues.SelectFontColor;

        /// <summary>
        /// 選択フォント色
        /// </summary>
        /// <remarks>
        /// 1=赤色・2=緑色・3=黄色・4=青色・5=紫色・6=水色・7=白色
        /// </remarks>
        public int SelectFontColor
        {
            get
            {
                return this._SelectFontColor;
            }
            set
            {
                int now = this._SelectFontColor;
                if (now == value)
                {
                    return;
                }
                this._SelectFontColor = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion
        #region 表示文章
        /// <summary>
        /// 初期値
        /// </summary>
        public static new partial class InitializeValues
        {
            /// <summary>
            /// 表示文章初期値
            /// </summary>
            public const string? DisplayText = null;
        }

        /// <summary>
        /// プロパティ名
        /// </summary>
        public static new partial class PropertyNames
        {
            /// <summary>
            /// 表示文章プロパティ名
            /// </summary>
            public const string DisplayText = "DisplayText";
        }

        /// <summary>
        /// 表示文章保持変数
        /// </summary>
        protected string? _DisplayText = InitializeValues.DisplayText;

        /// <summary>
        /// 表示文章
        /// </summary>
        public string? DisplayText
        {
            get
            {
                return this._DisplayText;
            }
            set
            {
                string? now = this._DisplayText;
                if (now == value)
                {
                    return;
                }
                this._DisplayText = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion

        #region 選択フラグ
        /// <summary>
        /// 初期値
        /// </summary>
        public static new partial class InitializeValues
        {
            /// <summary>
            /// 選択フラグ初期値
            /// </summary>
            public const bool IsSelected = false;
        }

        /// <summary>
        /// プロパティ名
        /// </summary>
        public static new partial class PropertyNames
        {
            /// <summary>
            /// 選択フラグプロパティ名
            /// </summary>
            public const string IsSelected = "IsSelected";
        }

        /// <summary>
        /// 選択フラグ保持変数
        /// </summary>
        protected bool _IsSelected = InitializeValues.IsSelected;

        /// <summary>
        /// 選択フラグ
        /// </summary>
        /// <remarks>
        /// 2026.06.05:CS)杉原:複数選択できるようにした
        /// </remarks>
        public bool IsSelected
        {
            get
            {
                return this._IsSelected;
            }
            set
            {
                bool now = this._IsSelected;
                if (now == value)
                {
                    return;
                }
                this._IsSelected = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion

        //「Documents\ViewModelプロパティ作成テンプレート.xlsx」の「AnimationImageItem」をコピー <<<<< ここまで
    }
}
