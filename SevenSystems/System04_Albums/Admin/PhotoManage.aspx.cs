using Antlr.Runtime.Misc;
using SevenSystems.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SevenSystems.System04_Albums.Admin
{
    public partial class PhotoManage : System.Web.UI.Page
    {
        //private int? CurrentAlbumId
        //{
        //    get
        //    {
        //        if (int.TryParse(Request.QueryString["albumId"], out int albumId))
        //            return albumId;

        //        return null;
        //    }
        //} 因為遇到很多次抓QueryString的 所以可以提出來讓大家直接抓統一位置

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect(ResolveUrl("~/User/Login.aspx"));
                return;
            }

            if (Session["IsAdmin"] == null ||
                !Convert.ToBoolean(Session["IsAdmin"]))
            {
                Response.Redirect(ResolveUrl("~/Default.aspx"));
                return;
            }
         //   Page.Form.Enctype = "multipart/form-data";
            if (!int.TryParse(Request.QueryString["albumId"], out int albumId))
            {
                Response.Redirect("AlbumManage.aspx");
                return;
            }
            if (!IsPostBack)
            {
                LoadAlbum(albumId);
                LoadPhotos(albumId);
            }
        }
        //載入標題
        private void LoadAlbum(int albumId)
        {
            //因為名稱在Album所以把兩個查詢用兩個方法表現出來
            string sql = @"
        SELECT Title
        FROM Albums
        WHERE Id = @AlbumId";

            SqlParameter[] parameters =
            {
        new SqlParameter("@AlbumId", SqlDbType.Int)
        {
            Value = albumId
        }
    };

            DataTable dt = DbHelper.ExcuteQuery(sql, parameters);

            if (dt.Rows.Count == 0)
            {
                Response.Redirect("AlbumManage.aspx");
                return;
            }

            lblAlbumTitle.Text = dt.Rows[0]["Title"].ToString();
        }
        private void LoadPhotos(int albumId)
        {
            string sql = @"
        SELECT Id, FileName, Caption, IsCover, CreatedAt
        FROM AlbumPhotos
        WHERE AlbumId = @AlbumId
        ORDER BY IsCover DESC, SortOrder, Id";

            SqlParameter[] parameters =
            {
        new SqlParameter("@AlbumId", SqlDbType.Int)
        {
            Value = albumId
        }
    };
            DataTable dt = DbHelper.ExcuteQuery(sql, parameters);

            gvPhotos.DataSource = dt;
            gvPhotos.DataBind();
        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(Request.QueryString["albumId"], out int albumId))
            {
                Response.Redirect("AlbumManage.aspx");
                return;
            }
            if (!fuPhoto.HasFile)
            {
                lblMessage.Text = "請選擇圖片";
                return;
            }

            //統一轉換成小寫
            string extension =
                Path.GetExtension(fuPhoto.FileName).ToLowerInvariant();

            string[] allowedExtensions =
            {
        ".jpg", ".jpeg", ".png", ".webp"
    };

            if (!allowedExtensions.Contains(extension))
            {
                lblMessage.Text = "只允許 JPG、PNG 或 WEBP 圖片";
                return;
            }
            if (fuPhoto.PostedFile.ContentLength > 5 * 1024 * 1024)
            {
                lblMessage.Text = "圖片不可超過 5 MB";
                return;
            }
            // 產生唯一檔名，避免檔名衝突
            string fileName =
                Guid.NewGuid().ToString("N") + extension;
            string uploadFolder =
                Server.MapPath("~/Uploads/Albums/" + albumId);
            // 建立資料夾但不會重複建立
            Directory.CreateDirectory(uploadFolder);
            string physicalPath =
                Path.Combine(uploadFolder, fileName);

            fuPhoto.SaveAs(physicalPath);

            string sql = @"
        INSERT INTO AlbumPhotos
            (AlbumId, FileName, Caption, IsCover)
        SELECT
            @AlbumId,
            @FileName,
            @Caption,
            CASE
                WHEN EXISTS
                (
                    SELECT 1
                    FROM AlbumPhotos
                    WHERE AlbumId = @AlbumId
                )
                THEN 0
                ELSE 1
            END";

            SqlParameter[] parameters =
            {
        new SqlParameter("@AlbumId", SqlDbType.Int)
        {
            Value = albumId
        },
        new SqlParameter("@FileName", SqlDbType.NVarChar, 260)
        {
            Value = fileName
        },
        new SqlParameter("@Caption", SqlDbType.NVarChar, 500)
        {
            Value = txtCaption.Text.Trim()
        }
    };

            int result = DbHelper.ExcuteNonQuery(sql, parameters);

            if (result > 0)
            {
                txtCaption.Text = "";
                lblMessage.Text = "照片上傳成功";
                LoadPhotos(albumId);
            }
            else
            {
                lblMessage.Text = "照片上傳失敗";
            }
        }

        protected string GetPhotoUrl(object fileName)
        {
            string albumId = Request.QueryString["albumId"];

            return ResolveUrl(
                "~/Uploads/Albums/" +
                albumId + "/" +
                fileName.ToString()
            );
        }
        protected void gvPhotos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "SetCover")
            {
                return;
            }
            
            if (!int.TryParse(
                Request.QueryString["albumId"],
                out int albumId))
            {
                Response.Redirect("AlbumManage.aspx");
                return;
            }

            int rowIndex =
                Convert.ToInt32(e.CommandArgument);

            int photoId =
                Convert.ToInt32(
                    gvPhotos.DataKeys[rowIndex].Value
                );

            string sql = @"
        UPDATE AlbumPhotos
        SET IsCover =
            CASE
                WHEN Id = @PhotoId THEN 1
                ELSE 0
            END
        WHERE AlbumId = @AlbumId";

            SqlParameter[] parameters =
            {
        new SqlParameter("@PhotoId", SqlDbType.Int)
        {
            Value = photoId
        },
        new SqlParameter("@AlbumId", SqlDbType.Int)
        {
            Value = albumId
        }
    };

            DbHelper.ExcuteNonQuery(sql, parameters);

            lblMessage.Text = "封面設定成功";
            LoadPhotos(albumId);
        }

        protected void gvPhotos_RowEditing(object sender, GridViewEditEventArgs e)
        {
            if (!int.TryParse(Request.QueryString["albumId"], out int albumId))
            {
                Response.Redirect("AlbumManage.aspx");
                return;
            }
            gvPhotos.EditIndex = e.NewEditIndex;// 明確指定要編輯的行索引
            gvPhotos.Columns[6].Visible = false;

            LoadPhotos(albumId);
        }

        protected void gvPhotos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            if (!int.TryParse(Request.QueryString["albumId"], out int albumId))
            {
                Response.Redirect("AlbumManage.aspx");
                return;
            }
            gvPhotos.EditIndex = -1;
            gvPhotos.Columns[6].Visible = true;

            LoadPhotos(albumId);
        }

        protected void gvPhotos_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (!int.TryParse(Request.QueryString["albumId"], out int albumId))
            {
                Response.Redirect("AlbumManage.aspx");
                return;
            }

            int photoId = Convert.ToInt32(gvPhotos.DataKeys[e.RowIndex].Value);

            string caption = e.NewValues["Caption"].ToString();

            string sql = "UPDATE AlbumPhotos SET Caption=@Caption WHERE Id=@Id";
            try
            {                 
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Caption", SqlDbType.NVarChar, 100)
                    {
                        Value = caption
                    },
                    new SqlParameter("@Id", SqlDbType.Int)
                    {
                        Value = photoId
                    }
                };
                int result = DbHelper.ExcuteNonQuery(sql, parameters);
                if (result > 0)
                {
                    lblMessage.Text = "照片更新成功";
                    gvPhotos.EditIndex = -1;
                    LoadPhotos(albumId);
                }
                else
                {
                    lblMessage.Text = "照片更新失敗";
                }
            }
            catch (SqlException ex)
             when(ex.Number == 2627 || ex.Number == 2601)
               {
                    lblMessage.Text = "照片已經存在 ";
                    
               }
            LoadPhotos(albumId);

        }


        protected void gvPhotos_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (!int.TryParse(Request.QueryString["albumId"], out int albumId))
            {
                Response.Redirect("AlbumManage.aspx");
                return;
            }

            int photoId = Convert.ToInt32(gvPhotos.DataKeys[e.RowIndex].Value);


            string sql = "DELETE FROM AlbumPhotos WHERE Id=@Id";
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {

                    new SqlParameter("@Id", SqlDbType.Int)
                    {
                        Value = photoId
                    }
                };
                int result = DbHelper.ExcuteNonQuery(sql, parameters);
                if (result > 0)
                {
                    lblMessage.Text = "照片刪除成功";
                    gvPhotos.EditIndex = -1;
                    LoadPhotos(albumId);
                }
                else
                {
                    lblMessage.Text = "照片刪除失敗";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "錯誤: " + ex;
            }
            LoadPhotos(albumId);
            

        }

    }
}