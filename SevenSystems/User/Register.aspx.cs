using System;
using System.Configuration;
using System.Data.SqlClient;

namespace SevenSystems.User
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password))
            {
                lbMessage.Text = "註冊失敗，欄位不得為空";
                return;
            }

            string connectionString =
                ConfigurationManager.ConnectionStrings["BoardDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql =
                    "INSERT INTO Users (UserName, Password) " +
                    "VALUES (@UserName, @Password)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserName", username);
                    cmd.Parameters.AddWithValue("@Password", password);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();

                        Response.Redirect(
                            ResolveUrl("~/User/Login.aspx"));
                    }
                    catch (SqlException)
                    {
                        lbMessage.Text = "註冊失敗，帳號可能已經存在";
                    }
                }
            }
        }

        protected void btToLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect(
                ResolveUrl("~/User/Login.aspx"));
        }
    }
}