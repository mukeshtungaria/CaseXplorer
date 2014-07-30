<%@ Page Language="C#" MasterPageFile="~/MasterPages/DQV3.master" AutoEventWireup="true" CodeFile="CourtEdit.aspx.cs" Inherits="Admin_CourtEdit" Title="Untitled Page" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script type="text/javascript">
    var pageUrl = '<%=ResolveUrl("~/Venue")%>';
</script>


<div class="divSingleRow" style="height: 32px; margin-bottom: 10px;">
<select id="Jurisdiction"><option value="0">Select Jurisdiction</option><option value="1">Federal</option><option value="2">State</option></select>
<select id="State"><option value="0">Select State</option></select>
<select id="Venue"><option value="0">Select Venue</option></select>
<button id="btnSearch">Search</button>
</div>
</asp:Content>

