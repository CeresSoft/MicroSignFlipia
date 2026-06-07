using System.Windows;
using System.Windows.Data;

namespace MicroSign.Core
{
    /// <summary>
    /// バインディングヘルパー
    /// </summary>
    public static partial class BindingHelper
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
        /// ソース無し
        /// </summary>
        public const object NO_SOURCE = null;

        /// <summary>
        /// コンバーター無し
        /// </summary>
        public const IValueConverter NO_CONVERTER = null;

        /// <summary>
        /// コンバートパラメータ無し
        /// </summary>
        public const object NO_CONVERTER_PARAMETER = null;

        /// <summary>
        /// Binding作成
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Binding CreateBinding(string path)
        {
            return BindingHelper.CreateBinding(BindingHelper.NO_SOURCE, path, BindingHelper.NO_CONVERTER, BindingHelper.NO_CONVERTER_PARAMETER);
        }

        /// <summary>
        /// Binding作成
        /// </summary>
        /// <param name="source"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Binding CreateBinding(object source, string path)
        {
            return BindingHelper.CreateBinding(source, path, BindingHelper.NO_CONVERTER, BindingHelper.NO_CONVERTER_PARAMETER);
        }

        /// <summary>
        /// Binding作成
        /// </summary>
        /// <param name="source"></param>
        /// <param name="path"></param>
        /// <param name="converter"></param>
        /// <returns></returns>
        public static Binding CreateBinding(object source, string path, IValueConverter? converter)
        {
            return BindingHelper.CreateBinding(source, path, converter, BindingHelper.NO_CONVERTER_PARAMETER);
        }

        /// <summary>
        /// Binding作成
        /// </summary>
        /// <param name="source"></param>
        /// <param name="path"></param>
        /// <param name="converter"></param>
        /// <param name="converterParameter"></param>
        /// <returns></returns>
        public static Binding CreateBinding(object source, string path, IValueConverter? converter, object? converterParameter)
        {
            Binding bind = new Binding(path);
            if (source != BindingHelper.NO_SOURCE)
            {
                bind.Source = source;
            }
            if (converter != BindingHelper.NO_CONVERTER)
            {
                bind.Converter = converter;
                if (converterParameter != BindingHelper.NO_CONVERTER_PARAMETER)
                {
                    bind.ConverterParameter = converterParameter;
                }
            }
            return bind;
        }

        /// <summary>
        /// バインディング
        /// </summary>
        /// <param name="target"></param>
        /// <param name="dp"></param>
        /// <param name="path"></param>
        public static void SetBinding(DependencyObject target, DependencyProperty dp, string path)
        {
            BindingHelper.SetBinding(target, dp, BindingHelper.NO_SOURCE, path);

        }

        /// <summary>
        /// バインディング
        /// </summary>
        /// <param name="target"></param>
        /// <param name="dp"></param>
        /// <param name="source"></param>
        /// <param name="path"></param>
        public static void SetBinding(DependencyObject target, DependencyProperty dp, object source, string path)
        {
            Binding bind = BindingHelper.CreateBinding(source, path);
            BindingHelper.SetBinding(target, dp, bind);
        }

        /// <summary>
        /// バインディング
        /// </summary>
        /// <param name="target"></param>
        /// <param name="dp"></param>
        /// <param name="source"></param>
        /// <param name="path"></param>
        /// <param name="converter"></param>
        public static void SetBinding(DependencyObject target, DependencyProperty dp, object source, string path, IValueConverter converter)
        {
            Binding bind = BindingHelper.CreateBinding(source, path, converter);
            BindingHelper.SetBinding(target, dp, bind);
        }

        /// <summary>
        /// バインディング
        /// </summary>
        /// <param name="target"></param>
        /// <param name="dp"></param>
        /// <param name="source"></param>
        /// <param name="path"></param>
        /// <param name="converter"></param>
        /// <param name="converterParameter"></param>
        public static void SetBinding(DependencyObject target, DependencyProperty dp, object source, string path, IValueConverter? converter, object? converterParameter)
        {
            Binding bind = BindingHelper.CreateBinding(source, path, converter, converterParameter);
            BindingHelper.SetBinding(target, dp, bind);
        }

        /// <summary>
        /// バインディング
        /// </summary>
        /// <param name="target"></param>
        /// <param name="dp"></param>
        /// <param name="bind"></param>
        public static void SetBinding(DependencyObject target, DependencyProperty dp, Binding bind)
        {
            BindingOperations.SetBinding(target, dp, bind);
        }
    }
}
