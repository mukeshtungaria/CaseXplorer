// ***********************************************************************
// Assembly         : C:\juryresearch\
// Author           : Dennis Sebenick
// Created          : 08-29-2011
//
// Last Modified By : dennis
// Last Modified On : 08-06-2012
// ***********************************************************************
// <copyright file="clsBLL_Courts.cs" company="DGCC.COM">
//     . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Web;
using JuryData.Data;
using JuryData.Entities;

/// <summary>
/// Business Objects Layer for Research
/// </summary>
[System.ComponentModel.DataObject]
public class clsBLL_Courts
{
    /// <summary>
    /// Initializes a new instance of the <see cref="clsBLL_Courts" /> class.
    /// </summary>
	public clsBLL_Courts()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    /// <summary>
    /// Gets the court data.
    /// </summary>
    /// <param name="intRecordID">Court ID</param>
    /// <param name="intType">Court Type</param>
    /// <param name="strSortField">Sort Field</param>
    /// <returns>TList{Courts}.</returns>
    [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, true)]
    public TList<Courts> GetCourtData(int intRecordID, int intType, string strSortField)
    {
        TList<Courts> tlstCourts;

        if (intType == 1)
            tlstCourts = DataRepository.CourtsProvider.GetByIntCourtDistrictID(intRecordID);
        else
        {
            tlstCourts = DataRepository.CourtsProvider.GetByIntCourtCountyFIPS(intRecordID);
            tlstCourts.ApplyFilter(delegate(Courts entCourts)
            {
                return entCourts.IntCourtLocale == 1;
            });

        }
        
            switch(strSortField.ToUpper())
        {
            case "STRCOURTLOCALE":
                tlstCourts.Sort(string.Format("{0} asc", CourtsColumn.StrCourtLocale.ToString()));
                break;

            case "STRCOURTLOCALE DESC":
                tlstCourts.Sort(string.Format("{0} desc", CourtsColumn.StrCourtLocale.ToString()));
                break;

            case "STRCOURTADDR1":
                tlstCourts.Sort(string.Format("{0} asc", CourtsColumn.StrCourtAddr1.ToString()));
                break;

            case "STRCOURTADDR1 DESC":
                tlstCourts.Sort(string.Format("{0} desc", CourtsColumn.StrCourtAddr1.ToString()));
                break;

            case "STRCOURTCITY":
                tlstCourts.Sort(string.Format("{0} asc", CourtsColumn.StrCourtCity.ToString()));
                break;

            case "STRCOURTCITY DESC":
                tlstCourts.Sort(string.Format("{0} desc", CourtsColumn.StrCourtCity.ToString()));
                break;

            case "STRCOURTSTATE":
                tlstCourts.Sort(string.Format("{0} asc", CourtsColumn.StrCourtState.ToString()));
                break;

            case "STRCOURTSTATE DESC":
                tlstCourts.Sort(string.Format("{0} desc", CourtsColumn.StrCourtState.ToString()));
                break;


        }
        
        return tlstCourts;
    }
}
