using System;
using System.Web;
using System.IO;
using System.Web.Script.Serialization;
using System.Drawing;
using System.Drawing.Imaging;



public class FileHandler : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        try
        {
            if (context.Request.Params["ResearchID"] != null)
            {
                int intResearchID = 0;

                int.TryParse(context.Request.Params["ResearchID"].ToString(), out intResearchID);

                // put in checks for validity to post to this research ID

                if (intResearchID > 0)
                {

                    string strSavePath = clsPublic.GetProgramSetting("keyVideoUploadLocation");
                    string strSaveImagePath = clsPublic.GetProgramSetting("keyImageUploadLocation");

                    context.Response.ContentType = "text/plain";//"application/json";
                    var r = new System.Collections.Generic.List<ViewDataUploadFilesResult>();
                    JavaScriptSerializer js = new JavaScriptSerializer();

                    foreach (string file in context.Request.Files)
                    {
                        HttpPostedFile hpf = context.Request.Files[file] as HttpPostedFile;
                        string FileName = string.Empty;
                        if (HttpContext.Current.Request.Browser.Browser.ToUpper() == "IE")
                        {
                            string[] files = hpf.FileName.Split(new char[] { '\\' });
                            FileName = files[files.Length - 1];
                        }
                        else
                        {
                            FileName = hpf.FileName;
                        }
                        if (hpf.ContentLength == 0)
                            continue;

                        bool boolIsVideo = false;
                        bool boolIsValid = false;

                        string strExt = System.IO.Path.GetExtension(FileName);
                        int intFileType = 0;
                        string strFileType = string.Empty;

                        switch (strExt.ToUpper())
                        {
                            case ".PNG":
                            case ".JPG":
                            case ".GIF":
                                boolIsValid = true;
                                intFileType = 12;
                                strFileType = "Image File";

                                break;
                            case ".PDF":
                                boolIsValid = true;
                                intFileType = 12;
                                strFileType = "PDF File";

                                break;
                            case ".MOV":
                            case ".AVI":
                            case ".WMV":
                            case ".VOB":
                            case ".FLV":
                            case ".MP4":
                            case ".MPG":
                            case ".MPEG":
                                boolIsVideo = true;
                                boolIsValid = true;
                                intFileType = 13;
                                strFileType = "Video File";

                                break;

                            default:

                                break;
                        }

                        

                        if (boolIsValid)
                        {
                            string strHTTPLocation = string.Empty;

                            Questions qNew = null;

                            string strNewFile = string.Empty;
                            string strSaveFile = string.Empty;
                            string savedFileName = string.Empty;
                            string appPath = HttpContext.Current.Request.ApplicationPath;
                            string physicalPath = HttpContext.Current.Request.MapPath(appPath);
                            string strFullFile = string.Empty;
                            string strQuestionText = string.Empty;

                            if (boolIsVideo)
                            {
                                QuestionAdd qAdd = new QuestionAdd();

                                short intQuestionOrder = qAdd.GetQuestionOrder(intResearchID);

                                string strGUID = Guid.NewGuid().ToString();

                                string strVideoExt = clsPublic.GetProgramSetting("keyVideoExtension");
                                if (string.IsNullOrEmpty(strVideoExt)) strVideoExt = "flv";

                                strNewFile = string.Format("{0}.{1}", strGUID, strVideoExt.Replace(".",""));
                                strSaveFile =  string.Format("{0}.{1}", strGUID, strExt.Replace(".",""));
                                strFullFile = string.Format(@"{0}\Video\Research\{1}", physicalPath, strSaveFile);
                                string inputPath = string.Format(@"{0}", strFullFile);
                                hpf.SaveAs(strFullFile);
                                strFullFile = string.Format(@"{0}\Video\Research\{1}", physicalPath, strNewFile);
                                string outputPath = string.Format(@"{0}", strFullFile);

                                //qNew = qAdd.SaveVideoImage(intResearchID, FileName, strSaveFile, strExt, hpf.ContentLength.ToString());
                                qNew = qAdd.SaveVideoImage(intResearchID, FileName, strNewFile, strExt, hpf.ContentLength.ToString());

                                strQuestionText = string.Format("Processing Video <span class='vidUpdate' id='V{1}'>0</span>% - {0}", FileName, qNew.QuestionID );

                                
                                ConvertToFlv(context, physicalPath, inputPath, outputPath);
                                
                            }
                            else
                            {
                                QuestionAdd qAdd = new QuestionAdd();
                                bool isPdf = strExt.ToUpper().Equals(".PDF");
                                short intQuestionOrder = qAdd.GetQuestionOrder(intResearchID);

                                strNewFile = isPdf ? string.Format("{0}.pdf", Guid.NewGuid().ToString()) : string.Format("{0}.jpg", Guid.NewGuid().ToString());
                                strFullFile = string.Format(@"{0}\images\Research\{1}", physicalPath, strNewFile);

                                qNew = qAdd.SaveVideoImage(intResearchID, FileName, strNewFile, strExt, hpf.ContentLength.ToString());

                                if (isPdf)
                                    ResizePDFStream(hpf.InputStream, strFullFile);
                                else
                                    ResizeStream(hpf.InputStream, strFullFile);

                                string strtarget = isPdf ? "target='_blank'" : "";
                                strHTTPLocation = string.Format("{0}://{1}{2}/images/Research/{3}", context.Request.Url.Scheme, context.Request.ServerVariables["HTTP_HOST"], 
context.Request.ApplicationPath, strNewFile);
                                strQuestionText = string.Format("<a href='{0}' class='cbox' title='{1} - {2}' {3}>{1} - {2}</a>", strHTTPLocation, strFileType, FileName, strtarget);
                            }


                            if (qNew != null)
                            {

                                r.Add(new ViewDataUploadFilesResult()
                                {
                                    ResearchID = qNew.ResearchID,
                                    QuestionID = qNew.QuestionID,
                                    NewFileName = strNewFile,
                                    Name = FileName,
                                    Length = hpf.ContentLength,
                                    Type = intFileType,
                                    FileType = strFileType,
                                    QuestionText = strQuestionText,
                                    IsVideo = boolIsVideo,
                                    IsVideoComplete = false,
                                    VideoPercentComplete = 0
                                });

                                var uploadedFiles = new
                                {
                                    files = r.ToArray()
                                };

                                var jsonObj = js.Serialize(uploadedFiles);
                                context.Response.Write(jsonObj.ToString());
                            }
                            else
                            {
                                var jsonObj = js.Serialize("Could not write file.");
                                context.Response.Write(jsonObj.ToString());
                            }
                        }
                        else
                        {
                            var jsonObj = js.Serialize("Invalid File Type.");
                            context.Response.Write(jsonObj.ToString());
                        }
                    }
                }
            }
            else
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                var jsonObj = js.Serialize("Invalid Research ID.");
                context.Response.Write(jsonObj.ToString());
            }
        }
        catch (Exception ex)
        {
            clsPublic.LogError(ex);
            JavaScriptSerializer js = new JavaScriptSerializer();
            var jsonObj = js.Serialize("An error occurred.");
            context.Response.Write(jsonObj.ToString());

        }
    }    

    private void ConvertToFlv(HttpContext context, string AppPath, string inputPath, string outputPath)
    {//-ar 22050
        string cmd = " -i \"" + inputPath + "\" -ar 22050 \""  + outputPath + "\"";
        //string cmd = " -i \"" + inputPath + "\" \""  + outputPath + "\"";
        string filebuffer = "-U \"" + outputPath + "\\" + "\"";

        System.Diagnostics.Process proc = new System.Diagnostics.Process();
        proc.StartInfo.FileName = AppPath + "\\app_code\\ffmpeg.exe" ;
        //Path of exe that will be executed, only for "filebuffer" it will be "flvtool2.exe"
        proc.StartInfo.Arguments = cmd; //The command which will be executed
        proc.StartInfo.UseShellExecute = false;
        proc.StartInfo.CreateNoWindow = true;
        proc.StartInfo.RedirectStandardOutput = false;
        proc.Start();

    }

    protected void ResizeStream(Stream strm, string strFullFile)
    {
        try
        {
            // Image Resize
            //
            System.Drawing.Image imgInput = System.Drawing.Image.FromStream(strm);
            //Determine image format 
            ImageFormat fmtImageFormat = imgInput.RawFormat;

            //get image original width and height 
            int intOldWidth = imgInput.Width;
            int intOldHeight = imgInput.Height;

            //determine if landscape or portrait 
            int intMaxSide;

            if (intOldWidth >= intOldHeight)
            {
                intMaxSide = intOldWidth;
            }
            else
            {
                intMaxSide = intOldHeight;
            }

            int intNewWidth;
            int intNewHeight;
            int MaxSideSize = 600;

            if (intMaxSide > MaxSideSize)
            {
                //set new width and height 
                double dblCoef = MaxSideSize / (double)intMaxSide;
                intNewWidth = Convert.ToInt32(dblCoef * intOldWidth);
                intNewHeight = Convert.ToInt32(dblCoef * intOldHeight);
            }
            else
            {
                intNewWidth = intOldWidth;
                intNewHeight = intOldHeight;
            }
            //create new bitmap 
            Bitmap bmpResized = new Bitmap(imgInput, intNewWidth, intNewHeight);
imgInput.Dispose();
            //save bitmap to disk 
            bmpResized.Save(strFullFile, ImageFormat.Jpeg);

            //Encoder Enc = Encoder.Transformation;
            //EncoderParameters EncParms = new EncoderParameters(1);
            //EncoderParameter EncParm;
            //ImageCodecInfo CodecInfo = clsImage.GetEncoderInfo(validFile.ContentType);


            //release used resources 
            
            bmpResized.Dispose();
        }
        catch (Exception ex)
        {
            clsPublic.LogError(ex);
        }

    }

protected void ResizePDFStream(Stream strm, string strFullFile)
    {
        try
        {
            if (strm.Length == 0) return;

            // Create a FileStream object to write a stream to a file
            using (FileStream fileStream = System.IO.File.Create(strFullFile, (int)strm.Length))
            {
                // Fill the bytes[] array with the stream data
                byte[] bytesInStream = new byte[strm.Length];
                strm.Read(bytesInStream, 0, (int)bytesInStream.Length);

                // Use FileStream object to write to the specified file
                fileStream.Write(bytesInStream, 0, bytesInStream.Length);
            }
        }
        catch (Exception ex)
        {
            clsPublic.LogError(ex);
        }

    }

    public bool IsReusable
    {
        get
        {
            return true;
        }
    }

}


public class ViewDataUploadFilesResult
{
    public string Name { get; set; }
    public string NewFileName { get; set; }
    public int Length { get; set; }
    public int Type { get; set; }
    public int ResearchID { get; set; }
    public int QuestionID { get; set; }
    public string FileType { get; set; }
    public string QuestionText { get; set; }
    public bool IsVideo { get; set; }
    public double VideoPercentComplete { get; set; }
    public bool IsVideoComplete { get; set; }
}