using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TcpServer.Ini
{
    public class Iniconfig
    {
        private object lockobj = new object();

        #region API函数声明

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        #endregion

        /// <summary> 
        /// 写入INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        /// <param name="Value">值</param> 
        /// <param name="FilePath">位置</param> 
        public void IniWriteValue(string Section, string Key, string Value, string FilePath)
        {
            WritePrivateProfileString(Section, Key, Value, FilePath);
        }

        /// <summary> 
        /// 读出INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        /// <param name="FilePath">位置</param> 
        public string IniReadValue(string Section, string Key, string FilePath)
        {
            StringBuilder temp = new StringBuilder(500);
            int i = GetPrivateProfileString(Section, Key, "", temp, 500, FilePath);
            return temp.ToString();
        }

        /// <summary>
        /// 类型转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T GetValue<T>(string value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }


        /// <summary> 
        /// 验证文件是否存在 
        /// </summary> 
        /// <returns>布尔值</returns> 
        public bool ExistINIFile(string FilePath)
        {
            return File.Exists(FilePath);
        }

        /// <summary>
        /// 文件不存在则创建
        /// </summary>
        /// <param name="FilePath">路径</param>
        /// <param name="ServerIPAddress">服务器默认地址</param>
        /// <param name="ServerPort">服务器默认端口</param>
        public void CreateIniFile(string FileName, string ServerIPAddress, int ServerPort)
        {
            using (FileStream writestream = new FileStream(FileName, FileMode.Create, FileAccess.Write))
            {
                string text = "[Server]\r\n";
                text += "IPAddress=127.0.0.1\r\n";
                text += "Port=11000\r\n";
                byte[] bytes = Encoding.UTF8.GetBytes(text);
                writestream.Write(bytes,0, bytes.Length);
                writestream.Flush();
                writestream.Close();
            }
        }
    }
}
