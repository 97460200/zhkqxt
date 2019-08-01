<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Set_now.aspx.cs" Inherits="RM.Web.AdminBase.SysSet.Set_now" %>

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
    <script src="/Themes/Scripts/jquery-1.8.2.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="setnow">
        <b>奖励规则</b>
        <div class="apply">
            <div class="guiz">
                <label name="is_reward" hid="1" class="active">
                    手动提现</label><label name="is_reward" hid="0">线下核算</label>
            </div>
            <div class="gadd">
                <div>
                    <span>单笔最小金额</span><input id="Minimum" onkeyup="this.value=this.value.replace(/\D/g,'')"
                        type="text" /><span>元</span><i class="jy">单笔最小金额默认为1元</i>
                </div>
                <div>
                    <span>单笔单日限额</span><input id="Limit" onkeyup="this.value=this.value.replace(/\D/g,'')"
                        type="text" /><span>元</span><i class="jy">单笔单日限额为2W</i>
                </div>
                <div>
                    <span>每日最多提现</span><input id="Maxmum" onkeyup="this.value=this.value.replace(/\D/g,'')"
                        type="text" /><span>次</span><i class="jy">每个用户每天最多可提现10次</i>
                </div>
            </div>
        </div>
    </div>
    <div class="poBtn">
        <a class="button buttonActive" id="btnyes">保存</a>
    </div>
    <div class="tip">
    </div>
    <%--判断是插入还是更新 --%>
    <input runat="server" type="hidden" id="isid" value="0" />
    <%--默认实时转账，1实时转账 ，0线下核算--%>
    <input runat="server" type="hidden" id="is_reward" value="1" />
    </form>
    <script type="text/javascript">
        var Tip = function (str) {
            if (!$('.tip').is(':hidden')) return;
            $('.tip').html(str).fadeIn(200);
            setTimeout(function () {
                $('.tip').fadeOut(200);
            }, 2000);
        }

        $('.setnow').on('click', 'label', function () {
            var name = $(this).attr("name");
            var hid = $(this).attr("hid");
            $(this).addClass('active').siblings().removeClass('active');

            switch (name) {
                case "is_reward":
                    $("#is_reward").val(hid);

                    if (hid == "1") {
                        $(".gadd").show();
                    } else {
                        $(".gadd").hide();
                    }
                    break;
                default:
                    break;
            }
        })

        $("#btnyes").click(function () {
            $.ajax({
                url: "../Ajax/SysAjax.ashx",
                data: {
                    Menu: "Set_now",
                    isid: $("#isid").val(),
                    is_reward: $("#is_reward").val(), //默认线下核算，1实时转账 ，线下核算               
                    Minimum: $("#Minimum").val(), //单笔最小金额
                    Limit: $("#Limit").val(), //单笔单日限额
                    Maxmum: $("#Maxmum").val() //每日最多提现
                },
                type: "POST",
                dataType: "text",
                async: false,
                success: function (data) {
                    if (data == "0") {
                        Tip("保存失败！");
                    } else {
                        Tip("保存成功！");
                    }
                }
            });
        });

        $(function () {
            show_fx();
        });

        function show_fx() {
            $.ajax({
                url: "../Ajax/SysAjax.ashx",
                data: {
                    Menu: "Show_Set_now"
                },
                type: "POST",
                dataType: "text",
                async: false,
                success: function (data) {

                    if (data == "") {
                        return;
                    }
                    var json = eval("(" + data + ")");
                    for (var i = 0; i < json.length; i++) {
                        var id = json[i].ID;
                        $("#isid").val(id);

                        var is_reward = json[i].IS_REWARD;
                        $("#is_reward").val(is_reward);
                        if (is_reward == $("label[name=is_reward]").eq(0).attr("hid")) {
                            $("label[name=is_reward]").eq(0).addClass('active').siblings().removeClass('active');
                            $(".gadd").show();
                            var Minimum = json[i].MINIMUM;
                            $("#Minimum").val(Minimum);
                            var Limit = json[i].LIMIT;
                            $("#Limit").val(Limit);
                            var Maxmum = json[i].MAXMUM;
                            $("#Maxmum").val(Maxmum);
                        }
                    }
                }
            });
        }
    
    </script>
</body>
</html>
