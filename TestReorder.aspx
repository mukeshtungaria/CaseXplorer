<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/DQV3.master" AutoEventWireup="true" CodeFile="TestReorder.aspx.cs" Inherits="TestReorder" %>

<%@ Register src="UserControls/ctlQuestionReorder.ascx" tagname="ctlQuestionReorder" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">



    <uc1:ctlQuestionReorder ID="ctlQuestionReorder1" runat="server" />



</asp:Content>

