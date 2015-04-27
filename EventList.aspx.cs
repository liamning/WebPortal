using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EventList : System.Web.UI.Page
{
    public System.Text.StringBuilder latestEvent;
    protected void Page_Load(object sender, EventArgs e)
    {

        //get top 5 training 
        Event eventHandler = new Event();
        List<EventInfo> latestEventList = eventHandler.getLatestEvent(0);
        latestEvent = new System.Text.StringBuilder();
        int i = 0;
        foreach (EventInfo eventItem in latestEventList)
        {
            i++;
            if (eventItem.NewIconInfo != null
                && DateTime.Now.Date < eventItem.NewIconInfo.ExpiryDate)
            {
                latestEvent.Append(string.Format("<li class='newIcon' style='word-wrap:break-word;'>"
                    + "<a href='{1}'>{3}. <span class='blueFont'>{2:" + GlobalSetting.DateTimeFormat + "}</span>"
                    + "&nbsp&nbsp&nbsp{0}</a></li>", eventItem.Name,
                                        "ViewEvent.aspx?ID=" + eventItem.ID.ToString(),
                                        eventItem.StartTime,
                                        i.ToString()));
            }
            else
            {
                latestEvent.Append(string.Format("<li style='word-wrap:break-word;'>"
                    + "<a href='{1}'>{3}. <span class='blueFont'>{2:" + GlobalSetting.DateTimeFormat + "}</span>"
                    + "&nbsp&nbsp&nbsp{0}</a></li>", eventItem.Name,
                                        "ViewEvent.aspx?ID=" + eventItem.ID.ToString(),
                                        eventItem.StartTime,
                                        i.ToString()));
            
            }
        }
    }
}