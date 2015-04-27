using System;
using System.Collections.Generic; 
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ImageService : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        System.Diagnostics.Debug.Print("start");

        int imageID = Convert.ToInt32(Request.QueryString["ID"]);
        string MIME = "";
        byte[] imageBytes = News.getImageByID(imageID, ref MIME);


        System.Diagnostics.Debug.Print("end");

        Response.Clear();
        Response.ClearContent();
        Response.ClearHeaders();
        Response.Buffer = true;
        Response.ContentType = MIME;
        Response.BinaryWrite(imageBytes);
       // Response.End(); 
    }
}