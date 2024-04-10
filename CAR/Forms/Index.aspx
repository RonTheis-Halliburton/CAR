<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="CAR.Forms.Index" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<script type="text/javascript" src="JS/TopLocation.js"></script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Index to QDMS</title>
    <link href="../Styles/styles.css" type="text/css" rel="stylesheet" />
    <style>
        .contentContainer th {
            font: bold 11px Arial, Verdana, Sans-serif;
            text-align: left;
        }
    </style>
</head>
<body style="margin: 0px; height: 100%; top: 0px;">
    <form id="form1" runat="server">
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


                function OnClientClicking_RadButton1(sender, eventArgs) {
                    var radGrid1 = $find("<%=RadGrid1.ClientID %>");

                    if (radGrid1 != null) {
                        //if (radGrid1.get_masterTableView().get_selectedItems().length == 0) {
                        //    alert("Number of selected documents : " + radGrid1.get_masterTableView().get_selectedItems().length + "\n\nPlease verify and try again.");
                        //    sender.set_autoPostBack(false);
                        //}
                    }

                    if (window.confirm("Are you sure?\n\n\Click OK to " + sender.get_text() + ".")) {
                        sender.set_autoPostBack(true);
                    }
                    else {
                        sender.set_autoPostBack(false);
                    }

                    return false;
                }

                function OnClientClicking_RadButton2(sender, eventArgs) {
                    if (window.confirm("Are you sure?\n\n\Click OK to " + sender.get_text() + ".")) {
                        sender.set_autoPostBack(true);
                    }
                    else {
                        sender.set_autoPostBack(false);
                    }
                    return false;
                }

                function OnClientClicking_RadButton3(sender, eventArgs) {
                    var oWnd = GetRadWindow();
                    if (oWnd != null) {
                        oWnd.close("Index");
                    }
                }

                function OnResponseEnd(ajaxPanel, eventArgs) {
                    eventArgs.set_enableAjax(true);
                }

                function OnRequestStart(ajaxPanel, eventArgs) {
                    var eventTarget = eventArgs.get_eventTarget();

                    if (eventTarget.indexOf("Download") >= 0) {
                        eventArgs.set_enableAjax(false); // cancel the ajax request
                    }
                }

            </script>
        </telerik:RadCodeBlock>
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

        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" ClientEvents-OnRequestStart="OnRequestStart" ClientEvents-OnResponseEnd="OnResponseEnd">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadButton1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadButton2" />
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                        <telerik:AjaxUpdatedControl ControlID="RadGrid2" LoadingPanelID="LoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="RadButton2">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadButton2" />
                        <telerik:AjaxUpdatedControl ControlID="RadButton1" />
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                        <telerik:AjaxUpdatedControl ControlID="RadGrid2" LoadingPanelID="LoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="RadGrid1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="RadGrid2">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid2" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <div class="contentContainer">
            <table>
                <tr>
                    <td style="vertical-align: top;">
                        <telerik:RadTreeView ID="RadTreeView1" runat="server" Width="100%">
                            <Nodes>
                                <telerik:RadTreeNode Text="General Information">
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
                                                        <th>Category:  </th>
                                                        <td>
                                                            <asp:Literal ID="Literal9" runat="server"></asp:Literal>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <th>Responsible Person:  </th>
                                                        <td>
                                                            <asp:Literal ID="Literal19" runat="server"></asp:Literal>
                                                            (<asp:Literal ID="Literal24" runat="server"></asp:Literal>)
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <th>Description of<br />
                                                            Finding:  </th>
                                                        <td>
                                                            <telerik:RadTextBox ID="RadTextBox19" runat="server" TextMode="MultiLine" Rows="5" Width="300px"></telerik:RadTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <th>Description of<br />
                                                            Improvement:  </th>
                                                        <td>
                                                            <telerik:RadTextBox ID="RadTextBox20" runat="server" TextMode="MultiLine" Rows="5" Width="300px"></telerik:RadTextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </NodeTemplate>
                                        </telerik:RadTreeNode>
                                    </Nodes>
                                </telerik:RadTreeNode>
                            </Nodes>
                            <Nodes>
                                <telerik:RadTreeNode Text="Vendor">
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
                                                        <th>Why did this nonconformance occur?</th>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <telerik:RadTextBox ID="RadTextBox20" runat="server" TextMode="MultiLine" Rows="5" Width="400px"></telerik:RadTextBox>
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
                                                        <th>What is the root cause / potential root cause of the non-conformance?</th>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <telerik:RadTextBox ID="RadTextBox20" runat="server" TextMode="MultiLine" Rows="5" Width="400px"></telerik:RadTextBox>
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
                                                        <th>Are there similar instances of this nonconformance in your area of responsibility? 
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Literal ID="Literal24" runat="server"></asp:Literal></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <telerik:RadTextBox ID="RadTextBox20" runat="server" TextMode="MultiLine" Rows="5" Width="400px"></telerik:RadTextBox>
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
                                                        <th>What action was taken (or is planned) to correct this nonconformance?
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <telerik:RadTextBox ID="RadTextBox20" runat="server" TextMode="MultiLine" Rows="5" Width="400px"></telerik:RadTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <th>What is the scheduled implementation date?
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
                                                        <th>What action was taken (or is planned) to preclude this and similar non-conformances?
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <telerik:RadTextBox ID="RadTextBox20" runat="server" TextMode="MultiLine" Rows="5" Width="400px"></telerik:RadTextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <th>What is the scheduled implementation date?
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
                                                        <th>Date Received:  </th>
                                                        <td>
                                                            <asp:Literal ID="Literal32" runat="server"></asp:Literal>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <th>Date Verified:  </th>
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
                                                        <th>Status:  </th>
                                                        <td>
                                                            <asp:Literal ID="Literal41" runat="server"></asp:Literal>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <th>Close Date:  </th>
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
                                                            <telerik:RadTextBox ID="RadTextBox21" runat="server" TextMode="MultiLine" Rows="5" Width="400px"></telerik:RadTextBox>
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
                    <td style="vertical-align: top; width: 80%;">
                        <table>
                            <tr>
                                <td style="vertical-align: top;">
                                    <fieldset class="fieldset">
                                        <legend>Documents</legend>
                                        <table>
                                            <tr>
                                                <td style="vertical-align: top;">
                                                    <telerik:RadGrid ID="RadGrid1" runat="server" AutoGenerateColumns="False" ShowStatusBar="true" AllowPaging="False"
                                                        GroupingEnabled="false" GroupPanel-Enabled="false" ShowGroupPanel="false" AllowSorting="true" AllowMultiRowSelection="True"
                                                        OnNeedDataSource="RadGrid1_NeedDataSource"
                                                        OnItemCommand="RadGrid1_ItemCommand">
                                                        <HeaderStyle HorizontalAlign="Center" Wrap="true" Font-Size="X-Small" Font-Bold="True" />
                                                        <StatusBarSettings LoadingText="Please wait...Loading data" />
                                                        <ClientSettings EnableRowHoverStyle="true">
                                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="250px" />
                                                            <Resizing AllowColumnResize="true" />
                                                        </ClientSettings>
                                                        <MasterTableView DataKeyNames="OID, CAR_OID,CAR_NBR,YEAR_ISSUED,FILENAMEALIAS" Width="100%" NoMasterRecordsText="No documents to display." Name="TopLevel" EnableNoRecordsTemplate="true" ShowHeadersWhenNoRecords="false">
                                                            <Columns>
                                                                <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn">
                                                                    <ItemStyle HorizontalAlign="Center" Width="25px" />
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="True" OnCheckedChanged="ToggleFileRowSelection_RadGrid1" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="35px" HorizontalAlign="Center" />
                                                                    <ItemStyle HorizontalAlign="Center" Width="35px" />
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridButtonColumn CommandName="Download" UniqueName="Download" ButtonType="ImageButton"
                                                                    ImageUrl="~/Img/Download.gif">
                                                                    <HeaderStyle Width="35px" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </telerik:GridButtonColumn>
                                                                <telerik:GridBoundColumn DataField="FILENAME" HeaderText="File Name" SortExpression="FILENAME" UniqueName="FILENAME">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="FILESIZE_BYTE" HeaderText="File Size" SortExpression="FILESIZE_BYTE" UniqueName="FILESIZE_BYTE">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="UPLOADEDBY" HeaderText="Uploaded By" SortExpression="UPLOADEDBY" UniqueName="UPLOADEDBY">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="DATEUPLOADED" HeaderText="Date" SortExpression="DATEUPLOADED" UniqueName="DATEUPLOADED">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="FILENAMEALIAS" HeaderText="FILENAMEALIAS" SortExpression="FILENAMEALIAS" UniqueName="FILENAMEALIAS" Display="false">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="FTPREMOTEPATH" HeaderText="FTPREMOTEPATH" SortExpression="FTPREMOTEPATH" UniqueName="FTPREMOTEPATH" Display="false">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="OID" HeaderText="OID" SortExpression="OID"
                                                                    UniqueName="OID" Display="false">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="FILESIZE" HeaderText="File Size" SortExpression="FILESIZE" UniqueName="FILESIZE" Display="false">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="CAR_OID" HeaderText="CAR_OID" SortExpression="CAR_OID"
                                                                    UniqueName="CAR_OID" Display="false">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="CAR_NBR" HeaderText="CAR_NBR" SortExpression="CAR_NBR"
                                                                    UniqueName="CAR_NBR" Display="false">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="YEAR_ISSUED" HeaderText="YEAR_ISSUED" SortExpression="YEAR_ISSUED"
                                                                    UniqueName="YEAR_ISSUED" Display="false">
                                                                </telerik:GridBoundColumn>
                                                            </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                                                </td>
                                                <td style="vertical-align: top;">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadButton ID="RadButton1" runat="server" Text="Create CAR Package" BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" Font-Size="X-Small"
                                                                    AutoPostBack="true" OnClick="RadButton1_Click" OnClientClicked="OnClientClicking_RadButton1">
                                                                    <Icon PrimaryIconCssClass="rbSave" />
                                                                </telerik:RadButton>
                                                            </td>
                                                            <td>
                                                                <telerik:RadButton ID="RadButton3" runat="server" Text="Close This Form" UseSubmitBehavior="true" CausesValidation="false"
                                                                    OnClientClicking="OnClientClicking_RadButton3" BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" Font-Size="X-Small">
                                                                    <Icon PrimaryIconCssClass="rbCancel"></Icon>
                                                                </telerik:RadButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align: top;">
                                    <fieldset class="fieldset">
                                        <legend>CAR Package</legend>
                                        <table>
                                            <tr>
                                                <td style="vertical-align: top;">
                                                    <telerik:RadGrid ID="RadGrid2" runat="server" AutoGenerateColumns="False" ShowStatusBar="true" AllowPaging="False"
                                                        GroupingEnabled="false" GroupPanel-Enabled="false" ShowGroupPanel="false" AllowSorting="true" AllowMultiRowSelection="True"
                                                        OnNeedDataSource="RadGrid2_NeedDataSource" OnItemCommand="RadGrid2_ItemCommand">
                                                        <HeaderStyle HorizontalAlign="Center" Wrap="true" Font-Size="X-Small" Font-Bold="True" />
                                                        <StatusBarSettings LoadingText="Please wait...Loading data" />
                                                        <ClientSettings EnableRowHoverStyle="true">
                                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="250px" />
                                                            <Resizing AllowColumnResize="true" />
                                                        </ClientSettings>
                                                        <MasterTableView DataKeyNames="OID, CAR_OID,CAR_NBR,YEAR_ISSUED,FILENAMEALIAS,FILENAME" Width="100%" NoMasterRecordsText="No packages to display." Name="TopLevel" EnableNoRecordsTemplate="true" ShowHeadersWhenNoRecords="false">
                                                            <Columns>
                                                                <telerik:GridButtonColumn CommandName="Download" UniqueName="Download" ButtonType="ImageButton"
                                                                    ImageUrl="~/Img/Download.gif">
                                                                    <HeaderStyle Width="35px" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </telerik:GridButtonColumn>
                                                                <telerik:GridBoundColumn DataField="FILENAMEALIAS" HeaderText="File Name" SortExpression="FILENAMEALIAS" UniqueName="FILENAMEALIAS">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="FILENAME" HeaderText="FILENAME" SortExpression="FILENAME" UniqueName="FILENAME" Display="false">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="FILESIZE_BYTE" HeaderText="File Size" SortExpression="FILESIZE_BYTE" UniqueName="FILESIZE_BYTE">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="UPLOADEDBY" HeaderText="Uploaded By" SortExpression="UPLOADEDBY" UniqueName="UPLOADEDBY">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="DATEUPLOADED" HeaderText="Date" SortExpression="DATEUPLOADED" UniqueName="DATEUPLOADED">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="QdmsDate" HeaderText="Load to QDMS Date" SortExpression="QdmsDate" UniqueName="QdmsDate">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="QdmsUploadBy" HeaderText="Load to QDMS By" SortExpression="QdmsUploadBy" UniqueName="QdmsUploadBy">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="FTPREMOTEPATH" HeaderText="FTPREMOTEPATH" SortExpression="FTPREMOTEPATH" UniqueName="FTPREMOTEPATH" Display="false">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="OID" HeaderText="OID" SortExpression="OID"
                                                                    UniqueName="OID" Display="false">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="FILESIZE" HeaderText="File Size" SortExpression="FILESIZE" UniqueName="FILESIZE" Display="false">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="CAR_OID" HeaderText="CAR_OID" SortExpression="CAR_OID"
                                                                    UniqueName="CAR_OID" Display="false">
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="CAR_NBR" HeaderText="CAR_NBR" SortExpression="CAR_NBR"
                                                                    UniqueName="CAR_NBR" Display="false">
                                                                </telerik:GridBoundColumn>
                                                            </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                                                </td>
                                                <td style="vertical-align: top;">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadButton ID="RadButton2" runat="server" Text="Accept and Load to QDMS" BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" Font-Size="X-Small"
                                                                    CausesValidation="false" OnClick="RadButton2_Click" OnClientClicked="OnClientClicking_RadButton2" AutoPostBack="true" ButtonType="StandardButton">
                                                                    <Icon PrimaryIconCssClass="rbUpload"></Icon>
                                                                </telerik:RadButton>
                                                            </td>
                                                            <td></td>
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
        </div>
        <asp:HiddenField ID="CAR_NBR" runat="server" />
        <asp:HiddenField ID="CAR_OID" runat="server" />
        <asp:HiddenField ID="YEAR_ISSUE" runat="server" />
    </form>
</body>
</html>
