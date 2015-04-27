using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class NewEvent : System.Web.UI.Page
{
    public string mode;
    public string systemParaBuildin;
    public bool isPublicHoliday = false;
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

    protected void initTraining(int ID)
    {

        hdfID.Value = ID.ToString();
        Event eventHandler = new Event();
        EventInfo eventInfo = eventHandler.getEvent(ID);

        txtVersionNo.InnerText = eventInfo.VersionNo.ToString("N");
        string[] serialNoArray = eventInfo.SerialNo.Split('-');

        txtSerialNo0.InnerText = serialNoArray[0] + "-" + serialNoArray[1];
        txtSerialNo1.Value = serialNoArray[2];
        txtSerialNo2.Value = serialNoArray[3];
       // comType.Value = eventInfo.Type;

        bool isAbandoned = true;
        foreach (ListItem item in comType.Items)
        {
            if (item.Value == eventInfo.Type.ToString())
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
            string description = SystemPara.getDescription(eventInfo.Type);
            comType.Items.Add(new ListItem(description, eventInfo.Type.ToString()));
            comType.Value = eventInfo.Type.ToString();
        }



        txtName.Value = eventInfo.Name;
        txtContactPerson.Value = eventInfo.ContactPerson;
        txtDepartment.Value = eventInfo.Department;


        txtEventDetails.Value = eventInfo.EventDetails;
        txtLocation.Value = eventInfo.Location;
        txtPhoneNumber.Value = eventInfo.PhoneNumber;

        dateStartDate.Value = eventInfo.StartTime.ToString("dd/MM/yyyy"); 
        dateStartTime.Value = eventInfo.StartTime.ToString("HH:mm");
        dateEndTime.Value = eventInfo.EndTime.ToString("HH:mm");

        dateDeadline.Value = eventInfo.Deadline.ToString("dd/MM/yyyy");


        //new icon
        if (eventInfo.NewIconInfo != null)
            txtIconExpiryDay.Value = eventInfo.NewIconInfo.ExpiryDate.ToString(GlobalSetting.DateTimeFormat);


        this.isPublicHoliday = eventInfo.IsPublicHoliday();
        if(isPublicHoliday)
        {
            dateStartTime.Value = "";
            dateEndTime.Value = "";
            dateDeadline.Value = "";
        }

    }

    protected void ControlDataBind()
    {
        System.Data.DataTable EventType = SystemPara.getSystemPara("EventType");

        foreach (System.Data.DataRow row in EventType.Rows)
        {
            this.comType.Items.Add(new ListItem(row["Description"].ToString(), row["ID"].ToString()));
        }

        systemParaBuildin = SystemPara.getSystemParaBuildinCode();
         
    }

    protected void setMaxLength()
    {
        txtSerialNo1.Attributes["maxLength"] = GlobalSetting.FieldLength.SerialNo1;
        txtSerialNo2.Attributes["maxLength"] = GlobalSetting.FieldLength.SerialNo2;

        txtName.Attributes["maxLength"] = GlobalSetting.FieldLength.Training.TrainingCourse;
        txtEventDetails.Attributes["maxLength"] = GlobalSetting.FieldLength.Training.Details;
        txtLocation.Attributes["maxLength"] = GlobalSetting.FieldLength.Location;
        txtContactPerson.Attributes["maxLength"] = GlobalSetting.FieldLength.ContactPerson;
        txtDepartment.Attributes["maxLength"] = GlobalSetting.FieldLength.Department;
        txtPhoneNumber.Attributes["maxLength"] = GlobalSetting.FieldLength.PhoneNumber;     
    }
}