using System.Net.Sockets;

namespace KellyElton.Networking
{
    public class SocketReceiveBundle
    {
        public const int BufferSize = 1024;
        public byte[] Buffer = new byte[BufferSize];
        public Socket Client;

        public SocketReceiveBundle(TcpClient tcpClient)
        {
            this.Client = tcpClient.Client;
        }

        public SocketReceiveBundle(UdpClient udpClient)
        {
            this.Client = udpClient.Client;
        }

        public SocketReceiveBundle(Socket sock)
        {
            Client = sock;
        }

        ~SocketReceiveBundle()
        {
            Client = null;
            this.Buffer = null;
        }
    }
}