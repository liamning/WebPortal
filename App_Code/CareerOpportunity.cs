using System;
using System.Collections.Generic;
using System.Web;
using Jayrock.Json;
/// <summary>
/// Summary description for CareerOpportunity
/// </summary>
public class CareerOpportunity
{
    //object for DB access
    private DatabaseAccess dbAccess = new DatabaseAccess();
    public void save(CareerOpportunityInfo careeroppty)
    {
        string sql = " INSERT INTO [CareerOpportunity]  "
                   + " ([Type] ,[SerialNo] ,[JobFunction] ,[Experience] ,[CareerLevel] ,[Qualification] ,[Division],[Department],[Location] , "
                   + " [EmploymentType] ,[Email] ,[Details], [Disclaimer] ,[Status] ,[CreateDate] ,[CreateBy]) "
                   + " VALUES "
                   + " (@Type ,@SerialNo, @JobFunction ,@Experience ,@CareerLevel ,@Qualification ,@Division ,@Department ,@Location , "
                   + " @EmploymentType ,@Email ,@Details, @Disclaimer ,@Status ,getDate() ,@CreateBy);select SCOPE_IDENTITY(); ";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("@Type", careeroppty.Type);
        dict.Add("@SerialNo", careeroppty.SerialNo);
        dict.Add("@JobFunction", careeroppty.JobFunction);
        dict.Add("@Experience", careeroppty.Experience);
        dict.Add("@CareerLevel", careeroppty.CareerLevel);
        dict.Add("@Qualification", careeroppty.Qualification);
        //dict.Add("@Salary", careeroppty.Salary);
        dict.Add("@Division", careeroppty.Division);
        dict.Add("@Department", careeroppty.Department);
        dict.Add("@Location", careeroppty.Location);
        dict.Add("@EmploymentType", careeroppty.EmploymentType);
        dict.Add("@Email", careeroppty.Email);
        dict.Add("@Details", careeroppty.Details);
        dict.Add("@Disclaimer", careeroppty.Disclaimer);
        dict.Add("@Status", careeroppty.Status);
        dict.Add("@CreateBy", careeroppty.CreateBy);
        this.dbAccess.open();
        try
        {
            this.dbAccess.BeginTransaction();
            System.Data.DataTable dt = this.dbAccess.select(sql, dict);
            string lastID = dt.Rows[0][0].ToString();

            #region new icon

            //insert the information of "New" icon 
            string sql_icon = "INSERT INTO [NewIconControl] "
                       + "([Category], [ParentID], [ExpiryDate]) VALUES "
                       + "(@Category, @ParentID, @ExpiryDate)";
            if (careeroppty.NewIconInfo != null)
            {
                dict = new Dictionary<string, object>();
                dict.Add("@Category", careeroppty.NewIconInfo.Category);
                dict.Add("@ParentID", lastID);
                dict.Add("@ExpiryDate", careeroppty.NewIconInfo.ExpiryDate);
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
    public void update(CareerOpportunityInfo careeroppty)
    {
        string sql = " UPDATE [CareerOpportunity]"
                + " SET [Type] = @Type"
                + " ,[VersionNo] = @VersionNo"
                + " ,[SerialNo] = @SerialNo"
                + " ,[JobFunction] = @JobFunction"
                + " ,[Experience] = @Experience"
                + " ,[CareerLevel] = @CareerLevel"
                + " ,[Qualification] = @Qualification"
                + " ,[Division] = @Division"
                + " ,[Department] = @Department"
                + " ,[Location] = @Location"
                + " ,[EmploymentType] = @EmploymentType"
                + " ,[Email] = @Email"
                + " ,[Details] = @Details "
                + " ,[Disclaimer] = @Disclaimer "
                + " ,[UpdateDate] = getDate() "
                + " ,[UpdateBy] = @UpdateBy "
                + " WHERE ID=@ID";

        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("@Type", careeroppty.Type);
        dict.Add("@VersionNo", careeroppty.VersionNo);
        dict.Add("@SerialNo", careeroppty.SerialNo);
        dict.Add("@JobFunction", careeroppty.JobFunction);
        dict.Add("@Experience", careeroppty.Experience);
        dict.Add("@CareerLevel", careeroppty.CareerLevel);
        dict.Add("@Qualification", careeroppty.Qualification);
        dict.Add("@Division", careeroppty.Division);
        dict.Add("@Department", careeroppty.Department);
        dict.Add("@Location", careeroppty.Location);
        dict.Add("@EmploymentType", careeroppty.EmploymentType);
        dict.Add("@Email", careeroppty.Email);
        dict.Add("@Details", careeroppty.Details);
        dict.Add("@Disclaimer", careeroppty.Disclaimer);
        dict.Add("@UpdateBy", careeroppty.UpdateBy);
        dict.Add("@ID", careeroppty.ID);
        this.dbAccess.open();
        try
        {
            this.dbAccess.BeginTransaction();
            this.dbAccess.update(sql, dict);
             
            #region New Icon
            //delete the information of "New" icon before insert
            string sql_icon_delete = "delete from [NewIconControl] where Category=@Category and ParentID = @ParentID";
            dict = new Dictionary<string, object>();
            dict.Add("@Category", GlobalSetting.ArticleCategory.Career);
            dict.Add("@ParentID", careeroppty.ID);
            this.dbAccess.update(sql_icon_delete, dict);

            //insert the information of "New" icon 
            string sql_icon = "INSERT INTO [NewIconControl] "
                       + "([Category], [ParentID], [ExpiryDate]) VALUES "
                       + "(@Category, @ParentID, @ExpiryDate)";
            if (careeroppty.NewIconInfo != null)
            {
                dict = new Dictionary<string, object>();
                dict.Add("@Category", careeroppty.NewIconInfo.Category);
                dict.Add("@ParentID", careeroppty.ID);
                dict.Add("@ExpiryDate", careeroppty.NewIconInfo.ExpiryDate);
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
    public void updateStatus(string articleIDs, string newStatus)
    {
        //create database access object 
        String sql = string.Format("UPDATE [CareerOpportunity] SET [Status] = '{0}' where ID in ({1}) ",
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
    public JsonArray getAllCareerOpportunities()
    {
        JsonArray ja = new JsonArray();
        JsonObject jo;
        String tmpStr;
        dbAccess.open();
        try
        {
            string sql = " select [CareerOpportunity].ID, SerialNo, PositionPara.Description Title,Status,'Career' Category, SystemPara.Description Type,  left(CONVERT(VARCHAR, [CreateDate], " + GlobalSetting.DatabaseDateTimeFormat + "),10) EffectiveDate from [CareerOpportunity] "
                       + " left outer join SystemPara on category = 'CareerType' and [CareerOpportunity].type = SystemPara.ID "
                       + " left outer join SystemPara PositionPara  on PositionPara.category = 'Position' and [CareerOpportunity].JobFunction = PositionPara.ID "
                       + " where status <> 'Trashed' order by status, CreateDate desc, SerialNo ";
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
    public CareerOpportunityInfo getCareerOpportunity(int ID)
    {
        CareerOpportunityInfo careerinfo = new CareerOpportunityInfo();
        dbAccess.open();
        try
        {
            string sql = "select * from [CareerOpportunity] where ID = " + ID.ToString();
            System.Data.DataTable dt = dbAccess.select(sql);
            careerinfo.Type = Convert.ToInt32(dt.Rows[0]["Type"].ToString());
            careerinfo.VersionNo = Convert.ToSingle(dt.Rows[0]["VersionNo"]);
            careerinfo.SerialNo = dt.Rows[0]["SerialNo"].ToString();
            careerinfo.Division = Convert.ToInt32(dt.Rows[0]["Division"]);
            careerinfo.Department = Convert.ToInt32(dt.Rows[0]["Department"]);
            careerinfo.CareerLevel = dt.Rows[0]["CareerLevel"].ToString();
            careerinfo.CreateBy = Convert.ToInt32(dt.Rows[0]["CreateBy"]);
            careerinfo.Details = dt.Rows[0]["Details"].ToString();
            careerinfo.Email = dt.Rows[0]["Email"].ToString();
            careerinfo.EmploymentType = Convert.ToInt32(dt.Rows[0]["EmploymentType"]);
            careerinfo.Experience = Convert.ToSingle(dt.Rows[0]["Experience"]);
            careerinfo.JobFunction = Convert.ToInt32(dt.Rows[0]["JobFunction"]);
            careerinfo.Location = dt.Rows[0]["Location"].ToString();
            careerinfo.Qualification = Convert.ToInt32(dt.Rows[0]["Qualification"]);
            careerinfo.Disclaimer = dt.Rows[0]["Disclaimer "].ToString();
            //careerinfo.Salary = Convert.ToInt32(dt.Rows[0]["Salary"]);
            careerinfo.Status = dt.Rows[0]["Status"].ToString();


            //new icon
            sql = string.Format("select ExpiryDate from [NewIconControl] where ParentID = {0} and Category = '{1}' ",
                ID, GlobalSetting.ArticleCategory.Career);
            dt = dbAccess.select(sql);
            if (dt.Rows.Count == 1)
            {
                careerinfo.NewIconInfo = new NewIconInfo();
                careerinfo.NewIconInfo.ExpiryDate = Convert.ToDateTime(dt.Rows[0]["ExpiryDate"]);
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
        return careerinfo;
    }
    public List<CareerOpportunityInfo> getLatestCareerOpportunities(int total)
    {
        List<CareerOpportunityInfo> list = new List<CareerOpportunityInfo>();
        CareerOpportunityInfo careerOpportunityInfo;
        dbAccess.open();
        try
        {
            string sql;

            if (total != 0) {
                sql = string.Format("select top {0} * from [CareerOpportunity] "
                + " join SystemPara para1 on para1.ID = JobFunction"
                + " join SystemPara para2 on para2.ID = Division"
                + " where status = '{1}' order by para2.[Description], para1.[Description]",
                                            total,
                                            GlobalSetting.ArticleStatus.Published);
            }
            else {
                sql = string.Format("select * from [CareerOpportunity] "
                + " join SystemPara para1 on para1.ID = JobFunction"
                + " join SystemPara para2 on para2.ID = Division"
                + " where status = '{0}' order by para2.[Description], para1.[Description]",
                                            GlobalSetting.ArticleStatus.Published);
            }
            System.Data.DataTable dt = dbAccess.select(sql);
            System.Data.DataTable dt2;
            foreach (System.Data.DataRow row in dt.Rows)
            {
                careerOpportunityInfo = new CareerOpportunityInfo();
                careerOpportunityInfo.CareerLevel = row["CareerLevel"].ToString();
                careerOpportunityInfo.CreateBy = Convert.ToInt32(row["CreateBy"]);
                careerOpportunityInfo.CreateDate = Convert.ToDateTime(row["CreateDate"]);
                careerOpportunityInfo.Division = Convert.ToInt32(row["Division"]);
                careerOpportunityInfo.Details = row["Details"].ToString();
                careerOpportunityInfo.Email = row["Email"].ToString();
                careerOpportunityInfo.EmploymentType = Convert.ToInt32(row["EmploymentType"]);
                careerOpportunityInfo.Experience = Convert.ToSingle(row["Experience"]);
                careerOpportunityInfo.ID = Convert.ToInt32(row["ID"]);
                careerOpportunityInfo.JobFunction = Convert.ToInt32(row["JobFunction"]);
                careerOpportunityInfo.Location = row["Location"].ToString();
                careerOpportunityInfo.Qualification = Convert.ToInt32(row["Qualification"]);
                //careerOpportunityInfo.Salary = Convert.ToInt32(row["Salary"]);
                careerOpportunityInfo.Status = row["Status"].ToString();


                //new icon
                sql = string.Format("select ExpiryDate from [NewIconControl] where ParentID = {0} and Category = '{1}' ",
                careerOpportunityInfo.ID, GlobalSetting.ArticleCategory.Career);
                dt2 = dbAccess.select(sql);
                if (dt2.Rows.Count == 1)
                {
                    careerOpportunityInfo.NewIconInfo = new NewIconInfo();
                    careerOpportunityInfo.NewIconInfo.ExpiryDate = Convert.ToDateTime(dt2.Rows[0]["ExpiryDate"]);
                } 

                list.Add(careerOpportunityInfo);
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


    public System.Text.StringBuilder getLatestCareerOpportunitiesStr(int total)
    {
        List<CareerOpportunityInfo> latestCareerOpportunityList = this.getLatestCareerOpportunities(0);
        System.Text.StringBuilder careerOpportunities = new System.Text.StringBuilder();
         
        int lastDivision = 0;
        careerOpportunities.Append("<ul class='list'>");
        int count = 0;
        foreach (CareerOpportunityInfo careerOppty in latestCareerOpportunityList)
        {
            if (count == total && total != 0) break;
            if (lastDivision == 0)
            { 
                careerOpportunities.Append("<li><a>");
                careerOpportunities.Append(SystemPara.getDescription(careerOppty.Division));
                //careerOpportunities.Append(careerOppty.Division);
                careerOpportunities.Append("</a>");
                careerOpportunities.Append("<ul class='margin20px'>");

                lastDivision = careerOppty.Division;
                count++;
                
            }
            else if (lastDivision != 0 && lastDivision != careerOppty.Division)
            {
                careerOpportunities.Append("</ul>");
                careerOpportunities.Append("</li>");
                careerOpportunities.Append("<li><a>");
                careerOpportunities.Append(SystemPara.getDescription(careerOppty.Division));
                careerOpportunities.Append("</a>");
                careerOpportunities.Append("<ul class='margin20px'>");

                lastDivision = careerOppty.Division;
                count++;
            }

            if (careerOppty.NewIconInfo != null)
            {
                careerOpportunities.Append("<li class='file newIcon'><a href=ViewCareer.aspx?ID=");
            }
            else
            {
                careerOpportunities.Append("<li class='file'><a href=ViewCareer.aspx?ID=");
            }
            careerOpportunities.Append(careerOppty.ID.ToString());
            careerOpportunities.Append(">");
            careerOpportunities.Append(SystemPara.getDescription(careerOppty.JobFunction));
            careerOpportunities.Append("</a>");
            careerOpportunities.Append("</li>"); 

        }

        careerOpportunities.Append("</ul>");
        careerOpportunities.Append("</li>");
        careerOpportunities.Append("</ul>");

        return careerOpportunities;
    }
}
