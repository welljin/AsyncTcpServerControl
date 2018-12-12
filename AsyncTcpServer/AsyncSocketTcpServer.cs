using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using TcpServer.AsyncSocketServer;

namespace AsyncTcpServer
{
    public partial class AsyncSocketTcpServer : UserControl
    {
        #region 控件属性

        [Browsable(true), Description("服务器IP"), Category("用户自定义属性")]
        /// <summary>
        /// server ip address
        /// </summary>
        public IPAddress ServerAddress { get; set; } = IPAddress.Parse("127.0.0.1");

        [Browsable(true), Description("服务器端口"), Category("用户自定义属性")]
        /// <summary>
        /// server port
        /// </summary>
        public int ServerPort { get; set; } = 11000;

        [Browsable(false),Description("服务器运行状态")]
        /// <summary>
        /// server state running or stop
        /// </summary>
        public bool IsRunning { get;  set; }

        [Browsable(false), Description("服务器当前连接客户端列表")]
        /// <summary>
        /// clients list
        /// </summary>
        public List<Socket> _clientsList { get; private set; }

        #endregion

        #region 控件事件

        /// <summary>
        /// 服务器打开操作
        /// </summary>
        //[Browsable(true),Description("服务器打开操作")]
        //public event EventHandler<TcpServerStartEventArgs> _ServerStart;

        ///// <summary>
        ///// 服务器关闭操作
        ///// </summary>
        //[Browsable(true), Description("服务器关闭操作")]
        //public event EventHandler<TcpServerStopEventArgs> _ServerStop;

        /// <summary>
        /// 客户端连接事件
        /// </summary>
        [Browsable(true), Description("客户端连接事件"),Category("用户自定义事件")]
        public event EventHandler<TcpServerClientConnectedEventArgs> ClientConnected;

        /// <summary>
        /// 服务器接收数据事件
        /// </summary>
        [Browsable(true), Description("服务器接收数据事件"), Category("用户自定义事件")]
        public event EventHandler<TcpServerReceiveDatadEventArgs> ReceiveData;

        ///// <summary>
        ///// 服务器发送数据事件
        ///// </summary>
        //[Browsable(true), Description("服务器发送数据事件"), Category("用户自定义事件")]
        //public event EventHandler<TcpServerSendDatadEventArgs> SendData;

        /// <summary>
        /// 客户端断开连接事件
        /// </summary>
        [Browsable(true), Description("客户端断开连接事件"), Category("用户自定义事件")]
        public event EventHandler<TcpServerClientDisconnectedEventArgs> ClientDisconnected;

        #endregion

        #region 私有属性

        private AsyncSocketServer server;

        #endregion

        public AsyncSocketTcpServer()
        {
            InitializeComponent();
        }

        #region 打开服务器，绑定事件，返回结果

        /// <summary>
        /// 打开服务器操作，此服务器最大连接数为10
        /// 默认服务器端口位11000
        /// </summary>
        /// <param name="iPAddress">服务器地址</param>
        /// <param name="port">服务器端口</param>
        /// <returns>服务器运行状态</returns>
        public bool _ServerStart(IPAddress iPAddress, int port = 11000)
        {
            ServerAddress = iPAddress;
            ServerPort = port;
            server = new AsyncSocketServer(iPAddress, port, 10);
            server._ClientConnected += ClientConnected;
            server._ClientDisconnected += ClientDisconnected;
            server._ReceiveData += ReceiveData;
            server.ServerStart();
            IsRunning = server.IsRunning;
            return server.IsRunning;
        }

        /// <summary>
        /// 打开服务器操作，此服务器最大连接数为10
        /// 默认服务器端口位11000
        /// </summary>
        /// <param name="iPAddress">服务器地址</param>
        /// <param name="port">服务器端口</param>
        /// <param name="maxbacklog">服务器最大连接数</param>
        /// <returns>服务器运行状态</returns>
        public bool _ServerStart(IPAddress iPAddress, int maxbacklog, int port = 11000)
        {
            ServerAddress = iPAddress;
            ServerPort = port;
            server = new AsyncSocketServer(iPAddress, port, maxbacklog);
            server._ClientConnected += ClientConnected;
            server._ClientDisconnected += ClientDisconnected;
            server._ReceiveData += ReceiveData;
            server.ServerStart(maxbacklog);
            IsRunning = server.IsRunning;
            return server.IsRunning;
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="data"></param>
        public void SendAsync(Socket socket,byte[] data)
        {
            server.HandleSendData(socket, data);
        }

        #endregion

        #region 服务器断开与释放资源

        /// <summary>
        /// 服务器断开操作
        /// </summary>
        public void _ServerStop()
        {
            if (server.IsRunning)
            {
                server._ClientConnected -= ClientConnected;
                server._ClientDisconnected -= ClientDisconnected;
                server._ReceiveData -= ReceiveData;
                server.ServerStop();
                IsRunning = server.IsRunning;
            }
        }
        #endregion

    }
}
