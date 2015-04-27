using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Control_CalendarHeader : System.Web.UI.UserControl
{
    public string eventData;
    protected void Page_Load(object sender, EventArgs e)
    {
        eventData = new Event().getEventByMonth(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)).ToString();
        eventData = Server.HtmlEncode(eventData);
    }
}