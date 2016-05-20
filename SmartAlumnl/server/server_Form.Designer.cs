namespace server
{
    partial class server_Form
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lbl_ServerIP = new System.Windows.Forms.Label();
            this.lbl_Port = new System.Windows.Forms.Label();
            this.lbl_ServerStatus = new System.Windows.Forms.Label();
            this.lb_Msg = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lbl_ServerIP
            // 
            this.lbl_ServerIP.AutoSize = true;
            this.lbl_ServerIP.Location = new System.Drawing.Point(87, 9);
            this.lbl_ServerIP.Name = "lbl_ServerIP";
            this.lbl_ServerIP.Size = new System.Drawing.Size(65, 12);
            this.lbl_ServerIP.TabIndex = 0;
            this.lbl_ServerIP.Text = "服务器IP：";
            // 
            // lbl_Port
            // 
            this.lbl_Port.AutoSize = true;
            this.lbl_Port.Location = new System.Drawing.Point(87, 49);
            this.lbl_Port.Name = "lbl_Port";
            this.lbl_Port.Size = new System.Drawing.Size(35, 12);
            this.lbl_Port.TabIndex = 1;
            this.lbl_Port.Text = "端口:";
            // 
            // lbl_ServerStatus
            // 
            this.lbl_ServerStatus.AutoSize = true;
            this.lbl_ServerStatus.Location = new System.Drawing.Point(197, 49);
            this.lbl_ServerStatus.Name = "lbl_ServerStatus";
            this.lbl_ServerStatus.Size = new System.Drawing.Size(71, 12);
            this.lbl_ServerStatus.TabIndex = 2;
            this.lbl_ServerStatus.Text = "服务器状态:";
            // 
            // lb_Msg
            // 
            this.lb_Msg.FormattingEnabled = true;
            this.lb_Msg.ItemHeight = 12;
            this.lb_Msg.Location = new System.Drawing.Point(12, 79);
            this.lb_Msg.Name = "lb_Msg";
            this.lb_Msg.ScrollAlwaysVisible = true;
            this.lb_Msg.Size = new System.Drawing.Size(457, 244);
            this.lb_Msg.TabIndex = 3;
            // 
            // server_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(481, 331);
            this.Controls.Add(this.lb_Msg);
            this.Controls.Add(this.lbl_ServerStatus);
            this.Controls.Add(this.lbl_Port);
            this.Controls.Add(this.lbl_ServerIP);
            this.MaximizeBox = false;
            this.Name = "server_Form";
            this.Text = "服务端";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_ServerIP;
        private System.Windows.Forms.Label lbl_Port;
        private System.Windows.Forms.Label lbl_ServerStatus;
        private System.Windows.Forms.ListBox lb_Msg;
    }
}

