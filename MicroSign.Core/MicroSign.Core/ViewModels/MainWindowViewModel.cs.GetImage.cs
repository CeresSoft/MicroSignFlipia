using System.Windows.Media.Imaging;

namespace MicroSign.Core.ViewModels
{
    partial class MainWindowViewModel
    {
        /// <summary>
        /// 画像読込
        /// </summary>
        /// <param name="imagePath">画像パス</param>
        /// <returns></returns>
        public BitmapSource? GetImage(string imagePath)
        {
            //モデルにリレー
            return this.Model.GetImage(imagePath);
        }
    }
}
