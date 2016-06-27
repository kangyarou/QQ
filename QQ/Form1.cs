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
    public partial class Form1 : Form
    {
        public static  string ServerIP; //IP
        public static  int port;   //端口
        public static  bool isExit = false;
        public static  TcpClient client;
        public static  BinaryReader br;
        public static  BinaryWriter bw;
        public static Form3 f3;
        public Form1()
        {
            InitializeComponent();
            SetServerIPAndPort();
        }      
        /// <summary>
        /// 根据当前程序目录的文本文件‘ServerIPAndPort.txt’内容来设定IP和端口
        /// 格式：127.0.0.1:8885
        /// </summary>
        private void SetServerIPAndPort()
        {
            try
            {
                FileStream fs = new FileStream("ServerIPAndPort.txt", FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                string IPAndPort = sr.ReadLine();//用户IP
                ServerIP = IPAndPort.Split(':')[0]; //设定IP
                port = int.Parse(IPAndPort.Split(':')[1]); //设定端口
                sr.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("配置IP与端口失败，错误原因：" + ex.Message);
                Application.Exit();
            }
        }
        //发送消息
        public static void SendMessage(string message)
        {
            try
            {
                //将字符串写入网络流，此方法会自动附加字符串长度前缀
                bw.Write(message);
                bw.Flush();
            }
            catch
            {
                MessageBox.Show("发送失败");
            }
        }
        /// <summary>
        /// 处理服务器信息
        /// </summary>
        public static void ReceiveData()
        {
            string receiveString = null;
            while (isExit == false)
            {
                try
                {
                    //从网络流中读出字符串
                    //此方法会自动判断字符串长度前缀，并根据长度前缀读出字符串
                    receiveString = br.ReadString();
                }
                catch
                {
                    if (isExit == false)
                    {
                        MessageBox.Show("与服务器失去连接");
                    }
                    break;
                }
        //        string[] splitString = receiveString.Split(',');
        //        string command = splitString[0].ToLower();
        //        switch (command)
        //        {
        ////            //case "login":   //格式： login,用户名
        ////            //    AddOnline(splitString[1]);
        ////            //    break;
        ////            //case "logout":  //格式： logout,用户名
        ////            //    RemoveUserName(splitString[1]);
        ////            //    break;
        //            case "talk":    //格式： talk,用户名,对话信息
        //                AddTalkMessage(splitString[1] + "：\r\n");
        //                AddTalkMessage(receiveString.Substring(splitString[0].Length + splitString[1].Length + 2));
        //                break;
        //            default:
        //                AddTalkMessage("什么意思啊：" + receiveString);
        //                break;
        //        }
            }
            Application.Exit();
        }
        //private delegate void AddTalkMessageDelegate(string message);
        ///// <summary>
        ///// 在聊天对话框（Form3.richTextBox1）中追加聊天信息
        ///// </summary>
        ///// <param name="message"></param>
        //private static void AddTalkMessage(string message)
        //{
        //    //    if (f3.richTextBox1.InvokeRequired)
        //    //    {
        //    //        AddTalkMessageDelegate d = new AddTalkMessageDelegate(AddTalkMessage);
        //    //        f3.richTextBox1.Invoke(d, new object[] { message });
        //    //    }
        //    //    else
        //    //    {
        //    //        f3.richTextBox1.AppendText(message + Environment.NewLine);
        //    //        f3.richTextBox1.ScrollToCaret();
        //    //    }
        //    // f3.richTextBox1.AppendText(message);
        //}
       /// <summary>
       //按钮与界面的设置
       /// </summary>
        public static string yonghu;
        private bool isMouseDown = false; 
        private Point FormLocation;    //form的location       
        private Point mouseOffset;      //鼠标的按下位置
        private void Form1_Load(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '*';
        }
        //鼠标拖动事件
        private void Form1_MouseDown(object sender, MouseEventArgs e) 
        { 
            if (e.Button == MouseButtons.Left) 
            { 
                isMouseDown = true;
                FormLocation = this.Location; 
                mouseOffset = Control.MousePosition;
            }
        }        
        private void Form1_MouseUp(object sender, MouseEventArgs e) 
        { 
            isMouseDown = false; 
        }       
        private void Form1_MouseMove(object sender, MouseEventArgs e) 
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
       //登陆按钮
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            try
            {
                //此处为方便演示，实际使用时要将Dns.GetHostName()改为服务器域名
                //IPAddress ipAd = IPAddress.Parse("182.150.193.7");
                client = new TcpClient();
                client.Connect(IPAddress.Parse(ServerIP), port);
            }
            catch (Exception)
            {
                MessageBox.Show("无法连接到服务器");
                button1.Enabled = true;
                return;
            }
            //获取网络流
            NetworkStream networkStream = client.GetStream();
            //将网络流作为二进制读写对象
            br = new BinaryReader(networkStream);
            bw = new BinaryWriter(networkStream);
            SendMessage("Login," + yonghu);
            Thread threadReceive = new Thread(new ThreadStart(ReceiveData));
            threadReceive.IsBackground = true;
            threadReceive.Start();
        new Thread(() => Application.Run(new Form2())).Start();
        this.Close();//应放到try里
      }
       //最小化按钮
       private void button2_Click(object sender, EventArgs e)
    {
          this.WindowState = FormWindowState.Minimized;
    }
      //关闭按钮
       private void button3_Click(object sender, EventArgs e)
   {
          this.Close();
    }
    //不同用户的获取
       private void textBox1_TextChanged(object sender, EventArgs e)
       {
           yonghu = textBox1.Text;
       }
    }
}
