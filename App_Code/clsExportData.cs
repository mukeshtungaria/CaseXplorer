// ***********************************************************************
// Author           : Dennis Sebenick
// Created          : 08-29-2011
//
// Last Modified By : dennis
// Last Modified On : 05-01-2012
// ***********************************************************************
// <copyright file="clsExportData.cs" company="DGCC.COM">
//     . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;

using System.Text;

using JuryData.Entities;
using JuryData.Data;

/// <summary>
/// Class for Data Export operations
/// </summary>
public class clsExportData
{
    /// <summary>
    /// Initializes a new instance of the <see cref="clsExportData" /> class.
    /// </summary>
	public clsExportData()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    /// <summary>
    /// Exports data to comma delimited file
    /// </summary>
    /// <param name="intResearchID">Research ID</param>
    public void ExportData(int intResearchID)
    {
        TList<ResearchQuestions> tlstResearchQuestions = DataRepository.ResearchQuestionsProvider.GetByIntQuesResearchID(intResearchID);

        if (tlstResearchQuestions != null)
        {
            if (tlstResearchQuestions.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sbDef = new StringBuilder();
            
                System.Data.Common.DbCommand cmdRespondentAnswerIDs = clsUtilities.GetDBCommand("_up_paramsel_GetRespondentAnswerIDs");

                clsUtilities.AddParameter(ref cmdRespondentAnswerIDs, "@ResearchID", ParameterDirection.Input, DbType.Int32, intResearchID);

                DataSet ds =clsUtilities.SQLExecuteDataSet(cmdRespondentAnswerIDs);
                tlstResearchQuestions.Sort(ResearchQuestionsColumn.IntQuesOrder.ToString());

                int intCounter = 0;

                string strColumnHeader = string.Empty;
                

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    intCounter++;

                    int intColumn = 0;

                    string strData = string.Empty;
                    string strDelimit = string.Empty;

                    ResearchAnswersQuery query1 = new ResearchAnswersQuery(true, true);
                    query1.Append(ResearchAnswersColumn.IntAnswerResearchID, intResearchID.ToString());
                    query1.Append(ResearchAnswersColumn.IntAnswerUserID, dr["intAnswerUserID"].ToString());

                    TList<ResearchAnswers> tlstResearchAnswers = DataRepository.ResearchAnswersProvider.Find(query1);

                    tlstResearchQuestions.ForEach(delegate(ResearchQuestions entResearchQuestions)
                    {


                        if (entResearchQuestions.IntQuesType != 10 && entResearchQuestions.IntQuesType != 11 && entResearchQuestions.IntQuesType != 12 && entResearchQuestions.IntQuesType != 13)
                        {

                            intColumn++;

                            if (intCounter == 1)
                            {
                                sbDef.AppendLine(string.Format("Field{0},\"{1}\"", intColumn, entResearchQuestions.StrQuesText.Replace("\"", "\"\"")));

                                TList<ResearchResultsResponses> tlstResearchResultsResponses = DataRepository.ResearchResultsResponsesProvider.GetByIntResultQuestionID(entResearchQuestions.AutoQuesID);

                                if (entResearchQuestions.IntQuesType != 2)
                                {

                                    tlstResearchResultsResponses.ForEach(delegate(ResearchResultsResponses entResearchResultsResponses)
                                    {
                                        sbDef.AppendLine(string.Format("{0},\"{1}\"", entResearchResultsResponses.IntResultResponseValue, entResearchResultsResponses.StrResultResponseText.Replace("\"", "\"\"")));
                                    });
                                }
                                else if (entResearchQuestions.IntQuesType == 2)
                                {
                                    sbDef.AppendLine(string.Format("{0},\"{1}\"", 1, "Yes"));
                                    sbDef.AppendLine(string.Format("{0},\"{1}\"", 2, "No"));
                                }

                                sbDef.AppendLine();

                                if (entResearchQuestions.IntQuesType == 5)
                                {
                                    tlstResearchResultsResponses.ForEach(delegate(ResearchResultsResponses entResearchResultsResponses)
                                    {
                                        strColumnHeader = string.Format("{0}{1}Field{2}_{3}", strColumnHeader, strDelimit, intColumn, entResearchResultsResponses.IntSGResponseID);
                                    });

                                }
                                else
                                    strColumnHeader = string.Format("{0}{1}Field{2}", strColumnHeader, strDelimit, intColumn);
                            }

                            string strOutput = string.Empty;

                            // Multi-response question
                            if (entResearchQuestions.IntQuesType != 5)      
                            {
                                tlstResearchAnswers.ApplyFilter(delegate(ResearchAnswers entResearchAnswers) { return entResearchAnswers.IntAnswerQuestionID == entResearchQuestions.AutoQuesID; });

                                if (tlstResearchAnswers.Count > 0)
                                {
                                    if (entResearchQuestions.IntQuesType == 1 )
                                    {
                                        strOutput = tlstResearchAnswers[0].MemAnswerOpenEnd;

                                        if (strOutput == null)
                                        {
                                            strOutput = tlstResearchAnswers[0].IntAnswerResponseID.ToString();

                                            if (strOutput == null)
                                                strOutput = string.Empty;
                                        }
                                        else
                                            strOutput = "\"" + strOutput.Replace("\"", "\"\"") + "\"";
                                    }
                                    else if (entResearchQuestions.IntQuesType != 11)
                                    {
                                        strOutput = tlstResearchAnswers[0].IntAnswerResponseID.ToString();
                                    }
                                }

                                strData = string.Format("{0}{1}{2}", strData, strDelimit, strOutput);
                            }
                            else
                            {
                                TList<ResearchResultsResponses> tlstResearchResultsResponses = DataRepository.ResearchResultsResponsesProvider.GetByIntResultQuestionID(entResearchQuestions.AutoQuesID);

                                tlstResearchResultsResponses.ForEach(delegate(ResearchResultsResponses entResearchResultsResponses)
                                {
                                    tlstResearchAnswers.ApplyFilter(delegate(ResearchAnswers entResearchAnswers) { return entResearchAnswers.IntAnswerQuestionID == entResearchQuestions.AutoQuesID && entResearchAnswers.IntAnswerResponseID == entResearchResultsResponses.IntSGResponseID; });

                                    if (tlstResearchAnswers.Count > 0)
                                    {
                                        strOutput = entResearchResultsResponses.IntSGResponseID.ToString();
                                    }
                                    else
                                    {
                                        strOutput = string.Empty;
                                    }

                                    strData = string.Format("{0}{1}{2}", strData, strDelimit, strOutput);

                                    strDelimit = ",";
                                });
                            }

                            strDelimit = ",";

                        }

                    });

                    if (intCounter == 1)
                    {
                        sb.AppendLine(strColumnHeader);
                    }

                    sb.AppendLine(strData);

                }

                sb.AppendLine();
                sb.AppendLine(sbDef.ToString());

                WriteToCSV(sb.ToString(), "data.csv");
            }
        }

    }

    /// <summary>
    /// Generates the physical csv file
    /// </summary>
    /// <param name="str">The data to export</param>
    /// <param name="strFileName">Name of the export file.</param>
    public static void WriteToCSV(string str, string strFileName)
    {
        string attachment = string.Format("attachment; filename={0}", strFileName);
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("Windows-1252");

        HttpContext.Current.Response.AddHeader("content-disposition", attachment);
        HttpContext.Current.Response.ContentType = "text/csv";
        HttpContext.Current.Response.AddHeader("Pragma", "public");

        HttpContext.Current.Response.Write(str);

        HttpContext.Current.Response.End();

    }
}

