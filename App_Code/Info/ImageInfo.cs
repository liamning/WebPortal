using System;
using System.Collections.Generic;

using System.Web;

/// <summary>
/// Summary description for Image
/// </summary>
public class ImageInfo
{
    public int ID;
    public int ParentID;
    public string FileName;
    public string Description;
    public string Extension;
    public string MIME;
    public string Type;
    public int ContentLength;
    public byte[] Content;
}