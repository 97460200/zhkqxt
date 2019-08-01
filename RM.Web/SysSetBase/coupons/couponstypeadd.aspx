<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="couponstypeadd.aspx.cs" Inherits="RM.Web.SysSetBase.coupons.couponstypeadd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">  
    <link href="../../Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../../Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script src="../../Themes/js/button.js" type="text/javascript"></script>
    <link href="../../App_Themes/admin/css/main.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
        <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>

    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <input runat="server" type="hidden" id="Hdhoteladmin" value="1" />
    <input runat="server" type="hidden" id="hdHotelId" value="-1" />
    <dl class="adifoli adifolikw" style="max-height: 76px; height: 76px; width: 400px;
        overflow: auto;">
        <dd>
            <small>类型名称</small>
            <div>
                <input name="txtKindName" type="text" maxlength="30" id="txtKindName" runat="server" style="width:100%">
            </div>
        </dd>
    </dl>
    <div class="adifoliBtn">
        <div style="float: right;">
          <asp:Button ID="btnSumit" runat="server" Text="提交" OnClientClick="return check()" OnClick="btnSumit_Click" />
          <asp:Button ID="btnCancel" runat="server" Text="重置" OnClientClick=" return clck()"  />
        </div>
    </div>
    </form>
</body>
</html>

<script>
    function check() {


        if ($('#txtKindName').val() == '') {
            showTipsMsg("请输入类型名称！", 3000, 3);
            $('#txtKindName').focus();
            return false;
        }
    }
    function clck() {
        document.getElementById("txtKindName").value = "";
    }
</script>
