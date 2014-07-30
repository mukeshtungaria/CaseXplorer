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

public partial class Progress : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string script = string.Format(@"function GetRadWindow()  
            {{              
                if (window.radWindow) return window.radWindow;  
                else if (window.frameElement.radWindow) return window.frameElement.radWindow;  
                return null;  
            }}  
            function RegisterProgressArea()  
            {{  
                var radWindow = GetRadWindow();  
                if (radWindow != null)  
                {{             
                    var progressAreasArray = radWindow.BrowserWindow.Telerik.Web.UI.ProgressAreas;  
                    progressAreasArray[progressAreasArray.length] = $find('{0}');  
                }}}}", RadProgressArea1.ClientID);
        Page.ClientScript.RegisterStartupScript(this.GetType(), "RegisterFunctions", script, true);
        Page.ClientScript.RegisterStartupScript(this.GetType(), "RegisterProgressArea", "Sys.Application.add_load(function(){ RegisterProgressArea();});", true);   
  

    }
}
