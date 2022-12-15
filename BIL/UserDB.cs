using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using DAL;

namespace BIL
{
    public class UserDB
    {
        #region 基本方法
        /// <summary>
        /// 添加数据到数据库
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rssi"></param>
        /// <returns></returns>
        public bool MessageAdd(MODEL.User model)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into receivedata(id,rssireceive)");
            strSql.Append(" values(@id,@rssireceive)");
            MySqlParameter[] parameters = {
                new MySqlParameter("@id",MySqlDbType.VarChar,16),
                new MySqlParameter("@rssireceive",MySqlDbType.Int32,8)};
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
        /// 更新一条数据
        /// </summary>
        public bool Update(MODEL.User model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update receivedata set ");
            strSql.Append("id=@id,");
            strSql.Append("rssi=@rssireceive");
            strSql.Append(" where id=@id ");
            MySqlParameter[] parameters = {
                    new MySqlParameter("@id", MySqlDbType.VarChar,255),
                    new MySqlParameter("@rssireceive", MySqlDbType.Int32,8)};
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
        /// 删除一条数据
        /// </summary>
        public bool Delete(long id)
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from receivedata ");
            strSql.Append(" where id=@id");
            MySqlParameter[] parameters = {
                 new MySqlParameter("@@id", SqlDbType.VarChar)
            };

            parameters[0].Value = id;

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
        /// 得到一个对象实体
        /// </summary>
        public MODEL.User GetModel()
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id from receivedata ");
            strSql.Append(" where ");
            MySqlParameter[] parameters = {};

            DataSet ds = MySQLHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public MODEL.User DataRowToModel(DataRow row)
        {
            MODEL.User model = new MODEL.User();
            if (row != null)
            {
                if (row["id"] != null && row["id"].ToString() != "")
                {
                    model.ID = row["id"].ToString();
                }
                if (row["rssireceive"] != null)
                {
                    model.RSSI = row["rssireceive"].ToString();
                }

            }
            return model;
        }


        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,rssireceive from receivedata");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return MySQLHelper.Query(strSql.ToString());
        }

        public DataSet GetList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,rssireceive");
            return MySQLHelper.Query(strSql.ToString());
        }


        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T. desc");
            }
            strSql.Append(")AS Row, T.*  from user T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return MySQLHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得user数据列表
        /// </summary>
        public List<MODEL.User> GetModelList(string strWhere)
        {
            DataSet ds = GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得user数据列表
        /// </summary>
        public List<MODEL.User> DataTableToList(DataTable dt)
        {
            List<MODEL.User> modelList = new List<MODEL.User>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                MODEL.User model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = DataRowToModel(dt.Rows[n]);
                    if (model != null)
                    {
                        modelList.Add(model);
                    }
                }
            }
            return modelList;
        }

        #endregion
    }
}
