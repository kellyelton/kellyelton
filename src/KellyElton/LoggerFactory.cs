using System;

namespace KellyElton
{
    public static class LoggerFactory
    {
        public static Func<Context, ILog> DefaultMethod { get; set; }

        public static ILog Create(string name) {
            var context = new Context {
                Name = name
            };
            return DefaultMethod?.Invoke(context) ?? new NullLogger(context);
        }

        public static ILog Create(Type type) => Create(type.Name);

        public class Context
        {
            public string Name { get; set; }
        }
    }
}