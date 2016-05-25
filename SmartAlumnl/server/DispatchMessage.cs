using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace server
{
    class DispatchMessage
    {
        #region 属性

        /// <summary>
        /// 缓冲区长度
        /// </summary>
        private const int RECV_BUF_LEN = 8192;

        /// <summary>
        /// 接收缓冲区
        /// </summary>
        private byte[] RECV_BUF = new byte[RECV_BUF_LEN];

        public int m_MsgLength = 4;

        /// <summary>
        /// 获取或设置客户端传过来的消息(含有消息格式)
        /// </summary>
        private MessageFormat m_MessageFormart;

        /// <summary>
        /// 获取或设置客户端IP
        /// </summary>
        public string m_IPString { get; set; }

        /// <summary>
        /// 收到的消息类型
        /// </summary>
        public string m_RecvMsgType { get; set; }

        /// <summary>
        /// 收到的消息内容
        /// </summary>
        public string m_RecvMsgData { get; set; }

        /// <summary>
        /// 设置与客户端通信的socket
        /// </summary>
        public SocketServerManager m_SocketManager;

        /// <summary>
        /// 用户名
        /// </summary>
        public string m_User;

        /// <summary>
        /// 密码
        /// </summary>
        public string m_Pwd;
        #endregion

        #region 构造函数
        public DispatchMessage(ref MessageFormat msg, ref SocketServerManager ssm)
        {
            m_MessageFormart = msg;
            m_IPString = m_MessageFormart.ipStr;
            m_SocketManager = ssm;
        }
        #endregion

        #region 获取消息体
        /// <summary>
        /// 获取消息体
        /// </summary>
        /// <returns>存在，返回true、失败返回false</returns>
        public bool GetMsgData()
        {
            /// 利用传过来的消息，按照消息格式拆包消息
            /// 拆分消息
            string[] ss = m_MessageFormart.msgStr.Split(new char[] { '|' });

            /// 若消息长度小于4，则返回false
            if (m_MsgLength > ss.Length)
            {
                return false;
            }

            /// 获取用户名、密码、消息类型、消息内容
            m_User = ss[1];
            //MessageBox.Show(m_User);
            m_Pwd = ss[2];
            //MessageBox.Show(m_Pwd);
            m_RecvMsgType = ss[3];
            //MessageBox.Show(m_RecvMsgType);
            m_RecvMsgData = ss[4];
            //MessageBox.Show(m_RecvMsgData);

            return true;
        }
        #endregion

        #region 登录验证

        /// <summary>
        ///  登录验证
        /// </summary>
        public void Check_Login()
        {
            MessageBox.Show("开始检查登录");
            /// 查询数据库中是否存在此用户与密码
            /// 学号 = 密码
            string queryStr = "SELECT * FROM SA_Login where SNo = '" + m_User + "' and SNo = '" + m_Pwd + "'";
            DataSet resultDs = AccessHelper.dataSet(queryStr);

            MessageBox.Show("数据库检查完毕");
            /// 回发消息体
            string msg = string.Empty;
            if (resultDs.Tables[0].Rows.Count == 1)
            {
                /// 回发登录成功消息
                /// 构造消息
                msg = MsgType.LOGIN_SUCCESS.ToString();
            }
            else
            {
                ///回发登录失败消息
                msg = MsgType.LOGIN_FAILED.ToString();
            }

            MessageBox.Show("回发消息：" + msg);
            /// 回发
            SendBackToClient(msg);
            MessageBox.Show("回发成功");
        }
        #endregion

        #region 获取消息类型
        /// <summary>
        /// 获取消息中的消息类型
        /// </summary>
        /// <returns></returns>
        public int GetMsgType()
        {
            int result = 0;

            try
            {
                /// 取得消息类型
                result = Int32.Parse(m_RecvMsgType);
            }
            catch (Exception ex)
            {
                throw new Exception("获取消息类型失败，原因：" + ex.Message);
            }

            return result;
        }
        #endregion


        #region 回发
        private void SendBackToClient(string msg)
        {
            MessageFormat mf = new MessageFormat(msg, m_MessageFormart.ipStr);
            m_SocketManager.OnSend(mf);
        }
        #endregion

    }
}
