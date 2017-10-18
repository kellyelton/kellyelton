using System;
using System.Linq;
using System.ServiceProcess;

namespace KellyElton.ModuleHost.WindowsService
{
    public partial class ModuleHostService : ServiceBase, IStartable
    {
        private ILog Log => LogComponent;

        public ModuleHostService() {
            try {
                InitializeComponent();
                foreach( var component in this.components.Components.OfType<ILoadable>() ) {
                    component.Load();
                }
            } catch {
                Dispose();
                throw;
            }
        }

        protected override void OnStart( string[] args ) {
            try {
                Log.Event( nameof( OnStart ) );
                DownloadFolderClearer.Start();
            } catch (Exception ex ) {
                Log.Error( ex );
                Signal.Exception(ex, Severity.Critical);
                Dispose();
                throw;
            }
        }

        protected override void OnStop() {
            try {
                Log.Event( nameof( OnStop ) );
                DownloadFolderClearer.Stop();
            } catch (Exception ex ) {
                Log.Error( ex );
                Signal.Exception(ex, Severity.Critical);
                Dispose();
                throw;
            }
        }

        public void Start() => OnStart( null );
    }

    public interface IStartable
    {
        void Start();
        void Stop();
    }
}
