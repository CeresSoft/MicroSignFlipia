using System;
using System.Text;

namespace MicroSign.Core.Models
{
    partial class Model
    {
        /// <summary>
        /// フレーム数からフレーム番号のフォーマットを生成
        /// </summary>
        /// <param name="frameCount"></param>
        /// <returns></returns>
        public string GetNumberFormat(long frameCount)
        {
            int log = (int)Math.Log10(frameCount);
            int m = log + CommonConsts.Collection.Step;
            StringBuilder sb = new StringBuilder();
            for (int i = CommonConsts.Index.First; i < m; i += CommonConsts.Index.Step)
            {
                sb.Append(CommonConsts.File.ZeroPrace);
            }
            string fileNumberFormat = sb.ToString();
            return fileNumberFormat;
        }
    }
}
