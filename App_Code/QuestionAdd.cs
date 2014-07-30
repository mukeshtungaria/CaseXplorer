// ***********************************************************************
// Author           : Dennis Sebenick
// Created          : 10-13-2011
//
// Last Modified By : dennis
// Last Modified On : 05-21-2012
// ***********************************************************************
// <copyright file="QuestionAdd.cs" company="DGCC.COM">
//     . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;

using JuryData.Data;
using JuryData.Entities;


/// <summary>
/// Question Service Functions
/// </summary>
[WebService(Namespace = "https://www.casexplorer.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class QuestionAdd : System.Web.Services.WebService {

    /// <summary>
    /// Initializes a new instance of the <see cref="QuestionAdd" /> class.
    /// </summary>
    public QuestionAdd () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    /// <summary>
    /// Deletes a question by ID
    /// </summary>
    /// <param name="intQuestionID">Question ID.</param>
    /// <returns>Returns Question ID Deleted (System.String)</returns>
    [WebMethod]
    public string DeleteQuestion(int intQuestionID)
    {
        try
        {

            if (intQuestionID > 0)
            {
                DataRepository.ResearchQuestionsProvider.Delete(intQuestionID);
            }

            return intQuestionID.ToString();
        }
        catch (Exception ex)
        {
            clsPublic.LogError(ex);
            return string.Empty;
        }

    }

    /// <summary>
    /// Saves a video image.
    /// </summary>
    /// <param name="intResearchID">Research ID.</param>
    /// <param name="strOriginalFileName">Name of the original file.</param>
    /// <param name="strUniqueFileName">Name of the unique file.</param>
    /// <param name="strExt">File extension.</param>
    /// <param name="strFileSize">File size</param>
    /// <returns>Returns Question Object</returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public Questions SaveVideoImage(int intResearchID, string strOriginalFileName, string strUniqueFileName, string strExt, string strFileSize)
    {
        try
        {
            // Put check in for permission

            string strFileType = string.Empty;
            bool boolIsImage = false;
            bool boolIsVideo = false;

            if (!strExt.StartsWith(".")) strExt = string.Concat(".", strExt);

            switch (strExt.ToUpper())
            {
                case ".PNG":
                case ".JPG":
                case ".GIF":
                    boolIsImage = true;
                    strFileType = "IMAGE";
                    break;
                case ".PDF":
                    boolIsImage = true;
                    strFileType = "PDF";
                    break;
                case ".MOV":
                case ".AVI":
                case ".WMV":
                case ".VOB":
                case ".FLV":
                case ".MP4":
                case ".MPG":
                case ".MPEG":

                    strFileType = "VIDEO";
                    boolIsVideo = true;

                    break;

                default:

                    break;

            }

            string strQuestionText = string.Empty;
            string strPath = string.Empty;
            int intQuestionType = 0;

            if (boolIsVideo)
            {
                string strVideoPath = System.Configuration.ConfigurationManager.AppSettings["Host"] + clsPublic.GetProgramSetting("keyVideoResearchOutput");
                string strVideoExt = clsPublic.GetProgramSetting("keyVideoExtension");

                if (string.IsNullOrEmpty(strVideoExt)) strVideoExt = "flv";

                if (!strVideoPath.EndsWith(@"\"))
                {
                    strVideoPath = string.Concat(strVideoPath, @"\");
                }

                strQuestionText = string.Format("Video File - {0}", strOriginalFileName);
                intQuestionType = 13;
                strPath = strVideoPath;
            }
            else if (boolIsImage)
            {
                string strfilename = strExt.ToUpper().Equals(".PDF") ? "PDF File - " + strOriginalFileName : "Image File - " + strOriginalFileName;
                strQuestionText = string.Format("{0}", strfilename);
                intQuestionType = 12;
                strPath = strExt.ToUpper().Equals(".PDF") ?"../images/Research/" : "~/images/Research/";
            }

            Questions QuestionsNew = null;

            if (intQuestionType > 0)
            {
                QuestionsNew = SaveQuestion(intResearchID, 0, intQuestionType.ToString(), strQuestionText, 0, string.Empty);
                ResearchQuestionsFiles entResearchQuestionsFiles = new ResearchQuestionsFiles();

                entResearchQuestionsFiles.IntResearchFilesQuestionID = QuestionsNew.QuestionID;
                entResearchQuestionsFiles.StrResearchFilesPath = strPath;
                entResearchQuestionsFiles.StrResearchFilesUniqueFileName = strUniqueFileName;
                entResearchQuestionsFiles.StrResearchFilesName = strOriginalFileName;
                entResearchQuestionsFiles.StrResearchFilesType = strFileType;
                entResearchQuestionsFiles.StrResearchFilesFileDesc = strQuestionText;

                int intFileSize = 0;

                int.TryParse(strFileSize, out intFileSize);

                entResearchQuestionsFiles.IntResearchFilesSize = intFileSize;
                DataRepository.ResearchQuestionsFilesProvider.Save(entResearchQuestionsFiles);

            }

            return QuestionsNew;
        }
        catch (Exception ex)
        {
            clsPublic.LogError(ex);
            return new Questions();
        }
    }

    /// <summary>
    /// Gets the video complete percent.
    /// </summary>
    /// <param name="intQuestionID">Question ID.</param>
    /// <returns>PercentCompleteReturn Object</returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public PercentCompleteReturn GetVidCompletePercent(int intQuestionID)
    {
        try
        {

            TList<ResearchVideoProcessed> tlstResearchVideoProcessed = DataRepository.ResearchVideoProcessedProvider.GetByIntVidFileQuestionID(intQuestionID);

            double dblPercentage = 0;
            PercentCompleteReturn returnValue = new PercentCompleteReturn();

            if (tlstResearchVideoProcessed != null)
            {
                if (tlstResearchVideoProcessed.Count > 0)
                {
                    double.TryParse(tlstResearchVideoProcessed[0].DblPercentComplete.ToString(), out dblPercentage);
                }
            }

            string strDesc = string.Empty;

            if (dblPercentage > 100) dblPercentage = 100;

            if (dblPercentage == 100)
            {
                ResearchQuestionsFiles entResearchQuestionsFiles = DataRepository.ResearchQuestionsFilesProvider.GetByIntResearchFilesQuestionID(intQuestionID);
                ResearchQuestions entResearchQuestions = DataRepository.ResearchQuestionsProvider.GetByAutoQuesID(intQuestionID);

                if (entResearchQuestionsFiles != null)
                {
                    string strVideoPath = clsPublic.GetProgramSetting("keyHTTPVideoPath");
                    string strActualVideoPath = clsPublic.GetProgramSetting("keyVideoResearchOutput");

                    if (!strActualVideoPath.EndsWith(@"\"))
                    {
                        strActualVideoPath = string.Concat(strActualVideoPath, @"\");
                    }

                    strActualVideoPath = string.Concat(strActualVideoPath, entResearchQuestionsFiles.StrResearchFilesUniqueFileName);

                    strDesc = string.Format("<a href='{0}{1}' class='cboxiframe cboxElement' title='{2}'>{3}</a>", strVideoPath, entResearchQuestionsFiles.StrResearchFilesUniqueFileName, entResearchQuestions.StrQuesText, entResearchQuestions.StrQuesText);

                    if (strDesc.Length == 0)
                    {
                        strDesc = "Video Processed - Click refresh to show link";
                    }
                }
            }

            returnValue.Percentage = dblPercentage;
            returnValue.QuestionID = intQuestionID;
            returnValue.QuestionDesc = strDesc;

            return returnValue;
        }
        catch (Exception ex)
        {
            clsPublic.LogError(ex);
            return new PercentCompleteReturn();
        }
    }

    /// <summary>
    /// Saves question to database
    /// </summary>
    /// <param name="intResearchID">Research ID.</param>
    /// <param name="intQuestionID">Question ID.</param>
    /// <param name="strQuestionType">Question Type</param>
    /// <param name="strQuestionText">Question Text</param>
    /// <param name="intQuestionOrder">Question Order</param>
    /// <param name="strQuestionResponses">Question Responses (delimited)</param>
    /// <returns>Questions Object</returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public Questions SaveQuestion(int intResearchID, int intQuestionID, string strQuestionType, string strQuestionText, short intQuestionOrder, string strQuestionResponses) 
    {
        try
        {
            ResearchQuestions entResearchQuestions = null;

            if (intQuestionID > 0)
            {
                entResearchQuestions = DataRepository.ResearchQuestionsProvider.GetByAutoQuesID(intQuestionID);
            }
            else
            {
                intQuestionOrder = GetQuestionOrder(intResearchID);

                entResearchQuestions = new ResearchQuestions();
                entResearchQuestions.IntQuesOrder = intQuestionOrder;
                entResearchQuestions.IntQuesResearchID = intResearchID;
            }

            short intQuestionType = 0;

            short.TryParse(strQuestionType, out intQuestionType);

            entResearchQuestions.StrQuesText = strQuestionText;
            entResearchQuestions.IntQuesType = intQuestionType;

            if (entResearchQuestions.IsDirty)
            {
                DataRepository.ResearchQuestionsProvider.Save(entResearchQuestions);
            }

            if (strQuestionResponses.Length > 0)
            {
                TList<ResearchQuestionsResponses> tlstResearchQuestionsResponses = DataRepository.ResearchQuestionsResponsesProvider.GetByIntQuesRespQuesID(entResearchQuestions.AutoQuesID);

                tlstResearchQuestionsResponses.ForEach(delegate(ResearchQuestionsResponses entResearchQuestionsResponses)
                {
                    DataRepository.ResearchQuestionsResponsesProvider.Delete(entResearchQuestionsResponses);
                });

                string[] strResponses = strQuestionResponses.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string str in strResponses)
                {
                    ResearchQuestionsResponses entResponses = new ResearchQuestionsResponses();
                    entResponses.IntQuesRespQuesID = entResearchQuestions.AutoQuesID;
                    entResponses.StrQuesRespText = str;

                    DataRepository.ResearchQuestionsResponsesProvider.Save(entResponses);
                }
            }

            return QuestionItemFromEntity(entResearchQuestions, false);
        }
        catch (Exception ex)
        {
            clsPublic.LogError(ex);
            return new Questions();
        }
    }

    /// <summary>
    /// Populates a question object from a ResearchQuestion data entity
    /// </summary>
    /// <param name="entResearchQuestions">ResearchQuestion data entity</param>
    /// <param name="boolIsVideo">if set to <c>true</c> [bool is video].</param>
    /// <returns>Questions Object</returns>
    private Questions QuestionItemFromEntity(ResearchQuestions entResearchQuestions, bool boolIsVideo)
    {
        Questions questionItem = new Questions();


        questionItem.QuestionOrder = entResearchQuestions.IntQuesOrder;
        questionItem.QuestionID = entResearchQuestions.AutoQuesID;
        questionItem.QuestionDescription = entResearchQuestions.StrQuesText;
        questionItem.ResearchID = entResearchQuestions.IntQuesResearchID;
        questionItem.QuestionType = entResearchQuestions.IntQuesType.ToString();
        questionItem.IsVideo = boolIsVideo;

        return questionItem;
    }


    /// <summary>
    /// Moves a question up or down in the order
    /// </summary>
    /// <param name="intQuestionID">Question ID.</param>
    /// <param name="intDown">Move Down (set to 1) all others move up.</param>
    /// <returns>QuestionMoved Object</returns>
    [WebMethod]
    public QuestionMoved MoveQuestion(int intQuestionID, int intDown)
    {
        QuestionMoved _QuestionMoved = new QuestionMoved();
        _QuestionMoved.intQuestionID = intQuestionID;
        _QuestionMoved.intMoveDown = intDown;


        if (intDown == 1)
            MoveQuestionDown(intQuestionID);
        else
            MoveQuestionUp(intQuestionID);

        return _QuestionMoved;
    }

    /// <summary>
    /// Moves a question up.
    /// </summary>
    /// <param name="intQuestionID">Question ID.</param>
    private void MoveQuestionUp(int intQuestionID)
    {
        DbCommand cmd = clsUtilities.GetDBCommand("_MoveQuestionV2");

        clsUtilities.AddParameter(ref cmd, "@intQuestionID", ParameterDirection.Input, DbType.Int32, intQuestionID);
        clsUtilities.AddParameter(ref cmd, "@bitMoveDown", ParameterDirection.Input, DbType.Boolean, 1);
        clsUtilities.AddParameter(ref cmd, "@bitMoveUp", ParameterDirection.Input, DbType.Boolean, 0);

        clsUtilities.SQLExecuteNonQuery(cmd);
    }

    /// <summary>
    /// Moves a question down.
    /// </summary>
    /// <param name="intQuestionID">Question ID.</param>
    private void MoveQuestionDown(int intQuestionID)
    {
        DbCommand cmd = clsUtilities.GetDBCommand("_MoveQuestionV2");

        clsUtilities.AddParameter(ref cmd, "@intQuestionID", ParameterDirection.Input, DbType.Int32, intQuestionID);
        clsUtilities.AddParameter(ref cmd, "@bitMoveDown", ParameterDirection.Input, DbType.Boolean, 0);
        clsUtilities.AddParameter(ref cmd, "@bitMoveUp", ParameterDirection.Input, DbType.Boolean, 1);

        clsUtilities.SQLExecuteNonQuery(cmd);
    }

    /// <summary>
    /// Gets the question by ID.
    /// </summary>
    /// <param name="intQuestionID">Question ID.</param>
    /// <returns>Questions Object</returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public Questions GetQuestionByID(int intQuestionID)
    {
        try
        {
            ResearchQuestions entResearchQuestion = DataRepository.ResearchQuestionsProvider.GetByAutoQuesID(intQuestionID);

            Questions questionItem = new Questions();

            if (entResearchQuestion != null)
            {
                questionItem.ResearchID = entResearchQuestion.IntQuesResearchID;
                questionItem.QuestionOrder = entResearchQuestion.IntQuesOrder;
                questionItem.QuestionType = entResearchQuestion.IntQuesType.ToString();
                questionItem.QuestionDescription = entResearchQuestion.StrQuesText;
                questionItem.QuestionID = entResearchQuestion.AutoQuesID;

                TList<ResearchQuestionsResponses> tlstResearchQuestionsResponses = DataRepository.ResearchQuestionsResponsesProvider.GetByIntQuesRespQuesID(entResearchQuestion.AutoQuesID);

                string strResponses = string.Empty;

                tlstResearchQuestionsResponses.ForEach(delegate(ResearchQuestionsResponses entResearchQuestionsResponses)
                {
                    strResponses = string.Concat(strResponses, entResearchQuestionsResponses.StrQuesRespText, '\n');
                });

                questionItem.QuestionResponses = strResponses;
            }
            else if (intQuestionID == -1)
            {
                questionItem.QuestionType = "10";
            }
            else
            {
                questionItem.QuestionType = "0";
            }

            return questionItem;
        }
        catch (Exception ex)
        {
            clsPublic.LogError(ex);
            return new Questions();
        }
    }

    /// <summary>
    /// Gets the questions.
    /// </summary>
    /// <param name="intResearchID">Research ID.</param>
    /// <returns>Returns array of Questions objects</returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public Questions[] GetQuestions(int intResearchID)
    {
        try
        {
            List<Questions> list = new List<Questions>();

            TList<ResearchQuestions> tlstResearchQuestions = DataRepository.ResearchQuestionsProvider.GetByIntQuesResearchID(intResearchID); //,intPage * this.GridView1.PageSize , this.GridView1.PageSize +1,  out intResult);

            tlstResearchQuestions.Sort(ResearchQuestionsColumn.IntQuesOrder.ToString());

            tlstResearchQuestions.ForEach(delegate(ResearchQuestions entResearchQuestions)
            {
                Questions _questions = new Questions();
                _questions.ResearchID = entResearchQuestions.IntQuesResearchID;
                _questions.QuestionDescription = entResearchQuestions.StrQuesText;
                _questions.QuestionID = entResearchQuestions.AutoQuesID;
                _questions.QuestionType = entResearchQuestions.IntQuesType.ToString();
                _questions.QuestionOrder = entResearchQuestions.IntQuesOrder;

                if (_questions.QuestionType == "12" || _questions.QuestionType == "13")
                {
                    ResearchQuestionsFiles entResearchQuestionsFiles = DataRepository.ResearchQuestionsFilesProvider.GetByIntResearchFilesQuestionID(_questions.QuestionID);

                    if (entResearchQuestionsFiles != null)
                    {
                        string strPathTemp = entResearchQuestionsFiles.StrResearchFilesPath.Replace("~", "");

                        string webApplicationRootUrl = string.Format("{0}://{1}{2}{3}{4}", this.Context.Request.Url.Scheme, this.Context.Request.ServerVariables["HTTP_HOST"], this.Context.Request.ApplicationPath, strPathTemp, entResearchQuestionsFiles.StrResearchFilesUniqueFileName);

                        _questions.FileLocation = webApplicationRootUrl;

                        if (_questions.QuestionType == "12")
                            _questions.QuestionDescription = string.Format("<a href='{0}' class='cbox' title='{1}'>{2}</a>", webApplicationRootUrl, _questions.QuestionDescription, _questions.QuestionDescription);
                        else
                        {
                            string strVideoPath = clsPublic.GetProgramSetting("keyHTTPVideoPath");
                            string strActualVideoPath = clsPublic.GetProgramSetting("keyVideoResearchOutput");

                            if (!strActualVideoPath.EndsWith(@"\"))
                            {
                                strActualVideoPath = string.Concat(strActualVideoPath, @"\");
                            }

                            _questions.IsVideo = true;

                            strActualVideoPath = string.Concat(strActualVideoPath, entResearchQuestionsFiles.StrResearchFilesUniqueFileName);

                            if (System.IO.File.Exists(strActualVideoPath))
                            {
                                _questions.IsVideoComplete = true;
                                _questions.VideoPercentComplete = 100;

                                _questions.QuestionDescription = string.Format("<a href='{0}{1}' class='cboxiframe cboxElement' title='{2}'>{3}</a>", strVideoPath, entResearchQuestionsFiles.StrResearchFilesUniqueFileName, _questions.QuestionDescription, _questions.QuestionDescription);
                            }
                            else
                            {
                                TList<ResearchVideoProcessed> tlstResearchVideoProcessed = DataRepository.ResearchVideoProcessedProvider.Find(string.Format("{0}={1}", ResearchVideoProcessedColumn.IntVidFileQuestionID.ToString(), entResearchQuestionsFiles.IntResearchFilesQuestionID));

                                double dblPercentage = 0;

                                if (tlstResearchVideoProcessed != null)
                                {
                                    if (tlstResearchVideoProcessed.Count > 0)
                                    {
                                        double.TryParse(tlstResearchVideoProcessed[0].DblPercentComplete.ToString(), out dblPercentage);

                                        _questions.QuestionDescription = string.Format("Processing Video <span class='vidUpdate' id='V{2}'>{1}</span>% - {0}", entResearchQuestionsFiles.StrResearchFilesName, dblPercentage, entResearchQuestionsFiles.IntResearchFilesQuestionID);
                                        _questions.VideoPercentComplete = dblPercentage;
                                    }
                                    else
                                    {
                                        _questions.QuestionDescription = string.Format("Processing Video <span class='vidUpdate' id='V{1}'>(Waiting)</span>% - {0}", entResearchQuestionsFiles.StrResearchFilesName, entResearchQuestionsFiles.IntResearchFilesQuestionID);
                                    }
                                }
                                else
                                {
                                    _questions.QuestionDescription = string.Format("Processing Video <span class='vidUpdate' id='V{1}'>(Waiting)</span>% - {0}", entResearchQuestionsFiles.StrResearchFilesName, entResearchQuestionsFiles.IntResearchFilesQuestionID);
                                }
                            }
                        }
                    }
                }

                list.Add(_questions);

            });

            return list.ToArray();
        }
        catch (Exception ex)
        {
            clsPublic.LogError(ex);
            List<Questions> list = new List<Questions>();
            return list.ToArray();
        }
    }

    /// <summary>
    /// Gets the question order.
    /// </summary>
    /// <param name="intResearchID">Research ID.</param>
    /// <returns>Returns Order (System.Int16)</returns>
    public short GetQuestionOrder(int intResearchID)
    {
        try
        {
            DbCommand cmd = clsUtilities.GetDBCommand("_up_paramsel_getLastQuestionOrder");

            clsUtilities.AddParameter(ref cmd, "@ResearchID", ParameterDirection.Input, DbType.Int32, intResearchID);
            clsUtilities.AddParameter(ref cmd, "@Return", ParameterDirection.ReturnValue, DbType.Int32, 0);

            clsUtilities.SQLExecuteNonQuery(cmd);

            short intVal = 0;

            short.TryParse(cmd.Parameters["@Return"].Value.ToString(), out intVal);

            return intVal;
        }
        catch (Exception ex)
        {
            clsPublic.LogError(ex);
            return -1;
        }
    }
}

/// <summary>
/// AJAX / json Question Moved object
/// </summary>
public class QuestionMoved
{
    /// <summary>
    /// 
    /// </summary>
    public int intQuestionID = 0;
    /// <summary>
    /// 
    /// </summary>
    public int intMoveDown = 0;
}

/// <summary>
/// AJAX / json return object for percent complete
/// </summary>
public class PercentCompleteReturn
{
    /// <summary>
    /// Gets or sets the question ID.
    /// </summary>
    /// <value>The question ID.</value>
    public int QuestionID { get; set; }
    /// <summary>
    /// Gets or sets the percentage.
    /// </summary>
    /// <value>The percentage.</value>
    public double Percentage { get; set; }
    /// <summary>
    /// Gets or sets the question desc.
    /// </summary>
    /// <value>The question desc.</value>
    public string QuestionDesc { get; set; }
}

/// <summary>
/// AJAX / json return object for promo codes
/// </summary>
public class PromoCodes
{
    /// <summary>
    /// Gets or sets the promo ID.
    /// </summary>
    /// <value>The promo ID.</value>
    public int PromoID { get; set; }
    /// <summary>
    /// Gets or sets the promo desc.
    /// </summary>
    /// <value>The promo desc.</value>
    public string PromoDesc { get; set; }
    /// <summary>
    /// Gets or sets the promo code.
    /// </summary>
    /// <value>The promo code.</value>
    public string PromoCode { get; set; }
    /// <summary>
    /// Gets or sets the type of the promo.
    /// </summary>
    /// <value>The type of the promo.</value>
    public int PromoType { get; set; }
    /// <summary>
    /// Gets or sets the promo unit.
    /// </summary>
    /// <value>The promo unit.</value>
    public int? PromoUnit { get; set; }
    /// <summary>
    /// Gets or sets the promo expire.
    /// </summary>
    /// <value>The promo expire.</value>
    public DateTime PromoExpire { get; set; }
    /// <summary>
    /// Gets or sets the promo amount.
    /// </summary>
    /// <value>The promo amount.</value>
    public decimal PromoAmount { get; set; }
    /// <summary>
    /// Gets or sets the total original.
    /// </summary>
    /// <value>The total original.</value>
    public decimal TotalOriginal { get; set; }
    /// <summary>
    /// Gets or sets the total off.
    /// </summary>
    /// <value>The total off.</value>
    public decimal TotalOff { get; set; }
    /// <summary>
    /// Gets or sets the finger print.
    /// </summary>
    /// <value>The finger print.</value>
    public string FingerPrint { get; set; }
    /// <summary>
    /// Gets or sets the seq.
    /// </summary>
    /// <value>The seq.</value>
    public string Seq { get; set; }
    /// <summary>
    /// Gets or sets the stamp.
    /// </summary>
    /// <value>The stamp.</value>
    public string Stamp { get; set; }


    /// <summary>
    /// Gets the new total.
    /// </summary>
    /// <value>The new total.</value>
    public decimal NewTotal {
        get
        {
            return this.TotalOriginal - this.TotalOff;
        }
    }
}


/// <summary>
/// AJAX / json Question Object
/// </summary>
public class Questions
{
    /// <summary>
    /// 
    /// </summary>
    private string m_strQuestionTypeString = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    private string m_strQuestionType = string.Empty;

    /// <summary>
    /// Gets or sets the research ID.
    /// </summary>
    /// <value>The research ID.</value>
    public int ResearchID { get; set; }
    /// <summary>
    /// Gets or sets the question ID.
    /// </summary>
    /// <value>The question ID.</value>
    public int QuestionID { get; set; }
    /// <summary>
    /// Gets or sets the file location.
    /// </summary>
    /// <value>The file location.</value>
    public string FileLocation { get; set; }
    /// <summary>
    /// Gets or sets the question description.
    /// </summary>
    /// <value>The question description.</value>
    public string QuestionDescription  { get; set; }
    /// <summary>
    /// Gets or sets the question responses.
    /// </summary>
    /// <value>The question responses.</value>
    public string QuestionResponses  { get; set; }
    /// <summary>
    /// Gets or sets the question order.
    /// </summary>
    /// <value>The question order.</value>
    public int QuestionOrder  { get; set; }
    /// <summary>
    /// Gets the question type string.
    /// </summary>
    /// <value>The question type string.</value>
    public string QuestionTypeString { 
        get { return m_strQuestionTypeString; } 
        set { m_strQuestionTypeString = value; } 
    }
    /// <summary>
    /// Gets or sets a value indicating whether this instance is video.
    /// </summary>
    /// <value><c>true</c> if this instance is video; otherwise, <c>false</c>.</value>
    public bool IsVideo { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether this instance is video complete.
    /// </summary>
    /// <value><c>true</c> if this instance is video complete; otherwise, <c>false</c>.</value>
    public bool IsVideoComplete { get; set; }
    /// <summary>
    /// Gets or sets the video percent complete.
    /// </summary>
    /// <value>The video percent complete.</value>
    public double VideoPercentComplete { get; set; }

    /// <summary>
    /// Gets or sets the type of the question.
    /// </summary>
    /// <value>The type of the question.</value>
    public string QuestionType
    {
        set 
        {
            switch (value)
            {
                case "1" :
                    m_strQuestionTypeString = "Open Ended Question";
                    break;

                case "2" :
                    m_strQuestionTypeString = "Yes/No Question";
                    break;

                case "3" :
                    //m_strQuestionTypeString = "Scale Response Question";
                    m_strQuestionTypeString = "Agree/Disagree Response Question";
                    break;

                case "4" :
                    m_strQuestionTypeString = "Single Response Question";
                    break;

                case "5" :
                    m_strQuestionTypeString = "Multiple Response Question";
                    //m_strQuestionTypeString = "Scale Question";
                    break;

                case "10" :
                    m_strQuestionTypeString = "Instruction Text";

                    break;

                case "11" :
                    m_strQuestionTypeString = "Start New Web Page";
                    break;

                case "12" :
                    m_strQuestionTypeString = "Image File";
                    break;

                case "13" :
                    m_strQuestionTypeString = "Video File";
                    break;


            }
        
            m_strQuestionType = value; 
        }
        get { return m_strQuestionType; }
    }


}
