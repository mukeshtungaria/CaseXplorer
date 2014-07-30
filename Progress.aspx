<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Progress.aspx.cs" Inherits="Progress" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<html xmlns="http://www.w3.org/1999/xhtml">   
<head id="Head1" runat="server">   
    <title>Upload Progress</title>  
</head>  
<body>  
    <form id="form1" runat="server">   
        <script type="text/javascript">      
            Telerik.Web.UI.RadProgressArea.prototype._cancelRequest =       
            Telerik.Web.UI.RadProgressArea.prototype.cancelRequest;      
            Telerik.Web.UI.RadProgressArea.prototype.cancelRequest = function()      
            {      
                this._cancelRequest();      
                var window = GetRadWindow();   
                window.close();   
            }      
       </script>     
        <asp:scriptmanager id="ScriptManager1" runat="server">   
        </asp:scriptmanager>  
        <telerik:radprogressmanager id="RadProgressManager1" runat="server"></telerik:radprogressmanager>  
        <telerik:radprogressarea id="RadProgressArea1" runat="server" displaycancelbutton="True">   
        </telerik:radprogressarea>  
    </form>  
</body>  
</html>  
