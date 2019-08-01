<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="kfsz.aspx.cs" Inherits="RM.Web.SysSetBase.xitongcanshu.kfsz" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>客房设置</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="../../Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <style type="text/css">
        .paymentsettings .fsl
        {
            margin-top: 22px;
        }
        .kefangsz
        {
            margin-bottom: 0;
        }
        .paymentsettings .kefangszb dd small
        {
            width: 120px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <input runat="server" type="hidden" id="hdAdminHotelId" />
    <div class="tools_bar btnbartitle btnbartitlenr" style="display: block;">
        <div>
            当前位置：<span><a title="系统首页" href="/zdyindex.aspx">系统首页</a></span><span> &gt; </span>
            <span>系统设置</span> &gt; <span>基础设置</span>
        </div>
    </div>
    <div class="memr" style="height: auto;">
        <div class="mrNav">
            <a class="active">基础参数设置</a><a href="/RMBase/SysHotel/SheSiList.aspx">客房设施设置</a><a
                href="Tips.aspx">提示显示设置</a><a href="/RMBase/SysParameter/HotelInformation.aspx">酒店信息</a>
        </div>
    </div>
    <div class="pd20 paymentsettings" style="padding-top: 0;">
        <div class="kefangsz">
            <div class="fsl">
                营业时间
            </div>
            <dl class="addevaluate kefangszb">
                <dd>
                    <small style="width: 84px;">酒店的营业时间</small>
                    <div style="float: left;">
                        <select id="BusinessHour" runat="server" style="width: 70px;">
                            <option value="0">00</option>
                            <option value="1">01</option>
                            <option value="2">02</option>
                            <option value="3">03</option>
                            <option value="4">04</option>
                            <option value="5">05</option>
                            <option value="6" selected="selected">06</option>
                            <option value="7">07</option>
                            <option value="8">08</option>
                        </select>：<select id="BusinessMinute" runat="server" style="width: 70px;">
                            <option value="0">00</option>
                            <option value="10">10</option>
                            <option value="20">20</option>
                            <option value="30">30</option>
                            <option value="40">40</option>
                            <option value="50">50</option>
                        </select>
                    </div>
                    <div style="float: left; color: #999;">
                        设置酒店营业时间，如：当天的凌晨6:00到次日的凌晨6:00，那么营业时间设置为6:00
                    </div>
                </dd>
            </dl>
        </div>
        <div class="kefangsz" style="display: none;">
            <div class="kefangszt">
                设定酒店营业时间，如：当天的凌晨6:00到次日的凌晨6:00，那么营业时间设置为6:00
            </div>
            <dl class="addevaluate kefangszb" style="width: 100%; margin-bottom: 24px;">
                <dd>
                    <small>酒店客房预订最长时间</small>
                    <div>
                        <select>
                            <option value="1">1</option>
                        </select>
                        <span>：</span>
                        <select>
                            <option value="1">1</option>
                        </select>
                    </div>
                </dd>
            </dl>
        </div>
        <div class="kefangsz">
            <%--
            <div class="kefangszt">
                设置客房预订时间和价格展示形式
            </div>--%>
            <div class="fsl">
                客房展示
            </div>
            <dl class="addevaluate kefangszb">
                <dd>
                    <small>酒店客房预订最长时间</small>
                    <div>
                        时长
                        <select id="num" runat="server">
                            <option value="1">1</option>
                            <option value="2">2</option>
                            <option value="3">3</option>
                            <option value="4">4</option>
                            <option value="5">5</option>
                            <option value="6">6</option>
                        </select>
                        个月
                    </div>
                </dd>
                <dd>
                    <small>前台价格列表展示形式</small>
                    <div class="radio" id="iszs" runat="server">
                        <label class="checked" value='1'>
                            展开</label>
                        <label value='0'>
                            收起</label>
                    </div>
                </dd>
                <dd style="display: none;">
                    <small>前台客房售价展示方式</small>
                    <div class="radio" id="Div3" runat="server">
                        <label class="checked" value='1'>
                            按售房规则</label>
                        <label value='0'>
                            按会员等级</label>
                    </div>
                    <div id="MoreClear" class="guige" style="width: 236px; margin-left: 135px; display: block;">
                    </div>
                    <div style="margin-left: 110px; display: block;">
                        <small>前台价格显示类型</small>
                        <div class="radio clearfix">
                            <label value="0">
                                门市价</label><label value="1">最低价</label>
                        </div>
                    </div>
                </dd>
                <dd>
                    <small>是否展示规则名称</small>
                    <div class="radio" id="isRuleName" runat="server">
                        <label value='1'>
                            是</label>
                        <label class="checked" value='0'>
                            否</label>
                    </div>
                </dd>
                <dd>
                    <small>是否展示其他会员价格</small>
                    <div class="radio" id="dVip" runat="server">
                        <label class="checked" value='1'>
                            是</label>
                        <label value='0'>
                            否</label>
                    </div>
                </dd>
                <dd>
                    <small>前台价格显示类型</small>
                    <div class="radio" id="showtypejg" runat="server">
                        <label class="checked" value='1'>
                            门市价</label>
                        <label value='0'>
                            最低价</label>
                    </div>
                </dd>
                <dd>
                    <small>前台客房列表展示形式</small>
                    <div class="radio" id="is_ShowType" runat="server">
                        <label class="checked" value='1'>
                            缩略图</label>
                        <label value='0'>
                            大图</label>
                    </div>
                </dd>
            </dl>
        </div>
        <div class="kefangsz">
            <%--<div class="kefangszt" style="padding-top: 20px;">
                设置退房/续住提醒时间及退房到时时间
            </div>--%>
            <div class="fsl">
                客房续住
            </div>
            <dl class="addevaluate kefangszb">
                <dd>
                    <small style="width: 72px;">续住提醒时间</small>
                    <div>
                        <asp:DropDownList ID="ddlCheckOutRemind" runat="server" Style="width: 70px">
                            <asp:ListItem Value="08">08</asp:ListItem>
                            <asp:ListItem Value="09" Selected="True">09</asp:ListItem>
                            <asp:ListItem Value="10">10</asp:ListItem>
                            <asp:ListItem Value="11">11</asp:ListItem>
                            <asp:ListItem Value="12">12</asp:ListItem>
                        </asp:DropDownList>
                        ：<asp:DropDownList ID="ddlCheckOutReminds" runat="server" Style="width: 70px">
                            <asp:ListItem Value="00">00</asp:ListItem>
                            <asp:ListItem Value="15">15</asp:ListItem>
                            <asp:ListItem Value="30">30</asp:ListItem>
                            <asp:ListItem Value="45">45</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </dd>
                <dd>
                    <small style="width: 72px;">退房提醒时间</small>
                    <div>
                        <asp:DropDownList ID="ddlHours" runat="server" Style="width: 70px">
                            <asp:ListItem Value="12">12</asp:ListItem>
                            <asp:ListItem Value="13">13</asp:ListItem>
                            <asp:ListItem Value="14">14</asp:ListItem>
                            <asp:ListItem Value="15">15</asp:ListItem>
                            <asp:ListItem Value="16">16</asp:ListItem>
                            <asp:ListItem Value="17">17</asp:ListItem>
                            <asp:ListItem Value="18">18</asp:ListItem>
                        </asp:DropDownList>
                        ：<asp:DropDownList ID="ddlMinutes" runat="server" Style="width: 70px">
                            <asp:ListItem Value="00">00</asp:ListItem>
                            <asp:ListItem Value="15">15</asp:ListItem>
                            <asp:ListItem Value="30">30</asp:ListItem>
                            <asp:ListItem Value="45">45</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </dd>
            </dl>
        </div>
        <div runat="server" id="dDeposit" class="kefangsz" style="display: none;">
            <div class="fsl">
                押金金额
            </div>
            <dl class="addevaluate kefangszb">
                <dd>
                    <small>客人交押金金额</small>
                    <div>
                        <input runat="server" type="text" id="CashPledgeMoney" style="width: 60px;" />元
                    </div>
                </dd>
                <dd>
                    <small>押金退回方式</small>
                    <div id="Div4" class="radio">
                        <label class="checked" value="1">
                            原路退回</label>
                        <label value="0">
                            退回至会员卡</label>
                    </div>
                </dd>
            </dl>
        </div>
        <div class="kefangsz" style="display: none;">
            <div class="kefangszt" style="padding-top: 20px;">
                设置预售券专区的列表页面展示形式
            </div>
            <dl class="addevaluate kefangszb">
                <dd>
                    <small>限时活动列表展示形式</small>
                    <div id="Div1" class="radio">
                        <label class="checked" value="1">
                            1行排列1个产品</label>
                        <label value="0">
                            1行排列2个产品</label>
                    </div>
                </dd>
                <dd>
                    <small class="lh14">是否以弹出窗口形式显 示热门预售券</small>
                    <div id="Div2" class="radio">
                        <label class="checked" value="1">
                            是</label>
                        <label value="0">
                            否</label>
                    </div>
                </dd>
            </dl>
        </div>
        <a class="btn" onclick="Submit()" style="margin-top: 15px; margin-left: 110px;">保存</a>
    </div>
    <script type="text/javascript">

        $('.radio').on('click', 'label', function () {
            $(this).addClass('checked').siblings().removeClass('checked');
        });
        bind();
        function Submit() {
            var num = $("#num").val();
            var iszs = $("#iszs").find(".checked").attr("value");
            var showtypejg = $("#showtypejg").find(".checked").attr("value");
            var is_vip = $("#dVip").find(".checked").attr("value");
            var is_ShowType = $("#is_ShowType").find(".checked").attr("value");
            var isRuleName = $("#isRuleName").find(".checked").attr("value");

            var BusinessHour = $("#BusinessHour").val();
            var BusinessMinute = $("#BusinessMinute").val();
            var CashPledgeMoney = $("#CashPledgeMoney").val();

            var is_CheckOutRemind = $("#ddlCheckOutRemind").val() + ":" + $("#ddlCheckOutReminds").val();
            var is_CheckOutTime = $("#ddlHours").val() + ":" + $("#ddlMinutes").val();

            var url = "kfsz.ashx?action=update";
            url += "&num=" + num;
            url += "&iszs=" + iszs;
            url += "&showtypejg=" + showtypejg;
            url += "&is_vip=" + is_vip;
            url += "&is_ShowType=" + is_ShowType;
            url += "&is_CheckOutRemind=" + is_CheckOutRemind;
            url += "&is_CheckOutTime=" + is_CheckOutTime;
            url += "&isRuleName=" + isRuleName;
            url += "&BusinessHour=" + BusinessHour;
            url += "&BusinessMinute=" + BusinessMinute;
            url += "&CashPledgeMoney=" + CashPledgeMoney;

            $.post(url, function (data) {
                if (data == "ok") {
                    showTipsMsg("保存成功！", 2000, 4);
                    //window.location.reload();
                }
                else {
                    showTipsMsg("保存失败！", 2000, 4);
                }
            });

            //alert(num + "," + iszs + "," + showtypejg);
        }

        function bind() {

            var url = "kfsz.ashx?action=getinfo";
            $.post(url, function (data) {
                var json = eval("(" + data + ")");
                var i = 0;
                $("#num").val(json.Moday[i].NUM);
                //$("#iszs").find("label").each(function () { if ($(this).attr("value") * 1 == json.Moday[i].IS_OPEN) $(this).addClass('checked').siblings().removeClass('checked'); });
                //$("#showtypejg").find("label").each(function () { if ($(this).attr("value") * 1 == json.Moday[i].IS_PRICE) $(this).addClass('checked').siblings().removeClass('checked'); });

                $("#iszs").find("label[value=" + json.Moday[i].IS_OPEN + "]").addClass('checked').siblings().removeClass('checked');
                $("#showtypejg").find("label[value=" + json.Moday[i].IS_PRICE + "]").addClass('checked').siblings().removeClass('checked');
                $("#dVip").find("label[value=" + json.Moday[i].IS_VIPPRICE + "]").addClass('checked').siblings().removeClass('checked');
                $("#is_ShowType").find("label[value=" + json.Moday[i].IS_SHOWTYPE + "]").addClass('checked').siblings().removeClass('checked');
                $("#isRuleName").find("label[value=" + json.Moday[i].ISRULENAME + "]").addClass('checked').siblings().removeClass('checked');


                $("#num").val(json.Moday[i].NUM);
                $("#num").val(json.Moday[i].NUM);
                $("#num").val(json.Moday[i].NUM);
            });
        }
    </script>
    </form>
</body>
</html>
