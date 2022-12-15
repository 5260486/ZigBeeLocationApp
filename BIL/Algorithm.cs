using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;

namespace BIL
{
    /// <summary>
    /// 三角定位算法
    /// </summary>
    public class Algorithm
    {
        EnvFactorDB envDB = new EnvFactorDB();
        IDDB idDB = new IDDB();
        CoordinateDB coordinateDB = new CoordinateDB();
        LocationDB locationDB = new LocationDB();

        Distance distance = new Distance();

        MODEL.Location location = new MODEL.Location();

        double totalWeight = 0;
       
        /// <summary>
        /// 根据收到的rssi值计算终端UE的位置
        /// </summary>
      /*  public void GetLocation()
        {
            List<MODEL.ID> idlist = new List<MODEL.ID>();

            List<MODEL.Average> rssilist = new List<MODEL.Average>();

            if (idlist==null || idlist.Count<3)
            {
                throw new Exception("the number of rssi is less then 3, cloud not located the mobel unit!");
            }

            //取rssi数据的前5个进行分组计算，如果参与计算的数量太多，会造成累计误差增大
            rssilist = (from ap in rssilist orderby ap.RSSI descending select ap).Take(5).ToList();

            //对rssi分组
            List<object> temp = rssilist.ConvertAll(s => (object)s);
            CombineAlgorithm combine = new CombineAlgorithm(temp.ToArray(), 3);  //5个取3个
            object[][] combineArray = combine.getResult();
            MODEL.Location deviceLocation = new MODEL.Location();
            /*
            for (int i = 0; i < combineArray.GetLength(0); i++)
            {
                List<MODEL.Average> coorlist = new List<MODEL.Average>();
                foreach (var item in combineArray[i])
                {
                    coorlist.Add((MODEL.Average)item);
                }
                //得到加权后坐标
                deviceLocation = Caculate(coorlist);
            }

            location.X = deviceLocation.X / totalWeight;
            location.Y = deviceLocation.Y / totalWeight;

            locationDB.MessageAdd(location);
        }*/





        /// <summary>
        /// 根据三角形定位算法计算移动端位置
        /// </summary>
        /// <param name="average"></param>
        public MODEL.Location Caculate()
        {
            double[,] a_array = new double[2, 2];
            double[] b_array = new double[2];

            List<MODEL.EnvFactor> envlist = envDB.GetEnvList();
            List<double> distancelist = new List<double>();

            /*得到环境影响因素的值*/
            double height, n, p0;
            height = envlist[0].Height; 
            n = envlist[0].N;  
            p0 = envlist[0].P0;  

           /* height=  envlist.Find(s => s.Height>0).Height;
            n = envlist.Find(s => s.N > 3).N;
            p0 = envlist.Find(s => s.P0 > 40).P0;*/

            distancelist = distance.GetDistance(height, n, p0);
            //距离数组
            double[] distanceArray = distancelist.ToArray();
           
            /*取收到的基站对应的坐标列表*/
            List<MODEL.ID> idlist = idDB.GetIDList();
            List<string> idd = new List<string>();
            foreach (var item in idlist)
            {
                idd.Add(item.ReceiveID);
            }
            string[] idarray = idd.ToArray();
            string id="";
            for (int i = 0; i < idlist.Count; i++)
            {
                id= idlist.Find(s => s.ReceiveID == ""+idarray[i]+"").ReceiveID+'|';
            }
            string strWhere = " id REGEXP '" + id + "'";
            List<MODEL.Coordinate> coorlist =coordinateDB.GetCoorList(strWhere);

            //系数矩阵A初始化
            for (int i = 0; i < 2; i++)
            {
                a_array[i, 0] = 2 * (coorlist[i].XAxis - coorlist[idlist.Count-1].XAxis);
                a_array[i, 1] = 2 * (coorlist[i].YAxis - coorlist[idlist.Count-1].YAxis);
            }

            //矩阵b初始化
            for (int i = 0; i < 2; i++)
            {
                b_array[i]=Math.Pow(coorlist[i].XAxis,2)
                            -Math.Pow(coorlist[idlist.Count-1].XAxis,2)
                            + Math.Pow(coorlist[i].YAxis, 2)
                            - Math.Pow(coorlist[idlist.Count - 1].YAxis, 2)
                            +Math.Pow(distanceArray[2],2)
                            - Math.Pow(distanceArray[i], 2);
            }
            var matrixA = DenseMatrix.OfArray(a_array);
            var vectorB = new DenseVector(b_array);
            //计算 X=(A^T * A)^-1 * A^T * b
            var a1 = matrixA.Transpose(); // A的转置
            var a2 = a1 * matrixA;
            var resultX = a2.Inverse() * a1 * vectorB;
            double[] res = resultX.ToArray();

            /*对应的权值*/
            double weight = 0;
            for (int i = 0; i < 3; i++)
            {
                weight += (1.0 / distanceArray[i]);
            }
            totalWeight += weight;

            location.X = res[0] * weight;
            location.Y = res[1] * weight;

            locationDB.MessageAdd(location);
            return location;
        }
    }

    
}
