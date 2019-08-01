<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="houseRuleAdd.aspx.cs" Inherits="RM.Web.SysSetBase.houseState.houseRuleAdd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>添加售房规则</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer" />
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/Validator/JValidator.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <style type="text/css">
        .adifoli dd div select, .adifoli dd div input
        {
            width: 36px;
            margin: 0 6px;
        }
        .adifoli dd.lj div label
        {
            color:#999;
            }
        .adifoli dd.lj div label.checked
        {
            color:#333;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <input runat="server" id="hdHotelId" type="hidden" />
    <input runat="server" id="hdId" type="hidden" />
    <input runat="server" id="Room_ID" type="hidden" />
    <input runat="server" id="Sales_Type" type="hidden" />
    <input runat="server" id="Sales_Val" type="hidden" />
    <input runat="server" id="Breakfast_Type" type="hidden" />
    <input runat="server" id="Breakfast_Val" type="hidden" />
    <input runat="server" id="Pay_Type" type="hidden" />
    <input runat="server" id="Vip_Type" type="hidden" />
    <input runat="server" id="Vip_Val" type="hidden" />
    <input runat="server" id="hdRoomIds" type="hidden" />
    <input runat="server" id="DiscountType" type="hidden" value="0" />
    <dl class="adifoli" style="width: 470px;height:320px;">
        <dd>
            <small>规则名称</small>
            <div>
                <input runat="server" type="text" id="Rule_Name" style="width: 240px; margin: 0" />
            </div>
        </dd>
        <dd id="roomtype">
            <small>房型</small>
            <div class="checkbox">
                <asp:Repeater ID="rptRoom" runat="server">
                    <ItemTemplate>
                        <label hid="<%# Eval("id") %>">
                            <%# Eval("name")%></label>
                    </ItemTemplate>
                </asp:Repeater>
                <label runat="server" id="lbEdit" edit="true" class="checked">
                </label>
                <%--<span class="addh">添加房型</span>--%>
            </div>
        </dd>
        <dd id="RuleSales" style=" display:none">
            <small>销售规则</small>
            <div class="radio">
                <label hid="0" class="checked">
                    无</label>
                <label hid="1">
                    提前<input id="advance_day" type="text" />天预订</label>
                <label hid="2">
                    连住<input id="many_day" type="text" />晚以上</label>
            </div>
        </dd>
        <dd id="breakfast">
            <small>早餐份数</small>
            <div class="radio">
                <label hid="0" class="checked">
                    无早</label>
                <label hid="1">
                    单早</label>
                <label hid="2">
                    双早</label>
                <label hid="3" >
                    其他<input type="text" id="breakfastNum" />份</label>
            </div>
        </dd>
        <dd id="paytype" style=" display:none">
            <small>付款方式</small>
            <div class="checkbox">
                <label class="checked" hid="4">
                    微信支付</label><label hid="1">到店支付</label><label hid="3">会员卡支付</label><label hid="2">积分兑换</label>
            </div>
        </dd>
        <dd style="display:none;">
            <small>立减</small>
            <div>
                <%--<input runat="server" type="text" id="Discount" style="width: 240px; margin: 0"  />--%>
                <%--<div class="radio">
                
                <label hid="1" class="checked">
                    每订单<input type="text" id="Discount" />份</label>
                    <label hid="2" >
                    每间夜<input type="text" id="jyDiscount" />份</label>
            </div>--%>
            </div>
        </dd>
        <dd class="lj" style="display:none;" >
            <small>立减</small>
            <div class="radio" id="ljradio">
                  <label class="checked" value="1">
                        每间夜减<input name="jyDiscount" type="text" id="jyDiscount" runat="server"  maxlength="5" onkeyup="value=value.replace(/[^\d.]/g,'')" autocomplete="off" style="width:50px;">元</label>
                    <label value="0">
                        每订单减<input name="Discount" type="text" id="Discount" runat="server" maxlength="5" onkeyup="value=value.replace(/[^\d.]/g,'')" autocomplete="off"  style="width:50px;">元</label>
            </div>
        </dd>
        <dd id="viptype" style=" display:none">
            <small>适用于</small>
            <div class="checkbox">
                <asp:Repeater runat="server" ID="rptVip">
                    <ItemTemplate>
                        <label hid="<%# Eval("code") %>" dataval="<%# Eval("LevelName")%>" class="checked">
                            <%# Eval("LevelName")%></label>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </dd>
        <dd>
            <small>内容</small>
            <div>
                <textarea runat="server" id="Remarks" cols="30" rows="10"></textarea>
            </div>
        </dd>
    </dl>
    <div class="adifoliBtn">
        <div style="float:right;">
            <asp:LinkButton ID="Save" runat="server" OnClientClick="return CheckValid();" OnClick="Save_Click">
                <span class="bbgg bbgg1">保存</span>
            </asp:LinkButton>
            <a class="bbgg" href="javascript:void(0)" onclick="OpenClose();"><span>关闭</span></a>
        </div>
    </div>
    </form>
    <script type="text/javascript">
        $(function () {
            if ($("#Sales_Type").val() != "") {//销售规则
                $("#RuleSales label").removeClass("checked");
                $("#RuleSales label[hid='" + $("#Sales_Type").val() + "']").addClass("checked").find("input").val($("#Sales_Val").val());
            }

            if ($("#Breakfast_Type").val() != "") {//早餐份数
                $("#breakfast label").removeClass("checked");
                $("#breakfast label[hid='" + $("#Breakfast_Type").val() + "']").addClass("checked").find("input").val($("#Breakfast_Val").val());
            }

            if ($("#Pay_Type").val() != "") {//付款方式
                $("#paytype label").removeClass("checked");
                var pays = $("#Pay_Type").val().split(",");
                for (var i = 0; i < pays.length; i++) {
                    $("#paytype label[hid='" + pays[i] + "']").addClass("checked");
                }
            }

            if ($("#Vip_Type").val() != "") {//适用于对象
                $("#viptype label").removeClass("checked");
                var vips = $("#Vip_Type").val().split(",");
                for (var i = 0; i < vips.length; i++) {
                    $("#viptype label[hid='" + vips[i] + "']").addClass("checked");
                }
            }
        });
        $('.radio').on('click', 'label', function () {
            $(this).addClass('checked').siblings().removeClass('checked');
            $("#DiscountType").val($("#ljradio").find('.checked').attr('value'));
        });

        $("#ljradio").find('label').each(function () {
            if ($(this).attr('value') == $("#DiscountType").val()) {
                $(this).addClass('checked').siblings().removeClass('checked');
            }
        });

        $('.checkbox').on('click', 'label', function () {
            if (!$(this).attr("edit")) {
                $(this).hasClass('checked') ? $(this).removeClass('checked') : $(this).addClass('checked');
            }
        });

        function CheckValid() {
            if ($.trim($("#Rule_Name").val()) == "") {
                $("#Rule_Name").focus();
                showTipsMsg('规则名称不能为空！', '5000', '4');
                return false;
            }
            //////
            var roomlistid = "";
            var num = $("#roomtype").find(".checked").length;
            if (num) {
                for (var i = 0; i < num; i++) {
                    roomlistid += $("#roomtype").find(".checked").eq(i).attr("hid") + ",";
                }
            } else {
                showTipsMsg('请选择房型！', '5000', '4');
                return false;
            }
            roomlistid = roomlistid.substring(0, roomlistid.length - 1);
            $("#hdRoomIds").val(roomlistid); //房型
            //////
            var RuleType = $("#RuleSales").find(".checked").attr("hid"); //销售规则
            var RuleSales = "";
            if (RuleType == "1") {
                RuleSales = $("#advance_day").val();
            } else if (RuleType == "2") {
                RuleSales = $("#many_day").val();
            }
            if (RuleType != "0" && RuleSales == "") {
                showTipsMsg('请填写销售规则！', '5000', '4');
                return false;
            }
            $("#Sales_Type").val(RuleType); //销售规则
            $("#Sales_Val").val(RuleSales); //销售规则val
            //////
            var breakfast_type = $("#breakfast").find(".checked").attr("hid"); //早餐
            var breakfast = "";
            if (breakfast_type == "3") {
                breakfast = $("#breakfastNum").val();
                if (breakfast == "") {
                    showTipsMsg('请填写早餐数量！', '5000', '4');
                    return false;
                }
            }
            $("#Breakfast_Type").val(breakfast_type); //早餐
            $("#Breakfast_Val").val(breakfast); //早餐数量

            //////
            var paylist = "";
            var pay = $("#paytype").find(".checked").length;
            if (pay > 0) {
                for (var i = 0; i < pay; i++) {
                    paylist += $("#paytype").find(".checked").eq(i).attr("hid") + ",";
                }
            } else {
                showTipsMsg('请选择支付方式！', '5000', '4');
                return false;
            }
            paylist = paylist.substring(0, paylist.length - 1);
            $("#Pay_Type").val(paylist); //支付方式
            //////
            debugger;
            var viplist = "";
            var vipvals = "";
            var vips = $("#viptype .checked").length;
//            if (vips > 0) {
//                for (var i = 0; i < vips; i++) {
//                    viplist += $("#viptype .checked").eq(i).attr("hid") + ",";
//                    vipvals += $("#viptype .checked").eq(i).attr("dataval") + "、";
//                }
//            } else {
//                showTipsMsg('请选择适用级别！', '5000', '4');
//                return false;
//            }
            viplist = viplist.substring(0, viplist.length - 1);
            vipvals = vipvals.substring(0, vipvals.length - 1);
            $("#Vip_Type").val(viplist); //支付方式
            $("#Vip_Val").val(vipvals); //支付方式
        }
    </script>
</body>
</html>
