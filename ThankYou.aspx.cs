using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ThankYou : JBBasePageSub
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.PageTitle = "Thank You";
            this.PageLabel = "Thank You";

            this.LoadPageText(litPageText, 1);
        }

    }
}
