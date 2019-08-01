<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetUpInfo.aspx.cs" Inherits="RM.Web.RMBase.SetUp.SetUpInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <style type="text/css">
        div.sharetabsliact
        {
            float: left;
            width: auto;
            padding: 0 10px;
            height: 28px;
            line-height: 28px;
            text-align: center;
            margin-top: 10px;
            border: 1px solid #dfdfdf;
            margin-right: 5px;
            border-top-left-radius: 5px;
            border-top-right-radius: 5px;
            cursor: pointer;
            font-weight: bolder;
            color: #000;
            background: #fff;
            border-bottom: 1px solid #fff;
        }
        div.sharetabsli
        {
            float: left;
            width: auto;
            padding: 0 10px;
            height: 28px;
            line-height: 28px;
            text-align: center;
            margin-top: 10px;
            border: 1px solid #dfdfdf;
            margin-right: 5px;
            border-top-left-radius: 5px;
            border-top-right-radius: 5px;
            background: #f9f9f9;
            cursor: pointer;
        }
        div.sharetabsli a, div.sharetabsliact a
        {
            border: none;
            padding: 0;
            margin-top: 0;
            font-size: 12px;
            height: auto;
            line-height: 28px;
        }
        div.sharetabsli a:hover, div.sharetabsliact a:hover
        {
            color: #000;
        }
    </style>
    <script type="text/javascript">
        //获取地址栏参数
        function getUrlParam(name) {

            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
            var r = window.location.search.substr(1).match(reg);  //匹配目标参数
            if (r != null) return unescape(r[2]); return null; //返回参数值
        }
        var AdminHotelid = "";
        var Hotelid = "";
        $(function () {
            AdminHotelid = getUrlParam('AdminHotelid');
            Hotelid = getUrlParam('Hotelid');
            var ifa = document.all("iframe1");
            if (Hotelid != "0") {
                ifa.src = "/RMBase/SetUp/ParameterWeixin.aspx?AdminHotelid=" + AdminHotelid + "&Hotelid=" + Hotelid;
            }
            else {
                ifa.src = "/RMBase/SetUp/ParameterMember.aspx?AdminHotelid=" + AdminHotelid + "&Hotelid=" + Hotelid;
            }

        });

        function GetTabClicks(val, type) {
            var a = document.getElementById("menutab").getElementsByTagName("div");

            for (var i = 0; i < a.length; i++) {
                a[i].className = "Tabremovesel sharetabsli";
            }
            val.className = "Tabsel sharetabsliact";

            var ifa = document.all("iframe1");
            if (type == 1) {
                ifa.src = "/RMBase/SetUp/ParameterMember.aspx?AdminHotelid=" + AdminHotelid + "&Hotelid=" + Hotelid;
            }
            else if (type == 3) {
                ifa.src = "/RMBase/SetUp/ParameterWeixin.aspx?AdminHotelid=" + AdminHotelid + "&Hotelid=" + Hotelid;
            }
            else if (type == 4) {
                ifa.src = "/RMBase/SysParameter/SetMsmInfo.aspx?AdminHotelid=" + AdminHotelid + "&Hotelid=" + Hotelid;
            }
            else if (type == 5) {
                ifa.src = "/RMBase/SysParameter/function_set.aspx?AdminHotelid=" + AdminHotelid + "&Hotelid=" + Hotelid;
            }
            else if (type == 6) {
                ifa.src = "/RMBase/SysParameter/SetAssociation.aspx?AdminHotelid=" + AdminHotelid + "&Hotelid=" + Hotelid;
            } else if (type == 7) {
                ifa.src = "/RMBase/SysParameter/SetDefault.aspx?AdminHotelid=" + AdminHotelid + "&Hotelid=" + Hotelid;
            } else if (type == 8) {
                ifa.src = "/RMBase/SysParameter/SetMemCode.aspx?AdminHotelid=" + AdminHotelid + "&Hotelid=" + Hotelid;
            } else if (type == 9) {
                ifa.src = "/SysSetBase/superAdmin/hotelsetting.aspx?AdminHotelid=" + AdminHotelid + "&Hotelid=" + Hotelid;
            } else if (type == 10) {
                //alert(AdminHotelid);
                ifa.src = "/RMBase/SysParameter/SetMenu.aspx?HoleID=" + AdminHotelid;
            } else if (type == 11) {

                ifa.src = "/RMBase/SysParameter/SetAgent.aspx?AdminHotelid=" + AdminHotelid;
            } else if (type == 12) {

                ifa.src = "/RMBase/SysCalendar/Weekend.aspx?AdminHotelid=" + AdminHotelid;
            } else if (type == 13) {
                ifa.src = "/RMBase/SysBonusGroup/BonusGroupList.aspx?AdminHotelid=" + AdminHotelid;
            }
        }
       
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <input runat="server" type="hidden" id="hdMenus" />
    <div class="tools_bar btnbartitle btnbartitlenr" style="display: ">
        <div>
            当前位置：<asp:SiteMapPath ID="SiteMapPath1" runat="server">
            </asp:SiteMapPath>
            - <span id="hotelname" runat="server"></span>
        </div>
    </div>

                <div id="main_title">
                </div>
                <div class="back">
                    <div class="back_left">
                        <em>
                            <asp:Label ID="lblClinetName" runat="server" Text=""></asp:Label></em></div>
                    <div class="back_right">
                    </div>
                </div>
   
  
                <div class="scrolling" style="">
                    <div id="div_content" class="cccN">
                        <div id="img_bag2">
                            <div id="pettabs" class="indentmenu">
                                <div class="frmtop">
                                    <table style="padding: 0px; margin: 0px;" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td id="menutab" style="vertical-align: bottom;">
                                                <div id="Menber" runat="server" class="Tabsel sharetabsliact" onclick="GetTabClicks(this,1);">
                                                    <a class="selected">会员设置</a></div>
                                                <div id="WeChat" runat="server" class="Tabremovesel sharetabsli" onclick="GetTabClicks(this,3);">
                                                    <a class="selected">微信设置</a></div>
                                                <div id="Sms" runat="server" class="Tabremovesel sharetabsli" onclick="GetTabClicks(this,4);">
                                                    <a class="selected">短信设置</a></div>
                                                <div id="Function" runat="server" class="Tabremovesel sharetabsli" onclick="GetTabClicks(this,5);">
                                                    <a class="selected">功能开启设置</a></div>
                                                <div id="SetAssociation" runat="server" class="Tabremovesel sharetabsli" onclick="GetTabClicks(this,6);">
                                                    <a class="selected">管理系统对接设置</a></div>
                                                <div id="SetDefault" runat="server" style="display: none" class="Tabremovesel sharetabsli"
                                                    onclick="GetTabClicks(this,7);">
                                                    <a class="selected">首页设置</a></div>
                                                <div id="SetMemCode" runat="server" class="Tabremovesel sharetabsli" onclick="GetTabClicks(this,8);">
                                                    <a class="selected">会员码设置</a></div>
                                                <div id="Setbonus" runat="server" class="Tabremovesel sharetabsli" onclick="GetTabClicks(this,9);">
                                                    <a class="selected">维护费设置</a></div>
                                                <div id="Div1" runat="server" class="Tabremovesel sharetabsli" onclick="GetTabClicks(this,13);">
                                                    <a class="selected">推广奖金设置</a></div>
                                                <div id="SetMenu" runat="server" class="Tabremovesel sharetabsli" onclick="GetTabClicks(this,10);">
                                                    <a class="selected">菜单设置</a></div>
                                                <div id="SetAgent" runat="server" class="Tabremovesel sharetabsli" onclick="GetTabClicks(this,11);">
                                                    <a class="selected">销售渠道</a></div>
                                                <div id="SetWeekend" runat="server" class="Tabremovesel sharetabsli" onclick="GetTabClicks(this,12);">
                                                    <a class="selected">周末设置</a></div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                     
                                    <iframe id="iframe1" runat="server" name="iframe1" width="100%" style="height:calc(100vh - 85px)" frameborder="0"
                                        marginheight="0" marginwidth="0" scrolling="yes"></iframe>
                 
                    </div>
                </div>

    </form>
</body>
</html>
