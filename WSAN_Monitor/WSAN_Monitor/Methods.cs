using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace WSAN_Monitor
{
    class Methods
    {
        static string Str = System.Configuration.ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
        public static SqlConnection conn = new SqlConnection(Str);
        public static SqlCommand cmd = new SqlCommand("", conn);
        public static SqlDataAdapter sqlda = new SqlDataAdapter("", conn);

        public static DataTable GetTable(string sql, params SqlParameter[] parameters)
        {
            try
            {
                if (conn == null)
                    conn = new SqlConnection(Str);
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                sqlda.SelectCommand.Parameters.Clear();
                sqlda.SelectCommand.CommandText = sql;
                foreach (SqlParameter parameter in parameters)
                    sqlda.SelectCommand.Parameters.Add(parameter);
                DataSet set = new DataSet();
                sqlda.Fill(set);
                return set.Tables[0];
            }
            catch (Exception ee)
            {
                System.Windows.Forms.MessageBox.Show(ee.ToString());
                return null;
            }
        }

        public static DataRow GetRow(string sql, params SqlParameter[] parameters)
        {
            DataRow row = null;
            DataTable table = GetTable(sql, parameters);
            try
            {
                if (table.Rows.Count > 0)
                    row = table.Rows[0];
            }
            finally
            {
                table.Dispose();
            }
            return row;
        }
        //执行数据的增删改查
        public static int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
        {
            int iss = 0;
            cmd.Parameters.Clear();
            cmd.CommandText = sql;
            foreach (SqlParameter parameter in parameters)
                cmd.Parameters.Add(parameter);
            try
            {
                if (conn == null)
                    conn = new SqlConnection(Str);
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.ExecuteNonQuery();
                iss = 1;
            }
            catch (Exception ee)
            {
                System.Windows.Forms.MessageBox.Show(ee.Message);
                iss = 0;

            }
            return iss;
        }

    }
}
