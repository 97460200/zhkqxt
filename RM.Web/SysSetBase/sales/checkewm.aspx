<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="checkewm.aspx.cs" Inherits="RM.Web.SysSetBase.sales.checkewm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
     <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <link href="/Themes/Scripts/PqGrid/style.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/PqGrid/jquery-ui.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/PqGrid/pqgrid.min.js" type="text/javascript"></script>
    <link href="/Themes/Scripts/PqGrid/pqgrid.min.css" rel="stylesheet" />
    <script type="text/javascript">
        //查看二维码
        function CheckErWeiMa() { 
            var url = "/SysSetBase/sales/selectsize.aspx?key=1";
            top.art.dialog.open(url, {
                id: 'CheckErWeiMa',
                title: '选择尺寸',
                width: 480,
                height: 250,
                close: function () { 
                 
                }
            }, false);
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:HiddenField ID="hdFxurl" runat="server" />
       <asp:HiddenField ID="hdwxName" runat="server" />
    <div class="checkewm clearfix">
        <div class="checkewml">
            <div class="xm">
               [<%=hdwxName.Value %>] 推广二维码
            </div>
            <div class="ewm">
                <img  id="fxsrc" runat="server"   src="../img/ewm.png"/>
            </div>

            <div class="ts">
                用于制作展牌,广告,印名片等, 供客人扫描使用
            </div> 
            <div class="xz">
<%--                <a   onclick="CheckErWeiMa()">下载二维码</a>--%>
                <a   download="<%=hdFxurl.Value %>.png"  href="../../QR_code/MemberQRCode/<%=hdFxurl.Value %>">下载二维码</a>
            </div>
        </div>
     
    </div>
    </form>
</body>
</html>
