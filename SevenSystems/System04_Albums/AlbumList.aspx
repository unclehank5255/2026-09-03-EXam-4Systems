<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AlbumList.aspx.cs" Inherits="SevenSystems.System04_Albums.AlbumList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
    .album-grid {
        display: grid;
        grid-template-columns: repeat(3, minmax(0, 1fr));
        gap: 24px;
    }

    .album-card {
        min-width: 0;
    }

    .album-cover-image,
    .album-cover-placeholder {
        width: 100%;
        height: 200px;
        border-radius: 4px;
    }

    .album-cover-image {
        display: block;
        object-fit: cover;
    }

    .album-cover-placeholder {
        display: flex;
        align-items: center;
        justify-content: center;
        box-sizing: border-box;
        background-color: #eeeeee;
        color: #666666;
    }

    .album-title {
        display: block;
        margin-top: 10px;
        color: #333333;
        font-size: 18px;
        text-decoration: none;
    }

    @media (max-width: 768px) {
        .album-grid {
            grid-template-columns: 1fr;
        }
    }
</style>

<div class="album-grid">
    <asp:Repeater ID="rptAlbums" runat="server">
        <ItemTemplate>
            <div class="album-card">

                <asp:HyperLink
                    ID="hlCover"
                    runat="server"
                    NavigateUrl='<%# "AlbumDetail.aspx?albumId=" + Eval("Id") %>'>

                    <asp:Image
                        ID="imgCover"
                        runat="server"
                        CssClass="album-cover-image"
                        ImageUrl='<%# GetCoverUrl(Eval("Id"), Eval("CoverFileName")) %>'
                        Visible='<%# HasCover(Eval("CoverFileName")) %>' />

                    <asp:Panel
                        ID="pnlNoCover"
                        runat="server"
                        CssClass="album-cover-placeholder"
                        Visible='<%# !HasCover(Eval("CoverFileName")) %>'>
                        暫無封面
                    </asp:Panel>

                </asp:HyperLink>

                <asp:HyperLink
                    ID="hlAlbumTitle"
                    runat="server"
                    CssClass="album-title"
                    NavigateUrl='<%# "AlbumDetail.aspx?albumId=" + Eval("Id") %>'
                    Text='<%#: Eval("Title") %>' />

            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>
</asp:Content>
