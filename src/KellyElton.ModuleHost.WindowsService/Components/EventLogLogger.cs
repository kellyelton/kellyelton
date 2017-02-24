using KellyElton.Logging;
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

        #region ILog

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

        internal EventLogEntryType Convert( ErrorType errorType ) {
            EventLogEntryType logType =
                errorType == ErrorType.Fatal || errorType == ErrorType.Unexpected
                    ? EventLogEntryType.Error
                    : errorType == ErrorType.Warning
                        ? EventLogEntryType.Warning
                        : EventLogEntryType.Information
            ;

            return logType;
        }

        private const string Seperator = ">";
        private static string ApplicationName = typeof( Program ).Assembly.GetName().Name;
        private static Version ApplicationVersion = typeof( Program ).Assembly.GetName().Version;
        private static string IdentifierFormat = $"{ApplicationName}.{{0}} v{ApplicationVersion}";

        public void Event( string message = null, params string[] tags )                        => WriteLog( "EVENT", ErrorType.None, message, tags );

        public void Fatal( string message, Exception exception = null, params string[] tags )   => LogException( message, ErrorType.Fatal, exception, tags );

        public void Fatal( Exception exception, string message = null, params string[] tags )   => LogException( message, ErrorType.Fatal, exception, tags );

        public void Error( string message, Exception exception = null, params string[] tags )   => LogException( message, ErrorType.Fatal, exception, tags );

        public void Error( Exception exception, string message = null, params string[] tags )   => LogException( message, ErrorType.Fatal, exception, tags );

        public void Warning( string message, Exception exception = null, params string[] tags ) => LogException( message, ErrorType.Fatal, exception, tags );

        public void Warning( Exception exception, string message = null, params string[] tags ) => LogException( message, ErrorType.Fatal, exception, tags );

        private void LogException( string message, ErrorType errorType, Exception exception = null, params string[] tags ) {
            switch( errorType ) {
                case ErrorType.None:
                    Event( message, tags );
                    break;
                case ErrorType.Warning:
                    WriteLog( "WARN ", errorType, message, tags );
                    break;
                case ErrorType.Unexpected:
                    WriteLog( "ERROR", errorType, message, tags );
                    break;
                case ErrorType.Fatal:
                    WriteLog( "FATAL", errorType, message, tags );
                    break;
            }
        }

        private void WriteLog( string logType, ErrorType errorType, string message, string[] tags ) {
            try {
                var completeMessage = string.Join( Seperator, CreateLogSections( logType, Module, message, tags ) );

                WriteEntry( completeMessage, Convert( errorType ), 0, 0 );
            } catch( Exception ex ) {
                // TODO: something better than this.
                if( Debugger.IsAttached )
                    System.Diagnostics.Debugger.Break();
            }
        }

        private static IEnumerable<string> CreateLogSections( string logType, string moduleName, string message, string[] tags ) {
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
        }

        #endregion ILog
    }
}
