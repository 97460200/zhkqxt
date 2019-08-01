<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserInfo_Form.aspx.cs"
    Inherits="RM.Web.RMBase.SysUser.UserInfo_Form" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>用户信息表单</title>
    <link href="/Themes/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/Validator/JValidator.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/TreeTable/jquery.treeTable.js" type="text/javascript"></script>
    <link href="/Themes/Scripts/TreeTable/css/jquery.treeTable.css" rel="stylesheet"
        type="text/css" />
    <link href="/Themes/Scripts/TreeView/treeview.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/TreeView/treeview.pack.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <style type="text/css">
        .checkAllOff
        {
            padding-left: 10px;
            background: url(/Themes/Images/checkAllOff.gif) no-repeat scroll 0px 2px;
            cursor: pointer;
        }
        .checkAllOn
        {
            padding-left: 10px;
            background: url(/Themes/Images/checkAllOn.gif) no-repeat scroll 0px 2px;
            cursor: pointer;
        }
        .Tabsel
        {
            background: #fff url(/Themes/images/bbgg.jpg) repeat-x left top;
            border-bottom: 1px solid #fff;
        }
        .Tabsel a
        {
            color: #cc0000;
            line-height: 33px;
            margin-top: 1px !important;
        }
        div.Tabsel
        {
            color: #cc0000;
            outline: none;
            line-height: 36px;
            height: 32px;
            margin-top: 7px;
            float: left;
            cursor: pointer;
            font-size: 14px;
            border-bottom: 1px solid #fcfcfc;
            margin-left: 10px;
            background: #fcfcfc url(../images/bbgg.jpg) repeat-x left top;
        }
        div.Tabsel
        {
            background: #fff url(/Themes/images/bbgg.jpg) repeat-x left top;
            border-bottom: 1px solid #fff;
        }
        
        #checkAllOff, #checkAllOn
        {
            width: 12px;
            height: 12px;
            display: inline-block;
            background: url(/Themes/Images/checkAllOff2.png) no-repeat scroll 0 0;
            cursor: pointer;
        }
        #checkAllOn
        {
            background: url(/Themes/Images/checkAllOn2.png) no-repeat scroll 0 0;
        }
        
        
        .frm th
        {
            width: 60px;
        }
    </style>
    <script type="text/javascript">

        //初始化
        $(function () {
            Setform();
            $('#table2').hide();
            $.lightTreeview.close('.strTree ol,.strTree ul')
            $("input[type=checkbox]").click(function () {
                var ck = false;
                if ($(this).attr("checked")) {
                    ck = true;
                }
                $(this).parent("div:eq(0)").next().find("input[type=checkbox]").attr("checked", ck);
            });
        })
        //点击切换面板
        var IsFixedTableLoad = 1;
        function panel(obj) {
            if (obj == 1) {
                $('#table1').show();
                $('#table2').hide();
            }
            else if (obj == 2) {
                $('#table1').hide();
                $("#table2").show();
                $("#dnd-example").treeTable({
                    initialState: "expanded" //collapsed 收缩 expanded 展开的
                });
                if (IsFixedTableLoad == 1) {
                    FixedTableHeader("#dnd-example", $(window).height() - 132);
                    IsFixedTableLoad = 0;
                }
            }
        }
        //附加信息表单赋值
        function Setform() {
            var pk_id = GetQuery('key');
            if (IsNullOrEmpty(pk_id)) {
                var strArray = new Array();
                var strArray1 = new Array();
                var item_value = $("#AppendProperty_value").val(); //后台返回值
                strArray = item_value.split(';');
                for (var i = 0; i < strArray.length; i++) {
                    var item_value1 = strArray[i];
                    strArray1 = item_value1.split('|');
                    $("#" + strArray1[0]).val(strArray1[1]);
                }
            }
        }
        //获取表单值
        function CheckValid() {
            if (!CheckDataValid('#form1')) {
                return false;
            }
            var item_value = '';
            $("#AppendProperty_value").empty;

            $("#AppendProperty_value").val(item_value);
            $("#checkbox_value").val(CheckboxValues())
            if (!confirm('您确认要保存此操作吗？')) {
                return false;
            }
        }

        function CheckboxValues() {
            var reVal = '';
            $('[type = checkbox]').each(function () {
                if ($(this).attr("checked")) {
                    reVal += $(this).val() + ",";
                }
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
    <%--获取附加信息值--%>
    <input id="AppendProperty_value" type="hidden" runat="server" />
    <div class="frmtop">
        <table style="padding: 0px; margin: 0px; height: 100%;" cellpadding="0" cellspacing="0">
            <tr>
                <td id="menutab" style="vertical-align: bottom; padding-left: 15px;">
                    <div id="tab0" class="Tabsel" onclick="GetTabClick(this);panel(1)">
                        <a href="#" class="selected">基本信息</a></div>
                    <div id="tab1" class="Tabremovesel" onclick="GetTabClick(this);panel(2);">
                        <a href="#" class="selected">用户权限</a></div>
                </td>
            </tr>
        </table>
    </div>
    <div class="div-frm" style="height: 295px;">
        <%--基本信息--%>
        <table id="table1" border="0" cellpadding="0" cellspacing="0" class="frm">
            <tr>
                
                <th>
                    所属酒店
                </th>
                <td>
                    <asp:DropDownList ID="DropDownList1" class="select" runat="server" Style="width: 138px">
                    </asp:DropDownList>
                </td>
                <th>
                    角色
                </th>
                <td>
                    <input id="Title" runat="server" type="text" class="txt" style="width: 138px" />
                </td>
            </tr>
            <tr>
                <th>
                    登录帐号
                </th>
                <td>
                    <input id="User_Account" runat="server" type="text" class="txt" datacol="yes" err="登录账户"
                        checkexpession="NotNull" style="width: 138px" />
                </td>
                <th>
                    密码
                </th>
                <td>
                    <input id="User_Pwd" runat="server" type="text" class="txt" datacol="yes" err="登录密码"
                        checkexpession="NotNull" style="width: 138px" />
                </td>
            </tr>
            <tr>
                <th>
                    用户姓名
                </th>
                <td>
                    <input id="User_Name" runat="server" type="text" class="txt" datacol="yes" err="姓名"
                        checkexpession="NotNull" style="width: 138px;" />
                </td>
                <th>
                    性别
                </th>
                <td>
                    <select id="User_Sex" class="select" runat="server" style="width: 138px">
                        <option value="1">男</option>
                        <option value="0">女</option>
                    </select>
                </td>
            </tr>
            <tr>
                <th>
                    联系电话
                </th>
                <td>
                    <input id="Theme" runat="server" type="text" class="txt" style="width: 138px" />
                </td>
                <th>
                    电子邮件
                </th>
                <td>
                    <input id="Email" runat="server" type="text" class="txt" style="width: 138px" />
                </td>
            </tr>
            <tr style="display: none;">
                <th>
                    创建用户
                </th>
                <td>
                    <input id="CreateUserName" disabled runat="server" type="text" class="txt" style="width: 200px" />
                </td>
                <th>
                    创建时间
                </th>
                <td>
                    <input id="CreateDate" disabled runat="server" type="text" class="txt" style="width: 200px" />
                </td>
            </tr>
            <tr style="display: none;">
                <th>
                    修改用户
                </th>
                <td>
                    <input id="ModifyUserName" disabled runat="server" type="text" class="txt" style="width: 200px" />
                </td>
                <th>
                    修改时间
                </th>
                <td>
                    <input id="ModifyDate" runat="server" type="text" class="txt" style="width: 200px" />
                </td>
            </tr>
            <tr>
                <th>
                    备注描述
                </th>
                <td colspan="3">
                    <textarea id="User_Remark" class="txtRemark" runat="server" style="width: 415px;
                        height: 83px;"></textarea>
                </td>
            </tr>
        </table>
        <%--用户权限--%>
        <div id="table2">
            <div class="div-body">
                <table class="example" id="dnd-example">
                    <thead>
                        <tr>
                            <td style="width: 200px; padding-left: 20px;">
                                URL权限
                            </td>
                            <%--  <td style="width: 30px; text-align: center;">
                                图标
                            </td>--%>
                            <td style="width: 20px; text-align: center;">
                                <label id="checkAllOff" onclick="CheckAllLine()" title="全选">
                                    &nbsp;</label>
                            </td>
                            <td>
                                操作按钮权限
                            </td>
                        </tr>
                    </thead>
                    <tbody>
                        <%=strUserRightHtml.ToString()%>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    <div class="frmbottom">
        <asp:LinkButton ID="Save" runat="server" OnClientClick="return CheckValid();" OnClick="Save_Click">
            <span class="bbgg bbgg1">保 存</span>
        </asp:LinkButton>
        <a class="bbgg" href="javascript:void(0)" onclick="OpenClose();"><span>关 闭</span></a>
    </div>
    </form>
</body>
</html>
