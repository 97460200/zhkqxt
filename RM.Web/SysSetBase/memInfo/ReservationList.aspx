<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReservationList.aspx.cs" Inherits="RM.Web.SysSetBase.memInfo.ReservationList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/Themes/Scripts/PqGrid/pqgrid.min.css" rel="stylesheet" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <link href="/Themes/Scripts/PqGrid/style.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/PqGrid/jquery-ui.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/PqGrid/pqgrid.min.js" type="text/javascript"></script>
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="../../WDatePicker/WdatePicker.js" type="text/javascript"></script>
    <style type="text/css">
        
        input.daochu
        {
            background-position: -410px 0px;
            display: inline-block;
            width: 28px;
            height: 28px;
            background: none;
            cursor: pointer;
            -webkit-appearance: none;
            border: 0;
            z-index: -1;
            opacity: 0;
        }
        
        /* 客房订单-dsx */
span.zffs, span.ddly, span.ddzt{
	background-color: #fff;
	padding: 0 8px;
	height: 22px;
	line-height: 22px;
	text-align: center;
	display: inline-block;
	color: #fff;
}
 span.wxzf{
	background-color: #339900;
}
 span.jfzf{
	background-color: #33CCCC;
}
 span.hykzf{
	background-color: #FF9933;
}
 span.qtzf{
	background-color: #D6487E;
}

 span.ygxs{
	background-color: #3A87AD;
}
 span.gw{
	background-color: #FF9933;
}

 span.wqr{
	background-color: #CC0000;
}
 span.yqr{
	background-color: #3A87AD;
}
 span.yqx{
	background-color: #E7E7E7;
	color: #999;
}
 span.yrz{
	background-color: #339900;
}
    </style>
    <script type="text/javascript">
        $(function () {
            ListGrid();
            


            //获取权限
            $.post("../../Ajax/Common_Ajax.ashx?action=GetMenus&pkName=订单管理", function (date) {
                if (date != "1") {
                    $("#hdMenus").val(date);
                } else {
                    showTipsMsg("登陆超时！", 2000, 4);
                    top.location = '/Frame/Login.htm';
                }
            });
            $(".pq-cont table").live('dblclick', function () {//双击编辑
                //                if ($("#hdMenus").val().indexOf("修改") < 0) {
                //                    showTipsMsg("您没有该功能的操作权限,如有疑问请联系管理员！", 2000, 4);
                //                    return false;
                //                }
                //edit();
            });
        });
        function GetWhere() {
            var strwhere = "";
            var Search = "";
            var svl = $("#ddlSearch").val();
            if (svl != '0') {
                Search += "Pay@" + svl + "|";
            }

            if ($("#ddlBranchList").val() != "")
                Search += "branch@" + $("#ddlBranchList").val() + "|";

            if ($("#DDLRoomList").val() != "")
                Search += "roomType@" + $("#DDLRoomList").val() + "|";

            svl = $("#txtSearch").val();
            if ($.trim(svl) != '') {
                Search += "ordernum@" + svl + "|";
            }
            if (Search != "") {
                strwhere += "&Search=" + Search.substr(0, Search.length - 1);
            }
            return strwhere;
        }
        /**加载表格函数**/
        function ListGrid() {
            //url：请求地址
            var url = "mem.ashx?Menu=GetInfoListss" + GetWhere() + "&" + location.search.split("?")[1];
            //colM：表头名称
            var colM = [
                
                { title: "订单号", width: 160, align: "center" },
                
                { title: "酒店名称", width: 200, align: "center" },
                { title: "预订房型", width: 200, align: "center" },
                { title: "入住时间", width: 80, align: "center" },
                { title: "离店时间", width: 80, align: "center" },
                { title: "房数", width: 30, align: "center" },
                { title: "联系人", width: 80, align: "center" },
                { title: "手机号码", width: 80, align: "center" },
                { title: "下单时间", width: 120, align: "center" },
                {
                    title: "订单总额", width: 80, align: "center",
                    render: function (ui) {

                        var rowData = ui.rowData[10];
                        var rowData = rowData.toString().substring(0, rowData.lastIndexOf('.'));
                        var px = rowData;
                        return "￥ " + px;
                    }

                },
                {
                    title: "优惠", width: 60, align: "center",
                    render: function (ui) {

                        var rowData = ui.rowData[11];

                        var px = rowData;
                        return "￥ " + px;
                    }

                },
                {
                    title: "应付", width: 60, align: "center",
                    render: function (ui) {

                        var rowData = ui.rowData[12];
                        var rowData = rowData.toString().substring(0, rowData.lastIndexOf('.'));
                        var px = rowData;
                        return "￥ " + px;
                    }

                },
                { title: "订单来源", width: 80, align: "center",
                    render: function (ui) {

                        var rowData = ui.rowData[9];

                        if (rowData == "员工销售") return "<span class='ddly ygxs'>员工销售</span>";
                        if (rowData == "官微") return "<span class='ddly gw'>官微</span>";
                    }
                },
                { title: "支付方式", width: 100, align: "center",
                    render: function (ui) {

                        var rowData = ui.rowData[13];

                        if (rowData == "微信支付") return "<span class='zffs wxzf'>微信支付</span>";
                        if (rowData == "会员卡支付") return "<span class='zffs hykzf'>会员卡支付</span>";
                        if (rowData == "积分兑换") return "<span class='zffs jfzf'>积分兑换</span>";
                        if (rowData == "到店付款") return "<span class='zffs qtzf'>到店付款</span>";

                    }
                },
                { title: "订单状态", width: 100, align: "center",
                    render: function (ui) {

                        var rowData = ui.rowData[14];

                        if (rowData == "已入住") return "<span class='ddzt yrz'>已入住</span>";
                        if (rowData == "已取消") return "<span class='ddzt yqx'>已取消</span>";
                        if (rowData == "已确认") return "<span class='ddzt yqr'>已确认</span>";
                        if (rowData == "未确认") return "<span class='ddzt wqr'>未确认</span>";

                    }
                }
            //                { title: "支付状态", width: 80, align: "center" },<span class="ddzt yrz">已入住</span>

                
            ];

            //sort：要显示字段,数组对应
            var sort = [
               "OrderNum",
               "OrderNum",
               "hotelname",
               "RoomType",
               "BeginTime",
               "EndTime",
               "Number",
               "Name",
               "Mobile",
               "AddTime",
               "Distributor",
               "ServiceTotal",
               "zip",
               "TomePrice",
               "payname",
               "State"

            ];
            PQLoadGrid("#grid_paging", url, colM, sort, 15, false);
//            $("#grid_paging").pqGrid({
//                freezeCols: 2, //固定前面列
//                title: false
//            });
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
    <script type="text/javascript">
        //回车键
        document.onkeydown = function (e) {
            if (!e) e = window.event; //火狐中是 window.event
            if ((e.keyCode || e.which) == 13) {
                ListGrid()
                return false;
            }
        }

        
        //修改
        function edit() {
            //            if ($("#hdMenus").val().indexOf("修改") < 0) {
            //                showTipsMsg("您没有该功能的操作权限,如有疑问请联系管理员！", 2000, 4);
            //                return false;
            //            }
            //var key = GetPqGridRowValue("#grid_paging", 0);

            var key = GetPqGridRowValue("#grid_paging", 0);
            if (IsEditdata(key)) {
                editById(key);
            }
        }
        function editById(key) {
            //            if ($("#hdMenus").val().indexOf("修改") < 0) {
            //                showTipsMsg("您没有该功能的操作权限,如有疑问请联系管理员！", 2000, 4);
            //                return false;
            //            }
            var url = "/RMBase/SysOrder/ReservationWiths.aspx?Id=" + key;
            top.art.dialog.open(url, {
                id: 'ReservationWith',
                title: '订单管理 > 查看订单',
                width: 760,
                height: 590,
                close: function () {
                    ListGrid();
                }
            }, false);
        }
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="gtall gmkf clearfix">
        <div class="bonusrecord">
            <div class="bonusrecord03 clearfix">
                <div class="w120" style=" display:none">
                    <select>
                        <option value="value">全部类型</option>
                        <option value="value">选项01</option>
                        <option value="value">选项01</option>
                    </select>
                </div>
                <div class="sharedate">
                    <input type="text" id="txtStartTime" runat="server" onfocus="WdatePicker()"/>
                    <input type="text" id="txtEndTime"  runat="server" onfocus="WdatePicker()" />
                </div>
                <div class="sharesearch">
                    <input type="text" name="name" value="" id="txtSearch" placeholder="请输入关键字..." />
                    <i class="icon-search" onclick="ListGrid()" ></i>
                </div>
                <div class="wdyhd" style="padding-right: 12px;">
                    <div class="r">
                        <span onclick="" style=" display:none">导出</span>
                    </div>
                </div>
            </div>
            <div id="grid_paging" >
            </div>
        </div>
    </div>
    </form>
</body>
</html>
