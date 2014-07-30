// ***********************************************************************
// Author           : Dennis Sebenick
// Created          : 08-29-2011
//
// Last Modified By : dennis
// Last Modified On : 08-29-2011
// ***********************************************************************
// <copyright file="AutoComplete.cs" company="DGCC.COM">
//     . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Collections;
using System.Web.Services;
using JuryData.Data;
using JuryData.Entities;

/// <summary>
/// Web service to retrieve autocomplete text 
/// -- City by Zip
/// -- Remove Zip Code from Study
/// 
/// </summary>
[WebService(Namespace = "https://www.casexplorer.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class AutoComplete : WebService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AutoComplete" /> class.
    /// </summary>
    public AutoComplete()
    {
    }

    /// <summary>
    /// Gets the city by zip code
    /// </summary>
    /// <param name="strZipCode">Zip Code</param>
    /// <returns>City (System.String)</returns>
    [WebMethod]
    public string GetCity(string strZipCode)
    {
        ZipCodeLookup entZipCodeLookup = DataRepository.ZipCodeLookupProvider.GetByStrZIPCode(strZipCode);

        if (entZipCodeLookup != null)
            return string.Format("{0}, {1} ({2})", entZipCodeLookup.StrZipCityPrimary, entZipCodeLookup.StrZipState, entZipCodeLookup.StrZipCounty);
        else
            return "City Not Found.";
    }

    /// <summary>
    /// Removes a zip code from a research study by Research ID
    /// </summary>
    /// <param name="strZipCode">Zip Code.</param>
    /// <param name="intResearchID">Research ID</param>
    /// <returns>Return not used - returns 0 always (System.Int32).</returns>
    [WebMethod]
    public int DeleteZipCode(string strZipCode, int intResearchID)
    {
        try
        {
            ResearchRespondersDetailZipCode entResearchRespondersDetailZipCode;

            entResearchRespondersDetailZipCode = DataRepository.ResearchRespondersDetailZipCodeProvider.GetByIntResponderIDStrResponderZipCode(intResearchID, strZipCode);

            if (entResearchRespondersDetailZipCode != null)
            {
                DataRepository.ResearchRespondersDetailZipCodeProvider.Delete(entResearchRespondersDetailZipCode);
            }

            return 0;
        }
        catch (Exception ex)
        {
            clsPublic.LogError(ex);
            return 0;
        }
    }


    /// <summary>
    /// Adds the zip code county to a study
    /// </summary>
    /// <param name="strZipCode">Zip Code</param>
    /// <param name="intResearchID">Research ID</param>
    /// <returns>Returns NULL (System.String[][])</returns>
    [WebMethod]
    public string[] AddZipCodeCounty(string strZipCode, int intResearchID)
    {

        try
        {
            ZipCodeLookup entZipCodeLookup = DataRepository.ZipCodeLookupProvider.GetByStrZIPCode(strZipCode);

            if (entZipCodeLookup != null)
            {
                TList<ZipCodeLookup> tlstZipCodeLookup = DataRepository.ZipCodeLookupProvider.GetByIntZipCountyFIPS(entZipCodeLookup.IntZipCountyFIPS);

                if (tlstZipCodeLookup != null)
                {
                    ArrayList aryList = new ArrayList();
                    tlstZipCodeLookup.ForEach(delegate(ZipCodeLookup entZipCodeLookup2)
                    {
                        if (this.AddZipCode(entZipCodeLookup2.StrZIPCode, intResearchID).Length > 0)
                            aryList.Add(string.Format("{0} - {1}, {2}", entZipCodeLookup2.StrZIPCode, entZipCodeLookup2.StrZipCityPrimary, entZipCodeLookup2.StrZipState));
                    });

                    return (string[])aryList.ToArray(typeof(string));
                }
                else
                    return null;
            }
            else
                return null;

        }
        catch (Exception ex)
        {
            clsPublic.LogError(ex);
            return null;
        }

    }


    /// <summary>
    /// Adds a zip code and then all zip codes for it's county for the study
    /// </summary>
    /// <param name="strZipCode">Zip Code</param>
    /// <param name="intResearchID">Research ID</param>
    /// <returns>Returns all zip codes for the county (System.String[][])</returns>
    [WebMethod]
    public string[] AddZipCodeCity(string strZipCode, int intResearchID)
    {

        try
        {
            ZipCodeLookup entZipCodeLookup = DataRepository.ZipCodeLookupProvider.GetByStrZIPCode(strZipCode);

            if (entZipCodeLookup != null)
            {
                TList<ZipCodeLookup> tlstZipCodeLookup = DataRepository.ZipCodeLookupProvider.GetByStrZipCityPrimaryStrZipState(entZipCodeLookup.StrZipCityPrimary, entZipCodeLookup.StrZipState);

                if (tlstZipCodeLookup != null)
                {
                    ArrayList aryList = new ArrayList();
                    tlstZipCodeLookup.ForEach(delegate(ZipCodeLookup entZipCodeLookup2)
                    {
                        if (this.AddZipCode(entZipCodeLookup2.StrZIPCode, intResearchID).Length >0)
                            aryList.Add(string.Format("{0} - {1}, {2}", entZipCodeLookup2.StrZIPCode, entZipCodeLookup2.StrZipCityPrimary, entZipCodeLookup2.StrZipState));
                    });

                    return (string[])aryList.ToArray(typeof(string));
                }
                else
                    return null;
            }
            else
                return null;

        }
        catch (Exception ex)
        {
            clsPublic.LogError(ex);
            return null;
        }

    }

    /// <summary>
    /// Adds a zip code to a study
    /// </summary>
    /// <param name="strZipCode">Zip Code</param>
    /// <param name="intResearchID">Research ID</param>
    /// <returns>Returns empty string (System.String)</returns>
    [WebMethod]
    public string AddZipCode(string strZipCode, int intResearchID)
    {
        try
        {
            ZipCodeLookup entZipCodeLookup = DataRepository.ZipCodeLookupProvider.GetByStrZIPCode(strZipCode);

            if (entZipCodeLookup != null)
            {

                ResearchRespondersDetailZipCode entResearchRespondersDetailZipCode;

                entResearchRespondersDetailZipCode = DataRepository.ResearchRespondersDetailZipCodeProvider.GetByIntResponderIDStrResponderZipCode(intResearchID, strZipCode);

                if (entResearchRespondersDetailZipCode == null)
                {

                    entResearchRespondersDetailZipCode = new ResearchRespondersDetailZipCode();

                    entResearchRespondersDetailZipCode.IntResponderID = intResearchID;
                    entResearchRespondersDetailZipCode.StrResponderZipCode = strZipCode;
                    entResearchRespondersDetailZipCode.StrResponderCity = entZipCodeLookup.StrZipCityPrimary;
                    entResearchRespondersDetailZipCode.StrResponderState = entZipCodeLookup.StrZipState;

                    DataRepository.ResearchRespondersDetailZipCodeProvider.Save(entResearchRespondersDetailZipCode);

                    return string.Format("{0} - {1}, {2}", strZipCode, entZipCodeLookup.StrZipCityPrimary, entZipCodeLookup.StrZipState);
                }
                else
                    return "";
            }
            else
                return "";
        }
        catch (Exception ex)
        {
            clsPublic.LogError(ex);
            return "";
        }
    }

}