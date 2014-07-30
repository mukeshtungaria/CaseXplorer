// ***********************************************************************
// Author           : Dennis Sebenick
// Created          : 05-09-2012
//
// Last Modified By : dennis
// Last Modified On : 09-11-2012
// ***********************************************************************
// <copyright file="clsPublic.cs" company="DGCC.COM">
//     . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Xml;
using System.Web.Security;
using System.Net.Mail;
using System.Data.Common;
using System.Web.UI;
using System.Net.Mime;
using System.Web.UI.WebControls;
using System.Web.Profile;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Security.Principal;
using System.Text.RegularExpressions;
using JuryData.Entities;
using JuryData.Data;

/// <summary>
/// Misc functions to be used across application
/// </summary>
public class clsPublic
{
    /// <summary>
    /// Initializes a new instance of the <see cref="clsPublic" /> class.
    /// </summary>
   public clsPublic()
    {
        //
        // TODO: Add constructor logic here
        //
    }

   /// <summary>
   /// Gets the XML text by an xml node string
   /// </summary>
   /// <param name="doc">XML Document</param>
   /// <param name="strNode">XML Node String.</param>
   /// <returns>Return text from node (System.String)</returns>
    public static string GetXMLText(XmlDocument doc, string strNode)
    {
        if (doc != null)
        {
            XmlNode xmlNodeTemp = doc.SelectSingleNode(strNode);

            if (xmlNodeTemp != null)
            {
                return xmlNodeTemp.InnerText;
            }
            else
                return string.Empty;
        }
        else
            return string.Empty;

    }

    /// <summary>
    /// Gets the XML text from an XML node by an XML node string
    /// </summary>
    /// <param name="xmlNodeTemp">XML Node</param>
    /// <param name="strNode">XML Node String.</param>
    /// <returns>Return text from node (System.String)</returns>
    public static string GetXMLText(XmlNode xmlNodeTemp, string strNode)
    {
        if (xmlNodeTemp != null)
        {
            XmlNode xmlNodeTemp2 = xmlNodeTemp.SelectSingleNode(strNode);

            if (xmlNodeTemp2 != null)
                return xmlNodeTemp2.InnerText;
            else
                return string.Empty;
        }
        else
            return string.Empty;

    }

    /// <summary>
    /// Get the current host with http as a string
    /// </summary>
    /// <returns>Returns Host String (System.String)</returns>
    public static string GetHost()
    {
        return HttpContext.Current.Request.ServerVariables["HTTP_HOST"];
    }


    /// <summary>
    /// Class CourtSelectedArgs
    /// </summary>
    public class CourtSelectedArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly int CourtID;

        /// <summary>
        /// Initializes a new instance of the <see cref="CourtSelectedArgs" /> class.
        /// </summary>
        /// <param name="num">The num.</param>
        public CourtSelectedArgs(int num)
        {
            CourtID = num;
        }

    }

    /// <summary>
    /// Determines if a research study exists by ID
    /// </summary>
    /// <param name="intResearchID">Research ID</param>
    /// <returns><c>true</c> if Research Study exists, <c>false</c> does not exist</returns>
    public static bool ResearchExists(int intResearchID)
    {
        ResearchMain entResearchMain = DataRepository.ResearchMainProvider.GetByAutoResearchID(intResearchID);

        if (entResearchMain != null)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Gets the name of the current page.
    /// </summary>
    /// <returns>Returns Page Name (System.String)</returns>
    public static string GetCurrentPageName() 
    { 
        string sPath = System.Web.HttpContext.Current.Request.Url.AbsolutePath; 
        System.IO.FileInfo oInfo = new System.IO.FileInfo(sPath); 
        string sRet = oInfo.Name; 
        
        return sRet; 
    }

    /// <summary>
    /// Determines whether the current logged in user has permission to the specified Research Study by ID
    /// </summary>
    /// <param name="intResearchID">Research ID</param>
    /// <returns><c>true</c> if user  has permission to research ID; otherwise, <c>false</c>.</returns>
    public static bool HasPermissionToResearchID(int intResearchID)
    {
        ResearchMain entResearchMain = DataRepository.ResearchMainProvider.GetByAutoResearchID(intResearchID);

        if (entResearchMain != null)
        {
            string strUser = HttpContext.Current.Profile.UserName.ToString();

            MembershipUser mu = Membership.GetUser(strUser);

            string userGuid = mu.ProviderUserKey.ToString();

            if (userGuid.Equals(entResearchMain.UserID.ToString()))
                return true;
            else
            {
                HttpContext.Current.Session["ResearchID"] = null;
                return false;
            }
        }

        HttpContext.Current.Session["ResearchID"] = null;
        return false;
    }

    /// <summary>
    /// Regex string to determine numeric
    /// </summary>
    private static Regex _isNumber = new Regex(@"^\d+$");

    /// <summary>
    /// Determines whether the specified string value is an integer.
    /// </summary>
    /// <param name="theValue">String integer value</param>
    /// <returns><c>true</c> if the specified the value is integer; otherwise, <c>false</c>.</returns>
    public static bool IsInteger(string theValue)
    {
        Match m = _isNumber.Match(theValue);
        return m.Success;
    } //IsInteger


    /// <summary>
    /// Determines if a specified string is an alpha numeric only
    /// </summary>
    /// <param name="strString">String integer value</param>
    /// <returns><c>true</c> if is alpha numeric, <c>false</c> otherwise</returns>
    public static bool AlphaNumericOnly(string strString)
    {
        string pattern = @"^[0-9a-zA-Z\s\.\-\/\&\%\+]+$";

        bool boolResult = false;

        boolResult = Regex.IsMatch(strString, pattern);

        return boolResult;
    }


    /// <summary>
    /// Determines whether the specified string string has HTML within the string
    /// </summary>
    /// <param name="strString">Possible HTML string value.</param>
    /// <returns><c>true</c> if the specified STR string has HTML; otherwise, <c>false</c>.</returns>
    public static bool HasHTML(string strString)
    {
        string pattern = @"<(.|\n)*?>";

        bool boolResult = false;

        boolResult = Regex.IsMatch(strString, pattern);

        return boolResult;
    }

    /// <summary>
    /// Returns encoded HTML string
    /// </summary>
    /// <param name="htmlString">The HTML string.</param>
    /// <returns>Encoded HTML (System.String)</returns>
    public static string ReplaceEncodedHTML(string htmlString)
    {
        return HttpUtility.HtmlDecode(htmlString);
    }

    /// <summary>
    /// Strips HTML from a string
    /// </summary>
    /// <param name="htmlString">The HTML string.</param>
    /// <returns>Stripped HTML (System.String)</returns>
    public static string StripHTML(string htmlString)
    {
        //This pattern Matches everything found inside html tags;
        //(.|\n) - > Look for any character or a new line
        // *?  -> 0 or more occurrences, and make a non-greedy search meaning
        //That the match will stop at the first available '>' it sees, and not at the last one
        //(if it stopped at the last one we could have overlooked 
        //nested HTML tags inside a bigger HTML tag..)
        // Thanks to Oisin and Hugh Brown for helping on this one...

        string pattern = @"<(.|\n)*?>";

        return Regex.Replace(htmlString, pattern, string.Empty);
    }

    /// <summary>
    /// Strips specified text from a string.
    /// </summary>
    /// <param name="targetString">The target string.</param>
    /// <returns>Stripped text (System.String)</returns>
    public static string StripString(string targetString)
    {
        return Regex.Replace(targetString, @"\W*", "");
    }


    /// <summary>
    /// Determines whether the specified string is date.
    /// </summary>
    /// <param name="strDate">Date String</param>
    /// <returns><c>true</c> if the specified string date is date; otherwise, <c>false</c>.</returns>
    public static bool IsDate(string strDate)
    {
        try
        {
            DateTime datTemp = Convert.ToDateTime(strDate);

            return true;
        }
        catch
        {
            return false;
        }

    }

    /// <summary>
    /// Dynamically adds a validator to an HTML panel
    /// </summary>
    /// <param name="pnl">HTML Panel</param>
    /// <param name="strControlID">Control ID</param>
    /// <param name="strMessage">Message to display</param>
    public static void AddValidator(Panel pnl, string strControlID, string strMessage)
    {
        RequiredFieldValidator req = new RequiredFieldValidator();
        req.ErrorMessage = strMessage;
        req.ControlToValidate = strControlID;
        req.Display = ValidatorDisplay.None;
        req.Enabled = true;
        req.Visible = true;
        pnl.Controls.Add(req);

    }

    /// <summary>
    /// Returns the selected radio value integer
    /// </summary>
    /// <param name="radList">Radio List.</param>
    /// <returns>Selected item value (System.Int32)</returns>
    public static int RadioListValue(RadioButtonList radList)
    {
        return clsPublic.IsInteger(radList.SelectedValue) ? Convert.ToInt32(radList.SelectedValue) : 0;
    }

    /// <summary>
    /// Returns the selected value from a drop down list
    /// </summary>
    /// <param name="ddlList">Drop Down List</param>
    /// <returns>Selected item value (System.Int32)</returns>
    public static int DDLValue(DropDownList ddlList)
    {
        return clsPublic.IsInteger(ddlList.SelectedValue) ? Convert.ToInt32(ddlList.SelectedValue) : 0;
    }


    /// <summary>
    /// Populates check list box from database by drop value type (Court Data List)
    /// </summary>
    /// <param name="chkLst">Checkbox List</param>
    /// <param name="intDropType">Court Data Type</param>
    public static void PopulateCheckBoxList(CheckBoxList chkLst, Int16 intDropType)
    {
        chkLst.DataSource = clsData.GetDropValuesCourtDataByType(intDropType, true);
        chkLst.DataTextField = JuryData.Entities.DropValuesCourtColumn.StrDropValueDescription.ToString();
        chkLst.DataValueField = JuryData.Entities.DropValuesCourtColumn.AutoDropValueID.ToString();
        chkLst.DataBind();
    }

    /// <summary>
    /// Populates the check box list by parent (Court Data)
    /// </summary>
    /// <param name="chkLst">Checkbox List</param>
    /// <param name="intDropType">Court Data Type</param>
    /// <param name="intDropParent">Court Data Parent Type</param>
    public static void PopulateCheckBoxListByParent(CheckBoxList chkLst, Int16 intDropType, int intDropParent)
    {
        chkLst.DataSource = clsData.GetDropValuesCourtDataByType(intDropType, intDropParent, true);
        chkLst.DataTextField = JuryData.Entities.DropValuesCourtColumn.StrDropValueDescription.ToString();
        chkLst.DataValueField = JuryData.Entities.DropValuesCourtColumn.AutoDropValueID.ToString();
        chkLst.DataBind();
    }

    /// <summary>
    /// Populates drop down list by type and parent (Court Data)
    /// </summary>
    /// <param name="ddlTemp">Drop Down List</param>
    /// <param name="intDropType">Court Data Type</param>
    /// <param name="intDropParent">Court Data Parent Type</param>
    /// <param name="boolAddBlank">if set to <c>true</c> add empty value.</param>
    /// <param name="strBlankDesc">If adding empty value, use this as text</param>
    /// <param name="strBlankValue">If adding blank value, use this as value</param>
    public static void PopulateDDLByParent(DropDownList ddlTemp, Int16 intDropType, int intDropParent, bool boolAddBlank, string strBlankDesc, string strBlankValue)
    {
        ddlTemp.DataSource = clsData.GetDropValuesCourtDataByType(intDropType, intDropParent, true);
        ddlTemp.DataTextField = JuryData.Entities.DropValuesCourtColumn.StrDropValueDescription.ToString();
        ddlTemp.DataValueField = JuryData.Entities.DropValuesCourtColumn.AutoDropValueID.ToString();
        ddlTemp.DataBind();

        if (boolAddBlank)
            ddlTemp.Items.Insert(0, new ListItem(strBlankDesc, strBlankValue));

    }

    /// <summary>
    /// Populates a drop down list (App Data Values)
    /// </summary>
    /// <param name="ddlTemp">Drop Down List</param>
    /// <param name="intDropType">App Data Type</param>
    /// <param name="boolAddBlank">if set to <c>true</c> add empty value.</param>
    /// <param name="strBlankDesc">If adding empty value, use this as text</param>
    /// <param name="strBlankValue">If adding blank value, use this as value</param>
    public static void PopulateDDL_App(DropDownList ddlTemp, Int16 intDropType, bool boolAddBlank, string strBlankDesc, string strBlankValue)
    {
        ddlTemp.DataSource = clsData.GetDropValuesAppDataByType(intDropType, true);
        ddlTemp.DataTextField = JuryData.Entities.DropValuesAppColumn.StrDropValueDescription.ToString();
        ddlTemp.DataValueField = JuryData.Entities.DropValuesAppColumn.AutoDropValueID.ToString();
        ddlTemp.DataBind();

        if (boolAddBlank)
            ddlTemp.Items.Insert(0, new ListItem(strBlankDesc, strBlankValue));

    }

    /// <summary>
    /// Populates a drop down list (Court Data Values)
    /// </summary>
    /// <param name="ddlTemp">Drop Down List</param>
    /// <param name="intDropType">Court Data Type</param>
    /// <param name="boolAddBlank">if set to <c>true</c> add empty value.</param>
    /// <param name="strBlankDesc">If adding empty value, use this as text</param>
    /// <param name="strBlankValue">If adding blank value, use this as value</param>
    public static void PopulateDDL(DropDownList ddlTemp, Int16 intDropType, bool boolAddBlank, string strBlankDesc, string strBlankValue)
    {
        ddlTemp.DataSource = clsData.GetDropValuesCourtDataByType(intDropType, true);
        ddlTemp.DataTextField = JuryData.Entities.DropValuesCourtColumn.StrDropValueDescription.ToString();
        ddlTemp.DataValueField = JuryData.Entities.DropValuesCourtColumn.AutoDropValueID.ToString();
        ddlTemp.DataBind();

        if (boolAddBlank)
            ddlTemp.Items.Insert(0, new ListItem(strBlankDesc, strBlankValue));

    }

    /// <summary>
    /// Gets the name of the site from the database
    /// </summary>
    /// <value>The name of the site.</value>
    public static string SiteName
    {
        get { return clsPublic.GetProgramSetting("keySiteName");}
    }

    /// <summary>
    /// Gets an app setting by key
    /// </summary>
    /// <param name="strKey">App Key.</param>
    /// <returns>Value from the key (System.String)</returns>
    public static string GetAppSetting(string strKey)
    {
        if (System.Configuration.ConfigurationManager.AppSettings[strKey] != null)
            return System.Configuration.ConfigurationManager.AppSettings[strKey].ToString();
        else
            return string.Empty;

    }

    /// <summary>
    /// Gets a program setting by key
    /// </summary>
    /// <param name="strKey">Program Key</param>
    /// <returns>Value from the key (System.String)</returns>
    public static string GetProgramSetting(string strKey)
    {
        ProgramSettings entProgramSettings = DataRepository.ProgramSettingsProvider.GetByStrKey(strKey);

        if (entProgramSettings != null)
            return entProgramSettings.StrValue;
        else
            return string.Empty;
    }

    /// <summary>
    /// Updates a program setting by key
    /// </summary>
    /// <param name="strKey">Program Key</param>
    /// <param name="strValue">Value to be stored</param>
    public static void UpdateProgramSetting(string strKey, string strValue)
    {
        ProgramSettings entProgramSettings = DataRepository.ProgramSettingsProvider.GetByStrKey(strKey);

        if (entProgramSettings != null)
        {
            entProgramSettings.StrValue = strValue;
        }
        else
        {
            entProgramSettings = new ProgramSettings();
            entProgramSettings.StrKey = strKey;
            entProgramSettings.StrValue = strValue;
        }

        DataRepository.ProgramSettingsProvider.Save(entProgramSettings);
    }

    #region SQL Direct Proc Calls
    /// <summary>
    /// Gets the DB.
    /// </summary>
    /// <returns>Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase.</returns>
    public static Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase GetDB()
    {
        JuryData.Data.SqlClient.SqlNetTiersProvider p = DataRepository.Provider as JuryData.Data.SqlClient.SqlNetTiersProvider;

        if (null != p)
        {
            // get the SqlDatabase 
            Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase db = new Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(p.ConnectionString);
            return db;
        }
        else
            return null;
    }

    /// <summary>
    /// Adds a parameter to a data command object (integer)
    /// </summary>
    /// <param name="dbCmd">Data Command object</param>
    /// <param name="strParameterName">Parameter Name</param>
    /// <param name="prmDirection">Parameter Direction</param>
    /// <param name="dbTypeExpr">Parameter Data Type</param>
    /// <param name="intValue">Parameter Value (integer)</param>
    public static void AddParameter(ref DbCommand dbCmd, string strParameterName, ParameterDirection prmDirection, DbType dbTypeExpr, int intValue)
    {
        DbParameter dbParam = dbCmd.CreateParameter();
        dbParam.ParameterName = strParameterName;
        dbParam.Direction = prmDirection;
        dbParam.DbType = dbTypeExpr;
        dbParam.Value = intValue;

        dbCmd.Parameters.Add(dbParam);
    }

    /// <summary>
    /// Adds a parameter to a data command object (string)
    /// </summary>
    /// <param name="dbCmd">Data Command object</param>
    /// <param name="strParameterName">Parameter Name</param>
    /// <param name="prmDirection">Parameter Direction</param>
    /// <param name="dbTypeExpr">Parameter Data Type</param>
    /// <param name="intValue">Parameter Value (string)</param>
    public static void AddParameter(ref DbCommand dbCmd, string strParameterName, ParameterDirection prmDirection, DbType dbTypeExpr, string strValue)
    {
        DbParameter dbParam = dbCmd.CreateParameter();
        dbParam.ParameterName = strParameterName;
        dbParam.Direction = prmDirection;
        dbParam.DbType = dbTypeExpr;
        dbParam.Value = strValue;

        dbCmd.Parameters.Add(dbParam);
    }


    /// <summary>
    /// Gets a DB command object by stored procedure text
    /// </summary>
    /// <param name="strSQLCommand">Stored procedure text</param>
    /// <returns>DB Command Object</returns>
    public static System.Data.Common.DbCommand GetDBCommand(string strSQLCommand)
    {
        JuryData.Data.SqlClient.SqlNetTiersProvider p = DataRepository.Provider as JuryData.Data.SqlClient.SqlNetTiersProvider;

        if (null != p)
        {
            // get the SqlDatabase 
            Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase db = new Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(p.ConnectionString);

            // create a command for the slow sproc
            System.Data.Common.DbCommand cmd = db.GetStoredProcCommand(strSQLCommand);

            return cmd;
        }
        else
            return null;


    }

    /// <summary>
    /// Execute a SQL Command
    /// </summary>
    /// <param name="cmd">Command object</param>
    /// <returns>DataSet Results</returns>
    public static DataSet SQLExecuteDataSet(System.Data.Common.DbCommand cmd)
    {

        JuryData.Data.SqlClient.SqlNetTiersProvider p = DataRepository.Provider as JuryData.Data.SqlClient.SqlNetTiersProvider;

        if (null == p)
        {
            // provider is not a SQL provider, do something else appropriate
            return DataRepository.Provider.ExecuteDataSet(cmd);
        }
        else
        {
            // get the SqlDatabase 
            Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase db = new Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(p.ConnectionString);

            // create a command for the slow stored procedures
            // set the timeout as set on the provider (or as needed)
            cmd.CommandTimeout = DataRepository.Provider.DefaultCommandTimeout;

            // execute the command (any of the Execute* family)
            return DataRepository.Provider.ExecuteDataSet(cmd);
        }
    }


    /// <summary>
    /// Execute a SQL Command - no data results
    /// </summary>
    /// <param name="cmd">Command object</param>
    public static void SQLExecuteNonQuery(System.Data.Common.DbCommand cmd)
    {
        // set the timeout as set on the provider (or as needed)
        cmd.CommandTimeout = DataRepository.Provider.DefaultCommandTimeout;

        // execute the command (any of the Execute* family)
        DataRepository.Provider.ExecuteNonQuery(cmd);
    }

    /// <summary>
    /// Execute a SQL Command - no data results
    /// </summary>
    /// <param name="cmd">SQL Command</param>
    public static void SQLExecuteNonQuery(string strSQLCommand)
    {

        JuryData.Data.SqlClient.SqlNetTiersProvider p = DataRepository.Provider as JuryData.Data.SqlClient.SqlNetTiersProvider;

        if (null == p)
        {
            // provider is not a SQL provider, do something else appropriate
            DataRepository.Provider.ExecuteNonQuery(strSQLCommand);
        }
        else
        {
            // get the SqlDatabase 
            Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase db = new Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(p.ConnectionString);

            // create a command for the slow sproc
            System.Data.Common.DbCommand cmd = db.GetStoredProcCommand(strSQLCommand);

            // set the timeout as set on the provider (or as needed)
            cmd.CommandTimeout = DataRepository.Provider.DefaultCommandTimeout;

            // execute the command (any of the Execute* family)
            DataRepository.Provider.ExecuteNonQuery(cmd);
        }
    }

    #endregion


    /// <summary>
    /// Return Selected value from Drop Down List
    /// </summary>
    /// <param name="ddl">Drop Down List</param>
    /// <returns>Return selected value (System.Int32)</returns>
    public static int ValueFromDDL(DropDownList ddl)
    {
        if (ddl != null)
        {
            if (ddl.SelectedValue != null)
            {
                if (clsPublic.IsInteger(ddl.SelectedValue))
                {
                    return Convert.ToInt32(ddl.SelectedValue);
                }
            }
        }

        return -1;
    }

    /// <summary>
    /// Server Name
    /// </summary>
    private static string m_httpServerName = String.Empty;

    /// <summary>
    /// Gets the absolute URL.
    /// </summary>
    /// <returns>URL (System.String)</returns>
    public static string GetAbsoluteUrl()
    {
        string protocol = HttpContext.Current.Request.ServerVariables["SERVER_PORT_SECURE"];
        if (String.IsNullOrEmpty(protocol) | String.Equals(protocol, "0"))
            protocol = "http://";
        else
            protocol = "https://";

        // *** Check for local host and dynamic ports
        string currentAddress = HttpContext.Current.Request.Url.ToString();
        Regex rx = new Regex(@"([^/]*)(localhost|\blocalhost:\d+\b)([^/]*)", RegexOptions.IgnoreCase);
        Match MatchObj = rx.Match(currentAddress);
        if (!(string.IsNullOrEmpty(MatchObj.Groups[0].Value)))
        {
            m_httpServerName = String.Concat(protocol,
                MatchObj.Groups[0].Value,
                    HttpContext.Current.Request.ApplicationPath);
        }
        else
        {
            m_httpServerName = String.Concat(protocol,
                HttpContext.Current.Request.ServerVariables["SERVER_NAME"],
                    HttpContext.Current.Request.ApplicationPath);
        }

        if (!m_httpServerName.EndsWith("/"))
            m_httpServerName += "/";

        return m_httpServerName;
    }


    /// <summary>
    /// Get a Formatted address.
    /// </summary>
    /// <param name="strStreet">Street Text</param>
    /// <param name="strStreet2">Suite Text</param>
    /// <param name="strCity">City Text</param>
    /// <param name="strState">State Text</param>
    /// <param name="strZip">Zip Text</param>
    /// <returns>Formatted Address (System.String)</returns>
    public static string FormattedAddress(string strStreet, string strStreet2, string strCity, string strState, string strZip)
    {
        string strFullAddress = string.Empty;

        if (strStreet.Length > 0 && strStreet2.Length > 0)
            strFullAddress = string.Format("{0}{1}{2}{3}{4}, {5}  {6}", strStreet, "<br/>", strStreet2, "<br/>", strCity, strState, strZip).Replace("  ", " ").Trim();
        else
            strFullAddress = string.Format("{0}{1}{2}, {3}  {4}", strStreet, "<br/>", strCity, strState, strZip).Replace("  ", " ").Trim();

        return strFullAddress;

    }

    /// <summary>
    /// Convert the session ResearchID to an integer
    /// </summary>
    /// <returns>Session ID (System.Int32)</returns>
    public static int SessionResearchID()
    {
        string strResearchID = SessionValue("ResearchID");

        if (IsInteger(strResearchID))
            return Convert.ToInt32(strResearchID);
        else
            return 0;

    }


    /// <summary>
    /// Return a session value by string
    /// </summary>
    /// <param name="strSessionExpr">Session string</param>
    /// <returns>Session Value (System.String)</returns>
    public static string SessionValue(string strSessionExpr)
    {
        if (HttpContext.Current.Session[strSessionExpr] != null)
        {
            return HttpContext.Current.Session[strSessionExpr].ToString();
        }
        else
            return string.Empty;

    }

    /// <summary>
    /// Replaces a carriage return with BR.
    /// </summary>
    /// <param name="strExpr">String to be replaced</param>
    /// <returns>Return modified string (System.String)</returns>
    public static string ReplaceCRWithBR(string strExpr)
    {
        char chrTemp = Convert.ToChar(10);

        return strExpr; // strExpr.Replace(Environment.NewLine, "<br/>").Replace(@"\n", "<br/>").Replace(chrTemp.ToString(), "<br/>");
    }

    /// <summary>
    /// Return the left substring of text
    /// </summary>
    /// <param name="text">Original text</param>
    /// <param name="length">Length of left text</param>
    /// <returns>Return modified string (System.String)</returns>
    /// <exception cref="System.ArgumentOutOfRangeException"></exception>
    public static string Left(string text, int length)
    {
        if (length < 0)
            throw new ArgumentOutOfRangeException("length", length, "length must be > 0");
        else if (length == 0 || text.Length == 0)
            return "";
        else if (text.Length <= length)
            return text;
        else
            return text.Substring(0, length);
    }

    /// <summary>
    /// Logs an exception to the database
    /// </summary>
    /// <param name="ex">Exception</param>
    public static void LogError(Exception ex)
    {
        JBErrors entJBErrors = new JBErrors();

        entJBErrors.DatJBErrorsEntered = DateTime.Now;
        entJBErrors.MemJBErrorsMessage = string.Format("Source: {0} - {1}{2}Stack: {3}", ex.Source, ex.Message, Environment.NewLine,ex.StackTrace);

        DataRepository.JBErrorsProvider.Save(entJBErrors);

        clsPublic.SendErrorEmail(entJBErrors);
    }

    /// <summary>
    /// Logs an error message to the database
    /// </summary>
    /// <param name="strMessage">Error Message to log</param>
    public static void LogError(string strMessage)
    {
        JBErrors entJBErrors = new JBErrors();

        entJBErrors.DatJBErrorsEntered = DateTime.Now;
        entJBErrors.MemJBErrorsMessage = strMessage;

        DataRepository.JBErrorsProvider.Save(entJBErrors);

        clsPublic.SendErrorEmail(entJBErrors);
    }

    /// <summary>
    /// Sends a message - not necessarily an error
    /// </summary>
    /// <param name="strMessage">The message.</param>
    public static void SendMessage(string strMessage)
    {
        JBErrors entJBErrors = new JBErrors();

        entJBErrors.DatJBErrorsEntered = DateTime.Now;
        entJBErrors.MemJBErrorsMessage = strMessage;

        DataRepository.JBErrorsProvider.Save(entJBErrors);

        SendMessageEmail(entJBErrors);
    }

    /// <summary>
    /// Get Jury Data Drop Description by ID
    /// </summary>
    /// <param name="intDropValueID">Jury data ID</param>
    /// <returns>Jury Data Description (System.String)</returns>
    public static string GetDropValueJB(int intDropValueID)
    {
        DropValuesJB entDropValuesJB = DataRepository.DropValuesJBProvider.GetByAutoDropValueID(intDropValueID);

        if (entDropValuesJB != null)
            return entDropValuesJB.StrDropValueDescription;
        else
            return string.Empty;
    }

    /// <summary>
    /// Formats an HTML email.
    /// </summary>
    /// <param name="strKey">Message Key</param>
    /// <param name="strTo">Send To.</param>
    /// <param name="items">Items to attach</param>
    /// <returns>Returns a mail message object</returns>
    public static MailMessage FormatHTMLEmail(string strKey, string strTo, params object[] items)
    {
        string strBody = clsPublic.GetProgramSetting(strKey + "_Body");
        string strSubject = clsPublic.GetProgramSetting(strKey + "_Subject");
        string strFrom = clsPublic.GetProgramSetting(strKey + "_From");
        string strBCC = clsPublic.GetProgramSetting(strKey + "_BCC");
        string strWrapperStart = clsPublic.GetProgramSetting("keyMailWrapperStart");
        string strWrapperEnd = clsPublic.GetProgramSetting("keyMailWrapperEnd");


        strBody = string.Format(strBody, items);
       

        string body = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">";
        body += "<HTML><HEAD><META http-equiv=Content-Type content=\"text/html; charset=iso-8859-1\">";
        body += "</HEAD><BODY>";
        body += strWrapperStart;
        body += strBody;
        body += strWrapperEnd;
        body += "</BODY></HTML>";

        strBody = strBody.Replace("<br/>", Environment.NewLine);
        strBody = strBody.Replace("</p>", Environment.NewLine);

        strBody = clsPublic.StripHTML(strBody);

        MailMessage mm = new MailMessage();
        mm.To.Add(strTo);
        mm.From = new MailAddress(strFrom);
        mm.Body = strBody;
        mm.Subject = strSubject;

        ContentType mimeType = new System.Net.Mime.ContentType("text/html");

        AlternateView alternate = AlternateView.CreateAlternateViewFromString(body, mimeType);
        mm.AlternateViews.Add(alternate);

        return mm;
    }

    /// <summary>
    /// Sends an email message object
    /// </summary>
    /// <param name="mm">Mail Message Object</param>
    public static void SendEmail(MailMessage mm)
    {
        try
        {
            SmtpClient smtp = new SmtpClient();

            smtp.Send(mm);
        }
        catch (Exception ex)
        {
            clsPublic.LogError(ex);
        }
    }


    /// <summary>
    /// Send an email
    /// </summary>
    /// <param name="strSubject">Email subject.</param>
    /// <param name="strBody">Email body.</param>
    /// <param name="strFrom">Email from.</param>
    /// <param name="strSentTo">Email sent to.</param>
    public static void SendEmail(string strSubject, string strBody, string strFrom, string strSentTo)
    {
        try
        {
            MailMessage msg = new MailMessage(strFrom, strSentTo);

            msg.Body = strBody;

            msg.Subject = strSubject;
            msg.IsBodyHtml = false;

            SmtpClient smtp = new SmtpClient();

            smtp.Send(msg);
        }
        catch(Exception)
        {

        }
    }


    /// <summary>
    /// Sends a message email from a Jury Research Error record
    /// </summary>
    /// <param name="entJBErrors">Error Record</param>
    public static void SendMessageEmail(JBErrors entJBErrors)
    {
        if (entJBErrors != null)
        {
            string strSentTo = clsPublic.GetProgramSetting("keyErrorEmailTo");
            string strFrom = clsPublic.GetProgramSetting("keyErrorEmailFrom");
            string strSubject = "Jury Research Message";
            string strBody = entJBErrors.MemJBErrorsMessage;

            clsPublic.SendEmail(strSubject, strBody, strFrom, strSentTo);

        }
    }

    /// <summary>
    /// Sends an error email from a Jury Research Error record
    /// </summary>
    /// <param name="entJBErrors">Error Record</param>
    public static void SendErrorEmail(JBErrors entJBErrors)
    {
        if (entJBErrors != null)
        {
            string strSentTo = clsPublic.GetProgramSetting("keyErrorEmailTo");
            string strFrom = clsPublic.GetProgramSetting("keyErrorEmailFrom");
            string strSubject = "Jury Research Error";
            string strBody = entJBErrors.MemJBErrorsMessage;

            clsPublic.SendEmail(strSubject, strBody, strFrom, strSentTo);
        }
    }

    /// <summary>
    /// Resolves the server URL.
    /// </summary>
    /// <param name="serverUrl">The server URL.</param>
    /// <returns>Returns URL (System.String)</returns>
    public static string ResolveServerUrl(string serverUrl) 
    { 
        return ResolveServerUrl(serverUrl, false); 
    }

    /// <summary>
    /// Resolves the server URL.
    /// </summary>
    /// <param name="serverUrl">The server URL.</param>
    /// <param name="forceHttps">if set to <c>true</c> [force HTTPS].</param>
    /// <returns>Returns URL (System.String)</returns>
    public static string ResolveServerUrl(string serverUrl, bool forceHttps)
    {    // *** Is it already an absolute Url?    
        if (serverUrl.IndexOf("://") > -1)        
            return serverUrl;     
        
        // *** Start by fixing up the Url an Application relative Url    
        
        string newUrl = ResolveUrl(serverUrl);     
        Uri originalUri = HttpContext.Current.Request.Url;    
        newUrl = (forceHttps ? "https" : originalUri.Scheme) +  "://" + originalUri.Authority + newUrl;     
        return newUrl;
    }

    /// <summary>
    /// Resolves the URL.
    /// </summary>
    /// <param name="originalUrl">The original URL.</param>
    /// <returns>Returns Resolved URL (System.String)</returns>
    public static string ResolveUrl(string originalUrl)
    {
        if (originalUrl == null)
            return null;

        // *** Absolute path - just return
        if (originalUrl.IndexOf("://") != -1)
            return originalUrl;

        // *** Fix up image path for ~ root app dir directory
        if (originalUrl.StartsWith("~"))
            return VirtualPathUtility.ToAbsolute(originalUrl);

        return originalUrl;
    }

    /// <summary>
    /// Sends a user an email by membership GUID
    /// </summary>
    /// <param name="userID">Membership GUID</param>
    /// <param name="strMessageKey">Message Key to retrieve and send.</param>
    public static void SendContactEmail(Guid userID, string strMessageKey)
    {
        MembershipUser mu = Membership.GetUser(userID);
        ProfileCommon muprofile = (ProfileCommon)ProfileBase.Create(mu.UserName, false);

        ProfileCommon targetProfile = muprofile.GetProfile(mu.UserName); 
    }

    /// <summary>
    /// Loads the page meta data.
    /// </summary>
    /// <param name="mp">Master Page</param>
    /// <param name="strPageName">Page Name</param>
    public static void LoadPageMetaData(BaseMasterPage mp, string strPageName)
    {
        TList<PageHTMLTextMeta> tlstPageHTMLTextMeta = DataRepository.PageHTMLTextMetaProvider.GetByStrHTMLPage(strPageName.ToUpper());

        tlstPageHTMLTextMeta.ForEach(delegate(PageHTMLTextMeta entPageHTMLTextMeta)
        {
            mp.AddMetaData(entPageHTMLTextMeta.StrHTMLMetaKeyword, entPageHTMLTextMeta.StrHTMLMetaContent);
        });

    }


    /// <summary>
    /// Loads the page meta data.
    /// </summary>
    /// <param name="mp">Master Page</param>
    /// <param name="strPageName">Page Name</param>
    public static void LoadPageMetaData(BaseMasterPageSub mp, string strPageName)
    {
        TList<PageHTMLTextMeta> tlstPageHTMLTextMeta = DataRepository.PageHTMLTextMetaProvider.GetByStrHTMLPage(strPageName.ToUpper());

        tlstPageHTMLTextMeta.ForEach(delegate(PageHTMLTextMeta entPageHTMLTextMeta)
        {
            mp.AddMetaData(entPageHTMLTextMeta.StrHTMLMetaKeyword, entPageHTMLTextMeta.StrHTMLMetaContent);
        });

    }

    /// <summary>
    /// Retrieves page text from database
    /// </summary>
    /// <param name="strPageName">Page Name</param>
    /// <param name="intIndex">Page text index</param>
    /// <param name="strPageTitle">Page Title</param>
    /// <param name="strPageLabel">Page Label.</param>
    /// <param name="strPageText">Page Text</param>
    public static void UpdatePageText(string strPageName, int intIndex, string strPageTitle, string strPageLabel, string strPageText)
    {
        PageHTMLText entPageHTMLText = DataRepository.PageHTMLTextProvider.GetByStrHTMLPageIntHTMLIndex(strPageName, intIndex);

        if (entPageHTMLText != null)
        {
            entPageHTMLText.MemHTMLText = strPageText;
            entPageHTMLText.StrHTMLPageLabel = strPageLabel;
            entPageHTMLText.StrHTMLPageTitle = strPageTitle;

            DataRepository.PageHTMLTextProvider.Save(entPageHTMLText);
        }
        else
        {
            entPageHTMLText = new PageHTMLText();

            entPageHTMLText.IntHTMLIndex = intIndex;
            entPageHTMLText.StrHTMLPage = strPageName;
            entPageHTMLText.MemHTMLText = strPageText;
            entPageHTMLText.StrHTMLPageLabel = strPageLabel;
            entPageHTMLText.StrHTMLPageTitle = strPageTitle;

            DataRepository.PageHTMLTextProvider.Save(entPageHTMLText);


        }

    }

    /// <summary>
    /// Retrieves page text from database
    /// - provides ability to return page text from the HTML table in the database
    /// - returns the text based upon the index specified (allows for multiple pages sections on a page)
    /// </summary>
    /// <param name="strPageName">Page Name</param>
    /// <param name="intIndex">Page text index</param>
    /// <param name="strPageTitle">Page Title</param>
    /// <param name="strPageLabel">Page Label</param>
    /// <returns>HTML Text (System.String)</returns>
    public static string PageText(string strPageName, int intIndex, ref string strPageTitle, ref string strPageLabel)
    {
        PageHTMLText entPageHTMLText = DataRepository.PageHTMLTextProvider.GetByStrHTMLPageIntHTMLIndex(strPageName, intIndex);

        if (entPageHTMLText != null)
        {
            strPageTitle = entPageHTMLText.StrHTMLPageTitle == null ? string.Empty : entPageHTMLText.StrHTMLPageTitle;
            strPageLabel = entPageHTMLText.StrHTMLPageLabel == null ? string.Empty : entPageHTMLText.StrHTMLPageLabel;

            return entPageHTMLText.MemHTMLText;
        }
        else
            return string.Empty;

    }

    /// <summary>
    /// Generates a random integer
    /// </summary>
    /// <param name="min">Min</param>
    /// <param name="max">Max</param>
    /// <returns>Random integer (System.Int32)</returns>
    public static int RandomNumber(int min, int max)
    {
        Random random = new Random();
        return random.Next(min, max);
    }

    /// <summary>
    /// Checks if the current user belongs to a certain role
    /// </summary>
    /// <param name="strRole">User Role</param>
    /// <returns><c>true</c> if User belongs to specified role, <c>false</c> otherwise</returns>
    public static bool CheckUserInRole(string strRole)
    {
        WindowsPrincipal User = new 
        WindowsPrincipal((WindowsIdentity)HttpContext.Current.User.Identity);
        if (User.IsInRole(strRole))
            return true;
        else
            return false;
    }


    /// <summary>
    /// Get file name page by href link
    /// </summary>
    /// <param name="hrefLink">Href link.</param>
    /// <returns>Filename (System.String)</returns>
    public static string GetFileName(string hrefLink)
    {    
        string[] parts = hrefLink.Split('/');    
        string fileName = "";    
        if (parts.Length > 0)        
            fileName = parts[parts.Length - 1];    
        else        
            fileName = hrefLink;    
        
        return fileName;
    }

    /// <summary>
    /// Gets the file name path by href link
    /// </summary>
    /// <param name="hrefLink">Href link</param>
    /// <returns>Path (System.String)</returns>
    public static string GetFileNamePath(string hrefLink)
    {
        string[] parts = hrefLink.Split('/');
        string fileName = "";
        if (parts.Length > 0)
            fileName = parts[parts.Length - 2];
        else
            fileName = hrefLink;

        return fileName;

    }

    /// <summary>
    /// Changes the research tab.
    /// </summary>
    /// <param name="intTab">Tab to change to</param>
    public static void ChangeResearchTab(int intTab)
    {
        HttpContext ctx = HttpContext.Current;

        if (ctx != null)
        {
            switch (intTab)
            {
                case 1:
                    ctx.Response.Redirect("~/Research/ResearchName.aspx");
                    break;
                case 2:
                    ctx.Response.Redirect("~/Research/ResearchQuestionsV2.aspx");
                    break;
                case 3:
                    ctx.Response.Redirect("~/Research/ResearchQuestionSummaryV2.aspx");
                    break;

                case 4:
                    ctx.Response.Redirect("~/Research/ResearchRespondents.aspx");
                    break;
                case 5:

                    ctx.Response.Redirect("~/Research/ResearchOrderSummary.aspx");
                    break;
            }
        }
    }


}
