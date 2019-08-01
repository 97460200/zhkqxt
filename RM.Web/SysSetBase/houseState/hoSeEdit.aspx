<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="hoSeEdit.aspx.cs" Inherits="RM.Web.SysSetBase.houseState.hoSeEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/Validator/JValidator.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <input runat="server" type="hidden" id="hdAdminHotelId" value="1" />
    <input runat="server" type="hidden" id="hdHotelId" value="-1" />
    <input runat="server" type="hidden" id="hdRoomId" value="" />
    <input runat="server" type="hidden" id="hdRuleId" value="" />
    <input runat="server" type="hidden" id="hdVipCode" value="" />
    <input runat="server" type="hidden" id="hdJFState" value="1" />
    <input runat="server" type="hidden" id="hdWeek" value="" />
    <dl class="addhousetypeInfo addsystemuser" style="width: 600px; height: 440px">
        <dd>
            <div runat="server" id="dTitle">
            </div>
        </dd>
        <dd>
            <small>售房规则</small>
            <div id="dRule" class="radio">
                <%--<label class="checked">
                    有早</label>
                <label>
                    无早</label>--%>
            </div>
        </dd>
        <dd>
            <small>日期</small>
            <div class="date">
                <input type="text" runat="server" id="txtStartTime" />
                <input type="text" runat="server" id="txtEndTime" />
            </div>
        </dd>
        <dd>
            <small>有效日期</small>
            <div id="dWeekDay" class="checkbox">
                <label wk="Monday">
                    周一</label>
                <label wk="Tuesday">
                    周二</label>
                <label wk="Wednesday">
                    周三</label>
                <label wk="Thursday">
                    周四</label>
                <label wk="Friday">
                    周五</label>
                <label wk="Saturday">
                    周六</label>
                <label wk="Sunday">
                    周日</label>
            </div>
        </dd>
        <dd>
            <small>房态</small>
            <div id="dStart" class="radio">
                <label dataid="1" class="checked">
                    开启</label>
                <label dataid="0">
                    关闭</label>
            </div>
        </dd>
        <dd>
            <small>可订房</small>
            <div>
                <input type="text" id="txtNumber" style="margin-left: 6px; width: 60px;" />间 当前<span
                    class="last"><em id="eOrderNumber">0</em>/<em id="eNumber">0</em></span>
            </div>
        </dd>
        <dd>
            <small>标签</small>
            <div id="dLabel" class="checkbox photo">
                <label dataid="1" re>
                    热门</label>
                <label dataid="2" te>
                    特价</label>
                <label dataid="3" cu>
                    促销</label>
            </div>
        </dd>
        <dd>
            <small>会员等级</small>
            <div class="table">
                <%=vipHtml %>
            </div>
        </dd>
        <dd>
            <small>客房价格</small>
            <div class="table">
                <%=jgHtml%>
            </div>
        </dd>
        <dd>
            <small>所需积分</small>
            <div class="table">
                <%=jfHtml%>
            </div>
        </dd>
    </dl>
    <div class="frmbottom" style="display: none;">
        <a href="javascript:void(0)" onclick="saveRoomPrice();"><span class="bbgg bbgg1">保存</span>
        </a><a class="bbgg" href="javascript:void(0)" onclick="OpenClose();"><span>关闭</span></a>
    </div>
    <div class="adifoliBtn">
        <div style="float:right;">
            <a href="javascript:void(0)" onclick="saveRoomPrice();"><span class="bbgg bbgg1">保存</span>
            <a class="bbgg" href="javascript:void(0)" onclick="OpenClose();"><span>关闭</span></a>
            </a>
        </div>
    </div>
    </form>
    <script type="text/javascript">
        $(function () {
            var wk = $("#hdWeek").val();
            if (wk != "") {
                $("#dWeekDay label[wk='" + wk + "']").addClass('checked'); ;
            }
            GetEditRule();
            GetEditPrice();
        });

        //售房规则 房态
        $('#dRule,#dStart').on('click', 'label', function () {
            $(this).addClass('checked').siblings().removeClass('checked');
        });
        //有效日期、 标签
        $('#dWeekDay,#dLabel').on('click', 'label', function () {
            $(this).hasClass('checked') ? $(this).removeClass('checked') : $(this).addClass('checked');
        });

        function GetEditRule() {
            $.post("../Ajax/SysAjax.ashx", {
                Menu: "GetEditRule",
                HotelId: $("#hdHotelId").val(),
                RoomId: $("#hdRoomId").val(),
                RuleId: $("#hdRuleId").val()
            }, function (data) {
                $("#dRule").html(data);
            });
        }

        function GetEditPrice() {
            $.ajax({
                url: "../Ajax/SysAjax.ashx",
                data: {
                    Menu: "GetEditPrice",
                    HotelId: $("#hdHotelId").val(),
                    RoomId: $("#hdRoomId").val(),
                    RuleId: $("#hdRuleId").val(),
                    VipCode: $("#hdVipCode").val(),
                    DateRange: $("#txtStartTime").val()
                },
                type: "GET",
                dataType: "JSON",
                async: false,
                success: function (data) {
                    if (data != null && data.code == "1") {
                        $("#dStart label[dataid='" + data.Room_State + "']").addClass('checked').siblings().removeClass('checked');
                        $("#txtNumber").val(data.Number);
                        $("#eNumber").html(data.Number);
                        $("#dLabel label[dataid='" + data.Room_Label + "']").addClass('checked');
                        $(".table input[valtype='price']").val(data.Price);
                        $(".table input[valtype='integral']").val(data.Integral);
                        var rls = data.Room_Label.split(",");
                        for (var i = 0; i < rls.length; i++) {
                            $("#dLabel label[dataid='" + rls[i] + "']").addClass('checked');
                        }
                    }
                }
            });
        }

        function saveRoomPrice() {
            var weeks = [];
            $("#dWeekDay label.checked").each(function () {
                weeks.push($(this).attr("wk"));
            });
            if (weeks.length < 1) {
                showTipsMsg('请选择有效日期！', '5000', '5');
                return;
            }
            var labels = [];
            $("#dLabel label.checked").each(function () {
                labels.push($(this).attr("dataid"));
            });
            var number = $("#txtNumber").val();
            var price = $(".table input[valtype='price']").val();
            var integral = $(".table input[valtype='integral']").val();

            $.ajax({
                url: "../Ajax/SysAjax.ashx",
                data: {
                    Menu: "SaveRoomPrice",
                    HotelId: $("#hdHotelId").val(),
                    RoomId: $("#hdRoomId").val(),
                    RuleId: $("#dRule label.checked").attr("dataid"),
                    RoomState: $("#dStart label.checked").attr("dataid"),
                    VipCode: $("#hdVipCode").val(),
                    StartTime: $("#txtStartTime").val(),
                    EndTime: $("#txtEndTime").val(),
                    Weeks: weeks.toString(),
                    Number: number,
                    Labels: labels.toString(),
                    Price: price,
                    Integral: integral
                },
                type: "GET",
                dataType: "text",
                async: false,
                success: function (data) {
                    if (data == "1") {
                        showTipsMsg('操作成功！', '5000', '4');
                        OpenClose();
                    } else {
                        showTipsMsg('操作失败！', '5000', '5');
                        return;
                    }
                }
            });
        }
    </script>
</body>
</html>
