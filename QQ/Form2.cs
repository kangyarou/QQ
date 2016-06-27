using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
namespace QQ
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        public static string duixiang;//聊天对象
        private bool isMouseDown = false;
        private Point FormLocation;    //form的location       
        private Point mouseOffset;      //鼠标的按下位置
        private void Form2_Load(object sender, EventArgs e)
        {
            //用户名的显示
            label1.Text = Form1.yonghu;
        }
        //对话对象的获取并打开对话框
        private void treeview_AfterSelect(object sender, TreeViewEventArgs e)
        {
          duixiang = e.Node.Text ;
          new Thread(() => Application.Run(new Form3())).Start();
        }
        //鼠标拖动
        private void Form2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = true;
                FormLocation = this.Location;
                mouseOffset = Control.MousePosition;
            }
        }
        private void Form2_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }
        private void Form2_MouseMove(object sender, MouseEventArgs e)
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
        //邮箱链接
        private void lbllink_LinkClicked(object sender,LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("iexplore.exe", "http://www.1397512019.qq.com");
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
        //群组的交替显示
        private void panel2_Click(object sender, EventArgs e)
        {
            treeView1.Visible = true;
            treeView2.Visible = false;
            treeView3.Visible = false;
        }
        private void panel3_Click(object sender, EventArgs e)
        {
            treeView2.Visible = true;
            treeView1.Visible = false;
            treeView3.Visible = false;
        }
        private void panel4_Click(object sender, EventArgs e)
        {
            treeView3.Visible = true;
            treeView1.Visible = false;
            treeView2.Visible = false;
        }

  }
}
