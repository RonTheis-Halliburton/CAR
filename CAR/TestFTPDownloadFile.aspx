<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestFTPDownloadFile.aspx.cs" Inherits="CAR.TestFTPDownloadFile" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>

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
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <div>
                <table>
                    <tr>
                        <th>FTP Server - Port 2122
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
                            <asp:Label ID="Label1" runat="server" Text="Not Logged In FTP"></asp:Label>
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
                            <asp:Label ID="Label3" runat="server"></asp:Label>
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
                            <asp:Label ID="Label6" runat="server"></asp:Label>
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
                                AutoPostBack="true" OnSelectedIndexChanged="RadComboBox1_SelectedIndexChanged">
                            </telerik:RadComboBox>
                            <asp:Label ID="Label4" runat="server"></asp:Label>
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
                            <asp:Label ID="Label2" runat="server"></asp:Label>
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
                </table>
                <asp:Label ID="Label7" runat="server"></asp:Label>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
