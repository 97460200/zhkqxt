<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserEntry.aspx.cs" Inherits="RM.Web.RMBase.SysUser.UserEntry" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>酒店信息</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer" />
    <script src="../../WDatePicker/WdatePicker.js" type="text/javascript"></script>
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/SysSetBase/css/IE.js" type="text/javascript"></script>
    <script src="/SysSetBase/css/jquery.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../App_Themes/admin/js/button.js"></script>
    <script src="../../Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="../../Themes/Scripts/Validator/JValidator.js" type="text/javascript"></script>
    <script src="../../Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="../../Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script src="../../Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <input runat="server" type="hidden" id="hdUserId" value="" />
    <input runat="server" type="hidden" id="hdUserName" value="" />
    <div class="clearfix">
        <dl class="adifoli addjdxx">
            <dd>
                <div>
                    是否重新入职及关联下线?</div>
            </dd>
            <dd>
                <div class="radio">
                    <label data_val='1' class="checked">
                        是</label>(关联回以前下线关系)
                </div>
                <div class="radio">
                    <label data_val='0'>
                        否</label>(永久删除以前下线关系)
                </div>
            </dd>
        </dl>
    </div>
    <div class="adifoliBtn">
        <div style="float: right;">
            <a class="bbgg" href="javascript:void(0)" onclick="CheckValid();"><span class="bbgg bbgg1">
                保 存</span></a> <a class="bbgg" href="javascript:void(0)" onclick="OpenClose();"><span>
                    关 闭</span></a>
        </div>
    </div>
    </form>
    <script type="text/javascript">
        $('.radio').on('click', 'label', function () {
            $(".radio .checked").removeClass('checked');
            $(this).addClass('checked');
        });

        function CheckValid() {
            var key = $("#hdUserId").val();
            if (key == "") {
                showTipsMsg("操作失败，请稍后重试", 4000, 5);
                OpenClose();
                return;
            }
            var data_val = $(".radio .checked").attr("data_val");
            if (data_val != "0" && data_val != "1") {
                showTipsMsg("请选择是否关联下线", 4000, 5);
                OpenClose();
                return;
            }
            var name = $("#hdUserName").val();
            var url = "/RMBase/SysUser/UserInfo.ashx";
            var parm = { action: "Entry", user_ID: key, user_name: name, data_val: data_val };
            getAjax(url, parm, function (rs) {
                if (rs.toLocaleLowerCase() == 'true') {
                    showTipsMsg("操作成功", 2000, 4);
                    OpenClose();
                } else if (rs.toLocaleLowerCase() == 'false') {
                    showTipsMsg("操作失败，请稍后重试", 4000, 5);
                } else {
                    showTipsMsg(rs, 4000, 3);
                }
            });
        }
    </script>
</body>
</html>
