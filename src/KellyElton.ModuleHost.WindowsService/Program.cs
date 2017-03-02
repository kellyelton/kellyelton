using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;

namespace KellyElton.ModuleHost.WindowsService
{
    static class Program
    {
        private static volatile bool KeepRunning = true;

        static void Main() {
            ServiceBase[] ServicesToRun = new ServiceBase[] {
                new ModuleHostService()
            };

            if( Debugger.IsAttached ) {
                foreach(IStartable sb in ServicesToRun ) {
                    sb.Start();
                }
                while( KeepRunning ) {
                    if( Console.KeyAvailable ) {
                        KeepRunning = false;
                        break;
                    }
                    Thread.Sleep( 10 );
                };
                foreach(IStartable sb in ServicesToRun ) {
                    sb.Stop();
                }
            } else {
                ServiceBase.Run( ServicesToRun );
            }
        }
    }
}
