using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
namespace QQ
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        private bool isMouseDown = false;
        private Point FormLocation;    //form的location       
        private Point mouseOffset;      //鼠标的按下位置
        public static Form1 f1;
        public static Form3 f3;
        //public static bool isExit = false;
        //public static TcpClient client;
        //public static BinaryReader br;
        //public static BinaryWriter bw;
        private void Form3_Load(object sender, EventArgs e)
        {
          //对话对象名的显示
            label1.Text = Form2.duixiang;
            //
            //获取网络流
            NetworkStream networkStream = Form1.client.GetStream();
            //将网络流作为二进制读写对象
            Form1.br = new BinaryReader(networkStream);
            Form1.bw = new BinaryWriter(networkStream);
            Thread threadReceive = new Thread(new ThreadStart(ReceiveData));
            threadReceive.IsBackground = true;
            threadReceive.Start();
        }
        //鼠标拖动
        private void Form3_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = true;
                FormLocation = this.Location;
                mouseOffset = Control.MousePosition;
            }
        }
        private void Form3_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }
        private void Form3_MouseMove(object sender, MouseEventArgs e)
        {
            int _x = 0; int _y = 0;
            if (isMouseDown)
            {
                Point pt = Control.MousePosition;
                _x = mouseOffset.X - pt.X;
                _y = mouseOffset.Y - pt.Y;
                this.Location = new Point(FormLocation.X - _x, FormLocation.Y - _y);
            }
        }
        //最小化按钮
        private void button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        //关闭按钮
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //发送按钮
        private void button3_Click(object sender, EventArgs e)
        {
            Form1.SendMessage("Talk," + label1.Text + "," + richTextBox2.Text + "\r\n");
            richTextBox2.Clear();
        }
        /// <summary>
        /// 在发送信息的文本框中按下【Enter】键触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void richtextbox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                //触发【发送】按钮的单击事件
               button3.PerformClick();
            }
        }
         /// <summary>
        /// 处理服务器信息
        /// </summary>
        public static void ReceiveData()
        {
            string receiveString = null;
            while (Form1.isExit == false)
            {
                try
                {
                    //从网络流中读出字符串
                    //此方法会自动判断字符串长度前缀，并根据长度前缀读出字符串
                    receiveString = Form1.br.ReadString();
                }
                catch
                {
                    if (Form1.isExit == false)
                    {
                        MessageBox.Show("与服务器失去连接");
                    }
                    break;
                }
                string[] splitString = receiveString.Split(',');
                string command = splitString[0].ToLower();
                switch (command)
                {
                    //case "login":   //格式： login,用户名
                    //    AddOnline(splitString[1]);
                    //    break;
                    //case "logout":  //格式： logout,用户名
                    //    RemoveUserName(splitString[1]);
                    //    break;
                    case "talk":    //格式： talk,用户名,对话信息
                        AddTalkMessage(splitString[1] + "：\r\n");
                        AddTalkMessage(receiveString.Substring(splitString[0].Length + splitString[1].Length + 2));
                        break;
                    default:
                        AddTalkMessage("什么意思啊：" + receiveString);
                        break;
                }
            }
            Application.Exit();
        }
        private delegate void AddTalkMessageDelegate(string message);
        ///// <summary>
        ///// 在聊天对话框（Form3.richTextBox1）中追加聊天信息
        ///// </summary>
        ///// <param name="message"></param>
        public static void AddTalkMessage(string message)
        {
            if (f3.richTextBox1.InvokeRequired)
            {
                AddTalkMessageDelegate d = new AddTalkMessageDelegate(AddTalkMessage);
                f3.richTextBox1.Invoke(d, new object[] { message });
            }
            else
            {
                f3.richTextBox1.AppendText(message + Environment.NewLine);
                f3.richTextBox1.ScrollToCaret();
            }
        //    //f3.richTextBox1.AppendText(message);
        }
    }
}
