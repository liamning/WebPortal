using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Service_PreviewImageService : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int maxLength = Convert.ToInt32(Request.QueryString["maxLength"]); 
        int imageID = Convert.ToInt32(Request.QueryString["ID"]);
        string MIME = "";
        byte[] imageBytes = News.getImageByID(imageID, ref MIME); 
        System.IO.Stream st = new System.IO.MemoryStream(imageBytes);
        imageBytes = ImageResize.getPreviewImage(st, maxLength, maxLength);
        MIME = "image/jpeg";

        Response.Clear();
        Response.ClearContent();
        Response.ClearHeaders();
        Response.Buffer = true;
        Response.ContentType = MIME;
        Response.BinaryWrite(imageBytes);
        Response.End(); 
    }
}