using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using JuryData.Data;
using JuryData.Entities;

using FreeTextBoxControls;


public partial class Editor3 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            jsLoad.LoadSpellCheck();
            jsLoad.LoadJQuery1_7_2x();
            jsLoad.LoadCleanWord();

            System.Web.HttpBrowserCapabilities browser = Request.Browser;

            string str = browser.Type;

            Toolbar tb1 = new Toolbar();
            tb1.Items.Add(new FontSizesMenu());
            tb1.Items.Add(new FontForeColorsMenu());
            tb1.Items.Add(new Bold());
            tb1.Items.Add(new Italic());
            tb1.Items.Add(new Underline());
            tb1.Items.Add(new SuperScript());
            tb1.Items.Add(new SubScript());
            tb1.Items.Add(new RemoveFormat());

            Toolbar tb2 = new Toolbar();
            tb2.Items.Add(new JustifyLeft());
            tb2.Items.Add(new JustifyRight());
            tb2.Items.Add(new JustifyCenter());
            tb2.Items.Add(new JustifyFull());

            Toolbar tb3 = new Toolbar();
            tb3.Items.Add(new FreeTextBoxControls.BulletedList());
            tb3.Items.Add(new NumberedList());
            tb3.Items.Add(new Indent());
            tb3.Items.Add(new Outdent());

            Toolbar tb4 = new Toolbar();
            tb4.Items.Add(new Cut());
            tb4.Items.Add(new Copy());
            tb4.Items.Add(new Paste());
            tb4.Items.Add(new Delete());
            tb4.Items.Add(new Undo());
            tb4.Items.Add(new Redo());
            tb4.Items.Add(new Print());
            tb4.Items.Add(new Preview());

            if (browser.Browser.ToUpper() == "IE" || browser.Browser.ToUpper() == "CHROME")
                tb4.Items.Add(new FreeTextBoxControls.NetSpell());

            Toolbar tb5 = new Toolbar();
            tb5.Items.Add(new SymbolsMenu());
            tb5.Items.Add(new InsertRule());
            tb5.Items.Add(new SelectAll());


            //txtQues.Toolbars.Add(tb1);
            //txtQues.Toolbars.Add(tb2);
            //txtQues.Toolbars.Add(tb3);
            //txtQues.Toolbars.Add(tb4);
            //txtQues.Toolbars.Add(tb5);

            if (Page.Request["QuesID"] != null)
            {
                int intQuestionID = 0;

                int.TryParse(Page.Request["QuesID"].ToString(), out intQuestionID);

                if (intQuestionID > 0)
                {
                    ResearchQuestions entResearchQuestions = DataRepository.ResearchQuestionsProvider.GetByAutoQuesID(intQuestionID);

                    if (entResearchQuestions != null)
                    {
                        txtQues.Content = entResearchQuestions.StrQuesText;
                    }
                }
                else
                {
                    txtQues.Content = string.Empty;
                }
            }
        }
    }
}