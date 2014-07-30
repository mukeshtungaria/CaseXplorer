// ***********************************************************************
// Author           : Dennis Sebenick
// Created          : 08-29-2011
//
// Last Modified By : dennis
// Last Modified On : 05-04-2012
// ***********************************************************************
// <copyright file="MockJury.cs" company="DGCC.COM">
//     . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;

using JuryData.Data;
using JuryData.Entities;


/// <summary>
/// MockJury Web Service functions
/// </summary>
[WebService(Namespace = "https://www.casexplorer.com")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class MockJury : System.Web.Services.WebService {

    /// <summary>
    /// Initializes a new instance of the <see cref="MockJury" /> class.
    /// </summary>
    public MockJury () {
    }

    /// <summary>
    /// AJAX / json Combo Values Object
    /// </summary>
    private class clsComboValues
    {
        /// <summary>
        /// Description
        /// </summary>
        public string _Description;
        /// <summary>
        /// Value
        /// </summary>
        public int _Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="clsComboValues" /> class.
        /// </summary>
        /// <param name="strDescription">The STR description.</param>
        /// <param name="intValue">The int value.</param>
        public clsComboValues(string strDescription, int intValue)
        {
            _Description = strDescription;
            _Value = intValue;
        }
    }


    /// <summary>
    /// Gets county list
    /// </summary>
    /// <param name="prefix">Not used</param>
    /// <returns>List of counties (System.String)</returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetCounties(string prefix)
    {
        TList<DropValuesJB> tlstDropValuesJB = DataRepository.DropValuesJBProvider.GetByIntDropValueType(8);

        System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

        ArrayList ary = new ArrayList();

        tlstDropValuesJB.ForEach(delegate(DropValuesJB entDropValuesJB)
        {
            ary.Add(new clsComboValues(entDropValuesJB.StrDropValueDescription, entDropValuesJB.AutoDropValueID));
        });

        string sJSON = oSerializer.Serialize(ary.ToArray());

        return sJSON;
    }

    /// <summary>
    /// Gets list of counties by state ID
    /// </summary>
    /// <param name="stateID">State ID.</param>
    /// <returns>Json List of Counties (System.String)</returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetCountiesV2(string stateID)
    {
        TList<DropValuesCourt> tlstDropValuesCourt = DataRepository.DropValuesCourtProvider.GetByIntDropValueTypeStrDropValueAbbreviation(15, stateID);

        System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

        ArrayList ary = new ArrayList();

        tlstDropValuesCourt.ForEach(delegate(DropValuesCourt entDropValuesCourt)
        {
            int intCountyParent = 0;

            int.TryParse(entDropValuesCourt.IntDropValueParentType.ToString(), out intCountyParent);

            ary.Add(new clsComboValues(entDropValuesCourt.StrDropValueDescription, intCountyParent));
        });

        string sJSON = oSerializer.Serialize(ary.ToArray());
        return sJSON;
    }

    /// <summary>
    /// Retrieves Mock Jury Image Data
    /// </summary>
    /// <param name="countyID">County ID.</param>
    /// <param name="Type">Respondent type.</param>
    /// <returns>List of Mock Jury Images and Data (System.String)</returns>
    [WebMethod()]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string UpdateImages(string countyID, string Type)
    {
        int intCountyID = 0;
        int.TryParse(countyID, out intCountyID);

        string strCountyDesc = string.Empty;

        TList<MockJurors> tlstMockJurors = null;

        if (Type == "2")
        {
            DropValuesJB entDropValuesJB = DataRepository.DropValuesJBProvider.GetByAutoDropValueID(intCountyID);
            TList<ZipCodes> tlstZipCodes = DataRepository.ZipCodesProvider.Find(string.Format("{0} = {1}", ZipCodesColumn.IntZipMSA.ToString(), entDropValuesJB.IntDropValueDefault));

            countyID = string.Empty;

            if (tlstZipCodes != null)
            {
                if (tlstZipCodes.Count > 0)
                    int.TryParse(tlstZipCodes[0].IntZipCountyFIPS.ToString(), out intCountyID);
            }

            tlstMockJurors = DataRepository.MockJurorsProvider.GetByIntJurorCountyID(intCountyID);

            strCountyDesc = string.Format("{0}", tlstMockJurors[0].StrJurorCounty);

        }
        else if (Type == "7")
        {
            TList<ZipCodes> lstZipCodes = DataRepository.ZipCodesProvider.GetByStrZIPCode(countyID);


            if (lstZipCodes != null)
            {
                if (lstZipCodes.Count > 0 )
                {
                    strCountyDesc = string.Format("{0}", lstZipCodes[0].StrZipCounty);

                    int.TryParse(lstZipCodes[0].IntZipCountyFIPS.ToString(), out intCountyID);

                    if (intCountyID > 0)
                    {
                        tlstMockJurors = DataRepository.MockJurorsProvider.GetByIntJurorCountyID(intCountyID);

                        if (tlstMockJurors.Count > 0)
                            strCountyDesc = string.Format("{0}", tlstMockJurors[0].StrJurorCounty);
                    }
                }
            }
        }
        else if (Type == "8")
        {
            tlstMockJurors = DataRepository.MockJurorsProvider.GetByIntJurorCountyID(intCountyID);

            if (tlstMockJurors.Count > 0)
                strCountyDesc = string.Format("{0}", tlstMockJurors[0].StrJurorCounty);

        }

        if (tlstMockJurors != null)
        {
            if (tlstMockJurors.Count > 0)
            {
                List<int> lstJurors = new List<int>();

                if (tlstMockJurors.Count > 12)
                {
                    while (lstJurors.Count < 12)
                    {
                        Random random = new Random();
                        int randomNumber = random.Next(0, tlstMockJurors.Count - 1);

                        if (!lstJurors.Contains(randomNumber))
                        {
                            lstJurors.Add(randomNumber);
                        }
                    }
                }

                StringBuilder sb = new StringBuilder();

                string strMessage = clsPublic.GetProgramSetting("keyMockJurorMessage");

                if (strMessage.Length == 0)
                {
                    strMessage = "<b>A sample of surrogate jurors from this geographic location who have previously participated in our surveys:</b><br/><br/>";
                }

                sb.AppendFormat(strMessage, strCountyDesc);

                sb.AppendLine();

                sb.Append("<ul>");

                foreach(int intJurorItem in lstJurors)
                {
                    MockJurors entMockJurors = tlstMockJurors[intJurorItem];

                    string strPath = System.Web.VirtualPathUtility.ToAbsolute(string.Format("{0}{1}{2}", "~/Research/images/", entMockJurors.StrPicturePath, entMockJurors.StrPictureName));

                    string strImgSource = System.Web.VirtualPathUtility.ToAbsolute("~/Research/frmDisplayThumbnail.aspx");
                    string strImgSource2 = System.Web.VirtualPathUtility.ToAbsolute("~/Research/frmDisplayImage.aspx");

                    string strAge = clsData.GetDropValuesJBDesc((int)entMockJurors.IntJurorAgeRange);
                    string strGender = clsData.GetDropValuesJBDesc((int)entMockJurors.IntJurorGender);
                    string strEducation = clsData.GetDropValuesJBDesc((int)entMockJurors.IntJurorEducation);
                    string strEmploymentStatus = clsData.GetDropValuesJBDesc((int)entMockJurors.IntJurorEmploymentStatus);
                    string strMaritalStatus = clsData.GetDropValuesJBDesc((int)entMockJurors.IntJurorMaritalStatus);
                    string strRace = clsData.GetDropValuesJBDesc((int)entMockJurors.IntJurorRace);

                    string strCombined = string.Format("{0} - {1}<br/>{2}<br/>{3} - {4}<br/>{5} - {6}", strGender, strAge, strMaritalStatus, strEmploymentStatus, entMockJurors.StrJurorTitle, strEducation, strRace);

                    sb.Append("<li>");
                    sb.Append("<div class='divRecord'>");
                    sb.Append("<div class='divImage'>");
                    sb.AppendFormat("<a href='{0}?file={1}' title='{2}' >", strImgSource2, strPath, HttpUtility.HtmlEncode(strCombined));
                    sb.AppendFormat("<img src='{0}?file={1}' alt='' />", strImgSource, strPath);
                    sb.Append("</a>");
                    sb.Append("</div>");        // End Image
                    sb.Append("<div class='divDescription'>");
                    sb.AppendFormat("{0} - {1}<br/>{2}<br/>{3}<br/>{4}<br/>{5}<br/>{6}", strGender, strAge, strMaritalStatus, strEmploymentStatus, entMockJurors.StrJurorTitle, strEducation, strRace);
                    sb.Append("</div>");

                    sb.Append("</div>");

                    sb.Append("</li>");
                }

                sb.Append("</ul>");

                return sb.ToString();
            }
            else
                return "<div id='divNoMockJuror'>No Mock Juror Records Founds...</div>";

        }
        else
            return "<div id='divNoMockJuror'>No Mock Juror Records Founds...</div>";

    }


}
