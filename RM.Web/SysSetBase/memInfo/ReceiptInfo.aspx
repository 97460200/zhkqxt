<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReceiptInfo.aspx.cs" Inherits="RM.Web.SysSetBase.memInfo.ReceiptInfo" %>

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
    <style type="text/css">
        .Explain
        {
            color: Red;
            display:block;
        }
    </style>
    <script type="text/javascript">

        function checkData() {

            var RId = parseInt($("#hdId").val());
            var PrintTime = $("#spPrintTime").html();

            $.ajax({
                url: "mem.ashx",
                data: {
                    Menu: "UpdatePrintTime",
                    RId: RId,
                    PrintTime: PrintTime
                },
                type: "POST",
                dataType: "JSON",
                async: false,
                success: function (data) {
                    if (data == null || data == "") {
                        alert("没有返回数据！");
                        return;
                    } else {//触发打印
                        $(".adifoliBtn").hide();
                        var obj = document.getElementById("ticketstyle");
                        obj.style.cssText = "max-height: 100%;height: 100%;width: 414px;margin:0 auto;";
                        window.print();
                        $(".adifoliBtn").show();
                        obj.style.cssText = "max-height: 600px;height: 600px;width: 414px;overflow: auto;";
                    }
                }
            });
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <input runat="server" type="hidden" id="hdAdminHotelid" value="0" />
    <input runat="server" type="hidden" id="hdId"  value="0"/>
    <input runat="server" type="hidden" id="hdMemberId"  value="0"/>
       <dl class="adifoli TicketPreview" id="ticketstyle" style="max-height: 600px;height: 600px;width: 414px;overflow: auto;">
        <div class="jdm">
            <span  id="spHotelName" runat="server"></span>
        </div>
        <dd>
            <small>卡号</small>
            <div  id="spCardNumber" runat="server"></div>
        </dd>
        <dd>
            <small>会员级别</small>
            <div id="spMemberLevel"  runat="server"></div>
        </dd>
        <dd>
            <small>姓名</small>
            <div id="spName"  runat="server"></div>
        </dd>
        <dd>
            <small>手机号码</small>
            <div  id="spPhone"  runat="server"></div>
        </dd>
        <dd>
            <small>打印时间</small>
            <div id="spPrintTime"  runat="server"></div>
        </dd>
        <div class="czje">
            <dd>
                <small>充值金额</small>
                <div id="spTopUpMoney"  runat="server"></div>
            </dd>
            <dd>
                <small>充值时间</small>
                <div id="TopUpTime" runat="server"></div>
            </dd>
            <dd>
                <small>剩余金额</small>
                <div style="font-weight:bold;font-size:16px;" id="spRemaining"  runat="server"></div>
            </dd>
        </div>
        <div class="kszfewm">
            <p class="p1">
                快捷消费码
            </p>
            <p class="p2">
                <img id="erweima" runat="server" src="../img/ewm.png"  />
            </p>
            <p class="p3">
                请出示二维码给工作人员扫描付款
            </p>
        </div>
        <div class="sygz">
            <p class="p1">
                使用规则
            </p>
            <p class="p2" id="RulesContent" runat="server">
                
            </p>
        </div>
    </dl>
    <div class="adifoliBtn">
        <div style="float: right;">
            <asp:Button ID="btnSumit" runat="server" Text="打印" OnClientClick="return checkData()"  />
              <a class="bbgg" href="javascript:void(0)" onclick="OpenClose();"><span>关 闭</span></a>
        </div>
    </div>
    </form>
</body>
</html>
