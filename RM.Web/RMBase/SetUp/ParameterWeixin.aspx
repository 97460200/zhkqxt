﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ParameterWeixin.aspx.cs"
    Inherits="RM.Web.RMBase.SetUp.ParameterWeixin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/jquery.pullbox.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        function fun(val) {
            var a = document.getElementById("ullist").getElementsByTagName("a");
            for (var i = 0; i < a.length; i++) {
                a[i].className = "";
            }
            val.className = "selected";
        }
    </script>
    <title></title>
    <style type="text/css">
        html, body
        {     
            overflow: hidden;
        }
        .sz
        {
            list-style: none;
            font-family: 微软雅黑;
            float: left;
            margin-bottom: 15px;
            border-bottom: 1px solid #ccc;
            height: auto;
            padding: 10px 0px 0px 8px;
            height: 39px;
        }
        .sz a
        {
            font-size: 14px;
        }
        .jl
        {
            font-weight: bold;
            color: #cc0000;
            outline: none;
            display: inline-block;
            line-height: 36px;
            height: 36px;
            margin-top: 3px;
            float: left;
            cursor: pointer;
            padding: 0px 20px;
            font-size: 14px;
            border-left: 1px solid #dfdfdf;
            border-right: 1px solid #dfdfdf;
            border-bottom: 1px solid #fff;
            margin-left: 10px;
            background: url(../images/bbgg.jpg) repeat-x left top;
        }
         div.Tabsel a,div.Tabremovesel a
        {
            border: 0;
            height:39px;
            line-height:39px;
            margin-top:0;
            margin: 0px 16px;
            padding:0;
        }
        div.Tabsel a
        {
            border-bottom:2px solid #f90;
            font-weight: bolder;
        }
        div.Tabsel
        {
            border-bottom: 0;
            background-position-y: 33px;
            margin-top:0;
            height:39px;
            line-height:39px;
        }
        div.Tabremovesel
        {
            border: 0;
            background: none;
            margin-top:0;
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
            if (Hotelid != "0") {
                $("#Template").hide();
                $("#Basic").hide();
            } else {
                $("#Authorization").hide();
            }
            var ifa = document.all("iframe1");
            ifa.src = "/RMBase/SysParameter/Pay2.aspx?AdminHotelid=" + AdminHotelid + "&Hotelid=" + Hotelid;
        });

        function GetTabClicks(val, type) {
            var a = document.getElementById("menutab").getElementsByTagName("div");
            for (var i = 0; i < a.length; i++) {
                a[i].className = "Tabremovesel";
            }
            val.className = "Tabsel";
            var ifa = document.all("iframe1");
            if (type == 1) {
                ifa.src = "/RMBase/SysParameter/Pay2.aspx?AdminHotelid=" + AdminHotelid + "&Hotelid=" + Hotelid;
            }
            else if (type == 2) {
                ifa.src = "/RMBase/SysParameter/Attachment.aspx?AdminHotelid=" + AdminHotelid + "&Hotelid=" + Hotelid;
            }
            else if (type == 3) {
                ifa.src = "/RMBase/SysParameter/wxMenu.aspx?AdminHotelid=" + AdminHotelid + "&Hotelid=" + Hotelid;
            }
            else if (type == 4) {
                ifa.src = "/RMBase/SysParameter/wxtsid.aspx?AdminHotelid=" + AdminHotelid + "&Hotelid=" + Hotelid;
            }
            else if (type == 5) {
                ifa.src = "/RMBase/SysParameter/WeChatInfo.aspx?AdminHotelid=" + AdminHotelid + "&Hotelid=" + Hotelid;
            }
        }

       
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <input runat="server" id="hdVal" type="hidden" value="" />
    <input runat="server" id="GSID" type="hidden" value="" />
    <input runat="server" id="HotelID" type="hidden" value="" />
    <table height="100%" cellspacing="0" cellpadding="0" width="100%">
        <tr>
            <td height="1">
                <div id="main_title">
                </div>
                <div class="back">
                    <div class="back_left">
                        <em>
                            <asp:Label ID="lblClinetName" runat="server" Text=""></asp:Label></em></div>
                    <div class="back_right">
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div class="scrolling" style="height: 100%;">
                    <div id="div_content">
                        <div id="img_bag2">
                            <div id="pettabs" class="indentmenu">
                                <div class="frmtop">
                                    <table style="padding: 0px; margin: 0px; height: 100%;" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td id="menutab" style="vertical-align: bottom;">
                                                <div id="Bind" runat="server" class="Tabsel" onclick="GetTabClicks(this,1);">
                                                    <a>微信绑定设置</a></div>
                                                <div id="Div1" runat="server" class="Tabremovesel" onclick="GetTabClicks(this,3);">
                                                    <a>微信菜单设置</a></div>
                                                <div id="Template" runat="server" class="Tabremovesel" onclick="GetTabClicks(this,4);">
                                                    <a>微信模板设置</a></div>
                                                <div id="Authorization" runat="server" class="Tabremovesel" onclick="GetTabClicks(this,2);">
                                                    <a>微信授权文件</a></div>
                                                <div id="Basic" runat="server" class="Tabremovesel" onclick="GetTabClicks(this,5);">
                                                    <a>公众号基本信息设置</a></div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                        <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                            <tr>
                                <td>
                                    <iframe id="iframe1" runat="server" name="iframe1" width="100%"  style="height:100vh;"
                                        frameborder="0" marginheight="0" marginwidth="0" scrolling="no"></iframe>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    </form>
    <script type="text/javascript">
        $("#menutab").on("click", "div", function () {
            var index = $(this).index();
            $("#menutab > div").attr("class", "Tabremovesel")
            $(this).attr("class", "Tabsel")
        })
        
    </script>
</body>
</html>
