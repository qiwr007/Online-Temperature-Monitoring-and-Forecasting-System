using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;
using System.IO.Ports;
using System.Diagnostics;

namespace WSAN_Monitor
{
    public partial class Parameter_Query : Form
    {
        public Parameter_Query()
        {
            InitializeComponent();
        }
        private static string AddCourConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
        public static SqlConnection conn = new SqlConnection(AddCourConnStr);
        public static string username="";
        public static int userflag=0;
        String[] Flag = { "超级管理员","一般管理员"};
        string tempA = "", f = "",humidityA,humidityB,tempB="",ss1="",ss2="",ss3="",ss4="",f1="",f2="",tempC="",t1="";
        int nid = 1;
        tb_UserInfo user = new tb_UserInfo();
        tb_Info info1 = new tb_Info();
        tb_Info info2 = new tb_Info();
        tb_Info info3 = new tb_Info();
        tb_Info info4 = new tb_Info();
        static Bitmap bMap = new Bitmap(690,220);
        static Bitmap map2;
        static Bitmap map3;
        Graphics gph = Graphics.FromImage(bMap);//画坐标轴
        Graphics gph1 = Graphics.FromImage(bMap);//画曲线1
        Graphics gph2 = Graphics.FromImage(bMap);//画曲线2
        Graphics g = Graphics.FromImage(bMap);//画图例
        Pen pen1 = new Pen(Color.DarkRed, 2);
        Pen pen2 = new Pen(Color.DarkBlue, 2);
        Pen mypen1 = new Pen(Color.DarkRed, 3);
        Pen mypen2 = new Pen(Color.DarkBlue, 3);
        Graphics gg1;
        Graphics gg2;
        static PointF cPt = new PointF(50, 210);//中心点
        int i, j = 1;
        bool draw = false,caiji=false;
        float[] tempp = new float[20];
        float[] tempq = new float[20];
        int reminder = 0;
        private void Parameter_Query_Activated(object sender, EventArgs e)
        {
           
        }

        private void Parameter_Query_Load(object sender, EventArgs e)
        {
            this.toolStripTextBox1.Text = username;
            string[] ports = System.IO.Ports.SerialPort.GetPortNames();
            this.toolStripTextBox2.Text = Flag[userflag - 1];
            if (userflag > 1)
            {
                this.用户信息查询ToolStripMenuItem.Visible = false;
            }
            mycomm.BaudRate = 115200;
            foreach (string port in ports)
            {
                cOM_ComboBox1.Items.Add(port);
            }
            //cOM_ComboBox1.SelectedIndex = 0;
            pictureBox1.Image = imageList1.Images[0];
            pictureBox3.Image = imageList1.Images[0];
            Console.WriteLine(pictureBox1.Height);
            Console.WriteLine(pictureBox2.Height);
            double He = 0.66 * pictureBox1.Height;
            pictureBox2.Height = (int)He;
            pictureBox2.Image = imageList1.Images[1];
            pictureBox4.Height = (int)He;
            pictureBox4.Image = imageList1.Images[1];
            Intial_picture();
            if (DateTime.Now.Date.Day == 30)
            {
                Initial_Database();
            }
        }
        private void Initial_Database()
        {
            try
            {
                string time = DateTime.Now.AddMonths(-1).ToShortDateString();
                //string yesterday = DateTime.Now.AddDays(-1).ToShortDateString();
                string today = DateTime.Now.ToShortDateString();
                StringBuilder sql = new StringBuilder();
                SqlConnection my_con;
                //string s1 = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                sql.Append("delete from Wm_Info where  Wdate < '"+time+"'");
                my_con = new SqlConnection();
                my_con.ConnectionString = AddCourConnStr;
                my_con.Open();
                SqlCommand cmd = new SqlCommand(sql.ToString(), my_con);
                int i = cmd.ExecuteNonQuery();
                my_con.Close();
                if (i != 0)
                    MessageBox.Show("月底数据库初始化成功!");
                   // richTextBox2.AppendText("通话记录初始化成功\r\n");
                else
                    MessageBox.Show("无初始化项目!");
                    //richTextBox2.AppendText("通话记录无可初始化项\r\n");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message + "初始化失败!");
               // richTextBox2.AppendText(ee.Message + "\r\n通话记录初始化失败\r\n");
            }
        }
        private void Intial_picture()
        {
            map2 = new Bitmap(pictureBox10.Width, pictureBox10.Height);
            map3 = new Bitmap(pictureBox11.Width, pictureBox11.Height);
            gg1 = Graphics.FromImage(map2);
            gg2 = Graphics.FromImage(map3);
            gg1.FillEllipse(Brushes.Red,50,50,50, 50);
            gg2.FillEllipse(Brushes.Red, 50, 50, 50, 50);
            pictureBox10.Image = map2;
            pictureBox11.Image = map3;
        }
        private string Infoid()
        {
            int Date = DateTime.Now.Day;
            int Month = DateTime.Now.Month;
            int Year = DateTime.Now.Year;
            int Hour = DateTime.Now.Hour;
          int Minute = DateTime.Now.Minute;
            int Second = DateTime.Now.Second;
            string strTime = null;
            strTime = Year.ToString();
            if (Month < 10)
            {
                strTime += "0" + Month.ToString();
            }
            else
            {
                strTime += Month.ToString();
            }
            if (Date < 10)
            {
                strTime += "0" + Date.ToString();
            }
            else
            {
                strTime += Date.ToString();
            }
            if (Hour < 10)
            {
                strTime += "0" + Hour.ToString();
            }
            else
            {
                strTime += Hour.ToString();
            }
            if (Minute < 10)
            {
                strTime += "0" + Minute.ToString();
            }
            else
            {
                strTime += Minute.ToString();
            }
            if (Second < 10)
            {
                strTime += "0" +Second.ToString();
            }
            else
            {
                strTime +=  Second.ToString();
            }
            return ("Wm-" + strTime);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)//开始采集数据
        {
            try
            {
                if (toolStripButton1.Text == "开始采集")
                {
                    if (txtserialstate.Text == "关闭")
                    {
                        MessageBox.Show("串口未开启!");
                        toolStripButton1.Text = "开始采集";
                        caiji = false;
                        return;
                    }
                    button1.Enabled = true;
                    caiji = true;
                    toolStripButton1.Text = "结束采集";
                    mycomm.DiscardInBuffer();
                    mycomm.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
                }
                else if(toolStripButton1.Text=="结束采集")
                {
                    if (MessageBox.Show("是否结束采集？", "提示", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        if (mycomm.IsOpen)
                            mycomm.Close();
                        toolStripButton1.Text = "开始采集";
                        txtserialstate.Text = "关闭";
                        openBtn.Text = "打开串口";
                        reminder = 0;
                        caiji = false;
                        button1.Enabled = false;
                        timer1.Enabled = false;
                        timer2.Enabled = false;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string textline = "";
            try
            {
                textline = mycomm.ReadTo("%");
               // SetText(textline);
                if (textline.Length <= 13)
                    return;
                f = textline.Substring(10, 1);
                f1 = textline.Substring(9,1);
                f2 = textline.Substring(12,1);
                Console.WriteLine(textline + " " + f + " " + humidityB+" "+f1+" "+f2);
                if (f == "A")
                {
                    nid = 1;
                    tempA = textline.Substring(6, 2);
                    humidityA = textline.Substring(11, 2);
                    ss1 = textline.Substring(9, 1);
                    ss2 = textline.Substring(13, 1);
                    info1.Temp = float.Parse(tempA);
                    info1.Humidity = int.Parse(humidityA);
                    info1.State = -1;
                    info1.Iremarks = "终端1温湿度信息";
                    SetText1(tempA);
                    SetText3(humidityA);
                    if (ss1 == "H" )
                    {
                       // txth1.Text = "高!";
                        SetTextt1("高!");
                    }
                    else if (ss1 == "L")
                    {
                       // txth1.Text = "低!";
                        SetTextt1("低!");
                    }
                    else
                    {
                      //  txth1.Text = "正常";
                        SetTextt1("正常");
                    }
                    if (ss2 == "H")
                    {
                      //  txtt1.Text = "高!";
                        SetTexth1("高!");
                    }
                    else if (ss2 == "L")
                    {
                      //  txtt1.Text = "低!";
                        SetTexth1("低!");
                    }
                    else
                    {
                       // txtt1.Text = "正常";
                        SetTexth1("正常");
                    }
                    info1.Iname = Infoid();
                    info1.NodeId = nid;
                 //   int i = SQLOperation.insert_Info(info,0);
                //    if (i < 0)
                        //MessageBox.Show("添加记录失败!");
                }
                else if (f == "B")
                {
                    nid = 2;
                    tempB = textline.Substring(6, 2);
                    humidityB = textline.Substring(11, 2);
                    ss1 = textline.Substring(9, 1);
                    ss2 = textline.Substring(13, 1);
                    info2.Temp = float.Parse(tempB);
                    info2.Humidity = int.Parse(humidityB);
                    info2.State = -1;
                    info2.Iremarks = "终端2温湿度信息";
                    SetText2(tempB);
                    SetText4(humidityB);
                    if (ss1 == "H" )
                    {
                       // txth2.Text = "高!";
                        SetTextt2("高!");
                    }
                    else if (ss1 == "L")
                    {
                       // txth2.Text = "低!";
                        SetTextt2("低!");
                    }
                    else
                    {
                       // txth2.Text = "正常";
                        SetTextt2("正常");
                    }
                    if (ss2 == "H")
                    {
                       // txtt2.Text = "高!";
                        SetTexth2("高!");
                    }
                    else if (ss2 == "L")
                    {
                      //  txtt2.Text = "低!";
                        SetTexth2("低!");
                    }
                    else
                    {
                       // txtt2.Text = "正常";
                        SetTexth2("正常");
                    }
                    info2.Iname = Infoid();
                    info2.NodeId = nid;
                   // int i = SQLOperation.insert_Info(info, 0);
                 //   if (i < 0)
                     //   MessageBox.Show("添加记录失败!");
                }
                if (f1 == "C"&&f2 == "D")
                {
                    nid = 3;
                    ss3 = textline.Substring(10, 1);
                     ss4 = textline.Substring(13, 1);
                    tempC = textline.Substring(6, 2);
                    t1 = textline.Substring(11, 1);
                    SetText7(tempC);
                    if (t1 == "H")
                    {
                        //textBox1.Text = "温度过高";
                        SetTextat("温度过高");

                    }
                    else if (t1 == "L")
                    {
                      //  textBox1.Text = "温度过低";
                        SetTextat("温度过低");
                    }
                    else
                    {
                       // textBox1.Text = "温度正常";
                        SetTextat("温度正常");
                    }
                    if(ss3 == "0" && ss4== "0")
                    {
                         SetText5("停止工作");
                        SetText6("停止工作");
                         info3.State = 0;
                        info3.Iremarks = "执行器未动作";
                        gg1.FillEllipse(Brushes.Red, 50, 50, 50, 50);
                        gg2.FillEllipse(Brushes.Red, 50, 50, 50, 50);
                        pictureBox10.Image = map2;
                        pictureBox11.Image = map3;
                    }
                    else if (ss3 == "1" && ss4 == "0")
                    {
                        SetText5("正在工作");
                        SetText6("停止工作");
                        info3.State = 1;
                        info3.Iremarks = "终端1参数异常导致执行器动作";
                        gg1.FillEllipse(Brushes.Green, 50, 50, 50, 50);
                        pictureBox10.Image = map2;
                        gg2.FillEllipse(Brushes.Red, 50, 50, 50, 50);
                        pictureBox11.Image = map3;
                    }
                    else if(ss3 == "0" && ss4 == "1")
                    {
                         SetText5("停止工作");
                         SetText6("正在工作");
                        info3.State = 1;
                        info3.Iremarks = "终端2参数异常导致执行器动作";
                        gg1.FillEllipse(Brushes.Red, 50, 50, 50, 50);
                        pictureBox10.Image = map2;
                        gg2.FillEllipse(Brushes.Green, 50, 50, 50, 50);
                        pictureBox11.Image = map3;
                    }
                    else if (ss3 == "1" && ss4 == "1")
                    {
                        SetText5("正在工作");
                        SetText6("正在工作");
                        info3.State = 1;
                        info3.Iremarks = "终端1终端2参数异常导致执行器动作";
                        gg1.FillEllipse(Brushes.Green, 50, 50, 50, 50);
                        pictureBox10.Image = map2;
                        gg2.FillEllipse(Brushes.Green, 50, 50, 50, 50);
                        pictureBox11.Image = map3;
                    }
                    info3.Temp = -1;
                    info3.Humidity = -1;
                    info3.Iname = Infoid();
                    info3.NodeId = nid;
                }
               Console.WriteLine(ss1 + ss2+ss3+ss4+t1);
            }
            catch
            {
                MessageBox.Show("接收失败");
            }
        }
        private delegate void SetTextCallback(string text);
        private delegate void SetPictureCallBack(Image img);
        private void SetText1(string text)
        {
            if (temp1value.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText1);
                Invoke(d, new object[] { text });
            }
            else
            {
                temp1value.Text = text + "℃";
            }
            double q = 0;
            q = (double.Parse(text) + 30) / 80 - 0.07;
            int He = (int)(q * pictureBox1.Height);
            pictureBox2.Height = He;
        }
        private void SetText2(string text)
        {
            if (temp2value.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText2);
                Invoke(d, new object[] { text });
            }
            else
            {
                temp2value.Text = text + "℃";
            }
            double q = 0;
            q = (double.Parse(text) + 30) / 80 - 0.07;
            int He = (int)(q * pictureBox3.Height);
            pictureBox4.Height = He;
           
        }
        private void SetText3(string text)
        {
            if (txthumidity1. InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText3);
                Invoke(d, new object[] { text });
            }
            else
            {
                txthumidity1.Text = text + "%";
            }
        }
        private void SetText4(string text)
        {
            if (txthumidity2.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText4);
                Invoke(d, new object[] { text });
            }
            else
            {
                txthumidity2.Text = text + "%";
            }
        }
        private void SetText5(string text)
        {
            if (txtstate1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText5);
                Invoke(d, new object[] { text });
            }
            else
            {
                txtstate1.Text = text ;
            }
        }
        private void SetText6(string text)
        {
            if (txtstate2.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText6);
                Invoke(d, new object[] { text });
            }
            else
            {
                txtstate2.Text = text;
            }
        }
        private void SetText7(string text)
        {
            if (txtatemp.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText7);
                Invoke(d, new object[] { text });
            }
            else
            {
                txtatemp.Text = text + "℃";
            }
        }
        private void SetTextt1(string text)
        {
            if (txtt1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetTextt1);
                Invoke(d, new object[] { text });
            }
            else
            {
                txtt1.Text = text ;
            }
        }
        private void SetTextt2(string text)
        {
            if (txtt2.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetTextt2);
                Invoke(d, new object[] { text });
            }
            else
            {
                txtt2.Text = text;
            }
        }
        private void SetTexth1(string text)
        {
            if (txth1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetTexth1);
                Invoke(d, new object[] { text });
            }
            else
            {
                txth1.Text = text;
            }
        }
        private void SetTexth2(string text)
        {
            if (txth2.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetTexth2);
                Invoke(d, new object[] { text });
            }
            else
            {
                txth2.Text = text;
            }
        }
        private void SetTextat(string text)
        {
            if (textBox1. InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetTextat);
                Invoke(d, new object[] { text });
            }
            else
            {
                textBox1.Text = text;
            }
        }
        private void 用户信息维护ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new UserInfo().Show();
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)//节点信息查询
        {
            NodeInfoQuery nq = new NodeInfoQuery();
            nq.Show();
        }
        private void 密码修改ToolStripMenuItem_Click(object sender, EventArgs e)//修改密码
        {
            this.Hide();
            PassEdit.name = username;
            PassEdit.flag = userflag;
            PassEdit pe = new PassEdit();
            pe.Show();
        }

        private void openBtn_Click(object sender, EventArgs e)
        {
            if (openBtn.Text == "关闭串口")
            {
                openBtn.Text = "打开串口";
                if (mycomm.IsOpen)
                    mycomm.Close();
                txtserialstate.Text = "关闭";
            }
            else
            {
                try
                {
                    mycomm.PortName = cOM_ComboBox1.SelectedItem.ToString();
                    mycomm.BaudRate = 115200;
                    mycomm.Open();
                    openBtn.Text = "关闭串口";
                   txtserialstate.Text = "打开";
                }
                catch
                {
                    MessageBox.Show("没发现串口");
                    openBtn.Text = "打开串口";
                    txtserialstate.Text = "关闭";
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)//开始画图
        {
            j = 0;
            if (button1.Text == "开始")
            {
                draw = true;
                if (radioButton1.Checked)
                {
                    timer1.Enabled = true;
                    timer2.Enabled = false;
                }
                else if (radioButton2.Checked)
                {
                    timer1.Enabled = false;
                    timer2.Enabled = true;
                }
                else
                {
                    timer1.Enabled = false;
                    timer2.Enabled = false;
                }
                button1.Text = "结束";
                return;
            }
            else
            {
                draw = false;
                timer1.Enabled = false;
                timer2.Enabled = false;
                button1.Text = "开始";
                return;
            }
        }
        private void Paint_Legend()
        {
            g.DrawLine(mypen1, cPt.X + 480, cPt.Y - 175, cPt.X + 500, cPt.Y - 175);
            g.DrawLine(mypen2, cPt.X + 480, cPt.Y - 160, cPt.X + 500, cPt.Y - 160);
            g.DrawString("终端1", new Font("宋体", 10), Brushes.Black, new PointF(cPt.X + 500, cPt.Y - 175));
            g.DrawString("终端2", new Font("宋体", 10), Brushes.Black, new PointF(cPt.X + 500, cPt.Y - 160));
           
        }
        private void Paint_Axis()
        {

            if (radioButton1.Checked)//温度曲线
            {
                gph.Clear(Color.LightCoral);
                gph1.Clear(Color.White);
                gph2.Clear(Color.White);
                g.Clear(Color.White);
                Paint_Legend();
                PointF[] xPt = new PointF[3] { new PointF(cPt.Y + 415, cPt.Y), new PointF(cPt.Y + 400, cPt.Y - 8), new PointF(cPt.Y + 400, cPt.Y + 8) };//X轴三角形
                PointF[] yPt = new PointF[3] { new PointF(cPt.X, cPt.X - 30), new PointF(cPt.X - 5, cPt.X - 15), new PointF(cPt.X + 5, cPt.X - 15) };//Y轴三角形
                //画X轴
                gph.DrawLine(Pens.Black, cPt.X, cPt.Y, cPt.Y + 400, cPt.Y);
                gph.DrawPolygon(Pens.Black, xPt);
                gph.FillPolygon(new SolidBrush(Color.Black), xPt);
                gph.DrawString("时刻", new Font("宋体", 8), Brushes.Black, new PointF(cPt.Y + 415, cPt.Y));
                //画Y轴
                gph.DrawLine(Pens.Black, cPt.X, cPt.Y, cPt.X, cPt.X - 20);
                gph.DrawPolygon(Pens.Black, yPt);
                gph.FillPolygon(new SolidBrush(Color.Black), yPt);
                gph.DrawString("温度(℃)", new Font("宋体", 10), Brushes.Black, new PointF(0, 7));
                for (i = 1; i <6; i++)
                {
                    //画Y轴
                    gph.DrawString((i * 10).ToString(), new Font("宋体", 11), Brushes.Black, new PointF(cPt.X - 30, cPt.Y - i * 30 - 6));
                    gph.DrawLine(Pens.Black, cPt.X - 3, cPt.Y - i * 30, cPt.X, cPt.Y - i * 30);
                }
                pictureBox5.Image = bMap;
          
            }
            else if (radioButton2.Checked)
            {
                gph.Clear(Color.White);
                gph1.Clear(Color.White);
                gph2.Clear(Color.White);
               Paint_Legend();
                PointF[] xPt = new PointF[3] { new PointF(cPt.Y + 415, cPt.Y), new PointF(cPt.Y + 400, cPt.Y - 8), new PointF(cPt.Y + 400, cPt.Y + 8) };//X轴三角形
                PointF[] yPt = new PointF[3] { new PointF(cPt.X, cPt.X - 30), new PointF(cPt.X - 5, cPt.X - 15), new PointF(cPt.X + 5, cPt.X - 15) };//Y轴三角形
                //画X轴
                gph.DrawLine(Pens.Black, cPt.X, cPt.Y, cPt.Y + 400, cPt.Y);
                gph.DrawPolygon(Pens.Black, xPt);
                gph.FillPolygon(new SolidBrush(Color.Black), xPt);
                gph.DrawString("时刻", new Font("宋体", 8), Brushes.Black, new PointF(cPt.Y + 415, cPt.Y));
                //画Y轴
                gph.DrawLine(Pens.Black, cPt.X, cPt.Y, cPt.X, cPt.X - 20);
                gph.DrawPolygon(Pens.Black, yPt);
                gph.FillPolygon(new SolidBrush(Color.Black), yPt);
                gph.DrawString("湿度(%)", new Font("宋体", 10), Brushes.Black, new PointF(0, 7));
                for (i = 1; i < 6; i++)
                {
                    //画Y轴
                    gph.DrawString((i * 20).ToString(), new Font("宋体", 11), Brushes.Black, new PointF(cPt.X - 30, cPt.Y - i * 30 - 6));
                    gph.DrawLine(Pens.Black, cPt.X - 3, cPt.Y - i * 30, cPt.X, cPt.Y - i * 30);
                }
                pictureBox5.Image = bMap;
            }
            else
            {
                pictureBox1.Image = null;
            }
           
        }
        private void Parameter_Query_Paint(object sender, PaintEventArgs e)
        {
            Paint_Axis();
            Paint_Legend();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (draw)
            {
                if (j >= 18)
                {
                    pictureBox5.Image = null;
                    gph1.Clear(Color.White);
                    gph2.Clear(Color.White);
                      Paint_Legend();
                    PointF[] xPt = new PointF[3] { new PointF(cPt.Y + 415, cPt.Y), new PointF(cPt.Y + 400, cPt.Y - 8), new PointF(cPt.Y + 400, cPt.Y + 8) };//X轴三角形
                    PointF[] yPt = new PointF[3] { new PointF(cPt.X, cPt.X - 30), new PointF(cPt.X - 5, cPt.X - 15), new PointF(cPt.X + 5, cPt.X - 15) };//Y轴三角形
                    //画X轴
                    gph.DrawLine(Pens.Black, cPt.X, cPt.Y, cPt.Y + 400, cPt.Y);
                    gph.DrawPolygon(Pens.Black, xPt);
                    gph.FillPolygon(new SolidBrush(Color.Black), xPt);
                    gph.DrawString("时刻", new Font("宋体", 8), Brushes.Black, new PointF(cPt.Y + 415, cPt.Y ));
                    //画Y轴
                    gph.DrawLine(Pens.Black, cPt.X, cPt.Y, cPt.X, cPt.X - 10);
                    gph.DrawPolygon(Pens.Black, yPt);
                    gph.FillPolygon(new SolidBrush(Color.Black), yPt);
                    gph.DrawString("温度(℃)", new Font("宋体", 10), Brushes.Black, new PointF(0, 7));
                    for (i = 1; i < 6; i++)
                    {
                        //画Y轴刻度
                        gph.DrawString((i * 10).ToString(), new Font("宋体", 11), Brushes.Black, new PointF(cPt.X - 30, cPt.Y - i * 30 - 6));
                        gph.DrawLine(Pens.Black, cPt.X - 3, cPt.Y - i * 30, cPt.X, cPt.Y - i * 30);
                    }
                    pictureBox5.Image = bMap;
                    j = 0;
                }
                tempp[j] = float.Parse(tempA);
                tempq[j] = float.Parse(tempB);
               gph1.DrawEllipse(Pens.Black, cPt.X +j * 30 - 1.5F, cPt.Y - float.Parse(tempA) * 3 - 1.5F, 3, 3);
                gph2.DrawEllipse(Pens.Black, cPt.X + j * 30 - 1.5F, cPt.Y - float.Parse(tempB) * 3 - 1.5F, 3, 3);
               // gph1.FillEllipse(new SolidBrush(Color.Red), cPt.X + j * 30 - 1.5F, cPt.Y - float.Parse(tempA) * 3 - 1.5F, 3, 3);
               // gph2.FillEllipse(new SolidBrush(Color.DarkBlue), cPt.X + j * 30 - 1.5F, cPt.Y - float.Parse(tempB) * 3 - 1.5F, 3, 3);
                //gph1.DrawString(float.Parse(tempA).ToString(), new Font("宋体", 8), Brushes.Red, new PointF(cPt.X + j * 30, cPt.Y - float.Parse(tempA) * 3));
              //  gph2.DrawString(float.Parse(tempB).ToString(), new Font("宋体", 8), Brushes.DarkBlue, new PointF(cPt.X + j * 30, cPt.Y - float.Parse(tempB) * 3.5F));
                if (j> 0)
                {
                    gph1.DrawLine(pen1, cPt.X + (j - 1) * 30, cPt.Y - tempp[j-1] * 3, cPt.X + j * 30, cPt.Y - tempp[j ] * 3);
                    gph2.DrawLine(pen2, cPt.X + (j - 1) * 30, cPt.Y - tempq[j - 1] * 3, cPt.X + j * 30, cPt.Y - tempq[j ] * 3);
                }
                j++;
                pictureBox5.Image = bMap;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (draw)
            {
                if (j >= 18)
                {
                    pictureBox5.Image = null;
                    gph1.Clear(Color.White);
                    gph2.Clear(Color.White);
                     Paint_Legend();
                    PointF[] xPt = new PointF[3] { new PointF(cPt.Y + 415, cPt.Y), new PointF(cPt.Y + 400, cPt.Y - 8), new PointF(cPt.Y + 400, cPt.Y + 8) };//X轴三角形
                    PointF[] yPt = new PointF[3] { new PointF(cPt.X, cPt.X - 30), new PointF(cPt.X - 5, cPt.X -15), new PointF(cPt.X + 5, cPt.X - 15) };//Y轴三角形
                    //画X轴
                    gph.DrawLine(Pens.Black, cPt.X, cPt.Y, cPt.Y + 400, cPt.Y);
                    gph.DrawPolygon(Pens.Black, xPt);
                    gph.FillPolygon(new SolidBrush(Color.Black), xPt);
                    gph.DrawString("时刻", new Font("宋体", 8), Brushes.Black, new PointF(cPt.Y + 415, cPt.Y ));
                    //画Y轴
                    gph.DrawLine(Pens.Black, cPt.X, cPt.Y, cPt.X, cPt.X - 20);
                    gph.DrawPolygon(Pens.Black, yPt);
                    gph.FillPolygon(new SolidBrush(Color.Black), yPt);
                    gph.DrawString("湿度(%)", new Font("宋体", 10), Brushes.Black, new PointF(0, 7));
                    for (i = 1; i < 6; i++)
                    {
                        //画Y轴
                        gph.DrawString((i * 20).ToString(), new Font("宋体", 11), Brushes.Black, new PointF(cPt.X - 30, cPt.Y - i * 30 - 6));
                        gph.DrawLine(Pens.Black, cPt.X - 3, cPt.Y - i * 30, cPt.X, cPt.Y - i * 30);
                    }
                    pictureBox5.Image = bMap;
                    j = 0;
                }
                tempp[j] = float.Parse(humidityA);
                tempq[j] = float.Parse(humidityB);
                gph1.DrawEllipse(Pens.Black, cPt.X + j * 30 - 1.5F, cPt.Y - float.Parse(humidityA) * 1.5F - 3, 3, 3);
                gph2.DrawEllipse(Pens.Black, cPt.X + j * 30 - 1.5F, cPt.Y - float.Parse(humidityB) * 1.5F - 3, 3, 3);
               // gph1.FillEllipse(new SolidBrush(Color.Red), cPt.X + j * 30 - 1.5F, cPt.Y - float.Parse(humidityA) * 1.5F - 3, 3, 3);
               // gph2.FillEllipse(new SolidBrush(Color.DarkBlue), cPt.X + j * 30 - 1.5F, cPt.Y - float.Parse(humidityB) * 1.5F - 3, 3, 3);
               // gph1.DrawString(float.Parse(humidityA).ToString(), new Font("宋体", 8), Brushes.Red, new PointF(cPt.X + j * 30, cPt.Y - float.Parse(humidityA) * 1.5F));
               // gph2.DrawString(float.Parse(humidityB).ToString(), new Font("宋体", 8), Brushes.DarkBlue, new PointF(cPt.X + j * 30, cPt.Y - float.Parse(humidityB) * 1.7F));
                if (j > 0)
                {
                    gph1.DrawLine(pen1, cPt.X + (j - 1) * 30, cPt.Y - tempp[j - 1] * 1.5F, cPt.X + j * 30, cPt.Y - tempp[j] *1.5F);
                    gph2.DrawLine(pen2, cPt.X + (j - 1) * 30, cPt.Y - tempq[j - 1] * 1.5F, cPt.X + j * 30, cPt.Y - tempq[j] * 1.5F);
                }
                j++;
                pictureBox5.Image = bMap;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            draw = false;
            button1.Text = "开始";
            timer1.Enabled = false;
            timer2.Enabled = false;
            j = 0;
            Paint_Axis();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            draw = false;
            button1.Text = "开始";
            timer1.Enabled = false;
            timer2.Enabled = false;
            j = 0;
            Paint_Axis();
        }

        private void Parameter_Query_FormClosed(object sender, FormClosedEventArgs e)//关闭窗口
        {
            draw = false;
            button1.Text = "开始";
            timer1.Enabled = false;
            timer2.Enabled = false;
            openBtn.Text = "打开串口";
            if (mycomm.IsOpen)
                mycomm.Close();
            txtserialstate.Text = "关闭";
            toolStripButton1.Text = "开始采集";
            Login.username = toolStripTextBox1.Text;
            new Login().Show();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            draw = false;
            button1.Text = "开始";
            timer1.Enabled = false;
            timer2.Enabled = false;
            openBtn.Text = "打开串口";
            if (mycomm.IsOpen)
                mycomm.Close();
            txtserialstate.Text = "关闭";
            toolStripButton1.Text = "开始采集";
            Login.username = toolStripTextBox1.Text;
            this.Hide();
            new Login().Show();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            toolStripTextBox4.Text = DateTime.Now.ToString();
            if (caiji)
            {
                reminder++;
            }
            if (reminder == 2)
            {
                info1.Iname = Infoid() + "1";
                int i = SQLOperation.insert_Info(info1, 0);
                if (i <= 0)
                    MessageBox.Show("添加记录失败!");
            }
            if (reminder == 3)
            {
                info2.Iname = Infoid() + "2";
                int k = SQLOperation.insert_Info(info2, 0);
                if (k <= 0)
                    MessageBox.Show("添加记录失败!");
            }
            if (reminder == 5)
            {
                reminder = 0;
                info3.Iname = Infoid()+"3";
                int j = SQLOperation.insert_Info(info3, 0);
               if (j<=0)
                   MessageBox.Show("添加记录失败!");
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ActorInfo ai = new ActorInfo();
            ai.Show();
        }
        
       

    

    

       
    }
}
