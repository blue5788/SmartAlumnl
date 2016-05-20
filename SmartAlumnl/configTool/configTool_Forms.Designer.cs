namespace configTool
{
    partial class configInfo
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
            this.lbl_ServerPort = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbl_ServerIP
            // 
            this.lbl_ServerIP.AutoSize = true;
            this.lbl_ServerIP.Location = new System.Drawing.Point(40, 25);
            this.lbl_ServerIP.Name = "lbl_ServerIP";
            this.lbl_ServerIP.Size = new System.Drawing.Size(65, 12);
            this.lbl_ServerIP.TabIndex = 0;
            this.lbl_ServerIP.Text = "服务器IP：";
            // 
            // lbl_ServerPort
            // 
            this.lbl_ServerPort.AutoSize = true;
            this.lbl_ServerPort.Location = new System.Drawing.Point(40, 71);
            this.lbl_ServerPort.Name = "lbl_ServerPort";
            this.lbl_ServerPort.Size = new System.Drawing.Size(71, 12);
            this.lbl_ServerPort.TabIndex = 1;
            this.lbl_ServerPort.Text = "服务器端口:";
            // 
            // configInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(213, 116);
            this.Controls.Add(this.lbl_ServerPort);
            this.Controls.Add(this.lbl_ServerIP);
            this.MaximizeBox = false;
            this.Name = "configInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "配置信息";
            this.Load += new System.EventHandler(this.configInfo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_ServerIP;
        private System.Windows.Forms.Label lbl_ServerPort;
    }
}

