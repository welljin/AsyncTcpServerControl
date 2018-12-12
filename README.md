# AsyncTcpServerControl
异步Socket服务器，控件形式，可直接使用，无需绑定服务器事件

 开启：
 asyncSocketTcpServer1._ServerStart(IPAddress.Parse("127.0.0.1"));
 
 关闭：
 asyncSocketTcpServer1._ServerStop();
 
 发送：
 asyncSocketTcpServer1.SendAsync(socket,byte[]);
 
 用户自定义事件：
 
        //客户端连接
        
         private void AsyncSocketTcpServer1_ClientConnected(object sender, TcpServerClientConnectedEventArgs e)
         
        { 
        
        }
        
        //客户端断开连接
        
        private void AsyncSocketTcpServer1_ClientDisconnected(object sender, TcpServerClientDisconnectedEventArgs e)
        
        {           
        
        }
        
        //接收数据
        
        private void AsyncSocketTcpServer1_ReceiveData(object sender, TcpServerReceiveDatadEventArgs e)
        
        {         
        
        }
        
