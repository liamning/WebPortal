using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class ViewCareer : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int ID = Convert.ToInt32(Request.QueryString["ID"]);
        trainingID.Value = ID.ToString();
        CareerOpportunity careerOpportunityHandler = new CareerOpportunity();
        CareerOpportunityInfo careerOpportunityInfo = careerOpportunityHandler.getCareerOpportunity(ID);
        txtCareerLevel.InnerText = careerOpportunityInfo.CareerLevel;
        txtQualification.InnerText = SystemPara.getDescription(careerOpportunityInfo.Qualification);
        txtDetails.InnerHtml = careerOpportunityInfo.Details;
        txtEmail.HRef = "mailto:" + careerOpportunityInfo.Email;
        txtEmail.InnerText = careerOpportunityInfo.Email;
        txtExp.InnerText = careerOpportunityInfo.Experience.ToString();
        txtJobFunction.InnerText = SystemPara.getDescription(careerOpportunityInfo.JobFunction);
        txtDivision.InnerText = SystemPara.getDescription(careerOpportunityInfo.Division);
        txtDepartment.InnerText = SystemPara.getDescription(careerOpportunityInfo.Department);
        txtLocation.InnerText = careerOpportunityInfo.Location;
        //txtQualification.InnerText = SystemPara.getDescription(careerOpportunityInfo.Qualification);
        // txtSalary.InnerText = careerOpportunityInfo.Salary.ToString();
        txtType.InnerText = SystemPara.getDescription(careerOpportunityInfo.EmploymentType);
        txtDisclaimer.InnerText = careerOpportunityInfo.Disclaimer;


    }
}