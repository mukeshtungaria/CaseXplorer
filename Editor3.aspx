<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Editor3.aspx.cs" Inherits="Editor3" %>

<html>
<head id="Head1" runat="server">
    <title></title>
</head>
<body>

    <form id="form1" runat="server">
    <div style="height: 90%;">
        <telerik:RadEditor ID="txtQues" Width="100%" Height="254px" StripFormattingOptions="MSWord" runat="server" Skin="Office2007" ToolsFile="~/Research/EditorXML/BasicTools.xml" ToolbarMode="Floating"></telerik:RadEditor>
        <%--<FTB:FreeTextBox id="txtQues" width="100%" Height="254px" AutoGenerateToolbarsFromString="false" ClientSidePaste="CheckWord" ClientIDMode="Static" runat="server" ToolbarStyleConfiguration="Office2003" DesignModeCss="~/css/ftb.css" EnableHtmlMode="true">
        </FTB:FreeTextBox>--%>
    </div>

    </form>

    <script type="text/javascript">
        function GetHTMLText() {
            return $find("<%=txtQues.ClientID%>").get_html();
            //return FTB_API['txtQues'].GetHtml();
        }

        function SetHTMLText(strText) {
            $find("<%=txtQues.ClientID%>").set_html(strText);
            //FTB_API['txtQues'].SetHtml(strText);
        }

        function SetFocus() {
            $find("<%=txtQues.ClientID%>").setFocus();
            //FTB_API['txtQues'].Focus();
        }

        function Resize(height) {
            var fo = document.getElementById('txtQues_TabRow');
            var de = document.getElementById('txtQues_designEditor');
            var he = document.getElementById('txtQues_htmlEditorArea');
            var pp = document.getElementById('txtQues_htmlEditorArea');
            var ar = document.getElementById('txtQues');
            var offset = 1;

            de.style.height = height;
            he.style.height = height;
            pp.style.height = height;
            ar.style.height = height;

            return false;

        }
   </script>
</body>
</html>