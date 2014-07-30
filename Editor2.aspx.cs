using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using JuryData.Data;
using JuryData.Entities;


public partial class Editor2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            jsLoad.LoadSpellCheck();
            jsLoad.LoadJQuery1_7_2x();
            jsLoad.LoadCleanWord();

            txtQues.Height = new Unit(150);

            if (Page.Request["QuesID"] != null)
            {
                int intQuestionID = 0;

                int.TryParse(Page.Request["QuesID"].ToString(), out intQuestionID);

                ResearchQuestions entResearchQuestions = DataRepository.ResearchQuestionsProvider.GetByAutoQuesID(intQuestionID);

                if (entResearchQuestions != null)
                {
                    txtQues.Content = entResearchQuestions.StrQuesText;
                }
            }

            if (Page.Request["Height"] != null)
            {
                int intHeight = 150;

                int.TryParse(Page.Request["Height"].ToString(), out intHeight);

                if (intHeight <= 0) intHeight = 150;

                txtQues.Height = intHeight;

            }
        }
    }
}