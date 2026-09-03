<%@ Page Title="註冊"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Register.aspx.cs"
    Inherits="SevenSystems.User.Register" %>

<asp:Content ID="BodyContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <h2>註冊</h2>

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

    <asp:Button ID="btRegister"
        runat="server"
        Text="註冊"
        OnClick="btRegister_Click" />

    <asp:Label ID="lbMessage"
        runat="server" />

    <asp:Button ID="btToLogin"
        runat="server"
        Text="已經有帳號？返回登入"
        OnClick="btToLogin_Click" />

</asp:Content>