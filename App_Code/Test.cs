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

using System.Data;
using System.Data.Common;

using JuryData.Entities;
using JuryData.Data;

/// <summary>
/// Summary description for Test
/// </summary>
[WebService(Namespace = "http://dgcc.com/services")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class Test : System.Web.Services.WebService
{

    public class JsonDataTable
    {
        public List<List<object>> aaData;
        public List<JsDataColumn> aoColumns;

        public JsonDataTable()
        {
            aaData = new List<List<object>>();
            aoColumns = new List<JsDataColumn>();
        }

        public void add_Row(List<object> cells)
        {
            this.aaData.Add(cells);
        }

        public class JsDataColumn
        {
            public string sTitle { get; set; }
            public string sClass { get; set; }
        }

        public void add_Column(JsDataColumn col)
        {
            this.aoColumns.Add(col);
        }
    }


    public Test () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }



    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string TestUpdate(int fromID, int toID, int fromPosition, int toPosition, string direction, string group) // int id, int fromPosition, int toPosition, string direction, string group)
    {
        DbCommand cmd = clsUtilities.GetDBCommand("_MoveQuestionV3");

        clsUtilities.AddParameter(ref cmd, "@TargetID", ParameterDirection.Input, DbType.Int32, toID);
        clsUtilities.AddParameter(ref cmd, "@SourceID", ParameterDirection.Input, DbType.Int32, fromID);

        clsUtilities.SQLExecuteNonQuery(cmd);

        return string.Empty;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public JsonDataTable GetQuestionsByResearchID(int intResearchID)
    {
        JsonDataTable jsDT = new JsonDataTable();

        TList<ResearchQuestions> tlstResearchQuestions = DataRepository.ResearchQuestionsProvider.GetByIntQuesResearchID(intResearchID);

        int intOrder = 0;

        tlstResearchQuestions.Sort(ResearchQuestionsColumn.IntQuesOrder.ToString());
        tlstResearchQuestions.ForEach(delegate(ResearchQuestions entResearchQuestions)
        {
            List<object> vl = new List<object>();
            intOrder++;
            vl.Add(intOrder);
            vl.Add(entResearchQuestions.AutoQuesID);
            vl.Add(entResearchQuestions.IntQuesResearchID);
            vl.Add(entResearchQuestions.StrQuesText);

            jsDT.add_Row(vl);
        });


        return jsDT;
    }

    
}
