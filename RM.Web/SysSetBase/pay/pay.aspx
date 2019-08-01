<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pay.aspx.cs" Inherits="RM.Web.SysSetBase.pay.pay" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>支付设置</title>
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
</head>
<body>
    <form id="form1" runat="server">
    <input runat="server" type="hidden" id="hdAdminHotelId" />
    <input runat="server" type="hidden" id="hdCashMoneyReturnType" value="1" />
    <input runat="server" type="hidden" id="hdCashMoneyEdit" value="1" />
    <input runat="server" type="hidden" id="hdPledgeMoneyEnable" value="2" />
    <input runat="server" type="hidden" id="hdPledgeMoneyRoom" value="2" />
    <div class="tools_bar btnbartitle btnbartitlenr" style="display: block;">
        <div>
            当前位置：<span><a title="系统首页" href="/Frame/HomeIndex.aspx">系统首页</a></span><span> &gt;
            </span><span>系统设置</span> &gt; <span>支付设置</span>
        </div>
    </div>
    <div class="pay">
        <b class="tx">设置是否启用以下支付方式，启用勾选，不启用不勾选</b>
        <ul class="pyList">
            <li><b>酒店名称</b>
                <div class="pytype">
                    <asp:DropDownList ID="DDLHotelList" runat="server" Style="width: 300px;">
                    </asp:DropDownList>
                    <i class="jy">*</i>
                </div>
            </li>
            <li><b>支付方式</b>
                <div class="pytype">
                    <span id="Pay" name="Pay">微信支付</span><span id="Hypay" name="Hypay">会员卡支付</span><span
                        id="jfdh" name="jfdh">积分兑换</span><span id="Qtpay" name="Qtpay" style="display: ">到店付款</span>
                </div>
            </li>
            <li><b>默认支付方式</b>
                <div class="pytype">
                    <asp:DropDownList ID="DropDownList1" runat="server" Style="width: 300px;">
                        <asp:ListItem Text="微信支付" Value="0" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="到店付款" Value="1"></asp:ListItem>
                    </asp:DropDownList>
                    <i class="jy">*</i>
                </div>
            </li>
            <li style="display: none"><b>积分兑换</b>
                <div class="pytype">
                    <span id="Jfpay" name="Jfpay">平日积分兑换</span><span id="JFZhoumo" name="JFZhoumo">周末积分兑换</span><span
                        id="JFJieri" name="JFJieri">节假日积分兑换</span>
                </div>
            </li>
            <li style="display: none"><b>卡券抵扣</b>
                <div class="pytype">
                    <span id="yhqzhoumo" name="yhqzhoumo">周末卡券抵扣</span><span id="yhqjieri" name="yhqjieri">节假日卡券抵扣</span>
                </div>
            </li>
        </ul>
        <div class="kefangsz functionsettings paymentsettings" style="height: auto;">
            <dl class="addevaluate kefangszb" style="border-bottom: 1px solid #f3f3f3; padding-left: 0;
                display: none;">
                <div class="fsl">
                    支付方式
                </div>
                <div style="float: left" class="zffs">
                    <table border="0" cellpadding="0" cellspacing="0" class="ul">
                        <tr>
                            <th>
                                启用
                            </th>
                            <th>
                                支付方式
                            </th>
                            <th>
                                说明
                            </th>
                        </tr>
                        <tr class="active">
                            <td>
                                <i class="icon-radio6"></i>
                            </td>
                            <td>
                                累计积分升级
                            </td>
                            <td>
                                <input type="text" style="width: 480px;">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <i class="icon-radio6"></i>
                            </td>
                            <td>
                                累计积分升级
                            </td>
                            <td>
                                <input type="text" style="width: 480px;">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <i class="icon-radio6"></i>
                            </td>
                            <td>
                                累计积分升级
                            </td>
                            <td>
                                <input type="text" style="width: 480px;">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <i class="icon-radio6"></i>
                            </td>
                            <td>
                                累计积分升级
                            </td>
                            <td>
                                <input type="text" style="width: 480px;">
                            </td>
                        </tr>
                    </table>
                </div>
            </dl>
            <dl class="addevaluate kefangszb" style="border-top: 1px solid #f3f3f3; padding-left: 0;">
                <div class="fsl">
                    卡券抵扣
                </div>
                <div style="float: left">
                    <dd>
                        <small style="width: 156px;">同一订单是否可叠加使用卡券</small>
                        <div class="radio" id="is_dj_Coupon">
                            <label value="1">
                                是</label>
                            <label value="0">
                                否</label>
                        </div>
                    </dd>
                    <dd style="display: none">
                        <small style="width: 156px; text-align: left;">立减客房是否可用卡券</small>
                        <div class="radio">
                            <label class="checked" value="1">
                                是</label>
                            <label value="0">
                                否</label>
                        </div>
                    </dd>
                    <dd>
                        <small style="width: 156px; text-align: left;">续住时是否可使用卡券</small>
                        <div class="radio" id="is_xz_Coupon">
                            <label value="1">
                                是</label>
                            <label value="0">
                                否</label>
                        </div>
                    </dd>
                </div>
            </dl>
            <%--押金金额--%>
            <dl runat="server" id="dDeposit" class="addevaluate kefangszb" style="border-top: 1px solid #f3f3f3;
                padding-left: 0;">
                <div class="fsl">
                    押金设置
                </div>
                <div style="float: left">
                    <dd>
                        <small style="width: 156px; text-align: left;">默认押金金额</small>
                        <div id="dPledgeMoneyEnable" class="radio" style="width: 256px;">
                            <label value="1">
                                是</label>
                            <label class="checked" value="2">
                                否</label>
                            <div style="display: none;">
                                <input runat="server" type="text" id="CashPledgeMoney" style="width: 60px;" />元
                            </div>
                        </div>
                    </dd>
                    <dd>
                        <small style="width: 156px; text-align: left;">房型押金金额</small>
                        <div id="dPledgeMoneyRoom" class="radio">
                            <label value="1">
                                是</label>
                            <label class="checked" value="2">
                                否</label>
                            <div style="display: none;">
                                <a class="btn" onclick="RoomPledgeMoney()" style="width: 100px;">设置房型押金</a>
                            </div>
                        </div>
                    </dd>
                    <dd id="ddCashMoneyEdit" style="display: none;">
                        <small style="width: 156px; text-align: left;">客人修改押金</small>
                        <div id="dCashMoneyEdit" class="radio">
                            <label class="checked" value="1">
                                是</label>
                            <label value="2">
                                否</label>
                        </div>
                    </dd>
                    <dd>
                        <small style="width: 156px; text-align: left;">押金退回方式</small>
                        <div id="dCmrt" class="radio">
                            <label class="checked" value="1">
                                原路退回</label>
                            <label value="2" style="display: none;">
                                退至会员卡</label>
                        </div>
                    </dd>
                    <dd>
                        <small style="width: 156px; text-align: left;">押金二维码</small>
                        <div>
                            <img runat="server" id="imgPledgeMoneyCode" width="135" height="135" />
                            <div class="xz">
                                <asp:Button ID="btnDownload" runat="server" Text="下载" class="btn" OnClick="btnDownload_Click" />
                            </div>
                        </div>
                    </dd>
                </div>
            </dl>
            <dl class="addevaluate kefangszb" style="border-top: 1px solid #f3f3f3; padding-left: 0;
                display: none;">
                <div class="fsl">
                    退款设置
                </div>
                <div style="float: left">
                    <dd>
                        <small>客房退款前提</small>
                        <div class="checkbox">
                            <label class="checked" value="1">
                                入住前</label>
                            <label value="0">
                                入住未退房</label>
                            <label value="0">
                                已退房</label>
                        </div>
                    </dd>
                    <dd>
                        <small>餐饮退款前提</small>
                        <div class="checkbox">
                            <label class="checked" value="1">
                                接单前</label>
                            <label value="0">
                                未配送</label>
                            <label value="0">
                                未送达</label>
                            <label value="0">
                                已送达</label>
                        </div>
                    </dd>
                </div>
            </dl>

            <%--会员卡快捷支付--%>
            <dl runat="server" id="Dl1" class="addevaluate kefangszb" style="border-top: 1px solid #f3f3f3;
                padding-left: 0; display:none;">
                <div class="fsl">
                    快捷支付设置
                </div>
                <div style="float: left">
                    
                    <dd>
                        <small style="width: 156px; text-align: left;">快捷支付二维码</small>
                        <div>
                            <img runat="server" id="kjzf" width="135" height="135" />
                            <div class="xz">
                                <asp:Button ID="Button1" runat="server" Text="下载" class="btn" OnClick="btnDownloads_Click" />
                            </div>
                        </div>
                    </dd>
                </div>
            </dl>

        </div>
        <div class="poBtn">
            <a class="button buttonActive" onclick="setzf()">保存</a>
            <asp:Button runat="server" ID="btnSave" Text="保存" OnClick="btnSave_Click" Style="display: none;" />
        </div>
    </div>
    </form>
    <script src="/SysSetBase/css/jquery.js" type="text/javascript"></script>
    <script type="text/javascript">
        $('.pyList').on('click', 'span', function () {
            $(this).hasClass('active') ? $(this).removeClass('active') : $(this).addClass('active');
        });

        $('#DDLHotelList').on('change', function () {
            bind($('#DDLHotelList').val());
        });


        $('.radio').on('click', 'label', function () {
            $(this).addClass('checked').siblings().removeClass('checked');
        });

        bind(0);
        function setzf() {

            var Pay = 0; var Qtpay = 0; var Hypay = 0;
            var Jfpay = 0; var JFZhoumo = 0; var JFJieri = 0;
            var yhqzhoumo = 0; var yhqjieri = 0;

            var is_xz_Coupon = $("#is_xz_Coupon").find(".checked").attr("value");
            var is_dj_Coupon = $("#is_dj_Coupon").find(".checked").attr("value");


            $(".pyList").find(".active").each(function () {
                if ($(this).attr("name") == "Pay") Pay = 1;
                if ($(this).attr("name") == "Qtpay") Qtpay = 1;
                if ($(this).attr("name") == "Hypay") Hypay = 1;
                if ($(this).attr("name") == "jfdh") { JFJieri = JFZhoumo = Jfpay = 1; }
                yhqzhoumo = 1; yhqjieri = 1;
            });
            var url = "pay.ashx?action=update";
            url = url + "&Hotelid=" + $("#DDLHotelList").val();
            url = url + "&Pay=" + Pay; url = url + "&Qtpay=" + Qtpay; url = url + "&Hypay=" + Hypay;
            url = url + "&Jfpay=" + Jfpay; url = url + "&JFZhoumo=" + JFZhoumo; url = url + "&JFJieri=" + JFJieri;
            url = url + "&yhqzhoumo=" + yhqzhoumo; url = url + "&yhqjieri=" + yhqjieri;
            url = url + "&mrzf=" + $("#DropDownList1").val();

            url = url + "&is_xz_Coupon=" + is_xz_Coupon;
            url = url + "&is_dj_Coupon=" + is_dj_Coupon;

            $.post(url, function (data) {
                if (data == "ok") {
                    showTipsMsg("保存成功！", 2000, 4);
                }
                else {
                    showTipsMsg("保存失败！", 2000, 4);
                }
            });
            var cmrt = $("#dCmrt").find(".checked").attr("value");
            $("#hdCashMoneyReturnType").val(cmrt);
            var cme = $("#dCashMoneyEdit").find(".checked").attr("value");
            $("#hdCashMoneyEdit").val(cme);
            $("#btnSave").click();
        }

        function bind(id) {

            $(".pyList").find(".active").each(function () {
                $(this).removeClass('active');
            });

            var url = "pay.ashx?action=getinfo";
            $.post(url, function (data) {
                var json = eval("(" + data + ")");
                if (id * 1 == 0) {
                    if (json.Hotel[0].PAY == 1) $("#Pay").addClass("active");
                    if (json.Hotel[0].QTPAY == 1) $("#Qtpay").addClass("active");
                    if (json.Hotel[0].HYPAY == 1) $("#Hypay").addClass("active");
                    if (json.Hotel[0].JFPAY == 1) $("#jfdh").addClass("active");

                    $("#is_xz_Coupon").find("label[value=" + json.Hotel[0].IS_XZ_COUPON + "]").addClass('checked').siblings().removeClass('checked');
                    $("#is_dj_Coupon").find("label[value=" + json.Hotel[0].IS_DJ_COUPON + "]").addClass('checked').siblings().removeClass('checked');
                } else {
                    for (var i = 0; i < json.Hotel.length; i++) {
                        if (id * 1 == json.Hotel[i].ID * 1) {
                            if (json.Hotel[i].PAY == 1) $("#Pay").addClass("active");
                            if (json.Hotel[i].QTPAY == 1) $("#Qtpay").addClass("active");
                            if (json.Hotel[i].HYPAY == 1) $("#Hypay").addClass("active");
                            if (json.Hotel[i].JFPAY == 1) $("#jfdh").addClass("active");
                            $("#is_xz_Coupon").find("label[value=" + json.Hotel[i].IS_XZ_COUPON + "]").addClass('checked').siblings().removeClass('checked');
                            $("#is_dj_Coupon").find("label[value=" + json.Hotel[i].IS_DJ_COUPON + "]").addClass('checked').siblings().removeClass('checked');
                        }
                    }
                }
                $("#DropDownList1").val(json.Hotel[0].MRZF);
            });
        }

    </script>
    <script type="text/javascript">
        $(function () {
            $("#dPledgeMoneyEnable,#dPledgeMoneyRoom").on("click", "label", function () {
                var enable_val = $("#dPledgeMoneyEnable .checked").attr("value");
                var room_val = $("#dPledgeMoneyRoom .checked").attr("value");

                if (enable_val == "1") {
                    $("#dPledgeMoneyEnable > div").show();
                } else {
                    $("#dPledgeMoneyEnable > div").hide();
                }
                if (room_val == "1") {
                    $("#dPledgeMoneyRoom > div").show();
                } else {
                    $("#dPledgeMoneyRoom > div").hide();
                }
                if (enable_val == "1" || room_val == "1") {
                    $("#ddCashMoneyEdit").show();
                } else {
                    $("#ddCashMoneyEdit").hide();
                }
                $("#hdPledgeMoneyEnable").val(enable_val);
                $("#hdPledgeMoneyRoom").val(room_val);
            });

            load_yj();
        });
        //加载押金设置
        function load_yj() {

            var enable_val = $("#hdPledgeMoneyEnable").val();
            var room_val = $("#hdPledgeMoneyRoom").val();

            if (enable_val == "1") {
                $("#dPledgeMoneyEnable > div").show();
            } else {
                $("#dPledgeMoneyEnable > div").hide();
            }
            if (room_val == "1") {
                $("#dPledgeMoneyRoom > div").show();
            } else {
                $("#dPledgeMoneyRoom > div").hide();
            }
            if (enable_val == "1" || room_val == "1") {
                $("#ddCashMoneyEdit").show();
            } else {
                $("#ddCashMoneyEdit").hide();
            }
            $("#dPledgeMoneyEnable label").removeClass('checked');
            $("#dPledgeMoneyRoom label").removeClass('checked');
            $("#dPledgeMoneyEnable label[value=" + enable_val + "]").addClass('checked');
            $("#dPledgeMoneyRoom label[value=" + room_val + "]").addClass('checked');


            //客人修改押金
            if ($("#hdCashMoneyEdit").val() != "1") {
                $("#dCashMoneyEdit label").removeClass('checked');
                $("#dCashMoneyEdit label[value=2]").addClass('checked');
            }
            //押金退回方式
            if ($("#hdCashMoneyReturnType").val() != "1") {
                $("#dCmrt label").removeClass('checked');
                $("#dCmrt label[value=2]").addClass('checked');
            }
        }

        function RoomPledgeMoney() {
            var AdminHotelid = $("#hdAdminHotelId").val();
            var url = "/SysSetBase/pay/RoomPledgeMoney.aspx?AdminHotelid=" + AdminHotelid;
            top.art.dialog.open(url, {
                id: 'SalesMoney',
                title: '设置房型押金',
                width: 760,
                height: 450,
                close: function () {

                }
            }, false);
        }
    </script>
</body>
</html>
