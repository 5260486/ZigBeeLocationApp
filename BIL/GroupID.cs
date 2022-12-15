using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BIL
{
    public class GroupID
    {
        MODEL.User modeluser = new MODEL.User();
        MODEL.ID modelid = new MODEL.ID();
        MODEL.Average modelaverage = new MODEL.Average();

        BIL.UserDB biluser = new BIL.UserDB();
        BIL.IDDB bilid = new IDDB();
        BIL.AverageDB bilaverage = new AverageDB();

        public string[] GetID(string[] split)
        {
            List<string> id = new List<string>();
            List<string> idd = new List<string>();

            for (int i = 0; i < split.Length; i++)
            {
                if (i % 2 == 0)
                {
                    idd.Add(split[i]);
                    modeluser.ID = split[i];
                }
                else
                {
                    modeluser.RSSI = split[i];
                    biluser.MessageAdd(modeluser);      //添加数据至receivedata
                }
            }
        
            //得到不重复的id列表，并加入数据库receiveid
            foreach (var item in idd)
            {
                if (!id.Contains(item))
                    id.Add(item);
            }

            
            foreach (var item in id)
            {
                modelid.ReceiveID = item;
                bilid.MessageAdd(modelid);
            }

            return id.ToArray();
        }

        /// <summary>
        /// 得到rssiaverage
        /// </summary>
        /// <param name="groupid"></param>
        public void AddData(string[] groupid)
        {
            List<string> rssi = new List<string>();

            if (groupid.Length >= 3)
            {
                foreach (var item in groupid) //对每个id求rssiaverage
                {
                     string strwhere = "id='" + item + "'";
                     List<MODEL.User> modellist = biluser.GetModelList(strwhere);
                     int sum = 0, rssiaverage;
                     foreach (MODEL.User p in modellist)
                     {
                        rssi.Add(p.RSSI);
                        //sum += Convert.ToInt32(p.RSSI);
                     }
                    /*rssiaverage = sum / modellist.Count;
                    modelaverage.ID = "" + item + "";
                    modelaverage.RSSI = rssiaverage.ToString();
                    bilaverage.MessageAdd(modelaverage);*/

                    string[] rssiarray = rssi.ToArray();
                    
                    int count = rssi.Count;
                    if (count >= 4)//收到rssi个数大于4，则取中间一部分的数求均值
                    {
                        for (int i = count / 4; i < count - count / 4; i++)
                        {
                            sum += Convert.ToInt32(rssiarray[i]);
                        }
                        rssiaverage = sum / (count - 2 * count / 4);
                        modelaverage.RSSI = rssiaverage.ToString();
                    }
                    else if (count == 1)//收到一个rssi值，则就用该值
                    {
                        rssiaverage = Convert.ToInt32(rssiarray[count]);
                        modelaverage.RSSI = rssiaverage.ToString();
                    }
                    else//收到大于1小于4个数的rssi，则求中位数
                    {

                        if (count % 2 == 0)
                        {
                            rssiaverage = (Convert.ToInt32(rssiarray[count/2 - 1]) + Convert.ToInt32(rssiarray[count/2])) / 2;
                        }
                        else
                            rssiaverage = Convert.ToInt32(rssiarray[count / 2]);

                        modelaverage.RSSI = rssiaverage.ToString();
                    }
                    modelaverage.ID = "" + item + "";
                    bilaverage.MessageAdd(modelaverage);
                    
                }

            }

        }

        /*public decimal GetMedian(decimal[] array)
        {

            int endIndex = array.Length / 2;

            for (int i = 0; i <= endIndex; i++)
            {
                for (int j = 0; j < array.Length - i - 1; j++)
                {
                    if ( array[j-1] < array[j])
                    {
                        decimal temp = array[j + 1];
                        array[j + 1] = array[j];
                        array[j] = temp;
                    }
                }
            }

            if (array.Length% 2 != 0)
            {
                return array[endIndex];
            }

            return (array[endIndex - 1] + array[endIndex]) / 2;
        }*/



    }
    
}

