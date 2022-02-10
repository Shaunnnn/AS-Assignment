<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="AS_Assignment.Login" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <script src="https://www.google.com/recaptcha/api.js?render=6LeR0RseAAAAAFQkOCTKMJ9UCgayXFWoY6LfHC5_"></script>
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
            <asp:Label ID="Label4" runat="server" Text="" ForeColor="Green"></asp:Label>
            <br />
            <br />
            <table class="style1">
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label2" runat="server" Text="User ID(Email)"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_userid" runat="server" Height="32px" Width="281px"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="regEmail" ControlToValidate="tb_userid" Text="Enter a valid email address" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" runat="server" Style="color: red" />

                        <br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="tb_userid" runat="server" ErrorMessage="Please enter your user id" Style="color: red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label3" runat="server" Text="Password"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_pwd" runat="server" Height="32px" Width="281px" TextMode="Password"></asp:TextBox>
                        <br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="tb_pwd" runat="server" ErrorMessage="Please enter your password" Style="color: red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="style4"></td>
                    <td class="style5">
                        <asp:Button ID="btn_Submit" runat="server" Height="48px"
                            OnClick="btn_Submit_Click" Text="Login" Width="288px" />
                    </td>
                </tr>
            </table>
            &nbsp;<br />
            <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response" />
            <asp:Label ID="lbl_error" runat="server" ForeColor="Red"></asp:Label>
            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
            <br />
        </div>
    </form>
    <script>
        grecaptcha.ready(function () {
            grecaptcha.execute('6LeR0RseAAAAAFQkOCTKMJ9UCgayXFWoY6LfHC5_', { action: 'Login' }).then(function (token) {
                document.getElementById("g-recaptcha-response").value = token;
            });
        });
    </script>
</body>
</html>
