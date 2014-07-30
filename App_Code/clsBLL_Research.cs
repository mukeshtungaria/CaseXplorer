// ***********************************************************************
// Author           : Dennis Sebenick
// Created          : 08-29-2011
//
// Last Modified By : dennis
// Last Modified On : 08-29-2011
// ***********************************************************************
// <copyright file="clsBLL_Research.cs" company="DGCC.COM">
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
/// Business Objects Layer for Research
/// </summary>
[System.ComponentModel.DataObject]

public class clsBLL_Research
{
    /// <summary>
    /// Initializes a new instance of the <see cref="clsBLL_Research" /> class.
    /// </summary>
    public clsBLL_Research()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    /// <summary>
    /// Gets the research by list by User ID
    /// - Returns a list of research data
    /// </summary>
    /// <param name="UserID">Unique User ID.</param>
    /// <returns>TList{ResearchMain}.</returns>
    [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, true)]
    public TList<ResearchMain> GetResearchByID(string UserID)
    {
        if (UserID.Length > 0)
        {
            return DataRepository.ResearchMainProvider.GetByUserID(new Guid(UserID));
        }
        else
            return null;
    }

    /// <summary>
    /// Gets the research by ID and status
    /// </summary>
    /// <param name="UserID">The user ID.</param>
    /// <param name="intStatus">Research Status.</param>
    /// <param name="strSortField">Sort By.</param>
    /// <returns>TList{ResearchMain}.</returns>
    [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, true)]
    public TList<ResearchMain> GetResearchByID(string UserID, int intStatus, string strSortField)
    {
       if (UserID.Length > 0)
        {
            TList<ResearchMain> tlstResearchMain = DataRepository.ResearchMainProvider.GetByUserID(new Guid(UserID));

            switch (intStatus)
            {
                case 1:
                    tlstResearchMain.ApplyFilter(delegate(ResearchMain entResearchMain) { return entResearchMain.DatResearchSubmitted.ToString().Length > 0 && entResearchMain.DatResearchResultsReceived.ToString().Length == 0; });

                    break;

                case 2:
                    tlstResearchMain.ApplyFilter(delegate(ResearchMain entResearchMain) { return entResearchMain.DatResearchResultsReceived.ToString().Length > 0; });

                    break;
            }

            switch (strSortField.ToUpper())
            {
                case "DATRESEARCHCREATED":
                case "":
                    tlstResearchMain.Sort(string.Format("{0} desc", ResearchMainColumn.DatResearchCreated.ToString()));
                    break;

                case "DATRESEARCHCREATED DESC":
                    tlstResearchMain.Sort(string.Format("{0} asc", ResearchMainColumn.DatResearchCreated.ToString()));
                    break;

                case "DATRESEARCHSUBMITTED":
                    tlstResearchMain.Sort(string.Format("{0} desc", ResearchMainColumn.DatResearchSubmitted.ToString()));
                    break;

                case "DATRESEARCHSUBMITTED DESC":
                    tlstResearchMain.Sort(string.Format("{0} asc", ResearchMainColumn.DatResearchSubmitted.ToString()));
                    break;

                default:
                    tlstResearchMain.Sort(strSortField);
                    break;

            }

            return tlstResearchMain;
        }
        else
            return null;
    }

    /// <summary>
    /// Gets the all research 
    /// </summary>
    /// <param name="intStatus">Research Status.</param>
    /// <param name="strSortField">Sort By.</param>
    /// <returns>TList{ResearchMain}.</returns>
    [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, true)]
    public TList<ResearchMain> GetAll(int intStatus, string strSortField)
    {
        TList<ResearchMain> tlstResearchMain = DataRepository.ResearchMainProvider.GetAll();

        switch (intStatus)
        {
            case 1:
                tlstResearchMain.ApplyFilter(delegate(ResearchMain entResearchMain) { return entResearchMain.DatResearchSubmitted.ToString().Length > 0 && entResearchMain.DatResearchResultsReceived.ToString().Length == 0; });

                break;

            case 2:
                tlstResearchMain.ApplyFilter(delegate(ResearchMain entResearchMain) { return entResearchMain.DatResearchResultsReceived.ToString().Length > 0; });

                break;
        }

        switch (strSortField.ToUpper())
        {
            case "DATRESEARCHCREATED":
            case "":
                tlstResearchMain.Sort(string.Format("{0} desc", ResearchMainColumn.DatResearchCreated.ToString()));
                break;

            case "DATRESEARCHCREATED DESC":
                tlstResearchMain.Sort(string.Format("{0} asc", ResearchMainColumn.DatResearchCreated.ToString()));
                break;

            case "DATRESEARCHSUBMITTED":
                tlstResearchMain.Sort(string.Format("{0} desc", ResearchMainColumn.DatResearchSubmitted.ToString()));
                break;

            case "DATRESEARCHSUBMITTED DESC":
                tlstResearchMain.Sort(string.Format("{0} asc", ResearchMainColumn.DatResearchSubmitted.ToString()));
                break;

            default:
                tlstResearchMain.Sort(strSortField);
                break;

        }

        return tlstResearchMain;
    }

    /// <summary>
    /// Deletes the research by research ID.
    /// </summary>
    /// <param name="original_AutoResearchID">Research ID</param>
    /// <returns>Returns TRUE unless an error occurs</returns>
    [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, true)]
    public bool DeleteResearchByResearchID(int original_AutoResearchID)
    {
        try
        {
            DataRepository.ResearchMainProvider.Delete(original_AutoResearchID);
            return true;
        }
        catch (Exception ex)
        {
            clsPublic.LogError(ex);
            return false;
        }
        
    }




}
