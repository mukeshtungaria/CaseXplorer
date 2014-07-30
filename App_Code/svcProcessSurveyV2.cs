// ***********************************************************************
// Author           : Dennis Sebenick
// Created          : 06-27-2012
//
// Last Modified By : dennis
// Last Modified On : 07-25-2012
// ***********************************************************************
// <copyright file="svcProcessSurveyV2.cs" company="DGCC.COM">
//     . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Threading;

/// <summary>
/// Survey Process Functions
/// </summary>
[WebService(Namespace = "https://www.casexplorer.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class svcProcessSurveyV2 : System.Web.Services.WebService {

    /// <summary>
    /// Initializes a new instance of the <see cref="svcProcessSurveyV2" /> class.
    /// </summary>
    public svcProcessSurveyV2 () {
    }


    /// <summary>
    /// Uploads the survey to SG.
    /// -- One way call to return control back to system immediately
    /// </summary>
    /// <param name="intResearchID">Research ID.</param>
    [WebMethod]
    [SoapDocumentMethod(OneWay = true)]
    public void UploadSurveyToSG(int intResearchID)
    {
        clsPublic.SendMessage(string.Format("Web Service - Research ID: {0}", intResearchID));

        clsSGizmo clsSGGizmoTemp = new clsSGizmo();

        clsSGGizmoTemp.UploadSurveyFromResearchID(intResearchID, false);
        clsGMIOutput clsGMIOutputTemp = new clsGMIOutput();
        clsGMIOutputTemp.SubmitSurveyV2(intResearchID);
    }

    /// <summary>
    /// Uploads a test survey to SurveyGizmo.
    /// </summary>
    /// <param name="intResearchID">Research ID.</param>
    [WebMethod]
    [SoapDocumentMethod(OneWay = true)]
    public void UploadTestSurvyToSG(int intResearchID)
    {

        //clsPublic.SendMessage(string.Format("Test Survey Uploading - Research ID: {0}", intResearchID));

        //clsSGizmo clsSGGizmoTemp = new clsSGizmo();

        //clsSGGizmoTemp.UploadSurveyFromResearchID(intResearchID, true);

        //clsGMIOutput clsGMIOutputTemp = new clsGMIOutput();

        //clsGMIOutputTemp.SubmitSurveyV2(intResearchID);


    }


    /// <summary>
    /// Imports the SG data.
    /// </summary>
    /// <param name="intSGID">The int SGID.</param>
    [WebMethod]
    [SoapDocumentMethod(OneWay = true)]
    public void ImportSGData(int intSGID)
    {


    }

    /// <summary>
    /// Checks SurveyGizmo data for results on open surveys
    /// </summary>
    [WebMethod]
    [SoapDocumentMethod(OneWay = true)]
    public void CheckSGData()
    {
        try
        {
            //clsSGizmo clsSGGizmoTemp = new clsSGizmo();
            //clsPublic.SendMessage(string.Format("Checking Survey Results:  {0}", DateTime.Now ));
            //clsSGGizmoTemp.CheckSurveys();
        }
        catch (Exception ex)
        {
            //clsPublic.LogError(ex);
        }
    }
}
