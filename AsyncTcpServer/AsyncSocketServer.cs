using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpServer.AsyncSocketServer
{
    public class AsyncSocketServer : IDisposable
    {
        #region 服务器事件

        public event EventHandler<TcpServerStopEventArgs> _ServerStop;
        public event EventHandler<TcpServerStartEventArgs> _ServerStart;
        public event EventHandler<TcpServerSendDatadEventArgs> _SendData;
        public event EventHandler<TcpServerReceiveDatadEventArgs> _ReceiveData;
        public event EventHandler<TcpServerClientConnectedEventArgs> _ClientConnected;
        public event EventHandler<TcpServerClientDisconnectedEventArgs> _ClientDisconnected;
        #endregion

        #region 私有属性

        /// <summary>
        /// max client 
        /// </summary>
        private int _maxClientCount;

        /// <summary>
        /// current clinet count
        /// </summary>
        private int _currentClientCount;

        /// <summary>
        /// server socket
        /// </summary>
        private Socket _serverSocket;

        private byte[] Receivebuffer;

        private bool disposed = false;

        #endregion

        #region 服务器属性

        /// <summary>
        /// server state running or stop
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// server ip address
        /// </summary>
        public IPAddress ServerAddress { get; private set; }

        /// <summary>
        /// clients list
        /// </summary>
        public List<Socket> _clientsList { get; private set; }

        /// <summary>
        /// server port
        /// </summary>
        public int ServerPort { get; private set; }

        #endregion

        #region 构造函数

        /// <summary>
        /// 空构造函数
        /// </summary>
        public AsyncSocketServer()
        {

        }

        /// <summary>
        /// 异步Socket TCP服务器
        /// </summary>
        /// <param name="localIPAddress">服务器地址</param>
        /// <param name="listenPort">服务器端口</param>
        /// <param name="maxClient">连接限制</param>
        public AsyncSocketServer(IPAddress localIPAddress, int listenPort, int maxClient)
        {
            this.ServerAddress = localIPAddress;
            this.ServerPort = listenPort;
            this._maxClientCount = maxClient;

            _clientsList = new List<Socket>();
            _serverSocket = new Socket(localIPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }

        /// <summary>
        /// 异步Socket TCP服务器
        /// </summary>
        /// <param name="listenPort">监听的端口</param>
        public AsyncSocketServer(int listenPort) : this(IPAddress.Any, listenPort, 10) { }


        /// <summary>
        /// 异步Socket TCP服务器
        /// </summary>
        /// <param name="localEP">监听的终结点</param>
        public AsyncSocketServer(IPEndPoint localEP) : this(localEP.Address, localEP.Port, 10) { }

        #endregion

        #region 打开关闭服务

        /// <summary>
        /// 启动服务器，默认最大连接数位10
        /// </summary>
        public void ServerStart()
        {
            if (!IsRunning)
            {
                _serverSocket.Bind(new IPEndPoint(this.ServerAddress, this.ServerPort));
                _serverSocket.Listen(10);
                _serverSocket.BeginAccept(new AsyncCallback(HandleAcceptClientCallback), _serverSocket);
                IsRunning = true;
                _ServerStart?.Invoke(this,new TcpServerStartEventArgs(_serverSocket));
                Trace.WriteLine($"{DateTime.Now.ToString()}:服务器{ServerAddress.ToString()}已启动");
            }
        }

        /// <summary>
        /// 启动服务器，并设置最大连接数
        /// </summary>
        public void ServerStart(int backlog)
        {
            if (!IsRunning)
            {
                _serverSocket.Bind(new IPEndPoint(this.ServerAddress, this.ServerPort));
                _serverSocket.Listen(backlog);
                _serverSocket.BeginAccept(new AsyncCallback(HandleAcceptClientCallback), _serverSocket);
                IsRunning = true;
                _ServerStart?.Invoke(this, new TcpServerStartEventArgs(_serverSocket));
                Trace.WriteLine($"{DateTime.Now.ToString()}:服务器{ServerAddress.ToString()}已启动");
            }
        }

        /// <summary>
        /// 关闭服务器
        /// </summary>
        public void ServerStop()
        {
            if (IsRunning)
            {
                IsRunning = false;               
                lock (_clientsList)
                {
                    foreach (Socket s in _clientsList)
                    {
                        s.Shutdown(SocketShutdown.Both);
                        s.Close();
                    }
                }
                _serverSocket.Close();
                _ServerStop?.Invoke(this, new TcpServerStopEventArgs(_serverSocket));
                Trace.WriteLine($"{DateTime.Now.ToString()}:服务器{ServerAddress.ToString()}已关闭");
            }
        }

        #endregion.

        #region 获取本机地址列表

        /// <summary>
        /// 获取本机IPadress列表
        /// </summary>
        private IPAddress[] GetLocalIPaddress()
        {
            return Dns.GetHostAddresses(Dns.GetHostName());
        }
        #endregion

        #region 接收连接+接收数据+发送数据

        /// <summary>
        /// 接受客户端连接
        /// </summary>
        /// <param name="ar">异步结果</param>
        public void HandleAcceptClientCallback(IAsyncResult ar)
        {
            if (IsRunning)
            {
                Socket _ServerSocket = (Socket)ar.AsyncState;  //服务端            
                Socket _ClientSocket = _ServerSocket.EndAccept(ar);//客户端
                if (_currentClientCount >= _maxClientCount)
                {
                    throw new Exception("连接数超过限制");
                }
                else
                {
                    _currentClientCount++;
                    _clientsList.Add(_ClientSocket);
                    _ClientConnected?.Invoke(this, new TcpServerClientConnectedEventArgs(_ClientSocket));
                    Trace.WriteLine($"{DateTime.Now.ToString()}:客户端{_ClientSocket.RemoteEndPoint.ToString()}已启动");
                    Receivebuffer = new byte[1024];
                    _ClientSocket.BeginReceive(Receivebuffer, 0, Receivebuffer.Length, SocketFlags.None, new AsyncCallback(HandleDataReceiveCallback), _ClientSocket);
                }
                _ServerSocket.BeginAccept(new AsyncCallback(HandleAcceptClientCallback), _ServerSocket);
            }
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="ar"></param>
        private void HandleDataReceiveCallback(IAsyncResult ar)
        {
            if (IsRunning)
            {
                Socket _ClientSocket = ar.AsyncState as Socket;

                try
                {
                    int receivecount = _ClientSocket.EndReceive(ar);
                    if (receivecount != 0)
                    {
                        _ClientSocket.BeginReceive(Receivebuffer, 0, Receivebuffer.Length, SocketFlags.None, new AsyncCallback(HandleDataReceiveCallback), _ClientSocket);
                        byte[] Receivebuff = new byte[receivecount];
                        Array.Copy(Receivebuffer, Receivebuff, receivecount);
                        _ReceiveData?.Invoke(this, new TcpServerReceiveDatadEventArgs(_ClientSocket, Receivebuff));
                    }
                    else//正常断开
                    {
                        _currentClientCount--;
                        _clientsList.Remove(_ClientSocket);
                        _ClientDisconnected?.Invoke(this, new TcpServerClientDisconnectedEventArgs(_ClientSocket));
                        Trace.WriteLine($"断开连接：{ _ClientSocket.RemoteEndPoint.ToString()}");
                    }
                }
                catch //(Exception ex)//异常断开
                {
                    _currentClientCount--;
                    _clientsList.Remove(_ClientSocket);
                    _ClientDisconnected?.Invoke(this, new TcpServerClientDisconnectedEventArgs(_ClientSocket));
                    Trace.WriteLine($"断开连接：{ _ClientSocket.RemoteEndPoint.ToString()}");
                }
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="server"></param>
        /// <param name="data"></param>
        public void HandleSendData(Socket server, byte[] data)
        {
            if (!IsRunning) throw new InvalidProgramException("服务端尚未启动！");
            if (server == null) throw new ArgumentNullException("接收端Client地址为空！");
            if (data == null) throw new ArgumentNullException("发送数据data为空！");
            //client.BeginSend(data, 0, data.Length, SocketFlags.None, null, null);
            server.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(HandleSendDataCallback), server);
            _SendData?.Invoke(this, new TcpServerSendDatadEventArgs(server, data));
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="ar"></param>
        public virtual void HandleSendDataCallback(IAsyncResult ar)
        {
            Socket server = ar.AsyncState as Socket;
            int bytesend = server.EndSend(ar);
            if (bytesend == 0)
            {
                //to-do 发送完成工作
            }
        }

        #endregion

        #region Dispose
        /// <summary>
        /// Performs application-defined tasks associated with freeing, 
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release 
        /// both managed and unmanaged resources; <c>false</c> 
        /// to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    try
                    {
                        ServerStop();
                        if (_serverSocket != null)
                        {
                            _serverSocket = null;
                        }
                    }
                    catch (SocketException)
                    {
                        //TODO
                        //RaiseOtherException(null);
                    }
                }
                disposed = true;
            }
        }
        #endregion
    }

    //\\\\\\\\\\\\\\\\\\\\\\\\\\定义服务器打开事件//////////////////////////\\
    /// <summary>
    /// TcpServer启动事件；绑定后，可接收启动事件。
    /// 参数：Socket（服务器Socket）
    /// </summary>
    public class TcpServerStartEventArgs : EventArgs
    {
        public Socket Socket { get; set; }

        public TcpServerStartEventArgs(Socket _Socket)
        {
            Socket = _Socket;
        }
    }

    //\\\\\\\\\\\\\\\\\\\\\\\\\\定义服务器关闭事件//////////////////////////\\
    /// <summary>
    /// TcpServer停止事件，绑定后，可接收停止事件
    /// </summary>
    public class TcpServerStopEventArgs : EventArgs
    {
        public Socket Socket { get; set; }

        public TcpServerStopEventArgs(Socket _Socket)
        {
            Socket = _Socket;
        }
    }

    //\\\\\\\\\\\\\\\\\\\\\\\定义服务器接收客户端事件///////////////////////\\
    /// <summary>
    /// TcpServer客户端连接事件，绑定后客户端连接可触发
    /// </summary>
    public class TcpServerClientConnectedEventArgs : EventArgs
    {
        public Socket Socket { get; set; }

        public TcpServerClientConnectedEventArgs(Socket _Socket)
        {
            Socket = _Socket;
        }
    }

    //\\\\\\\\\\\\\\\\\\\\\\\\定义服务器接收数据事件////////////////////////\\
    /// <summary>
    /// 服务器接收数据事件
    /// Socket：当前会话客户端Socket
    /// Data：客户端发送数据
    /// </summary>
    public class TcpServerReceiveDatadEventArgs : EventArgs
    {
        public Socket Socket { set; get; }
        public byte[] Data { set; get; }

        public TcpServerReceiveDatadEventArgs(Socket _Socket,byte[] _Data)
        {
            Socket = _Socket;
            Data = _Data;
        }
    }

    //\\\\\\\\\\\\\\\\\\\\\\\\定义服务器发送数据事件////////////////////////\\
    /// <summary>
    /// 服务器发送数据事件
    /// Socket：当前会话客户端Socket
    /// Data：客户端发送数据
    /// </summary>
    public class TcpServerSendDatadEventArgs : EventArgs
    {
        public Socket Socket { set; get; }
        public byte[] Data { set; get; }

        public TcpServerSendDatadEventArgs(Socket _Socket, byte[] _Data)
        {
            Socket = _Socket;
            Data = _Data;
        }
    }


    //\\\\\\\\\\\\\\\\\\\\\\\定义服务器客户端断开事件///////////////////////\\
    /// <summary>
    /// 
    /// </summary>
    public class TcpServerClientDisconnectedEventArgs : EventArgs
    {
        public Socket Socket { get; set; }

        public TcpServerClientDisconnectedEventArgs(Socket _Socket)
        {
            Socket = _Socket;
        }
    }

}