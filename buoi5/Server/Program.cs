using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
namespace Server
{
    class Program
    {
        public static string receivedData="1";
        public static int connect = 0;
        public static byte[] data;
        public static Socket sock;
        private static void CallAccept(IAsyncResult iar)
        {            
            Socket client = sock.EndAccept(iar);
            client.BeginReceive(data, 0, data.Length, SocketFlags.None, new AsyncCallback(ReceivedData), sock);
            
        }
        private static void SendData(IAsyncResult iar)
        {
            Socket server = (Socket)iar.AsyncState;
            int Sent = server.EndSend(iar);
        }
        public static void ReceivedData(IAsyncResult iar)
        {
            data = new byte[1024];
            Socket sock = (Socket)iar.AsyncState;
            int recv = sock.EndReceive(iar);
            sock.BeginReceive(data, 0, data.Length, SocketFlags.None, new AsyncCallback(ReceivedData), sock);
        }
        static void Main(string[] args)
        {
            data = new byte[1024];
            IPEndPoint ipe = new IPEndPoint(IPAddress.Any, 9050);
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sock.Bind(ipe);
            sock.Listen(5);
            sock.BeginAccept(new AsyncCallback(CallAccept), sock);
            
            while(true)
            { Console.ReadLine(); }
            
            

        }   
    }
}
