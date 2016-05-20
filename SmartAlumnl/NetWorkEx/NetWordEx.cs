using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Data;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;

namespace NetworkEx
{
    public class MyNetAPI
    {
        /// <summary>
        /// 从网络从接收 wdatlen个字节数据到buf，必须收满后才返回
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="buf"></param>
        /// <param name="wdatlen">期望接收数据的长度</param>
        /// <returns></returns>
        public static int Receive(NetworkStream ns, byte[] buf, int wdatlen)
        {
            int count = 0;
            int alen = 0;
            byte[] tmp = new byte[wdatlen];

            // 如果接收缓冲区不够长，直接返回
            if (buf.Length < wdatlen)
            {
                return -1;
            }

            while (count < wdatlen)
            {
                try
                {
                    alen = ns.Read(tmp, 0, tmp.Length);
                }
                // 网络断开异常
                catch (IOException ex)
                {
                    throw ex;
                }

                // 如果没有读到数据，继续读
                if (alen == 0)
                {
                    //throw new Exception("网络连接异常断开");
                    continue;
                }

                // 把每次收的加入到总的缓冲区
                Array.Copy(tmp, 0, buf, count, alen);
                count += alen;
            }

            return count;
        }

        /// <summary>
        /// 获取本机IP（IP4）
        /// </summary>
        /// <returns></returns>
        public static string GetHostIP4()
        {
            try
            {
                /// 获取主机名
                string strHostName = Dns.GetHostName();
                IPHostEntry IpEntry = Dns.GetHostEntry(strHostName);

                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {
                    /// 从IP地址列表中筛选出IP4
                    ///  addressFamily.InterNetWorkV6表示此地址为IP6类型
                    if (AddressFamily.InterNetwork == IpEntry.AddressList[i].AddressFamily)
                    {
                        return IpEntry.AddressList[i].ToString();
                    }
                }

                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}