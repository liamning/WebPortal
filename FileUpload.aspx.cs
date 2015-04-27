using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.setMaxLength();
            this.bindControl();
        }
    }

    protected void bindControl()
    {
        System.Data.DataTable dataSource = SystemPara.getSystemPara("LinkType");
        foreach (System.Data.DataRow row in dataSource.Rows)
        {
            comType.Items.Add(new ListItem(row["Description"].ToString(), row["ID"].ToString()));
        } 
    }

    protected void setMaxLength()
    {
        txtDirectory.Attributes["maxLength"] = GlobalSetting.FieldLength.FileUploadManager.Directory;
        txtFileName.Attributes["maxLength"] = GlobalSetting.FieldLength.FileUploadManager.FileName;
        txtDescription.Attributes["maxLength"] = GlobalSetting.FieldLength.FileUploadManager.Description; 
    }
}