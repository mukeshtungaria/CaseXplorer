// ***********************************************************************
// Author           : Dennis Sebenick
// Created          : 08-29-2011
//
// Last Modified By : dennis
// Last Modified On : 08-29-2011
// ***********************************************************************
// <copyright file="DropList.cs" company="DGCC.COM">
//     . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Web;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using AjaxControlToolkit;
using JuryData.Entities;
using JuryData.Data;


/// <summary>
/// Drop List Service Functions
/// </summary>
[WebService(Namespace = "https://www.casexplorer.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class DropList : System.Web.Services.WebService
{

    /// <summary>
    /// Initializes a new instance of the <see cref="DropList" /> class.
    /// </summary>
    public DropList()
    {
    }

    /// <summary>
    /// Get a city by zip code
    /// </summary>
    /// <param name="strZipCode">Zip code.</param>
    /// <returns>Returns City (System.String)</returns>
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
    /// Gets the drop down contents.
    /// </summary>
    /// <param name="knownCategoryValues">The known category values.</param>
    /// <param name="category">The category.</param>
    /// <returns>AjaxControlToolkit.CascadingDropDownNameValue[][].</returns>
    [WebMethod]
    public AjaxControlToolkit.CascadingDropDownNameValue[] GetDropDownContents(string knownCategoryValues, string category)
    {
        StringDictionary kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);

        List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();

        Int16 intDropValueType = 0;
        Int16 intDropValueParent = 0;


        if (kv.ContainsKey("CASETYPE"))
            Int16.TryParse (kv ["CaseType"], out intDropValueParent);
        
        if (kv.ContainsKey("CASECATEGORY"))
            Int16.TryParse(kv["CaseCategory"], out intDropValueParent);

        switch (category.ToUpper())
        {
            case "CASETYPE":
                intDropValueType = 2;
                break;

            case "CASECATEGORY":
                intDropValueType = 3;
                break;

            case "CASEDETAIL":
                intDropValueType = 4;
                break;

        }

        TList<DropValuesJB> tlstDropValuesJB = null;

        if (intDropValueParent == 0)
            tlstDropValuesJB = DataRepository.DropValuesJBProvider.GetByIntDropValueType(intDropValueType);
        else
            tlstDropValuesJB = DataRepository.DropValuesJBProvider.GetByIntDropValueTypeIntDropValueParentType(intDropValueType, intDropValueParent );

        tlstDropValuesJB.ForEach(delegate(DropValuesJB entDropValuesJB)
        {
            values.Add(new CascadingDropDownNameValue(entDropValuesJB.StrDropValueDescription, entDropValuesJB.AutoDropValueID.ToString()));
        }
        );

        return values.ToArray();
    }


}

