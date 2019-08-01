<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="daoru.aspx.cs" Inherits="RM.Web.RMBase.SysUser.daoru" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table class="tb1" style="border-collapse: collapse; width: 100%;">
            <tr>
                <td class="td1">
                    导入订单文件：
                </td>
                <td class="td22" style="padding-left: 5px;">
                    <asp:TextBox ID="txtFile" runat="server" Visible="false" Style="background: none;
                        border: none; padding: 0px;"></asp:TextBox>
                    <asp:FileUpload ID="FileUpload1" runat="server" onchange="selectIcon(this)" Style="background: none;
                        padding: 0px; width: 220px" />
                    <asp:Button ID="Button1" runat="server"  OnClick="Button1_Click" Text="导入订单" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
