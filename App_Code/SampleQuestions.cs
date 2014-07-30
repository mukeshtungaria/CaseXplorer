using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;

using JuryData.Data;
using JuryData.Entities;

/// <summary>
/// Summary description for SampleQuestions
/// </summary>
[WebService(Namespace = "http://dgcc.com/services/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class SampleQuestions : System.Web.Services.WebService
{

    public class SampleQuestionCategory
    {
        public int CategoryID { get; set; }
        public string CategoryDesc { get; set; }
    }



    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public SampleQuestionCategory[] GetSampleQuestionCategories(string strTemp)
    {
        try
        {
            List<SampleQuestionCategory> list = new List<SampleQuestionCategory>();
            TList<DropValuesJB> tlstDropValuesJB = clsData.GetDropValuesJBDataByType(15);

            tlstDropValuesJB.Sort(DropValuesJBColumn.StrDropValueDescription.ToString());

            tlstDropValuesJB.ForEach(delegate(DropValuesJB entDropValuesJB)
            {
                SampleQuestionCategory QuesCat = new SampleQuestionCategory();
                QuesCat.CategoryDesc = entDropValuesJB.StrDropValueDescription;
                QuesCat.CategoryID = entDropValuesJB.AutoDropValueID;

                list.Add(QuesCat);

            });

            return list.ToArray();


        }
        catch (Exception ex)
        {
            clsPublic.LogError(ex);

            List<SampleQuestionCategory> list = new List<SampleQuestionCategory>();
            return list.ToArray();
        }

    }

}
