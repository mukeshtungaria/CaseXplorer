<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/DQV3.master" AutoEventWireup="true" CodeFile="TestSampleQuestions.aspx.cs" Inherits="TestSampleQuestions" %>

<%@ Register src="Research/UserControls/ctlQuestionSelect.ascx" tagname="ctlQuestionSelect" tagprefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script type="text/javascript">
    $(function () {
    });

</script>



<button id="Show" onclick="return ShowSampleQuestions();">Sample</button>

<div id="dialog-sampleQuestions" title="Sample Questions" class="hidden">
    <uc1:ctlQuestionSelect ID="ctlQuestionSelect1" runat="server" />
</div>
</asp:Content>


