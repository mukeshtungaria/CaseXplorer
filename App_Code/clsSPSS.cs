using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Text;

using System.Data;
using System.Data.Common;
using System.Data.SqlClient;


/// <summary>
/// Summary description for clsSPSS
/// </summary>
public class clsSPSS
{
    public string ExportSPSS_SPSDefinition(int intResearchID)
    {
        try
        {
            DbCommand cmd = clsPublic.GetDBCommand("_up_paramsel_SPSSDataResults");
            clsPublic.AddParameter(ref cmd, "@ResearchID", ParameterDirection.Input, DbType.Int32, intResearchID);

            DataSet ds = clsPublic.SQLExecuteDataSet(cmd);

            int intCount = ds.Tables.Count;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(@"DATA LIST LIST (',') / ");

            List<int> lstUserIDs = new List<int>();
            List<int> lstQuestionIDs = new List<int>();
            List<clsMRS> lstMultiResponse = new List<clsMRS>();

            foreach (DataRow dr in ds.Tables[2].Rows)
            {
                int intUserID = 0;
                int.TryParse(dr["UserID"].ToString(), out intUserID);

                if (intUserID > 0)
                    lstUserIDs.Add(intUserID);
            }


            int intVarPerLine = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                intVarPerLine++;

                string strType = dr["intQuesType"].ToString();
                int intQuesOrder = 0;
                int.TryParse(dr["intQuesOrder"].ToString(), out intQuesOrder);

                if (intQuesOrder > 0)
                {
                    lstQuestionIDs.Add(intQuesOrder);

                    if (strType == "5")
                    {
                        int intRespCount = 0;
                        int.TryParse(dr["ResponseCounts"].ToString(), out intRespCount);

                        for (int i = 1; i <= intRespCount; i++)
                        {
                            sb.AppendFormat(" Q{0}_{1}", dr["intQuesOrder"].ToString(), i);

                            if (intVarPerLine == 10)
                            {
                                sb.AppendLine();
                                intVarPerLine = 0;
                            }

                            intVarPerLine++;
                        }
                    }
                    else
                    {
                        sb.AppendFormat(" Q{0}", dr["intQuesOrder"].ToString());
                    }
                }

                if (intVarPerLine == 10)
                {
                    sb.AppendLine();
                    intVarPerLine = 0;
                }
            }

            sb.AppendLine(" .");
            sb.AppendLine();
            sb.AppendLine("BEGIN DATA");


            DataTable dt = ds.Tables[3];

            foreach (int intUserID in lstUserIDs)
            {
                string strDelimiter = string.Empty;

                foreach (int intQuestionIDs in lstQuestionIDs)
                {
                    DataRow[] drs = dt.Select(string.Format("(intAnswerUserID = {0}) and (intQuesOrder = {1})", intUserID, intQuestionIDs));

                    int intResponseCounts = 0;
                    int intQuestionType = 0;

                    foreach (DataRow dr in drs)
                    {
                        if (intResponseCounts == 0)
                        {
                            int.TryParse(dr["intQuesType"].ToString(), out intQuestionType);
                            int.TryParse(dr["ResponseCounts"].ToString(), out intResponseCounts);
                        }

                        if (intQuestionType == 1)
                        {


                        }
                        else
                        {
                            sb.AppendFormat("{0}{1}", strDelimiter, dr["intAnswerResponseID"].ToString());
                        }

                        strDelimiter = ",";
                    }

                    if (intQuestionType == 5)
                    {
                        int intCountDiff = intResponseCounts - drs.Count();

                        for (int i = 0; i < intCountDiff; i++)
                        {
                            sb.Append(",0");
                        }
                    }

                }

                sb.AppendLine();
            }

            sb.AppendLine("END DATA.");

            sb.AppendLine();


            sb.AppendLine(@"Execute.");
            sb.AppendLine();

            sb.AppendLine(@"/************************************************.");
            sb.AppendLine(@"/****          VARIABLE LABELING           ******.");
            sb.AppendLine(@"/****   NOTE:  SPSS TRUNCATES LABELS AT    ******.");
            sb.AppendLine(@"/****   255 CHARACTERS SO EDIT AS NEEDED.  ******.");
            sb.AppendLine(@"/************************************************.");
            sb.AppendLine();
            sb.AppendLine(@"VARIABLE LABELS");

            string strCharOne = string.Empty;
            string strQuote = @"""";

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["intQuesType"].ToString() == "5")
                {
                    int intResponseCounts = 0;
                    int.TryParse(dr["ResponseCounts"].ToString(), out intResponseCounts);

                    string strMultiVariables = string.Empty;

                    for (int i = 1; i <= intResponseCounts; i++)
                    {
                        strMultiVariables = strMultiVariables + string.Format("Q{0}_{1} ", dr["intQuesOrder"].ToString(), i);

                        sb.AppendFormat(@"{0}Q{1}_{3}    ""{2}""", strCharOne, dr["intQuesOrder"].ToString(), clsPublic.ReplaceEncodedHTML(clsPublic.StripHTML(dr["strResultQuestionText"].ToString().Replace(strQuote, ""))), i);
                        sb.AppendLine();
                    }

                    lstMultiResponse.Add(new clsMRS(string.Format("Q{0}", dr["intQuesOrder"].ToString()), clsPublic.ReplaceEncodedHTML(clsPublic.StripHTML(dr["strResultQuestionText"].ToString().Replace(strQuote, ""))), strMultiVariables));
                }
                else
                {
                    sb.AppendFormat(@"{0}Q{1}    ""{2}""", strCharOne, dr["intQuesOrder"].ToString(), clsPublic.ReplaceEncodedHTML(clsPublic.StripHTML(dr["strResultQuestionText"].ToString().Replace(strQuote, ""))));
                    sb.AppendLine();
                }


                strCharOne = @"/";
            }

            sb.AppendLine(".");
            sb.AppendLine(@"Execute.");

            sb.AppendLine();
            sb.AppendLine(@"/************************************************.");
            sb.AppendLine(@"/****           VALUE LABELING             ******.");
            sb.AppendLine(@"/****   NOTE:  SPSS TRUNCATES LABELS AT    ******.");
            sb.AppendLine(@"/****   60 CHARACTERS SO EDIT AS NEEDED.   ******.");
            sb.AppendLine(@"/************************************************.");

            sb.AppendLine();
            sb.AppendLine(@"VALUE LABELS");

            strCharOne = string.Empty;

            string strPrevOrder = string.Empty;

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                if (strPrevOrder != dr["intQuesOrder"].ToString())
                {
                    if (dr["intQuesType"].ToString() == "5")
                    {
                        int intResponseCounts = 0;
                        int.TryParse(dr["ResponseCounts"].ToString(), out intResponseCounts);

                        sb.Append(strCharOne);

                        for (int i = 1; i <= intResponseCounts; i++)
                        {
                            sb.AppendFormat(@"Q{0}_{1} ", dr["intQuesOrder"].ToString(),i);
                        }
                    }
                    else
                    {
                        sb.AppendFormat(@"{0}Q{1}", strCharOne, dr["intQuesOrder"].ToString());
                    }
                    sb.AppendLine();
                    strCharOne = @"/";

                    strPrevOrder = dr["intQuesOrder"].ToString();
                }

                sb.AppendFormat(@"{0}    ""{1}""", dr["intSGResponseID"].ToString(), clsPublic.ReplaceEncodedHTML(clsPublic.StripHTML(dr["strResultResponseText"].ToString().Replace(strQuote, ""))));
                sb.AppendLine();
            }

            sb.AppendLine(".");
            sb.AppendLine(@"Execute.");
            sb.AppendLine();

            foreach (clsMRS clsMRSTemp in lstMultiResponse)
            {
                sb.AppendFormat("MRSETS /MCGROUP NAME=${0}", clsMRSTemp.MRSName);
                sb.AppendLine();
                sb.AppendFormat("LABEL='{0}'", clsMRSTemp.MRSLabel.Replace("'", ""));
                sb.AppendLine();
                sb.AppendFormat("VARIABLES={0}.)", clsMRSTemp.MRSVariables);
                sb.AppendLine();
                sb.AppendLine();
            }

            foreach (clsMRS clsMRSTemp in lstMultiResponse)
            {
                sb.AppendFormat("MISSING VALUES {0} (0).", clsMRSTemp.MRSVariables);
                sb.AppendLine();
                sb.AppendLine();
            }

            sb.AppendLine(@"Execute.");
            sb.AppendLine();


            string strFinalName = string.Format(@"c:\juryresearch\spss_ouput-{0}.sps", intResearchID);

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(strFinalName, false))
            {
                file.Write(sb.ToString());
            }

            return string.Empty;

        }
        catch (Exception ex)
        {
            clsPublic.LogError(ex);

            return string.Empty;
        }
    }

    public class clsMRS
    {
        private string m_strName = string.Empty;
        private string m_strLabel = string.Empty;
        private string m_strVariables = string.Empty;

        public clsMRS(string strName, string strLabel, string strVariables)
        {
            m_strName = strName;
            m_strLabel = strLabel;
            m_strVariables = strVariables;
        }

        public string MRSName
        {
            get { return m_strName;}
            set { m_strName = value;}
        }

        public string MRSLabel
        {
            get { return m_strLabel; }
            set { m_strLabel = value;}
        }

        public string MRSVariables
        {
            get { return m_strVariables; }
            set { m_strVariables = value; }
        }
    }
}

