<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Recharge.aspx.cs" Inherits="RM.Web.SysSetBase.memInfo.Recharge" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>会员卡管理</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <link href="../../Themes/Scripts/PqGrid/style.css" rel="stylesheet" type="text/css" />
    <script src="../../Themes/Scripts/PqGrid/jquery-ui.min.js"></script>
    <script src="../../Themes/Scripts/PqGrid/pqgrid.min.js"></script>
    <link href="../../Themes/Scripts/PqGrid/pqgrid.min.css" rel="stylesheet" />
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="../../Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script src="../../WDatePicker/WdatePicker.js" type="text/javascript"></script>
</head>
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

        var start = $("#txtStartTime").val();
        var end = $("#txtEndTime").val();

        if ($.trim(start) != '') {
            Search += "start@" + start + "|";
        }
        if ($.trim(end) != '') {
            Search += "end@" + end + "|";
        }

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
        var url = "mem.ashx?Menu=GetInfoListsR" + GetWhere();
        //colM：表头名称
        var colM = [
                { title: "订单号", width: 160, align: "center" },
                { title: "会员卡号", width: 100, align: "center" },
                { title: "姓名", width: 120, align: "center" },
                { title: "手机号码", width: 100, align: "center" },
                { title: "金额", width: 80, align: "center" },

                { title: "充值类型", editable: false, width: 100, align: "center", sortable: false,
                     render: function (ui) {
                         var rowData = ui.rowData;
                         var px = "";
                         if (rowData[5] == "4") {
                           px += "会员卡充值"
                         }else if (rowData[5] == "41") {
                           px += "充值退款-原路退回";
                         }else if (rowData[5] == "42") {
                         px += "充值退款-现金退款";
                         }else if (rowData[5] == "44") {
                         px += "后台充值会员卡";
                         }else if (rowData[5] == "45") {
                         px += "后台会员卡扣费";
                         }else {
                             px += "-";
                         }
                         return px;
                     }
                 },
                { title: "备注", width: 120, align: "center" },
                { title: "微信交易单号", width: 220, align: "center" },
                { title: "时间", width: 130, align: "center" },
                { title: "操作", editable: false, width: 100, align: "center", sortable: false,
                     render: function (ui) {
                         var rowData = ui.rowData;
                         var px = "";
                         if (rowData[5] == "4") {
                             px += "<a class='icon-xianqing' title='处理' onclick='cancelOrder(" + rowData[9] + ")'>处理</a>"
                         } else {
                             px += "-";
                         }
                         return px;
                     }
                 }

            ];

        //sort：要显示字段,数组对应
                 var sort = [
                 
                "Number",
               "CardNum",
               "Name",
               "sjhm",
               "Monery",
               "Type",
               "bz",
               "wxddh",
               "AddTime",
               "Id"
               
               

            ];
        PQLoadGrid("#grid_paging", url, colM, sort, 15, false);

        //        $("#grid_paging").pqGrid({
        //            freezeCols: 2, //固定前面列
        //            title: false
        //        });


        pqGridResize("#grid_paging", 45, 100);
        $(window).bind({
            resize: function () {
                if ($(window).width() < 1241) {
                    pqGridResize("#grid_paging", -65, 0);
                }
                else {
                    pqGridResize("#grid_paging", -45, 0);
                }
            },
            click: function () {

                if ($(window).width() < 1241) {
                    pqGridResize("#grid_paging", -65, 0);
                }
                else {
                    pqGridResize("#grid_paging", -45, 0);
                }
            }
        })
    }

    //取消订单
        function cancelOrder(key) {
            var url = "/SysSetBase/memInfo/CancelOrder.aspx?Id=" + key;
            top.art.dialog.open(url, {
                id: 'CancelOrder',
                title: '订单处理 > 充值退款',
                width: 450,
                height: 400,
                close: function () {
                    ListGrid();
                }
            }, false);
        }

    function plye() {
        var url = "/RMBase/SysMember/BatchManagementBalance.aspx";
        top.art.dialog.open(url, {
            id: 'plye',
            title: '批量管理余额',
            width: 530,
            height: 310,
            close: function () {
                ListGrid();
            }
        }, false);
    }

    function ye(key) {
        var url = "/RMBase/SysMember/ManagementBalance.aspx?ID=" + key;
        top.art.dialog.open(url, {
            id: 'ye',
            title: '管理余额',
            width: 530,
            height: 370,
            close: function () {
                ListGrid();
            }
        }, false);
    }

    function jf(key) {
        var url = "/RMBase/SysMember/ManagementIntegral.aspx?ID=" + key;
        top.art.dialog.open(url, {
            id: 'jf',
            title: '管理积分',
            width: 530,
            height: 370,
            close: function () {
                ListGrid();
            }
        }, false);
    }

    


</script>
<body>
    <form id="form1" runat="server">
    <div class="tools_bar btnbartitle btnbartitlenr" style="display: block;">
        <div>
            当前位置：<span><a title="系统首页" href="/Frame/HomeIndex.aspx">系统首页</a></span><span> &gt;
            </span><span>会员管理</span> &gt; <span>充值记录</span>
        </div>
    </div>
    <div class="gtall gmkf clearfix" style="display: ">
        <div class="bonusrecord">
            <div class="bonusrecord03 clearfix">
                <div class="sharedate">
                    
                </div>
                <div class="sharesearch">
                    <input type="text" name="name" value="" id="txtSearch" placeholder="请输入关键字..." />
                    <i class="icon-search" onclick="ListGrid()" id="btn_search"></i>
                </div>
                <div class="wdyhd" style="padding-right: 12px;">
                    <div class="r">
                        
                        <span onclick="" style="display: none">导出</span>
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

