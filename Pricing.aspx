<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/DQV3.master" AutoEventWireup="true" CodeFile="Pricing.aspx.cs" Inherits="pricing" %>
<%@ Register src="UserControls/ctlPricingDescV2.ascx" tagname="ctlPricingDescV2" tagprefix="uc1" %>
<%@ Register src="UserControls/ctlPricingCalculator.ascx" tagname="ctlPricingCalculator" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<uc1:ctlPricingDescV2 ID="ctlPricingDescV21" runat="server" />
    <asp:Literal runat="server" ID="litTop"></asp:Literal>
    <asp:Literal runat="server" ID="litBottom"></asp:Literal>
</asp:Content>

