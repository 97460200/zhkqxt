<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="hotelsetting.aspx.cs" Inherits="RM.Web.SysSetBase.superAdmin.hotelsetting" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>酒店管理</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/PqGrid/jquery-ui.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/PqGrid/pqgrid.min.js" type="text/javascript"></script>
    <link href="/Themes/Scripts/PqGrid/pqgrid.min.css" rel="stylesheet" />
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <input runat="server" id="AdminHotelid" type="hidden" value="0" />
    <div class="hotelsetting" style="overflow: auto;">
        <div class="sharetabs" style="display: none">
            <ul class="clearfix">
                <li class="act"><a href="###">营销设置</a> </li>
                <li><a href="###">微信设置</a> </li>
                <li><a href="###">短信设置</a> </li>
                <li><a href="###">功能设置</a> </li>
                <li><a href="###">系统对接</a> </li>
            </ul>
        </div>
        <dl class="addevaluate yingxiaoshezhi" style="height: 800px; overflow: initial;">
            <div class="hotelname">
                营销系统维护费
            </div>
            <dd>
                <small>维护费</small>
                <div class="radio" id="Maintain" runat="server">
                    <label class="checked" value='1'>
                        固定<input type="text" id="MaintainMoney" maxlength="5" onkeyup="value=value.replace(/[^\d.]/g,'')"
                            runat="server" />元</label>
                    <label value='0'>
                        比例<input type="text" id="MaintainProportion" maxlength="5" onkeyup="value=value.replace(/[^\d.]/g,'')"
                            runat="server" />%</label>
                    <div>
                        <a onclick="SetMaintainMoney()" class="btn" style="width: 90px; max-width: 90px;">三级维护费</a>
                    </div>
                </div>
            </dd>
            <dd>
                <small>商城维护费</small>
                <div class="radio" id="MallMaintain" runat="server">
                    <label class="checked" value='1'>
                        固定<input type="text" id="MallMaintainMoney" maxlength="5" onkeyup="value=value.replace(/[^\d.]/g,'')"
                            runat="server" />元</label>
                    <label value='0'>
                        比例<input type="text" id="MallMaintainProportion" maxlength="5" onkeyup="value=value.replace(/[^\d.]/g,'')"
                            runat="server" />%</label>
                </div>
            </dd>
            <div class="hotelname" style="border-top: 1px solid #f3f3f3; padding-top: 20px;">
                设置酒店营销奖金
            </div>
            <dd>
                <small>拉新奖金</small>
                <div class="radio">
                    <label class="checked" value='1'>
                        固定<input type="text" id="RegisterMoney" maxlength="5" onkeyup="value=value.replace(/[^\d.]/g,'')"
                            runat="server" />元</label>
                </div>
            </dd>
            <dd>
                <small>售房奖金</small>
                <div class="radio" id="CheckIn" runat="server">
                    <span style="float: left; margin-right: 10px;">客人首次扫码预订</span>
                    <label class="checked" value='1'>
                        固定<input type="text" id="CheckInMoney" maxlength="5" onkeyup="value=value.replace(/[^\d.]/g,'')"
                            runat="server" />元</label>
                    <label value='0'>
                        比例<input type="text" id="CheckInProportion" maxlength="5" onkeyup="value=value.replace(/[^\d.]/g,'')"
                            runat="server" />%</label>
                    <div>
                        <a onclick="SetRoomMoney()" class="btn" style="width: 90px; max-width: 90px;">设置房型奖金</a>
                    </div>
                </div>
            </dd>
            <dd>
                <small>&nbsp;</small>
                <div class="radio" id="CheckInTwo" runat="server">
                    <span style="float: left; margin-right: 10px;">第二次及以后预订</span>
                    <label class="checked" value='1'>
                        固定<input type="text" id="CheckInTwoMoney" maxlength="5" onkeyup="value=value.replace(/[^\d.]/g,'')"
                            runat="server" />元</label>
                    <label value='0'>
                        比例<input type="text" id="CheckInTwoProportion" maxlength="5" onkeyup="value=value.replace(/[^\d.]/g,'')"
                            runat="server" />%</label>
                </div>
            </dd>
            <dd>
                <small>&nbsp;</small>
                <div class="radio" id="CheckTwoStandard" runat="server">
                    <span style="float: left; margin-right: 10px;">二次预订判别标准</span>
                    <label class="checked" value='1'>
                        按订单</label>
                    <label value='0'>
                        按间夜</label>
                </div>
            </dd>
            <dd>
                <small>充值奖金</small>
                <div class="radio" id="Recharge" runat="server">
                    <label class="checked" value='1'>
                        固定<input type="text" id="RechargeMoney" maxlength="5" onkeyup="value=value.replace(/[^\d.]/g,'')"
                            runat="server" />元</label>
                    <label value='0'>
                        比例<input type="text" id="RechargeProportion" maxlength="5" onkeyup="value=value.replace(/[^\d.]/g,'')"
                            runat="server" />%</label>
                </div>
            </dd>
            <dd>
                <small>升级奖金</small>
                <div class="radio" id="Upgrade" runat="server">
                    <label class="checked" value='1'>
                        固定<input type="text" id="UpgradeMoney" maxlength="5" onkeyup="value=value.replace(/[^\d.]/g,'')"
                            runat="server" />元</label>
                    <label value='0'>
                        比例<input type="text" id="UpgradeProportion" maxlength="5" onkeyup="value=value.replace(/[^\d.]/g,'')"
                            runat="server" />%</label>
                </div>
            </dd>
            <div>
                <dd>
                    <small>点餐奖金</small>
                    <div class="radio" id="StaffFood" runat="server" style="float: left">
                        <span style="float: left; margin-right: 10px;">客人扫员工推广码点餐</span>
                        <label class="checked" value="1">
                            固定<input name="StaffFoodMoney" type="text" id="StaffFoodMoney" maxlength="5" onkeyup="value=value.replace(/[^\d.]/g,'')">元</label>
                        <label value="0">
                            比例<input name="StaffFoodProportion" type="text" id="StaffFoodProportion" maxlength="5"
                                onkeyup="value=value.replace(/[^\d.]/g,'')">%</label>
                    </div>
                    <div class="checkbox" style="float: left" id="StaffMaintain" runat="server">
                        <label class="checked" value="1">
                            收取维护费</label>
                    </div>
                </dd>
                <dd style="display: none">
                    <small>&nbsp;</small>
                    <div class="radio" style="float: left;" id="TableFood" runat="server">
                        <span style="float: left; margin-right: 10px;">客人扫酒店餐桌码点餐</span>
                        <label class="checked" value="1">
                            固定<input name="TableFoodMoney" type="text" id="TableFoodMoney" maxlength="5" onkeyup="value=value.replace(/[^\d.]/g,'')">元</label>
                        <label value="0">
                            比例<input name="TableFoodProportion" type="text" id="TableFoodProportion" maxlength="5"
                                onkeyup="value=value.replace(/[^\d.]/g,'')">%</label>
                    </div>
                    <div class="checkbox" style="float: left" id="TableMaintain" runat="server">
                        <label class="checked" value="1">
                            收取维护费</label>
                    </div>
                    <div class="radio" style="float: left;" id="TableWinning" runat="server">
                        <span style="float: left; margin-right: 10px;">获奖人</span>
                        <label class="checked" value="1">
                            推广员工获得奖金</label>
                        <label value="0">
                            餐厅员工获得奖金</label>
                    </div>
                </dd>
                <dd style="display: none">
                    <small>&nbsp;</small>
                    <div class="radio" style="float: left;" id="RoomFood" runat="server">
                        <span style="float: left; margin-right: 10px;">客人扫酒店客房码点餐</span>
                        <label class="checked" value="1">
                            固定<input name="RoomFoodMoney" type="text" id="RoomFoodMoney" maxlength="5" onkeyup="value=value.replace(/[^\d.]/g,'')">元</label>
                        <label value="0">
                            比例<input name="RoomFoodProportion" type="text" id="RoomFoodProportion" maxlength="5"
                                onkeyup="value=value.replace(/[^\d.]/g,'')">%</label>
                    </div>
                    <div class="checkbox" style="float: left" id="RoomMaintain" runat="server">
                        <label class="checked" value="1">
                            收取维护费</label>
                    </div>
                    <div class="radio" style="float: left;" id="RoomWinning" runat="server">
                        <span style="float: left; margin-right: 10px;">获奖人</span>
                        <label class="checked" value="1">
                            推广员工获得奖金</label>
                        <label value="0">
                            楼层员工获得奖金</label>
                    </div>
                </dd>
                <dd style="display: none">
                    <small>&nbsp;</small>
                    <div class="radio" id="GuestFood" runat="server" style="float: left">
                        <span style="float: left; margin-right: 22px;">酒店员工为客人点餐</span>
                        <label class="checked" value="1">
                            固定<input name="GuestFoodMoney" type="text" id="GuestFoodMoney" maxlength="5" onkeyup="value=value.replace(/[^\d.]/g,'')">元</label>
                        <label value="0">
                            比例<input name="GuestFoodProportion" type="text" id="GuestFoodProportion" maxlength="5"
                                onkeyup="value=value.replace(/[^\d.]/g,'')">%</label>
                    </div>
                    <div class="checkbox" style="float: left" id="GuestMaintain" runat="server">
                        <label class="checked" value="1">
                            收取维护费</label>
                    </div>
                </dd>
            </div>
            <dd>
                <small>微商城奖金</small>
                <div class="radio" id="MallProduct" runat="server">
                    <label class="checked" value='1'>
                        固定<input type="text" id="MallProductMoney" maxlength="5" onkeyup="value=value.replace(/[^\d.]/g,'')"
                            runat="server" />元</label>
                    <label value='0'>
                        比例<input type="text" id="MallProductProportion" maxlength="5" onkeyup="value=value.replace(/[^\d.]/g,'')"
                            runat="server" />%</label>
                    <div>
                        <a onclick="SetProductMoney()" class="btn" style="width: 90px; max-width: 90px;">设置商品奖金</a>
                    </div>
                </div>
            </dd>
            <dd>
                <small class="lh14">营销费最低充值金额</small>
                <div class="radio">
                    <label class="checked" value='1'>
                        固定<input type="text" id="MinPayMoney" maxlength="5" onkeyup="value=value.replace(/[^\d.]/g,'')"
                            runat="server" value="100" />元</label>
                </div>
            </dd>
            <%--公共奖金--%>
            <div class="hotelname" style="border-top: 1px solid #f3f3f3; padding-top: 20px;">
                设置酒店营销公共奖金
            </div>
            <dd>
                <small>拉新奖金</small>
                <div class="radio">
                    <label class="checked" value='1'>
                        固定<input type="text" id="PublicRegisterMoney" maxlength="5" onkeyup="value=value.replace(/[^\d.]/g,'')"
                            runat="server" />元</label>
                </div>
            </dd>
            <dd>
                <small>售房奖金</small>
                <div class="radio" id="PublicCheckIn" runat="server">
                    <span style="float: left; margin-right: 10px;">客人首次扫码预订</span>
                    <label class="checked" value='1'>
                        固定<input type="text" id="PublicCheckInMoney" maxlength="5" onkeyup="value=value.replace(/[^\d.]/g,'')"
                            runat="server" />元</label>
                    <label value='0'>
                        比例<input type="text" id="PublicCheckInProportion" maxlength="5" onkeyup="value=value.replace(/[^\d.]/g,'')"
                            runat="server" />%</label>
                    <div>
                        <a onclick="PublicSetRoomMoney()" class="btn" style="width: 90px; max-width: 90px;">
                            设置房型奖金</a>
                    </div>
                </div>
            </dd>
            <dd>
                <small>&nbsp;</small>
                <div class="radio" id="PublicCheckInTwo" runat="server">
                    <span style="float: left; margin-right: 10px;">第二次及以后预订</span>
                    <label class="checked" value='1'>
                        固定<input type="text" id="PublicCheckInTwoMoney" maxlength="5" onkeyup="value=value.replace(/[^\d.]/g,'')"
                            runat="server" />元</label>
                    <label value='0'>
                        比例<input type="text" id="PublicCheckInTwoProportion" maxlength="5" onkeyup="value=value.replace(/[^\d.]/g,'')"
                            runat="server" />%</label>
                </div>
            </dd>
            <dd>
                <small>充值奖金</small>
                <div class="radio" id="PublicRecharge" runat="server">
                    <label class="checked" value='1'>
                        固定<input type="text" id="PublicRechargeMoney" maxlength="5" onkeyup="value=value.replace(/[^\d.]/g,'')"
                            runat="server" />元</label>
                    <label value='0'>
                        比例<input type="text" id="PublicRechargeProportion" maxlength="5" onkeyup="value=value.replace(/[^\d.]/g,'')"
                            runat="server" />%</label>
                </div>
            </dd>
            <dd>
                <small>升级奖金</small>
                <div class="radio" id="PublicUpgrade" runat="server">
                    <label class="checked" value='1'>
                        固定<input type="text" id="PublicUpgradeMoney" maxlength="5" onkeyup="value=value.replace(/[^\d.]/g,'')"
                            runat="server" />元</label>
                    <label value='0'>
                        比例<input type="text" id="PublicUpgradeProportion" maxlength="5" onkeyup="value=value.replace(/[^\d.]/g,'')"
                            runat="server" />%</label>
                </div>
            </dd>
            <div>
                <dd>
                    <small>点餐奖金</small>
                    <div class="radio" id="PublicStaffFood" runat="server" style="float: left">
                        <span style="float: left; margin-right: 10px;">客人扫员工推广码点餐</span>
                        <label class="checked" value="1">
                            固定<input name="PublicStaffFoodMoney" type="text" id="PublicStaffFoodMoney" maxlength="5"
                                onkeyup="value=value.replace(/[^\d.]/g,'')">元</label>
                        <label value="0">
                            比例<input name="PublicStaffFoodProportion" type="text" id="PublicStaffFoodProportion"
                                maxlength="5" onkeyup="value=value.replace(/[^\d.]/g,'')">%</label>
                    </div>
                </dd>
                <dd style="display: none">
                    <small>&nbsp;</small>
                    <div class="radio" style="float: left;" id="Div7" runat="server">
                        <span style="float: left; margin-right: 10px;">客人扫酒店餐桌码点餐</span>
                        <label class="checked" value="1">
                            固定<input name="TableFoodMoney" type="text" id="Text12" maxlength="5" onkeyup="value=value.replace(/[^\d.]/g,'')">元</label>
                        <label value="0">
                            比例<input name="TableFoodProportion" type="text" id="Text13" maxlength="5" onkeyup="value=value.replace(/[^\d.]/g,'')">%</label>
                    </div>
                    <div class="radio" style="float: left;" id="Div9" runat="server">
                        <span style="float: left; margin-right: 10px;">获奖人</span>
                        <label class="checked" value="1">
                            推广员工获得奖金</label>
                        <label value="0">
                            餐厅员工获得奖金</label>
                    </div>
                </dd>
                <dd style="display: none">
                    <small>&nbsp;</small>
                    <div class="radio" style="float: left;" id="Div10" runat="server">
                        <span style="float: left; margin-right: 10px;">客人扫酒店客房码点餐</span>
                        <label class="checked" value="1">
                            固定<input name="RoomFoodMoney" type="text" id="Text14" maxlength="5" onkeyup="value=value.replace(/[^\d.]/g,'')">元</label>
                        <label value="0">
                            比例<input name="RoomFoodProportion" type="text" id="Text15" maxlength="5" onkeyup="value=value.replace(/[^\d.]/g,'')">%</label>
                    </div>
                    <div class="radio" style="float: left;" id="Div12" runat="server">
                        <span style="float: left; margin-right: 10px;">获奖人</span>
                        <label class="checked" value="1">
                            推广员工获得奖金</label>
                        <label value="0">
                            楼层员工获得奖金</label>
                    </div>
                </dd>
                <dd style="display: none">
                    <small>&nbsp;</small>
                    <div class="radio" id="Div13" runat="server" style="float: left">
                        <span style="float: left; margin-right: 22px;">酒店员工为客人点餐</span>
                        <label class="checked" value="1">
                            固定<input name="GuestFoodMoney" type="text" id="Text16" maxlength="5" onkeyup="value=value.replace(/[^\d.]/g,'')">元</label>
                        <label value="0">
                            比例<input name="GuestFoodProportion" type="text" id="Text17" maxlength="5" onkeyup="value=value.replace(/[^\d.]/g,'')">%</label>
                    </div>
                </dd>
                <dd>
                    <small>提现人员</small>
                    <div>
                        <asp:DropDownList ID="DDLUserList" runat="server" Style="width: 300px;">
                        </asp:DropDownList>
                    </div>
                </dd>
            </div>
            <div class="membtn">
                <a class="button buttonActive" onclick="Submit()">保存</a>
            </div>
        </dl>
    </div>
    <script type="text/javascript">
        $('.radio').on('click', 'label', function () {
            $(this).addClass('checked').siblings().removeClass('checked');
        });

        $('.checkbox').on('click', 'label', function () {
            if ($(this).hasClass('checked') == true) {
                $(this).attr("value", "0");
                $(this).removeClass('checked');
            } else {
                $(this).attr("value", "1");
                $(this).addClass('checked')
            }
        });

        bind();
        function Submit() {
            var AdminHotelid = $("#AdminHotelid").val();
            var Maintain = $("#Maintain").find(".checked").attr("value");
            var MaintainMoney = $("#MaintainMoney").val();
            var MaintainProportion = $("#MaintainProportion").val();
            var RegisterMoney = $("#RegisterMoney").val();

            var CheckIn = $("#CheckIn").find(".checked").attr("value");
            var CheckInMoney = $("#CheckInMoney").val();
            var CheckInProportion = $("#CheckInProportion").val();


            var CheckTwoStandard = $("#CheckTwoStandard").find(".checked").attr("value");

            var CheckInTwo = $("#CheckInTwo").find(".checked").attr("value");
            var CheckInTwoMoney = $("#CheckInTwoMoney").val();
            var CheckInTwoProportion = $("#CheckInTwoProportion").val();

            var Recharge = $("#Recharge").find(".checked").attr("value");
            var RechargeMoney = $("#RechargeMoney").val();
            var RechargeProportion = $("#RechargeProportion").val();

            var Upgrade = $("#Upgrade").find(".checked").attr("value");
            var UpgradeMoney = $("#UpgradeMoney").val();
            var UpgradeProportion = $("#UpgradeProportion").val();


            //公共奖金
            var PublicRegisterMoney = $("#PublicRegisterMoney").val();

            var PublicCheckIn = $("#PublicCheckIn").find(".checked").attr("value");
            var PublicCheckInMoney = $("#PublicCheckInMoney").val();
            var PublicCheckInProportion = $("#PublicCheckInProportion").val();

            var PublicCheckInTwo = $("#PublicCheckInTwo").find(".checked").attr("value");
            var PublicCheckInTwoMoney = $("#PublicCheckInTwoMoney").val();
            var PublicCheckInTwoProportion = $("#PublicCheckInTwoProportion").val();

            var PublicRecharge = $("#PublicRecharge").find(".checked").attr("value");
            var PublicRechargeMoney = $("#PublicRechargeMoney").val();
            var PublicRechargeProportion = $("#PublicRechargeProportion").val();

            var PublicUpgrade = $("#PublicUpgrade").find(".checked").attr("value");
            var PublicUpgradeMoney = $("#PublicUpgradeMoney").val();
            var PublicUpgradeProportion = $("#PublicUpgradeProportion").val();

            var PublicStaffFood = $("#PublicStaffFood").find(".checked").attr("value");
            var PublicStaffFoodMoney = $("#PublicStaffFoodMoney").val();
            var PublicStaffFoodProportion = $("#PublicStaffFoodProportion").val();

            var PublicUser = $("#DDLUserList").val();
            //点餐参数


            var StaffFood = $("#StaffFood").find(".checked").attr("value");
            var StaffFoodMoney = $("#StaffFoodMoney").val();
            var StaffFoodProportion = $("#StaffFoodProportion").val();
            var StaffMaintain = $("#StaffMaintain").find("label").attr("value");

            var TableFood = $("#TableFood").find(".checked").attr("value");
            var TableFoodMoney = $("#TableFoodMoney").val();
            var TableFoodProportion = $("#TableFoodProportion").val();
            var TableMaintain = $("#TableMaintain").find("label").attr("value");
            var TableWinning = $("#TableWinning").find(".checked").attr("value");

            var RoomFood = $("#RoomFood").find(".checked").attr("value");
            var RoomFoodMoney = $("#RoomFoodMoney").val();
            var RoomFoodProportion = $("#RoomFoodProportion").val();
            var RoomMaintain = $("#RoomMaintain").find("label").attr("value");
            var RoomWinning = $("#RoomWinning").find(".checked").attr("value");

            var GuestFood = $("#GuestFood").find(".checked").attr("value");
            var GuestFoodMoney = $("#GuestFoodMoney").val();
            var GuestFoodProportion = $("#GuestFoodProportion").val();
            var GuestMaintain = $("#GuestMaintain").find("label").attr("value");


            //商城参数  
            var MallMaintain = $("#MallMaintain").find(".checked").attr("value");
            var MallMaintainMoney = $("#MallMaintainMoney").val();
            var MallMaintainProportion = $("#MallMaintainProportion").val();

            var MallProduct = $("#MallProduct").find(".checked").attr("value");
            var MallProductMoney = $("#MallProductMoney").val();
            var MallProductProportion = $("#MallProductProportion").val();


            var url = "MarketingConfigure.ashx?action=update";
            url = url + "&AdminHotelid=" + AdminHotelid;
            url = url + "&Maintain=" + Maintain;
            url = url + "&MaintainMoney=" + MaintainMoney;
            url = url + "&MaintainProportion=" + MaintainProportion;
            url = url + "&RegisterMoney=" + RegisterMoney;
            url = url + "&CheckIn=" + CheckIn;
            url = url + "&CheckInMoney=" + CheckInMoney;
            url = url + "&CheckInProportion=" + CheckInProportion;
            url = url + "&CheckTwoStandard=" + CheckTwoStandard;
            url = url + "&CheckInTwo=" + CheckInTwo;
            url = url + "&CheckInTwoMoney=" + CheckInTwoMoney;
            url = url + "&CheckInTwoProportion=" + CheckInTwoProportion;
            url = url + "&Recharge=" + Recharge;
            url = url + "&RechargeMoney=" + RechargeMoney;
            url = url + "&RechargeProportion=" + RechargeProportion;
            url = url + "&Upgrade=" + Upgrade;
            url = url + "&UpgradeMoney=" + UpgradeMoney;
            url = url + "&UpgradeProportion=" + UpgradeProportion;
            url = url + "&MinPayMoney=" + $("#MinPayMoney").val();

            //点餐参数
            url = url + "&StaffFood=" + StaffFood;
            url = url + "&StaffFoodMoney=" + StaffFoodMoney;
            url = url + "&StaffFoodProportion=" + StaffFoodProportion;
            url = url + "&StaffMaintain=" + StaffMaintain;
            url = url + "&StaffMaintainMoney=" + MaintainMoney;
            url = url + "&StaffMaintainProportion=" + MaintainProportion;
            url = url + "&TableFood=" + TableFood;
            url = url + "&TableFoodMoney=" + TableFoodMoney;
            url = url + "&TableFoodProportion=" + TableFoodProportion;
            url = url + "&TableMaintain=" + TableMaintain;
            url = url + "&TableWinning=" + TableWinning;
            url = url + "&RoomFood=" + RoomFood;
            url = url + "&RoomFoodMoney=" + RoomFoodMoney;
            url = url + "&RoomFoodProportion=" + RoomFoodProportion;
            url = url + "&RoomMaintain=" + RoomMaintain;
            url = url + "&RoomWinning=" + RoomWinning;
            url = url + "&GuestFood=" + GuestFood;
            url = url + "&GuestFoodMoney=" + GuestFoodMoney;
            url = url + "&GuestFoodProportion=" + GuestFoodProportion;
            url = url + "&GuestMaintain=" + GuestMaintain;




            //商城参数
            url = url + "&MallProduct=" + MallProduct;
            url = url + "&MallProductMoney=" + MallProductMoney;
            url = url + "&MallProductProportion=" + MallProductProportion;
            url = url + "&MallMaintain=" + MallMaintain;
            url = url + "&MallMaintainMoney=" + MallMaintainMoney;
            url = url + "&MallMaintainProportion=" + MallMaintainProportion;


            //公共奖金
            url = url + "&PublicRegisterMoney=" + PublicRegisterMoney;
            url = url + "&PublicCheckIn=" + PublicCheckIn;
            url = url + "&PublicCheckInMoney=" + PublicCheckInMoney;
            url = url + "&PublicCheckInProportion=" + PublicCheckInProportion;
            url = url + "&PublicCheckInTwo=" + PublicCheckInTwo;
            url = url + "&PublicCheckInTwoMoney=" + PublicCheckInTwoMoney;
            url = url + "&PublicCheckInTwoProportion=" + PublicCheckInTwoProportion;
            url = url + "&PublicRecharge=" + PublicRecharge;
            url = url + "&PublicRechargeMoney=" + PublicRechargeMoney;
            url = url + "&PublicRechargeProportion=" + PublicRechargeProportion;
            url = url + "&PublicUpgrade=" + PublicUpgrade;
            url = url + "&PublicUpgradeMoney=" + PublicUpgradeMoney;
            url = url + "&PublicUpgradeProportion=" + PublicUpgradeProportion;
            url = url + "&PublicStaffFood=" + PublicStaffFood;
            url = url + "&PublicStaffFoodMoney=" + PublicStaffFoodMoney;
            url = url + "&PublicStaffFoodProportion=" + PublicStaffFoodProportion;
            url = url + "&PublicUser=" + PublicUser;

            $.post(url, function (data) {
                if (data == "ok") {
                    showTipsMsg("保存成功！", 2000, 4);

                } else {
                    showTipsMsg("保存失败！", 2000, 4);
                }
            });
        }

        function bind() {
            var AdminHotelid = $("#AdminHotelid").val();
            var url = "MarketingConfigure.ashx?action=getinfo";
            url = url + "&AdminHotelid=" + AdminHotelid;
            $.post(url, function (data) {
                var json = eval("(" + data + ")");
                console.log(data)
                $("#Maintain").find("label").each(function () { if ($(this).attr("value") * 1 == json.MarketingConfigure[0].MAINTAIN) $(this).addClass('checked').siblings().removeClass('checked'); });
                $("#MaintainMoney").val(json.MarketingConfigure[0].MAINTAINMONEY);
                $("#MaintainProportion").val(json.MarketingConfigure[0].MAINTAINPROPORTION);
                $("#RegisterMoney").val(json.MarketingConfigure[0].REGISTERMONEY);

                $("#CheckTwoStandard").find("label[value='" + json.MarketingConfigure[0].CHECKTWOSTANDARD + "']").addClass('checked').siblings().removeClass('checked');

                $("#CheckIn").find("label").each(function () { if ($(this).attr("value") * 1 == json.MarketingConfigure[0].CHECKIN) $(this).addClass('checked').siblings().removeClass('checked'); });
                $("#CheckInMoney").val(json.MarketingConfigure[0].CHECKINMONEY);
                $("#CheckInProportion").val(json.MarketingConfigure[0].CHECKINPROPORTION);

                $("#CheckInTwo").find("label").each(function () { if ($(this).attr("value") * 1 == json.MarketingConfigure[0].CHECKINTWO) $(this).addClass('checked').siblings().removeClass('checked'); });
                $("#CheckInTwoMoney").val(json.MarketingConfigure[0].CHECKINTWOMONEY);
                $("#CheckInTwoProportion").val(json.MarketingConfigure[0].CHECKINTWOPROPORTION);

                $("#Recharge").find("label").each(function () { if ($(this).attr("value") * 1 == json.MarketingConfigure[0].RECHARGE) $(this).addClass('checked').siblings().removeClass('checked'); });
                $("#RechargeMoney").val(json.MarketingConfigure[0].RECHARGEMONEY);
                $("#RechargeProportion").val(json.MarketingConfigure[0].RECHARGEPROPORTION);

                $("#Upgrade").find("label").each(function () { if ($(this).attr("value") * 1 == json.MarketingConfigure[0].UPGRADE) $(this).addClass('checked').siblings().removeClass('checked'); });
                $("#UpgradeMoney").val(json.MarketingConfigure[0].UPGRADEMONEY);
                $("#UpgradeProportion").val(json.MarketingConfigure[0].UPGRADEPROPORTION);
                $("#MinPayMoney").val(json.MarketingConfigure[0].MINPAYMONEY);




            });


            var urls = "MarketingConfigure.ashx?action=getfoodinfo";
            urls = urls + "&AdminHotelid=" + AdminHotelid;
            $.post(urls, function (datas) {
                var json = eval("(" + datas + ")");
                console.log(datas)

                //客人扫员工推广码点餐   
                $("#StaffFood").find("label").each(function () { if ($(this).attr("value") * 1 == json.FoodMarketingConfigure[0].STAFFFOOD) $(this).addClass('checked').siblings().removeClass('checked'); });
                $("#StaffFoodMoney").val(json.FoodMarketingConfigure[0].STAFFFOODMONEY);
                $("#StaffFoodProportion").val(json.FoodMarketingConfigure[0].STAFFFOODPROPORTION);


                if (json.FoodMarketingConfigure[0].STAFFMAINTAIN == "1") {
                    $("#StaffMaintain").find("label").addClass('checked');
                    $("#StaffMaintain").find("label").attr("value", "1");
                }
                else {
                    $("#StaffMaintain").find("label").removeClass('checked');
                    $("#StaffMaintain").find("label").attr("value", "0");
                }

                //客人扫酒店餐桌码点餐
                $("#TableFood").find("label").each(function () { if ($(this).attr("value") * 1 == json.FoodMarketingConfigure[0].TABLEFOOD) $(this).addClass('checked').siblings().removeClass('checked'); });
                $("#TableFoodMoney").val(json.FoodMarketingConfigure[0].TABLEFOODMONEY);
                $("#TableFoodProportion").val(json.FoodMarketingConfigure[0].TABLEFOODPROPORTION);
                if (json.FoodMarketingConfigure[0].TABLEMAINTAIN == "1") {
                    $("#TableMaintain").find("label").addClass('checked');
                    $("#TableMaintain").find("label").attr("value", "1");
                }
                else {
                    $("#TableMaintain").find("label").removeClass('checked');
                    $("#TableMaintain").find("label").attr("value", "0");
                }
                $("#TableWinning").find("label").each(function () { if ($(this).attr("value") * 1 == json.FoodMarketingConfigure[0].TABLEWINNING) $(this).addClass('checked').siblings().removeClass('checked'); });

                //客人扫酒店客房码点餐
                $("#RoomFood").find("label").each(function () { if ($(this).attr("value") * 1 == json.FoodMarketingConfigure[0].ROOMFOOD) $(this).addClass('checked').siblings().removeClass('checked'); });
                $("#RoomFoodMoney").val(json.FoodMarketingConfigure[0].ROOMFOODMONEY);
                $("#RoomFoodProportion").val(json.FoodMarketingConfigure[0].ROOMFOODPROPORTION);
                if (json.FoodMarketingConfigure[0].ROOMMAINTAIN == "1") {
                    $("#RoomMaintain").find("label").addClass('checked');
                    $("#RoomMaintain").find("label").attr("value", "1");
                }
                else {
                    $("#RoomMaintain").find("label").removeClass('checked');
                    $("#RoomMaintain").find("label").attr("value", "0");
                }
                $("#RoomWinning").find("label").each(function () { if ($(this).attr("value") * 1 == json.FoodMarketingConfigure[0].ROOMWINNING) $(this).addClass('checked').siblings().removeClass('checked'); });

                //酒店员工为客人点餐
                $("#GuestFood").find("label").each(function () { if ($(this).attr("value") * 1 == json.FoodMarketingConfigure[0].GUESTFOOD) $(this).addClass('checked').siblings().removeClass('checked'); });
                $("#GuestFoodMoney").val(json.FoodMarketingConfigure[0].GUESTFOODMONEY);
                $("#GuestFoodProportion").val(json.FoodMarketingConfigure[0].GUESTFOODPROPORTION);
                if (json.FoodMarketingConfigure[0].GUESTMAINTAIN == "1") {
                    $("#GuestMaintain").find("label").addClass('checked');
                    $("#GuestMaintain").find("label").attr("value", "1");
                }
                else {
                    $("#GuestMaintain").find("label").removeClass('checked');
                    $("#GuestMaintain").find("label").attr("value", "0");
                }
            });







            //商城
            var urles = "MarketingConfigure.ashx?action=getproductinfo";
            urles = urles + "&AdminHotelid=" + AdminHotelid;
            $.post(urles, function (dataes) {
                var json = eval("(" + dataes + ")");
                console.log(dataes)

                $("#MallProduct").find("label").each(function () { if ($(this).attr("value") * 1 == json.ProductMarketingConfigure[0].MALLPRODUCT) $(this).addClass('checked').siblings().removeClass('checked'); });
                $("#MallProductMoney").val(json.ProductMarketingConfigure[0].MALLPRODUCTMONEY);
                $("#MallProductProportion").val(json.ProductMarketingConfigure[0].MALLPRODUCTPROPORTION);

                $("#MallMaintain").find("label").each(function () { if ($(this).attr("value") * 1 == json.ProductMarketingConfigure[0].MALLMAINTAIN) $(this).addClass('checked').siblings().removeClass('checked'); });
                $("#MallMaintainMoney").val(json.ProductMarketingConfigure[0].MALLMAINTAINMONEY);
                $("#MallMaintainProportion").val(json.ProductMarketingConfigure[0].MALLMAINTAINPROPORTION);

            });

            //公共奖金
            var urlgg = "MarketingConfigure.ashx?action=getpublicinfo";
            urlgg = urlgg + "&AdminHotelid=" + AdminHotelid;
            $.post(urlgg, function (data) {
                var json = eval("(" + data + ")");
                console.log(data)
                //alert(json.Public_MarketingConfigure[0].PUBLICWITHDRAWAL);
                //公共奖金
                $("#PublicRegisterMoney").val(json.Public_MarketingConfigure[0].REGISTERMONEY);

                $("#PublicCheckIn").find("label").each(function () { if ($(this).attr("value") * 1 == json.Public_MarketingConfigure[0].CHECKIN) $(this).addClass('checked').siblings().removeClass('checked'); });
                $("#PublicCheckInMoney").val(json.Public_MarketingConfigure[0].CHECKINMONEY);
                $("#PublicCheckInProportion").val(json.Public_MarketingConfigure[0].CHECKINPROPORTION);

                $("#PublicCheckInTwo").find("label").each(function () { if ($(this).attr("value") * 1 == json.Public_MarketingConfigure[0].CHECKINTWO) $(this).addClass('checked').siblings().removeClass('checked'); });
                $("#PublicCheckInTwoMoney").val(json.Public_MarketingConfigure[0].CHECKINTWOMONEY);
                $("#PublicCheckInTwoProportion").val(json.Public_MarketingConfigure[0].CHECKINTWOPROPORTION);

                $("#PublicRecharge").find("label").each(function () { if ($(this).attr("value") * 1 == json.Public_MarketingConfigure[0].RECHARGE) $(this).addClass('checked').siblings().removeClass('checked'); });
                $("#PublicRechargeMoney").val(json.Public_MarketingConfigure[0].RECHARGEMONEY);
                $("#PublicRechargeProportion").val(json.Public_MarketingConfigure[0].RECHARGEPROPORTION);

                $("#PublicUpgrade").find("label").each(function () { if ($(this).attr("value") * 1 == json.Public_MarketingConfigure[0].UPGRADE) $(this).addClass('checked').siblings().removeClass('checked'); });
                $("#PublicUpgradeMoney").val(json.Public_MarketingConfigure[0].UPGRADEMONEY);
                $("#PublicUpgradeProportion").val(json.Public_MarketingConfigure[0].UPGRADEPROPORTION);

                $("#PublicStaffFood").find("label").each(function () { if ($(this).attr("value") * 1 == json.Public_MarketingConfigure[0].STAFFFOOD) $(this).addClass('checked').siblings().removeClass('checked'); });
                $("#PublicStaffFoodMoney").val(json.Public_MarketingConfigure[0].STAFFFOODMONEY);
                $("#PublicStaffFoodProportion").val(json.Public_MarketingConfigure[0].STAFFFOODPROPORTION);
                $("#DDLUserList").val(json.Public_MarketingConfigure[0].PUBLICWITHDRAWAL);


            });



        }

        function SetRoomMoney() {
            var AdminHotelid = $("#AdminHotelid").val();
            var url = "/SysSetBase/superAdmin/SalesMoney.aspx?AdminHotelid=" + AdminHotelid;
            top.art.dialog.open(url, {
                id: 'SalesMoney',
                title: '设置房型奖金',
                width: 760,
                height: 450,
                close: function () {

                }
            }, false);
        }

        function PublicSetRoomMoney() {
            var AdminHotelid = $("#AdminHotelid").val();
            var url = "/SysSetBase/superAdmin/SalesMoneys.aspx?AdminHotelid=" + AdminHotelid;
            top.art.dialog.open(url, {
                id: 'SalesMoneys',
                title: '设置房型奖金',
                width: 760,
                height: 450,
                close: function () {

                }
            }, false);
        }


        function SetProductMoney() {
            var AdminHotelid = $("#AdminHotelid").val();
            var url = "/SysSetBase/superAdmin/SalesMoneysp.aspx?AdminHotelid=" + AdminHotelid;
            top.art.dialog.open(url, {
                id: 'SalesMoneysp',
                title: '设置商品奖金',
                width: 600,
                height: 450,
                close: function () {

                }
            }, false);
        }



        //三级维护费 设置
        function SetMaintainMoney() {
            var AdminHotelid = $("#AdminHotelid").val();
            var url = "/SysSetBase/superAdmin/ConditionMaintainMoney.aspx?AdminHotelid=" + AdminHotelid;
            top.art.dialog.open(url, {
                id: 'SalesMoney',
                title: '设置三级维护费',
                width: 380,
                height: 250,
                close: function () {

                }
            }, false);
        }
    </script>
    </form>
</body>
</html>
