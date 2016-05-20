using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetworkEx;

namespace configTool
{
    public partial class configInfo : Form
    {
        public configInfo()
        {
            InitializeComponent();
        }

        private int m_Port = 12345;

        public int Port
        {
            get { return m_Port; }
            set { m_Port = value; }
        }

        private void configInfo_Load(object sender, EventArgs e)
        {
            /// 获取本机IP4，并显示
            lbl_ServerIP.Text += MyNetAPI.GetHostIP4();
            lbl_ServerPort.Text += m_Port.ToString();
        }
        
    }
}
