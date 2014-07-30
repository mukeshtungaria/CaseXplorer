// ***********************************************************************
// Author           : Dennis Sebenick
// Created          : 07-15-2012
//
// Last Modified By : dennis
// Last Modified On : 07-28-2012
// ***********************************************************************
// <copyright file="clsSGizmo.cs" company="DGCC.COM">
//     . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Web;
using System.Web.Script.Serialization;
using System.IO;
using System.Text.RegularExpressions;
using System.Net.Cache;
using System.Net;

using System.Data;

using System.Security.Cryptography;

using JuryData.Data;
using JuryData.Entities;


/// <summary>
/// Result class object for SG API
/// </summary>
public class apiresult
{
    /// <summary>
    /// 
    /// </summary>
    public string resultok;
    /// <summary>
    /// 
    /// </summary>
    public string totalcount;
    /// <summary>
    /// 
    /// </summary>
    public string page;
    /// <summary>
    /// 
    /// </summary>
    public string totalpages;
    /// <summary>
    /// 
    /// </summary>
    public string resultsperpage;
    /// <summary>
    /// 
    /// </summary>
    public data data;
}

/// <summary>
/// Survey Gizmo Class Data
/// </summary>
public class data
{
    /// <summary>
    /// Gets or sets the unknown node.
    /// </summary>
    /// <value>The unknown node.</value>
    [XmlElement("unknownNode")]
    public List<unknownNode> unknownNode {get;set;}
}

/// <summary>
/// Class SurveyList
/// </summary>
public class SurveyList
{
    /// <summary>
    /// Survey Gizmo ID
    /// </summary>
    public int SGID;
    /// <summary>
    /// Question ID
    /// </summary>
    public int QuesID;
}

/// <summary>
/// Survey Gizmo UnknownNode for XML Parsing
/// </summary>
public class unknownNode
{
    /// <summary>
    /// 
    /// </summary>
    public string id;
    /// <summary>
    /// 
    /// </summary>
    public string contactid;
    /// <summary>
    /// 
    /// </summary>
    public string status;
    /// <summary>
    /// 
    /// </summary>
    public string istestdata;
    /// <summary>
    /// 
    /// </summary>
    public string datesubmitted;

    /// <summary>
    /// Gets or sets the datapoint.
    /// </summary>
    /// <value>The datapoint.</value>
    [XmlElement("datapoint")]
    public List<datapoint> datapoint {get; set;}
    /// <summary>
    /// 
    /// </summary>
    public string urlac;
    /// <summary>
    /// 
    /// </summary>
    public string urlsn;
    /// <summary>
    /// 
    /// </summary>
    public string urllang;
    /// <summary>
    /// 
    /// </summary>
    public string variableSTANDARDIP;
    /// <summary>
    /// 
    /// </summary>
    public string variableSTANDARDLONG;
    /// <summary>
    /// 
    /// </summary>
    public string variableSTANDARDLAT;
    /// <summary>
    /// 
    /// </summary>
    public string variableSTANDARDGEOCOUNTRY;
    /// <summary>
    /// 
    /// </summary>
    public string variableSTANDARDGEOCITY;
    /// <summary>
    /// 
    /// </summary>
    public string variableSTANDARDGEOREGION;
    /// <summary>
    /// 
    /// </summary>
    public string variableSTANDARDGEOPOSTAL;
    /// <summary>
    /// 
    /// </summary>
    public string variableSTANDARDRESPONSETIME;
    /// <summary>
    /// 
    /// </summary>
    public string variableSTANDARDCOMMENTS;
    /// <summary>
    /// 
    /// </summary>
    public string variableSTANDARDGEODMA;

    /// <summary>
    /// Gets or sets the variable.
    /// </summary>
    /// <value>The variable.</value>
    [XmlElement("variable")]
    public List<string> variable { get; set; }

}


/// <summary>
/// Class datapoint
/// </summary>
public class datapoint
{
    /// <summary>
    /// 
    /// </summary>
    public string fieldname;
    /// <summary>
    /// 
    /// </summary>
    public string value;

}

/// <summary>
/// Class SGErrorQuestion
/// </summary>
public class SGErrorQuestion
{
    /// <summary>
    /// 
    /// </summary>
    protected string m_strQuestionText;
    /// <summary>
    /// 
    /// </summary>
    protected int m_intQuesID;
    /// <summary>
    /// 
    /// </summary>
    protected int m_intSGID;
    /// <summary>
    /// 
    /// </summary>
    protected string m_strRemarks;

    /// <summary>
    /// Gets or sets the Question text.
    /// </summary>
    /// <value>The Question text.</value>
    public string QuestionText
    {
        set { m_strQuestionText = value; }
        get { return m_strQuestionText; }
    }

    /// <summary>
    /// Gets or sets the QuesID value.
    /// </summary>
    /// <value>The QuesID value.</value>
    public int QuesID
    {
        set { m_intQuesID = value; }
        get { return m_intQuesID; }
    }

    /// <summary>
    /// Gets or sets the SG ID.
    /// </summary>
    /// <value>The SG ID.</value>
    public int SGID
    {
        set { m_intSGID = value; }
        get { return m_intSGID; }
    }

    /// <summary>
    /// Gets or sets the Remarks.
    /// </summary>
    /// <value>The Remarks.</value>
    public string Remarks
    {
        set { m_strRemarks = value; }
        get { return m_strRemarks; }
    }
}

/// <summary>
/// Summary description for clsSGizmo
/// </summary>
public class clsSGizmo
{
    /// <summary>
    /// Class clsQuestion
    /// </summary>
    public class clsQuestion
    {
        /// <summary>
        /// 
        /// </summary>
        protected string m_strQuestionText;
        /// <summary>
        /// 
        /// </summary>
        protected int m_intQuestionID;

        /// <summary>
        /// Gets or sets the question text.
        /// </summary>
        /// <value>The question text.</value>
        public string QuestionText
        {
            set { m_strQuestionText = value;}
            get { return m_strQuestionText;}
        }

        /// <summary>
        /// Gets or sets the question ID.
        /// </summary>
        /// <value>The question ID.</value>
        public int QuestionID
        {
            set { m_intQuestionID = value;}
            get { return m_intQuestionID;}
        }
    }

    /// <summary>
    /// Class clsQuestionOptions
    /// </summary>
    public class clsQuestionOptions
    {
        /// <summary>
        /// 
        /// </summary>
        protected string m_strOptionText = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        protected string m_strOptionValue = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        protected string m_strOptionID = string.Empty;

        /// <summary>
        /// Gets or sets the option text.
        /// </summary>
        /// <value>The option text.</value>
        public string OptionText
        {
            set { m_strOptionText = value;}
            get { return m_strOptionText;}
        }

        /// <summary>
        /// Gets or sets the option value.
        /// </summary>
        /// <value>The option value.</value>
        public string OptionValue
        {
            set { m_strOptionValue = value;}
            get { return m_strOptionValue;}
        }

        /// <summary>
        /// Gets or sets the option ID.
        /// </summary>
        /// <value>The option ID.</value>
        public string OptionID
        {
            set { m_strOptionID = value;}
            get { return m_strOptionID;}
        }
    }

    /// <summary>
    /// Class clsAllQuestionOptions
    /// </summary>
    public class clsAllQuestionOptions
    {
        /// <summary>
        /// 
        /// </summary>
        protected string m_strOptionText;
        /// <summary>
        /// 
        /// </summary>
        protected int m_intQuesID;
        /// <summary>
        /// 
        /// </summary>
        protected int m_intOptionID;

        /// <summary>
        /// Gets or sets the option text.
        /// </summary>
        /// <value>The option text.</value>
        public string OptionText
        {
            set { m_strOptionText = value; }
            get { return m_strOptionText; }
        }

        /// <summary>
        /// Gets or sets the QuesID value.
        /// </summary>
        /// <value>The QuesID value.</value>
        public int QuesID
        {
            set { m_intQuesID = value; }
            get { return m_intQuesID; }
        }

        /// <summary>
        /// Gets or sets the option ID.
        /// </summary>
        /// <value>The option ID.</value>
        public int OptionID
        {
            set { m_intOptionID = value; }
            get { return m_intOptionID; }
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="clsSGizmo" /> class.
    /// </summary>
    public clsSGizmo()
    {

    }

    /// <summary>
    /// Enum SGQuestionTypes
    /// </summary>
    public enum SGQuestionTypes { radio, checkbox, textbox, essay, instructions, hidden, urlredirect, image, video, httpinsert };

    /// <summary>
    /// Encodes password to be sent to SGizmo (MD5)
    /// </summary>
    /// <param name="originalPassword">The original password.</param>
    /// <returns>Encoded Password (System.String)</returns>
    public string EncodePassword(string originalPassword)
    {
        // step 1, calculate MD5 hash from input
        MD5 md5 = System.Security.Cryptography.MD5.Create();
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(originalPassword);
        byte[] hash = md5.ComputeHash(inputBytes);

        // step 2, convert byte array to hex string
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            sb.Append(hash[i].ToString("X2"));
        }
        return sb.ToString();

    }

    /// <summary>
    /// Send a post request to external server
    /// - no used
    /// </summary>
    /// <param name="requestUri">The request URI.</param>
    /// <param name="method">The method.</param>
    /// <param name="postData">The post data.</param>
    /// <param name="cookieContainer">The cookie container.</param>
    /// <param name="userAgent">The user agent.</param>
    /// <param name="acceptHeaderString">The accept header string.</param>
    /// <param name="referer">The referer.</param>
    /// <param name="contentType">Type of the content.</param>
    /// <param name="responseUri">The response URI.</param>
    /// <returns>System.String.</returns>
    public string doRequestWithBytesPostData(string requestUri, string method, byte[] postData, CookieContainer cookieContainer, string userAgent, string acceptHeaderString, string referer, string contentType, string responseUri) 
    {
        var request = WebRequest.Create(requestUri) as HttpWebRequest;

        if (request != null)
        {
            request.KeepAlive = true;
            request.Method = "POST";
            request.Timeout = 50000;

            //Set the ContentLength property of the "HttpWebRequest"
            request.ContentLength = postData.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(postData, 0, postData.Length);
            requestStream.Close();

            HttpWebResponse resp = (HttpWebResponse)request.GetResponse();
            StreamReader responseReader = new StreamReader(resp.GetResponseStream(), Encoding.Default);
            string strRet = responseReader.ReadToEnd();
            resp.Close();
        }

        return string.Empty;
    }

    /// <summary>
    /// Post data to SurveyGizmo
    /// </summary>
    /// <param name="strPath">SurveyGizmo URL path</param>
    /// <returns>XmlDocument object with SG results</returns>
    private XmlDocument PostToSQ(string strPath)
    {
      
	XmlDocument doc = new XmlDocument();
        //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 ;
            //clsPublic.LogError(strPath);
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(strPath);
            WebReq.Method = "GET";
            HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();

            Stream dataStream = WebResp.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            
            string strResponseFromServer = reader.ReadToEnd();

            reader.Close();
            dataStream.Close();
            WebResp.Close();

            clsPublic.LogError("strResponseFromServer: " + strResponseFromServer);
            try
            { 
            
            if (strResponseFromServer.Length > 0)
            {
                doc.LoadXml(strResponseFromServer);
            }
        }
        catch (Exception ex)
        {
            int intXML = strPath.IndexOf(".xml");

            if (intXML > 0)
            {
                strPath = strPath.Substring(0, intXML);
                clsPublic.LogError(string.Format("Issue with parsing surveygizmo:  {0}; " + ex.Message, strPath));
            }
            else
                clsPublic.LogError(ex);
        }

        return doc;

    }

    /// <summary>
    /// Retrieve encoded user name and password for SurveyGizmo
    /// </summary>
    /// <returns>User name / password combo (System.String)</returns>
    public string UserNamePassword()
    {
        //return string.Format("user:md5=BMorse@decisionquest.com:{0}", EncodePassword("DecisionQuest21535"));
        if (System.Configuration.ConfigurationManager.AppSettings["SG-Testing"].ToString().ToLower().Trim() == "false")
        {
            return string.Format("user:md5=BMorse@decisionquest.com:{0}", EncodePassword("DecisionQuest21535"));
        }
        else
        {
            return string.Format("user:md5=BMorse@decisionquest.com:{0}", EncodePassword("DecisionQuest21535"));
        }
        
    }

    /// <summary>
    /// SurveyGizmo Call
    /// - Close Survey
    /// </summary>
    /// <param name="intSurveyID">SurveyGizmo Survey ID</param>
    public void CloseSurvey(int intSurveyID)
    {
        string strPath = string.Format("https://restapi.surveygizmo.com/V3/survey/{0}.xml", intSurveyID);
        string strParams = string.Format("_method=post&type=survey&status=Closed&{0}", UserNamePassword());

        string strFullPath = string.Format("{0}?{1}", strPath, strParams);

        XmlDocument doc = PostToSQ(strFullPath);

    }


    /// <summary>
    /// SurveyGizmo Call
    /// - Creates the new survey.
    /// </summary>
    /// <param name="strTitle">Survey Title</param>
    /// <param name="boolIsTest">Is Test Survey</param>
    /// <param name="intResearchID">Research ID.</param>
    /// <returns>Returns SurveyGizmo ID (System.Int32)</returns>
    public int CreateNewSurvey(string strTitle, bool boolIsTest, int intResearchID)
    {
        strTitle = HttpUtility.UrlEncode(strTitle);

        string strPath = "https://restapi.surveygizmo.com/V3/survey.xml";
        string strParams = string.Empty;

        if (boolIsTest)
            strParams = string.Format("_method=put&type=survey&title={0}&{1}&status=Testing", intResearchID, UserNamePassword());
        else
            strParams = string.Format("_method=put&type=survey&title={0}&{1}&status=Launched", strTitle, UserNamePassword());

        string strFullPath = string.Format("{0}?{1}", strPath, strParams);

        XmlDocument doc = PostToSQ(strFullPath);
        XmlNode xmlResult = doc.SelectSingleNode("/apiresult/data/id");

        int intID = 0;

        if (xmlResult != null)
            int.TryParse(xmlResult.InnerText, out intID);

        if (boolIsTest)
        {
            strPath = "https://restapi.surveygizmo.com/V3/survey.xml";

            strParams = string.Format("_method=get&{0}&filter[field][0]=title&filter[operator][0]==&filter[value][0]={1}", UserNamePassword(), intResearchID);

            strFullPath = string.Format("{0}?{1}", strPath, strParams);

            doc = PostToSQ(strFullPath);
            xmlResult = doc.SelectSingleNode("/apiresult/data/_0/id");

            if (xmlResult != null)
                int.TryParse(xmlResult.InnerText, out intID);

            if (intID > 0)
            {
                strPath = string.Format(@"https://restapi.surveygizmo.com/V3/survey/{0}.xml", intID) ;
                strParams = string.Format("_method=post&type=survey&title={0}&{1}", strTitle, UserNamePassword());

                strFullPath = string.Format("{0}?{1}", strPath, strParams);

                doc = PostToSQ(strFullPath);
                xmlResult = doc.SelectSingleNode("/apiresult/data/_0/id");
            }
        }

        return intID;
    }

    /// <summary>
    /// SurveyGizmo Call
    /// - Creates the campaign URL.
    /// </summary>
    /// <param name="intResearchID">Research ID.</param>
    /// <param name="strCampaignName">Campaign Name</param>
    /// <returns>Returns Campaign ID (System.String)</returns>
    public string CreateCampaignURL(int intResearchID, string strCampaignName)
    {
        string strURI = string.Empty;

        ResearchMain entResearchMain = DataRepository.ResearchMainProvider.GetByAutoResearchID(intResearchID);

        if (entResearchMain != null)
        {
            int intSurveyID = 0;

            int.TryParse(entResearchMain.IntSGSurveyID.ToString(), out intSurveyID);

            string strPath = string.Format("https://restapi.surveygizmo.com/V3/survey/{0}/surveycampaign.xml", intSurveyID);
            string strParams = string.Format("_method=put&name={1}&type=link&{0}&SSL=True", UserNamePassword(), strCampaignName);

            string strFullPath = string.Format("{0}?{1}", strPath, strParams);

            XmlDocument doc = PostToSQ(strFullPath);
            XmlNodeList xmlList = doc.SelectNodes("/apiresult/data");

            if (entResearchMain != null)
            {
                entResearchMain.StrResearchURLDemo = string.Format("http://appv3.sgizmo.com/testsurvey/survey?id={0}", intSurveyID);
                DataRepository.ResearchMainProvider.Save(entResearchMain);
            }

            if (xmlList.Count > 0)
            {
                for (int i = 0; i < xmlList.Count; i++)
                {
                    XmlNode xmlResult = xmlList[i];

                    string strName = clsPublic.GetXMLText(xmlResult, "name");

                    if (strName.ToUpper() == "PUBLIC")
                    {
                        strURI = clsPublic.GetXMLText(xmlResult, "uri");

                        if (entResearchMain != null)
                        {
                            if (strURI.Length > 7)
                            {
                                if (strURI.Substring(0, 7) != "http://")
                                {
                                    strURI = "http://" + strURI;
                                }
                            }

                            entResearchMain.StrResearchURL = strURI;
                            DataRepository.ResearchMainProvider.Save(entResearchMain);
                        }

                        break;
                    }
                }
            }
        }

        return strURI;

    }

    /// <summary>
    /// SurveyGizmo Call
    /// - Gets the campaign URL.
    /// </summary>
    /// <param name="intResearchID">Research ID.</param>
    /// <returns>Returns Campaign URL (System.String)</returns>
    public string GetCampaignURL(int intResearchID)
    {
        string strURI = string.Empty;

        ResearchMain entResearchMain = DataRepository.ResearchMainProvider.GetByAutoResearchID(intResearchID);

        if (entResearchMain != null)
        {
            int intSurveyID = 0;

            int.TryParse(entResearchMain.IntSGSurveyID.ToString(), out intSurveyID);

            string strPath = string.Format("https://restapi.surveygizmo.com/V3/survey/{0}/surveycampaign.xml", intSurveyID);
            string strParams = string.Format("_method=get&{0}", UserNamePassword());

            string strFullPath = string.Format("{0}?{1}", strPath, strParams);

            XmlDocument doc = PostToSQ(strFullPath);
            XmlNodeList xmlList = doc.SelectNodes("/apiresult/data/SurveyCampaign");


            if (xmlList.Count > 0)
            {
                for (int i = 0; i < xmlList.Count; i++)
                {
                    XmlNode xmlResult = xmlList[i];

                    string strName = clsPublic.GetXMLText(xmlResult, "name");


                    if (strName.ToUpper() == "DEFAULT LINK")
                    {
                        strURI = clsPublic.GetXMLText(xmlResult, "uri");

                        if (entResearchMain != null)
                        {
                            entResearchMain.StrResearchURL = strURI;
                            DataRepository.ResearchMainProvider.Save(entResearchMain);
                        }

                        break;
                    }

                    
                }

            }
        }

        return strURI;
    }

    /// <summary>
    /// SurveyGizmo Call
    /// - Creates a new page in the survey
    /// </summary>
    /// <param name="intSurveyID">SurveyGizmo Survey ID</param>
    /// <param name="intPageNumber">Page Number</param>
    /// <param name="strTitle">Page Title</param>
    /// <param name="strDescription">Page Description</param>
    /// <returns>Returns Page ID (System.Int32)</returns>
    public int CreateNewPage(int intSurveyID, int intPageNumber, string strTitle, string strDescription)
    {
        strTitle = HttpUtility.UrlEncode(strTitle);
        strDescription = HttpUtility.UrlEncode(strDescription);

        string strPath = string.Format("https://restapi.surveygizmo.com/V3/survey/{0}/surveypage.xml", intSurveyID);
        string strParams = string.Format("_method=put&title[English]={1}&after={2}&{3}", intSurveyID, strTitle, intPageNumber - 1, UserNamePassword());

        string strFullPath = string.Format("{0}?{1}", strPath, strParams);

        XmlDocument doc = PostToSQ(strFullPath);
        XmlNode xmlResult = doc.SelectSingleNode("/apiresult/data/id");

        int intID = 0;

        if (xmlResult != null)
            int.TryParse(xmlResult.InnerText, out intID);

        return intID;
    }

    /// <summary>
    /// SurveyGizmo Call
    /// - Creates a new question.
    /// </summary>
    /// <param name="questionTypeExpr">SurveyGizmo Question Type</param>
    /// <param name="intSurveyID">SurveyGizmo Survey ID</param>
    /// <param name="intPageNumber">Page Number</param>
    /// <param name="strTitle">Question Title</param>
    /// <param name="strDescription">Question Text</param>
    /// <param name="intAfterQuestion">After which question ID</param>
    /// <param name="strAddedParameters">Additional Question Parameters</param>
    /// <returns>Returns Question ID (System.Int32)</returns>
    public int CreateNewQuestion(SGQuestionTypes questionTypeExpr, int intSurveyID, int intPageNumber, string strTitle, string strDescription, int intAfterQuestion, string strAddedParameters)
    {
        string strTitleTemp = strTitle;
        bool boolLongTitle = false;

        strTitle = HttpUtility.UrlEncode(strTitle);
        strDescription = HttpUtility.UrlEncode(strDescription);

        if (strTitle.Length > 1000)
        {
            strTitle = string.Empty;
            boolLongTitle = true;
        }

        int intLen = strTitle.Length;

        string strPath = string.Format("https://restapi.surveygizmo.com/V3/survey/{0}/surveypage/{1}/surveyquestion.xml", intSurveyID, intPageNumber);
        string strParams = string.Empty;

        //_method=PUT&title[English]=ExTitle&type=urlredirect&properties[url]=http://www.surveygizmo.com&properties[outbound][0][fieldname]=variableName&properties[outbound][0][mapping]=16&properties[outbound][0][default]=DefaultValue

        if (questionTypeExpr == SGQuestionTypes.hidden)
            strParams = string.Format("_method=put&type={0}&{2}&title[English]={1}&properties[required]=true&after={3}&properties[defaulttext][English]={4}", questionTypeExpr.ToString(), strTitle, UserNamePassword(), intAfterQuestion, strDescription);

        else if (questionTypeExpr == SGQuestionTypes.httpinsert)
            strParams = string.Format("_method=put&type={0}&{2}&title[English]={1}&after={3}", questionTypeExpr.ToString(), strTitle, UserNamePassword(), intAfterQuestion);


        else if (questionTypeExpr != SGQuestionTypes.instructions && questionTypeExpr != SGQuestionTypes.video && questionTypeExpr != SGQuestionTypes.image)
            strParams = string.Format("_method=put&type={0}&{2}&title[English]={1}&properties[required]=true&after={3}", questionTypeExpr.ToString(), strTitle, UserNamePassword(), intAfterQuestion);

        else if (questionTypeExpr == SGQuestionTypes.urlredirect && intPageNumber==2)
            strParams = string.Format("_method=put&type={0}&{2}&title[English]={1}}&properties[Disqualify]=true&after={3}", questionTypeExpr.ToString(), strTitle, UserNamePassword(), intAfterQuestion);

        else
            strParams = string.Format("_method=put&type={0}&{2}&title[English]={1}&after={3}", questionTypeExpr.ToString(), strTitle, UserNamePassword(), intAfterQuestion);


        if (strAddedParameters.Length > 0)
        {
            strParams = string.Concat(strParams, "&", strAddedParameters);
        }

        string strFullPath = string.Format("{0}?{1}", strPath, strParams);

        XmlDocument doc = PostToSQ(strFullPath);
        XmlNode xmlResult = doc.SelectSingleNode("/apiresult/data/id");

        int intID = 0;

        if (xmlResult != null)
            int.TryParse(xmlResult.InnerText, out intID);


        if (boolLongTitle)
        {
            PostSubmitter post = new PostSubmitter();

            post.Url = string.Format("https://restapi.surveygizmo.com/V3/survey/{0}/surveypage/{1}/surveyquestion/{2}.xml", intSurveyID, intPageNumber, intID);
            //System.Configuration.ConfigurationManager.AppSettings[""]
            //post.PostItems.Add("user:pass", "BMorse@decisionquest.com:DecisionQuest21535");
            if (System.Configuration.ConfigurationManager.AppSettings["SG-Testing"].ToString().ToLower().Trim() == "false")
            {
                post.PostItems.Add("user:pass", "BMorse@decisionquest.com:DecisionQuest21535");
            }
            else
            {
                post.PostItems.Add("user:pass", "mukesh.tungaria@exavalu.com:Mukesh@1");
            }
            
            post.PostItems.Add("_method", "PUT");
            post.PostItems.Add("title", strTitleTemp);
            post.Type = PostSubmitter.PostTypeEnum.Post;
            string result = post.Post();
        }

        return intID;
    }

    /// <summary>
    /// SurveyGizmo Call
    /// - Checks the status of the current open surveys in SurveyGizmo
    /// </summary>
    public void CheckSurveys()
    {
        ResearchMainQuery query1 = new ResearchMainQuery(true, true);
        query1.AppendIsNotNull(ResearchMainColumn.IntSGSurveyID);
        query1.AppendIsNull(ResearchMainColumn.DatResearchResultsReceived);

        TList<ResearchMain> tlstResearchMain = DataRepository.ResearchMainProvider.Find(query1);

        tlstResearchMain.ForEach(delegate(ResearchMain entResearchMain)
        {
            string[] strQuestTypes = new string[] {"3","7","8"};

            ResearchRespondersQuery query2 = new ResearchRespondersQuery(true, true);
            query2.AppendEquals(ResearchRespondersColumn.IntRespondersResearchID, entResearchMain.AutoResearchID.ToString());
            query2.AppendIn(ResearchRespondersColumn.IntRespondersType, strQuestTypes);
            query2.AppendGreaterThan(ResearchRespondersColumn.IntRespondersCount, "0");

            TList<ResearchResponders> tlstResearchResponders = DataRepository.ResearchRespondersProvider.Find(query2);

            if (entResearchMain.AutoResearchID < 2429)
            {
                
            }
		
            else
            {
            if (tlstResearchResponders.Count > 0)
            {
                if (tlstResearchResponders.Count == 1)
                {
                    int intCount = tlstResearchResponders[0].IntRespondersCount;

                    if (CheckSurveyResponseCount(entResearchMain.AutoResearchID, entResearchMain.IntSGSurveyID, intCount))
                    {
                        int intPages = (int)Math.Ceiling((double)intCount / (double)500);
                        GetSurveyResponses(entResearchMain.AutoResearchID, 1, true);
                    }

                }
                else
                {
                    clsPublic.SendMessage(string.Format("Too many responder types for research ID {0} have been found.", entResearchMain.AutoResearchID));
                }
            }
            else
            {
                clsPublic.SendMessage(string.Format("No responders for research ID {0} have been found.", entResearchMain.AutoResearchID));
            }
            }

        });

       

    }

    /// <summary>
    /// SurveyGizmo Call
    /// - Checks the open survey response count.
    /// </summary>
    /// <param name="intResearchID">Research ID.</param>
    /// <param name="intSurveyID">SurveyGizmo Survey ID</param>
    /// <param name="intTargetCount">Count expected for survey</param>
    /// <returns><c>true</c> if Count of responses meets or exceeds, <c>false</c> otherwise</returns>
    public bool CheckSurveyResponseCount(int intResearchID, int? intSurveyID, int intTargetCount)
    {
        bool boolSurveyComplete = false;


        if (intSurveyID.HasValue)
        {
            string strPath = string.Format("https://restapi.surveygizmo.com/V3/survey/{0}.xml", intSurveyID);
            string strParams = string.Format("{0}", UserNamePassword());
            string strFullPath = string.Format("{0}?{1}", strPath, strParams);

            XmlDocument doc = PostToSQ(strFullPath);
            XmlNodeList xmlList = doc.SelectNodes("/apiresult/data/statistics/Complete");
            XmlNodeList xmlListTest = doc.SelectNodes("/apiresult/data/statistics/TestData");

            if (xmlList.Count > 0)
            {
                string strCount = xmlList[0].InnerText;

                int intCompleteTotal = 0;
                int intTestTotal = 0;

                int.TryParse(strCount, out intCompleteTotal);

                if (xmlListTest.Count > 0)
                {
                    string strTestCount = xmlListTest[0].InnerText;
                    int.TryParse(strTestCount, out intTestTotal);
                }



                if ((intCompleteTotal - intTestTotal) >= intTargetCount)
                {
                    boolSurveyComplete = true;
                }
            }
        }

        return boolSurveyComplete;

    }

    /// <summary>
    /// SurveyGizmo Call
    /// - Gets the survey respondents responses.
    /// </summary>
    /// <param name="intResearchID">Research ID.</param>
    /// <param name="intPage">Survey Page Number</param>
    /// <param name="boolRemoveDefinitionsandData">if set to <c>true</c> [bool remove definitionsand data].</param>
    public void GetSurveyResponses(int intResearchID, int intPage, bool boolRemoveDefinitionsandData)
    {
        if (intPage == 1)
            LoadSurveyDefinition(intResearchID);

        ResearchMain entResearchMain = DataRepository.ResearchMainProvider.GetByAutoResearchID(intResearchID);

        if (entResearchMain != null)
        {
            TList<ResearchQuestions> tlstResearchQuestions = DataRepository.ResearchQuestionsProvider.GetByIntQuesResearchID(intResearchID);

            int intSurveyID = 0;

            int.TryParse(entResearchMain.IntSGSurveyID.ToString(), out intSurveyID);

            if (intSurveyID > 0)
            {
                CloseSurvey(intSurveyID);

                string strPath = string.Format("https://restapi.surveygizmo.com/V2/survey/{0}/surveyresponse.xml", intSurveyID);
                string strParams = string.Format("page={1}&resultsperpage=500&{0}&filter[field][0]=status&filter[operator][0]==&filter[value][0]=Complete", UserNamePassword(), intPage);
                string strFullPath = string.Format("{0}?{1}", strPath, strParams);

                XmlDocument doc = PostToSQ(strFullPath);
                XmlNodeList xmlList = doc.SelectNodes("/apiresult/data/unknownNode");

                // Loop over each data record
                foreach (XmlNode xmlnodeTemp in xmlList)
                {
                    string strStatus = xmlnodeTemp["status"].InnerText;
                    string strID = xmlnodeTemp["id"].InnerText;

                    int intID = 0;

                    int.TryParse(strID, out intID);

                    if (strStatus.ToUpper() == "COMPLETE")
                    {
                        int intQuestionID = 0;

                        // loop over each data response per respondent.
                        foreach (XmlNode xmlnodeDataTemp in xmlnodeTemp.ChildNodes)
                        {
                            if (xmlnodeDataTemp.Name == "datapoint")
                            {
                                string str = xmlnodeDataTemp["fieldname"].InnerText;

                                string[] strResult = str.Replace("(", "").Replace(")", "").Replace("question", "").Replace("option", "").Split(new char[] { ',', '[', ']' }, StringSplitOptions.RemoveEmptyEntries);

                                int.TryParse(strResult[0], out intQuestionID);

                                string strValue = xmlnodeDataTemp["value"].InnerText;

                                if (strResult.Length > 1)
                                {
                                    if (strValue.Length > 0)
                                    {
                                        strValue = strResult[1];
                                    }
                                }

                                ResearchQuestions entResearchQuestion2 = tlstResearchQuestions.Find(delegate(ResearchQuestions entResearchQuestion) { return entResearchQuestion.IntSGQuestionID == intQuestionID; });

                                if (entResearchQuestion2 != null)
                                {
                                    if (entResearchQuestion2.IntQuesType == 1)
                                    {
                                        ResearchAnswers entResearchAnswers = new ResearchAnswers();
                                        entResearchAnswers.IntAnswerResearchID = intResearchID;
                                        entResearchAnswers.IntAnswerQuestionID = entResearchQuestion2.AutoQuesID;
                                        entResearchAnswers.IntAnswerUserID = intID;

                                        entResearchAnswers.IntAnswerResponseID = 0;
                                        entResearchAnswers.MemAnswerOpenEnd = strValue;
                                        entResearchAnswers.IntSGAnswerID = intQuestionID;
                                        entResearchAnswers.BitSGCompletedData = true;

                                        DataRepository.ResearchAnswersProvider.Save(entResearchAnswers);
                                    }
                                    else
                                    {
                                        if (strResult.Length > 1)
                                        {
                                            if (strValue.Length > 0)
                                            {
                                                string strValueTemp = strResult[1];

                                                SaveResearchAnswer(strValueTemp.Trim() == "0" ? strValue : strValueTemp, intResearchID, intID, intQuestionID, strValueTemp.Trim() == "0" ? true : false, entResearchQuestion2.AutoQuesID);
                                            }
                                        }
                                        else
                                        {
                                            SaveResearchAnswer(strValue, intResearchID, intID, intQuestionID, false, entResearchQuestion2.AutoQuesID);
                                        }
                                    }
                                }

                            }  // End data point record
                            else if (xmlnodeDataTemp.Name == "variable")
                            {
//                                SaveResearchAnswer(xmlnodeDataTemp.InnerText, intResearchID, intID, intQuestionID, false,0);
                            }

                        }  // End data variable loop

                    }  // End completed records

                    System.Data.Common.DbCommand cmdUpdateAnswerTable = clsUtilities.GetDBCommand("up_paramupd_UpdateResearchAnswerText");

                    clsUtilities.AddParameter(ref cmdUpdateAnswerTable, "@ResearchID", ParameterDirection.Input, DbType.Int32, intResearchID);
                    clsUtilities.SQLExecuteNonQuery(cmdUpdateAnswerTable);


                }  // End loop

                entResearchMain.DatResearchResultsReceived = DateTime.Now;
                DataRepository.ResearchMainProvider.Save(entResearchMain);

                //Code to send email to intimate user
                clsSendMessages.SendResultEmail(entResearchMain.StrResearchSurveyName);
                //Intimation code ends here
            }  // Has Survey ID
        }
    }

    /// <summary>
    /// SurveyGizmo Call
    /// - Gets the survey respondents responses.
    /// </summary>
    /// <param name="intResearchID">Research ID.</param>
    /// <param name="intPage">Survey Page Number</param>
    /// <param name="boolRemoveDefinitionsandData">if set to <c>true</c> [bool remove definitionsand data].</param>
    public XmlNode GetSurveyResponsesFromGizmo(int intResearchID)
    {
        ResearchMain entResearchMain = DataRepository.ResearchMainProvider.GetByAutoResearchID(intResearchID);

        if (entResearchMain != null)
        {
            TList<ResearchQuestions> tlstResearchQuestions = DataRepository.ResearchQuestionsProvider.GetByIntQuesResearchID(intResearchID);

            int intSurveyID = 0;

            int.TryParse(entResearchMain.IntSGSurveyID.ToString(), out intSurveyID);

            if (intSurveyID > 0)
            {
                string strPath = string.Format("https://restapi.surveygizmo.com/V2/survey/{0}/surveyresponse.xml", intSurveyID);
                string strParams = string.Format("page={1}&resultsperpage=500&{0}&filter[field][0]=status&filter[operator][0]==&filter[value][0]=Complete", UserNamePassword(), 1);
                string strFullPath = string.Format("{0}?{1}", strPath, strParams);

                XmlDocument doc = PostToSQ(strFullPath);
                XmlNodeList xmlList = doc.SelectNodes("/apiresult/data/unknownNode");
                return xmlList[0];
            }  // Has Survey ID
        }
        return null;
    }

    /// <summary>
    /// SurveyGizmo Call
    /// - Gets the survey List.
    /// </summary>
    public List<SurveyListFromGizmo> GetSurveyListFromGizmo()
    {
        List<SurveyListFromGizmo> tlist = new List<SurveyListFromGizmo>();
        string strPath = "https://restapi.surveygizmo.com/V2/survey.xml";
        string strParams = string.Format("page={1}&resultsperpage=500&{0}&filter[field][0]=status&filter[operator][0]==&filter[value][0]=Launched", UserNamePassword(), 1);
        string strFullPath = string.Format("{0}?{1}", strPath, strParams);

        XmlDocument doc = PostToSQ(strFullPath);
        XmlNodeList xmlNodeList = doc.SelectNodes("/apiresult/data/unknownNode");
        foreach (XmlNode xmlnode in xmlNodeList)
        {
            long sid = Convert.ToInt64(xmlnode["id"].InnerText);
            if (sid > 1280267)
            {
                SurveyListFromGizmo slist = new SurveyListFromGizmo();
		        slist.Sid = sid;
                slist.SurveyName = xmlnode["title"].InnerText;
                slist.CreatedDate = Convert.ToDateTime(xmlnode["createdon"].InnerText);
                slist.Status = xmlnode["status"].InnerText;


                foreach (XmlNode xml in xmlnode["statistics"].ChildNodes)
                {
                    switch (xml.Name)
                    {
                        case "Partial":
                            slist.Partial = Convert.ToInt32(xml.InnerText);
                            break;
                        case "Complete":
                            slist.Complete = Convert.ToInt32(xml.InnerText);
                            break;
                        case "Disqualified":
                            slist.Disqualified = Convert.ToInt32(xml.InnerText);
                            break;
                        case "Deleted":
                            slist.Deleted = Convert.ToInt32(xml.InnerText);
                            break;

                    }
                }
                slist.Partial = slist.Partial == null ? 0 : slist.Partial;
                slist.Complete = slist.Complete == null ? 0 : slist.Complete;
                slist.Disqualified = slist.Disqualified == null ? 0 : slist.Disqualified;
                slist.Deleted = slist.Deleted == null ? 0 : slist.Deleted;
		        slist.TotalPages = slist.Partial + slist.Complete + slist.Disqualified + slist.Deleted;
                tlist.Add(slist);
            }
        }
        return tlist;
    }

    /// <summary>
    /// SurveyGizmo Call
    /// Gets the survey respond List.
    /// </summary>
    public DataTable GetSurveyRespondListFromGizmo(String SID, int intResultperPage)
    {
        DataTable dtRespondList = new DataTable();
        int partialRowIndex = 0, CompleteRowIndex = 0, DisqualifiedRowIndex = 0, DeletedRowIndex = 0;
        dtRespondList.Columns.Add("Partial", typeof(string)); dtRespondList.Columns.Add("Complete", typeof(string));
        dtRespondList.Columns.Add("Disqualified", typeof(string)); dtRespondList.Columns.Add("Deleted", typeof(string));

        string strPath = string.Format("https://restapi.surveygizmo.com/v3/survey/{0}/surveyresponse.xml", SID);
        string strParams = string.Format("page=1&resultsperpage={0}&{1}", intResultperPage,UserNamePassword());
        string strFullPath = string.Format("{0}?{1}", strPath, strParams);
        //String Time = DateTime.Now.ToString("hh:mm:ss:sss");
        XmlDocument doc = PostToSQ(strFullPath);
        //Time += ": " + DateTime.Now.ToString("hh:mm:ss:sss");
        int total_count =Convert.ToInt32(doc.SelectNodes("/apiresult/total_count")[0].InnerText);
        for (int n = 0; n < total_count; n++)
        {
            XmlNodeList xmlNodeList = doc.SelectNodes("/apiresult/data/_" + n.ToString());
            if (xmlNodeList.Count > 0)
                dtRespondList = SetRespondData(dtRespondList, xmlNodeList, partialRowIndex, CompleteRowIndex, DisqualifiedRowIndex, DeletedRowIndex, out partialRowIndex, out CompleteRowIndex, out DisqualifiedRowIndex, out DeletedRowIndex);
        }
        //Time += ": " + DateTime.Now.ToString("hh:mm:ss:sss");
        return dtRespondList;
    }

    /// <summary>
    /// Set Respond Table Data
    /// </summary>
    /// <param name="dtRespondList"></param>
    /// <param name="xmlNodeList"></param>
    /// <param name="_partialRowIndex"></param>
    /// <param name="_CompleteRowIndex"></param>
    /// <param name="_DisqualifiedRowIndex"></param>
    /// <param name="_DeletedRowIndex"></param>
    /// <param name="partialRowIndex"></param>
    /// <param name="CompleteRowIndex"></param>
    /// <param name="DisqualifiedRowIndex"></param>
    /// <param name="DeletedRowIndex"></param>
    /// <returns></returns>
    private DataTable SetRespondData(DataTable dtRespondList, XmlNodeList xmlNodeList, int _partialRowIndex, int _CompleteRowIndex, int _DisqualifiedRowIndex, int _DeletedRowIndex, out int partialRowIndex, out int CompleteRowIndex, out int DisqualifiedRowIndex, out int DeletedRowIndex)
    {
        String status = xmlNodeList[0]["status"].InnerText;
        String gMIID = xmlNodeList[0]["question4option0"].InnerText;

        switch (status)
        {
            case "Partial":
                if (dtRespondList.Rows.Count == _partialRowIndex)
                {
                    dtRespondList.Rows.Add(gMIID, null, null, null);
                }
                else
                {
                    dtRespondList.Rows[_partialRowIndex]["Partial"] = gMIID;
                    dtRespondList.AcceptChanges();
                }
                _partialRowIndex++;
                break;
            case "Complete":
                if (dtRespondList.Rows.Count == _CompleteRowIndex)
                {
                    dtRespondList.Rows.Add(null, gMIID, null, null);
                }
                else
                {
                    dtRespondList.Rows[_CompleteRowIndex]["Complete"] = gMIID;
                    dtRespondList.AcceptChanges();
                }
                _CompleteRowIndex++;
                break;
            case "Disqualified":
                if (dtRespondList.Rows.Count == _DisqualifiedRowIndex)
                {
                    dtRespondList.Rows.Add(null, null, gMIID, null);
                }
                else
                {
                    dtRespondList.Rows[_DisqualifiedRowIndex]["Disqualified"] = gMIID;
                    dtRespondList.AcceptChanges();
                }
                _DisqualifiedRowIndex++;
                break;
            case "Deleted":
                if (dtRespondList.Rows.Count == _DeletedRowIndex)
                {
                    dtRespondList.Rows.Add(null, null, null, gMIID);
                }
                else
                {
                    dtRespondList.Rows[_DeletedRowIndex]["Deleted"] = gMIID;
                    dtRespondList.AcceptChanges();
                }
                _DeletedRowIndex++;
                break;

        }
        partialRowIndex = _partialRowIndex; CompleteRowIndex = _CompleteRowIndex; DisqualifiedRowIndex = _DisqualifiedRowIndex; DeletedRowIndex = _DeletedRowIndex;
        return dtRespondList;
    }

    /// <summary>
    /// SurveyGizmo Call
    /// CheckValidateSurveyQuestion.
    /// </summary>
    public void CheckValidateSurveyQuestion(int intResearchID)
    {
        String strBody = "";
        bool isSurveyOk = false;
        int NoOfDBQns = 0, NoOfSGQns = 0;
        List<clsQuestion> lstQuestion = new List<clsQuestion>();
        List<clsAllQuestionOptions> lstAllQuestionOptions = new List<clsAllQuestionOptions>();
        List<SGErrorQuestion> lstSGErrorQuestion = new List<SGErrorQuestion>();

        ResearchMain entResearchMain = DataRepository.ResearchMainProvider.GetByAutoResearchID(intResearchID);

        if (entResearchMain != null)
        {
            int intSurveyID = 0;
            int.TryParse(entResearchMain.IntSGSurveyID.ToString(), out intSurveyID);
            if (intSurveyID > 0)
            {
                string strPath = string.Format("https://restapi.surveygizmo.com/V2/survey/{0}/surveyquestion.xml", intSurveyID);
                string strParams = string.Format("page=1&resultsperpage=500&{0}", UserNamePassword());
                string strFullPath = string.Format("{0}?{1}", strPath, strParams);

                XmlDocument doc = PostToSQ(strFullPath);
                XmlNodeList xmlList = doc.SelectNodes("/apiresult/data/unknownNode");
                foreach (XmlNode xmlnode in xmlList)
                {
                    clsQuestion clsQuestionTemp = new clsQuestion();
                    clsQuestionTemp.QuestionID = Convert.ToInt32(xmlnode["id"].InnerText);
                    clsQuestionTemp.QuestionText = xmlnode["title"].SelectSingleNode("English") == null ? "" : xmlnode["title"].SelectSingleNode("English").InnerText;
                    lstQuestion.Add(clsQuestionTemp);
                    if (xmlnode["options"] != null)
                    {
                        XmlNodeList xmlOptionList = xmlnode["options"].SelectNodes("unknownNode");
                        if (xmlOptionList != null)
                        {
                            foreach (XmlNode xmlOptionListTemp in xmlOptionList)
                            {
                                clsAllQuestionOptions clsAllQuestionOptionsTemp = new clsAllQuestionOptions();
                                clsAllQuestionOptionsTemp.OptionID = Convert.ToInt32(xmlOptionListTemp["id"].InnerText);
                                clsAllQuestionOptionsTemp.OptionText = xmlOptionListTemp["title"].SelectSingleNode("English").InnerText;
                                clsAllQuestionOptionsTemp.QuesID = clsQuestionTemp.QuestionID;
                                lstAllQuestionOptions.Add(clsAllQuestionOptionsTemp);
                            }
                        }
                    }
                }
            }  // Has Survey ID
        }

        if (lstQuestion.Count > 0)
        {
            TList<ResearchQuestions> tlstResearchQuestions = DataRepository.ResearchQuestionsProvider.GetByIntQuesResearchID(intResearchID);
            tlstResearchQuestions = tlstResearchQuestions.FindAll(delegate(ResearchQuestions rfd) { return rfd.IntSGQuestionID != null; });
            
            if (tlstResearchQuestions.FindAllDistinct("IntSGQuestionID").Count != tlstResearchQuestions.Count)
            {
                TList<ResearchQuestions> tlstVals = tlstResearchQuestions.FindAll(delegate(ResearchQuestions RQi) { return tlstResearchQuestions.FindAll(delegate(ResearchQuestions RQj) { return RQj.IntSGQuestionID == RQi.IntSGQuestionID; }).Count() > 1; });//.FindAllDistinct("IntSGQuestionID");
                foreach(ResearchQuestions tlstValsTemp in tlstVals)
                {
                    isSurveyOk = true;
                    lstSGErrorQuestion.Add(SetSGErrorQuestionValue(tlstValsTemp, tlstValsTemp.IntQuesOrder, "SGID is not unique."));

                }
            }
            foreach (ResearchQuestions ResearchQuestiontemp in tlstResearchQuestions)
            {
                NoOfDBQns++;
                clsQuestion clsQuestionTemp = lstQuestion.Find(delegate(clsQuestion rfd) { return rfd.QuestionID == ResearchQuestiontemp.IntSGQuestionID; });
                if (clsQuestionTemp != null)
                {
                    NoOfSGQns++;
                    if (!clsQuestionTemp.QuestionText.Equals(ResearchQuestiontemp.StrQuesText) && ResearchQuestiontemp.IntQuesType != 12 && ResearchQuestiontemp.IntQuesType != 13)
                    {
                        isSurveyOk = true;
                        lstSGErrorQuestion.Add(SetSGErrorQuestionValue(ResearchQuestiontemp, ResearchQuestiontemp.IntQuesOrder, "Question Text is not identical."));
                    }

                    TList<ResearchQuestionsResponses> tlstResearchQuestionsResponses = DataRepository.ResearchQuestionsResponsesProvider.GetByIntQuesRespQuesID(ResearchQuestiontemp.AutoQuesID);
                    List<clsAllQuestionOptions> clsAllQuestionOptionsTemp = lstAllQuestionOptions.FindAll(delegate(clsAllQuestionOptions rfd) { return rfd.QuesID == clsQuestionTemp.QuestionID; });
                    if (tlstResearchQuestionsResponses.Count != clsAllQuestionOptionsTemp.Count && ResearchQuestiontemp.IntQuesType != 2)
                    {
                        isSurveyOk = true;
                        lstSGErrorQuestion.Add(SetSGErrorQuestionValue(ResearchQuestiontemp, ResearchQuestiontemp.IntQuesOrder, "No of Responses is not identical."));
                    }
                    foreach (ResearchQuestionsResponses ResearchQuestionsResponsestemp in tlstResearchQuestionsResponses)
                    {
                        clsAllQuestionOptions clsQuestionOptionsTemp = clsAllQuestionOptionsTemp.Find(delegate(clsAllQuestionOptions rfd) { return rfd.OptionID == ResearchQuestionsResponsestemp.IntSGResponseID; });
                        if (clsQuestionOptionsTemp == null)
                        {
                            isSurveyOk = true;
                            lstSGErrorQuestion.Add(SetSGErrorQuestionValue(ResearchQuestiontemp, ResearchQuestiontemp.IntQuesOrder, "Question Response (" + ResearchQuestionsResponsestemp.IntSGResponseID + ") is not available."));
                        }
                    }
                }
            }

            if (NoOfDBQns != NoOfSGQns)
            {
                isSurveyOk = true;
                lstSGErrorQuestion.Add(SetSGErrorQuestionValue(null, 0, "Difference in no of questions."));
            }
        }

        if (!isSurveyOk)
            strBody = "Survey '" + entResearchMain.StrResearchSurveyName + "' Submitted Successfully.";
        else
        {
            int ncount = 0;
            strBody += "<table style='border:0px solid #ffffff;width: 733px;' cellpadding='0' cellspacing='0'><tr style='background-color:#00477E;font-weight:bold;color:White;height:30px;'><th style='padding:0px 5px;text-align:left;font-family: Calibri;'>QuestionID</th><th style='padding:0px 5px;text-align:left;font-family: Calibri;'>Question Text</th><th style='padding:0px 5px;text-align:left;font-family: Calibri;'>SGID</th><th style='padding:0px 5px;text-align:left;font-family: Calibri;'>Remarks</th></tr>";
            foreach (SGErrorQuestion SGErrorQuestiontemp in lstSGErrorQuestion)
            {
                if (ncount % 2 == 0)
                {
                    strBody += "<tr style='background-color:White;color:#284775;'>"
                                + "<td style='padding:5px 5px 0px 5px;text-align:left;font-family: Calibri;'>" + SGErrorQuestiontemp.QuesID + "</td>"
                                + "<td style='padding:5px 5px 0px 5px;text-align:left;font-family: Calibri;'>" + SGErrorQuestiontemp.QuestionText + "</td>"
                                + "<td style='padding:5px 5px 0px 5px;text-align:left;font-family: Calibri;'>" + SGErrorQuestiontemp.SGID + "</td>"
                                + "<td style='padding:5px 5px 0px 5px;text-align:left;font-family: Calibri;'>" + SGErrorQuestiontemp.Remarks + "</td></tr>";
                }
                else
                {
                    strBody += "<tr style='background-color:#F7F6F3;color:#333333;'>"
                                + "<td style='padding:5px 5px 0px 5px;text-align:left;font-family: Calibri;'>" + SGErrorQuestiontemp.QuesID + "</td>"
                                + "<td style='padding:5px 5px 0px 5px;text-align:left;font-family: Calibri;'>" + SGErrorQuestiontemp.QuestionText + "</td>"
                                + "<td style='padding:5px 5px 0px 5px;text-align:left;font-family: Calibri;'>" + SGErrorQuestiontemp.SGID + "</td>"
                                + "<td style='padding:5px 5px 0px 5px;text-align:left;font-family: Calibri;'>" + SGErrorQuestiontemp.Remarks + "</td></tr>";
                }
                ncount++;
            }
            strBody += "</table>";
        }

        clsSendMessages.SendErrorSGQuestionEmail(strBody);
    }

    /// <summary>
    /// Set SGErrorQuestionValue
    /// </summary>
    /// <param name="ResearchQuestiontemp"></param>
    /// <param name="intQuesID"></param>
    /// <param name="Remarks"></param>
    /// <returns></returns>
    private SGErrorQuestion SetSGErrorQuestionValue(ResearchQuestions ResearchQuestiontemp, int intQuesID, String Remarks)
    {
        SGErrorQuestion SGErrorQuestionTemp = new SGErrorQuestion();
        int IntSGQuesID = 0;
        if (ResearchQuestiontemp != null)
            int.TryParse(ResearchQuestiontemp.IntSGQuestionID.ToString(), out IntSGQuesID);
        SGErrorQuestionTemp.QuesID = intQuesID;
        SGErrorQuestionTemp.QuestionText = ResearchQuestiontemp == null ? "No Text" : ResearchQuestiontemp.StrQuesText;
        SGErrorQuestionTemp.SGID = IntSGQuesID;
        SGErrorQuestionTemp.Remarks = Remarks;
        return SGErrorQuestionTemp;
    }

    public void TestJavascript()
    {
        int intSurveyID =  this.CreateNewSurvey("Testing Javascript", false, 296);

        string strJavaScript = "<script type='text/JavaScript'>alert('hi');window.location.href='http://www.recruittrack.com';</script>";


        int intQuestionID = CreateNewQuestion(SGQuestionTypes.instructions, intSurveyID, 1, strJavaScript, string.Empty, 0, string.Empty);

    }


    /// <summary>
    /// SurveyGizmo Call
    /// - Loads the survey definition.
    /// </summary>
    /// <param name="intResearchID">Research ID.</param>
    protected void LoadSurveyDefinition(int intResearchID)
    {
        // Remove existing response questions and answers
        System.Data.Common.DbCommand cmdResearchResultTables = clsUtilities.GetDBCommand("_up_paramdel_ResearchResultTables");

        clsUtilities.AddParameter(ref cmdResearchResultTables, "@ResearchID", ParameterDirection.Input, DbType.Int32, intResearchID);
        clsUtilities.SQLExecuteNonQuery(cmdResearchResultTables);
        
        ResearchMain entResearchMain = DataRepository.ResearchMainProvider.GetByAutoResearchID(intResearchID);

        if (entResearchMain != null)
        {
            int intSurveyID = 0;

            int.TryParse(entResearchMain.IntSGSurveyID.ToString(), out intSurveyID);

            if (intSurveyID > 0)
            {
                TList<ResearchQuestions> tlstResearchQuestions = DataRepository.ResearchQuestionsProvider.GetByIntQuesResearchID(intResearchID);

                string strPath = string.Format("https://restapi.surveygizmo.com/V1/survey/{0}.xml", intSurveyID);
                string strParams = string.Format("{0}", UserNamePassword());
                string strFullPath = string.Format("{0}?{1}", strPath, strParams);

                XmlDocument doc = PostToSQ(strFullPath);
                XmlNodeList xmlList = doc.SelectNodes("/apiresult/data/pages/SurveyPage/questions");

                foreach (XmlNode xmlnodeTemp in xmlList)
                {

                    foreach (XmlNode xmlnodeDataTemp in xmlnodeTemp.ChildNodes)
                    {
                        try
                        {
                            string str = xmlnodeDataTemp.Name;
                            string strID = xmlnodeDataTemp["id"].InnerText;

                            int intID = 0;
                            int intQuestionID = 0;

                            int.TryParse(strID, out intID);

                            string strDesc2 = xmlnodeDataTemp["title"]["English"].InnerText;

                            string strFind = string.Format("{0} = {1}", ResearchQuestionsColumn.IntSGQuestionID, strID);

                            ResearchQuestions entResearchQuestion2 = tlstResearchQuestions.Find(delegate(ResearchQuestions entResearchQuestion) { return entResearchQuestion.IntSGQuestionID == intID; });

                            if (entResearchQuestion2 != null)
                            {
                                ResearchResultsQuestions entResearchResultsQuestions = new ResearchResultsQuestions();
                                intQuestionID = entResearchQuestion2.AutoQuesID;
                                entResearchResultsQuestions.IntResultQuestionID = intQuestionID;
                                entResearchResultsQuestions.IntResultResearchID = intResearchID;
                                entResearchResultsQuestions.StrResultQuestionText = strDesc2;
                                entResearchResultsQuestions.IntSGQuestionID = intID;

                                DataRepository.ResearchResultsQuestionsProvider.Save(entResearchResultsQuestions);
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }  // End Questions

                xmlList = doc.SelectNodes("/apiresult/data/pages/SurveyPage/questions/SurveyQuestion/options/SurveyOption");

                foreach (XmlNode xmlnodeTemp in xmlList)
                {
                    try
                    {

                        string strID = xmlnodeTemp["id"].InnerText;
                        string strValue = xmlnodeTemp["value"].InnerText;
                        string strDesc = xmlnodeTemp["title"]["English"].InnerText;

                        XmlNode xmlNodeParent = xmlnodeTemp.ParentNode.ParentNode;

                        string strParentID = xmlNodeParent["id"].InnerText;

                        int intID = 0;
                        int intValue = 0;

                        int.TryParse(strID, out intID);
                        int.TryParse(strValue, out intValue);


                        if (intValue > 0 || intID > 0)
                        {
                            if (intValue > 50)      // Values less than this are Yes/No or Scaled
                            {
                                ResearchQuestionsResponses entResearchQuestionsResponses = DataRepository.ResearchQuestionsResponsesProvider.GetByAutoQuesRespID(intValue);

                                if (entResearchQuestionsResponses != null)
                                {
                                    ResearchResultsQuestions entResearchResultsQuestions = DataRepository.ResearchResultsQuestionsProvider.GetByIntResultQuestionID(entResearchQuestionsResponses.IntQuesRespQuesID);

                                    ResearchResultsResponses entResearchResultsResponses = new ResearchResultsResponses();

                                    entResearchResultsResponses.IntResultResearchID = intResearchID;
                                    entResearchResultsResponses.IntResultQuestionID = entResearchQuestionsResponses.IntQuesRespQuesID;
                                    entResearchResultsResponses.IntResultResponseQuestion = entResearchResultsQuestions.AutoResultMapID;
                                    entResearchResultsResponses.IntResultResponseValue = intValue;
                                    entResearchResultsResponses.IntSGResponseID = intID;
                                    entResearchResultsResponses.StrResultResponseText = strDesc;

                                    DataRepository.ResearchResultsResponsesProvider.Save(entResearchResultsResponses);
                                }
                            }
                            else
                            {
                                ResearchResultsQuestionsQuery query1 = new ResearchResultsQuestionsQuery(true, true);
                                query1.AppendEquals(ResearchResultsQuestionsColumn.IntResultResearchID, intResearchID.ToString());
                                query1.AppendEquals(ResearchResultsQuestionsColumn.IntSGQuestionID, strParentID);

                                TList<ResearchResultsQuestions> tlstResearchResultsQuestions = DataRepository.ResearchResultsQuestionsProvider.Find(query1);

                                if (tlstResearchResultsQuestions != null)
                                {
                                    if (tlstResearchResultsQuestions.Count > 0)
                                    {
                                        ResearchResultsResponses entResearchResultsResponses = new ResearchResultsResponses();

                                        if (intValue == 0)
                                            intValue = intID;

                                        entResearchResultsResponses.IntResultResearchID = intResearchID;
                                        entResearchResultsResponses.IntResultQuestionID = tlstResearchResultsQuestions[0].IntResultQuestionID;
                                        entResearchResultsResponses.IntResultResponseQuestion = tlstResearchResultsQuestions[0].AutoResultMapID;
                                        entResearchResultsResponses.IntResultResponseValue = intValue;
                                        entResearchResultsResponses.IntSGResponseID = intID;
                                        entResearchResultsResponses.StrResultResponseText = strDesc;

                                        DataRepository.ResearchResultsResponsesProvider.Save(entResearchResultsResponses);
                                    }
                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }



            }
        }

    }

    /// <summary>
    /// Saves the research answers from SurveyGizmo to the Jury Research Database
    /// </summary>
    /// <param name="strValue">Response Value - Text or Numeric</param>
    /// <param name="intResearchID">Research ID.</param>
    /// <param name="intUserID">Respondent ID</param>
    /// <param name="intSGQuestionID">SurveyGizmo Question ID</param>
    /// <param name="boolLookupQuestion">if set to <c>true</c> Lookup question in database</param>
    /// <param name="intQuestionID">Jury Research Question ID</param>
    protected void SaveResearchAnswer(string strValue, int intResearchID, int intUserID, int intSGQuestionID, bool boolLookupQuestion, int intQuestionID)
    {
        int intValue = 0;
        int intResponseID = 0;

        string strText = string.Empty;

        int.TryParse(strValue, out intValue);

        if (intValue == 0)
        {
            ResearchQuestionsResponsesQuery query1 = new ResearchQuestionsResponsesQuery(true, true);
            query1.AppendEquals(ResearchQuestionsResponsesColumn.IntQuesRespQuesID, intQuestionID.ToString());
            query1.AppendContains(ResearchQuestionsResponsesColumn.StrQuesRespText, strValue);

            TList<ResearchQuestionsResponses> tlstResearchQuestionsResponses = DataRepository.ResearchQuestionsResponsesProvider.Find(query1);

            if (tlstResearchQuestionsResponses.Count > 0)
            {
                strText = tlstResearchQuestionsResponses[0].StrQuesRespText;
                intValue = tlstResearchQuestionsResponses[0].AutoQuesRespID;

                int.TryParse(tlstResearchQuestionsResponses[0].IntSGResponseID.ToString(), out intResponseID);
            }
            else
            {
                strText = strValue;
            }

        }

        ResearchAnswers entResearchAnswers = new ResearchAnswers();
        entResearchAnswers.IntAnswerResearchID = intResearchID;
        entResearchAnswers.IntAnswerQuestionID = intQuestionID;
        entResearchAnswers.IntAnswerUserID = intUserID;
        entResearchAnswers.StrAnswerText = strText;
        entResearchAnswers.IntAnswerResponseID = intValue;
        entResearchAnswers.IntSGAnswerID = intResponseID;
        entResearchAnswers.BitSGCompletedData = true;

        DataRepository.ResearchAnswersProvider.Save(entResearchAnswers);
    }


    /// <summary>
    /// SurveyGizmo Call
    /// - Gets the question definition from SurveyGizmo
    /// </summary>
    /// <param name="intSurveyID">SurveyGizmo Survey ID</param>
    /// <param name="intQuestionID">SurveyGizmo Question ID.</param>
    /// <returns>clsQuestion.</returns>
    public clsQuestion GetQuestion(int intSurveyID, int intQuestionID)
    {
        string strPath = string.Format("https://restapi.surveygizmo.com/V3/survey/{0}/surveyquestion/{1}.xml", intSurveyID, intQuestionID);
        string strParams = string.Format("{0}", UserNamePassword());
        string strFullPath = string.Format("{0}?{1}", strPath, strParams);

        XmlDocument doc = PostToSQ(strFullPath);

        clsQuestion clsQuestionTemp = new clsQuestion();

        string strQuestion = clsPublic.GetXMLText(doc, "/apiresult/data/title/English");
        string strQuestionID = clsPublic.GetXMLText(doc, "/apiresult/data/id");

        clsQuestionTemp.QuestionID =  intQuestionID;
        clsQuestionTemp.QuestionText = strQuestion;

        return clsQuestionTemp;

    }

    /// <summary>
    /// SurveyGizmo Call
    /// - Gets all the questions by Survey ID
    /// </summary>
    /// <param name="intSurveyID">SurveyGizmo Survey ID</param>
    /// <returns>List{clsQuestion}.</returns>
    public List<clsQuestion> GetQuestions(int intSurveyID)
    {
        string strPath = string.Format("https://restapi.surveygizmo.com/V3/survey/{0}/surveyquestion.xml", intSurveyID);
        string strParams = string.Format("{0}", UserNamePassword());
        string strFullPath = string.Format("{0}?{1}", strPath, strParams);

        XmlDocument doc = PostToSQ(strFullPath);

        XmlNodeList xmlData = doc.GetElementsByTagName("SurveyQuestion");

        List<clsQuestion> lstclsQuestion = new List<clsQuestion>();

        foreach (XmlNode xmlNodeTemp in xmlData)
        {
            clsQuestion clsQuestionTemp = new clsQuestion();

            int intQuestionID = 0;

            int.TryParse(clsPublic.GetXMLText(xmlNodeTemp, "id"), out intQuestionID);

            clsQuestionTemp.QuestionID = intQuestionID;
            clsQuestionTemp.QuestionText = clsPublic.GetXMLText(xmlNodeTemp, "title/English");

            lstclsQuestion.Add(clsQuestionTemp);
        }

        return lstclsQuestion;

    }

    /// <summary>
    /// SurveyGizmo Call
    /// - Gets the question responses (options to the question).
    /// </summary>
    /// <param name="intSurveyID">SurveyGizmo Survey ID</param>
    /// <param name="intQuestionID">SurveyGizmo Question ID.</param>
    /// <returns>List{clsQuestionOptions}.</returns>
    public List<clsQuestionOptions> GetQuestionResponses(int intSurveyID, int intQuestionID)
    {
        string strPath = string.Format("https://restapi.surveygizmo.com/V3/survey/{0}/surveyquestion/{1}/surveyoption.xml", intSurveyID, intQuestionID);
        string strParams = string.Format("{0}", UserNamePassword());
        string strFullPath = string.Format("{0}?{1}", strPath, strParams);

        XmlDocument doc = PostToSQ(strFullPath);
        
        string strTotalCount = clsPublic.GetXMLText(doc, "/apiresult/totalcount");

        XmlNodeList xmlData = doc.GetElementsByTagName("SurveyOption");

        List<clsQuestionOptions> lstclsQuestionOptions = new List<clsQuestionOptions>();

        foreach (XmlNode xmlNodeTemp in xmlData)
        {
            clsQuestionOptions clsQuestionOptions = new clsQuestionOptions();

            clsQuestionOptions.OptionID = clsPublic.GetXMLText(xmlNodeTemp, "id");
            clsQuestionOptions.OptionText = clsPublic.GetXMLText(xmlNodeTemp, "title/English");
            clsQuestionOptions.OptionValue = clsPublic.GetXMLText(xmlNodeTemp, "value");

            lstclsQuestionOptions.Add(clsQuestionOptions);
        }

        return lstclsQuestionOptions;
    }

    /// <summary>
    /// SurveyGizmo Call
    /// - Creates a new question option.
    /// </summary>
    /// <param name="intSurveyID">SurveyGizmo Survey ID</param>
    /// <param name="intPageNumber">Survey Page Number</param>
    /// <param name="intQuestionNumber">SurveyGizmo Question ID</param>
    /// <param name="strTitle">Option Description</param>
    /// <param name="strValue">Option Value</param>
    /// <returns>Returns Option ID (System.Int32)</returns>
    public int CreateNewQuestionOption(int intSurveyID, int intPageNumber, int intQuestionNumber, string strTitle, string strValue)
    {

        strTitle = HttpUtility.UrlEncode(strTitle);
        strValue = HttpUtility.UrlEncode(strValue);

        string strPath = string.Format("http://restapi.surveygizmo.com/V3/survey/{0}/surveypage/{1}/surveyquestion/{2}/surveyoption.xml", intSurveyID, intPageNumber, intQuestionNumber);
        string strParams = string.Format("_method=put&title[English]={0}&value={1}&{2}", strTitle, strValue, UserNamePassword());
        string strFullPath = string.Format("{0}?{1}", strPath, strParams);

        XmlDocument doc = PostToSQ(strFullPath);
        XmlNode xmlResult = doc.SelectSingleNode("/apiresult/data/id");

        int intID = 0;

        if ( xmlResult != null)
            int.TryParse(xmlResult.InnerText, out intID);

        return intID;

    }

    /// <summary>
    /// SurveyGizmo Call
    /// - Injects javascript on each survey page in SurveyGizmo
    /// - This javascript prevents users from printing or copying the survey to their clipboard
    /// </summary>
    /// <param name="intResearchID">Research ID.</param>
    public void InsertSecurityJavaScript(int intResearchID)
    {
        ResearchMain entResearchMain = DataRepository.ResearchMainProvider.GetByAutoResearchID(intResearchID);
        if (entResearchMain != null)
        {
            int intSurveyID = 0;

            int.TryParse(entResearchMain.IntSGSurveyID.ToString(), out intSurveyID);

            if (intSurveyID > 0)
            {
                string strPath = string.Format("https://restapi.surveygizmo.com/V3/survey/{0}.xml", intSurveyID);
                string strParams = string.Format("_method=get&{0}", UserNamePassword());
                string strFullPath = string.Format("{0}?{1}", strPath, strParams);

                PageHTMLText pgText = DataRepository.PageHTMLTextProvider.GetByStrHTMLPageIntHTMLIndex("SGJavaText", 1);

                if (pgText != null)
                {
                    XmlDocument doc = PostToSQ(strFullPath);
                    XmlNode xmlResult = doc.SelectSingleNode("/apiresult/data/pages");

                    int intCount = xmlResult.ChildNodes.Count;

                    string strJavaScript = pgText.MemHTMLText;

                    if (strJavaScript.Length > 0)
                    {
                        for (int intCounter = 1; intCounter <= intCount; intCounter++)
                        {
                            int intQuestionID = CreateNewQuestion(SGQuestionTypes.instructions, intSurveyID, intCounter, strJavaScript, string.Empty , 0, string.Empty);
                        }
                    }
                }

            }
        }
    }

    /// <summary>
    /// SurveyGizmo Call
    /// - Starts the process of retrieving a Jury Research Study
    /// - Then goes through all the calls to upload the questions, options, and other related items
    /// </summary>
    /// <param name="intResearchID">Research ID.</param>
    /// <param name="boolIsTest">if set to <c>true</c> [bool is test].</param>
    public void UploadSurveyFromResearchID(int intResearchID, bool boolIsTest)
    {
        ResearchMain entResearchMain = DataRepository.ResearchMainProvider.GetByAutoResearchID(intResearchID);
        
        if (entResearchMain != null)
        {
            TList<ResearchQuestions> tlstResearchQuestions = DataRepository.ResearchQuestionsProvider.GetByIntQuesResearchID(intResearchID);

            if (tlstResearchQuestions != null)
            {
                int intSurveyID = 0;
                int intThankyouPageID = 2;
                int intPageID = 1;

                intSurveyID = this.CreateNewSurvey(entResearchMain.StrResearchSurveyName, boolIsTest, intResearchID);

                if (!boolIsTest)
                {
                    entResearchMain.IntSGSurveyID = intSurveyID;
                    DataRepository.ResearchMainProvider.Save(entResearchMain);

                }

                int intQuestionID = 0;
                int intQuestionID_Last = 0;

                string strQuote = @"""";

                int intSN = 0;
                int intAC = 0;
                int intLang = 0;
                int intSCO = 0;

                int intSID = 0;
                int intGMIRedirect = 0;

                intSID = this.CreateNewQuestion(SGQuestionTypes.hidden, intSurveyID, 1, "Research ID", intResearchID.ToString(), 0, string.Empty);

                if (!boolIsTest)
                {//
                    intSN = this.CreateNewQuestion(SGQuestionTypes.hidden, intSurveyID, 1, "GMI Study Number", "[url(" + strQuote + "sn" + strQuote + ")]", 0, string.Empty);
                    intAC = this.CreateNewQuestion(SGQuestionTypes.hidden, intSurveyID, 1, "GMI Access Code", "[url(" + strQuote + "ac" + strQuote + ")]", intQuestionID, string.Empty);
                    intLang = this.CreateNewQuestion(SGQuestionTypes.hidden, intSurveyID, 1, "GMI Language", "[url(" + strQuote + "lang" + strQuote + ")]", intQuestionID, string.Empty);
                    intSCO = this.CreateNewQuestion(SGQuestionTypes.hidden, intSurveyID, 1, "Static Value (use default)", "[url(" + strQuote + "SCO" + strQuote + ")]", intQuestionID, string.Empty);
                }

                intQuestionID = this.CreateNewQuestion(SGQuestionTypes.instructions, intSurveyID, 1, entResearchMain.StrResearchIntroduction, string.Empty, 0, string.Empty);
                intQuestionID_Last = intQuestionID;

                if (!boolIsTest)
                {
                    string strAddParam = string.Format(@"properties[url]=http://www.globaltestmarket.com/20/survey/finished.phtml&properties[outbound][0][fieldname]=sn&properties[outbound][0][mapping]={0}&properties[outbound][1][fieldname]=ac&properties[outbound][1][mapping]={1}&properties[outbound][2][fieldname]=lang&properties[outbound][2][mapping]={2}&properties[outbound][2][default]=E&properties[outbound][3][fieldname]=SCO&properties[outbound][3][mapping]={3}&properties[outbound][3][default]=s", intSN, intAC, intLang, intSCO);

                    intQuestionID = this.CreateNewQuestion(SGQuestionTypes.urlredirect, intSurveyID, intThankyouPageID, "GMI Redirect", "GMI Redirect", 0, strAddParam);
                    intGMIRedirect = intQuestionID;
                }


                //
                //  Process Questions to SG
                //
                tlstResearchQuestions.Sort(ResearchQuestionsColumn.IntQuesOrder.ToString());

                tlstResearchQuestions.ForEach(delegate(ResearchQuestions entResearchQuestions)
                {
                    SGQuestionTypes quesType = SGQuestionTypes.radio;

                    string strQuestionText = entResearchQuestions.StrQuesText;

                    if (entResearchQuestions.IntQuesType != 11)
                    {

                        switch (entResearchQuestions.IntQuesType)
                        {
                            case 1:            // Open Ended
                                quesType = SGQuestionTypes.essay;

                                break;

                            case 2:
                            case 3:
                            case 4:
                                quesType = SGQuestionTypes.radio;
                                break;

                            case 5:

                                quesType = SGQuestionTypes.checkbox;
                                break;

                            case 10:

                                quesType = SGQuestionTypes.instructions;
                                break;

                            case 12:
                                quesType = SGQuestionTypes.instructions;

                                ResearchQuestionsFiles entResearchQuestionsFiles = DataRepository.ResearchQuestionsFilesProvider.GetByIntResearchFilesQuestionID(entResearchQuestions.AutoQuesID);

                                if (entResearchQuestionsFiles != null)
                                {
                                    strQuestionText = string.Format(@"<img src='" + clsPublic.GetAppSetting("Host") + "/images/Research/{0}' style='border-width:0px;' />", entResearchQuestionsFiles.StrResearchFilesUniqueFileName);
                                    //strQuestionText = string.Format(@"<img src='{0}/images/Research/{1}' style='border-width:0px;' />", clsPublic.GetAppSetting("Host"), entResearchQuestionsFiles.StrResearchFilesUniqueFileName);
                                }
                                else
                                    strQuestionText = string.Empty;

                                break;

                            case 13:
                                quesType = SGQuestionTypes.instructions;

                                ResearchQuestionsFiles entResearchQuestionsFilesVid = DataRepository.ResearchQuestionsFilesProvider.GetByIntResearchFilesQuestionID(entResearchQuestions.AutoQuesID);

                                if (entResearchQuestionsFilesVid != null)
                                {
                                    strQuestionText = string.Format(@"<iframe frameBorder='0' width='500' height='480' src='" + System.Configuration.ConfigurationManager.AppSettings["Host"] + "/Video/Research/videoview.aspx?video={0}'></iframe>", entResearchQuestionsFilesVid.StrResearchFilesUniqueFileName);
                                }
                                else
                                    strQuestionText = string.Empty;

                                break;

                        }

                        intQuestionID = this.CreateNewQuestion(quesType, intSurveyID, intPageID, strQuestionText, string.Empty, intQuestionID_Last, string.Empty);

                        entResearchQuestions.IntSGQuestionID = intQuestionID;
                        DataRepository.ResearchQuestionsProvider.Save(entResearchQuestions);

                        intQuestionID_Last = intQuestionID;

                        if (entResearchQuestions.IntQuesType == 2)
                        {
                            this.CreateNewQuestionOption(intSurveyID, intPageID, intQuestionID, "Yes", "1");
                            this.CreateNewQuestionOption(intSurveyID, intPageID, intQuestionID, "No", "2");
                        }

                        if (entResearchQuestions.IntQuesType == 3)
                        {
                            this.CreateNewQuestionOption(intSurveyID, intPageID, intQuestionID, "Strongly Agree", "1");
                            this.CreateNewQuestionOption(intSurveyID, intPageID, intQuestionID, "Somewhat Agree", "2");
                            this.CreateNewQuestionOption(intSurveyID, intPageID, intQuestionID, "Somewhat Disagree", "3");
                            this.CreateNewQuestionOption(intSurveyID, intPageID, intQuestionID, "Strongly Disagree", "4");
                            this.CreateNewQuestionOption(intSurveyID, intPageID, intQuestionID, "Neither Agree Nor Disagree", "5");
                        }

                        if (entResearchQuestions.IntQuesType == 4 || entResearchQuestions.IntQuesType == 5)
                        {
                            TList<ResearchQuestionsResponses> tlstResearchQuestionsResponses = DataRepository.ResearchQuestionsResponsesProvider.GetByIntQuesRespQuesID(entResearchQuestions.AutoQuesID);

                            tlstResearchQuestionsResponses.Sort(ResearchQuestionsResponsesColumn.AutoQuesRespID.ToString());

                            tlstResearchQuestionsResponses.ForEach(delegate(ResearchQuestionsResponses entResearchQuestionResponses)
                            {
                                int intOptionID = this.CreateNewQuestionOption(intSurveyID, intPageID, intQuestionID, entResearchQuestionResponses.StrQuesRespText, entResearchQuestionResponses.AutoQuesRespID.ToString());
                                entResearchQuestionResponses.IntSGResponseID = intOptionID;
                                DataRepository.ResearchQuestionsResponsesProvider.Save(entResearchQuestionResponses);
                            });
                        }
                    }
                    else
                    {
                        intPageID++;
                        intPageID = this.CreateNewPage(intSurveyID, intPageID, string.Empty, string.Empty);

                    }
                });

                //****************************************************
                //****************************************************
                //Code to add second last page for screening questions
                //****************************************************
                //****************************************************
                //Starts here
                intPageID++;
                intPageID = this.CreateNewPage(intSurveyID, intPageID, string.Empty, string.Empty);

                if (!boolIsTest)
                {
                    string strAddParam = string.Format(@"properties[url]=http://www.globaltestmarket.com/20/survey/finished.phtml&properties[outbound][0][fieldname]=sn&properties[outbound][0][mapping]={0}&properties[outbound][1][fieldname]=ac&properties[outbound][1][mapping]={1}&properties[outbound][1][default]=2F8B385779A9D018&properties[outbound][2][fieldname]=lang&properties[outbound][2][mapping]={2}&properties[outbound][2][default]=E", intSN, intAC, intLang);

                    intQuestionID = this.CreateNewQuestion(SGQuestionTypes.urlredirect, intSurveyID, intPageID, "End Survey", "End Survey", 0, strAddParam);
                    intGMIRedirect = intQuestionID;
                }
                //****************************************************
                //Ends here
                //****************************************************
                //****************************************************
                if (!boolIsTest)
                {
                    this.CreateCampaignURL(intResearchID, "Public");
                }

                this.InsertSecurityJavaScript(intResearchID);

//                this.LoadHTTPPost(intSurveyID, intThankyouPageID);
                CheckValidateSurveyQuestion(intResearchID);
            }
        }

    }


    /// <summary>
    /// Exports to CSV.
    /// </summary>
    /// <param name="intResearchID">Research ID.</param>
    public void ExportToCSV(int intResearchID)
    {
        ResearchMain entResearchMain = DataRepository.ResearchMainProvider.GetByAutoResearchID(intResearchID);

        if (entResearchMain != null)
        {
            TList<ResearchQuestions> tlstResearchQuestions = DataRepository.ResearchQuestionsProvider.GetByIntQuesResearchID(intResearchID);

            int intSurveyID = 0;

            int.TryParse(entResearchMain.IntSGSurveyID.ToString(), out intSurveyID);

            if (intSurveyID > 0)
            {
                string strPath = string.Format("https://restapi.surveygizmo.com/V3/survey/{0}/surveyresponse.xml", intSurveyID);
                string strParams = string.Format("page={1}&resultsperpage=200&{0}", UserNamePassword(), 1);
                string strFullPath = string.Format("{0}?{1}", strPath, strParams);

                XmlDocument doc = PostToSQ(strFullPath);

                StringReader stream = null;
                XmlTextReader reader = null;

                XmlSerializer serializer = new XmlSerializer(typeof(apiresult));
               stream = new StringReader(doc.InnerXml); // read xml data

                reader = new XmlTextReader(stream);  // create reader

                // covert reader to object

                apiresult test = (apiresult)serializer.Deserialize(reader);

            }
        }
    }

    /// <summary>
    /// This is just sample code that was used for testing a straight post call to SG
    /// </summary>
    public void TestPost()
    {
        // We use the HttpUtility class from the System.Web namespace  


        Uri address = new Uri("https://restapi.surveygizmo.com/V3/survey");  // 783009

            // Create the web request  
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;  

            // Set type to POST  
            request.Method = "GET";
            request.ContentType = "text/xml";  

            StringBuilder data = new StringBuilder();

            //_method=put&title[English]={1}&after={2}&{3}
            //data.Append("user:pass=BMorse@decisionquest.com:DecisionQuest21535");
            if (System.Configuration.ConfigurationManager.AppSettings["SG-Testing"].ToString().ToLower().Trim() == "false")
            {
                data.Append("user:pass=BMorse@decisionquest.com:DecisionQuest21535");
            }
            else
            {
                data.Append("user:pass=mukesh.tungaria@exavalu.com:Mukesh@1");
            }
            
//            data.Append("_method=put&title=Testing&type=survey&status=launched&user:pass=" + HttpUtility.UrlEncode("BMorse@decisionquest.com:DecisionQuest21535"));
//            data.Append("&_method=put&after=1&title[English]=" + HttpUtility.UrlEncode("This is a testing title"));

            // Create a byte array of the data we want to send  
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());  

            // Set the content length in the request headers  
            request.ContentLength = byteData.Length;  

            // Write data  
            using (Stream postStream = request.GetRequestStream())  
            {  
                postStream.Write(byteData, 0, byteData.Length);  
            }  
    
            // Get response  
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)  
            {  
            // Get the response stream  
            StreamReader reader = new StreamReader(response.GetResponseStream());  
 
            // Console application output  
            string str = reader.ReadToEnd();
        }  

    }

}


