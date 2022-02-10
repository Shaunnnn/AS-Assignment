<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Verify.aspx.cs" Inherits="AS_Assignment.Verify" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>
                <br />
                <asp:Label ID="Label1" runat="server" Text="Verification"></asp:Label>
                <br />
                <br />
            </h2>
            <asp:Label ID="Label3" runat="server" Text=""></asp:Label>
            <br />
            <br />
            <table class="style1">
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label2" runat="server" Text="Verification Code"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_vcode" runat="server" Height="32px" Width="281px"></asp:TextBox>
                        <br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="tb_vcode" runat="server" ErrorMessage="Please enter your verification code" Style="color: red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="style4"></td>
                    <td class="style5">
                        <asp:Button ID="btn_Submit" runat="server" Height="48px"
                            OnClick="btn_Verify_Click" Text="Verify" Width="288px" />
                    </td>
                </tr>
            </table>
            <asp:Label ID="lbl_error" runat="server" ForeColor="Red"></asp:Label>
        </div>
    </form>
</body>
</html>
