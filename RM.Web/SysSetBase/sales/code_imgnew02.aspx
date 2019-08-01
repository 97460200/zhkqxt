<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="code_imgnew02.aspx.cs" Inherits="RM.Web.SysSetBase.sales.code_imgnew02" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
    <form id="form1" runat="server">
    <div id="EWM" class="checkewm checkewmnew01 clearfix">
        <div id="tgewm" class="checkewml">
            <div class="xm">
                智订云商户平台二维码
            </div>
            <div class="ewm">
                <img src="../img/sewaewm.jpg" />
            </div>
            <div class="ts">
                关注智订云商户版后可以更方便的领取奖金
            </div>
            <div class="xz">
               <asp:Button ID="btnSumit" runat="server"  Text="下载二维码"  OnClick="btnSumit_Click" style="width: auto;background: #fff;border: none;color: #39c;cursor: pointer;"/>
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
