<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/DQV3Sub.master" AutoEventWireup="true" CodeFile="UserList.aspx.cs" Inherits="Admin_UserList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .list
        {}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<asp:GridView runat="server" ID="Users" AutoGenerateColumns="False"
	CssClass="list" AlternatingRowStyle-CssClass="odd" GridLines="None" 
        CellPadding="4" ForeColor="#333333" Font-Names="Verdana" Font-Size="12px" Width="901px"
	>
<AlternatingRowStyle CssClass="odd" BackColor="White" ForeColor="#284775"></AlternatingRowStyle>
<Columns>
	<asp:TemplateField>
		<HeaderTemplate>User Name</HeaderTemplate>
		<ItemTemplate>
		<a href="edit_user.aspx?username=<%# Eval("UserName") %>"><%# Eval("UserName") %></a>
		</ItemTemplate>
	    <HeaderStyle Font-Names="Verdana" Font-Size="13px" HorizontalAlign="Left" />
	</asp:TemplateField>
	<asp:BoundField DataField="email" HeaderText="Email" >
	<HeaderStyle HorizontalAlign="Left" />
    </asp:BoundField>
	<asp:BoundField DataField="creationdate" HeaderText="Creation Date" >
	<HeaderStyle HorizontalAlign="Left" />
    </asp:BoundField>
	<asp:BoundField DataField="lastactivitydate" HeaderText="Last Activity Date" >
	<HeaderStyle HorizontalAlign="Left" />
    </asp:BoundField>
	<asp:BoundField DataField="isapproved" HeaderText="Is Active" >
	<HeaderStyle HorizontalAlign="Center" />
    </asp:BoundField>
	<asp:BoundField DataField="isonline" HeaderText="Is Online" >
	<HeaderStyle HorizontalAlign="Center" />
    </asp:BoundField>
	<asp:BoundField DataField="islockedout" HeaderText="Is Locked Out" >
    <HeaderStyle HorizontalAlign="Center" />
    </asp:BoundField>
</Columns>
    <EditRowStyle BackColor="#999999" />
    <FooterStyle BackColor="#00477E" Font-Bold="True" ForeColor="White" />
    <HeaderStyle BackColor="#00477E" Font-Bold="True" ForeColor="White" />
    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
    <SortedAscendingCellStyle BackColor="#E9E7E2" />
    <SortedAscendingHeaderStyle BackColor="#506C8C" />
    <SortedDescendingCellStyle BackColor="#FFFDF8" />
    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
</asp:GridView>

</asp:Content>

