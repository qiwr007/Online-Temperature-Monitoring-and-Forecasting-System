using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace WSAN_Monitor
{
     
    class SQLOperation
    {
         static string Str = System.Configuration.ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
        public static SqlConnection conn = new SqlConnection(Str);
        public static SqlCommand cmd = new SqlCommand("", conn);
        public static SqlDataAdapter sqlda = new SqlDataAdapter("", conn);
        static string[] REMARKS = { "超级管理员","一般管理员"};
        public static int ExistCount(tb_UserInfo User)//判断查找的对象是否在数据库中
        {

            string sql = "select * from Wm_Login where Lname=@userId and Lpass=@userpassword";
            DataRow datarow = Methods.GetRow(sql, new SqlParameter("@userId", User.Lname1), new SqlParameter("@userpassword", User.Lpass1));
            try
            {
                if (datarow != null)
                {
                    object i = datarow["Llimit"];
                    if (i.Equals(1)) return 1;
                    else if (i.Equals(2)) return 2;
                    else if (i.Equals(3)) return 3;
                    else if (i.Equals(4)) return 4;
                    else if (i.Equals(5)) return 5;
                    datarow.CancelEdit();
                }
                return -1;//用户名或者密码错误，或者不存在此用户;
            }
            catch (Exception ee)
            {
                System.Windows.Forms.MessageBox.Show(ee.ToString());
                datarow.CancelEdit();
                return -1;
            }

        }
        //插入用户
        public static int insert_User(tb_UserInfo  uer,int flag)
        {
            int i = 0;
            string sqL = "insert into Wm_Login values(@ID,@passward,@limite,@remarks)";
            i = Methods.ExecuteNonQuery(sqL, new SqlParameter("@ID", uer.Lname1), new SqlParameter("@passward", uer.Lpass1), new SqlParameter("@limite", flag),new SqlParameter("@remarks",REMARKS[flag-1]));
            if (i != 0)
                return i;
            else 
            return 0;
        }
        //修改密码
        public static int UpdatePwd(tb_UserInfo mp)
        {
            string sql = "update Wm_Login set Lpass=@passward where Lname=@ID";
            int i = Methods.ExecuteNonQuery(sql, new SqlParameter("@ID",mp.Lname1), new SqlParameter("@passward", mp.Lpass1));
            if (i  != 0)
                return i;
            else return 0;
        }
        //删除用户信息
        public static int Delete_User(string lname)
        {
            string sql = "delete from Wm_Login  where Lname=@Jhid";
            int i = Methods.ExecuteNonQuery(sql, new SqlParameter("@Jhid", lname));
            return i;
        }
        //插入采集的信息
        public static int insert_Info(tb_Info info,int k)
        {
            int i = 0;
            string sqL = "insert into Wm_Info (Iname,Temp,humidity,NodeId,states,Wdate,Iremarks) values(@Iname,@Temp,@humidity,@NodeId,@State,@Wdate,@remarks)";
            i = Methods.ExecuteNonQuery(sqL, new SqlParameter("@Iname", info.Iname), new SqlParameter("@Temp", info.Temp), new SqlParameter("@humidity", info.Humidity), new SqlParameter("@NodeId", info.NodeId), new SqlParameter("@State", info.State), new SqlParameter("@Wdate", DateTime.Now.ToShortDateString().ToString()), new SqlParameter("@remarks", info.Iremarks));
            if (i != 0)
                return i;
            else
                return 0;
        }
        //删除记录信息
        public static int Delete_Info(string ino)
        {
            string sql = "delete from Wm_Info  where Iname=@Jhid";
            int i = Methods.ExecuteNonQuery(sql, new SqlParameter("@Jhid", ino));
            return i;
        }
        //在信息表中，返回参数中指定的字段集合
        public static DataTable QuerySensorInfo(string filter)
        {
            string sql = "";
            if (filter == "")
            {
                sql = "select * from SensorNodeView";
            }
            else
                sql = "select distinct " + filter + " from SensorNodeView";
            DataTable table = Methods.GetTable(sql);
            return table;
        }
        public static DataTable QueryActorInfo(string filter)
        {
            string sql = "";
            if (filter == "")
            {
                sql = "select * from ActorNodeView";
            }
            else
                sql = "select distinct " + filter + " from ActorNodeView";
            DataTable table = Methods.GetTable(sql);
            return table;
        }
    }
}
