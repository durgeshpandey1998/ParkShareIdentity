using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace ParkShareIdentity.Service
{
    public static class MainClass
    {
        public const string GetVerificationList = @"SelectData";
        public static async Task<DataSet> GetQueryDatatableAsync(this Microsoft.EntityFrameworkCore.DbContext context, string sqlQuery, SqlParameter[] sqlParam = null, CommandType type = CommandType.StoredProcedure)
        {
            try
            {
                using (DbCommand cmd = context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.Connection = context.Database.GetDbConnection();
                    cmd.CommandType = type;

                    //if (sqlParam != null)
                    //{ cmd.Parameters.AddRange(sqlParam); }
                    cmd.CommandText = sqlQuery;
                    using (DbDataAdapter dataAdapter = new SqlDataAdapter())
                    {
                        dataAdapter.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        await Task.Run(() => dataAdapter.Fill(ds));
                        var data = ds;
                        return ds;

                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
