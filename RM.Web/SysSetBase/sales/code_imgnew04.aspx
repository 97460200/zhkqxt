<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="code_imgnew04.aspx.cs"  Inherits="RM.Web.SysSetBase.sales.code_imgnew04" %>

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
    <style type="text/css">
        .d23 span,.d23 p
        {
            font-size:32px !important;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <input runat="server" type="hidden" id="Hdhoteladmin" />
    <input runat="server" type="hidden" id="HdUser_ID" />
    <div id="EWM" class="checkewm checkewmnew clearfix"   style="width:597px;height:844px;padding-left:0;margin:0 auto;">
        <div id="zdyewm" class="checkewml" style="margin-right:0;width:595px">
            <div class="ewm" style="width: 595px; height: 844px;padding-left: 0;border:none;">
                <div class="zpyl" style="width: 595px;height:844px;margin-top:0;">
                    <div class="d2" style="width: 595px;height:844px;border:none;">
                        <div class="d21" style="width: 210px; height: 210px;padding-top:32px;padding-bottom:12px;">
                            <img src="../img/ewm.png" id="HotelLogo" runat="server" alt="Alternate Text" style="margin-top:0;"/>
                        </div>

                         <div>
                             <asp:Label ID="lblHotelNameCode" runat="server"  style="font-size: 32px;"></asp:Label>  
                        </div>

                        <div class="d22" style="width: 286px; height: 286px;margin-top: 26px;">
                            <img src="../img/sewaewm.jpg"  id="Second_code"  runat="server" alt="Alternate Text" style="margin-top:0;"/>
                            <img src="../img/ewm.png" id="Second_codes"  runat="server"  alt="Alternate Text" class="ewmzx" style="margin-top:0;top: 104px;left: 104px;width: 78px;height: 78px;border: 1px solid #ccc;border-radius: 5px;-moz-border-radius: 5px; -webkit-border-radius: 5px;"/>
                        </div>
                        <div class="d23" id="lblAdvertising"  runat="server" style="line-height: 48px;margin-top: 26px;font-size: 32px;">
                            通过扫描维码<br />                            关注“XX酒店”微信公众号<br />                            首次入住立减40元<br />                          （另送早餐）   
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>

    </form>
</body>
</html>
