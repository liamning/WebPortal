using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ArticleManager : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) bindControl();
    }

    protected void bindControl()
    {
        ddlFilter.Items.Add(new ListItem(GlobalSetting.ArticleCategory.News, GlobalSetting.ArticleCategory.News));
        ddlFilter.Items.Add(new ListItem(GlobalSetting.ArticleCategory.Training, GlobalSetting.ArticleCategory.Training));
        ddlFilter.Items.Add(new ListItem(GlobalSetting.ArticleCategory.Event, GlobalSetting.ArticleCategory.Event));
        ddlFilter.Items.Add(new ListItem(GlobalSetting.ArticleCategory.Career, GlobalSetting.ArticleCategory.Career));
    }
}