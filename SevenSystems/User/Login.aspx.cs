using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace SevenSystems.User
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;

            string password = txtPassword.Text;
            string connectionString = ConfigurationManager.ConnectionStrings["BoardDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "SELECT * FROM Users WHERE UserName = @UserName AND Password = @Password";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@UserName",SqlDbType.NVarChar,50).Value=username.Trim();
                    cmd.Parameters.Add("@Password",SqlDbType.NVarChar,50).Value=password.Trim();
                    bool loginSuccess = false;
                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Session["UserID"] = reader["ID"];
                                Session["UserName"] = reader["UserName"].ToString();
                                Session["IsAdmin"] = Convert.ToBoolean(reader["IsAdmin"]);
                                loginSuccess = true;
                            }
                            else
                            {
                                lbMessage.Text = "帳號或密碼錯誤";
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        lbMessage.Text = "資料庫錯誤:"+ex;
                    }
                    catch (Exception ex)
                    {
                        lbMessage.Text = "系統錯誤:"+ex;
                    }
                    if (loginSuccess)
                    {
                        Response.Redirect(ResolveUrl("~/Default.aspx"));
                    }

                }
            }

        }

        protected void btRegister_Click(object sender, EventArgs e)
        {
            Response.Redirect(ResolveUrl("~/User/Register.aspx")); 
        }
    }
}