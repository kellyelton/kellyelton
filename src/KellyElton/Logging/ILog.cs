using System;

namespace KellyElton.Logging
{
    public interface ILog
    {
        string Module { get; }

        void Fatal( string message, Exception exception = null, params string[] tags );
        void Fatal( Exception exception, string message = null, params string[] tags );

        void Error( string message, Exception exception = null, params string[] tags );
        void Error( Exception exception, string message = null, params string[] tags );

        void Warning( string message, Exception exception = null, params string[] tags );
        void Warning( Exception exception, string message = null, params string[] tags );

        void Event( string message = null, params string[] tags );
    }

    public enum ErrorType : byte
    {
        None       = 0,
        Warning    = 1,
        Unexpected = 2,
        Fatal      = 3
    }

    public static class LogExtensions
    {
    }
}
