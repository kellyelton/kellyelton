using KellyElton.Logging;
using System.ComponentModel;
using System;
using System.Diagnostics;

namespace KellyElton.ModuleHost.WindowsService.Components
{
    public partial class EventLogLogger : EventLog, ILog, ILoadable
    {
        [Browsable(false)]
        public string Module => base.Log;

        public EventLogLogger() : base() { }
        public EventLogLogger( IContainer container ) : base() {
            InitializeComponent();
        }

        #region LoadableComponent

        public void Load() {
            CreateEventSource();
        }

        #endregion LoadableComponent

        #region ILog

        private void CreateEventSource() {
            if( LicenseManager.UsageMode == LicenseUsageMode.Designtime ) return;

            //EventLog.Delete( Module );
            //EventLog.DeleteEventSource( Module );

            if( EventLog.SourceExists( Source ) ) {
                EventLog evLog = new EventLog { Source = Source };
                if( evLog.Log != base.Log ) {
                    EventLog.DeleteEventSource( Source );
                }
            }

            if( !EventLog.SourceExists( Source ) ) {
                EventLog.CreateEventSource( Source, base.Log );
                EventLog.WriteEntry( Source, String.Format( "Event Log Created '{0}'/'{1}'", base.Log, Source ), EventLogEntryType.Information );
            }
        }

        public void Trace( string message, Exception exception = null, params string[] tags ) {
            Log( base.Log, LogMessageType.Trace, message, exception, tags );
        }

        public void Debug( string message, Exception exception = null, params string[] tags ) {
            Log( base.Log, LogMessageType.Debug, message, exception, tags );
        }

        public void Standard( string message, Exception exception = null, params string[] tags ) {
            Log( base.Log, LogMessageType.Standard, message, exception, tags );
        }

        public void Warning( string message, Exception exception = null, params string[] tags ) {
            Log( base.Log, LogMessageType.Warning, message, exception, tags );
        }

        public void Error( string message, Exception exception = null, params string[] tags ) {
            Log( base.Log, LogMessageType.Error, message, exception, tags );
        }

        public void Fatal( string message, Exception exception = null, params string[] tags ) {
            Log( base.Log, LogMessageType.Fatal, message, exception, tags );
        }

        public void Log( string module, LogMessageType messageType, string message, Exception exception = null, params string[] tags ) {
            try {
                WriteEntry( message, Convert( messageType ), (int)messageType, 0 );
            } catch( Exception ex ) {
                if( Debugger.IsAttached )
                    System.Diagnostics.Debugger.Break();
                // TODO: something better than this.
            }
        }

        internal EventLogEntryType Convert( LogMessageType messageType ) {
            EventLogEntryType logType =
                messageType == LogMessageType.Error || messageType == LogMessageType.Fatal
                    ? EventLogEntryType.Error
                    : messageType == LogMessageType.Warning
                        ? EventLogEntryType.Warning
                        : EventLogEntryType.Information
            ;

            return logType;
        }

        #endregion ILog
    }
}
