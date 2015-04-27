using System;
using System.Collections.Generic;

using System.Web;
using Jayrock.Json;

/// <summary>
/// Summary description for Training
/// </summary>
public class Training
{
    //object for DB access
    private DatabaseAccess dbAccess = new DatabaseAccess();

    public void save(TrainingInfo traininginfo)
    {
        string sql = " INSERT INTO [Training] "
                   + " ([SerialNo], [Type], [Name], [OptionalAttendance], [MaxAttendance], [Deadline], [Location] ,[ContactPerson] ,"
                   + " [PhoneNumber] ,[Department] ,[Email] ,[Details], [Status] ,[CreateDate] ,[CreateBy]) "
                   + " VALUES "
                   + " (@SerialNo, @Type, @Name, @OptionalAttendance, @MaxAttendance, @Deadline, @Location,@ContactPerson,@PhoneNumber, "
                   + " @Department, @Email, @Details, @Status, getDate(), @CreateBy);select SCOPE_IDENTITY();";
        string sqlSche = "INSERT INTO [TrainingSche] "
                   + " ([TrainingID] ,[StartTime] ,[EndTime]) "
                   + " VALUES "
                   + " (@TrainingID, @StartTime,@EndTime);";

        string sqlForm = "INSERT INTO [TrainingForm] "
                   + " ([TrainingID] ,[FormType] ,[FormPath],[Description]) "
                   + " VALUES "
                   + " (@TrainingID, @FormType,@FormPath, @Description);select SCOPE_IDENTITY();";


        string sql_File = "INSERT INTO [File] "
                   + "([FileID], [ParentID], [Type] ,[Name] ,[OriginalName] ,[Description] ,[Content] ,[UploadDate] ,[UploadUser],[Status]) VALUES "
                   + "(newid(), @ParentID, @Type, @Name, @OriginalName, @Description, @Content, @UploadDate, @UploadUser, @Status);select SCOPE_IDENTITY()";
         

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("@SerialNo", traininginfo.SerialNo);
        dict.Add("@Type", traininginfo.Type);
        dict.Add("@Name", traininginfo.Name);
        dict.Add("@OptionalAttendance", traininginfo.OptionalAttendance);
        dict.Add("@MaxAttendance", traininginfo.MaxAttendance);
        dict.Add("@Deadline", traininginfo.Deadline);
        dict.Add("@Location", traininginfo.Location);
        dict.Add("@ContactPerson", traininginfo.ContactPerson);
        dict.Add("@PhoneNumber", traininginfo.PhoneNumber);
        dict.Add("@Department", traininginfo.Department);
        dict.Add("@Email", traininginfo.Email);
        dict.Add("@Details", traininginfo.Details);
        dict.Add("@Status", traininginfo.Status);
        //dict.Add("@FormPath", traininginfo.FormPath);
        dict.Add("@CreateBy", traininginfo.CreateBy);

        Dictionary<string, object> dictSche = new Dictionary<string, object>();
        Dictionary<string, object> dictForm = new Dictionary<string, object>();

        this.dbAccess.open();

        try
        {
            this.dbAccess.BeginTransaction();

            System.Data.DataTable dt = this.dbAccess.select(sql, dict);
            int trainingID = Convert.ToInt32(dt.Rows[0][0]);

            foreach (TrainingScheInfo item in traininginfo.Schedule)
            {
                dictSche.Clear();
                dictSche.Add("@TrainingID", trainingID);
                dictSche.Add("@StartTime", item.StartTime);
                dictSche.Add("@EndTime", item.EndTime);
                this.dbAccess.update(sqlSche, dictSche);
            }

            TrainingFormInfo formItem;
            FileInfo fileInfo;
            int formID = 0;
            for (int i = 0; i < traininginfo.FormList.Count; i++)
            {
                formItem = traininginfo.FormList[i];

                dictForm.Clear();
                if (formItem.FormPath.ToUpper().StartsWith("WWW."))
                {
                    formItem.FormPath = "http://" + formItem.FormPath;
                }

                dictForm.Add("@TrainingID", trainingID);
                dictForm.Add("@FormType", formItem.FormType);
                dictForm.Add("@FormPath", formItem.FormPath);
                dictForm.Add("@Description", formItem.Description);
                formID = Convert.ToInt32(this.dbAccess.select(sqlForm, dictForm).Rows[0][0]);

                if (formItem.FileInfo != null)
                {
                    fileInfo = formItem.FileInfo;
                    //insert the News and return the row ID
                    dict = new Dictionary<string, object>();
                    dict.Add("@ParentID", formID);
                    dict.Add("@Type", File.ArticleFileType.Training);
                    dict.Add("@Name", fileInfo.Name);
                    dict.Add("@OriginalName", fileInfo.OriginalName);
                    dict.Add("@Description", fileInfo.Description);
                    dict.Add("@Content", fileInfo.Content);
                    dict.Add("@UploadUser", fileInfo.UploadUser);
                    dict.Add("@UploadDate", fileInfo.UploadDate);
                    dict.Add("@Status", GlobalSetting.ArticleStatus.Attached);

                    this.dbAccess.update(sql_File, dict);
                }

            }


            #region new icon

            //insert the information of "New" icon 
            string sql_icon = "INSERT INTO [NewIconControl] "
                       + "([Category], [ParentID], [ExpiryDate]) VALUES "
                       + "(@Category, @ParentID, @ExpiryDate)";
            if (traininginfo.NewIconInfo != null)
            {
                dict = new Dictionary<string, object>();
                dict.Add("@Category", traininginfo.NewIconInfo.Category);
                dict.Add("@ParentID", trainingID);
                dict.Add("@ExpiryDate", traininginfo.NewIconInfo.ExpiryDate);
                this.dbAccess.update(sql_icon, dict);
            }

            #endregion


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

    public void update(TrainingInfo traininginfo, bool resetAttendance, string deletedFormID)
    {
        //update the training information and delete the old date schedule
        string sql = "  UPDATE [Training]"
                    + " SET [SerialNo] = @SerialNo"
                    + " ,[VersionNo] = @VersionNo"
                    + " ,[Type] = @Type"
                    + " ,[Name] = @Name"
                    + " ,[OptionalAttendance] = @OptionalAttendance"
                    + " ,[MaxAttendance] = @MaxAttendance"
                    + " ,[Deadline] = @Deadline"
                    + " ,[Location] = @Location"
                    + " ,[ContactPerson] = @ContactPerson"
                    + " ,[PhoneNumber] = @PhoneNumber"
                    + " ,[Department] = @Department"
                    + " ,[Email] = @Email"
                    + " ,[Details] = @Details  "
                    + " ,[UpdateDate] = getDate()"
                    + " ,[UpdateBy] = @UpdateBy"
                    + " WHERE ID= @ID;";

        string sqlSche1 = "INSERT INTO [TrainingSche] "
                   + " ([TrainingID] ,[StartTime] ,[EndTime]) "
                   + " VALUES "
                   + " (@TrainingID, @StartTime,@EndTime);";

        string sqlSche2 = "delete from TrainingSche where trainingID = @TrainingID;";

        string sqlResetSche = "delete "
                        + "FROM [ActivityLog] "
                        + "where ActivityID = @ActivityID";


        string sqlForm = "INSERT INTO [TrainingForm] "
                   + " ([TrainingID] ,[FormType] ,[FormPath],[Description]) "
                   + " VALUES "
                   + " (@TrainingID, @FormType,@FormPath, @Description);select SCOPE_IDENTITY();";
         
        string sqlFormUpdate = "update [TrainingForm] "
                   + " set [FormType]=@FormType ,[FormPath]=@FormPath,[Description]=@Description "
                   + " where ID=@ID"; 

        string sql_File = "INSERT INTO [File] "
                   + "([FileID], [ParentID], [Type] ,[Name] ,[OriginalName] ,[Description] ,[Content] ,[UploadDate] ,[UploadUser],[Status]) VALUES "
                   + "(newid(), @ParentID, @Type, @Name, @OriginalName, @Description, @Content, @UploadDate, @UploadUser, @Status);";
        string sql_File_delete = string.Format("delete from [File] where ParentID = @ParentID and Type={0}", File.ArticleFileType.Training);

        string sqlFormDelete = string.Format("delete from [TrainingForm] where ID in ({0})", deletedFormID);
        string sqlPhysicalFile = string.Format("delete from [File] where ParentID in ({0}) and Type={1}", deletedFormID, File.ArticleFileType.Training);

        Dictionary<string, object> dict = new Dictionary<string, object>();

        dict.Add("@SerialNo", traininginfo.SerialNo);
        dict.Add("@VersionNo", traininginfo.VersionNo);
        dict.Add("@Type", traininginfo.Type);
        dict.Add("@Name", traininginfo.Name);
        dict.Add("@OptionalAttendance", traininginfo.OptionalAttendance);
        dict.Add("@MaxAttendance", traininginfo.MaxAttendance);
        dict.Add("@Deadline", traininginfo.Deadline);
        dict.Add("@Location", traininginfo.Location);
        dict.Add("@ContactPerson", traininginfo.ContactPerson);
        dict.Add("@PhoneNumber", traininginfo.PhoneNumber);
        dict.Add("@Department", traininginfo.Department);
        dict.Add("@Email", traininginfo.Email);
        dict.Add("@Details", traininginfo.Details);
        //dict.Add("@FormPath", traininginfo.FormPath);
        dict.Add("@UpdateBy", traininginfo.UpdateBy);

        dict.Add("@ID", traininginfo.ID);


        Dictionary<string, object> dictSche = new Dictionary<string, object>();
        Dictionary<string, object> dictForm = new Dictionary<string, object>();

        this.dbAccess.open();
        try
        {
            this.dbAccess.BeginTransaction();

            this.dbAccess.update(sql, dict); 
            if (resetAttendance)
            { 
                dictSche.Clear();
                dictSche.Add("@TrainingID", traininginfo.ID);
                this.dbAccess.update(sqlSche2, dictSche);

                foreach (TrainingScheInfo item in traininginfo.Schedule)
                {
                    dictSche.Clear();
                    dictSche.Add("@TrainingID", traininginfo.ID);
                    dictSche.Add("@StartTime", item.StartTime);
                    dictSche.Add("@EndTime", item.EndTime);
                    this.dbAccess.update(sqlSche1, dictSche);

                }

                //reset attendance
                dictSche.Clear();
                dictSche.Add("@ActivityID", traininginfo.ID);
                this.dbAccess.update(sqlResetSche, dictSche);
            } 

            TrainingFormInfo formItem;
            FileInfo fileInfo;
            int formID = 0;
            for (int i = 0; i < traininginfo.FormList.Count; i++)
            {
                formItem = traininginfo.FormList[i];
                if (formItem.FormPath.ToUpper().StartsWith("WWW."))
                {
                    formItem.FormPath = "http://" + formItem.FormPath;
                }

                //update form
                if (formItem.ID != 0 && formItem.Status == "updated")
                {
                    dictForm.Clear(); 
                    dictForm.Add("@ID", formItem.ID);
                    dictForm.Add("@FormType", formItem.FormType);
                    dictForm.Add("@FormPath", formItem.FormPath);
                    dictForm.Add("@Description", formItem.Description);
                    this.dbAccess.update(sqlFormUpdate, dictForm);
                     
                    formID = formItem.ID;


                    dictForm.Clear();
                    dict.Add("@ParentID", formID);
                    this.dbAccess.update(sql_File_delete, dict);

                } else if(formItem.ID != 0 && formItem.Status != "updated"){
                    continue;
                }
                else //insert form
                {

                    dictForm.Clear();
                    dictForm.Add("@TrainingID", traininginfo.ID);
                    dictForm.Add("@FormType", formItem.FormType);
                    dictForm.Add("@FormPath", formItem.FormPath);
                    dictForm.Add("@Description", formItem.Description);
                    formID = Convert.ToInt32(this.dbAccess.select(sqlForm, dictForm).Rows[0][0]);
                }


                if (formItem.FileInfo != null)
                {
                    fileInfo = formItem.FileInfo;
                    //insert the News and return the row ID
                    dict = new Dictionary<string, object>();
                    dict.Add("@ParentID", formID);
                    dict.Add("@Type", File.ArticleFileType.Training);
                    dict.Add("@Name", fileInfo.Name);
                    dict.Add("@OriginalName", fileInfo.OriginalName);
                    dict.Add("@Description", fileInfo.Description);
                    dict.Add("@Content", fileInfo.Content);
                    dict.Add("@UploadUser", fileInfo.UploadUser);
                    dict.Add("@UploadDate", fileInfo.UploadDate);
                    dict.Add("@Status", GlobalSetting.ArticleStatus.Attached);

                    this.dbAccess.update(sql_File, dict);
                }

            }


            if (!string.IsNullOrEmpty(deletedFormID))
            {
                //delete form
                this.dbAccess.update(sqlFormDelete);
                //delete related physical file
                this.dbAccess.update(sqlPhysicalFile);
            }


            #region New Icon
            //delete the information of "New" icon before insert
            string sql_icon_delete = "delete from [NewIconControl] where Category=@Category and ParentID = @ParentID";
            dict = new Dictionary<string, object>();
            dict.Add("@Category", GlobalSetting.ArticleCategory.Training);
            dict.Add("@ParentID", traininginfo.ID);
            this.dbAccess.update(sql_icon_delete, dict);

            //insert the information of "New" icon 
            string sql_icon = "INSERT INTO [NewIconControl] "
                       + "([Category], [ParentID], [ExpiryDate]) VALUES "
                       + "(@Category, @ParentID, @ExpiryDate)";
            if (traininginfo.NewIconInfo != null)
            {
                dict = new Dictionary<string, object>();
                dict.Add("@Category", traininginfo.NewIconInfo.Category);
                dict.Add("@ParentID", traininginfo.ID);
                dict.Add("@ExpiryDate", traininginfo.NewIconInfo.ExpiryDate);
                this.dbAccess.update(sql_icon, dict);
            }
            #endregion


            this.dbAccess.Commit();

        }

        catch (Exception ex)
        {
            this.dbAccess.rollback();
            throw ex;
        }
        finally
        {
            this.dbAccess.close();
        }

    }

    public Jayrock.Json.JsonArray getAllTraining()
    {
        JsonArray trainings = new JsonArray();
        JsonObject tmpTraining;

        //create database access object 
        String sql = " SELECT SerialNo, [Training].[ID] ,[Name] Title, Status, 'Training' Category, SystemPara.Description type,  left(CONVERT(VARCHAR, sc.StartTime, " + GlobalSetting.DatabaseDateTimeFormat + "),10) EffectiveDate FROM [Training]  "
                   + " left outer join SystemPara on category = 'TrainingType' and [Training].type = SystemPara.ID "
                   + " join (select MIN(StartTime) as StartTime, TrainingID from TrainingSche group by TrainingID) sc on [Training].ID = sc.TrainingID "
                   + " where status <> 'Trashed' order by status, sc.StartTime desc, SerialNo";

        dbAccess.open();
        try
        {
            System.Data.DataTable dt = dbAccess.select(sql);
            if (dt.Rows.Count > 0)
            {
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    tmpTraining = new JsonObject();
                    foreach (System.Data.DataColumn col in dt.Columns)
                    {
                        tmpTraining.Accumulate(col.ColumnName.ToLower(), row[col.ColumnName]);
                    }

                    trainings.Add(tmpTraining);
                }
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

        return trainings;
    }

    public TrainingInfo getTraining(int id)
    {
        TrainingInfo traininginfo = new TrainingInfo();

        String sql = string.Format("SELECT [VersionNo], [SerialNo], [ID] ,[Type], [OptionalAttendance], [MaxAttendance], [Deadline],"
                                 + "[Name],[Location] ,[ContactPerson], "
                                 + "[PhoneNumber] ,[Department], [Email], [FormPath], [Details] ,[Status] ,[CreateDate] ,[CreateBy] "
                                 + "FROM [Training] where [ID] = {0}",
                    id);

        dbAccess.open();
        try
        {
            System.Data.DataTable dt = dbAccess.select(sql);

            traininginfo.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
            traininginfo.VersionNo = Convert.ToSingle(dt.Rows[0]["VersionNo"]);
            traininginfo.SerialNo = dt.Rows[0]["SerialNo"].ToString();
            traininginfo.Type = Convert.ToInt32(dt.Rows[0]["Type"].ToString());
            traininginfo.Name = dt.Rows[0]["Name"].ToString();
            traininginfo.OptionalAttendance = Convert.ToBoolean(dt.Rows[0]["OptionalAttendance"]);
            traininginfo.MaxAttendance = Convert.ToInt32(dt.Rows[0]["MaxAttendance"]);
            traininginfo.Deadline = Convert.ToDateTime(dt.Rows[0]["Deadline"]);
            traininginfo.Location = dt.Rows[0]["Location"].ToString();
            traininginfo.PhoneNumber = dt.Rows[0]["PhoneNumber"].ToString();
            traininginfo.ContactPerson = dt.Rows[0]["ContactPerson"].ToString();
            traininginfo.CreateBy = Convert.ToInt32(dt.Rows[0]["CreateBy"]);
            traininginfo.CreateDate = Convert.ToDateTime(dt.Rows[0]["CreateDate"]);
            traininginfo.Department = dt.Rows[0]["Department"].ToString();
            traininginfo.Email = dt.Rows[0]["Email"].ToString();
            traininginfo.Details = dt.Rows[0]["Details"].ToString();
            traininginfo.Status = dt.Rows[0]["Status"].ToString();
            traininginfo.FormPath = dt.Rows[0]["FormPath"].ToString();

            //new icon
            sql = string.Format("select ExpiryDate from [NewIconControl] where ParentID = {0} and Category = '{1}' ",
                traininginfo.ID, GlobalSetting.ArticleCategory.Training);
            dt = dbAccess.select(sql);
            if (dt.Rows.Count == 1)
            {
                traininginfo.NewIconInfo = new NewIconInfo();
                traininginfo.NewIconInfo.ExpiryDate = Convert.ToDateTime(dt.Rows[0]["ExpiryDate"]);
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

        return traininginfo;
    }

    public Jayrock.Json.JsonArray getTrainingSchedule(int id)
    {

        JsonArray scheList = new JsonArray();
        JsonObject tmpSche;

        String sql = string.Format("select * from TrainingSche where TrainingID = {0}",
                    id);

        dbAccess.open();
        try
        {
            System.Data.DataTable dt = dbAccess.select(sql);


            foreach (System.Data.DataRow row in dt.Rows)
            {
                tmpSche = new JsonObject();
                tmpSche.Accumulate("ID", row["ID"].ToString());
                tmpSche.Accumulate("scheduleDate", Convert.ToDateTime(row["StartTime"]).ToString(GlobalSetting.DateTimeFormat));
                tmpSche.Accumulate("startTime", Convert.ToDateTime(row["StartTime"]).ToString("HH:mm"));
                tmpSche.Accumulate("endTime", Convert.ToDateTime(row["EndTime"]).ToString("HH:mm"));

                scheList.Add(tmpSche);
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

        return scheList;
    }

    public Jayrock.Json.JsonArray getTrainingFormList(int id)
    {

        JsonArray scheList = new JsonArray();
        JsonObject tmpSche;

        String sql = string.Format("select [File].ID FileID, *, SystemPara.SubSequence "
                    + " from TrainingForm "
                    + " join SystemPara on formType = SystemPara.ID"
                    + " left outer join [File] on TrainingForm.ID = [File].ParentID and [File].Type = {1}"
                    + "where TrainingID = {0} ", id, File.ArticleFileType.Training);

        dbAccess.open();
        try
        {
            System.Data.DataTable dt = dbAccess.select(sql);


            foreach (System.Data.DataRow row in dt.Rows)
            {
                tmpSche = new JsonObject();
                tmpSche.Accumulate("ID", row["ID"].ToString());
                tmpSche.Accumulate("FormType", row["FormType"].ToString());
                tmpSche.Accumulate("Description", row["Description"].ToString());
                tmpSche.Accumulate("SubSequence", row["SubSequence"].ToString());

                if (row["FileID"] == DBNull.Value)
                {
                    tmpSche.Accumulate("FormPath", row["FormPath"].ToString());
                }
                else
                {
                    tmpSche.Accumulate("FormPath", "Service/FileService.aspx?ID=" + row["FileID"].ToString());
                }

                scheList.Add(tmpSche);
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

        return scheList;
    }

    public List<TrainingInfo> getLatestTrainings(int total)
    {
        List<TrainingInfo> list = new List<TrainingInfo>();
        TrainingInfo traininginfo;

        dbAccess.open();
        try
        {
            string sql = string.Format("select top {0} tr.*, sc.StartTime from training tr "
                                     + "join (select MIN(StartTime) as StartTime, TrainingID from TrainingSche group by TrainingID) sc "
                                     + "on tr.ID = sc.TrainingID "
                                     + "where status = '{1}' "
                                     + "order by StartTime desc, ID desc ",
                                        total,
                                        GlobalSetting.ArticleStatus.Published);

            if (total != 0)
            {
                sql = string.Format("select top {0} tr.*, sc.StartTime from training tr "
                                         + "join (select MIN(StartTime) as StartTime, TrainingID from TrainingSche group by TrainingID) sc "
                                         + "on tr.ID = sc.TrainingID "
                                         + "where status = '{1}' "
                                         + "order by StartTime desc, ID desc ",
                                            total,
                                            GlobalSetting.ArticleStatus.Published);
            }
            else
            {
                sql = string.Format("select tr.*, sc.StartTime from training tr "
                                         + "join (select MIN(StartTime) as StartTime, TrainingID from TrainingSche group by TrainingID) sc "
                                         + "on tr.ID = sc.TrainingID "
                                         + "where status = '{0}' "
                                         + "order by StartTime desc, ID desc ",
                                            GlobalSetting.ArticleStatus.Published);
            }
            System.Data.DataTable dt = dbAccess.select(sql);
            System.Data.DataTable dt2;

            foreach (System.Data.DataRow row in dt.Rows)
            {
                traininginfo = new TrainingInfo();

                traininginfo.ID = Convert.ToInt32(row["ID"]);
                traininginfo.Name = row["Name"].ToString();
                traininginfo.Location = row["Location"].ToString();
                traininginfo.PhoneNumber = row["PhoneNumber"].ToString();
                //traininginfo.EndTime = Convert.ToDateTime(row["EndTime"]);
                traininginfo.ContactPerson = row["ContactPerson"].ToString();
                traininginfo.CreateBy = Convert.ToInt32(row["CreateBy"]);
                traininginfo.CreateDate = Convert.ToDateTime(row["CreateDate"]);
                traininginfo.Department = row["Department"].ToString();
                traininginfo.Details = row["Details"].ToString();
                traininginfo.Status = row["Status"].ToString();

                traininginfo.Schedule = new List<TrainingScheInfo>();
                TrainingScheInfo tmp = new TrainingScheInfo();
                tmp.StartTime = Convert.ToDateTime(row["StartTime"]);
                traininginfo.Schedule.Add(tmp);

                //new icon
                sql = string.Format("select ExpiryDate from [NewIconControl] where ParentID = {0} and Category = '{1}' ",
                traininginfo.ID, GlobalSetting.ArticleCategory.Training);
                dt2 = dbAccess.select(sql);
                if (dt2.Rows.Count == 1)
                {
                    traininginfo.NewIconInfo = new NewIconInfo();
                    traininginfo.NewIconInfo.ExpiryDate = Convert.ToDateTime(dt2.Rows[0]["ExpiryDate"]);
                } 

                list.Add(traininginfo);
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

    public void updateStatus(string articleIDs, string newStatus)
    {

        //create database access object 
        String sql = string.Format("UPDATE [Training] SET [Status] = '{0}' where ID in ({1}) ",
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

    public JsonArray getTrainingDecision(int ID, int loginID)
    {

        Jayrock.Json.JsonArray ja = new JsonArray();
        Jayrock.Json.JsonObject jo;
        string sql = "select "
                    + "case when a.UserAction = 'NotAttend' then 'Not Attend' else a.UserAction END 'Decision' , "
                    + "Convert(varchar,s.StartTime, 103) + ' ' + "
                    + "left(Convert(varchar,s.StartTime, 114),5) 'DateTime' "
                    + "FROM ActivityLog a  "
                    + "join Training t on a.ActivityID = t.ID  "
                    + "join TrainingSche s on s.TrainingID = a.ActivityID and s.ID = a.ExtField "
                    + "where a.ExtField is not null and a.Category = 'Training' and t.ID = @ID and a.UserID = @UserID "
                    + "union all "
                    + "select "
                    + "case when a.UserAction = 'NotAttend' then 'Not Attend' else a.UserAction END 'Decision' , "
                    + "'' 'DateTime' "
                    + "FROM ActivityLog a  "
                    + "join Training t on a.ActivityID = t.ID  "
                    + "where a.ExtField is null and a.Category = 'Training' and t.ID = @ID and a.UserID = @UserID ";

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("@ID", ID);
        dict.Add("@UserID", loginID);

        dbAccess.open();

        try
        {
            System.Data.DataTable dt = dbAccess.select(sql, dict);

            foreach (System.Data.DataRow row in dt.Rows)
            {
                jo = new JsonObject();
                foreach (System.Data.DataColumn col in dt.Columns)
                {
                    jo.Accumulate(col.ColumnName.ToLower(), row[col.ColumnName]);
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
            dbAccess.close();
        }

        return ja;
    }

    public TrainingFormInfo getTrainingFormInfo(int formID)
    {
        TrainingFormInfo trainingFormInfo = new TrainingFormInfo();

        String sql = string.Format("select * from [TrainingForm] where [ID] = {0}",
                    formID);

        dbAccess.open();
        try
        {
            System.Data.DataTable dt = dbAccess.select(sql);
            trainingFormInfo.Description = dt.Rows[0]["Description"].ToString();
            trainingFormInfo.FormPath = dt.Rows[0]["FormPath"].ToString();
            trainingFormInfo.FormType = Convert.ToInt32(dt.Rows[0]["FormType"]);
            trainingFormInfo.ID = Convert.ToInt32(dt.Rows[0]["ID"]);

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            dbAccess.close();
        }

        return trainingFormInfo;
    }


}