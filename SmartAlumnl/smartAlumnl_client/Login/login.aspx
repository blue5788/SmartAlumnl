<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="smartAlumnl_client.Login.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lbl_Account" runat="server" Text="帐号："></asp:Label>
        <asp:TextBox ID="tb_Account" runat="server" MaxLength="10"></asp:TextBox>
        <br />
        <asp:Label ID="lbl_Pwd" runat="server" Text="密码："></asp:Label>
        <asp:TextBox ID="tb_Password" runat="server" MaxLength="10" TextMode="Password"></asp:TextBox>
        <br />
        <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="Btn_Login" runat="server" Text="Login" OnClick="Btn_Login_Click" />
    </div>
    </form>
</body>
</html>
