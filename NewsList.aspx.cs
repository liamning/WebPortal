using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class NewsList : System.Web.UI.Page
{
    public System.Text.StringBuilder latestNews;
    protected void Page_Load(object sender, EventArgs e)
    {
        //get latest article
        News article = new News();
        List<NewsInfo> latestList = article.getLatestNews(0);

        latestNews = new System.Text.StringBuilder();
        int i = 0;
        foreach (NewsInfo news in latestList)
        {
            i++;
            //append top 5 news 
            if (news.NewIconInfo != null
                && DateTime.Now.Date < news.NewIconInfo.ExpiryDate)
            {
                latestNews.Append(string.Format("<li class ='newIcon' style='word-wrap:break-word;'><a href='{1}'>"
                                + "{3}. <span class='blueFont'>{2:" + GlobalSetting.DateTimeFormat + "}</span>"
                                + "&nbsp&nbsp&nbsp{0}</a></li>", news.Title,
                                 "ViewArticle.aspx?ID=" + news.ID.ToString(), news.EffectiveDate, i.ToString()));
            }
            else
            {

                latestNews.Append(string.Format("<li style='word-wrap:break-word;'><a href='{1}'>"
                                + "{3}. <span class='blueFont'>{2:" + GlobalSetting.DateTimeFormat + "}</span>"
                                + "&nbsp&nbsp&nbsp{0}</a></li>", news.Title,
                                 "ViewArticle.aspx?ID=" + news.ID.ToString(), news.EffectiveDate, i.ToString()));
            }
        } 
    }
}