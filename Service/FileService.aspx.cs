using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Service_FileService : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        int fileID = Convert.ToInt32(Request.QueryString["ID"]);
        string type = Request.QueryString["type"];
        string fileName= string.Empty;
        byte[] imageBytes = null;

        if (type == "training")
        {
            try
            {
                Training trainingHandler = new Training();
                TrainingFormInfo formInfo = trainingHandler.getTrainingFormInfo(fileID);

                System.IO.FileStream file = new System.IO.FileStream(formInfo.FormPath, System.IO.FileMode.Open);
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(formInfo.FormPath);

                fileName = formInfo.Description + fileInfo.Extension;
                int length = Convert.ToInt32(file.Length);
                imageBytes = new byte[length];
                file.Read(imageBytes, 0, length);
                file.Close();
            }
            catch(Exception ex)
            {
                Log.log(ex.Message + "\r\n" + ex.StackTrace, Log.Type.Exception);
                Response.ClearContent();
                Response.Clear();
                Response.Redirect("~/FileNotFound.html");
            }

        }
        else
        {
            File file = new File();
            FileInfo fileInfo = file.GetFileInfo(fileID);
            fileName = fileInfo.FileNameForDownload;
            imageBytes = file.GetFileByID(fileID);

        }

        Response.ClearContent();
        Response.Clear();
        Response.ContentType = "text/plain";
        Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + ";");

        Response.BinaryWrite(imageBytes);
        Response.Flush();
        Response.End();

         
    }
}