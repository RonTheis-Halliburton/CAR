<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InputBox.aspx.cs" Inherits="CAR.Forms.InputBox" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CAR Number</title>
    <link href="../Styles/styles.css" type="text/css" rel="stylesheet" />
</head>
<body style="margin: 0px; height: 100%; top: 0px;">
    <telerik:RadCodeBlock runat="server" ID="RadCodeBlock1">
        <script type="text/javascript">
            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow) oWindow = window.radWindow;
                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                return oWindow;
            }

            function RadComboBox5_OnClientSelectedIndexChanged(sender, eventArgs) {
                var item = eventArgs.get_item();
                document.getElementById('<%=HiddenField1.ClientID %>').value = item.get_attributes().getAttribute("OID");
                document.getElementById('<%=HiddenField2.ClientID %>').value = item.get_attributes().getAttribute("CAR_NBR");
            }

            function returnToParent(sender, eventArgs) {
                //create the argument that will be returned to the parent page
                var oArg = new Object();
                oArg.carNbrOid = document.getElementById('<%=HiddenField1.ClientID %>').value;
                oArg.carNbr = document.getElementById('<%=HiddenField2.ClientID %>').value;
                //get a reference to the current RadWindow
                var oWnd = GetRadWindow();
                oWnd.close(oArg);
            }

            function GetParentPageCreate() {
                //Call Start.aspx
                var splitterPageWnd = window.parent;
                splitterPageWnd.GetParentPage();
            }
        </script>

    </telerik:RadCodeBlock>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js" />
            </Scripts>
        </telerik:RadScriptManager>
        <div class="contentContainer">
            <table>
                <tr>
                    <td>
                        <telerik:RadComboBox ID="RadComboBox5" runat="server" MarkFirstMatch="true" AllowCustomText="true" Sort="Ascending"
                            EnableLoadOnDemand="true" ShowMoreResultsBox="True" DropDownAutoWidth="enabled" LoadingMessage="Searching...Please wait"
                            Height="200px" Width="300px" DropDownWidth="300px" OnClientSelectedIndexChanged="RadComboBox5_OnClientSelectedIndexChanged"
                            OnItemsRequested="RadComboBox5_ItemsRequested"
                            OpenDropDownOnLoad="false"
                            ShowDropDownOnTextboxClick="false"
                            HighlightTemplatedItems="true"
                            EnableVirtualScrolling="true"
                            Filter="Contains"
                            EmptyMessage="Search Existing CAR Number">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadButton ID="RadButton3" runat="server" Text="Select" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" Font-Size="Small"
                            AutoPostBack="false" CausesValidation="false" OnClientClicked="returnToParent">
                        </telerik:RadButton>
                    </td>
                </tr>
            </table>
        </div>
        <asp:HiddenField ID="HiddenField2" runat="server" Value="0" />
        <asp:HiddenField ID="HiddenField1" runat="server" Value="0" />
    </form>
</body>

</html>
