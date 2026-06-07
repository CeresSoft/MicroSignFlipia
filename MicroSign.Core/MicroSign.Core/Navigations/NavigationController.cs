using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

#nullable disable //Variousから移植したコードなので、Null非許容警告を無効化します

namespace MicroSign.Core.Navigations
{
    /// <summary>
    /// ナビゲーションコントローラー
    /// </summary>
    /// <remarks>
    /// 2021.05.19:CS)杉原:埠頭監視向けに作成
    /// WPF標準のPageとNavigationServiceを使った方法だと
    /// <see cref="http://sourcechord.hatenablog.com/entry/2016/02/01/003758"/>
    /// > ナビゲーションの履歴管理が邪魔
    ///     > Frame/NavigationWindowを使ったページ遷移では、特に何もしなくてもページ遷移の履歴などが記録されます。
    ///     > この履歴がviewクラスの参照を掴んでいて、なかなかGCされなくなる
    ///     > ナビゲーション関係のコマンドを受け付けると、勝手にナビゲーション動作をする。。
    ///         > F5キー押したらページリロードする・・・
    ///         > 一方通行なページ遷移しかさせたくないのに、マウスの戻るボタン押したら前のページに戻ってしまう・・・
    ///         > などなど。
    ///     > ナビゲーション時に音が出るケースがある
    ///         > OSの設定によっては、ページ遷移するときに、エクスプローラで移動したときとかに鳴るような「カチッ」という音がします。
    ///         > Win8.x系/Win10では標準ではオフになってるようですが、Win7では・・・
    /// > ナビゲーション先の指定方法が微妙
    ///     > XAML上でページ遷移先の定義をするときは、遷移先ページのURI指定。
    ///     > ⇒もっと厳密に型で指定したい
    /// > NavigationSeriviceの管理する範囲が微妙に扱いにくい
    ///     > NavigationServiceは、Frameコントロールの内部に位置する。
    ///         > Frame内部からページ遷移するのと、Frame外部からページ遷移するときとで、扱い方が変わる
    ///         > ページ遷移用のメニューを作るときなどは、Frame外部からのページ遷移を多用するかと思います。
    /// といろいろ問題がありページの制御が思った通りにできないので
    /// オレオレ仕様のナビゲーションを作成しました
    /// 先のURLにある仕様だと今度はオーバースペック気味なので、もっと単純な物にします
    /// -> ちょうどUnity向けに作成したAppControllerが適度に使いやすい(遷移とコールができる)のでそれを参考に作成
    /// </remarks>
    public partial class NavigationController : DependencyObject
    {
        //2026.06.07:CS)杉原:LOGGERを修正 >>>>> ここから
        ///// <summary>
        ///// LOG4NETのロガー
        ///// </summary>
        //private static readonly log4net.ILog LOGGER = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //----------
        /// <summary>
        /// LOG4NETのロガー
        /// </summary>
        private static readonly MicroSignLogger LOGGER = MicroSignLogger.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType!);
        //2026.06.07:CS)杉原:LOGGERを修正 <<<<< ここまで

        /// <summary>
        /// ナビゲーションで使うパネル
        /// </summary>
        /// <remarks>コンテンツとして表示するエレメントを複数追加するのでPanelにします</remarks>
        private Panel _NavigationPanel = null;

        /// <summary>
        /// エレメントスタック用のクラス
        /// </summary>
        /// <remarks>2021.12.28:CS)杉原:追加</remarks>
        private class ElementStackData
        {
            /// <summary>
            /// エレメント
            /// </summary>
            public FrameworkElement Element { get; protected set; } = null;

            /// <summary>
            /// 戻りコールバック
            /// </summary>
            public System.Action<object> ReturnCallback { get; protected set; } = null;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="element"></param>
            /// <param name="returncallback"></param>
            public ElementStackData(FrameworkElement element, System.Action<object> returncallback)
            {
                this.Element = element;
                this.ReturnCallback = returncallback;
            }
        }

        /// <summary>
        /// エレメントスタック
        /// </summary>
        /// <remarks>表示しているコンテンツのスタック状態を管理します</remarks>
        private Stack<ElementStackData> _ElementStack = new Stack<ElementStackData>();

        /// <summary>
        /// 現在表示しているエレメント
        /// </summary>
        private FrameworkElement _CurrentElement = null;

        /// <summary>
        /// 現在表示しているエレメントの戻り時に呼び出す関数
        /// </summary>
        /// <remarks>2021.12.28:CS)杉原:追加</remarks>
        private System.Action<object> _CurrentReturnCallback = null;

        /// <summary>
        /// ウインドウクローズフラグ
        /// </summary>
        public bool IsWindowClosed { get; protected set; } = false;

        /// <summary>
        /// ホストに戻った時に呼出関数
        /// </summary>
        /// <remarks>
        /// 2021.05.22:CS)杉原:
        /// オーバーラップしか使わない場合、最後はナビゲーションしているElementHostに戻ってくるが
        /// ナビゲーションしているElementHostには戻り値を渡す関数が無いので、
        /// 自エレメントをコンストラクタで指定できるようにする
        /// </remarks>
        private INavigationReturnParameter HostReturn { get; set; } = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="naviPanel">コンテンツを表示する領域になるPanel</param>
        public NavigationController(Panel naviPanel): this(naviPanel, null)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="naviPanel">コンテンツを表示する領域になるPanel</param>
        /// <param name="hostReturn">ホストに戻った時に呼び出すインターフェイス</param>
        public NavigationController(Panel naviPanel, INavigationReturnParameter hostReturn)
        {
            //パネルを保存
            this._NavigationPanel = naviPanel;

            //パネルの親のWindowのクローズを購読
            if(naviPanel == null)
            {
                //無効の場合は何もしない
            }
            else
            {
                //有効の場合はウインドウを取得
                Window w = Window.GetWindow(naviPanel);
                if(w == null)
                {
                    //無効の場合は何もしない
                }
                else
                {
                    //有効の場合はクローズを購読
                    w.Closed += this.OnWindowClose;
                }
            }

            //ホストのインターフェイスを保存
            this.HostReturn = hostReturn;
        }

        /// <summary>
        /// ウインドウクローズ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void OnWindowClose(object sender, System.EventArgs e)
        {
            //ウインドウクローズフラグをtrueにする
            this.IsWindowClosed = true;
        }

        /// <summary>
        /// ナビゲーションコントローラー初期化
        /// </summary>
        /// <param name="naviPanel"></param>
        /// <param name="hostReturn"></param>
        /// <returns></returns>
        public static NavigationController Initialize(Panel naviPanel)
        {
            //ナビゲーションコントローラー生成
            NavigationController result = new NavigationController(naviPanel);

            //パネルにナビゲーションコントローラーを設定
            NavigationController.SetNavigationController(naviPanel, result);

            //終了
            return result;
        }

        /// <summary>
        /// ナビゲーションコントローラー初期化
        /// </summary>
        /// <param name="naviPanel"></param>
        /// <param name="hostReturn"></param>
        /// <returns></returns>
        public static NavigationController Initialize(Panel naviPanel, INavigationReturnParameter hostReturn)
        {
            //ナビゲーションコントローラー生成
            NavigationController result = new NavigationController(naviPanel, hostReturn);

            //パネルにナビゲーションコントローラーを設定
            NavigationController.SetNavigationController(naviPanel, result);

            //終了
            return result;
        }

        /// <summary>
        /// 一番上のエレメントを取得
        /// </summary>
        /// <returns></returns>
        private ElementStackData PopElement()
        {
            int n = this._ElementStack.Count;
            if(CommonConsts.Collection.Empty < n)
            {
                //要素が存在する場合
                ElementStackData result = this._ElementStack.Pop();
                return result;
            }
            else
            {
                //要素が存在しない場合
                return null;
            }
        }

        /// <summary>
        /// 遷移処理
        /// </summary>
        /// <param name="moveElement">遷移先のエレメント</param>
        /// <remarks>
        /// 現在表示しているエレメントを破棄し、新しいエレメントを表示します
        /// 【注意】コールバックの変更は行いません(ReturnしたらCall/Overwrapされたときのコールバックを呼び出します)
        /// これは設定画面のような、Callで画面Aを呼び出し、この画面Aが画面B遷移し、画面BでReturnした場合
        /// 画面Aを呼びだした時に指定したコールバックを呼び出すパターンが多いからと判断したためです
        /// </remarks>
        public void Move(FrameworkElement moveElement)
        {
            this.Move(moveElement, null);
        }

        /// <summary>
        /// 遷移処理
        /// </summary>
        /// <param name="moveElement">遷移先のエレメント</param>
        /// <param name="arg">遷移先エレメントに渡すパラメータ</param>
        /// <remarks>
        /// 現在表示しているエレメントを破棄し、新しいエレメントを表示します
        /// 【注意】コールバックの変更は行いません(ReturnしたらCall/Overwrapされたときのコールバックを呼び出します)
        /// これは設定画面のような、Callで画面Aを呼び出し、この画面Aが画面B遷移し、画面BでReturnした場合
        /// 画面Aを呼びだした時に指定したコールバックを呼び出すパターンが多いからと判断したためです
        /// </remarks>
        public void Move(FrameworkElement moveElement, object arg)
        {
            //移動先エレメント有効判定
            if(moveElement == null)
            {
                //無効の場合は即終了
                LOGGER.Warn("遷移先エレメントが無効です");
                return;
            }
            else
            {
                //有効の場合は処理続行
            }

            //ナビゲーションパネル有効判定
            Panel naviPanel = this._NavigationPanel;
            if (naviPanel == null)
            {
                //無効の場合は即終了
                LOGGER.Warn("ナビゲーションパネルが無効です");
                return;
            }
            else
            {
                //有効の場合は処理続行
            }

            //現在のエレメントを破棄する
            FrameworkElement currentElement = this._CurrentElement;
            if (currentElement == null)
            {
                //現在のエレメントが無効の場合は何もしない
            }
            else
            {
                //現在エレメントから遷移で終了のインターフェイスを取得
                // >> 2021.12.28:CS)杉原:追加
                {
                    INavigationMove naviParam = currentElement as INavigationMove;
                    if (naviParam == null)
                    {
                        //インターフェイスが無い場合は何もしない
                    }
                    else
                    {
                        //インターフェイスがある場合は呼び出し
                        naviParam.NavigationMove(moveElement, arg);
                    }
                }

                //カレントエレメントを解除
                naviPanel.Children.Remove(currentElement);
            }

            //ナビゲーションコントローラー(=this)を設定
            // >> ナビゲーションコントローラーを使って遷移したエレメント内で
            // >> this.NavigationXxxx()系関数が使えるようナビゲーションコントローラー
            // >> を設定しておく
            NavigationController.SetNavigationController(moveElement, this);

            //移動先のエレメントを設定
            naviPanel.Children.Add(moveElement);
            this._CurrentElement = moveElement;

            //遷移先エレメントからパラメータ受け渡しのインターフェイスを取得
            {
                INavigationReceiveParameter naviParam = moveElement as INavigationReceiveParameter;
                if(naviParam == null)
                {
                    //インターフェイスが無い場合は何もしない
                }
                else
                {
                    //インターフェイスがある場合は呼び出し
                    naviParam.NavigationReceiveParameter(NavigationActions.Move, currentElement, arg);
                }
            }
        }

        /// <summary>
        /// 呼出処理
        /// </summary>
        /// <param name="callElement">呼出先エレメント</param>
        /// <remarks>現在表示しているエレメントを非表示時にし、新しいエレメントを表示します</remarks>
        public void Call(FrameworkElement callElement)
        {
            this.Call(callElement, null, null);
        }

        /// <summary>
        /// 呼出処理
        /// </summary>
        /// <param name="callElement">呼出先エレメント</param>
        /// <param name="arg">呼出先エレメントに渡すパラメータ</param>
        /// <remarks>現在表示しているエレメントを非表示時にし、新しいエレメントを表示します</remarks>
        public void Call(FrameworkElement callElement, object arg)
        {
            this.Call(callElement, arg, null);
        }

        /// <summary>
        /// 呼出処理
        /// </summary>
        /// <param name="callElement">呼出先エレメント</param>
        /// <param name="returnCallback">戻り時のコールバック</param>
        /// <remarks>現在表示しているエレメントを非表示時にし、新しいエレメントを表示します</remarks>
        public void Call(FrameworkElement callElement, System.Action<object> returnCallback)
        {
            this.Call(callElement, null, returnCallback);
        }

        /// <summary>
        /// 呼出処理
        /// </summary>
        /// <param name="callElement">呼出先エレメント</param>
        /// <param name="arg">呼出先エレメントに渡すパラメータ</param>
        /// <param name="returnCallback">戻り時のコールバック</param>
        /// <remarks>現在表示しているエレメントを非表示時にし、新しいエレメントを表示します</remarks>
        public void Call(FrameworkElement callElement, object arg, System.Action<object> returnCallback)
        {
            //呼出先エレメント有効判定
            if (callElement == null)
            {
                //無効の場合は即終了
                LOGGER.Warn("呼出先エレメントが無効です");
                return;
            }
            else
            {
                //有効の場合は処理続行
            }

            //ナビゲーションパネル有効判定
            Panel naviPanel = this._NavigationPanel;
            if (naviPanel == null)
            {
                //無効の場合は即終了
                LOGGER.Warn("ナビゲーションパネルが無効です");
                return;
            }
            else
            {
                //有効の場合は処理続行
            }

            //現在のエレメントを非表示にしてスタックに保存
            FrameworkElement currentElement = this._CurrentElement;
            System.Action<object> currentCallback = this._CurrentReturnCallback;
            if (currentElement == null)
            {
                //何もしない
                //2025.04.19:CS)杉原:ナビゲーションパネル内の子要素に対応 >>>>> ここから
                //----------
                // >> ナビゲーションパネル内の子要素を非表示にする
                int n = naviPanel.Children.Count;
                for (int i = CommonConsts.Index.First; i < n; i += CommonConsts.Index.Step)
                {
                    UIElement child = naviPanel.Children[i];
                    child.Visibility = Visibility.Collapsed;
                }
                //2025.04.19:CS)杉原:ナビゲーションパネル内の子要素に対応 <<<<< ここまで
            }
            else
            {
                //カレントエレメントをスタックに追加
                ElementStackData stackData = new ElementStackData(currentElement, currentCallback);
                this._ElementStack.Push(stackData);
                //カレントエレメントを非表示にする
                currentElement.Visibility = Visibility.Collapsed;
            }

            //ナビゲーションコントローラー(=this)を設定
            // >> ナビゲーションコントローラーを使って遷移したエレメント内で
            // >> this.NavigationXxxx()系関数が使えるようナビゲーションコントローラー
            // >> を設定しておく
            NavigationController.SetNavigationController(callElement, this);

            //新しいエレメントを設定
            naviPanel.Children.Add(callElement);
            this._CurrentElement = callElement;
            this._CurrentReturnCallback = returnCallback;

            //遷移先エレメントからパラメータ受け渡しのインターフェイスを取得
            {
                INavigationReceiveParameter naviParam = callElement as INavigationReceiveParameter;
                if (naviParam == null)
                {
                    //インターフェイスが無い場合は何もしない
                }
                else
                {
                    //インターフェイスがある場合は呼び出し
                    naviParam.NavigationReceiveParameter(NavigationActions.Call, currentElement, arg);
                }
            }
        }

        /// <summary>
        /// オーバーラップ処理
        /// </summary>
        /// <param name="callElement">呼出先エレメント</param>
        /// <remarks>現在表示しているエレメントはそのままで、新しいエレメントをオーバーラップ表示します</remarks>
        public void Overwrap(FrameworkElement callElement)
        {
            this.Overwrap(callElement, null, null);
        }

        /// <summary>
        /// オーバーラップ処理
        /// </summary>
        /// <param name="callElement">呼出先エレメント</param>
        /// <param name="arg">呼出先エレメントに渡すパラメータ</param>
        /// <remarks>現在表示しているエレメントはそのままで、新しいエレメントをオーバーラップ表示します</remarks>
        public void Overwrap(FrameworkElement callElement, object arg)
        {
            this.Overwrap(callElement, arg, null);
        }

        /// <summary>
        /// オーバーラップ処理
        /// </summary>
        /// <param name="callElement">呼出先エレメント</param>
        /// <param name="returnCallback">戻り時のコールバック</param>
        /// <remarks>現在表示しているエレメントはそのままで、新しいエレメントをオーバーラップ表示します</remarks>
        public void Overwrap(FrameworkElement callElement, System.Action<object> returnCallback)
        {
            this.Overwrap(callElement, null, returnCallback);
        }

        /// <summary>
        /// オーバーラップ処理
        /// </summary>
        /// <param name="callElement">呼出先エレメント</param>
        /// <param name="arg">呼出先エレメントに渡すパラメータ</param>
        /// <param name="returnCallback">戻り時のコールバック</param>
        /// <remarks>現在表示しているエレメントはそのままで、新しいエレメントをオーバーラップ表示します</remarks>
        public void Overwrap(FrameworkElement callElement, object arg, System.Action<object> returnCallback)
        {
            //呼出先エレメント有効判定
            if (callElement == null)
            {
                //無効の場合は即終了
                LOGGER.Warn("呼出先エレメントが無効です");
                return;
            }
            else
            {
                //有効の場合は処理続行
            }

            //ナビゲーションパネル有効判定
            Panel naviPanel = this._NavigationPanel;
            if (naviPanel == null)
            {
                //無効の場合は即終了
                LOGGER.Warn("ナビゲーションパネルが無効です");
                return;
            }
            else
            {
                //有効の場合は処理続行
            }

            //現在のエレメントを非表示にしてスタックに保存
            FrameworkElement currentElement = this._CurrentElement;
            System.Action<object> currentCallback = this._CurrentReturnCallback;
            if (currentElement == null)
            {
                //何もしない
                //2025.04.19:CS)杉原:ナビゲーションパネル内の子要素に対応 >>>>> ここから
                //----------
                // >> ナビゲーションパネル内の子要素を無効にする
                int n = naviPanel.Children.Count;
                for(int i = CommonConsts.Index.First; i < n; i+= CommonConsts.Index.Step)
                {
                    UIElement child = naviPanel.Children[i];
                    child.IsEnabled = false;
                }
                //2025.04.19:CS)杉原:ナビゲーションパネル内の子要素に対応 <<<<< ここまで
            }
            else
            {
                //カレントエレメントをスタックに追加
                ElementStackData stackData = new ElementStackData(currentElement, currentCallback);
                this._ElementStack.Push(stackData);
                // >>>>> 非表示にしない
                ////カレントエレメントを非表示にする
                //currentElement.Visibility = Visibility.Collapsed;
                // <<<<< 非表示にしない
                //2025.04.19:CS)杉原:操作無効にする >>>>> ここから
                //----------
                currentElement.IsEnabled = false;
                //2025.04.19:CS)杉原:操作無効にする <<<<< ここまで
            }

            //ナビゲーションコントローラー(=this)を設定
            // >> ナビゲーションコントローラーを使って遷移したエレメント内で
            // >> this.NavigationXxxx()系関数が使えるようナビゲーションコントローラー
            // >> を設定しておく
            NavigationController.SetNavigationController(callElement, this);

            //新しいエレメントを設定
            naviPanel.Children.Add(callElement);
            this._CurrentElement = callElement;
            this._CurrentReturnCallback = returnCallback;

            //遷移先エレメントからパラメータ受け渡しのインターフェイスを取得
            {
                INavigationReceiveParameter naviParam = callElement as INavigationReceiveParameter;
                if (naviParam == null)
                {
                    //インターフェイスが無い場合は何もしない
                }
                else
                {
                    //インターフェイスがある場合は呼び出し
                    naviParam.NavigationReceiveParameter(NavigationActions.Overwrap, currentElement, arg);
                }
            }
        }

        /// <summary>
        /// 戻り処理
        /// </summary>
        /// <remarks>現在表示しているエレメントを破棄し、呼出元のエレメントを表示します</remarks>
        public void Return()
        {
            this.Return(null);
        }

        /// <summary>
        /// 戻り処理
        /// </summary>
        /// <param name="arg">呼出元エレメントに返却するパラメータ</param>
        /// <remarks>現在表示しているエレメントを破棄し、呼出元のエレメントを表示します</remarks>
        public void Return(object arg)
        {
            //ナビゲーションパネル有効判定
            Panel naviPanel = this._NavigationPanel;
            if (naviPanel == null)
            {
                //無効の場合は即終了
                LOGGER.Warn("ナビゲーションパネルが無効です");
                return;
            }
            else
            {
                //有効の場合は処理続行
            }

            //現在表示しているエレメントを無効にする
            FrameworkElement currentElement = this._CurrentElement;
            if (currentElement == null)
            {
                //無効の場合は即終了
                LOGGER.Warn("カレントエレメントが無効です");
                return;
            }
            else
            {
                //カレントエレメントを解除
                naviPanel.Children.Remove(currentElement);
            }

            //戻りコールバック取得
            System.Action<object> currentCallback = this._CurrentReturnCallback;

            //ひとつ前のエレメントを取得
            ElementStackData stackData = this.PopElement();
            FrameworkElement prevElement = stackData?.Element;
            System.Action<object> prevCallback = stackData?.ReturnCallback;
            if (prevElement == null)
            {
                //ひとつ前が無い場合はカレントをnullにして終了
                this._CurrentElement = null;
                this._CurrentReturnCallback = null;
                LOGGER.Debug("前エレメントがなくなりました");
                //2025.04.19:CS)杉原:ナビゲーションパネル内の子要素に対応 >>>>> ここから
                //----------
                // >> ナビゲーションパネル内の子要素を無効にする
                int n = naviPanel.Children.Count;
                for (int i = CommonConsts.Index.First; i < n; i += CommonConsts.Index.Step)
                {
                    UIElement child = naviPanel.Children[i];
                    child.IsEnabled = true;
                    child.Visibility = Visibility.Visible;
                }
                //2025.04.19:CS)杉原:ナビゲーションパネル内の子要素に対応 <<<<< ここまで
                //2021.05.22:CS)杉原 >>>>> ここから
                // >> オーバーラップ品使わない場合、最後はナビゲーションしているElementHostに戻ってくるが
                // >> 戻り値を渡す関数が無いので、コンストラクタで指定できるようにする
                INavigationReturnParameter hostReturn = this.HostReturn;
                if(hostReturn == null)
                {
                    //ホスト戻り無し
                    LOGGER.Debug("ホスト戻りなし");
                }
                else
                {
                    //ホスト戻りあり
                    LOGGER.Debug("ホスト戻りあり");
                    hostReturn.NavigationReturnParameter(currentElement, arg);
                }
                //2021.05.22:CS)杉原 <<<<< ここまで
            }
            else
            {
                //カレントエレメントに設定
                this._CurrentElement = prevElement;
                this._CurrentReturnCallback = prevCallback;

                //非表示にしてあるので表示に設定
                prevElement.Visibility = Visibility.Visible;

                //2025.04.19:CS)杉原:操作無効にする >>>>> ここから
                //----------
                // >> 操作を有効にする
                prevElement.IsEnabled = true;
                //2025.04.19:CS)杉原:操作無効にする <<<<< ここまで

                //遷移先エレメントからパラメータ受け渡しのインターフェイスを取得
                {
                    INavigationReturnParameter naviParam = prevElement as INavigationReturnParameter;
                    if (naviParam == null)
                    {
                        //インターフェイスが無い場合は何もしない
                    }
                    else
                    {
                        //インターフェイスがある場合は呼び出し
                        naviParam.NavigationReturnParameter(currentElement, arg);
                    }
                }
            }

            //戻りコールバック呼び出し
            if(currentCallback == null)
            {
                //戻りコールバックが無い場合はなにもしない
            }
            else
            {
                //戻りコールバックが有効の場合は呼び出し
                currentCallback(arg);
            }
        }

        /// <summary>
        /// OK通知
        /// </summary>
        /// <remarks>現在表示しているエレメントにOKを通知します</remarks>
        /// <returns>
        /// 表示しているエレメントでINavigationOkインターフェイスを実装していない場合にfalseになります
        /// Ok処理の結果ではないので注意してください。
        /// </returns>
        public bool Ok()
        {
            return this.Ok(null);
        }

        /// <summary>
        /// OK通知
        /// </summary>
        /// <param name="arg">表示エレメントに渡すパラメータ</param>
        /// <remarks>現在表示しているエレメントにOKを通知します</remarks>
        /// <returns>
        /// 表示しているエレメントでINavigationOkインターフェイスを実装していない場合にfalseになります
        /// Ok処理の結果ではないので注意してください。
        /// </returns>
        public bool Ok(object arg)
        {
            //現在表示しているエレメントを取得
            FrameworkElement currentElement = this._CurrentElement;
            if (currentElement == null)
            {
                //カレントエレメントが無効の場合何もしない
                return false;
            }
            else
            {
                //カレントエレメントが有効の場合は処理続行
            }

            //カレントエレメントからキャンセルのインターフェイスを取得して呼び出し
            {
                INavigationOk naviOk = currentElement as INavigationOk;
                if (naviOk == null)
                {
                    //インターフェイスが無い場合は何もしない
                    return false;
                }
                else
                {
                    //インターフェイスがある場合は呼び出し
                    naviOk.NavigationOk(arg);
                    return true;
                }
            }
        }

        /// <summary>
        /// キャンセル通知
        /// </summary>
        /// <remarks>現在表示しているエレメントにキャンセルを通知します</remarks>
        /// <returns>
        /// 表示しているエレメントでINavigationCancelインターフェイスを実装していない場合にfalseになります
        /// キャンセル処理の結果ではないので注意してください。
        /// </returns>
        public bool Cancel()
        {
            return this.Cancel(null);
        }

        /// <summary>
        /// キャンセル通知
        /// </summary>
        /// <param name="arg">表示エレメントに渡すパラメータ</param>
        /// <remarks>現在表示しているエレメントにキャンセルを通知します</remarks>
        /// <returns>
        /// 表示しているエレメントでINavigationCancelインターフェイスを実装していない場合にfalseになります
        /// キャンセル処理の結果ではないので注意してください。
        /// </returns>
        public bool Cancel(object arg)
        {
            //現在表示しているエレメントを取得
            FrameworkElement currentElement = this._CurrentElement;
            if (currentElement == null)
            {
                //カレントエレメントが無効の場合何もしない
                return false;
            }
            else
            {
                //カレントエレメントが有効の場合は処理続行
            }

            //カレントエレメントからキャンセルのインターフェイスを取得して呼び出し
            {
                INavigationCancel naviCancel = currentElement as INavigationCancel;
                if (naviCancel == null)
                {
                    //インターフェイスが無い場合は何もしない
                    return false;
                }
                else
                {
                    //インターフェイスがある場合は呼び出し
                    naviCancel.NavigationCancel(arg);
                    return true;
                }
            }
        }

        /// <summary>
        /// 表示しているエレメント
        /// </summary>
        public FrameworkElement Current
        {
            get
            {
                return this._CurrentElement;
            }
        }

        /// <summary>
        /// ナビゲーションコントローラー取得
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static NavigationController GetNavigationController(DependencyObject obj)
        {
            NavigationController result = obj.GetValue(NavigationControllerProperty) as NavigationController;
            return result;
        }

        /// <summary>
        /// ナビゲーションコントローラー設定
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="navigation"></param>
        public static void SetNavigationController(DependencyObject obj, NavigationController navigation)
        {
            obj.SetValue(NavigationControllerProperty, navigation);
        }

        /// <summary>
        /// ナビゲーションコントローラー添付プロパティ
        /// </summary>
        public static readonly DependencyProperty NavigationControllerProperty =
            DependencyProperty.Register("NavigationController", typeof(NavigationController), typeof(NavigationController), new PropertyMetadata(null));
    }
}
