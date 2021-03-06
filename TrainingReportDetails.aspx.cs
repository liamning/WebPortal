﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TrainingReportDetails : System.Web.UI.Page
{
    public string reportStr;
    protected void Page_Load(object sender, EventArgs e)
    {
        initControl();
        string serialNo = Request.QueryString["serialNo"];
        string type = Request.QueryString["type"];
        string decision = Request.QueryString["decision"];
        string name = Request.QueryString["name"];
        string from = Request.QueryString["from"];
        string to = Request.QueryString["to"];
         
        string typeDesc = "";
        string decisionDesc = "";

        if (type != null && type != "0")
        {
            string[] typeIDArray = type.Split(',');
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
        else if (type == null) typeDesc = "ALL";
        if (decision != null && decision != "'0'")
        {
            string[] decisionArray = decision.Split(',');
            foreach (string decisionItem in decisionArray)
            {
                if (decisionDesc == "")
                {
                    decisionDesc = (decisionItem == "'NotAttend'" ? "Not Attend" : decisionItem.Replace("'",""));
                }
                else
                {
                    decisionDesc = decisionDesc + ", " + (decisionItem == "'NotAttend'" ? "Not Attend" : decisionItem.Replace("'", ""));
                }

            }
        }
        else if (decision == null) decisionDesc = "ALL";
        

        DateTime fromDate = DateTime.MinValue;
        DateTime toDate = DateTime.MinValue;

        txtType.InnerText = typeDesc;
        txtDecision.InnerText = decisionDesc;

        if (string.IsNullOrEmpty(serialNo))
        {
            txtSerialNo.InnerText = "ALL";
        }
        else
        {
            txtSerialNo.InnerText = serialNo;
        }
        if (string.IsNullOrEmpty(name))
        {
            txtName.InnerText = "ALL";
        }
        else
        {
            txtName.InnerText = name;
        }

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

        reportStr = new Report().getTrainingReport(serialNo, type, decision, name, fromDate, toDate,
                        Session["LOGINID"].ToString(), Session["USERGROUP"].ToString());


    }

    
    private void initControl()
    {
        string userGroup = Session["USERGROUP"].ToString();
        if (userGroup == GlobalSetting.SystemRoles.Normal)
        {
            liType.Attributes.Add("style", "display:none");
            liSerialNo.Attributes.Add("style", "display:none");
        }

    }
}