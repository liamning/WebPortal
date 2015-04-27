using System;
using System.Collections.Generic;

using System.Web;

/// <summary>
/// Summary description for NewsInfo
/// </summary>
public class NewsInfo
{
	public NewsInfo()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public int ID;
    public float VersionNo;
    public string SerialNo;
    public int Type;
    public string Title;
    public string Headline;
    public string Summary;
    public string Content;
    public DateTime EffectiveDate;
    public string Status;
    public DateTime CreateDate;
    public int CreateBy;
    public DateTime UpdateDate;
    public int UpdateBy;

    public NewIconInfo NewIconInfo;

    public List<ImageInfo> getImageList()
    {
        DatabaseAccess dbAccess = new DatabaseAccess();
        List<ImageInfo> images = new List<ImageInfo>();
        ImageInfo image;
        System.Data.DataTable dt;
        string sql = "select ID, FileName, Description,content, MIME from [Image] where Category='"
            + GlobalSetting.ArticleCategory.News
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
            image.Description = row["Description"].ToString();
            image.MIME = row["MIME"].ToString();
            images.Add(image);
        }

        return images;
    }

    public Jayrock.Json.JsonArray getImageJsonArray()
    {
        DatabaseAccess dbAccess = new DatabaseAccess();
        Jayrock.Json.JsonArray images = new Jayrock.Json.JsonArray();
        Jayrock.Json.JsonObject image;
        System.Data.DataTable dt;
        string sql = "select ID, FileName, Description, MIME from [Image] where Category='"
            + GlobalSetting.ArticleCategory.News
            +"' and ParentID = " + ID.ToString();
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
            foreach (System.Data.DataColumn col  in dt.Columns)
            {
                image.Accumulate(col.ColumnName.ToLower(), row[col.ColumnName]);
            }
            images.Add(image);
        }

        return images;
    }

    public Jayrock.Json.JsonArray getFileJsonArray()
    {
        DatabaseAccess dbAccess = new DatabaseAccess();
        Jayrock.Json.JsonArray images = new Jayrock.Json.JsonArray();
        Jayrock.Json.JsonObject image;
        System.Data.DataTable dt;
        string sql = string.Format(" select ID, Name FileName, Description from [File] where parentid = {0}  order by ID ", ID.ToString());
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

    public List<FileInfo> getFileList()
    {

        DatabaseAccess dbAccess = new DatabaseAccess();
        List<FileInfo> files = new List<FileInfo>();
        FileInfo file;
        System.Data.DataTable dt;
        string sql = string.Format(" select ID, Name FileName, Description, content from [File] where ParentID = {0} and Type = {1}  order by ID", 
                    ID.ToString(), 
                    File.ArticleFileType.News);
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
            file = new FileInfo();
            file.Content = (byte[])row["content"];
            file.ID = Convert.ToInt32(row["ID"]);
            file.Name = row["FileName"].ToString();
            file.Description = row["Description"].ToString();
            files.Add(file);
        }


        return files;
    }
	
}