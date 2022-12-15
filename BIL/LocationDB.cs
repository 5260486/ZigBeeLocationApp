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
    public class LocationDB
    {
        public bool MessageAdd(MODEL.Location model)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into location(X,Y)");
            strSql.Append(" values(@X,@Y)");
            MySqlParameter[] parameters = {
                new MySqlParameter("@X",MySqlDbType.Double,16),
                new MySqlParameter("@Y",MySqlDbType.Double,16)};
            parameters[0].Value = model.X;
            parameters[1].Value = model.Y;

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
    }
}
