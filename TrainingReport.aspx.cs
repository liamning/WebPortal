using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TrainingReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            this.initControl();
            this.ControlDataBind();

            string data = new Report().getReportHTML("http://localhost:81/test/TrainingReportDetails.aspx?serialNo=T-GL-TRAINING-03");
            new User().sendMail("ning.cheung@flexsystem.com", "test123", data );
        }
    }

    private void initControl()
    {
        string userGroup = Session["USERGROUP"].ToString();
        if (userGroup == GlobalSetting.SystemRoles.Normal)
        {
            trSerial.Attributes.Add("style", "display:none");
            trType.Attributes.Add("style", "display:none");
        }

    }
    private void ControlDataBind()
    {

        txtSerialNo0.Items.Add(new ListItem("All", "0"));
        System.Data.DataTable dt = SystemPara.getSystemPara("TrainingType", true);
        foreach(System.Data.DataRow row in dt.Rows)
        {
            txtSerialNo0.Items.Add(new ListItem(row["cDescription"].ToString(), row["cDescription"].ToString()));
        }
         
    }
}