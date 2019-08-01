<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserInfo_Info.aspx.cs"
    Inherits="RM.Web.RMBase.SysUser.UserInfo_Info" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>用户信息表单</title>
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .addevaluate dd div input
        {
            width: 100%;
        }
    </style>
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

            if ($("#hdUserId").val() != "") {
                $('#User_Pwd').val("*************");
            }
            if ($("#hdOrgId").val() != "") {
                $.post("UserInfo.ashx?action=GetParentIds&orgid=" + $("#hdOrgId").val(), function (date) {
                    debugger;
                    if (date != "") {
                        var pids = date.split(",");
                        $("#ddlOrg").val(pids[0]);
                        GetOrgInfo(pids[0]);

                        if (pids.length > 0) {
                            $("#dOrgSelect select").val(pids[1])
                        }

                    }
                });
            }
        })

        //表单提交验证
        function CheckValid() {
            if ($('#User_Name').val() == '') {
                showTipsMsg("请输入姓名！", 3000, 3);
                $('#User_Name').focus();
                return false;
            }
            if ($('#User_Account').val() == '') {
                showTipsMsg("请输入手机号码！", 3000, 3);
                $('#User_Account').focus();
                return false;
            }
            if ($('#User_Pwd').val() == '') {
                showTipsMsg("请输入登录密码！", 3000, 3);
                $('#User_Pwd').focus();
                return false;
            }

            //            if ($("#Credentials_Type").val() == "" || $("#Credentials_Type").val() == "-1") {
            //                showTipsMsg('请选择证件类型！', '5000', '3');
            //                return false;
            //            }


            //            if ($('#Credentials_Number').val() == '') {
            //                showTipsMsg("请输入证件号！", 3000, 3);
            //                $('#Credentials_Number').focus();
            //                return false;
            //            }
            if ($("#lbRoomState").attr('class') == "checked") {
                $("#hdRoomState").val(1);
            } else {
                $("#hdRoomState").val(0);
            }
            if ($("#lbHotelData").attr('class') == "checked") {
                $("#hdHotelData").val(1);
            } else {
                $("#hdHotelData").val(0);
            }
            if ($("#lbPublicShow").attr('class') == "checked") {
                $("#hdPublicShow").val(1);
            } else {
                $("#hdPublicShow").val(0);
            }
            if ($("#lbReserveMoney").attr('class') == "checked") {
                $("#hdReserveMoney").val(1);
            } else {
                $("#hdReserveMoney").val(0);
            }            

            $("#checkbox_value").val(CheckboxValues());

            //            if (!confirm('您确认要保存此操作吗？')) {
            //                return false;
            //            }
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

        function OrgSelect(org) {
            var orgid = $(org).val();
            if (orgid != "") {
                GetOrgInfo(orgid);
            }
        }

        function GetOrgInfo(orgid) {
            $("#hdOrgId").val(orgid);
            $.ajax({
                url: "UserInfo.ashx",
                data: {
                    action: "GetOrgSelect",
                    AdminHotelId: $("#AdminHotelid").val(),
                    HotelId: $("#HotelId").val(),
                    orgid: orgid
                },
                type: "POST",
                dataType: "JSON",
                async: false,
                success: function (data) {
                    debugger;
                    if (data != null && data.list.length > 0) {
                        var orgSelect = "<div><select onchange='OrgSelect(this)'><option value=''>==请选择==</option>";
                        for (var i = 0; i < data.list.length; i++) {
                            var orgInfo = data.list[i];
                            orgSelect += "<option value='" + orgInfo.Organization_ID + "'>" + orgInfo.Organization_Name + "</option>";
                        }
                        orgSelect += "</div>";
                        $("#dOrgSelect").html(orgSelect);
                    }
                }
            });
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <%-- 所有打勾复选框值--%>
    <input id="checkbox_value" type="hidden" runat="server" />
    <input id="AdminHotelid" type="hidden" runat="server" />
    <input id="HotelId" type="hidden" runat="server" value="0" />
    <input id="hdUserId" type="hidden" runat="server" value="" />
    <input id="hRestore" type="hidden" runat="server" value="" />
    <input id="hdRoomState" type="hidden" runat="server" value="1" />
    <input id="hdHotelData" type="hidden" runat="server" value="0" />
    <input id="hdPublicShow" type="hidden" runat="server" value="0" />
    <input id="hdReserveMoney" type="hidden" runat="server" value="0" />
    <input id="hdOrgId" type="hidden" runat="server" value="" />
    <div class="addevaluate" style="height: 280px; width: 100%; overflow-y: auto; padding: 18px 42px 18px 0;">
        <dl>
            <dd>
                <small>姓名</small>
                <div>
                    <input id="User_Name" runat="server" type="text" />
                </div>
            </dd>
            <dd>
                <small>手机号码</small>
                <div>
                    <input id="User_Account" runat="server" type="text" />
                </div>
            </dd>
            <dd style="display: none">
                <small>证件类型</small>
                <div>
                    <select id="Credentials_Type" runat="server">
                    </select>
                </div>
            </dd>
            <dd style="display: none">
                <small>证件号</small>
                <div>
                    <input id="Credentials_Number" runat="server" type="text" />
                </div>
            </dd>
            <dd id="dOrg" runat="server">
                <small>部门</small>
                <div>
                    <asp:DropDownList ID="ddlOrg" runat="server" onchange="OrgSelect(this)">
                    </asp:DropDownList>
                    <div id="dOrgSelect" style="margin-top: 5px;">
                    </div>
                </div>
            </dd>
            <dd>
                <small>角色</small>
                <div>
                    <asp:DropDownList ID="ddlUser_Role" runat="server">
                    </asp:DropDownList>
                </div>
            </dd>
            <dd>
                <small>密码</small>
                <div>
                    <input id="User_Pwd" runat="server" type="password" value="" />
                </div>
            </dd>
            <dd>
                <small>PMS工号</small>
                <div>
                    <input id="ygh" runat="server" type="text" value="" />
                </div>
            </dd>
            <dd>
                <small>授权功能</small>
                <div class='checkbox'>
                    <label runat="server" id="lbRoomState">
                        房态房价设置(营销中心)</label>
                    <label runat="server" id="lbHotelData">
                        数据统计(营销中心)</label>
                    <label runat="server" id="lbPublicShow">
                        查看公共奖金(营销中心)</label>
                    <label runat="server" id="lbReserveMoney">
                        充值预留金(营销中心)</label>
                </div>
            </dd>
        </dl>
        <asp:HiddenField ID="hfdID" runat="server" Value="0" />
    </div>
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
