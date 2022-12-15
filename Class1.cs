using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    public class Class1
    {
        public void GetN()
        {
            int[] a03 = { 31, 31, 31, 31, 31, 31, 31, 31, 32, 32, 32, 32, 32, 32, 32, 31, 31, 31, 31, 31 };
            int[] a06 = { 42, 42, 42, 42, 42, 42, 42, 42, 42, 42, 42, 42, 42, 42, 42, 42, 42, 42, 42, 42 };
            int[] a09 = { 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50 };
            int[] a12 = { 53, 52, 52, 52, 52, 52, 52, 52, 52, 52, 53, 52, 52, 52, 52, 52, 52, 52, 52, 53 };
            int[] a15 = { 42, 42, 42, 42, 42, 42, 42, 42, 42, 42, 42, 42, 42, 42, 42, 42, 42, 42, 42, 42 };
            int[] a18 = { 45, 45, 46, 45, 45, 45, 45, 46, 46, 46, 46, 45, 46, 46, 45, 45, 45, 46, 45, 45 };
            int[] a21 = { 51, 52, 52, 52, 52, 52, 52, 52, 52, 52, 52, 52, 52, 51, 51, 51, 51, 52, 52, 52 };
            int[] a24 = { 47, 48, 48, 48, 48, 48, 48, 48, 48, 48, 48, 48, 48, 48, 48, 48, 47, 48, 47, 48 };

            double[] pi = new double[8];
            for (int i = 0; i<8; i++)
			{
                double di = 0.3 * i;
                pi[i] = -Math.Log10(di);

            }

            double piaverage = pi.Average();

            double a03average = a03.Average();
            double a06average = a06.Average();
            double a09average = a09.Average();
            double a12average = a12.Average();
            double a15average = a15.Average();
            double a18average = a18.Average();
            double a21average = a21.Average();
            double a24average = a24.Average();

            double[] rssi = { a03average,a06average,a09average,a12average,a15average,
                               a18average,a21average,a24average };
            double rssiavrage = rssi.Average();
            double n1 = 0, n2 = 0 ;

            for (int i = 0; i < 8; i++)
            {
                n1 += (pi[i] - piaverage) * rssi[i];
                n2 += Math.Pow((pi[i] - piaverage), 2);
            }
            double n = n1 / n2;
        }



    }
}
