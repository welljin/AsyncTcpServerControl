using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TcpServer.Ini;

namespace AsyncTcpServer
{
    public partial class Setting : Form
    {
        Point mouse_offset;
        private Iniconfig iniConfig;
        public static Setting settingForm = null;
        public readonly string inipath = "";

        public Setting()
        {
            InitializeComponent();
            iniConfig = new Iniconfig();
            inipath = AsyncTcpServer.Path;
        }

        public static void ShowForm()
        {
            if ((settingForm == null) || (settingForm.IsDisposed))
            {
                settingForm = new Setting();
                settingForm.Show();
            }
            else
            {
                settingForm.WindowState = FormWindowState.Normal;
                settingForm.Activate();
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mouse_offset = new Point(-e.X, -e.Y);
        }

        private void Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePos = Control.MousePosition;
                mousePos.Offset(mouse_offset.X, mouse_offset.Y);
                Location = mousePos;
            }
        }

        private void Setting_Load(object sender, EventArgs e)
        {
            try
            {
                this.IPAddresstxt.Text = iniConfig.IniReadValue("Server", "IPAddress", inipath);
                this.Porttxt.Text = iniConfig.IniReadValue("Server", "Port", inipath);
            }
            catch
            {

            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                IPAddress ip = IPAddress.Parse(this.IPAddresstxt.Text);
            }
            catch
            {
                MessageBox.Show("IP地址格式不正确，请重新输入");
                return;
            }

            try
            {
                int port = int.Parse(this.Porttxt.Text);
            }
            catch
            {
                MessageBox.Show("端口地址格式不正确，请重新输入");
                return;
            }

            if (!iniConfig.ExistINIFile(inipath))//不存在
            {
                iniConfig.CreateIniFile(inipath, this.IPAddresstxt.Text, int.Parse(this.Porttxt.Text));
            }
            else
            {
                iniConfig.IniWriteValue("Server","IPAddress", this.IPAddresstxt.Text, inipath);
                iniConfig.IniWriteValue("Server", "Port", this.Porttxt.Text, inipath);
                if(MessageBox.Show("配置修改成功，重启生效","配置",MessageBoxButtons.OK,MessageBoxIcon.Information)==DialogResult.OK)
                {
                    Close();
                }
            }
        }
    }
}
