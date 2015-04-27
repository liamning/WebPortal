<%@ WebHandler Language="C#" Class="ImageHandler" %>

using System;
using System.Web;

public class ImageHandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {

        HttpRequest Request = context.Request;
        HttpResponse Response = context.Response;

         
        int imageID = Convert.ToInt32(Request.QueryString["ID"]);
        string MIME = "";
        byte[] imageBytes = News.getImageByID(imageID, ref MIME);

         
        Response.ContentType = MIME;
        Response.BinaryWrite(imageBytes);

    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}