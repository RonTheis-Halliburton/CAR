<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Start.aspx.cs" Inherits="CAR.Forms.Start" %>

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

            function storeSplitButtonReference(sender, eventArgs) {
                splitButton = sender;
            }

            function OnClientClicked(sender, eventArgs) {
                if (eventArgs.IsSplitButtonClick() || !sender.get_commandName()) {
                    var currentLocation = $telerik.getBounds(sender.get_element());
                    contextMenu.showAt(currentLocation.x, currentLocation.y + currentLocation.height);
                } else if (sender.get_commandName() == "NewRequest") {
                    //NewRequest;
                    document.getElementById("Label1").innerHTML = "0";
                    document.getElementById("Label2").innerHTML = "0";
                } else {
                    //FromExistingRequest;
                    OpenInput();
                }
            }

            function storeContextMenuReference(sender, eventArgs) {
                contextMenu = sender;
            }

            function OnClientItemClicked(sender, eventArgs) {
                var itemText = eventArgs.get_item().get_text();

                if (itemText.toLowerCase() == "new request") {
                    document.getElementById("Label1").innerHTML = "0";
                    document.getElementById("Label2").innerHTML = "0";
                    splitButton.set_text("New Request");
                    splitButton.set_commandName("NewRequest");

                    var ajaxManager = $find("<%= RadAjaxPanel1.ClientID %>");
                    ajaxManager.ajaxRequest("NewRequest");

                } else if (itemText.toLowerCase() == "from existing request") {
                    OpenInput();
                    splitButton.set_text("From Existing Request");
                    splitButton.set_commandName("FromExistingRequest");
                }
            }

            function DateOnError(sender, eventArgs) {
                alert(" Invalid date!\n Please verify and try again.");
                sender.set_value('');
            }

            function OpenInput() {
                var oManager = GetRadWindowManager();

                strUrl = "InputBox.aspx";

                var oWindow = oManager.Open(strUrl, "oWndInputBox");
                oWindow.SetModal(true);
                oWindow.SetSize(400, 350);
                oWindow.MoveTo(10, 10);
                return false;
            }

            function CallBackFunction(oWnd, eventArgs) {
                //get the transferred arguments
                var arg = eventArgs.get_argument();
                var ajaxManager = $find("<%= RadAjaxPanel1.ClientID %>");

                if (arg) {
                    if (oWnd.get_name() == "oWndInputBox") {
                        var carNbrOid = arg.carNbrOid;
                        var carNbr = arg.carNbr;

                        document.getElementById("Label1").innerHTML = carNbrOid;
                        document.getElementById("Label2").innerHTML = carNbr;

                        ajaxManager.ajaxRequest(carNbrOid);

                    }
                    //else {
                    //   ajaxManager.ajaxRequest();
                    //}
                }
            }

            function RadComboBox3_OnClientSelectedIndexChanged(sender, eventArgs) {
                var item = eventArgs.get_item();
                document.getElementById('<%=HiddenOriginatorUserId.ClientID %>').value = item.get_attributes().getAttribute("UserId");
                document.getElementById('<%=HiddenOriginatorUserName.ClientID %>').value = item.get_attributes().getAttribute("FullName");
                document.getElementById('<%=HiddenOriginatorUserEmail.ClientID %>').value = item.get_attributes().getAttribute("Email");
            }

            function RadComboBox11_OnClientSelectedIndexChanged(sender, eventArgs) {
                var item = eventArgs.get_item();
                document.getElementById('<%=HiddenVendorNumber.ClientID %>').value = item.get_attributes().getAttribute("VendorNumber");
                document.getElementById('<%=HiddenVendorName.ClientID %>').value = item.get_attributes().getAttribute("VendorName");
            }

            function RadComboBox7_OnClientTextChange(sender, eventArgs) {
                var textBox7 = $find("<%= RadTextBox7.ClientID %>"); //Email
                textBox7.set_value("");

                document.getElementById('<%=HiddenIssuedToUserId.ClientID %>').value = "";
                document.getElementById('<%=HiddenIssuedToUserName.ClientID %>').value = sender.get_text();
                document.getElementById('<%=HiddenIssuedToUserEmail.ClientID %>').value = "";

                return;
            }

            function RadComboBox7_OnClientSelectedIndexChanged(sender, eventArgs) {
                var textBox7 = $find("<%= RadTextBox7.ClientID %>"); //Email
                var textBox10 = $find("<%= RadTextBox10.ClientID %>"); //City, State
                var radComboBox10 = $find("<%= RadComboBox10.ClientID %>");  //Country

                var item = eventArgs.get_item();
                document.getElementById('<%=HiddenIssuedToUserId.ClientID %>').value = item.get_attributes().getAttribute("UserId");
                document.getElementById('<%=HiddenIssuedToUserName.ClientID %>').value = item.get_attributes().getAttribute("FullName");
                document.getElementById('<%=HiddenIssuedToUserEmail.ClientID %>').value = item.get_attributes().getAttribute("Email");

                if (textBox7 != null) {
                    textBox7.set_value(item.get_attributes().getAttribute("Email"));
                }

                if (textBox10 != null) {
                    textBox10.set_value(item.get_attributes().getAttribute("CITY"));
                }

                if (radComboBox10 != null) {
                    var itemCombo = radComboBox10.findItemByText(item.get_attributes().getAttribute("CNTRY_NM"));
                    if (itemCombo) {
                        itemCombo.select();
                    }
                }
            }

            function RadComboBox8_OnClientSelectedIndexChanged(sender, eventArgs) {
                var item = eventArgs.get_item();
                document.getElementById('<%=HiddenRecipientId.ClientID %>').value = item.get_attributes().getAttribute("UserId");
                document.getElementById('<%=HiddenRecipientName.ClientID %>').value = item.get_attributes().getAttribute("FullName");
                document.getElementById('<%=HiddenRecipientEmail.ClientID %>').value = item.get_attributes().getAttribute("Email");
            }

            function OnClientClicking_RadButton1(sender, eventArgs) {
                var startPage;

                startPage = CheckStartPage();

                //alert(startPage);


                if (!startPage) {
                    eventArgs.set_cancel(true);
                }
                return;
            }

            function CheckStartPage() {
                var uReturn = true;
                var datePicker1 = $find("<%= RadDatePicker1.ClientID %>"); //Date issued
                var datePicker2 = $find("<%= RadDatePicker2.ClientID %>"); //Due Date

                var textBox12 = $find("<%= RadTextBox12.ClientID %>");

                var radComboBox2 = $find("<%= RadComboBox2.ClientID %>");  //Area
                var radComboBox3 = $find("<%= RadComboBox3.ClientID %>");  //Originator
                var radComboBox4 = $find("<%= RadComboBox4.ClientID %>");  //PSL
                var radComboBox5 = $find("<%= RadComboBox5.ClientID %>");  //Plant
                var radComboBox6 = $find("<%= RadComboBox6.ClientID %>");  //API, ISO
                var radComboBox9 = $find("<%= RadComboBox9.ClientID %>");  //Finding Type
                var radComboBox12 = $find("<%= RadComboBox12.ClientID %>");  //Category                

                if (uReturn) {
                    if (datePicker1.isEmpty()) {
                        uReturn = false;
                        datePicker1.get_dateInput().focus();
                        alert("Error... Required field.\n\nPlease select a Date Issued.");
                        return;
                    }
                }

                if (uReturn) {
                    if (datePicker2.isEmpty()) {
                        uReturn = false;
                        datePicker2.get_dateInput().focus();
                        alert("Error... Required field.\n\nPlease select a Due Date.");
                        return;
                    }
                }

                if (radComboBox9 != null) {
                    if (radComboBox9.get_value() == "" || radComboBox9.get_value() == "0") {
                        var comboInput9 = radComboBox9.get_inputDomElement();
                        uReturn = false;
                        alert("Error... Required field.\n\nPlease select a Finding Type.");
                        comboInput9.focus();
                        return;
                    }
                }

                if (radComboBox2 != null) {
                    if (radComboBox2.get_value() == "" || radComboBox2.get_value() == "0") {
                        var comboInput2 = radComboBox2.get_inputDomElement();
                        uReturn = false;
                        alert("Error... Required field.\n\nPlease select an Area.");
                        comboInput2.focus();
                        return;
                    }
                }

                if (radComboBox3 != null) {
                    if (radComboBox3.get_value() == "") {
                        var comboInput3 = radComboBox3.get_inputDomElement();
                        uReturn = false;
                        alert("Error... Required field.\n\nPlease select a name Originator.");
                        document.getElementById('<%=HiddenOriginatorUserId.ClientID %>').value = "";
                        document.getElementById('<%=HiddenOriginatorUserName.ClientID %>').value = "";
                        document.getElementById('<%=HiddenOriginatorUserEmail.ClientID %>').value = "";
                        comboInput3.focus();
                        return;
                    }
                }

                if (radComboBox4 != null) {
                    if (radComboBox4.get_value() == "" || radComboBox4.get_value() == "0") {
                        var comboInput4 = radComboBox4.get_inputDomElement();
                        uReturn = false;
                        alert("Error... Required field.\n\nPlease select a PSL.");
                        comboInput4.focus();
                        return;
                    }
                }

                if (radComboBox5 != null) {
                    if (radComboBox5.get_value() == "" || radComboBox5.get_value() == "0") {
                        var comboInput5 = radComboBox5.get_inputDomElement();
                        uReturn = false;
                        alert("Error... Required field.\n\nPlease select a Plant.");
                        comboInput5.focus();
                        return;
                    }
                }

                if (radComboBox6 != null) {
                    var checkedItems = radComboBox6.get_checkedItems();

                    if (checkedItems.length == 0) {
                        var comboInput6 = radComboBox6.get_inputDomElement();
                        uReturn = false;
                        alert("Error... Required field.\n\nPlease select API or ISO Reference.");
                        comboInput6.focus();
                        return;
                    }
                }

                if (radComboBox12 != null) {
                    if (radComboBox12.get_value() == "" || radComboBox12.get_value() == "0") {
                        var comboInput12 = radComboBox12.get_inputDomElement();
                        uReturn = false;
                        alert("Error... Required field.\n\nPlease select a Category.");
                        comboInput12.focus();
                        return;
                    }
                }

                if (textBox12.get_value().length == 0 && uReturn == true) {
                    uReturn = false;
                    alert("Error... Required field.\n\nPlease type a Description of Finding.")
                    textBox12.focus();
                }

                return uReturn;
            }

            function OnClientClicking_RadButton2(sender, eventArgs) {
                var vendorPage;

                vendorPage = CheckVendorPage();

                if (!vendorPage) {
                    eventArgs.set_cancel(true);
                }
                return;
            }

            function CheckVendorPage() {
                var radComboBox11 = $find("<%= RadComboBox11.ClientID %>");  //Vendor
                var uReturn = true;

                if (radComboBox11 != null) {
                    if (radComboBox11.get_value() == "") {
                        var comboInput11 = radComboBox11.get_inputDomElement();
                        uReturn = false;
                        alert("Error... Required field.\n\nPlease select find a valid supplier and select from the list.");
                        document.getElementById('<%=HiddenVendorNumber.ClientID %>').value = "";
                        document.getElementById('<%=HiddenVendorName.ClientID %>').value = "";
                        comboInput11.focus();
                        return;
                    }
                }
                return uReturn;
            }

            function getRadioButtonList() {
                var uReturn;
                var radioButtonList = document.getElementById("<%= RadioButtonList2.ClientID %>");
                var radioButtons = radioButtonList.getElementsByTagName("input");


                for (var i = 0; i < radioButtons.length; i++) {
                    if (radioButtons[i].checked) {
                        uReturn = radioButtons[i].value;
                        break;
                    }
                }
                return uReturn
            }

            function OnClientClicking_RadButton3(sender, eventArgs) {
                var issuedToPage;

                issuedToPage = CheckIssuedToPage();

                if (!issuedToPage) {
                    eventArgs.set_cancel(true);
                }
                return;
            }

            function CheckIssuedToPage() {

                var areaType = getRadioButtonList();

                var radComboBox7 = $find("<%= RadComboBox7.ClientID %>");  //Issued To
                var radComboBox10 = $find("<%= RadComboBox10.ClientID %>");  //Location Country

                var textBox7 = $find("<%= RadTextBox7.ClientID %>"); //Email

                var uReturn = true;

                if (radComboBox7 != null) {

                    if (areaType == "Hal") { //Halliburton recipients

                        if (radComboBox7.get_value() == "") {
                            alert("Error... Invalid recipient.\n\nPlease select a valid Halliburton recipient.");
                            uReturn = false;
                        }
                    }
                    else {
                        //Vendor recipients
                        if (radComboBox7.get_text().length == 0 || radComboBox7.get_text() == "Type a name to search...") {
                            alert("Error... Required field.\n\nPlease select or type in Full Name.");
                            uReturn = false;
                        }
                    }

                    if (uReturn == false) {
                        var comboInput7 = radComboBox7.get_inputDomElement();

                        document.getElementById('<%=HiddenIssuedToUserId.ClientID %>').value = "";
                        document.getElementById('<%=HiddenIssuedToUserName.ClientID %>').value = "";
                        document.getElementById('<%=HiddenIssuedToUserEmail.ClientID %>').value = "";

                        comboInput7.focus();
                        return;
                    }

                 <%--   ///if (radComboBox7.get_value() == "") {
                    if (radComboBox7.get_text().length ==0 || radComboBox7.get_text() == "Type a name to search...") {
                        var comboInput7 = radComboBox7.get_inputDomElement();
                        uReturn = false;
                        alert("Error... Required field.\n\nPlease select or type in Full Name.");

                        document.getElementById('<%=HiddenIssuedToUserId.ClientID %>').value = "";
                        document.getElementById('<%=HiddenIssuedToUserName.ClientID %>').value = "";
                        document.getElementById('<%=HiddenIssuedToUserEmail.ClientID %>').value = "";

                        comboInput7.focus();
                        return;
                    }--%>



                }

                if (textBox7 != null && uReturn == true) {
                    if (textBox7.get_value().length == 0) {
                        uReturn = false;
                        alert("Error... Required field.\n\nPlease enter an E-mail address.");
                        textBox7.focus();
                        return;
                    }
                }

                if (radComboBox10 != null && uReturn == true) {
                    if (radComboBox10.get_value() == "" || radComboBox10.get_value() == "0") {
                        var comboInput10 = radComboBox10.get_inputDomElement();
                        uReturn = false;
                        alert("Error... Required field.\n\nPlease select Location/Country.");

                        comboInput10.focus();
                        return;
                    }
                }

                return uReturn;
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
                    alert("Select at least 1 recipient from Halliburton to send.\n\nPlease verify and continue.");
                }

                if (hiddenAreaOid == "2") {
                    var checkedNodes3 = list3.get_checkedItems();
                    if (checkedNodes3.length == 0) {
                        uCheck = false;
                        alert("Select at least 1 recipient from supplier to send.\n\nPlease verify and continue.");
                    }
                }

                if (textBox9.get_value().length == 0 && uCheck == true) {
                    uCheck = false;
                    alert("Message is empty.\n\nPlease type a message and continue.")
                    textBox9.focus();
                }

                if (uCheck == true) {
                    if (window.confirm("Create CAR and Send notification.\n\n\Are you sure?\nClick OK to " + sender.get_text() + ".")) {
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

            function OnClientClicked_RadButton7(sender, eventArgs) {
                var rowOid = document.getElementById("Label1").innerHTML;

                var oManager = GetRadWindowManager();
                var urlForm = "Edit.aspx?OID=" + rowOid + "&Car_Nbr=" + sender.get_value() + "&Status_Nm=Awaiting Response";

                var oWindow = oManager.Open(urlForm, "oEditDialog");
                oWindow.SetModal(true);
                oWindow.MoveTo(0, 0);
                oWindow.Maximize();
                //oWindow.SetSize(1000, 700);
                oWindow.set_centerIfModal(false);
            }

            function OnValueChanging_RadTextBox7(sender, eventArgs) {
                var value = eventArgs.get_newValue();
                var trimmed = value.replace(/^\s+|\s+$/g, '');
                eventArgs.set_newValue(trimmed);
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
            Behaviors="Resize, Close, Move, Maximize" InitialBehaviors="Resize, Close, Move" OnClientClose="CallBackFunction">
        </telerik:RadWindowManager>
        <telerik:RadAjaxLoadingPanel ID="LoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" OnAjaxRequest="RadAjaxPanel1_AjaxRequest">
            <div class="contentContainer">
                <fieldset class="fieldset">
                    <legend style="font-family: Arial; font-style: italic;">Create Corrective Action Request</legend>
                    <table>
                        <tr>
                            <td style="vertical-align: top;">
                                <telerik:RadTabStrip RenderMode="Lightweight" ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" Font-Bold="true" Orientation="HorizontalTop"
                                    SelectedIndex="0" Width="100%" OnTabClick="RadTabStrip1_TabClick" CausesValidation="false">
                                    <Tabs>
                                        <telerik:RadTab PageViewID="RadPageView0" Value="RadPageView0" Text="Start" Selected="true">
                                        </telerik:RadTab>
                                        <telerik:RadTab PageViewID="RadPageView1" Value="RadPageView1" Text="Supplier">
                                        </telerik:RadTab>
                                        <telerik:RadTab PageViewID="RadPageView2" Value="RadPageView2" Text="Issued To">
                                        </telerik:RadTab>
                                        <telerik:RadTab PageViewID="RadPageView3" Value="RadPageView3" Text="Notification">
                                        </telerik:RadTab>
                                        <telerik:RadTab PageViewID="RadPageView4" Value="RadPageView4" Text="Finish">
                                        </telerik:RadTab>
                                    </Tabs>
                                </telerik:RadTabStrip>
                                <telerik:RadMultiPage ID="RadMultiPage1" SelectedIndex="0" Height="100%" runat="server" RenderSelectedPageOnly="true">
                                    <telerik:RadPageView runat="server" ID="RadPageView0" Selected="true" DefaultButton="RadButton1">
                                        <table>
                                            <tr>
                                                <td>
                                                    <fieldset class="fieldset">
                                                        <legend>
                                                            <telerik:RadButton RenderMode="Lightweight" EnableSplitButton="true" ID="SplitButton" AutoPostBack="false" Height="19px" Width="200px"
                                                                runat="server" Text="New Request" OnClientClicked="OnClientClicked" OnClientLoad="storeSplitButtonReference">
                                                            </telerik:RadButton>
                                                            <telerik:RadContextMenu ID="RadContextMenu1" runat="server" OnClientItemClicked="OnClientItemClicked" OnClientLoad="storeContextMenuReference">
                                                                <Items>
                                                                    <telerik:RadMenuItem Text="New Request">
                                                                    </telerik:RadMenuItem>
                                                                    <telerik:RadMenuItem Text="From Existing Request">
                                                                    </telerik:RadMenuItem>
                                                                </Items>
                                                            </telerik:RadContextMenu>
                                                        </legend>
                                                        <table>
                                                            <tr>
                                                                <th>Originator</th>
                                                                <td>
                                                                    <telerik:RadComboBox ID="RadComboBox3" runat="server" MarkFirstMatch="true" AllowCustomText="true" Sort="Ascending"
                                                                        EnableLoadOnDemand="true" ShowMoreResultsBox="True" DropDownAutoWidth="enabled" LoadingMessage="Searching...Please wait"
                                                                        Height="200px" Width="200px" OnClientSelectedIndexChanged="RadComboBox3_OnClientSelectedIndexChanged"
                                                                        OnItemsRequested="RadComboBox3_ItemsRequested"
                                                                        OpenDropDownOnLoad="false"
                                                                        ShowDropDownOnTextboxClick="false"
                                                                        HighlightTemplatedItems="true"
                                                                        EnableVirtualScrolling="true"
                                                                        EmptyMessage="Search recipients......">
                                                                        <HeaderTemplate>
                                                                            <table style="width: 590px;">
                                                                                <tr>
                                                                                    <th style="width: 200px; text-align: left;">Name
                                                                                    </th>
                                                                                    <th style="width: 75px; text-align: left;">User ID
                                                                                    </th>
                                                                                    <th style="width: 150px; text-align: left;">Location
                                                                                    </th>
                                                                                    <th style="width: 150px; text-align: left;">Country
                                                                                    </th>
                                                                                </tr>
                                                                            </table>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <table style="width: 590px; border: 1px solid lightgray;">
                                                                                <tr>
                                                                                    <td style="width: 200px; text-align: left;">
                                                                                        <%# DataBinder.Eval(Container,expression: "Attributes['FullName']")%>
                                                                                    </td>
                                                                                    <td style="width: 75px; text-align: left;">
                                                                                        <%# DataBinder.Eval(Container,expression: "Attributes['UserID']")%>
                                                                                    </td>
                                                                                    <td style="width: 150px; text-align: left;">
                                                                                        <%# DataBinder.Eval(Container,expression: "Attributes['CITY']")%>
                                                                                    </td>
                                                                                    <td style="width: 150px; text-align: left;">
                                                                                        <%# DataBinder.Eval(Container,expression: "Attributes['CNTRY_NM']")%>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </telerik:RadComboBox>
                                                                </td>
                                                                <th>Audit Number
                                                                </th>
                                                                <td>
                                                                    <telerik:RadTextBox ID="RadTextBox1" runat="server" EmptyMessage="(if applicable)" Width="200px" MaxLength="50">
                                                                        <FocusedStyle BackColor="LightCyan" />
                                                                    </telerik:RadTextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <th>Date Issued</th>
                                                                <td>
                                                                    <telerik:RadDatePicker ID="RadDatePicker1" runat="server" PopupDirection="TopLeft" Width="200px">
                                                                        <DateInput ID="DateInput1" runat="server" ForeColor="Black" Font-Names="Arial"
                                                                            ClientEvents-OnError="DateOnError" EmptyMessage="Date Issued">
                                                                        </DateInput>
                                                                        <Calendar ID="Calendar1" runat="server" ShowRowHeaders="false">
                                                                            <WeekendDayStyle BackColor="yellow" />
                                                                        </Calendar>
                                                                    </telerik:RadDatePicker>
                                                                </td>
                                                                <th>Q Note Number
                                                                </th>
                                                                <td>
                                                                    <telerik:RadTextBox ID="RadTextBox2" runat="server" EmptyMessage="(if applicable)" Width="200px" MaxLength="50">
                                                                        <FocusedStyle BackColor="LightCyan" />
                                                                    </telerik:RadTextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <th>Due Date</th>
                                                                <td>
                                                                    <telerik:RadDatePicker ID="RadDatePicker2" runat="server" PopupDirection="TopLeft" Width="200px">
                                                                        <DateInput ID="DateInput2" runat="server" ForeColor="Black" Font-Names="Arial"
                                                                            ClientEvents-OnError="DateOnError" EmptyMessage="Due Date">
                                                                        </DateInput>
                                                                        <Calendar ID="Calendar2" runat="server" ShowRowHeaders="false">
                                                                            <WeekendDayStyle BackColor="yellow" />
                                                                        </Calendar>
                                                                    </telerik:RadDatePicker>
                                                                </td>
                                                                <th>CPI Number
                                                                </th>
                                                                <td>
                                                                    <telerik:RadTextBox ID="RadTextBox3" runat="server" EmptyMessage="(if applicable)" Width="200px" MaxLength="50">
                                                                        <FocusedStyle BackColor="LightCyan" />
                                                                    </telerik:RadTextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <th>Finding Type</th>
                                                                <td>
                                                                    <telerik:RadComboBox ID="RadComboBox9" runat="server" Width="200px" Height="200px" Filter="Contains"
                                                                        EmptyMessage="Select Finding Type..." MarkFirstMatch="true" DropDownAutoWidth="Enabled">
                                                                    </telerik:RadComboBox>
                                                                </td>
                                                                <th>Material Number
                                                                </th>
                                                                <td>
                                                                    <telerik:RadTextBox ID="RadTextBox4" runat="server" EmptyMessage="(if applicable)" Width="200px" MaxLength="50">
                                                                        <FocusedStyle BackColor="LightCyan" />
                                                                    </telerik:RadTextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <th>Area</th>
                                                                <td>
                                                                    <telerik:RadComboBox ID="RadComboBox2" runat="server" Width="200px" Height="200px" Filter="Contains" EmptyMessage="Select Area..." MarkFirstMatch="true" DropDownAutoWidth="Enabled">
                                                                    </telerik:RadComboBox>
                                                                </td>
                                                                <th>Purchase Order Number
                                                                </th>
                                                                <td>
                                                                    <telerik:RadTextBox ID="RadTextBox5" runat="server" EmptyMessage="(if applicable)" Width="200px" MaxLength="50">
                                                                        <FocusedStyle BackColor="LightCyan" />
                                                                    </telerik:RadTextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <th>PSL</th>
                                                                <td>
                                                                    <telerik:RadComboBox ID="RadComboBox4" runat="server" Width="200px" Height="200px" Filter="Contains" EmptyMessage="Select PSL..." MarkFirstMatch="true" DropDownAutoWidth="Enabled">
                                                                    </telerik:RadComboBox>
                                                                </td>
                                                                <th>Production Order Number
                                                                </th>
                                                                <td>
                                                                    <telerik:RadTextBox ID="RadTextBox6" runat="server" EmptyMessage="(if applicable)" Width="200px" MaxLength="50">
                                                                        <FocusedStyle BackColor="LightCyan" />
                                                                    </telerik:RadTextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <th>Plant</th>
                                                                <td>
                                                                    <telerik:RadComboBox ID="RadComboBox5" runat="server" Width="200px" Height="200px" Filter="Contains" EmptyMessage="Select Plant..." MarkFirstMatch="true" DropDownAutoWidth="Enabled">
                                                                    </telerik:RadComboBox>
                                                                </td>
                                                                <th>API Audit Number
                                                                </th>
                                                                <td>
                                                                    <telerik:RadTextBox ID="RadTextBox8" runat="server" EmptyMessage="(if applicable)" Width="200px" MaxLength="50">
                                                                        <FocusedStyle BackColor="LightCyan" />
                                                                    </telerik:RadTextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <th>API or ISO Reference</th>
                                                                <td>
                                                                    <telerik:RadComboBox ID="RadComboBox6" runat="server" Width="200px" Height="200px" DropDownAutoWidth="Enabled" CheckBoxes="true" EnableCheckAllItemsCheckBox="true">
                                                                    </telerik:RadComboBox>
                                                                </td>
                                                                 <th>Maintenance Order Number
                                                                </th>
                                                                <td>
                                                                    <telerik:RadTextBox ID="RadTextBox11" runat="server" EmptyMessage="(if applicable)" Width="200px" MaxLength="50">
                                                                        <FocusedStyle BackColor="LightCyan" />
                                                                    </telerik:RadTextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <th>Category</th>
                                                                <td>
                                                                    <telerik:RadComboBox ID="RadComboBox12" runat="server" Width="200px" Height="200px" Filter="Contains"
                                                                        EmptyMessage="Select Category..." MarkFirstMatch="true" DropDownAutoWidth="Enabled">
                                                                    </telerik:RadComboBox>
                                                                </td>
                                                                 <th>Equipment Number
                                                                </th>
                                                                <td>
                                                                    <telerik:RadTextBox ID="RadTextBox14" runat="server" EmptyMessage="(if applicable)" Width="200px" MaxLength="50">
                                                                        <FocusedStyle BackColor="LightCyan" />
                                                                    </telerik:RadTextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <th>Description of<br />
                                                                    Finding</th>
                                                                <td colspan="3">
                                                                    <telerik:RadTextBox ID="RadTextBox12" runat="server" EmptyMessage="Type Description of Finding..." Height="75px" Width="525px" TextMode="MultiLine">
                                                                        <FocusedStyle BackColor="LightCyan" />
                                                                    </telerik:RadTextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <th>Description of<br />
                                                                    Improvement</th>
                                                                <td colspan="3">
                                                                    <telerik:RadTextBox ID="RadTextBox13" runat="server" EmptyMessage="If applicable, type Description of Improvement..." Height="75px" Width="525px" TextMode="MultiLine">
                                                                        <FocusedStyle BackColor="LightCyan" />
                                                                    </telerik:RadTextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right;">
                                                    <telerik:RadButton RenderMode="Lightweight" ID="RadButton1" runat="server" Text="Confirm Next" BorderColor="Blue"
                                                        CausesValidation="true" SingleClick="true" SingleClickText="Processing..." OnClientClicking="OnClientClicking_RadButton1" OnClick="RadButton1_Click">
                                                        <Icon SecondaryIconCssClass="rbNext"></Icon>
                                                    </telerik:RadButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </telerik:RadPageView>
                                    <telerik:RadPageView runat="server" ID="RadPageView1" DefaultButton="RadButton2">
                                        <table>
                                            <tr>
                                                <th>Supplier Number</th>
                                                <td>
                                                    <telerik:RadComboBox ID="RadComboBox11" runat="server" MarkFirstMatch="true" AllowCustomText="true" Sort="Ascending"
                                                        EnableLoadOnDemand="true" ShowMoreResultsBox="True" DropDownAutoWidth="enabled" LoadingMessage="Searching...Please wait"
                                                        Height="200px" Width="350px" AutoPostBack="true"
                                                        OnItemsRequested="RadComboBox11_ItemsRequested" OnClientSelectedIndexChanged="RadComboBox11_OnClientSelectedIndexChanged"
                                                        OpenDropDownOnLoad="false"
                                                        ShowDropDownOnTextboxClick="false"
                                                        HighlightTemplatedItems="true"
                                                        EnableVirtualScrolling="true"
                                                        Filter="Contains"
                                                        EmptyMessage="Enter a supplier number to search...">
                                                        <HeaderTemplate>
                                                            <table style="width: 350px;">
                                                                <tr>
                                                                    <th style="width: 75px; text-align: left;">Number
                                                                    </th>
                                                                    <th style="width: 275px; text-align: left;">Name
                                                                    </th>
                                                                </tr>
                                                            </table>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <table style="width: 350px; border: 1px solid lightgray;">
                                                                <tr>
                                                                    <td style="width: 75px; text-align: left;">
                                                                        <%# DataBinder.Eval(Container,expression: "Attributes['VendorNumber']")%>
                                                                    </td>
                                                                    <td style="width: 275px; text-align: left;">
                                                                        <%# DataBinder.Eval(Container,expression: "Attributes['VendorName']")%>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                    </telerik:RadComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td style="text-align: right;">
                                                    <telerik:RadButton RenderMode="Lightweight" ID="RadButton2" runat="server" Text="Confirm Next" Font-Bold="true" BorderStyle="Solid" BorderWidth="1px" BorderColor="Blue"
                                                        CausesValidation="true" SingleClick="true" SingleClickText="Processing..." OnClientClicking="OnClientClicking_RadButton2" OnClick="RadButton2_Click">
                                                        <Icon SecondaryIconCssClass="rbNext"></Icon>
                                                    </telerik:RadButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </telerik:RadPageView>
                                    <telerik:RadPageView runat="server" ID="RadPageView2" DefaultButton="RadButton3">
                                        <table>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <asp:RadioButtonList ID="RadioButtonList2" runat="server" RepeatDirection="Horizontal"
                                                        AutoPostBack="true" OnSelectedIndexChanged="RadioButtonList2_SelectedIndexChanged">
                                                        <asp:ListItem Value="Hal" Text="Halliburton">Halliburton Recipients</asp:ListItem>
                                                        <asp:ListItem Value="Sup" Text="Supplier">Supplier Recipients</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>Full Name</th>
                                                <td>
                                                    <telerik:RadComboBox ID="RadComboBox7" runat="server" MarkFirstMatch="true" AllowCustomText="true" Sort="Ascending"
                                                        EnableLoadOnDemand="true" ShowMoreResultsBox="True" DropDownAutoWidth="enabled" LoadingMessage="Searching...Please wait" EmptyMessage="Type a name to search..."
                                                        Height="200px" Width="350px" OnClientSelectedIndexChanged="RadComboBox7_OnClientSelectedIndexChanged"
                                                        OnItemsRequested="RadComboBox7_ItemsRequested" OnClientTextChange="RadComboBox7_OnClientTextChange"
                                                        OpenDropDownOnLoad="false"
                                                        ShowDropDownOnTextboxClick="false"
                                                        HighlightTemplatedItems="true"
                                                        EnableVirtualScrolling="true">
                                                        <HeaderTemplate>
                                                            <table style="width: 590px;">
                                                                <tr>
                                                                    <th style="width: 200px; text-align: left;">Name
                                                                    </th>
                                                                    <th style="width: 75px; text-align: left;">User ID
                                                                    </th>
                                                                    <th style="width: 150px; text-align: left;">Location
                                                                    </th>
                                                                    <th style="width: 150px; text-align: left;">Country
                                                                    </th>
                                                                </tr>
                                                            </table>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <table style="width: 590px; border: 1px solid lightgray;">
                                                                <tr>
                                                                    <td style="width: 200px; text-align: left;">
                                                                        <%# DataBinder.Eval(Container,expression: "Attributes['FullName']")%>
                                                                    </td>
                                                                    <td style="width: 75px; text-align: left;">
                                                                        <%# DataBinder.Eval(Container,expression: "Attributes['UserID']")%>
                                                                    </td>
                                                                    <td style="width: 150px; text-align: left;">
                                                                        <%# DataBinder.Eval(Container,expression: "Attributes['CITY']")%>
                                                                    </td>
                                                                    <td style="width: 150px; text-align: left;">
                                                                        <%# DataBinder.Eval(Container,expression: "Attributes['CNTRY_NM']")%>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                    </telerik:RadComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>E-Mail</th>
                                                <td>
                                                    <telerik:RadTextBox ID="RadTextBox7" runat="server" Width="350px">
                                                        <ClientEvents OnValueChanging="OnValueChanging_RadTextBox7" />
                                                    </telerik:RadTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <asp:RegularExpressionValidator ID="emailValidator" runat="server" Display="Dynamic" ForeColor="Red"
                                                        ErrorMessage="Please, enter valid e-mail address." ValidationExpression="^[\w\.\-]+@[a-zA-Z0-9\-]+(\.[a-zA-Z0-9\-]{1,})*(\.[a-zA-Z]{2,3}){1,2}$"
                                                        ControlToValidate="RadTextBox7">
                                                    </asp:RegularExpressionValidator>
                                                    <asp:RequiredFieldValidator ID="Requiredfieldvalidator1" runat="server" Display="Dynamic" ForeColor="Red"
                                                        ControlToValidate="RadTextBox7" ErrorMessage="Please, enter an e-mail!" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>Location/Country</th>
                                                <td>
                                                    <telerik:RadComboBox ID="RadComboBox10" runat="server" Width="350px" Height="200px" Filter="Contains"
                                                        EmptyMessage="Select Country..." MarkFirstMatch="true" DropDownAutoWidth="Enabled">
                                                    </telerik:RadComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>Location/City State</th>
                                                <td>
                                                    <telerik:RadTextBox ID="RadTextBox10" runat="server" EmptyMessage="(if applicable)" Width="350px" MaxLength="50">
                                                        <FocusedStyle BackColor="LightCyan" />
                                                    </telerik:RadTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td style="text-align: right;">
                                                    <telerik:RadButton RenderMode="Lightweight" ID="RadButton3" runat="server" Text="Confirm Next" BorderColor="Blue"
                                                        CausesValidation="true" SingleClick="true" SingleClickText="Processing..." OnClientClicking="OnClientClicking_RadButton3" OnClick="RadButton3_Click">
                                                        <Icon SecondaryIconCssClass="rbNext"></Icon>
                                                    </telerik:RadButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </telerik:RadPageView>
                                    <telerik:RadPageView runat="server" ID="RadPageView3">
                                        <table>
                                            <tr>
                                                <td style="vertical-align: top;">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Panel ID="Panel1" runat="server">
                                                                    <fieldset class="fieldset">
                                                                        <legend style="font-family: Arial; font-size: x-small; font-style: italic;">Supplier Recipients</legend>
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
                                                                    <telerik:RadListBox ID="RadListBox1" RenderMode="Lightweight" runat="server" Width="350px" Height="160px" SelectionMode="Multiple" Sort="Ascending" AllowDelete="false"
                                                                        CheckBoxes="true" ShowCheckAll="true">
                                                                        <ButtonSettings Position="Right" VerticalAlign="Middle" />
                                                                        <HeaderTemplate>
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                        <telerik:RadComboBox ID="RadComboBox8" runat="server" MarkFirstMatch="true" AllowCustomText="true" Sort="Ascending"
                                                                                            EnableLoadOnDemand="true" ShowMoreResultsBox="True" DropDownAutoWidth="enabled" LoadingMessage="Searching...Please wait"
                                                                                            Height="200px" Width="200px" OnClientSelectedIndexChanged="RadComboBox8_OnClientSelectedIndexChanged"
                                                                                            OnItemsRequested="RadComboBox8_ItemsRequested"
                                                                                            OpenDropDownOnLoad="false"
                                                                                            ShowDropDownOnTextboxClick="false"
                                                                                            HighlightTemplatedItems="true"
                                                                                            EnableVirtualScrolling="true"
                                                                                            Filter="StartsWith"
                                                                                            EmptyMessage="Search recipients...">
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
                                                                                        <telerik:RadButton ID="RadButton4" runat="server" ButtonType="StandardButton" UseSubmitBehavior="true" RegisterWithScriptManager="true" Text="" Font-Size="X-Small"
                                                                                            OnClick="RadButton4_Click" SingleClick="true" SingleClickText="" Width="35px" Height="25px" ToolTip="Add Name">
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
                                                                <telerik:RadTextBox ID="RadTextBox9" runat="server" TextMode="MultiLine" Width="400px" Height="300px" EmptyMessage="Type message here...">
                                                                    <FocusedStyle BackColor="LightCyan" />
                                                                </telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td style="text-align: left;">
                                                    <telerik:RadButton RenderMode="Lightweight" ID="RadButton5" runat="server" Text="Create CAR and Send" BorderColor="Blue"
                                                        CausesValidation="true" SingleClick="true" SingleClickText="Processing..." OnClientClicking="OnClientClicking_RadButton5" OnClick="RadButton5_Click">
                                                        <Icon SecondaryIconCssClass="rbNext"></Icon>
                                                    </telerik:RadButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </telerik:RadPageView>
                                    <telerik:RadPageView runat="server" ID="RadPageView4">
                                        <table>
                                            <tr>
                                                <th>Corrective Action Request:  
                                                </th>
                                                <td>
                                                    <asp:Label ID="Label3" runat="server" Text="Label"></asp:Label>
                                                </td>
                                            </tr>

                                            <tr>
                                                <th style="vertical-align: top;">Request sent to:</th>
                                                <td>
                                                    <telerik:RadListBox ID="RadListBox2" runat="server" Width="260px" Height="150px"></telerik:RadListBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <div class="lineSeparator">
                                                        <!-- -->
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <telerik:RadButton ID="RadButton6" runat="server" Text="Start New Request " UseSubmitBehavior="true" BorderColor="Blue"
                                                        OnClick="RadButton6_Click" SingleClick="true" SingleClickText="Processing...">
                                                    </telerik:RadButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <telerik:RadButton ID="RadButton7" runat="server" Text="Edit  Request " Value="0" UseSubmitBehavior="true" BorderColor="Blue"
                                                        SingleClick="true" SingleClickText="Processing..." OnClientClicked="OnClientClicked_RadButton7">
                                                    </telerik:RadButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </telerik:RadPageView>
                                </telerik:RadMultiPage>
                            </td>
                            <td style="vertical-align: top;">
                                <asp:Panel ID="Panel2" runat="server">
                                    <fieldset class="fieldset">
                                        <legend style="font-family: Arial; font-size: x-small; font-style: italic;">Correct Action Preview</legend>
                                        <table>
                                            <tr>
                                                <th>Originator</th>
                                                <td>: 
                                            <asp:Literal ID="Literal2" runat="server"></asp:Literal>
                                                    (<asp:Literal ID="Literal1" runat="server"></asp:Literal>)
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>Date Issued</th>
                                                <td>: 
                                            <asp:Literal ID="Literal3" runat="server"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>Due Date</th>
                                                <td>: 
                                            <asp:Literal ID="Literal4" runat="server"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>Finding Type</th>
                                                <td>: 
                                            <asp:Literal ID="Literal5" runat="server"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>Area</th>
                                                <td>: 
                                            <asp:Literal ID="Literal6" runat="server"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>PSL</th>
                                                <td>: 
                                            <asp:Literal ID="Literal7" runat="server"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>Plant</th>
                                                <td>: 
                                            <asp:Literal ID="Literal8" runat="server"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>Description of<br />
                                                    Finding</th>
                                                <td>
                                                    <div style="height: auto; width: 350px; vertical-align: top; font-family: Arial; word-wrap: break-word; border: solid; border-width: thin; border-color: lightgray;" runat="server" id="Div1">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>Description of<br />
                                                    Improvement</th>
                                                <td>
                                                    <div style="height: auto; width: 350px; vertical-align: top; font-family: Arial; word-wrap: break-word; border: solid; border-width: thin; border-color: lightgray;" runat="server" id="Div2">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>API / ISO Reference</th>
                                                <td>: 
                                              <telerik:RadComboBox ID="RadComboBox1" runat="server" Width="350px" Height="200px" Filter="None" DropDownAutoWidth="Enabled">
                                              </telerik:RadComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>Audit Number</th>
                                                <td>: 
                                            <asp:Literal ID="Literal13" runat="server"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>Q Note Number</th>
                                                <td>: 
                                            <asp:Literal ID="Literal14" runat="server"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>CPI Number</th>
                                                <td>: 
                                            <asp:Literal ID="Literal15" runat="server"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>Material Number</th>
                                                <td>: 
                                            <asp:Literal ID="Literal16" runat="server"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>Purchase Order Number</th>
                                                <td>: 
                                            <asp:Literal ID="Literal17" runat="server"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>Production Order Number</th>
                                                <td>: 
                                            <asp:Literal ID="Literal18" runat="server"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>API Audit Number</th>
                                                <td>: 
                                            <asp:Literal ID="Literal20" runat="server"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>Maintenance Order Number</th>
                                                <td>: 
                                            <asp:Literal ID="Literal24" runat="server"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>Equipment Number</th>
                                                <td>: 
                                            <asp:Literal ID="Literal25" runat="server"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>Category</th>
                                                <td>: 
                                            <asp:Literal ID="Literal9" runat="server"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>Supplier Number</th>
                                                <td>: 
                                            <asp:Literal ID="Literal10" runat="server"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>Supplier Name</th>
                                                <td>:
                                                <asp:Literal ID="Literal11" runat="server"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>Issued To</th>
                                                <td>: 
                                            <asp:Literal ID="Literal12" runat="server"></asp:Literal>
                                                    (<asp:Literal ID="Literal21" runat="server"></asp:Literal>)
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>Issued To E-mail</th>
                                                <td>:                                             
                                                    <asp:Literal ID="Literal19" runat="server"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>Location/Country</th>
                                                <td>: 
                                            <asp:Literal ID="Literal22" runat="server"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>Location/City State</th>
                                                <td>: 
                                            <asp:Literal ID="Literal23" runat="server"></asp:Literal>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <asp:Label ID="Label1" runat="server" Text="0" Font-Size="X-Small"></asp:Label>
                <asp:Label ID="Label2" runat="server" Text="0" Font-Size="X-Small"></asp:Label>
            </div>

            <asp:HiddenField ID="HiddenAreaOid" runat="server" />

            <asp:HiddenField ID="HiddenCountryOid" runat="server" />
            <asp:HiddenField ID="HiddenOriginatorUserId" runat="server" />
            <asp:HiddenField ID="HiddenOriginatorUserName" runat="server" />
            <asp:HiddenField ID="HiddenOriginatorUserEmail" runat="server" />

            <asp:HiddenField ID="HiddenVendorNumber" runat="server" />
            <asp:HiddenField ID="HiddenVendorName" runat="server" />

            <asp:HiddenField ID="HiddenIssuedToUserId" runat="server" />
            <asp:HiddenField ID="HiddenIssuedToUserName" runat="server" />
            <asp:HiddenField ID="HiddenIssuedToUserEmail" runat="server" />

            <asp:HiddenField ID="HiddenRecipientId" runat="server" />
            <asp:HiddenField ID="HiddenRecipientName" runat="server" />
            <asp:HiddenField ID="HiddenRecipientEmail" runat="server" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
