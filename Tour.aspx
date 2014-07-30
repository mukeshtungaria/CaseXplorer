<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/DQV3.master" AutoEventWireup="true" CodeFile="Tour.aspx.cs" Inherits="Tour" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table>
	<tr>
		<td>
			<asp:Literal runat="server" ID="litPageText"></asp:Literal>
		</td>
		<td>
<div style="width: 200px; float:right;z-index:10;right:0;top: 200px; height: 160px; position:fixed;">
<asp:ImageButton Height="150px" Width="200px" runat="server" ID="imgStart" ImageUrl="~/images/StartNow.png" 
PostBackUrl="~/Login/RegisterNewUser.aspx" /></div>
</td>
	</tr>
</table>
    



    <div style="width: 100%; margin-left: auto; margin-right: auto; position: relative;">
    <iframe src="http://player.vimeo.com/video/43864719" width="600" height="400" frameborder="0" 
webkitAllowFullScreen mozallowfullscreen allowFullScreen></iframe>
</div>

</asp:Content>



