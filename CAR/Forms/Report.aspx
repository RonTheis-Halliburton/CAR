<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="CAR.Forms.Report" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>
<script type="text/javascript" src="JS/TopLocation.js"></script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title>Report</title>
    <link href="../Styles/styles.css" type="text/css" rel="stylesheet" /></head>
<body style="margin: 0px; height: 100%; top: 0px;">
  <telerik:RadCodeBlock runat="server" ID="RadCodeBlock1">
        <script type="text/javascript">

            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow) oWindow = window.radWindow; //Will work in Moz in all cases, including clasic dialog
                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow; //IE (and Moz as well)
                return oWindow;
            }

            function GetParentPage() {
                //Call Main.aspx
                var splitterPageWnd = window.parent;
                splitterPageWnd.MainParent();
            }
        </script>
    </telerik:RadCodeBlock>
    <form id="form1" runat="server">
    <div class="contentContainer">
        <asp:LinkButton ID="LinkButton1" runat="server" Font-Names="Arial" Font-Size="Small">Download QlikView IE PlugIn</asp:LinkButton>
    </div>
    </form>
</body>
</html>