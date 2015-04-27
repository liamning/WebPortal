<%@ WebHandler Language="C#" Class="PreviewImageHandler" %>

using System;
using System.Web;

public class PreviewImageHandler : IHttpHandler {


    public void ProcessRequest(HttpContext context)
    {

        HttpRequest Request = context.Request;
        HttpResponse Response = context.Response;
         
        int maxLength = Convert.ToInt32(Request.QueryString["maxLength"]);
        int imageID = Convert.ToInt32(Request.QueryString["ID"]);
        string MIME = "";
        byte[] imageBytes = News.getImageByID(imageID, ref MIME);
        System.IO.Stream st = new System.IO.MemoryStream(imageBytes);
        imageBytes = ImageResize.getPreviewImage(st, maxLength, maxLength);
        MIME = "image/jpeg";

        Response.ContentType = MIME;
        Response.BinaryWrite(imageBytes);

    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}