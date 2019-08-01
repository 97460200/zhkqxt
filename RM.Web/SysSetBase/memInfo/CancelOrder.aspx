<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CancelOrder.aspx.cs" Inherits="RM.Web.SysSetBase.memInfo.CancelOrder" %>

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
        $(function () {
            Keypress("txtNight");
            Keypress("txtMoney");
        });

        function checkData() {
            
            var money = parseInt($("#spTomePrice").html());
            var t_Money = parseFloat($("#txtMoney").val());

            if (t_Money > money) {
                showTipsMsg('退回金额不能大于订单支付金额!', '5000', '5');
                return false;
            }

            if ($("#txtReason").val() == "") {
                showTipsMsg('请输入取消原因!', '5000', '5');
                return false;
            }
        }
        //提示
        function cancelTips() {
            var url = "/RMBase/SysOrder/CancelTips.aspx";
            top.art.dialog.open(url, {
                id: 'CancelTips',
                title: '系统提示',
                width: 400,
                height: 150,
                close: function () {
                }
            }, false);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <input runat="server" type="hidden" id="hdId" />
    <input runat="server" type="hidden" id="hdPayType" />
    <div class="addevaluate" style="height: 350px; overflow-y: auto;">
        <dl>
            <dd>
                <small>订单编号</small>
                <div>
                    <span runat="server" id="spOrderNum"></span>
                </div>
            </dd>
            
            <dd>
                <small>支付金额</small>
                <div>
                    <span runat="server" id="spTomePrice">0</span>
                </div>
            </dd>

            <dd>
                <small>原路退回金额</small>
                <div>
                    <asp:TextBox ID="txtMoney" runat="server" MaxLength="8" autocomplete="off" Style="width: 150px;"></asp:TextBox><span
                        class="jy">*</span>
                </div>
            </dd>
            <dd>
                <small>原因</small>
                <div>
                    <asp:TextBox ID="txtReason" runat="server" autocomplete="off" TextMode="MultiLine"
                        Style="width: 280px;"></asp:TextBox><span class="jy jy01">*</span>
                </div>
            </dd>
        </dl>
        <asp:HiddenField ID="hfdID" runat="server" Value="0" />
    </div>
    <div class="adifoliBtn">
        <div style="float: right;">
            <asp:Button ID="btnSumit" runat="server" Text="确定退款" OnClientClick="return checkData()"
                OnClick="btnSumit_Click" />
        </div>
    </div>
    </form>
</body>
</html>
