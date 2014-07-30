<%@ Page Language="C#" MasterPageFile="~/MasterPages/DQV3.master" AutoEventWireup="true" CodeFile="FeedBack.aspx.cs" Inherits="FeedBack" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <script type="text/javascript">

    function CheckFeedback()
    {

        var feedBack = document.getElementById("<%=txtFeedback.ClientID%>");
        
        if (feedBack != null)
        {
            if (feedBack.innerHTML.length == 0)
            {
                var result = document.getElementById("<%=lblError.ClientID%>");
                result.innerHTML = "- You must supply feedback text.";
                feedBack.focus();
                return false;
            }               
        }
        
        return true;        
    }
 
 </script>

<asp:Panel runat="server" ID="pnlMain">
<div class="ContentWidth Font10">
    <asp:Literal runat="server" ID="litTop"></asp:Literal>
</div>


<div style="width: 100%; height: 300px;">
<div class="FeedbackDesc">Name (optional):</div>
<div class="FeedbackInput"><asp:TextBox runat="server" ID="txtName" CssClass="ctlFlat" MaxLength="50" Width="300px"></asp:TextBox> </div>

<div class="FeedbackDesc">Email (optional):</div>
<div class="FeedbackInput">
    <asp:TextBox ID="txtEmail" runat="server" CssClass="ctlFlat" MaxLength="255" Width="300px"></asp:TextBox>
</div>


<div class="FeedbackDesc">Reason:</div>
<div class="FeedbackInput">
    <asp:DropDownList runat="server" ID="ddlReason" CssClass="ctlFlat" Width="215px"></asp:DropDownList>
</div>

<div class="FeedbackDescMulti">Feedback:</div>
<div class="FeedbackInputMulti">
    <asp:TextBox ID="txtFeedback" runat="server" CssClass="ctlFlat" TextMode="multiLine" Width="90%" Height="135px"></asp:TextBox>
</div>


<div class="FeedbackDesc">&nbsp;</div>
<div class="FeedbackInput">
    <asp:Button runat="server" ID="btnSubmit" Text="Submit" CssClass="ctlFlatButton" OnClick="btnSubmit_Click" OnClientClick="return CheckFeedback();" />
</div>

<div class="FeedbackDesc">&nbsp;</div>
<div class="FeedbackInput" style="color: Red; font-family: Arial; font-size: 10pt; font-weight: bold;">
    <asp:Label runat="server" ID="lblError" />
</div>
</div>
</asp:Panel>
<asp:Panel runat="server" ID="pnlResult" Visible="false">
<div class="ContentWidth Center" style="margin-top: 40px; font-weight: bold;">
Thank you for your feedback.
</div>
</asp:Panel>


</asp:Content>
