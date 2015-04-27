using System;
using System.Collections.Generic;
using System.Web;
using System.IO;

/// <summary>
/// Summary description for Image
/// </summary>
public class ImageResize
{
	public ImageResize()
	{
		//
		// TODO: Add constructor logic here
		//
	}

   
    public static byte[] ResizeImage(Stream imageStream)
    {
        System.Drawing.Image orignImage = System.Drawing.Image.FromStream(imageStream);
        byte[] result;
        System.Drawing.Image resized;
        if (orignImage.Height > GlobalSetting.MaxImageHeight || orignImage.Width > GlobalSetting.MaxImageWidth)
        {
            resized = ResizeImage(orignImage, new System.Drawing.Size(GlobalSetting.MaxImageWidth, GlobalSetting.MaxImageHeight), true);
        }
        else
        {
            resized = orignImage;
        }
        MemoryStream memStream = new MemoryStream();
        resized.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
        result = memStream.ToArray();
        return result;
    }


    public static byte[] ResizeImage(Stream imageStream, System.Drawing.Imaging.ImageFormat format)
    {
        System.Drawing.Image orignImage = System.Drawing.Image.FromStream(imageStream);
        byte[] result;
        System.Drawing.Image resized;
        if (orignImage.Height > GlobalSetting.MaxImageHeight || orignImage.Width > GlobalSetting.MaxImageWidth)
        {
            resized = ResizeImage(orignImage, new System.Drawing.Size(GlobalSetting.MaxImageWidth, GlobalSetting.MaxImageHeight), true);
        }
        else
        {
            resized = orignImage;
        }
        MemoryStream memStream = new MemoryStream();
        resized.Save(memStream, format);
        result = memStream.ToArray();
        return result;
    }

    public static byte[] getPreviewImage(Stream imageStream, int width, int height)
    {
        System.Drawing.Image orignImage = System.Drawing.Image.FromStream(imageStream);
        byte[] result;
        System.Drawing.Image resized;
        if (orignImage.Height > height || orignImage.Width > width)
        {
            resized = ResizeImage(orignImage, new System.Drawing.Size(width, height), true);
        }
        else
        {
            resized = ResizeImage(orignImage, new System.Drawing.Size(orignImage.Width, orignImage.Height), true);
        }
        MemoryStream memStream = new MemoryStream();
        resized.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
        result = memStream.ToArray();
        return result;
    }


    public static System.Drawing.Image ResizeImage(System.Drawing.Image image, System.Drawing.Size size, bool preserveAspectRatio)
    {
        int newWidth;
        int newHeight;
        if (preserveAspectRatio)
        {
            int originalWidth = image.Width;
            int originalHeight = image.Height;
            float percentWidth = (float)size.Width / (float)originalWidth;
            float percentHeight = (float)size.Height / (float)originalHeight;
            float percent = percentHeight < percentWidth ? percentHeight : percentWidth;
            newWidth = (int)(originalWidth * percent);
            newHeight = (int)(originalHeight * percent);
        }
        else
        {
            newWidth = size.Width;
            newHeight = size.Height;
        }

        System.Drawing.Image newImage = new System.Drawing.Bitmap(newWidth, newHeight);
        using (System.Drawing.Graphics graphicsHandle = System.Drawing.Graphics.FromImage(newImage))
        {
            graphicsHandle.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphicsHandle.DrawImage(image, 0, 0, newWidth, newHeight);
        }
        return newImage;
    }
}