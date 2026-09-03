using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SevenSystems.Model;
using System.Data;
using System.Data.SqlClient;

namespace SevenSystems.System04_Albums
{
    public partial class AlbumDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect(ResolveUrl("~/User/Login.aspx"));
                return;
            }
            string albumId = Request.QueryString["albumId"];
            if(!int.TryParse(albumId, out int albumIdValue))
            {
                lbAlbumTitle.Text="相簿編號錯誤";
                return;
            }
            string sql = "SELECT Title FROM Albums WHERE Id = @AlbumId";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@AlbumId", SqlDbType.Int)
                {
                    Value = albumIdValue
                }
            };
            
             DataTable dt = DbHelper.ExcuteQuery(sql, parameters);
            if(dt!=null&&dt.Rows.Count>0)
            {
                lbAlbumTitle.Text = dt.Rows[0]["Title"].ToString();
                LoadPhotos(albumIdValue);
            }
            else 
            { 
                lbAlbumTitle.Text = "找不到相簿"; 
            }
        }

        private void LoadPhotos(int albumId)
        {
            // Implementation for loading photos
            string sql = "SELECT Id, FileName, Caption FROM AlbumPhotos WHERE AlbumId=@AlbumId";
            SqlParameter[] parameters =new SqlParameter[]
            {
                new SqlParameter("@AlbumId", SqlDbType.Int)
                {
                    Value = albumId
                }
            };
            DataTable dt = DbHelper.ExcuteQuery(sql, parameters);
            if(dt != null && dt.Rows.Count > 0)
            {
                rptPhotos.DataSource = dt;
                rptPhotos.DataBind();
            }
            else
            {
                // Handle case when there are no photos
                rptPhotos.DataSource = null;
                rptPhotos.DataBind();
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
    }
}