using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    class DispatchMessage
    {
        #region 属性

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

        #region 检查用户是否存在
        /// <summary>
        /// 检查用户是否可用
        /// </summary>
        /// <returns>存在，返回true、失败返回false</returns>
        public bool IsUserAvailable()
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
            m_Pwd = ss[2];
            m_RecvMsgType = ss[3];
            m_RecvMsgData = ss[4];


             /// 查询数据库中是否存在此用户与密码


            return true;
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


        #region 回发：用户名不存在。
        public void OnSendUserIsNotAvailable()
        {
            MessageFormat mf = new MessageFormat("1002", m_MessageFormart.ipStr);
            m_SocketManager.OnSend(mf);
        }
        #endregion

    }
}
