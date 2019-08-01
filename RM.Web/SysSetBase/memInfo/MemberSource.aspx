<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberSource.aspx.cs" Inherits="RM.Web.SysSetBase.memInfo.MemberSource" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <link href="/Themes/Scripts/PqGrid/style.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/PqGrid/jquery-ui.min.js"></script>
    <script src="/Themes/Scripts/PqGrid/pqgrid.min.js"></script>
    <link href="/Themes/Scripts/PqGrid/pqgrid.min.css" rel="stylesheet" />
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            ListGrid();
        });
        function GetWhere() {
            var strwhere = "";
            var Search = "";
            Search += "openid@" + $("#hdOpenId").val() + "|";
            var svl = $("#txtSearch").val();
            if ($.trim(svl) != '') {
                Search += "text@" + svl + "|";
            }

            if (Search != "") {
                strwhere += "&Search=" + Search.substr(0, Search.length - 1);
            }
            return strwhere;
        }
        /**加载表格函数**/
        function ListGrid() {
            //url：请求地址
            var url = "mem.ashx?Menu=MemberSourceList" + GetWhere();

            //colM：表头名称
            var colM = [
                { title: "主键编号", width: 50, align: "center", hidden: true },
                { title: "酒店名称", width: 160, align: "center" },
                { title: "扫码时间", width: 120, align: "center" },
                { title: "推广员", width: 220, align: "center" }
            ];

            //sort：要显示字段,数组对应
            var sort = [
               "ID",
               "HotelName",
               "AddTime",
               "User_Name"
            ];
            PQLoadGrid("#grid_paging", url, colM, sort, 10, false);
            pqGridResize("#grid_paging", 40, 0);
            var kjheight = $("#grid_paging .pq-grid-header-table").width();
            if ($("#grid_paging").width() < kjheight) {
                pqGridResize("#grid_paging", -20, 0);
            }
            else {
                pqGridResize("#grid_paging", 0, 0);
            }
            $(window).bind({
                resize: function () {
                    if ($("#grid_paging").width() < kjheight) {
                        pqGridResize("#grid_paging", -20, 0);
                    }
                    else {
                        pqGridResize("#grid_paging", 0, 0);
                    }
                }
            })
        }
    </script>
    <script type="text/javascript">
        //回车键
        document.onkeydown = function (e) {
            if (!e) e = window.event; //火狐中是 window.event
            if ((e.keyCode || e.which) == 13) {
                ListGrid()
                return false;
            }
        }       
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <input runat="server" type="hidden" id="hdMemberId" />
    <input runat="server" type="hidden" id="hdOpenId" />
    <div class="gtall gmkf clearfix">
        <div class="bonusrecord">
            <div class="bonusrecord03 clearfix" >
                <div class="sharesearch" style="visibility:hidden;">
                    <asp:TextBox runat="server" ID="txtSearch" placeholder="请输入关键字..."></asp:TextBox>
                    <i class="icon-search" onclick="ListGrid()"></i>
                </div>
                <div class="wdyhd" style="padding-right: 12px;">
                </div>
            </div>
            <div id="grid_paging">
            </div>
        </div>
    </div>
    </form>
</body>
</html>
