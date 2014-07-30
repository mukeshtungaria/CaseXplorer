using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class ImportDataTest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        clsImportSurveyResults clsImport = new clsImportSurveyResults();

        clsImport.LoadExcelFile(@"C:\emailimport\test_century.xls");

//        this.GridView1.DataSource = ds.Tables["ExcelInfo"].DefaultView;
//        GridView1.DataBind();
    }
}
