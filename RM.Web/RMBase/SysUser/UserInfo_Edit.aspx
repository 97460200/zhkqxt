<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserInfo_Edit.aspx.cs"
    Inherits="RM.Web.RMBase.SysUser.UserInfo_Edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>用户信息表单</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <link href="/Themes/Scripts/TreeTable/css/jquery.treeTable.css" rel="stylesheet"
        type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/Validator/JValidator.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/TreeTable/jquery.treeTable.js" type="text/javascript"></script>
    <link href="/Themes/Scripts/TreeView/treeview.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/TreeView/treeview.pack.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .checkAllOff
        {
            padding-left: 10px;
            background: url(/Themes/Images/checkAllOff.gif) no-repeat scroll 0px 2px;
            cursor: pointer;
        }
        .checkAllOn
        {
            padding-left: 10px;
            background: url(/Themes/Images/checkAllOn.gif) no-repeat scroll 0px 2px;
            cursor: pointer;
        }
        .frm tr td:first-child,
        .frm tr td:nth-child(3)
        {
            width: 70px;
            text-align: right;
            padding-right: 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="div-frm" style="height: 199px;">
        <%--基本信息--%>
        <table id="table1" border="0" cellpadding="0" cellspacing="0" class="frm">
            <tr>
                <td>
                    用户名
                </td>
                <td>
                    <asp:TextBox Enabled="false" ID="User_Account" runat="server" class="txt"></asp:TextBox>
                </td>
                <td>
                    姓名
                </td>
                <td>
                    <asp:TextBox ID="User_Name" runat="server" class="txt"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    性别
                </td>
                <td>
                    <select id="User_Sex" class="select" runat="server" style="width: 86px">
                        <option value="1">男</option>
                        <option value="0">女</option>
                    </select>
                </td>
                <td>
                    电子邮件
                </td>
                <td>
                    <input id="Email" runat="server" type="text" class="txt" style="width: 138px" />
                </td>
            </tr>
            <tr>
                <td>
                    职称
                </td>
                <td>
                    <input id="Title" runat="server" type="text" class="txt" style="width: 110px" />
                </td>
                <td>
                    联系电话
                </td>
                <td>
                    <input id="Theme" runat="server" type="text" class="txt" style="width: 138px" />
                </td>
            </tr>
            <tr id="hotelid" runat="server" visible="false">
                <td>
                    所属酒店
                </td>
                <td>
                    <span id="hotelname" runat="server"></span>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top;">
                    备注
                </td>
                <td colspan="3">
                    <textarea id="User_Remark" class="txtRemark" runat="server" style="width: 395px;
                        height: 70px;"></textarea>
                </td>
            </tr>
        </table>
    </div>
    <div class="adifoliBtn">
        <div style="float:right;">
            <asp:LinkButton ID="Save" runat="server" OnClientClick="return CheckValid();" OnClick="Save_Click">
                <span class="bbgg bbgg1">保 存</span>
            </asp:LinkButton>
            <a class="bbgg" href="javascript:void(0)" onclick="OpenClose();"><span>关 闭</span></a>
        </div>
    </div>
    </form>
</body>
</html>
