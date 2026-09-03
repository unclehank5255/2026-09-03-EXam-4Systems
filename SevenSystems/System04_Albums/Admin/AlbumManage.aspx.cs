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
    }
}