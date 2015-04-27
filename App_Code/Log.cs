using System;
using System.Collections.Generic;
using System.Web;

using System.IO;

/// <summary>
/// Summary description for Log
/// </summary>
public class Log
{
    public static readonly string LogFolder = System.Configuration.ConfigurationManager.AppSettings["LogFolder"];
	public Log()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public class Type
    {
        public const string Message = "Message";
        public const string Exception = "Exception";
    }
    public static void log(string msg, string type)
    {
        FileStream fileStream = new FileStream(LogFolder + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".txt", FileMode.Append);
        StreamWriter writer = new StreamWriter(fileStream);

        writer.WriteLine(string.Format("[{0:yyyy-MM-dd HH:mm:ss}] [{1}]: {2}", DateTime.Now, type, msg));

        writer.Close();
        fileStream.Close();
    }

}