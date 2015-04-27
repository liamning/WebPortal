using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Jayrock.Json;
using System.IO;
public partial class AjaxService : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        String action = Request.Form["action"];
        string result = "";
        try
        {
            // throw new Exception("\"test\"");
            result = this.getResult(action);
        }
        catch (Exception ex)
        {
            Log.log(ex.Message + "\r\n" + ex.StackTrace, Log.Type.Exception);
            result = "{\"error\": \"" + Server.HtmlEncode("System Error:  "
                                                         + ex.Message.Replace("\r\n", " ")) + "\" }";
        }
        Response.Clear();
        Response.Write(result);
        Response.End();
    }
    private string getResult(string action)
    {
        string result = "";
        switch (action)
        {
            case "getSystemParaBuildinCode":
                result = getSystemParaBuildinCode();
                break;
            case "updateLinkIconStaus":
                result = updateLinkIconStaus();
                break;
            case "deleteLinkIcon":
                result = deleteLinkIcon();
                break;
            case "getAllIconJsonArray":
                result = getAllIconJsonArray();
                break;
            case "createLinkIcon":
                result = createLinkIcon();
                break;
            case "updateLinkIcon":
                result = updateLinkIcon();
                break;
            case "updateUserRole":
                result = updateUserRole();
                break;
            case "logOut":
                result = Logout();
                break;
            case "getUserStatus":
                result = getUserStatus();
                break;
            case "updateStatus":
                String userIDs = Request.Form["IDs[]"];
                String loginID = Session["LOGINID"].ToString();
                String newStatus = Request.Form["newStatus"];
                result = updateStatus(userIDs, loginID, newStatus);
                break;
            case "getAllUsers":
                result = getAllUsers();
                break;
            case "getTrainingDecision":
                result = this.getTrainingDecision();
                break;
            case "abandonSystemItem":
                result = this.abandonSystemItem();
                break;
            case "getSystemTypeItemList":
                result = this.getSystemTypeItemList();
                break;
            case "addSystemItem":
                result = this.addSystemItem();
                break;
            case "updateSystemItem":
                result = this.updateSystemItem();
                break;
            case "commitSuggestion":
                result = commitSuggestion();
                break;
            case "getArticleByCategory":
                string category = Request.Form["category"];
                result = getArticleByCategory(category);
                break;
            case "publishFiles":
                result = publishFiles();
                break;
            case "unpublishFiles":
                result = unpublishFiles();
                break;
            case "deleteFiles":
                result = deleteFiles();
                break;
            case "updateArticleStatus":
                String articles = Request.Form["IDs[]"];
                String newArticleStatus = Request.Form["newStatus"];
                String article_category = Request.Form["category"];
                result = updateArticleStatus(articles, newArticleStatus, article_category);
                break;

            case "getArticleCategory":
                result = getArticleCategory();
                break;
            case "saveEvent":
            case "publishEvent":
            case "updateEvent":

                bool isPublicHoliday = Request.Form["chkPublicHoliday"] == "True";

                EventInfo eventinfo = new EventInfo();
                eventinfo.SerialNo = Request.Form["txtSerialNo"].ToUpper();
                eventinfo.Name = Request.Form["txtName"]; 
                eventinfo.Type = Convert.ToInt32(Request.Form["comType"]);

                if(isPublicHoliday)
                {
                    eventinfo.StartTime = DateTime.ParseExact(Request.Form["dateStartDate"] + " 00:00:00", GlobalSetting.DateTimeFormat + " HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    eventinfo.EndTime = eventinfo.StartTime.AddDays(1).AddSeconds(-1);
                    eventinfo.Deadline = eventinfo.EndTime;

                    eventinfo.Location = "";
                    eventinfo.ContactPerson = "";
                    eventinfo.PhoneNumber = "";
                    eventinfo.Department = "";
                    eventinfo.EventDetails = "";
                }
                else
                {
                    eventinfo.StartTime = DateTime.ParseExact(Request.Form["dateStartDate"] + " " + Request.Form["dateStartTime"] + ":00", GlobalSetting.DateTimeFormat + " HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    eventinfo.EndTime = DateTime.ParseExact(Request.Form["dateStartDate"] + " " + Request.Form["dateEndTime"] + ":00", GlobalSetting.DateTimeFormat + " HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    eventinfo.Deadline = DateTime.ParseExact(Request.Form["dateDeadline"], GlobalSetting.DateTimeFormat, System.Globalization.CultureInfo.InvariantCulture);
                    eventinfo.Location = Request.Form["txtLocation"];
                    eventinfo.ContactPerson = Request.Form["txtContactPerson"];
                    eventinfo.PhoneNumber = Request.Form["txtPhoneNumber"];
                    eventinfo.Department = Request.Form["txtDepartment"];
                    eventinfo.EventDetails = Request.Form["txtEventDetails"];

                    //new icon
                    if (!string.IsNullOrEmpty(Request.Form["txtIconExpiryDay"]))
                    {
                        eventinfo.NewIconInfo = new NewIconInfo();
                        eventinfo.NewIconInfo.Category = GlobalSetting.ArticleCategory.Event;
                        eventinfo.NewIconInfo.ExpiryDate = DateTime.ParseExact(Request.Form["txtIconExpiryDay"], GlobalSetting.DateTimeFormat,
                                                System.Globalization.CultureInfo.InvariantCulture);
                    }
                }
                eventinfo.CreateBy = Convert.ToInt32(Session["LOGINID"]);


                switch (action)
                {
                    case "saveEvent":
                        eventinfo.Status = GlobalSetting.ArticleStatus.Unpublished;
                        result = saveEvent(eventinfo);
                        break;
                    case "publishEvent":
                        eventinfo.Status = GlobalSetting.ArticleStatus.Published;
                        result = saveEvent(eventinfo);
                        break;
                    case "updateEvent":
                        eventinfo.ID = Convert.ToInt32(Request.Form["hdfID"]);
                        eventinfo.VersionNo = Convert.ToSingle(Request.Form["txtVersionNo"]);
                        eventinfo.UpdateBy = Convert.ToInt32(Session["LOGINID"]);
                        eventinfo.VersionNo += (float)0.01;
                        result = updateEvent(eventinfo);
                        break;
                }

                break;
            case "getEventByMonth":
                DateTime dateOfMonth = Convert.ToDateTime(Request.Form["DateOfMonth"]);
                result = getEventByMonth(dateOfMonth);
                break;
            case "logActivity":
                string userID = Session["LOGINID"].ToString();
                string activityID = Request.Form["activityID"];
                string category_log = Request.Form["category"];
                string action_log = Request.Form["action_log"];
                string schedules = Request.Form["schedules[]"];
                result = logActivity(userID, activityID, category_log, action_log, schedules);
                break;
            case "saveTraining":
            case "publishTraining":
            case "updateTraining":
                TrainingInfo traininginfo = new TrainingInfo();
                traininginfo.Name = Request.Form["txtName"];
                traininginfo.SerialNo = Request.Form["txtSerialNo"].ToUpper();
                traininginfo.Type = Convert.ToInt32(Request.Form["comType"]);
                traininginfo.OptionalAttendance = !string.IsNullOrEmpty(Request.Form["chkOptionalAttendance"]);
                traininginfo.MaxAttendance = string.IsNullOrEmpty(Request.Form["txtMaxAttendance"]) ? 0 : Convert.ToInt32(Request.Form["txtMaxAttendance"]);
                traininginfo.Deadline = DateTime.ParseExact(Request.Form["dateDeadline"], GlobalSetting.DateTimeFormat, System.Globalization.CultureInfo.InvariantCulture);
                traininginfo.Location = Request.Form["txtLocation"];
                traininginfo.ContactPerson = Request.Form["txtContactPerson"];
                traininginfo.PhoneNumber = Request.Form["txtPhoneNumber"];
                traininginfo.Department = Request.Form["txtDepartment"];
                traininginfo.Email = Request.Form["txtEmail"];
                traininginfo.Details = Request.Form["txtDetails"];
                //traininginfo.FormPath = Request.Form["FormPath"];

                //new icon
                if (!string.IsNullOrEmpty(Request.Form["txtIconExpiryDay"]))
                {
                    traininginfo.NewIconInfo = new NewIconInfo();
                    traininginfo.NewIconInfo.Category = GlobalSetting.ArticleCategory.Training;
                    traininginfo.NewIconInfo.ExpiryDate = DateTime.ParseExact(Request.Form["txtIconExpiryDay"], GlobalSetting.DateTimeFormat,
                                            System.Globalization.CultureInfo.InvariantCulture);
                }

                traininginfo.CreateBy = Convert.ToInt32(Session["LOGINID"]);
                traininginfo.UpdateBy = Convert.ToInt32(Session["LOGINID"]);
                traininginfo.Schedule = new List<TrainingScheInfo>();
                string dateStartDateKey; 
                string dateStartTimeKey;
                string dateEndTimeKey;
                TrainingScheInfo tmpScheduleItem;
                for (int i = 0; ; i++)
                {
                    dateStartDateKey = "dateStartDate" + i.ToString();
                    dateStartTimeKey = "dateStartTime" + i.ToString();
                    dateEndTimeKey = "dateEndTime" + i.ToString();
                    if (!string.IsNullOrEmpty(Request.Form[dateStartDateKey])
                        && !string.IsNullOrEmpty(Request.Form[dateStartTimeKey])
                        && !string.IsNullOrEmpty(Request.Form[dateEndTimeKey]))
                    {
                        tmpScheduleItem = new TrainingScheInfo();
                        tmpScheduleItem.StartTime = DateTime.ParseExact(Request.Form[dateStartDateKey] + " " + Request.Form[dateStartTimeKey] + ":00", GlobalSetting.DateTimeFormat + " HH:mm:ss",
                                            System.Globalization.CultureInfo.InvariantCulture);
                        tmpScheduleItem.EndTime = DateTime.ParseExact(Request.Form[dateStartDateKey] + " " + Request.Form[dateEndTimeKey] + ":00", GlobalSetting.DateTimeFormat + " HH:mm:ss",
                                            System.Globalization.CultureInfo.InvariantCulture);
                        traininginfo.Schedule.Add(tmpScheduleItem);
                    }
                    else
                    {
                        break;
                    }
                }

                traininginfo.FormList = new List<TrainingFormInfo>();
                string tmpFormType;
                string tmpFormPath;
                string tmpDescription;
                string tmpFormID;
                string tmpFormStatus;
                HttpPostedFile trainingForm;
                TrainingFormInfo tmpTrainingForm;
                byte[] tmpFile;

                int fileCount = Request.Files.Count;
                for (int i = 0; ; i++)
                {
                    tmpFormType = Request.Form[string.Format("FormType{0}", i)];
                    tmpFormPath = Request.Form[string.Format("FormPath{0}", i)];
                    tmpDescription = Request.Form[string.Format("FormDescription{0}", i)];
                    trainingForm = Request.Files[string.Format("TrainingForm{0}", i)];
                    tmpFormID = Request.Form[string.Format("FormID{0}", i)];
                    tmpFormStatus = Request.Form[string.Format("FormStatus{0}", i)];

                    if (fileCount == 0) break;

                    if (!string.IsNullOrEmpty(tmpFormPath) || trainingForm != null) fileCount--;

                    if (!string.IsNullOrEmpty(tmpFormType) 
                        && (!string.IsNullOrEmpty(tmpFormPath) || trainingForm != null) 
                        && !string.IsNullOrEmpty(tmpDescription))
                    { 
                        tmpTrainingForm = new TrainingFormInfo();
                        tmpTrainingForm.FormType = Convert.ToInt32(tmpFormType);


                        tmpTrainingForm.FormPath = tmpFormPath;
                        tmpTrainingForm.Description = tmpDescription;
                        tmpTrainingForm.ID = string.IsNullOrEmpty(tmpFormID) ? 0 : Int32.Parse(tmpFormID);
                        tmpTrainingForm.Status = tmpFormStatus;
                         
                        if (trainingForm != null)
                        {
                            if (trainingForm.ContentLength > GlobalSetting.MaxImageLength * 1024)
                            {
                                return "{\"error\": \"File size must be smaller than " + GlobalSetting.MaxImageLength + " KB\"}";
                            };

                            if (trainingForm.ContentLength == 0) continue;

                            tmpTrainingForm.FileInfo = new FileInfo();
                            tmpTrainingForm.FileInfo.OriginalName = trainingForm.FileName;
                            tmpTrainingForm.FileInfo.OriginalName = tmpTrainingForm.FileInfo.OriginalName.Substring(tmpTrainingForm.FileInfo.OriginalName.LastIndexOf("\\") + 1);
                            tmpTrainingForm.FileInfo.Name = tmpTrainingForm.FileInfo.OriginalName;
                            tmpTrainingForm.FileInfo.Description = tmpTrainingForm.FileInfo.OriginalName;

                            using (BinaryReader br = new BinaryReader(trainingForm.InputStream))
                            {
                                tmpFile = br.ReadBytes(trainingForm.ContentLength);
                            }
                            tmpTrainingForm.FileInfo.Content = tmpFile;
                            tmpTrainingForm.FileInfo.Path = "Training Form";
                            tmpTrainingForm.FileInfo.UploadDate = DateTime.Now;

                        }

                        traininginfo.FormList.Add(tmpTrainingForm);
                    }
                    else
                    {
                        continue;
                    }
                }


                switch (action)
                {
                    case "saveTraining":
                        traininginfo.Status = GlobalSetting.ArticleStatus.Unpublished;
                        result = saveTraining(traininginfo);
                        break;
                    case "publishTraining":
                        traininginfo.Status = GlobalSetting.ArticleStatus.Published;
                        result = saveTraining(traininginfo);
                        break;
                    case "updateTraining":
                        traininginfo.ID = Convert.ToInt32(Request.Form["hdfID"]);
                        traininginfo.VersionNo = Convert.ToSingle(Request.Form["hdfVersionNo"]);
                        traininginfo.VersionNo += (float)0.01;
                        result = updateTraining(traininginfo);
                        break;
                }
                break;

            case "getTrainingFormList":
                result = getTrainingFormList();
                break;
            case "saveCareer":
            case "updateCareer":
            case "publishCareer":
                CareerOpportunityInfo careerinfo = new CareerOpportunityInfo();
                careerinfo.Type = Convert.ToInt32(Request.Form["Type"]);
                careerinfo.SerialNo = Request.Form["SerialNo"].ToUpper();
                careerinfo.CareerLevel = Request.Form["CareerLevel"];
                careerinfo.CreateBy = Convert.ToInt32(Session["LOGINID"]);
                careerinfo.Details = Request.Form["Details"];
                careerinfo.Email = Request.Form["Email"];
                careerinfo.EmploymentType = Convert.ToInt32(Request.Form["EmploymentType"]);
                careerinfo.Experience = Convert.ToSingle(Request.Form["Experience"]);
                careerinfo.JobFunction = Convert.ToInt32(Request.Form["JobFunction"]);
                careerinfo.Division = Convert.ToInt32(Request.Form["Division"]);
                careerinfo.Department = Convert.ToInt32(Request.Form["Department"]);
                careerinfo.Location = Request.Form["Location"];
                careerinfo.Qualification = Convert.ToInt32(Request.Form["Qualification"]);
                careerinfo.Disclaimer = Request.Form["Disclaimer"];
                careerinfo.UpdateBy = Convert.ToInt32(Session["LOGINID"]);


                //new icon
                if (!string.IsNullOrEmpty(Request.Form["IconExpiryDay"]))
                {
                    careerinfo.NewIconInfo = new NewIconInfo();
                    careerinfo.NewIconInfo.Category = GlobalSetting.ArticleCategory.Career;
                    careerinfo.NewIconInfo.ExpiryDate = DateTime.ParseExact(Request.Form["IconExpiryDay"], GlobalSetting.DateTimeFormat,
                                            System.Globalization.CultureInfo.InvariantCulture);
                }

                switch (action)
                {
                    case "saveCareer":
                        careerinfo.Status = GlobalSetting.ArticleStatus.Unpublished;
                        result = saveCareerOpportunity(careerinfo);
                        break;
                    case "publishCareer":
                        careerinfo.Status = GlobalSetting.ArticleStatus.Published;
                        result = saveCareerOpportunity(careerinfo);
                        break;
                    case "updateCareer":
                        careerinfo.ID = Convert.ToInt32(Request.Form["ID"]);
                        careerinfo.VersionNo = Convert.ToSingle(Request.Form["VersionNo"]);
                        careerinfo.VersionNo += (float)0.01;
                        result = updateCareerOpportunity(careerinfo);
                        break;
                }

                break;
            case "createNews":
                result = this.createNews();
                break;
            case "updateNews":
                result = this.updateNews();
                break;
            case "uploadFile":
                result = this.uploadFile();
                break;
            case "updateFile":
                result = this.updateFile();
                break;
            case "getFileList":
                result = this.getFileList();
                break;
            case "GetDirectoryList":
                result = this.GetDirectoryList();
                break;
            case "autoComplete":
                result = this.GetPath();
                break;
            case "getTrainingSchedule":
                int ID = Convert.ToInt32(Request.Form["ID"]);
                result = this.getTrainingSchedule(ID);
                break;
            case "getSystemPara":
                string category2 = Request.Form["category"];
                string includeabandon = Request.Form["includeabandon"];
                result = getSystemPara(category2, includeabandon == "true");
                break;
            case "getSystemLinkList":
                result = getSystemLinkList();
                break;
            case "saveSystemLink":
                result = saveSystemLink();
                break;
            case "updateSystemLink":
                result = updateSystemLink();
                break;
            case "deleteSystemLinks":
                result = deleteSystemLinks();
                break;
            case "getImageJsonArray":
                int newsID = Convert.ToInt32(Request.Form["ID"]);
                result = getImageJsonArray(newsID);
                break;
            case "getFileJsonArray":
                int newsID2 = Convert.ToInt32(Request.Form["ID"]);
                result = getFileJsonArray(newsID2);
                break;
            case "getEventImageJsonArray":
                int eventID = Convert.ToInt32(Request.Form["ID"]);
                result = getEventImageJsonArray(eventID);
                break;
        }
        return result;
    }
    private string getEventByMonth(DateTime dateOfMonth)
    {
        Event eventHandler = new Event();
        Jayrock.Json.JsonArray result = eventHandler.getEventByMonth(dateOfMonth);

        return result.ToString();
    }
    private string saveEvent(EventInfo eventinfo)
    {
        Event eventHandler = new Event();

        //create Image info object start
        List<ImageInfo> imageList = new List<ImageInfo>();
        ImageInfo image;
        byte[] tmpImage;
        foreach (string key in Request.Files.Keys)
        {
            if (Request.Files[key].ContentLength == 0) continue;
            image = new ImageInfo();
            image.Type = key.Remove(key.Length - 1);
            image.FileName = Request.Files[key].FileName;
            image.FileName = image.FileName.Substring(image.FileName.LastIndexOf("\\") + 1);

            if (Request.Files[key].ContentType.IndexOf("image/") != 0)
            {
                return "{\"error\": \"Invalid image found: " + image.FileName + "\"}";
            }



            using (BinaryReader br = new BinaryReader(Request.Files[key].InputStream))
            {
                tmpImage = br.ReadBytes(Request.Files[key].ContentLength);
            }
            image.Extension = image.FileName.Substring(image.FileName.LastIndexOf("."));
            image.MIME = Request.Files[key].ContentType; 

           // tmpImage = ImageResize.ResizeImage(Request.Files[key].InputStream);
            image.ContentLength = (int)tmpImage.Length;
            image.Content = tmpImage;
            imageList.Add(image);
        }
        //create Image info object End

        eventHandler.save(eventinfo, imageList);
        Jayrock.Json.JsonObject result = new JsonObject();
        result.Accumulate("message", "Done");
        result.Accumulate("success", true);
        return result.ToString();
    }
    private string updateEvent(EventInfo eventinfo)
    {
        Event eventHandler = new Event();

        //create Image info object start
        List<ImageInfo> imageList = new List<ImageInfo>();
        ImageInfo image;
        byte[] tmpImage;
        string tmpID;
        string status;
        string[] tmpArray;
        foreach (string key in Request.Files.Keys)
        {
            if (Request.Files[key].ContentLength == 0) continue;
            if (string.IsNullOrEmpty(Request.Form["status_" + key])) continue;


            image = new ImageInfo();  
            image.FileName = Request.Files[key].FileName;
            image.FileName = image.FileName.Substring(image.FileName.LastIndexOf("\\") + 1);

            if (Request.Files[key].ContentType.IndexOf("image/") != 0)
            {
                return "{\"error\": \"Invalid image found: " + image.FileName + "\"}";
            }

            tmpArray = Request.Form["status_" + key].Split('-');
            tmpID = tmpArray[0];
            status = tmpArray[1];
            if (status != "updated") continue; 
            image.ID = Convert.ToInt32(tmpID);
            image.Type = key.Remove(key.Length - 1); 

            //image.Extension = ".jpg";
            //image.MIME = "image/jpeg";
            //tmpImage = ImageResize.ResizeImage(Request.Files[key].InputStream);

            using (BinaryReader br = new BinaryReader(Request.Files[key].InputStream))
            {
                tmpImage = br.ReadBytes(Request.Files[key].ContentLength);
            }
            image.Extension = image.FileName.Substring(image.FileName.LastIndexOf("."));
            image.MIME = Request.Files[key].ContentType; 

            image.ContentLength = (int)tmpImage.Length;
            image.Content = tmpImage;

            imageList.Add(image);
        }
        //create Image info object End

        bool resetAttendance = Convert.ToBoolean(Request.Form["resetAttendance"]);

        eventHandler.update(eventinfo, imageList, resetAttendance);
        Jayrock.Json.JsonObject result = new JsonObject();
        result.Accumulate("message", "Done");
        result.Accumulate("success", true);
        return result.ToString();
    }
    private string saveTraining(TrainingInfo trainingInfo)
    {
        Training trainingHandler = new Training();
         

        trainingHandler.save(trainingInfo);
        Jayrock.Json.JsonObject result = new JsonObject();
        result.Accumulate("message", "Done");
        return result.ToString();
    }
    private string updateTraining(TrainingInfo trainingInfo)
    {
        Training trainingHandler = new Training();
        bool resetAttendance = Convert.ToBoolean(Request.Form["ResetAttendance"]);

        //get deleted form ID
        string deletedFormID = Request.Form["TrainingFormDeleted"];


        trainingHandler.update(trainingInfo, resetAttendance, deletedFormID);
        Jayrock.Json.JsonObject result = new JsonObject();
        result.Accumulate("message", "Done");
        return result.ToString();
    }
    private string saveCareerOpportunity(CareerOpportunityInfo careerOpportunityInfo)
    {
        CareerOpportunity careerOpportunityHandler = new CareerOpportunity();
        careerOpportunityHandler.save(careerOpportunityInfo);
        Jayrock.Json.JsonObject result = new JsonObject();
        result.Accumulate("message", "Done");
        return result.ToString();
    }
    private string updateCareerOpportunity(CareerOpportunityInfo careerOpportunityInfo)
    {
        CareerOpportunity careerOpportunityHandler = new CareerOpportunity();
        careerOpportunityHandler.update(careerOpportunityInfo);
        Jayrock.Json.JsonObject result = new JsonObject();
        result.Accumulate("message", "Done");
        return result.ToString();
    }

    private string getArticleCategory()
    {
        //Article articleHandler = new Article();
        Jayrock.Json.JsonObject articleStatus = new JsonObject(); //= articleHandler.getArticleCategory();
        articleStatus.Accumulate(GlobalSetting.ArticleCategory.News, GlobalSetting.ArticleCategory.News);
        articleStatus.Accumulate(GlobalSetting.ArticleCategory.Training, GlobalSetting.ArticleCategory.Training);
        articleStatus.Accumulate(GlobalSetting.ArticleCategory.Event, GlobalSetting.ArticleCategory.Event);
        articleStatus.Accumulate(GlobalSetting.ArticleCategory.Career, GlobalSetting.ArticleCategory.Career);
        return articleStatus.ToString();
    }
    private string getArticleByCategory(string category)
    {
        Jayrock.Json.JsonArray result;
        switch (category)
        {
            case GlobalSetting.ArticleCategory.News:
                News articleHandler = new News();
                result = articleHandler.getAllArticle();
                break;
            case GlobalSetting.ArticleCategory.Event:
                Event eventHandler = new Event();
                result = eventHandler.getAllEvent();
                break;
            case GlobalSetting.ArticleCategory.Training:
                Training trainingHandler = new Training();
                result = trainingHandler.getAllTraining();
                break;
            case GlobalSetting.ArticleCategory.Career:
                CareerOpportunity careerHandler = new CareerOpportunity();
                result = careerHandler.getAllCareerOpportunities();
                break;
            default:
                result = new JsonArray();
                break;
        }
        return result.ToString();
    }
    private string updateArticleStatus(string articles, string newStatus, string category)
    {

        try
        {
            switch (category)
            {
                case "News":
                    News articleHandler = new News();
                    articleHandler.updateStatus(articles, newStatus);
                    break;
                case "Event":
                    Event eventHandler = new Event();
                    eventHandler.updateStatus(articles, newStatus);
                    break;
                case "Training":
                    Training trainingHandler = new Training();
                    trainingHandler.updateStatus(articles, newStatus);
                    break;
                case "Career":
                    CareerOpportunity careerOpportunityHandler = new CareerOpportunity();
                    careerOpportunityHandler.updateStatus(articles, newStatus);
                    break;
            }
        }
        catch (Exception ex)
        {
            return "{ \"result\": false, \"message\": \"" + ex.Message + "\" }";
        }

        return "{ \"result\": true, \"message\": \"Status is updated.\" }";
    }
    private string logActivity(string userID, string activityID, string category, string action, string schedules)
    {
        User user = new User();
        if (schedules != null)
        {
            user.logActivity(userID, activityID, category, action, schedules.Split(','));
        }
        else
        {
            user.logActivity(userID, activityID, category, action);
        }

        Jayrock.Json.JsonObject result = new JsonObject();
        result.Accumulate("message", "Done");
        return result.ToString();
    }
    private string createNews()
    {
        //new article object
        News article = new News();
        //create News info object
        NewsInfo articleInfo = new NewsInfo();
        int effectiveYear = Convert.ToInt32(Request.Form["ddlYear"]);
        int effectiveMonth = Convert.ToInt32(Request.Form["ddlMonth"]);
        int effectiveDay = Convert.ToInt32(Request.Form["ddlDay"]);

        int iconExpiryFromYear = Convert.ToInt32(Request.Form["ddlIconExpiryYear"]);
        int iconExpiryFromMonth = Convert.ToInt32(Request.Form["ddlIconExpiryMonth"]);
        int iconExpiryFromDay = Convert.ToInt32(Request.Form["ddlIconExpiryDay"]);
        
        if (iconExpiryFromYear > 0 && iconExpiryFromMonth > 0 && iconExpiryFromDay > 0)
        {
            articleInfo.NewIconInfo = new NewIconInfo();
            articleInfo.NewIconInfo.Category = GlobalSetting.ArticleCategory.News;
            articleInfo.NewIconInfo.ExpiryDate = new DateTime(iconExpiryFromYear, iconExpiryFromMonth, iconExpiryFromDay);
        }

        articleInfo.Type = Convert.ToInt32(Request.Form["comType"]);
        articleInfo.SerialNo = Request.Form["txtSerialNo"].ToUpper();
        articleInfo.Title = Request.Form["txtTitle"];
        articleInfo.Headline = Request.Form["txtHeadline"];
        articleInfo.Summary = Request.Form["txtSummary"];
        articleInfo.Content = Request.Form["txtMainBody"];
        articleInfo.EffectiveDate = new DateTime(effectiveYear, effectiveMonth, effectiveDay);
        articleInfo.CreateBy = Convert.ToInt32(Session["LOGINID"]);
        articleInfo.Status = GlobalSetting.ArticleStatus.Unpublished;
        //create Image info object
        List<ImageInfo> imageList = new List<ImageInfo>();
        ImageInfo image;
        byte[] tmpImage;
        foreach (string key in Request.Files.Keys)
        {
            if (key.Contains("Attachment")) continue;
            if (Request.Files[key].ContentLength > GlobalSetting.MaxImageLength * 1024)
            {
                return "{\"error\": \"Image size must be smaller than " + GlobalSetting.MaxImageLength + " KB\"}";
            };
            if (Request.Files[key].ContentLength == 0) break;
            image = new ImageInfo();
            image.FileName = Request.Files[key].FileName;
            image.FileName = image.FileName.Substring(image.FileName.LastIndexOf("\\") + 1);
            if (Request.Files[key].ContentType.IndexOf("image/") != 0)
            {
                return "{\"error\": \"Invalid image found: " + image.FileName + "\"}";
            }
            image.Type = key.Remove(key.Length - 1);

            image.Description = Request.Form[key] == null ? image.FileName : Request.Form[key];

            if (key.Contains("Thumbnail") || key.Contains("Enlarge0"))
            { 
                using (BinaryReader br = new BinaryReader(Request.Files[key].InputStream))
                {
                    tmpImage = br.ReadBytes(Request.Files[key].ContentLength);
                }
                image.Extension = image.FileName.Substring(image.FileName.LastIndexOf("."));
                image.MIME = Request.Files[key].ContentType;
            }    
            else
            {
                tmpImage = ImageResize.ResizeImage(Request.Files[key].InputStream);
                image.Extension = ".jpg";
                image.MIME = "image/jpeg";
            }
                

            image.ContentLength = (int)tmpImage.Length;
            image.Content = tmpImage;
            imageList.Add(image);
        }



        List<FileInfo> attachmentList = new List<FileInfo>();
        FileInfo attachment;
        byte[] tmpFile;
        foreach (string key in Request.Files.Keys)
        {
            if (!key.Contains("Attachment")) continue;

            if (Request.Files[key].ContentLength > GlobalSetting.MaxImageLength * 1024)
            {
                return "{\"error\": \"File size must be smaller than " + GlobalSetting.MaxImageLength + " KB\"}";
            };

            if (Request.Files[key].ContentLength == 0) break;

            attachment = new FileInfo();
            attachment.OriginalName = Request.Files[key].FileName;
            attachment.OriginalName = attachment.OriginalName.Substring(attachment.OriginalName.LastIndexOf("\\") + 1); 
            attachment.Name = attachment.OriginalName;
            attachment.Description = attachment.OriginalName;

            using (BinaryReader br = new BinaryReader(Request.Files[key].InputStream))
            {
                tmpFile = br.ReadBytes(Request.Files[key].ContentLength);
            }
            attachment.Content = tmpFile;
            attachment.Path = "News Attachment";
            attachment.UploadDate = DateTime.Now;
            attachmentList.Add(attachment);
        }


        //create aricle
        article.createNews(articleInfo, imageList, attachmentList);
        //reponse the result to client side via the hidden field
        //hfMessage.Value = "Done";
        return "{\"success\": true, \"message\": \"Done\" }";
    }
    private string updateNews()
    {
        //new article object
        News article = new News();
        //create News info object
        NewsInfo articleInfo = new NewsInfo();
        int effectiveYear = Convert.ToInt32(Request.Form["ddlYear"]);
        int effectiveMonth = Convert.ToInt32(Request.Form["ddlMonth"]);
        int effectiveDay = Convert.ToInt32(Request.Form["ddlDay"]);

        int iconExpiryFromYear = Convert.ToInt32(Request.Form["ddlIconExpiryYear"]);
        int iconExpiryFromMonth = Convert.ToInt32(Request.Form["ddlIconExpiryMonth"]);
        int iconExpiryFromDay = Convert.ToInt32(Request.Form["ddlIconExpiryDay"]);
         

        if (iconExpiryFromYear > 0 && iconExpiryFromMonth > 0 && iconExpiryFromDay > 0)
        {
            articleInfo.NewIconInfo = new NewIconInfo();
            articleInfo.NewIconInfo.Category = GlobalSetting.ArticleCategory.News;
            articleInfo.NewIconInfo.ExpiryDate = new DateTime(iconExpiryFromYear, iconExpiryFromMonth, iconExpiryFromDay); 
        }

        articleInfo.ID = Convert.ToInt32(Request.Form["hdfID"]);
        articleInfo.Type = Convert.ToInt32(Request.Form["comType"]);
        articleInfo.VersionNo = Convert.ToSingle(Request.Form["txtVersionNo"]);
        articleInfo.VersionNo += (float)0.01;
        articleInfo.SerialNo = Request.Form["txtSerialNo"].ToUpper();
        articleInfo.Title = Request.Form["txtTitle"];
        articleInfo.Headline = Request.Form["txtHeadline"];
        articleInfo.Summary = Request.Form["txtSummary"];
        articleInfo.Content = Request.Form["txtMainBody"];
        articleInfo.UpdateBy = Convert.ToInt32(Session["LOGINID"]);
        articleInfo.EffectiveDate = new DateTime(effectiveYear, effectiveMonth, effectiveDay);

        //create Image info object
        List<ImageInfo> imageListToAdd = new List<ImageInfo>();
        Dictionary<int, ImageInfo> imageDictToUpdate = new Dictionary<int, ImageInfo>();
        Dictionary<int, ImageInfo> imageDescDictToUpdate = new Dictionary<int, ImageInfo>();
        System.Text.StringBuilder imageIDToDelete = new System.Text.StringBuilder();
        ImageInfo image;
        byte[] tmpImage;
        string[] valueArray;
        string status;
        string tmpImageID;
        string key;
        foreach (string formKey in Request.Form.Keys)
        {
            if (!formKey.StartsWith("status_")) continue;
            key = formKey.Replace("status_", "");
            if (string.IsNullOrEmpty(Request.Form[formKey])) continue;

            valueArray = Request.Form[formKey].Split('-');
            tmpImageID = valueArray[0];
            status = valueArray[1];
            switch (status)
            {
                case "added":
                    if (Request.Files[key].ContentLength == 0) continue;
                    if (Request.Files[key].ContentType.IndexOf("image/") != 0)
                    {
                        return "{\"error\": \"Invalid image found: " + Request.Files[key].FileName + "\"}";
                    }

                    image = new ImageInfo();
                    image.Type = key.Remove(key.Length - 1);
                    image.FileName = Request.Files[key].FileName;
                    image.FileName = image.FileName.Substring(image.FileName.LastIndexOf("\\") + 1);

                    image.Description = Request.Form[key] == null ? image.FileName : Request.Form[key];
                   // image.Extension = ".jpg";
                   // image.MIME = "image/jpeg";
                    //tmpImage = ImageResize.ResizeImage(Request.Files[key].InputStream);


                    if (key.Contains("Thumbnail"))
                    {
                        using (BinaryReader br = new BinaryReader(Request.Files[key].InputStream))
                        {
                            tmpImage = br.ReadBytes(Request.Files[key].ContentLength);
                        }
                        image.Extension = image.FileName.Substring(image.FileName.LastIndexOf("."));
                        image.MIME = Request.Files[key].ContentType;
                    }
                    else
                    {
                        tmpImage = ImageResize.ResizeImage(Request.Files[key].InputStream);
                        image.Extension = ".jpg";
                        image.MIME = "image/jpeg";
                    }

                    image.ContentLength = (int)tmpImage.Length;
                    image.Content = tmpImage;
                    imageListToAdd.Add(image);
                    break;
                case "updated":
                    if (Request.Files[key].ContentLength == 0) continue;
                    if (Request.Files[key].ContentType.IndexOf("image/") != 0)
                    {
                        return "{\"error\": \"Invalid image found: " + Request.Files[key].FileName + "\"}";
                    }
                    image = new ImageInfo();
                    image.ID = Convert.ToInt32(tmpImageID);
                    image.Type = key.Remove(key.Length - 1);
                    image.FileName = Request.Files[key].FileName;
                    image.FileName = image.FileName.Substring(image.FileName.LastIndexOf("\\") + 1);

                    image.Description = Request.Form[key] == null ? image.FileName : Request.Form[key];
                    //image.Extension = ".jpg";
                   // image.MIME = "image/jpeg";

                    //tmpImage = ImageResize.ResizeImage(Request.Files[key].InputStream); 

                    if (key.Contains("Thumbnail") || key.Contains("Enlarge0"))
                    {
                        using (BinaryReader br = new BinaryReader(Request.Files[key].InputStream))
                        {
                            tmpImage = br.ReadBytes(Request.Files[key].ContentLength);
                        }
                        image.Extension = image.FileName.Substring(image.FileName.LastIndexOf("."));
                        image.MIME = Request.Files[key].ContentType;
                    }
                    else
                    {
                        tmpImage = ImageResize.ResizeImage(Request.Files[key].InputStream);
                        image.Extension = ".jpg";
                        image.MIME = "image/jpeg";
                    }

                    image.ContentLength = (int)tmpImage.Length;
                    image.Content = tmpImage;
                    imageDictToUpdate.Add(image.ID, image);
                    break;
                case "descchanged":
                    image = new ImageInfo();
                    image.ID = Convert.ToInt32(tmpImageID);
                    image.Description = Request.Form[key] == null ? image.FileName : Request.Form[key];
                    imageDescDictToUpdate.Add(image.ID, image);
                    break;
                case "deleted":
                    if (imageIDToDelete.Length == 0)
                    {
                        imageIDToDelete.Append(tmpImageID);
                    }
                    else
                    {
                        imageIDToDelete.Append(",");
                        imageIDToDelete.Append(tmpImageID);
                    }
                    break;
            }
        }

        //update file

        //create File info object
        List<FileInfo> fileListToAdd = new List<FileInfo>();
        Dictionary<int, FileInfo> fileDictToUpdate = new Dictionary<int, FileInfo>();
        Dictionary<int, FileInfo> fileDescDictToUpdate = new Dictionary<int, FileInfo>();
        System.Text.StringBuilder fileIDToDelete = new System.Text.StringBuilder();
        FileInfo file;
        byte[] tmpFile; 
        string tmpFileID; 
        foreach (string formKey in Request.Form.Keys)
        {
            if (!formKey.StartsWith("attachmentStatus_")) continue;
            key = formKey.Replace("attachmentStatus_", "");
            if (string.IsNullOrEmpty(Request.Form[formKey])) continue;

            valueArray = Request.Form[formKey].Split('-');
            tmpFileID = valueArray[0];
            status = valueArray[1];
            switch (status)
            {
                case "added":
                    if (Request.Files[key].ContentLength == 0) continue;

                    file = new FileInfo();
                    file.OriginalName = Request.Files[key].FileName;
                    file.OriginalName = file.OriginalName.Substring(file.OriginalName.LastIndexOf("\\") + 1);
                    file.Name = file.OriginalName;
                    file.Description = file.OriginalName;

                    using (BinaryReader br = new BinaryReader(Request.Files[key].InputStream))
                    {
                        tmpFile = br.ReadBytes(Request.Files[key].ContentLength);
                    }
                    file.Content = tmpFile;
                    file.Path = "News Attachment";
                    file.UploadDate = DateTime.Now;

                    fileListToAdd.Add(file);

                    break;

                case "updated":
                    if (Request.Files[key].ContentLength == 0) 
                    {
                        if (fileIDToDelete.Length == 0)
                        {
                            fileIDToDelete.Append(tmpFileID);
                        }
                        else
                        {
                            fileIDToDelete.Append(",");
                            fileIDToDelete.Append(tmpFileID);
                        } 
                    }
                    else
                    {

                        file = new FileInfo();
                        file.ID = Convert.ToInt32(tmpFileID);
                        file.OriginalName = Request.Files[key].FileName;
                        file.OriginalName = file.OriginalName.Substring(file.OriginalName.LastIndexOf("\\") + 1);
                        file.Name = file.OriginalName;
                        file.Description = file.OriginalName;

                        using (BinaryReader br = new BinaryReader(Request.Files[key].InputStream))
                        {
                            tmpFile = br.ReadBytes(Request.Files[key].ContentLength);
                        }
                        file.Content = tmpFile;

                        fileDictToUpdate.Add(file.ID, file);
                    }


                    break; 
            }
        }
         
        //create aricle
        article.UpdateNews(articleInfo, imageListToAdd, imageDictToUpdate, imageDescDictToUpdate, imageIDToDelete.ToString(),
             fileListToAdd, fileDictToUpdate, fileIDToDelete.ToString());


        return "{\"success\": true, \"message\": \"Done\" }";

    }
    private string uploadFile()
    {
        File file = new File();
        FileInfo fileInfo = new FileInfo();
        byte[] fileContent;
        int fileContentLength;
        List<DirectoryInfo> directories = new List<DirectoryInfo>();
        DirectoryInfo tmpDirectory = null;
        string tmp;
        foreach (string key in Request.Files.Keys)
        {
            if (Request.Files[key].ContentLength == 0) break;
            fileInfo.Type = Convert.ToInt32(Request.Form["comType"]);
            fileInfo.Name = Request.Form["txtFileName"];
            fileInfo.Description = Request.Form["txtDescription"];
            fileInfo.OriginalName = Request.Files[key].FileName;
            fileInfo.OriginalName = fileInfo.OriginalName.Substring(fileInfo.OriginalName.LastIndexOf("\\") + 1);

            fileInfo.UploadUser = Convert.ToInt32(Session["LoginID"]);
            fileInfo.UploadDate = DateTime.Now;

            using (BinaryReader br = new BinaryReader(Request.Files[key].InputStream))
            {
                fileContentLength = (int)Request.Files[key].ContentLength;
                fileContent = br.ReadBytes(fileContentLength);
            }
            string directory = Request.Form["txtDirectory"];
            string[] directoryArray = directory.Split('/');
            for (int i = 0; i < directoryArray.Length; i++)
            {
                tmp = directoryArray[i];
                tmpDirectory = new DirectoryInfo();
                tmpDirectory.Depth = i;
                tmpDirectory.Type = fileInfo.Type;
                tmpDirectory.Name = tmp.Trim();
                directories.Add(tmpDirectory);
            }
            file.UploadFile(fileInfo, directories, fileContent);
            break;

        }
        return "{\"message\": \"Done\" }";
    }
    private string updateFile()
    {
        File file = new File();
        FileInfo fileInfo = new FileInfo();
        byte[] fileContent = null;
        int fileContentLength;
        DirectoryInfo tmpDirectory = new DirectoryInfo(); 
        foreach (string key in Request.Files.Keys)
        {
            //if (Request.Files[key].ContentLength == 0) break;
           // fileInfo.Type = Convert.ToInt32(Request.Form["comType"]);
            if (Request.Files[key].ContentLength != 0)
            { 
                fileInfo.OriginalName = Request.Files[key].FileName;
                fileInfo.OriginalName = fileInfo.OriginalName.Substring(fileInfo.OriginalName.LastIndexOf("\\") + 1);

                using (BinaryReader br = new BinaryReader(Request.Files[key].InputStream))
                {
                    fileContentLength = (int)Request.Files[key].ContentLength;
                    fileContent = br.ReadBytes(fileContentLength);
                    fileInfo.Content = fileContent; 
                }
            }
            fileInfo.ID = Convert.ToInt32(Request.Form["ID"]);
            fileInfo.Name = Request.Form["txtFileName"];
            fileInfo.Description = Request.Form["txtDescription"];
            string directory = Request.Form["txtDirectory"];
            tmpDirectory.FullName = directory;
            tmpDirectory.ID = Convert.ToInt32(Request.Form["DirectoryID"]);
            string[] directoryArray = directory.Split('/');
            tmpDirectory.Name = directoryArray[directoryArray.Length - 1];
               
            return file.UpdateFile(fileInfo, tmpDirectory, fileContent);
            //break;

        }
        return "{\"error\": \"File not found\" }";
    }
    private string getFileList()
    {
        File file = new File();
        string type = Request.Form["type"];
        string directory = Request.Form["directory"];
        JsonArray ja = file.GetFileList(type, directory);
        return ja.ToString();
    }
    private string GetDirectoryList()
    {
        File file = new File();
        string type = Request.Form["type"];
        JsonArray ja = file.GetDirectoryList(type);
        return ja.ToString();

    }
    private string GetPath()
    {
        File file = new File();
        string term = Request.Form["term"];
        string type = Request.Form["type"];
        return file.GetPath(term, type);
    }
    private string publishFiles()
    {
        string result = string.Empty;
        String fileIDs = Request.Form["IDs[]"];
        if (fileIDs.Split(',').Length > 1000)
        {
            result = "{\"message\": \"Too many files selected.\"}";
        }
        else
        {
            String loginID = Session["LOGINID"].ToString();
            File file = new File();
            file.updateFilesStatus(fileIDs, GlobalSetting.ArticleStatus.Published);
            result = "{\"message\": \"Done.\"}";
        }
        return result;
    }
    private string unpublishFiles()
    {
        string result = string.Empty;
        String fileIDs = Request.Form["IDs[]"];
        if (fileIDs.Split(',').Length > 1000)
        {
            result = "{\"message\": \"Too many files selected.\"}";
        }
        else
        {
            String loginID = Session["LOGINID"].ToString();
            File file = new File();
            file.updateFilesStatus(fileIDs, GlobalSetting.ArticleStatus.Unpublished);
            result = "{\"message\": \"Done.\"}";
        }
        return result;
    }
    private string deleteFiles()
    {
        string result = string.Empty;
        String fileIDs = Request.Form["IDs[]"];
        if (fileIDs.Split(',').Length > 1000)
        {
            result = "{\"message\": \"Too many files selected.\"}";
        }
        else
        {
            String loginID = Session["LOGINID"].ToString();
            File file = new File();
            file.DeleteFiles(fileIDs);
            result = "{\"message\": \"Done.\"}";
        }
        return result;
    }
    private string getTrainingSchedule(int ID)
    {
        Training training = new Training();
        return training.getTrainingSchedule(ID).ToString();
    }
    private string getSystemPara(string category, bool includeabandon)
    {
        JsonArray ja = new JsonArray();
        JsonObject jo;
        System.Data.DataTable dt = SystemPara.getSystemPara(category, includeabandon);
        foreach (System.Data.DataRow row in dt.Rows)
        {
            jo = new JsonObject();
            foreach (System.Data.DataColumn col in dt.Columns)
            {
                jo.Accumulate(col.ColumnName.ToLower(), row[col.ColumnName]);
            }
            ja.Add(jo);
        }
        return ja.ToString();
    }
    private string getSystemLinkList()
    {
        OtherSystemLink handler = new OtherSystemLink();
        return handler.getSystemLinkList().ToString();
    }
    private string saveSystemLink()
    {
        OtherSystemLink handler = new OtherSystemLink();
        OtherSystemLinkInfo info = new OtherSystemLinkInfo();
        info.Name = Request.Form["Name"];
        info.Link = Request.Form["Link"];
        if (!info.Link.ToLower().Contains("http"))
        {
            info.Link = "http://" + info.Link;
        }
        info.CreateBy = Convert.ToInt32(Session["LoginID"]);
        handler.save(info);
        JsonObject jo = new JsonObject();
        jo.Accumulate("message", "Done");
        return jo.ToString();
    }
    private string updateSystemLink()
    {
        OtherSystemLink handler = new OtherSystemLink();
        OtherSystemLinkInfo info = new OtherSystemLinkInfo();
        info.ID = Convert.ToInt32(Request.Form["ID"]);
        info.Name = Request.Form["Name"];
        info.Link = Request.Form["Link"];
        if (!info.Link.ToLower().Contains("http"))
        {
            info.Link = "http://" + info.Link;
        }
        info.CreateBy = Convert.ToInt32(Session["LoginID"]);
        handler.update(info);

        JsonObject jo = new JsonObject();
        jo.Accumulate("message", "Done");
        return jo.ToString();
    }
    private string deleteSystemLinks()
    {
        OtherSystemLink handler = new OtherSystemLink();
        OtherSystemLinkInfo info = new OtherSystemLinkInfo();
        string IDs = Request.Form["IDs[]"];
        handler.delete(IDs);
        JsonObject jo = new JsonObject();
        jo.Accumulate("message", "Done");
        return jo.ToString();
    }
    private string getImageJsonArray(int ID)
    {
        NewsInfo newsInfo = new NewsInfo();
        newsInfo.ID = ID;
        return newsInfo.getImageJsonArray().ToString();
    }
    private string getFileJsonArray(int ID)
    {
        NewsInfo newsInfo = new NewsInfo();
        newsInfo.ID = ID;
        return newsInfo.getFileJsonArray().ToString();
    }
    private string getEventImageJsonArray(int ID)
    {
        EventInfo eventInfo = new EventInfo();
        eventInfo.ID = ID;
        return eventInfo.getImageJsonArray().ToString();
    }
    protected string commitSuggestion()
    {
        SuggestionInfo info = new SuggestionInfo();
        info.UserName = Request.Form["UserName"];
        info.Email = Request.Form["Email"];
        info.Type = Convert.ToInt32(Request.Form["Type"]);
        info.PhoneNumber = Request.Form["PhoneNumber"];
        info.OtherEmail = Request.Form["OtherEmail"];
        info.Suggestion = Request.Form["Suggestion"];
        info.CreateBy = Convert.ToInt32(Session["LOGINID"]);
        User userHandler = new User();
        userHandler.commitSuggestion(info);
        return "{\"message\":\"Done\"}";
    }
    protected string getSystemTypeItemList()
    {
        string category = Request.Form["category"];
        System.Data.DataTable dt = SystemPara.getSystemParaDetails(category);
        JsonArray ja = new JsonArray();
        JsonObject tmpJo;
        foreach (System.Data.DataRow row in dt.Rows)
        {
            tmpJo = new JsonObject();
            foreach (System.Data.DataColumn col in dt.Columns)
            {
                tmpJo.Accumulate(col.ColumnName.ToLower(), row[col.ColumnName].ToString());
            }
            ja.Add(tmpJo);
        }
        return ja.ToString();
    }
    protected string addSystemItem()
    {
        SystemPara sysPara = new SystemPara();
        string category = Request.Form["Category"];
        string categoryDesc = Request.Form["CategoryDesc"];
        string description = Request.Form["Description"];
        string cdescription = Request.Form["cDescription"].ToUpper();
        bool isMultiDesc = Convert.ToBoolean(Request.Form["IsMultiDesc"]);
        sysPara.save(category, categoryDesc, description, cdescription, isMultiDesc);

        return "{\"message\":\"Done\"}";
    }
    protected string updateSystemItem()
    {
        SystemPara sysPara = new SystemPara();
        int ID = Convert.ToInt32(Request.Form["ID"]);
        string description = Request.Form["Description"];
        int cID = string.IsNullOrEmpty(Request.Form["cID"]) ? 0 : Convert.ToInt32(Request.Form["cID"]);
        string cdescription = Request.Form["cDescription"].ToUpper();
        sysPara.update(ID, description, cID, cdescription);
        return "{\"message\":\"Done\"}";
    }
    protected string abandonSystemItem()
    {
        SystemPara sysPara = new SystemPara();
        int ID = Convert.ToInt32(Request.Form["ID"]);
        sysPara.abandon(ID);
        return "{\"message\":\"Done\"}";
    }
    protected string getTrainingDecision()
    {
        Training trainingHandler = new Training();
        int ID = Convert.ToInt32(Request.Form["ID"]);
        int loginID = Convert.ToInt32(Request.Form["loginID"]);
        return trainingHandler.getTrainingDecision(ID, loginID).ToString();
    }

    private string getAllUsers()
    {
        User userHandler = new User();
        Jayrock.Json.JsonArray users = userHandler.getAllUsers();
        return users.ToString();
    }
    private string updateStatus(string userIDs, string loginID, string newStatus)
    {
        User userHandler = new User();
        try
        {
            userHandler.updateStatus(userIDs, loginID, newStatus);
        }
        catch (Exception ex)
        {
            return "{ \"result\": false, \"message\": \"" + ex.Message + "\" }";
        }

        return "{ \"result\": true, \"message\": \"Status is updated.\" }";
    }

    private string getUserStatus()
    {
        User userHandler = new User();
        Jayrock.Json.JsonObject usersStatus = userHandler.getUserStatus();
        return usersStatus.ToString();
    }

    private string Logout()
    {
        Session.Clear();
        Jayrock.Json.JsonObject result = new JsonObject();
        result.Accumulate("message", "Done");
        return result.ToString();
    }
    
    private string getTrainingFormList()
    { 
        Jayrock.Json.JsonArray ja;
        int trainingID = Convert.ToInt32(Request.Form["ID"]);
        Training trainingHandler = new Training();
        ja = trainingHandler.getTrainingFormList(trainingID);
        return ja.ToString();
    }
    private string updateUserRole()
    {

        String userIDs = Request.Form["IDs[]"];
        String role = Request.Form["Role"];

        User userHandler = new User();
        try
        {
            userHandler.updateUserRole(userIDs, role);
        }
        catch (Exception ex)
        {
            return "{ \"result\": false, \"message\": \"" + ex.Message + "\" }";
        }

        return "{ \"result\": true, \"message\": \"User role is updated.\" }";
    }
    private string createLinkIcon()
    {
        LinkIcon handler = new LinkIcon();

        LinkIconInfo info = new LinkIconInfo();
        info.IconName = Request.Form["txtIconName"];
        info.Link = Request.Form["txtLink"];
        info.SequenceNo = Convert.ToInt32(Request.Form["txtSequenceNo"]);
        info.CreateBy = Convert.ToInt32(Session["LOGINID"]);
        info.CreateDate = DateTime.Now;

        ImageInfo image = null;
        byte[] tmpImage;
        foreach (string key in Request.Files.Keys)
        {

            if (Request.Files[key].ContentLength == 0)
            {
                return "{\"error\": \"Icon required\"}";
            };

            image = new ImageInfo();
            image.Type = key.Remove(key.Length - 1);
            image.FileName = Request.Files[key].FileName;
            image.FileName = image.FileName.Substring(image.FileName.LastIndexOf("\\") + 1);

            if (Request.Files[key].ContentType.IndexOf("image/") != 0)
            {
                return "{\"error\": \"Invalid image found: " + image.FileName + "\"}";
            }



            image.Extension = ".png";
            image.MIME = "image/png";
            tmpImage = ImageResize.ResizeImage(Request.Files[key].InputStream, System.Drawing.Imaging.ImageFormat.Png);
            image.ContentLength = (int)tmpImage.Length;
            image.Content = tmpImage;
        }

        handler.Create(info, image);

        return "{\"message\": \"Done\"}";
    }
    private string updateLinkIcon()
    {
        LinkIcon handler = new LinkIcon();
        int ID = Convert.ToInt32(Request.Form["ID"]);
        LinkIconInfo info = handler.getLinkIconByID(ID); 
        info.IconName = Request.Form["txtIconName"];
        info.Link = Request.Form["txtLink"];
        info.SequenceNo = Convert.ToInt32(Request.Form["txtSequenceNo"]);
        info.UpdateBy = Convert.ToInt32(Session["LOGINID"]);
        info.UpdateDate = DateTime.Now;

        ImageInfo image = null;
        byte[] tmpImage;
        foreach (string key in Request.Files.Keys)
        { 
            if (Request.Files[key].ContentLength > 0 && Request.Files[key].ContentType.IndexOf("image/") == 0)
            { 
                image = new ImageInfo();
                image.Extension = ".png";
                image.MIME = "image/png";
                tmpImage = ImageResize.ResizeImage(Request.Files[key].InputStream, System.Drawing.Imaging.ImageFormat.Png);
                image.ContentLength = (int)tmpImage.Length;
                image.Content = tmpImage;

                image.Type = key.Remove(key.Length - 1);
                image.FileName = Request.Files[key].FileName;
                image.FileName = image.FileName.Substring(image.FileName.LastIndexOf("\\") + 1);
            };

            break;

        }

        handler.Update(info, image);

        return "{\"message\": \"Done\"}";
    }
    private string getAllIconJsonArray()
    {
        LinkIcon handler = new LinkIcon();
        return handler.getAllIconJsonArray();
    }
    private string deleteLinkIcon()
    {
        LinkIcon handler = new LinkIcon();
        string ids = Request.Form["ID[]"];

        foreach (string id in ids.Split(','))
            handler.Delete(Convert.ToInt32(id));

        return "{\"message\": \"Done\"}";
    }
    private string updateLinkIconStaus()
    {
        LinkIcon handler = new LinkIcon();
        string ids = Request.Form["IDs[]"];
        string status = Request.Form["Status"];

        foreach (string id in ids.Split(','))
            handler.UpdateLinkIconStatus(Convert.ToInt32(id), status);

        return "{\"message\": \"Done\"}";
    }

    private string getSystemParaBuildinCode()
    {
        return "";
    }
}