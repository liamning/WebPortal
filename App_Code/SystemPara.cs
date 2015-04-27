using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for SystemPara
/// </summary>
public class SystemPara
{
    public static string getDescription(int ID)
    {
        DatabaseAccess dbAccess = new DatabaseAccess();
        string sql = "select p.description from SystemPara p where p.ID = " + ID.ToString();
        System.Data.DataTable dt;
        dbAccess.open();
        try
        {
            dt = dbAccess.select(sql);
        }
        catch
        {
            throw;
        }
        finally
        {
            dbAccess.close();
        }

        return dt.Rows[0][0].ToString();

    }

    public static System.Data.DataTable getSystemPara(string category)
    {
        DatabaseAccess dbAccess = new DatabaseAccess();
        string sql = "select p.ID, p.description, c.Description cDescription  "
                    + "from SystemPara p "
                    + "left outer join SystemPara c on p.ID = c.ParentID "
                    + "where p.category = '" + category + "' "
                    + "and p.IsAbandoned <> 1  and p.parentID is null and p.subsequence <> 0 " 
                    + "order by p.IsAbandoned ";
        System.Data.DataTable dt;
        dbAccess.open();
        try
        {
            dt = dbAccess.select(sql);
        }
        catch
        {
            throw;
        }
        finally
        {
            dbAccess.close();
        }

        return dt;

    }

    public static System.Data.DataTable getSystemPara(string category, bool includeAbandon)
    {
        DatabaseAccess dbAccess = new DatabaseAccess();
        string sql = "select p.ID, p.description, c.Description cDescription  "
                    + "from SystemPara p "
                    + "left outer join SystemPara c on p.ID = c.ParentID "
                    + "where p.category = '" + category + "' "
                    + (includeAbandon ? "" : "and p.IsAbandoned <> 1 ")
                    + "and p.parentID is null and p.subsequence <> 0 " 
                    + "order by p.IsAbandoned ";
        System.Data.DataTable dt;
        dbAccess.open();
        try
        {
            dt = dbAccess.select(sql);
        }
        catch
        {
            throw;
        }
        finally
        {
            dbAccess.close();
        }

        return dt;

    }

    public static System.Data.DataTable getSystemParaDetails(string category)
    {
        DatabaseAccess dbAccess = new DatabaseAccess();
        string sql = "select p.ID, p.IsAbandoned, p.subsequence, p.description, p.IsMultiDesc, c.ID cID, c.Description cDescription  "
                    + "from SystemPara p "
                    + "left outer join SystemPara c on p.ID = c.ParentID "
                    + "where p.category = '" + category + "' "
                    + "and p.parentID is null " 
                    + "and not exists (select 1 from SystemParaBuildin where SystemParaBuildin.ID = p.ID) "
                    + "order by p.IsAbandoned, p.subsequence ";

        System.Data.DataTable dt;
        dbAccess.open();
        try
        {
            dt = dbAccess.select(sql);
        }
        catch
        {
            throw;
        }
        finally
        {
            dbAccess.close();
        }

        return dt;

    }

    public static System.Data.DataTable getSystemParaType()
    {
        DatabaseAccess dbAccess = new DatabaseAccess();
        string sql = "select distinct category, categorydesc, sequence from SystemPara "
                   + "where subsequence = 0 "
                   + "and not exists (select 1 from SystemParaBuildin where SystemParaBuildin.ID = SystemPara.ID) "
                   +" order by sequence";
        System.Data.DataTable dt;
        dbAccess.open();
        try
        {
            dt = dbAccess.select(sql);
        }
        catch
        {
            throw;
        }
        finally
        {
            dbAccess.close();
        }

        return dt;

    }

    public static string getSystemParaBuildinCode()
    {
        Jayrock.Json.JsonObject ja = new Jayrock.Json.JsonObject();
        Jayrock.Json.JsonObject jo;

        DatabaseAccess dbAccess = new DatabaseAccess();
        string sql = "select * from SystemParaBuildin";
        System.Data.DataTable dt;
        dbAccess.open();
        try
        {
            dt = dbAccess.select(sql);

            foreach(System.Data.DataRow row in dt.Rows)
            {
                jo = new Jayrock.Json.JsonObject();
                foreach(System.Data.DataColumn col in dt.Columns)
                {
                    jo.Accumulate(col.ColumnName, row[col.ColumnName]);
                }
                ja.Accumulate(row["ID"].ToString(), jo);
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

        return ja.ToString(); 
    }

    

    public void save(string category, string categoryDesc, string description, string cdescription, bool isMultiDesc)
    {
        DatabaseAccess dbAccess = new DatabaseAccess();
        string sql = "insert into SystemPara (Sequence, Subsequence, Category, CategoryDesc, Description, IsMultiDesc) values "
            + " ((select top 1 Sequence from SystemPara where Category = @Category), "
            + " (select top 1 max(Subsequence) + 1 from SystemPara where Category = @Category), "
            + " @Category, @CategoryDesc,"
            + " @Description, @IsMultiDesc);select SCOPE_IDENTITY();";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("@Category", category);
        dict.Add("@CategoryDesc", categoryDesc);
        dict.Add("@Description", description);
        dict.Add("@IsMultiDesc", isMultiDesc);

        dbAccess.open();
        dbAccess.BeginTransaction();
        try
        {
            System.Data.DataTable dt = dbAccess.select(sql, dict);
            int parentID = Convert.ToInt32(dt.Rows[0][0]);


            if (isMultiDesc)
            {

                sql = " insert into SystemPara (ParentID, Sequence, Category,CategoryDesc, Description) values "
                    + " (@ParentID, (select top 1 sequence from SystemPara where ID = @ParentID), @Category, @CategoryDesc, @Description)";

                dict.Clear();
                dict.Add("@ParentID", parentID);
                dict.Add("@Sequence", category);
                dict.Add("@Category", category);
                dict.Add("@CategoryDesc", categoryDesc);
                dict.Add("@Description", cdescription);
                dbAccess.update(sql, dict);
            }

            dbAccess.Commit();
        }
        catch
        {
            dbAccess.rollback();
            throw;
        }
        finally
        {
            dbAccess.close();
        }
    }

    public void update(int ID, string description, int cID, string cdescription)
    {
        DatabaseAccess dbAccess = new DatabaseAccess();
        //string sql = "update SystemPara set Description = @Description where ID = @ID;";


        string sql = "update SystemPara set Description = @Description where ID = @ID;"
            + "update SystemPara set Description = @cDescription where ID = @cID;";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("@Description", description);
        dict.Add("@ID", ID);
        dict.Add("@cDescription", cdescription);
        dict.Add("@cID", cID);

        dbAccess.open();
        try
        {
            dbAccess.update(sql, dict);
        }
        catch
        {
            throw;
        }
        finally
        {
            dbAccess.close();
        }

    }

    public void abandon(int ID)
    {
        DatabaseAccess dbAccess = new DatabaseAccess();
        string sql = "update SystemPara set isAbandoned = 1 where ID = @ID and isAbandoned <> 1;";
        Dictionary<string, object> dict = new Dictionary<string, object>(); 
        dict.Add("@ID", ID);

        dbAccess.open();
        try
        {
            dbAccess.update(sql, dict);
        }
        catch
        {
            throw;
        }
        finally
        {
            dbAccess.close();
        }

    }
}