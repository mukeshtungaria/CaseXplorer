using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TestSampleQuestions : JBBasePageSub
{
    protected void Page_Load(object sender, EventArgs e)
    {
//        jsLoad.LoadJQuery1_7_2();
        if (!Page.IsPostBack)
        {
            jsLoad.LoadJQueryUIDialog();
        }
    }
}