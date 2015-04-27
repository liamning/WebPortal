using System;
using System.Collections.Generic;
using System.Web;

using Jayrock.Json;

/// <summary>
/// Summary description for OtherSystemLink
/// </summary>
public class OtherSystemLink
{
    DatabaseAccess dbAccess = new DatabaseAccess();

    public Jayrock.Json.JsonArray getSystemLinkList()
    {

        Jayrock.Json.JsonArray ja = new Jayrock.Json.JsonArray();
        Jayrock.Json.JsonObject jo;
        string sql = "select ID, name, link from OtherSystemLink";
        dbAccess.open();

        try
        {
            System.Data.DataTable dt= dbAccess.select(sql);

            foreach (System.Data.DataRow dr in dt.Rows)
            {
                jo = new JsonObject();
                foreach (System.Data.DataColumn col in dt.Columns)
                {
                    jo.Accumulate(col.ColumnName.ToLower(), dr[col.ColumnName].ToString());
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

    public List<OtherSystemLinkInfo> getSystemLinkDetailList()
    {
        List<OtherSystemLinkInfo> result = new List<OtherSystemLinkInfo>();
        OtherSystemLinkInfo tmp;
        string sql = "select ID, name, link from OtherSystemLink";
        dbAccess.open();

        try
        {
            System.Data.DataTable dt = dbAccess.select(sql);
            foreach (System.Data.DataRow row in dt.Rows)
            {
                tmp = new OtherSystemLinkInfo(); 
                tmp.ID = Convert.ToInt32(row["ID"]);
                //tmp.CreateBy = Convert.ToInt32(row["CreateBy"]);
                //tmp.CreateDate = Convert.ToDateTime(row["CreateDate"]);
                tmp.Link = Convert.ToString(row["Link"]);
                tmp.Name = Convert.ToString(row["Name"]);
                result.Add(tmp);
            }
            return result;
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
    public void save(OtherSystemLinkInfo info)
    {
        string sql = "insert into OtherSystemLink (Name, Link, CreateDate, CreateBy) values (@Name, @Link, getDate(), @CreateBy)";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("Name", info.Name);
        dict.Add("Link", info.Link);
        dict.Add("CreateBy", info.CreateBy);

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

    public void update(OtherSystemLinkInfo info)
    {
        string sql = "update OtherSystemLink set name = @Name, link=@Link where [ID] = @ID;";
        Dictionary<string, object> dict = new Dictionary<string, object>();
        dict.Add("Name", info.Name);
        dict.Add("Link", info.Link);
        dict.Add("ID", info.ID);

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

    public void delete(string IDs)
    {
        string sql = string.Format("delete from OtherSystemLink where ID in ({0})", IDs) ; 

        dbAccess.open();

        try
        {
            dbAccess.update(sql);
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