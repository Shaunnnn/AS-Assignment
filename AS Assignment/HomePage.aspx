<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="AS_Assignment.HomePage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Welcome</h1>
            <div>
                User Profile<br />
                <br />
                <asp:Label ID="Label1" runat="server" Text="User ID: "></asp:Label>
                <asp:Label ID="lbl_userID" runat="server" Text=""></asp:Label>
                <br />
                <br />
                <asp:Label ID="Label3" runat="server" Text="Full Name: "></asp:Label>
                <asp:Label ID="lbl_fullName" runat="server" Text=""></asp:Label>
                <br />
                <br />
                <asp:Label ID="Label5" runat="server" Text="Date of Birth: "></asp:Label>
                <asp:Label ID="lbl_DoB" runat="server" Text=""></asp:Label>
                <br />
                <br />
                <asp:Label ID="Label2" runat="server" Text="Credit Card Info: "></asp:Label>
                <asp:Label ID="lbl_creditCard" runat="server" Text=""></asp:Label>
            </div>
        </div>
        <table class="style1">
            <tr>
                <td class="style4"></td>
                <td class="style5">
                    <asp:Button ID="btn_change" runat="server" Height="48px"
                        OnClick="btn_Change_Click" Text="Change Password" Width="288px" />
                </td>
            </tr>
            <tr>
                <td class="style4"></td>
                <td class="style5">
                    <asp:Button ID="btn_Logout" runat="server" Height="48px"
                        OnClick="btn_LogOut_Click" Text="Logout" Width="288px" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
