using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ViewTraining : System.Web.UI.Page
{
    public int ID;
    public bool metDeadline;
    public string schedulesJSString;
    public string decisionsJSString;
    public string formsJSString;
    protected void Page_Load(object sender, EventArgs e)
    {

        ID = Convert.ToInt32(Request.QueryString["ID"]);
     
        initTraining(ID);
         
    }


    private void initTraining(int ID)
    {

        hdfID.Value = ID.ToString();
        Training trainingHandler = new Training();
        TrainingInfo trainingInfo = trainingHandler.getTraining(ID);
        txtMaximumAttendance.InnerText = trainingInfo.MaxAttendance.ToString();
        txtName.InnerText = trainingInfo.Name;  
        hdfOptional.Value = trainingInfo.OptionalAttendance.ToString();
        txtContactPerson.InnerText = trainingInfo.ContactPerson;
        txtDepartment.InnerText = trainingInfo.Department;

        txtDetails.InnerHtml = trainingInfo.Details;
        txtLocation.InnerText = trainingInfo.Location;
        txtPhoneNumber.InnerText = trainingInfo.PhoneNumber;
        txtEmail.HRef = "mailto:" + trainingInfo.Email;
        txtEmail.InnerText = trainingInfo.Email;

        dateDeadline.InnerText = trainingInfo.Deadline.ToString(GlobalSetting.DateTimeFormat);

        if (trainingInfo.Deadline.Date <= DateTime.Now.Date)
            metDeadline = true;
        else
            metDeadline = false;
        //linkForm.HRef = "Service/FileService.aspx?type=training&ID=" + trainingInfo.ID;

        int loginID = Convert.ToInt32(Session["LOGINID"]);

        schedulesJSString = Server.HtmlEncode(trainingHandler.getTrainingSchedule(ID).ToString());
        decisionsJSString = Server.HtmlEncode(trainingHandler.getTrainingDecision(ID, loginID).ToString());
        formsJSString = Server.HtmlEncode(trainingHandler.getTrainingFormList(ID).ToString());
    }
}