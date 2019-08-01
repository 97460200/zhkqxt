<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FinancialReport.aspx.cs"
    Inherits="RM.Web.SysSetBase.statement.FinancialReport" %>

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
        .addsearch dd div label .time_type
        {
            color: #999;
            margin-left: 10px;
            font-size: 12px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <input runat="server" type="hidden" id="hdAdminHotelId" value="" />
    <input runat="server" type="hidden" id="hdHotelId" value="0" />
    <input runat="server" type="hidden" id="hdPaySource" value="0" />
    <input runat="server" type="hidden" id="hdTransaction_Type" value="0" />
    <input runat="server" type="hidden" id="hdDateType" value="0" />
    <input runat="server" type="hidden" id="hdTime" value="0" />
    <input runat="server" type="hidden" id="hdReportType" value="1" />
    <input runat="server" type="hidden" id="hdExport" value="0" />
    <input runat="server" type="hidden" id="hdBusinessTime" value="00:00" />
    <div class="shareframe">
        <div class="shareframel" style="width: 300px;">
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
                    <small>收款场景</small>
                    <div class="radio" id="dTransaction_Type">
                        <label val="0">
                            全部</label>
                        <label val="JSAPI">
                            在线支付</label>
                        <label val="MICROPAY">
                            扫码支付</label>
                    </div>
                </dd>
                <dd>
                    <small>时间类型</small>
                    <div class="radio" id="dDateType">
                        <label val="0">
                            系统时间<span class="time_type">当日00:00-次日00:00</span></label>
                        <label val="1">
                            营业时间<span runat="server" id="spBusinessTime" class="time_type">当日06:00-次日06:00</span></label>
                    </div>
                </dd>
                <dd>
                    <small>统计方式</small>
                    <div class="radio" id="dTime">
                        <label val="0">
                            按日</label>
                        <label val="1">
                            自定义</label>
                    </div>
                </dd>
                <dd class="dTime time0" style="display: none;">
                    <small>时间</small>
                    <div class="sharedate01">
                        <input type="text" id="txtDate" runat="server" class="Wdate" onfocus="WdatePicker({dateFmt: 'yyyy-MM-dd'})"
                            placeholder="">
                    </div>
                </dd>
                <dd class="dTime time1" style="display: none;">
                    <small>开始时间</small>
                    <div class="sharedate01">
                        <input type="text" id="txtStartDate" runat="server" placeholder="">
                    </div>
                </dd>
                <dd class="dTime time1" style="display: none;">
                    <small>结束时间</small>
                    <div class="sharedate01">
                        <input type="text" id="txtEndDate" runat="server" placeholder="">
                    </div>
                </dd>
                <dd>
                    <small>报表类型</small>
                    <div class="radio" id="dReportType">
                        <label val="1">
                            汇总表</label>
                        <label val="2">
                            明细表</label>
                        <label val="3">
                            日统计表</label>
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
                <span runat="server" id="spTitle">财务对账日统计表</span>
            </div>
            <div class="hzbb" id="Div5" style="font-size: 14px; color: #666; margin-top: 10px;">
                统计时间：<span runat="server" id="spDate">2019-10-01</span>
            </div>
            <%--财务对账日统计表--%>
            <div class="bbgl" id="hdReport3" style="display: none;">
                <div class="d2 clearfix">
                    <div class="d21" style="font-size: 12px; color: #333;">
                        <%= ddlHotel.SelectedItem.Text %>
                    </div>
                    <div class="d22">
                        <span id="Span1">制表时间：<span runat="server" id="spSysTime1">2019-05-20 12:00</span></span><span
                            onclick="daochu()" class="dc">导出</span>
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
                                        账单日期
                                    </th>
                                    <th>
                                        收入
                                    </th>
                                    <th>
                                        收入笔数
                                    </th>
                                    <th>
                                        支出
                                    </th>
                                    <th>
                                        支出笔数
                                    </th>
                                    <th>
                                        收支金额
                                    </th>
                                    <th>
                                        手续费
                                    </th>
                                    <th>
                                        日终净额
                                    </th>
                                </tr>
                            </thead>
                            <tbody id="Tbody2" class="lasttr">
                                <asp:Repeater runat="server" ID="rptData3">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <%# Container.ItemIndex+1 %>
                                            </td>
                                            <td>
                                                <%= ddlHotel.SelectedItem.Text %>
                                            </td>
                                            <td>
                                                <%# Eval("Bill_Date")%>
                                            </td>
                                            <td>
                                                <%# Eval("Income_Money")%>
                                            </td>
                                            <td>
                                                <%# Eval("Income_Number")%>
                                            </td>
                                            <td>
                                                <%# Eval("Refund_Money")%>
                                            </td>
                                            <td>
                                                <%# Eval("Refund_Number")%>
                                            </td>
                                            <td>
                                                <%# Eval("Total_Money")%>
                                            </td>
                                            <td>
                                                <%# Eval("Service_Charge")%>
                                            </td>
                                            <td>
                                                <%# Eval("DayNetAmount")%>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <%--财务对账明细表--%>
            <div class="bbgl" id="hdReport2" style="display: none;">
                <div class="d2 clearfix">
                    <div class="d21" style="font-size: 12px; color: #333;">
                        <%= ddlHotel.SelectedItem.Text %>
                    </div>
                    <div class="d22">
                        <span id="Span2">制表时间：<span runat="server" id="spSysTime2">2019-05-20 12:00</span></span><span
                            onclick="daochu()" class="dc">导出</span>
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
                                        资金渠道
                                    </th>
                                    <th>
                                        收款场景
                                    </th>
                                    <th>
                                        交易时间
                                    </th>
                                    <th>
                                        商户订单号
                                    </th>
                                    <th>
                                        交易状态
                                    </th>
                                    <th>
                                        收款金额
                                    </th>
                                    <th>
                                        商户退款单号
                                    </th>
                                    <th>
                                        退款金额
                                    </th>
                                    <th>
                                        手续费
                                    </th>
                                </tr>
                            </thead>
                            <tbody id="Tbody1" class="lasttr">
                                <asp:Repeater runat="server" ID="rptData2">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <%# Container.ItemIndex+1 %>
                                            </td>
                                            <td>
                                                <%# Eval("PaySource")%>
                                            </td>
                                            <td>
                                                <%# Eval("Transaction_Type")%>
                                            </td>
                                            <td>
                                                <%# Eval("Transaction_Time")%>
                                            </td>
                                            <td>
                                                <%# Eval("Order_Numbe")%>
                                            </td>
                                            <td>
                                                <%# Eval("Transaction_State")%>
                                            </td>
                                            <td>
                                                <%# Eval("Total_Money")%>
                                            </td>
                                            <td>
                                                <%# Eval("Refund_Numbe")%>
                                            </td>
                                            <td>
                                                <%# Eval("Refund_Money")%>
                                            </td>
                                            <td>
                                                <%# Eval("Service_Charge")%>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <%--财务对账汇总表--%>
            <div class="bbgl cwdzhzb" id="hdReport1" style="display: none;">
                <div class="d2 clearfix">
                    <div class="d21" style="font-size: 12px; color: #333;">
                        <%= ddlHotel.SelectedItem.Text %>
                    </div>
                    <div class="d22">
                        <span id="Span3">制表时间：<span runat="server" id="spSysTime3">2019-05-20 12:00</span></span><span
                            onclick="daochu()" class="dc">导出</span>
                    </div>
                </div>
                <div class="rge">
                    <div class="rgetable">
                        <table class="ul">
                            <thead>
                                <tr>
                                    <th>
                                        资金渠道/收款场景
                                    </th>
                                    <th>
                                        收入
                                    </th>
                                    <th>
                                        收入笔数
                                    </th>
                                    <th>
                                        支出
                                    </th>
                                    <th>
                                        支出笔数
                                    </th>
                                    <th>
                                        收支金额
                                    </th>
                                    <th>
                                        手续费
                                    </th>
                                    <th>
                                        期末净额
                                    </th>
                                </tr>
                            </thead>
                            <tbody id="tbList1" class="lasttr">
                                <asp:Repeater runat="server" ID="rptData1">
                                    <ItemTemplate>
                                        <tr class="row_<%# Eval("SerialNumber")%>" pay_source="<%# Eval("PaySource")%>" transaction_type="<%# Eval("Transaction_Type")%>">
                                            <td>
                                                <%# Eval("Pay_Type")%>
                                            </td>
                                            <td>
                                                <%# Eval("Income_Money")%>
                                            </td>
                                            <td>
                                                <%# Eval("Income_Number")%>
                                            </td>
                                            <td>
                                                <%# Eval("Refund_Money")%>
                                            </td>
                                            <td>
                                                <%# Eval("Refund_Number")%>
                                            </td>
                                            <td>
                                                <%# Eval("Total_Money")%>
                                            </td>
                                            <td>
                                                <%# Eval("Service_Charge")%>
                                            </td>
                                            <td>
                                                <%# Eval("DayNetAmount")%>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>
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
            //收款场景
            $("#dTransaction_Type").on("click", "label", function () {
                $(this).addClass("checked").siblings().removeClass("checked");
                $("#hdTransaction_Type").val($(this).attr("val"));
            });
            //时间类型
            $("#dDateType").on("click", "label", function () {
                $(this).addClass("checked").siblings().removeClass("checked");
                var dt = $(this).attr("val");
                if ($("#hdDateType").val() != dt) {
                    $(".time1 input").val("");
                }
                $("#hdDateType").val(dt);
            });
            //时间类型
            $("#dTime").on("click", "label", function () {
                $(this).addClass("checked").siblings().removeClass("checked");
                var t_val = $(this).attr("val");
                $(".dTime").hide();
                $(".time" + t_val).show();
                $("#hdTime").val(t_val);
            });
            //报表类型
            $("#dReportType").on("click", "label", function () {
                $(this).addClass("checked").siblings().removeClass("checked");
                $("#hdReportType").val($(this).attr("val"));
            });

            $(".time1").on("focus", "input", function () {
                var t_val = $("#dDateType .checked").attr("val");
                var hm = "00:00";
                if (t_val == "1") {
                    hm = $("#hdBusinessTime").val();
                }
                WdatePicker({ dateFmt: 'yyyy-MM-dd ' + hm })
            });
            bindVal();
        });

        function bindVal() {
            var ps = $("#hdPaySource").val();
            $("#dPaySource label[val=" + ps + "]").addClass("checked");

            var tt = $("#hdTransaction_Type").val();
            $("#dTransaction_Type label[val=" + tt + "]").addClass("checked");

            var dt = $("#hdDateType").val();
            $("#dDateType label[val=" + dt + "]").addClass("checked");

            var t_val = $("#hdTime").val();
            $("#dTime label[val=" + t_val + "]").addClass("checked");
            $(".time" + t_val).show();

            var rt = $("#hdReportType").val();
            $("#dReportType label[val=" + rt + "]").addClass("checked");
            $("#hdReport" + rt).show();
            if (rt == "1") {
                if (ps == "1") {
                    $("#tbList1 tr[pay_source=2]").hide();
                } else if (ps == "2") {
                    $("#tbList1 tr[pay_source=1]").hide();
                }
                if (tt == "JSAPI") {
                    $("#tbList1 tr[transaction_type='MICROPAY']").hide();
                } else if (tt == "MICROPAY") {
                    $("#tbList1 tr[transaction_type='JSAPI']").hide();
                }
            }
        }

        function QueryData() {

            var rt = $("#hdTime").val();
            if (rt == "0") {
                if ($("#txtDate").val() == "") {
                    showTipsMsg('请选择日期！', '5000', '5');
                    return false;
                }
            }
            if (rt == "1") {
                if ($("#txtStartDate").val() == "") {
                    showTipsMsg('请选择开始日期！', '5000', '5');
                    return false;
                }
                if ($("#txtEndDate").val() == "") {
                    showTipsMsg('请选择结束日期！', '5000', '5');
                    return false;
                }
            }
            $("#hdExport").val("0");
            __doPostBack('lbSumit', '');
        }

        function daochu() {
            $("#hdExport").val("1");
            __doPostBack('lbSumit', '');
        }

    </script>
</body>
</html>
