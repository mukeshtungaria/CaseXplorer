using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Threading;

/// <summary>
/// Summary description for svcProcessSurvey
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class svcProcessSurvey : System.Web.Services.WebService {

    public svcProcessSurvey () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    
    //[WebMethod]
    //[SoapDocumentMethod(OneWay = true)]
    //public void UploadSurveyToSG(int intResearchID)
    //{
    //    clsSGizmo clsSGGizmoTemp = new clsSGizmo();

    //    clsSGGizmoTemp.UploadSurveyFromResearchID(intResearchID);

    //    clsGMIOutput clsGMIOutputTemp = new clsGMIOutput();

    //    clsGMIOutputTemp.SubmitSurveyV2(intResearchID);

    //}



}
