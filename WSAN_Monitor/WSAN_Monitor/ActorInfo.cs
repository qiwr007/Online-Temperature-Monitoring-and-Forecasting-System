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
    public partial class ActorInfo : Form
    {
        public ActorInfo()
        {
            InitializeComponent();
        }
        private static string AddCourConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
        public static SqlConnection conn = new SqlConnection(AddCourConnStr);
        BindingSource bing = new BindingSource();
        private void ActorInfo_Load(object sender, EventArgs e)
        {
            DataTable temptable1 = SQLOperation.QueryActorInfo("");
            bing.DataSource = temptable1;
            bing.Filter = "";
            dataGridView1.DataSource = bing;
            // dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            change_name();
        }
        private void change_name()
        {
            dataGridView1.Columns[0].HeaderText = "序号";
            dataGridView1.Columns[1].HeaderText = "记录编号";
            dataGridView1.Columns[2].HeaderText = "节点号";
            dataGridView1.Columns[3].HeaderText = "节点状态";
            dataGridView1.Columns[4].HeaderText = "记录时间";
            dataGridView1.Columns[5].HeaderText = "备注";
        }

        private void Refreshing()
        {
            DataTable temptable1 = SQLOperation.QueryActorInfo("");
            bing.DataSource = temptable1;
            bing.Filter = "";
            dataGridView1.DataSource = bing;
        }

        private void button1_Click(object sender, EventArgs e)//查询
        {
            if (radioButton1.Checked)
            {
                if (comboBox1.SelectedIndex == 0)
                    bing.Filter = "states = 0";
                else
                    bing.Filter = "states = 1";
            }
            else if (radioButton2.Checked)
            {
                bing.Filter = "Wdate = '" + WD.Value.ToShortDateString() + "'";
            }
            else
            {
                bing.Filter = "";
            }
            dataGridView1.DataSource = bing;
        }

        private void button2_Click(object sender, EventArgs e)//全部信息
        {
            Refreshing();
        }

        private void button3_Click(object sender, EventArgs e)//删除信息
        {
            string Iname = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            if (MessageBox.Show("确定删除该条信息吗？", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                int i = SQLOperation.Delete_Info(Iname);
                MessageBox.Show("成功删除" + i + "条信息!");
                Refreshing();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)//生成报表
        {
            Actor_Report rv = new Actor_Report();
            rv.Show();
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                int j = i + 1;
                dataGridView1.Rows[i].Cells[0].Value = i + 1;
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)//格式化动作状态
        {
            if (e == null || e.Value == null || !(sender is DataGridView))
            {
                return;
            }
            DataGridView dgv = (DataGridView)sender;
            object originalValue = e.Value;
            if (dgv.Columns[e.ColumnIndex].DataPropertyName == "NodeId")
            {
                //e.Value = ((int)originalValue == 1) ? "缴费" : "退费";
                e.Value =  "执行器节点";
            }
            if (dgv.Columns[e.ColumnIndex].DataPropertyName == "states")
            {
                e.Value = ((int)originalValue == 1) ? "执行器动作" : "执行器未动作";
            }
        }
    }
}
