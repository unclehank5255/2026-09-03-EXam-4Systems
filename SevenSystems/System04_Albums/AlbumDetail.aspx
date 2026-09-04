<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AlbumDetail.aspx.cs" Inherits="SevenSystems.System04_Albums.AlbumDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="lbAlbumTitle" runat="server" Text=""></asp:Label>
    <asp:Repeater ID="rptPhotos" runat="server">
        <ItemTemplate>
            <div>
                <asp:Image ID="imgPhoto" runat="server" ImageUrl='<%# GetPhotoUrl(Eval("FileName")) %>' AlternateText='<%# Eval("FileName")%>'/>
                <asp:Label ID="lbPhotoCaption" runat="server" Text='<%#: Eval("Caption") %>'></asp:Label>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</asp:Content>
