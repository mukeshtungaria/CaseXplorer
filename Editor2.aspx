<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Editor2.aspx.cs" Inherits="Editor2" %>

<html>
<head id="Head1" runat="server">
	<title>JavaScript Demo</title>


</head>
<body>

    <form id="Form1" runat="server">
    	<telerik:RadEditor ID="txtQues" Width="600px" Height="200px" runat="server" StripFormattingOptions="MSWord" Skin="Office2007" ToolsFile="~/Research/EditorXML/BasicTools.xml" ToolbarMode="Floating"></telerik:RadEditor>
        <%--<FTB:FreeTextBox id="txtQues" width="600px" height="200px" runat="server" 
            ClientSidePaste="CheckWord" ClientIDMode="Static" 
            DesignModeCss="~/css/ftb.css" />--%>

					<a href="#" onclick="" style="display:block; border: solid 1px #ccc; padding: 3px; text-decoration:none;">
						Copy from
						<br />
						FreeTextBox -->
					</a>

                    <asp:Button runat="server" ID="btnTest" OnClientClick="alert($('txtQues_designEditor').contents().find('html').html() ); return false;" />
					
<asp:TextBox id="TextBox1" name="TextBox1" TextMode="MultiLine" Columns="30" rows="15" runat="server" />
		

                      <script type="text/javascript">
                          var test = '';

                          function Test(a) {
                              var temp = $find("<%=txtQues.ClientID%>").get_html();

                              if (test != temp) {
                                  test = temp;
                                  if (test.indexOf('MsoNormal') != -1) {
                                      var out = cleanHTML(test);
                                      out = out;
                                      $find("<%=txtQues.ClientID%>").set_html(out);

                                  }
                              }
                          }


            </script>


    
	</form>
</body>
</html>

