<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DaiLiKeHu.aspx.cs" Inherits="RM.Web.RMBase.SysSetBase.sales.DaiLiKeHu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/PqGrid/jquery-ui.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/PqGrid/pqgrid.min.js" type="text/javascript"></script>
    <link href="/Themes/Scripts/PqGrid/pqgrid.min.css" rel="stylesheet" />
    <link href="../css/reset.css" rel="stylesheet" type="text/css" />
    <link href="../css/backer.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .rge .rgetable
        {
            border: none;
        }
        .rge .rgetable th, .rge .rgetable td
        {
            background: none;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            ListGrid();
            $(".pq-cont table").live('dblclick', function () {//双击编辑
                var key = GetPqGridRowValue("#grid_paging", 0);
                editByck(key);
            });

            $("#ddlSearch").change(function () {
                ListGrid();
            });
        });
        //获取地址栏参数
        function getUrlParam(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
            var r = window.location.search.substr(1).match(reg);  //匹配目标参数
            if (r != null) return unescape(r[2]); return null; //返回参数值
        }
        var UserId = "";

        function GetWhere() {
            UserId = getUrlParam('UserId');
            var strwhere = "";
            var Search = "";
            Search += "Parent_ID@" + UserId + "|";

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
            var url = "salesOrderManager.ashx?Menu=GetDaiLaiKeHuByMemberID" + GetWhere();
            //colM：表头名称
            var colM = [
                { title: "主键编号", width: 50, align: "center", hidden: true },
                { title: "手机号码", width: 90, align: "center" },
                { title: "姓名", width: 90, align: "center" },
                { title: "会员级别", width: 90, align: "center" },
                { title: "带来时间", width: 120, align: "center" },
                { title: "来源", width: 80, align: "center" }
            ];

            //sort：要显示字段,数组对应
            var sort = [
                "id",
                "sjhm",
                "xm",
                "hylxname",
                "addtime",
                "hy_from"
            ];
            PQLoadGrid("#grid_paging", url, colM, sort, 10, false);
            pqGridResize("#grid_paging", 0, 0);
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
</head>
<body>
    <form id="form1" runat="server">
    <div class="bonusrecord">
        <div class="bonusrecord03 clearfix">
            <div class="sharesearch">
                <input type="text" id="txtSearch" value="" placeholder="请输入手机号码..." />
                <i class="icon-search" onclick="ListGrid();"></i>
            </div>
        </div>
        <div id="grid_paging" style="margin-top: 1px;">
        </div>
    </div>
    </form>
</body>
</html>
