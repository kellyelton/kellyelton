using System;

namespace KellyElton
{
    public interface ISignaler
    {
        void Event(string type, int severity, object[] attachements = null);
    }

    public static class Signal
    {
        public static ISignaler DefaultSignaler { get; set; }

        public static void Exception(Exception ex, Severity severity = default(Severity)) {
            Event(nameof(Exception), severity, ex);
        }

        public static void Event(string type, Severity severity = Severity.Not, params object[] attachements) {
            DefaultSignaler?.Event(type, (int)severity, attachements);
        }
    }

    public enum Severity
    {
        Not = -1,
        Normal = 0,
        High = 1,
        Critical = 2
    }
}
