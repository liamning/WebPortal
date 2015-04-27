using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SystemLinkManager : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.setMaxLength();
        }
    }
    
    protected void setMaxLength()
    {
        txtName.Attributes["maxLength"] = GlobalSetting.FieldLength.SystemLink.Name;
        txtLink.Attributes["maxLength"] = GlobalSetting.FieldLength.SystemLink.Link;
    }
}