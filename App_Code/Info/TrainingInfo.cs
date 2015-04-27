using System;
using System.Collections.Generic;

using System.Web;

/// <summary>
/// Summary description for EventInfo
/// </summary>
public class TrainingInfo
{
    public int ID;
    public float VersionNo;
    public string SerialNo;
    public int Type;
    public string Name;
    public bool OptionalAttendance;
    public int MaxAttendance;
    public DateTime Deadline;
    public string Location;
    public string ContactPerson;
    public string PhoneNumber;
    public string Department;
    public string Email;
    public string Details;
    public string FormPath;
    public string Status;
    public DateTime CreateDate;
    public int CreateBy;
    public DateTime UpdateDate;
    public int UpdateBy;

    public NewIconInfo NewIconInfo;
    public List<TrainingScheInfo> Schedule;
    public List<TrainingFormInfo> FormList;


}

public class TrainingScheInfo
{
    public int ID;
    public DateTime StartTime;
    public DateTime EndTime;
}
public class TrainingFormInfo
{
    public int ID;
    public string Status;
    public int SubSequence;
    public int FormType;
    public string FormPath;
    public string Description;
    public FileInfo FileInfo;
}