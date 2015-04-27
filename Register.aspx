<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="Register" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title></title>
    <script src="jquery-ui-1.10.4.custom/js/jquery-1.10.2.js"></script>
    <script type="text/javascript">
        $(function () {

            $("#btnRegister").click(function () {

                if ($("#txtUserName").val().trim() != "" &&
                $("#txtPassword").val() == $("#txtPassword2").val()
                && $("#txtPassword").val().trim() != "") {
                    return true;
                }
                return false;
            });


        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <h3>Register</h3>
    <table>
        <tr>
            <td>User Name: </td>
            <td><asp:TextBox ID="txtUserName" runat="server" style="width:150px;"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Password: </td>
            <td><asp:TextBox ID="txtPassword" runat="server" TextMode="Password" style="width:150px;"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Confirm Password: </td>
            <td><asp:TextBox ID="txtPassword2" runat="server" TextMode="Password" style="width:150px;"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Age: </td>
            <td><asp:TextBox ID="txtAge" runat="server" style="width:60px;"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Sex: </td>
            <td>
                <asp:DropDownList ID="ddlSex" runat="server" Width="150">
                    <asp:ListItem Text="Male" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Female" Value="1"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>Post: </td>
            <td>
                <asp:DropDownList ID="ddlPost" runat="server" Width="150">
                    <asp:ListItem Text="Post1" Value="Post1"></asp:ListItem>
                    <asp:ListItem Text="Post2" Value="Post2"></asp:ListItem>
                    <asp:ListItem Text="Post3" Value="Post3"></asp:ListItem>
                    <asp:ListItem Text="Post4" Value="Post4"></asp:ListItem>
                    <asp:ListItem Text="Post5" Value="Post5"></asp:ListItem>
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Department: </td>
            <td>
                <asp:DropDownList ID="ddlDepartment" runat="server" Width="150" >
                    <asp:ListItem Text="Dep1" Value="Dep1"></asp:ListItem>
                    <asp:ListItem Text="Dep2" Value="Dep2"></asp:ListItem>
                    <asp:ListItem Text="Dep3" Value="Dep3"></asp:ListItem>
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td colspan=2>
                <asp:Button ID="btnRegister" runat="server" Text="Register" 
                    onclick="btnRegister_Click" /></td> 
        </tr>
    </table>

    </div>
    </form>
</body>
</html>
