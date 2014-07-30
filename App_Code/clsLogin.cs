// ***********************************************************************
// Author           : Dennis Sebenick
// Created          : 08-29-2011
//
// Last Modified By : dennis
// Last Modified On : 08-29-2011
// ***********************************************************************
// <copyright file="clsLogin.cs" company="DGCC.COM">
//     . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
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
/// Login functions
/// </summary>
public class clsLogin
{
    /// <summary>
    /// Initializes a new instance of the <see cref="clsLogin" /> class.
    /// </summary>
    public clsLogin()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    /// <summary>
    /// Check if user is logged in
    /// </summary>
    /// <returns><c>true</c> if [is logged in]; otherwise, <c>false</c>.</returns>
    public static bool IsLoggedIn()
    {
        if (GetMembershipUserID().Length == 0)
            return false;
        else
            return true;
    }

    /// <summary>
    /// Gets the current user membership ID
    /// </summary>
    /// <returns>Membership ID (System.String)</returns>
    public static string GetMembershipUserID()
    {
        System.Web.HttpContext objHttp = System.Web.HttpContext.Current;

        if (objHttp.User.Identity.IsAuthenticated == true)
        {
            MembershipUser MemUser = Membership.GetUser(objHttp.User.Identity.Name);

            if (MemUser != null)
                return MemUser.ProviderUserKey.ToString();
            else
                return "";
        }
        else
            return "";
    }

    /// <summary>
    /// Redirects user to Research Home Page
    /// </summary>
    public static void GotoResearchHome()
    {
        System.Web.HttpResponse objResp = System.Web.HttpContext.Current.Response;
        objResp.Redirect("~/Research/ResearchHome.aspx");
    }

}
