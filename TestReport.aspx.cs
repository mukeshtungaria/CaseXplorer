using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using JuryData.Entities;
using JuryData.Data;


public partial class TestReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        clsReports.rptSummary rptSummaryTemp = new clsReports.rptSummary();

        ResearchMain entResearchMain = DataRepository.ResearchMainProvider.GetByAutoResearchID(297);

        rptSummaryTemp.ReportParameters["ResearchID"].Value = 297;
        rptSummaryTemp.SetTitle(entResearchMain.StrResearchSurveyName);

        this.ReportViewer1.Report = rptSummaryTemp;
        this.ReportViewer1.RefreshReport();


    }
}