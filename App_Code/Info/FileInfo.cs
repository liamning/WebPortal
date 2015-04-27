using System;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for FileInfo
/// </summary>
public class FileInfo
{
	public FileInfo()
	{
		//
		// TODO: Add constructor logic here
		//
	}


    public int  ID;
    public int Type;
    public string Name;
    public string OriginalName;
    public string Description;
    public DateTime UploadDate;
    public int UploadUser;
    public byte[] Content;

    public string Path;
      
    public string Extension
    {
        get
        {
            if (!String.IsNullOrEmpty(OriginalName))
            {
                return OriginalName.Substring(OriginalName.IndexOf("."));
            }
            return "";
        }
    }
    public string FileNameForDownload
    {
        get
        {
            return this.Name + this.Extension;
        }
    }

}