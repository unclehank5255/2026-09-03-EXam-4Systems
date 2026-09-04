<%@ Page Title="七大系統" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SevenSystems._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <h2>使用者</h2>
  
        <asp:Label ID="lbWelcome" runat="server" Text=""></asp:Label>
  
    <hr />
  <asp:HyperLink
    ID="hlAlbumAdmin"
    runat="server"
    Text="相簿管理後台"
    NavigateUrl="~/System04_Albums/Admin/CategoryManage.aspx" />
       <hr />
    <asp:HyperLink ID="hpAlbumManage" runat="server" Text="相簿列表" NavigateUrl="~/System04_Albums/Admin/AlbumManage.aspx" />
    <hr />  
    <asp:Button ID="btLogout" runat="server" Text="登出" OnClick="btLogout_Click" />
    </asp:Content>
