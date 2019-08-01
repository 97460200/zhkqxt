<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="points.aspx.cs" Inherits="RM.Web.SysSetBase.points.points" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>积分设置</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/SysSetBase/css/IE.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script src="../../Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="../../Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <style type="text/css">
        .poCfgList th:nth-child(1)
        {
            width: 40px;
        }
        .poCfgList th:nth-child(2)
        {
            width: 175px;
            text-align: left;
            position: relative;
            left: 15px;
        }
        .poCfgList th
        {
            width: 120px;
            color: #666;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <input runat="server" id="hdAdminHotelid" />
    <div class="tools_bar btnbartitle btnbartitlenr" style="display: block;">
        <div>
            当前位置：<span><a title="系统首页" href="/Frame/HomeIndex.aspx">系统首页</a></span><span> &gt;
            </span><span>系统设置</span> &gt; <span>积分设置</span>
        </div>
    </div>
    <div class="points">
        <div class="poConfig clearfix">
            <span>会员中心积分功能</span>
            <input type="radio" id="op1" value="1" name="open" checked /><label for="op1">开启</label>
            <input type="radio" id="op2" value="0" name="open" /><label for="op2">关闭</label>
        </div>
        <%--<div class="poConfig clearfix">
            <span>设置积分获得规则</span>
        </div>--%>
        <ul class="poCfgList" style="width: 960px; border-bottom: 1px solid #e3e3e3; margin-bottom: 20px;">
            <div class="fsl" style="margin-top: 10px;">
                积分规则
            </div>
            <li>
                <div class="upgrdtable" style="width: auto;">
                    <table class="ul" id="tab2" style="width: auto; float: left;">
                    </table>
                </div>
            </li>
            <%--<li  name="isEnble" id="isEnble"><i class="icon-radio6"></i><span>微网注册</span><span>注册会员获得</span><input
                type="text" value="0" id="jfzhi" /><small>分</small></li>
            <li name="isEnble2" id="isEnble2"><i class="icon-radio6"></i><span>客房点评</span><span>每点评1条获得</span><input type="text"
                value="0" id="jfzhi2" /><small>分</small></li>
            <li name="isEnble1" id="isEnble1"><i class="icon-radio6"></i><span>消费积分</span><span>每消费1元获得</span><input type="text"
                value="0" id="jfzhi1" /><small>分</small></li>--%>
        </ul>
        <div class="fsl" style="margin-top: 5px;">
            兑换规则
        </div>
        <div style="padding-bottom: 20px;">
            <span>每人每天可兑房数&nbsp;<input type="text" value="1" id="jfDayNumber" maxlength="2" style="width: 60px;" />
                <span style="font-size: 8px; color: #666;">&nbsp; 0为不限制,够积分就可兑换</span> </span>
        </div>
        <div class="poBtn" style="width: 960px;">
            <a class="button buttonActive" onclick="Submits()">保存</a>
        </div>
    </div>
    </form>
    <script src="/SysSetBase/css/jquery.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            Keypress("jfDayNumber");
            getDayNumber();
        });

        function getDayNumber() {
            $.post("points.ashx?action=getDayNumber&AdminHotelid=" + $("#hdAdminHotelid").val(), function (data) {
                $("#jfDayNumber").val(data);
            });
        }
        function setDayNumber() {
            $.post("points.ashx?action=setDayNumber&AdminHotelid=" + $("#hdAdminHotelid").val() + "&DayNumber=" + $("#jfDayNumber").val(), function (data) {
            });
        }
        ////////////////////////////////////////////////////////////////////////
        binds();
        function binds() {
            $("#tab2").html("");
            var url = "points.ashx?action=gethtmls";
            $.post(url, function (data) {
                $("#tab2").html(data);
                get();
            });
        }


        function Submits() {
            setDayNumber();
            var qy = new Array();
            var i = 0;
            var jfzhi = 0; var jfzhi1 = 0; var jfzhi2 = 0;
            var jfzhi3 = 0;
            $(".hylxnames").each(function () {
                var jb = $(this).attr("jb");
                qy[i] = jb;

                $(".jfzhi").each(function () { if ($(this).attr("jb") == jb) qy[i] += "," + ($(this).val() == "" ? "0" : $(this).val()); $(this).parent().parent().hasClass('active') ? jfzhi = 1 : jfzhi = 0; });
                $(".jfzhi1").each(function () { if ($(this).attr("jb") == jb) qy[i] += "," + ($(this).val() == "" ? "0" : $(this).val()); $(this).parent().parent().hasClass('active') ? jfzhi1 = 1 : jfzhi1 = 0; });
                $(".jfzhi2").each(function () { if ($(this).attr("jb") == jb) qy[i] += "," + ($(this).val() == "" ? "0" : $(this).val()); $(this).parent().parent().hasClass('active') ? jfzhi2 = 1 : jfzhi2 = 0; });
                $(".jfzhi3").each(function () { if ($(this).attr("jb") == jb) qy[i] += "," + ($(this).val() == "" ? "0" : $(this).val()); $(this).parent().parent().hasClass('active') ? jfzhi3 = 1 : jfzhi3 = 0; });

                i++;

            });

            var url = "points.ashx?action=updates"; var values = "";
            for (var j = 0; j < qy.length; j++) { values += qy[j] + "|"; }
            values = values.substring(0, values.length - 1);
            url += "&values=" + values;
            url += "&isEnble=" + jfzhi;
            url += "&isEnble1=" + jfzhi1;
            url += "&isEnble2=" + jfzhi2;
            url += "&isEnble3=" + jfzhi3;
            var isjf = $("input[name='open']:checked").val();
            url += "&isjf=" + isjf;

            $.post(url, function (data) {
                showTipsMsg("保存成功！", 2000, 4);
            });
        }


        function get() {
            var url = "points.ashx?action=getinfos";
            $.post(url, function (data) {
                var json = eval("(" + data + ")");

                json.jfmatter[0].ISJF == "1" ? $("input[name='open']").eq(0).attr("checked", "checked") : $("input[name='open']").eq(1).attr("checked", "");
                for (var i = 0; i < json.jfmatter.length; i++) {
                    $(".jfzhi").each(function () { if ($(this).attr("jb") == json.jfmatter[i].JB) $(this).val(json.jfmatter[i].JFZHI); json.jfmatter[i].ISENBLE == 1 ? $(this).parent().parent().addClass('active') : $(this).parent().parent().removeClass('active'); });
                    $(".jfzhi1").each(function () { if ($(this).attr("jb") == json.jfmatter[i].JB) $(this).val(json.jfmatter[i].JFZHI1); json.jfmatter[i].ISENBLE1 == 1 ? $(this).parent().parent().addClass('active') : $(this).parent().parent().removeClass('active'); });
                    $(".jfzhi2").each(function () { if ($(this).attr("jb") == json.jfmatter[i].JB) $(this).val(json.jfmatter[i].JFZHI2); json.jfmatter[i].ISENBLE2 == 1 ? $(this).parent().parent().addClass('active') : $(this).parent().parent().removeClass('active'); });
                    $(".jfzhi3").each(function () { if ($(this).attr("jb") == json.jfmatter[i].JB) $(this).val(json.jfmatter[i].JFZHI3); json.jfmatter[i].ISENBLE3 == 1 ? $(this).parent().parent().addClass('active') : $(this).parent().parent().removeClass('active'); });
                }

            });

        }



        var selected = function (Selector) {
            var htr = function () {
                $(this).parents('tr').hasClass('active') ? $(Selector).find('tr').removeClass('active') : $(Selector).find('tr').addClass('active');
            };
            var btr = function () {
                $(this).parents('tr').hasClass('active') ? $(this).parents('tr').removeClass('active') : $(this).parents('tr').addClass('active');
                isCheckAll() ? $(Selector).find('thead tr').removeClass('active') : $(Selector).find('thead tr').addClass('active');
            };
            var isCheckAll = function () {
                var otr = $(Selector).find('tbody tr');
                for (var i = 0; i < otr.length; i++) {
                    if (!otr.eq(i).hasClass('active')) return true;
                }
                return false;
            };
            $(Selector).on('click', 'thead .icon-radio6', htr);
            $(Selector).on('click', 'tbody .icon-radio6', btr);
        };

        selected('.upgrdtable');
    </script>
</body>
</html>
