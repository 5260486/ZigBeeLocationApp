using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public class EnvFactor  //环境因子
    {

        /* 高度补偿值*/
        double height;

        /*环境衰减因子*/
        double n;

        /*一米处接收到的rssi值*/
        double p0;

        public double Height
        {
            get { return height; }
            set { height = value; }
        }

        public double N
        {
            get { return n; }
            set { n = value; }
        }

        public double P0
        {
            get { return p0; }
            set { p0 = value; }
        }
    }
}
