<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegistrationQdms.cs" Inherits="CAR.RegistrationQdms" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<script type="text/javascript" src="JS/TopLocation.js"></script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>QDMS Registration</title>
    <link href="Styles/styles.css" type="text/css" rel="stylesheet" />
    <style type="text/css">
        html {
            /*added to prevent scroll bars in radwindow*/
            overflow: hidden;
        }
    </style>
</head>
<body>
 <telerik:RadCodeBlock runat="server" ID="RadCodeBlockStart">
        <script type="text/javascript">
            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow) oWindow = window.radWindow; //Will work in Moz in all cases, including clasic dialog
                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow; //IE (and Moz az well)

                return oWindow;
            }

            function ParentPage() {
                var splitterPageWnd = window.parent;
                splitterPageWnd.ParentPage();  //Call function on ControlPage.aspx
            }

            function OnClientClicking_RadButton1(sender, eventArgs) {

                if (!CheckRequirement()) {
                    eventArgs.set_cancel(true);
                }
                else {

                    var equipment = document.getElementById("Label1").innerHTML;
                    var conf = window.confirm('Confirm registration information.\n\nClick OK to continue.');

                    if (conf == true) {
                        eventArgs.set_cancel(false);
                    }
                    else {
                        eventArgs.set_cancel(true);
                    }
                }
            }

            function CheckRequirement() {
                var uReturn = true;

                var txtUserID = $find("<%= RadTextBox1.ClientID %>");
                var txtFirstName = $find("<%= RadTextBox2.ClientID %>");                   
                var txtLastName = $find("<%= RadTextBox3.ClientID %>");

                var combo2 = $find("<%= RadComboBox2.ClientID %>"); //Plant                
                var input = combo2.get_inputDomElement();

                if (uReturn) {
                    if (txtUserID.isEmpty()) {
                        uReturn = false;
                        txtUserID.focus();
                        alert("Required field User ID.  Please verify and continue.")
                    }
                }

                if (uReturn) {
                    if (txtFirstName.isEmpty()) {
                        uReturn = false;
                        txtFirstName.focus();
                        alert("Required field First Name. Please verify and continue.")
                    }
                }

                if (uReturn) {
                    if (txtLastName.isEmpty()) {
                        uReturn = false;
                        txtLastName.focus();
                        alert("Required field Last Name.  Please verify and continue.")
                    }
                }

                if (uReturn)
                {
                    uReturn = true;

                    if (combo2 == null) {
                        uReturn = false;
                        alert("Plant field cannot be empty.\n\nVerify and try continue.");
                        input.focus();
                    }
                    else{
                        
                        if (combo2.get_text().length == 0 || combo2.get_text() == "Select plant(s)") {
                            alert("Plant field cannot be empty.\n\nVerify and try again.");
                            uReturn = false;
                            input.focus();
                        }                        
                    }
                }

                return uReturn;
            }

            function CloseRegistration(args) {
                alert("Your information has been submitted successfully.\n\nYou will receive an e-mail when your access is ready.");
                GetRadWindow().Close();
            }
        </script>
    </telerik:RadCodeBlock>
  <form id="form1" runat="server" defaultbutton="RadButton1">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <div class="contentContainer" style="width: 100%; height: 100%;">
            <asp:Label ID="Label3" runat="server" Text="Label"></asp:Label>
            <table style="width: 100%;">
                <tr>
                    <td colspan="2" style="vertical-align: top;">
                        <fieldset class="fieldset">
                            <legend>User Information</legend>
                            <table>
                                <tr>
                                    <td style="vertical-align: top;">
                                        <table style="width: 40%; vertical-align: top;">
                                            <tr>
                                                <td>Halliburton Network Login User ID:</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <telerik:RadTextBox ID="RadTextBox1" runat="server" ></telerik:RadTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>First Name:
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <telerik:RadTextBox ID="RadTextBox2" runat="server" ></telerik:RadTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Last Name:
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <telerik:RadTextBox ID="RadTextBox3" runat="server" ></telerik:RadTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Plant</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <telerik:RadComboBox ID="RadComboBox2" runat="server" MarkFirstMatch="true" AllowCustomText="true"
                                                        Height="150px" Width="250px" DropDownWidth="300px" EmptyMessage ="Select plant(s)" CheckBoxes="true">
                                                    </telerik:RadComboBox>
                                                </td>
                                            </tr>                                          
                                            <tr>
                                                <td>What is your purpose for requesting access?</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtPurpose" runat="server"  Width="350px" TextMode="MultiLine" Rows="4" Columns="100"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>

                                                    <telerik:RadButton ID="RadButton1" runat="server" Text="Submit" UseSubmitBehavior="true" CausesValidation="false" RenderMode="Lightweight"
                                                        OnClientClicking="OnClientClicking_RadButton1" OnClick="RadButton1_Click" SingleClick="true" SingleClickText="Processing..." 
                                                        BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" Font-Size="X-Small">
                                                    </telerik:RadButton>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <hr />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <a href="mailto:fhounbcit@halliburton.com">Site Administrator</a>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="vertical-align: top;">
                                        <asp:Label ID="Label1" runat="server"></asp:Label><br />
                                        <asp:Label ID="Label2" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>
            </table>
        </div>

        <asp:HiddenField ID="HiddenField1" runat="server" />
        <asp:HiddenField ID="HiddenField2" runat="server" />
        <asp:HiddenField ID="HiddenField3" runat="server" />
    </form>
</body>
</html>
