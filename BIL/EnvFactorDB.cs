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
    public class EnvFactorDB
    {
        public MODEL.EnvFactor DataRowToEnv(DataRow row)
        {
            MODEL.EnvFactor model = new MODEL.EnvFactor();
            if (row != null)
            {
                if (row["height"] != null && row["height"].ToString() != "")
                {
                    model.Height = Convert.ToDouble(row["height"]);
                }
                if (row["n"] != null)
                {
                    model.N = Convert.ToDouble(row["n"]);
                }
                if (row["p0"] != null)
                {
                    model.P0 = Convert.ToDouble(row["p0"]);
                }

            }
            return model;
        }

        public DataSet GetList()
        {
            StringBuilder strSql = new StringBuilder();
            //strSql.Append("select id,rssireceive");
            strSql.Append("select height,n,p0 from envfactors ");

            return MySQLHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得环境因子数据列表
        /// </summary>
        public List<MODEL.EnvFactor> GetEnvList()
        {
            DataSet ds = GetList();
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得环境因子数据列表
        /// </summary>
        public List<MODEL.EnvFactor> DataTableToList(DataTable dt)
        {
            List<MODEL.EnvFactor> modelList = new List<MODEL.EnvFactor>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                MODEL.EnvFactor model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = DataRowToEnv(dt.Rows[n]);
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
