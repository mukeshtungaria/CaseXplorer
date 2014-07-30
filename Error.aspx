<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/DQV3.master" AutoEventWireup="true" CodeFile="Error.aspx.cs" Inherits="Error" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   
    <div style="text-align: left; height: auto; overflow: hidden; width: 100%;">
    
        <asp:Label ID="lblMessage" runat="server" Text="An Error Has Occurred." Font-Bold="true" ForeColor="Red" />
        
        <br />
        <asp:HyperLink ID="lnkBack" runat="server" Text="Back" NavigateUrl="javascript:history.go(-1)" />
    
    </div>
</asp:Content>

