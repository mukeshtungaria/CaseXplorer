using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for MasterPageModule
/// </summary>
public class MasterPageModule : IHttpModule
{
    public void Init(HttpApplication context)
    {
        context.PreRequestHandlerExecute += new EventHandler(context_PreRequestHandlerExecute);
    }

    void context_PreRequestHandlerExecute(object sender, EventArgs e)
    {
        Page page = HttpContext.Current.CurrentHandler as Page;
        if (page != null)
        {
            page.PreInit += new EventHandler(page_PreInit);
        }
    }

    void page_PreInit(object sender, EventArgs e)
    {
        Page page = sender as Page;

        if (page.Master != null)
        {
            string str = page.Master.GetType().ToString();

            if (page != null && str != "ASP.masterpages_testmaster_master")
            {
                bool boolIsSub = false;

                if (str.ToUpper().IndexOf("DQV3SUB") >= 0)
                    boolIsSub = true;

                string strMasterPage = string.Empty;

                if (boolIsSub)
                    strMasterPage = clsPublic.GetProgramSetting("keyMasterPageSub");
                else
                    strMasterPage = clsPublic.GetProgramSetting("keyMasterPage");

                if (strMasterPage != null)
                {
                    if (strMasterPage.Length > 0)
                    {
                        page.MasterPageFile = strMasterPage;
                    }
                }

            }
        }
    }

    public void Dispose()
    {
    }

}
