using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Disclaimer : JBBasePageSub
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.PageTitle = "Disclaimer";
            this.PageLabel = "Disclaimer";

            this.LoadPageText(litPageText, 1);
        }
    }
}
