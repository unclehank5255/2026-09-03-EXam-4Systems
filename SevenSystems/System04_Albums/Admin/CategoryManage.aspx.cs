using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SevenSystems.Model;
using System.Data.SqlClient;

namespace SevenSystems.System04_Albums.Admin
{
    public partial class CategoryManage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect(ResolveUrl("~/User/Login.aspx"));
                return;
            }
            if (Session["IsAdmin"] == null || !Convert.ToBoolean(Session["IsAdmin"]))
            {
                Response.Redirect(ResolveUrl("~/Default.aspx"));
                return;
            }
            if (!IsPostBack)
            {
                LoadCategories();
            }

        }

        private void LoadCategories()
        {
            string sql = "SELECT Id,Name FROM AlbumCategories ORDER BY Id";
            DataTable dt = DbHelper.ExcuteQuery(sql, null);
            gvCategories.DataSource = dt;
            gvCategories.DataBind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string categoryName = txtCategoryName.Text.Trim();
            string sortOrderText = txtSortOrder.Text.Trim();
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                lbMessage.Text = "類別名稱不得為空";
                return;
            }

            if (!int.TryParse(sortOrderText, out int sortOrder) || sortOrder < 0)
            {
                lbMessage.Text = "排序必須是大於或等於 0 的整數";
                return;
            }
            string sql = "INSERT INTO AlbumCategories(Name,SortOrder) VALUES (@Name,@SortOrder)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                    new SqlParameter("@Name", SqlDbType.NVarChar, 100)
            {
                  Value = categoryName
                 },
                    new SqlParameter("@SortOrder", SqlDbType.Int)
             {
                     Value = sortOrder
                }

            };
            try
            {
                int result =DbHelper.ExcuteNonQuery(sql, parameters);
                if (result > 0)
                {
                    txtCategoryName.Text = "";
                    txtSortOrder.Text = "";
                    lbMessage.Text = "類別新增成功";
                    LoadCategories();

                }
                else
                {
                    lbMessage.Text = "類別新增失敗";
                    return;
                }
            }
            catch (SqlException ex)
             when(ex.Number == 2627 || ex.Number == 2601)
               {
                    lbMessage.Text = "類別已經存在 ";
                    
               }
                
            

            
        }
    }
}
