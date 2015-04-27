using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Register : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
 
    }
    protected void btnRegister_Click(object sender, EventArgs e)
    {
        //get the user information from user input
        DetailUserInfo user = new DetailUserInfo();
        user.Name = Request.Form["txtUserName"];
        user.Password = Request.Form["txtPassword"];
        user.Age = Convert.ToInt16(Request.Form["txtAge"]);
        user.Sex = Convert.ToInt16(Request.Form["ddlSex"]);
        user.Post = Request.Form["ddlPost"];
        user.Department = Request.Form["ddlDepartment"];
        user.UserGroup = GlobalSetting.SystemRoles.Normal;

        //register the user
        User userHandler = new User();
        bool result = userHandler.register(user);
        if (result)
        {
            //Server.Transfer("~/login.aspx");
            ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "error", "alert('Your registry is being processed, please wait.')", true);
        }
        else
        {
            ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "error", "alert('Duplicated user name')", true);
        }

        

    }


}