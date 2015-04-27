using System;
using System.Collections.Generic;

using System.Web;
using Jayrock.Json;

/// <summary>
/// Summary description for News
/// </summary>
public class News
{
    //object for DB access
    private DatabaseAccess dbAccess = new DatabaseAccess();

    //insert News to database
    public void createNews(NewsInfo articleInfo, List<ImageInfo> imageList, List<FileInfo> fileList)
    {
        string sql = " INSERT INTO [News] ([SerialNo] ,[Type] ,[Title] ,[Headline] ,[Summary] ,[Content] ,"
                   + " [EffectiveDate] ,[CreateDate] ,[CreateBy],[Status]) "
                   + " VALUES (@SerialNo, @Type, @Title, @Headline, @Summary, @Content,"
                   + " @EffectiveDate,getDate(),@CreateBy,@Status);select SCOPE_IDENTITY();";

        string sql_Image = " INSERT INTO [Image] ([Category] ,[ParentID] ,[FileName],[Description],[Extension],[MIME], [Type],[ContentLength],[Content]) "
                         + " VALUES (@Category, @ParentID, @FileName, @Description, @Extension, @MIME, @Type ,@ContentLength,@Content)";
         
        string sql_File = "INSERT INTO [File] "
                   + "([FileID], [ParentID], [Type] ,[Name] ,[OriginalName] ,[Description] ,[Content] ,[UploadDate] ,[UploadUser],[Status]) VALUES "
                   + "(newid(), @ParentID, @Type, @Name, @OriginalName, @Description, @Content, @UploadDate, @UploadUser, @Status);select SCOPE_IDENTITY()";


        Dictionary<string, object> dict;
        this.dbAccess.open();
        dbAccess.BeginTransaction();
        try
        {
            //insert the News and return the row ID
            dict = new Dictionary<string, object>();
            dict.Add("@SerialNo", articleInfo.SerialNo);
            dict.Add("@Type", articleInfo.Type);
            dict.Add("@Title", articleInfo.Title);
            dict.Add("@Headline", articleInfo.Headline);
            dict.Add("@Summary", articleInfo.Summary);
            dict.Add("@Content", articleInfo.Content);
            dict.Add("@EffectiveDate", articleInfo.EffectiveDate);
            dict.Add("@CreateBy", articleInfo.CreateBy);
            dict.Add("@Status", articleInfo.Status);
            string lastID = this.dbAccess.select(sql, dict).Rows[0][0].ToString();

            //insert the images
            for (int i = 0; i < imageList.Count; i++)
            {
                dict = new Dictionary<string, object>();
                dict.Add("@Category", GlobalSetting.ArticleCategory.News);
                dict.Add("@ParentID", lastID);
                dict.Add("@FileName", imageList[i].FileName);
                dict.Add("@Description", imageList[i].Description);
                dict.Add("@Extension", imageList[i].Extension);
                dict.Add("@MIME", imageList[i].MIME);
                dict.Add("@Type", imageList[i].Type);
                dict.Add("@ContentLength", imageList[i].ContentLength);
                dict.Add("@Content", imageList[i].Content);
                this.dbAccess.update(sql_Image, dict);
            }


            //insert the images
            FileInfo fileInfo;
            for (int i = 0; i < fileList.Count; i++)
            {
                fileInfo = fileList[i];
                //insert the News and return the row ID
                dict = new Dictionary<string, object>();
                dict.Add("@ParentID", lastID);
                dict.Add("@Type", File.ArticleFileType.News);
                dict.Add("@Name", fileInfo.Name);
                dict.Add("@OriginalName", fileInfo.OriginalName);
                dict.Add("@Description", fileInfo.Description);
                dict.Add("@Content", fileInfo.Content);
                dict.Add("@UploadUser", fileInfo.UploadUser);
                dict.Add("@UploadDate", fileInfo.UploadDate);
                dict.Add("@Status", GlobalSetting.ArticleStatus.Attached);

                this.dbAccess.update(sql_File, dict);
            }

            #region new icon

            //insert the information of "New" icon 
            string sql_icon = "INSERT INTO [NewIconControl] "
                       + "([Category], [ParentID], [ExpiryDate]) VALUES "
                       + "(@Category, @ParentID, @ExpiryDate)";
            if (articleInfo.NewIconInfo != null)
            {
                dict = new Dictionary<string, object>();
                dict.Add("@Category", articleInfo.NewIconInfo.Category);
                dict.Add("@ParentID", lastID);
                dict.Add("@ExpiryDate", articleInfo.NewIconInfo.ExpiryDate);
                this.dbAccess.update(sql_icon, dict);
            }

            #endregion


            //commit the transation
            this.dbAccess.Commit();
        }
        catch (Exception ex)
        {
            //roll back if any exception
            this.dbAccess.rollback();
            throw ex;
        }
        finally
        {
            this.dbAccess.close();
        }
    }

    public void UpdateNews(NewsInfo articleInfo, List<ImageInfo> imageListToAdd,
        Dictionary<int, ImageInfo> imageDictToUpdate, Dictionary<int, ImageInfo> imageDescDictToUpdate, string imageListToDelete,
        List<FileInfo> fileListToAdd, Dictionary<int, FileInfo> fileDictToUpdate, string fileListToDelete)
    {
        string sql = " UPDATE [News] "
                   + " SET [VersionNo] = @VersionNo,[SerialNo] = @SerialNo, [Type] = @Type, [Title] = @Title ,[Headline] = @Headline ,[EffectiveDate] = @EffectiveDate ,"
                   + " [Summary] = @Summary ,[Content] = @Content ,[UpdateDate] = getDate(),[UpdateBy] = @UpdateBy "
                   + " WHERE ID=@ID ";

        Dictionary<string, object> dict;
        this.dbAccess.open();
        dbAccess.BeginTransaction();
        try
        {
            //insert the News and return the row ID
            dict = new Dictionary<string, object>();
            dict.Add("@VersionNo", articleInfo.VersionNo);
            dict.Add("@SerialNo", articleInfo.SerialNo);
            dict.Add("@Type", articleInfo.Type);
            dict.Add("@Title", articleInfo.Title);
            dict.Add("@Headline", articleInfo.Headline);
            dict.Add("@Summary", articleInfo.Summary);
            dict.Add("@EffectiveDate", articleInfo.EffectiveDate);
            dict.Add("@Content", articleInfo.Content);
            dict.Add("@UpdateBy", articleInfo.UpdateBy);
            dict.Add("@ID", articleInfo.ID);

            //update news
            this.dbAccess.update(sql, dict);
            

            //delete images
            string sql_Image = string.Format(" Delete from [Image] WHERE ID in ({0})", imageListToDelete);
            if (!String.IsNullOrEmpty(imageListToDelete))
            {
                this.dbAccess.update(sql_Image);
            }

            //insert the new images  
            sql_Image = " INSERT INTO [Image] ([Category],[ParentID] ,[FileName],[Description],[Extension], [MIME], [Type],[ContentLength],[Content]) "
                             + " VALUES (@Category, @ParentID, @FileName, @Description, @Extension, @MIME, @Type ,@ContentLength,@Content)";

            for (int i = 0; i < imageListToAdd.Count; i++)
            {
                dict = new Dictionary<string, object>();
                dict.Add("@Category", GlobalSetting.ArticleCategory.News);
                dict.Add("@ParentID", articleInfo.ID);
                dict.Add("@FileName", imageListToAdd[i].FileName);
                dict.Add("@Description", imageListToAdd[i].Description);
                dict.Add("@Extension", imageListToAdd[i].Extension);
                dict.Add("@MIME", imageListToAdd[i].MIME);
                dict.Add("@Type", imageListToAdd[i].Type);
                dict.Add("@ContentLength", imageListToAdd[i].ContentLength);
                dict.Add("@Content", imageListToAdd[i].Content);
                this.dbAccess.update(sql_Image, dict);
            }

            //update the old images  
            sql_Image = " Update [Image] set [FileName] = @FileName,[Description] = @Description,[Extension] = @Extension, [MIME] = @MIME, "
                      + " [Type] = @Type,[ContentLength] = @ContentLength,[Content] = @Content where ID = @ID";

            foreach (int key in imageDictToUpdate.Keys)
            {
                dict = new Dictionary<string, object>();
                dict.Add("@FileName", imageDictToUpdate[key].FileName);
                dict.Add("@Description", imageDictToUpdate[key].Description);
                dict.Add("@Extension", imageDictToUpdate[key].Extension);
                dict.Add("@MIME", imageDictToUpdate[key].MIME);
                dict.Add("@Type", imageDictToUpdate[key].Type);
                dict.Add("@ContentLength", imageDictToUpdate[key].ContentLength);
                dict.Add("@Content", imageDictToUpdate[key].Content);
                dict.Add("@ID", imageDictToUpdate[key].ID);
                this.dbAccess.update(sql_Image, dict);
            }

            //update the old images description  
            sql_Image = " Update [Image] set  [Description] = @Description where ID = @ID";

            foreach (int key in imageDescDictToUpdate.Keys)
            {
                dict = new Dictionary<string, object>();
                dict.Add("@Description", imageDescDictToUpdate[key].Description);
                dict.Add("@ID", imageDescDictToUpdate[key].ID);
                this.dbAccess.update(sql_Image, dict);
            }
             
            #region



            //delete file
            string sql_File = string.Format(" Delete from [File] WHERE ID in ({0})", fileListToDelete);
            if (!String.IsNullOrEmpty(fileListToDelete))
            {
                this.dbAccess.update(sql_File);
            }

            //insert the new file  
            sql_File = "INSERT INTO [File] "
                   + "([FileID], [ParentID], [Type] ,[Name] ,[OriginalName] ,[Description] ,[Content] ,[UploadDate] ,[UploadUser],[Status]) VALUES "
                   + "(newid(), @ParentID, @Type, @Name, @OriginalName, @Description, @Content, @UploadDate, @UploadUser, @Status);";

            for (int i = 0; i < fileListToAdd.Count; i++)
            {
                dict = new Dictionary<string, object>();
                dict.Add("@ParentID", articleInfo.ID);
                dict.Add("@Type", File.ArticleFileType.News);
                dict.Add("@Name", fileListToAdd[i].Name);
                dict.Add("@OriginalName", fileListToAdd[i].OriginalName);
                dict.Add("@Description", fileListToAdd[i].Description);
                dict.Add("@Content", fileListToAdd[i].Content);
                dict.Add("@UploadDate", fileListToAdd[i].UploadDate);
                dict.Add("@UploadUser", fileListToAdd[i].UploadUser);
                dict.Add("@Status", GlobalSetting.ArticleStatus.Attached);

                this.dbAccess.update(sql_File, dict);
            }

            //update the old file  
            sql_File = " Update [File] set [OriginalName] = @OriginalName, [Name] = @Name, [Description] = @Description, "
                     + " [Content] = @Content where ID = @ID ";

            foreach (int key in fileDictToUpdate.Keys)
            {
                dict = new Dictionary<string, object>();
                dict.Add("@OriginalName", fileDictToUpdate[key].OriginalName);
                dict.Add("@Name", fileDictToUpdate[key].Name);
                dict.Add("@Description", fileDictToUpdate[key].Description);   
                dict.Add("@Content", fileDictToUpdate[key].Content);
                dict.Add("@ID", fileDictToUpdate[key].ID);
                this.dbAccess.update(sql_File, dict);
            }
             
            #endregion
             
            #region New Icon
            //delete the information of "New" icon before insert
            string sql_icon_delete = "delete from [NewIconControl] where Category=@Category and ParentID = @ParentID"; 
            dict = new Dictionary<string, object>();
            dict.Add("@Category", GlobalSetting.ArticleCategory.News);
            dict.Add("@ParentID", articleInfo.ID);
            this.dbAccess.update(sql_icon_delete, dict);

            //insert the information of "New" icon 
            string sql_icon = "INSERT INTO [NewIconControl] "
                       + "([Category], [ParentID], [ExpiryDate]) VALUES "
                       + "(@Category, @ParentID, @ExpiryDate)";
            if (articleInfo.NewIconInfo != null)
            {
                dict = new Dictionary<string, object>();
                dict.Add("@Category", articleInfo.NewIconInfo.Category); 
                dict.Add("@ParentID", articleInfo.ID);
                dict.Add("@ExpiryDate", articleInfo.NewIconInfo.ExpiryDate); 
                this.dbAccess.update(sql_icon, dict);
            }
            #endregion
             
            //commit the transation
            this.dbAccess.Commit();
        }
        catch (Exception ex)
        {
            //roll back if any exception
            this.dbAccess.rollback();
            throw ex;
        }
        finally
        {
            this.dbAccess.close();
        }
    }

    public JsonArray getAllArticle()
    { 
        JsonArray ja = new JsonArray();
        JsonObject jo;
        String tmpStr;

        dbAccess.open();
        try
        {
            string sql = " select SerialNo, [News].ID, Title,Status,left(CONVERT(VARCHAR, [EffectiveDate], " + GlobalSetting.DatabaseDateTimeFormat + "),10) EffectiveDate, SystemPara.Description type, 'News' Category from [News]  "
                       + " left outer join SystemPara on category = 'NewsType' and [News].type = SystemPara.ID"
                       + " where status <> 'Trashed' order by status, [News].EffectiveDate desc, SerialNo desc ";
            System.Data.DataTable dt = dbAccess.select(sql);

            foreach (System.Data.DataRow dr in dt.Rows)
            {
                jo = new JsonObject();
                foreach (System.Data.DataColumn col in dt.Columns)
                {
                    tmpStr = dr[col.ColumnName].ToString();
                    jo.Accumulate(col.ColumnName.ToLower(), tmpStr);
                }
                ja.Add(jo);
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

        return ja;
    }

    public List<NewsInfo> getLatestNews(int total)
    {
        List<NewsInfo> list = new List<NewsInfo>();
        NewsInfo articleInfo; 

        dbAccess.open();
        try
        {
            string sql;

            if (total != 0)
            {
                sql = string.Format("select top {0} * from [News] where status = '{1}' order by [EffectiveDate] desc",
                                        total,
                                        GlobalSetting.ArticleStatus.Published);
            }
            else
            {
                sql = string.Format("select * from [News] where status = '{0}' order by [EffectiveDate] desc",
                                        GlobalSetting.ArticleStatus.Published);
            }

            System.Data.DataTable dt = dbAccess.select(sql);
            System.Data.DataTable dt2;

            foreach (System.Data.DataRow row in dt.Rows)
            {
                articleInfo = new NewsInfo(); 

                articleInfo.ID = Convert.ToInt32(row["ID"]);
                articleInfo.Title = row["Title"].ToString();
                articleInfo.Headline = row["Headline"].ToString();
                articleInfo.Summary = row["Summary"].ToString();
                articleInfo.Content = row["Content"].ToString();
                articleInfo.Status = row["Status"].ToString(); 
                articleInfo.EffectiveDate = Convert.ToDateTime(row["EffectiveDate"]);

                sql = string.Format("select ExpiryDate from [NewIconControl] where ParentID = {0} and Category = '{1}' ",
                articleInfo.ID, GlobalSetting.ArticleCategory.News);
                dt2 = dbAccess.select(sql);
                if (dt2.Rows.Count == 1)
                {
                    articleInfo.NewIconInfo = new NewIconInfo();
                    articleInfo.NewIconInfo.ExpiryDate = Convert.ToDateTime(dt2.Rows[0]["ExpiryDate"]); 
                } 

                list.Add(articleInfo);
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

        return list;
    }

    public NewsInfo getArticle(int ID)
    {
        NewsInfo articleInfo = new NewsInfo();
        ImageInfo image = new ImageInfo();

        dbAccess.open();
        try
        {

            string sql = "select * from [News] where ID = " + ID.ToString();
            System.Data.DataTable dt = dbAccess.select(sql);
            articleInfo.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
            articleInfo.VersionNo = Convert.ToSingle(dt.Rows[0]["VersionNo"]);
            articleInfo.Title = dt.Rows[0]["Title"].ToString();
            articleInfo.SerialNo = dt.Rows[0]["SerialNo"].ToString();
            articleInfo.Headline = dt.Rows[0]["Headline"].ToString();
            articleInfo.Type = Convert.ToInt32(dt.Rows[0]["Type"].ToString());
            articleInfo.Summary = dt.Rows[0]["Summary"].ToString();
            articleInfo.Content = dt.Rows[0]["Content"].ToString();
            articleInfo.Status = dt.Rows[0]["Status"].ToString(); 
            articleInfo.EffectiveDate = Convert.ToDateTime(dt.Rows[0]["EffectiveDate"]);

            sql = string.Format("select ExpiryDate from [NewIconControl] where ParentID = {0} and Category = '{1}' ",
                articleInfo.ID, GlobalSetting.ArticleCategory.News);
            dt = dbAccess.select(sql);
            if(dt.Rows.Count == 1)
            {
                articleInfo.NewIconInfo = new NewIconInfo();
                articleInfo.NewIconInfo.ExpiryDate = Convert.ToDateTime(dt.Rows[0]["ExpiryDate"]); 
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
         
        return articleInfo;
    }

    public static byte[] getImageByID(int ID, ref string MIME)
    {
        DatabaseAccess dbAccess = new DatabaseAccess();

        byte[] imageBytes;
        dbAccess.open();
        try
        {
            string sql = "select MIME,content  from [Image] where ID = " + ID.ToString();
            System.Data.DataTable dt = dbAccess.select(sql);
            imageBytes = (byte[])dt.Rows[0]["content"];
            MIME = dt.Rows[0]["MIME"].ToString(); 
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            dbAccess.close();
        }

        return imageBytes;
    }

    public void updateStatus(string articleIDs,  string newStatus)
    {
        //create database access object 
        String sql = string.Format("UPDATE [News] SET [Status] = '{0}' where ID in ({1}) ",
                                    newStatus,
                                    articleIDs);
        dbAccess.open();
        try
        {
            dbAccess.update(sql);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            dbAccess.close();
        }
    }

}