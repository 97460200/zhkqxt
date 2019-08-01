<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CardMoneyCode.aspx.cs" Inherits="RM.Web.SysSetBase.pay.CardMoneyCode" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

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
    <form id="form1" runat="server">
    <input runat="server" type="hidden" id="hdAdminHotelId" />
    <div style="position: relative; width: 540px; height: 540px; margin: 0 auto;">
        <img id="imgPledgeMoneyCode" runat="server" style="width: 540px; height: 540px;">
        <img id="First_codes" runat="server" class="ewmzx1" style="top: 200px; left: 200px;
            width: 140px; height: 140px; position: absolute; border: 1px solid #fff; border-radius: 5px;">
    </div>
    </form>
</body>
</html>
