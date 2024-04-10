<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SystemAdmin.aspx.cs" Inherits="CAR.Forms.SystemAdmin" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>
<script type="text/javascript" src="JS/TopLocation.js"></script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Start</title>
    <link href="../Styles/styles.css" type="text/css" rel="stylesheet" />
    <style>
        .contentContainer th {
            font: bold 11px Arial, Verdana, Sans-serif;
            text-align: left;
        }
    </style>
</head>
<body style="margin: 0px; height: 100%; top: 0px;">
    <telerik:RadCodeBlock runat="server" ID="RadCodeBlock1">
        <script type="text/javascript">
            var splitButton = null;
            var contextMenu = null;

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
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js" />
            </Scripts>
        </telerik:RadScriptManager>
        <telerik:RadAjaxLoadingPanel ID="LoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>

        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <div class="contentContainer">
                <telerik:RadTabStrip RenderMode="Lightweight" ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" Font-Bold="true" Orientation="HorizontalTop"
                    SelectedIndex="0" Width="100%" OnTabClick="RadTabStrip1_TabClick" CausesValidation="false">
                    <Tabs>
                        <telerik:RadTab PageViewID="RadPageView1" Value="RadPageView1" Text="User List" Selected="true">
                        </telerik:RadTab>
                        <telerik:RadTab PageViewID="RadPageView2" Value="RadPageView2" Text="Portal">
                        </telerik:RadTab>
                    </Tabs>
                </telerik:RadTabStrip>
                <telerik:RadMultiPage ID="RadMultiPage1" SelectedIndex="0" Height="100%" runat="server" RenderSelectedPageOnly="true">
                    <telerik:RadPageView runat="server" ID="RadPageView1" Selected="true">
                        <telerik:RadTextBox ID="RadTextBox1" runat="server" Text="Coming Soon"></telerik:RadTextBox>
                    </telerik:RadPageView>
                    <telerik:RadPageView runat="server" ID="RadPageView2">
                        <table>
                            <tr>
                                <td style="vertical-align: top;">
                                    <table>
                                        <tr>
                                            <th>Portal</th>
                                            <td>
                                                <telerik:RadComboBox ID="RadComboBox22" runat="server" OnSelectedIndexChanged="RadComboBox22_SelectedIndexChanged" AutoPostBack="true" Width="200px">
                                                    <Items>
                                                        <telerik:RadComboBoxItem runat="server" Value="0" Text="Internal Site" />
                                                        <telerik:RadComboBoxItem runat="server" Value="1" Text="External Site via HSN" />
                                                    </Items>
                                                </telerik:RadComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>Status</th>
                                            <td>
                                                <telerik:RadComboBox ID="RadComboBox23" runat="server" MarkFirstMatch="True" Width="200px">
                                                    <Items>
                                                        <telerik:RadComboBoxItem runat="server" Text="ON" Value="1" />
                                                        <telerik:RadComboBoxItem runat="server" Text="OFF" Value="0" />
                                                    </Items>
                                                </telerik:RadComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <telerik:RadButton ID="RadButton12s" runat="server" Font-Bold="true" UseSubmitBehavior="true" RegisterWithScriptManager="true" Text="Save" 
                                                    OnClick="RadButton12s_Click" SingleClick="true" SingleClickText="Processing..."
                                                    BorderColor="Blue">
                                                    <Icon PrimaryIconCssClass="rbSave" PrimaryIconRight="5" SecondaryIconTop="5"></Icon>
                                                </telerik:RadButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="Label4" runat="server" Font-Size="Small"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <fieldset class="fieldset">
                                                    <legend>OFFLINE message required, please type a brief description below.</legend>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadTextBox ID="RadTextBox4" runat="server" Rows="15" Width="450px"
                                                                    BorderStyle="Solid" TextMode="MultiLine">
                                                                    <FocusedStyle BackColor="LightCyan" />
                                                                </telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <fieldset class="fieldset">
                                                    <legend>Notification Message Display</legend>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadTextBox ID="RadTextBox5" runat="server" Rows="20" Width="450px"
                                                                    BorderStyle="Solid" TextMode="MultiLine" EmptyMessage="Type a brief message to be displayed...">
                                                                    <FocusedStyle BackColor="LightCyan" />
                                                                </telerik:RadTextBox>
                                                            </td>
                                                            <td style="vertical-align: top;">
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <fieldset class="fieldset">
                                                                                <legend>Status</legend>

                                                                                <telerik:RadComboBox ID="RadComboBox24" runat="server" Width="200px">
                                                                                    <Items>
                                                                                        <telerik:RadComboBoxItem runat="server" Text="ON" Value="1" />
                                                                                        <telerik:RadComboBoxItem runat="server" Text="OFF" Value="0" />
                                                                                    </Items>
                                                                                </telerik:RadComboBox>
                                                                            </fieldset>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <fieldset class="fieldset">
                                                                                <legend>Position</legend>
                                                                                <telerik:RadComboBox ID="RadComboBox25" runat="server" Width="200px" Height="150px">
                                                                                    <Items>
                                                                                        <telerik:RadComboBoxItem runat="server" Value="TopLeft" Text="Top Left" />
                                                                                        <telerik:RadComboBoxItem runat="server" Value="TopCenter" Text="Top Center" />
                                                                                        <telerik:RadComboBoxItem runat="server" Value="TopRight" Text="Top Right" />
                                                                                        <telerik:RadComboBoxItem runat="server" Value="MiddleLeft" Text="Middle Left" />
                                                                                        <telerik:RadComboBoxItem runat="server" Value="Center" Text="Center" Selected="true" />
                                                                                        <telerik:RadComboBoxItem runat="server" Value="MiddleRight" Text="Middle Right" />
                                                                                        <telerik:RadComboBoxItem runat="server" Value="BottomLeft" Text="Bottom Left" />
                                                                                        <telerik:RadComboBoxItem runat="server" Value="BottomCenter" Text="Bottom Center" />
                                                                                        <telerik:RadComboBoxItem runat="server" Value="BottomRight" Text="Bottom Right" />
                                                                                    </Items>
                                                                                </telerik:RadComboBox>
                                                                            </fieldset>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <fieldset class="fieldset">
                                                                                <legend>Auto Close Delay (ms)</legend>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <telerik:RadNumericTextBox ID="RadNumericTextBox2" runat="server" MinValue="0" Value="5000" ShowSpinButtons="true" AllowOutOfRangeAutoCorrect="true">
                                                                                                <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                                                            </telerik:RadNumericTextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>Off = Zero (0)</td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>On = recommended 8000 ms or longer</td>
                                                                                    </tr>
                                                                                </table>
                                                                            </fieldset>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <fieldset class="fieldset">
                                                                                <legend>Show Interval (ms)</legend>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <telerik:RadNumericTextBox ID="RadNumericTextBox3" runat="server" MinValue="0" Value="8000" ShowSpinButtons="true" AllowOutOfRangeAutoCorrect="true">
                                                                                                <NumberFormat DecimalDigits="0" GroupSeparator="" />
                                                                                            </telerik:RadNumericTextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>Off = Zero (0)</td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>On = recommended 8000 ms or longer</td>
                                                                                    </tr>
                                                                                </table>
                                                                            </fieldset>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </telerik:RadPageView>
                </telerik:RadMultiPage>

            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
