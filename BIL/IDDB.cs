using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using DAL;

namespace BIL
{
    public class IDDB
    {
        public bool MessageAdd(MODEL.ID model)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into receiveid(id)");
            strSql.Append("values(@id)");
            MySqlParameter[] parameters = {
                new MySqlParameter("@id",MySqlDbType.VarChar,16)};
            parameters[0].Value = model.ReceiveID;

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

        public MODEL.ID DataRowToID(DataRow row)
        {
            MODEL.ID id = new MODEL.ID();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    id.ReceiveID = row["id"].ToString();
                }

            }
            return id;
        }

        public DataSet GetList()
        {
            StringBuilder strSql = new StringBuilder();
            //strSql.Append("select id,rssireceive");
            strSql.Append("select id from receiveid ");

            return MySQLHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得ID数据列表
        /// </summary>
        public List<MODEL.ID> GetIDList()
        {
            DataSet ds = GetList();
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得ID数据列表
        /// </summary>
        public List<MODEL.ID> DataTableToList(DataTable dt)
        {
            List<MODEL.ID> modelList = new List<MODEL.ID>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                MODEL.ID model;
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
