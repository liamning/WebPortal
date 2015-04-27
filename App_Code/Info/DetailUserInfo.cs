using System;
using System.Collections.Generic;

using System.Web;

/// <summary>
/// Summary description for UserInfo
/// </summary>
public class DetailUserInfo
{
    public int ID;
    public string Name;
    public string Password;
    public Int16 Age;
    public Int16 Sex;
    public string Post;
    public string Department;
    public string Status;
    public string StatusUpdateBy;
    public DateTime StatusUpdateDate;
    public DateTime RegisterDate;
    public string UserGroup;


    //static string for user status

    public class UserStatus
    {
        public static string Pending = "Pending";
        public static string Approved = "Approved";
        public static string Suspended = "Suspended";
        public static string Rejected = "Rejected";
    }

}