// ***********************************************************************
// Author           : Dennis Sebenick
// Created          : 08-29-2011
//
// Last Modified By : dennis
// Last Modified On : 11-01-2011
// ***********************************************************************
// <copyright file="clsSurveyHelper.cs" company="DGCC.COM">
//     . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using JuryData.Data;
using JuryData.Entities;

/// <summary>
/// Summary description for clsSurveyHelper
/// </summary>
public class clsSurveyHelper
{
    /// <summary>
    /// Initializes a new instance of the <see cref="clsSurveyHelper" /> class.
    /// </summary>
	public clsSurveyHelper()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    /// <summary>
    /// Gets or sets the current research ID.
    /// </summary>
    /// <value>The current research ID.</value>
    public static int CurrentResearchID
    {
        get
        {
            if (HttpContext.Current.Session["ResearchID"] != null)
            {
                int intResearchID = 0;

                int.TryParse(HttpContext.Current.Session["ResearchID"].ToString(), out intResearchID);

                return intResearchID;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            HttpContext.Current.Session["ResearchID"] = value;
        }
    }



    /// <summary>
    /// Determines if a study by research ID has any questions
    /// </summary>
    /// <param name="intResearchID">Research ID.</param>
    /// <returns>Number of questions in study (System.Int32)</returns>
    public static int CheckHasQuestions(int intResearchID)
    {
        TList<ResearchQuestions> tlstResearchQuestions = DataRepository.ResearchQuestionsProvider.GetByIntQuesResearchID(intResearchID);

        if (tlstResearchQuestions != null)
        {
            return tlstResearchQuestions.Count;
        }
        else
            return 0;
    }

    /// <summary>
    /// Determines whether [is survey submitted] [the specified int research ID].
    /// </summary>
    /// <param name="intResearchID">Research ID.</param>
    /// <returns><c>true</c> if [is survey submitted] [the specified int research ID]; otherwise, <c>false</c>.</returns>
    public static bool IsSurveySubmitted(int intResearchID)
    {
        ResearchMain entResearchMain = DataRepository.ResearchMainProvider.GetByAutoResearchID(intResearchID);

        if (entResearchMain != null)
        {
            if (entResearchMain.DatResearchSubmitted != null)
                return true;
            else
                return false;

        }
        else
            return false;
    }

    /// <summary>
    /// Gets study respondent counts (Nationwide)
    /// </summary>
    /// <param name="intResearchID">Research ID.</param>
    /// <returns>Respondent Count (System.Int32)</returns>
    public static int RespondentCountBasic(int intResearchID)
    {
        ResearchResponders entResearchResponders = DataRepository.ResearchRespondersProvider.GetByIntRespondersResearchIDIntRespondersType(intResearchID, 3);

        if (entResearchResponders != null)
        {
            return entResearchResponders.IntRespondersCount;
        }
        else
            return 0;


    }

    /// <summary>
    /// Get Study Respondent Counts (DMA)
    /// </summary>
    /// <param name="intResearchID">Research ID.</param>
    /// <returns>Respondent Count (System.Int32)</returns>
    public static int RespondentCountDMA(int intResearchID)
    {
        ResearchResponders entResearchResponders = DataRepository.ResearchRespondersProvider.GetByIntRespondersResearchIDIntRespondersType(intResearchID, 4);

        if (entResearchResponders != null)
        {
            return entResearchResponders.IntRespondersCount;
        }
        else
            return 0;

    }

    /// <summary>
    /// Get Study Respondent Counts (Zip Code)
    /// </summary>
    /// <param name="intResearchID">Research ID.</param>
    /// <returns>Respondent Count (System.Int32)</returns>
    public static int RespondentCountZipCode(int intResearchID)
    {
        ResearchResponders entResearchResponders = DataRepository.ResearchRespondersProvider.GetByIntRespondersResearchIDIntRespondersType(intResearchID, 7);

        if (entResearchResponders != null)
        {
            return entResearchResponders.IntRespondersCount;
        }
        else
            return 0;

    }


    /// <summary>
    /// Get Study Respondent Counts (County)
    /// </summary>
    /// <param name="intResearchID">Research ID.</param>
    /// <returns>Respondent Count (System.Int32)</returns>
    public static int RespondentCountCounty(int intResearchID)
    {
        ResearchResponders entResearchResponders = DataRepository.ResearchRespondersProvider.GetByIntRespondersResearchIDIntRespondersType(intResearchID, 8);

        if (entResearchResponders != null)
        {
            return entResearchResponders.IntRespondersCount;
        }
        else
            return 0;

    }
    
    /// <summary>
    /// Checks that a study has respondents
    /// - Not used
    /// </summary>
    /// <param name="intResearchID">Research ID.</param>
    /// <returns>System.Int32.</returns>
    public static int CheckHasRespondents(int intResearchID)
    {

        return 0;
    }

    /// <summary>
    /// Sets up tab
    /// </summary>
    /// <param name="tabStrip">The tab strip.</param>
    /// <param name="intResearchID">Research ID.</param>
    public static void TabSetup(Telerik.Web.UI.RadTabStrip tabStrip, int intResearchID)
    {
        if (tabStrip != null)
        {
            tabStrip.Tabs[2].Enabled = true;
            tabStrip.Tabs[3].Enabled = true;
            tabStrip.Tabs[4].Enabled = true;
            
            if (clsSurveyHelper.CheckHasQuestions(intResearchID) == 0)
            {
                tabStrip.Tabs[2].Enabled = false;
                tabStrip.Tabs[3].Enabled = false;
                tabStrip.Tabs[4].Enabled = false;
            }
            

        }
    }
}
