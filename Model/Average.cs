using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public class Average
    {
        string id;
        string rssiaverage;

        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        public string RSSI
        {
            get { return rssiaverage; }
            set { rssiaverage = value; }
        }
    }
}
