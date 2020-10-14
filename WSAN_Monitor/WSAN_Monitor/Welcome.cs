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
    public partial class Welcome : Form
    {
        public Welcome()
        {
            InitializeComponent();
            this.setPosizition();
            timer1.Interval = 3000;
            timer1.Start();
        }

        private void Welcome_Load(object sender, EventArgs e)
        {
        }
        public void setPosizition()
        {
            int height = System.Windows.Forms.SystemInformation.WorkingArea.Height;
            int width = System.Windows.Forms.SystemInformation.WorkingArea.Width;

            int formheight = this.Size.Height;
            int formwidth = this.Size.Width;

            int newformx = width / 2 - formwidth / 2;
            int newformy = height / 2 - formheight / 2;

            this.SetDesktopLocation(newformx, newformy);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Visible = false;
            new Login().Show();
            timer1.Stop();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
