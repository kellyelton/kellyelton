using System;

namespace KellyElton
{
    public interface ILog
    {
        /// <summary>
        /// The module this <see cref="ILog"/> is logging.
        /// </summary>
        string Module { get; }

        /// <summary>
        /// Log level marking unexpected behavior that indicates something is wrong.
        /// </summary>
        void Error(string message, Exception exception = null, params string[] tags);
        /// <summary>
        /// Log level marking unexpected behavior that indicates something is wrong.
        /// </summary>
        void Error(Exception exception, string message = null, params string[] tags);

        /// <summary>
        /// Log level marking expected behavior that indicates something might be wrong.
        /// </summary>
        void Warning(string message, Exception exception = null, params string[] tags);
        /// <summary>
        /// Log level marking expected behavior that indicates something might be wrong.
        /// </summary>
        void Warning(Exception exception, string message = null, params string[] tags);

        /// <summary>
        /// Log level specifying an event that happened in the application.
        /// </summary>
        void Event(string name, string message = null, params string[] tags);

        /// <summary>
        /// Log an event with a specific log level.
        /// </summary>
        void Log(string logLevel, string module, string message = null, Exception exception = null, params string[] tags);
    }
}