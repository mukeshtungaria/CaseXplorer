<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/DQV3Sub.master" AutoEventWireup="true" CodeFile="Edit_User.aspx.cs" Inherits="Admin_Edit_User" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<table class="webparts">
<tr>
<td class="details" valign="top">

<h1>Roles:</h1>
<asp:CheckBoxList ID="UserRoles" runat="server" />

<h1>User Info:</h1>
<asp:DetailsView AutoGenerateRows="False" DataSourceID="MemberData" ID="UserInfo" 
        runat="server" OnItemUpdating="UserInfo_ItemUpdating" CellPadding="4" 
        ForeColor="#333333" GridLines="None" Width="400px" >
  
    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
    <CommandRowStyle BackColor="#E2DED6" Font-Bold="True" />
    <EditRowStyle BackColor="#999999" />
    <FieldHeaderStyle BackColor="#E9ECF1" Font-Bold="True" />
  
<Fields>
	<asp:BoundField DataField="UserName" HeaderText="User Name" ReadOnly="True" HeaderStyle-CssClass="detailheader" ItemStyle-CssClass="detailitem">
<HeaderStyle CssClass="detailheader"></HeaderStyle>

<ItemStyle CssClass="detailitem"></ItemStyle>
	</asp:BoundField>
	<asp:BoundField DataField="Email" HeaderText="Email" HeaderStyle-CssClass="detailheader" ItemStyle-CssClass="detailitem">
<HeaderStyle CssClass="detailheader"></HeaderStyle>

<ItemStyle CssClass="detailitem"></ItemStyle>
    </asp:BoundField>
	<asp:BoundField DataField="Comment" HeaderText="Comment" HeaderStyle-CssClass="detailheader" ItemStyle-CssClass="detailitem">
<HeaderStyle CssClass="detailheader"></HeaderStyle>

<ItemStyle CssClass="detailitem"></ItemStyle>
    </asp:BoundField>
	<asp:CheckBoxField DataField="IsApproved" HeaderText="Active User" 
        HeaderStyle-CssClass="detailheader" ItemStyle-CssClass="detailitem" >
<HeaderStyle CssClass="detailheader"></HeaderStyle>

<ItemStyle CssClass="detailitem"></ItemStyle>
    </asp:CheckBoxField>
	<asp:CheckBoxField DataField="IsLockedOut" HeaderText="Is Locked Out" 
        ReadOnly="true" HeaderStyle-CssClass="detailheader" 
        ItemStyle-CssClass="detailitem" >
	
<HeaderStyle CssClass="detailheader"></HeaderStyle>

<ItemStyle CssClass="detailitem"></ItemStyle>
    </asp:CheckBoxField>
	
	<asp:CheckBoxField DataField="IsOnline" HeaderText="Is Online" ReadOnly="True" 
        HeaderStyle-CssClass="detailheader" ItemStyle-CssClass="detailitem" >
<HeaderStyle CssClass="detailheader"></HeaderStyle>

<ItemStyle CssClass="detailitem"></ItemStyle>
    </asp:CheckBoxField>
	<asp:BoundField DataField="CreationDate" HeaderText="CreationDate" ReadOnly="True"
	 HeaderStyle-CssClass="detailheader" ItemStyle-CssClass="detailitem">
<HeaderStyle CssClass="detailheader"></HeaderStyle>

<ItemStyle CssClass="detailitem"></ItemStyle>
    </asp:BoundField>
	<asp:BoundField DataField="LastActivityDate" HeaderText="LastActivityDate" ReadOnly="True" HeaderStyle-CssClass="detailheader" ItemStyle-CssClass="detailitem">
<HeaderStyle CssClass="detailheader"></HeaderStyle>

<ItemStyle CssClass="detailitem"></ItemStyle>
	</asp:BoundField>
	<asp:BoundField DataField="LastLoginDate" HeaderText="LastLoginDate" ReadOnly="True" HeaderStyle-CssClass="detailheader" ItemStyle-CssClass="detailitem">
<HeaderStyle CssClass="detailheader"></HeaderStyle>

<ItemStyle CssClass="detailitem"></ItemStyle>
	</asp:BoundField>
	<asp:BoundField DataField="LastLockoutDate" HeaderText="LastLockoutDate" ReadOnly="True" HeaderStyle-CssClass="detailheader" ItemStyle-CssClass="detailitem">
<HeaderStyle CssClass="detailheader"></HeaderStyle>

<ItemStyle CssClass="detailitem"></ItemStyle>
    </asp:BoundField>
	<asp:BoundField DataField="LastPasswordChangedDate" HeaderText="LastPasswordChangedDate"
	ReadOnly="True" HeaderStyle-CssClass="detailheader" ItemStyle-CssClass="detailitem">
<HeaderStyle CssClass="detailheader"></HeaderStyle>

<ItemStyle CssClass="detailitem"></ItemStyle>
    </asp:BoundField>
	<asp:CommandField ButtonType="button" ShowEditButton="true" EditText="Edit User Info" />
</Fields>
    <FooterStyle BackColor="#00477E" Font-Bold="True" ForeColor="White" />
    <HeaderStyle BackColor="#00477E" Font-Bold="True" ForeColor="White" />
    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
    <RowStyle BackColor="#F7F6F3" Font-Names="Verdana" Font-Size="13px" 
        ForeColor="#333333" />
</asp:DetailsView>
<div class="alert" style="padding: 5px;">
<asp:Literal ID="UserUpdateMessage" runat="server">&nbsp;</asp:Literal>
</div>


<div style="text-align: right; width: 100%; margin: 20px 0px;">
<asp:Button ID="Button1" runat="server" Text="Unlock User" OnClick="UnlockUser" />
&nbsp;&nbsp;&nbsp;
<asp:Button ID="Button2" runat="server" Text="Delete User" OnClick="DeleteUser" />
</div>


<asp:ObjectDataSource ID="MemberData" runat="server" DataObjectTypeName="System.Web.Security.MembershipUser" SelectMethod="GetUser" UpdateMethod="UpdateUser" TypeName="System.Web.Security.Membership">
	<SelectParameters>
		<asp:QueryStringParameter Name="username" QueryStringField="username" />
	</SelectParameters>
</asp:ObjectDataSource> 
</td>

</tr></table>


</asp:Content>

