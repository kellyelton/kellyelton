using KellyElton.Logging;
using System.Linq;
using System.ServiceProcess;
using System;

namespace KellyElton.ModuleHost.WindowsService
{
    public partial class ModuleHostService : ServiceBase, IStartable
    {
        private ILog Log => LogComponent;

        public ModuleHostService() {
            InitializeComponent();
            foreach( var component in this.components.Components.OfType<ILoadable>() ) {
                component.Load();
            }
        }

        protected override void OnStart( string[] args ) {
            Log.Event( nameof( OnStart ) );
        }

        protected override void OnStop() {
            Log.Event( nameof( OnStop ) );
        }

        public void Start() {
            OnStart( null );
        }
        public new void Stop() {
            base.Stop();
        }
    }

    public interface IStartable
    {
        void Start();
        void Stop();
    }
}
