// ***********************************************************************
// Author           : Dennis Sebenick
// Created          : 08-29-2011
//
// Last Modified By : dennis
// Last Modified On : 05-20-2012
// ***********************************************************************
// <copyright file="clsData.cs" company="DGCC.COM">
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
using JuryData.Entities;
using JuryData.Data;

/// <summary>
/// Provides public functions to retrieve data from database
/// </summary>
public class clsData
{
    /// <summary>
    /// Initializes a new instance of the <see cref="clsData" /> class.
    /// </summary>
    public clsData()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #region tblDropValuesCourt
    //
    // Default Court Drop Values
    //
    /// <summary>
    /// Retrieve Court related combobox / drop values
    /// </summary>
    /// <param name="intDropValueType">Drop Value Type.</param>
    /// <param name="intParentType">Parent Value (if any).</param>
    /// <param name="boolIncludeAll">Not used</param>
    /// <returns>TList{JuryData.Entities.DropValuesCourt}.</returns>
    public static TList<JuryData.Entities.DropValuesCourt> GetDropValuesCourtDataByType(Int16 intDropValueType, int intParentType, bool boolIncludeAll)
    {
        TList<JuryData.Entities.DropValuesCourt> lstDropValuesCourt = GetDropValuesCourtData();

        lstDropValuesCourt.RemoveFilter();

        string strFilter = string.Format("intDropValueType = {0} and intDropValueParentType = {1}", intDropValueType, intParentType);
        lstDropValuesCourt.Filter = strFilter;
        lstDropValuesCourt.ApplyFilter();

        lstDropValuesCourt.Sort(DropValuesCourtColumn.StrDropValueDescription.ToString());

        return lstDropValuesCourt;

    }

    //
    // Default Court Drop Values
    //
    /// <summary>
    /// Gets the type of the drop values court data by type and abbreviation
    /// </summary>
    /// <param name="intDropValueType">Drop Value Type.</param>
    /// <param name="strAbbreviation">The STR abbreviation.</param>
    /// <param name="boolIncludeAll">Not used</param>
    /// <returns>TList{JuryData.Entities.DropValuesCourt}.</returns>
    public static TList<JuryData.Entities.DropValuesCourt> GetDropValuesCourtDataByType(Int16 intDropValueType, string strAbbreviation, bool boolIncludeAll)
    {
        TList<JuryData.Entities.DropValuesCourt> lstDropValuesCourt = GetDropValuesCourtData();

        lstDropValuesCourt.RemoveFilter();

        string strFilter = string.Format("intDropValueType = {0} and strDropValueAbbreviation = {1}", intDropValueType, strAbbreviation);
        lstDropValuesCourt.Filter = strFilter;
        lstDropValuesCourt.ApplyFilter();

        lstDropValuesCourt.Sort(DropValuesCourtColumn.StrDropValueDescription.ToString());

        return lstDropValuesCourt;

    }

    /// <summary>
    /// Get Jury Data Drop Values by type
    /// </summary>
    /// <param name="intDropValueType">Drop Value Type</param>
    /// <returns>TList{JuryData.Entities.DropValuesJB}.</returns>
    public static TList<JuryData.Entities.DropValuesJB> GetDropValuesJBDataByType(Int16 intDropValueType)
    {
        TList<JuryData.Entities.DropValuesJB> lstDropValuesJB = GetDropValuesJBData();

        lstDropValuesJB.RemoveFilter();

        string strFilter = string.Format("intDropValueType = {0}", intDropValueType);
        lstDropValuesJB.Filter = strFilter;
        lstDropValuesJB.ApplyFilter();

        lstDropValuesJB.Sort(DropValuesJBColumn.StrDropValueDescription.ToString());

        return lstDropValuesJB;
    }


    /// <summary>
    /// Get Jury Data Drop Values by type and parent
    /// </summary>
    /// <param name="intDropValueType">Drop Value Type</param>
    /// <param name="intParentType">Parent Type</param>
    /// <param name="boolIncludeAll">Not used</param>
    /// <returns>TList{JuryData.Entities.DropValuesJB}.</returns>
    public static TList<JuryData.Entities.DropValuesJB> GetDropValuesJBDataByType(Int16 intDropValueType, int intParentType, bool boolIncludeAll)
    {
        TList<JuryData.Entities.DropValuesJB> lstDropValuesJB = GetDropValuesJBData();

        lstDropValuesJB.RemoveFilter();

        string strFilter = string.Format("intDropValueType = {0} and intDropValueParentType = {1}", intDropValueType, intParentType);
        lstDropValuesJB.Filter = strFilter;
        lstDropValuesJB.ApplyFilter();

        lstDropValuesJB.Sort(DropValuesJBColumn.StrDropValueDescription.ToString());

        return lstDropValuesJB;

    }


    /// <summary>
    /// Get app drop values by type
    /// </summary>
    /// <param name="intDropValueType">Drop Value Type</param>
    /// <param name="boolIncludeAll">Not used</param>
    /// <returns>TList{JuryData.Entities.DropValuesApp}.</returns>
    public static TList<JuryData.Entities.DropValuesApp> GetDropValuesAppDataByType(Int16 intDropValueType, bool boolIncludeAll)
    {

        TList<JuryData.Entities.DropValuesApp> lstDropValuesApp = GetDropValuesAppData();

        lstDropValuesApp.RemoveFilter();

        string strFilter = string.Format("intDropValueType = {0}", intDropValueType);
        lstDropValuesApp.Filter = strFilter;
        lstDropValuesApp.ApplyFilter();
        lstDropValuesApp.Sort(DropValuesCourtColumn.StrDropValueDescription.ToString());
        return lstDropValuesApp;
    }




    /// <summary>
    /// Get court drop values by type
    /// </summary>
    /// <param name="intDropValueType">Drop Value Type</param>
    /// <param name="boolIncludeAll">Not used</param>
    /// <returns>TList{JuryData.Entities.DropValuesCourt}.</returns>
    public static TList<JuryData.Entities.DropValuesCourt> GetDropValuesCourtDataByType(Int16 intDropValueType, bool boolIncludeAll)
    {

        TList<JuryData.Entities.DropValuesCourt> lstDropValuesCourt = GetDropValuesCourtData();

        lstDropValuesCourt.RemoveFilter();

        string strFilter = string.Format("intDropValueType = {0}", intDropValueType);
        lstDropValuesCourt.Filter = strFilter;
        lstDropValuesCourt.ApplyFilter();
        lstDropValuesCourt.Sort(DropValuesCourtColumn.StrDropValueDescription.ToString());
        return lstDropValuesCourt;
    }



    /// <summary>
    /// Get all app drop values 
    /// </summary>
    /// <returns>TList{JuryData.Entities.DropValuesApp}.</returns>
    public static TList<JuryData.Entities.DropValuesApp> GetDropValuesAppData()
    {
        TList<JuryData.Entities.DropValuesApp> lstDropValuesApp = null;

        if (lstDropValuesApp == null)
        {
            lstDropValuesApp = DataRepository.DropValuesAppProvider.GetAll();
        }

        return lstDropValuesApp;
    }

    /// <summary>
    /// Get all court drop values
    /// </summary>
    /// <returns>TList{JuryData.Entities.DropValuesCourt}.</returns>
    public static TList<JuryData.Entities.DropValuesCourt> GetDropValuesCourtData()
    {
        TList<JuryData.Entities.DropValuesCourt> lstDropValuesCourt = null;

        //        lstDropValuesCourt = EntityCache.GetItem<TList<JuryData.Entities.DropValuesCourt>>("keyDropValueCourt");

        if (lstDropValuesCourt == null)
        {
            lstDropValuesCourt = DataRepository.DropValuesCourtProvider.GetAll();
            //            EntityCache.AddCache("keyDropValueCourt", lstDropValuesCourt);
        }

        return lstDropValuesCourt;
    }

    /// <summary>
    /// Gets Court Drop Value Description By ID
    /// </summary>
    /// <param name="intTypeID">Drop Value ID</param>
    /// <returns>Court Drop Value Description (System.String).</returns>
    public static string GetDesc_DropValuesCourtByType(int intTypeID)
    {
        DropValuesCourt entDropValuesCourt = DataRepository.DropValuesCourtProvider.GetByAutoDropValueID(intTypeID);

        if (entDropValuesCourt != null)
        {
            return entDropValuesCourt.StrDropValueDescription;
        }
        else
            return string.Empty;


    }

    /// <summary>
    /// Gets Jury Data Value Description by ID
    /// </summary>
    /// <param name="intTypeID">Drop Value ID</param>
    /// <returns>Jury Data Drop Value Description (System.String).</returns>
    public static string GetDesc_DropValueJBByType(int intTypeID)
    {
        DropValuesJB entDropValuesJB = DataRepository.DropValuesJBProvider.GetByAutoDropValueID(intTypeID);

        if (entDropValuesJB != null)
        {
            return entDropValuesJB.StrDropValueDescription;
        }
        else
            return string.Empty;

    }

    /// <summary>
    /// Get all Jury Data drop values
    /// </summary>
    /// <returns>TList{JuryData.Entities.DropValuesJB}.</returns>
    public static TList<JuryData.Entities.DropValuesJB> GetDropValuesJBData()
    {
        TList<JuryData.Entities.DropValuesJB> lstDropValuesJB = null;

        if (lstDropValuesJB == null)
        {
            lstDropValuesJB = DataRepository.DropValuesJBProvider.GetAll();
        }

        return lstDropValuesJB;
    }

    /// <summary>
    /// Gets the default ID for Jury Data drop values
    /// </summary>
    /// <param name="intType">Drop Value Type</param>
    /// <param name="intDefaultValue">Which default value (1st, 2nd, 3rd, etc)</param>
    /// <returns>Jury Data ID (int32)</returns>
    public static Int32 GetDropValuesJBDefaultValue(int intType, int intDefaultValue)
    {
        int intReturnValue;

        TList<JuryData.Entities.DropValuesJB> lstDropValuesJB = GetDropValuesJBData();

        if (lstDropValuesJB != null)
        {
            string strFilter = string.Format("intDropValueType = {0} and intDropValueDefault = {1}", intType, intDefaultValue);
            lstDropValuesJB.RemoveFilter();
            lstDropValuesJB.Filter = strFilter;
            lstDropValuesJB.ApplyFilter();

            if (lstDropValuesJB.Count > 0)
                intReturnValue = lstDropValuesJB[0].AutoDropValueID;
            else
                intReturnValue = -1;

            lstDropValuesJB.RemoveFilter();
        }
        else
            intReturnValue = -1;

        return intReturnValue;
    }

    /// <summary>
    /// Gets the Parent ID for Jury Data drop values
    /// </summary>
    /// <param name="intType">Drop Value Type</param>
    /// <param name="intDefaultValue">Which default value (1st, 2nd, 3rd, etc)</param>
    /// <returns>Parent ID (int32)</returns>
    public static Int32 GetDropValuesJBDefaultParentValue(int intType, int intDefaultValue)
    {
        int intReturnValue;

        TList<JuryData.Entities.DropValuesJB> lstDropValuesJB = GetDropValuesJBData();

        if (lstDropValuesJB != null)
        {
            string strFilter = string.Format("intDropValueParentType = {0} and intDropValueDefault = {1}", intType, intDefaultValue);
            lstDropValuesJB.RemoveFilter();
            lstDropValuesJB.Filter = strFilter;
            lstDropValuesJB.ApplyFilter();

            if (lstDropValuesJB.Count > 0)
                intReturnValue = lstDropValuesJB[0].AutoDropValueID;
            else
                intReturnValue = -1;

            lstDropValuesJB.RemoveFilter();
        }
        else
            intReturnValue = -1;

        return intReturnValue;
    }


    /// <summary>
    /// Get description text of a Jury Data drop value by ID
    /// </summary>
    /// <param name="intDropValue">ID of drop value</param>
    /// <returns>Jury Data drop value description (System.String)</returns>
    public static string GetDropValuesJBDesc(int intDropValue)
    {
        DropValuesJB entDropValuesJB = DataRepository.DropValuesJBProvider.GetByAutoDropValueID(intDropValue);

        if (entDropValuesJB != null)
            return entDropValuesJB.StrDropValueDescription;
        else
            return string.Empty;
    }

    /// <summary>
    /// Get Jury Data drop value object
    /// </summary>
    /// <param name="intType">Drop value type</param>
    /// <param name="intDefaultValue">Which default value (1st, 2nd, 3rd).</param>
    /// <param name="intDropValue">Not used</param>
    /// <returns>DropValuesJB.</returns>
    public static DropValuesJB GetDropValuesJB(int intType, int intDefaultValue, int intDropValue)
    {
        DropValuesJB entDropValuesJB;

        TList<JuryData.Entities.DropValuesJB> lstDropValuesJB = GetDropValuesJBData();

        if (lstDropValuesJB != null)
        {
            string strFilter = string.Format("intDropValueType = {0} and intDropValueDefault = {1}", intType, intDefaultValue);
            lstDropValuesJB.RemoveFilter();
            lstDropValuesJB.Filter = strFilter;
            lstDropValuesJB.ApplyFilter();

            if (lstDropValuesJB.Count > 0)
                entDropValuesJB = lstDropValuesJB[0];
            else
                entDropValuesJB = new DropValuesJB();

            lstDropValuesJB.RemoveFilter();
        }
        else
            entDropValuesJB = new DropValuesJB();

        return entDropValuesJB;
    }

    /// <summary>
    /// Get Court Drop Value object by type and default
    /// </summary>
    /// <param name="intType">Drop Value Type.</param>
    /// <param name="intDefaultValue">Default value.</param>
    /// <param name="intDropValue">Not used</param>
    /// <returns>DropValuesCourt.</returns>
    public static DropValuesCourt GetDropValuesCourt(int intType, int intDefaultValue, int intDropValue)
    {
        DropValuesCourt entDropValuesCourt;

        TList<JuryData.Entities.DropValuesCourt> lstDropValuesCourt = GetDropValuesCourtData();

        if (lstDropValuesCourt != null)
        {
            string strFilter = string.Format("intDropValueType = {0} and intDropValueDefault = {1}", intType, intDefaultValue);
            lstDropValuesCourt.RemoveFilter();
            lstDropValuesCourt.Filter = strFilter;
            lstDropValuesCourt.ApplyFilter();

            if (lstDropValuesCourt.Count > 0)
                entDropValuesCourt = lstDropValuesCourt[0];
            else
                entDropValuesCourt = new DropValuesCourt();

            lstDropValuesCourt.RemoveFilter();
        }
        else
            entDropValuesCourt = new DropValuesCourt();

        return entDropValuesCourt;
    }

    /// <summary>
    /// Gets the court default desc.
    /// </summary>
    /// <param name="intType">Drop Value Type</param>
    /// <param name="intDefaultValue">Drop Value  Default (1st, 2nd, 3rd, etc)</param>
    /// <returns>Court Description (System.String)</returns>
    public static string GetCourtDefaultDesc(int intType, int intDefaultValue)
    {
        string strReturnValue;

        TList<JuryData.Entities.DropValuesCourt> lstDropValuesCourt = GetDropValuesCourtData();

        if (lstDropValuesCourt != null)
        {
            string strFilter = string.Format("intDropValueType = {0} and intDropValueDefault = {1}", intType, intDefaultValue);
            lstDropValuesCourt.RemoveFilter();
            lstDropValuesCourt.Filter = strFilter;
            lstDropValuesCourt.ApplyFilter();

            if (lstDropValuesCourt.Count > 0)
                strReturnValue = lstDropValuesCourt[0].StrDropValueDescription;
            else
                strReturnValue = "";

            lstDropValuesCourt.RemoveFilter();
        }
        else
            strReturnValue = "";

        return strReturnValue;
    }

    /// <summary>
    /// Gets the court default value.
    /// </summary>
    /// <param name="intType">Drop Value Type</param>
    /// <param name="intDefaultValue">Drop Value  Default (1st, 2nd, 3rd, etc)</param>
    /// <returns>Default Value (Int32)</returns>
    public static Int32 GetCourtDefaultValue(int intType, int intDefaultValue)
    {
        int intReturnValue;

        TList<JuryData.Entities.DropValuesCourt> lstDropValuesCourt = GetDropValuesCourtData();

        if (lstDropValuesCourt != null)
        {
            string strFilter = string.Format("intDropValueType = {0} and intDropValueDefault = {1}", intType, intDefaultValue);
            lstDropValuesCourt.RemoveFilter();
            lstDropValuesCourt.Filter = strFilter;
            lstDropValuesCourt.ApplyFilter();

            if (lstDropValuesCourt.Count > 0)
                intReturnValue = lstDropValuesCourt[0].AutoDropValueID;
            else
                intReturnValue = -1;

            lstDropValuesCourt.RemoveFilter();
        }
        else
            intReturnValue = -1;

        return intReturnValue;
    }

    #endregion


    #region tblCatalog

    //
    // Default Catalog
    //

    /// <summary>
    /// Gets the catalog data.
    /// </summary>
    /// <returns>TList{JuryData.Entities.Catalog}.</returns>
    public static TList<JuryData.Entities.Catalog> GetCatalogData()
    {
        TList<JuryData.Entities.Catalog> lstCatalog;

        lstCatalog = EntityCache.GetItem<TList<JuryData.Entities.Catalog>>("keyCatalog");

        if (lstCatalog == null)
        {
            lstCatalog = DataRepository.CatalogProvider.GetAll();
            EntityCache.AddCache("keyCatalog", lstCatalog);
        }

        return lstCatalog;
    }

    /// <summary>
    /// Enum Sort_Catalog
    /// </summary>
    public enum Sort_Catalog { Desc = 0, Abbr = 1, Seq = 2 };


    /// <summary>
    /// Gets the type of the catalog data by Type and parent
    /// </summary>
    /// <param name="intCatGroupID">Catalog type ID.</param>
    /// <param name="intParentID">Parent ID.</param>
    /// <param name="boolIncludeAll">Not Used.</param>
    /// <returns>TList{JuryData.Entities.Catalog}.</returns>
    public static TList<JuryData.Entities.Catalog> GetCataLogDataByType(Int16 intCatGroupID, Int16 intParentID, bool boolIncludeAll)
    {
        TList<JuryData.Entities.Catalog> lstCatalogData = GetCatalogData();

        lstCatalogData.RemoveFilter();

        string strFilter = string.Format("intCatGroupID = {0} and intParentID = {1} ", intCatGroupID, intParentID);

        lstCatalogData.Filter = strFilter;
        lstCatalogData.ApplyFilter();

        return lstCatalogData;
    }

    /// <summary>
    /// Gets the type of the catalog data by type and parent (with sort option)
    /// </summary>
    /// <param name="intCatGroupID">Catalog type ID.</param>
    /// <param name="intParentID">Parent ID</param>
    /// <param name="boolIncludeAll">Not Used.</param>
    /// <param name="eSort">Sort by</param>
    /// <returns>TList{JuryData.Entities.Catalog}.</returns>
    public static TList<JuryData.Entities.Catalog> GetCataLogDataByType(Int16 intCatGroupID, Int16 intParentID, bool boolIncludeAll, Sort_Catalog eSort)
    {
        TList<JuryData.Entities.Catalog> lstCatalogData = GetCatalogData();

        lstCatalogData.RemoveFilter();

        string strFilter = string.Format("intCatGroupID = {0} and intParentID = {1} ", intCatGroupID, intParentID);

        lstCatalogData.Filter = strFilter;
        lstCatalogData.ApplyFilter();

        if (eSort == Sort_Catalog.Abbr)
            lstCatalogData.Sort(CatalogColumn.StrAbbreviation.ToString());
        else if (eSort == Sort_Catalog.Desc)
            lstCatalogData.Sort(CatalogColumn.StrDesc.ToString());
        else if (eSort == Sort_Catalog.Seq)
            lstCatalogData.Sort(CatalogColumn.IntSequence.ToString());


        return lstCatalogData;
    }

    /// <summary>
    /// Get Catalog data by type, parent, with a sort option
    /// </summary>
    /// <param name="intCatGroupID">Catalog Type ID</param>
    /// <param name="strParentValue">Parent ID</param>
    /// <param name="boolIncludeAll">Not User</param>
    /// <param name="eSort">Sort By</param>
    /// <returns>TList{JuryData.Entities.Catalog}.</returns>
    public static TList<JuryData.Entities.Catalog> GetCataLogDataByType(Int16 intCatGroupID, string strParentValue, bool boolIncludeAll, Sort_Catalog eSort)
    {
        TList<JuryData.Entities.Catalog> lstCatalogData = GetCatalogData();

        lstCatalogData.RemoveFilter();

        string strFilter = string.Format("intCatGroupID = {0} and strDesc = '{1}' ", intCatGroupID, strParentValue.Replace("'", "''"));

        lstCatalogData.Filter = strFilter;
        lstCatalogData.ApplyFilter();

        if (eSort == Sort_Catalog.Abbr)
            lstCatalogData.Sort(CatalogColumn.StrAbbreviation.ToString());
        else if (eSort == Sort_Catalog.Desc)
            lstCatalogData.Sort(CatalogColumn.StrDesc.ToString());
        else if (eSort == Sort_Catalog.Seq)
            lstCatalogData.Sort(CatalogColumn.IntSequence.ToString());


        return lstCatalogData;
    }

    /// <summary>
    /// Gets catalog data by type, with sort option
    /// </summary>
    /// <param name="intCatGroupID">Catalog Type ID</param>
    /// <param name="boolIncludeAll">Not User</param>
    /// <param name="eSort">Sort By</param>
    /// <returns>TList{JuryData.Entities.Catalog}.</returns>
    public static TList<JuryData.Entities.Catalog> GetCataLogDataByType(Int16 intCatGroupID, bool boolIncludeAll, Sort_Catalog eSort)
    {
        TList<JuryData.Entities.Catalog> lstCatalogData = GetCatalogData();

        lstCatalogData.RemoveFilter();

        string strFilter = string.Format("intCatGroupID = {0}", intCatGroupID);

        lstCatalogData.Filter = strFilter;
        lstCatalogData.ApplyFilter();

        if (eSort == Sort_Catalog.Abbr)
            lstCatalogData.Sort(CatalogColumn.StrAbbreviation.ToString());
        else if (eSort == Sort_Catalog.Desc)
            lstCatalogData.Sort(CatalogColumn.StrDesc.ToString());
        else if (eSort == Sort_Catalog.Seq)
            lstCatalogData.Sort(CatalogColumn.IntSequence.ToString());


        return lstCatalogData;
    }


    /// <summary>
    /// Get catalog data by type
    /// </summary>
    /// <param name="intCatGroupID">Catalog Type ID</param>
    /// <param name="boolIncludeAll">Not Used</param>
    /// <returns>TList{JuryData.Entities.Catalog}.</returns>
    public static TList<JuryData.Entities.Catalog> GetCataLogDataByType(Int16 intCatGroupID, bool boolIncludeAll)
    {
        TList<JuryData.Entities.Catalog> lstCatalogData = GetCatalogData();

        lstCatalogData.RemoveFilter();

        string strFilter = string.Format("intCatGroupID = {0}", intCatGroupID);

        lstCatalogData.Filter = strFilter;
        lstCatalogData.ApplyFilter();

        return lstCatalogData;
    }

    #endregion

}
