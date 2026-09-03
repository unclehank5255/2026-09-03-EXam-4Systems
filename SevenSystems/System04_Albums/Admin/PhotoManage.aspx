<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PhotoManage.aspx.cs" Inherits="SevenSystems.System04_Albums.Admin.PhotoManage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>照片管理</h2>

<asp:HyperLink
    ID="hlBack"
    runat="server"
    NavigateUrl="~/System04_Albums/Admin/AlbumManage.aspx"
    Text="返回相簿管理" />

<h3>
    <asp:Label
        ID="lblAlbumTitle"
        runat="server" />
</h3>

<div>
    <asp:FileUpload
        ID="fuPhoto"
        runat="server" />
</div>

<div>
    <asp:Label
        ID="lblCaption"
        runat="server"
        Text="照片說明" />

    <asp:TextBox
        ID="txtCaption"
        runat="server" />
</div>

<asp:Button
    ID="btnUpload"
    runat="server"
    Text="上傳照片"
    OnClick="btnUpload_Click" />

<asp:Label
    ID="lblMessage"
    runat="server" />

<hr />

<asp:GridView
    ID="gvPhotos"
    runat="server"
    AutoGenerateColumns="false"
    DataKeyNames="Id"
    OnRowCommand="gvPhotos_RowCommand">
    <Columns>
        <asp:TemplateField HeaderText="照片">
            <ItemTemplate>
                <asp:Image
                    ID="imgPhoto"
                    runat="server"
                    ImageUrl='<%# GetPhotoUrl(Eval("FileName")) %>'
                    Width="160" />
            </ItemTemplate>
        </asp:TemplateField>

        <asp:BoundField
            DataField="Caption"
            HeaderText="照片說明" />

        <asp:CheckBoxField
            DataField="IsCover"
            HeaderText="封面" />

        <asp:BoundField
            DataField="CreatedAt"
            HeaderText="上傳時間" />
        <asp:ButtonField
    Text="設為封面"
    CommandName="SetCover"
    ButtonType="Button" />
    </Columns>
</asp:GridView>
</asp:Content>
