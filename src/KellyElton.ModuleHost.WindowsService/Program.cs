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
            ServiceBase[] ServicesToRun = new ServiceBase[] {
                new ModuleHostService()
            };

            Thread appThread = new Thread( ApplicationThread );
            appThread.SetApartmentState( ApartmentState.STA );
            appThread.Start();

            if( Debugger.IsAttached ) {
                foreach(IStartable sb in ServicesToRun ) {
                    sb.Start();
                }
                while( !EndResetEvent.WaitOne( 10 ) ) {
                    if( Console.KeyAvailable ) break;
                };
                foreach(IStartable sb in ServicesToRun ) {
                    sb.Stop();
                }
                Application.ExitThread();
            } else {
                ServiceBase.Run( ServicesToRun );
            }
            appThread.Join();
        }

        private static volatile bool KeepRunning;
        private static ManualResetEvent EndResetEvent = new ManualResetEvent(false);

        static void ApplicationThread() {
            try {
                while( KeepRunning ) {
                    Application.DoEvents();
                }
            } finally {
                EndResetEvent.Set();
            }
        }
    }
}
