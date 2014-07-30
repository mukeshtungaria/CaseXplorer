// ***********************************************************************
// Author           : Dennis Sebenick
// Created          : 10-27-2011
//
// Last Modified By : dennis
// Last Modified On : 08-30-2012
// ***********************************************************************
// <copyright file="QuestionService.cs" company="DGCC.COM">
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
/// Question Services
/// </summary>
[WebService(Namespace = "http://dgcc.com/services")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class QuestionService : System.Web.Services.WebService {

    /// <summary>
    /// Initializes a new instance of the <see cref="QuestionService" /> class.
    /// </summary>
    public QuestionService () 
    {
        //
    }

    /// <summary>
    /// Deletes a question.
    /// </summary>
    /// <param name="intQuestionID">Question ID.</param>
    /// <returns>Returns Question ID (System.String)</returns>
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
    /// Saves a video / image.
    /// </summary>
    /// <param name="intResearchID">Research ID.</param>
    /// <param name="strOriginalFileName">Name of the original file.</param>
    /// <param name="strUniqueFileName">Name of the unique file.</param>
    /// <param name="strExt">Filename Extension</param>
    /// <param name="strFileSize">Size of the file.</param>
    /// <returns>Questions Object</returns>
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
                string strVideoPath = clsPublic.GetProgramSetting("keyVideoResearchOutput");
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
                strQuestionText = string.Format("Image File - {0}", strOriginalFileName);
                intQuestionType = 12;
                strPath = "~/images/Research/";
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
    /// Gets question help text
    /// </summary>
    /// <param name="intQuestionType">Type of the int question.</param>
    /// <returns>Help Text (System.String)</returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetQuestionHelp(int intQuestionType)
    {
        try
        {
            string strPageTitle = string.Empty;
            string strPageLabel = string.Empty;

            string strReturn = string.Empty;

            strReturn = clsPublic.PageText("QUESTTUTORIALMAIN", intQuestionType, ref strPageTitle, ref strPageLabel);

            return strReturn;

        }
        catch (Exception ex)
        {
            clsPublic.LogError(ex);
            return string.Empty;
        }

    }

    /// <summary>
    /// Gets the vid complete percent.
    /// </summary>
    /// <param name="intQuestionID">Question ID.</param>
    /// <returns>PercentCompleteReturn object</returns>
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

            if (dblPercentage > 100) 
                dblPercentage = 100;

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
    /// Select a sample question.
    /// </summary>
    /// <param name="intSampleQuestionID">Sample question ID.</param>
    /// <param name="intResearchID">Research ID.</param>
    /// <returns>Questions Object</returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public Questions AddSampleQuestion(int intSampleQuestionID, int intResearchID)
    {
        try
        {
            System.Data.Common.DbCommand cmd = clsUtilities.GetDBCommand("_up_paramins_AddSampleQuestion");
            clsUtilities.AddParameter(ref cmd, "@ResearchID", ParameterDirection.Input, DbType.Int32, intResearchID);
            clsUtilities.AddParameter(ref cmd, "@SampleQuestionID", ParameterDirection.Input, DbType.Int32, intSampleQuestionID);
            clsUtilities.AddParameter(ref cmd, "@ReturnValue", ParameterDirection.ReturnValue, DbType.Int32, 0);

            clsUtilities.SQLExecuteNonQuery(cmd);

            int intNewQuestionID = (int) cmd.Parameters["@ReturnValue"].Value;


            Questions quesReturn = new Questions();

            ResearchQuestions entResearchQuestions = DataRepository.ResearchQuestionsProvider.GetByAutoQuesID(intNewQuestionID);

            quesReturn.QuestionDescription = entResearchQuestions.StrQuesText;
            quesReturn.QuestionID = entResearchQuestions.AutoQuesID;
            quesReturn.QuestionType =  entResearchQuestions.IntQuesType.ToString();
            quesReturn.QuestionOrder = entResearchQuestions.IntQuesOrder;

            return quesReturn;

        }
        catch (Exception ex)
        {
            clsPublic.LogError(ex);
            return new Questions();
        }
    }

    /// <summary>
    /// Saves a question.
    /// </summary>
    /// <param name="intResearchID">Research ID.</param>
    /// <param name="intQuestionID">Question ID.</param>
    /// <param name="strQuestionType">Question Type</param>
    /// <param name="strQuestionText">Question text.</param>
    /// <param name="intQuestionOrder">Question Order.</param>
    /// <param name="strQuestionResponses">Question Responses (delimited)</param>
    /// <returns>Questions Object</returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public Questions SaveQuestion(int intResearchID, int intQuestionID, string strQuestionType, string strQuestionText, short intQuestionOrder, string strQuestionResponses)
    {
        try
        {
            strQuestionText = strQuestionText.Replace(Environment.NewLine, "").Replace("\n", "").Replace("\r", "").Replace("<div>","").Replace("</div>","").Replace("~~", "'");
            strQuestionResponses = strQuestionResponses.Replace("~~", "'");

            ResearchQuestions entResearchQuestions = null;

            if (intQuestionID > 0)
            {
                entResearchQuestions = DataRepository.ResearchQuestionsProvider.GetByAutoQuesID(intQuestionID);
            }
            else
            {
                //intQuestionOrder = GetQuestionOrder(intResearchID);

                entResearchQuestions = new ResearchQuestions();
                entResearchQuestions.IntQuesOrder = intQuestionOrder;
                entResearchQuestions.IntQuesResearchID = intResearchID;
            }

            short intQuestionType = 0;

            short.TryParse(strQuestionType, out intQuestionType);

            entResearchQuestions.IntQuesOrder = strQuestionType == "11" ? GetQuestionOrder(intResearchID) : intQuestionOrder;
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
    public QuestionMoved MoveQuestion(int intQuestionID, int intDown, int MoveIndex)
    {
        QuestionMoved _QuestionMoved = new QuestionMoved();
        _QuestionMoved.intQuestionID = intQuestionID;
        _QuestionMoved.intMoveDown = intDown;

        MoveQuestionUp(intQuestionID, MoveIndex, intDown == 1 ? 0 : 1);
        //if (intDown == 1)
        //    MoveQuestionDown(intQuestionID);
        //else
        //    MoveQuestionUp(intQuestionID);

        return _QuestionMoved;
    }

    /// <summary>
    /// Moves a question up.
    /// </summary>
    /// <param name="intQuestionID">Question ID.</param>
    private void MoveQuestionUp(int intQuestionID, int MoveIndex, int MoveType)
    {
        DbCommand cmd = clsUtilities.GetDBCommand("_MoveQuestionV2_Revised");

        clsUtilities.AddParameter(ref cmd, "@intQuestionID", ParameterDirection.Input, DbType.Int32, intQuestionID);
        clsUtilities.AddParameter(ref cmd, "@bitMove", ParameterDirection.Input, DbType.Boolean, MoveType);
        clsUtilities.AddParameter(ref cmd, "@bitMoveIndex", ParameterDirection.Input, DbType.Int32, MoveIndex);

        clsUtilities.SQLExecuteNonQuery(cmd);
    }

    /// <summary>
    /// Moves a question down.
    /// </summary>
    /// <param name="intQuestionID">Question ID.</param>
    private void MoveQuestionDown(int intQuestionID, int MoveIndex)
    {
        DbCommand cmd = clsUtilities.GetDBCommand("_MoveQuestionV2_Revised"); //_MoveQuestionV2

        clsUtilities.AddParameter(ref cmd, "@intQuestionID", ParameterDirection.Input, DbType.Int32, intQuestionID);
        clsUtilities.AddParameter(ref cmd, "@bitMove", ParameterDirection.Input, DbType.Boolean, 0);
        clsUtilities.AddParameter(ref cmd, "@bitMoveIndex", ParameterDirection.Input, DbType.Int32, MoveIndex);

        clsUtilities.SQLExecuteNonQuery(cmd);
    }

    /// <summary>
    /// Gets a question by ID.
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
    /// AJAX / json SampleQuestionCategory Object
    /// </summary>
    public class SampleQuestionCategory
    {
        /// <summary>
        /// Gets or sets the category ID.
        /// </summary>
        /// <value>The category ID.</value>
        public int CategoryID { get; set; }
        /// <summary>
        /// Gets or sets the category desc.
        /// </summary>
        /// <value>The category desc.</value>
        public string CategoryDesc { get; set; }
    }

    /// <summary>
    /// AJAX / json SampleQuestion Object
    /// </summary>
    public class SampleQuestion
    {
        /// <summary>
        /// Gets or sets the category ID.
        /// </summary>
        /// <value>The category ID.</value>
        public int CategoryID { get; set;}
        /// <summary>
        /// Gets or sets the question ID.
        /// </summary>
        /// <value>The question ID.</value>
        public int QuestionID { get; set; }
        /// <summary>
        /// Gets or sets the question text.
        /// </summary>
        /// <value>The question text.</value>
        public string QuestionText { get; set; }
        /// <summary>
        /// 
        /// </summary>
        private string m_strQuestionTypeString = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        private string m_strQuestionType = string.Empty;
        /// <summary>
        /// Gets the question type string.
        /// </summary>
        /// <value>The question type string.</value>
        public string QuestionTypeString { get { return m_strQuestionTypeString; } }

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
                    case "1":
                        m_strQuestionTypeString = "Open Ended Question";
                        break;

                    case "2":
                        m_strQuestionTypeString = "Yes/No Question";
                        break;

                    case "3":
                        //m_strQuestionTypeString = "Scale Response Question";
                        m_strQuestionTypeString = "Agree/Disagree Response Question";
                        break;

                    case "4":
                        m_strQuestionTypeString = "Single Response Question";
                        break;

                    case "5":
                        m_strQuestionTypeString = "Multiple Response Question";
                        //m_strQuestionTypeString = "Scale Question";
                        break;

                    case "10":
                        m_strQuestionTypeString = "Instruction Text";

                        break;

                    case "11":
                        m_strQuestionTypeString = "Start New Web Page";
                        break;

                    case "12":
                        m_strQuestionTypeString = "Image File";
                        break;

                    case "13":
                        m_strQuestionTypeString = "Video File";
                        break;


                }

                m_strQuestionType = value;
            }
            get { return m_strQuestionType; }
        }

    }

    /// <summary>
    /// Gets sample question list
    /// </summary>
    /// <param name="intCategory">Sample Question Category</param>
    /// <returns>Array of SampleQuestion Objects</returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public SampleQuestion[] GetSampleQuestions(short intCategory)
    {
        try
        {
            List<SampleQuestion> list = new List<SampleQuestion>();
            TList<ResearchSampleQuestions> tlstResearchSampleQuestions = DataRepository.ResearchSampleQuestionsProvider.GetByIntSampleQuesCategory(intCategory);

            tlstResearchSampleQuestions.ForEach(delegate(ResearchSampleQuestions entResearchSampleQuestions)
            {
                SampleQuestion QuesCat = new SampleQuestion();
                QuesCat.CategoryID = entResearchSampleQuestions.IntSampleQuesCategory;
                QuesCat.QuestionID = entResearchSampleQuestions.AutoSampleQuesID;
                QuesCat.QuestionText = entResearchSampleQuestions.StrSampleQuesText;
                QuesCat.QuestionType = entResearchSampleQuestions.IntSampleQuesType.ToString();

                list.Add(QuesCat);
            });

            //list.Sort();
            return list.ToArray();

        }
        catch (Exception ex)
        {
            clsPublic.LogError(ex);
            List<SampleQuestion> list = new List<SampleQuestion>();
            return list.ToArray();

        }
    }


    /// <summary>
    /// Gets the sample question categories.
    /// </summary>
    /// <returns>Returns SampleQuestionCategory object array</returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public SampleQuestionCategory[] GetSampleQuestionCategories()
    {
        try
        {
            List<SampleQuestionCategory> list = new List<SampleQuestionCategory>();
            TList<DropValuesJB> tlstDropValuesJB = clsData.GetDropValuesJBDataByType(15);

            tlstDropValuesJB.Sort(DropValuesJBColumn.StrDropValueDescription.ToString());

            tlstDropValuesJB.ForEach(delegate(DropValuesJB entDropValuesJB)
            {
                SampleQuestionCategory QuesCat = new SampleQuestionCategory();
                QuesCat.CategoryDesc = entDropValuesJB.StrDropValueDescription;
                QuesCat.CategoryID = entDropValuesJB.AutoDropValueID;

                list.Add(QuesCat);

            });

            return list.ToArray();


        }
        catch (Exception ex)
        {
            clsPublic.LogError(ex);

            List<SampleQuestionCategory> list = new List<SampleQuestionCategory>();
            return list.ToArray();
        }
    }

    /// <summary>
    /// Gets a list of questions from a study
    /// </summary>
    /// <param name="intResearchID">Research ID.</param>
    /// <returns>returns Questions object array</returns>
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
{
                            if (entResearchQuestionsFiles.StrResearchFilesType.ToUpper().Equals("PDF"))
                            {
                                _questions.QuestionTypeString = "PDF File";
                                _questions.QuestionDescription = string.Format("<a href='{0}' target='_blank' title='{1}'>{2}</a>", webApplicationRootUrl, _questions.QuestionDescription, 
_questions.QuestionDescription);
                            }
                            else
                                _questions.QuestionDescription = string.Format("<a href='{0}' class='cbox' title='{1}'>{2}</a>", webApplicationRootUrl, _questions.QuestionDescription, 
_questions.QuestionDescription);
}
                        else
                        {
                            string strVideoPath = clsPublic.GetProgramSetting("keyHTTPVideoPath");
                            //string strVideoPath = clsPublic.GetAppSetting("Host") + "/Video/Research/VideoView.aspx?Video=";

                            string strActualVideoPath = clsPublic.GetProgramSetting("keyVideoResearchOutput");

                            if (!strActualVideoPath.EndsWith(@"\"))
                            {
                                strActualVideoPath = string.Concat(strActualVideoPath, @"\");
                            }

                            _questions.IsVideo = true;

                            strActualVideoPath = string.Concat(strActualVideoPath, entResearchQuestionsFiles.StrResearchFilesUniqueFileName);

                            ////if (System.IO.File.Exists(strActualVideoPath))
                            ////{
                                _questions.IsVideoComplete = true;
                                _questions.VideoPercentComplete = 100;

                                _questions.QuestionDescription = string.Format("<a href='{0}{1}' class='cboxiframe cboxElement' title='{2}'>{3}</a>", strVideoPath, entResearchQuestionsFiles.StrResearchFilesUniqueFileName, _questions.QuestionDescription, _questions.QuestionDescription);
                            //}
                            //else
                            //{
                            //    TList<ResearchVideoProcessed> tlstResearchVideoProcessed = DataRepository.ResearchVideoProcessedProvider.Find(string.Format("{0}={1}", ResearchVideoProcessedColumn.IntVidFileQuestionID.ToString(), entResearchQuestionsFiles.IntResearchFilesQuestionID));

                            //    double dblPercentage = 0;

                            //    if (tlstResearchVideoProcessed != null)
                            //    {
                            //        if (tlstResearchVideoProcessed.Count > 0)
                            //        {
                            //            double.TryParse(tlstResearchVideoProcessed[0].DblPercentComplete.ToString(), out dblPercentage);

                            //            _questions.QuestionDescription = string.Format("Processing Video <span class='vidUpdate' id='V{2}'>{1}</span>% - {0}", entResearchQuestionsFiles.StrResearchFilesName, dblPercentage, entResearchQuestionsFiles.IntResearchFilesQuestionID);
                            //            _questions.VideoPercentComplete = dblPercentage;
                            //        }
                            //        else
                            //        {
                            //            _questions.QuestionDescription = string.Format("Processing Video <span class='vidUpdate' id='V{1}'>(Waiting)</span>% - {0}", entResearchQuestionsFiles.StrResearchFilesName, entResearchQuestionsFiles.IntResearchFilesQuestionID);
                            //        }
                            //    }
                            //    else
                            //    {
                            //        _questions.QuestionDescription = string.Format("Processing Video <span class='vidUpdate' id='V{1}'>(Waiting)</span>% - {0}", entResearchQuestionsFiles.StrResearchFilesName, entResearchQuestionsFiles.IntResearchFilesQuestionID);
                            //    }
                            //}
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
    /// Gets the last question order value
    /// </summary>
    /// <param name="intResearchID">Research ID.</param>
    /// <returns>Last Question Order Value (System.Int16)</returns>
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

    /// <summary>
    /// Gets a promo code object
    /// </summary>
    /// <param name="intResearchID">Research ID.</param>
    /// <param name="strPromoCode">Promo code.</param>
    /// <returns>PromoCodes Object</returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public PromoCodes GetPromoCode(int intResearchID, string strPromoCode)
    {
        return clsPricing.ApplyPromoCode(intResearchID, strPromoCode);
    }

}
