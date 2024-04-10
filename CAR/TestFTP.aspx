<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestFTP.aspx.cs" Inherits="CAR.TestFTP" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Test FTP</title>
    <link href="../Styles/styles.css" type="text/css" rel="stylesheet" />
</head>
<body>
      <telerik:RadCodeBlock runat="server" ID="RadCodeBlock1">
        <script type="text/javascript">
            var UploadedFileSize = 0;
            var MaxFileSize = 52428800; //50MB
    
            function OnClientValidationFailed1(sender, eventArgs) {
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

                sender.deleteFileInputAt(0);
            }

            function OnClientFileUploaded1(sender, eventArgs) {

                if (UploadedFileSize > MaxFileSize) {
                    alert("This operation has been cancelled due to the total size of the selected file is more than the limit 50 Mb.");
                    return false;
                }
                else {

                    document.getElementById("<%= btnDummy1.ClientID %>").click();
                    sender.deleteFileInputAt(0);
                }
            }

            function OnClientFileUploadFailed1(sender, eventArgs) {
                alert("File upload failed:  " + eventArgs.get_message());
                if (eventArgs.get_message() == "error") {
                    eventArgs.set_handled(true);  // returns whether the error message should be suppressed. The default is true.
                }
            }

            function OnProgressUpdating1(sender, eventArgs) {
                UploadedFileSize = eventArgs.get_data().fileSize;
            }

             function OnRequestStart(ajaxPanel, eventArgs) {
                    var eventTarget = eventArgs.get_eventTarget();
                    if (eventTarget == "<%= RadButton7.UniqueID %>") {
                        eventArgs.set_enableAjax(false); // cancel the ajax request

                        var button = $find("<%= RadButton7.ClientID %>");
                        button.enableAfterSingleClick();
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
       <telerik:RadProgressManager ID="RadProgressManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" ClientEvents-OnRequestStart="OnRequestStart">            
            <div class="contentContainer">
                <table>
                     <tr>
                        <th>
                            Componet Pro SFTP - Port 22
                        </th>
                        <td>
                            <asp:Label ID="Label5" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadButton RenderMode="Lightweight" ID="RadButton1" runat="server" Text="Validate Login" BorderColor="Blue"
                                CausesValidation="true" SingleClick="true" SingleClickText="Processing..." OnClick="RadButton1_Click">
                                <Icon SecondaryIconCssClass="rbLogin"></Icon>
                            </telerik:RadButton>
                        </td>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Not Logged in SFTP"></asp:Label>
                        </td>
                    </tr>
                   
                    <tr>
                        <td>
                            <telerik:RadButton RenderMode="Lightweight" ID="RadButton2" runat="server" Text="Create a Folder" BorderColor="Blue"
                                CausesValidation="true" SingleClick="true" SingleClickText="Processing..." OnClick="RadButton2_Click">
                                <Icon SecondaryIconCssClass="rbLogin"></Icon>
                            </telerik:RadButton>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="RadTextBox1" runat="server" Width="200px" EmptyMessage="Folder name..." MaxLength="150"></telerik:RadTextBox>
                            <asp:Label ID="Label2" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadButton RenderMode="Lightweight" ID="RadButton3" runat="server" Text="Delete a Folder" BorderColor="Blue"
                                CausesValidation="true" SingleClick="true" SingleClickText="Processing..." OnClick="RadButton3_Click">
                                <Icon SecondaryIconCssClass="rbLogin"></Icon>
                            </telerik:RadButton>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="RadTextBox2" runat="server" Width="200px" EmptyMessage="Folder name..." MaxLength="150"></telerik:RadTextBox>
                            <asp:Label ID="Label3" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadButton RenderMode="Lightweight" ID="RadButton4" runat="server" Text="List All Folders" BorderColor="Blue"
                                CausesValidation="true" SingleClick="true" SingleClickText="Processing..." OnClick="RadButton4_Click">
                                <Icon SecondaryIconCssClass="rbLogin"></Icon>
                            </telerik:RadButton>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="RadComboBox1" runat="server" MarkFirstMatch="True" Width="200px" DropDownWidth="400px" Height="150px"
                               AutoPostBack="true" OnSelectedIndexChanged="RadComboBox1_SelectedIndexChanged"></telerik:RadComboBox>
                            <asp:Label ID="Label4" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>File Upload (Max. 50 Mb per file)</td>
                        <td>
                            <telerik:RadAsyncUpload runat="server" ID="AsyncUpload1" Width="200px" ManualUpload="false"
                                ToolTip="Pdf file type and max file size 50MB per file"
                                MultipleFileSelection="Disabled"
                                RenderMode="Auto"
                                EnableInlineProgress="false"
                                UploadedFilesRendering="BelowFileInput"
                                AllowedFileExtensions=".pdf"
                                MaxFileSize="52428800"
                                 ChunkSize="3145728"
                                OnFileUploaded="AsyncUpload1_FileUploaded"                               
                                OnClientFileUploadFailed="OnClientFileUploadFailed1"
                                OnClientValidationFailed="OnClientValidationFailed1"
                                OnClientFileUploaded="OnClientFileUploaded1"
                                OnClientProgressUpdating="OnProgressUpdating1"
                                >
                                <FileFilters>
                                    <telerik:FileFilter Description="Docs(pdf)" Extensions="pdf" />
                                </FileFilters>
                            </telerik:RadAsyncUpload>
                        </td>
                    </tr>
                    <tr>                        
                        <td><asp:Button ID="btnDummy1" runat="server" OnClick="BtnDummy1_Click" Style="display: none;" /></td>
                        <td>
                              <asp:Label ID="Label6" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                          <td>
                            <telerik:RadButton RenderMode="Lightweight" ID="RadButton5" runat="server" Text="List All Files" BorderColor="Blue"
                                CausesValidation="true" SingleClick="true" SingleClickText="Processing..." OnClick="RadButton5_Click">
                                <Icon SecondaryIconCssClass="rbLogin"></Icon>
                            </telerik:RadButton>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="RadComboBox2" runat="server" MarkFirstMatch="True" Width="200px" DropDownWidth="400px" Height="150px"></telerik:RadComboBox>
                            <asp:Label ID="Label7" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                             <telerik:RadButton RenderMode="Lightweight" ID="RadButton6" runat="server" Text="Delete Selected File" BorderColor="Blue"
                                CausesValidation="true" SingleClick="true" SingleClickText="Processing..." OnClick="RadButton6_Click">
                                <Icon SecondaryIconCssClass="rbLogin"></Icon>
                            </telerik:RadButton>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                             <telerik:RadButton RenderMode="Lightweight" ID="RadButton7" runat="server" Text="Download Selected File" BorderColor="Blue"
                                CausesValidation="true" SingleClick="true" SingleClickText="Processing..." OnClick="RadButton7_Click" >
                                <Icon SecondaryIconCssClass="rbLogin"></Icon>
                            </telerik:RadButton>
                        </td>
                    </tr>
                </table>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
