using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class NewTraining : System.Web.UI.Page
{
    public string mode;
    public System.Text.StringBuilder formType;

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
                this.initTraining(ID);

            }
            else
            {
                this.mode = "New";
            }
        } 
    }

    private void initTraining(int ID)
    {

        hdfID.Value = ID.ToString();
        Training trainingHandler = new Training();
        TrainingInfo trainingInfo = trainingHandler.getTraining(ID);

        txtVersionNo.InnerText = trainingInfo.VersionNo.ToString("N");
        hdfVersionNo.Value = trainingInfo.VersionNo.ToString("N");
        string[] serailNoArray = trainingInfo.SerialNo.Split('-');
        txtSerialNo0.InnerText = serailNoArray[0] + "-" + serailNoArray[1];
        txtSerialNo1.Value = serailNoArray[2];
        txtSerialNo2.Value = serailNoArray[3];

        txtName.Value = trainingInfo.Name;
        //comType.Value = trainingInfo.Type;


        bool isAbandoned = true;
        foreach (ListItem item in comType.Items)
        {
            if (item.Value == trainingInfo.Type.ToString())
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
            string description = SystemPara.getDescription(trainingInfo.Type);
            comType.Items.Add(new ListItem(description, trainingInfo.Type.ToString()));
            comType.Value = trainingInfo.Type.ToString();
        }


        txtMaxAttendance.Value = trainingInfo.MaxAttendance.ToString();
        chkOptionalAttendance.Checked = trainingInfo.OptionalAttendance;
        txtContactPerson.Value = trainingInfo.ContactPerson;
        txtDepartment.Value = trainingInfo.Department;

        txtDetails.Value = trainingInfo.Details;
        txtLocation.Value = trainingInfo.Location;
        txtPhoneNumber.Value = trainingInfo.PhoneNumber;
        txtEmail.Value = trainingInfo.Email;
        //txtFormPath.Value = trainingInfo.FormPath;

        //new icon
        if (trainingInfo.NewIconInfo != null)
            txtIconExpiryDay.Value = trainingInfo.NewIconInfo.ExpiryDate.ToString(GlobalSetting.DateTimeFormat);
         
        dateDeadline.Value = trainingInfo.Deadline.ToString(GlobalSetting.DateTimeFormat);
         
    }

    private void ControlDataBind()
    {
        System.Data.DataTable dataSource = SystemPara.getSystemPara("TrainingType");

        foreach (System.Data.DataRow row in dataSource.Rows)
        {
            this.comType.Items.Add(new ListItem(row["Description"].ToString(), row["ID"].ToString()));
        }

        formType = new System.Text.StringBuilder();

        dataSource = SystemPara.getSystemPara("TrainingFormType");
        foreach (System.Data.DataRow row in dataSource.Rows)
        {
            formType.Append(string.Format("<option value='{0}'>{1}</option>", row["ID"].ToString(), row["Description"].ToString()));    
        }

    }

    protected void setMaxLength()
    {
        txtSerialNo1.Attributes["maxLength"] = GlobalSetting.FieldLength.SerialNo1;
        txtSerialNo2.Attributes["maxLength"] = GlobalSetting.FieldLength.SerialNo2;

        txtName.Attributes["maxLength"] = GlobalSetting.FieldLength.Training.TrainingCourse;
        txtMaxAttendance.Attributes["maxLength"] = GlobalSetting.FieldLength.Training.MaximumAttendance;
        txtDetails.Attributes["maxLength"] = GlobalSetting.FieldLength.Training.Details;
        //txtFormPath.Attributes["maxLength"] = GlobalSetting.FieldLength.Training.FormPath;
        txtLocation.Attributes["maxLength"] = GlobalSetting.FieldLength.Location;
        txtContactPerson.Attributes["maxLength"] = GlobalSetting.FieldLength.ContactPerson;
        txtDepartment.Attributes["maxLength"] = GlobalSetting.FieldLength.Department;
        txtPhoneNumber.Attributes["maxLength"] = GlobalSetting.FieldLength.PhoneNumber;
        txtEmail.Attributes["maxLength"] = GlobalSetting.FieldLength.EmailAddress;
     
    }

}