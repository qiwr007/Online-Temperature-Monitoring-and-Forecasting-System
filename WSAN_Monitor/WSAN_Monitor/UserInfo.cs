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
    public partial class UserInfo : Form
    {
        public UserInfo()
        {
            InitializeComponent();
        }
        private static string AddCourConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
        public static SqlConnection conn = new SqlConnection(AddCourConnStr);
        private SqlDataAdapter AddCourDA;
        private DataSet AddCourseset = new DataSet();
        private DataSet AddCourseset1 = new DataSet();
        private string FPsql = "select * from Wm_Login ";
        private bool op = false;
        BindingSource bing = new BindingSource();
        private void UserInfo_Load(object sender, EventArgs e)
        {
            this.AddCourDA = new SqlDataAdapter(this.FPsql, AddCourConnStr);
            AddCourseset.Clear();
            this.AddCourDA.Fill(AddCourseset, "Wm_Login");
            bing.DataSource = AddCourseset.Tables[0];
            bing.Filter = "";
            dataGridView1.DataSource = bing;
            dataGridView1.AllowUserToAddRows = false;
            Fill();
        }
        private void Fill()
        {
            txtlname.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            txtpass.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            int flag = int.Parse(dataGridView1.CurrentRow.Cells[2].Value.ToString());
            comboBox1.Text = comboBox1.Items[flag - 1].ToString();
        }
        private void Refreshing()
        {
            this.AddCourDA = new SqlDataAdapter(this.FPsql, AddCourConnStr);
            AddCourseset.Clear();
            this.AddCourDA.Fill(AddCourseset, "Wm_Login");
            dataGridView1.DataSource = AddCourseset.Tables[0];
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Fill();
        }

        private void button1_Click(object sender, EventArgs e)//添加用户
        {
            groupBox2.Enabled = true;
            txtlname.Text = "";
            txtpass.Text = "";
            comboBox1.SelectedIndex = 0;
            op = false;
        }

        private void button2_Click(object sender, EventArgs e)//修改
        {
            op = true;
            groupBox2.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)//保存
        {
            int flag = comboBox1.SelectedIndex+1;
            while (!op)
            {
                if (txtlname.Text == "")
                {
                    return;
                }
                else if (txtpass.Text == "")
                {
                    return;
                }
                else
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand();
                        conn.Open();
                        cmd.Connection = conn;
                        string sql = "insert into Wm_Login values('" + txtlname.Text.Trim() + "','" + txtpass.Text.Trim() + "'," + flag + ",'" + comboBox1.Text.Trim() + "')";
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("添加成功!");
                    }
                    catch
                    {
                        MessageBox.Show("添加失败!");
                    }
                    finally
                    {
                        op = true;
                        conn.Close();
                        Fill();
                        Refreshing();
                        groupBox2.Enabled = false;
                    }
                }
            }
            while (op)
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    conn.Open();
                    cmd.Connection = conn;
                    string s1 = dataGridView1.CurrentRow.Cells[0].Value.ToString().Trim();
                    String sql = "update    Wm_Login  set  Lname='" + txtlname.Text + "',Lpass='" + txtpass.Text + "',Llimit=" + flag+ ",Lremarks='" + comboBox1.Text + "' where Lname='" + s1 + "'";
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("修改成功!");
                }
                catch
                {
                    MessageBox.Show("修改失败!");
                }
                finally
                {
                    op = false;
                    conn.Close();
                    Refreshing();
                    Fill();
                    groupBox2.Enabled = false;
                }
                return;
            }
        }

        private void button4_Click(object sender, EventArgs e)//删除用户
        {
            string Lname = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            if (MessageBox.Show("确定删除该用户吗？", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                int i = SQLOperation.Delete_User(Lname);
                MessageBox.Show("成功删除" + i + "条信息!");
                Refreshing();
            }
        }

        private void button5_Click(object sender, EventArgs e)//查询
        {
             bing.Filter = "Lname   like  '%" + txtlname1.Text + "%'";
            dataGridView1.DataSource = bing;
        }

        private void txtlname1_TextChanged(object sender, EventArgs e)
        {
            bing.Filter = "Lname  like  '%" + txtlname1.Text + "%'";
            dataGridView1.DataSource = bing;
        }
    }
}
