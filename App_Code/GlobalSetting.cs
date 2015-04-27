using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for GlobalSetting
/// </summary>
public class GlobalSetting
{
    public static readonly int MaxImageLength = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["maxImageLength"]);
    public static readonly int MaxImageHeight = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["maxImageHeight"]);
    public static readonly int MaxImageWidth = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["maxImageWidth"]);
    public static readonly string LoginMethod = System.Configuration.ConfigurationManager.AppSettings["LoginMethod"];

    public static readonly Dictionary<string, string> DisabledFunctionDict = AccessControl.getDisabledFunctionDict();
    public static readonly Dictionary<string, string> AdminFunctionDict = AccessControl.getAdminFunctionDict();
    public static readonly Dictionary<string, string> SuperFunctionDict = AccessControl.getSuperFunctionDict();

    public static readonly string[] ValidDomains = System.Configuration.ConfigurationManager.AppSettings["DomainList"].Split(',');


    //the value in the class must be aligned with the data table "SystemparaBuildin"
    public class SystemBuildinCode
    {
        public const string QuickLinks = "QuickLinks";
        public const string Newsletter = "Newsletter";
        public const string PublicHoliday = "PubHoliday";
    }

    public class SystemRoles
    {
        public const string Normal = "Normal";
        public const string Admin = "Admin";
        public const string Super = "Super";
        public static string getDesc(string roleID)
        {
            if (roleID == Super)
            {
                return "Super User";
            }
            else if (roleID == Admin)
            {
                return "Administrator";
            }
            else 
            {
                return "Normal User";
            }


        }
    }
    public class ArticleStatus
    {
        public const string Published = "Published";
        public const string Unpublished = "Unpublished";
        public const string Trashed = "Trashed";
        public const string Attached = "Attached";

    }
    public class ArticleCategory
    {
        public const string News = "News";
        public const string Training = "Training";
        public const string Event = "Event";
        public const string Career = "Career";
        public const string Icon = "Icon";
    }

    public static string DateTimeFormat = "dd/MM/yyyy";
    public static string DatabaseDateTimeFormat = "103";

    public class FieldLength
    {
        public static string SerialNo1 = "10";
        public static string SerialNo2 = "2";
        public static string Location = "250";
        public static string ContactPerson = "50";
        public static string Department = "30";
        public static string PhoneNumber = "25";
        public static string EmailAddress = "50";
        public static string Division = "30";

        public class News
        {
            public static string Title = "100";
            public static string Headline = "524288";
            public static string Summary = "524288";
            public static string MainBody = "524288";
        }
        public class Training
        {
            public static string TrainingCourse = "250";
            public static string MaximumAttendance = "2";
            public static string Details = "524288";
            public static string FormPath = "300";
        }
        public class Event
        {
            public static string EventName = "250";
            public static string Details = "524288"; 
        }
        public class Career
        {
            public static string CareerLevel = "20";
            public static string Experience = "5";
            public static string JobFunction = "100"; //Position
            public static string EmploymentType = "50";
            public static string Details = "524288";
            public static string Disclaimer = "524288";
        }
        public class FileUploadManager
        {
            public static string Directory = "100";
            public static string FileName = "100";
            public static string Description = "100";
        }
        public class SystemLink
        {
            public static string Name = "30";
            public static string Link = "150";
        }
        public class Suggestion
        {
            public static string Content = "524288";
        }
    }

    public class AlertMessage
    {
        public static string AlertWhenUpdateTrainingSchedule = "The training attendance records will be clear if the schedule is updated.";
        public static string AlertWhenUpdateEventSchedule = "The event attendance records will be clear if the schedule is updated.";
    }
}