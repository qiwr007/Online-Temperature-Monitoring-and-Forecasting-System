﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WSAN_Monitor
{
    public partial class Actor_Report : Form
    {
        public Actor_Report()
        {
            InitializeComponent();
        }

        private void Actor_Report_Load(object sender, EventArgs e)
        {
            // TODO: 这行代码将数据加载到表“WSAN_monitorDataSet.Actor_count_Info”中。您可以根据需要移动或删除它。
            this.Actor_count_InfoTableAdapter.Fill(this.WSAN_monitorDataSet.Actor_count_Info);

            this.reportViewer1.RefreshReport();
        }
    }
}
