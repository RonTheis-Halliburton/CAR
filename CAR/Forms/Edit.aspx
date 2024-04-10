<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="CAR.Forms.Edit" %>

<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>
<script type="text/javascript" src="JS/TopLocation.js"></script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edit</title>
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
                oWindow.SetSize(400, 220);
                oWindow.MoveTo(10, 10);
                return false;
            }

            function RadComboBox3_OnClientSelectedIndexChanged(sender, eventArgs) {
                var item = eventArgs.get_item();
                document.getElementById('<%=HiddenOriginatorUserId.ClientID %>').value = item.get_attributes().getAttribute("UserId");
                document.getElementById('<%=HiddenOriginatorUserName.ClientID %>').value = item.get_attributes().getAttribute("FullName");
                document.getElementById('<%=HiddenOriginatorUserEmail.ClientID %>').value = item.get_attributes().getAttribute("Email");
            }

            function RadComboBox7_OnClientSelectedIndexChanged(sender, eventArgs) {
                var textBox10 = $find("<%= RadTextBox10.ClientID %>"); //City, State
                var radComboBox10 = $find("<%= RadComboBox10.ClientID %>");  //Country

                var item = eventArgs.get_item();
                document.getElementById('<%=HiddenIssuedToUserId.ClientID %>').value = item.get_attributes().getAttribute("UserId");
                document.getElementById('<%=HiddenIssuedToUserName.ClientID %>').value = item.get_attributes().getAttribute("FullName");
                document.getElementById('<%=HiddenIssuedToUserEmail.ClientID %>').value = item.get_attributes().getAttribute("Email");

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

            function RadComboBox11_OnClientSelectedIndexChanged(sender, eventArgs) {
                var item = eventArgs.get_item();
                document.getElementById('<%=HiddenVendorNumber.ClientID %>').value = item.get_attributes().getAttribute("VendorNumber");
                document.getElementById('<%=HiddenVendorName.ClientID %>').value = item.get_attributes().getAttribute("VendorName");
            }

            function RadComboBox13_OnClientSelectedIndexChanged(sender, eventArgs) {
                var item = eventArgs.get_item();
                document.getElementById('<%=HiddenResponsibleUserId.ClientID %>').value = item.get_attributes().getAttribute("UserId");
                document.getElementById('<%=HiddenResponsibleUserName.ClientID %>').value = item.get_attributes().getAttribute("FullName");
                document.getElementById('<%=HiddenResponsibleUserEmail.ClientID %>').value = item.get_attributes().getAttribute("Email");
            }

            function RadComboBox15_OnClientSelectedIndexChanged(sender, eventArgs) {
                var item = eventArgs.get_item();
                document.getElementById('<%=HiddenActionTakenUserId.ClientID %>').value = item.get_attributes().getAttribute("UserId");
                document.getElementById('<%=HiddenActionTakenUserName.ClientID %>').value = item.get_attributes().getAttribute("FullName");
                document.getElementById('<%=HiddenActionTakenUserEmail.ClientID %>').value = item.get_attributes().getAttribute("Email");
            }

            function RadComboBox16_OnClientSelectedIndexChanged(sender, eventArgs) {
                var datePicker7 = $find("<%= RadDatePicker7.ClientID %>"); //Date
                var item = eventArgs.get_item();
                document.getElementById('<%=HiddenReIssuedUserId.ClientID %>').value = item.get_attributes().getAttribute("UserId");
                document.getElementById('<%=HiddenReIssuedUserName.ClientID %>').value = item.get_attributes().getAttribute("FullName");
                document.getElementById('<%=HiddenReIssuedUserEmail.ClientID %>').value = item.get_attributes().getAttribute("Email");

                if (item.get_attributes().getAttribute("UserId").length > 0) {
                    if (datePicker7.isEmpty()) {
                        datePicker7.get_dateInput().focus();
                    }
                }
                else {
                    datePicker7.clear();
                }

            }

            function RadComboBox17_OnClientSelectedIndexChanged(sender, eventArgs) {
                var item = eventArgs.get_item();

                document.getElementById('<%=HiddenVerifiedByUserId.ClientID %>').value = item.get_attributes().getAttribute("UserId");
                document.getElementById('<%=HiddenVerifiedByUserName.ClientID %>').value = item.get_attributes().getAttribute("FullName");
                document.getElementById('<%=HiddenVerifiedByUserEmail.ClientID %>').value = item.get_attributes().getAttribute("Email");
            }

            function RadComboBox18_OnClientSelectedIndexChanged(sender, eventArgs) {
                var item = eventArgs.get_item();
                document.getElementById('<%=HiddenAcceptedByUserId.ClientID %>').value = item.get_attributes().getAttribute("UserId");
                document.getElementById('<%=HiddenAcceptedByUserName.ClientID %>').value = item.get_attributes().getAttribute("FullName");
                document.getElementById('<%=HiddenAcceptedByUserEmail.ClientID %>').value = item.get_attributes().getAttribute("Email");
            }

            function RadComboBox19_OnClientSelectedIndexChanged(sender, eventArgs) {
                var datePicker9 = $find("<%= RadDatePicker9.ClientID %>"); //Follow-Up Date
                var item = eventArgs.get_item();

                if (item.get_value() == "True") {
                    if (datePicker9.isEmpty()) {
                        datePicker9.get_dateInput().focus();
                    }
                }
                else {
                    datePicker9.clear();
                }
            }

            //*** Start ***//
            function OnClientClicking_RadButton1(sender, eventArgs) {
                var startPage;
                startPage = CheckStartPage();

                if (!startPage) {
                    eventArgs.set_cancel(true);
                }
                return;
            }

            function OnClientClicking_RadButton25(sender, eventArgs) {

                var proceed = confirm(sender.get_text() + "\n\n    Are you sure?");
                if (proceed) {
                    var saveLater;
                    saveLater = CheckStartPage();

                    if (!saveLater) {
                        eventArgs.set_cancel(true);
                    }
                }
                else {
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
                var radComboBox11 = $find("<%= RadComboBox11.ClientID %>");  //Supplier
                var uReturn = true;

                if (radComboBox11 != null) {
                    if (radComboBox11.get_value() == "") {
                        var comboInput11 = radComboBox11.get_inputDomElement();
                        uReturn = false;
                        alert("Error... Required field.\n\nPlease select a supplier.");
                        document.getElementById('<%=HiddenVendorNumber.ClientID %>').value = "";
                        document.getElementById('<%=HiddenVendorName.ClientID %>').value = "";
                        comboInput11.focus();
                        return;
                    }
                }
                return uReturn;
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
                var radComboBox7 = $find("<%= RadComboBox7.ClientID %>");  //Issued To
                var radComboBox10 = $find("<%= RadComboBox10.ClientID %>");  //Location Country

                var uReturn = true;

                if (radComboBox7 != null) {

                    if (radComboBox7.get_value() == "") {
                        var comboInput7 = radComboBox7.get_inputDomElement();
                        uReturn = false;

                        alert("Error... Required field.\n\nPlease select Issued To.");

                        document.getElementById('<%=HiddenIssuedToUserId.ClientID %>').value = "";
                        document.getElementById('<%=HiddenIssuedToUserName.ClientID %>').value = "";
                        document.getElementById('<%=HiddenIssuedToUserEmail.ClientID %>').value = "";

                        comboInput7.focus();
                        return;
                    }
                }

                if (radComboBox10 != null) {
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
                var list3 = $find("<%= RadListBox3.ClientID %>"); //Supplier
                var hiddenAreaOid = document.getElementById('<%=HiddenAreaOid.ClientID %>').value;  //Area                

                var textBox9 = $find("<%= RadTextBox9.ClientID %>");
                var uCheck = true;

                var checkedNodes = list1.get_checkedItems();
                if (checkedNodes.length == 0) {
                    uCheck = false;
                    alert("Select at least 1 recipient from Halliburton to send.\n\nPlease verify and continue.");
                }

                //***  If supplier recipients are required, remove REM below ****
                //if (hiddenAreaOid == "2" && uCheck == true) {
                //    var checkedNodes3 = list3.get_checkedItems();
                //    if (checkedNodes3.length == 0) {
                //        uCheck = false;
                //        alert("Select at least 1 recipient from Supplier to send.\n\nPlease verify and continue.");
                //    }
                //}

                if (textBox9.get_value().length == 0 && uCheck == true) {
                    uCheck = false;
                    alert("Message is empty.\n\nPlease type a message and continue.");
                    textBox9.focus();
                }

                if (uCheck == true) {
                    if (window.confirm("Update CAR and Send notification.\n\n\Are you sure?\nClick OK to " + sender.get_text() + ".")) {
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

            function OnClientClicked_RadButton6() {
                //GetRadWindow().BrowserWindow.ReloadGrids('Grid1');
                GetRadWindow().Close('oEditDialog');
            }

            //*** Question 1 ***//
            function OnClientClicking_RadButton7(sender, eventArgs) {
                var textBox11 = $find("<%= RadTextBox11.ClientID %>");

                if (textBox11 != null) {
                    if (textBox11.get_value().length == 0) {
                        document.getElementById('<%=RequiredField_Q1.ClientID %>').value = "";
                    }
                    else {
                        document.getElementById('<%=RequiredField_Q1.ClientID %>').value = textBox11.get_value();
                    }
                }
                return;
            }

            //*** Question 2 ***//
            function OnClientClicking_RadButton8(sender, eventArgs) {
                var textBox14 = $find("<%= RadTextBox14.ClientID %>");

                if (textBox14 != null) {
                    if (textBox14.get_value().length == 0) {
                        document.getElementById('<%=RequiredField_Q2.ClientID %>').value = "";
                    }
                    else {
                        document.getElementById('<%=RequiredField_Q2.ClientID %>').value = textBox14.get_value();
                    }
                }
                return;
            }

            //*** Question 3 ***//
            function OnClientClicking_RadButton4(sender, eventArgs) {
                var radComboBox14 = $find("<%= RadComboBox14.ClientID %>");

                if (radComboBox14 != null) {
                    document.getElementById('<%=RequiredField_Q3YN.ClientID %>').value = radComboBox14.get_value();

                    if (radComboBox14.get_value() == "Y") {
                        var textBox15 = $find("<%= RadTextBox15.ClientID %>");
                        if (textBox15 != null) {
                            document.getElementById('<%=RequiredField_Q3.ClientID %>').value = textBox15.get_value();
                        }
                    }
                    else {
                        document.getElementById('<%=RequiredField_Q3.ClientID %>').value = "";
                    }
                }
                return;
            }

            //*** Question 4 ***//
            function OnClientClicking_RadButton9(sender, eventArgs) {
                var textBox16 = $find("<%= RadTextBox16.ClientID %>");
                var dateScheduled = $find("<%= RadDatePicker3.ClientID %>");

                if (textBox16 != null) {
                    if (textBox16.get_value().length == 0) {
                        document.getElementById('<%=RequiredField_Q4.ClientID %>').value = "";
                    }
                    else {
                        document.getElementById('<%=RequiredField_Q4.ClientID %>').value = textBox16.get_value();
                    }
                }

                if (dateScheduled.isEmpty()) {
                    document.getElementById('<%=RequiredField_Q4Date.ClientID %>').value = "";
                }
                else {
                    document.getElementById('<%=RequiredField_Q4Date.ClientID %>').value = dateScheduled.get_dateInput().get_selectedDate();
                }

                return;
            }

            //*** Question 5 ***//
            function OnClientClicking_RadButton10(sender, eventArgs) {
                var textBox17 = $find("<%= RadTextBox17.ClientID %>");
                var dateScheduled = $find("<%= RadDatePicker4.ClientID %>");

                if (textBox17 != null) {
                    if (textBox17.get_value().length == 0) {
                        document.getElementById('<%=RequiredField_Q5.ClientID %>').value = "";
                    }
                    else {
                        document.getElementById('<%=RequiredField_Q5.ClientID %>').value = textBox17.get_value();
                    }
                }

                if (dateScheduled.isEmpty()) {
                    document.getElementById('<%=RequiredField_Q5Date.ClientID %>').value = "";
                }
                else {
                    document.getElementById('<%=RequiredField_Q5Date.ClientID %>').value = dateScheduled.get_dateInput().get_selectedDate();
                }

                return;
            }

            // *** Originator Use Only **//
            function OnClientClicking_RadButton12(sender, eventArgs) {
                var uCheck = true;
                //var uReturn = true;
                var orig = document.getElementById('<%=HiddenOriginatorUserId.ClientID %>').value;
                var admn = '<%= MySession.Current.SessionCarAdmin %>';
                var cuid = '<%= SesUsrId %>';
                //alert("admn " + admn);
                if (orig == cuid || admn == true) {
                    //alert("passed");
                    <%--var duedateExt = $find("<%= RadDatePicker6.ClientID %>"); //optional--%>
                    var reissuedTo = $find("<%= RadComboBox16.ClientID %>");  // Re-Issued To
                    var datereIssued = $find("<%= RadDatePicker7.ClientID %>");  // Re-Issued Date

                    var followUp = $find("<%= RadComboBox19.ClientID %>");  //Follow Up
                    var dateFollowup = $find("<%= RadDatePicker9.ClientID %>");  //Follow Up Date

                    var dateRecvd = $find("<%= RadDatePicker8.ClientID %>");  //Received Date
                    var dateVerfd = $find("<%= RadDatePicker10.ClientID %>");  //Verified Date

                    var verifyBy = $find("<%= RadComboBox17.ClientID %>");  // Verified By
                    var acceptedBy = $find("<%= RadComboBox18.ClientID %>");  // Response Accepted By

                    //duedateExt is not reqred?
                    //NOT REQUIRED, BUT BOTH PERSON & DATE MUST BE ENTERED IF RE-ISSUED
                    if (reissuedTo.get_text().length == 0 || reissuedTo.get_text() == "Search recipient...") {
                        datereIssued.clear();
                        document.getElementById('<%=HiddenReIssuedUserName.ClientID %>').value = "";
                        document.getElementById('<%=HiddenReIssuedUserId.ClientID %>').value = "";
                    }
                    else {
                        if (datereIssued.isEmpty()) {
                            uCheck = false;
                            alert("If this CAR has been re-issued, select the re-issue date.\n\nPlease verify and continue.");
                        }
                    }
                    if (datereIssued.isEmpty()) {
                        //nothing
                    }
                    else {
                        if (reissuedTo.get_text().length == 0 || reissuedTo.get_text() == "Search recipient...") {
                            uCheck = false;
                            alert("If this CAR has been re-issued, select to whom it was re-issued.\n\nPlease verify and continue.");
                        }
                    }

                    //NOT REQUIRED, BUT IF A DATE MUST BE SELECTED IF YES IS SELECTED
                    if (followUp.get_text() == "No" || followUp.get_text() == "") {
                        dateFollowup.clear();
                    }
                    else {
                        if (dateFollowup.isEmpty()) {
                            uCheck = false;
                            alert("If this CAR has a follow up date, please select that date.\n\nPlease verify and continue.");
                        }
                    }
                    if (dateFollowup.isEmpty()) {
                        //nothing
                    }
                    else {
                        if (followUp.get_text() == "No" || followUp.get_text() == "") {
                            uCheck = false;
                            alert("If this CAR has a follow up date, please select Yes.\n\nPlease verify and continue.");
                        }
                    }

                    //REQUIRED
                    if (dateRecvd.isEmpty()) {
                        uCheck = false;
                        alert("If this CAR must have a Date Received entered.\n\nPlease verify and continue.");
                    }
                    //REQUIRED
                    if (dateVerfd.isEmpty()) {
                        uCheck = false;
                        alert("If this CAR must have a Date Verified entered.\n\nPlease verify and continue.");
                    }

                    if (verifyBy.get_text().length == 0 || verifyBy.get_text() == "Search recipient...") {
                        uCheck = false;
                        alert("If this CAR must have a Verified By entered.\n\nPlease verify and continue.");
                        document.getElementById('<%=HiddenVerifiedByUserName.ClientID %>').value = "";
                        document.getElementById('<%=HiddenVerifiedByUserId.ClientID %>').value = "";
                    }

                    if (acceptedBy.get_text().length == 0 || acceptedBy.get_text() == "Search recipient...") {
                        uCheck = false;
                        alert("If this CAR must have a Accepted By entered.\n\nPlease verify and continue.");
                        document.getElementById('<%=HiddenAcceptedByUserName.ClientID%>').value = "";
                        document.getElementById('<%=HiddenAcceptedByUserId.ClientID%>').value = "";
                    }

                    if ($find("<%= RadComboBox20.ClientID %>").get_value() == "3") {
                        ////Close status is set                        
                        var Q1 = document.getElementById('<%=RequiredField_Q1.ClientID %>').value;
                        var Q2 = document.getElementById('<%=RequiredField_Q2.ClientID %>').value;

                        var Q3YN = document.getElementById('<%=RequiredField_Q3YN.ClientID %>').value;
                        var Q3 = document.getElementById('<%=RequiredField_Q3.ClientID %>').value;

                        var Q4 = document.getElementById('<%=RequiredField_Q4.ClientID %>').value;
                        var Q4Date = document.getElementById('<%=RequiredField_Q4Date.ClientID %>').value;

                        var Q5 = document.getElementById('<%=RequiredField_Q5.ClientID %>').value;
                        var Q5Date = document.getElementById('<%=RequiredField_Q5Date.ClientID %>').value;

                        var responsiblePerson = document.getElementById('<%=HiddenResponsibleUserId.ClientID %>').value;
                        var textBox18 = $find("<%= RadTextBox18.ClientID %>");  //Effectiveness Validation comments
                        var comboBox21 = $find("<%= RadComboBox21.ClientID %>");  //Effectiveness validation
                        var comboBox20 = $find("<%= RadComboBox20.ClientID %>");  //Status

                        <%--var radComboBox13 = $find("<%= RadComboBox13.ClientID %>");  //Responsible Person
                        if (radComboBox13 != null) {
                            if (radComboBox13.get_text().length == 0 || radComboBox13.get_text() == "Search recipients...") {
                                var comboInput13 = radComboBox13.get_inputDomElement();
                                uReturn = false;
                                alert("Error... Required field.\n\nPlease select a Responsible Person.");
                                 alert("Responsible Person missing.\n\nSelect a Responsible Person on the Start tab and try again.")
                                document.getElementById('<%=HiddenResponsibleUserName.ClientID %>').value = "";
                                document.getElementById('<%=HiddenResponsibleUserId.ClientID %>').value = "";
                                comboInput13.focus();
                                return;
                            }
                        }--%>

                        if (reissuedTo.get_text().length > 0 && reissuedTo.get_text() != "Search recipient...") {
                            if (datereIssued.isEmpty()) {
                                alert("Required field:  Date Re-Issued missing.\n\nPlease select Date Re-Issued.");
                                datereIssued.get_dateInput().focus();
                                eventArgs.set_cancel(true);
                                return;
                            }
                        }

                        if (followUp.get_value() == "NA") {
                            var comboInput19 = followUp.get_inputDomElement();

                            alert("Required field:  Follow-Up Required must be flagged.\n\nPlease select Follow-Up.");
                            comboInput19.focus();
                            eventArgs.set_cancel(true);
                            return;
                        }
                        if (followUp.get_value() == "True" && dateFollowup.isEmpty()) {
                            alert("Required field:  Follow-Up Date missing.\n\nPlease select Date to Follow-Up.");
                            dateFollowup.get_dateInput().focus();
                            eventArgs.set_cancel(true);
                            return;
                        }

                        if (dateRecvd.isEmpty()) {
                            alert("Required field:  Date Received missing.\n\nPlease select Date Received.");
                            dateRecvd.get_dateInput().focus();
                            eventArgs.set_cancel(true);
                            return;
                        }

                        if (dateVerfd.isEmpty()) {
                            alert("Required field:  Date Verified missing.\n\nPlease select Date Verified.");
                            dateVerfd.get_dateInput().focus();
                            eventArgs.set_cancel(true);
                            return;
                        }

                        var checkedNodes = comboBox21.get_checkedItems();
                        if (checkedNodes.length == 0) {
                            uCheck = false;
                            alert("Select at least 1 Effectiveness Validation option and type a description below that.\n\nPlease verify and continue.");
                            comboBox21.focus();
                            eventArgs.set_cancel(true);
                            return;
                        }

                        if (textBox18.get_value().length == 0) {
                            alert("Must answer how effectiveness was validated.\n\nPlease verify and try again.")
                            textBox18.focus();
                            eventArgs.set_cancel(true);
                            return;
                        }

                        if (responsiblePerson.length == 0) {
                            alert("Required field:  Responsible Person missing.\n\nSelect a Responsible Person on the Start tab.")
                            eventArgs.set_cancel(true);
                            return;
                        }

                        if (Q1.length == 0) {
                            alert("Question #1 Answer missing.\n\nAnswer question on the #1 tab and try again.")
                            eventArgs.set_cancel(true);
                            return;
                        }

                        if (Q2.length == 0) {
                            alert("Question #2 Answer missing.\n\nAnswer question on the #2 tab and try again.")
                            eventArgs.set_cancel(true);
                            return;
                        }

                        if (Q3YN == "Y" && Q3.length == 0) {
                            alert("Question #3 Answer missing.\n\nAnswer question on the #3 tab and try again.")
                            eventArgs.set_cancel(true);
                            return;
                        }

                        if (Q4.length == 0) {
                            alert("Question #4 Answer missing.\n\nAnswer question on the #4 tab and try again.")
                            eventArgs.set_cancel(true);
                            return;
                        }

                        if (Q4Date.length == 0) {
                            alert("Question #4 implementation Date missing.\n\nSelect a date on the #4 tab and try again.")
                            eventArgs.set_cancel(true);
                            return;
                        }

                        if (Q5.length == 0) {
                            alert("Question #5 Answer missing.\n\nAnswer question on the #5 tab and try again.")
                            eventArgs.set_cancel(true);
                            return;
                        }

                        if (Q5Date.length == 0) {
                            alert("Question #5 implementation Date missing.\n\nSelect a date on the #5 tab and try again.")
                            eventArgs.set_cancel(true);
                            return;
                        }

                        var chkText = document.getElementById('<%= RadTextBox18.ClientID %>').value;
                        if (chkText.indexOf("Verified per") == -1 && chkText.indexOf("Other --") == -1) {
                            alert("An Effectiveness Validation method must be selected from the drop down list.\n\nPlease verify and try again.")
                            comboBox21.focus();
                            eventArgs.set_cancel(true);
                            return;
                        }

                        //alert(radvalue);                        
                        let txtText = chkText;
                        var chkCnt1 = txtText.indexOf("Verified per Audit");    //0//23
                        var chkCnt2 = txtText.indexOf("Verified per Corrective Action description");    //47//70
                        var chkCnt3 = txtText.indexOf("Verified per interview confirmation");   //40/110
                        var chkCnt4 = txtText.indexOf("Verified per evidence submission");  //37//147
                        var chkCnt5 = txtText.indexOf("Other"); //10
                        //alert(chkCnt1 + " " + chkCnt2 + " " + chkCnt3 + " " + chkCnt4 + " " + chkCnt5);
                        var txtCnt = 0;
                        if (radvalue = "Verified per Audit") {
                            if (chkCnt1 == 0) {
                                txtCnt = 23;
                                if (chkText.substr(txtCnt, 1000).length == 0) {
                                    alert("Originator must type a description of how effectiveness was validated after the drop down selection.\n\nPlease verify and try again.");
                                    textBox18.focus();
                                    eventArgs.set_cancel(true);
                                    return;
                                }
                            }
                        }
                        if (radvalue = "Verified per Corrective Action description") {
                            if (chkCnt2 == 23) {
                                txtCnt = 70;
                            }
                            else {
                                txtCnt = 47;
                            }
                            if (chkText.substr(txtCnt, 1000).length == 0) {
                                alert("Originator must type a description of how effectiveness was validated.\n\nPlease verify and try again.");
                                textBox18.focus();
                                eventArgs.set_cancel(true);
                                return;
                            }
                        }
                        if (radvalue = "Verified per interview confirmation") {
                            if (chkCnt3 == 70) {
                                txtCnt = 110;
                            }
                            if (chkCnt3 == 47) {
                                txtCnt = 87;
                            }
                            if (chkCnt3 == 23) {
                                txtCnt = 63;
                            }
                            if (chkCnt3 == 0) {
                                txtCnt = 40;
                            }
                            if (chkText.substr(txtCnt, 1000).length == 0) {
                                alert("Originator must type a description of how effectiveness was validated.\n\nPlease verify and try again.");
                                textBox18.focus();
                                eventArgs.set_cancel(true);
                                return;
                            }
                        }
                        if (radvalue = "Verified per evidence submission") {
                            if (chkCnt4 == 110) {
                                txtCnt = 147;
                            }
                            if (chkCnt4 == 87) {
                                txtCnt = 124;
                            }
                            if (chkCnt4 == 70) {
                                txtCnt = 107;
                            }
                            if (chkCnt4 == 63) {
                                txtCnt = 100;
                            }
                            if (chkCnt4 == 47) {
                                txtCnt = 84;
                            }
                            if (chkCnt4 == 40) {
                                txtCnt = 77;
                            }
                            if (chkCnt4 == 23) {
                                txtCnt = 60;
                            }
                            if (chkCnt4 == 0) {
                                txtCnt = 37;
                            }
                            if (chkText.substr(txtCnt, 1000).length == 0) {
                                alert("Originator must type a description of how effectiveness was validated.\n\nPlease verify and try again.");
                                textBox18.focus();
                                eventArgs.set_cancel(true);
                                return;
                            }
                        }
                        if (radvalue = "Other") {
                            if (chkCnt5 == 147) {
                                txtCnt = 157;
                            }
                            if (chkCnt5 == 124) {
                                txtCnt = 134;
                            }
                            if (chkCnt5 == 110) {
                                txtCnt = 147;
                            }
                            if (chkCnt5 == 107) {
                                txtCnt = 117;
                            }
                            if (chkCnt5 == 100) {
                                txtCnt = 110;
                            }
                            if (chkCnt5 == 87) {
                                txtCnt = 97;
                            }
                            if (chkCnt5 == 84) {
                                txtCnt = 94;
                            }
                            if (chkCnt5 == 77) {
                                txtCnt = 87;
                            }
                            if (chkCnt5 == 70) {
                                txtCnt = 80;
                            }
                            if (chkCnt5 == 63) {
                                txtCnt = 73;
                            }
                            if (chkCnt5 == 60) {
                                txtCnt = 70;
                            }
                            if (chkCnt5 == 47) {
                                txtCnt = 57;
                            }
                            if (chkCnt5 == 40) {
                                txtCnt = 50;
                            }
                            if (chkCnt5 == 37) {
                                txtCnt = 47;
                            }
                            if (chkCnt5 == 23) {
                                txtCnt = 33;
                            }
                            if (chkCnt5 == 0) {
                                txtCnt = 10;
                            }
                            if (chkText.substr(txtCnt, 1000).length == 0) {
                                alert("Originator must type a description of how effectiveness was validated.\n\nPlease verify and try again.");
                                textBox18.focus();
                                eventArgs.set_cancel(true);
                                return;
                            }
                        }
                    }
                }
                if (uCheck == true) {
                    //if (window.confirm("Update CAR and Send notification.\n\n\Are you sure?\nClick OK to " + sender.get_text() + ".")) {
                    uCheck = true;
                }
                else {
                    uCheck = false;
                    eventArgs.set_cancel(true);
                }
                //}
                //else {
                //    eventArgs.set_cancel(true);
                //}
                return uCheck;
                return;
            }

            function OnClientItemChecked_RadComboBox21(sender, eventArgs) {
                var curTxt = document.getElementById('<%= RadTextBox18.ClientID %>').value;
                var checkeditems = "";
                radvalue = "";
                var radcombo21 = $find("<%= RadComboBox21.ClientID%>")
                var items = radcombo21.get_checkedItems();
                for (i = 0; i < items.length; i++) {
                    radvalue = items[i].get_text();
                    if (i == 0) {
                        checkeditems = radvalue + " -- \n";
                    }
                    else {
                        checkeditems = checkeditems + radvalue + " -- \n";
                    }
                }
                document.getElementById('<%= RadTextBox18.ClientID %>').value = checkeditems;
            }

            function CheckCloseDateRequired() {
                var uReturn = true;
                var responsiblePerson = document.getElementById('<%=HiddenResponsibleUserId.ClientID %>').value;
                var Q1 = document.getElementById('<%=RequiredField_Q1.ClientID %>').value;
                var Q2 = document.getElementById('<%=RequiredField_Q2.ClientID %>').value;
                var Q3 = document.getElementById('<%=RequiredField_Q3YN.ClientID %>').value;
                var Q3YN = document.getElementById('<%=RequiredField_Q3.ClientID %>').value;
                var Q4 = document.getElementById('<%=RequiredField_Q4.ClientID %>').value;
                var Q4Date = document.getElementById('<%=RequiredField_Q4Date.ClientID %>').value;
                var Q5 = document.getElementById('<%=RequiredField_Q5.ClientID %>').value;
                var Q5Date = document.getElementById('<%=RequiredField_Q5Date.ClientID %>').value;

            }

            function OnClientClick_RadButtonCancel(sender, eventArgs) {
                var proceed = confirm("Cancel Edit.\n\n    Are you sure?");
                if (proceed) {
                    GetRadWindow().Close("Cancel");
                }
                return false;
            }

            function OnValueChanging_RadTextBox7(sender, eventArgs) {
                var value = eventArgs.get_newValue();
                var trimmed = value.replace(/^\s+|\s+$/g, '');
                eventArgs.set_newValue(trimmed);
            }

        </script>
    </telerik:RadCodeBlock>
    <form id="form1" runat="server">
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        </telerik:RadAjaxManager>
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js"></asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js"></asp:ScriptReference>
            </Scripts>
        </telerik:RadScriptManager>
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
            EnableShadow="true" ShowContentDuringLoad="false" VisibleStatusbar="false" ReloadOnShow="true" VisibleOnPageLoad="false"
            Behaviors="Resize, Close, Move, Maximize" InitialBehaviors="Resize, Close, Move">
        </telerik:RadWindowManager>
        <telerik:RadAjaxLoadingPanel ID="LoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" OnAjaxRequest="RadAjaxPanel1_AjaxRequest">
            <div class="contentContainer">
                <table>
                    <tr>
                        <td style="vertical-align: top;">
                            <table>
                                <tr>
                                    <td style="vertical-align: top;">
                                        <telerik:RadTabStrip RenderMode="Lightweight" ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" Font-Bold="true" Orientation="HorizontalTop"
                                            SelectedIndex="0" OnTabClick="RadTabStrip1_TabClick" CausesValidation="false" Skin="Silk">
                                            <Tabs>
                                                <telerik:RadTab PageViewID="RadPageView0" Value="RadPageView0" Text="Start" Selected="true">
                                                </telerik:RadTab>
                                                <telerik:RadTab PageViewID="RadPageView1" Value="RadPageView1" Text="Supplier">
                                                </telerik:RadTab>
                                                <telerik:RadTab PageViewID="RadPageView2" Value="RadPageView2" Text="Issued To">
                                                </telerik:RadTab>
                                                <telerik:RadTab PageViewID="RadPageView3" Value="RadPageView3" Text="#1">
                                                </telerik:RadTab>
                                                <telerik:RadTab PageViewID="RadPageView4" Value="RadPageView4" Text="#2">
                                                </telerik:RadTab>
                                                <telerik:RadTab PageViewID="RadPageView5" Value="RadPageView5" Text="#3">
                                                </telerik:RadTab>
                                                <telerik:RadTab PageViewID="RadPageView6" Value="RadPageView6" Text="#4">
                                                </telerik:RadTab>
                                                <telerik:RadTab PageViewID="RadPageView7" Value="RadPageView7" Text="#5">
                                                </telerik:RadTab>
                                                <telerik:RadTab PageViewID="RadPageView8" Value="RadPageView8" Text="Action Taken By">
                                                </telerik:RadTab>
                                                <telerik:RadTab PageViewID="RadPageView9" Value="RadPageView9" Text="Originator Use Only">
                                                </telerik:RadTab>
                                                <telerik:RadTab PageViewID="RadPageView10" Value="RadPageView10" Text="Notification">
                                                </telerik:RadTab>
                                                <telerik:RadTab PageViewID="RadPageView11" Value="RadPageView11" Text="Finish">
                                                </telerik:RadTab>
                                            </Tabs>
                                        </telerik:RadTabStrip>
                                        <telerik:RadMultiPage ID="RadMultiPage1" SelectedIndex="0" Height="100%" runat="server" RenderSelectedPageOnly="true">
                                            <telerik:RadPageView runat="server" ID="RadPageView0" Selected="true" DefaultButton="RadButton1">
                                                <fieldset class="fieldset">
                                                    <table>
                                                        <tr>
                                                            <td>
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
                                                                                EmptyMessage="Search recipients...">
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
                                                                            <telerik:RadTextBox ID="RadTextBox22" runat="server" EmptyMessage="(if applicable)" Width="200px" MaxLength="50">
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
                                                                            <telerik:RadTextBox ID="RadTextBox23" runat="server" EmptyMessage="(if applicable)" Width="200px" MaxLength="50">
                                                                                <FocusedStyle BackColor="LightCyan" />
                                                                            </telerik:RadTextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <th>* Responsible Person</th>
                                                                        <td>
                                                                            <telerik:RadComboBox ID="RadComboBox13" runat="server" MarkFirstMatch="true" AllowCustomText="true" Sort="Ascending"
                                                                                EnableLoadOnDemand="true" ShowMoreResultsBox="True" DropDownAutoWidth="enabled" LoadingMessage="Searching...Please wait"
                                                                                Height="200px" Width="200px" OnClientSelectedIndexChanged="RadComboBox13_OnClientSelectedIndexChanged"
                                                                                OnItemsRequested="RadComboBox13_ItemsRequested"
                                                                                OpenDropDownOnLoad="false"
                                                                                ShowDropDownOnTextboxClick="false"
                                                                                HighlightTemplatedItems="true"
                                                                                EnableVirtualScrolling="true"
                                                                                EmptyMessage="Search recipients...">
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
                                                                        <td></td>
                                                                        <td></td>
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
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right;">
                                                                <telerik:RadButton ID="RadButton25" runat="server" Text="Save and Continue Later" CausesValidation="true" SingleClick="true" SingleClickText="Processing..." BorderColor="Blue" UseSubmitBehavior="true"
                                                                    RenderMode="Lightweight" OnClientClicking="OnClientClicking_RadButton25" OnClick="RadButton25_Click">
                                                                    <Icon PrimaryIconCssClass="rbSave" />
                                                                </telerik:RadButton>
                                                                <telerik:RadButton ID="RadButton14" runat="server" Text="Cancel" SingleClick="true" CausesValidation="false" BorderColor="Blue"
                                                                    RenderMode="Lightweight" OnClientClicked="OnClientClick_RadButtonCancel">
                                                                    <Icon PrimaryIconCssClass="rbCancel" />
                                                                </telerik:RadButton>
                                                                <telerik:RadButton RenderMode="Lightweight" ID="RadButton1" runat="server" Text="Confirm Next" BorderColor="Blue"
                                                                    CausesValidation="true" SingleClick="true" SingleClickText="Processing..." OnClientClicking="OnClientClicking_RadButton1" OnClick="RadButton1_Click" UseSubmitBehavior="true">
                                                                    <Icon SecondaryIconCssClass="rbNext"></Icon>
                                                                </telerik:RadButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </telerik:RadPageView>
                                            <telerik:RadPageView runat="server" ID="RadPageView1" DefaultButton="RadButton2">
                                                <fieldset class="fieldset">
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
                                                            <td colspan="2"></td>
                                                        </tr>
                                                        <tr>
                                                            <td></td>
                                                            <td style="text-align: right;">
                                                                <telerik:RadButton ID="RadButton26" runat="server" Text="Save and Continue Later" CausesValidation="true" SingleClick="true" SingleClickText="Processing..." BorderColor="Blue" UseSubmitBehavior="true"
                                                                    RenderMode="Lightweight" OnClientClicking="OnClientClicking_RadButton2" OnClick="RadButton26_Click" Visible="false">
                                                                    <Icon PrimaryIconCssClass="rbSave" />
                                                                </telerik:RadButton>
                                                                <telerik:RadButton ID="RadButton15" runat="server" Text="Cancel" SingleClick="true" CausesValidation="false" BorderColor="Blue"
                                                                    RenderMode="Lightweight" OnClientClicked="OnClientClick_RadButtonCancel">
                                                                    <Icon PrimaryIconCssClass="rbCancel" />
                                                                </telerik:RadButton>
                                                                <telerik:RadButton RenderMode="Lightweight" ID="RadButton2" runat="server" Text="Confirm Next" Font-Bold="true" BorderStyle="Solid" BorderWidth="1px" BorderColor="Blue"
                                                                    CausesValidation="true" SingleClick="true" SingleClickText="Processing..." OnClientClicking="OnClientClicking_RadButton2" OnClick="RadButton2_Click">
                                                                    <Icon SecondaryIconCssClass="rbNext"></Icon>
                                                                </telerik:RadButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </telerik:RadPageView>
                                            <telerik:RadPageView runat="server" ID="RadPageView2" DefaultButton="RadButton3">
                                                <fieldset class="fieldset">
                                                    <table>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                <asp:RadioButtonList ID="RadioButtonList2" runat="server" RepeatDirection="Horizontal"
                                                                    AutoPostBack="true" OnSelectedIndexChanged="RadioButtonList2_SelectedIndexChanged">
                                                                    <asp:ListItem Value="Hal" Text="Halliburton">Halliburton Recipient</asp:ListItem>
                                                                    <asp:ListItem Value="Sup" Text="Supplier">Supplier Recipient</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th>Full Name</th>
                                                            <td>
                                                                <telerik:RadComboBox ID="RadComboBox7" runat="server" MarkFirstMatch="true" AllowCustomText="true" Sort="Ascending"
                                                                    EnableLoadOnDemand="true" ShowMoreResultsBox="True" DropDownAutoWidth="enabled" LoadingMessage="Searching...Please wait"
                                                                    Height="200px" Width="350px" OnClientSelectedIndexChanged="RadComboBox7_OnClientSelectedIndexChanged"
                                                                    OnItemsRequested="RadComboBox7_ItemsRequested"
                                                                    OpenDropDownOnLoad="false"
                                                                    ShowDropDownOnTextboxClick="false"
                                                                    HighlightTemplatedItems="true"
                                                                    EnableVirtualScrolling="true"
                                                                    EmptyMessage="Search recipients...">
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
                                                                <telerik:RadButton ID="RadButton27" runat="server" Text="Save and Continue Later" CausesValidation="true" SingleClick="true" SingleClickText="Processing..." BorderColor="Blue" UseSubmitBehavior="true"
                                                                    RenderMode="Lightweight" OnClientClicking="OnClientClicking_RadButton3" OnClick="RadButton27_Click">
                                                                    <Icon PrimaryIconCssClass="rbSave" />
                                                                </telerik:RadButton>
                                                                <telerik:RadButton ID="RadButton16" runat="server" Text="Cancel" SingleClick="true" CausesValidation="false" BorderColor="Blue"
                                                                    RenderMode="Lightweight" OnClientClicked="OnClientClick_RadButtonCancel">
                                                                    <Icon PrimaryIconCssClass="rbCancel" />
                                                                </telerik:RadButton>
                                                                <telerik:RadButton RenderMode="Lightweight" ID="RadButton3" runat="server" Text="Confirm Next" BorderColor="Blue"
                                                                    CausesValidation="true" SingleClick="true" SingleClickText="Processing..." OnClientClicking="OnClientClicking_RadButton3" OnClick="RadButton3_Click">
                                                                    <Icon SecondaryIconCssClass="rbNext"></Icon>
                                                                </telerik:RadButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </telerik:RadPageView>
                                            <telerik:RadPageView runat="server" ID="RadPageView3">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <fieldset class="fieldset">
                                                                <legend style="font-size: small; font-weight: bold; font-style: italic;">* Why did this nonconformance occur?</legend>
                                                                <table>
                                                                    <tr>
                                                                        <td>Provide a detailed explanation.  
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>A simple re-wording of the nonconformance will not be accepted.  
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Terms such as oversight or human error require further explanation.
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <telerik:RadTextBox ID="RadTextBox11" runat="server" EmptyMessage="Why did this nonconformance occur?" TextMode="MultiLine" Width="800px" Rows="15">
                                                                                <FocusedStyle BackColor="LightCyan" />
                                                                            </telerik:RadTextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="text-align: right;">
                                                                            <telerik:RadButton ID="RadButton28" runat="server" Text="Save and Continue Later" CausesValidation="true" SingleClick="true" SingleClickText="Processing..." BorderColor="Blue" UseSubmitBehavior="true"
                                                                                RenderMode="Lightweight" OnClientClicking="OnClientClicking_RadButton7" OnClick="RadButton28_Click">
                                                                                <Icon PrimaryIconCssClass="rbSave" />
                                                                            </telerik:RadButton>
                                                                            <telerik:RadButton ID="RadButton17" runat="server" Text="Cancel" SingleClick="true" CausesValidation="false" BorderColor="Blue"
                                                                                RenderMode="Lightweight" OnClientClicked="OnClientClick_RadButtonCancel">
                                                                                <Icon PrimaryIconCssClass="rbCancel" />
                                                                            </telerik:RadButton>
                                                                            <telerik:RadButton RenderMode="Lightweight" ID="RadButton7" runat="server" Text="Confirm Next" Font-Bold="true" BorderStyle="Solid" BorderColor="Blue" OnClick="RadButton7_Click"
                                                                                OnClientClicking="OnClientClicking_RadButton7">
                                                                                <Icon SecondaryIconCssClass="rbNext"></Icon>
                                                                            </telerik:RadButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </fieldset>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </telerik:RadPageView>
                                            <telerik:RadPageView runat="server" ID="RadPageView4">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <fieldset class="fieldset">
                                                                <legend style="font-size: small; font-weight: bold; font-style: italic;">* What is the root cause / potential root cause of the non-conformance?</legend>
                                                                <table>
                                                                    <tr>
                                                                        <td>Use the final “why” if the 5y methodology is utilized. </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <telerik:RadTextBox ID="RadTextBox14" runat="server" EmptyMessage="What is the root cause / potential root cause of the non-conformance?" TextMode="MultiLine" Width="800px" Rows="15">
                                                                                <FocusedStyle BackColor="LightCyan" />
                                                                            </telerik:RadTextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="text-align: right;">
                                                                            <telerik:RadButton ID="RadButton29" runat="server" Text="Save and Continue Later" CausesValidation="true" SingleClick="true" SingleClickText="Processing..." BorderColor="Blue" UseSubmitBehavior="true"
                                                                                RenderMode="Lightweight" OnClientClicking="OnClientClicking_RadButton8" OnClick="RadButton29_Click">
                                                                                <Icon PrimaryIconCssClass="rbSave" />
                                                                            </telerik:RadButton>
                                                                            <telerik:RadButton ID="RadButton18" runat="server" Text="Cancel" SingleClick="true" CausesValidation="false" BorderColor="Blue"
                                                                                RenderMode="Lightweight" OnClientClicked="OnClientClick_RadButtonCancel">
                                                                                <Icon PrimaryIconCssClass="rbCancel" />
                                                                            </telerik:RadButton>
                                                                            <telerik:RadButton RenderMode="Lightweight" ID="RadButton8" runat="server" Text="Confirm Next" Font-Bold="true" BorderStyle="Solid" BorderColor="Blue" OnClick="RadButton8_Click"
                                                                                OnClientClicking="OnClientClicking_RadButton8">
                                                                                <Icon SecondaryIconCssClass="rbNext"></Icon>
                                                                            </telerik:RadButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </fieldset>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </telerik:RadPageView>
                                            <telerik:RadPageView runat="server" ID="RadPageView5">
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td>
                                                            <fieldset class="fieldset">
                                                                <legend style="font-size: small; font-weight: bold; font-style: italic;">* Are there similar instances of this nonconformance in your area of responsibility? (Y/N)</legend>
                                                                <table>
                                                                    <tr>
                                                                        <th>
                                                                            <telerik:RadComboBox ID="RadComboBox14" runat="server" Width="200px" OnSelectedIndexChanged="RadComboBox14_SelectedIndexChanged" AutoPostBack="true">
                                                                                <Items>
                                                                                    <telerik:RadComboBoxItem Text="No" Value="N" Selected="true" />
                                                                                    <telerik:RadComboBoxItem Text="Yes" Value="Y" />
                                                                                </Items>
                                                                            </telerik:RadComboBox>
                                                                        </th>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Panel ID="Panel3" runat="server">
                                                                                <table>
                                                                                    <tr>
                                                                                        <th style="font-size: small; font-style: italic;">If Yes, describe similar instance below.</th>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <telerik:RadTextBox ID="RadTextBox15" runat="server" EmptyMessage="Are there similar instances of this nonconformance in your area of responsibility?" TextMode="MultiLine" Width="800px" Rows="10">
                                                                                                <FocusedStyle BackColor="LightCyan" />
                                                                                            </telerik:RadTextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </asp:Panel>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="text-align: right;">
                                                                            <telerik:RadButton ID="RadButton30" runat="server" Text="Save and Continue Later" CausesValidation="true" SingleClick="true" SingleClickText="Processing..." BorderColor="Blue" UseSubmitBehavior="true"
                                                                                RenderMode="Lightweight" OnClientClicking="OnClientClicking_RadButton4" OnClick="RadButton30_Click">
                                                                                <Icon PrimaryIconCssClass="rbSave" />
                                                                            </telerik:RadButton>
                                                                            <telerik:RadButton ID="RadButton19" runat="server" Text="Cancel" SingleClick="true" CausesValidation="false" BorderColor="Blue"
                                                                                RenderMode="Lightweight" OnClientClicked="OnClientClick_RadButtonCancel">
                                                                                <Icon PrimaryIconCssClass="rbCancel" />
                                                                            </telerik:RadButton>
                                                                            <telerik:RadButton RenderMode="Lightweight" ID="RadButton4" runat="server" Text="Confirm Next" Font-Bold="true" BorderStyle="Solid" BorderColor="Blue" OnClick="RadButton4_Click"
                                                                                OnClientClicking="OnClientClicking_RadButton4">
                                                                                <Icon SecondaryIconCssClass="rbNext"></Icon>
                                                                            </telerik:RadButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </fieldset>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </telerik:RadPageView>
                                            <telerik:RadPageView runat="server" ID="RadPageView6">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <fieldset class="fieldset">
                                                                <legend style="font-size: small; font-weight: bold; font-style: italic;">* What action was taken (or is planned) to correct this nonconformance?</legend>
                                                                <table>
                                                                    <tr>
                                                                        <td>Only actions completed are considered fully acceptable.  
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Future system revisions, training sessions, reviews etc. though indicating intent do not substantiate completed action.
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <telerik:RadTextBox ID="RadTextBox16" runat="server" EmptyMessage="What action was taken (or is planned) to correct this nonconformance?" TextMode="MultiLine" Width="800px" Rows="10">
                                                                                <FocusedStyle BackColor="LightCyan" />
                                                                            </telerik:RadTextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <th style="font-size: small; font-style: italic;">* What is the scheduled implementation date? </th>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <telerik:RadDatePicker ID="RadDatePicker3" runat="server" PopupDirection="TopLeft" Width="200px">
                                                                                <DateInput ID="DateInput3" runat="server" ForeColor="Black" Font-Names="Arial"
                                                                                    ClientEvents-OnError="DateOnError" EmptyMessage="Scheduled Date">
                                                                                </DateInput>
                                                                                <Calendar ID="Calendar3" runat="server" ShowRowHeaders="false">
                                                                                    <WeekendDayStyle BackColor="yellow" />
                                                                                </Calendar>
                                                                            </telerik:RadDatePicker>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="text-align: right;">
                                                                            <telerik:RadButton ID="RadButton31" runat="server" Text="Save and Continue Later" CausesValidation="true" SingleClick="true" SingleClickText="Processing..." BorderColor="Blue" UseSubmitBehavior="true"
                                                                                RenderMode="Lightweight" OnClientClicking="OnClientClicking_RadButton9" OnClick="RadButton31_Click">
                                                                                <Icon PrimaryIconCssClass="rbSave" />
                                                                            </telerik:RadButton>
                                                                            <telerik:RadButton ID="RadButton20" runat="server" Text="Cancel" SingleClick="true" CausesValidation="false" BorderColor="Blue"
                                                                                RenderMode="Lightweight" OnClientClicked="OnClientClick_RadButtonCancel">
                                                                                <Icon PrimaryIconCssClass="rbCancel" />
                                                                            </telerik:RadButton>
                                                                            <telerik:RadButton RenderMode="Lightweight" ID="RadButton9" runat="server" Text="Confirm Next" Font-Bold="true" BorderStyle="Solid" BorderColor="Blue" OnClick="RadButton9_Click"
                                                                                OnClientClicking="OnClientClicking_RadButton9">
                                                                                <Icon SecondaryIconCssClass="rbNext"></Icon>
                                                                            </telerik:RadButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </fieldset>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </telerik:RadPageView>
                                            <telerik:RadPageView runat="server" ID="RadPageView7">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <fieldset class="fieldset">
                                                                <legend style="font-size: small; font-weight: bold; font-style: italic;">* What action was taken (or is planned) to preclude this and similar non-conformances?</legend>
                                                                <table>
                                                                    <tr>
                                                                        <td>Only actions completed are considered fully acceptable. 
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>Future system revisions, training sessions, reviews etc. though indicating intent do not substantiate completed action.
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <telerik:RadTextBox ID="RadTextBox17" runat="server" EmptyMessage="What action was taken (or is planned) to preclude this and similar non-conformances?" TextMode="MultiLine" Width="800px" Rows="10">
                                                                                <FocusedStyle BackColor="LightCyan" />
                                                                            </telerik:RadTextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <th style="font-size: small; font-style: italic;">* What is the scheduled implementation date? </th>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <telerik:RadDatePicker ID="RadDatePicker4" runat="server" PopupDirection="TopLeft" Width="200px">
                                                                                <DateInput ID="DateInput4" runat="server" ForeColor="Black" Font-Names="Arial"
                                                                                    ClientEvents-OnError="DateOnError" EmptyMessage="Scheduled Date">
                                                                                </DateInput>
                                                                                <Calendar ID="Calendar4" runat="server" ShowRowHeaders="false">
                                                                                    <WeekendDayStyle BackColor="yellow" />
                                                                                </Calendar>
                                                                            </telerik:RadDatePicker>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="text-align: right;">
                                                                            <telerik:RadButton ID="RadButton32" runat="server" Text="Save and Continue Later" CausesValidation="true" SingleClick="true" SingleClickText="Processing..." BorderColor="Blue" UseSubmitBehavior="true"
                                                                                RenderMode="Lightweight" OnClientClicking="OnClientClicking_RadButton10" OnClick="RadButton32_Click">
                                                                                <Icon PrimaryIconCssClass="rbSave" />
                                                                            </telerik:RadButton>
                                                                            <telerik:RadButton ID="RadButton21" runat="server" Text="Cancel" SingleClick="true" CausesValidation="false" BorderColor="Blue"
                                                                                RenderMode="Lightweight" OnClientClicked="OnClientClick_RadButtonCancel">
                                                                                <Icon PrimaryIconCssClass="rbCancel" />
                                                                            </telerik:RadButton>
                                                                            <telerik:RadButton RenderMode="Lightweight" ID="RadButton10" runat="server" Text="Confirm Next" Font-Bold="true" BorderStyle="Solid" BorderColor="Blue" OnClick="RadButton10_Click"
                                                                                OnClientClicking="OnClientClicking_RadButton10">
                                                                                <Icon SecondaryIconCssClass="rbNext"></Icon>
                                                                            </telerik:RadButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </fieldset>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </telerik:RadPageView>
                                            <telerik:RadPageView runat="server" ID="RadPageView8">
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td>
                                                            <fieldset class="fieldset">
                                                                <legend style="font-size: small; font-weight: bold; font-style: italic;">Action Taken By</legend>
                                                                <table>
                                                                    <tr>
                                                                        <td></td>
                                                                        <td>
                                                                            <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal"
                                                                                AutoPostBack="true" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged">
                                                                                <asp:ListItem Value="Hal" Text="Halliburton">Halliburton Recipient</asp:ListItem>
                                                                                <asp:ListItem Value="Sup" Text="Supplier">Supplier Recipient</asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <th>Name</th>
                                                                        <td>
                                                                            <telerik:RadComboBox ID="RadComboBox15" runat="server" MarkFirstMatch="true" AllowCustomText="true" Sort="Ascending"
                                                                                EnableLoadOnDemand="true" ShowMoreResultsBox="True" DropDownAutoWidth="enabled" LoadingMessage="Searching...Please wait"
                                                                                Height="200px" Width="300px" OnClientSelectedIndexChanged="RadComboBox15_OnClientSelectedIndexChanged"
                                                                                OnItemsRequested="RadComboBox15_ItemsRequested"
                                                                                OpenDropDownOnLoad="false"
                                                                                ShowDropDownOnTextboxClick="false"
                                                                                HighlightTemplatedItems="true"
                                                                                EnableVirtualScrolling="true"
                                                                                Filter="StartsWith"
                                                                                EmptyMessage="Search recipient...">
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
                                                                        <th>Response Date</th>
                                                                        <td>
                                                                            <telerik:RadDatePicker ID="RadDatePicker5" runat="server" PopupDirection="TopLeft" Width="200px">
                                                                                <DateInput ID="DateInput5" runat="server" ForeColor="Black" Font-Names="Arial"
                                                                                    ClientEvents-OnError="DateOnError" EmptyMessage="Response Date">
                                                                                </DateInput>
                                                                                <Calendar ID="Calendar5" runat="server" ShowRowHeaders="false">
                                                                                    <WeekendDayStyle BackColor="yellow" />
                                                                                </Calendar>
                                                                            </telerik:RadDatePicker>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td></td>
                                                                        <td>
                                                                            <telerik:RadButton ID="RadButton33" runat="server" Text="Save and Continue Later" CausesValidation="true" SingleClick="true" SingleClickText="Processing..." BorderColor="Blue" UseSubmitBehavior="true"
                                                                                RenderMode="Lightweight" OnClick="RadButton33_Click">
                                                                                <Icon PrimaryIconCssClass="rbSave" />
                                                                            </telerik:RadButton>
                                                                            <telerik:RadButton ID="RadButton22" runat="server" Text="Cancel" SingleClick="true" CausesValidation="false" BorderColor="Blue"
                                                                                RenderMode="Lightweight" OnClientClicked="OnClientClick_RadButtonCancel">
                                                                                <Icon PrimaryIconCssClass="rbCancel" />
                                                                            </telerik:RadButton>
                                                                            <telerik:RadButton RenderMode="Lightweight" ID="RadButton11" runat="server" Text="Confirm Next" Font-Bold="true" BorderStyle="Solid" BorderColor="Blue" OnClick="RadButton11_Click">
                                                                                <Icon SecondaryIconCssClass="rbNext"></Icon>
                                                                            </telerik:RadButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </fieldset>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </telerik:RadPageView>
                                            <telerik:RadPageView runat="server" ID="RadPageView9">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <fieldset class="fieldset">
                                                                <legend style="font-size: small; font-weight: bold; font-style: italic;">*** ORIGINATOR USE ONLY *** </legend>
                                                                <table>
                                                                    <tr>
                                                                        <th>Due Date Extension</th>
                                                                        <td colspan="2">
                                                                            <telerik:RadDatePicker ID="RadDatePicker6" runat="server" PopupDirection="TopLeft" Width="220px">
                                                                                <DateInput ID="DateInput6" runat="server" ForeColor="Black" Font-Names="Arial"
                                                                                    ClientEvents-OnError="DateOnError" EmptyMessage="Due Date Extension">
                                                                                </DateInput>
                                                                                <Calendar ID="Calendar6" runat="server" ShowRowHeaders="false">
                                                                                    <WeekendDayStyle BackColor="yellow" />
                                                                                </Calendar>
                                                                            </telerik:RadDatePicker>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <th>Re-Issued To</th>
                                                                        <td>
                                                                            <telerik:RadComboBox ID="RadComboBox16" runat="server" MarkFirstMatch="true" AllowCustomText="true" Sort="Ascending"
                                                                                EnableLoadOnDemand="true" ShowMoreResultsBox="True" DropDownAutoWidth="enabled" LoadingMessage="Searching...Please wait"
                                                                                Height="200px" Width="220px" OnClientSelectedIndexChanged="RadComboBox16_OnClientSelectedIndexChanged"
                                                                                OnItemsRequested="RadComboBox16_ItemsRequested"
                                                                                OpenDropDownOnLoad="false"
                                                                                ShowDropDownOnTextboxClick="false"
                                                                                HighlightTemplatedItems="true"
                                                                                EnableVirtualScrolling="true"
                                                                                Filter="StartsWith"
                                                                                EmptyMessage="Search recipient...">
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
                                                                            (If selected, specify a date)
                                                                        </td>
                                                                        <td>
                                                                            <telerik:RadDatePicker ID="RadDatePicker7" runat="server" PopupDirection="TopLeft" Width="200px">
                                                                                <DateInput ID="DateInput7" runat="server" ForeColor="Black" Font-Names="Arial"
                                                                                    ClientEvents-OnError="DateOnError" EmptyMessage="Date Re-Issued">
                                                                                </DateInput>
                                                                                <Calendar ID="Calendar7" runat="server" ShowRowHeaders="false">
                                                                                    <WeekendDayStyle BackColor="yellow" />
                                                                                </Calendar>
                                                                            </telerik:RadDatePicker>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <th>Follow-up Required (Yes/No)</th>
                                                                        <td>
                                                                            <telerik:RadComboBox ID="RadComboBox19" runat="server" Width="220px" OnClientSelectedIndexChanged="RadComboBox19_OnClientSelectedIndexChanged">
                                                                                <Items>
                                                                                    <telerik:RadComboBoxItem Text="" Value="NA" Selected="true" />
                                                                                    <telerik:RadComboBoxItem Text="No" Value="False" />
                                                                                    <telerik:RadComboBoxItem Text="Yes" Value="True" />
                                                                                </Items>
                                                                            </telerik:RadComboBox>
                                                                            (If Yes, specify a date)
                                                                        </td>
                                                                        <td>
                                                                            <telerik:RadDatePicker ID="RadDatePicker9" runat="server" PopupDirection="TopLeft" Width="200px">
                                                                                <DateInput ID="DateInput9" runat="server" ForeColor="Black" Font-Names="Arial"
                                                                                    ClientEvents-OnError="DateOnError" EmptyMessage="Date Follow-up">
                                                                                </DateInput>
                                                                                <Calendar ID="Calendar9" runat="server" ShowRowHeaders="false">
                                                                                    <WeekendDayStyle BackColor="yellow" />
                                                                                </Calendar>
                                                                            </telerik:RadDatePicker>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <th>* Date Received</th>
                                                                        <td colspan="2">
                                                                            <telerik:RadDatePicker ID="RadDatePicker8" runat="server" PopupDirection="TopLeft" Width="220px">
                                                                                <DateInput ID="DateInput8" runat="server" ForeColor="Black" Font-Names="Arial"
                                                                                    ClientEvents-OnError="DateOnError" EmptyMessage="Date Received">
                                                                                </DateInput>
                                                                                <Calendar ID="Calendar8" runat="server" ShowRowHeaders="false">
                                                                                    <WeekendDayStyle BackColor="yellow" />
                                                                                </Calendar>
                                                                            </telerik:RadDatePicker>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <th>* Date Verified</th>
                                                                        <td colspan="2">
                                                                            <telerik:RadDatePicker ID="RadDatePicker10" runat="server" PopupDirection="TopLeft" Width="220px">
                                                                                <DateInput ID="DateInput10" runat="server" ForeColor="Black" Font-Names="Arial"
                                                                                    ClientEvents-OnError="DateOnError" EmptyMessage="Date Verified">
                                                                                </DateInput>
                                                                                <Calendar ID="Calendar10" runat="server" ShowRowHeaders="false">
                                                                                    <WeekendDayStyle BackColor="yellow" />
                                                                                </Calendar>
                                                                            </telerik:RadDatePicker>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <th>Verified By</th>
                                                                        <td colspan="2">
                                                                            <telerik:RadComboBox ID="RadComboBox17" runat="server" MarkFirstMatch="true" AllowCustomText="true" Sort="Ascending"
                                                                                EnableLoadOnDemand="true" ShowMoreResultsBox="True" DropDownAutoWidth="enabled" LoadingMessage="Searching...Please wait"
                                                                                Height="200px" Width="220px" OnClientSelectedIndexChanged="RadComboBox17_OnClientSelectedIndexChanged"
                                                                                OnItemsRequested="RadComboBox17_ItemsRequested"
                                                                                OpenDropDownOnLoad="false"
                                                                                ShowDropDownOnTextboxClick="false"
                                                                                HighlightTemplatedItems="true"
                                                                                EnableVirtualScrolling="true"
                                                                                Filter="StartsWith"
                                                                                EmptyMessage="Search recipient...">
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
                                                                        <th>Response Accepted By</th>
                                                                        <td colspan="2">
                                                                            <telerik:RadComboBox ID="RadComboBox18" runat="server" MarkFirstMatch="true" AllowCustomText="true" Sort="Ascending"
                                                                                EnableLoadOnDemand="true" ShowMoreResultsBox="True" DropDownAutoWidth="enabled" LoadingMessage="Searching...Please wait"
                                                                                Height="200px" Width="220px" OnClientSelectedIndexChanged="RadComboBox18_OnClientSelectedIndexChanged"
                                                                                OnItemsRequested="RadComboBox18_ItemsRequested"
                                                                                OpenDropDownOnLoad="false"
                                                                                ShowDropDownOnTextboxClick="false"
                                                                                HighlightTemplatedItems="true"
                                                                                EnableVirtualScrolling="true"
                                                                                Filter="StartsWith"
                                                                                EmptyMessage="Search recipient...">
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
                                                                                    <a href="Edit.aspx">Edit.aspx</a>
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
                                                                    <%--MOVE TR--%>
                                                                    <tr runat="server" id="carEV">
                                                                        <th>* How was effectiveness validated?</th>
                                                                        <td>
                                                                            <telerik:RadComboBox ID="RadComboBox21" runat="server" AllowCustomText="false" Width="250px" CheckBoxes="True"
                                                                                OnClientItemChecked="OnClientItemChecked_RadComboBox21">
                                                                            </telerik:RadComboBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr runat="server" id="carEVd">
                                                                        <th style="vertical-align: top;">* Input verification description here.</th>
                                                                        <td colspan="2">
                                                                            <telerik:RadTextBox ID="RadTextBox18" runat="server" EmptyMessage="How was effectiveness validated?" TextMode="MultiLine"
                                                                                Width="600px" Rows="5" AutoPostBack="True">
                                                                                <FocusedStyle BackColor="LightCyan" />
                                                                            </telerik:RadTextBox>
                                                                        </td>
                                                                    </tr>

                                                                    <tr>
                                                                        <th>* Change CAR Status</th>
                                                                        <td>
                                                                            <telerik:RadComboBox ID="RadComboBox20" runat="server" Width="220px" AutoPostBack="true"
                                                                                OnSelectedIndexChanged="RadComboBox20_SelectedIndexChanged">
                                                                                <%--OnClientSelectedIndexChanged="ClientSelectedIndexChanged_RadComboBOx20"--%>
                                                                            </telerik:RadComboBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td></td>
                                                                        <td style="text-align: right;" colspan="2">
                                                                            <telerik:RadButton ID="RadButton34" runat="server" Text="Save and Continue Later" CausesValidation="true" SingleClick="true" SingleClickText="Processing..." BorderColor="Blue" UseSubmitBehavior="true"
                                                                                RenderMode="Lightweight" OnClientClicking="OnClientClicking_RadButton12" OnClick="RadButton34_Click">
                                                                                <Icon PrimaryIconCssClass="rbSave" />
                                                                            </telerik:RadButton>
                                                                            <telerik:RadButton ID="RadButton23" runat="server" Text="Cancel" SingleClick="true" CausesValidation="false" BorderColor="Blue"
                                                                                RenderMode="Lightweight" OnClientClicked="OnClientClick_RadButtonCancel">
                                                                                <Icon PrimaryIconCssClass="rbCancel" />
                                                                            </telerik:RadButton>
                                                                            <telerik:RadButton RenderMode="Lightweight" ID="RadButton12" runat="server" Text="Confirm Next" Font-Bold="true" BorderStyle="Solid" BorderColor="Blue" OnClick="RadButton12_Click"
                                                                                OnClientClicking="OnClientClicking_RadButton12">
                                                                                <Icon SecondaryIconCssClass="rbNext"></Icon>
                                                                            </telerik:RadButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </fieldset>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </telerik:RadPageView>
                                            <telerik:RadPageView runat="server" ID="RadPageView10">
                                                <fieldset class="fieldset">
                                                    <table>
                                                        <tr>
                                                            <td style="vertical-align: top;">
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Panel ID="Panel1" runat="server">
                                                                                <fieldset class="fieldset">
                                                                                    <legend style="font-family: Arial; font-size: x-small; font-style: italic;">Supplier Recipients (optional)</legend>
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
                                                                            <telerik:RadButton RenderMode="Lightweight" ID="RadButton5" runat="server" Text="Update CAR and Send" BorderColor="Blue"
                                                                                CausesValidation="true" SingleClick="true" SingleClickText="Processing..." OnClientClicking="OnClientClicking_RadButton5" OnClick="RadButton5_Click">
                                                                                <Icon SecondaryIconCssClass="rbNext"></Icon>
                                                                            </telerik:RadButton>
                                                                            <telerik:RadButton ID="RadButton24" runat="server" Text="Cancel" SingleClick="true" CausesValidation="false" BorderColor="Blue"
                                                                                RenderMode="Lightweight" OnClientClicked="OnClientClick_RadButtonCancel">
                                                                                <Icon PrimaryIconCssClass="rbCancel" />
                                                                            </telerik:RadButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </fieldset>
                                            </telerik:RadPageView>
                                            <telerik:RadPageView runat="server" ID="RadPageView11">
                                                <table>
                                                    <tr>
                                                        <th>Corrective Action Request:  
                                                        </th>
                                                        <td>
                                                            <asp:Label ID="Label3" runat="server" Text="Label"></asp:Label>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <th style="vertical-align: top;">Notification sent to:</th>
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
                                                        <td></td>
                                                        <td style="text-align: right;">
                                                            <telerik:RadButton ID="RadButton6" runat="server" Text="Close Edit Form" UseSubmitBehavior="true" BorderColor="Blue" OnClientClicked="OnClientClicked_RadButton6"
                                                                SingleClick="true" SingleClickText="Processing...">
                                                                <Icon PrimaryIconCssClass="rbCancel" />
                                                            </telerik:RadButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </telerik:RadPageView>
                                        </telerik:RadMultiPage>
                                    </td>
                                </tr>
                            </table>
                            <table style="width: 100%;">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="0"></asp:Label>
                                        <asp:Label ID="Label2" runat="server" Text="0"></asp:Label>
                                    </td>
                                    <td style="text-align: right;">*  Required field to close CAR
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="vertical-align: top;">
                            <fieldset class="fieldset">
                                <legend style="font-family: Arial; font-size: x-small; font-style: italic; width: auto;">CAR Preview</legend>
                                <table>
                                    <tr>
                                        <td style="vertical-align: top;">
                                            <telerik:RadTreeView ID="RadTreeView1" runat="server" Width="100%">
                                                <Nodes>
                                                    <telerik:RadTreeNode Text="Start">
                                                        <Nodes>
                                                            <telerik:RadTreeNode Text="0">
                                                                <NodeTemplate>
                                                                    <table>
                                                                        <tr>
                                                                            <th>Originator:  </th>
                                                                            <td>
                                                                                <asp:Literal ID="Literal2" runat="server"></asp:Literal>
                                                                                (<asp:Literal ID="Literal1" runat="server"></asp:Literal>)
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <th>Date Issued:  </th>
                                                                            <td>
                                                                                <asp:Literal ID="Literal3" runat="server"></asp:Literal>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <th>Due Date:  </th>
                                                                            <td>
                                                                                <asp:Literal ID="Literal4" runat="server"></asp:Literal>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <th>Finding Type:  </th>
                                                                            <td>
                                                                                <asp:Literal ID="Literal5" runat="server"></asp:Literal>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <th>Area:  </th>
                                                                            <td>
                                                                                <asp:Literal ID="Literal6" runat="server"></asp:Literal>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <th>PSL:  </th>
                                                                            <td>
                                                                                <asp:Literal ID="Literal7" runat="server"></asp:Literal>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <th>Plant:  </th>
                                                                            <td>
                                                                                <asp:Literal ID="Literal8" runat="server"></asp:Literal>
                                                                            </td>
                                                                        </tr>

                                                                        <tr>
                                                                            <th>API / ISO Reference:  </th>
                                                                            <td>
                                                                                <telerik:RadComboBox ID="RadComboBox1" runat="server" Width="300px" Height="200px" Filter="None" DropDownAutoWidth="Enabled">
                                                                                </telerik:RadComboBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <th>Audit #:  </th>
                                                                            <td>
                                                                                <asp:Literal ID="Literal13" runat="server"></asp:Literal>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <th>Q Note #:  </th>
                                                                            <td>
                                                                                <asp:Literal ID="Literal14" runat="server"></asp:Literal>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <th>CPI Number:  </th>
                                                                            <td>
                                                                                <asp:Literal ID="Literal15" runat="server"></asp:Literal>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <th>Material #:  </th>
                                                                            <td>
                                                                                <asp:Literal ID="Literal16" runat="server"></asp:Literal>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <th>Purchase Order #:  </th>
                                                                            <td>
                                                                                <asp:Literal ID="Literal17" runat="server"></asp:Literal>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <th>Production Order #:  </th>
                                                                            <td>
                                                                                <asp:Literal ID="Literal18" runat="server"></asp:Literal>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <th>API Audit #:  </th>
                                                                            <td>
                                                                                <asp:Literal ID="Literal20" runat="server"></asp:Literal>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <th>Maintenance Order #:</th>
                                                                            <td>
                                                                                <asp:Literal ID="Literal25" runat="server"></asp:Literal>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <th>Equipment #:</th>
                                                                            <td>
                                                                                <asp:Literal ID="Literal42" runat="server"></asp:Literal>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <th>Category:  </th>
                                                                            <td>
                                                                                <asp:Literal ID="Literal9" runat="server"></asp:Literal>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <th>* Responsible Person:  </th>
                                                                            <td>
                                                                                <asp:Literal ID="Literal19" runat="server"></asp:Literal>
                                                                                (<asp:Literal ID="Literal24" runat="server"></asp:Literal>)
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <th>Description of<br />
                                                                                Finding:  </th>
                                                                            <td>
                                                                                <telerik:RadTextBox ID="RadTextBox19" runat="server" TextMode="MultiLine" Rows="5" Width="300px" ReadOnly="true"></telerik:RadTextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <th>Description of<br />
                                                                                Improvement:  </th>
                                                                            <td>
                                                                                <telerik:RadTextBox ID="RadTextBox20" runat="server" TextMode="MultiLine" Rows="5" Width="300px" ReadOnly="true"></telerik:RadTextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </NodeTemplate>
                                                            </telerik:RadTreeNode>
                                                        </Nodes>
                                                    </telerik:RadTreeNode>
                                                </Nodes>
                                                <Nodes>
                                                    <telerik:RadTreeNode Text="Supplier">
                                                        <Nodes>
                                                            <telerik:RadTreeNode Text="1">
                                                                <NodeTemplate>
                                                                    <table>
                                                                        <tr>
                                                                            <th>Number:  </th>
                                                                            <td>
                                                                                <asp:Literal ID="Literal10" runat="server"></asp:Literal>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <th>Name:  </th>
                                                                            <td>
                                                                                <asp:Literal ID="Literal11" runat="server"></asp:Literal>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </NodeTemplate>
                                                            </telerik:RadTreeNode>
                                                        </Nodes>
                                                    </telerik:RadTreeNode>
                                                </Nodes>
                                                <Nodes>
                                                    <telerik:RadTreeNode Text="Issued To">
                                                        <Nodes>
                                                            <telerik:RadTreeNode Text="2">
                                                                <NodeTemplate>
                                                                    <table>
                                                                        <tr>
                                                                            <th>Issued To:  </th>
                                                                            <td>
                                                                                <asp:Literal ID="Literal12" runat="server"></asp:Literal>
                                                                                (<asp:Literal ID="Literal21" runat="server"></asp:Literal>)
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <th>Issued To E-mail:  </th>
                                                                            <td>
                                                                                <asp:Literal ID="Literal19" runat="server"></asp:Literal>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <th>Location/Country:  </th>
                                                                            <td>
                                                                                <asp:Literal ID="Literal22" runat="server"></asp:Literal>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <th>Location/City State:</th>
                                                                            <td>
                                                                                <asp:Literal ID="Literal23" runat="server"></asp:Literal>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </NodeTemplate>
                                                            </telerik:RadTreeNode>
                                                        </Nodes>
                                                    </telerik:RadTreeNode>
                                                </Nodes>
                                                <Nodes>
                                                    <telerik:RadTreeNode Text="# 1">
                                                        <Nodes>
                                                            <telerik:RadTreeNode Text="3">
                                                                <NodeTemplate>
                                                                    <table>
                                                                        <tr>
                                                                            <th>* Why did this nonconformance occur?</th>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <telerik:RadTextBox ID="RadTextBox20" runat="server" TextMode="MultiLine" Rows="5" Width="400px" ReadOnly="true"></telerik:RadTextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </NodeTemplate>
                                                            </telerik:RadTreeNode>
                                                        </Nodes>
                                                    </telerik:RadTreeNode>
                                                </Nodes>
                                                <Nodes>
                                                    <telerik:RadTreeNode Text="# 2">
                                                        <Nodes>
                                                            <telerik:RadTreeNode Text="4">
                                                                <NodeTemplate>
                                                                    <table>
                                                                        <tr>
                                                                            <th>* What is the root cause / potential root cause of the non-conformance?</th>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <telerik:RadTextBox ID="RadTextBox20" runat="server" TextMode="MultiLine" Rows="5" Width="400px" ReadOnly="true"></telerik:RadTextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </NodeTemplate>
                                                            </telerik:RadTreeNode>
                                                        </Nodes>
                                                    </telerik:RadTreeNode>
                                                </Nodes>
                                                <Nodes>
                                                    <telerik:RadTreeNode Text="# 3">
                                                        <Nodes>
                                                            <telerik:RadTreeNode Text="5">
                                                                <NodeTemplate>
                                                                    <table>
                                                                        <tr>
                                                                            <th>* Are there similar instances of this nonconformance in your area of responsibility? 
                                                                            </th>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Literal ID="Literal24" runat="server"></asp:Literal></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <telerik:RadTextBox ID="RadTextBox20" runat="server" TextMode="MultiLine" Rows="5" Width="400px" ReadOnly="true"></telerik:RadTextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </NodeTemplate>
                                                            </telerik:RadTreeNode>
                                                        </Nodes>
                                                    </telerik:RadTreeNode>
                                                </Nodes>
                                                <Nodes>
                                                    <telerik:RadTreeNode Text="# 4">
                                                        <Nodes>
                                                            <telerik:RadTreeNode Text="6">
                                                                <NodeTemplate>
                                                                    <table>
                                                                        <tr>
                                                                            <th>* What action was taken (or is planned) to correct this nonconformance?
                                                                            </th>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <telerik:RadTextBox ID="RadTextBox20" runat="server" TextMode="MultiLine" Rows="5" Width="400px" ReadOnly="true"></telerik:RadTextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <th>* What is the scheduled implementation date?
                                                                            </th>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Literal ID="Literal24" runat="server"></asp:Literal></td>
                                                                        </tr>
                                                                    </table>
                                                                </NodeTemplate>
                                                            </telerik:RadTreeNode>
                                                        </Nodes>
                                                    </telerik:RadTreeNode>
                                                </Nodes>
                                                <Nodes>
                                                    <telerik:RadTreeNode Text="# 5">
                                                        <Nodes>
                                                            <telerik:RadTreeNode Text="7">
                                                                <NodeTemplate>
                                                                    <table>
                                                                        <tr>
                                                                            <th>* What action was taken (or is planned) to preclude this and similar non-conformances?
                                                                            </th>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <telerik:RadTextBox ID="RadTextBox20" runat="server" TextMode="MultiLine" Rows="5" Width="400px" ReadOnly="true"></telerik:RadTextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <th>* What is the scheduled implementation date?
                                                                            </th>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Literal ID="Literal24" runat="server"></asp:Literal></td>
                                                                        </tr>
                                                                    </table>
                                                                </NodeTemplate>
                                                            </telerik:RadTreeNode>
                                                        </Nodes>
                                                    </telerik:RadTreeNode>
                                                </Nodes>
                                                <Nodes>
                                                    <telerik:RadTreeNode Text="Action Taken By">
                                                        <Nodes>
                                                            <telerik:RadTreeNode Text="8">
                                                                <NodeTemplate>
                                                                    <table>
                                                                        <tr>
                                                                            <th>Action Taken By:  </th>
                                                                            <td>
                                                                                <asp:Literal ID="Literal25" runat="server"></asp:Literal>
                                                                                (<asp:Literal ID="Literal26" runat="server"></asp:Literal>)
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <th>Reponse Date:  </th>
                                                                            <td>
                                                                                <asp:Literal ID="Literal27" runat="server"></asp:Literal>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </NodeTemplate>
                                                            </telerik:RadTreeNode>
                                                        </Nodes>
                                                    </telerik:RadTreeNode>
                                                </Nodes>
                                                <Nodes>
                                                    <telerik:RadTreeNode Text="Originator Use Only">
                                                        <Nodes>
                                                            <telerik:RadTreeNode Text="9">
                                                                <NodeTemplate>
                                                                    <table>
                                                                        <tr>
                                                                            <th>Due Date Extension</th>
                                                                            <td>
                                                                                <asp:Literal ID="Literal28" runat="server"></asp:Literal>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <th>Re-Issued To:  </th>
                                                                            <td>
                                                                                <asp:Literal ID="Literal29" runat="server"></asp:Literal>
                                                                                (<asp:Literal ID="Literal30" runat="server"></asp:Literal>)
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <th>Date Re-Issued:  </th>
                                                                            <td>
                                                                                <asp:Literal ID="Literal31" runat="server"></asp:Literal>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <th>Follow-up Required:  </th>
                                                                            <td>
                                                                                <asp:Literal ID="Literal33" runat="server"></asp:Literal>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <th>Date to Follow-up:  </th>
                                                                            <td>
                                                                                <asp:Literal ID="Literal34" runat="server"></asp:Literal>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <th>* Date Received:  </th>
                                                                            <td>
                                                                                <asp:Literal ID="Literal32" runat="server"></asp:Literal>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <th>* Date Verified:  </th>
                                                                            <td>
                                                                                <asp:Literal ID="Literal35" runat="server"></asp:Literal>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <th>Verified By:  </th>
                                                                            <td>
                                                                                <asp:Literal ID="Literal36" runat="server"></asp:Literal>
                                                                                (<asp:Literal ID="Literal37" runat="server"></asp:Literal>)
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <th>Response Accepted By:  </th>
                                                                            <td>
                                                                                <asp:Literal ID="Literal38" runat="server"></asp:Literal>
                                                                                (<asp:Literal ID="Literal39" runat="server"></asp:Literal>)
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <th>* Status:  </th>
                                                                            <td>
                                                                                <asp:Literal ID="Literal41" runat="server"></asp:Literal>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <th>* Close Date:  </th>
                                                                            <td>
                                                                                <asp:Literal ID="Literal40" runat="server"></asp:Literal>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <th colspan="2">How was effectiveness validated?
                                                                            </th>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2">
                                                                                <telerik:RadTextBox ID="RadTextBox21" runat="server" TextMode="MultiLine" Rows="5" Width="400px" ReadOnly="true"></telerik:RadTextBox>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </NodeTemplate>
                                                            </telerik:RadTreeNode>
                                                        </Nodes>
                                                    </telerik:RadTreeNode>
                                                </Nodes>
                                            </telerik:RadTreeView>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:HiddenField ID="HiddenAreaOid" runat="server" />

            <asp:HiddenField ID="HiddenCountryOid" runat="server" />
            >
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

            <asp:HiddenField ID="HiddenResponsibleUserId" runat="server" />
            <asp:HiddenField ID="HiddenResponsibleUserName" runat="server" />
            <asp:HiddenField ID="HiddenResponsibleUserEmail" runat="server" />

            <asp:HiddenField ID="HiddenActionTakenUserId" runat="server" />
            <asp:HiddenField ID="HiddenActionTakenUserName" runat="server" />
            <asp:HiddenField ID="HiddenActionTakenUserEmail" runat="server" />

            <asp:HiddenField ID="HiddenReIssuedUserId" runat="server" />
            <asp:HiddenField ID="HiddenReIssuedUserName" runat="server" />
            <asp:HiddenField ID="HiddenReIssuedUserEmail" runat="server" />

            <asp:HiddenField ID="HiddenVerifiedByUserId" runat="server" />
            <asp:HiddenField ID="HiddenVerifiedByUserName" runat="server" />
            <asp:HiddenField ID="HiddenVerifiedByUserEmail" runat="server" />

            <asp:HiddenField ID="HiddenAcceptedByUserId" runat="server" />
            <asp:HiddenField ID="HiddenAcceptedByUserName" runat="server" />
            <asp:HiddenField ID="HiddenAcceptedByUserEmail" runat="server" />

            <asp:HiddenField ID="RequiredField_Responsible" runat="server" />
            <asp:HiddenField ID="RequiredField_Q1" runat="server" />
            <asp:HiddenField ID="RequiredField_Q2" runat="server" />
            <asp:HiddenField ID="RequiredField_Q3YN" runat="server" />
            <asp:HiddenField ID="RequiredField_Q3" runat="server" />
            <asp:HiddenField ID="RequiredField_Q4" runat="server" />
            <asp:HiddenField ID="RequiredField_Q4Date" runat="server" />
            <asp:HiddenField ID="RequiredField_Q5" runat="server" />
            <asp:HiddenField ID="RequiredField_Q5Date" runat="server" />

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
