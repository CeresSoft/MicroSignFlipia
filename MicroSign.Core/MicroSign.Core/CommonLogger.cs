using System;

namespace MicroSign.Core
{
    /// <summary>
    /// 共通ロガー
    /// </summary>
    public static class CommonLogger
    {
        /// <summary>
        /// メッセージだけのロガーデリゲート
        /// </summary>
        /// <param name="message">メッセージ(log4netの定義がobjectになっている)</param>
        public delegate void LoggerMessageDelegate(object message);

        /// <summary>
        /// 例外付きのロガーデリゲート
        /// </summary>
        /// <param name="message">メッセージ(log4netの定義がobjectになっている)</param>
        /// <param name="ex">例外</param>
        public delegate void LoggerExceptionDelegate(object message, Exception ex);

        /// <summary>
        /// 呼び出し元関数情報取得
        /// </summary>
        /// <param name="frame">スタックフレーム</param>
        /// <param name="message">メッセージ</param>
        /// <returns>呼び出し元関数情報の文字列</returns>
        private static string CreateLogText(System.Diagnostics.StackFrame frame, object message)
        {
            string typeName = "(null)";
            string methodName = "(null)";

            //関数取得
            System.Reflection.MethodBase? method = frame.GetMethod();
            if(method == null)
            {
                //メソッドが無効の場合は何もしない
            }
            else
            {
                //メソッドが有効の場合
                Type? type = method.DeclaringType;
                if (type == null)
                {
                    //型が無効の場合は型名は取得しない
                }
                else
                {
                    //型が有効の場合は名前を取得する
                    typeName = type.Name;
                }

                //関数名を取得
                methodName = method.Name;
            }

            //終了
            return $"<{typeName}.{methodName}> - {message}";
        }

        /// <summary>
        /// デバッグメッセージログデリゲート
        /// </summary>
        public static LoggerMessageDelegate? DebugMessageAction = null;

        /// <summary>
        /// デバッグ例外ログデリゲート
        /// </summary>
        public static LoggerExceptionDelegate? DebugExceptionAction = null;

        /// <summary>
        /// デバッグメッセージログ
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <returns>メッセージがそのまま返されます</returns>
        public static string Debug(string message)
        {
            LoggerMessageDelegate? action = CommonLogger.DebugMessageAction;
            if(action == null)
            {
                //デリゲートが無効の場合は何もしない
            }
            else
            {
                //デリゲートが有効の場合はログ出力
                System.Diagnostics.StackFrame frame = new System.Diagnostics.StackFrame(1);
                string logText = CommonLogger.CreateLogText(frame, message);
                action.Invoke(logText);
            }

            //終了
            return message;
        }

        /// <summary>
        /// デバッグ例外ログ
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="ex">例外</param>
        /// <returns>メッセージがそのまま返されます</returns>
        public static string Debug(string message, Exception ex)
        {
            LoggerExceptionDelegate? action = CommonLogger.DebugExceptionAction;
            if(action == null)
            {
                //デリゲートが無効の場合は何もしない
            }
            else
            {
                //デリゲートが有効の場合はログ出力
                System.Diagnostics.StackFrame frame = new System.Diagnostics.StackFrame(1);
                string logText = CommonLogger.CreateLogText(frame, message);
                action.Invoke(logText, ex);
            }

            //終了
            return message;
        }

        /// <summary>
        /// 情報メッセージログデリゲート
        /// </summary>
        public static LoggerMessageDelegate? InfoMessageAction = null;

        /// <summary>
        /// 情報例外ログデリゲート
        /// </summary>
        public static LoggerExceptionDelegate? InfoExceptionAction = null;

        /// <summary>
        /// 情報メッセージログ
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <returns>メッセージがそのまま返されます</returns>
        public static string Info(string message)
        {
            LoggerMessageDelegate? action = CommonLogger.InfoMessageAction;
            if(action == null)
            {
                //デリゲートが無効の場合は何もしない
            }
            else
            {
                //デリゲートが有効の場合はログ出力
                System.Diagnostics.StackFrame frame = new System.Diagnostics.StackFrame(1);
                string logText = CommonLogger.CreateLogText(frame, message);
                action.Invoke(logText);
            }

            //終了
            return message;
        }

        /// <summary>
        /// 情報例外ログ
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="ex">例外</param>
        /// <returns>メッセージがそのまま返されます</returns>
        public static string Info(string message, Exception ex)
        {
            LoggerExceptionDelegate? action = CommonLogger.InfoExceptionAction;
            if (action == null)
            {
                //デリゲートが無効の場合は何もしない
            }
            else
            {
                //デリゲートが有効の場合はログ出力
                System.Diagnostics.StackFrame frame = new System.Diagnostics.StackFrame(1);
                string logText = CommonLogger.CreateLogText(frame, message);
                action.Invoke(logText, ex);
            }

            //終了
            return message;
        }

        /// <summary>
        /// 警告メッセージログデリゲート
        /// </summary>
        public static LoggerMessageDelegate? WarnMessageAction = null;

        /// <summary>
        /// 警告例外ログデリゲート
        /// </summary>
        public static LoggerExceptionDelegate? WarnExceptionAction = null;

        /// <summary>
        /// 警告メッセージログ
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <returns>メッセージがそのまま返されます</returns>
        public static string Warn(string message)
        {
            LoggerMessageDelegate? action = CommonLogger.WarnMessageAction;
            if(action == null)
            {
                //デリゲートが無効の場合は何もしない
            }
            else
            {
                //デリゲートが有効の場合はログ出力
                System.Diagnostics.StackFrame frame = new System.Diagnostics.StackFrame(1);
                string logText = CommonLogger.CreateLogText(frame, message);
                action.Invoke(logText);
            }

            //終了
            return message;
        }

        /// <summary>
        /// 警告例外ログ
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="ex">例外</param>
        /// <returns>メッセージがそのまま返されます</returns>
        public static string Warn(string message, Exception ex)
        {
            LoggerExceptionDelegate? action = CommonLogger.WarnExceptionAction;
            if (action == null)
            {
                //デリゲートが無効の場合は何もしない
            }
            else
            {
                //デリゲートが有効の場合はログ出力
                System.Diagnostics.StackFrame frame = new System.Diagnostics.StackFrame(1);
                string logText = CommonLogger.CreateLogText(frame, message);
                action.Invoke(logText, ex);
            }

            //終了
            return message;
        }

        /// <summary>
        /// エラーメッセージログデリゲート
        /// </summary>
        public static LoggerMessageDelegate? ErrorMessageAction = null;

        /// <summary>
        /// エラー例外ログデリゲート
        /// </summary>
        public static LoggerExceptionDelegate? ErrorExceptionAction = null;

        /// <summary>
        /// エラーメッセージログ
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <returns>メッセージがそのまま返されます</returns>
        public static string Error(string message)
        {
            LoggerMessageDelegate? action = CommonLogger.ErrorMessageAction;
            if (action == null)
            {
                //デリゲートが無効の場合は何もしない
            }
            else
            {
                //デリゲートが有効の場合はログ出力
                System.Diagnostics.StackFrame frame = new System.Diagnostics.StackFrame(1);
                string logText = CommonLogger.CreateLogText(frame, message);
                action.Invoke(logText);
            }

            //終了
            return message;
        }

        /// <summary>
        /// エラー例外ログ
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="ex">例外</param>
        /// <returns>メッセージがそのまま返されます</returns>
        public static string Error(string message, Exception ex)
        {
            LoggerExceptionDelegate? action = CommonLogger.ErrorExceptionAction;
            if (action == null)
            {
                //デリゲートが無効の場合は何もしない
            }
            else
            {
                //デリゲートが有効の場合はログ出力
                System.Diagnostics.StackFrame frame = new System.Diagnostics.StackFrame(1);
                string logText = CommonLogger.CreateLogText(frame, message);
                action.Invoke(logText, ex);
            }

            //終了
            return message;
        }

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
                    CommonLogger.Debug($"LOG PATH 無効");
                    return;
                }
                else
                {
                    //有効の場合は処理続行
                    CommonLogger.Debug($"LOG PATH 有効 ('{logfilepath}')");
                }
            }

            //ディレクトリを取得
            string? dir = System.IO.Path.GetDirectoryName(logfilepath);

            {
                bool isNull = string.IsNullOrEmpty(dir);
                if (isNull)
                {
                    //無効の場合は何もしないで終了
                    CommonLogger.Debug($"LOG DIR 無効 ('{logfilepath}' -> null)");
                    return;
                }
                else
                {
                    //有効の場合は処理続行
                    CommonLogger.Debug($"LOG PATH 有効 ('{logfilepath}' -> '{dir}')");
                }
            }

            //ディレクトリを保持する
            CommonLogger.LogDir = dir;
        }
    }
}
