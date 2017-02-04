using System;
using System.Runtime.CompilerServices;

namespace KellyElton.Logging
{
    public interface ILog
    {
        string Module { get; }

        void Trace   ( string message, Exception exception = null, params string[] tags );
        void Debug   ( string message, Exception exception = null, params string[] tags );
        void Standard( string message, Exception exception = null, params string[] tags );
        void Warning ( string message, Exception exception = null, params string[] tags );
        void Error   ( string message, Exception exception = null, params string[] tags );
        void Fatal   ( string message, Exception exception = null, params string[] tags );

        void Log( string module, LogMessageType messageType, string message, Exception exception = null, params string[] tags );
    }

    public enum LogMessageType : byte
    {
        Trace    = 0,
        Debug    = 1,
        Standard = 2,
        Warning  = 3,
        Error    = 4,
        Fatal    = 5
    }

    public static class LogExtensions
    {
        public static void Trace( this ILog log, [CallerMemberName] string methodName = null ) {
            log.Trace( methodName );
        }

        public static void TraceExit( this ILog log, [CallerMemberName] string methodName = null ) {
            log.Trace( $"Exit {methodName}" );
        }
    }
}
