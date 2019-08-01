<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin_Info.aspx.cs" Inherits="RM.Web.RMBase.SysUser.Admin_Info" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>用户信息表单</title>
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/Validator/JValidator.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">

        //初始化
        $(function () {
            $('.checkbox').on('click', 'label', function () {
                $(this).hasClass('checked') ? $(this).removeClass('checked') : $(this).addClass('checked');
            });

            $("#ddlUser_Role").change(function () {
                var ur = $(this).val();
                if (ur != "-1") {
                    $.post("UserInfo.ashx?action=GetRoleGroup&roleid=" + ur, function (data) {
                        if (data != "") {
                            $(".qxsz .checked").removeClass('checked');
                            var json = eval("(" + data + ")");
                            if (json.length > 0) {
                                for (var i = 0; i < json.length; i++) {
                                    $(".qxsz label[dataid='" + json[i] + "']").addClass('checked');
                                }
                            }
                        }
                    });
                }
            });
            var hs = $("#HotelListId").val();
            if (hs != "") {
                $("#dHotel label").removeClass("checked");
                var hotelIds = hs.split(",");
                for (var i = 0; i < hotelIds.length; i++) {
                    $("#dHotel label[hotelid='" + hotelIds[i] + "']").addClass("checked");
                }
            }
        })

        //表单提交验证
        function CheckValid() {
            var dlc = $("#dHotel label.checked");
            if (dlc.length < 1) {
                showTipsMsg('请选择可管理的酒店！', '5000', '4');
                return false;
            }
            if (dlc.length == 1) {
                showTipsMsg('单店管理员可在员工管理添加！', '5000', '4');
                return false;
            }

            var reVal = '';
            dlc.each(function () {
                reVal += $(this).attr("hotelid") + ",";
            });
            reVal = reVal.substr(0, reVal.length - 1);

            if ($("#dHotel label.checked").val() == "-1") {
                showTipsMsg('请选择所属酒店！', '5000', '4');
                return false;
            }

            $("#HotelListId").val(reVal);
            if (!CheckDataValid('#form1')) {
                return false;
            }

            $("#checkbox_value").val(CheckboxValues());

            if (!confirm('您确认要保存此操作吗？')) {
                return false;
            }
        }
        //获取 所有打勾复选框值
        function CheckboxValues() {
            var reVal = '';
            $('.qxsz label.checked').each(function () {
                reVal += $(this).attr("dataid") + ",";
            });
            reVal = reVal.substr(0, reVal.length - 1);
            return reVal;
        }

    </script>
    <style type="text/css">
        .tjyh dd
        {
            width: 50%;
            float: left;
        }
        .tjyh dd div
        {
            width: calc(100% - 105px);
            float: left;
        }
        .tjyh dd div input, .tjyh dd div select
        {
            width: 100%;
        }
        .tjyh
        {
            margin-bottom: 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <%-- 所有打勾复选框值--%>
    <input id="checkbox_value" type="hidden" runat="server" />
    <input id="HotelAdmin" type="hidden" runat="server" />
    <input id="HotelId" type="hidden" runat="server" value="0" />
    <input id="hdUserId" type="hidden" runat="server" value="" />
    <input id="HotelListId" type="hidden" runat="server" value="" />
    <dl class="addsystemuser">
        <div class="sharehead">
            <span>基本信息</span><span class="line"></span>
        </div>
        <div class="tjyh clearfix">
            <dd>
                <small>姓名</small>
                <div>
                    <input id="User_Name" runat="server" type="text" datacol="yes" err="姓名" checkexpession="NotNull" />
                </div>
            </dd>
            <dd>
                <small>手机号码</small>
                <div>
                    <input id="User_Account" runat="server" type="text" datacol="yes" err="手机号码" checkexpession="NotNull" />
                </div>
            </dd>
            <dd>
                <small>证件类型</small>
                <div>
                    <select id="Credentials_Type" runat="server">
                    </select>
                </div>
            </dd>
            <dd>
                <small>证件号</small>
                <div>
                    <input id="Credentials_Number" runat="server" type="text" />
                </div>
            </dd>
            <dd>
                <small>密码</small>
                <div>
                    <input id="User_Pwd" runat="server" type="text" datacol="yes" err="登录密码" checkexpession="NotNull" />
                </div>
            </dd>
        </div>
        <div class="sharehead">
            <span>酒店设置</span><span class="line"></span>
        </div>
        <dd>
            <div id="dHotel" class="checkbox" style="margin-left: 25px;">
                <asp:Repeater runat="server" ID="rptHotelList">
                    <ItemTemplate>
                        <label class="checked" hotelid="<%# Eval("id")%>">
                            <%# Eval("name")%></label>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </dd>
        <div class="sharehead sharehead01" style="display: none;">
            <span>权限设置</span><span class="line"></span>
        </div>
        <div style="display: none;">
            <asp:Repeater runat="server" ID="rptMenuGroup">
                <ItemTemplate>
                    <div class="qxsz">
                        <p>
                            <span>
                                <%# Eval("Group_Name")%></span><em><%# Eval("Group_Remark")%></em>
                        </p>
                        <%# GetMenuGroup(Eval("Group_ID").ToString()) %>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
        <%--
        <div class="qxsz">
            <p>
                <span>重要功能</span><em>设置房型、促销，维护酒店信息，管理协议客户等</em>
            </p>
            <div class="checkbox">
                <label class="checked">
                    促销维护</label>
                <label>
                    酒店信息</label>
                <label>
                    协议客户管理</label>
                <label>
                    创建客房推广页面</label>
                <label>
                    公众号认证</label>
            </div>
        </div>
        --%>
    </dl>
    <div class="adifoliBtn">
        <div style="float: right;">
            <asp:LinkButton ID="Save" runat="server" OnClientClick="return CheckValid();" OnClick="Save_Click">
            <span class="bbgg bbgg1">保 存</span>
            </asp:LinkButton>
            <a class="bbgg" href="javascript:void(0)" onclick="OpenClose();"><span>关 闭</span></a>
        </div>
    </div>
    </form>
</body>
</html>
