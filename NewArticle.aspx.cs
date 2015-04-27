using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class NewArticle : System.Web.UI.Page
{

    public string mode;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.EnableViewState = false;
        if (!IsPostBack)
        {
            this.setMaxLength();
            this.ControlDataBind();
            this.controlValueInit();

            int newID = Convert.ToInt32(Request.QueryString["ID"]);
            if (newID > 0)
            {
                this.mode = "Edit";
                this.initArticle(newID);
                btnEdit.Visible = true;
                btnSave.Visible = false;
            }
            else
            {
                this.mode = "New";
                btnEdit.Visible = false;
                btnSave.Visible = true;
            }

        }
    }

    protected void initArticle(int ID)
    {

        hdfID.Value = ID.ToString();

        News newsHandler = new News();
        NewsInfo newInfo = newsHandler.getArticle(ID);
        
        txtVersionNo.InnerText = newInfo.VersionNo.ToString("N");
        
        string[] serialNos = newInfo.SerialNo.Split('-');
        txtSerialNo0.InnerText = serialNos[0] + "-" + serialNos[1];
        txtSerialNo1.Value = serialNos[2];
        txtSerialNo2.Value = serialNos[3];

       // comType.Value = newInfo.Type;

        bool isAbandoned = true;
        foreach (ListItem item in comType.Items)
        {
            if (item.Value == newInfo.Type.ToString())
            {
                item.Selected = true;
                isAbandoned = false;
                break;
            }
            else
            {
                item.Selected = false;
            }
        }
        if (isAbandoned)
        {
            string description = SystemPara.getDescription(newInfo.Type);
            comType.Items.Add(new ListItem(description, newInfo.Type.ToString()));
            comType.Value = newInfo.Type.ToString();
        }


        txtTitle.Value = newInfo.Title;
        txtHeadline.Value = newInfo.Headline;
        txtSummary.Value = newInfo.Summary;
        txtMainBody.Value = newInfo.Content;

        ddlYear.SelectedIndex = -1;
        ddlMonth.SelectedIndex = -1;
        ddlDay.SelectedIndex = -1;
        foreach(ListItem item in ddlYear.Items)
        {
            if(item.Value == newInfo.EffectiveDate.Year.ToString())
            {
                item.Selected = true;
                break;
            }
        }
        foreach (ListItem item in ddlMonth.Items)
        {
            if (item.Value == newInfo.EffectiveDate.Month.ToString("00"))
            {
                item.Selected = true;
                break;
            }
        }
        foreach (ListItem item in ddlDay.Items)
        {
            if (item.Value == newInfo.EffectiveDate.Day.ToString("00"))
            {
                item.Selected = true;
                break;
            }
        }


        if (newInfo.NewIconInfo != null)
        {

            ddlIconExpiryYear.SelectedIndex = -1;
            ddlIconExpiryMonth.SelectedIndex = -1;
            ddlIconExpiryDay.SelectedIndex = -1;
            foreach (ListItem item in ddlIconExpiryYear.Items)
            {
                if (item.Value == newInfo.NewIconInfo.ExpiryDate.Year.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
            foreach (ListItem item in ddlIconExpiryMonth.Items)
            {
                if (item.Value == newInfo.NewIconInfo.ExpiryDate.Month.ToString("00"))
                {
                    item.Selected = true;
                    break;
                }
            }
            foreach (ListItem item in ddlIconExpiryDay.Items)
            {
                if (item.Value == newInfo.NewIconInfo.ExpiryDate.Day.ToString("00"))
                {
                    item.Selected = true;
                    break;
                }
            }
             

        }

    }

    protected void ControlDataBind()
    {
        int currentYear = DateTime.Now.Year;

        ddlIconExpiryYear.Items.Add(new ListItem("", "0"));
        ddlIconExpiryMonth.Items.Add(new ListItem("", "0"));
        ddlIconExpiryDay.Items.Add(new ListItem("", "0"));

        for (int i = currentYear - 10; i < currentYear + 10; i++)
        {
            ddlYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
            ddlIconExpiryYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
        }
        for (int i = 1; i <= 12; i++)
        {
            ddlMonth.Items.Add(new ListItem(i.ToString("00"), i.ToString("00")));
            ddlIconExpiryMonth.Items.Add(new ListItem(i.ToString("00"), i.ToString("00")));
        }
        for (int i = 1; i <= 31; i++)
        {
            ddlDay.Items.Add(new ListItem(i.ToString("00"), i.ToString("00")));
            ddlIconExpiryDay.Items.Add(new ListItem(i.ToString("00"), i.ToString("00")));
        }

        System.Data.DataTable dataSource = SystemPara.getSystemPara("NewsType");

        foreach (System.Data.DataRow row in dataSource.Rows)
        {
            this.comType.Items.Add(new ListItem(row["Description"].ToString(), row["ID"].ToString()));
        }
    }

    protected void controlValueInit()
    {
        ddlYear.Value = DateTime.Now.Year.ToString();
        ddlMonth.Value = DateTime.Now.Month.ToString("00");
        ddlDay.Value = DateTime.Now.Day.ToString("00");
    }

    protected void setMaxLength()
    {
        txtSerialNo1.Attributes["maxLength"] = GlobalSetting.FieldLength.SerialNo1;
        txtSerialNo2.Attributes["maxLength"] = GlobalSetting.FieldLength.SerialNo2;
        txtTitle.Attributes["maxLength"] = GlobalSetting.FieldLength.News.Title;
        txtHeadline.Attributes["maxLength"] = GlobalSetting.FieldLength.News.Headline;
        txtSummary.Attributes["maxLength"] = GlobalSetting.FieldLength.News.Summary;
        txtMainBody.Attributes["maxLength"] = GlobalSetting.FieldLength.News.MainBody;
    }
}