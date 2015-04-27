using System;
using System.Collections.Generic;

using System.Web;

/// <summary>
/// Summary description for EventInfo
/// </summary>
public class EventInfo
{
    public int ID;
    public float VersionNo;
    public string SerialNo;
    public int Type;
    public string Name;
    public DateTime StartTime;
    public DateTime EndTime;
    public DateTime Deadline;
    public string Location;
    public string ContactPerson;
    public string PhoneNumber;
    public string Department;
    public string EventDetails;
    public string Status;
    public DateTime CreateDate;
    public int CreateBy;
    public DateTime UpdateDate;
    public int UpdateBy;
    public NewIconInfo NewIconInfo;

    public bool IsPublicHoliday()
    {
        DatabaseAccess dbAccess = new DatabaseAccess(); 
        System.Data.DataTable dt;
        string sql = "select * from [SystemParaBuildin] where ID = " + this.Type;

        dbAccess.open();
        try
        {
            dt = dbAccess.select(sql);

            if (dt.Rows.Count == 1
                && dt.Rows[0]["ProgramCode"].ToString() == GlobalSetting.SystemBuildinCode.PublicHoliday)
            {
                return true;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            dbAccess.close();
        }

        return false;
    }
    public ImageInfo getImage()
    {
        DatabaseAccess dbAccess = new DatabaseAccess(); 
        ImageInfo image = null;
        System.Data.DataTable dt;
        string sql = "select ID, FileName,content, MIME from [Image] where Category = '"
            + GlobalSetting.ArticleCategory.Event
            + "' and ParentID = " + ID.ToString();
        dbAccess.open();
        try
        {
            dt = dbAccess.select(sql);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            dbAccess.close();
        }

        foreach (System.Data.DataRow row in dt.Rows)
        {
            image = new ImageInfo();
            image.Content = (byte[])row["content"];
            image.ID = Convert.ToInt32(row["ID"]);
            image.FileName = row["FileName"].ToString();
            image.MIME = row["MIME"].ToString();
            break;
        }

        return image;
    }
    public Jayrock.Json.JsonArray getImageJsonArray()
    {
        DatabaseAccess dbAccess = new DatabaseAccess();

        Jayrock.Json.JsonArray images = new Jayrock.Json.JsonArray();
        Jayrock.Json.JsonObject image;

        System.Data.DataTable dt;
        string sql = "select ID, FileName, MIME from [Image] where Category = '"
            + GlobalSetting.ArticleCategory.Event
            + "' and ParentID = " + ID.ToString();
        dbAccess.open();
        try
        {
            dt = dbAccess.select(sql);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            dbAccess.close();
        } 
         
        foreach (System.Data.DataRow row in dt.Rows)
        {
            image = new Jayrock.Json.JsonObject();
            foreach (System.Data.DataColumn col in dt.Columns)
            {
                image.Accumulate(col.ColumnName.ToLower(), row[col.ColumnName]);
            }
            images.Add(image);
        }

        return images;
    }
}