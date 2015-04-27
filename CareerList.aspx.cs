using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CareerList : System.Web.UI.Page
{
    public System.Text.StringBuilder careerOpportunities;
    protected void Page_Load(object sender, EventArgs e)
    {

        //get top 5 career opportunities 
        CareerOpportunity careerOpportunityHandler = new CareerOpportunity();
        careerOpportunities = careerOpportunityHandler.getLatestCareerOpportunitiesStr(0);
    }
}