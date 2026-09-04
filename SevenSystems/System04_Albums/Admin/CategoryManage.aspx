<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CategoryManage.aspx.cs" Inherits="SevenSystems.System04_Albums.Admin.CategoryManage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <div>
        <asp:Label ID="lblCategoryName" runat="server" Text="類別名稱"></asp:Label>
        <asp:TextBox ID="txtCategoryName" runat="server"></asp:TextBox>
    </div>
    <div>
        <asp:Label ID="lblSort" runat="server" Text="類別排序"></asp:Label>
        <asp:TextBox ID="txtSortOrder" runat="server"></asp:TextBox>

        <asp:Button ID="btnAdd" runat="server" Text="新增類別" OnClick="btnAdd_Click" />
        <asp:Label ID="lbMessage" runat="server"></asp:Label>
    </div>
        <hr />

    <div>
        <asp:GridView ID="gvCategories" runat="server">

        </asp:GridView>
    </div>



</asp:Content>
