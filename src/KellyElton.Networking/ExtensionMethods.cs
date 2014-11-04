using System.Net.Sockets;
using System.Reflection;

namespace KellyElton.Networking
{
    public static class ExtensionMethods
    {
        public static object ReadPrivateField<T>(this T obj, string name)
        {
            var field = typeof(T).GetField(name, BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
            var value = field.GetValue(obj);
            return value;
        }

        public static bool IsDisposed(this TcpClient client)
        {
            return (bool)client.ReadPrivateField("m_CleanedUp");
        }

        public static bool IsDisposed(this Socket sock)
        {
            return (bool) sock.ReadPrivateField("CleanedUp");
        }

    }
}