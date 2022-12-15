using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public class Coordinate
    {
        double xAxis;
        double yAxis;
        string id;

        public string ID
        {
            get { return id; }
            set { id = value; }
        }


        public double XAxis
        {
            get { return xAxis; }
            set { xAxis = value; }
        }

        public double YAxis
        {
            get { return yAxis; }
            set { yAxis = value; }
        }
    }
}
