using Livet;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;

namespace CtrlLauncher
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            DispatcherHelper.UIDispatcher = Dispatcher;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        // 集約エラーハンドラ
        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var ts = DateTime.Now.ToString("yyyyMMddHHmmss");
                var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ex" + ts + ".log");
                using (var sw = new StreamWriter(path, false, new UTF8Encoding(false)))
                    sw.Write((e.ExceptionObject as Exception).ToString());

                MessageBox.Show(
                    "不明なエラーが発生したため、アプリケーションを終了します。ご不便をおかけして申し訳ございません。\r\n\r\n例外ログファイルが作成されました: " + path,
                    "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "不明なエラーが発生したため、アプリケーションを終了します。ご不便をおかけして申し訳ございません。\r\n\r\n例外ログファイルは作成されませんでした: " + ex.Message,
                    "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Environment.Exit(1);
            }
        }
    }
}
