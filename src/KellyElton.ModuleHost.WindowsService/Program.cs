using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;

namespace KellyElton.ModuleHost.WindowsService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main() {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ModuleHostService()
            };
            if( Debugger.IsAttached ) {
                foreach(IStartable sb in ServicesToRun ) {
                    sb.Start();
                }
                while( !Console.KeyAvailable ) {
                    Application.DoEvents();
                }
                foreach(IStartable sb in ServicesToRun ) {
                    sb.Stop();
                }
                Application.ExitThread();
            } else {
                ServiceBase.Run( ServicesToRun );
            }
        }
    }
}
