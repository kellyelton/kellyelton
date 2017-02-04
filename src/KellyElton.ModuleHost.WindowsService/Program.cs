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
            ServiceBase.Run( ServicesToRun );
        }
    }
}
