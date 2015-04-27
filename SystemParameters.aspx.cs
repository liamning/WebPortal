using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SystemParameters : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.ControlDataBind();
    }

    protected void ControlDataBind()
    {
        System.Data.DataTable dt = SystemPara.getSystemParaType();

        foreach (System.Data.DataRow row in dt.Rows)
        {
            txtSystemType.Items.Add(new ListItem(row["categorydesc"].ToString(), row["category"].ToString()));
             
        }

    }
}