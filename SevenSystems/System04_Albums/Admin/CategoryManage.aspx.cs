using SevenSystems.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

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


        protected void gvCategories_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvCategories.EditIndex = e.NewEditIndex;// 明確指定要編輯的行索引
            gvCategories.Columns[1].Visible = false;
            LoadCategories();
        }

        protected void gvCategories_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvCategories.EditIndex = -1;
            gvCategories.Columns[1].Visible = true;

            LoadCategories();
        }

        protected void gvCategories_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int categoryId = Convert.ToInt32(gvCategories.DataKeys[e.RowIndex].Value);

            string name = e.NewValues["Name"].ToString();

            lbMessage.Text = "categoryId =" + categoryId + "，新名稱=" + name;
            string sql = "UPDATE AlbumCategories SET Name=@Name WHERE Id=@Id";

            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Name", SqlDbType.NVarChar, 100)
                    {
                        Value = name
                    },
                    new SqlParameter("@Id", SqlDbType.Int)
                    {
                        Value = categoryId
                    }
                };
                int result = DbHelper.ExcuteNonQuery(sql, parameters);
                if (result > 0)
                {
                    lbMessage.Text = "類別更新成功";
                }
                else
                {
                    lbMessage.Text = "類別更新失敗";
                }
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                lbMessage.Text = "類別已經存在";
            } catch (Exception ex)
            {
                lbMessage.Text = "更新類別時發生錯誤:" + ex.Message;
            }
            LoadCategories();
        }


        protected void gvCategories_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int categoryId = Convert.ToInt32(gvCategories.DataKeys[e.RowIndex].Value);
            lbMessage.Text = "拿到的ID是" + categoryId;
            string sql = "DELETE FROM AlbumCategories WHERE Id=@Id";
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {

                    new SqlParameter("@Id", SqlDbType.Int)
                    {
                        Value = categoryId
                    }
                };
                int result = DbHelper.ExcuteNonQuery(sql, parameters);
                if (result > 0)
                {
                    lbMessage.Text = "類別刪除成功";
                }
                else
                {
                    lbMessage.Text = "類別刪除失敗";
                }
            }
         // 刪除不會遇到重複類別的問題
            catch (SqlException ex) when (ex.Number == 547)
            {
                lbMessage.Text = "此類別已有相簿使用，無法刪除";
            }
            catch (Exception ex)
            {
                lbMessage.Text = "刪除類別時發生錯誤:" + ex.Message;
            }
            LoadCategories();
        }



    }
    
}
