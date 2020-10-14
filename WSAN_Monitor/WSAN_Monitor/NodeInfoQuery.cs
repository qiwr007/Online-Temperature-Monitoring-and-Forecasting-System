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
    public partial class NodeInfoQuery : Form
    {
        private static string AddCourConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
        public static SqlConnection conn = new SqlConnection(AddCourConnStr);
        BindingSource bing = new BindingSource();
        public NodeInfoQuery()
        {
            InitializeComponent();
        }

        private void NodeInfoQuery_Load(object sender, EventArgs e)
        {
            DataTable temptable1 = SQLOperation.QuerySensorInfo("");
             bing.DataSource=temptable1;
            bing.Filter="";
            dataGridView1.DataSource=bing;
            DataTable temptable2 = SQLOperation.QuerySensorInfo("NodeId");
            txtnodeid.DataSource=temptable2;
            txtnodeid.DisplayMember="NodeId";
            txtnodeid.ValueMember="NodeId";
           // dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToAddRows = false;
            //dataGridView1.Columns[0].Visible = false;
            change_name();
        }
        private void change_name()
        {
            dataGridView1.Columns[0].HeaderText = "序号";
            dataGridView1.Columns[1].HeaderText = "记录编号";
            dataGridView1.Columns[2].HeaderText = "温度";
            dataGridView1.Columns[3].HeaderText = "湿度";
            dataGridView1.Columns[4].HeaderText = "终端号";
            dataGridView1.Columns[5].HeaderText = "记录时间";
            dataGridView1.Columns[6].HeaderText = "备注";

        }

        private void Refreshing()
        {
            DataTable temptable1 = SQLOperation.QuerySensorInfo("");
            bing.DataSource = temptable1;
            bing.Filter = "";
            dataGridView1.DataSource = bing;
        }
        private void button1_Click(object sender, EventArgs e)//查询
        {
           int nodeid;
            if(radioButton1.Checked)
            {
                nodeid=int.Parse(txtnodeid.Text.ToString().Trim());
                bing.Filter="NodeId = "+nodeid+"";
            }
            else if(radioButton2.Checked)
            {
                bing.Filter="Wdate = '"+WD.Value.ToShortDateString()+"'";
            }
            else 
            {
                bing.Filter="";
            }
            dataGridView1.DataSource=bing;
        }

        private void button2_Click(object sender, EventArgs e)//全部信息
        {
             bing.Filter="";
            dataGridView1.DataSource=bing;
        }

        private void button3_Click(object sender, EventArgs e)//删除
        {
            string Iname =dataGridView1.CurrentRow.Cells[1].Value.ToString();
            if(MessageBox.Show("确定删除该条信息吗？","提示",MessageBoxButtons.YesNoCancel,MessageBoxIcon.Warning)==DialogResult.Yes)
            {
                int i=SQLOperation.Delete_Info(Iname);
                MessageBox.Show("成功删除"+i+"条信息!");
                Refreshing();
            }
          }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //DataGridViewTextBoxColumn dgv_Text = new DataGridViewTextBoxColumn();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                int j = i + 1;
                dataGridView1.Rows[i].Cells[0].Value = i + 1;
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e == null || e.Value == null || !(sender is DataGridView))
            {
                return;
            }
            DataGridView dgv = (DataGridView)sender;
            object originalValue = e.Value;
            if (dgv.Columns[e.ColumnIndex].DataPropertyName == "NodeId")
            {
                e.Value = ((int)originalValue == 1) ? "终端节点1" : "终端节点2";
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

    }
}
