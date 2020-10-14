using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WSAN_Monitor
{
    public partial class PassEdit : Form
    {
        public PassEdit()
        {
            InitializeComponent();
        }
        public static String name = "";
        public static string passs = "";
        public static int flag;
        tb_UserInfo user = new tb_UserInfo();
        private void button1_Click(object sender, EventArgs e)//确认修改
        {
            Login.username = name;
            if (txtpassold.Text != passs)
            {
                MessageBox.Show("原密码输入错误!");
                return;
            }
            else if (txtpassnew.Text != txtpassneww.Text)
            {
                MessageBox.Show("两次密码输入不正确!");
                return;
            }
            user.Lname1 = txtuser1.Text;
            user.Lpass1 = txtpassnew.Text.ToString().Trim();
            int i = SQLOperation.UpdatePwd(user);
            if (i > 0)
            {
                MessageBox.Show("成功修改密码!");
                this.Hide();
                Login FM = new Login();
                FM.Show();
            }
            else
            {
                MessageBox.Show("修改失败!");
            }
        }

        private void PassEdit_Load(object sender, EventArgs e)
        {
            txtuser1.Text = name;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Parameter_Query.username = name;
            Parameter_Query.userflag = flag;
            Parameter_Query pq = new Parameter_Query();
            pq.Show();
        }
    }
}
