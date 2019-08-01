<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="memAdmin.aspx.cs" Inherits="RM.Web.SysSetBase.memInfo.memAdmin" %>

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
        var url = "mem.ashx?Menu=GetInfoList" + GetWhere();
        //colM：表头名称
        var colM = [
                { title: "会员卡号", width: 100, align: "center" },
                { title: "姓名", width: 80, align: "center" },
                { title: "手机号码", width: 100, align: "center" },
                { title: "会员级别", width: 80, align: "center" },
                { title: "创建时间", width: 80, align: "center" },
                { title: "累计充值", width: 80, align: "center" },
                { title: "累计赠送充值", width: 100, align: "center" },
                { title: "累计消费", width: 80, align: "center" },
                { title: "会员卡余额", width: 90, align: "center" },
                { title: "操作", editable: false, width: 350, align: "center", sortable: false,
                    render: function (ui) {
                        var rowData = ui.rowData;
                        var px = "";
                        px += "<a class='icon-guanliyue' title='管理余额' onclick='ye(" + rowData[9] + ")'>管理余额</a>"
                        px += "<a class='icon-guanlijifen' title='管理积分' onclick='jf(" + rowData[9] + ")'>管理积分</a>"
                        px += "<a class='icon-guanlijifen' title='打印消费码' onclick='dy(" + rowData[9] + ")'>打印消费码</a>"
                        px += "<a class='icon-xianqing' title='查看' onclick='lookInfo(" + rowData[9] + ")'>查看</a>"

                        return px;
                    }
                }
            ];

        //sort：要显示字段,数组对应
        var sort = [
               "kh",
               "xm",
               "sjhm",
               "hylxname",
               "addtime",
               "ljcz",
               "czzs",
               "ljxf",
               "hykye",
               "lsh"

            ];
        PQLoadGrid("#grid_paging", url, colM, sort, 15, false);

        //        $("#grid_paging").pqGrid({
        //            freezeCols: 2, //固定前面列
        //            title: false
        //        });


        pqGridResize("#grid_paging", 45, 100);
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

    //查看
    function lookInfo(key) {
        window.location.href = "memAdminInfo.aspx?lsh=" + key
    }

    function plye() {
        var url = "/RMBase/SysMember/BatchManagementBalance.aspx";
        top.art.dialog.open(url, {
            id: 'plye',
            title: '批量管理余额',
            width: 417,
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

    function addmember() {
        var url = "/SysSetBase/memInfo/MemberAdd.aspx";
        top.art.dialog.open(url, {
            id: 'MemberAdd',
            title: '添加会员',
            width: 417,
            height: 370,
            close: function () {
                ListGrid();
            }
        }, false);
    }

    

    function rules() {
        var url = "/SysSetBase/memInfo/ReceiptRules.aspx";
        top.art.dialog.open(url, {
            id: 'gz',
            title: '使用规则',
            width: 400,
            height: 300,
            close: function () {
                //ListGrid();
            }
        }, false);

    }
    
    function dy(key) {

        var url = "/SysSetBase/memInfo/TicketPreview.aspx?MemberId=" + key;
        top.art.dialog.open(url, {
            id: 'dy',
            title: '消费码预览',
            width: 414,
            height: 650,
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
            </span><span>会员管理</span> &gt; <span>会员卡管理</span>
        </div>
    </div>
    <div class="gtall gmkf clearfix" style="display: ">
        <div class="bonusrecord">
            <div class="bonusrecord03 clearfix">
                <div class="sharedate">
                    <input type="text" id="txtStartTime" runat="server" onfocus="WdatePicker()" />
                    <input type="text" id="txtEndTime" runat="server" onfocus="WdatePicker()" />
                </div>
                <div class="sharesearch">
                    <input type="text" name="name" value="" id="txtSearch" placeholder="请输入关键字..." />
                    <i class="icon-search" onclick="ListGrid()" id="btn_search"></i>
                </div>
                <div class="wdyhd" style="padding-right: 12px;">
                    <div class="r">
                      <span onclick="addmember()" style="display: ">添加会员</span>
                        <span onclick="rules()" style="display: ">使用规则</span>
                        <span onclick="plye()" style="display: ">批量管理余额</span>
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
