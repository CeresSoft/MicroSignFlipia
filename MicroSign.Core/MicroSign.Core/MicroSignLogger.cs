using System;
using System.Reflection;

namespace MicroSign.Core
{
    /// <summary>
    /// 共通ロガー
    /// </summary>
    public class MicroSignLogger
    {
        //2026.06.02:CS0杉原:LOGGERを変更 >>>>> ここから
        ///// <summary>
        ///// メッセージだけのロガーデリゲート
        ///// </summary>
        ///// <param name="message">メッセージ(log4netの定義がobjectになっている)</param>
        //public delegate void LoggerMessageDelegate(object message);
        //
        ///// <summary>
        ///// 例外付きのロガーデリゲート
        ///// </summary>
        ///// <param name="message">メッセージ(log4netの定義がobjectになっている)</param>
        ///// <param name="ex">例外</param>
        //public delegate void LoggerExceptionDelegate(object message, Exception ex);
        //
        ///// <summary>
        ///// 呼び出し元関数情報取得
        ///// </summary>
        ///// <param name="frame">スタックフレーム</param>
        ///// <param name="message">メッセージ</param>
        ///// <returns>呼び出し元関数情報の文字列</returns>
        //private static string CreateLogText(System.Diagnostics.StackFrame frame, object message)
        //{
        //    string typeName = "(null)";
        //    string methodName = "(null)";
        //
        //    //関数取得
        //    System.Reflection.MethodBase? method = frame.GetMethod();
        //    if(method == null)
        //    {
        //        //メソッドが無効の場合は何もしない
        //    }
        //    else
        //    {
        //        //メソッドが有効の場合
        //        Type? type = method.DeclaringType;
        //        if (type == null)
        //        {
        //            //型が無効の場合は型名は取得しない
        //        }
        //        else
        //        {
        //            //型が有効の場合は名前を取得する
        //            typeName = type.Name;
        //        }
        //
        //        //関数名を取得
        //        methodName = method.Name;
        //    }
        //
        //    //終了
        //    return $"<{typeName}.{methodName}> - {message}";
        //}
        //
        ///// <summary>
        ///// デバッグメッセージログデリゲート
        ///// </summary>
        //public static LoggerMessageDelegate? DebugMessageAction = null;
        //
        ///// <summary>
        ///// デバッグ例外ログデリゲート
        ///// </summary>
        //public static LoggerExceptionDelegate? DebugExceptionAction = null;
        //
        ///// <summary>
        ///// デバッグメッセージログ
        ///// </summary>
        ///// <param name="message">メッセージ</param>
        ///// <returns>メッセージがそのまま返されます</returns>
        //public static string Debug(string message)
        //{
        //    LoggerMessageDelegate? action = CommonLogger.DebugMessageAction;
        //    if(action == null)
        //    {
        //        //デリゲートが無効の場合は何もしない
        //    }
        //    else
        //    {
        //        //デリゲートが有効の場合はログ出力
        //        System.Diagnostics.StackFrame frame = new System.Diagnostics.StackFrame(1);
        //        string logText = CommonLogger.CreateLogText(frame, message);
        //        action.Invoke(logText);
        //    }
        //
        //    //終了
        //    return message;
        //}
        //
        ///// <summary>
        ///// デバッグ例外ログ
        ///// </summary>
        ///// <param name="message">メッセージ</param>
        ///// <param name="ex">例外</param>
        ///// <returns>メッセージがそのまま返されます</returns>
        //public static string Debug(string message, Exception ex)
        //{
        //    LoggerExceptionDelegate? action = CommonLogger.DebugExceptionAction;
        //    if(action == null)
        //    {
        //        //デリゲートが無効の場合は何もしない
        //    }
        //    else
        //    {
        //        //デリゲートが有効の場合はログ出力
        //        System.Diagnostics.StackFrame frame = new System.Diagnostics.StackFrame(1);
        //        string logText = CommonLogger.CreateLogText(frame, message);
        //        action.Invoke(logText, ex);
        //    }
        //
        //    //終了
        //    return message;
        //}
        //
        ///// <summary>
        ///// 情報メッセージログデリゲート
        ///// </summary>
        //public static LoggerMessageDelegate? InfoMessageAction = null;
        //
        ///// <summary>
        ///// 情報例外ログデリゲート
        ///// </summary>
        //public static LoggerExceptionDelegate? InfoExceptionAction = null;
        //
        ///// <summary>
        ///// 情報メッセージログ
        ///// </summary>
        ///// <param name="message">メッセージ</param>
        ///// <returns>メッセージがそのまま返されます</returns>
        //public static string Info(string message)
        //{
        //    LoggerMessageDelegate? action = CommonLogger.InfoMessageAction;
        //    if(action == null)
        //    {
        //        //デリゲートが無効の場合は何もしない
        //    }
        //    else
        //    {
        //        //デリゲートが有効の場合はログ出力
        //        System.Diagnostics.StackFrame frame = new System.Diagnostics.StackFrame(1);
        //        string logText = CommonLogger.CreateLogText(frame, message);
        //        action.Invoke(logText);
        //    }
        //
        //    //終了
        //    return message;
        //}
        //
        ///// <summary>
        ///// 情報例外ログ
        ///// </summary>
        ///// <param name="message">メッセージ</param>
        ///// <param name="ex">例外</param>
        ///// <returns>メッセージがそのまま返されます</returns>
        //public static string Info(string message, Exception ex)
        //{
        //    LoggerExceptionDelegate? action = CommonLogger.InfoExceptionAction;
        //    if (action == null)
        //    {
        //        //デリゲートが無効の場合は何もしない
        //    }
        //    else
        //    {
        //        //デリゲートが有効の場合はログ出力
        //        System.Diagnostics.StackFrame frame = new System.Diagnostics.StackFrame(1);
        //        string logText = CommonLogger.CreateLogText(frame, message);
        //        action.Invoke(logText, ex);
        //    }
        //
        //    //終了
        //    return message;
        //}
        //
        ///// <summary>
        ///// 警告メッセージログデリゲート
        ///// </summary>
        //public static LoggerMessageDelegate? WarnMessageAction = null;
        //
        ///// <summary>
        ///// 警告例外ログデリゲート
        ///// </summary>
        //public static LoggerExceptionDelegate? WarnExceptionAction = null;
        //
        ///// <summary>
        ///// 警告メッセージログ
        ///// </summary>
        ///// <param name="message">メッセージ</param>
        ///// <returns>メッセージがそのまま返されます</returns>
        //public static string Warn(string message)
        //{
        //    LoggerMessageDelegate? action = CommonLogger.WarnMessageAction;
        //    if(action == null)
        //    {
        //        //デリゲートが無効の場合は何もしない
        //    }
        //    else
        //    {
        //        //デリゲートが有効の場合はログ出力
        //        System.Diagnostics.StackFrame frame = new System.Diagnostics.StackFrame(1);
        //        string logText = CommonLogger.CreateLogText(frame, message);
        //        action.Invoke(logText);
        //    }
        //
        //    //終了
        //    return message;
        //}
        //
        ///// <summary>
        ///// 警告例外ログ
        ///// </summary>
        ///// <param name="message">メッセージ</param>
        ///// <param name="ex">例外</param>
        ///// <returns>メッセージがそのまま返されます</returns>
        //public static string Warn(string message, Exception ex)
        //{
        //    LoggerExceptionDelegate? action = CommonLogger.WarnExceptionAction;
        //    if (action == null)
        //    {
        //        //デリゲートが無効の場合は何もしない
        //    }
        //    else
        //    {
        //        //デリゲートが有効の場合はログ出力
        //        System.Diagnostics.StackFrame frame = new System.Diagnostics.StackFrame(1);
        //        string logText = CommonLogger.CreateLogText(frame, message);
        //        action.Invoke(logText, ex);
        //    }
        //
        //    //終了
        //    return message;
        //}
        //
        ///// <summary>
        ///// エラーメッセージログデリゲート
        ///// </summary>
        //public static LoggerMessageDelegate? ErrorMessageAction = null;
        //
        ///// <summary>
        ///// エラー例外ログデリゲート
        ///// </summary>
        //public static LoggerExceptionDelegate? ErrorExceptionAction = null;
        //
        ///// <summary>
        ///// エラーメッセージログ
        ///// </summary>
        ///// <param name="message">メッセージ</param>
        ///// <returns>メッセージがそのまま返されます</returns>
        //public static string Error(string message)
        //{
        //    LoggerMessageDelegate? action = CommonLogger.ErrorMessageAction;
        //    if (action == null)
        //    {
        //        //デリゲートが無効の場合は何もしない
        //    }
        //    else
        //    {
        //        //デリゲートが有効の場合はログ出力
        //        System.Diagnostics.StackFrame frame = new System.Diagnostics.StackFrame(1);
        //        string logText = CommonLogger.CreateLogText(frame, message);
        //        action.Invoke(logText);
        //    }
        //
        //    //終了
        //    return message;
        //}
        //
        ///// <summary>
        ///// エラー例外ログ
        ///// </summary>
        ///// <param name="message">メッセージ</param>
        ///// <param name="ex">例外</param>
        ///// <returns>メッセージがそのまま返されます</returns>
        //public static string Error(string message, Exception ex)
        //{
        //    LoggerExceptionDelegate? action = CommonLogger.ErrorExceptionAction;
        //    if (action == null)
        //    {
        //        //デリゲートが無効の場合は何もしない
        //    }
        //    else
        //    {
        //        //デリゲートが有効の場合はログ出力
        //        System.Diagnostics.StackFrame frame = new System.Diagnostics.StackFrame(1);
        //        string logText = CommonLogger.CreateLogText(frame, message);
        //        action.Invoke(logText, ex);
        //    }
        //
        //    //終了
        //    return message;
        //}
        //----------
        //旧コードをコメント化
        //2026.06.02:CS0杉原:LOGGERを変更 <<<<< ここまで

        /// <summary>
        /// ログディレクトリ
        /// </summary>
        public static string? LogDir { get; private set; } = null;

        /// <summary>
        /// ログディレクトリを設定
        /// </summary>
        /// <param name="logfilepath">ログファイルのパスを指定する</param>
        public static void SetLogDir(string? logfilepath)
        {
            //パス有効判定
            {
                bool isNull = string.IsNullOrEmpty(logfilepath);
                if (isNull)
                {
                    //無効の場合は何もしないで終了
                    return;
                }
                else
                {
                    //有効の場合は処理続行
                }
            }

            //ディレクトリを取得
            string? dir = System.IO.Path.GetDirectoryName(logfilepath);

            {
                bool isNull = string.IsNullOrEmpty(dir);
                if (isNull)
                {
                    //無効の場合は何もしないで終了
                    return;
                }
                else
                {
                    //有効の場合は処理続行
                }
            }

            //ディレクトリを保持する
            MicroSignLogger.LogDir = dir;
        }


        #region 定数定義
        public static class Consts
        {
            /// <summary>
            /// 関数名
            /// </summary>
            public static class FuncName
            {
                /// <summary>
                /// デバッグ出力関数名
                /// </summary>
                public static readonly string Debug = "Debug";

                /// <summary>
                /// 情報出力関数名
                /// </summary>
                public static readonly string Info = "Info";

                /// <summary>
                /// 警告出力関数名
                /// </summary>
                public static readonly string Warn = "Warn";

                /// <summary>
                /// エラー出力関数名
                /// </summary>
                public static readonly string Error = "Error";

                /// <summary>
                /// 致命的エラー出力関数名
                /// </summary>
                public static readonly string Fatal = "Fatal";
            }

            /// <summary>
            /// 引数リスト
            /// </summary>
            public static class ArgsList
            {
                /// <summary>
                /// メッセージ引数リスト
                /// </summary>
                public static readonly Type[] MessageList = { typeof(string) };

                /// <summary>
                /// 例外付メッセージ引数リスト
                /// </summary>
                public static readonly Type[] ExceptionList = { typeof(string), typeof(Exception) };
            }
        }
        #endregion //定数定期


        #region デリゲート定義
        /// <summary>
        /// メッセージだけのロガーデリゲート
        /// </summary>
        /// <param name="message"></param>
        public delegate void LoggerMessageDelegate(string message);

        /// <summary>
        /// 例外付きのロガーデリゲート
        /// </summary>
        /// <param name="message"></param>
        public delegate void LoggerExceptionDelegate(string message, Exception ex);
        #endregion  //デリゲート定義


        #region 何も出力しないロガー関数定義
        /// <summary>
        /// 何も出力しないロガー
        /// </summary>
        /// <param name="message"></param>
        protected static void NullLogger(string message)
        {
            //どこにも出力しない
        }

        /// <summary>
        /// 例外付きの何も出力しないロガー
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        protected static void NullLoggerEx(string message, Exception ex)
        {
            //どこにも出力しない
        }
        #endregion //何も出力しないロガー関数定義


        #region プロパティ定義
        /// <summary>
        /// デバッグ出力
        /// </summary>
        public LoggerMessageDelegate Debug { get; protected set; } = MicroSignLogger.NullLogger;


        /// <summary>
        /// 例外付デバッグ出力
        /// </summary>
        public LoggerExceptionDelegate DebugEx { get; protected set; } = MicroSignLogger.NullLoggerEx;

        /// <summary>
        /// 情報出力
        /// </summary>
        public LoggerMessageDelegate Info { get; protected set; } = MicroSignLogger.NullLogger;

        /// <summary>
        /// 例外付情報出力
        /// </summary>
        public LoggerExceptionDelegate InfoEx { get; protected set; } = MicroSignLogger.NullLoggerEx;

        /// <summary>
        /// 警報出力
        /// </summary>
        public LoggerMessageDelegate Warn { get; protected set; } = MicroSignLogger.NullLogger;

        /// <summary>
        /// 例外付警報出力
        /// </summary>
        public LoggerExceptionDelegate WarnEx { get; protected set; } = MicroSignLogger.NullLoggerEx;

        /// <summary>
        /// エラー出力
        /// </summary>
        public LoggerMessageDelegate Error { get; protected set; } = MicroSignLogger.NullLogger;

        /// <summary>
        /// 例外付エラー出力
        /// </summary>
        public LoggerExceptionDelegate ErrorEx { get; protected set; } = MicroSignLogger.NullLoggerEx;

        /// <summary>
        /// 致命的エラー出力
        /// </summary>
        public LoggerMessageDelegate Fatal { get; protected set; } = MicroSignLogger.NullLogger;

        /// <summary>
        /// 例外付致命的エラー出力
        /// </summary>
        public LoggerExceptionDelegate FatalEx { get; protected set; } = MicroSignLogger.NullLoggerEx;
        #endregion //プロパティ定義

        #region 設定関数定義
        /// <summary>
        /// デバッグ出力設定
        /// </summary>
        /// <param name="func"></param>
        public void SetDebugFunc(LoggerMessageDelegate func)
        {
            if (func == null)
            {
                //無効の場合は何も出力しない関数を設定
                this.Debug = MicroSignLogger.NullLogger;
            }
            else
            {
                //有効の場合はそのまま設定
                this.Debug = func;
            }
        }

        /// <summary>
        /// 例外付デバッグ出力設定
        /// </summary>
        /// <param name="func"></param>
        public void SetDebugExFunc(LoggerExceptionDelegate func)
        {
            if (func == null)
            {
                //無効の場合は何も出力しない関数を設定
                this.DebugEx = MicroSignLogger.NullLoggerEx;
            }
            else
            {
                //有効の場合はそのまま設定
                this.DebugEx = func;
            }
        }

        /// <summary>
        /// 情報出力設定
        /// </summary>
        /// <param name="func"></param>
        public void SetInfoFunc(LoggerMessageDelegate func)
        {
            if (func == null)
            {
                //無効の場合は何も出力しない関数を設定
                this.Info = MicroSignLogger.NullLogger;
            }
            else
            {
                //有効の場合はそのまま設定
                this.Info = func;
            }
        }

        /// <summary>
        /// 例外付情報出力設定
        /// </summary>
        /// <param name="func"></param>
        public void SetInfoExFunc(LoggerExceptionDelegate func)
        {
            if (func == null)
            {
                //無効の場合は何も出力しない関数を設定
                this.InfoEx = MicroSignLogger.NullLoggerEx;
            }
            else
            {
                //有効の場合はそのまま設定
                this.InfoEx = func;
            }
        }

        /// <summary>
        /// 警報出力設定
        /// </summary>
        /// <param name="func"></param>
        public void SetWarnFunc(LoggerMessageDelegate func)
        {
            if (func == null)
            {
                //無効の場合は何も出力しない関数を設定
                this.Warn = MicroSignLogger.NullLogger;
            }
            else
            {
                //有効の場合はそのまま設定
                this.Warn = func;
            }
        }

        /// <summary>
        /// 例外付警報出力設定
        /// </summary>
        /// <param name="func"></param>
        public void SetWarnExFunc(LoggerExceptionDelegate func)
        {
            if (func == null)
            {
                //無効の場合は何も出力しない関数を設定
                this.WarnEx = MicroSignLogger.NullLoggerEx;
            }
            else
            {
                //有効の場合はそのまま設定
                this.WarnEx = func;
            }
        }

        /// <summary>
        /// エラー出力設定
        /// </summary>
        /// <param name="func"></param>
        public void SetErrorFunc(LoggerMessageDelegate func)
        {
            if (func == null)
            {
                //無効の場合は何も出力しない関数を設定
                this.Error = MicroSignLogger.NullLogger;
            }
            else
            {
                //有効の場合はそのまま設定
                this.Error = func;
            }
        }

        /// <summary>
        /// 例外付エラー出力設定
        /// </summary>
        /// <param name="func"></param>
        public void SetErrorExFunc(LoggerExceptionDelegate func)
        {
            if (func == null)
            {
                //無効の場合は何も出力しない関数を設定
                this.ErrorEx = MicroSignLogger.NullLoggerEx;
            }
            else
            {
                //有効の場合はそのまま設定
                this.ErrorEx = func;
            }
        }

        /// <summary>
        /// 致命的エラー出力設定
        /// </summary>
        /// <param name="func"></param>
        public void SetFatalFunc(LoggerMessageDelegate func)
        {
            if (func == null)
            {
                //無効の場合は何も出力しない関数を設定
                this.Fatal = MicroSignLogger.NullLogger;
            }
            else
            {
                //有効の場合はそのまま設定
                this.Fatal = func;
            }
        }

        /// <summary>
        /// 例外付致命的エラー出力設定
        /// </summary>
        /// <param name="func"></param>
        public void SetFatalExFunc(LoggerExceptionDelegate func)
        {
            if (func == null)
            {
                //無効の場合は何も出力しない関数を設定
                this.FatalEx = MicroSignLogger.NullLoggerEx;
            }
            else
            {
                //有効の場合はそのまま設定
                this.FatalEx = func;
            }
        }

        #endregion //設定関数定義

        #region 静的関数定義
        /// <summary>
        /// ロガーを取得するデリゲート定義
        /// </summary>
        public delegate object GetLoggerDelegate(System.Type type);

        /// <summary>
        /// ロガーを取得するデリゲート
        /// </summary>
        private static GetLoggerDelegate? _GetLoggerFunc = null;

        /// <summary>
        /// ロガーを取得するデリゲート登録
        /// </summary>
        /// <param name="func"></param>
        /// <remarks>
        /// App.xaml.cs等のプログラム開始時に
        /// Various.VariousLogger.RegistGetLoggerFunction(log4net.LogManager.GetLogger);
        /// の処理を追加してください
        /// </remarks>
        public static void RegistGetLoggerFunction(GetLoggerDelegate func)
        {
            MicroSignLogger._GetLoggerFunc = func;
        }

        /// <summary>
        /// ロガー取得
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static MicroSignLogger GetLogger(System.Type type)
        {
            //戻り値生成
            MicroSignLogger result = new MicroSignLogger();

            //GetLoggerデリゲート取得
            GetLoggerDelegate? func = MicroSignLogger._GetLoggerFunc;
            if (func == null)
            {
                //無効の場合は何もしないで終了
                // >> 何も出力しないNET10Loggerが返される
                return result;
            }
            else
            {
                //有効の場合は処理続行
            }

            //実際にログを出力するLoggerインスタンス取得
            object? logger = null;
            try
            {
                logger = func(type);
                if (logger == null)
                {
                    //取得できなかった場合は何もしないで終了
                    // >> 何も出力しないNET10Loggerが返される
                    return result;
                }
                else
                {
                    //取得できた場合は処理続行
                }
            }
            catch (Exception)
            {
                //例外は握りつぶして終了
                // >> 何も出力しないNET10Loggerが返される
                return result;
            }

            //Loggerインスタンスの型を取得する
            // >> Typeがnullになることはないのでnullチェック不要
            Type t = logger.GetType();

            //デバッグ出力設定
            {
                try
                {
                    //stringを1個受け取るDebug関数を取得
                    MethodInfo? m = t.GetMethod(MicroSignLogger.Consts.FuncName.Debug, MicroSignLogger.Consts.ArgsList.MessageList);
                    if (m == null)
                    {
                        //取得できなかった場合は何もしない
                    }
                    else
                    {
                        //取得できた場合はデリゲートに変換して設定
                        LoggerMessageDelegate d = (LoggerMessageDelegate)Delegate.CreateDelegate(typeof(LoggerMessageDelegate), logger, m);
                        result.SetDebugFunc(d);
                    }
                }
                catch (Exception)
                {
                    //例外は握りつぶして処理続行(=他のログ出力を行う)
                }
            }

            //例外付デバッグ出力設定
            {
                try
                {
                    //stringを1個受け取るDebug関数を取得
                    MethodInfo? m = t.GetMethod(MicroSignLogger.Consts.FuncName.Debug, MicroSignLogger.Consts.ArgsList.ExceptionList);
                    if (m == null)
                    {
                        //取得できなかった場合は何もしない
                    }
                    else
                    {
                        //取得できた場合はデリゲートに変換して設定
                        LoggerExceptionDelegate d = (LoggerExceptionDelegate)Delegate.CreateDelegate(typeof(LoggerExceptionDelegate), logger, m);
                        result.SetDebugExFunc(d);
                    }
                }
                catch (Exception)
                {
                    //例外は握りつぶして処理続行(=他のログ出力を行う)
                }
            }

            //情報出力設定
            {
                try
                {
                    //stringを1個受け取るDebug関数を取得
                    MethodInfo? m = t.GetMethod(MicroSignLogger.Consts.FuncName.Info, MicroSignLogger.Consts.ArgsList.MessageList);
                    if (m == null)
                    {
                        //取得できなかった場合は何もしない
                    }
                    else
                    {
                        //取得できた場合はデリゲートに変換して設定
                        LoggerMessageDelegate d = (LoggerMessageDelegate)Delegate.CreateDelegate(typeof(LoggerMessageDelegate), logger, m);
                        result.SetInfoFunc(d);
                    }
                }
                catch (Exception)
                {
                    //例外は握りつぶして処理続行(=他のログ出力を行う)
                }
            }

            //例外付情報出力設定
            {
                try
                {
                    //stringを1個受け取るDebug関数を取得
                    MethodInfo? m = t.GetMethod(MicroSignLogger.Consts.FuncName.Info, MicroSignLogger.Consts.ArgsList.ExceptionList);
                    if (m == null)
                    {
                        //取得できなかった場合は何もしない
                    }
                    else
                    {
                        //取得できた場合はデリゲートに変換して設定
                        LoggerExceptionDelegate d = (LoggerExceptionDelegate)Delegate.CreateDelegate(typeof(LoggerExceptionDelegate), logger, m);
                        result.SetInfoExFunc(d);
                    }
                }
                catch (Exception)
                {
                    //例外は握りつぶして処理続行(=他のログ出力を行う)
                }
            }

            //警報出力設定
            {
                try
                {
                    //stringを1個受け取るDebug関数を取得
                    MethodInfo? m = t.GetMethod(MicroSignLogger.Consts.FuncName.Warn, MicroSignLogger.Consts.ArgsList.MessageList);
                    if (m == null)
                    {
                        //取得できなかった場合は何もしない
                    }
                    else
                    {
                        //取得できた場合はデリゲートに変換して設定
                        LoggerMessageDelegate d = (LoggerMessageDelegate)Delegate.CreateDelegate(typeof(LoggerMessageDelegate), logger, m);
                        result.SetWarnFunc(d);
                    }
                }
                catch (Exception)
                {
                    //例外は握りつぶして処理続行(=他のログ出力を行う)
                }
            }

            //例外付警報出力設定
            {
                try
                {
                    //stringを1個受け取るDebug関数を取得
                    MethodInfo? m = t.GetMethod(MicroSignLogger.Consts.FuncName.Warn, MicroSignLogger.Consts.ArgsList.ExceptionList);
                    if (m == null)
                    {
                        //取得できなかった場合は何もしない
                    }
                    else
                    {
                        //取得できた場合はデリゲートに変換して設定
                        LoggerExceptionDelegate d = (LoggerExceptionDelegate)Delegate.CreateDelegate(typeof(LoggerExceptionDelegate), logger, m);
                        result.SetWarnExFunc(d);
                    }
                }
                catch (Exception)
                {
                    //例外は握りつぶして処理続行(=他のログ出力を行う)
                }
            }

            //エラー出力設定
            {
                try
                {
                    //stringを1個受け取るDebug関数を取得
                    MethodInfo? m = t.GetMethod(MicroSignLogger.Consts.FuncName.Error, MicroSignLogger.Consts.ArgsList.MessageList);
                    if (m == null)
                    {
                        //取得できなかった場合は何もしない
                    }
                    else
                    {
                        //取得できた場合はデリゲートに変換して設定
                        LoggerMessageDelegate d = (LoggerMessageDelegate)Delegate.CreateDelegate(typeof(LoggerMessageDelegate), logger, m);
                        result.SetErrorFunc(d);
                    }
                }
                catch (Exception)
                {
                    //例外は握りつぶして処理続行(=他のログ出力を行う)
                }
            }

            //例外付エラー出力設定
            {
                try
                {
                    //stringを1個受け取るDebug関数を取得
                    MethodInfo? m = t.GetMethod(MicroSignLogger.Consts.FuncName.Error, MicroSignLogger.Consts.ArgsList.ExceptionList);
                    if (m == null)
                    {
                        //取得できなかった場合は何もしない
                    }
                    else
                    {
                        //取得できた場合はデリゲートに変換して設定
                        LoggerExceptionDelegate d = (LoggerExceptionDelegate)Delegate.CreateDelegate(typeof(LoggerExceptionDelegate), logger, m);
                        result.SetErrorExFunc(d);
                    }
                }
                catch (Exception)
                {
                    //例外は握りつぶして処理続行(=他のログ出力を行う)
                }
            }

            //致命的エラー出力設定
            {
                try
                {
                    //stringを1個受け取るDebug関数を取得
                    MethodInfo? m = t.GetMethod(MicroSignLogger.Consts.FuncName.Fatal, MicroSignLogger.Consts.ArgsList.MessageList);
                    if (m == null)
                    {
                        //取得できなかった場合は何もしない
                    }
                    else
                    {
                        //取得できた場合はデリゲートに変換して設定
                        LoggerMessageDelegate d = (LoggerMessageDelegate)Delegate.CreateDelegate(typeof(LoggerMessageDelegate), logger, m);
                        result.SetFatalFunc(d);
                    }
                }
                catch (Exception)
                {
                    //例外は握りつぶして処理続行(=他のログ出力を行う)
                }
            }

            //例外付致命的エラー出力設定
            {
                try
                {
                    //stringを1個受け取るDebug関数を取得
                    MethodInfo? m = t.GetMethod(MicroSignLogger.Consts.FuncName.Fatal, MicroSignLogger.Consts.ArgsList.ExceptionList);
                    if (m == null)
                    {
                        //取得できなかった場合は何もしない
                    }
                    else
                    {
                        //取得できた場合はデリゲートに変換して設定
                        LoggerExceptionDelegate d = (LoggerExceptionDelegate)Delegate.CreateDelegate(typeof(LoggerExceptionDelegate), logger, m);
                        result.SetFatalExFunc(d);
                    }
                }
                catch (Exception)
                {
                    //例外は握りつぶして処理続行(=他のログ出力を行う)
                }
            }

            //終了
            return result;

        }

        #endregion //静的関数定義



    }
}
