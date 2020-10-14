namespace WSAN_Monitor
{
    partial class Actor_Report
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.WSAN_monitorDataSet = new WSAN_Monitor.WSAN_monitorDataSet();
            this.Actor_count_InfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.Actor_count_InfoTableAdapter = new WSAN_Monitor.WSAN_monitorDataSetTableAdapters.Actor_count_InfoTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.WSAN_monitorDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Actor_count_InfoBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "Actor_Count_Dataset";
            reportDataSource1.Value = this.Actor_count_InfoBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "WSAN_Monitor.Report1.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(699, 318);
            this.reportViewer1.TabIndex = 0;
            // 
            // WSAN_monitorDataSet
            // 
            this.WSAN_monitorDataSet.DataSetName = "WSAN_monitorDataSet";
            this.WSAN_monitorDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // Actor_count_InfoBindingSource
            // 
            this.Actor_count_InfoBindingSource.DataMember = "Actor_count_Info";
            this.Actor_count_InfoBindingSource.DataSource = this.WSAN_monitorDataSet;
            // 
            // Actor_count_InfoTableAdapter
            // 
            this.Actor_count_InfoTableAdapter.ClearBeforeFill = true;
            // 
            // Actor_Report
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(699, 318);
            this.Controls.Add(this.reportViewer1);
            this.Name = "Actor_Report";
            this.Text = "Actor_Report";
            this.Load += new System.EventHandler(this.Actor_Report_Load);
            ((System.ComponentModel.ISupportInitialize)(this.WSAN_monitorDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Actor_count_InfoBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource Actor_count_InfoBindingSource;
        private WSAN_monitorDataSet WSAN_monitorDataSet;
        private WSAN_monitorDataSetTableAdapters.Actor_count_InfoTableAdapter Actor_count_InfoTableAdapter;
    }
}