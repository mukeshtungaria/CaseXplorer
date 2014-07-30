<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/DQV3.master" AutoEventWireup="true" CodeFile="TestEditor.aspx.cs" Inherits="TestEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <iframe src="Editor3.aspx" style="width: 100%; height: 500px;"></iframe>
    <telerik:RadEditor ID="txtQues" Width="100%" Height="254px" StripFormattingOptions="MSWord" runat="server" Skin="Office2007" ToolsFile="~/Research/EditorXML/BasicTools.xml" ToolbarMode="Floating"></telerik:RadEditor>
<%--    <FTB:FreeTextBox id="txtQues" width="100%" Height="254px" AutoGenerateToolbarsFromString="false" ClientSidePaste="CheckWord" ClientIDMode="Static" runat="server" ToolbarStyleConfiguration="Office2003" DesignModeCss="~/css/ftb.css" EnableHtmlMode="true">
    </FTB:FreeTextBox>
--%>
</asp:Content>

