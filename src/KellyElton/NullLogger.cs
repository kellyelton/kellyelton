using System;
using System.Diagnostics;

namespace KellyElton
{
    public class NullLogger : ILogger
    {
#if (DEBUG)
        internal static System.Collections.Concurrent.ConcurrentQueue<string> LogMessages { get; set; } = new System.Collections.Concurrent.ConcurrentQueue<string>();
#endif

        public string Module { get; }

        public NullLogger(LoggerFactory.Context context) {
            Module = context.Name;
        }

        public void Error(string message, Exception exception = null, params string[] tags) {
            Log(nameof(Error), Module, message, exception, tags);
        }

        public void Error(Exception exception, string message = null, params string[] tags) {
            Log(nameof(Error), Module, message, exception, tags);
        }

        public void Warning(string message, Exception exception = null, params string[] tags) {
            Log(nameof(Warning), Module, message, exception, tags);
        }

        public void Warning(Exception exception, string message = null, params string[] tags) {
            Log(nameof(Warning), Module, message, exception, tags);
        }

        public void Event(string name, string message = null, params string[] tags) {
            Log(nameof(Event), Module, name + ": " + message, null, tags);
        }

        public void Log(string logLevel, string module, string message = null, Exception exception = null, params string[] tags) {
            Write(FormatMessage(logLevel, module, message, exception, tags));
        }

        private static string FormatMessage(string logLevel, string module, string message = null, Exception exception = null, params string[] tags) {
            var ext = exception == null ? string.Empty : Environment.NewLine + exception.ToString();
            var tagString = string.Join(",", tags);
            if (tagString.Length > 0)
                tagString = "[" + tagString + "]";

            return $"{logLevel.ToUpper()}{tagString}: {module}: {message} {ext}";
        }

        private static void Write(string message) {
            Debug.WriteLine(message);
#if (DEBUG)
            LogMessages.Enqueue(message);
            if (LogMessages.Count > 200) {
                LogMessages.TryDequeue(out string blah);
            }
#endif
        }
    }
}