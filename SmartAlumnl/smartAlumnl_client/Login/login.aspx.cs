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

        #region 登录验证
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

                    ///// 学号 = 密码
                    //string queryStr = "SELECT * FROM SA_Login where SNo = '"+str_Account+"' and SNo = '"+str_Password+"'";

                    //DataSet resultDs = AccessHelper.dataSet(queryStr);
                    //if (resultDs.Tables[0].Rows.Count > 0)
                    //{
                    //    /// 提示
                    //    ClientScript.RegisterStartupScript(typeof(string), "print", "<script>alert('登录成功')</script>");
                    //}
                    //else
                    //{
                    //    /// 提示
                    //    ClientScript.RegisterStartupScript(typeof(string), "print", "<script>alert('用户名或者密码错误，请重新输入')</script>");

                    //    /// 清空输入
                    //    tb_Account.Text = "";
                    //    tb_Password.Text = "";
                    //}
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

        #region 请求结果
        private int IsAvailable(string accout, string pwd)
        {
            /// 请求消息格式：|用户名|密码|消息类型|
            string msg = "|" + accout + "|" + pwd+"|"+MsgType.LOGIN.ToString()+"|";
            
            /// 将发送消息编码成二进制
            byte[] msgByte = Encoding.Unicode.GetBytes(msg);

            /// 获取消息长度
            int msgByteLength = msgByte.Length;

            byte[] lengthArr = BitConverter.GetBytes(msgByteLength);

            /// 拷贝int
            Array.Copy(lengthArr, SEND_BUF, lengthArr.Length);

            /// 拷贝 msgByte 
            Array.Copy(msgByte, 0, SEND_BUF, 4, msgByte.Length);


            /// 创建通信套接字
            TcpClient client = new TcpClient();
            /// 连接服务器
            client.Connect(serverIP, serverPort);

            /// 发送和接收超时都为60秒
            client.SendTimeout = 60 * 1000;
            client.ReceiveTimeout = 60 * 1000;

             /// 发送数据
                NetworkStream ns = client.GetStream();
            try
            {
                lock (ns)
                {
                     ns.Write(SEND_BUF, 0, lengthArr.Length + msgByte.Length);
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
            byte[] recvBuf = new byte[4];
            int login_Status = MyNetAPI.Receive(ns, recvBuf, 4);

            /// 断开连接
            ns.Dispose();
            ns.Close();
            client.Close();

            /// 解析消息中返回的登录状态
            login_Status = BitConverter.ToInt32(recvBuf, 0);

            return login_Status;
        }
        #endregion
        #endregion
    }
}