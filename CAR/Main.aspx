<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.cs" Inherits="CAR.Main" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>
<script type="text/javascript" src="JS/TopLocation.js"></script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>Corrective Action</title>
    <link href="Styles/styles.css" type="text/css" rel="stylesheet" />
</head>
<body style="margin: 0px; height: 100%; top: 0px;">
    <telerik:radcodeblock runat="server" id="RadCodeBlock1">
        <script type="text/javascript">
            function MainParent() {
                //call when seesion is expired
                alert('Your session has expired.');
                location.reload(true);
            }

            function MenuItemClicked(sender, eventArgs) {
                var splitter = $find("<%= RadSplitter1.ClientID %>");
                var pane = splitter.GetPaneById("RadPane1");

                pane.set_width("100%");

                if (eventArgs.get_item().get_value() != "Ignore") {

                    //alert(eventArgs.get_item().get_level());

                    if (eventArgs.get_item().get_text() == "User Guide") {
                        pane.set_contentUrl("Help/" + eventArgs.get_item().get_value());
                    }
                    else {
                        pane.set_contentUrl("Forms/" + eventArgs.get_item().get_value() + ".aspx");
                    }

                }
                return false;
            }

        </script>
    </telerik:radcodeblock>
    <form id="form1" runat="server">
        <telerik:radscriptmanager id="RadScriptManager1" runat="server" enablescriptcombine="false">
            <scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js" />
            </scripts>
        </telerik:radscriptmanager>
        <telerik:radnotification id="RadNotification1" runat="server" keeponmouseover="true" animation="Slide" title="Alert" contentscrolling="Auto" width="100%" height="250px" overlay="true"
            showinterval="0" autoclosedelay="0" position="TopCenter" offsety="50" contenticon="Warning" titleicon="Warning">
            <contenttemplate>
                <pre>
                <div style="height: auto; width: auto; vertical-align: top; font-family: Arial; font-size:medium; word-wrap: break-word;" runat="server" id="Div1">
                </div>
                    </pre>
            </contenttemplate>
        </telerik:radnotification>
        <telerik:radnotification id="RadNotification2" runat="server" keeponmouseover="true" animation="Slide" title="Notification" contentscrolling="Auto" width="100%" overlay="true">
            <contenttemplate>
                <pre>
                <div style="height: auto; width: auto; vertical-align: top; font-family: Arial; font-size:small; word-wrap: break-word;" runat="server" id="Div2">
                </div>
                    </pre>
            </contenttemplate>
        </telerik:radnotification>
        <div class="contentContainer">
            <table style="width: 100%;">
                <tr>
                    <td style="border-right-style: solid; border-right-width: thin; border-right-color: lightgray; border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: lightgray; width: 10%;">
                        <asp:Image ID="Image3" ImageUrl="~/Img/HAL_Horz.jpg" runat="server" Height="35px" AlternateText="Halliburton" />
                    </td>
                    <td style="vertical-align: middle; border-right-style: none; border-right-width: thin; border-right-color: lightgray; border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: lightgray; width: 15%;">
                        <b><i style="font-style: italic; font-family: Arial, Sans-Serif; font-size: medium; align-content: center;">Corrective Action Request</i></b>
                    </td>
                    <td style="vertical-align: middle; text-align: left; border-bottom-style: solid; border-bottom-width: thin; border-bottom-color: lightgray;">
                        <table style="width: 100%;">
                            <tr>
                                <td style="text-align: right;">
                                    <asp:Label ID="Label1" runat="server" Font-Bold="true" CssClass="label"></asp:Label>
                                </td>
                                <td rowspan="2" style="text-align: right;">
                                    <table>
                                        <tr>
                                            <td>
                                                <telerik:radpushbutton id="RadPushButton1" runat="server" text="RadPushButton" onclick="RadPushButton1_Click" singleclick="True" singleclicktext="Processing..." bordercolor="blue" usesubmitbehavior="false"></telerik:radpushbutton>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label8" runat="server" Font-Size="Small"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <asp:Label ID="Label500" runat="server" CssClass="label"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">
                                    <asp:Label ID="Label200" runat="server" CssClass="labelBold"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Label600" runat="server" CssClass="label"></asp:Label>
                                </td>
                                <td>
                                    <a href="mailto:fhounbcit@halliburton.com">Site Administrator</a>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <telerik:radmenu rendermode="Lightweight" id="RadMenu1" skin="Silk" runat="server" showtogglehandle="true" onclientitemclicked="MenuItemClicked" font-bold="false" font-size="Small" borderstyle="None">
                            <items>
                                <telerik:radmenuitem text="Active Requests" value="Active" />
                                <telerik:radmenuitem text="Create" value="Start" />
                                <telerik:radmenuitem text="Search" value="Search" />
                                <telerik:radmenuitem text="Report" value="Report" />
                                <telerik:radmenuitem text="Help" value="Ignore">
                                    <items>
                                        <telerik:radmenuitem runat="server" text="User Guide" forecolor="Black" value="Corrective Action Request.pptx" target="_blank"></telerik:radmenuitem>
                                    </items>
                                </telerik:radmenuitem>
                                <telerik:radmenuitem text="System Admin" value="SystemAdmin" />
                            </items>
                        </telerik:radmenu>
                    </td>
                </tr>
            </table>
            <telerik:radsplitter id="RadSplitter1" runat="server" orientation="Horizontal" width="100%" bordersize="0" panesbordersize="0">
                <telerik:radpane id="RadPane1" runat="server" width="100%" height="900px" scrolling="None"></telerik:radpane>
            </telerik:radsplitter>
        </div>
        <asp:HiddenField ID="HiddenLocalDateField" runat="server" />
        <asp:HiddenField ID="HiddenLocalTimeField" runat="server" />
    </form>
</body>
</html>
