// ***********************************************************************
// Author           : Dennis Sebenick
// Created          : 04-21-2012
//
// Last Modified By : dennis
// Last Modified On : 04-21-2012
// ***********************************************************************
// <copyright file="clsMapping.cs" company="DGCC.COM">
//     . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using JuryData.Data;
using JuryData.Entities;


/// <summary>
/// Summary description for clsMapping
/// </summary>
public class clsMapping
{
    /// <summary>
    /// 
    /// </summary>
    private int m_intCountyID = 0;
    /// <summary>
    /// 
    /// </summary>
    private ArrayList aryCounties;
    /// <summary>
    /// 
    /// </summary>
    private ArrayList aryCountyDesc;
    /// <summary>
    /// 
    /// </summary>
    private int m_intDistrictID = 0;


    /// <summary>
    /// Initializes a new instance of the <see cref="clsMapping" /> class.
    /// </summary>
	public clsMapping()
	{
		//
		// TODO: Add constructor logic here
		//
	}


    /// <summary>
    /// Gets or sets the district ID.
    /// </summary>
    /// <value>The district ID.</value>
    public int DistrictID
    {
        get
        {
            return m_intDistrictID;
        }
        set
        {
            m_intDistrictID = value;

            TList<CourtsCounty> tlstCourtsCounty = DataRepository.CourtsCountyProvider.GetByIntCountyLookupDistrictPrimary(m_intDistrictID);
            tlstCourtsCounty.Sort(CourtsCountyColumn.StrCountyName.ToString());

            if (aryCounties != null)
                aryCounties = null;

            if (aryCountyDesc != null)
                aryCountyDesc = null;

            aryCounties = new ArrayList();
            aryCountyDesc = new ArrayList();

            tlstCourtsCounty.ForEach(delegate(CourtsCounty entCourtsCounty)
            {
                aryCounties.Add(entCourtsCounty.IntCountyLookupCountyID);
                aryCountyDesc.Add(entCourtsCounty);
            });

            aryCountyDesc.Sort(new clsCountySort());

        }
    }

    /// <summary>
    /// Gets the county list.
    /// </summary>
    /// <value>The county list.</value>
    public ArrayList CountyList
    {
        get
        {
            return aryCountyDesc;
        }
    }

    /// <summary>
    /// Gets or sets the county ID.
    /// </summary>
    /// <value>The county ID.</value>
    public int CountyID
    {
        get
        {
            return m_intCountyID;
        }
        set
        {
            m_intCountyID = value;

            aryCounties = null;
            aryCounties = new ArrayList();
            aryCounties.Add(m_intCountyID);

        }
    }
}


/// <summary>
/// County Sort Compare Routine
/// </summary>
public class clsCountySort : IComparer
{
    /// <summary>
    /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
    /// </summary>
    /// <param name="x">The first object to compare.</param>
    /// <param name="y">The second object to compare.</param>
    /// <returns>A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero <paramref name="x" /> is less than <paramref name="y" />. Zero <paramref name="x" /> equals <paramref name="y" />. Greater than zero <paramref name="x" /> is greater than <paramref name="y" />.</returns>
    int IComparer.Compare(object x, object y)
    {
        CourtsCounty x1 = (CourtsCounty)x;
        CourtsCounty y1 = (CourtsCounty)y;
        return ((new CaseInsensitiveComparer()).Compare(x1.StrCountyName, y1.StrCountyName));
    }
}

