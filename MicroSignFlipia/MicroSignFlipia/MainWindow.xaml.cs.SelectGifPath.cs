namespace MicroSignFlipia
{
    partial class MainWindow
    {
        /// <summary>
        /// GIF画像読込
        /// </summary>
        /// <param name="title">タイトル</param>
        /// <returns></returns>
        private string? SelectGifPath(string title)
        {
            return this.SelectImagePath(title, App.Consts.Files.GifExt, App.Consts.Files.GifFilter);
        }
    }
}
