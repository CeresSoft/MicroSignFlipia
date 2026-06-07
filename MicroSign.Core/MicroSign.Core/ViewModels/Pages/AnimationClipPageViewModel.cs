using MicroSign.Core.Navigations.ViewModels;

namespace MicroSign.Core.ViewModels.Pages
{
    /// <summary>
    /// 画像切り抜きページViewModel
    /// </summary>
    public partial class AnimationClipPageViewModel : OkCancelNavigationViewModelBase
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
        /// コンストラクタ
        /// </summary>
        public AnimationClipPageViewModel()
        {
            //移動方向選択コマンド
            this.MoveDirectionSelectCommand = new MicroSign.Core.ViewModels.DelegateCommand(this.OnMoveDirectionSelect);
        }

        /// <summary>
        /// 移動方向選択クリック
        /// </summary>
        /// <param name="parameter"></param>
        private void OnMoveDirectionSelect(object? parameter)
        {
            LOGGER.Debug($"移動方向選択クリック parameter='{parameter}'");

            //パラメータ有効判定
            if (parameter is AnimationMoveDirection direction)
            {
                //移動方向を変更
                LOGGER.Debug($"移動方向変更 MoveDirection='{direction}'");
                this.MoveDirection = direction;
            }
            else
            {
                //型が異なる場合は何もしない
                LOGGER.Warn($"AnimationMoveDirection型以外のパラメータです");
            }
        }

        /// <summary>
        /// モデル
        /// </summary>
        public Models.Model Model
        {
            get
            {
                return Models.Model.Instance;
            }
        }
    }
}
