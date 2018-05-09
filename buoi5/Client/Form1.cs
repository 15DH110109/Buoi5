using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
namespace Client
{
    public partial class Form1 : Form
    {
        public static IPEndPoint ipe;
        public static Socket ClientSock;
        public static byte[] data = new byte[1024];
        public static int connect = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void Connect(IAsyncResult iar)
        {
            Socket sock = (Socket)iar.AsyncState;
            sock.EndConnect(iar);
            connect = 1;
        }
        public void ReceivedData(IAsyncResult iar)
        {
            Socket sock = (Socket)iar.AsyncState;
            int recv = sock.EndReceive(iar);
            string receivedData = Encoding.ASCII.GetString(data, 0, recv);
            listBox1.Items.Add(receivedData);
        }
        private void btnConnect_Click(object sender, EventArgs e)
        {
            ClientSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ipe = new IPEndPoint(IPAddress.Parse(txtIP.Text), 9050);
            ClientSock.BeginConnect(ipe, new AsyncCallback(Connect), ClientSock);
            ClientSock.BeginReceive(data, 0, data.Length, SocketFlags.None, new AsyncCallback(ReceivedData), ClientSock);
            
        }
        private static void SendData(IAsyncResult iar)
        {
            Socket server = (Socket)iar.AsyncState;
            int Sent = server.EndSend(iar);
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            data = Encoding.ASCII.GetBytes(txtMS.Text);
            ClientSock.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendData), ClientSock);
            txtMS.Text = "";
        }
    }
}
