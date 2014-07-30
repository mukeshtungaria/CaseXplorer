// ***********************************************************************
// Author           : Dennis Sebenick
// Created          : 04-01-2012
//
// Last Modified By : dennis
// Last Modified On : 04-01-2012
// ***********************************************************************
// <copyright file="wsPricingCalc.cs" company="DGCC.COM">
//     . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;


/// <summary>
/// Pricing Calculator Service Functions
/// </summary>
[WebService(Namespace = "https://www.casexplorer.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class wsPricingCalc : System.Web.Services.WebService {

    /// <summary>
    /// Initializes a new instance of the <see cref="wsPricingCalc" /> class.
    /// </summary>
    public wsPricingCalc () {
    }

    /// <summary>
    /// Calculates the price.
    /// </summary>
    /// <returns>System.String.</returns>
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string CalculatePrice() {
        return "Hello World";
    }
    
}
