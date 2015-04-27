using System;
using System.Collections.Generic; 
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ViewEvent : System.Web.UI.Page
{
    public bool metDeadline;
    protected void Page_Load(object sender, EventArgs e)
    {
        int ID = Convert.ToInt32(Request.QueryString["ID"]);
        eventID.Value = ID.ToString();
        Event eventHandler = new Event();
        EventInfo eventInfo = eventHandler.getEvent(ID);
        if (eventInfo == null) Response.Redirect("~/PageNotFound.html");
        if (eventInfo.IsPublicHoliday()) Response.Redirect("~/PageNotFound.html");

        txtName.InnerText = eventInfo.Name;
        txtContactPerson.InnerText = eventInfo.ContactPerson;
        txtDepartment.InnerText = eventInfo.Department;


        txtEventDetails.InnerHtml = eventInfo.EventDetails;
        txtLocation.InnerText = eventInfo.Location;
        txtPhoneNumber.InnerText = eventInfo.PhoneNumber;

        dateStartDate.InnerText = eventInfo.StartTime.ToString(GlobalSetting.DateTimeFormat);
        //dateEndDate.InnerText = eventInfo.EndTime.ToString("yyyy-MM-dd");
        dateStartTime.InnerText = eventInfo.StartTime.ToString("HH:mm");
        dateEndTime.InnerText = eventInfo.EndTime.ToString("HH:mm");

        dateDeadline.InnerText = eventInfo.Deadline.ToString(GlobalSetting.DateTimeFormat);

        if (eventInfo.Deadline.Date <= DateTime.Now.Date)
            metDeadline = true;
        else
            metDeadline = false;

        ImageInfo eventImage = eventInfo.getImage();


        imgSummary.ImageUrl = "Service/ImageHandler.ashx?ID=" + eventImage.ID.ToString();


        int loginID = Convert.ToInt32(Session["LOGINID"]);
        txtDecision.InnerText = eventHandler.getEventDecision(eventInfo.ID, loginID);


    }
}