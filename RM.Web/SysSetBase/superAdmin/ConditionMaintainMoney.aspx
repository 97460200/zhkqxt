<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConditionMaintainMoney.aspx.cs"
    Inherits="RM.Web.SysSetBase.superAdmin.ConditionMaintainMoney" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/PqGrid/jquery-ui.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/PqGrid/pqgrid.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('.radio').on('click', 'label', function () {
                $(this).addClass('checked').siblings().removeClass('checked');
            });
            $("#Maintain").find("label[value='" + $("#hdMaintain").val() + "']").addClass("checked");

            Keypress("TotalNumber");
            Keypress("MoreNumber");
            Keypress("MoreProportion");
            $("#TotalNumber").blur(function () {
                var tn = parseInt($("#TotalNumber").val());
                var mn = parseInt($("#MoreNumber").val());
                var bl = parseFloat(mn / tn) * 100;
                if (!isNaN(bl)) {
                    $("#MoreProportion").val(parseInt(bl));
                }
            });
            $("#MoreNumber").blur(function () {
                var tn = parseInt($("#TotalNumber").val());
                var mn = parseInt($("#MoreNumber").val());
                var bl = parseFloat(mn / tn) * 100;
                if (!isNaN(bl)) {
                    $("#MoreProportion").val(parseInt(bl));
                }
            });
            $("#MoreProportion").blur(function () {
                var tn = parseInt($("#TotalNumber").val());
                var mp = parseInt($("#MoreProportion").val());
                var bl = tn * (mp / 100);
                if (!isNaN(bl)) {
                    $("#MoreNumber").val(parseInt(bl));
                }
            });
        });


        function num(obj) {
            obj.value = obj.value.replace(/[^\d.]/g, ""); //清除"数字"和"."以外的字符
            obj.value = obj.value.replace(/^\./g, ""); //验证第一个字符是数字
            obj.value = obj.value.replace(/\.{2,}/g, "."); //只保留第一个, 清除多余的
            obj.value = obj.value.replace(".", "$#$").replace(/\./g, "").replace("$#$", ".");
            obj.value = obj.value.replace(/^(\-)*(\d+)\.(\d\d).*$/, '$1$2.$3'); //只能输入两个小数
        }

        function checkData() {
            if ($("#TotalNumber").val() == "") {
                showTipsMsg('总房间数不能为空!', '3000', '5');
                $("#TotalNumber").focus();
                return false;
            }
            if ($("#MoreNumber").val() == "") {
                showTipsMsg('订房比例及数量不能为空!', '3000', '5');
                $("#MoreNumber").focus();
                return false;
            }

            var Maintain = $("#Maintain").find(".checked").attr("value");
            if (Maintain == "1") {
                $("#MaintainProportion").val(0);
            } else {
                $("#MaintainMoney").val(0);
            }
            $("#hdMaintain").val(Maintain);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <input runat="server" type="hidden" id="hdAdminHotelid" />
    <input runat="server" type="hidden" id="hdMaintain" value="1" />
    <div class="addevaluate" style="height: 200px; overflow-y: auto;">
        <dl>
            <dd>
                <small>总房间数</small>
                <div>
                    <input id="TotalNumber" runat="server" type="text" autocomplete="off" style="width: 80px;" />
                </div>
            </dd>
            <dd>
                <small>当订房数大于</small>
                <div>
                    <input id="MoreNumber" runat="server" type="text" autocomplete="off" style="width: 92px;" />间,或
                    <input id="MoreProportion" runat="server" type="text" autocomplete="off" style="width: 88px;" />%
                </div>
            </dd>
            <dd>
                <small>维护费</small>
                <div class="radio" id="Maintain" runat="server">
                    <label value='1'>
                        固定<input type="text" id="MaintainMoney" maxlength="5" onkeyup="num(this)" runat="server"
                            autocomplete="off" style="width: 50px;" />元</label>
                    <label value='0'>
                        比例<input type="text" id="MaintainProportion" maxlength="5" onkeyup="num(this)" runat="server"
                            autocomplete="off" style="width: 50px;" />%</label>
                </div>
            </dd>
        </dl>
    </div>
    <div class="adifoliBtn">
        <div style="float: right;">
            <asp:Button ID="btnSumit" runat="server" Text="确定" OnClientClick="return checkData()"
                OnClick="btnSumit_Click" />
        </div>
    </div>
    </form>
</body>
</html>
