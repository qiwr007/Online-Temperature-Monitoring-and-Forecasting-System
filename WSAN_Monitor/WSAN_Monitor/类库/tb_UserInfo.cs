using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSAN_Monitor
{
    public class tb_UserInfo
    {
        private string Lname;
        private string Lpass;

        public string Lpass1
        {
            get { return Lpass; }
            set { Lpass = value; }
        }
        private int Llimit;

        public int Llimit1
        {
            get { return Llimit; }
            set { Llimit = value; }
        }
        private string Lremarks;

        public string Lremarks1
        {
            get { return Lremarks; }
            set { Lremarks = value; }
        }

        public string Lname1
        {
            get { return Lname; }
            set { Lname = value; }
        }
    }
}
