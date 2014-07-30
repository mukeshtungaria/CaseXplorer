<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestReport.aspx.cs" Inherits="TestReport" %>
<%@ Register assembly="Telerik.ReportViewer.WebForms" namespace="Telerik.ReportViewer.WebForms" tagprefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <telerik:ReportViewer ID="ReportViewer1" runat="server" Height="900px" Width="900px">
</telerik:ReportViewer>

</div></form></body></html>