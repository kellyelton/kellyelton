using System.ComponentModel;
using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace KellyElton.ModuleHost.WindowsService.Components
{
    public partial class EventLogLogger : EventLog, ILog, ILoadable
    {
        public EventLogLogger() : base() { }
        public EventLogLogger( IContainer container ) : base() {
            InitializeComponent();
        }

        #region LoadableComponent

        public void Load() {
            CreateEventSource();
        }

        #endregion LoadableComponent

        #region ILogger

        [Browsable( false )]
        public string Module => base.Source;

        private void CreateEventSource() {
            if( LicenseManager.UsageMode == LicenseUsageMode.Designtime ) return;

            //EventLog.Delete( Module );
            //EventLog.DeleteEventSource( Module );

            if( EventLog.SourceExists( Source ) ) {
                EventLog evLog = new EventLog { Source = Source };
                if( evLog.Log != Module ) {
                    EventLog.DeleteEventSource( Source );
                }
            }

            if( !EventLog.SourceExists( Source ) ) {
                EventLog.CreateEventSource( Source, Module );
                EventLog.WriteEntry( Source, String.Format( "Event Log Created '{0}'/'{1}'", Module, Source ), EventLogEntryType.Information );
            }
        }

        private const string Seperator = ">";
        private static string ApplicationName = typeof( Program ).Assembly.GetName().Name;
        private static Version ApplicationVersion = typeof( Program ).Assembly.GetName().Version;
        private static string IdentifierFormat = $"{ApplicationName}.{{0}} v{ApplicationVersion}";


        public void Error( string message, Exception exception = null, params string[] tags )   => ((ILog)this).Log(nameof(Error), Module, message, exception, tags);

        public void Error( Exception exception, string message = null, params string[] tags )   => ((ILog)this).Log(nameof(Error), Module, message, exception, tags);

        public void Warning( string message, Exception exception = null, params string[] tags ) => ((ILog)this).Log(nameof(Warning), Module, message, exception, tags);

        public void Warning( Exception exception, string message = null, params string[] tags ) => ((ILog)this).Log(nameof(Warning), Module, message, exception, tags);

        public void Event(string name, string message = null, params string[] tags)             => ((ILog)this).Log(name, Module, message, null, tags);

        void ILog.Log(string logLevel, string module, string message, Exception exception, params string[] tags) {
            try {
                var completeMessage = string.Join( Seperator, CreateLogSections(logLevel, Module, message, tags, exception ) );

                WriteEntry( completeMessage, Convert(logLevel), 0, 0 );
                Console.WriteLine( completeMessage );
            } catch( Exception ex ) {
                // TODO: something better than this.
                if( Debugger.IsAttached )
                    System.Diagnostics.Debugger.Break();
            }
        }

        internal EventLogEntryType Convert( string logLevel ) {
            switch (logLevel) {
                case nameof(ILog.Error):
                    return EventLogEntryType.Error;
                case nameof(ILog.Warning):
                    return EventLogEntryType.Warning;
                default:
                    return EventLogEntryType.Information;
            }
        }

        private static IEnumerable<string> CreateLogSections( string logType, string moduleName, string message, string[] tags, Exception exception ) {
            // Create log type
            yield return logType;

            // Date Time
            yield return DateTimeOffset.Now.ToString( "u" );

            //Application.Module v1.2.3.4
            yield return string.Format( IdentifierFormat, moduleName );

            if( message != null ) yield return message;

            if( (tags?.Length ?? 0) > 0 ) {
                yield return Seperator + string.Join( " ", tags );
            }

            if(exception != null ) {
                yield return Environment.NewLine;
                yield return exception.ToString();
            }
        }

        #endregion ILogger
    }
}
