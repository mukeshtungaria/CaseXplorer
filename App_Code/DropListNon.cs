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
/// Summary description for DropList
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class DropListNon : System.Web.Services.WebService
{

    public DropListNon()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public AjaxControlToolkit.CascadingDropDownNameValue[] GetDropDownContents(string knownCategoryValues, string category)
    {
        StringDictionary kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);

        List<CascadingDropDownNameValue> values = new List<CascadingDropDownNameValue>();

        Int16 intDropValueType = 0;
        Int16 intDropValueParent = 0;


        if (kv.ContainsKey("CASETYPE"))
            Int16.TryParse(kv["CaseType"], out intDropValueParent);

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
            tlstDropValuesJB = DataRepository.DropValuesJBProvider.GetByIntDropValueTypeIntDropValueParentType(intDropValueType, intDropValueParent);

        tlstDropValuesJB.ForEach(delegate(DropValuesJB entDropValuesJB)
        {
            values.Add(new CascadingDropDownNameValue(entDropValuesJB.StrDropValueDescription, entDropValuesJB.AutoDropValueID.ToString()));
        }
        );

        return values.ToArray();
    }


}

