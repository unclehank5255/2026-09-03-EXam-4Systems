using SevenSystems.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace SevenSystems.System04_Albums
{
    public partial class AlbumList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect(ResolveUrl("~/User/Login.aspx"));
                return;
            }
            if(!IsPostBack)
            {
                LoadAlbums();
            }

        }
        private void LoadAlbums()
        {
            string sql = @"
    SELECT
        A.Id,
        A.Title,
        AP.FileName AS CoverFileName
    FROM Albums A
    LEFT JOIN AlbumPhotos AP
        ON AP.AlbumId = A.Id
        AND AP.IsCover = 1
    ORDER BY A.SortOrder, A.Id"; 
            DataTable dt = DbHelper.ExcuteQuery(sql, null);

            rptAlbums.DataSource = dt;
            rptAlbums.DataBind();
        }

        protected bool HasCover(object fileName)
        {
            return fileName != null &&
                   fileName != DBNull.Value &&
                   !string.IsNullOrWhiteSpace(fileName.ToString());
        }

        protected string GetCoverUrl(object albumId, object fileName)
        {
            return ResolveUrl(
                "~/Uploads/Albums/" +
                albumId + "/" +
                fileName
            );
        }
    }
}