using System.Windows;

//LOG4NET
[assembly: log4net.Config.XmlConfigurator(ConfigFile = @".\Prms\log4net.xml", Watch = true)]

namespace MicroSign
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
            base.OnStartup(e);

            //MicroSign共通ログ設定
            {
                log4net.ILog logger = log4net.LogManager.GetLogger(typeof(MicroSign.Core.CommonLogger));
                MicroSign.Core.CommonLogger.DebugMessageAction = logger.Debug;
                MicroSign.Core.CommonLogger.DebugExceptionAction = logger.Debug;
                MicroSign.Core.CommonLogger.InfoMessageAction = logger.Info;
                MicroSign.Core.CommonLogger.InfoExceptionAction = logger.Info;
                MicroSign.Core.CommonLogger.WarnMessageAction = logger.Warn;
                MicroSign.Core.CommonLogger.WarnExceptionAction = logger.Warn;
                MicroSign.Core.CommonLogger.ErrorMessageAction = logger.Error;
                MicroSign.Core.CommonLogger.ErrorExceptionAction = logger.Error;
            }
        }
    }
}
