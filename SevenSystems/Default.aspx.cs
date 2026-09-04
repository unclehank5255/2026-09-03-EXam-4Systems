using SevenSystems.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace SevenSystems
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect(ResolveUrl("~/User/Login.aspx"));
                return;
            }
            lbWelcome.Text = "歡迎使用者:" + Session["UserName"];
            hlAlbumAdmin.Visible =
       Session["IsAdmin"] != null &&
       Convert.ToBoolean(Session["IsAdmin"]);
            hpAlbumManage.Visible = hlAlbumAdmin.Visible;


        }
        protected void btLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect(ResolveUrl("~/User/Login.aspx"));
        }



    }
    
}
