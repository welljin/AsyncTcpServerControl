using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TcpServer.AsyncSocketServer;

namespace TESTForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();          
        }

        private void Button1_Click(object sender, EventArgs e)
        {

        }

        private void Openbtn_Click(object sender, EventArgs e)
        {
            asyncTcpServer1._ServerStart();
            Openbtn.Enabled = !asyncTcpServer1.IsRunning;
            Closebtn.Enabled= asyncTcpServer1.IsRunning;
        }

        private void Closebtn_Click(object sender, EventArgs e)
        {
            asyncTcpServer1._ServerStop();
            Openbtn.Enabled = !asyncTcpServer1.IsRunning;
            Closebtn.Enabled = asyncTcpServer1.IsRunning;
        }

        private void AsyncTcpServer1_ClientConnected(object sender, TcpServerClientConnectedEventArgs e)
        {
            Trace.WriteLine($"{e.Socket.RemoteEndPoint.ToString()}:已连接");
        }

        private void AsyncTcpServer1_ClientDisconnected(object sender, TcpServerClientDisconnectedEventArgs e)
        {
            Trace.WriteLine($"{e.Socket.RemoteEndPoint.ToString()}:已断开");
        }

        private void AsyncTcpServer1_ReceiveData(object sender, TcpServerReceiveDatadEventArgs e)
        {
            Trace.WriteLine($"{e.Socket.RemoteEndPoint.ToString()}:收到{ Encoding.ASCII.GetString(e.Data)}");
            
        }

    }
}
