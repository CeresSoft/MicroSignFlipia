using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSignFlipia
{
    partial class MainWindow
    {
        /// <summary>
        /// 画像読込
        /// </summary>
        /// <param name="title">タイトル</param>
        /// <returns></returns>
        private string? SelectImagePath(string title)
        {
            return this.SelectImagePath(title, App.Consts.Files.DefaultExt ,App.Consts.Files.Filter);
        }

        /// <summary>
        /// 画像読込
        /// </summary>
        /// <param name="title">タイトル</param>
        /// <param name="defaultExt">デフォルト拡張子</param>
        /// <param name="filter">フィルター</param>
        /// <returns></returns>
        private string? SelectImagePath(string title, string defaultExt, string filter)
        {
            //読込イメージ選択
            string imagePath;
            {
                Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

                //タイトル
                dialog.Title = title;

                // Default file name
                dialog.FileName = "";

                // Default file extension
                dialog.DefaultExt = defaultExt;

                // Filter files by extension
                dialog.Filter = filter;

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
                imagePath = dialog.FileName;
            }

            //終了
            return imagePath;
        }
    }
}
