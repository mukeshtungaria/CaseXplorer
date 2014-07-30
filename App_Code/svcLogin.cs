// ***********************************************************************
// Assembly         : C:\juryresearch\
// Author           : dennis
// Created          : 06-23-2012
//
// Last Modified By : dennis
// Last Modified On : 06-24-2012
// ***********************************************************************
// <copyright file="svcLogin.cs" company="">
//     . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.Script.Services;
using System.Runtime.Serialization.Json;
using System.Web.Services.Protocols;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Net.Mail;
using JuryData.Data;
using JuryData.Entities;


/// <summary>
/// Login Service Functions
/// </summary>
[WebService(Namespace = "http://dgcc.com/services")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class svcLogin : System.Web.Services.WebService
{

    /// <summary>
    /// Initializes a new instance of the <see cref="svcLogin" /> class.
    /// </summary>
    public svcLogin () {
    }

    /// <summary>
    /// Recover account by email
    /// </summary>
    /// <param name="strEmail">Email address</param>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    [SoapDocumentMethod(OneWay = true)]
    public void RecoverAccount(string strEmail) {
        string strUserName = Membership.GetUserNameByEmail(strEmail);

        if (strUserName != null)
        {
            if (strUserName.Length > 0)
            {
                clsPublic.SendEmail(clsPublic.FormatHTMLEmail("keyRecoverAccount", strEmail, strUserName));
            }
        }
    }
  
}
