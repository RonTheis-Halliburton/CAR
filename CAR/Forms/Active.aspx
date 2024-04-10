<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Active.aspx.cs" Inherits="CAR.Forms.Active" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Active Requests</title>
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
                splitterPageWnd.MainParent();
            }


            function MenuClientItemClicking(sender, eventArgs) {
                var itemValue = eventArgs.get_item().get_value();
                var rowOid = document.getElementById('<%=RowOid.ClientID %>').value;
                var rowIndex = document.getElementById('<%=RowIndex.ClientID %>').value;

                var masterTable = $find("<%= RadGrid1.ClientID %>").get_masterTableView();
                masterTable.clearSelectedItems();
                masterTable.selectItem(masterTable.get_dataItems()[rowIndex].get_element());

                //var statusEv = masterTable.getCellByColumnUniqueName(masterTable.get_dataItems()[rowIndex], "Car_Ev").innerHTML;
                var statusOid = masterTable.getCellByColumnUniqueName(masterTable.get_dataItems()[rowIndex], "CAR_STATUS_OID").innerHTML;
                var statusNm = masterTable.getCellByColumnUniqueName(masterTable.get_dataItems()[rowIndex], "CAR_STATUS_NM").innerHTML;
                var delFlg = masterTable.getCellByColumnUniqueName(masterTable.get_dataItems()[rowIndex], "DEL_FLG").innerHTML;
                var carNbr = masterTable.getCellByColumnUniqueName(masterTable.get_dataItems()[rowIndex], "CAR_NBR").innerHTML;
                var yearIssued = masterTable.getCellByColumnUniqueName(masterTable.get_dataItems()[rowIndex], "YEAR_ISSUED").innerHTML;
                var areaOid = masterTable.getCellByColumnUniqueName(masterTable.get_dataItems()[rowIndex], "AREA_DESCRIPT_OID").innerHTML;
                var vendorNbr = masterTable.getCellByColumnUniqueName(masterTable.get_dataItems()[rowIndex], "VNDR_NBR").innerHTML;
                var vendorNm = masterTable.getCellByColumnUniqueName(masterTable.get_dataItems()[rowIndex], "VENDOR_NM").innerHTML;

                switch (itemValue) {
                    case "Edit":
                        if (statusOid != 3 && delFlg == "False") {
                            var oManager = GetRadWindowManager();
                            var urlForm = "Edit.aspx?OID=" + rowOid + "&Car_Nbr=" + carNbr + "&Status_Nm=" + statusNm;

                            var oWindow = oManager.Open(urlForm, "oEditDialog");
                            oWindow.SetModal(true);
                            oWindow.MoveTo(0, 0);
                            oWindow.Maximize();
                            //oWindow.SetSize(1000, 700);
                            oWindow.set_centerIfModal(false);
                        }
                        else {
                            alert("Cannot edit a closed or deleted CAR. \n\nPlease contact a Quality Administrator")
                        }

                        eventArgs.set_cancel(true);
                        return false;

                        break;
                    case "Message":
                        var oManager = GetRadWindowManager();
                        var urlForm = "Message.aspx?OID=" + rowOid + "&Car_Nbr=" + carNbr + "&Status_Nm=" + statusNm + "&areaOid=" + areaOid + "&vendorNbr=" + vendorNbr + "&vendorNm=" + vendorNm;

                        var oWindow = oManager.Open(urlForm, "oMessageDialog");
                        oWindow.SetModal(true);
                        oWindow.MoveTo(0, 0);
                        //oWindow.Maximize();
                        oWindow.SetSize(850, 480);
                        oWindow.set_centerIfModal(false);

                        eventArgs.set_cancel(true);
                        return false;
                        break;
                    case "View":
                        var strUrl = "View.aspx?OID=" + rowOid + "&Car_Nbr=" + carNbr;
                        window.open(strUrl, 'Print', 'width=400,height=200,status=yes, sizeable=no');

                        eventArgs.set_cancel(true);
                        return false;
                        break;
                    case "ViewDocx":
                        var strUrl = "ViewDocx.aspx?OID=" + rowOid + "&Car_Nbr=" + carNbr;
                        window.open(strUrl, 'Print', 'width=400,height=200,status=yes, sizeable=no');

                        eventArgs.set_cancel(true);
                        return false;
                        break;
                    case "Upload":

                        var oManager = GetRadWindowManager();
                        var urlForm = "Document.aspx?OID=" + rowOid + "&Car_Nbr=" + carNbr + "&Status_Nm=" + statusNm + "&Year_Issued=" + yearIssued;

                        var oWindow = oManager.Open(urlForm, "oUploadDialog");
                        oWindow.SetModal(true);
                        oWindow.MoveTo(0, 0);
                        oWindow.Maximize();
                        //oWindow.SetSize(1000, 700);
                        oWindow.set_centerIfModal(false);

                        eventArgs.set_cancel(true);
                        return false;
                        break;

                    case "Index":
                        var oManager = GetRadWindowManager();
                        var urlForm = "Index.aspx?OID=" + rowOid + "&Car_Nbr=" + carNbr + "&Status_Nm=" + statusNm + "&Year_Issued=" + yearIssued;

                        var oWindow = oManager.Open(urlForm, "oIndexDialog");
                        oWindow.set_behaviors(Telerik.Web.UI.WindowBehaviors.None);
                        oWindow.SetModal(true);
                        oWindow.MoveTo(0, 0);
                        oWindow.Maximize();
                        oWindow.set_centerIfModal(false);

                        eventArgs.set_cancel(true);
                        return false;
                        break;
                    default:

                        var delConf = window.confirm(itemValue + " CAR # " + carNbr + "\n\nDo you want to proceed?\n\nClick OK - to proceed");

                        if (!delConf) {
                            eventArgs.set_cancel(true);
                            return false;
                        }

                        return false;
                        break;
                }
            }

            function ShowMenu(evt, oid, rowIndex) {
                var menu = $find("<%=RadContextMenu1.ClientID %>");

                document.getElementById('<%=RowOid.ClientID %>').value = oid;
                document.getElementById('<%=RowIndex.ClientID %>').value = rowIndex;

                var masterTable = $find("<%= RadGrid1.ClientID %>").get_masterTableView();
                masterTable.clearSelectedItems();
                masterTable.selectItem(masterTable.get_dataItems()[rowIndex].get_element());

                var carAdmin = masterTable.getCellByColumnUniqueName(masterTable.get_dataItems()[rowIndex], "CAR_ADMIN").innerHTML;
                var delFlg = masterTable.getCellByColumnUniqueName(masterTable.get_dataItems()[rowIndex], "DEL_FLG").innerHTML;
                //var statusEv = masterTable.getCellByColumnUniqueName(masterTable.get_dataItems()[rowIndex], "Car_Ev").innerHTML;
                var statusOid = masterTable.getCellByColumnUniqueName(masterTable.get_dataItems()[rowIndex], "CAR_STATUS_OID").innerHTML;
                var originatorID = masterTable.getCellByColumnUniqueName(masterTable.get_dataItems()[rowIndex], "ORIGINATOR_USR_ID").innerHTML;
                var originatorMgrID = masterTable.getCellByColumnUniqueName(masterTable.get_dataItems()[rowIndex], "ORIGINATOR_MGR_ID").innerHTML;
                var verifiedID = masterTable.getCellByColumnUniqueName(masterTable.get_dataItems()[rowIndex], "VERIFIED_BY_USR_ID").innerHTML;
                var qdmsIndexed = masterTable.getCellByColumnUniqueName(masterTable.get_dataItems()[rowIndex], "QDMS_INDEXED").innerHTML;
                var createBy = masterTable.getCellByColumnUniqueName(masterTable.get_dataItems()[rowIndex], "CREATE_BY").innerHTML;
                var issuedToUsrId = masterTable.getCellByColumnUniqueName(masterTable.get_dataItems()[rowIndex], "ISSUED_TO_USR_ID").innerHTML;
                var curUsrID = document.getElementById('<%=CurrentUserID.ClientID %>').value;

                if (carAdmin == "True") {

                    if (delFlg == "True") {
                        menu.get_items().getItem(0).set_visible(false); //Delete
                        menu.get_items().getItem(1).set_visible(false); //re-open
                        menu.get_items().getItem(2).set_visible(false); //edit
                        menu.get_items().getItem(3).set_visible(false); //Index QDMS
                        menu.get_items().getItem(6).set_visible(true); //admin override - undelete
                        menu.get_items().getItem(7).set_visible(false); //file upload
                    }
                    else {
                        menu.get_items().getItem(6).set_visible(false); //admin override - undelete

                        if (statusOid == 3) { //Closed
                            menu.get_items().getItem(0).set_visible(false); //Delete
                            menu.get_items().getItem(1).set_visible(true); //re-open
                            menu.get_items().getItem(2).set_visible(false); //edit
                            menu.get_items().getItem(8).set_visible(false); //file upload

                            if (qdmsIndexed == 0) {
                                menu.get_items().getItem(3).set_visible(true); //Index QDMS
                            }
                            else {
                                menu.get_items().getItem(3).set_visible(false); //Index QDMS
                            }
                        }
                        else {
                            menu.get_items().getItem(0).set_visible(true); //Delete
                            menu.get_items().getItem(1).set_visible(false); //re-open
                            menu.get_items().getItem(2).set_visible(true); //edit
                            menu.get_items().getItem(3).set_visible(false); //Index QDMS
                            menu.get_items().getItem(8).set_visible(true); //file upload
                        }
                    }
                }
                else {

                    menu.get_items().getItem(8).set_visible(false); //file upload
                    menu.get_items().getItem(7).set_visible(false); //Send Message
                    menu.get_items().getItem(6).set_visible(false); //admin override - undelete
                    menu.get_items().getItem(3).set_visible(false); //Index QDMS
                    menu.get_items().getItem(2).set_visible(false); //edit
                    menu.get_items().getItem(1).set_visible(false); //re-open
                    menu.get_items().getItem(0).set_visible(false); //Delete

                    if (originatorID == curUsrID || verifiedID == curUsrID || originatorMgrID == curUsrID || createBy == curUsrID || issuedToUsrId == curUsrID) {
                        menu.get_items().getItem(2).set_visible(true); //edit
                        menu.get_items().getItem(7).set_visible(true); //Send Message
                        menu.get_items().getItem(8).set_visible(true); //file upload
                    }
                }

                evt.cancelBubble = true;
                evt.returnValue = false;

                if (evt.stopPropagation) {
                    evt.stopPropagation();
                    evt.preventDefault();
                }

                menu.show(evt);
            }

            function MenuClientShowing(sender, args) {
                var element = sender.get_contextMenuElement();
                var handler = function (e) {
                    var relatedTarget = e.rawEvent.relatedTarget || e.rawEvent.toElement;
                    if (!$telerik.isDescendantOrSelf(element, relatedTarget)) {
                        sender.hide();
                        $removeHandler(element, "mouseout", handler);
                        return;
                    }
                };
                $addHandler(element, "mouseout", handler);
            }

            function CallBackFunction(oWnd, eventArgs) {
                //get the transferred arguments
                //var arg = eventArgs.get_argument();
                var ajaxManager = $find("<%= RadAjaxPanel1.ClientID %>");

                if (oWnd) {

                    ajaxManager.ajaxRequest(oWnd.get_name());
                }
            }

        </script>
    </telerik:RadCodeBlock>
    <form id="form1" runat="server" defaultbutton="RadButton1">
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
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" OnAjaxRequest="RadAjaxPanel1_AjaxRequest" Height="100%" LoadingPanelID="LoadingPanel1">
            <div class="contentContainer">
                <fieldset class="fieldset">
                    <legend style="font-family: Arial; ">Active Requests</legend>
                    <table>
                        <tr>
                            <td>
                                <fieldset class="fieldset">
                                    <legend>Plant</legend>
                                    <telerik:RadComboBox ID="RadComboBox2" runat="server" MarkFirstMatch="True" Width="300px" Height="250px" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" DropDownAutoWidth="Enabled"></telerik:RadComboBox>
                                </fieldset>
                            </td>
                            <td>
                                <fieldset class="fieldset">
                                    <legend>PSL</legend>
                                    <telerik:RadComboBox ID="RadComboBox3" runat="server" MarkFirstMatch="True" Width="300px" DropDownWidth="300px" Height="250px" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"></telerik:RadComboBox>
                                </fieldset>
                            </td>
                            <td>
                                <telerik:RadButton ID="RadButton1" runat="server" Text="Search" RenderMode="Lightweight" BorderColor="Blue"
                                    SingleClick="true" SingleClickText="Searching..." Value="Search" CausesValidation="false" OnClick="RadButton1_Click" ButtonType="SkinnedButton">
                                    <Icon PrimaryIconCssClass="rbSearch"></Icon>
                                </telerik:RadButton>
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox1" runat="server" Checked="true" Text="Created by me" />
                            </td>
                        </tr>
                    </table>
                    <telerik:RadGrid ID="RadGrid1" runat="server" AutoGenerateColumns="False" ShowStatusBar="true" AllowPaging="True" PageSize="100"
                        FilterType="HeaderContext" EnableHeaderContextMenu="true" EnableHeaderContextFilterMenu="true"
                        GroupingEnabled="false" GroupPanel-Enabled="false" ShowGroupPanel="false" AllowSorting="true"
                        OnNeedDataSource="RadGrid1_NeedDataSource" OnItemCommand="RadGrid1_ItemCommand" OnItemCreated="RadGrid1_ItemCreated" OnItemDataBound="RadGrid1_ItemDataBound"
                        OnPreRender="RadGrid1_PreRender" OnFilterCheckListItemsRequested="RadGrid1_FilterCheckListItemsRequested" OnExcelMLWorkBookCreated="RadGrid1_ExcelMLWorkBookCreated">
                        <GroupingSettings CaseSensitive="false" />
                        <HeaderStyle HorizontalAlign="Center" Wrap="true" Font-Size="X-Small" Font-Bold="True" />
                        <StatusBarSettings LoadingText="Please wait...Loading data" />
                        <PagerStyle Mode="NextPrev" Position="TopAndBottom" />
                        <ClientSettings EnableRowHoverStyle="true">
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="600px" />
                            <Resizing AllowColumnResize="true" />
                        </ClientSettings>
                        <ExportSettings HideStructureColumns="true" ExportOnlyData="true" IgnorePaging="true"
                            OpenInNewWindow="true" Excel-Format="ExcelML">
                        </ExportSettings>
                        <MasterTableView DataKeyNames="OID, CAR_NBR" Width="100%" NoMasterRecordsText="No records to display." Name="TopLevel" EnableNoRecordsTemplate="true"
                            ShowHeadersWhenNoRecords="true" CommandItemDisplay="Top" ShowHeader="true" AllowFilteringByColumn="True">
                            <CommandItemSettings ExportToExcelImageUrl="../Img/Excel.jpg" ShowExportToExcelButton="true" ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Menu" UniqueName="MENU" EnableHeaderContextMenu="false">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Img/group_gear32.png" ToolTip="Menu" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="50px" Font-Bold="true" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="CAR_NBR" HeaderText="CAR #" SortExpression="CAR_NBR" UniqueName="CAR_NBR">
                                    <HeaderStyle Width="90px" HorizontalAlign="Center" />
                                    <ItemStyle Font-Bold="true" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="AREA_DESCR" HeaderText="AREA" SortExpression="AREA_DESCR" UniqueName="AREA_DESCR">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="FINDING_TYPE" HeaderText="FINDING TYPE" SortExpression="FINDING_TYPE" UniqueName="FINDING_TYPE">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ORIGINATOR_NM" HeaderText="ORIGINATOR" SortExpression="ORIGINATOR_NM" UniqueName="ORIGINATOR_NM">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ISSUED_TO_NM" HeaderText="ISSUED TO" SortExpression="ISSUED_TO_NM" UniqueName="ISSUED_TO_NM">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="DATE_ISSUED_NM" HeaderText="DATE ISSUED" SortExpression="DATE_ISSUED_NM" UniqueName="DATE_ISSUED_NM">
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="DUE_DT_NM" HeaderText="DUE DATE" SortExpression="DUE_DT_NM" UniqueName="DUE_DT_NM">
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="VNDR_NBR" HeaderText="VENDOR NUMBER" SortExpression="VNDR_NBR" UniqueName="VNDR_NBR">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="VENDOR_NM" HeaderText="VENDOR NAME" SortExpression="VENDOR_NM" UniqueName="VENDOR_NM">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="PSL" HeaderText="PSL" SortExpression="PSL" UniqueName="PSL">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="FACILITY_NM" HeaderText="PLANT" SortExpression="FACILITY_NM" UniqueName="FACILITY_NM">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="VERIFIED_BY_NM" HeaderText="Verified By" SortExpression="VERIFIED_BY_NM" UniqueName="VERIFIED_BY_NM">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="CLOSE_DT" HeaderText="CLOSED DATE" SortExpression="CLOSE_DT" UniqueName="CLOSE_DT">
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn UniqueName="TemplateStatusColumn" HeaderText="STATUS" AllowSorting="true" SortExpression="CAR_STATUS_NM" EnableHeaderContextMenu="false">
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <table border="0">
                                            <tr>
                                                <td rowspan="3" style="vertical-align: central;">
                                                    <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Img/Lock32.png" ToolTip="Click to View" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label1" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label2" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label3" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="CAR_STATUS_NM" HeaderText="STATUS" SortExpression="CAR_STATUS_NM" UniqueName="CAR_STATUS_NM" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="AUDIT_NBR" HeaderText="AUDIT_NBR" SortExpression="AUDIT_NBR" UniqueName="AUDIT_NBR" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="QNOTE_NBR" HeaderText="QNOTE_NBR" SortExpression="QNOTE_NBR" UniqueName="QNOTE_NBR" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="CPI_NBR" HeaderText="CPI_NBR" SortExpression="CPI_NBR" UniqueName="CPI_NBR" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="MATERIAL_NBR" HeaderText="MATERIAL_NBR" SortExpression="MATERIAL_NBR" UniqueName="MATERIAL_NBR" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="PURCHASE_ORDER_NBR" HeaderText="PURCHASE_ORDER_NBR" SortExpression="PURCHASE_ORDER_NBR" UniqueName="PURCHASE_ORDER_NBR" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="PRODUCTION_ORDER_NBR" HeaderText="PRODUCTION_ORDER_NBR" SortExpression="PRODUCTION_ORDER_NBR" UniqueName="PRODUCTION_ORDER_NBR" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="API_AUDIT_NBR" HeaderText="API_AUDIT_NBR" SortExpression="API_AUDIT_NBR" UniqueName="API_AUDIT_NBR" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="MAINTENANCE_ORDER_NBR" HeaderText="MAINTENANCE_ORDER_NBR" SortExpression="MAINTENANCE_ORDER_NBR" UniqueName="MAINTENANCE_ORDER_NBR" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="EQUIPMENT_NBR" HeaderText="EQUIPMENT_NBR" SortExpression="EQUIPMENT_NBR" UniqueName="EQUIPMENT_NBR" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="CATEGORY_NM" HeaderText="CATEGORY" SortExpression="CATEGORY_NM" UniqueName="CATEGORY_NM" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="RESP_PERSON_NM" HeaderText="RESPONSIBLE_PERSON" SortExpression="RESP_PERSON_NM" UniqueName="RESP_PERSON_NM" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="FINDING_DESC" HeaderText="DESC_OF_FINDING" SortExpression="FINDING_DESC" UniqueName="FINDING_DESC" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="DESC_OF_IMPROVEMENT" HeaderText="DESC_OF_IMPROVEMENT" SortExpression="DESC_OF_IMPROVEMENT" UniqueName="DESC_OF_IMPROVEMENT" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="LOC_COUNTRY_NM" HeaderText="LOCATION_COUNTRY" SortExpression="LOC_COUNTRY_NM" UniqueName="LOC_COUNTRY_NM" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="LOC_SUPPLIER" HeaderText="LOCATION_CITY_STATE" SortExpression="LOC_SUPPLIER" UniqueName="LOC_SUPPLIER" Display="false">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn DataField="ACTION_TAKEN_BY_NM" HeaderText="ACTION_TAKEN_BY" SortExpression="ACTION_TAKEN_BY_NM" UniqueName="ACTION_TAKEN_BY_NM" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="RESPONSE_DT" HeaderText="RESPONSE_DATE" SortExpression="RESPONSE_DT" UniqueName="RESPONSE_DT" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="NON_CONFORM_RSN" HeaderText="Why did this nonconformance occur?" SortExpression="NON_CONFORM_RSN" UniqueName="NON_CONFORM_RSN" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ROOT_CAUSE" HeaderText="What is the root cause / potential root cause of the non-conformance?" SortExpression="ROOT_CAUSE" UniqueName="ROOT_CAUSE" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="SIMILAR_INSTANCE_Y_N_TEXT" HeaderText="Are there similar instances of this nonconformance in your area of responsibility? (Y/N)" SortExpression="SIMILAR_INSTANCE_Y_N_TEXT" UniqueName="SIMILAR_INSTANCE_Y_N_TEXT" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="SIMILAR_INSTANCE" HeaderText="SIMILAR_INSTANCE" SortExpression="SIMILAR_INSTANCE" UniqueName="SIMILAR_INSTANCE" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="CORR_ACTION_TAKEN" HeaderText="What action was taken (or is planned) to correct this nonconformance?" SortExpression="CORR_ACTION_TAKEN" UniqueName="CORR_ACTION_TAKEN" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="CORR_ACTION_TAKEN_DT" HeaderText="What is the scheduled implementation date?" SortExpression="CORR_ACTION_TAKEN_DT" UniqueName="CORR_ACTION_TAKEN_DT" Display="false">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn DataField="DUE_DT_EXT" HeaderText="DUE_DATE_EXTENSION" SortExpression="DUE_DT_EXT" UniqueName="DUE_DT_EXT" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="REISSUED_TO_NM" HeaderText="REISSUED_TO" SortExpression="REISSUED_TO_NM" UniqueName="REISSUED_TO_NM" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="REISSUED_DT" HeaderText="REISSUED_DATE" SortExpression="REISSUED_DT" UniqueName="REISSUED_DT" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="FOLLOW_UP_REQD" HeaderText="FOLLOW_UP_REQUIRED" SortExpression="FOLLOW_UP_REQD" UniqueName="FOLLOW_UP_REQD" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="FOLLOW_UP_DT" HeaderText="FOLLOW_UP_DATE" SortExpression="FOLLOW_UP_DT" UniqueName="FOLLOW_UP_DT" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="RECEIVED_DT" HeaderText="RECEIVED_DT" SortExpression="RECEIVED_DT" UniqueName="RECEIVED_DT" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="VERIFY_DT" HeaderText="VERIFY_DT" SortExpression="VERIFY_DT" UniqueName="VERIFY_DT" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="VERIFIED_BY_NM" HeaderText="VERIFIED_BY" SortExpression="VERIFIED_BY_NM" UniqueName="VERIFIED_BY_NM" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="RESPONSE_ACCEPT_BY_NM" HeaderText="RESPONSE_ACCEPT_BY" SortExpression="RESPONSE_ACCEPT_BY_NM" UniqueName="RESPONSE_ACCEPT_BY_NM" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="REMARKS" HeaderText="How was effectiveness validated?" SortExpression="REMARKS" UniqueName="REMARKS" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="QDMS_INDEXED_NM" HeaderText="Indexed QDMS" SortExpression="QDMS_INDEXED_NM" UniqueName="QDMS_INDEXED_NM" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="QDMS_INDEXED_BY_NM" HeaderText="Indexed QDMS By" SortExpression="QDMS_INDEXED_BY_NM" UniqueName="QDMS_INDEXED_BY_NM" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="QDMS_INDEXED_DT" HeaderText="Indexed QDMS Date" SortExpression="QDMS_INDEXED_DT" UniqueName="QDMS_INDEXED_DT" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="QDMS_INDEXED" HeaderText="QDMS_INDEXED" SortExpression="QDMS_INDEXED" UniqueName="QDMS_INDEXED" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="Car_Ev" HeaderText="Car_Ev" SortExpression="Car_Ev"
                                    UniqueName="Car_Ev" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="CAR_STATUS_OID" HeaderText="CAR_STATUS_OID" SortExpression="CAR_STATUS_OID"
                                    UniqueName="CAR_STATUS_OID" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ORIGINATOR_USR_ID" HeaderText="ORIGINATOR_USR_ID" SortExpression="ORIGINATOR_USR_ID" UniqueName="ORIGINATOR_USR_ID" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="VERIFIED_BY_USR_ID" HeaderText="VERIFIED_BY_USR_ID" SortExpression="VERIFIED_BY_USR_ID" UniqueName="VERIFIED_BY_USR_ID" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="CAR_STATUS_NM" HeaderText="CAR_STATUS_NM" SortExpression="CAR_STATUS_NM" UniqueName="CAR_STATUS_NM" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="CATEGORY_NM" HeaderText="CATEGORY_NM" SortExpression="CATEGORY_NM" UniqueName="CATEGORY_NM" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="API_ISO_ELEM" HeaderText="API_ISO_ELEM" SortExpression="API_ISO_ELEM" UniqueName="API_ISO_ELEM" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="DATE_ISSUED" HeaderText="DATE_ISSUED" SortExpression="DATE_ISSUED" UniqueName="DATE_ISSUED" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="DUE_DT" HeaderText="DUE_DT" SortExpression="DUE_DT" UniqueName="DUE_DT" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="CLOSE_DT" HeaderText="CLOSE_DT" SortExpression="CLOSE_DT" UniqueName="CLOSE_DT" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="AWAITING_DAYS" HeaderText="AWAITING_DAYS" SortExpression="AWAITING_DAYS" UniqueName="AWAITING_DAYS" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="CLOSED_DAYS" HeaderText="CLOSED_DAYS" SortExpression="CLOSED_DAYS" UniqueName="CLOSED_DAYS" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="YEAR_ISSUED" HeaderText="YEAR ISSUED" SortExpression="YEAR_ISSUED" UniqueName="YEAR_ISSUED" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ORIGINATOR_MGR" HeaderText="ORIGINATOR_MGR" SortExpression="ORIGINATOR_MGR"
                                    UniqueName="ORIGINATOR_MGR" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ORIGINATOR_MGR_EMAIL" HeaderText="ORIGINATOR_MGR_EMAIL" SortExpression="ORIGINATOR_MGR_EMAIL"
                                    UniqueName="ORIGINATOR_MGR_EMAIL" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ORIGINATOR_MGR_ID" HeaderText="ORIGINATOR_MGR_ID" SortExpression="ORIGINATOR_MGR_ID"
                                    UniqueName="ORIGINATOR_MGR_ID" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="CAR_ADMIN" HeaderText="CAR_ADMIN" SortExpression="CAR_ADMIN"
                                    UniqueName="CAR_ADMIN" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="DEL_FLG" HeaderText="DEL_FLG" SortExpression="DEL_FLG"
                                    UniqueName="DEL_FLG" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="API_ISO_OID" HeaderText="API_ISO_OID" SortExpression="API_ISO_OID"
                                    UniqueName="API_ISO_OID" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="CATEGORY_OID" HeaderText="CATEGORY_OID" SortExpression="CATEGORY_OID"
                                    UniqueName="CATEGORY_OID" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="FACILITY_NAME_OID" HeaderText="FACILITY_NAME_OID" SortExpression="FACILITY_NAME_OID"
                                    UniqueName="FACILITY_NAME_OID" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="FINDING_TYPE_OID" HeaderText="FINDING_TYPE_OID" SortExpression="FINDING_TYPE_OID"
                                    UniqueName="FINDING_TYPE_OID" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="AREA_DESCRIPT_OID" HeaderText="AREA_DESCRIPT_OID" SortExpression="AREA_DESCRIPT_OID"
                                    UniqueName="AREA_DESCRIPT_OID" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="OID" HeaderText="OID" SortExpression="OID"
                                    UniqueName="OID" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="CREATE_BY" HeaderText="CREATE_BY" SortExpression="CREATE_BY"
                                    UniqueName="CREATE_BY" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ISSUED_TO_USR_ID" HeaderText="ISSUED_TO_USR_ID" SortExpression="ISSUED_TO_USR_ID"
                                    UniqueName="ISSUED_TO_USR_ID" Display="false">
                                </telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                    <telerik:RadContextMenu ID="RadContextMenu1" runat="server" EnableRoundedCorners="true" EnableAutoScroll="true"
                        OnClientItemClicking="MenuClientItemClicking" OnClientShowing="MenuClientShowing"
                        OnItemClick="RadContextMenu1_ItemClick">
                        <DefaultGroupSettings ExpandDirection="Right" Height="150px" />
                        <Items>             
                            <telerik:RadMenuItem Text="Delete" ImageUrl="../Img/Delete.png" Value="Delete" Font-Size="Small" Font-Bold="true" />
                            <telerik:RadMenuItem Text="Re-Open" ImageUrl="../Img/Lock_Open.png" Value="Re-Open" Font-Size="Small" Font-Bold="true" />
                            <telerik:RadMenuItem Text="Edit" ImageUrl="../Img/Edit.gif" Value="Edit" Font-Size="Small" Font-Bold="true" />
                            <telerik:RadMenuItem Text="Index to QDMS" ImageUrl="../Img/database_add.png" Value="Index" Font-Size="Small" Font-Bold="true" />
                            <telerik:RadMenuItem Text="Print PDF Document" ImageUrl="../Img/page_white_acrobat.png" Value="View" Font-Size="Small" Font-Bold="true" />
                            <telerik:RadMenuItem Text="Print Word Document" ImageUrl="../Img/Paste_Word.png" Value="ViewDocx" Font-Size="Small" Font-Bold="true" />
                            <telerik:RadMenuItem Text="Undelete" ImageUrl="../Img/add.png" Value="Undelete" Font-Size="Small" Font-Bold="true" />
                            <telerik:RadMenuItem Text="Send Message" ImageUrl="../Img/email.png" Value="Message" Font-Size="Small" Font-Bold="true" />
                            <telerik:RadMenuItem Text="View / Upload Documents" ImageUrl="../Img/folder_explore.png" Value="Upload" Font-Size="Small" Font-Bold="true" />
                        </Items>
                    </telerik:RadContextMenu>
                </fieldset>
            </div>
        </telerik:RadAjaxPanel>

        <asp:HiddenField ID="RowIndex" runat="server" />
        <asp:HiddenField ID="RowOid" runat="server" />
        <asp:HiddenField ID="CurrentUserID" runat="server" />
    </form>
</body>
</html>