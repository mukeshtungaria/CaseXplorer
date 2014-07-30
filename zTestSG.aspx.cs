using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Net;
using System.Web.UI.WebControls;

public partial class zTestSG : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //        jsLoad.LoadJQuery1_7_2();
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        clsSGizmo clsSGTemp = new clsSGizmo();

        clsSGTemp.TestJavascript();


        //clsSPSS clsSPSSTemp = new clsSPSS();
        //clsSPSSTemp.ExportSPSS_SPSDefinition(279);

//        clsGMIOutput clsGMI = new clsGMIOutput();
//        clsGMI.SubmitSurveyV2(189);

    }

}