using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TestEditor : JBBasePageSub
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            jsLoad.LoadSpellCheck();
//            jsLoad.LoadJQuery1_7_2x();
            jsLoad.LoadCleanWord();
        }

    }
}