<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BusinOrderEdit.aspx.cs"
    Inherits="RM.Web.SysSetBase.Busin.BusinOrderEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>营业点订单</title>
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
    <script language="javascript" type="text/javascript">
        function check() {

            if (!confirm('您确认要提交此操作吗？')) {
                return false;
            }
        }

  
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <dl class="adifoli">
        <dd>
            <small>订单号</small>
            <div>
                <asp:Label ID="lblOrderNumber" runat="server"></asp:Label>
            </div>
        </dd>
        <dd>
            <small>营业点</small>
            <div>
                <asp:Label ID="lblBusinessName" runat="server"></asp:Label>
            </div>
        </dd>
        <dd>
            <small>预订时间</small>
            <div>
                <asp:Label ID="lblReservedTime" runat="server"></asp:Label>
            </div>
        </dd>
        <dd>
            <small>联系人</small>
            <div>
                <asp:Label ID="lblContact" runat="server"></asp:Label>
            </div>
        </dd>
        <dd>
            <small>手机号码</small>
            <div>
                <asp:Label ID="lblContactPhone" runat="server"></asp:Label>
            </div>
        </dd>
        <dd>
            <small>人数</small>
            <div>
                <asp:Label ID="lblNumber" runat="server">0</asp:Label>人
            </div>
        </dd>
        <dd>
            <small>特殊要求</small>
            <div>
                <asp:Label ID="lblRequirements" runat="server">无</asp:Label>
            </div>
        </dd>
        <dd>
            <small>订单状态</small>
            <div>
                <%=_StateContent%>
            </div>
        </dd>
        <dd>
            <small>所在位置</small>
            <div>
                <asp:Label ID="lbAddress" runat="server">无</asp:Label>
            </div>
        </dd>
        <hr />
        <dd>
            <small>订单处理</small>
            <div class="radio" id="State">
                <label>
                    已确认</label><label class="checked">未确认</label><label>已取消</label>
            </div>
        </dd>
        <dd>
            <small>处理情况</small>
            <div class="checkbox" id="Processing">
                <textarea cols="30" id="lblProcessing" runat="server" rows="10"></textarea>
                <%--  <label class="checked">以短信形式把处理结果发给预订人</label><label>以微信形式把处理结果发给预订人</label>--%>
            </div>
        </dd>
    </dl>
    <div class="adifoliBtn">
        <div style="float: right;">
            <asp:Button ID="btnSumit" runat="server" Text="提交" OnClientClick="return check()"
                OnClick="btnSumit_Click" />
        </div>
        <asp:HiddenField ID="hfState" runat="server" />
    </div>
    </form>
    <script type="text/javascript">

        $("#State").on('click', 'label', function () {

            $(this).addClass('checked').siblings().removeClass('checked');

            if ($(this).html() == "已确认") {

                $("#hfState").val(1);
            }
            if ($(this).html() == "未确认") {

                $("#hfState").val(2);
            }
            if ($(this).html() == "已取消") {
                $("#hfState").val(3);
            }

        });


        $(function () {

            $("#State label").each(function () {
                var state = "";
                if ('<%=_State %>' == "1") {
                    state = "已确认";
                } else if ('<%=_State %>' == "2") {
                    state = "未确认";
                } else if ('<%=_State %>' == "3") {
                    state = "已取消";
                }
                if (state == $(this).html()) {
                    $(this).addClass('checked').siblings().removeClass('checked');
                }
            });


        });
 
    </script>
</body>
</html>
