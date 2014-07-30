// ***********************************************************************
// Author           : Dennis Sebenick
// Created          : 08-29-2011
//
// Last Modified By : Dennis Sebenick
// Last Modified On : 09-19-2012
// ***********************************************************************
// <copyright file="BasePage.cs" company="DGCC.COM">
//     . All rights reserved.
// </copyright>
// <summary>
//   Base page functions 
// </summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Base Master page object
/// </summary>
public class JBBasePage : Page
{
    /// <summary>
    /// Initializes a new instance of the <see cref="JBBasePage" /> class.
    /// </summary>
    public JBBasePage()
    {
        this.PreInit += new EventHandler(JBBasePage_PreInit);
    }

    /// <summary>
    /// Handles the PreInit event of the JBBasePage control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
    void JBBasePage_PreInit(object sender, EventArgs e)
    {
        string strTheme = clsPublic.GetProgramSetting("keyTheme");

        if (strTheme != null)
        {
            if (strTheme.Length > 0)
                this.Page.Theme = strTheme;
            else
                this.Page.Theme = "Default";
        }
        else
            this.Page.Theme = "Default";

    }

    /// <summary>
    /// Gets or sets the page title.
    /// </summary>
    /// <value>The page title.</value>
    public string PageTitle
    {
        set
        {
            BaseMasterPage mp = (BaseMasterPage)this.Page.Master;
            mp.PageTitle = string.Format("{0} - {1}", clsPublic.SiteName, value);
        }
        get
        {
            BaseMasterPage mp = (BaseMasterPage)this.Page.Master;
            return mp.PageTitle;
        }
    }



    /// <summary>
    /// Gets the name of the site.
    /// </summary>
    /// <value>The name of the site.</value>
    public string SiteName
    {
        get { return clsPublic.SiteName; }
    }


    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.TemplateControl.Error" /> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
    protected override void OnError(EventArgs e)
    {
        Session["CurrentError"] = null;


        HttpContext ctx = HttpContext.Current;

        Exception ex = ctx.Server.GetLastError();

        if (ex != null)
        {
//            clsPublic.LogError(ex, string.Format("Unhandled Error:  {0}", ex.Source));
            Session["CurrentError"] = ex;
        }

        base.OnError(e);

        int intIndex = ex.Message.IndexOf("potentially dangerous Request");

        if (intIndex > 0)
        {
            Response.Redirect("~/Error.aspx?ErrorID=1000");
        }
        else
        {
            Response.Redirect("~/Error.aspx?ErrorID=1001");
        }
    }

    /// <summary>
    /// Gets the name of the current page
    /// </summary>
    /// <returns>System.String.</returns>
    protected string PageName()
    {
        return clsPublic.GetCurrentPageName();
    }

    /// <summary>
    /// Loads the page text.
    /// </summary>
    /// <param name="litControl">ASP.NET Literal control to be populated with text from database</param>
    /// <param name="intIndex">Which text record to choose from database</param>
    protected void LoadPageText(Literal litControl, int intIndex)
    {
        string strPageTitle = string.Empty;
        string strPageLabel = string.Empty;

        string strPageText = clsPublic.PageText(this.PageName().ToUpper(), intIndex, ref strPageTitle, ref strPageLabel);

        string strPageText2 = strPageText.Replace("URL~/", this.ResolveUrl("~"));

        litControl.Text = strPageText2;

        if (strPageTitle.Length > 0)
            this.PageTitle = strPageTitle;

        BaseMasterPage mp = (BaseMasterPage)this.Page.Master;

        clsPublic.LoadPageMetaData(mp, this.PageName().ToUpper());

    }
}



/// <summary>
/// Base page for sub master pages
/// </summary>
public class JBBasePageSub : Page
{
    /// <summary>
    /// Initializes a new instance of the <see cref="JBBasePageSub" /> class.
    /// </summary>
    public JBBasePageSub()
    {
        //this.PreInit += new EventHandler(JBBasePage_PreInit);
    }

    /// <summary>
    /// Handles the PreInit event of the JBBasePage control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
    void JBBasePage_PreInit(object sender, EventArgs e)
    {
        string strTheme = clsPublic.GetProgramSetting("keyTheme");

        if (strTheme != null)
        {
            if (strTheme.Length > 0)
            {

                this.Page.Theme = strTheme;

            }
            else
                this.Page.Theme = "Default";
        }
        else
            this.Page.Theme = "Default";

    }

    /// <summary>
    /// Adds the meta tag.
    /// </summary>
    /// <param name="strKeyWord">The STR key word.</param>
    /// <param name="strContent">Content of the STR.</param>
    public void AddMetaTag(string strKeyWord, string strContent)
    {




    }

    /// <summary>
    /// Sets the top image.
    /// </summary>
    /// <param name="intWhich">The int which.</param>
    public void SetTopImage(int intWhich)
    {
        BaseMasterPageSub mp = (BaseMasterPageSub)this.Page.Master;
        mp.ImageHeader = intWhich;
    }

    /// <summary>
    /// Hides footer image
    /// </summary>
    /// <value>True hides image</value>
    public bool HideFooterImage
    {
        set
        {
            BaseMasterPageSub mp = (BaseMasterPageSub)this.Page.Master;
            mp.HideFooterImage = value;
        }
    }

    /// <summary>
    /// Gets or sets the page title.
    /// </summary>
    /// <value>The page title.</value>
    public string PageTitle
    {
        set
        {
            BaseMasterPageSub mp = (BaseMasterPageSub)this.Page.Master;
            mp.PageTitle = string.Format("{0} - {1}", clsPublic.SiteName, value);
        }
        get
        {
            BaseMasterPageSub mp = (BaseMasterPageSub)this.Page.Master;
            return mp.PageTitle;
        }
    }

    /// <summary>
    /// Sets the page slogan.
    /// </summary>
    /// <value>The page slogan.</value>
    public string PageSlogan
    {
        set
        {
            BaseMasterPageSub mp = (BaseMasterPageSub)this.Page.Master;
            mp.PageSlogan = value;
        }
    }

    /// <summary>
    /// Gets or sets the page label.
    /// </summary>
    /// <value>The page label.</value>
    public string PageLabel
    {
        set
        {
            BaseMasterPageSub mp = (BaseMasterPageSub)this.Page.Master;
            mp.PageLabel = value;
        }
        get
        {
            BaseMasterPageSub mp = (BaseMasterPageSub)this.Page.Master;
            return mp.PageLabel;
        }
    }

    /// <summary>
    /// Gets the name of the site.
    /// </summary>
    /// <value>The name of the site.</value>
    public string SiteName
    {
        get { return clsPublic.SiteName; }
    }


    /// <summary>
    /// Pages the name.
    /// </summary>
    /// <returns>System.String.</returns>
    protected string PageName()
    {
        return clsPublic.GetCurrentPageName();
    }

    /// <summary>
    /// Loads the page slogan.
    /// </summary>
    protected void LoadPageSlogan()
    {
        string strPageTitle = string.Empty;
        string strPageLabel = string.Empty;

        string strPageText = clsPublic.PageText(this.PageName().ToUpper(), 200, ref strPageTitle, ref strPageLabel);

        string strPageText2 = strPageText.Replace("URL~/", this.ResolveUrl("~"));

        this.PageSlogan = strPageText2;

    }


    /// <summary>
    /// Loads the page text.
    /// </summary>
    /// <param name="litControl">ASP.NET Literal control to be populated with text from database</param>
    /// <param name="intIndex">Which text record to choose from database</param>
    protected void LoadPageText(Literal litControl, int intIndex)
    {
        string strPageTitle = string.Empty;
        string strPageLabel = string.Empty;

        string strPageText = clsPublic.PageText(this.PageName().ToUpper(), intIndex, ref strPageTitle, ref strPageLabel);

        if (strPageText != null)
        {
            string strPageText2 = strPageText.Replace("URL~/", this.ResolveUrl("~"));

            litControl.Text = strPageText2;
        }

        if (strPageTitle.Length > 0)
            this.PageTitle = strPageTitle;

        if (strPageLabel.Length > 0)
            this.PageLabel = strPageLabel;

        BaseMasterPageSub mp = (BaseMasterPageSub)this.Page.Master;

        clsPublic.LoadPageMetaData(mp, this.PageName().ToUpper());


    }


    /// <summary>
    /// Unhandled error event.
    /// </summary>
    /// <param name="e">Error event argument</param>
    protected override void OnError(EventArgs e)
    {
        Session["CurrentError"] = null;

        HttpContext ctx = HttpContext.Current;

        Exception ex = ctx.Server.GetLastError();

        if (ex != null)
        {
            //            clsPublic.LogError(ex, string.Format("Unhandled Error:  {0}", ex.Source));
            Session["CurrentError"] = ex;
        }

        base.OnError(e);

        int intIndex = ex.Message.IndexOf("potentially dangerous Request");

        if (intIndex > 0)
        {
            Response.Redirect("~/Error.aspx?ErrorID=1000");
        }
        else
        {
            Response.Redirect("~/Error.aspx?ErrorID=1001");
        }
    }
}


/// <summary>
/// Class BaseMasterPageSub
/// </summary>
public class BaseMasterPageSub : System.Web.UI.MasterPage
{
    /// <summary>
    /// Pages the name.
    /// </summary>
    /// <returns>System.String.</returns>
    protected string PageName()
    {
        return clsPublic.GetCurrentPageName();
    }

    /// <summary>
    /// Loads the page slogan.
    /// </summary>
    protected void LoadPageSlogan()
    {
        string strPageTitle = string.Empty;
        string strPageLabel = string.Empty;

        string strPageText = clsPublic.PageText(this.PageName().ToUpper(), 200, ref strPageTitle, ref strPageLabel);

        string strPageText2 = strPageText.Replace("URL~/", this.ResolveUrl("~"));

        this.PageSlogan = strPageText2;

    }

    /// <summary>
    /// Sets a value indicating whether [hide footer image].
    /// </summary>
    /// <value><c>true</c> if [hide footer image]; otherwise, <c>false</c>.</value>
    public bool HideFooterImage
    {
        set
        {
            Image img = (Image)this.FindControl("imgFooter");

            if (img != null)
            {
                img.Visible = false;
            }
        }

    }


    /// <summary>
    /// Gets or sets the page title.
    /// </summary>
    /// <value>The page title.</value>
    public string PageTitle
    {
        set { this.Page.Title = value; }
        get { return this.Page.Title; }
    }

    /// <summary>
    /// 
    /// </summary>
    private string m_strPageSlogan = string.Empty;

    /// <summary>
    /// Gets or sets the page slogan.
    /// </summary>
    /// <value>The page slogan.</value>
    public string PageSlogan
    {
        set
        {
            m_strPageSlogan = value;
        }
        get
        {
            return m_strPageSlogan;
        }


    }


    /// <summary>
    /// Adds the meta data.
    /// </summary>
    /// <param name="strMetaKeyWord">The STR meta key word.</param>
    /// <param name="strMetaContent">Content of the STR meta.</param>
    public void AddMetaData(string strMetaKeyWord, string strMetaContent)
    {
        Control ctl = this.FindControl("Head1");

        if (ctl != null)
        {
            System.Web.UI.HtmlControls.HtmlHead head = (System.Web.UI.HtmlControls.HtmlHead)ctl;

            System.Web.UI.HtmlControls.HtmlMeta meta = new System.Web.UI.HtmlControls.HtmlMeta();

            //Specify meta attributes
            meta.Attributes.Add("name", strMetaKeyWord);
            meta.Attributes.Add("content", strMetaContent);


            // Add the meta object to the htmlhead's control collection
            head.Controls.AddAt(0, meta);
        }
    }

    /// <summary>
    /// Sets the image header.
    /// </summary>
    /// <value>The image header.</value>
    public virtual int ImageHeader
    {
        set
        {

        }
    }

    /// <summary>
    /// Gets or sets the page label.
    /// </summary>
    /// <value>The page label.</value>
    public virtual string PageLabel
    {
        set 
        {

        }
        get 
        {
            return string.Empty;
        }
    }

}


/// <summary>
/// Class BaseMasterPage
/// </summary>
public class BaseMasterPage : System.Web.UI.MasterPage
{
    /// <summary>
    /// Gets or sets the page title.
    /// </summary>
    /// <value>The page title.</value>
    public string PageTitle
    {
        set { this.Page.Title = value; }
        get { return this.Page.Title; }
    }

    /// <summary>
    /// Adds the meta data.
    /// </summary>
    /// <param name="strMetaKeyWord">Keywords to add</param>
    /// <param name="strMetaContent">Metadata content.</param>
    public void AddMetaData(string strMetaKeyWord, string strMetaContent)
    {
        Control ctl = this.FindControl("Head1");

        if (ctl != null)
        {
            System.Web.UI.HtmlControls.HtmlHead head = (System.Web.UI.HtmlControls.HtmlHead)ctl;

            System.Web.UI.HtmlControls.HtmlMeta meta = new System.Web.UI.HtmlControls.HtmlMeta();

            //Specify meta attributes
            meta.Attributes.Add("name", strMetaKeyWord);
            meta.Attributes.Add("content", strMetaContent);


            // Add the meta object to the htmlhead's control collection
            head.Controls.AddAt(0, meta);
        }
    }
}