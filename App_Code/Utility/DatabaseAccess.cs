using System;
using System.Collections.Generic; 
using System.Web;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Summary description for DatabaseAccess
/// </summary>
public class DatabaseAccess
{
    private string connectionString = System.Configuration.ConfigurationManager.AppSettings["connectionString"];
    private SqlConnection conn;
    private SqlTransaction transation;

    public DatabaseAccess()
	{
        conn = new SqlConnection(this.connectionString);
	}

    public void BeginTransaction()
    {
        transation = conn.BeginTransaction();
    }

    public void Commit()
    {
        transation.Commit();
        this.transation.Dispose();
        this.transation = null;
    }

    public void rollback()
    {
        transation.Rollback();
        this.transation.Dispose();
        this.transation = null;
    }

    public void open()
    {
        if (conn.State != ConnectionState.Open)
            conn.Open(); 
    }

    public void close()
    {
        conn.Close();
    }

    public void update(string sql)
    {
        SqlCommand cmd = conn.CreateCommand();
        if (this.transation != null)
        {
            cmd.Transaction = this.transation;
        }
        cmd.CommandText = sql;
        cmd.ExecuteNonQuery();
    }

    public void update(string sql,Dictionary<string, object> dict)
    {
        SqlCommand cmd = conn.CreateCommand();

        foreach (string key in dict.Keys)
        {
            cmd.Parameters.Add(new SqlParameter(key, dict[key]));
        }

        if (this.transation != null)
        {
            cmd.Transaction = this.transation;
        }
        cmd.CommandText = sql;
        cmd.ExecuteNonQuery();
    }

    public System.Data.DataTable select(string sql)
    {
        SqlCommand cmd = conn.CreateCommand();
        cmd.CommandText = sql;

        if (this.transation != null)
        {
            cmd.Transaction = this.transation;
        }

        SqlDataAdapter da = new SqlDataAdapter(cmd);
        System.Data.DataTable dt = new System.Data.DataTable();
        da.Fill(dt);
        return dt;
    }
    public System.Data.DataTable select(string sql, Dictionary<string, object> dict)
    {
        SqlCommand cmd = conn.CreateCommand();
        cmd.CommandText = sql;

        foreach (string key in dict.Keys)
        {
            cmd.Parameters.Add(new SqlParameter(key, dict[key]));
        }

        if (this.transation != null)
        {
            cmd.Transaction = this.transation;
        }

        SqlDataAdapter da = new SqlDataAdapter(cmd);
        System.Data.DataTable dt = new System.Data.DataTable();
        da.Fill(dt);
        return dt;
    }
}