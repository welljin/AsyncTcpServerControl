namespace TESTForm
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
            this.Openbtn = new System.Windows.Forms.Button();
            this.Closebtn = new System.Windows.Forms.Button();
            this.asyncTcpServer1 = new AsyncTcpServer.AsyncTcpServer();
            this.SuspendLayout();
            // 
            // Openbtn
            // 
            this.Openbtn.Location = new System.Drawing.Point(364, 12);
            this.Openbtn.Name = "Openbtn";
            this.Openbtn.Size = new System.Drawing.Size(75, 23);
            this.Openbtn.TabIndex = 3;
            this.Openbtn.Text = "Open";
            this.Openbtn.UseVisualStyleBackColor = true;
            this.Openbtn.Click += new System.EventHandler(this.Openbtn_Click);
            // 
            // Closebtn
            // 
            this.Closebtn.Location = new System.Drawing.Point(364, 41);
            this.Closebtn.Name = "Closebtn";
            this.Closebtn.Size = new System.Drawing.Size(75, 23);
            this.Closebtn.TabIndex = 4;
            this.Closebtn.Text = "Close";
            this.Closebtn.UseVisualStyleBackColor = true;
            this.Closebtn.Click += new System.EventHandler(this.Closebtn_Click);
            // 
            // asyncTcpServer1
            // 
            this.asyncTcpServer1.IsRunning = false;
            this.asyncTcpServer1.Location = new System.Drawing.Point(325, 248);
            this.asyncTcpServer1.Name = "asyncTcpServer1";
            this.asyncTcpServer1.Size = new System.Drawing.Size(114, 18);
            this.asyncTcpServer1.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 278);
            this.Controls.Add(this.asyncTcpServer1);
            this.Controls.Add(this.Closebtn);
            this.Controls.Add(this.Openbtn);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button Openbtn;
        private System.Windows.Forms.Button Closebtn;
        private AsyncTcpServer.AsyncTcpServer asyncTcpServer1;
    }
}

