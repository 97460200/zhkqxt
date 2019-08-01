<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="couponappoint.aspx.cs" Inherits="RM.Web.SysSetBase.coupons.couponappoint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>卡券设置</title>
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/WDatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="/SysSetBase/css/IE.js" type="text/javascript"></script>
    <script src="/SysSetBase/css/jquery.js" type="text/javascript"></script>
    <script type="text/javascript" src="/App_Themes/admin/js/button.js"></script>
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hddxlx" runat="server" Value="0" />
        <asp:HiddenField ID="hdhydj" runat="server" Value="0" />
        <asp:HiddenField ID="hdxfcs" runat="server" Value="0" />
        <asp:HiddenField ID="hdxfjg" runat="server" Value="0" />

        <asp:HiddenField ID="dj" runat="server" Value="" />
        <asp:HiddenField ID="cs" runat="server" Value="" />
        <asp:HiddenField ID="jg" runat="server" Value="" />
        
    <dl class="adifoli" style="width: 554px; height: 267px; max-height: 267px;">
            <dd>
                <small>赠送对象</small>
                <div class="radio" id="dxlx">
                    <label class="checked" val="0">会员</label><label val="1">个人</label>
                </div>
                <div id="MoreClear" class="guige shi">
                </div>
                <div class="checkbox clearfix" style="margin-left:85px;" id="hydj">
                    <%--<label val="0">全部会员</label><label val="1">金卡会员</label><label val="1">金卡会员</label><label val="1">金卡会员</label>--%>
                    <%=_yxMembergrade%>
                </div>
            </dd>
            
            <dd id="ddxfcs">
                <small>消费次数</small>
                <div class="radio" id="xfcs">
                    <label class="checked" val="0">不限</label><label val="1">1-3次</label><label val="2">4-6次</label><label val="3">7-10次</label><label val="4">10次以上</label>
                </div>
            </dd>
            <dd id="ddxfjg">
                <small>消费间隔</small>
                <div class="radio" id="xfjg">
                    <label class="checked" val="0">不限</label><label val="1">1个月</label><label val="2">2个月</label><label val="3">3个月</label><label val="4">3个月以上</label>
                    <label val="5">自定义
                        <input type="text" id="StartData" runat="server" name="name" value="" onfocus="WdatePicker()" class="date" style="width:100px;"/>-<input type="text" id="EndData" runat="server"  name="name" value="" onfocus="WdatePicker()" class="date" style="width:100px;margin-left:6px;"/>
                    </label>
                </div>
            </dd>
            <%--个人--%>
            <dd style="display:none;" id="ddgr">
                <small>会员手机号</small>
                <div>
                    <textarea style="width:317px;" id="sjhm" runat="server"></textarea>
                    <a href="#" class="btn" style="float: right;margin-right: 30px; display:none">添加</a>
                    <div style="color:#c00">
                        添加多个请用英文逗号分开，例“13576457177,13576457277”
                    </div>
                </div>
            </dd>
            <dd id="dd1">
                <small>赠送时间</small>
                <input id="B_sj" type="text" runat="server" onfocus="WdatePicker()" class="date" />
            </dd>
        </dl>

        <div class="copnliBtn">
        <asp:Button ID="btnSumit" runat="server" Text="提交" OnClientClick="return checkIput()"
            class="copnliBtn" OnClick="btnSumit_Click" />
    </div>
    </form>
    <script>
        $(function () {
            var i1 = parent.window.frames['MattersEdit'];
            var val1 = i1.document.getElementById('hy');
            var val2 = i1.document.getElementById('cs');
            var val3 = i1.document.getElementById('jg');
            $(val1).html("会员：全部会员");
            $(val2).html("消费次数：不限");
            $(val3).html("消费间隔：不限");
            $('#hydj').on('click', 'label', function () {
                $(this).hasClass('checked') ? $(this).removeClass('checked') : $(this).addClass('checked');

                var ishy = "";
                var dj = "";
                for (var i = 0; i < $("#hydj").find(".checked").length; i++) {
                    if ($("#hydj").find(".checked").eq(i).attr("val") == "0") {
                        ishy = "0";
                        dj = "全部会员";
                        break;
                    } else {
                        ishy += $("#hydj").find(".checked").eq(i).attr("val") + ',';
                        dj += $("#hydj").find(".checked").eq(i).html();
                    }

                }
                $("#hdhydj").val(ishy);
                $("#dj").val(dj);
                $(val1).html("会员：" + dj);
            });

            $('#dxlx').on('click', 'label', function () {
                $(this).addClass('checked').siblings().removeClass('checked');
                if ($.trim($(this).html()) == "会员") {
                    $("#MoreClear").show(); $("#hydj").show(); $("#ddxfcs").show(); $("#ddxfjg").show(); $("#ddgr").hide();

                } else if ($.trim($(this).html()) == "个人") {
                    $("#MoreClear").hide(); $("#hydj").hide(); $("#ddxfcs").hide(); $("#ddxfjg").hide(); $("#ddgr").show();
                }
                $("#hddxlx").val($(this).attr("val"));

            });

            $('#xfcs').on('click', 'label', function () {
                $(this).addClass('checked').siblings().removeClass('checked');
                $("#hdxfcs").val($(this).attr("val"));
                $("#cs").val($(this).html());
                $(val2).html("消费次数：" + $(this).html());
            });

            $('#xfjg').on('click', 'label', function () {
                $(this).addClass('checked').siblings().removeClass('checked');
                $("#hdxfjg").val($(this).attr("val"));
                $("#jg").val($(this).html());
                if ($(this).attr("val") != "5") {
                    $(val3).html("消费间隔：" + $(this).html());
                } else {
                    $(val3).html("消费间隔：" + $("#StartData").val() + "至" + $("#EndData").val());
                }
            });
        });

        //alert(window.parent.$("#txtName").val());  
        //alert(parent.window.$("#MattersEdit").html());
        //var i1 = parent.window.frames['MattersEdit']; var val = i1.document.getElementById('zsdx'); $(val).show();
        // alert(val);
        //val.style.display= "";
        
//        var i1 = parent.window.frames['iframeId'];
//        var val = i1.document.getElementById("text1").value;

        //alert($("#txtName").val());
    </script>
</body>
</html>
