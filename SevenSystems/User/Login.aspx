<%@ Page Title="登入"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Login.aspx.cs"
    Inherits="SevenSystems.User.Login" %>

<asp:Content ID="BodyContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <h2>登入</h2>

    <asp:Label ID="lbUsername"
        runat="server"
        Text="帳號" />

    <asp:TextBox ID="txtUsername"
        runat="server" />

    <br />

    <asp:Label ID="lbPassword"
        runat="server"
        Text="密碼" />

    <asp:TextBox ID="txtPassword"
        runat="server"
        TextMode="Password" />

    <br />

    <asp:Button ID="btLogin"
        runat="server"
        Text="登入"
        OnClick="btLogin_Click" />

    <asp:Label ID="lbMessage"
        runat="server" />

    <br />

    <asp:Button ID="btRegister"
        runat="server"
        Text="還沒有帳號？先註冊"
        OnClick="btRegister_Click" />

</asp:Content>