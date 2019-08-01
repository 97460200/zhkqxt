<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StoreStaff_List.aspx.cs"
    Inherits="RM.Web.RMBase.SysUser.StoreStaff_List" %>

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
            DefaultRole();
            ListGrid(true);
            $(".pq-cont table").live('dblclick', function () {//双击编辑
                if ($("#dUser").is(":hidden")) {
                    edit_role();
                } else {
                    edit();
                }
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
                $("#dRole").hide();
                DefaultRole();
                ListGrid(false);
            });
        });

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
            DefaultRole();
            ListGrid(false);
        }

        function DefaultRole() {
            var aid = $("#Hdhoteladmin").val();
            var hId = $("#hdHotelId").val();
            $.post("UserInfo.ashx", {
                action: "DefaultRole",
                AdminHotelid: aid,
                HotelId: hId
            }, function (date) {
            });
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
                { title: "手机号码", width: 90, align: "center" },
                { title: "真实姓名", width: 90, align: "center" },
            //                { title: "证件类型", width: 90, align: "center" },
            //                { title: "证件号码", width: 150, align: "center" },
                {title: "微信昵称", width: 110, align: "center" },
                { title: "角色", width: 120, align: "center" },
                { title: "状态", width: 120, align: "center" },
                { title: "创建时间", width: 160, align: "center" },
                { title: "操作", editable: false, width: 120, align: "center", sortable: false,
                    render: function (ui) {
                        var rowData = ui.rowData;
                        var px = "";
                        if ($("#hDeleteMark").val() == "1") {
                            px += "<span  class='icon-edit4' onclick=\"editById('" + rowData[0] + "')\">修改</span>"
                            px += "<span  class='icon-lbx' onclick=\"DeleteById('" + rowData[0] + "')\">离职</span>";
                        } else {
                            px += "<span  class='icon-edit4' onclick=\"editByIds('" + rowData[0] + "')\">入职</span>";
                        }
                        return px;
                    }
                }
            ];

            //sort：要显示字段,数组对应   
            var sort = [
               "User_ID",
               "User_Account",
               "User_Name",
            //               "Credentials_Type",
            //               "Credentials_Number",
               "WX_Nickname",
               "Roles_Name",
               "Mark",
               "CreateDate"
            ];
            var HotelId = $("#hdHotelId").val();
            if (HotelId == "" || HotelId == "-1") {
                url = "";
            }

            PQLoadGrid("#grid_paging", url, colM, sort, 20, false);
            pqGridResize("#grid_paging", 0, 0);
        }
    </script>
    <script type="text/javascript">
        //添加
        function add() {
            var HotelId = $("#hdHotelId").val();
            if (HotelId == "" || HotelId == "-1") {
                showTipsMsg('请先选择所属门店！', '5000', '5');
                return false;
            }
            var url = "/RMBase/SysUser/UserInfo_Info.aspx?HotelId=" + HotelId;
            top.openDialog(url, 'UserInfo_Info', '添加门店用户', 350, 310, 50, 50);
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
                showTipsMsg('请先选择所属门店！', '5000', '5');
                return false;
            }
            var url = "/RMBase/SysUser/UserInfo_Info.aspx?HotelId=" + HotelId + "&UserId=" + key;
            top.openDialog(url, 'UserInfo_Info', '修改门店用户', 350, 310, 50, 50);
        }

        function editByIds(key) {
            showConfirmMsg("请确认要设置入职吗?&nbsp;&nbsp;", function (r) {
                if (r) {
                    var url = "/RMBase/SysUser/UserInfo.ashx";
                    var parm = { action: "Entry", user_ID: key };
                    getAjax(url, parm, function (rs) {
                        if (rs.toLocaleLowerCase() == 'true') {
                            showTipsMsg("操作成功", 2000, 4);
                            ListGrid(true);
                        } else if (rs.toLocaleLowerCase() == 'false') {
                            showTipsMsg("操作失败，请稍后重试", 4000, 5);
                        } else {
                            showTipsMsg(rs, 4000, 3);
                        }
                    });
                }
            });
        }

        // 删除
        function Delete() {
            var key = GetPqGridRowValue("#grid_paging", 0);
            if (IsDelData(key)) {
                DeleteById(key)
            }
        }

        function DeleteById(key) {
            var delparm = 'action=ISDelete&module=员工管理系统&tableName=Base_UserInfo&pkName=User_ID&pkVal=' + key;
            delConfig('/Ajax/Common_Ajax.ashx', delparm);
        }
        //回车键
        document.onkeydown = function (e) {
            if (!e) e = window.event; //火狐中是 window.event
            if ((e.keyCode || e.which) == 13) {
                ListGrid(false);
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <input runat="server" type="hidden" id="Hdhoteladmin" value="1" />
    <input runat="server" type="hidden" id="hdHotelId" value="-1" />
    <input runat="server" type="hidden" id="htHotelTree" value="true" />
    <input runat="server" type="hidden" id="hDeleteMark" value="1" />
    <div class="tools_bar btnbartitle btnbartitlenr" style="display: block;">
        <div>
            当前位置：<span><a title="系统首页" href="/Frame/HomeIndex.aspx">系统首页</a></span><span> &gt;
            </span><span>系统用户</span> &gt; <span>门店用户管理</span>
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
                <div class="sharesearch">
                    <input type="text" id="txtSearch" value="" placeholder="请输入关键字..." />
                    <i class="icon-search" onclick="ListGrid(false);"></i>
                </div>
                <div class="wdyhd" style="padding-right: 12px;">
                    <div class="r">
                        <%--<span onclick="Roles()">角色管理</span>--%>
                        <span onclick="add()" id="tj">添加</span><span onclick="edit()" id="xg">修改</span><span
                            onclick="Delete()" id="lz">删除</span><span onclick="lzxx(this)">离职用户</span>
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
