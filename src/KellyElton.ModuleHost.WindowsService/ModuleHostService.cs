using KellyElton.Logging;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;

namespace KellyElton.ModuleHost.WindowsService
{
    public partial class ModuleHostService : ServiceBase
    {
        private ILog Log => LogComponent;

        public ModuleHostService() {
            InitializeComponent();
            ConfigureLogger();
            foreach(var component in this.components.Components.OfType<ILoadable>() ) {
                component.Load();
            }
        }

        private void ConfigureLogger() {
            LogComponent.Source = "ModuleHostService";
            ((EventLog)LogComponent).Log = "KellyElton Module Host";
        }

        protected override void OnStart( string[] args ) {
            Log.Trace();
        }

        protected override void OnStop() {
            Log.Trace();
        }
    }
}
