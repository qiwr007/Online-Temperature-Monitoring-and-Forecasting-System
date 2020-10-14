using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WSAN_Monitor
{
    class tb_Info
    {
        int ino;
        string iname, wdate, iremarks;
        float temp;
        int humidity;
        int state;

        public int State
        {
            get { return state; }
            set { state = value; }
        }
  
        int nodeId;
       public int Ino
        {
            get { return ino; }
            set { ino = value; }
        }
        public string Iname
        {
            get { return iname; }
            set { iname = value; }
        }
        public float Temp
        {
            get { return temp; }
            set { temp = value; }
        }
        public int NodeId
        {
            get { return nodeId; }
            set { nodeId = value; }
        }
        public int Humidity
        {
            get { return humidity; }
            set { humidity = value; }
        }
        public string Iremarks
        {
            get { return iremarks; }
            set { iremarks = value; }
        }

        public string Wdate
        {
            get { return wdate; }
            set { wdate = value; }
        }



    }
}
