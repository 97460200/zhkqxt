<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Staff_List.aspx.cs" Inherits="RM.Web.RMBase.SysUser.Staff_List" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>员工列表</title>
    <meta name="viewport" content="width=device-width, user-scalable=no" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <link href="/Themes/Scripts/PqGrid/style.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/PqGrid/jquery-ui.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/PqGrid/pqgrid.min.js" type="text/javascript"></script>
    <link href="/Themes/Scripts/PqGrid/pqgrid.min.css" rel="stylesheet" />
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            GetOrgInfo("0");
            ListGrid(true);
            $(".pq-cont table").live('dblclick', function () {//双击编辑
                edit();
            });
            //左边导航
            $('.gmkfNav').on('click', 'b', function () {
                $(this).siblings('ul').slideToggle(120);
                $(this).parents('dd').toggleClass('down');
            });
            $('.gmkfNav ul').on('click', 'li', function () {
                $(this).addClass("active").siblings().removeClass("active");
                $("#Hdhoteladmin").val($(this).attr("AdminHotelId"));
                $("#hdHotelId").val($(this).attr("HotelId"));
                $("#dUser").show();

                $(".org").remove();
                GetOrgInfo("0");
                ListGrid(false);
            });
        });

        function OrgSelect(sel) {
            var orgid = $(sel).val();
            var num = parseInt($(sel).parent().attr("number"));
            $(".org[number=" + (num + 1) + "]").remove();
            GetOrgInfo(orgid);
        }

        function GetOrgInfo(orgid) {
            var HotelId = $("#hdHotelId").val();
            if (HotelId == "" || HotelId == "-1") {
                return;
            }
            $("#hdOrgId").val(orgid);
            if (orgid == "") {
                return;
            }
            $.ajax({
                url: "UserInfo.ashx",
                data: {
                    action: "GetOrgSelect",
                    AdminHotelId: $("#Hdhoteladmin").val(),
                    HotelId: $("#hdHotelId").val(),
                    orgid: orgid
                },
                type: "POST",
                dataType: "JSON",
                async: false,
                success: function (data) {
                    if (data != null && data.list.length > 0) {
                        var orgSelect = "<div number='" + $(".org").length + "' class='w120 org'><select onchange='OrgSelect(this)'><option value=''>==请选择==</option>";
                        for (var i = 0; i < data.list.length; i++) {
                            var orgInfo = data.list[i];
                            orgSelect += "<option value='" + orgInfo.Organization_ID + "'>" + orgInfo.Organization_Name + "</option>";
                        }
                        orgSelect += "</div>";
                        $("#searchRole").before(orgSelect);
                    }
                }
            });
        }

        function lzxx(obj) {
            if ($(obj).html() == "离职用户") {
                $("#hDeleteMark").val("0");
                $(obj).html("在职用户");
                $("#lz").hide();
                $("#tj").hide();
                $("#xg").hide();
            } else {
                $("#hDeleteMark").val("1");
                $(obj).html("离职用户");
                $("#lz").show();
                $("#tj").show();
                $("#xg").show();
            }
            ListGrid(false);
        }

        function GetWhere() {
            var strwhere = "";
            var Search = "";
            var svl = $("#txtSearch").val();
            if ($.trim(svl) != '') {
                Search += "SearchText@" + svl + "|";
            }
            if ($("#Credentials_Type").val() != '') {
                Search += "Credentials_Type@" + $("#Credentials_Type").val() + "|";
            }
            var Hdhoteladmin = $("#Hdhoteladmin").val();
            var HotelId = $("#hdHotelId").val();
            if (Hdhoteladmin != "") {
                Search += "AdminHotelid@" + Hdhoteladmin + "|" + "HotelId@" + HotelId + "|";
            }
            var hDeleteMark = $("#hDeleteMark").val();
            if (hDeleteMark != "") {
                Search += "DeleteMark@" + hDeleteMark + "|";
            }
            var orgid = $("#hdOrgId").val();
            if (orgid != "" && orgid != "0") {
                Search += "orgid@" + orgid + "|";
            }
            var role_id = $("#selRole").val();
            if (role_id != "" && role_id != "0") {
                Search += "Roles_Name@" + role_id + "|";
            }
            if (Search != "") {
                strwhere += "&Search=IsAdmin@2|" + Search.substr(0, Search.length - 1);
            }
            return strwhere;
        }
        /**加载表格函数**/
        function ListGrid(load_bl) {
            //url：请求地址
            var url = "/RMBase/SysUser/UserInfo.ashx?action=GetUserRoleList" + GetWhere();
            //colM：表头名称
            var colM = [
                { title: "主键编号", width: 50, align: "center", hidden: true },
                { title: "真实姓名", width: 80, align: "center" },
                { title: "用户名", width: 100, align: "center" },
                { title: "手机号码", width: 100, align: "center" },
            //                { title: "证件类型", width: 90, align: "center" },
            //                { title: "证件号码", width: 150, align: "center" },
                {title: "微信昵称", width: 100, align: "center" },
                { title: "部门职位", width: 200, align: "center" },
                { title: "角色", width: 100, align: "center" },
                { title: "状态", width: 80, align: "center" },
                { title: "创建时间", width: 140, align: "center" },
                { title: "操作", editable: false, width: 160, align: "center", sortable: false,
                    render: function (ui) {
                        var rowData = ui.rowData;
                        var px = "";
                        if ($("#hDeleteMark").val() == "1") {
                            px += "<span  class='icon-edit4' onclick=\"editById('" + rowData[0] + "')\">修改</span>"
                            px += "<span  class='icon-lbx' onclick=\"DeleteById('" + rowData[0] + "','" + rowData[1] + "')\">离职</span>";
                            if (rowData[3] != "") {
                                px += "<span  class='icon-edit4' onclick=\"UntieById('" + rowData[0] + "','" + rowData[1] + "')\">解绑</span>"
                            }
                        } else {
                            px += "<span  class='icon-edit4' onclick=\"UserEntry('" + rowData[0] + "','" + rowData[1] + "')\">入职</span>";
                        }
                        return px;
                    }
                }
            ];

            //sort：要显示字段,数组对应   
            var sort = [
               "User_ID",
               "User_Name",
               "User_Account",
               "User_Phone",
            //               "Credentials_Type",
            //               "Credentials_Number",
               "WX_Nickname",
               "OrgName",
               "Roles_Name",
               "Mark",
               "CreateDate"
            ];
            var HotelId = $("#hdHotelId").val();
            if (HotelId == "" || HotelId == "-1") {
                url = "";
            }
            PQLoadGrid("#grid_paging", url, colM, sort, 20, false);
            //pqGridResize("#grid_paging", 0, 0);
            var kjheight = $("#grid_paging .pq-grid-header-table").width();
            if ($("#grid_paging").width() < kjheight) {
                pqGridResize("#grid_paging", -65, 0);
            }
            else {
                pqGridResize("#grid_paging", -45, 0);
            }
            $(window).bind({
                resize: function () {
                    if ($("#grid_paging").width() < kjheight) {
                        pqGridResize("#grid_paging", -65, 0);
                    }
                    else {
                        pqGridResize("#grid_paging", -45, 0);
                    }
                }
            })
        }
    </script>
    <script type="text/javascript">
        //添加
        function add() {
            var HotelId = $("#hdHotelId").val();
            if (HotelId == "" || HotelId == "-1") {
                showTipsMsg('请先选择所属酒店！', '5000', '5');
                return false;
            }
            var url = "/RMBase/SysUser/UserInfo_Info.aspx?HotelId=" + HotelId;
            top.openDialog(url, 'UserInfo_Info', '添加酒店用户', 350, 330, 50, 50);
        }
        //修改
        function edit() {
            var key = GetPqGridRowValue("#grid_paging", 0);
            if (IsEditdata(key)) {
                editById(key);
            }
        }

        function editById(key) {
            var HotelId = $("#hdHotelId").val();
            if (HotelId == "" || HotelId == "-1") {
                showTipsMsg('请先选择所属酒店！', '5000', '5');
                return false;
            }
            var url = "/RMBase/SysUser/UserInfo_Info.aspx?HotelId=" + HotelId + "&UserId=" + key;
            top.openDialog(url, 'UserInfo_Info', '修改酒店用户', 350, 330, 50, 50);
        }

        function UserEntry(key, name) {
            var HotelId = $("#hdHotelId").val();
            if (HotelId == "" || HotelId == "-1") {
                showTipsMsg('请先选择所属酒店！', '5000', '5');
                return false;
            }
            var url = "/RMBase/SysUser/UserEntry.aspx?HotelId=" + HotelId + "&UserId=" + key + "&name=" + name;
            top.openDialog(url, 'UserEntry', '用户入职', 350, 205, 50, 50);
        }

        // 离职
        function Delete() {
            var key = GetPqGridRowValue("#grid_paging", 0);
            if (IsDelData(key)) {
                DeleteById(key)
            }
        }

        //查看二维码 
        function lookErWeiMa(key, wxName) {
            var url = "/SysSetBase/sales/code_imgnew01.aspx?id=" + key + "&wxName=" + wxName;
            top.art.dialog.open(url, {
                id: 'code_imgnew01',
                title: '查看推广码',
                width: 640,
                height: 560,
                close: function () {
                    ListGrid();
                }
            }, false);
        }

        function DeleteById(key, name) {
            showConfirmMsg("员工离职未提现奖金将退回营销费中，是否现在离职？&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", function (r) {
                if (r) {
                    var url = "/RMBase/SysUser/UserInfo.ashx";
                    var parm = { action: "Dimission", user_ID: key, user_name: name };
                    getAjax(url, parm, function (rs) {
                        if (rs.toLocaleLowerCase() == 'true') {
                            showTipsMsg("操作成功", 2000, 4);
                            ListGrid();
                        } else if (rs.toLocaleLowerCase() == 'false') {
                            showTipsMsg("操作失败，请稍后重试", 4000, 5);
                        } else {
                            showTipsMsg(rs, 4000, 3);
                        }
                    });
                }
            });
        }
        //解绑
        function UntieById(key, name) {
            showConfirmMsg("您确定要解除该员工绑定的微信吗？&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", function (r) {
                if (r) {
                    var url = "/RMBase/SysUser/UserInfo.ashx";
                    var parm = { action: "UntieById", user_ID: key, user_name: name };
                    getAjax(url, parm, function (rs) {
                        if (rs.toLocaleLowerCase() == 'true') {
                            showTipsMsg("操作成功", 2000, 4);
                            ListGrid();
                        } else if (rs.toLocaleLowerCase() == 'false') {
                            showTipsMsg("操作失败，请稍后重试", 4000, 5);
                        } else {
                            showTipsMsg(rs, 4000, 3);
                        }
                    });
                }
            });
        }
        //回车键
        document.onkeydown = function (e) {
            if (!e) e = window.event; //火狐中是 window.event
            if ((e.keyCode || e.which) == 13) {
                ListGrid(false);
            }
        }

        function Import() {
            var HotelId = $("#hdHotelId").val();
            if (HotelId == "" || HotelId == "-1") {
                showTipsMsg('请先选择所属酒店！', '5000', '5');
                return false;
            }
            var url = "/RMBase/SysUser/ImportUsers.aspx?HotelId=" + HotelId;
            //top.openDialog(url, 'ImportUsers', '用户导入', 400, 270, 50, 50);
            top.art.dialog.open(url, {
                id: '用户导入',
                title: '用户导入',
                width: 400,
                height: 270,
                close: function () {
                    ListGrid();
                }
            }, false);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <input runat="server" type="hidden" id="Hdhoteladmin" value="1" />
    <input runat="server" type="hidden" id="hdHotelId" value="-1" />
    <input runat="server" type="hidden" id="htHotelTree" value="true" />
    <input runat="server" type="hidden" id="hDeleteMark" value="1" />
    <input runat="server" type="hidden" id="hdOrgId" value="" />
    <div class="tools_bar btnbartitle btnbartitlenr" style="display: block;">
        <div>
            当前位置：<span><a title="系统首页" href="/Frame/HomeIndex.aspx">系统首页</a></span><span> &gt;
            </span><span>系统用户</span> &gt; <span>酒店用户管理</span>
        </div>
    </div>
    <div class="gtall gmkf clearfix">
        <div id="HotelTree" runat="server" class="gmkfNav">
            <dl>
                <%=hotelTreeHtml.ToString()%>
            </dl>
        </div>
        <div id="dUser" class="shareright">
            <div class="ptb8">
                <div class="w120">
                    <select id="Credentials_Type" runat="server" onchange="ListGrid(false)" style="display: none;">
                        <option value="value">证件类型</option>
                        <option value="value">选项01</option>
                        <option value="value">选项01</option>
                    </select>
                </div>
                <div id="searchRole" class="w120">
                    <select id="selRole" runat="server">
                        <option value="">==选择角色==</option>
                        <option value="酒店员工">酒店员工</option>
                        <option value="酒店经理">酒店经理</option>
                        <option value="酒店财务">酒店财务</option>
                        <option value="酒店管理员">酒店管理员</option>
                    </select>
                </div>
                <div class="sharesearch">
                    <input type="text" id="txtSearch" value="" placeholder="请输入关键字..." />
                    <i class="icon-search" onclick="ListGrid(false);"></i>
                </div>
                <div class="wdyhd" style="padding-right: 12px;">
                    <div class="r">
                        <%--style="display: none;"--%>
                        <span onclick="add()" id="tj">添加</span><span onclick="edit()" id="xg">修改</span><span
                            onclick="Delete()" id="lz">离职</span><span onclick="lzxx(this)">离职用户</span> <span
                                onclick="Import()" id="dr">导入</span>
                    </div>
                </div>
            </div>
            <div id="grid_paging" style="margin-top: 1px;" class="grid_paging">
            </div>
        </div>
    </div>
    </form>
</body>
</html>
