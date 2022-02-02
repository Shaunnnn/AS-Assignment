<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="AS_Assignment.Login" %>

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
                <asp:Label ID="Label1" runat="server" Text="Login"></asp:Label>
                <br />
                <br />
            </h2>
            <table class="style1">
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label2" runat="server" Text="User ID(Email)"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_userid" runat="server" Height="32px" Width="281px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label3" runat="server" Text="Password"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_pwd" runat="server" Height="32px" Width="281px"></asp:TextBox>
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
            <asp:Label ID="lbl_error" runat="server"></asp:Label>
            <br />
        </div>
    </form>
</body>
</html>
