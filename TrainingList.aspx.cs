using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TrainingList : System.Web.UI.Page
{
    public System.Text.StringBuilder latestTraining;
    protected void Page_Load(object sender, EventArgs e)
    {

        //get top 5 training 
        Training trainingHandler = new Training();
        List<TrainingInfo> latestTrainingList = trainingHandler.getLatestTrainings(0);
        latestTraining = new System.Text.StringBuilder();
        int i = 0;
        foreach (TrainingInfo training in latestTrainingList)
        {
            i++;

            if (training.NewIconInfo != null
                && DateTime.Now.Date < training.NewIconInfo.ExpiryDate)
            {
                latestTraining.Append(string.Format("<li class='newIcon' style='word-wrap:break-word;'>"
                   + "<a href='{1}'>{3}. <span class='blueFont'>{2:" + GlobalSetting.DateTimeFormat + "}</span>"
                   + "&nbsp&nbsp&nbsp{0}</a></li>", training.Name,
                                       "ViewTraining.aspx?ID=" + training.ID.ToString(),
                                       training.Schedule[0].StartTime,
                                       i.ToString()));
            }
            else
            {
                latestTraining.Append(string.Format("<li style='word-wrap:break-word;'>"
                + "<a href='{1}'>{3}. <span class='blueFont'>{2:" + GlobalSetting.DateTimeFormat + "}</span>"
                + "&nbsp&nbsp&nbsp{0}</a></li>", training.Name,
                                    "ViewTraining.aspx?ID=" + training.ID.ToString(),
                                    training.Schedule[0].StartTime,
                                    i.ToString()));
            }
            
        }
    }
}