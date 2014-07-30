// ***********************************************************************
// Author           : Dennis Sebenick
// Created          : 05-30-2012
//
// Last Modified By : dennis
// Last Modified On : 06-04-2012
// ***********************************************************************
// <copyright file="clsSendMessages.cs" company="DGCC">
//     . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

using JuryData.Data;
using JuryData.Entities;
using System.Net.Mail;
using System.Text;
using System.IO;


/// <summary>
/// Class for sending email messages
/// </summary>
public static class clsSendMessages
{

    /// <summary>
    /// Gets a users email by their login ID
    /// </summary>
    /// <param name="strLoginID">Login ID</param>
    /// <returns>Email Address (System.String)</returns>
    public static string GetLoginEmail(string strLoginID)
    {
        try
        {
            MembershipUser mu = Membership.GetUser(strLoginID);

            if (mu != null)
            {
                return mu.Email;

            }
            else
                return string.Empty;
        }
        catch (Exception ex)
        {
            clsPublic.LogError(ex);
            return string.Empty;
        }

    }

    /// <summary>
    /// Sends email message to a user
    /// </summary>
    /// <param name="strLoginID">Login ID</param>
    /// <param name="strFrom">From</param>
    /// <param name="strCC">CC</param>
    /// <param name="strBCC">BCC</param>
    /// <param name="strSubject">Subject</param>
    /// <param name="strBody">Body</param>
    /// <param name="boolIsHTML">Send HTML version</param>
    public static void SendMessageToUser(string strLoginID, string strFrom, string strCC, string strBCC, string strSubject, string strBody, bool boolIsHTML )
    {
        try
        {
            MailMessage mm = new MailMessage();
            
            string strTo = GetLoginEmail(strLoginID);

            if (strTo.Length > 0)
            {
                mm.To.Add(strTo);

                if (strCC.Length > 0)
                {
                    mm.CC.Add(strCC);
                }

                if (strBCC.Length > 0)
                {
                    mm.Bcc.Add(strBCC);
                }

                if (strSubject.Length > 0)
                {
                    mm.Subject = strSubject;
                }

                if (strBody.Length > 0)
                {
                    mm.IsBodyHtml = boolIsHTML;
                    mm.Body = strBody;
                }

                SetLogo(mm);
                clsPublic.SendEmail(mm);
            }
        }
        catch (Exception ex)
        {
            clsPublic.LogError(ex);
        }
    }

    private static void SetLogo(MailMessage mm)
    {
        AlternateView av1 = AlternateView.CreateAlternateViewFromString(mm.Body, null, System.Net.Mime.MediaTypeNames.Text.Html);
        string strImageUrl = System.Web.HttpContext.Current.Server.MapPath("~/images/193S.jpg");
        LinkedResource logo = new LinkedResource(strImageUrl, System.Net.Mime.MediaTypeNames.Image.Jpeg);
        logo.ContentId = "Logojpg";
        av1.LinkedResources.Add(logo);
        mm.AlternateViews.Add(av1);
    }

    /// <summary>
    /// Send email verification to new user
    /// </summary>
    /// <param name="strLoginID">Login ID</param>
    public static void SendVerificationEmail(string strLoginID)
    {
        string strFrom = clsPublic.GetProgramSetting("keyLoginVerify_From");
        string strSubject = clsPublic.GetProgramSetting("keyLoginVerify_Subject");
        string strBody = clsPublic.GetProgramSetting("keyLoginVerify_Body");
        string strBCC = clsPublic.GetProgramSetting("keyLoginVerify_BCC");

        string strSiteURL = clsPublic.ResolveServerUrl("~/Login/UserVerification.aspx", false);

        MembershipUser mu = Membership.GetUser(strLoginID);

        if (mu != null)
        {
            strBody = GetVerifyPage(strSiteURL + "?ID=" + mu.ProviderUserKey.ToString(), mu.UserName);
            //strBody = string.Format(strBody, strSiteURL + "?ID=" + mu.ProviderUserKey.ToString());
            SendMessageToUser(strLoginID, strFrom, string.Empty, strBCC, strSubject, strBody, true);
        }
    }

    private static string GetVerifyPage(String strLinkURL, String username)
    {
        String strResult = "";
        String spath = System.Web.HttpContext.Current.Server.MapPath("~/Login/EmailTemplates/VerifyUserMail.htm");
        using (StreamReader reader = File.OpenText(spath))
        {
            strResult = reader.ReadToEnd();
        }

        strResult = strResult.Replace("--UserName--", username);
        strResult = strResult.Replace("--urllink--", strLinkURL);

        return strResult;
    }

    private static string GetNewUserPage(MembershipUser mu,UserData uData,ProfileCommon pc)
    {
        String strResult = "";
        String spath = System.Web.HttpContext.Current.Server.MapPath("~/Login/EmailTemplates/NewUserMail.htm");
        using (StreamReader reader = File.OpenText(spath))
        {
            strResult = reader.ReadToEnd();
        }

        strResult = strResult.Replace("--UserName--", mu.UserName);
        strResult = strResult.Replace("--Email--", mu.Email);
        strResult = strResult.Replace("--Hear About--", uData.StrReferrer);
        strResult = strResult.Replace("--FirstName--", uData.StrFirstName);
        strResult = strResult.Replace("--LastName--", uData.StrLastName);
        strResult = strResult.Replace("--Employer--", pc.Employer);
        strResult = strResult.Replace("--Title--", uData.StrTitle);
        strResult = strResult.Replace("--Street--", uData.StrStreet);
        strResult = strResult.Replace("--Suite--", uData.StrSuite);
        strResult = strResult.Replace("--City--", uData.StrCity);
        strResult = strResult.Replace("--State--", uData.StrState);
        strResult = strResult.Replace("--Zip--", uData.StrZip);

        return strResult;
    }

    /// <summary>
    /// Sends the new user email.
    /// </summary>
    /// <param name="mu">Membership User Object</param>
    /// <param name="uData">User Data</param>
    /// <param name="pc">Profile</param>
    public static void SendNewUserEmail(MembershipUser mu, UserData uData, ProfileCommon pc)
    {
        try
        {
            string strSendTo = clsPublic.GetProgramSetting("keyNewUserSendTo");

            if (strSendTo.Length == 0)
            {
                strSendTo = clsPublic.GetProgramSetting("keyContactSendTo");
                clsPublic.UpdateProgramSetting("keyNewUserSendTo", strSendTo);
            }

            MailMessage mm = new MailMessage();
            mm.To.Add(strSendTo);
            mm.From = new MailAddress(strSendTo);

            string strSubject = clsPublic.GetProgramSetting("keyNewUserSubject");

            if (strSubject.Length == 0)
            {
                strSubject = "DecisionQuest Online New User Account Created";
                clsPublic.UpdateProgramSetting("keyNewUserSubject", strSubject);
            }

            mm.Subject = strSubject;

            //StringBuilder sb = new StringBuilder();

            //sb.AppendFormat("UserName:  {0}", mu.UserName);
            //sb.AppendLine();
            //sb.AppendFormat("Email:  {0}", mu.Email);
            //sb.AppendLine();
            //sb.AppendFormat("First Name:  {0}", uData.StrFirstName);
            //sb.AppendLine();
            //sb.AppendFormat("Last Name:  {0}", uData.StrLastName);
            //sb.AppendLine();
            //sb.AppendFormat("Employer:  {0}", pc.Employer);
            //sb.AppendLine();
            //sb.AppendFormat("Title:  {0}", uData.StrTitle);
            //sb.AppendLine();
            //sb.AppendFormat("Street:  {0}", uData.StrStreet);
            //sb.AppendLine();
            //sb.AppendFormat("Suite:  {0}", uData.StrSuite);
            //sb.AppendLine();
            //sb.AppendFormat("City:  {0}", uData.StrCity);
            //sb.AppendLine();
            //sb.AppendFormat("State:  {0}", uData.StrState);
            //sb.AppendLine();
            //sb.AppendFormat("Zip:  {0}", uData.StrZip);
            //sb.AppendLine();

            mm.Body = GetNewUserPage(mu, uData, pc);
            SetLogo(mm);
            clsPublic.SendEmail(mm);

        SendVerificationEmail(mu.UserName);


        }
        catch (Exception ex)
        {
            clsPublic.LogError(ex);
        }
    }

    /// <summary>
    /// Sends the new user email.
    /// </summary>
    public static void SendCurrentStatusEmail(String tblData)
    {
        try
        {
            String strResult = "";
            String spath = System.Web.HttpContext.Current.Server.MapPath("~/GetResults/EmailTemplate/current_status.htm");
            using (StreamReader reader = File.OpenText(spath))
            {
                strResult = reader.ReadToEnd();
            }

            strResult = strResult.Replace("--tableData--", tblData);

            MailMessage mm = new MailMessage();
            ////String[] mmtoList = new String[] { "v-nbekirski@gmi-mr.com", "cneau@gmi-mr.com", "v-yvulov@gmi-mr.com", "v-llubenov@gmi-mr.com", "v-nbozhkov@gmi-mr.com", "thunter@gmi-mr.com", "Sample-NA@gmi-mr.com" };

	String[] mmtoList = new String[] { "Sadaf.Ahsan@surveysampling.com", "Travis.Miller@surveysampling.com", "April.Siao@surveysampling.com","Ahsan_Delivery_Team@SurveySampling.com", "Afterhours_Delivery_Team@SurveySampling.com" };

            String[] mmccList = new String[] { "bmorse@decisionquest.com", "saurav.basu@exavalu.com", "rakesh.ranjan@exavalu.com", "mukesh.tungaria@exavalu.com" };

            //String[] mmtoList = new String[] { "mukesh.tungaria@exavalu.com", "ravendra.chaudhary@exavalu.com" };
            //String[] mmccList = new String[] { "mukesh.tungaria@exavalu.com", "ravendra.chaudhary@exavalu.com" };
            foreach (String mmto in mmtoList)
                mm.To.Add(mmto);
            foreach (String mmcc in mmccList)
                mm.CC.Add(mmcc);

            mm.Subject = "DecisionQuest Online Open Survey Status";
            mm.Body = strResult;
            SetLogo(mm);
            clsPublic.SendEmail(mm);
        }
        catch (Exception ex)
        {
            clsPublic.LogError(ex);
        }
    }

    /// <summary>
    /// Send Error SGQuestion Email.
    /// </summary>
    public static void SendErrorSGQuestionEmail(String strbody)
    {
        try
        {
            String strResult = "";
            String spath = System.Web.HttpContext.Current.Server.MapPath("~/GetResults/EmailTemplate/SGQuestion_status.htm");
            using (StreamReader reader = File.OpenText(spath))
            {
                strResult = reader.ReadToEnd();
            }

            strResult = strResult.Replace("--tableData--", strbody);
            MailMessage mm = new MailMessage();
            String[] mmtoList = new String[] { "mukesh.tungaria@exavalu.com", "ravendra.chaudhary@exavalu.com" };
            foreach (String mmto in mmtoList)
                mm.To.Add(mmto);
            mm.Subject = "DecisionQuest Online Difference/Error Survey Gizmo Question";
            mm.Body = strResult;
            SetLogo(mm);
            clsPublic.SendEmail(mm);
        }
        catch (Exception ex)
        {
            clsPublic.LogError(ex);
        }
    }

    /// <summary>
    /// Gets mail body for the result intimation email to be sent to user, who created the survey.
    /// </summary>
    /// <param name="mu">Membership User Object</param>
    /// <param name="strSurveyTitle">Title of the survey</param>
    /// <returns></returns>
    private static string GetResultMailBody(MembershipUser mu,String strSurveyTitle)
    {
        String strResult = "";
        String spath = System.Web.HttpContext.Current.Server.MapPath("~/Login/EmailTemplates/SurveyResult.htm");
        using (StreamReader reader = File.OpenText(spath))
        {
            strResult = reader.ReadToEnd();
        }

        strResult = strResult.Replace("--FirstName--", mu.UserName);
        strResult = strResult.Replace("--Survey--", strSurveyTitle);
        strResult = strResult.Replace("--Link--", System.Configuration.ConfigurationManager.AppSettings["Host"] );
        
        return strResult;
    }

    /// <summary>
    /// Sends the result intimation email to user email, who created the survey.
    /// </summary>
    /// <param name="strSurveyTitle">Title of the survey</param>
    public static void SendResultEmail(String strSurveyTitle)
    {
        try
        {
            MembershipUser mu = Membership.GetUser(true);
            MailMessage mm = new MailMessage();
            mm.To.Add(mu.Email);
            mm.From = new MailAddress(clsPublic.GetProgramSetting("keyMessage_ResultsSentByEmail"));

            string strSubject = clsPublic.GetProgramSetting("keyMessage_ResultsSubject");

            if (strSubject.Length == 0)
            {
                strSubject = "DecisionQuest Survey Results Ready";
                clsPublic.UpdateProgramSetting("keyMessage_ResultsSubject", strSubject);
            }

            mm.Subject = strSubject;

            mm.Body = GetResultMailBody(mu, strSurveyTitle);
            SetLogo(mm);
            clsPublic.SendEmail(mm);
            
        }
        catch (Exception ex)
        {
            clsPublic.LogError(ex);
        }
    }
}