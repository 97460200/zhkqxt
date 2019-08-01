<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReceiptRules.aspx.cs" Inherits="RM.Web.SysSetBase.memInfo.ReceiptRules" %>

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

</head>
<body>
    <form id="form1" runat="server">
    <input runat="server" type="hidden" id="hdAdminHotelid" value="0" />
    <div class="addevaluate" style="height: 250px; overflow-y: auto;">
        <dl>
            <dd>
                <small>使用规则</small>
                <div>
                    <textarea style="width:224px;height:150px;" id="txtRulesContent" runat="server"></textarea>
                </div>
            </dd>
        </dl>
    </div>
    <div class="adifoliBtn">
        <div style="float: right;">
               <asp:Button ID="btnSumit" runat="server" Text="提交" OnClientClick="return check()"
                OnClick="btnSumit_Click" />
                 <asp:Button ID="btnReSubmit" runat="server" Text="重置" OnClientClick=" return clck()" />
        </div>
    </div>
    </form>
</body>
</html>
<script type="text/javascript">
    function check() {

        if ($('#txtRulesContent').val() == '') {
            showTipsMsg("请输入使用规则！", 3000, 3);
            $('#txtRulesContent').focus();
            return false;
        }

    }
    function clck() {
        document.getElementById("txtRulesContent").value = "";
    }
</script>