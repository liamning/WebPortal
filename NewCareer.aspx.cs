using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class NewCareer : System.Web.UI.Page
{
    public string mode;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.setMaxLength();
            this.ControlDataBind();
            if (!string.IsNullOrEmpty(Request.QueryString["ID"]))
            {
                int ID = Convert.ToInt32(Request.QueryString["ID"]);
                this.mode = "Edit";
                this.initCareer(ID);
            }
            else
            {
                this.mode = "New";
                string disclaimer = SystemPara.getSystemPara("Disclaimer", false).Rows[0]["Description"].ToString();
                txtDisclaimer.InnerText = disclaimer;
            }
        }
    }
    protected void initCareer(int ID)
    {
        hdfID.Value = ID.ToString();
        CareerOpportunity careerOpportunity = new CareerOpportunity();
        CareerOpportunityInfo careerOpportunityInfo = careerOpportunity.getCareerOpportunity(ID);
        txtVersionNo.InnerText = careerOpportunityInfo.VersionNo.ToString("N");
        string[] serialNoArray = careerOpportunityInfo.SerialNo.Split('-');
        txtSerialNo0.InnerText = serialNoArray[0] + "-" + serialNoArray[1];
        txtSerialNo1.Value = serialNoArray[2];
        txtSerialNo2.Value = serialNoArray[3];
        //comType.Value = careerOpportunityInfo.Type;


        //new icon
        if (careerOpportunityInfo.NewIconInfo != null)
            txtIconExpiryDay.Value = careerOpportunityInfo.NewIconInfo.ExpiryDate.ToString(GlobalSetting.DateTimeFormat);
         

        bool isAbandoned = true;
        foreach (ListItem item in comType.Items)
        {
            if (item.Value == careerOpportunityInfo.Type.ToString())
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
            string description = SystemPara.getDescription(careerOpportunityInfo.Type);
            comType.Items.Add(new ListItem(description, careerOpportunityInfo.Type.ToString()));
            comType.Value = careerOpportunityInfo.Type.ToString();
        }
        txtCareerLevel.Value = careerOpportunityInfo.CareerLevel;
        txtExp.Value = (careerOpportunityInfo.Experience + 100.001).ToString().Substring(1, 5);
        //comQualification.Value = careerOpportunityInfo.Qualification.ToString();
        isAbandoned = true;
        foreach (ListItem item in comQualification.Items)
        {
            if (item.Value == careerOpportunityInfo.Qualification.ToString())
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
            string description = SystemPara.getDescription(careerOpportunityInfo.Qualification);
            comQualification.Items.Add(new ListItem(description, careerOpportunityInfo.Qualification.ToString()));
            comQualification.Value = careerOpportunityInfo.Qualification.ToString();
        }


        /////---------------1

        isAbandoned = true;
        foreach (ListItem item in txtJobFunction.Items)
        {
            if (item.Value == careerOpportunityInfo.JobFunction.ToString())
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
            string description = SystemPara.getDescription(careerOpportunityInfo.JobFunction);
            txtJobFunction.Items.Add(new ListItem(description, careerOpportunityInfo.JobFunction.ToString()));
            txtJobFunction.Value = careerOpportunityInfo.JobFunction.ToString();
        }
        /////---------------

        /////---------------2

        isAbandoned = true;
        foreach (ListItem item in txtDivision.Items)
        {
            if (item.Value == careerOpportunityInfo.Division.ToString())
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
            string description = SystemPara.getDescription(careerOpportunityInfo.Division);
            txtDivision.Items.Add(new ListItem(description, careerOpportunityInfo.Division.ToString()));
            txtDivision.Value = careerOpportunityInfo.Division.ToString();
        }
        /////---------------
        /////---------------3

        isAbandoned = true;
        foreach (ListItem item in txtDepartment.Items)
        {
            if (item.Value == careerOpportunityInfo.Department.ToString())
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
            string description = SystemPara.getDescription(careerOpportunityInfo.Department);
            txtDepartment.Items.Add(new ListItem(description, careerOpportunityInfo.Department.ToString()));
            txtDepartment.Value = careerOpportunityInfo.Department.ToString();
        }
        /////---------------


        /////---------------4

        isAbandoned = true;
        foreach (ListItem item in txtType.Items)
        {
            if (item.Value == careerOpportunityInfo.EmploymentType.ToString())
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
            string description = SystemPara.getDescription(careerOpportunityInfo.EmploymentType);
            txtType.Items.Add(new ListItem(description, careerOpportunityInfo.EmploymentType.ToString()));
            txtType.Value = careerOpportunityInfo.EmploymentType.ToString();
        }
        /////---------------

        //txtJobFunction.Value = careerOpportunityInfo.JobFunction;
        //txtDivision.Value = careerOpportunityInfo.Division;
        //txtDepartment.Value = careerOpportunityInfo.Department;
        txtLocation.Value = careerOpportunityInfo.Location;
        //txtType.Value = careerOpportunityInfo.EmploymentType;
        txtEmail.Value = careerOpportunityInfo.Email;
        txtDetails.Value = careerOpportunityInfo.Details;
        txtDisclaimer.Value = careerOpportunityInfo.Disclaimer;
    }
    protected void ControlDataBind()
    {
        System.Data.DataTable dataSource = SystemPara.getSystemPara("CareerType");
        foreach (System.Data.DataRow row in dataSource.Rows)
        {
            this.comType.Items.Add(new ListItem(row["Description"].ToString(), row["ID"].ToString()));
        }
        System.Data.DataTable EducationLevel = SystemPara.getSystemPara("EducationLevel");
        foreach (System.Data.DataRow row in EducationLevel.Rows)
        {
            this.comQualification.Items.Add(new ListItem(row["Description"].ToString(), row["ID"].ToString()));
        }
        System.Data.DataTable Position = SystemPara.getSystemPara("Position");
        foreach (System.Data.DataRow row in Position.Rows)
        {
            this.txtJobFunction.Items.Add(new ListItem(row["Description"].ToString(), row["ID"].ToString()));
        }
        System.Data.DataTable Division = SystemPara.getSystemPara("Division");
        foreach (System.Data.DataRow row in Division.Rows)
        {
            this.txtDivision.Items.Add(new ListItem(row["Description"].ToString(), row["ID"].ToString()));
        }

        System.Data.DataTable Department = SystemPara.getSystemPara("Department");
        foreach (System.Data.DataRow row in Department.Rows)
        {
            this.txtDepartment.Items.Add(new ListItem(row["Description"].ToString(), row["ID"].ToString()));
        }

        System.Data.DataTable EmploymentType = SystemPara.getSystemPara("EmploymentType");
        foreach (System.Data.DataRow row in EmploymentType.Rows)
        {
            this.txtType.Items.Add(new ListItem(row["Description"].ToString(), row["ID"].ToString()));
        }



    }
    protected void setMaxLength()
    {
        txtSerialNo1.Attributes["maxLength"] = GlobalSetting.FieldLength.SerialNo1;
        txtSerialNo2.Attributes["maxLength"] = GlobalSetting.FieldLength.SerialNo2;
        txtLocation.Attributes["maxLength"] = GlobalSetting.FieldLength.Location;
        //txtDepartment.Attributes["maxLength"] = GlobalSetting.FieldLength.Department;
        txtEmail.Attributes["maxLength"] = GlobalSetting.FieldLength.EmailAddress;
        //txtDivision.Attributes["maxLength"] = GlobalSetting.FieldLength.Division;
        txtCareerLevel.Attributes["maxLength"] = GlobalSetting.FieldLength.Career.CareerLevel;
        txtExp.Attributes["maxLength"] = GlobalSetting.FieldLength.Career.Experience;
        //txtJobFunction.Attributes["maxLength"] = GlobalSetting.FieldLength.Career.JobFunction;
        //txtType.Attributes["maxLength"] = GlobalSetting.FieldLength.Career.EmploymentType;
        txtDetails.Attributes["maxLength"] = GlobalSetting.FieldLength.Career.Details;
        txtDisclaimer.Attributes["maxLength"] = GlobalSetting.FieldLength.Career.Disclaimer;

    }
}