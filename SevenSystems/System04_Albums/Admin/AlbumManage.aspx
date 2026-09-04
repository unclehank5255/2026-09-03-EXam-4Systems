<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AlbumManage.aspx.cs" Inherits="SevenSystems.System04_Albums.Admin.AlbumManage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>相簿管理</h2>

<div>
    <asp:Label
        ID="lblCategory"
        runat="server"
        Text="相簿類別" />

    <asp:DropDownList
        ID="ddlCategory"
        runat="server" />
</div>

<div>
    <asp:Label
        ID="lblTitle"
        runat="server"
        Text="相簿標題" />

    <asp:TextBox
        ID="txtTitle"
        runat="server" />
</div>

<asp:Button
    ID="btnAddAlbum"
    runat="server"
    Text="新增相簿"
    OnClick="btnAddAlbum_Click" />

<asp:Label
    ID="lblMessage"
    runat="server" />

<hr />

<asp:GridView
    ID="gvAlbums"
    runat="server"
    AutoGenerateColumns="false">
    <Columns>
        <asp:BoundField
            DataField="Id"
            HeaderText="編號" />

        <asp:BoundField
            DataField="Title"
            HeaderText="相簿標題" />

        <asp:BoundField
            DataField="CategoryName"
            HeaderText="類別" />

        <asp:BoundField
            DataField="CreatedAt"
            HeaderText="建立時間" />

        <asp:HyperLinkField
            Text="管理照片"
            DataNavigateUrlFields="Id"
            DataNavigateUrlFormatString="PhotoManage.aspx?albumId={0}" />
    </Columns>
</asp:GridView>
</asp:Content>
