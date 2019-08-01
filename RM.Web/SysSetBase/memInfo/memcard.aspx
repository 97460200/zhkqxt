<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="memcard.aspx.cs" Inherits="RM.Web.SysSetBase.memInfo.memcard" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <link href="/Themes/Scripts/PqGrid/style.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/PqGrid/jquery-ui.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/PqGrid/pqgrid.min.js" type="text/javascript"></script>
    <%--<link href="/Themes/Scripts/PqGrid/pqgrid.min.css" rel="stylesheet" />--%>
    <link href="/Themes/Scripts/PqGrid/pqgrid.min.css" rel="stylesheet" />
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script src="../../WDatePicker/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            ListGrid();
            $(".pq-cont table").live('dblclick', function () {//双击编辑
                //                if ($("#hdMenus").val().indexOf("修改") < 0) {
                //                    showTipsMsg("您没有该功能的操作权限,如有疑问请联系管理员！", 2000, 4);
                //                    return false;
                //                }
                edit();
            });
        });
        function GetWhere() {
            var strwhere = "";
            var Search = "";
            var svl = $("#ddlSearch").val();
            if (svl != '0') {
                Search += "type@" + svl + "|";
            }

            var start = $("#txtStartTime").val();
            var end = $("#txtEndTime").val();

            if ($.trim(start) != '') {
                Search += "start@" + start + "|";
            }
            if ($.trim(end) != '') {
                Search += "end@" + end + "|";
            }

            svl = $("#txtSearch").val();
            if ($.trim(svl) != '') {
                Search += "text@" + svl + "|";
            }
            if (Search != "") {
                strwhere += "&Search=" + Search.substr(0, Search.length - 1);
            }
            //alert(strwhere);
            return strwhere;
        }
        /**加载表格函数**/
        function ListGrid() {

            //url：请求地址
            var url = "mem.ashx?Menu=GetInfoLists" + GetWhere() + "&" + location.search.split("?")[1];
            //colM：表头名称
            var colM = [
                { title: "订单号", width: 160, align: "center" },
                { title: "类型", width: 120, align: "center" },
                { title: "时间", width: 180, align: "center" },
                { title: "充值金额", width: 80, align: "center" },
                { title: "消费金额", width: 80, align: "center" },
            //{ title: "余额", width: 100, align: "center" },
                {title: "备注", width: 200, align: "center" }
            ];

            //sort：要显示字段,数组对应
            var sort = [
               "Number",
               "payname",
               "AddTime",
               "czje",
               "xfje",
               "Detail"
            ];
            PQLoadGrid("#grid_paging", url, colM, sort, 15, false);

            //           $("#grid_paging").pqGrid({
            //               freezeCols: 2, //固定前面列
            //               title: false
            //           });

            pqGridResize("#grid_paging", -20, 0);
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
                <div class="w120">
                    <asp:DropDownList ID="ddlSearch" runat="server">
                        
                    </asp:DropDownList>
                </div>
                <div class="sharedate">
                    <input type="text" id="txtStartTime" runat="server" onfocus="WdatePicker()"/>
                    <input type="text" id="txtEndTime"  runat="server" onfocus="WdatePicker()" />
                </div>
                <div class="sharesearch">
                    <input type="text" name="name" value="" id="txtSearch" placeholder="请输入关键字..." />
                    <i class="icon-search" onclick="ListGrid()" id="btn_search"></i>
                </div>
                <div class="wdyhd" style="padding-right: 12px;">
                    <div class="r">
                        <span onclick="" style=" display:none">导出</span>
                    </div>
                </div>
            </div>

    <div class="gtall gmkf clearfix">
        <div class="bonusrecord">
            <div id="grid_paging">
            </div>
        </div>
    </div>
    </form>
</body>
</html>
