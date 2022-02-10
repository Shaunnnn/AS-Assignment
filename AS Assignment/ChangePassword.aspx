<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="AS_Assignment.ChangePassword" ValidateRequest="false"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Change password</title>
    <script type="text/javascript">
        function validate() {
            var str = document.getElementById('<%=tb_npwd.ClientID %>').value;

            if (str.length < 8) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password Length Must be at least 8 Characters";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("too_short");
            }

            else if (str.search(/[0-9]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password require at least 1 number";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("no_number");
            }

            else if (str.search(/[A-Z]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password require caps";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("no_upercase");
            }

            else if (str.search(/[a-z]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password require small alphabets";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("no_lowercase");
            }

            else if (str.search(/[^A-Za-z0-9]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password require special character";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("no_specialchar");
            }

            document.getElementById("lbl_pwdchecker").innerHTML = "Excellent";
            document.getElementById("lbl_pwdchecker").style.color = "Blue";
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>
                <br />
                <asp:Label ID="Label1" runat="server" Text="Change Password"></asp:Label>
                <br />
                <br />
            </h2>
            <table class="style1">
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label3" runat="server" Text="User ID"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_userid" runat="server" Height="32px" Width="281px"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="regEmail" ControlToValidate="tb_userid" Text="Enter a valid email address" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" runat="server" Style="color: red" />
                        <br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="tb_userid" runat="server" ErrorMessage="Please enter your user id" Style="color: red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label6" runat="server" Text="New Password"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_npwd" runat="server" Height="32px" Width="281px" onkeyup="javascript:validate()"></asp:TextBox>
                        <asp:Label ID="lbl_pwdchecker" runat="server" Text=""></asp:Label>
                        <br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="tb_npwd" runat="server" ErrorMessage="Please enter your new password" Style="color: red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label8" runat="server" Text="Confirm Password"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_confirm" runat="server" Height="32px" Width="281px"></asp:TextBox>
                        <asp:CompareValidator runat="server" ID="Comp1" ControlToValidate="tb_npwd" ControlToCompare="tb_confirm" Text="Password mismatch" Style="color: red"></asp:CompareValidator>
                        <br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ControlToValidate="tb_confirm" runat="server" ErrorMessage="Please confirm your password" Style="color: red"></asp:RequiredFieldValidator>

                    </td>
                </tr>
                <tr>
                    <td class="style4"></td>
                    <td class="style5">
                        <asp:Button ID="btn_Submit" runat="server" Height="48px"
                            OnClick="btn_Change_Click" Text="Change Password" Width="288px" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
