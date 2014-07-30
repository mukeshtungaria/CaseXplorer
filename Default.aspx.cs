using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class _Default : JBBasePageSub
{
    protected void Page_Load(object sender, EventArgs e)
    {
	
	
	this.PageTitle = "Home Page";
        this.LoadPageText(lit1, 101);
        this.LoadPageText(lit2, 102);
        this.LoadPageText(lit3, 103);
        this.LoadPageText(lit4, 104);
        this.LoadPageText(lit5, 105);

        this.HideFooterImage = true;

    }
}
