using System;
using System.Diagnostics;
using System.ServiceProcess;

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
                Console.ReadKey();
                foreach(IStartable sb in ServicesToRun ) {
                    sb.Stop();
                }
            } else {
                ServiceBase.Run( ServicesToRun );
            }
        }
    }
}
