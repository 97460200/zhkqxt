<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ParameterWeixins.aspx.cs" Inherits="RM.Web.RMBase.SetUp.ParameterWeixins" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
        var holeID = "";
        $(function () {
            debugger
            holeID = getUrlParam('HoleID');
            var ifa = document.all("iframe1");
            ifa.src = "/SysSetBase/xitongcanshu/weixinsz.aspx";
        });

        function GetTabClicks(val, type) {
            var a = document.getElementById("menutab").getElementsByTagName("div");
            for (var i = 0; i < a.length; i++) {
                a[i].className = "Tabremovesel";
            }
            val.className = "Tabsel";
            var ifa = document.all("iframe1");
            if (type == 1) {
                ifa.src = "/SysSetBase/xitongcanshu/weixinsz.aspx";
            }
            else if (type == 2) {
                ifa.src = "/RMBase/SysParameter/wxMenu.aspx?AdminHotelid=" + $("#hdAdminHotelid").val() + "&Hotelid=0";
            }
            else if (type == 3) {
                ifa.src = "/RMBase/SysReply/ReplyList.aspx";
                
            }
            
        }

       
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <input runat="server" id="hdVal" type="hidden" value="" />
    <input runat="server" id="GSID" type="hidden" value="" />
    <input runat="server" id="HotelID" type="hidden" value="" />

    <input runat="server" id="hdAdminHotelid" type="hidden" value="" />
    <div class="tools_bar btnbartitle btnbartitlenr" style="display:block;">
        <div>
            当前位置：<span><a title="系统首页" href="/Frame/HomeIndex.aspx">系统首页</a></span><span> &gt; </span><span>系统设置</span> &gt; </span><span>微信设置</span></span>
        </div>
    </div>
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
                                            <td id="menutab">
                                                <div id="tab1" runat="server" class="Tabsel" onclick="GetTabClicks(this,1);">
                                                    <a>微信公众号设置</a></div>
                                                <div id="tab4" runat="server" class="Tabremovesel" onclick="GetTabClicks(this,2);" style=" display">
                                                    <a>微信菜单设置</a></div>
                                                <div id="Div1" runat="server" class="Tabremovesel" onclick="GetTabClicks(this,3);">
                                                    <a>微信关键词设置</a></div>
                                                    
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                        <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                            <tr>
                                <td>
                                    <iframe id="iframe1" runat="server" name="iframe1" width="100%" height="1000px"
                                        frameborder="0" marginheight="0" marginwidth="0" scrolling="no" ></iframe>         
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
        var aaa = $(window).height() - 86; //滚动后左侧菜单高度
        $("#iframe1").height(aaa);
        $(window).bind({
            resize: function () {
                $("#iframe1").height(aaa);
            }
        })
        $("#menutab").on("click", "div", function () {
            var index = $(this).index();
            $("#menutab > div").attr("class", "Tabremovesel")
            $(this).attr("class", "Tabsel");
        })
       
    </script>
</body>
</html>
