using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class LinkIconManager : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            this.initControl();
            this.ControlDataBind();
        }
    }

    private void initControl()
    {
        

    }
    private void ControlDataBind()
    {
         
         
    }
}