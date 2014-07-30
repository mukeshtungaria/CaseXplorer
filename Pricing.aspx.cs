using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class pricing : JBBasePageSub 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.PageTitle = "Pricing";
            this.PageLabel = "Pricing";

            //this.ctlPricingDesc1.Expanded = true;
            //this.ctlPricingDesc1.HideHeader = true;

            this.LoadPageText(litTop, 1);
            this.LoadPageText(litBottom, 2);

            ctlPricingDescV21.HideHeader = true;
            ctlPricingDescV21.Expanded = true;
            ctlPricingDescV21.ShowStaticHeader = true;

        }
    }
}