using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIL
{
   public class Distance
   {
     
        public List<double> GetDistance(double height,double n,double p0)
        {
           
            AverageDB averageDB = new AverageDB();
            List<MODEL.Average> rssilist = averageDB.GetAverageList();
            List<double> distance = new List<double>();
            foreach (var item in rssilist)
            {
                //基站到定位端的直线距离
                double verDistance,horiDistance;
                verDistance = Math.Pow(10, (p0 - Convert.ToDouble(item.RSSI)) / (10 * n));
                //基站到定位端的水平距离
                horiDistance =Math.Sqrt( Math.Pow(verDistance, 2)- Math.Pow(height, 2));
                distance.Add(horiDistance);               
            }
            return distance;

        }

        /// <summary>
        /// 对环境衰减因子进行修正
        /// </summary>
        /// <param name="rssi"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public double Revise(double rssi, double d)
        {
            return (-68 - rssi) / (10 * Math.Log10(d));
        }

        public double Revise(double rssi)
        {
            return (-68 - rssi) / (10 * Math.Log10(2));
        }


    }
}
