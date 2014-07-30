<%@ Page Language="C#" MasterPageFile="~/MasterPages/DQV3.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" Title="Default Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <link rel="stylesheet" href="<%=ResolveUrl("~/Styles/redmond/jquery-ui-1.8.16.custom.css")%>"  type="text/css"/>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.6.4/jquery.min.js" type="text/javascript"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.16/jquery-ui.js" type="text/javascript"></script>

<script type="text/javascript">
    $(function () {
        $("#dialog-1:ui-dialog").dialog("destroy");
        $("#dialog-2:ui-dialog").dialog("destroy");
        $("#dialog-3:ui-dialog").dialog("destroy");
        $("#dialog-4:ui-dialog").dialog("destroy");
        $("#dialog-5:ui-dialog").dialog("destroy");

        $("#dialog-1").dialog({
            autoOpen: false,
            bgiframe: false,
            resizable: false,
            width: 700,
            modal: true,
            overlay: {
                backgroundColor: '#000',
                opacity: 0.5
            },
            buttons: {
                'Close': function () {
                    $(this).dialog('close');
                }
            }
        });

        $("#dialog-2").dialog({
            autoOpen: false,
            bgiframe: false,
            resizable: false,
            width: 700,
            modal: true,
            overlay: {
                backgroundColor: '#000',
                opacity: 0.5
            },
            buttons: {
                'Close': function () {
                    $(this).dialog('close');
                }
            }
        });


        $("#dialog-3").dialog({
            autoOpen: false,
            bgiframe: false,
            resizable: false,
            width: 700,
            modal: true,
            overlay: {
                backgroundColor: '#000',
                opacity: 0.5
            },
            buttons: {
                'Close': function () {
                    $(this).dialog('close');
                }
            }
        });

        $("#dialog-4").dialog({
            autoOpen: false,
            bgiframe: false,
            resizable: false,
            width: 700,
            modal: true,
            overlay: {
                backgroundColor: '#000',
                opacity: 0.5
            },
            buttons: {
                'Close': function () {
                    $(this).dialog('close');
                }
            }
        });

        $("#dialog-5").dialog({
            autoOpen: false,
            bgiframe: false,
            resizable: false,
            width: 700,
            modal: true,
            overlay: {
                backgroundColor: '#000',
                opacity: 0.5
            },
            buttons: {
                'Close': function () {
                    $(this).dialog('close');
                }
            }
        });

/*
        function positionDialog() {
            linkOffset = $("#divmapImage").position();
            linkWidth = $("#divmapImage").width();
            linkHeight = $("#divmapImage").height();
            scrolltop = $(window).scrollTop();

            alert(scrolltop);

            $("#dialog-2").dialog("option", "position", linkOffset.left, linkOffset.top + 200);
        }


        positionDialog();

        $(window).resize(function () {
            positionDialog();
        });

        $(window).scroll(function () {
            positionDialog();
        });
*/

    });


    function Show1() {
        $("#dialog-1").dialog("open");
    }

    function Show2() {
        $("#dialog-2").dialog("open");
    }

    function Show3() {
        $("#dialog-3").dialog("open");
    }

    function Show4() {
        $("#dialog-4").dialog("open");
    }

    function Show5() {
        $("#dialog-5").dialog("open");
    }    

    </script>

<div style="width:690px; float:right;">
    <h1 class="laptop_title">DECISIONQUEST'S PROPRIETARY WEB-BASED TOOL</h1>
    <div id="laptop" style="background:url('images/laptop300.png') no-repeat center center;">
        <!--<img src="images/laptop2.png" width="483" height="329" alt="DecisionQuest"/>-->
    </div>


             
   <!-- ************************************* div with Ball Points ************************************************************ -->      
          <div id="ballMenu">
   
   
   <!-- old image map commented out
   
   <img id="ctl01_ContentPlaceHolder1_mapHomeImage" src="images/intro_page.png" usemap="#ImageMapctl01_ContentPlaceHolder1_mapHomeImage" border="0" /><map name="ImageMapctl01_ContentPlaceHolder1_mapHomeImage" id="ImageMapctl01_ContentPlaceHolder1_mapHomeImage">
	<area shape="rect" coords="260,10,382,40" href="HomePage.aspx" title="" alt="" /><area shape="circle" coords="20,20,20" href="javascript: Show1();" title="" alt="" /><area shape="circle" coords="20,90,20" href="javascript: Show2();" title="" alt="" /><area shape="circle" coords="20,161,20" href="javascript: Show3();" title="" alt="" /><area shape="circle" coords="20,231,20" href="javascript: Show4();" title="" alt="" /><area shape="circle" coords="20,301,20" href="javascript: Show5();" title="" alt="" />
</map>-->

 
<ul style="margin-top: 10px;">
<li>
<a href="javascript: Show1();"><img src="images/ball1.png"  width="40" height="44" border="0" align="absmiddle" onmouseover = "this.src = 'images/ball1_hover.png'"
onmouseout = "this.src = 'images/ball1.png'" />&nbsp;&nbsp;See What <b style="font-style:italic;">CaseXplorer</b> Does</a>&nbsp;&nbsp;<a href="Login/RegisterNewUser.aspx"><img src="images/start_button.png" width="135" height="46" border="0" align="absmiddle" /></a></li>

<li><a href="javascript: Show2();"><img src="images/ball2.png" width="40" height="44" border="0" align="absmiddle" onmouseover = "this.src = 'images/ball2_hover.png'"
onmouseout = "this.src = 'images/ball2.png'" />&nbsp;&nbsp;Draft Questions or Choose From Our List</a></li>

<li style="width: 280px;"><a href="javascript: Show3();"><img src="images/ball3.png" width="40" height="44" border="0" align="absmiddle" onmouseover = "this.src = 'images/ball3_hover.png'"
onmouseout = "this.src = 'images/ball3.png'"/>&nbsp;&nbsp;Review Your Custom Research</a></li>

<li style="width: 280px;"><a href="javascript: Show4();">
<div style="float:left; width:40px;"><img src="images/ball4.png" width="40" height="44" border="0" align="absmiddle" onmouseover = "this.src = 'images/ball4_hover.png'"
onmouseout = "this.src = 'images/ball4.png'"/></div><div style=" float:left; width:240px; margin-top:2px;">&nbsp;&nbsp;Select Jurors From a Specific <BR />&nbsp;&nbsp;Venue or Nationwide</div><BR style="clear:both;" /></a></li>

<li style="width: 250px;"><a href="javascript: Show5();"><img src="images/ball5.png" width="40" height="44" border="0" align="absmiddle" onmouseover = "this.src = 'images/ball5_hover.png'"
onmouseout = "this.src = 'images/ball5.png'"/>&nbsp;&nbsp;Submit to Surrogate Jurors</a></li>
</ul>

<div id="assist_image">
<a href="mailto:onlinejuryadmin@decisionquest.com"><img src="images/assist4.png" width="287px" height="130px" border="0"/></a>


</div> <!-- end assist image div-->

                   </div> <!-- end div ballMenu -->




</div>

<div id="dialog-1" title="Design Your Research" style="display:none;">
<asp:Literal runat="server" ID="lit1"></asp:Literal>
</div>
<div id="dialog-2" title="Draft Question You Want Answered" style="display:none;">
<asp:Literal runat="server" ID="lit2"></asp:Literal>
</div>

<div id="dialog-3" title="Review Your Custom Research" style="display:none;">
<asp:Literal runat="server" ID="lit3"></asp:Literal>
</div>
<div id="dialog-4" title="Select Jurors From Any Nationwide Venue" style="display:none;">
<asp:Literal runat="server" ID="lit4"></asp:Literal>
</div>
<div id="dialog-5" title="Submit To Surrogate Jurors" style="display:none;">
<asp:Literal runat="server" ID="lit5"></asp:Literal>
</div>

</asp:Content>


