<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin_List.aspx.cs" Inherits="RM.Web.RMBase.SysUser.Admin_List" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>管理员列表</title>
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
            ListGrid();
            $(".pq-cont table").live('dblclick', function () {//双击编辑
                edit();
            });
        });

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
            if (Hdhoteladmin != "") {
                Search += "AdminHotelid@" + Hdhoteladmin + "|";
            }

            if (Search != "") {
                strwhere += "&Search=IsAdmin@1|" + Search.substr(0, Search.length - 1);
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
                { title: "证件类型", width: 90, align: "center" },
                { title: "证件号码", width: 150, align: "center" },
                { title: "创建时间", width: 110, align: "center" },
                { title: "操作", editable: false, width: 120, align: "center", sortable: false,
                    render: function (ui) {
                        var rowData = ui.rowData;
                        var px = "";
                        px += "<span  class='icon-edit4' onclick=\"editById('" + rowData[0] + "')\">修改</span>"
                        //px += "<span  class='icon-lbx' onclick='DeleteById(" + rowData[0] + ")'>删除</span>";
                        return px;
                    }
                }
            ];

            //sort：要显示字段,数组对应   
            var sort = [
               "User_ID",
               "User_Account",
               "User_Name",
               "Credentials_Type",
               "Credentials_Number",
               "CreateDate"
            ];
            PQLoadGrid("#grid_paging", url, colM, sort, 20, false);
            pqGridResize("#grid_paging", 0, 0);
        }
    </script>
    <script type="text/javascript">
        //添加
        function add() {
            var url = "/RMBase/SysUser/Admin_Info.aspx";
            top.art.dialog.open(url, {
                id: 'Admin_Info',
                title: '添加管理员',
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
            var url = "/RMBase/SysUser/Admin_Info.aspx?UserId=" + key;
            top.art.dialog.open(url, {
                id: 'Admin_Info',
                title: '修改管理员',
                width: 650,
                height: 590,
                close: function () {
                    ListGrid();
                }
            }, false);
        }

        // 删除
        function Delete() {
            var key = GetPqGridRowValue("#grid_paging", 0);
            if (IsDelData(key)) {
                DeleteById(key)
            }
        }
        function DeleteById(key) {
            var delparm = 'action=ISDelete&module=用户管理系统&tableName=Base_UserInfo&pkName=User_ID&pkVal=' + key;
            delConfig('/Ajax/Common_Ajax.ashx', delparm);
        }
        //回车键
        document.onkeydown = function (e) {
            if (!e) e = window.event; //火狐中是 window.event
            if ((e.keyCode || e.which) == 13) {
                ListGrid();
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <input runat="server" type="hidden" id="Hdhoteladmin" value="1" />
    <input runat="server" type="hidden" id="hdHotelId" value="-1" />
    <div class="tools_bar btnbartitle btnbartitlenr" style="display: block;">
        <div>
            当前位置：<span><a title="系统首页" href="/Frame/HomeIndex.aspx">系统首页</a></span><span> &gt;
            </span><span>系统用户</span></span>
        </div>
    </div>
    <div class="gtall gmkf clearfix">
        <div class="shareright">
            <div class="ptb8">
                <div class="w120">
                    <select id="Credentials_Type" runat="server" onchange="ListGrid()">
                        <option value="value">证件类型</option>
                        <option value="value">选项01</option>
                        <option value="value">选项01</option>
                    </select>
                </div>
                <div class="sharesearch">
                    <input type="text" id="txtSearch" value="" placeholder="请输入关键字..." />
                    <i class="icon-search" onclick="ListGrid();"></i>
                </div>
                <div class="wdyhd" style="padding-right: 12px;">
                    <div class="r">
                        <span onclick="add()">添加</span><span onclick="edit()">修改</span><span onclick="Delete()"
                            style="display: none;">删除</span>
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
