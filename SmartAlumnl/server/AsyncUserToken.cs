using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    public class AsyncUserToken
    {
        public Socket UserSocket { get; set; }
        public String ID { get; set; }
    }
}
