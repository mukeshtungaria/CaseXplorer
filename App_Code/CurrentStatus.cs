using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

/// <summary>
/// Summary description for CurrentStatus
/// </summary>
[WebService(Namespace = "https://casexplorer.com")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class CurrentStatus : System.Web.Services.WebService {

    /// <summary>
    /// Initializes a new instance of the <see cref="CurrentStatus" /> class.
    /// </summary>
    public CurrentStatus () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    [SoapDocumentMethod(OneWay = false)]
    public string OpenSurveyinSG()
    {
        String tbldata = "";
        clsSGizmo clsgizmo = new clsSGizmo();
        List<SurveyListFromGizmo> tlist = clsgizmo.GetSurveyListFromGizmo();
        if (tlist.Count > 0)
        {
            int ncount = 0;
            tbldata += "<table style='border:0px solid #ffffff;width: 733px;' cellpadding='0' cellspacing='0'><tr style='background-color:#00477E;font-weight:bold;color:White;height:30px;'><th style='padding:0px 5px;text-align:left;font-family: Calibri;'>Project</th><th style='padding:0px 5px;text-align:left;font-family: Calibri;'>Date Created</th><th style='padding:0px 5px;text-align:left;font-family: Calibri;'>Partial</th><th style='padding:0px 5px;text-align:left;font-family: Calibri;'>Complete</th><th style='padding:0px 5px;text-align:left;font-family: Calibri;'>Disqualified</th><th style='padding:0px 5px;text-align:left;font-family: Calibri;'>Deleted</th><th style='padding:0px 5px;text-align:left;font-family: Calibri;'>&nbsp;</th></tr>";
            foreach (SurveyListFromGizmo slist in tlist)
            {
                String Details = "<a href='http://www.casexplorer.com/GetResults/RespondDetails.aspx?sid=" + slist.Sid + "&rpp=" + slist.TotalPages + "&sname=" + slist.SurveyName + "' target='_blank'>Details</a>";
                
                if (ncount % 2 == 0)
                {
                    tbldata += "<tr style='background-color:White;color:#284775;'>"
                                + "<td style='padding:5px 5px 0px 5px;text-align:left;font-family: Calibri;'>" + slist.SurveyName + "</td>"
                                + "<td style='padding:5px 5px 0px 5px;text-align:left;font-family: Calibri;'>" + slist.CreatedDate.ToString("dd-MMM-yyyy") + "</td>"
                                + "<td style='padding:5px 5px 0px 5px;text-align:left;font-family: Calibri;'>" + slist.Partial + "</td>"
                                + "<td style='padding:5px 5px 0px 5px;text-align:left;font-family: Calibri;'>" + slist.Complete + "</td>"
                                + "<td style='padding:5px 5px 0px 5px;text-align:left;font-family: Calibri;'>" + slist.Disqualified + "</td>"
                                + "<td style='padding:5px 5px 0px 5px;text-align:left;font-family: Calibri;'>" + slist.Deleted + "</td>"
                                + "<td style='padding:5px 5px 0px 5px;text-align:left;font-family: Calibri;'>" + Details + "</td></tr>";
                }
                else
                {
                    tbldata += "<tr style='background-color:#F7F6F3;color:#333333;'>"
                                + "<td style='padding:5px 5px 0px 5px;text-align:left;font-family: Calibri;'>" + slist.SurveyName + "</td>"
                                + "<td style='padding:5px 5px 0px 5px;text-align:left;font-family: Calibri;'>" + slist.CreatedDate.ToString("dd-MMM-yyyy") + "</td>"
                                + "<td style='padding:5px 5px 0px 5px;text-align:left;font-family: Calibri;'>" + slist.Partial + "</td>"
                                + "<td style='padding:5px 5px 0px 5px;text-align:left;font-family: Calibri;'>" + slist.Complete + "</td>"
                                + "<td style='padding:5px 5px 0px 5px;text-align:left;font-family: Calibri;'>" + slist.Disqualified + "</td>"
                                + "<td style='padding:5px 5px 0px 5px;text-align:left;font-family: Calibri;'>" + slist.Deleted + "</td>"
                                + "<td style='padding:5px 5px 0px 5px;text-align:left;font-family: Calibri;'>" + Details + "</td></tr>";
                }
                ncount++;
            }
            tbldata += "</table>";
            clsSendMessages.SendCurrentStatusEmail(tbldata);
        }
        return tlist.Count > 0 ? "open survey is " + tlist.Count.ToString() : "No open survey Found";
    }
    
}
