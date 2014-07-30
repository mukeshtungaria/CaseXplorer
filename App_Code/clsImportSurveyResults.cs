// ***********************************************************************
// Author           : Dennis Sebenick
// Created          : 08-29-2011
//
// Last Modified By : dennis
// Last Modified On : 08-29-2011
// ***********************************************************************
// <copyright file="clsImportSurveyResults.cs" company="DGCC.COM">
//     . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using JuryData.Data;
using JuryData.Entities;

/// <summary>
/// Original Excel Import - no longer used
/// - Original from GMI - no longer used
/// </summary>
public class clsImportSurveyResults
{
    /// <summary>
    /// Initializes a new instance of the <see cref="clsImportSurveyResults" /> class.
    /// </summary>
    public clsImportSurveyResults()
    {
        //
        // TODO: Add constructor logic here
        //

    }

    /// <summary>
    /// Loads the excel file.
    /// </summary>
    /// <param name="strFileName">Excel File Name.</param>
    /// <returns>DataSet.</returns>
    public DataSet LoadExcelFile(string strFileName)
    {
        StringBuilder sb = new StringBuilder();
        string strConn;
        int intMRResponseNumber = 0;
        int intQuestionID = 0;
        int intResearchID = 0;
        int intResponseMapID = 0;

        OleDbConnection conn = null;
        OleDbCommand cmd = null;
        OleDbCommand cmdMap = null;
        OleDbDataReader rdrMap = null;

        try
        {
            // Open Excel File

            if (System.IO.File.Exists(strFileName))
            {

                strConn = string.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=""Excel 8.0;HDR=YES;IMEX=1;""", strFileName);

                //You must use the $ after the object you reference in the spreadsheet
                conn = new OleDbConnection(strConn);
                conn.Open();

                cmd = new OleDbCommand("SELECT * FROM [Data$]", conn);
                cmdMap = new OleDbCommand("SELECT * FROM [Map$]", conn);

                cmdMap.CommandType = CommandType.Text;
                cmd.CommandType = CommandType.Text;

                rdrMap = cmdMap.ExecuteReader();
            }
        }
        catch (Exception ex)
        {
            sb.AppendFormat("Connection Error (Load Excel File):  {0}", ex.Message);
            clsPublic.LogError(ex);
        }

        //
        // Read Import Map Sheet
        //
        try
        {
            if (rdrMap != null)
            {
                while (rdrMap.Read())
                {
                    try
                    {
                        if (rdrMap[0].ToString().Length > 0)
                        {
                            string[] strSplit = rdrMap[1].ToString().Split(new char[] { '(' }, 4);
                            string strQuestionID = strSplit[1].Replace(")", "").Replace("Q", "").Replace(" ", "");
                            string strQuestionType = strSplit[2].Replace(")", "").Trim();
                            string strQuestionText = string.Empty;

                            if (clsPublic.IsInteger(strQuestionID))
                            {
                                intQuestionID = Convert.ToInt32(strQuestionID);

                                if (intResearchID == 0)
                                {
                                    ResearchQuestions entResearchQuestions = DataRepository.ResearchQuestionsProvider.GetByAutoQuesID(intQuestionID);

                                    if (entResearchQuestions != null)
                                    {
                                        intResearchID = entResearchQuestions.IntQuesResearchID;

                                        System.Data.Common.DbCommand cmdClearAnswers = clsUtilities.GetDBCommand("usp_paramdel_ClearResearchResultsMap");

                                        clsUtilities.AddParameter(ref cmdClearAnswers, "@ResearchID", ParameterDirection.Input, DbType.Int32, intResearchID);
                                        clsUtilities.SQLExecuteNonQuery(cmdClearAnswers);
                                    }
                                }

                                ResearchResultsQuestions entResearchResultsQuestions = DataRepository.ResearchResultsQuestionsProvider.GetByIntResultQuestionID(intQuestionID);

                                if (entResearchResultsQuestions == null)
                                {
                                    intMRResponseNumber = 0;
                                    entResearchResultsQuestions = new ResearchResultsQuestions();
                                    entResearchResultsQuestions.IntResultResearchID = intResearchID;
                                    entResearchResultsQuestions.IntResultQuestionID = intQuestionID;
                                }

                                if (strQuestionType.ToUpper() == "MR" || strQuestionType.ToUpper() == "MX")
                                {
                                    intMRResponseNumber++;

                                    strQuestionText = strSplit[3].Trim();

                                    int intPosMR = strQuestionText.IndexOf(string.Format(") {0}.", intMRResponseNumber));

                                    if (entResearchResultsQuestions.StrResultQuestionText.Length == 0)
                                    {
                                        entResearchResultsQuestions.StrResultQuestionText = strQuestionText.Substring(0, intPosMR);
                                        DataRepository.ResearchResultsQuestionsProvider.Save(entResearchResultsQuestions);
                                    }

                                    string strQuestionResponse = strQuestionText.Substring(intPosMR + 1).Trim();

                                    intResponseMapID = entResearchResultsQuestions.AutoResultMapID;

                                    string[] strMRResponses = strQuestionResponse.Split(new char[] { '.' }, 2);

                                    SaveResponses(intResponseMapID, intQuestionID, strMRResponses[0].Trim(), strMRResponses[1]);


                                }
                                else
                                {
                                    intMRResponseNumber = 0;
                                    entResearchResultsQuestions.StrResultQuestionText = strSplit[3].Substring(0, strSplit[3].Length - 1);

                                    DataRepository.ResearchResultsQuestionsProvider.Save(entResearchResultsQuestions);
                                    intResponseMapID = entResearchResultsQuestions.AutoResultMapID;
                                }
                            }
                        }
                        else
                        {
                            string[] strSplit = rdrMap[1].ToString().Split(new char[] { ':' }, 2);

                            string strValue = strSplit[0];

                            SaveResponses(intResponseMapID, intQuestionID, strValue, strSplit[1].Trim());
                        }
                    }
                    catch (Exception ex)
                    {
                        sb.AppendFormat("Error: {0}", ex.Message);
                        clsPublic.LogError(ex);
                    }
                }

                rdrMap.Close();
            }
        }
        catch (Exception ex)
        {
            sb.AppendFormat("Import Processing Error (Read Map Sheet):  {0}", ex.Message);
            clsPublic.LogError(ex);
        }

        try
        {

            OleDbDataReader rdr = cmd.ExecuteReader();

            int intColumnCount = rdr.FieldCount;
            int intRespondentID = 0;

            while (rdr.Read())
            {
                try
                {
                    string strRespondentID = rdr[0].ToString();

                    if (clsPublic.IsInteger(strRespondentID))
                        intRespondentID = Convert.ToInt32(strRespondentID);
                    else
                        intRespondentID = 0;

                    for (int intCounter = 1; intCounter < intColumnCount; intCounter++)
                    {
                        ResearchAnswers entResearchAnswers = new ResearchAnswers();

                        entResearchAnswers.IntAnswerResearchID = intResearchID;
                        entResearchAnswers.IntAnswerUserID = intRespondentID;

                        string strField = rdr.GetName(intCounter).Replace("Q_Q", "");
                        string strResponseValue = string.Empty;

                        if (clsPublic.IsInteger(strField))
                            entResearchAnswers.IntAnswerQuestionID = Convert.ToInt32(strField);
                        else
                        {
                            string[] strMRValue = strField.Split(new char[] { '_' }, 2);

                            if (strMRValue.Length > 1)
                            {

                                strField = strMRValue[0].Trim();
                                strResponseValue = strMRValue[1].Trim();

                                if (clsPublic.IsInteger(strField))
                                {
                                    entResearchAnswers.IntAnswerQuestionID = Convert.ToInt32(strField);

                                    if (!clsPublic.IsInteger(strResponseValue))
                                        strResponseValue = string.Empty;
                                }
                                else
                                    entResearchAnswers.IntAnswerQuestionID = 0;
                            }
                            else
                                entResearchAnswers.IntAnswerQuestionID = 0;

                        }

                        bool boolCancel = false;
                        string strValue = rdr[intCounter].ToString();

                        if (clsPublic.IsInteger(strValue))
                        {
                            if (strResponseValue.Length > 0)
                            {
                                if (strValue == "1")
                                    entResearchAnswers.IntAnswerResponseID = Convert.ToInt32(strResponseValue);
                                else
                                    boolCancel = true;

                                entResearchAnswers.StrAnswerText = string.Empty;
                            }
                            else
                            {
                                entResearchAnswers.IntAnswerResponseID = Convert.ToInt32(strValue);
                                entResearchAnswers.StrAnswerText = string.Empty;
                            }
                        }
                        else
                        {
                            entResearchAnswers.IntAnswerResponseID = 0;
                            entResearchAnswers.MemAnswerOpenEnd = strValue;
                        }

                        if (!boolCancel)
                            DataRepository.ResearchAnswersProvider.Save(entResearchAnswers);
                        else
                            entResearchAnswers = null;
                    }
                }
                catch (Exception ex)
                {
                    sb.AppendFormat("Import Processing Error (In Loop Continuing):  {0}{1}{2}", ex.Message, Environment.NewLine, ex.StackTrace);
                    clsPublic.LogError(ex);
                }
            }

            rdr.Close();

            // Update Results Received
            ResearchMain entResearchMain = DataRepository.ResearchMainProvider.GetByAutoResearchID(intResearchID);

            if (entResearchMain != null)
            {
                entResearchMain.DatResearchResultsReceived = DateTime.Now;
                DataRepository.ResearchMainProvider.Save(entResearchMain);

                clsPublic.SendContactEmail((Guid)entResearchMain.UserID, "ResultsComplete");


            }

        }
        catch (Exception ex)
        {
            sb.AppendFormat("Import Processing Error (Read Data Sheet):  {0}", ex.Message);
            clsPublic.LogError(ex);
        }

        //Always close the DataReader and connection.
        
        conn.Close();

        //
        // Mark 


        return null;

    }

    /// <summary>
    /// Stores responses to database
    /// </summary>
    /// <param name="intResponseMapID">The int response map ID.</param>
    /// <param name="intQuestionID">The int question ID.</param>
    /// <param name="strResponseValue">The STR response value.</param>
    /// <param name="strResponseText">The STR response text.</param>
    protected void SaveResponses(int intResponseMapID, int intQuestionID, string strResponseValue, string strResponseText)
    {

//        ResearchResultsResponses entResearchResultsResponses = DataRepository.ResearchResultsResponsesProvider.GetByIntResultQuestionIDIntResultResponseValue( new ResearchResultsResponses();
        ResearchResultsResponses entResearchResultsResponses =  new ResearchResultsResponses();

        entResearchResultsResponses.IntResultResponseQuestion = intResponseMapID;
        entResearchResultsResponses.IntResultQuestionID = intQuestionID;
        entResearchResultsResponses.StrResultResponseText = strResponseText.Trim();

        if (clsPublic.IsInteger(strResponseValue))
            entResearchResultsResponses.IntResultResponseValue = Convert.ToInt32(strResponseValue);
        else
            entResearchResultsResponses.IntResultResponseValue = 0;

        DataRepository.ResearchResultsResponsesProvider.Save(entResearchResultsResponses);


    }
}
