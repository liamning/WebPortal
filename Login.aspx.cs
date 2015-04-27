using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        //register the user
        String userName = Request.Form["txtUserName"];
        String password = Request.Form["txtPassword"];

        User userHandler = new User();
        DetailUserInfo userinfo = userHandler.login(userName, password);
        if (userinfo != null)
        {
            Session["LOGINID"] = userinfo.ID;
            Session["LOGINNAME"] = userName;
            Session["USERGROUP"] = userinfo.UserGroup; ;

            if (!Request.UrlReferrer.LocalPath.ToLower().Contains("login.aspx") &&
                !Request.UrlReferrer.LocalPath.ToLower().Contains("register.aspx"))
            {
                Response.Redirect(Request.UrlReferrer.AbsoluteUri);
            }

            //routing
            switch (userinfo.UserGroup)
            {
                case GlobalSetting.SystemRoles.Admin:
                    Response.Redirect("~/Approve.aspx");
                    break;
                default:
                    Response.Redirect("~/Home.aspx");
                    break;

            }
        }
    }
}