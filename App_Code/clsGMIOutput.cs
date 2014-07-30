// ***********************************************************************
// Author           : Dennis Sebenick
// Created          : 08-29-2011
//
// Last Modified By : dennis
// Last Modified On : 09-04-2012
// ***********************************************************************
// <copyright file="clsGMIOutput.cs" company="">
//     . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Net.Mail;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;


using JuryData.Data;
using JuryData.Entities;

/// <summary>
/// Class to control output to GMI
/// </summary>
public class clsGMIOutput
{
    /// <summary>
    /// 
    /// </summary>
    string CrLf = "\r\n";

    /// <summary>
    /// Initializes a new instance of the <see cref="clsGMIOutput" /> class.
    /// </summary>
    public clsGMIOutput()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    /// <summary>
    /// Generates email to GMI
    /// - request to start survey
    /// </summary>
    /// <param name="intResearchID">Research ID</param>
    /// <returns>True on success, false on failure</returns>
    public bool SubmitSurveyV2(int intResearchID)
    {
        try
        {

            ResearchMain entResearchMain = DataRepository.ResearchMainProvider.GetByAutoResearchID(intResearchID);

            if (entResearchMain != null)
            {
                int intRespondentCount_Basic = clsSurveyHelper.RespondentCountBasic(intResearchID);
                int intRespondentCount_DMA = clsSurveyHelper.RespondentCountDMA(intResearchID);
                int intRespondentCount_Zip = clsSurveyHelper.RespondentCountZipCode(intResearchID);
                int intRespondentCount_County = clsSurveyHelper.RespondentCountCounty(intResearchID);

                StringBuilder sb = new StringBuilder();
                StringBuilder sb2 = new StringBuilder();

                System.Guid guidUserID;

                System.Guid.TryParse(entResearchMain.UserID.ToString(), out guidUserID);

                System.Web.Security.MembershipUser mu = Membership.GetUser(guidUserID);

                string strDemoLink = clsPublic.GetProgramSetting("keySGDemoLink");

                strDemoLink = string.Format(strDemoLink, entResearchMain.IntSGSurveyID);

                sb.AppendLine("Hello," + CrLf);
                sb.AppendLine("We need the following study launched ASAP:");

                sb.AppendLine();
                sb.AppendLine("USA");
                sb.AppendLine("Region/geo-specifics:");
                sb.AppendLine("{0}");
                sb.AppendLine();
                sb.AppendLine("Estimated length of interview (LOI): {1}");
                sb.AppendLine("Estimated incidence (IR): {2}");
                sb.AppendLine("Required N: {3}");
                sb.AppendLine("Expected timeline: {4}");
                sb.AppendLine("Screening Criteria: {5}");

                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine("Live Link:");
                sb.AppendLine();

                sb.AppendFormat(@"Survey Preview Link: {0}&sn=[sn]&ac=[ac]&lang=E", strDemoLink);
                sb.AppendLine();
                sb.AppendFormat(@"Production Link:  {0}?sn=[sn]&ac=[ac]&lang=E", entResearchMain.StrResearchURL);
                sb.AppendLine();
                sb.AppendLine();
                sb.Append("-- Replace [SN] with Study Number and [AC] with Account Code");
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine("Once test IDs have been approved, you have approval to full launch for the required N and can close ASAP");
                sb.AppendLine();

                TList<ResearchResponders> tlstResearchResponders = DataRepository.ResearchRespondersProvider.GetByIntRespondersResearchID(intResearchID);

                StringBuilder sbTarget = new StringBuilder();
                int intTargetN = 0;

                if (tlstResearchResponders != null)
                {
                    if (intRespondentCount_Zip > 0)
                    {
                        intTargetN = intRespondentCount_Zip;

                        sbTarget.AppendLine("-------------------");
                        sbTarget.AppendLine("- Target Zip Codes:");
                        sbTarget.AppendLine("-------------------");

                        TList<ResearchRespondersDetailZipCode> tlstResearchResondersDetailZipCode = DataRepository.ResearchRespondersDetailZipCodeProvider.GetByIntResponderID(intResearchID);
                        tlstResearchResondersDetailZipCode.Sort(ResearchRespondersDetailZipCodeColumn.StrResponderZipCode.ToString());
                        tlstResearchResondersDetailZipCode.ForEach(delegate(ResearchRespondersDetailZipCode entResearchRespondersDetailZipCode)
                        {
                            sbTarget.AppendLine(string.Format("-- {0}", entResearchRespondersDetailZipCode.StrResponderZipCode));
                        });
                    }
                    else if (intRespondentCount_County > 0)
                    {
                        intTargetN = intRespondentCount_County;

                        SortedList<string, string> dictZipCodes = new SortedList<string, string>();

                        sbTarget.AppendLine("-------------------");
                        sbTarget.AppendLine("- Target Counties:");
                        sbTarget.AppendLine("-------------------");

                        TList<ResearchRespondersDetail> tlstResearchRespondersDetail = DataRepository.ResearchRespondersDetailProvider.GetByIntResponderID(intResearchID);

                        tlstResearchRespondersDetail.ForEach(delegate(ResearchRespondersDetail entResearchRespondersDetail)
                        {
                            int intCountyID = 0;

                            int.TryParse(entResearchRespondersDetail.IntResponderDetailValue.ToString(), out intCountyID);

                            TList< DropValuesCourt> tlstDropValuesCourt = DataRepository.DropValuesCourtProvider.GetByIntDropValueTypeIntDropValueParentType(15, intCountyID);

                            if (tlstDropValuesCourt != null)
                            {
                                if (tlstDropValuesCourt.Count > 0)
                                {
                                    sbTarget.AppendFormat("-- {0}, {1}", tlstDropValuesCourt[0].StrDropValueDescription, tlstDropValuesCourt[0].StrDropValueAbbreviation);
                                    sbTarget.AppendLine();
                                }
                            }
                        });
                    }
                    
                    else if (intRespondentCount_Basic > 0)
                    {
                        intTargetN = intRespondentCount_Basic;

                        sbTarget.AppendLine("----------------------------------------");
                        sbTarget.AppendLine("Target Nationwide (no zip code filter)");
                        sbTarget.AppendLine("----------------------------------------");
                    }
                }

                try
                {
                    string strSentTo = clsPublic.GetProgramSetting("keyGMISentToEmail");
                    string strFrom = clsPublic.GetProgramSetting("keyGMIFromEmail");
                    string strSubject = string.Format(clsPublic.GetProgramSetting("keyGMISubject"), intResearchID);
                    string strCC = clsPublic.GetProgramSetting("keyGMICC");
                    string strBody = clsPublic.GetProgramSetting("keyGMIBody");

                    MailMessage msg = new MailMessage(strFrom, strSentTo);

                    strBody = string.Format(sb.ToString(), sbTarget.ToString(), string.Empty, string.Empty, intTargetN, string.Empty, string.Empty);

                    msg.Body = strBody;

                    string[] strAddresses = strCC.Split(',');

                    foreach (string strAddr in strAddresses)
                    {
                        if (strAddr.Length > 0)
                            msg.CC.Add(new MailAddress(strAddr));
                    }

                    msg.Subject = strSubject;
                    msg.IsBodyHtml = false;

                    SmtpClient smtp = new SmtpClient();

                    smtp.Send(msg);

                    entResearchMain.DatResearchSubmitted = DateTime.Now;

                    DataRepository.ResearchMainProvider.Save(entResearchMain);

                    if (mu != null)
                    {
                        clsPublic.SendEmail(clsPublic.FormatHTMLEmail("keySurveyStart", mu.Email, strDemoLink));
                    }
                }
                catch (Exception ex)
                {
                    clsPublic.LogError(ex);
                    return false;
                }

                return true;
            }
            else
                return false;

        }
        catch (Exception ex)
        {
            clsPublic.LogError(ex);

            return false;
        }


    }

    /// <summary>
    /// Submits the survey to GMI
    /// - old version, no longer used
    /// </summary>
    /// <param name="intResearchID">Research ID</param>
    /// <returns>Returns true on success</returns>
    public bool SubmitSurvey(int intResearchID)
    {
        try
        {

            ResearchMain entResearchMain = DataRepository.ResearchMainProvider.GetByAutoResearchID(intResearchID);


            if (entResearchMain != null)
            {
                int intRespondentCount_Basic = clsSurveyHelper.RespondentCountBasic(intResearchID);
                int intRespondentCount_DMA = clsSurveyHelper.RespondentCountDMA(intResearchID);

                StringBuilder sb = new StringBuilder();

                sb.AppendLine("GMI Service Bureau request for survey programming");
                sb.AppendLine();
                sb.AppendFormat("Account Number:  {0}{1}{2}", clsPublic.GetProgramSetting("keyGMIAccount"), CrLf, CrLf);
                sb.AppendFormat("Study Title:  {0}{1}", entResearchMain.StrResearchSurveyName, CrLf);
                sb.AppendFormat("Target Quota:  {0}{1}", string.Empty, CrLf);
                sb.AppendFormat("Target Date:  {0}{1}{2}", string.Empty, DateTime.Now.AddDays(2), CrLf);
                sb.AppendLine("Gender: 50/50");
                sb.AppendLine("Age Categories: 18+ only, as per current USA demographic composition");
                sb.AppendLine("Income Level: As per current USA demographic composition");
                sb.AppendLine("Education: As per current USA demographic composition");
                sb.AppendLine();

                if (intRespondentCount_Basic > 0)
                    sb.AppendFormat("Respondent Count:  {0}{1}", intRespondentCount_Basic, CrLf);
                else if (intRespondentCount_DMA > 0)
                {
                    TList<ResearchRespondersDetail> tlstResearchRespondersDetail = DataRepository.ResearchRespondersDetailProvider.GetByIntResponderID(intResearchID );

                    sb.AppendFormat("Respondent DMA Count:  {0}{1}", intRespondentCount_DMA, CrLf);

                    tlstResearchRespondersDetail.ForEach(delegate(ResearchRespondersDetail entResearchRespondersDetail)
                    {
                        sb.AppendFormat("DMA:  {0}{1}", entResearchRespondersDetail.StrResponderDetail, CrLf);
                    });
                        
                }   

                sb.AppendLine("--------------------------------------------------");
                sb.AppendLine();
                sb.AppendLine("Scratchpad stuff:");
                sb.AppendLine();
                sb.AppendLine();

                // Introduction Question

                sb.AppendLine("[QUESTION]");
                sb.AppendLine("TYPE=M, \"Introduction\", @INTRO");

                string strIntroduction = clsPublic.ReplaceCRWithBR(entResearchMain.StrResearchIntroduction);

                char chr8217 = Convert.ToChar(8217);

                strIntroduction = strIntroduction.Replace(chr8217.ToString(), "&#8217;");


                sb.AppendLine(strIntroduction);

                TList<ResearchFiles> tlstResearchFiles = DataRepository.ResearchFilesProvider.GetByIntResearchFilesParentID(entResearchMain.AutoResearchID);

                if (tlstResearchFiles != null)
                {
                    if (tlstResearchFiles.Count > 0)
                    {
                        if (tlstResearchFiles[0].StrResearchFilesUniqueFileName.Length > 0)
                        {
                            sb.AppendLine();

                            if (tlstResearchFiles[0].StrResearchFilesType.Length == 0)
                                tlstResearchFiles[0].StrResearchFilesType = "VIDEO";

                            if (tlstResearchFiles[0].StrResearchFilesType.Substring(0, 5).ToUpper().Equals("IMAGE"))
                            {
                                sb.AppendFormat("<p/><img src=\"{0}images/Research/{1}\"><p/>", clsPublic.GetAbsoluteUrl(), tlstResearchFiles[0].StrResearchFilesUniqueFileName);
                            }
                            else
                            {
                                sb.AppendFormat("<iframe id=\"videos_list\" name=\"videos_list\" src=\"{0}Video/Research/videoView.aspx?Video={1}\" scrolling=\"auto\" width=\"640\" height=\"480\" frameborder=\"0\" marginheight=\"0\" marginwidth=\"0\"></iframe>", clsPublic.GetAbsoluteUrl(), tlstResearchFiles[0].StrResearchFilesUniqueFileName);
                            }

                            sb.AppendLine();
                        }
                    }
                }

                TList<ResearchQuestions> tlstResearchQuestions = DataRepository.ResearchQuestionsProvider.GetByIntQuesResearchID(entResearchMain.AutoResearchID);

                if (tlstResearchQuestions != null)
                {
                    tlstResearchQuestions.Sort(ResearchQuestionsColumn.IntQuesOrder.ToString());

                    tlstResearchQuestions.ForEach(delegate(ResearchQuestions entResearchQuestions)
                    {
                        string strQuesType = QuestionAbbreviation(entResearchQuestions.IntQuesType);

                        if (strQuesType.Length > 0)
                        {
                            sb.AppendLine();
                            sb.AppendLine("[QUESTION]");
                            sb.AppendFormat("TYPE={0}, \"Q{1}\", @Q{2}{3}", strQuesType, entResearchQuestions.AutoQuesID, entResearchQuestions.AutoQuesID, CrLf);
                            sb.AppendLine(entResearchQuestions.StrQuesText);

                            if (entResearchQuestions.IntQuesType == 2 || entResearchQuestions.IntQuesType == 4 || entResearchQuestions.IntQuesType == 5)
                            {
                                if (entResearchQuestions.IntQuesType != 2)
                                {
                                    TList<ResearchQuestionsResponses> tlstResearchQuestionsResponses = DataRepository.ResearchQuestionsResponsesProvider.GetByIntQuesRespQuesID(entResearchQuestions.AutoQuesID);

                                    if (tlstResearchQuestionsResponses != null)
                                    {
                                        sb.AppendLine("[RESPONSE]");
                                        tlstResearchQuestionsResponses.ForEach(delegate(ResearchQuestionsResponses entResearchQuestionsResponses)
                                        {
                                            sb.AppendLine(entResearchQuestionsResponses.StrQuesRespText);
                                        });
                                    }
                                }
                                else
                                {
                                    sb.AppendLine("[RESPONSE]");
                                    sb.AppendFormat("Yes{0}No{1}", CrLf, CrLf);
                                }
                            }
                            else if (entResearchQuestions.IntQuesType == 3)     // SRR
                            {
                                sb.AppendLine("[RESPONSE]");
                                sb.AppendLine("Strongly Agree");
                                sb.AppendLine("Somewhat Agree");
                                sb.AppendLine("Somewhat Disagree");
                                sb.AppendLine("Strongly Disagree");
                                sb.AppendLine("Neither Agree Nor Disagree");
                            }

                            ResearchQuestionsFiles entResearchQuestionsFiles = DataRepository.ResearchQuestionsFilesProvider.GetByIntResearchFilesQuestionID(entResearchQuestions.AutoQuesID);

                            if (entResearchQuestionsFiles != null)
                            {
                                if (entResearchQuestionsFiles.StrResearchFilesUniqueFileName.Length > 0)
                                {
                                    sb.AppendLine();

                                    if (entResearchQuestionsFiles.StrResearchFilesType.Length == 0)
                                        entResearchQuestionsFiles.StrResearchFilesType = "VIDEO";

                                    if (entResearchQuestionsFiles.StrResearchFilesType.Substring(0, 5).ToUpper().Equals("IMAGE"))
                                    {
                                        sb.AppendFormat("<p/><img src=\"{0}images/Research/{1}\"><p/>", clsPublic.GetAbsoluteUrl(), entResearchQuestionsFiles.StrResearchFilesUniqueFileName);
                                    }
                                    else
                                    {
                                        sb.AppendFormat("<p/><iframe id=\"videos_list\" name=\"videos_list\" src=\"{0}Video/Research/VideoView.aspx?Video={1}\" scrolling=\"auto\" width=\"640\" height=\"480\" frameborder=\"0\" marginheight=\"0\" marginwidth=\"0\"></iframe><p/>", clsPublic.GetAbsoluteUrl(), entResearchQuestionsFiles.StrResearchFilesUniqueFileName);
                                    }

                                    sb.AppendLine();
                                }
                            }

                        }
                    });
                }

                try
                {
                    string strSentTo = clsPublic.GetProgramSetting("keyGMISentToEmail");
                    string strFrom = clsPublic.GetProgramSetting("keyGMIFromEmail");
                    string strSubject = clsPublic.GetProgramSetting("keyGMISubject");
                    string strCC = clsPublic.GetProgramSetting("keyGMICC");
                    string strBody = clsPublic.GetProgramSetting("keyGMIBody");

                    MailMessage msg = new MailMessage(strFrom, strSentTo);

                    byte[] data = Encoding.ASCII.GetBytes(sb.ToString());

                    MemoryStream ms = new MemoryStream(data);

                    msg.Body = strBody;
                    msg.CC.Add(new MailAddress(strCC));


                    msg.Subject = strSubject;
                    msg.IsBodyHtml = false;

                    msg.Attachments.Add(new Attachment(ms, string.Format("Decision_Question_{0}.txt", entResearchMain.StrResearchSurveyName.Replace(" ", "_")), "text/plain"));

                    SmtpClient smtp = new SmtpClient();

                    smtp.Send(msg);

                    entResearchMain.DatResearchSubmitted = DateTime.Now;

                    DataRepository.ResearchMainProvider.Save(entResearchMain);
                }
                catch (Exception ex)
                {
                    clsPublic.LogError(ex);
                    return false;
                }

                return true;
            }
            else
                return false;
        }
        catch (Exception ex)
        {
            clsPublic.LogError(ex);

            return false;
        }
        
    }

    /// <summary>
    /// Gets the question type code for GMI
    /// </summary>
    /// <param name="intQuestionType">Question Type</param>
    /// <returns>Returns the GMI question type (System.String)</returns>
    protected string QuestionAbbreviation(Int16 intQuestionType)
    {
        switch (intQuestionType)
        {
            case 1:
                return "T";

            case 2:
                return "SRQ";

            case 3:
                return "SRR";

            case 4:
                return "SRQ";

            case 5:
                return "MR";

        }

        return string.Empty;
    }
}
