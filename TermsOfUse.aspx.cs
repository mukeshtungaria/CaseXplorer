using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TermsOfUse : JBBasePageSub
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.PageTitle = "Terms of Use";
            this.PageLabel = "Terms of Use";

            this.LoadPageText(litPageText, 1);
        }
    }
}
