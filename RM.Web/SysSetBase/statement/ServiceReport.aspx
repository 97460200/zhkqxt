<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ServiceReport.aspx.cs"
    Inherits="RM.Web.SysSetBase.statement.ServiceReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>财务报表</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script src="/WDatePicker/WdatePicker.js" type="text/javascript"></script>
    <style type="text/css">
        .rge .rgetable td
        {
            background-color: #fff;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <input runat="server" type="hidden" id="hdAdminHotelId" value="" />
    <input runat="server" type="hidden" id="hdHotelId" value="0" />
    <input runat="server" type="hidden" id="hdPaySource" value="0" />
    <input runat="server" type="hidden" id="hdReportType" value="0" />
    <div class="shareframe">
        <div class="shareframel">
            <dl class="addsearch">
                <dd>
                    <small>酒店名称</small>
                    <div>
                        <asp:DropDownList runat="server" ID="ddlHotel">
                        </asp:DropDownList>
                    </div>
                </dd>
                <dd>
                    <small>资金渠道</small>
                    <div class="radio" id="dPaySource">
                        <label val="0">
                            全部</label>
                        <label val="1">
                            微信</label>
                        <label val="2">
                            支付宝</label>
                    </div>
                </dd>
                <dd>
                    <small>统计方式</small>
                    <div class="radio" id="dReportType">
                        <label val="0">
                            全部</label>
                        <label val="1">
                            按日</label>
                        <label val="2">
                            按月</label>
                        <label val="3">
                            按年</label>
                        <label val="4">
                            自定义</label>
                    </div>
                </dd>
                <dd class="ReportType drType1" style="display: none;">
                    <small>选择日期</small>
                    <div class="sharedate01">
                        <input type="text" id="txtDate" runat="server" class="Wdate" onfocus="WdatePicker({dateFmt: 'yyyy-MM-dd'})"
                            placeholder="" />
                    </div>
                </dd>
                <dd class="ReportType drType2" style="display: none;">
                    <small>选择日期</small>
                    <div class="sharedate01">
                        <input type="text" id="txtStartMonth" runat="server" class="Wdate" onfocus="WdatePicker({dateFmt: 'yyyy-MM'})"
                            placeholder="" />
                    </div>
                </dd>
                <dd class="ReportType drType3" style="display: none;">
                    <small>选择年份</small>
                    <div class="sharedate01">
                        <asp:DropDownList runat="server" ID="ddlYear">
                        </asp:DropDownList>
                    </div>
                </dd>
                <dd class="ReportType drType4" style="display: none;">
                    <small>开始日期</small>
                    <div class="sharedate01">
                        <input type="text" id="txtStartDate" runat="server" class="Wdate" onfocus="WdatePicker({dateFmt: 'yyyy-MM-dd'})"
                            placeholder="">
                    </div>
                </dd>
                <dd class="ReportType drType4" style="display: none;">
                    <small>结束日期</small>
                    <div class="sharedate01">
                        <input type="text" id="txtEndDate" runat="server" class="Wdate" onfocus="WdatePicker({dateFmt: 'yyyy-MM-dd'})"
                            placeholder="">
                    </div>
                </dd>
                <div class="sharesearchbtn">
                    <input type="submit" name="btnSumit" value="查询" onclick="QueryData();return false;" />
                    <asp:LinkButton ID="lbSumit" runat="server" OnClick="lbSumit_Click" Style="display: none;">查询</asp:LinkButton>
                </div>
            </dl>
        </div>
        <div class="zhankai">
        </div>
        <div class="shareframer">
            <div class="hzbb" id="bbmc" style="border-bottom: none; margin-bottom: 0; padding-bottom: 0;">
                服务商业务报表
            </div>
            <div class="hzbb" id="Div5" style="font-size: 14px; color: #666; margin-top: 10px;">
                统计时间：<span runat="server" id="spReportDate">2018年</span>
            </div>
            <%--服务商业务报表--%>
            <div class="bbgl" style="" id="kftjbb">
                <div class="d2 clearfix">
                    <div class="d21" style="font-size: 12px; color: #333;">
                        智订云演示酒店
                    </div>
                    <div class="d22">
                        <span id="Span1">制表时间：<span runat="server" id="spSysTime">2016-04-28 13:00</span></span><span
                            class="dc">导出</span>
                    </div>
                </div>
                <div class="rge">
                    <div class="rgetable">
                        <table class="ul">
                            <thead>
                                <tr>
                                    <th width="40">
                                        序号
                                    </th>
                                    <th>
                                        酒店名称
                                    </th>
                                    <th>
                                        资金渠道
                                    </th>
                                    <asp:Repeater runat="server" ID="rptTitle">
                                        <ItemTemplate>
                                            <th>
                                                <%# Eval("Name") %>
                                            </th>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tr>
                            </thead>
                            <tbody runat="server" id="tbData" class="lasttr lasttd">
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>
    <script type="text/javascript">
        jQuery.fn.rowspan = function (colIdx) { //封装的一个JQuery小插件
            return this.each(function () {
                var that;
                $('tr', this).each(function (row) {
                    $('td:eq(' + colIdx + ')', this).filter(':visible').each(function (col) {
                        if (that != null && $(this).html() == $(that).html()) {
                            rowspan = $(that).attr("rowSpan");
                            if (rowspan == undefined) {
                                $(that).attr("rowSpan", 1);
                                rowspan = $(that).attr("rowSpan");
                            }
                            rowspan = Number(rowspan) + 1;
                            $(that).attr("rowSpan", rowspan);
                            $(this).hide();
                        } else {
                            that = this;
                        }
                    });
                });
            });
        }
    </script>
    <script type="text/javascript">
        $(function () {
            //展开收起
            $('.zhankai').on('click', function () {
                $(this).hasClass('act') ? $(this).removeClass('act') : $(this).addClass('act');
                $(".shareframel").toggle();
            });
            //资金渠道
            $("#dPaySource").on("click", "label", function () {
                $(this).addClass("checked").siblings().removeClass("checked");
                $("#hdPaySource").val($(this).attr("val"));
            });
            //报表类型
            $("#dReportType").on("click", "label", function () {
                $(this).addClass("checked").siblings().removeClass("checked");
                $("#hdReportType").val($(this).attr("val"));
                $(".ReportType").hide();
                $(".drType" + $(this).attr("val")).show();
            });
            bindVal();
            $("#tbData").rowspan(1); //传入的参数是对应的列数从0开始
        });

        function bindVal() {
            var ps = $("#hdPaySource").val();
            $("#dPaySource label[val=" + ps + "]").addClass("checked");

            var rt = $("#hdReportType").val();
            $("#dReportType label[val=" + rt + "]").addClass("checked");
            $(".drType" + rt).show();
        }


        function QueryData() {

            var rt = $("#hdReportType").val();
            if (rt == "1") {
                if ($("#txtDate").val() == "") {
                    showTipsMsg('请选择日期！', '5000', '5');
                    return false;
                }
            } else if (rt == "2") {
                if ($("#txtStartMonth").val() == "") {
                    showTipsMsg('请选择月份！', '5000', '5');
                    return false;
                }
            } else if (rt == "3") {

            } else if (rt == "4") {
                if ($("#txtStartDate").val() == "") {
                    showTipsMsg('请选择开始日期！', '5000', '5');
                    return false;
                }
                if ($("#txtEndDate").val() == "") {
                    showTipsMsg('请选择结束日期！', '5000', '5');
                    return false;
                }
                var ds = daysBetween($("#txtStartDate").val(), $("#txtEndDate").val());
                if (ds > 31) {
                    showTipsMsg('日期跨度不能超过一个月！', '5000', '5');
                    return false;
                }
            }
            __doPostBack('lbSumit', '');
        }

        function daysBetween(sDate1, sDate2) {
            //Date.parse() 解析一个日期时间字符串，并返回1970/1/1 午夜距离该日期时间的毫秒数
            var time1 = Date.parse(new Date(sDate1));
            var time2 = Date.parse(new Date(sDate2));
            var nDays = Math.abs(parseInt((time2 - time1) / 1000 / 3600 / 24));
            return nDays;
        };

    </script>
</body>
</html>
