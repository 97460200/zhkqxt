<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wx_menu.aspx.cs" Inherits="RM.Web.WX_SET.wx_menu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btnCreateMenu" runat="server" Text="创建菜单" OnClick="btnCreateMenu_Click" />
        <asp:Button ID="btnDeleteMenu" runat="server" Text="删除菜单" OnClick="btnDeleteMenu_Click" />
        <asp:Label ID="lblResult" runat="server" Text="结果"></asp:Label>
    </div>
    </form>
</body>
</html>
