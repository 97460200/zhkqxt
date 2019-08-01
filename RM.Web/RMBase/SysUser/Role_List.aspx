<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Role_List.aspx.cs" Inherits="RM.Web.RMBase.SysUser.Role_List" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>角色列表</title>
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
                ListGrid(false);
            });
        });

        function GetWhere() {
            var strwhere = "";
            var Search = "";
            var svl = $("#txtSearch").val();
            if ($.trim(svl) != '') {
                Search += "SearchText@" + svl + "|";
            }
            var Hdhoteladmin = $("#Hdhoteladmin").val();
            var HotelId = $("#hdHotelId").val();
            if (Hdhoteladmin != "") {
                Search += "AdminHotelid@" + Hdhoteladmin + "|" + "Hotel_Id@" + HotelId + "|";
            }

            if (Search != "") {
                strwhere += "&Search=" + Search.substr(0, Search.length - 1);
            }
            return strwhere;
        }
        /**加载表格函数**/
        function ListGrid(load_bl) {
            //url：请求地址
            var url = "/RMBase/SysUser/UserInfo.ashx?action=GetRoleList" + GetWhere();
            //colM：表头名称
            var colM = [
                { title: "主键编号", width: 50, align: "center", hidden: true },
                { title: "角色名称", width: 160, align: "center" },
                { title: "排序", width: 40, align: "center" },
                { title: "备注", width: 80, align: "center" },
                { title: "创建时间", width: 140, align: "center" },
                { title: "操作", editable: false, width: 80, align: "center", sortable: false,
                    render: function (ui) {
                        var rowData = ui.rowData;
                        var px = "<div class='caozuo'>";
                        px += "<span class='icon-edit4' onclick=\"editById('" + rowData[0] + "')\">编辑</span>"
                        //px += "<span class='icon-lbx' onclick=\"DeleteById('" + rowData[0] + "')\">删除</span>";
                        px += "</div>";
                        return px;
                    }
                }
            ];

            //sort：要显示字段,数组对应   
            var sort = [
               "Roles_ID",
               "Roles_Name",
               "SortCode",
               "Roles_Remark",
               "CreateDate"
            ];

            var HotelId = $("#hdHotelId").val();
            if (HotelId == "" || HotelId == "-1") {
                url = "";
            }
            PQLoadGrid("#grid_paging", url, colM, sort, 20, false);
            //                            $("#grid_paging").pqGrid({
            //                                freezeCols: 2, //固定前面列
            //                                title: false
            //                            });
            pqGridResize("#grid_paging", 0, 0);
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
            var url = "/RMBase/SysUser/Role_Info.aspx?HotelId=" + HotelId;
            top.art.dialog.open(url, {
                id: 'Role_Info',
                title: '角色管理 > 添加角色',
                width: 650,
                height: 590,
                close: function () {
                    ListGrid();
                }
            }, false);
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
            var url = "/RMBase/SysUser/Role_Info.aspx?HotelId=" + HotelId + "&roleId=" + key;
            top.art.dialog.open(url, {
                id: 'Role_Info',
                title: '角色管理 > 修改角色',
                width: 650,
                height: 590,
                close: function () {
                    ListGrid();
                }
            }, false);
        }

        // 删除
        function Delete() {
            var rn = GetPqGridRowValue("#grid_paging", 1);
            if (rn == "酒店员工" || rn == "酒店财务" || rn == "酒店经理" || rn == "酒店管理员") {
                showTipsMsg('默认角色不可删除！', '5000', '5');
                return false;
            }
            var key = GetPqGridRowValue("#grid_paging", 0);
            if (IsDelData(key)) {
                DeleteById(key)
            }
        }
        function DeleteById(key) {
            var rn = GetPqGridRowValue("#grid_paging", 1);
            if (rn == "酒店员工" || rn == "酒店财务" || rn == "酒店经理" || rn == "酒店管理员") {
                showTipsMsg('默认角色不可删除！', '5000', '5');
                return false;
            } else {
                var delparm = 'action=ISDelete&module=角色管理系统&tableName=Base_Roles&pkName=Roles_ID&pkVal=' + key;
                delConfig('/Ajax/Common_Ajax.ashx', delparm);
            }
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
    <div class="tools_bar btnbartitle btnbartitlenr" style="display: block;">
        <div>
            当前位置：<span><a title="系统首页" href="/Frame/HomeIndex.aspx">系统首页</a></span><span> &gt;
            </span><span>系统管理</span> &gt; <span>角色管理</span>
        </div>
    </div>
    <div class="gtall gmkf clearfix">
        <div id="HotelTree" runat="server" class="gmkfNav">
            <dl>
                <%=hotelTreeHtml.ToString()%>
            </dl>
        </div>
        <div class="shareright">
            <div class="ptb8">
                <div class="sharesearch">
                    <input type="text" id="txtSearch" value="" placeholder="请输入关键字..." />
                    <i class="icon-search" onclick="ListGrid(false);"></i>
                </div>
                <div class="wdyhd" style="padding-right: 12px;">
                    <div class="r">
                        <span onclick="add()">添加</span><span onclick="edit()">修改</span><span onclick="Delete()">删除</span>
                    </div>
                </div>
            </div>
            <div id="grid_paging" style="margin-top: 1px;">
            </div>
        </div>
    </div>
    </form>
</body>
</html>
