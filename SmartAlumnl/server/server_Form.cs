using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace server
{
    public partial class server_Form : Form
    {
        #region 属性
        /// <summary>
        /// 通信socket
        /// </summary>
        public SocketServerManager _ssm;

        /// <summary>
        /// serverIP 
        /// </summary>
        public string serverIPString = "127.0.0.1";

        /// <summary>
        /// 服务端口
        /// </summary>
        public int serverPort = 12345;
        #endregion

        public server_Form()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 窗口加载函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {

                /// 通信套接字
            _ssm = new SocketServerManager(serverIPString, serverPort);

            /// 设置回调函数
            _ssm.OnReceiveCompletedEvent += this.OnReceiveCompleted;
            _ssm.OnConnectedEvent += this.OnConnectCompleted;

            /// 启动监听
            _ssm.Start();

            /// 设置服务器相关状态
            lbl_ServerIP.Text += serverIPString;
            lbl_Port.Text += serverPort.ToString();
            lbl_ServerStatus.Text += ServerStatus.Start;
        }

        #region 远端成功连接服务器的UI处理
        /// <summary>
        /// 远端成功连接服务器的UI处理（回调函数）
        /// </summary>
        /// <param name="mf">消息体</param>
        public void OnConnectCompleted(MessageFormat mf)
        {
            /// 添加传过来的消息到服务器窗口上
            this.Invoke(new Action(() =>
                {
                    /// 消息窗口添加消息
                    lb_Msg.Items.Add(DateTime.Now.ToString("HH:mm:ss") +": "+ mf.msgStr);
                }));
        }
        #endregion

        #region 远端断开服务器连接时，服务器的UI处理（回调函数）
        public void OnDisconnectCompleted(MessageFormat mf)
        {
            this.Invoke(new Action(() =>
               {
                   /// 添加消息
                   lb_Msg.Items.Add(DateTime.Now.ToString("HH:mm:ss") + ": " + mf.msgStr);
               }));
        }
        
        #endregion

        #region 收到消息时，UI的处理
        /// <summary>
        /// 收到消息时，UI的处理
        /// </summary>
        /// <param name="mf">收到的消息体</param>
        public void OnReceiveCompleted(MessageFormat mf)
        {
            /// 消息对立对象
            DispatchMessage dm = new DispatchMessage(ref mf, ref _ssm);

            /// 检查用户名是否可用
            if (!dm.IsUserAvailable())
            {
                dm.OnSendUserIsNotAvailable();
                return;
            }

            /// 判断消息类型
            switch (dm.GetMsgType())
            {
                case MessageType.LOGIN_login:
                    break;
                default:
                    break;
            }

            /// -------------------------------  UI显示消息
            this.Invoke(new Action(() =>
            {
                /// 添加消息
                lb_Msg.Items.Add(DateTime.Now.ToString("HH:mm:ss : ") + "[" + mf.ipStr + "] saying: " + mf.msgStr);
            }));
        }
        #endregion
    }
}
