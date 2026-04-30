using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccountAPP
{
    internal static class Program
    {
        // 全域 logger，所有地方直接用 Program.Log
        internal static readonly AccountAPP.Logging.AppLogger Log = new AccountAPP.Logging.AppLogger();

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        [STAThread]
        static void Main()
        {
            Application.ThreadException += (s, e) =>
                Log.Error("未處理的 UI 執行緒例外", e.Exception);

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                Log.Error("未處理的例外", e.ExceptionObject as Exception);

            if (System.Environment.OSVersion.Version.Major >= 6) { SetProcessDPIAware(); }

            Log.Info("app start");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Account());
            Log.Info("app exit");
        }
    }
}
