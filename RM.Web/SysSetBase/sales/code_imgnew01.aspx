<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="code_imgnew01.aspx.cs" Inherits="RM.Web.SysSetBase.sales.code_imgnew01" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
   <script src="/Themes/Scripts/Validator/JValidator.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script src="../../Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>              <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />


</head>
<body>
    <form id="form1" runat="server" style="scroll: none; ">
    <input runat="server" type="hidden" id="Hdhoteladmin" />
        <input runat="server" type="hidden" id="HdHotelName" />
        <input runat="server" type="hidden" id="HdUser_ID" />
    <div id="EWM" class="checkewm checkewmnew clearfix">
        <div id="tgewm" class="checkewml" style="width:270px;">
            <div class="xm">
                [<span id="Span_Name"  runat="server">张磊</span>] 推广二维码
            </div>
            <div class="ts">
                用于制作广告,印名片等, 供客人扫描使用
            </div>
            <div class="ewm">
                <div style="position:relative;width: 135px;height: 135px;margin: 0 auto;margin-top:92.5px;">
                  <img  id="First_code"  runat="server">
                  <img  id="First_codes"  runat="server"  class="ewmzx1">
                </div>
            </div>
            <div class="xz">
               <asp:Button ID="btnSumit" runat="server"  Text="下载" class="btn"  OnClick="btnSumit_Click" />
            </div>
        </div>
        <div id="zdyewm" class="checkewml" style="width:320px;">
            <div class="xm">
                [<span id="Span_Names"  runat="server">张磊</span>] 推广二维码展牌
            </div>
            <div class="ts">
                用于在酒店前台或收银出示给客人扫描使用
            </div>
            <div class="ewm">
                <div class="zpyl" style="height:320px;margin-top:0;">
                    <div class="d2">
                        <div class="d21">
                            <img id="HotelLogo" runat="server" src="../img/ewm.png" alt="Alternate Text" style="margin-top:0;"/>
                        </div>

                        <div class="">
                              <asp:Label ID="lblHotelNameCode" runat="server"></asp:Label>  
                        </div>

                        <div class="d22">
                            <img src="../img/sewaewm.jpg"  id="Second_code"  runat="server" alt="Alternate Text" style="margin-top:0;"/>
                            <img src="../img/ewm.png" id="Second_codes"  runat="server"  alt="Alternate Text" class="ewmzx" style="margin-top:0;"/>
                        </div>
                        <div class="d23"   id="lblAdvertising" runat="server">
                            通过扫描维码<br />                            关注“XX酒店”微信公众号<br />                            首次入住立减40元<br />                          （另送早餐）   
                        </div>
                    </div>
                </div>

            </div>
            <div class="xz">
                   <asp:Button ID="btnSumits" runat="server"  Text="下载" class="btn"  OnClick="btnSumits_Click" />

                   <a class="btn" href="<%=_code_imgnew05 %>"   target="_blank" style="margin-left:5px;" ><span>打印</span></a>
                  
            </div>
        </div>
    </div>
    <div class="adifoliBtn">
        <div style="float: right;">
                 <a class="bbgg" href="javascript:void(0)" onclick="OpenClose();"><span>关 闭</span></a>
        </div>
    </div>
    </form>
</body>
</html>
