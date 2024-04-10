<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Message.aspx.cs" Inherits="CAR.Forms.Message" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>
<script type="text/javascript" src="JS/TopLocation.js"></script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Message</title>
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
            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow) oWindow = window.radWindow; //Will work in Moz in all cases, including clasic dialog
                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow; //IE (and Moz as well)
                return oWindow;
            }

            function GetParentPage() {
                //Call Main.aspx
                var splitterPageWnd = window.parent;
                splitterPageWnd.GetParentPage();
            }

            function RadComboBox8_OnClientSelectedIndexChanged(sender, eventArgs) {
                var item = eventArgs.get_item();
                document.getElementById('<%=HiddenRecipientId.ClientID %>').value = item.get_attributes().getAttribute("UserId");
                document.getElementById('<%=HiddenRecipientName.ClientID %>').value = item.get_attributes().getAttribute("FullName");
                document.getElementById('<%=HiddenRecipientEmail.ClientID %>').value = item.get_attributes().getAttribute("Email");
            }


            function OnClientClicking_RadButton5(sender, eventArgs) {
                var list1 = $find("<%= RadListBox1.ClientID %>"); //HES
                var list3 = $find("<%= RadListBox3.ClientID %>"); //Vendor
                var hiddenAreaOid = document.getElementById('<%=HiddenAreaOid.ClientID %>').value;  //Area                

                var textBox9 = $find("<%= RadTextBox9.ClientID %>");
                var uCheck = true;

                var checkedNodes = list1.get_checkedItems();
                if (checkedNodes.length == 0) {
                    uCheck = false;
                    alert("Select at least 1 recipient from HES to send.\n\nPlease verify and continue.");
                }

                ////***  If vendor recipients are required, remove REM below ****
                if (hiddenAreaOid == "2" && uCheck == true) {
                    var checkedNodes3 = list3.get_checkedItems();
                    if (checkedNodes3.length == 0) {
                        uCheck = false;
                        alert("Select at least 1 recipient from vendor to send.\n\nPlease verify and continue.");
                    }
                }

                if (textBox9.get_value().length == 0 && uCheck == true) {
                    uCheck = false;
                    alert("Message is empty.\n\nPlease type a message and continue.");
                    textBox9.focus();
                }

                if (uCheck == true) {
                    if (window.confirm("Send message.\n\n\Are you sure?\nClick OK to " + sender.get_text() + ".")) {
                        uCheck = true;
                    }
                    else {
                        uCheck = false;
                        eventArgs.set_cancel(true);
                    }
                }
                else {
                    eventArgs.set_cancel(true);
                }

                return uCheck;
            }

            function OnClientClick_RadButton2(sender, eventArgs) {
                var proceed = confirm("Cancel Send Message.\n\n    Are you sure?");
                if (proceed) {
                    GetRadWindow().Close("Cancel");
                }
                return false;
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
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
            EnableShadow="true" ShowContentDuringLoad="false" VisibleStatusbar="false" ReloadOnShow="true" VisibleOnPageLoad="false"
            Behaviors="Resize, Close, Move, Maximize" InitialBehaviors="Resize, Close, Move">
        </telerik:RadWindowManager>
        <telerik:RadAjaxLoadingPanel ID="LoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <div class="contentContainer">
                <table style="width: 100%;">
                    <tr>
                        <td>
                            <telerik:RadTabStrip RenderMode="Lightweight" ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" Font-Bold="true" Orientation="HorizontalTop"
                                SelectedIndex="0" Width="100%" OnTabClick="RadTabStrip1_TabClick" CausesValidation="false">
                                <Tabs>
                                    <telerik:RadTab PageViewID="RadPageView0" Value="RadPageView0" Text="Compose New Mesage" Selected="true">
                                    </telerik:RadTab>
                                    <telerik:RadTab PageViewID="RadPageView1" Value="RadPageView1" Text="Sent Items">
                                    </telerik:RadTab>
                                </Tabs>
                            </telerik:RadTabStrip>
                            <telerik:RadMultiPage ID="RadMultiPage1" SelectedIndex="0" Height="100%" runat="server" RenderSelectedPageOnly="true">
                                <telerik:RadPageView runat="server" ID="RadPageView0" Selected="true">
                                    <table>
                                        <tr>
                                            <td style="vertical-align: top;">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Panel ID="Panel1" runat="server">
                                                                <fieldset class="fieldset">
                                                                    <legend style="font-family: Arial; font-size: x-small; font-style: italic;">Vendor Recipients (optional)</legend>
                                                                    <telerik:RadListBox ID="RadListBox3" RenderMode="Lightweight" runat="server" Width="350px" Height="150px" SelectionMode="Multiple" Sort="Ascending" AllowDelete="false"
                                                                        CheckBoxes="true" ShowCheckAll="true">
                                                                    </telerik:RadListBox>
                                                                </fieldset>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <fieldset class="fieldset">
                                                                <legend style="font-family: Arial; font-size: x-small; font-style: italic;">HES Recipients</legend>
                                                                <telerik:RadListBox ID="RadListBox1" RenderMode="Lightweight" runat="server" Width="350px" Height="300px" SelectionMode="Multiple" Sort="Ascending" AllowDelete="false"
                                                                    CheckBoxes="true" ShowCheckAll="true">
                                                                    <ButtonSettings Position="Right" VerticalAlign="Middle" />
                                                                    <HeaderTemplate>
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <telerik:RadComboBox ID="RadComboBox8" runat="server" MarkFirstMatch="true" AllowCustomText="true" Sort="Ascending"
                                                                                        EnableLoadOnDemand="true" ShowMoreResultsBox="True" DropDownAutoWidth="enabled" LoadingMessage="Searching...Please wait"
                                                                                        Height="230px" Width="200px" OnClientSelectedIndexChanged="RadComboBox8_OnClientSelectedIndexChanged"
                                                                                        OnItemsRequested="RadComboBox8_ItemsRequested"
                                                                                        OpenDropDownOnLoad="false"
                                                                                        ShowDropDownOnTextboxClick="false"
                                                                                        HighlightTemplatedItems="true"
                                                                                        EnableVirtualScrolling="true"
                                                                                        Filter="StartsWith"
                                                                                        EmptyMessage="Search recipient...">
                                                                                        <HeaderTemplate>
                                                                                            <table style="width: 425px;">
                                                                                                <tr>
                                                                                                    <th style="width: 250px;">Name
                                                                                                    </th>
                                                                                                    <th style="width: 75px;">User ID
                                                                                                    </th>
                                                                                                    <th style="width: 50px; font-size: small;">Location
                                                                                                    </th>
                                                                                                    <th style="width: 50px; font-size: small;">Country
                                                                                                    </th>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <table style="width: 425px; border: 1px solid lightgray;">
                                                                                                <tr>
                                                                                                    <td style="width: 250px;">
                                                                                                        <%# DataBinder.Eval(Container,expression: "Attributes['FullName']")%>
                                                                                                    </td>
                                                                                                    <td style="width: 75px;">
                                                                                                        <%# DataBinder.Eval(Container,expression: "Attributes['UserID']")%>
                                                                                                    </td>
                                                                                                    <td style="width: 50px;">
                                                                                                        <%# DataBinder.Eval(Container,expression: "Attributes['CITY']")%>
                                                                                                    </td>
                                                                                                    <td style="width: 50px;">
                                                                                                        <%# DataBinder.Eval(Container,expression: "Attributes['CNTRY_NM']")%>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </ItemTemplate>
                                                                                    </telerik:RadComboBox>
                                                                                </td>
                                                                                <td>
                                                                                    <telerik:RadButton ID="RadButton13" runat="server" ButtonType="StandardButton" UseSubmitBehavior="true" RegisterWithScriptManager="true" Text="" Font-Size="X-Small"
                                                                                        OnClick="RadButton13_Click" SingleClick="true" SingleClickText="" Width="35px" Height="25px" ToolTip="Add Name">
                                                                                        <Icon PrimaryIconCssClass="rbAdd"></Icon>
                                                                                    </telerik:RadButton>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </HeaderTemplate>
                                                                </telerik:RadListBox>
                                                            </fieldset>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td style="vertical-align: top;">
                                                <table>
                                                    <tr>
                                                        <th style="vertical-align: top;">Message</th>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <telerik:RadTextBox ID="RadTextBox9" runat="server" TextMode="MultiLine" Width="410px" Height="300px" EmptyMessage="Type message here...">
                                                                <FocusedStyle BackColor="LightCyan" />
                                                            </telerik:RadTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: left;">
                                                            <telerik:RadButton RenderMode="Lightweight" ID="RadButton5" runat="server" Text="Send Message" BorderColor="Blue"
                                                                CausesValidation="true" SingleClick="true" SingleClickText="Processing..." OnClientClicking="OnClientClicking_RadButton5" OnClick="RadButton5_Click">
                                                                <Icon PrimaryIconCssClass="rbMail"></Icon>
                                                            </telerik:RadButton>
                                                            <telerik:RadButton ID="RadButton2" runat="server" Text="Cancel" SingleClick="true" CausesValidation="false" BorderColor="Blue"
                                                                RenderMode="Lightweight" OnClientClicked="OnClientClick_RadButton2">
                                                                <Icon PrimaryIconCssClass="rbCancel" />
                                                            </telerik:RadButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>

                                    </table>
                                </telerik:RadPageView>
                                <telerik:RadPageView runat="server" ID="RadPageView1">
                                    <telerik:RadGrid ID="RadGrid2" runat="server" AutoGenerateColumns="False"
                                        Width="100%" ShowStatusBar="true" AllowMultiRowSelection="True" AllowPaging="False"
                                        OnNeedDataSource="RadGrid2_NeedDataSource"
                                        OnItemDataBound="RadGrid2_ItemDataBound">
                                        <HeaderStyle HorizontalAlign="Center" Wrap="false" Font-Size="X-Small" Font-Bold="True" />
                                        <StatusBarSettings LoadingText="Please wait...Loading data" />
                                        <ClientSettings EnableRowHoverStyle="true">
                                            <Selecting AllowRowSelect="true" />
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="450px" />

                                            <Resizing AllowColumnResize="true" />
                                        </ClientSettings>
                                        <MasterTableView DataKeyNames="OID, CAR_OID" Width="100%" NoMasterRecordsText="No messages to display.">
                                            <Columns>
                                                <telerik:GridBoundColumn DataField="CTEAT_DTTM" HeaderText="Created Date" SortExpression="CTEAT_DTTM"
                                                    UniqueName="CTEAT_DTTM">
                                                    <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="CREATED_USER_NM" HeaderText="Created By" SortExpression="CREATED_USER_NM"
                                                    UniqueName="CREATED_USER_NM">
                                                    <HeaderStyle Width="150px" HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Sent_TO" UniqueName="Sent_TO" HeaderText="Sent To"
                                                    ReadOnly="true">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Message" HeaderText="Message" SortExpression="Message"
                                                    UniqueName="Message">
                                                    <ItemStyle Wrap="true" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="OID" HeaderText="OID" SortExpression="OID" UniqueName="OID"
                                                    Display="false">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Subject" UniqueName="Subject" HeaderText="Subject" Display="false"
                                                    ReadOnly="true">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </telerik:GridBoundColumn>
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>



                                </telerik:RadPageView>
                            </telerik:RadMultiPage>
                        </td>
                    </tr>
                </table>

            </div>
            <asp:HiddenField ID="HiddenCarOid" runat="server" />
            <asp:HiddenField ID="HiddenCarNbr" runat="server" />
            <asp:HiddenField ID="HiddenAreaOid" runat="server" />
            <asp:HiddenField ID="HiddenRecipientId" runat="server" />
            <asp:HiddenField ID="HiddenRecipientName" runat="server" />
            <asp:HiddenField ID="HiddenRecipientEmail" runat="server" />
            <asp:HiddenField ID="HiddenVendorNumber" runat="server" />
            <asp:HiddenField ID="HiddenVendorName" runat="server" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
