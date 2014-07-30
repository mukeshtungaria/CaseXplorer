<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/DQV3.master" AutoEventWireup="true" CodeFile="Contact.aspx.cs" Inherits="Contact" %>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManagerProxy ID="ScriptManager1" runat="server">
    </asp:ScriptManagerProxy>

<asp:Panel runat="server" ID="pnlMain">

    <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
    
    <div class="ContentWidth MarginBottom10">
        <asp:Literal runat="server" ID="litPageText"></asp:Literal>
    </div>    
    
    <div class="FeedbackDesc">Name (Required):</div>
    <div class="FeedbackInput">
        <asp:TextBox runat="server" ID="txtName" CssClass="ctlFlat" MaxLength="50" Width="300px"></asp:TextBox>
    </div>

    <div class="FeedbackDesc">Firm (Required):</div>
    <div class="FeedbackInput">
        <asp:TextBox runat="server" ID="txtFirm" CssClass="ctlFlat" MaxLength="50" Width="300px"></asp:TextBox>
    </div>

    <div class="FeedbackDesc">Title (Required):</div>
    <div class="FeedbackInput">
        <asp:TextBox ID="txtTitle" runat="server" CssClass="ctlFlat" MaxLength="50" Width="300px"></asp:TextBox>
    </div>

    <div class="FeedbackDesc">Email (Required):</div>
    <div class="FeedbackInput">
        <asp:TextBox ID="txtEmail" ClientIDMode="Static" runat="server" CssClass="ctlFlat" MaxLength="255" Width="300px"></asp:TextBox>
    </div>        

    <div class="FeedbackDesc">Phone (Required):</div>
    <div class="FeedbackInput">
        <telerik:RadMaskedTextBox ID="txtPhone" Runat="server" Mask="(###) ###-#### x####" TextWithLiterals="() - x" Width="125px"></telerik:RadMaskedTextBox>
    </div>

    <div class="FeedbackDesc">Comment / Question (Optional):</div>
    <div class="FeedbackInput" style="height: 125px;">
        <asp:TextBox ID="txtComment" runat="server" CssClass="ctlFlat" MaxLength="8000" Width="300px" TextMode="MultiLine" Height="100px"></asp:TextBox>
    </div>

    <div class="FeedbackDesc">&nbsp;</div>
    <div class="FeedbackInput">
        <asp:Button runat="server" ID="btnSubmit" Text="Submit" CssClass="ctlFlatButton" OnClick="btnSubmit_Click" />
    </div>

    <div class="FeedbackDesc">&nbsp;</div>
    <div class="FeedbackInput" style="color: Red; font-family: Arial; font-size: 10pt; font-weight: bold;">
        <asp:Label runat="server" ID="lblError" />
    </div>

    <div class="ContentWidth">
        <asp:Literal runat="server" ID="litPageText2"></asp:Literal>
    </div>

    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Name Required" ControlToValidate="txtName" SetFocusOnError="True" Display="None"></asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Firm Required" ControlToValidate="txtFirm" SetFocusOnError="True" Display="None"></asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Title Required" ControlToValidate="txtTitle" SetFocusOnError="True" Display="None"></asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Email Required" ControlToValidate="txtEmail" SetFocusOnError="True" Display="None"></asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Phone Required" ControlToValidate="txtPhone" SetFocusOnError="True" Display="None"></asp:RequiredFieldValidator>
    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtEmail" runat="server" ErrorMessage="You must enter a valid email address" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="None"></asp:RegularExpressionValidator>

</asp:Panel>

</asp:Content>


