<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Document.aspx.cs" Inherits="CAR.Forms.Document" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<script type="text/javascript" src="JS/TopLocation.js"></script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Document</title>
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
            var UploadedFileSize = 0;
            var MaxFileSize = 52428800; //50MB

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

            function OnClientValidationFailed(sender, eventArgs) {
                var fileExtention = eventArgs.get_fileName().substring(eventArgs.get_fileName().lastIndexOf('.') + 1, eventArgs.get_fileName().length);

                if (eventArgs.get_fileName().lastIndexOf('.') != -1) {
                    //this checks if the extension is correct
                    if (sender.get_allowedFileExtensions().indexOf(fileExtention) == -1) {
                        alert("This file type is not supported.  Pdf file format only.\nPlease verify and try again.");
                    }
                    else {
                        alert("File size exceeds maximum size of 50 MB");
                    }
                }
                else {
                    alert("Not correct extension!  Pdf file format only.\n\nPlease verify and try again.");
                }
                sender.set_enabled(true);
                sender.deleteFileInputAt(0);
            }

            function OnClientFileUploaded(sender, eventArgs) {

                if (UploadedFileSize > MaxFileSize) {
                    alert("This operation has been cancelled due to the total size of the selected file is more than the limit 50 Mb.");
                    return false;
                }
                else {
                    UploadedFileSize = 0;
                    sender.set_enabled(true);
                    document.getElementById("<%= btnDummy1.ClientID %>").click();
                    sender.deleteFileInputAt(0);
                }
            }

            function OnClientFileUploadFailed(sender, eventArgs) {
                alert("File upload failed:  " + eventArgs.get_message());
                if (eventArgs.get_message() == "error") {
                    eventArgs.set_handled(true);  // returns whether the error message should be suppressed. The default is true.
                }
            }

            function OnProgressUpdating(sender, eventArgs) {
                if (eventArgs != null) {
                    UploadedFileSize = eventArgs.get_data().fileSize;
                }
            }

            function OnClientFileSelected(sender, eventArgs) {
                sender.set_enabled(false);
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

            function OnClientClicking_RadButton3(sender, eventArgs) {
                var oWnd = GetRadWindow();
                if (oWnd != null) {
                    oWnd.close("Document");
                }
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
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" OnAjaxRequest="RadAjaxPanel1_AjaxRequest" LoadingPanelID="LoadingPanel1"
            ClientEvents-OnRequestStart="OnRequestStart" ClientEvents-OnResponseEnd="OnResponseEnd">
            <div class="contentContainer">
                <fieldset class="fieldset">
                    <legend style="font-family: Arial; font-style: italic;">Corrective Action Documents</legend>
                    <table>
                        <tr>
                            <td colspan="2">File Upload (Pdf file format 50 Mb max. per file)</td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <telerik:RadAsyncUpload runat="server" ID="AsyncUpload1" Width="300px" ManualUpload="false"
                                                TemporaryFileExpiration="00:05:00"
                                                ToolTip="Pdf file type and max file size 50MB per file"
                                                MultipleFileSelection="Disabled"
                                                RenderMode="Lightweight"
                                                EnableInlineProgress="false"
                                                UploadedFilesRendering="BelowFileInput"
                                                AllowedFileExtensions=".pdf"
                                                MaxFileSize="52428800"
                                                ChunkSize="3145728"
                                                OnFileUploaded="AsyncUpload1_FileUploaded"
                                                OnClientFileSelected="OnClientFileSelected"
                                                OnClientFileUploadFailed="OnClientFileUploadFailed"
                                                OnClientValidationFailed="OnClientValidationFailed"
                                                OnClientFileUploaded="OnClientFileUploaded"
                                                OnClientProgressUpdating="OnProgressUpdating">
                                                <FileFilters>
                                                    <telerik:FileFilter Description="Docs(pdf)" Extensions="pdf" />
                                                </FileFilters>
                                            </telerik:RadAsyncUpload>
                                        </td>
                                        <td>
                                            <telerik:RadButton ID="RadButton3" runat="server" Text="Close" UseSubmitBehavior="true" CausesValidation="false"
                                                OnClientClicking="OnClientClicking_RadButton3" BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" Font-Size="X-Small">
                                                <Icon PrimaryIconCssClass="rbCancel"></Icon>
                                            </telerik:RadButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 50%; vertical-align: top;" rowspan="3">
                                <telerik:RadProgressManager runat="server" ID="RadProgressManager1" />
                                <telerik:RadProgressArea RenderMode="Lightweight" runat="server" ID="RadProgressArea1" Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadGrid ID="RadGrid1" runat="server" AutoGenerateColumns="False" ShowStatusBar="true" AllowPaging="False"
                                    GroupingEnabled="false" GroupPanel-Enabled="false" ShowGroupPanel="false" AllowSorting="true"
                                    OnNeedDataSource="RadGrid1_NeedDataSource" OnItemDataBound="RadGrid1_ItemDataBound"
                                    OnItemCommand="RadGrid1_ItemCommand">
                                    <HeaderStyle HorizontalAlign="Center" Wrap="true" Font-Size="X-Small" Font-Bold="True" />
                                    <StatusBarSettings LoadingText="Please wait...Loading data" />
                                    <ClientSettings EnableRowHoverStyle="true">
                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="500px" />
                                        <Resizing AllowColumnResize="true" />
                                    </ClientSettings>
                                    <MasterTableView DataKeyNames="OID, CAR_OID,CAR_NBR,YEAR_ISSUED,FILENAMEALIAS" Width="100%" NoMasterRecordsText="No documents to display." Name="TopLevel" EnableNoRecordsTemplate="true" ShowHeadersWhenNoRecords="false">
                                        <Columns>
                                            <telerik:GridTemplateColumn UniqueName="TemplateColumnDelete">
                                                <HeaderStyle Width="35px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImageButton1" runat="server" AlternateText="Delete Document" CommandName="Delete" ToolTip="Delete Document"
                                                        OnClientClick="javascript:if(!confirm('This action will delete the selected document.\n\n Are you sure?')){return false;}"
                                                        ImageUrl="~/Img/TrashBin.png" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridButtonColumn CommandName="Download" UniqueName="Download" ButtonType="ImageButton" Display="false"
                                                ImageUrl="~/Img/Download.gif">
                                                <HeaderStyle Width="35px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </telerik:GridButtonColumn>
                                            <telerik:GridButtonColumn CommandName="Download2" UniqueName="Download2" ButtonType="ImageButton"
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

                        </tr>
                    </table>
                </fieldset>
            </div>
            <asp:Button ID="btnDummy1" runat="server" OnClick="BtnDummy1_Click" Style="display: none;" />
            <asp:HiddenField ID="CAR_OID" runat="server" />
            <asp:HiddenField ID="CAR_NBR" runat="server" />
            <asp:HiddenField ID="YEAR_ISSUED" runat="server" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
