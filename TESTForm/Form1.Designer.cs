﻿namespace TESTForm
{
    partial class Form1
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.Openbtn = new System.Windows.Forms.Button();
            this.Closebtn = new System.Windows.Forms.Button();
            this.asyncSocketTcpServer1 = new AsyncTcpServer.AsyncSocketTcpServer();
            this.SuspendLayout();
            // 
            // Openbtn
            // 
            this.Openbtn.Location = new System.Drawing.Point(29, 12);
            this.Openbtn.Name = "Openbtn";
            this.Openbtn.Size = new System.Drawing.Size(75, 23);
            this.Openbtn.TabIndex = 3;
            this.Openbtn.Text = "Open";
            this.Openbtn.UseVisualStyleBackColor = true;
            this.Openbtn.Click += new System.EventHandler(this.Openbtn_Click);
            // 
            // Closebtn
            // 
            this.Closebtn.Location = new System.Drawing.Point(151, 12);
            this.Closebtn.Name = "Closebtn";
            this.Closebtn.Size = new System.Drawing.Size(75, 23);
            this.Closebtn.TabIndex = 4;
            this.Closebtn.Text = "Close";
            this.Closebtn.UseVisualStyleBackColor = true;
            this.Closebtn.Click += new System.EventHandler(this.Closebtn_Click);
            // 
            // asyncSocketTcpServer1
            // 
            this.asyncSocketTcpServer1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.asyncSocketTcpServer1.IsRunning = false;
            this.asyncSocketTcpServer1.Location = new System.Drawing.Point(170, 110);
            this.asyncSocketTcpServer1.Name = "asyncSocketTcpServer1";
            this.asyncSocketTcpServer1.ServerAddress = ((System.Net.IPAddress)(resources.GetObject("asyncSocketTcpServer1.ServerAddress")));
            this.asyncSocketTcpServer1.ServerPort = 11000;
            this.asyncSocketTcpServer1.Size = new System.Drawing.Size(130, 17);
            this.asyncSocketTcpServer1.TabIndex = 2;
            this.asyncSocketTcpServer1.ClientConnected += new System.EventHandler<TcpServer.AsyncSocketServer.TcpServerClientConnectedEventArgs>(this.AsyncSocketTcpServer1_ClientConnected);
            this.asyncSocketTcpServer1.ReceiveData += new System.EventHandler<TcpServer.AsyncSocketServer.TcpServerReceiveDatadEventArgs>(this.AsyncSocketTcpServer1_ReceiveData);
            this.asyncSocketTcpServer1.ClientDisconnected += new System.EventHandler<TcpServer.AsyncSocketServer.TcpServerClientDisconnectedEventArgs>(this.AsyncSocketTcpServer1_ClientDisconnected);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(302, 129);
            this.Controls.Add(this.Closebtn);
            this.Controls.Add(this.Openbtn);
            this.Controls.Add(this.asyncSocketTcpServer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        /// <summary>
        /// 异步服务器
        /// </summary>
        private AsyncTcpServer.AsyncSocketTcpServer asyncSocketTcpServer1;
        private System.Windows.Forms.Button Openbtn;
        private System.Windows.Forms.Button Closebtn;
    }
}

