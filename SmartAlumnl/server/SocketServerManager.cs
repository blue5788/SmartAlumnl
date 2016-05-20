using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms.VisualStyles;
using System.Xml.Serialization;

namespace server
{
    public class SocketServerManager
    {
        readonly Socket _socket;        //监听Socket
        readonly EndPoint _endPoint;
        private const int Backlog = 100;     //允许连接数目
        private int totalBytes = 1024 * 50;
        private int byteSize = 64;

        //同时UI界处理的事件
        public delegate void OnEventCompletedHanlder(MessageFormat msg);
        public event OnEventCompletedHanlder OnReceiveCompletedEvent;
        public event OnEventCompletedHanlder OnSendCompletedEvent;
        public event OnEventCompletedHanlder OnConnectedEvent;
        public event OnEventCompletedHanlder OnDisconnectEvent;
        public event OnEventCompletedHanlder OnNotConnectEvent;

        //private BufferManager bufferManager;        //消息缓存管理
        SocketAsyncEventArgsPool rwPool;              //SAEA池
        private Semaphore maxClient;
        private Dictionary<string, SocketAsyncEventArgs> dicSAEA = null; 

        public SocketServerManager(string ip, int port)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAddress = IPAddress.Parse(ip);
            _endPoint = new IPEndPoint(ipAddress, port);
            //bufferManager = new BufferManager(totalBytes, byteSize);
            maxClient = new Semaphore(Backlog, Backlog);
            Init();
        }

        public void Init()
        {
            //bufferManager.InitBuffer();
            SocketAsyncEventArgs rwEventArgs;
            rwPool = new SocketAsyncEventArgsPool(Backlog);
            dicSAEA = new Dictionary<string, SocketAsyncEventArgs>();
            for (int i = 0; i < 100; i++)
            {
                rwEventArgs = new SocketAsyncEventArgs();
                rwEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
               rwEventArgs.UserToken = new AsyncUserToken();

                rwEventArgs.SetBuffer(new byte[byteSize],0,byteSize);
                //bufferManager.SetBuffer(rwEventArgs);

                rwPool.Push(rwEventArgs);
            }
        }

        /// <summary>
        /// 开启Socket监听
        /// </summary>
        public void Start()
        {
            _socket.Bind(_endPoint);        //绑定本地地址进行监听
            _socket.Listen(Backlog);        //设置监听数量

            StartAccept(null);
        }

        public void StartAccept(SocketAsyncEventArgs acceptEventArg)
        {
            if (acceptEventArg == null)
            {
                acceptEventArg = new SocketAsyncEventArgs();
                acceptEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(OnConnectedCompleted);
            }
            else
            {
                acceptEventArg.AcceptSocket = null;
            }

            maxClient.WaitOne();
            bool willRaiseEvent = _socket.AcceptAsync(acceptEventArg);
            if (!willRaiseEvent)
            {
                ProcessAccept(acceptEventArg);
            }
        }

        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            if (e.SocketError != SocketError.Success) return;       //异步处理失败，不做处理
            SocketAsyncEventArgs saea = rwPool.Pop();
            AsyncUserToken token = saea.UserToken as AsyncUserToken;
            token.UserSocket = e.AcceptSocket;       //获取远端对话Socket对象
            string ipRemote = token.UserSocket.RemoteEndPoint.ToString();
            string ip = token.UserSocket.RemoteEndPoint.ToString();
            MessageFormat msg = new MessageFormat(string.Format("[{0}] 成功连接到服务器", ipRemote), ip);
            if (OnConnectedEvent != null) OnConnectedEvent(msg);            //调用UI方法处理

            //连接成功后，发送消息通知远程客户端
            //OnSend("Connected Success !", _sendSocket.RemoteEndPoint);
            SocketAsyncEventArgs sendArgs = new SocketAsyncEventArgs();
            sendArgs.RemoteEndPoint = token.UserSocket.RemoteEndPoint;
            sendArgs.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
            sendArgs.UserToken = saea.UserToken;
            dicSAEA.Add(token.UserSocket.RemoteEndPoint.ToString(), sendArgs);
            bool willRaiseEvent = e.AcceptSocket.ReceiveAsync(saea);
            if (!willRaiseEvent)
            {
                OnReceiveCompleted(saea);
            }

            StartAccept(e);
        }

        /// <summary>
        /// 远端地址连接本地成功的回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnConnectedCompleted(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }

        public void IO_Completed(object sender, SocketAsyncEventArgs e)
        {
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    OnReceiveCompleted(e);
                    break;
                case SocketAsyncOperation.Send:
                    OnSendCompleted(e);
                    break;
                default:
                    throw new ArgumentException("The last operation completed on the socket was not a receive or send");
            }   
        }

        /// <summary>
        /// 执行异步发送消息
        /// </summary>
        /// <param name="msg">消息内容</param>
        /// <param name="ip">发送远端地址</param>
        public void OnSend(MessageFormat mf)
        {
            if (!dicSAEA.ContainsKey(mf.ipStr))
            {
                if (OnNotConnectEvent != null)
                {
                    OnNotConnectEvent(new MessageFormat("不存在此连接客户端",""));
                    return;
                }
            }
            SocketAsyncEventArgs saea = dicSAEA[mf.ipStr];
            AsyncUserToken token = saea.UserToken as AsyncUserToken;
            if (saea == null) return;
            //saea.SetBuffer(sendBuffer, 0, sendBuffer.Length);  //设置SAEA的buffer消息内容
            byte[] sendBuffer = Encoding.Unicode.GetBytes(mf.msgStr);
            saea.SetBuffer(sendBuffer, 0, sendBuffer.Length);
            bool willRaiseEvent = token.UserSocket.SendAsync(saea);
            if (!willRaiseEvent)
            {
                OnSendCompleted(saea);
            }
        }

        /// <summary>
        /// 发送消息回调处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnSendCompleted(SocketAsyncEventArgs e)
        {
            AsyncUserToken token = e.UserToken as AsyncUserToken;
            byte[] sendBuffer = e.Buffer;
            string msgStr = Encoding.Unicode.GetString(sendBuffer);
            string ipAddress = token.UserSocket.RemoteEndPoint.ToString();
            MessageFormat msg = new MessageFormat(msgStr, ipAddress);
            if (OnSendCompletedEvent != null) OnSendCompletedEvent(msg);        //调用UI方法处理
        }

        /// <summary>
        /// 接收消息回调处理
        /// </summary>
        /// <param name="e"></param>
        public void OnReceiveCompleted(SocketAsyncEventArgs e)
        {
            if (e.SocketError != SocketError.Success) return;   //判断消息的接收状态
            AsyncUserToken token = e.UserToken as AsyncUserToken;
            int lengthBuffer = e.BytesTransferred;      //获取接收的字节长度
            string ipAddress = token.UserSocket.RemoteEndPoint.ToString();
            MessageFormat msg = new MessageFormat();
            //如果接收的字节长度为0，则判断远端服务器关闭连接
            if (lengthBuffer <= 0)
            {
                msg.msgStr = "远端服务器已经断开连接";
                msg.ipStr = ipAddress;
                if (OnDisconnectEvent != null) OnDisconnectEvent(msg);
                CloseClientSocket(e);
            }
            else
            {
                byte[] receiveBuffer = e.Buffer;
                byte[] buffer = new byte[lengthBuffer];
                Buffer.BlockCopy(receiveBuffer, 0, buffer, 0, lengthBuffer);
                msg.msgStr = Encoding.Unicode.GetString(buffer);
                msg.ipStr = ipAddress;
                bool willRaiseEvent = token.UserSocket.ReceiveAsync(e);     //继续异步接收消息
                if (!willRaiseEvent)
                {
                    OnReceiveCompleted(e);
                }
                if (OnReceiveCompletedEvent != null) OnReceiveCompletedEvent(msg);        //调用UI方法处理
            }
        }

        private void CloseClientSocket(SocketAsyncEventArgs e)
        {
            AsyncUserToken token = e.UserToken as AsyncUserToken;
            try
            {
                token.UserSocket.Shutdown(SocketShutdown.Send);
            }
            catch (Exception) { }
            dicSAEA.Remove(token.UserSocket.RemoteEndPoint.ToString());
            token.UserSocket.Close();

            maxClient.Release();
            rwPool.Push(e);
        }
    }
}
