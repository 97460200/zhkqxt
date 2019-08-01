<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImportUsers.aspx.cs" Inherits="RM.Web.RMBase.SysUser.ImportUsers" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>修改密码</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/jquery.md5.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
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
    <input id="hdAdminHotelId" type="hidden" runat="server" value="" />
    <input id="hdHotelId" type="hidden" runat="server" value="" />
    <input id="hdUserId" type="hidden" runat="server" value="" />
    <table border="0" cellpadding="0" cellspacing="0" class="frm" style="margin-top: 10px;
        height: 210px;">
        <tr>
            <th>
                模板
            </th>
            <td>
                <a target="_blank" href="员工导入模板.xls"><span runat="server" id="spUserName">下载《员工导入模板》</span></a>
            </td>
        </tr>
        <tr>
            <th>
                导入文件
            </th>
            <td>
                <asp:FileUpload ID="FileUpload1" runat="server" onchange="selectIcon(this)" Style="background: none;
                    padding: 0px; width: 220px" />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <span id="errorMsg" runat="server" visible="false">错误提示</span>
            </td>
        </tr>
    </table>
    <div class="adifoliBtn">
        <div style="float: right;">
            <asp:LinkButton ID="Save" runat="server" class="bbgg bbgg1" OnClientClick="return CheckValid();"
                OnClick="Save_Click">导 入</asp:LinkButton>
            <a id="aHandle" style="display: none;" class="bbgg" href="javascript:void(0)">处理中</a>
            <a class="bbgg" href="javascript:void(0)" onclick="OpenClose();">关 闭</a>
        </div>
    </div>
    <script type="text/javascript">
        function CheckValid() {
            $("#Save").hide();
            $("#aHandle").show();
        }
    </script>
    </form>
</body>
</html>
