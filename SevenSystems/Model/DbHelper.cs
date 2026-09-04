using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace SevenSystems.Model
{
    public class DbHelper
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["BoardDB"].ConnectionString;
        public static DataTable ExcuteQuery(string sql, SqlParameter[] parameters) { 
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
                if (parameters != null)
                {
                    adapter.SelectCommand.Parameters.AddRange(parameters);
                }
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            } 
        }

        public static int ExcuteNonQuery(string sql, SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using(SqlCommand  cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }
    }
}