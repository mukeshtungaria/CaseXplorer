// ***********************************************************************
// Author           : Dennis Sebenick
// Created          : 08-29-2011
//
// Last Modified By : dennis
// Last Modified On : 02-06-2012
// ***********************************************************************
// <copyright file="clsVideo.cs" company="DGCC.COM">
//     . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Web;
using System.IO;

/// <summary>
/// Video Processing Procedures
/// </summary>
public class clsVideo
{
    /// <summary>
    /// Initializes a new instance of the <see cref="clsVideo" /> class.
    /// </summary>
	public clsVideo()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    /// <summary>
    /// Gets the URL.
    /// </summary>
    /// <param name="relativeUrl">The relative URL.</param>
    /// <returns>Returns URL (System.String)</returns>
    public static string GetURL(string relativeUrl)
    {
        if (HttpContext.Current == null)
            return relativeUrl;

        if (relativeUrl.StartsWith("/"))
            relativeUrl = relativeUrl.Insert(0, "~");
        if (!relativeUrl.StartsWith("~/"))
            relativeUrl = relativeUrl.Insert(0, "~/");

        var url = HttpContext.Current.Request.Url;
        var port = url.Port != 80 ? (":" + url.Port) : String.Empty;

        return String.Format("{0}://{1}{2}{3}",
               url.Scheme, url.Host, port, VirtualPathUtility.ToAbsolute(relativeUrl));


    }

    /// <summary>
    /// Gets the duration of the Flash Video File (FLV)
    /// </summary>
    /// <param name="filename">The filename.</param>
    /// <returns>Duration (System.Double)</returns>
    public static double GetFLVDuration(string filename)
    {
        if (!File.Exists(filename)) return 0;
        try
        {
            int duration = 0;
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            fs.Position = fs.Length - 4;
            using (BinaryReader br = new BinaryReader(fs))
            {
                byte[] hex = br.ReadBytes(4);
                int offset = hex[3] + (hex[2] << 8) + (hex[1] << 16) + (hex[0] << 24);
                fs.Position = fs.Length - offset;
                hex = br.ReadBytes(3);
                duration = hex[2] + (hex[1] << 8) + (hex[0] << 16);
            }
            fs.Close();
            fs.Dispose();
            return duration / 1000.0;
        }
        catch (Exception)
        {
            return 0;
        }
    } 

}

/// <summary>
/// Reads the meta information embedded in an FLV file
/// </summary>
public class FlvMetaDataReader
{
    /// <summary>
    /// 
    /// </summary>
    static string onMetaData = "";
    /// <summary>
    /// 
    /// </summary>
    static string bytesToFile = "";

    /// <summary>
    /// Gets the FLV meta info.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <returns>FlvMetaInfo.</returns>
    /// <exception cref="System.Exception"></exception>
    public static FlvMetaInfo GetFlvMetaInfo(string path)
    {
        if (!File.Exists(path))
        {
            throw new Exception(String.Format("File '{0}' doesn't exist for FlvMetaDataReader.GetFlvMetaInfo(path)", path));
        }
        bool hasMetaData = false;
        double duration = 0;
        double width = 0;
        double height = 0;
        double videoDataRate = 0;
        double audioDataRate = 0;
        Double frameRate = 0;
        // open file 

        using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            try
            {
                byte[] bytes = new byte[1000];
                fileStream.Seek(27, SeekOrigin.Begin);
                int result = fileStream.Read(bytes, 0, 1000);
                bytesToFile = ByteArrayToString(bytes);
                onMetaData = bytesToFile.Substring(0, 10);

                // if "onMetaData" exists then proceed to read the attributes 
                if (onMetaData == "onMetaData")
                {
                    hasMetaData = true;
                    // 16 bytes past "onMetaData" is the data for "duration" 
                    duration = GetNextDouble(bytes, bytesToFile.IndexOf("duration") + 9, 8);
                    // 8 bytes past "duration" is the data for "width" 
                    width = GetNextDouble(bytes, bytesToFile.IndexOf("width") + 6, 8);
                    // 9 bytes past "width" is the data for "height" 
                    height = GetNextDouble(bytes, bytesToFile.IndexOf("height") + 7, 8);
                    // 16 bytes past "height" is the data for "videoDataRate" 
                    videoDataRate = GetNextDouble(bytes, bytesToFile.IndexOf("videodatarate") + 14, 8);
                    // 16 bytes past "videoDataRate" is the data for "audioDataRate" 
                    audioDataRate = GetNextDouble(bytes, bytesToFile.IndexOf("audiodatarate") + 14, 8);
                    // 12 bytes past "audioDataRate" is the data for "frameRate" 
                    frameRate = GetNextDouble(bytes, bytesToFile.IndexOf("framerate") + 10, 8);
                }
            }
            catch (Exception e)
            {
                // no error handling 
            }
            finally
            {
                fileStream.Close();
            }
        }
        return new FlvMetaInfo(hasMetaData, duration, width, height, videoDataRate, audioDataRate, frameRate);
    }

    /// <summary>
    /// Gets the next double.
    /// </summary>
    /// <param name="b">The b.</param>
    /// <param name="offset">The offset.</param>
    /// <param name="length">The length.</param>
    /// <returns>Double.</returns>
    private static Double GetNextDouble(Byte[] b, int offset, int length)
    {
        MemoryStream ms = new MemoryStream(b);
        // move the desired number of places in the array 
        ms.Seek(offset, SeekOrigin.Current);
        // create byte array 
        byte[] bytes = new byte[length];
        // read bytes 
        int result = ms.Read(bytes, 0, length);
        // convert to double (all flass values are written in reverse order) 
        return ByteArrayToDouble(bytes, true);
    }

    /// <summary>
    /// Bytes the array to string.
    /// </summary>
    /// <param name="bytes">The bytes.</param>
    /// <returns>System.String.</returns>
    private static string ByteArrayToString(byte[] bytes)
    {
        string byteString = string.Empty;
        foreach (byte b in bytes)
        {
            byteString += Convert.ToChar(b).ToString();
        }
        return byteString;
    }

    /// <summary>
    /// Bytes the array to double.
    /// </summary>
    /// <param name="bytes">The bytes.</param>
    /// <param name="readInReverse">if set to <c>true</c> [read in reverse].</param>
    /// <returns>Double.</returns>
    /// <exception cref="System.Exception"></exception>
    private static Double ByteArrayToDouble(byte[] bytes, bool readInReverse)
    {
        if (bytes.Length != 8)
            throw new Exception("bytes must be exactly 8 in Length");
        if (readInReverse)
            Array.Reverse(bytes);
        return BitConverter.ToDouble(bytes, 0);
    }
}
/// <summary>
/// Read only container holding meta data embedded in FLV files
/// </summary>
public class FlvMetaInfo
{
    /// <summary>
    /// 
    /// </summary>
    private Double _duration;
    /// <summary>
    /// 
    /// </summary>
    private Double _width;
    /// <summary>
    /// 
    /// </summary>
    private Double _height;
    /// <summary>
    /// 
    /// </summary>
    private Double _frameRate;
    /// <summary>
    /// 
    /// </summary>
    private Double _videoDataRate;
    /// <summary>
    /// 
    /// </summary>
    private Double _audioDataRate;
    /// <summary>
    /// 
    /// </summary>
    private bool _hasMetaData;
    /// <summary>
    /// The duration in seconds of the video
    /// </summary>
    /// <value>The duration.</value>
    public Double Duration
    {
        get { return _duration; }
        //set { _duration = value; } 
    }
    /// <summary>
    /// The width in pixels of the video
    /// </summary>
    /// <value>The width.</value>
    public Double Width
    {
        get { return _width; }
        //set { _width = value; } 
    }
    /// <summary>
    /// The height in pixels of the video
    /// </summary>
    /// <value>The height.</value>
    public Double Height
    {
        get { return _height; }
        //set { _height = value; } 
    }
    /// <summary>
    /// The data rate in KB/sec of the video
    /// </summary>
    /// <value>The video data rate.</value>
    public Double VideoDataRate
    {
        get { return _videoDataRate; }
        //set { _videoDataRate = value; } 
    }
    /// <summary>
    /// The data rate in KB/sec of the video's audio track
    /// </summary>
    /// <value>The audio data rate.</value>
    public Double AudioDataRate
    {
        get { return _audioDataRate; }
        //set { _audioDataRate = value; } 
    }
    /// <summary>
    /// The frame rate of the video
    /// </summary>
    /// <value>The frame rate.</value>
    public Double FrameRate
    {
        get { return _frameRate; }
        //set { _frameRate = value; } 
    }
    /// <summary>
    /// Whether or not the FLV has meta data
    /// </summary>
    /// <value><c>true</c> if this instance has meta data; otherwise, <c>false</c>.</value>
    public bool HasMetaData
    {
        get { return _hasMetaData; }
        //set { _hasMetaData = value; } 
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="FlvMetaInfo" /> class.
    /// </summary>
    /// <param name="hasMetaData">if set to <c>true</c> [has meta data].</param>
    /// <param name="duration">The duration.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <param name="videoDataRate">The video data rate.</param>
    /// <param name="audioDataRate">The audio data rate.</param>
    /// <param name="frameRate">The frame rate.</param>
    internal FlvMetaInfo(bool hasMetaData, Double duration, Double width, Double height, Double videoDataRate, Double audioDataRate, Double frameRate)
    {
        _hasMetaData = hasMetaData;
        _duration = duration;
        _width = width;
        _height = height;
        _videoDataRate = videoDataRate;
        _audioDataRate = audioDataRate;
        _frameRate = frameRate;
    }
}
