using MicroSign.Core;
using System.Windows;

//LOG4NET
// >> log4net.xmlの設定でログの出力先が{USERPROFILE}\MicroSign\Log\フォルダとなっています
// >> これはアプリの配布がMicrosoft Storeのためログファイルを見つけやすい位置に保存するためです
[assembly: log4net.Config.XmlConfigurator(ConfigFile = @".\Prms\log4net.xml", Watch = true)]

namespace MicroSignFlipia
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// LOG4NETのロガー
        /// </summary>
        private static readonly log4net.ILog LOGGER = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType!);

        /// <summary>
        /// 開始処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            LOGGER.Info(">>>>>>>>>> MicroSignFlipia Start");
            base.OnStartup(e);

            //MicroSign共通ログ設定
            {
                //2026.06.07:CS)杉原:LOGGERを修正 >>>>> ここから
                //log4net.ILog logger = log4net.LogManager.GetLogger(typeof(MicroSign.Core.MicroSignLogger));
                //MicroSign.Core.MicroSignLogger.DebugMessageAction = logger.Debug;
                //MicroSign.Core.MicroSignLogger.DebugExceptionAction = logger.Debug;
                //MicroSign.Core.MicroSignLogger.InfoMessageAction = logger.Info;
                //MicroSign.Core.MicroSignLogger.InfoExceptionAction = logger.Info;
                //MicroSign.Core.MicroSignLogger.WarnMessageAction = logger.Warn;
                //MicroSign.Core.MicroSignLogger.WarnExceptionAction = logger.Warn;
                //MicroSign.Core.MicroSignLogger.ErrorMessageAction = logger.Error;
                //MicroSign.Core.MicroSignLogger.ErrorExceptionAction = logger.Error;
                //----------
                // >> log4netのGetLogger()を登録する
                MicroSignLogger.RegistGetLoggerFunction(log4net.LogManager.GetLogger);
                //2026.06.07:CS)杉原:LOGGERを修正 <<<<< ここまで

                //ログのディレクトリを取得して設定
                {
                    log4net.Appender.IAppender[]? appenders = LOGGER.Logger.Repository?.GetAppenders();
                    int n = CommonUtils.GetCount(appenders);
                    for (int i = CommonConsts.Index.First; i < n; i += CommonConsts.Index.Step)
                    {
                        log4net.Appender.IAppender appender = appenders![i];
                        if (appender is log4net.Appender.FileAppender fileAppender)
                        {
                            //FileAppenderの場合ファイル名を取得して設定
                            string? path = fileAppender.File;
                            MicroSign.Core.MicroSignLogger.SetLogDir(path);
                        }
                        else
                        {
                            //異なる場合は何もしない
                        }
                    }

                }
            }
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            LOGGER.Info("<<<<<<<<<< MicroSignFlipia End");
        }
    }
}
