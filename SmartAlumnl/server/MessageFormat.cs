using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    /// <summary>
    /// 格式处理类
    /// </summary>
    public class MessageFormat
    {
        public string msgStr { get; set; }
        public string ipStr { get; set; }

        public MessageFormat(){ }

        public MessageFormat(string msg, string ip)
        {
            msgStr = msg;
            ipStr = ip;
        }
    }
}
