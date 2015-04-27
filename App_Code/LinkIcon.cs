using System;
using System.Collections.Generic;
using System.Web;
using Jayrock.Json;

/// <summary>
/// Summary description for LinkIcon
/// </summary>
public class LinkIcon
{
    private DatabaseAccess dbAccess = new DatabaseAccess();
	public LinkIcon()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public void Create(LinkIconInfo info, ImageInfo imageInfo)
    {
        
        this.dbAccess.open();
        this.dbAccess.BeginTransaction();
        try
        {
            if(info.Link.ToUpper().StartsWith("WWW."))
            {
                info.Link = "http://" + info.Link;
            }
            //insert the linkIcon
            string sql = "INSERT INTO [LinkIcon] "
                   + "([IconName] "
                   + ",[SequenceNo] "
                   + ",[Link] "
                   + ",[Status] "
                   + ",[CreateBy] "
                   + ",[CreateDate]) "
                   + "VALUES "
                   + "(@IconName "
                   + ",@SequenceNo "
                   + ",@Link "
                   + ",@Status "
                   + ",@CreateBy "
                   + ",@CreateDate "
                   + ");select SCOPE_IDENTITY();"; 
            Dictionary<string, object> dict = new Dictionary<string, object>(); 
            dict.Add("@IconName", info.IconName);
            dict.Add("@SequenceNo", info.SequenceNo);
            dict.Add("@Link", info.Link);
            dict.Add("@Status", GlobalSetting.ArticleStatus.Unpublished);
            dict.Add("@CreateBy", info.CreateBy);
            dict.Add("@CreateDate", info.CreateDate);

             string lastID = this.dbAccess.select(sql, dict).Rows[0][0].ToString();


            //insert the icon to Image table
             string sql_Image = " INSERT INTO [Image] ([Category] ,[ParentID] ,[FileName],[Extension], [MIME],[Type],[ContentLength],[Content]) "
                         + " VALUES (@Category, @ParentID, @FileName, @Extension,@MIME, @Type ,@ContentLength,@Content);select SCOPE_IDENTITY();";
            dict.Clear();
            dict.Add("@Category", GlobalSetting.ArticleCategory.Icon);
            dict.Add("@ParentID", lastID);
            dict.Add("@FileName", imageInfo.FileName);
            dict.Add("@Extension", imageInfo.Extension);
            dict.Add("@MIME", imageInfo.MIME);
            dict.Add("@Type", imageInfo.Type);
            dict.Add("@ContentLength", imageInfo.ContentLength);
            dict.Add("@Content", imageInfo.Content);
            string lastImageID = this.dbAccess.select(sql_Image, dict).Rows[0][0].ToString();

            //udpate ImageID to LinkIcon Record
            string sql_update = "Update [LinkIcon] set [ImageID] = @ImageID where ID = @ID;";
            dict.Clear();
            dict.Add("@ImageID", lastImageID);
            dict.Add("@ID", lastID);
            this.dbAccess.select(sql_update, dict);

            this.dbAccess.Commit();
             
        }
        catch
        {
            this.dbAccess.rollback();
            throw;
        }
        finally
        {
            this.dbAccess.close();
        }
         
    }

    public void Update(LinkIconInfo info, ImageInfo imageInfo)
    {

        this.dbAccess.open();
        this.dbAccess.BeginTransaction();
        try
        {
            if (info.Link.ToUpper().StartsWith("WWW."))
            {
                info.Link = "http://" + info.Link;
            }
            //insert the linkIcon
            string sql = "UPDATE [LinkIcon] "
                        + "SET [IconName] = @IconName "
                        + ",[SequenceNo] = @SequenceNo "
                        + ",[Link] = @Link "
                        + ",[UpdateBy] = @UpdateBy "
                        + ",[UpdateDate] = @UpdateDate "
                        + "WHERE ID=@ID ";
 
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("@IconName", info.IconName);
            dict.Add("@SequenceNo", info.SequenceNo);
            dict.Add("@Link", info.Link);
            dict.Add("@UpdateBy", info.UpdateBy);
            dict.Add("@UpdateDate", info.UpdateDate);
            dict.Add("@ID", info.ID); 
            this.dbAccess.update(sql, dict);


            if (imageInfo != null)
            { 
                //insert the icon to Image table
                string sql_Image = "UPDATE [Image] "
                                + "SET [Category] = @Category "
                                + ",[FileName] = @FileName "
                                + ",[Extension] = @Extension "
                                + ",[MIME] = @MIME "
                                + ",[Type] = @Type "
                                + ",[ContentLength] = @ContentLength "
                                + ",[Content] = @Content "
                                + "WHERE ID = @ID ";
                dict.Clear();
                dict.Add("@Category", GlobalSetting.ArticleCategory.Icon);
                dict.Add("@FileName", imageInfo.FileName);
                dict.Add("@Extension", imageInfo.Extension);
                dict.Add("@MIME", imageInfo.MIME);
                dict.Add("@Type", imageInfo.Type);
                dict.Add("@ContentLength", imageInfo.ContentLength);
                dict.Add("@Content", imageInfo.Content);
                dict.Add("@ID", info.ImageID);
                this.dbAccess.update(sql_Image, dict);
            }


            this.dbAccess.Commit();

        }
        catch
        {
            this.dbAccess.rollback();
            throw;
        }
        finally
        {
            this.dbAccess.close();
        }

    }

    public string getAllIconJsonArray()
    {
        JsonArray ja = new JsonArray();
        JsonObject jo = null;


        string sql = "select * from LinkIcon  order by SequenceNo;";
        this.dbAccess.open();

        try
        {
            System.Data.DataTable dt = this.dbAccess.select(sql);
            foreach (System.Data.DataRow row in dt.Rows)
            {
                jo = new JsonObject();
                foreach (System.Data.DataColumn col in dt.Columns)
                {
                    jo.Accumulate(col.ColumnName, row[col.ColumnName]);
                }

                ja.Add(jo);
            }
        }
        catch
        {
            throw;
        }
        finally
        {
            this.dbAccess.close();
        }


        return ja.ToString();
    }

    public List<LinkIconInfo> getAllActiveIconList()
    {
        List<LinkIconInfo> list = new List<LinkIconInfo>();
        LinkIconInfo info = null;


        string sql = string.Format("select * from LinkIcon where status = '{0}' order by SequenceNo;", GlobalSetting.ArticleStatus.Published);
        this.dbAccess.open();

        try
        {
            System.Data.DataTable dt = this.dbAccess.select(sql);
            foreach (System.Data.DataRow row in dt.Rows)
            {
                info = new LinkIconInfo();
                info.ID = Convert.ToInt32(row["ID"]);
                info.ImageID = Convert.ToInt32(row["ImageID"]);
                info.IconName = row["IconName"].ToString();
                info.Link = row["Link"].ToString();
                info.Status = row["Status"].ToString(); 

                list.Add(info);
            }
        }
        catch
        {
            throw;
        }
        finally
        {
            this.dbAccess.close();
        }


        return list;
    }

    public LinkIconInfo getLinkIconByID(int id)
    { 
        LinkIconInfo info = null;


        string sql = string.Format("select * from LinkIcon where ID={0};", id);
        this.dbAccess.open();

        try
        {
            System.Data.DataTable dt = this.dbAccess.select(sql);
            foreach (System.Data.DataRow row in dt.Rows)
            {
                info = new LinkIconInfo();
                info.ID = Convert.ToInt32(row["ID"]);
                info.ImageID = Convert.ToInt32(row["ImageID"]);
                info.IconName = row["IconName"].ToString();
                info.Link = row["Link"].ToString();
                info.Link = row["Link"].ToString();
                info.Status = row["Status"].ToString();
                break;
            }
        }
        catch
        {
            throw;
        }
        finally
        {
            this.dbAccess.close();
        }


        return info;
    }

    public void Delete(int id)
    {
        LinkIconInfo info = this.getLinkIconByID(id);
        string sql = string.Format("delete from LinkIcon where ID = {0};", id);
        string sql_image = string.Format("delete from Image where ID = {0};", info.ImageID);
        this.dbAccess.open();

        this.dbAccess.BeginTransaction();
        try
        {
            this.dbAccess.update(sql);
            this.dbAccess.update(sql_image);
            this.dbAccess.Commit();
        }
        catch
        {
            this.dbAccess.rollback();
            throw;
        }
        finally
        {
            this.dbAccess.close();
        }
    }


    public void UpdateLinkIconStatus(int id, string status)
    {
        LinkIconInfo info = this.getLinkIconByID(id);
        string sql = string.Format("update LinkIcon set Status = '{0}' where ID = {1};", status , id); 
        this.dbAccess.open();
         
        try
        {
            this.dbAccess.update(sql);  
        }
        catch
        { 
            throw;
        }
        finally
        {
            this.dbAccess.close();
        }
    }
}