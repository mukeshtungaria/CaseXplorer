using System;
using System.ComponentModel;
using System.Drawing;
using Telerik.Reporting;
using Telerik.Reporting.Drawing;

/// <summary>
/// Summary description for rpt_Results.
/// </summary>
public class rpt_Results : Report
{
    private Telerik.Reporting.PageHeaderSection pageHeader;
    private Telerik.Reporting.DetailSection detail;
    private Telerik.Reporting.PageFooterSection pageFooter;

    public rpt_Results()
    {
        /// <summary>
        /// Required for telerik Reporting designer support
        /// </summary>
        InitializeComponent();

        //
        // TODO: Add any constructor code after InitializeComponent call
        //
    }

    #region Component Designer generated code
    /// <summary>
    /// Required method for telerik Reporting designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.pageHeader = new Telerik.Reporting.PageHeaderSection();
        this.detail = new Telerik.Reporting.DetailSection();
        this.pageFooter = new Telerik.Reporting.PageFooterSection();
        // 
        // rpt_Results
        // 
        this.Style.BackgroundColor = System.Drawing.Color.White;
        this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
                                                                       this.pageHeader,
                                                                       this.detail,
                                                                       this.pageFooter});
    }
    #endregion
}