using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using NetworkEx;

namespace smartAlumnl_client.Login
{
    public partial class login : System.Web.UI.Page
    {
        #region 属性定义

        /// <summary>
        /// 通信套接字
        /// </summary>
        private TcpClient client;

        /// <summary>
        ///  发送缓冲区1024字节。
        /// </summary>
        private const int SEND_BUF_LENG = 1024;

        /// <summary>
        ///  设置发送缓冲区大小
        /// </summary>
        private byte[] SEND_BUF = new byte[SEND_BUF_LENG];

        /// <summary>
        /// 服务器IP
        /// </summary>
        private string serverIP;

        /// <summary>
        /// 服务器端口
        /// </summary>
        private int serverPort;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            serverIP = "127.0.0.1";
            serverPort = 12345;
        }

        /// <summary>
        /// 登录按钮被单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Login_Click(object sender, EventArgs e)
        {
            try
            {
                /// 获取帐号
                string str_Account = tb_Account.Text;
                /// 获取密码
                string str_Password = tb_Password.Text;

                if (str_Account != string.Empty && str_Password != string.Empty)
                {
                    /// 请求登录，返回登录结果
                    int result = IsAvailable(str_Account, str_Password);
                    switch (result)
                    {
                        case MsgType.LOGIN_FAILED:
                            ClientScript.RegisterStartupScript(typeof(string), "print", "<script>alert('登录失败')</script>");
                            break;
                        case MsgType.LOGIN_SUCCESS:
                            ClientScript.RegisterStartupScript(typeof(string), "print", "<script>alert('登录成功')</script>");
                            break;
                        default:
                            break;
                    }

                    /// 接收服务器返回消息
                    
                }
                else
                {
                    ClientScript.RegisterStartupScript(typeof(string), "print", "<script>alert('用户名或密码不能为空')</script>");
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(typeof(string), "print", "<script>alert('" + ex.Message + "')</script>");
            }
        }

        #region 接收服务器登录的反馈消息
        /// <summary>
        /// 接收服务器登录的反馈消息
        /// </summary>
        /// <returns>成功登录返回true,失败返回false</returns>
        private bool RecvFromSever()
        {
            byte []recvBuf = new byte[8];
            client.GetStream().Read(recvBuf, 0, recvBuf.Length);

            int result =Int32.Parse( Encoding.Unicode.GetString(recvBuf));
            bool login_Status = false;
            switch (result)
            {
                case MsgType.LOGIN_FAILED:
                    login_Status = false;
                    break;
                case MsgType.LOGIN_SUCCESS:
                    login_Status = true;
                    break;
                default:
                    break;
            }
            return login_Status;
        }
        #endregion

        #region 请求结果
        private int IsAvailable(string accout, string pwd)
        {
            /// 请求消息格式：|用户名|密码|消息类型|
            string msg = "|" + accout + "|" + pwd+"|"+"1001|";

            /// 将发送消息编码成二进制
            byte[] msgarr = Encoding.Unicode.GetBytes(msg);

            /// 获取消息长度
            int msglen = msgarr.Length;

            byte[] lenarr = BitConverter.GetBytes(msglen);

            /// 拷贝int
            Array.Copy(lenarr, SEND_BUF, lenarr.Length);

            /// 拷贝 msg
            Array.Copy(msgarr, 0, SEND_BUF,  4, msgarr.Length);


            /// 创建通信套接字
            client = new TcpClient();
            /// 连接服务器
            client.Connect(serverIP, serverPort);

            /// 发送和接收超时都为60秒
            //client.SendTimeout = 60 * 1000;
            //client.ReceiveTimeout = 60 * 1000;

             /// 发送数据
            NetworkStream ns = client.GetStream();
            try
            {
                lock (ns)
                {
                    ns.Write(SEND_BUF, 0, lenarr.Length + msgarr.Length);
                    ns.Flush();
                }
            }
            catch (Exception ex)
            {
                ns.Dispose();
                ns.Close();
                client.Close();
                ClientScript.RegisterStartupScript(typeof(string), "print", "<script>alert('向服务器请求失败，原因：+" + ex.Message + "')</script>");
                ///throw;
            }
            
            /// 接收服务器返回消息
            byte[] recvBuf = new byte[8];
            int login_Status = MyNetAPI.Receive(ns, recvBuf, 8);

            /// 断开连接
            ns.Dispose();
            ns.Close();
            client.Close();

            /// 解析消息中返回的登录状态
            login_Status = Int32.Parse(Encoding.Unicode.GetString(recvBuf));

            return login_Status;
        }
        #endregion
    }
}