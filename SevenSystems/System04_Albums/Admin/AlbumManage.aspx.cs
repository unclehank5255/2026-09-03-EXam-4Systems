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
    public partial class AlbumManage : System.Web.UI.Page
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
            if(!IsPostBack)
            {
                LoadCategories();
                LoadAlbums();
            }
        }
        private void LoadCategories()
        {
            string sql = @"
        SELECT Id, Name
        FROM AlbumCategories
        ORDER BY SortOrder, Id";

            DataTable dt = DbHelper.ExcuteQuery(sql, null);

            ddlCategory.DataSource = dt;
            ddlCategory.DataTextField = "Name";
            ddlCategory.DataValueField = "Id";
            ddlCategory.DataBind();
        }
        private void LoadAlbums()
        {
            string sql = @"
        SELECT
            A.Id,
            A.Title,
            C.Name AS CategoryName,
            A.CreatedAt
        FROM Albums A
        INNER JOIN AlbumCategories C
            ON C.Id = A.CategoryId
        ORDER BY A.Id DESC";

            DataTable dt = DbHelper.ExcuteQuery(sql, null);

            gvAlbums.DataSource = dt;
            gvAlbums.DataBind();
        }
        protected void btnAddAlbum_Click(object sender, EventArgs e)
        {
            string title = txtTitle.Text.Trim();

            if (string.IsNullOrWhiteSpace(title))
            {
                lblMessage.Text = "相簿標題不得為空";
                return;
            }

            if (!int.TryParse(ddlCategory.SelectedValue, out int categoryId))
            {
                lblMessage.Text = "請先建立並選擇相簿類別";
                return;
            }

            string sql = @"
        INSERT INTO Albums (CategoryId, Title)
        VALUES (@CategoryId, @Title)";

            SqlParameter[] parameters =
            {
        new SqlParameter("@CategoryId", SqlDbType.Int)
        {
            Value = categoryId
        },
        new SqlParameter("@Title", SqlDbType.NVarChar, 200)
        {
            Value = title
        }
    };

            int result = DbHelper.ExcuteNonQuery(sql, parameters);

            if (result > 0)
            {
                txtTitle.Text = "";
                lblMessage.Text = "相簿新增成功";
                LoadAlbums();
            }
            else
            {
                lblMessage.Text = "相簿新增失敗";
            }
        }

 

        protected void gvAlbums_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvAlbums.EditIndex = e.NewEditIndex;
            LoadAlbums();
        }

        protected void gvAlbums_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvAlbums.EditIndex = -1;
            LoadAlbums();
        }


        protected void gvAlbums_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int albumId = Convert.ToInt32(gvAlbums.DataKeys[e.RowIndex].Value);
            string title = Convert.ToString(e.NewValues["Title"]).Trim();
            string sql = @"UPDATE Albums SET Title = @Title WHERE Id = @Id";
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                   {
                    new SqlParameter("@Title", SqlDbType.NVarChar, 200)
                    {
                        Value = title
                    },
                    new SqlParameter("@Id", SqlDbType.Int)
                    {
                        Value = albumId
                    }
                   };
                int result = DbHelper.ExcuteNonQuery(sql, parameters);
                if (result > 0)
                {
                    gvAlbums.EditIndex = -1;
                    LoadAlbums();
                    lblMessage.Text = "相簿更新成功";
                }
                else
                {
                    lblMessage.Text = "相簿更新失敗";
                }
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                lblMessage.Text = "相簿標題已經存在";
                
            }
        }
        

        protected void gvAlbums_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int albumId = Convert.ToInt32(gvAlbums.DataKeys[e.RowIndex].Value);
            string sql = @"DELETE FROM Albums WHERE Id = @Id";
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                   {
                   
                    new SqlParameter("@Id", SqlDbType.Int)
                    {
                        Value = albumId
                    }
                   };
                int result = DbHelper.ExcuteNonQuery(sql, parameters);
                if (result > 0)
                {
                    gvAlbums.EditIndex = -1;
                    LoadAlbums();
                    lblMessage.Text = "相簿刪除成功";
                }
                else
                {
                    lblMessage.Text = "相簿刪除失敗";
                }
            }
            catch (SqlException ex) when (ex.Number == 547 )
            {
                lblMessage.Text = "相簿有相關照片，無法刪除";

            }

        }

    }
}