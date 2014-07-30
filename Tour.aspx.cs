using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Tour : JBBasePageSub
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.PageTitle = "Guided Tour";
            this.PageLabel = "Guided Tour";

            this.LoadPageText(litPageText, 1);
        }

    }
}