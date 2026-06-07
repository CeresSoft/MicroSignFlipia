using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using MicroSign.Core.BindingUtils;

namespace MicroSign.Core
{
    /// <summary>
    /// バインディングヘルパー - エレメントバインド
    /// </summary>
    partial class BindingHelper
    {
        /// <summary>
        /// エレメントバインディング実装
        /// </summary>
        /// <param name="elm">エレメント</param>
        /// <param name="vm">ViewModel</param>
        private static void ElementBindingImpl(FrameworkElement elm, object vm)
        {
            //ViewModel有効判定
            if (vm == null)
            {
                //無効の場合終了
                return;
            }

            //ViewModelの型情報を取得
            Type t = vm.GetType();

            //全プロパティを取得
            PropertyInfo[] pis = t.GetProperties();
            if (pis == null)
            {
                //取得できない場合は終了
                return;
            }

            //全プロパティ処理
            foreach (PropertyInfo pi in pis)
            {
                BindingHelper.ElementBindingImplPropertyInfo(elm, vm, pi);
            }
        }

        /// <summary>
        /// エレメントバインディング実装
        /// </summary>
        /// <param name="elm">エレメント</param>
        /// <param name="vm">ViewModel</param>
        /// <param name="pi">プロパティ情報</param>
        private static void ElementBindingImplPropertyInfo(FrameworkElement elm, object vm, PropertyInfo pi)
        {
            //プロパティ情報有効判定
            if (pi == null)
            {
                //無効の場合終了
                return;
            }

            //属性を取得
            IEnumerable<ElementBindingAttribute> ebas = pi.GetCustomAttributes<ElementBindingAttribute>(true);
            if (ebas == null)
            {
                //取得できない場合は終了
                return;
            }

            //属性処理
            foreach (ElementBindingAttribute eba in ebas)
            {
                BindingHelper.ElementBindingImplAttributes(elm, vm, pi, eba);
            }
        }

        /// <summary>
        /// エレメントバインディング実装
        /// </summary>
        /// <param name="elm">エレメント</param>
        /// <param name="vm">ViewModel</param>
        /// <param name="pi">プロパティ情報</param>
        /// <param name="eba">属性</param>
        private static void ElementBindingImplAttributes(FrameworkElement elm, object vm, PropertyInfo pi, ElementBindingAttribute eba)
        {
            //エレメント有効判定
            if (elm == null)
            {
                //無効の場合終了
                return;
            }

            //プロパティ情報有効判定
            if (pi == null)
            {
                //無効の場合終了
                return;
            }

            //属性有効判定
            if (eba == null)
            {
                //無効の場合終了
                return;
            }

            //属性情報取得
            string targetElementName = eba.TargetElementName;
            string targetPropertyName = eba.TargetPropertyName;
            Type? converterType = eba.ConverterType;
            object? param = eba.ConverterParameter;

            //プロパティ名を取得(=path)
            string pname = pi.Name;

            //2021.08.13:CS)杉原:白山のElementPropertyBinding機能を追加 >>>>> ここから
            //----------
            //エレメントから対象エレメント取得
            FrameworkElement? target = elm.FindName(targetElementName) as FrameworkElement;
            if (target == null)
            {
                //取得できなかった場合はエラー終了
                LOGGER.Warn($"ターゲットエレメントが見つかりませんでした [name='{targetElementName}' property={targetPropertyName}' vm.property='{pname}']");
                return;
            }

            //ターゲットプロパティ名が空の場合はプロパティ値(=クラスインスタンス)のプロパティをバインドする
            if (string.IsNullOrEmpty(targetPropertyName))
            {
                //プロパティの値を取得
                object? value = pi.GetValue(vm);
                if(value == null)
                {
                    //プロパティ値が無効の場合は何もしない
                }
                else
                {
                    //対象エレメントに対してエレメントプロパティバインディングを行う
                    BindingHelper.ElementPropertyBindingImpl(target, value);
                }

                //ここで終了
                return;
            }
            //2021.08.13:CS)杉原:白山のElementPropertyBinding機能を追加 <<<<< ここまで

            //TypeからIValueConverterを取得
            IValueConverter? converter = BindingHelper.NO_CONVERTER;
            if (converterType != null)
            {
                try
                {
                    //インスタンス生成
                    object? obj = Activator.CreateInstance(converterType);
                    if (obj == null)
                    {
                        //生成失敗の場合
                        LOGGER.Warn($"ValueConverterの生成に失敗しました [name='{targetElementName}' property={targetPropertyName}' vm.property='{pname}']");
                        return;
                    }

                    //IValueConverterにキャスト
                    converter = obj as IValueConverter;
                    if (converter == null)
                    {
                        //キャストできない場合はエラー終了
                        LOGGER.Warn($"IValueConverterにキャストできませんでした [name='{targetElementName}' property={targetPropertyName}' vm.property='{pname}']");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    //例外が発生した場合握りつぶして終了
                    LOGGER.WarnEx($"ValueConverterの生成で例外発生 [name='{targetElementName}' property={targetPropertyName}' vm.property='{pname}']", ex);
                    return;
                }
            }

            //2021.08.13:CS)杉原:白山のElementPropertyBinding機能を追加 >>>>> ここから
            ////エレメントから対象エレメント取得
            //DependencyObject target = elm.FindName(targetElementName) as DependencyObject;
            //if (target == null)
            //{
            //    //取得できなかった場合はエラー終了
            //    LOGGER.Warn($"ターゲットエレメントが見つかりませんでした [name='{targetElementName}' property={targetPropertyName}' vm.property='{pname}']");
            //    return;
            //}
            //----------
            //2021.08.13:CS)杉原:白山のElementPropertyBinding機能を追加 <<<<< ここまで

            //依存関係プロパティを取得する
            DependencyProperty? dp = BindingHelper.GetDependencyProperty(target, targetPropertyName);
            if (dp == null)
            {
                //フィールド値が取得できない場合はエラー終了
                LOGGER.Warn($"ターゲットにプロパティが見つかりませんでした [name='{targetElementName}' property={targetPropertyName}' vm.property='{pname}']");
                return;
            }

            //バインディング
            BindingHelper.SetBinding(target, dp, vm, pname, converter, param);
            LOGGER.Info($"ターゲットプロパティをバインドしました [name='{targetElementName}' property={targetPropertyName}' vm.property='{pname}']");
        }

        /// <summary>
        /// エレメントプロパティバインディング実装
        /// </summary>
        /// <param name="targetElement">対象エレメント</param>
        /// <param name="value">vmのプロパティの値(=クラスインスタンス)</param>
        /// <remarks>2021.08.13:CS)杉原:白山のElementPropertyBinding機能を追加</remarks>
        private static void ElementPropertyBindingImpl(FrameworkElement targetElement, object value)
        {
            //値有効判定
            if (value == null)
            {
                //無効の場合終了
                return;
            }

            //vmのプロパティの値の型情報を取得
            Type t = value.GetType();

            //全プロパティを取得
            PropertyInfo[] pis = t.GetProperties();
            if (pis == null)
            {
                //取得できない場合は終了
                return;
            }

            //全プロパティ処理
            foreach (PropertyInfo pi in pis)
            {
                BindingHelper.ElementPropertyBindingImplPropertyInfo(targetElement, value, pi);
            }
        }

        /// <summary>
        /// エレメントプロパティバインディング実装
        /// </summary>
        /// <param name="targetElement">対象エレメント</param>
        /// <param name="value">vmのプロパティの値(=クラスインスタンス)</param>
        /// <param name="pi">プロパティ情報</param>
        /// <remarks>2021.08.13:CS)杉原:白山のElementPropertyBinding機能を追加</remarks>
        private static void ElementPropertyBindingImplPropertyInfo(FrameworkElement targetElement, object value, PropertyInfo pi)
        {
            //プロパティ情報有効判定
            if (pi == null)
            {
                //無効の場合終了
                return;
            }

            //属性を取得
            IEnumerable<ElementPropertyBindingAttribute> epbas = pi.GetCustomAttributes<ElementPropertyBindingAttribute>(true);
            if (epbas == null)
            {
                //取得できない場合は終了
                return;
            }

            //属性処理
            foreach (ElementPropertyBindingAttribute epba in epbas)
            {
                BindingHelper.ElementPropertyBindingImplAttributes(targetElement, value, pi, epba);
            }
        }

        /// <summary>
        /// エレメントプロパティバインディング実装
        /// </summary>
        /// <param name="targetElement">エレメント</param>
        /// <param name="value">vmのプロパティの値(=クラスインスタンス)</param>
        /// <param name="pi">プロパティ情報</param>
        /// <param name="epba">エレメントプロパティバインディング属性</param>
        /// <remarks>2021.08.13:CS)杉原:白山のElementPropertyBinding機能を追加</remarks>
        private static void ElementPropertyBindingImplAttributes(FrameworkElement targetElement, object value, PropertyInfo pi, ElementPropertyBindingAttribute epba)
        {
            //対象エレメント有効判定
            if (targetElement == null)
            {
                //無効の場合終了
                return;
            }

            //プロパティ情報有効判定
            if (pi == null)
            {
                //無効の場合終了
                return;
            }

            //属性有効判定
            if (epba == null)
            {
                //無効の場合終了
                return;
            }

            //エレメントから対象エレメント取得
            DependencyObject target = targetElement;
            string targetElementName = targetElement.Name;


            //属性情報取得
            string targetPropertyName = epba.TargetPropertyName;
            Type? converterType = epba.ConverterType;
            object? param = epba.ConverterParameter;

            //プロパティ名を取得(=path)
            string pname = pi.Name;

            //ターゲットプロパティ名が空の場合は更にプロパティ値(=クラスインスタンス)のプロパティをバインドする
            if (string.IsNullOrEmpty(targetPropertyName))
            {
                //プロパティの値を取得
                object? v = pi.GetValue(value);

                if(v == null)
                {
                    //無効の場合は何もしない
                }
                else
                {
                    //有効の場合はエレメントプロパティバインディング
                    BindingHelper.ElementPropertyBindingImpl(targetElement, v);
                }

                //ここで終了
                return;
            }

            //依存関係プロパティを取得する
            DependencyProperty? dp = BindingHelper.GetDependencyProperty(target, targetPropertyName);
            if (dp == null)
            {
                //フィールド値が取得できない場合はエラー終了
                LOGGER.Warn($"ターゲットにプロパティが存在しません [name='{targetElementName}' property='{targetPropertyName}' vm.property='{pname}'");
                return;
            }

            //TypeからIValueConverterを取得
            IValueConverter? converter = BindingHelper.NO_CONVERTER;
            if (converterType != null)
            {
                try
                {
                    //インスタンス生成
                    object? obj = Activator.CreateInstance(converterType);
                    if (obj == null)
                    {
                        //生成失敗の場合
                        LOGGER.Warn($"ValueConverterの生成に失敗しました [name='{targetElementName}' property='{targetPropertyName}' vm.property='{pname}'");
                        return;
                    }

                    //IValueConverterにキャスト
                    converter = obj as IValueConverter;
                    if (converter == null)
                    {
                        //キャストできない場合はエラー終了
                        LOGGER.Warn($"IValueConverterにキャストできませんでした [name='[targetElementName]' property='{targetPropertyName}' vm.property='{pname}'");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    //例外が発生した場合握りつぶして終了
                    LOGGER.WarnEx($"ValueConverterの生成で例外発生 [name='{targetElementName}' property='{targetPropertyName}' vm.property='{pname}'", ex);
                    return;
                }
            }

            //バインディング
            BindingHelper.SetBinding(target, dp, value, pname, converter, param);
            LOGGER.Info($"ターゲットプロパティをバインドしました [name='{targetElementName}' property='{targetPropertyName}' vm.property='{pname}'");
        }

        /// <summary>
        /// 特定依存関係プロパティ配列
        /// </summary>
        /// <remarks>主に添付プロパティを指定する。その他として依存関係プロパティ名＋"Property"となっていないもの</remarks>
        private static readonly DependencyProperty[] SPECIFY_DEPENDENCY_PROPERTY_ARRAY =
        {
            Canvas.TopProperty,
            Canvas.LeftProperty,
            Grid.RowProperty,
            Grid.RowSpanProperty,
            Grid.ColumnProperty,
            Grid.ColumnSpanProperty,
        };

        /// <summary>
        /// 特定依存関係プロパティ辞書
        /// </summary>
        /// <remarks>静的コンストラクタに特定依存関係プロパティ配列から生成</remarks>
        private static readonly Dictionary<string, DependencyProperty> SPECIFY_DEPENDENCY_PROPERTY_DIC = new Dictionary<string, DependencyProperty>();

        /// <summary>
        /// 静的コンストラクタ
        /// </summary>
        static BindingHelper()
        {
            //特定依存関係プロパティ変換
            DependencyProperty[] dps = BindingHelper.SPECIFY_DEPENDENCY_PROPERTY_ARRAY;
            Dictionary<string, DependencyProperty> dic = BindingHelper.SPECIFY_DEPENDENCY_PROPERTY_DIC;
            foreach (DependencyProperty dp in dps)
            {
                if (dp == null)
                {
                    //無効の場合は何もしない
                }
                else
                {
                    //クラス名取得
                    string cname = dp.OwnerType.Name;
                    //プロパティ名取得
                    string pname = dp.Name;
                    //フル名取得
                    string name = cname + "." + pname;

                    //辞書に追加
                    dic.Add(name, dp);
                }
            }
        }

        /// <summary>
        /// 依存関係プロパティ取得
        /// </summary>
        /// <param name="target"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        private static DependencyProperty? GetDependencyProperty(DependencyObject target, string propertyName)
        {
            //ターゲット有効判定
            if (target == null)
            {
                //無効の場合終了
                return null;
            }

            //プロパティ名有効判定
            bool isNull = string.IsNullOrEmpty(propertyName);
            if (isNull)
            {
                //無効の場合終了
                return null;
            }

            //-------------------------------------------
            //特定名
            {
                DependencyProperty? result = null;
                bool ret = BindingHelper.SPECIFY_DEPENDENCY_PROPERTY_DIC.TryGetValue(propertyName, out result);
                if (ret)
                {
                    return result;
                }
            }

            //-------------------------------------------
            //一般名
            {
                string fname = propertyName + "Property";
                Type t = target.GetType();
                FieldInfo? fi = t.GetField(fname, BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
                if (fi == null)
                {
                    //フィールドが取得できない場合はエラー終了
                    LOGGER.Warn($"ターゲットプロパティが見つからない [property='{propertyName}'");
                    return null;
                }

                //フィールド値を取得する
                DependencyProperty? result = fi.GetValue(null) as DependencyProperty;

                //終了
                return result;
            }
        }
    }
}
