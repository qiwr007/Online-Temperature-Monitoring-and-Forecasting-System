using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WSAN_Monitor
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        private static string Str = System.Configuration.ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
        private static SqlConnection AddCourConn = new SqlConnection(Str);
        private string AddCourSql;
        private SqlDataAdapter AddCourDA;
        private DataSet AddCourseset = new DataSet();
        public static  string username = "";
      static  string s1, s2;
        private void Form1_Load(object sender, EventArgs e)
        {
            label4.Text = this.YanZM();
            txtuser.Text = username;
            //Console.WriteLine(DateTime.Now.AddMonths(-2).ToShortDateString());
        }
       
        public string YanZM()
        {
            Random r = new Random();
            int m = r.Next(100000, 999999);
            string ssss = m.ToString();
            return ssss;
        }

        private void btn_login_Click(object sender, EventArgs e)//登录按钮
        {
            tb_UserInfo User = new tb_UserInfo();
            User.Lname1 = txtuser.Text.ToString().Trim();
            User.Lpass1 = txtpass.Text.ToString().Trim();
            if (txtuser.Text == "" || txtpass.Text == "")
            {
                MessageBox.Show("用户名或密码不能为空！");
                if (txtuser.Text == "")
                {
                    txtuser.Focus();
                }
                else if (txtpass.Text == "")
                {
                    txtpass.Focus();
                }
                return;
            }
            else if (txtyzm.Text == "")
            {
                MessageBox.Show("验证码不能为空");
                txtyzm.Focus();
                return;
            }
            else if (txtyzm.Text != label4.Text)
            {
                MessageBox.Show("验证码输入有误");
                txtyzm.Focus();
                txtyzm.Clear();
                label4.Text = YanZM();
                return;
            }
           // string sql = "select * from Wm_Login where Lname=@userId and Lpass=@userpassword";
            //DataRow row = Methods.GetRow(sql, new SqlParameter("@userId", txtuser.Text), new SqlParameter("@userpassword", txtpass.Text));
            int flag = SQLOperation.ExistCount(User);
            if (flag>0)
            {
              //  MessageBox.Show("登录成功!\t"+flag.ToString());
                Parameter_Query.username = this.txtuser.Text;
                Parameter_Query.userflag = flag;
                PassEdit.passs = this.txtpass.Text;
                Parameter_Query PQ = new Parameter_Query();
                PQ.Show();
                this.Hide();
                return;
            }
            else
            {
                MessageBox.Show("不存在此用户!请重新登录!");
                txtuser.Text = "";
                txtpass.Text = "";
                txtyzm.Text = "";
                label4.Text = this.YanZM();
                txtuser.Focus();
                return;
            }
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            label4.Text = this.YanZM();
        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

    }
}
