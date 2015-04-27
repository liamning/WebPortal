<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <h3>Login</h3>
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
            <td colspan=2>
            <asp:Button ID="btnLogin" runat="server" Text="Login" onclick="btnLogin_Click" />
            <asp:Button ID="btnRegister" runat="server" Text="Register" onclientClick="location='Register.aspx';return false;" />
            </td>
        </tr>
    
    </table>  
    </div>
    </form>
</body>
</html>
