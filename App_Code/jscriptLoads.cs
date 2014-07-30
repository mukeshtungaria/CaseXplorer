using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;


/// <summary>
/// Class for loading javascript include files across application
/// </summary>
public class jsLoad
{
    /// <summary>
    /// 
    /// </summary>
    private static int m_JQueryUICore = 201;
    /// <summary>
    /// 
    /// </summary>
    private static int m_JQueryMouseWheel = 202;
    /// <summary>
    /// 
    /// </summary>
    private static int m_JQueryUIWidget = 203;
    /// <summary>
    /// 
    /// </summary>
    private static int m_JQueryUIButton = 204;
    /// <summary>
    /// 
    /// </summary>
    private static int m_JQueryUISpinner = 205;
    /// <summary>
    /// 
    /// </summary>
    private static int m_JQueryUIPosition = 206;
    /// <summary>
    /// 
    /// </summary>
    private static int m_JQueryDropDown = 207;
    /// <summary>
    /// 
    /// </summary>
    private static int m_JQueryCurrency = 208;
    /// <summary>
    /// 
    /// </summary>
    private static int m_JQueryUIDraggable = 209;
    /// <summary>
    /// 
    /// </summary>
    private static int m_JQueryUIMouse = 210;
    /// <summary>
    /// 
    /// </summary>
    private static int m_JQueryUIResizable = 211;
    /// <summary>
    /// 
    /// </summary>
    private static int m_JQueryUIDialog = 212;
    /// <summary>
    /// 
    /// </summary>
    private static int m_JQueryUIMenu = 213;
    /// <summary>
    /// 
    /// </summary>
    private static int m_JQueryUIAutoComplete = 214;
    /// <summary>
    /// 
    /// </summary>
    private static int m_JQueryUIToolTip = 215;
    /// <summary>
    /// 
    /// </summary>
    private static int m_JQueryUISelectMenu = 216;
    /// <summary>
    /// 
    /// </summary>
    private static int m_JQueryRoundedCorners = 217;
    /// <summary>
    /// 
    /// </summary>
    private static int m_JQueryCollapsible = 218;

    /// <summary>
    /// Load jQuery 1.4.0-min
    /// </summary>
    public static void LoadCurrencyJS()
    {
        Page page = HttpContext.Current.Handler as Page;

        if (page.ClientScript.IsClientScriptIncludeRegistered("JQueryCur") == false)
        {
            string strURL = page.ResolveClientUrl("~/js/jquery.formatCurrency-1.4.0.min.js");

            page.ClientScript.RegisterClientScriptInclude("JQueryCur", page.ResolveUrl(strURL));
        }
    }

    /// <summary>
    /// Loads the Jquery mouse wheel.
    /// </summary>
    public static void LoadJQueryMouseWheel()
    {
        LoadInclude("~/jQueryExternal/jquery.mousewheel-3.0.4.js", m_JQueryMouseWheel);
    }

    /// <summary>
    /// Loads the J query UI widget.
    /// </summary>
    public static void LoadJQueryUIWidget()
    {
        jsLoad.LoadJQueryUICore();
        LoadInclude("~/jQueryUI-1.9m7/jquery.ui.widget.js", m_JQueryUIWidget);
    }

    /// <summary>
    /// Loads the JS order summary.
    /// </summary>
    public static void LoadJSOrderSummary()
    {
        LoadInclude("~/jssecure/jsOrderSummary.js", 99);
    }

    /// <summary>
    /// Loads the J query UI button.
    /// </summary>
    public static void LoadJQueryUIButton()
    {
        jsLoad.LoadJQueryUICore();
        jsLoad.LoadJQueryUIWidget();

        LoadInclude("~/jQueryUI-1.9m7/jquery.ui.button.js", m_JQueryUIButton);
    }

    /// <summary>
    /// Loads the J query UI sortable.
    /// </summary>
    public static void LoadJQueryUISortable()
    {
        jsLoad.LoadJQueryUICore();
        jsLoad.LoadJQueryUIMouse();
        jsLoad.LoadJQueryUIWidget();

        LoadInclude("~/jQueryUI-1.9m7/jquery.ui.sortable.js", m_JQueryUIButton);
    }

    /// <summary>
    /// Loads the J query UI tabs.
    /// </summary>
    public static void LoadJQueryUITabs()
    {
        jsLoad.LoadJQueryUICore();
        jsLoad.LoadJQueryUIWidget();

        LoadInclude("~/jQueryUI-1.9m7/jquery.ui.tabs.js", m_JQueryUIButton);
    }

    /// <summary>
    /// Loads the J query numeric.
    /// </summary>
    public static void LoadJQueryNumeric()
    {
        LoadInclude("~/jQuery/jquery.numeric.js", 99);

    }

    /// <summary>
    /// Loads the J query UI spinner.
    /// </summary>
    public static void LoadJQueryUISpinner()
    {
        jsLoad.LoadJQueryUICore();
        jsLoad.LoadJQueryUIWidget();
        jsLoad.LoadJQueryUIButton();

        LoadInclude("~/jQueryUI-1.9m7/jquery.ui.spinner.js", m_JQueryUISpinner);
    }

    /// <summary>
    /// Loads the spinner.
    /// </summary>
    public static void LoadSpinner()
    {
        LoadInclude("~/jQuery/spin.min.js", 99);
    }

    /// <summary>
    /// Loads the J query spinner.
    /// </summary>
    public static void LoadJQuerySpinner()
    {
        LoadInclude("~/jQuery/jquery.spinner.js", 99);
    }

    /// <summary>
    /// Loads the J query masked input.
    /// </summary>
    public static void LoadJQueryMaskedInput()
    {
        LoadInclude("~/jQuery/jquery.maskedinput-1.3.min.js", 99);
    }

    /// <summary>
    /// Loads the clean word.
    /// </summary>
    public static void LoadCleanWord()
    {
        LoadInclude("~/jQuery/CleanWordHTML.js", 99);
    }

    /// <summary>
    /// Loads the spell check.
    /// </summary>
    public static void LoadSpellCheck()
    {
        LoadInclude("~/SpellCheck/spellV2.js", 99);
    }

    /// <summary>
    /// Loads the nic edit.
    /// </summary>
    public static void LoadNicEdit()
    {
        LoadInclude("~/jQuery/nicEdit.js", 99);
        //LoadInclude("~/jQuery/jquery-impromptu.4.0.min.js", 99);
        //LoadInclude("~/jQuery/jquery-spellchecker.js", 99);
        //LoadInclude("~/jQuery/jquery-impspeller.js", 99);
        //LoadStyleSheet("~/jQuery/screen.css", 50);
        //LoadStyleSheet("~/jQuery/spellchecker.css", 50);
    }

    /// <summary>
    /// Loadjs the HTML.
    /// </summary>
    public static void LoadjHTML()
    {
        LoadInclude("~/js/jHTML/jHtmlArea-0.7.0.js", 99);
        LoadStyleSheet("~/js/jHTML/jHtmlArea.css", 50);

    }

    /// <summary>
    /// Loads the spell check CSS.
    /// </summary>
    public static void LoadSpellCheckCSS()
    {
        LoadStyleSheet("~/SpellCheck/spell.css", 50);
    }

    /// <summary>
    /// Loads the J query UI position.
    /// </summary>
    public static void LoadJQueryUIPosition()
    {
        LoadInclude("~/jQueryUI-1.9m7/jquery.ui.position.js", m_JQueryUIPosition);
    }

    /// <summary>
    /// Loads the J query menu.
    /// </summary>
    public static void LoadJQueryMenu()
    {
        LoadInclude("~/jQueryUI-1.9m7/jquery.ui.menu.js", m_JQueryUIMenu);
    }

    /// <summary>
    /// Loads the J query UI auto complete.
    /// </summary>
    public static void LoadJQueryUIAutoComplete()
    {
        LoadInclude("~/jQueryUI-1.9m7/jquery.ui.autocomplete.js", m_JQueryUIAutoComplete);
    }

    /// <summary>
    /// Loads the J query UI tooltip.
    /// </summary>
    public static void LoadJQueryUITooltip()
    {
        LoadInclude("~/jQueryUI-1.9m7/jquery.ui.tooltip.js", m_JQueryUIToolTip);
    }

    /// <summary>
    /// Loads the J query select menu.
    /// </summary>
    public static void LoadJQuerySelectMenu()
    {
        LoadStyleSheet("~/JQueryUI-themes/base/jquery.ui.selectmenu.css", 50);
        LoadInclude("~/jQueryUI-1.9m7/jquery.ui.selectmenu.js", m_JQueryUISelectMenu);
    }

    /// <summary>
    /// Loads the J query corner.
    /// </summary>
    public static void LoadJQueryCorner()
    {
        LoadInclude("~/jQuery/jquery.corner.js", 100);
    }

    /// <summary>
    /// Loads the J query rounded corners.
    /// </summary>
    public static void LoadJQueryRoundedCorners()
    {
        LoadInclude("~/jQuery/jquery.curvycorners.packed.js", m_JQueryRoundedCorners);

    }

    /// <summary>
    /// Loads the J query collapsible.
    /// </summary>
    public static void LoadJQueryCollapsible()
    {
        LoadInclude("~/jQuery/jquery.collapsible.js", m_JQueryCollapsible);

    }

    /// <summary>
    /// Loads the DQ javascript.
    /// </summary>
    public static void LoadDQJavascript()
    {
        LoadInclude("~/jssecure/dq.js", 0);
    }


    /// <summary>
    /// Loads the J query cascading drop down.
    /// </summary>
    public static void LoadJQueryCascadingDropDown()
    {
        LoadInclude("~/jQuery/jquery.cascadingDropDown.js", m_JQueryCollapsible);
    }

    /// <summary>
    /// Loads the J query drop down.
    /// </summary>
    public static void LoadJQueryDropDown()
    {
        LoadStyleSheet("~/JQueryUI-themes/base/dd.css", 51);
        LoadInclude("~/jQueryUI-1.9m7/jquery.dd.js", m_JQueryDropDown);
    }

    /// <summary>
    /// Loads the data table.
    /// </summary>
    public static void LoadDataTable()
    {
        //        LoadStyleSheet("~/jssecure/demo_page.css", 100);
        LoadStyleSheet("~/jssecure/demo_table.css", 101);
        LoadInclude("~/jssecure/jquery.dataTables.js", 102);
    }

    /// <summary>
    /// Loads the data table row reorder.
    /// </summary>
    public static void LoadDataTableRowReorder()
    {
        LoadInclude("~/jssecure/jquery.dataTables.rowReordering.js", 102);
    }

    /// <summary>
    /// Loads the drag drop.
    /// </summary>
    public static void LoadDragDrop()
    {
        LoadStyleSheet("~/jssecure/tablednd.css", 51);
        LoadInclude("~/jssecure/jquery.tablednd.js", 2);
    }

    /// <summary>
    /// Loads the J query currency.
    /// </summary>
    public static void LoadJQueryCurrency()
    {
        LoadInclude("~/js/jquery.formatCurrency-1.4.0.min.js", m_JQueryCurrency);
    }


    /// <summary>
    /// Loads the style sheet.
    /// </summary>
    /// <param name="strLink">The STR link.</param>
    /// <param name="intAt">The int at.</param>
    public static void LoadStyleSheet(string strLink, int intAt)
    {
        Page page = HttpContext.Current.Handler as Page;

        bool boolExist = false;

        strLink = System.Web.VirtualPathUtility.ToAbsolute(strLink);


        //        strLink = page.ResolveClientUrl(strLink);

        HtmlHead head = (HtmlHead)page.Header;

        foreach (Control ctl in head.Controls)
        {
            string strType = ctl.GetType().ToString();

            if (ctl is HtmlLink)
            {
                HtmlLink linkTemp = (HtmlLink)ctl;

                if (linkTemp.Href == strLink)
                {
                    boolExist = true;
                    break;
                }
            }
        }

        if (!boolExist)
        {
            HtmlLink link = new HtmlLink();

            link.Attributes.Add("href", strLink);
            link.Attributes.Add("type", "text/css");
            link.Attributes.Add("rel", "stylesheet");
            head.Controls.Add(link);
        }



    }


    /// <summary>
    /// Loads the J query NS.
    /// </summary>
    public static void LoadJQueryNS()
    {
        Page page = HttpContext.Current.Handler as Page;

        bool boolExist = false;
        string strLink = page.ResolveClientUrl("~/Styles/redmond/jquery-ui-1.8.16.custom.css");

        HtmlHead head = (HtmlHead)page.Header;

        foreach (Control ctl in head.Controls)
        {
            string strType = ctl.GetType().ToString();

            if (ctl is HtmlLink)
            {
                HtmlLink linkTemp = (HtmlLink)ctl;

                if (linkTemp.Href == strLink)
                {
                    boolExist = true;
                    break;
                }
            }
        }

        if (!boolExist)
        {
            HtmlLink link = new HtmlLink();

            link.Attributes.Add("id", "jQueryUICSS");
            link.Attributes.Add("href", strLink);
            link.Attributes.Add("type", "text/css");
            link.Attributes.Add("rel", "stylesheet");
            head.Controls.Add(link);
        }


        if (page.ClientScript.IsClientScriptIncludeRegistered("JQuery") == false)
        {
            page.ClientScript.RegisterClientScriptInclude("JQuery", page.ResolveUrl("http://ajax.googleapis.com/ajax/libs/jquery/1.6.4/jquery.min.js"));
        }

        if (page.ClientScript.IsClientScriptIncludeRegistered("JQueryUI") == false)
        {
            page.ClientScript.RegisterClientScriptInclude("JQueryUI", page.ResolveUrl("http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.16/jquery-ui.js"));
        }
    }

    /// <summary>
    /// Loads the J query1_7_2x.
    /// </summary>
    public static void LoadJQuery1_7_2x()
    {
        LoadInclude("~/jQuery/jquery-1.7.2.js", 0);
    }

    /// <summary>
    /// Loads the password strength.
    /// </summary>
    public static void LoadPasswordStrength()
    {
        LoadInclude("~/jQuery/pwdmeter.js", 99);
        LoadStyleSheet("~/jQuery/pwdmeter.css", 99);
    }

    /// <summary>
    /// Loads the JS new user.
    /// </summary>
    public static void LoadJSNewUser()
    {
        LoadInclude("~/jssecure/jsNewUser.js", 99);
    }

    /// <summary>
    /// Loads the J query validation.
    /// </summary>
    public static void LoadJQueryValidation()
    {
        LoadInclude("~/jQuery/jquery.metadata.js", 99);
        LoadInclude("~/jQuery/jquery.validate.js", 99);
    }

    /// <summary>
    /// Loads the form script.
    /// </summary>
    public static void LoadFormScript()
    {
        LoadInclude("~/jquery/form.js", 0);
        LoadStyleSheet("~/jquery/formstyle.css", 0);
    }

    /// <summary>
    /// Loads the Jquery G map.
    /// </summary>
    public static void LoadJQueryGMap()
    {
        //        LoadJQuery1_7_2();
        LoadInclude("https://maps.google.com/maps/api/js?sensor=false", 0);
        LoadInclude("~/js/ctlGoogleMap.js", 0);
        LoadStyleSheet("~/css/ctlGoogleMap.css", 0);
        LoadInclude("~/jQuery/gmap3.js", 0);
    }

    /// <summary>
    /// Loads Venue Selection DropDown
    /// </summary>
    public static void LoadVenueSelectDropDown()
    {
        LoadInclude("~/js/jsEditVenues.js",0);

    }

    /// <summary>
    /// Loads the include.
    /// </summary>
    /// <param name="strURL">The STR URL.</param>
    /// <param name="intAddAt">The int add at.</param>
    public static void LoadInclude(string strURL, int intAddAt)
    {

        Page page = HttpContext.Current.Handler as Page;

        bool boolExist = false;
        strURL = page.ResolveClientUrl(strURL);

        HtmlHead head = (HtmlHead)page.Header;

        foreach (Control ctl in head.Controls)
        {
            string strType = ctl.GetType().ToString();

            if (ctl is HtmlGenericControl)
            {
                HtmlGenericControl linkTemp = (HtmlGenericControl)ctl;

                if (linkTemp.Attributes["src"].ToUpper() == strURL.ToUpper())
                {
                    boolExist = true;
                    break;
                }
            }
        }

        if (!boolExist)
        {
            HtmlGenericControl Include = new HtmlGenericControl("script");
            Include.Attributes.Add("type", "text/javascript");
            Include.Attributes.Add("src", strURL);

            if (intAddAt >= 0)
            {
                page.Header.Controls.AddAt(page.Header.Controls.Count, Include);
            }
            else
            {
                page.Header.Controls.Add(Include);
            }
        }
    }

    /// <summary>
    /// Loads the J query UI core.
    /// </summary>
    public static void LoadJQueryUICore()
    {
        LoadStyleSheet("~/Styles/redmond/jquery-ui-1.8.16.custom.css", 9);
        LoadInclude("~/jQueryUI-1.9m7/jquery.ui.core.js", 1);
    }

    /// <summary>
    /// Loads the J query UI.
    /// </summary>
    public static void LoadJQueryUI()
    {
        //        LoadStyleSheet("~/JQueryUI-themes/base/jquery.ui.all.css", 9);
        LoadInclude("~/jQueryUI-1.9m7/jquery.ui.js", 2);
    }

    /// <summary>
    /// Loads the J query UI dialog.
    /// </summary>
    public static void LoadJQueryUIDialog()
    {
        //        jsLoad.LoadJQuery1_7_2();
        jsLoad.LoadJQueryUICore();
        jsLoad.LoadJQueryUIWidget();
        jsLoad.LoadJQueryUIButton();
        jsLoad.LoadJQueryUIPosition();
        jsLoad.LoadJQueryUIMouse();
        jsLoad.LoadJQueryUIDraggable();
        jsLoad.LoadJQueryUIResizable();

        LoadInclude("~/jQueryUI-1.9m7/jquery.ui.dialog.js", m_JQueryUIDialog);
    }

    /// <summary>
    /// Loads the J query UI mouse.
    /// </summary>
    public static void LoadJQueryUIMouse()
    {
        LoadJQueryUIWidget();
        LoadInclude("~/jQueryUI-1.9m7/jquery.ui.mouse.js", m_JQueryUIMouse);
    }

    /// <summary>
    /// Loads the J query UI draggable.
    /// </summary>
    public static void LoadJQueryUIDraggable()
    {
        LoadInclude("~/jQueryUI-1.9m7/jquery.ui.draggable.js", m_JQueryUIDraggable);
    }

    /// <summary>
    /// Loads the J query UI resizable.
    /// </summary>
    public static void LoadJQueryUIResizable()
    {
        LoadInclude("~/jQueryUI-1.9m7/jquery.ui.resizable.js", m_JQueryUIResizable);
    }

    /// <summary>
    /// Loads the J query.
    /// </summary>
    public static void LoadJQuery()
    {
        Page page = HttpContext.Current.Handler as Page;

        if (page.ClientScript.IsClientScriptIncludeRegistered("JQuery") == false)
        {
            page.ClientScript.RegisterClientScriptInclude("JQuery", page.ResolveUrl("https://ajax.googleapis.com/ajax/libs/jquery/1.6.4/jquery.min.js"));
        }

        if (page.ClientScript.IsClientScriptIncludeRegistered("JQueryUI") == false)
        {
            page.ClientScript.RegisterClientScriptInclude("JQueryUI", page.ResolveUrl("https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.16/jquery-ui.js"));
        }

        bool boolExist = false;
        string strLink = page.ResolveClientUrl("~/Styles/redmond/jquery-ui-1.8.16.custom.css");

        HtmlHead head = (HtmlHead)page.Header;

        foreach (Control ctl in head.Controls)
        {
            string strType = ctl.GetType().ToString();

            if (ctl is HtmlLink)
            {
                HtmlLink linkTemp = (HtmlLink)ctl;

                if (linkTemp.Href == strLink)
                {
                    boolExist = true;
                    break;
                }
            }
        }

        if (!boolExist)
        {
            HtmlLink link = new HtmlLink();

            link.Attributes.Add("id", "jQueryUICSS");
            link.Attributes.Add("href", strLink);
            link.Attributes.Add("type", "text/css");
            link.Attributes.Add("rel", "stylesheet");
            head.Controls.Add(link);
        }

    }

    /// <summary>
    /// Loads the light box.
    /// </summary>
    public static void LoadLightBox()
    {
        Page page = HttpContext.Current.Handler as Page;

        HtmlLink link = new HtmlLink();
        page.Header.Controls.Add(link);
        link.EnableViewState = false;
        link.Attributes.Add("type", "text/css");
        link.Attributes.Add("rel", "stylesheet");
        link.Href = "~/research/css/jquery.lightbox-0.5.css";

        page.ClientScript.RegisterClientScriptInclude("LightBox", System.Web.VirtualPathUtility.ToAbsolute("~/research/js/jquery.lightbox-0.5.js"));
    }

    /// <summary>
    /// Loads the light box NS.
    /// </summary>
    public static void LoadLightBoxNS()
    {
        Page page = HttpContext.Current.Handler as Page;

        HtmlLink link = new HtmlLink();
        page.Header.Controls.Add(link);
        link.EnableViewState = false;
        link.Attributes.Add("type", "text/css");
        link.Attributes.Add("rel", "stylesheet");
        link.Href = "~/research/css/jquery.lightbox-0.5.css";

        page.ClientScript.RegisterClientScriptInclude("LightBox", System.Web.VirtualPathUtility.ToAbsolute("~/js/jquery.lightbox-0.5.js"));
    }




}