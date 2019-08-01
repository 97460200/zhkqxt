<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TiXianList.aspx.cs" Inherits="RM.Web.RMBase.SysSetBase.sales.TiXianList" %>

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
    <script src="../../WDatePicker/WdatePicker.js" type="text/javascript"></script>
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
        });

        //获取地址栏参数
        function getUrlParam(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
            var r = window.location.search.substr(1).match(reg);  //匹配目标参数
            if (r != null) return unescape(r[2]); return null; //返回参数值
        }

        function GetWhere() {
            var UserId = getUrlParam('UserId');
            var strwhere = "";
            var Search = "";
            Search += "UserId@" + UserId + "|";
            var svl = $("#txtStartTime").val();
            if ($.trim(svl) != '' && $.trim(svl) != '0') {
                Search += "addtime@" + svl + "|";
            }

            svl = $("#txtEndTime").val();
            if ($.trim(svl) != '' && $.trim(svl) != '0') {
                Search += "addtime2@" + svl + "|";
            }
            if (Search != "") {
                strwhere += "&Search=" + Search.substr(0, Search.length - 1);
            }
            return strwhere;
        }
        /**加载表格函数**/
        function ListGrid() {

            //url：请求地址
            var url = "salesOrderManager.ashx?Menu=GetTiXianByMemberID" + GetWhere();
            //colM：表头名称
            var colM = [
                { title: "checkbox", width: 60, align: "center", sortable: false,
                    render: function (ui) {
                        var rowData = ui.rowData;

                        return "<div onclick='CheckAllLine3(this)' class='xuanxuan' dateId='" + rowData[0] + "' name='checkbox'></div>";
                    }
                },
                { title: "流水号", width: 220, align: "left",
                    render: function (ui) {
                        var rowData = ui.rowData;
                        return rowData[0];
                    }
                },
                { title: "时间", width: 220, align: "center",
                    render: function (ui) {
                        var rowData = ui.rowData;
                        return rowData[1];
                    }
                },
                { title: "提现金额", width: 220, align: "center",
                    render: function (ui) {
                        var rowData = ui.rowData;
                        return rowData[2];
                    }
                },
                { title: "提现去向", width: 220, align: "center",
                    render: function (ui) {
                        var rowData = ui.rowData;
                        return rowData[3];
                    }
                }
            ];

            //sort：要显示字段,数组对应
            var sort = [
                "ID",
                "wxddh",
                "addtime",
                "money",
                "bz",
                "money"
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
    <div class="bonusrecord03 clearfix">
        <div class="sharedate">
            <input type="text" id="txtStartTime" runat="server" onfocus="WdatePicker({onpicked:function(){ListGrid();}})" />
            <input type="text" id="txtEndTime" runat="server" onfocus="WdatePicker({onpicked:function(){ListGrid();}})" />
        </div>
    </div>
    <div id="grid_paging" style="margin-top: 1px;">
    </div>
    </form>
</body>
</html>
