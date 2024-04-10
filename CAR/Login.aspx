<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.cs" Inherits="CAR.Login" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<script type="text/javascript" src="JS/TopLocation.js"></script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <link href="Styles/Styles.css" type="text/css" rel="stylesheet" />
</head>
<body>
   <form id="form1" runat="server" defaultbutton="RadButton1">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <div class="contentContainer" style="width: 100%; height: 100%;">
            <table style="width: 100%;">
                <tr>
                    <td style="border-right-style: solid; border-bottom-style: solid; border-right-width: thin; border-bottom-width: thin; width: 10%;">
                        <asp:Image ID="Image3" ImageUrl="~/Img/HAL_Horz.jpg" runat="server" Height="35px" />
                    </td>
                    <td valign="middle" style="border-bottom-style: solid; border-bottom-width: thin; width: 15%;">
                        <b><i style="font-style: italic; font-family: Arial, Sans-Serif; font-size: medium; align-content: center;">Corrective Action</i></b>
                    </td>
                    <td valign="middle" style="border-bottom-style: solid; border-bottom-width: thin;"></td>
                </tr>
            </table>
            <table>
                <tr>
                    <td colspan="2" style="vertical-align: top;">
                        <table>
                            <tr>
                                <td style="background-image: url('Img/panelContainer.gif'); width: 20%; vertical-align: baseline;" rowspan="2"></td>
                                <td style="vertical-align: top;">

                                    <table style="width: 40%; vertical-align: top;">
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="Label4" runat="server" Font-Bold="True" Text="Hint:  Use your current network user ID and password"></asp:Label>
                                                <br />
                                                <asp:Label ID="Label5" runat="server" ForeColor="Red"></asp:Label>
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 70px;">User ID:
                                            </td>
                                            <td style="width: 300px">
                                                <asp:TextBox ID="txtUserID" runat="server" ></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 70px;">Password:
                                            </td>
                                            <td style="width: 300px;">
                                                <asp:TextBox ID="txtPCode" runat="server" TextMode="Password" ></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 70px"></td>
                                            <td>

                                                <telerik:RadButton ID="RadButton1" runat="server" Font-Bold="true" Text="Log In" UseSubmitBehavior="true" CausesValidation="false"
                                                    OnClick="RadButton1_Click" SingleClick="true" SingleClickText="Processing...">
                                                </telerik:RadButton>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="Label9" runat="server"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="lblStatus" runat="server" Font-Bold="True" Height="50px" Width="400px"
                                                    Text="Not logged yet."></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: auto; height: auto;" colspan="2">
                                                <asp:Label ID="Label7" runat="server" Font-Names="Arial" Font-Size="Small" ForeColor="Red"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <hr />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <a href="mailto:fhounbcit@halliburton.com">Site Administrator</a>
                                            </td>
                                            <td style="text-align: right;">
                                                <asp:Label ID="Label1" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>

                                </td>
                                <td style="vertical-align: bottom;"></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="height: .1px; background-color: Gray; vertical-align: top;" colspan="2"></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
