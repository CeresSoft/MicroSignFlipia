namespace MicroSignFlipia
{
    partial class MainWindow
    {
        /// <summary>
        /// 画像複数読込
        /// </summary>
        /// <param name="title">タイトル</param>
        /// <returns></returns>
        private string[]? MultiSelectImagePath(string title)
        {
            //読込イメージ選択
            string[]? imagePath = null;
            {
                Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

                //タイトル
                dialog.Title = title;

                // Default file name
                dialog.FileName = "";

                // Default file extension
                dialog.DefaultExt = ".png";

                // Filter files by extension
                dialog.Filter = "Image(*.png,*.jpg,*.jpeg,*.bmp)|*.png;*.jpg;*.jpeg;*.bmp|すべてのファイル (*.*)|*.*";

                //複数選択
                dialog.Multiselect = true;

                //表示
                bool ret = dialog.ShowDialog(this) ?? false;
                if (ret)
                {
                    //選択した場合は処理続行
                }
                else
                {
                    //選択しなかった場合は終了
                    return null;
                }

                //画像パス取得
                imagePath = dialog.FileNames;
            }

            //終了
            return imagePath;
        }
    }
}
