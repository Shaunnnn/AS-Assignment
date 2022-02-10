<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="AS_Assignment.Registration" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Registration</title>
    <script type="text/javascript">
        function validate() {
            var str = document.getElementById('<%=tb_password.ClientID %>').value;

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
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="tb_firstName" Text="Enter a valid first name" ValidationExpression="^([A-z][A-Za-z]*\s*[A-Za-z]*)$" runat="server" Style="color: red" />
                        <br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="tb_firstName" runat="server" ErrorMessage="Please enter your first name" Style="color: red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label3" runat="server" Text="Last Name"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_lastName" runat="server" Height="32px" Width="281px"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="tb_lastName" Text="Enter a valid last name" ValidationExpression="^([A-z][A-Za-z]*\s*[A-Za-z]*)$" runat="server" Style="color: red" />
                        <br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="tb_lastName" runat="server" ErrorMessage="Please enter your last name" Style="color: red"></asp:RequiredFieldValidator>

                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label4" runat="server" Text="Credit Card Info"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_creditCard" runat="server" Height="32px" Width="281px"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" ControlToValidate="tb_creditCard" Text="Enter a valid credit card number" ValidationExpression="^[0-9]+$" runat="server" Style="color: red" />
                        <br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="tb_creditCard" runat="server" ErrorMessage="Please enter your credit card info" Style="color: red"></asp:RequiredFieldValidator>

                    </td>
                </tr>
                <tr>
                    <td class="style6">
                        <asp:Label ID="Label5" runat="server" Text="Email Address"></asp:Label>
                    </td>
                    <td class="style7">
                        <asp:TextBox ID="tb_email" runat="server" Height="32px" Width="281px"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="regEmail" ControlToValidate="tb_email" Text="Enter a valid email address" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" runat="server" Style="color: red" />
                        <br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="tb_email" runat="server" ErrorMessage="Please enter your email" Style="color: red"></asp:RequiredFieldValidator>

                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label6" runat="server" Text="Password"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_password" runat="server" Height="32px" Width="281px" onkeyup="javascript:validate()"></asp:TextBox>
                        <asp:Label ID="lbl_pwdchecker" runat="server" Text=""></asp:Label>
                        <br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="tb_password" runat="server" ErrorMessage="Please enter your password" Style="color: red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label8" runat="server" Text="Confirm Password"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_confirm" runat="server" Height="32px" Width="281px"></asp:TextBox>
                        <asp:CompareValidator runat="server" ID="Comp1" ControlToValidate="tb_password" ControlToCompare="tb_confirm" Text="Password mismatch" Style="color: red"></asp:CompareValidator>
                        <br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ControlToValidate="tb_password" runat="server" ErrorMessage="Please confirm your password" Style="color: red"></asp:RequiredFieldValidator>

                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label7" runat="server" Text="Date of Birth"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_DoB" runat="server" Height="32px" Width="281px" TextMode="Date"></asp:TextBox>
                        <br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ControlToValidate="tb_DoB" runat="server" ErrorMessage="Please enter your Date of Birth" Style="color: red"></asp:RequiredFieldValidator>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td class="style6">
                        <asp:Label ID="Label9" runat="server" Text="Photo"></asp:Label>
                    </td>
                    <td class="style7">
                        <asp:FileUpload ID="FileUpload1" runat="server" Height="36px" Width="401px" />
                        <asp:Label ID="lbl_upload" runat="server" Text=""></asp:Label>
                        <br />
                        <asp:RequiredFieldValidator ID="rfvFileupload" runat="server" ControlToValidate="FileUpload1"  ErrorMessage="Please upload a photo" Style="color: red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="style4"></td>
                    <td class="style5">
                        <asp:Button ID="btn_Submit" runat="server" Height="48px"
                            OnClick="btn_Submit_Click" Text="Register" Width="288px" />
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
