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
    public class AverageDB
    {
        public bool MessageAdd(MODEL.Average model)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into rssiaverage(id,rssiaverage)");
            strSql.Append("values(@id,@rssiaverage)");
            MySqlParameter[] parameters = {
                new MySqlParameter("@id",MySqlDbType.VarChar,16),
                new MySqlParameter("@rssiaverage",MySqlDbType.Int32,8)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.RSSI;

            int rows = MySQLHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 排序后只取前三条
        /// </summary>
        public bool OrderBy()
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT id,rssiaverage");
            strSql.Append(" FROM rssiaverage");
            strSql.Append(" ORDER BY rssiaverage DESC");
            strSql.Append(" LIMIT 3");

            int rows = MySQLHelper.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public MODEL.Average DataRowToID(DataRow row)
        {
            MODEL.Average average= new MODEL.Average();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    average.ID = row["id"].ToString();
                }
                if (row["rssiaverage"] != null)
                {
                    average.RSSI = row["rssiaverage"].ToString();
                }
            }

            return average;
        }

        public DataSet GetList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,rssiaverage from rssiaverage ");

            return MySQLHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得ID数据列表
        /// </summary>
        public List<MODEL.Average> GetAverageList()
        {
            DataSet ds = GetList();
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得ID数据列表
        /// </summary>
        public List<MODEL.Average> DataTableToList(DataTable dt)
        {
            List<MODEL.Average> modelList = new List<MODEL.Average>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                MODEL.Average model;
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
