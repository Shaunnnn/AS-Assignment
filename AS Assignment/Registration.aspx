<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="AS_Assignment.Registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="registration" runat="server">
        <div>
            <h2>
                <br />
                <asp:Label ID="Label1" runat="server" Text="Registration"></asp:Label>
                <br />
                <br />
            </h2>
            <table class="style1">
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label2" runat="server" Text="First Name"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_firstName" runat="server" Height="32px" Width="281px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label3" runat="server" Text="Last Name"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_lastName" runat="server" Height="32px" Width="281px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label4" runat="server" Text="Credit Card Info"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_creditCard" runat="server" Height="32px" Width="281px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style6">
                        <asp:Label ID="Label5" runat="server" Text="Email Address"></asp:Label>
                    </td>
                    <td class="style7">
                        <asp:TextBox ID="tb_email" runat="server" Height="32px" Width="281px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label6" runat="server" Text="Password"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_password" runat="server" Height="32px" Width="281px"></asp:TextBox>
                        <asp:Label ID="lbl_pwdchecker" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label7" runat="server" Text="Date of Birth"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_DoB" runat="server" Height="32px" Width="281px" TextMode="Date"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style4"></td>
                    <td class="style5">
                        <asp:Button ID="btn_Submit" runat="server" Height="48px"
                            OnClick="btn_Submit_Click" Text="Submit" Width="288px" />
                    </td>
                </tr>
            </table>
            &nbsp;<br />
            <asp:Label ID="lbl_feedback" runat="server"></asp:Label>
            <br />
        </div>
    </form>
</body>
</html>
