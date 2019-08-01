<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Role_Info.aspx.cs" Inherits="RM.Web.RMBase.SysUser.Role_Info" %>

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
            var rn = $("#Roles_Name").val();
            if (rn == "酒店员工" || rn == "酒店财务" || rn == "酒店经理" || rn == "酒店管理员") {
                $("#Roles_Name").attr("disabled", "disabled");
            }
        })

        //表单提交验证
        function CheckValid() {
            if (!CheckDataValid('#form1')) {
                return false;
            }
            $("#checkbox_value").val(CheckboxValues())
            if (!confirm('您确认要保存此操作吗？')) {
                return false;
            }
        }
        //获取 所有打勾复选框值
        function CheckboxValues() {
            var reVal = '';
            $('label.checked').each(function () {
                reVal += $(this).attr("dataid") + ",";
            });
            reVal = reVal.substr(0, reVal.length - 1);
            return reVal;
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <%-- 所有打勾复选框值--%>
    <input id="checkbox_value" type="hidden" runat="server" />
    <input id="HotelAdmin" type="hidden" runat="server" />
    <input id="HotelId" type="hidden" runat="server" value="0" />
    <input id="hdRoleId" type="hidden" runat="server" value="" />
    <dl class="addsystemuser">
        <div class="sharehead">
            <span>基本信息</span><span class="line"></span>
        </div>
        <dd>
            <small>角色名称</small>
            <div>
                <input id="Roles_Name" runat="server" type="text" datacol="yes" err="角色名称" checkexpession="NotNull" />
            </div>
        </dd>
        <dd>
            <small>排序</small>
            <div>
                <input id="SortCode" runat="server" type="text" />
            </div>
        </dd>
        <dd>
            <small>备注</small>
            <div>
                <input id="Roles_Remark" runat="server" type="text" />
            </div>
        </dd>
        <div class="sharehead sharehead01">
            <span>权限设置</span><span class="line"></span>
        </div>
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
