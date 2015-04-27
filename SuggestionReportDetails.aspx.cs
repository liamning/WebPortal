using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SuggestionReportDetails : System.Web.UI.Page
{

    public string reportStr;
    protected void Page_Load(object sender, EventArgs e)
    {
         
        string type = Request.QueryString["type"];
        string from = Request.QueryString["from"];
        string to = Request.QueryString["to"];

        string[] typeIDArray = type.Split(',');
        string typeDesc = "";

        if (type != "0")
        {
            foreach (string id in typeIDArray)
            {
                if (typeDesc == "")
                {
                    typeDesc = SystemPara.getDescription(Convert.ToInt32(id));
                }
                else
                {
                    typeDesc = typeDesc + ", " + SystemPara.getDescription(Convert.ToInt32(id));
                }

            }
        }


        DateTime fromDate = DateTime.MinValue;
        DateTime toDate = DateTime.MinValue;

        txtType.InnerText = typeDesc;
          
        DateTime.TryParseExact(from, "dd/MM/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out fromDate);
        DateTime.TryParseExact(to, "dd/MM/yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out toDate);

        if (fromDate == DateTime.MinValue && toDate == DateTime.MinValue)
        {
            txtDateRange.InnerText = "ALL";
        }
        else
        {
            txtDateRange.InnerText = (fromDate == DateTime.MinValue ? "" : fromDate.ToString("dd/MM/yyyy"))
                                    + " ~ "
                                    + (toDate == DateTime.MinValue ? "" : toDate.ToString("dd/MM/yyyy"));
        }

        reportStr = new Report().getSuggestionReport(type, fromDate, toDate,
                        Session["LOGINID"].ToString(), Session["USERGROUP"].ToString());


    }
}