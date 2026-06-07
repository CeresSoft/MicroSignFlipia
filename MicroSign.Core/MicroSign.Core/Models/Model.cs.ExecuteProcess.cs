using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSign.Core.Models
{
    partial class Model
    {
        /// <summary>
        /// 外部プロセスを実行結果
        /// </summary>
        public struct ExecuteProcessResult
        {
            /// <summary>
            /// 成功フラグ
            /// </summary>
            public readonly bool IsSuccess;

            /// <summary>
            /// メッセージ
            /// </summary>
            public readonly string? Message;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="isSuccess">成功フラグ</param>
            /// <param name="message">メッセージ</param>
            private ExecuteProcessResult(bool isSuccess, string? message)
            {
                this.IsSuccess = isSuccess;
                this.Message = message;
            }

            /// <summary>
            /// 失敗
            /// </summary>
            /// <param name="message">メッセージ</param>
            /// <returns></returns>
            public static ExecuteProcessResult Failed(string message)
            {
                ExecuteProcessResult result = new ExecuteProcessResult(false, message);
                return result;
            }

            /// <summary>
            /// 成功
            /// </summary>
            /// <returns></returns>
            public static ExecuteProcessResult Success()
            {
                ExecuteProcessResult result = new ExecuteProcessResult(true, null);
                return result;
            }
        }

        /// <summary>
        /// 外部プロセスを実行
        /// </summary>
        /// <param name="name">処理名</param>
        /// <param name="path">実行パス</param>
        /// <param name="args">引数</param>
        /// <param name="timeout">タイムアウト値</param>
        /// <returns></returns>
        private ExecuteProcessResult ExecuteProcess(string name, string path, string args, TimeSpan timeout)
        {
            try
            {
                ProcessStartInfo psInfo = new ProcessStartInfo();
                psInfo.FileName = path;
                psInfo.Arguments = args;
                psInfo.CreateNoWindow = true;
                psInfo.RedirectStandardOutput = true;
                psInfo.RedirectStandardError = true;

                LOGGER.Info($"{name} - 開始 (file='{path}' arg='{args}')");
                using (Process? p = Process.Start(psInfo))
                {
                    if (p == null)
                    {
                        //プロセスの起動に失敗した場合
                        string msg = $"{name} プロセスの起動に失敗しました";
                        LOGGER.Warn(msg);
                        return ExecuteProcessResult.Failed(msg);
                    }
                    else
                    {
                        try
                        {
                            //プロセスの起動に成功した場合終わるまで待つ
                            // >> 標準出力都標準エラーをマージして取得する
                            StringBuilder sb = new StringBuilder(MicroSign.Core.CommonConsts.Text.STRING_BUILDER_CAPACITY);
                            object lockObj = new object();
                            p.ErrorDataReceived += (s, e) => { lock (lockObj) { sb.AppendLine(e.Data); } };
                            p.OutputDataReceived += (s, e) => { lock (lockObj) { sb.AppendLine(e.Data); } };
                            p.BeginOutputReadLine();
                            p.BeginErrorReadLine();

                            // >> プロセス終了待ち
                            int waitTime = (int)timeout.TotalMilliseconds;
                            LOGGER.Info($"{name} - プロセス終了待ち開始 ({waitTime}ms) (file='{path}' arg='{args}')");
                            p.WaitForExit(waitTime);
                            LOGGER.Info($"{name} - プロセス終了待ち完了 ({waitTime}ms) (file='{path}' arg='{args}')");

                            // >>  標準出力の読み取り(エラーでも読込する)
                            string outputText = string.Empty;
                            lock (lockObj)
                            {
                                outputText = sb.ToString();
                            }
                            //ログに出力
                            LOGGER.Info($"{name} >>>{outputText}<<<");

                            // >> 終了したか判定
                            if (p.HasExited)
                            {
                                //終了したので処理続行
                                LOGGER.Debug($"{name} - プロセス終了しました (file='{path}' arg='{args}')");
                            }
                            else
                            {
                                //終了しなかったのでエラーで終了
                                string msg = $"{name} - 時間内に終了しませんでした (file='{path}' arg='{args}')";
                                LOGGER.Warn(msg);
                                return ExecuteProcessResult.Failed(msg);
                            }

                            //終了コード判定
                            {
                                int n = p.ExitCode;
                                if (n == MicroSignConsts.ExitCodes.Success)
                                {
                                    //成功の場合処理続行
                                    LOGGER.Debug($"{name} - プロセス終了コード正常 Exit={n} (file='{path}' arg='{args}')");
                                }
                                else
                                {
                                    //それ以外はエラーとみなす
                                    string msg = $"{name} - プロセス終了コード異常 Exit={n} (file='{path}' arg='{args}')";
                                    LOGGER.Warn(msg);
                                    return ExecuteProcessResult.Failed(msg);

                                }
                            }

                            //ここまで来たら成功
                            // >> メッセージに出力された内容を出力する
                            {
                                string msg = $"{name} - 成功 (file='{path}' arg='{args}')";
                                LOGGER.Info(msg);
                                return ExecuteProcessResult.Success();
                            }
                        }
                        finally
                        {
                            p.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //例外が発生したら終了
                string msg = $"{name}で例外発生";
                LOGGER.WarnEx(msg, ex);
                return ExecuteProcessResult.Failed(msg);
            }
        }


    }
}
