<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="code_img.aspx.cs" Inherits="RM.Web.SysSetBase.sales.code_img" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="../../App_Themes/default/js/jquery-1.8.3.min.js" type="text/javascript"></script>
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <link href="/Themes/Scripts/PqGrid/style.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/PqGrid/jquery-ui.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/PqGrid/pqgrid.min.js" type="text/javascript"></script>
    <link href="/Themes/Scripts/PqGrid/pqgrid.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server" style="scroll: none; ">
    <div class="checkewm clearfix" id="EWM" runat="server">
        <div class="checkewml" id="tgewm" runat="server">
            <div class="xm">
                [<asp:Label ID="txtTitles" runat="server"></asp:Label>] 推广二维码
            </div>
            <div class="ewm">
                <img id="img_codes" runat="server" src="../img/ewm.png" />
            </div>
            <div class="ts">
                用于制作展牌,广告,印名片等, 供客人扫描使用
            </div>
            <div class="xz">
                <a download="<%=_img_codes %>" href="<%=_img_codes %>">下载二维码</a>
            </div>
        </div>
        <div class="checkewml" id="zdyewm" runat="server">
            <div class="xm">
                智订云商户平台二维码
            </div>
            <div class="ewm">
                <img src="../img/sewaewm.jpg" />
            </div>
            <div class="ts">
                关注智订云商户平台后可以查询及领取奖金
            </div>
            <div class="xz">
                <a download="../img/sewaewm.jpg" href="../img/sewaewm.jpg">下载二维码</a>
            </div>
        </div>
    </div>
    <div class="adifoliBtn">
        <div style="float: right;">
            <a href="#">关闭</a>
        </div>
    </div>
    </form>
</body>
</html>
