<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberDetails.aspx.cs"
    Inherits="RM.Web.RMBase.SysSetBase.sales.MemberDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/jquery.pullbox.js" type="text/javascript"></script>
    <script src="../../Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
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
        div.Tabsel a, div.Tabremovesel a
        {
            font-family: 宋体;
            font-size: 14px;
        }
        div.Tabsel a
        {
            color: #cc0000;
            line-height: 28px;
            margin-top: 1px;
        }
        .connn .f_w
        {
            width: 60px;
            text-align: left;
        }
        .connn tr td:first-child
        {
            padding-left: 20px;
        }
    </style>
    <script type="text/javascript">
        function jl2(a) {
            a.className = ".jl";
        }

        function GetTabClicks(val, type) {
            var a = document.getElementById("menutab").getElementsByTagName("div");
            for (var i = 0; i < a.length; i++) {
                a[i].className = "Tabremovesel";
            }
            val.className = "Tabsel";
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:HiddenField ID="hdUser_ID" runat="server" />
    <div class="scrolling" style="height: 100%;">
        <div id="div_content">
            <div class="div-body" style="overflow: hidden; margin-top: 20px">
                <div class="connn">
                    <table height="100%" cellspacing="0" cellpadding="0" width="100%">
                        <tr style="display: none">
                            <td colspan="6" height="1">
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
                            <td class="f_w">
                                手机号码
                            </td>
                            <td>
                                <asp:Literal ID="lblPhone" runat="server"></asp:Literal>
                            </td>
                            <td class="f_w">
                                微信昵称
                            </td>
                            <td>
                                <asp:Literal ID="lblWXName" runat="server"></asp:Literal>
                            </td>
                            <td class="f_w">
                                角色
                            </td>
                            <td>
                                <asp:Literal ID="lblRolse" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td class="f_w">
                                创建时间
                            </td>
                            <td>
                                <asp:Literal ID="lblAddTime" runat="server"></asp:Literal>
                            </td>
                            <td class="f_w">
                                累计销售额
                            </td>
                            <td>
                                <asp:Literal ID="lblKeFang" runat="server"></asp:Literal>
                            </td>
                            <td class="f_w">
                                累计获得奖金
                            </td>
                            <td>
                                <asp:Literal ID="lblJiangJin" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr id="Tr1" runat="server">
                            <td class="f_w">
                                累计提现金额
                            </td>
                            <td>
                                <asp:Literal ID="lblTiXian" runat="server"></asp:Literal>
                            </td>
                            <td class="f_w">
                                剩余可提奖金
                            </td>
                            <td>
                                <asp:Label ID="lblShengYu" runat="server"></asp:Label>
                            </td>
                            <td class="f_w">
                                累计带来客户
                            </td>
                            <td>
                                <asp:Literal ID="lblDaiLai" runat="server"></asp:Literal>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="img_bag2">
                    <div id="pettabs" class="indentmenu">
                        <div class="frmtop">
                            <table style="padding: 0px; margin: 0px; height: 100%;" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td id="menutab" style="vertical-align: bottom;">
                                        <div id="tab0" class="Tabsel" onclick="GetTabClicks(this);panel(1)">
                                            <a target="iframe1" onclick="jl2(this)" onfocus="this.blur()" href="OrderRecord.aspx?id=<%=ae%>"
                                                class="selected">奖金记录</a></div>
                                        <div id="tab1" runat="server" class="Tabremovesel" onclick="GetTabClicks(this);panel(2);">
                                            <a target="iframe1" onfocus="this.blur()" onclick="jl2(this)" href="TiXianList.aspx?id=<%=ae %>"
                                                id="hrefBase">提现记录</a></div>
                                        <div id="Div1" visible="true" runat="server" class="Tabremovesel" onclick="GetTabClicks(this);panel(2);">
                                            <a target="iframe1" onfocus="this.blur()" onclick="jl2(this)" href="DaiLiKeHu.aspx?id=<%=ae %>"
                                                id="A1">带来客户</a></div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <table cellpadding="0" cellspacing="0" border="0" style="width: 99%;">
                    <tr>
                        <td>
                            <iframe id="iframe1" runat="server" name="iframe1" width="100%" height="700px" frameborder="0"
                                marginheight="0" marginwidth="0" scrolling="no"></iframe>
                            <script language="javascript">
                                var url = location.search; //获取url中"?"符后的字串
                                var str = "";
                                if (url.indexOf("?") != -1) {
                                    var str = url.substr(1);
                                    strs = str.split("&");
                                    for (var i = 0; i < strs.length; i++) {
                                        str = (strs[i].split("=")[1]);
                                    }
                                }
                                var ifa = document.all("iframe1");
                                ifa.src = "OrderRecord.aspx?id=" + str;     
                            </script>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    </form>
    <script type="text/javascript">
        $("#menutab").on("click", "div", function () {
            $(this).siblings().attr("class", "Tabremovesel");
            $(this).attr("class", "Tabsel");
        });
    </script>
</body>
</html>
