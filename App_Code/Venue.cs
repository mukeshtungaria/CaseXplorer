// ***********************************************************************
// Author           : Dennis Sebenick
// Created          : 08-29-2011
//
// Last Modified By : dennis
// Last Modified On : 09-19-2012
// ***********************************************************************
// <copyright file="Venue.cs" company="DGCC.COM">
//     . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Text;
using System.Web;
using System.Reflection;

using System.Collections.Specialized;
using System.Collections.Generic;
using System.Collections;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Runtime.Serialization.Json;
using System.Web.Services.Protocols;
using AjaxControlToolkit;
using JuryData.Entities;
using JuryData.Data;

/// <summary>
/// Venue Web Service Functions
/// </summary>
[WebService(Namespace = "http://dgcc.com/services")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class Venue : System.Web.Services.WebService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Venue" /> class.
    /// </summary>
    public Venue()
    {

    }

    /// <summary>
    /// Get Drop Down Data
    /// </summary>
    /// <param name="knownCategoryValues">The known category values.</param>
    /// <param name="category">The category.</param>
    /// <returns>AjaxControlToolkit.CascadingDropDownNameValue[][].</returns>
    [WebMethod]
    public AjaxControlToolkit.CascadingDropDownNameValue[] GetDropDownContentsV2(string knownCategoryValues, string category)
    {
        StringDictionary kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);

        List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();

        if (kv.Count == 0)
        {
            TList<DropValuesCourt> tlstDropValuesCourt = DataRepository.DropValuesCourtProvider.GetByIntDropValueType(13);
            tlstDropValuesCourt.Sort(DropValuesCourtColumn.StrDropValueDescription.ToString());
            tlstDropValuesCourt.ForEach(delegate(DropValuesCourt entDropValuesCourt)
            {
                values.Add(new CascadingDropDownNameValue(entDropValuesCourt.StrDropValueDescription, entDropValuesCourt.AutoDropValueID.ToString()));
            }
            );

        }
        else
        {
            if (category.ToUpper() == "STATE")
            {
                TList<Catalog> tlstCatalog = clsData.GetCataLogDataByType(5, 226, true, clsData.Sort_Catalog.Desc);

                tlstCatalog.ForEach(delegate(Catalog entCatalog)
                {
                    values.Add(new CascadingDropDownNameValue(entCatalog.StrDesc, entCatalog.StrAbbreviation));
                }
                );
            }

            if (category.ToUpper() == "DISTRICT")
            {
                string strState = kv["State"];
                string strType = kv["Jurisdiction"];

                if (strType.ToUpper() == "116   ")
                {
                    TList<DropValuesCourt> tlstDropValuesCourt = DataRepository.DropValuesCourtProvider.GetByIntDropValueTypeStrDropValueAbbreviation(14, strState.ToUpper());
                    tlstDropValuesCourt.Sort(DropValuesCourtColumn.StrDropValueDescription.ToString());
                    tlstDropValuesCourt.ForEach(delegate(DropValuesCourt entDropValuesCourt)
                    {
                        values.Add(new CascadingDropDownNameValue(entDropValuesCourt.StrDropValueDescription, entDropValuesCourt.AutoDropValueID.ToString()));
                    });
                }
                else
                {
                    TList<DropValuesCourt> tlstDropValuesCourt = DataRepository.DropValuesCourtProvider.GetByIntDropValueTypeStrDropValueAbbreviation(15, strState.ToUpper());
                    tlstDropValuesCourt.Sort(DropValuesCourtColumn.StrDropValueDescription.ToString());
                    tlstDropValuesCourt.ForEach(delegate(DropValuesCourt entDropValuesCourt)
                    {
                        values.Add(new CascadingDropDownNameValue(entDropValuesCourt.StrDropValueDescription, entDropValuesCourt.AutoDropValueID.ToString()));
                    });

                }
            }
        }

        return values.ToArray();
    }


    /// <summary>
    /// Get Drop Down Contents
    /// </summary>
    /// <param name="knownCategoryValues">The known category values.</param>
    /// <param name="category">The category.</param>
    /// <returns>AjaxControlToolkit.CascadingDropDownNameValue[][].</returns>
    [WebMethod]
    public AjaxControlToolkit.CascadingDropDownNameValue[] GetDropDownContents(string knownCategoryValues, string category)
    {
        StringDictionary kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);

        List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();

        if (kv.Count == 0)
        {
            TList<DropValuesCourt> tlstDropValuesCourt = DataRepository.DropValuesCourtProvider.GetByIntDropValueType(13);
            tlstDropValuesCourt.Sort(DropValuesCourtColumn.StrDropValueDescription.ToString());

            tlstDropValuesCourt.ForEach(delegate(DropValuesCourt entDropValuesCourt)
            {
                values.Add(new CascadingDropDownNameValue(entDropValuesCourt.StrDropValueDescription, entDropValuesCourt.StrDropValueDescription));
            }
            );

        }
        else
        {
            if (category.ToUpper() == "STATE")
            {
                TList<Catalog> tlstCatalog = clsData.GetCataLogDataByType(5, 226, true, clsData.Sort_Catalog.Desc);
                tlstCatalog.Sort(CatalogColumn.StrDesc.ToString());

                tlstCatalog.ForEach(delegate(Catalog entCatalog)
                {
                    values.Add(new CascadingDropDownNameValue(entCatalog.StrDesc, entCatalog.StrAbbreviation));
                }
                );
            }

            if (category.ToUpper() == "DISTRICT")
            {

                string strState = kv["State"];
                string strType = kv["Jurisdiction"];

                if (strType.ToUpper() == "FEDERAL")
                {
                    TList<DropValuesCourt> tlstDropValuesCourt = DataRepository.DropValuesCourtProvider.GetByIntDropValueTypeStrDropValueAbbreviation(14, strState.ToUpper());
                    tlstDropValuesCourt.Sort(DropValuesCourtColumn.StrDropValueDescription.ToString());

                    tlstDropValuesCourt.ForEach(delegate(DropValuesCourt entDropValuesCourt)
                    {
                        values.Add(new CascadingDropDownNameValue(entDropValuesCourt.StrDropValueDescription, entDropValuesCourt.AutoDropValueID.ToString()));
                    });
                }
                else
                {
                    TList<DropValuesCourt> tlstDropValuesCourt = DataRepository.DropValuesCourtProvider.GetByIntDropValueTypeStrDropValueAbbreviation(15, strState.ToUpper());
                    tlstDropValuesCourt.Sort(DropValuesCourtColumn.StrDropValueDescription.ToString());
                    tlstDropValuesCourt.ForEach(delegate(DropValuesCourt entDropValuesCourt)
                    {
                        values.Add(new CascadingDropDownNameValue(entDropValuesCourt.StrDropValueDescription, entDropValuesCourt.IntDropValueParentType.ToString()));
                    });

                }
            }
        }

        return values.ToArray();
    }

    /// <summary>
    /// Class MarkerPoint
    /// </summary>
    public class MarkerPoint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MarkerPoint" /> class.
        /// </summary>
        /// <param name="LatExpr">The lat expr.</param>
        /// <param name="LngExpr">The LNG expr.</param>
        /// <param name="DataExpr">The data expr.</param>
        public MarkerPoint(double LatExpr, double LngExpr, string DataExpr)
        {
            lat = LatExpr;
            lng = LngExpr;
            data = DataExpr;
        }

        /// <summary>
        /// Gets or sets the lat.
        /// </summary>
        /// <value>The lat.</value>
        public double lat { set; get; }
        /// <summary>
        /// Gets or sets the LNG.
        /// </summary>
        /// <value>The LNG.</value>
        public double lng { set; get; }
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public string data { set; get; }
    }

    /// <summary>
    /// AJAX / json Drop Down Pair Return Object
    /// </summary>
    public class DropDownPair
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DropDownPair" /> class.
        /// </summary>
        /// <param name="intValue">DropDown Value</param>
        /// <param name="strText">DropDown Description.</param>
        /// <param name="strAbbreviation">DropDown Abbreviation</param>
        public DropDownPair(int intValue, string strText, string strAbbreviation)
        {
            Text = strText;
            Value = intValue;
            Abbreviation = strAbbreviation;
        }
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { set; get; }
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public int Value { set; get; }
        /// <summary>
        /// Gets or sets the abbreviation.
        /// </summary>
        /// <value>The abbreviation.</value>
        public string Abbreviation { set; get; }
    }

    /// <summary>
    /// JsonDataTable Return Object
    /// </summary>
    public class JsonDataTable
    {
        /// <summary>
        /// 
        /// </summary>
        public List<List<object>> aaData;
        /// <summary>
        /// 
        /// </summary>
        public List<JsDataColumn> aoColumns;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonDataTable" /> class.
        /// </summary>
        public JsonDataTable()
        {
            aaData = new List<List<object>>();
            aoColumns = new List<JsDataColumn>();
        }

        /// <summary>
        /// Add_s the row.
        /// </summary>
        /// <param name="cells">The cells.</param>
        public void add_Row(List<object> cells)
        {
            this.aaData.Add(cells);
        }

        /// <summary>
        /// Class JsDataColumn
        /// </summary>
        public class JsDataColumn
        {
            /// <summary>
            /// Gets or sets the s title.
            /// </summary>
            /// <value>The s title.</value>
            public string sTitle { get; set; }
            /// <summary>
            /// Gets or sets the s class.
            /// </summary>
            /// <value>The s class.</value>
            public string sClass { get; set; }
        }

        /// <summary>
        /// Add_s the column.
        /// </summary>
        /// <param name="col">The col.</param>
        public void add_Column(JsDataColumn col)
        {
            this.aoColumns.Add(col);
        }
    }

    /// <summary>
    /// AJAX / json Court Record Return Object
    /// </summary>
    public class CourtRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CourtRecord" /> class.
        /// </summary>
        public CourtRecord()
        {

        }

        /// <summary>
        /// Gets or sets the name of the court.
        /// </summary>
        /// <value>The name of the court.</value>
        public string CourtName { set; get; }

    }

    /// <summary>
    /// Get All States
    /// </summary>
    /// <returns>DropDownPair[][]. <see cref="DropDownPair"/> </returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public DropDownPair[] GetStates()
    {
        List<DropDownPair> DropDownPairTemp = new List<DropDownPair>();

        TList<Catalog> tlstCatalog = clsData.GetCataLogDataByType(5, 226, true, clsData.Sort_Catalog.Desc);
        tlstCatalog.Sort(CatalogColumn.StrDesc.ToString());

        tlstCatalog.ForEach(delegate(Catalog entCatalog)
        {
            DropDownPairTemp.Add(new DropDownPair(entCatalog.IntCatAutoID, entCatalog.StrDesc, entCatalog.StrAbbreviation));
        }
        );

        return DropDownPairTemp.ToArray();
    }

    /// <summary>
    /// Gets the district venues.
    /// </summary>
    /// <param name="strStateAbbreviation">The STR state abbreviation.</param>
    /// <returns>DropDownPair[][].</returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public DropDownPair[] GetDistrictVenues(string strStateAbbreviation)
    {
        List<DropDownPair> DropDownPairTemp = new List<DropDownPair>();

        TList<DropValuesCourt> tlstDropValuesCourt = DataRepository.DropValuesCourtProvider.GetByIntDropValueTypeStrDropValueAbbreviation(14, strStateAbbreviation);
        tlstDropValuesCourt.Sort(DropValuesCourtColumn.StrDropValueDescription.ToString());

        tlstDropValuesCourt.ForEach(delegate(DropValuesCourt entDropValuesCourt)
        {
            DropDownPairTemp.Add(new DropDownPair(entDropValuesCourt.AutoDropValueID, entDropValuesCourt.StrDropValueDescription, entDropValuesCourt.AutoDropValueID.ToString()));
        });

        return DropDownPairTemp.ToArray();
    }

    /// <summary>
    /// Gets the state venues.
    /// </summary>
    /// <param name="strStateAbbreviation">The STR state abbreviation.</param>
    /// <returns>DropDownPair[][].</returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public DropDownPair[] GetStateVenues(string strStateAbbreviation)
    {
        List<DropDownPair> DropDownPairTemp = new List<DropDownPair>();

        TList<DropValuesCourt> tlstDropValuesCourt = DataRepository.DropValuesCourtProvider.GetByIntDropValueTypeStrDropValueAbbreviation(15, strStateAbbreviation);
        tlstDropValuesCourt.Sort(DropValuesCourtColumn.StrDropValueDescription.ToString());

        tlstDropValuesCourt.ForEach(delegate(DropValuesCourt entDropValuesCourt)
        {
            int intCountyID = 0;

            int.TryParse(entDropValuesCourt.IntDropValueParentType.ToString(), out intCountyID);

            if (intCountyID > 0)
            {
                DropDownPairTemp.Add(new DropDownPair(entDropValuesCourt.AutoDropValueID, entDropValuesCourt.StrDropValueDescription, intCountyID.ToString()));
            }
        });

        return DropDownPairTemp.ToArray();
    }

    /// <summary>
    /// Gets the coords.
    /// </summary>
    /// <param name="intCountyGeoID">The int county geo ID.</param>
    /// <returns>MarkerPoint[][].</returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public MarkerPoint[] GetCoords(int intCountyGeoID)
    {
        List<MarkerPoint> MarkerpointTemp = new List<MarkerPoint>();

        TList<CountyGeoCoords> tlstCountyGeoCoords = DataRepository.CountyGeoCoordsProvider.GetByIntGeoCountyID(intCountyGeoID);

        tlstCountyGeoCoords.ForEach(delegate(CountyGeoCoords entCountyGeoCoords)
        {
            MarkerpointTemp.Add(new MarkerPoint(entCountyGeoCoords.FltGeoCountyLat, entCountyGeoCoords.FltGeoCountyLon, ""));
        });

        return MarkerpointTemp.ToArray();
    }

    /// <summary>
    /// Gets the county from district.
    /// </summary>
    /// <param name="intDistrictID">The int district ID.</param>
    /// <returns>System.Int32[][].</returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public int[] GetCountyFromDistrict(int intDistrictID)
    {
        TList<CourtsCounty> tlstCourtsCounty = DataRepository.CourtsCountyProvider.GetByIntCountyLookupDistrictPrimary(intDistrictID);
        tlstCourtsCounty.Sort(CourtsCountyColumn.StrCountyName.ToString());

        List<int> intGeoCoords = new List<int>();

        tlstCourtsCounty.ForEach(delegate(CourtsCounty entCourtsCounty)
        {
            intGeoCoords.Add(entCourtsCounty.IntCountyLookupCountyID);
        });

        return intGeoCoords.ToArray();
    }


    /// <summary>
    /// Gets the county geos.
    /// </summary>
    /// <param name="intCountyID">The int county ID.</param>
    /// <returns>System.Int32[][].</returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public int[] GetCountyGeos(int intCountyID)
    {
        List<int> intGeoCoords = new List<int>();

        TList<CountyGeoList> tlstCountGeoList = DataRepository.CountyGeoListProvider.GetByIntGeoFIPS(intCountyID);

        tlstCountGeoList.ForEach(delegate(CountyGeoList entCountyGeoList)
        {
            intGeoCoords.Add(entCountyGeoList.IntGeoCountyID);
        });

        return intGeoCoords.ToArray();
    }

    /// <summary>
    /// Gets the venues by district.
    /// </summary>
    /// <param name="intDistrictID">The int district ID.</param>
    /// <returns>JsonDataTable.</returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public JsonDataTable GetVenuesByDistrict(int intDistrictID)
    {
        TList<Courts> tlstCourts;
        tlstCourts = DataRepository.CourtsProvider.GetByIntCourtDistrictID(intDistrictID);

        JsonDataTable jsDT = new JsonDataTable();

        if (tlstCourts.Count > 0)
        {
            string strQuote = @"""";

            tlstCourts.ForEach(delegate(Courts entCourts)
            {
                string strAddress = entCourts.StrCourtAddr1;

                if (entCourts.StrCourtAddr2 != null)
                {
                    if (entCourts.StrCourtAddr2.Length > 0)
                    {
                        strAddress = strAddress + ", " + entCourts.StrCourtAddr2;
                    }
                }

                string strCSZ = string.Empty;

                if (entCourts.StrCourtCity != null && entCourts.StrCourtState != null && entCourts.StrCourtZip != null)
                {
                    strCSZ = string.Format("{0}, {1}  {2}", entCourts.StrCourtCity, entCourts.StrCourtState, entCourts.StrCourtZip);
                }

                if (strCSZ.Length > 0)
                {
                    strAddress = strAddress + "<br/>" + strCSZ;
                }

                string strLocale = entCourts.StrCourtLocale;

                if (entCourts.StrCourtCircuit != null)
                {
                    if (entCourts.StrCourtCircuit.Length > 0)
                        strLocale = strLocale + "<br/>" + entCourts.StrCourtCircuit;
                }

                if (entCourts.StrCourtAddr3 != null)
                {
                    if (entCourts.StrCourtAddr3.Length > 0)
                        strLocale = strLocale + "<br/>" + entCourts.StrCourtAddr3;
                }

                if (entCourts.StrCourtDesc != null)
                {
                    if (entCourts.StrCourtDesc.Length > 0)
                        strLocale = strLocale + "<br/>" + entCourts.StrCourtDesc;

                }

                List<object> vl = new List<object>();

                vl.Add(entCourts.AutoCourtID.ToString());

                if (entCourts.DecLat != null && entCourts.DecLon != null)
                {
                    vl.Add(entCourts.DecLat.ToString());
                    vl.Add(entCourts.DecLon.ToString());

                    //                    vl.Add("<a href='javascript: ShowMap(" + entCourts.DecLat.ToString() + "," + entCourts.DecLon.ToString() + ");'>Show Map</a>");
                    vl.Add("<a href='#'>Show Map</a>");
                }
                else
                {
                    string strAddress2 = strAddress.Replace(",", " ").Replace("<br/>", ", ");

                    vl.Add(strAddress2);
                    vl.Add("");

                    //                    vl.Add("<a href='javascript: ShowAddress(" + entCourts.AutoCourtID + "," + strQuote + strAddress2 + strQuote + ");'>Show Map</a>");

                    vl.Add("<a href='#'>Show Map</a>");
                }

                vl.Add(strLocale);
                vl.Add(strAddress);
                vl.Add(entCourts.StrCourtCity);

                jsDT.add_Row(vl);
            });
        }

        return jsDT;
    }

    /// <summary>
    /// Class LatLng
    /// </summary>
    public class LatLng
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LatLng" /> class.
        /// </summary>
        /// <param name="decLat">The dec lat.</param>
        /// <param name="decLng">The dec LNG.</param>
        public LatLng(decimal decLat, decimal decLng)
        {
            Lat = decLat;
            Lng = decLng;
        }

        /// <summary>
        /// Gets or sets the lat.
        /// </summary>
        /// <value>The lat.</value>
        public decimal Lat { set; get; }
        /// <summary>
        /// Gets or sets the LNG.
        /// </summary>
        /// <value>The LNG.</value>
        public decimal Lng { set; get; }
    }

    /// <summary>
    /// Saves the lat lon.
    /// </summary>
    /// <param name="intCourtID">The int court ID.</param>
    /// <param name="decLat">The dec lat.</param>
    /// <param name="decLng">The dec LNG.</param>
    /// <returns>LatLng.</returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public LatLng SaveLatLon(int intCourtID, decimal decLat, decimal decLng)
    {
        Courts entCourts = DataRepository.CourtsProvider.GetByAutoCourtID(intCourtID);

        entCourts.DecLat = decLat;
        entCourts.DecLon = decLng;

        DataRepository.CourtsProvider.Save(entCourts);

        return new LatLng(decLat, decLng);
    }

    /// <summary>
    /// Gets the venues by county.
    /// </summary>
    /// <param name="intCountyID">The int county ID.</param>
    /// <returns>JsonDataTable.</returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public JsonDataTable GetVenuesByCounty(int intCountyID)
    {
        TList<Courts> tlstCourts;
        tlstCourts = DataRepository.CourtsProvider.GetByIntCourtCountyFIPS(intCountyID);

        tlstCourts.ApplyFilter(delegate(Courts entCourts)
        {
            return entCourts.IntCourtLocale == 1;
        });

        JsonDataTable jsDT = new JsonDataTable();

        if (tlstCourts.Count > 0)
        {
            string strQuote = @"""";

            tlstCourts.ForEach(delegate(Courts entCourts)
            {
                string strAddress = entCourts.StrCourtAddr1;

                if (entCourts.StrCourtAddr2 != null)
                {
                    if (entCourts.StrCourtAddr2.Length > 0)
                    {
                        strAddress = strAddress + ", " + entCourts.StrCourtAddr2;
                    }
                }

                string strCSZ = string.Empty;

                if (entCourts.StrCourtCity != null && entCourts.StrCourtState != null && entCourts.StrCourtZip != null)
                {
                    strCSZ = string.Format("{0}, {1}  {2}", entCourts.StrCourtCity, entCourts.StrCourtState, entCourts.StrCourtZip);
                }

                if (strCSZ.Length > 0)
                {
                    strAddress = strAddress + "<br/>" + strCSZ;
                }

                string strLocale = entCourts.StrCourtLocale;

                if (entCourts.StrCourtCircuit != null)
                {
                    if (entCourts.StrCourtCircuit.Length > 0)
                        strLocale = strLocale + "<br/>" + entCourts.StrCourtCircuit;
                }

                if (entCourts.StrCourtAddr3 != null)
                {
                    if (entCourts.StrCourtAddr3.Length > 0)
                        strLocale = strLocale + "<br/>" + entCourts.StrCourtAddr3;
                }

                if (entCourts.StrCourtDesc != null)
                {
                    if (entCourts.StrCourtDesc.Length > 0)
                        strLocale = strLocale + "<br/>" + entCourts.StrCourtDesc;
                }

                List<object> vl = new List<object>();

                vl.Add(entCourts.AutoCourtID.ToString());

                if (entCourts.DecLat != null && entCourts.DecLon != null)
                {
                    vl.Add(entCourts.DecLat.ToString());
                    vl.Add(entCourts.DecLon.ToString());
                    vl.Add("<a href='#' class='link'>Show Map</a>");
                }
                else
                {
                    string strAddress2 = strAddress.Replace(",", " ").Replace("<br/>", ", ");

                    vl.Add(strAddress2);
                    vl.Add("");
                    vl.Add("<a href='#' class='link'>Show Map</a>");
                }

                vl.Add(strLocale);
                vl.Add(strAddress);
                vl.Add(entCourts.StrCourtCity);

                jsDT.add_Row(vl);
            });
        }

        return jsDT;
    }


    /// <summary>
    /// Gets the demographics by county.
    /// </summary>
    /// <param name="intCountyID">The int county ID.</param>
    /// <returns>System.String.</returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetDemographicsByCounty(int intCountyID)
    {
        ArrayList aryCounties = new ArrayList();
        aryCounties.Add(intCountyID);

        return GetDemographics(aryCounties);
    }

    /// <summary>
    /// Gets the demographics.
    /// </summary>
    /// <param name="aryCounties">The ary counties.</param>
    /// <returns>System.String.</returns>
    private string GetDemographics(ArrayList aryCounties)
    {
        if (aryCounties != null)
        {
            // intIncome

            int intBase = 0;
            int intMedianIncome = 0;
            int[] intIncomes = new int[8];
            StringBuilder sb = new StringBuilder();
            string[] strDescInc = { "Less than $25,000", "$25,000 - $34,999", "$35,000 - $49,999", "$50,000 - $74,999", "$75,000 - $99,999", "$100,000 - $149,999", "$150,000 - $199,999", "$200,000 +" };

            foreach (int intCounty in aryCounties)
            {
                ESRIIncome entESRIIncome = DataRepository.ESRIIncomeProvider.GetByID(intCounty);

                if (entESRIIncome != null)
                {
                    if (aryCounties.Count == 1)
                        intMedianIncome = (int)entESRIIncome.MEDHINCCY;

                    //intBase += (int)entESRIIncome.HINCBASECY;
                    intIncomes[0] += (int)(entESRIIncome.HINC0CY + entESRIIncome.HINC10CY + entESRIIncome.HINC15CY + entESRIIncome.HINC20CY);    // Under $25,000
                    intIncomes[1] += (int)(entESRIIncome.HINC25CY + entESRIIncome.HINC30CY);  // $25,000 - $34,999
                    intIncomes[2] += (int)(entESRIIncome.HINC35CY + entESRIIncome.HINC40CY + entESRIIncome.HINC45CY);  // $35,000 - $49,999
                    intIncomes[3] += (int)(entESRIIncome.HINC50CY + entESRIIncome.HINC60CY);  // $50,000 - $74,999
                    intIncomes[4] += (int)(entESRIIncome.HINC75CY);  // $75,000 - $99,999
                    intIncomes[5] += (int)(entESRIIncome.HINC100CY + entESRIIncome.HINC125CY);  // $99,999 - $149,999
                    intIncomes[6] += (int)(entESRIIncome.HINC150CY);  // $150,000 - $199,999
                    intIncomes[7] += (int)(entESRIIncome.HINC200CY + entESRIIncome.HINC250CY + entESRIIncome.HINC500CY);  // $200,000+
                }
            }
            foreach (int i in intIncomes)
                intBase += i;

            sb.Append("<div class='IncContainer'>");
            sb.AppendFormat("<div class='IncHeader'>Total Households:  {0:0,0}</div>", intBase);

            if (aryCounties.Count == 1)
                sb.AppendFormat("<div class='IncHeader2'>Median Household Income:  {0:$#,##0;($#,##0);Zero}</div>", intMedianIncome);

            sb.Append("<div class='IncDesc_HDR'>Household Income</div><div class='PctCol_HDR'>%</div>");

            for (int intCounter = 0; intCounter < 8; intCounter++)
            {
                decimal decPct = 0;

                if (intBase > 0)
                    decPct = (decimal)intIncomes[intCounter] / (decimal)intBase;

                if (intCounter == 7)
                    sb.AppendFormat("<div class='IncDesc BottomBorder'>{0}</div><div class='PctCol BottomBorder'>{1:0.00%}</div>", strDescInc[intCounter], decPct);
                else
                    sb.AppendFormat("<div class='IncDesc'>{0}</div><div class='PctCol'>{1:0.00%}</div>", strDescInc[intCounter], decPct);

            }

            sb.Append("</div>");

            // Age
            intBase = 0;
            int intMedianAge = 0;
            int[] intAges = new int[7];
            string[] strDescAge = { "Under 15", "15-24", "25-34", "35-44", "45-54", "55-64", "64+" };

            foreach (int intCounty in aryCounties)
            {
                ESRIAgeSex entESRIAgeSex = DataRepository.ESRIAgeSexProvider.GetByID(intCounty);

                if (entESRIAgeSex != null)
                {
                    if (aryCounties.Count == 1)
                        intMedianAge = (int)entESRIAgeSex.MEDAGECY;

                    //intBase += (int)entESRIAgeSex.AGEBASECY;
                    intAges[0] += (int)(entESRIAgeSex.POPU5CY + entESRIAgeSex.POP5CY + entESRIAgeSex.POP10CY);    // Under 15
                    intAges[1] += (int)(entESRIAgeSex.POP15CY + entESRIAgeSex.POP20CY);  // 15 - 24
                    intAges[2] += (int)(entESRIAgeSex.POP25CY + entESRIAgeSex.POP30CY);  // 25 - 34
                    intAges[3] += (int)(entESRIAgeSex.POP35CY + entESRIAgeSex.POP40CY);  // 35 - 44
                    intAges[4] += (int)(entESRIAgeSex.POP45CY + entESRIAgeSex.POP50CY);  // 45 - 54
                    intAges[5] += (int)(entESRIAgeSex.POP55CY + entESRIAgeSex.POP60CY);  // 55 - 64
                    intAges[6] += (int)(entESRIAgeSex.POP65CY + entESRIAgeSex.POP70CY + entESRIAgeSex.POP75CY + entESRIAgeSex.POP80CY + entESRIAgeSex.POP85CY);  // 65+
                }
            }
            foreach (int i in intAges)
                intBase += i;

            sb.Append("<div class='AgeContainer'>");
            sb.AppendFormat("<div class='AgeHeader'>Population Base:  {0:0,0}</div>", intBase);

            if (aryCounties.Count == 1)
                sb.AppendFormat("<div class='AgeHeader2'>Median Age:  {0}</div>", intMedianAge);

            sb.Append("<div class='AgeDesc_HDR'>Age Range</div><div class='PctCol_HDR'>%</div>");

            for (int intCounter = 0; intCounter < 7; intCounter++)
            {
                decimal decPct = 0;

                if (intBase > 0)
                    decPct = (decimal)intAges[intCounter] / (decimal)intBase;

                if (intCounter == 6)
                    sb.AppendFormat("<div class='AgeDesc BottomBorder'>{0}</div><div class='PctCol BottomBorder'>{1:0.00%}</div>", strDescAge[intCounter], decPct);
                else
                    sb.AppendFormat("<div class='AgeDesc'>{0}</div><div class='PctCol'>{1:0.00%}</div>", strDescAge[intCounter], decPct);
            }

            sb.Append("</div>");

            // Race
            if (aryCounties != null)
            {
                intBase = 0;
                int[] intRace = new int[5];
                double dblDiversityIndex = 0;

                string[] strDescRace = { "Caucasian", "African American", "Asian American", "Latino American", "Other" };


                foreach (int intCounty in aryCounties)
                {
                    ESRIRace entESRIRace = DataRepository.ESRIRaceProvider.GetByID(intCounty);

                    if (entESRIRace != null)
                    {
                        if (aryCounties.Count == 1)
                            dblDiversityIndex = (double)entESRIRace.DIVINDXCY;

                        //intBase += (int)entESRIRace.RACEBASECY;  //+ (int)entESRIRace.HISPPOPCY;
                        intRace[0] += (int)(entESRIRace.WHITECY);        // Caucasian
                        intRace[1] += (int)(entESRIRace.BLACKCY);        // African American
                        intRace[2] += (int)(entESRIRace.ASIANCY);        // Asian American
                        intRace[3] += (int)(entESRIRace.HISPPOPCY);      // Hispanic
                        intRace[4] += (int)(entESRIRace.AMERINDCY + entESRIRace.PACIFICCY + entESRIRace.OTHRACECY);  // Other
                    }
                }
                foreach (int i in intRace)
                    intBase += i;

                sb.Append("<div class='RaceContainer'>");
                sb.AppendFormat("<div class='RaceHeader'>Population Base:  {0:0,0}</div>", intBase);

                if (aryCounties.Count == 1)
                    sb.AppendFormat("<div class='RaceHeader2'>Diversity Index:  {0}</div>", dblDiversityIndex);

                sb.Append("<div class='RaceDesc_HDR'>Race</div><div class='PctCol_HDR'>%</div>");


                for (int intCounter = 0; intCounter < 5; intCounter++)
                {
                    decimal decPct = 0;

                    if (intBase > 0)
                        decPct = (decimal)intRace[intCounter] / (decimal)intBase;

                    if (intCounter == 4)
                        sb.AppendFormat("<div class='RaceDesc BottomBorder'>{0}</div><div class='PctCol BottomBorder'>{1:0.00%}</div>", strDescRace[intCounter], decPct);
                    else
                        sb.AppendFormat("<div class='RaceDesc'>{0}</div><div class='PctCol'>{1:0.00%}</div>", strDescRace[intCounter], decPct);
                }
                sb.Append("</div>");
            }

            sb.Append(OccupationIndustry(aryCounties, 2));
            sb.Append(OccupationIndustry(aryCounties, 3));

            return sb.ToString();
        }

        else
            return string.Empty;
    }

    /// <summary>
    /// Occupations the industry.
    /// </summary>
    /// <param name="aryCounties">The ary counties.</param>
    /// <param name="intDisplayType">Display type of the int.</param>
    /// <returns>System.String.</returns>
    private string OccupationIndustry(ArrayList aryCounties, short intDisplayType)
    {
        StringBuilder sb = new StringBuilder();

        //
        // Occupation / Industry
        //

        ArrayList lstValues = new ArrayList();

        TList<ESRIMappings> tlstESRIMappings = null;

        // 2 - Industry
        // 3 - Occupation
        tlstESRIMappings = DataRepository.ESRIMappingsProvider.GetByIntESRIMappingType(intDisplayType);

        string strDescription = string.Empty;

        if (intDisplayType == 2)
            strDescription = "Industry";
        else if (intDisplayType == 3)
            strDescription = "Occupation";

        int intUnemp = 0;
        int intWorkForceTotal = 0;
        int intBase = 0;
        int intCounter2 = 0;
        bool boolFirst = true;

        foreach (int intCounty in aryCounties)
        {
            ESRIOccupationIndustry entESRIOccupationIndustry = DataRepository.ESRIOccupationIndustryProvider.GetByID(intCounty);

            if (entESRIOccupationIndustry != null)
            {
                intBase += (int)entESRIOccupationIndustry.INDBASECY;
                intUnemp += (int)entESRIOccupationIndustry.UNEMPCY;
                intWorkForceTotal += (int)entESRIOccupationIndustry.CIVLBFRCY;

                if (tlstESRIMappings != null)
                {
                    intCounter2 = 0;
                    tlstESRIMappings.ForEach(delegate(ESRIMappings entESRIMappings)
                    {
                        PropertyInfo pi = entESRIOccupationIndustry.GetType().GetProperty(entESRIMappings.StrESRIMappingField, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                        int intValue = (int)pi.GetValue(entESRIOccupationIndustry, null);

                        if (boolFirst)
                            lstValues.Add(new OccValues(intValue, entESRIMappings.StrESRIMappingDesc));
                        else
                        {
                            OccValues valCurr = (OccValues)lstValues[intCounter2];
                            valCurr.Value += intValue;
                            lstValues[intCounter2] = valCurr;
                        }

                        intCounter2++;
                    });

                    boolFirst = false;
                }
            }
        }
        intBase = intUnemp;
        foreach (OccValues occValue in lstValues)
            intBase += occValue.Value;

        sb.Append("<div class='OccContainer'>");
        sb.AppendFormat("<div class='OccHeader'>{1} Base Workforce:  {0:0,0}</div>", intBase, strDescription);
        sb.AppendFormat("<div class='OccDesc_HDR'>{0}</div><div class='PctCol_HDR'>%</div>", strDescription);

        if (intBase > 0)
            sb.AppendFormat("<div class='OccDesc'>{0}</div><div class='PctCol'>{1:0.00%}</div>", "Unemployment<sup> *</sup>", (decimal)intUnemp / (decimal)intBase);

        int intCounterLoop = 0;

        lstValues.Sort(new clsSorter());

        foreach (OccValues occValue in lstValues)
        {
            intCounterLoop++;

            if (intCounterLoop == lstValues.Count)
            {
                if (intBase > 0)
                    sb.AppendFormat("<div class='OccDesc BottomBorder'>{0}</div><div class='PctCol BottomBorder'>{1:0.00%}</div>", occValue.Description, (decimal)occValue.Value / (decimal)intBase);
            }
            else
            {
                if (intBase > 0)
                    sb.AppendFormat("<div class='OccDesc'>{0}</div><div class='PctCol'>{1:0.00%}</div>", occValue.Description, (decimal)occValue.Value / (decimal)intBase);
            }
        }

        sb.Append("<div class='OccFooter'>* = Unemployment not include in aggregrate employment percentage.</div>");
        sb.Append("</div>");

        return sb.ToString();

    }

    /// <summary>
    /// Gets the segment desc.
    /// </summary>
    /// <param name="intSegmentID">The int segment ID.</param>
    /// <returns>System.String.</returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetSegmentDesc(int intSegmentID)
    {
        ESRIHHSegmentsDesc entESRIHHSegmentsDesc = DataRepository.ESRIHHSegmentsDescProvider.GetByIntSegDescESRIID(intSegmentID);

        if (entESRIHHSegmentsDesc != null)
        {
            return entESRIHHSegmentsDesc.StrSegDesc;
        }
        else
            return string.Empty;
    }


    /// <summary>
    /// Gets the demographics by district.
    /// </summary>
    /// <param name="intDistrictID">The int district ID.</param>
    /// <returns>System.String.</returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetDemographicsByDistrict(int intDistrictID)
    {
        ArrayList aryCounties = new ArrayList();
        TList<CourtsCounty> tlstCourtsCounty = DataRepository.CourtsCountyProvider.GetByIntCountyLookupDistrictPrimary(intDistrictID);

        tlstCourtsCounty.ForEach(delegate(CourtsCounty entCourtsCounty)
        {
            aryCounties.Add(entCourtsCounty.IntCountyLookupCountyID);
        });

        return GetDemographics(aryCounties);
    }





    /// <summary>
    /// Gets the life styles by district.
    /// </summary>
    /// <param name="intDistrictID">The int district ID.</param>
    /// <returns>System.String.</returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetLifeStylesByDistrict(int intDistrictID)
    {
        ArrayList aryCounties = new ArrayList();

        TList<CourtsCounty> tlstCourtsCounty = DataRepository.CourtsCountyProvider.GetByIntCountyLookupDistrictPrimary(intDistrictID);

        tlstCourtsCounty.ForEach(delegate(CourtsCounty entCourtsCounty)
        {
            aryCounties.Add(entCourtsCounty.IntCountyLookupCountyID);
        });

        StringBuilder sb = new StringBuilder();

        sb.Append(HouseholdSegments(aryCounties));
        sb.Append(PopulationSegments(aryCounties));

        return sb.ToString();
    }

    /// <summary>
    /// Gets the life styles by county.
    /// </summary>
    /// <param name="intCountyID">The int county ID.</param>
    /// <returns>System.String.</returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetLifeStylesByCounty(int intCountyID)
    {
        ArrayList aryCounties = new ArrayList();
        aryCounties.Add(intCountyID);

        StringBuilder sb = new StringBuilder();

        sb.Append(HouseholdSegments(aryCounties));
        sb.Append(PopulationSegments(aryCounties));

        return sb.ToString();
    }


    /// <summary>
    /// Populations the segments.
    /// </summary>
    /// <param name="aryCounties">The ary counties.</param>
    /// <returns>System.String.</returns>
    private string PopulationSegments(ArrayList aryCounties)
    {
        if (aryCounties != null)
        {
            ArrayList lstValues = new ArrayList();

            TList<ESRIMappings> tlstESRIMappings = null;

            tlstESRIMappings = DataRepository.ESRIMappingsProvider.GetByIntESRIMappingType(4);

            int intBase = 0;
            int intCounter = 0;
            bool boolFirst = true;

            foreach (int intCounty in aryCounties)
            {
                ESRIPopSegments entESRIPopSegments = DataRepository.ESRIPopSegmentsProvider.GetByID(intCounty);

                if (entESRIPopSegments != null)
                {
                    //intBase += (int)entESRIPopSegments.TAPPBASECY;

                    if (tlstESRIMappings != null)
                    {
                        intCounter = 0;
                        tlstESRIMappings.ForEach(delegate(ESRIMappings entESRIMappings)
                        {
                            PropertyInfo pi = entESRIPopSegments.GetType().GetProperty(entESRIMappings.StrESRIMappingField, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                            int intValue = (int)pi.GetValue(entESRIPopSegments, null);

                            if (boolFirst)
                                lstValues.Add(new OccValuesLS(intValue, entESRIMappings.StrESRIMappingDesc, entESRIMappings.IntESRIMappingIndex));
                            else
                            {
                                OccValuesLS valCurr = (OccValuesLS)lstValues[intCounter];
                                valCurr.Value += intValue;
                                lstValues[intCounter] = valCurr;
                            }

                            intCounter++;
                        });

                        boolFirst = false;
                    }
                }
            }
            foreach (OccValuesLS occValue in lstValues)
                intBase += occValue.Value;

            StringBuilder sb = new StringBuilder();

            sb.Append("<div class='OccContainer'>");
            sb.AppendFormat("<div class='OccHeader'>{1} Base:  {0:0,0}</div>", intBase, "Population Segments");
            sb.AppendFormat("<div class='OccDesc_HDR'>{0}</div><div class='PctCol_HDR'>%</div>", "Population Segments");

            int intCounterLoop = 0;

            lstValues.Sort(new clsSorterLS());

            foreach (OccValuesLS occValue in lstValues)
            {
                intCounterLoop++;

                if (occValue.Value > 0)
                {
                    if (intCounterLoop == lstValues.Count)
                        sb.AppendFormat("<div class='OccDesc BottomBorder'><a href='' onclick='OpenLifeStyle({2});return false;'>{0}</a></div><div class='PctCol BottomBorder'>{1:0.00%}</div>", occValue.Description, (decimal)occValue.Value / (decimal)intBase, occValue.SegID);
                    else
                        sb.AppendFormat("<div class='OccDesc'><a href='' onclick='OpenLifeStyle({2});return false;'>{0}</a></div><div class='PctCol'>{1:0.00%}</div>", occValue.Description, (decimal)occValue.Value / (decimal)intBase, occValue.SegID);
                }
            }

            sb.Append("<div class='OccDesc BottomBorder' style='height: 1px;'></div><div class='PctCol BottomBorder' style='height: 1px;'></div>");
            sb.Append("</div>");

            return sb.ToString();
        }
        else
            return string.Empty;
    }


    /// <summary>
    /// Households the segments.
    /// </summary>
    /// <param name="aryCounties">The ary counties.</param>
    /// <returns>System.String.</returns>
    private string HouseholdSegments(ArrayList aryCounties)
    {
        if (aryCounties != null)
        {
            ArrayList lstValues = new ArrayList();

            TList<ESRIMappings> tlstESRIMappings = null;

            tlstESRIMappings = DataRepository.ESRIMappingsProvider.GetByIntESRIMappingType(5);

            int intBase = 0;
            int intCounter = 0;
            bool boolFirst = true;

            foreach (int intCounty in aryCounties)
            {
                ESRIHHSegments entESRIHHSegments = DataRepository.ESRIHHSegmentsProvider.GetByID(intCounty);

                if (entESRIHHSegments != null)
                {
                    //intBase += (int)entESRIHHSegments.TAPHBASECY;

                    if (tlstESRIMappings != null)
                    {
                        intCounter = 0;
                        tlstESRIMappings.ForEach(delegate(ESRIMappings entESRIMappings)
                        {
                            PropertyInfo pi = entESRIHHSegments.GetType().GetProperty(entESRIMappings.StrESRIMappingField, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                            int intValue = (int)pi.GetValue(entESRIHHSegments, null);

                            if (boolFirst)
                                lstValues.Add(new OccValuesLS(intValue, entESRIMappings.StrESRIMappingDesc, entESRIMappings.IntESRIMappingIndex));
                            else
                            {
                                OccValuesLS valCurr = (OccValuesLS)lstValues[intCounter];
                                valCurr.Value += intValue;
                                lstValues[intCounter] = valCurr;
                            }

                            intCounter++;
                        });

                        boolFirst = false;
                    }
                }
            }
            foreach (OccValuesLS occValue in lstValues)
                intBase += occValue.Value;

            StringBuilder sb = new StringBuilder();

            string strDescription = "Household Segments";

            sb.Append("<div class='OccContainer'>");
            sb.AppendFormat("<div class='OccHeader'>{1} Base:  {0:0,0}</div>", intBase, strDescription);
            sb.AppendFormat("<div class='OccDesc_HDR'>{0}</div><div class='PctCol_HDR'>%</div>", strDescription);

            int intCounterLoop = 0;

            lstValues.Sort(new clsSorterLS());

            foreach (OccValuesLS occValue in lstValues)
            {
                intCounterLoop++;

                if (occValue.Value > 0)
                {
                    if (intCounterLoop == lstValues.Count)
                        sb.AppendFormat("<div class='OccDesc BottomBorder'><a href='' onclick='OpenLifeStyle({2});return false;'>{0}</a></div><div class='PctCol BottomBorder'>{1:0.00%}</div>", occValue.Description, (decimal)occValue.Value / (decimal)intBase, occValue.SegID);
                    else
                        sb.AppendFormat("<div class='OccDesc'><a href='' onclick='OpenLifeStyle({2});return false;'>{0}</a></div><div class='PctCol'>{1:0.00%}</div>", occValue.Description, (decimal)occValue.Value / (decimal)intBase, occValue.SegID);
                }
            }

            sb.Append("<div class='OccDesc BottomBorder' style='height: 1px;'></div><div class='PctCol BottomBorder' style='height: 1px;'></div>");
            sb.Append("</div>");
            return sb.ToString();
        }
        else
            return string.Empty;

    }


    /// <summary>
    /// Struct OccValues
    /// </summary>
    public struct OccValues
    {
        /// <summary>
        /// 
        /// </summary>
        public int Value;
        /// <summary>
        /// 
        /// </summary>
        public string Description;

        /// <summary>
        /// Initializes a new instance of the <see cref="OccValues" /> struct.
        /// </summary>
        /// <param name="intValue">The int value.</param>
        /// <param name="strDescription">The STR description.</param>
        public OccValues(int intValue, string strDescription)
        {
            Value = intValue;
            Description = strDescription;
        }
    }


    /// <summary>
    /// Class clsSorter
    /// </summary>
    public class clsSorter : IComparer
    {
        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero <paramref name="x" /> is less than <paramref name="y" />. Zero <paramref name="x" /> equals <paramref name="y" />. Greater than zero <paramref name="x" /> is greater than <paramref name="y" />.</returns>
        int IComparer.Compare(object x, object y)
        {
            OccValues x1 = (OccValues)x;
            OccValues y1 = (OccValues)y;
            return ((new CaseInsensitiveComparer()).Compare(y1.Value, x1.Value));
        }
    }


    /// <summary>
    /// Class clsSorterLS
    /// </summary>
    public class clsSorterLS : IComparer
    {
        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero <paramref name="x" /> is less than <paramref name="y" />. Zero <paramref name="x" /> equals <paramref name="y" />. Greater than zero <paramref name="x" /> is greater than <paramref name="y" />.</returns>
        int IComparer.Compare(object x, object y)
        {
            OccValuesLS x1 = (OccValuesLS)x;
            OccValuesLS y1 = (OccValuesLS)y;
            return ((new CaseInsensitiveComparer()).Compare(y1.Value, x1.Value));
        }
    }

    /// <summary>
    /// Struct OccValuesLS
    /// </summary>
    public struct OccValuesLS
    {
        /// <summary>
        /// 
        /// </summary>
        public int Value;
        /// <summary>
        /// 
        /// </summary>
        public string Description;
        /// <summary>
        /// 
        /// </summary>
        public int SegID;

        /// <summary>
        /// Initializes a new instance of the <see cref="OccValuesLS" /> struct.
        /// </summary>
        /// <param name="intValue">The int value.</param>
        /// <param name="strDescription">The STR description.</param>
        /// <param name="intSegID">The int seg ID.</param>
        public OccValuesLS(int intValue, string strDescription, int intSegID)
        {
            Value = intValue;
            Description = strDescription;
            SegID = intSegID;
        }
    }


    /// <summary>
    /// Tests the update.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <param name="fromPosition">From position.</param>
    /// <param name="toPosition">To position.</param>
    /// <param name="direction">The direction.</param>
    /// <param name="group">The group.</param>
    /// <returns>System.String.</returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string TestUpdate(int id, int fromPosition, int toPosition, string direction, string group) // int id, int fromPosition, int toPosition, string direction, string group)
    {

        return string.Empty;
    }



}