using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using DAL;

namespace BIL
{
    public class CoordinateDB
    {
        public MODEL.Coordinate DataRowToID(DataRow row)
        {
            MODEL.Coordinate coordinate = new MODEL.Coordinate();
            if (row != null)
            {
                //if (row["id"] != null && row["id"].ToString() != "")
                //{
                //    coordinate.ID = row["id"].ToString();
                //}
                if (row["X"] != null)
                {
                    coordinate.XAxis =Convert.ToDouble( row["X"]);
                }
                if (row["Y"] != null)
                {
                    coordinate.YAxis = Convert.ToDouble(row["Y"]);
                }
            }

            return coordinate;
        }

        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select X,Y from coordinate ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            return MySQLHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得坐标数据列表
        /// </summary>
        public List<MODEL.Coordinate> GetCoorList(string strWhere)
        {
            DataSet ds = GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得坐标数据列表
        /// </summary>
        public List<MODEL.Coordinate> DataTableToList(DataTable dt)
        {
            List<MODEL.Coordinate> modelList = new List<MODEL.Coordinate>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                MODEL.Coordinate model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = DataRowToID(dt.Rows[n]);
                    if (model != null)
                    {
                        modelList.Add(model);
                    }
                }
            }
            return modelList;
        }



    }
}
