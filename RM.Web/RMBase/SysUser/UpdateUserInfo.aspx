<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdateUserInfo.aspx.cs"
    Inherits="RM.Web.RMBase.SysUser.UpdateUserInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>修改密码</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/jquery.md5.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        function CheckValid() {
            if ($('#User_Name').val() == '') {
                $("#User_Name").focus();
                $("#errorMsg").show();
                $("#errorMsg").html("请输入姓名！");
                return false;
            }
            if ($('#User_Account').val() == '') {
                $("#User_Account").focus();
                $("#errorMsg").show();
                $("#errorMsg").html("请输入账号/手机号码！");
                return false;
            }
        }
    </script>
    <style type="text/css">
        #errorMsg
        {
            border: 1px solid #A8A8A8;
            width: auto;
            padding-left: 30px;
            padding-right: 20px;
            padding-top: 3px;
            padding-bottom: 2px;
            background: rgb(255, 253, 215) url('/Themes/images/exclamation_octagon_fram.png') no-repeat scroll 5px 2px;
            color: red;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <input id="hdUserId" type="hidden" runat="server" value="" />
    <table border="0" cellpadding="0" cellspacing="0" class="frm" style="margin-top: 10px;
        height: 210px;">
        <tr>
            <th>
                账号/手机号
            </th>
            <td>
                <input id="User_Account" runat="server" type="text" class="txt" style="width: 200px" />
            </td>
        </tr>
        <tr>
            <th>
                姓名
            </th>
            <td>
                <input id="User_Name" runat="server" type="text" class="txt" style="width: 200px" />
            </td>
        </tr>
        <tr>
            <th>
                性别
            </th>
            <td>
                <select id="User_Sex" class="select" runat="server" style="width: 86px">
                    <option value="1">男</option>
                    <option value="0">女</option>
                </select>
            </td>
        </tr>
        <tr>
            <th>
                角色
            </th>
            <td>
                <asp:DropDownList ID="ddlUser_Role" runat="server" class="select" Enabled="false"
                    Style="height: 28px;">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <span id="errorMsg" runat="server" style="display: none">修改密码</span>
            </td>
        </tr>
    </table>
    <div class="adifoliBtn">
        <div style="float: right;">
            <asp:LinkButton ID="Save" runat="server" class="bbgg bbgg1" OnClientClick="return CheckValid();"
                OnClick="Save_Click">保 存</asp:LinkButton>
            <a class="bbgg" href="javascript:void(0)" onclick="OpenClose();">关 闭</a>
        </div>
    </div>
    </form>
</body>
</html>
