using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class confidentiality : JBBasePageSub 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.PageTitle = "About Jury Research";
            this.PageLabel = "About Jury Research";

            this.LoadPageText(litTop, 1);
        }
    }
}