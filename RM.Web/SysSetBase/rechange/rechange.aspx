<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rechange.aspx.cs" Inherits="RM.Web.SysSetBase.rechange.rechange" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>充值设置</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/SysSetBase/css/IE.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script src="../../Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="../../Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <link href="/Themes/Scripts/PqGrid/style.css" rel="stylesheet" type="text/css" />
    <link href="/Themes/Scripts/PqGrid/pqgrid.min.css" rel="stylesheet" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/PqGrid/jquery-ui.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/PqGrid/pqgrid.min.js" type="text/javascript"></script>
        <script type="text/javascript">
            //获取地址栏参数
            function getUrlParam(name) {
                var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
                var r = window.location.search.substr(1).match(reg);  //匹配目标参数
                if (r != null) return unescape(r[2]); return null; //返回参数值
            }



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
                Search += "Title@" + svl + "|";
            }

            var hotelid = $("#hdHotelId").val();
            if (hotelid != '' && hotelid != "-1") {
                Search += "hotelid@" + hotelid + "|";
            }


            if (Search != "") {
                strwhere += "&Search=" + Search.substr(0, Search.length - 1);
            }
            return strwhere;
        }


        /**加载表格函数**/
        function ListGrid() {

            //url：请求地址

            var url = "rechange.ashx?action=getinfo" + GetWhere();
            
            //colM：表头名称
            var colM = [
                { title: "checkbox", width: 60, align: "center", sortable: false,
                    render: function (ui) {
                        var rowData = ui.rowData;
                        return "<div onclick='CheckAllLine3(this)' class='xuanxuan' dateId='" + rowData[0] + "' name='checkbox'></div>";
                    }
                },
                { title: "酒店/门店", width: 120, align: "center" },
                { title: "充值金额", width: 80, align: "center" },
                { title: "赠送", width: 60, align: "center",
                    render: function (ui) {
                        var rowData = ui.rowData;

                        var px = "";
                        var zs = "";
                        if (rowData[4] * 1 == 1) zs += rowData[3] + "元 ";
                        if (rowData[6] * 1 == 1) zs += rowData[5] + "积分 ";
                        if (rowData[8] * 1 == 1) zs += rowData[7] + "1张 ";
                        if (rowData[10] * 1 == 1) zs += rowData[9] + " ";
                        px = zs == "" ? "-" : zs;
                        return px;
                    }
                },
                { title: "说明", width: 200, align: "center",
                    render: function (ui) {
                        var rowData = ui.rowData;
                        var px = "";
                        px = rowData[11];
                        return px;
                    }

                },
                { title: "操作", editable: false, width: 120, align: "center", sortable: false,
                    render: function (ui) {
                        var rowData = ui.rowData;
                        var px = "<div class='caozuo'>";

                        px += "<span class='icon-edit4' title='修改' onclick='edit2(" + rowData[0] + ")'>修改</span>"
                        px += "<span class='icon-lbx' title='删除' onclick='de2(" + rowData[0] + ")'>删除</span></div>";
                        return px;
                    }
                }
            ];

            // sort：要显示字段,数组对应
            var sort = [
               "Id",
               "Id",
               "HotelName",
               "moneys",
               "zsmoneys",
               "iszsmoneys",
               "zsjf",
               "iszsjf",
               "CouponName",
               "iscouponid",
               "hylxname",
               "ishylxcode",
               "bz"
            ];
            var HotelId = $("#hdHotelId").val();
            if (HotelId == "" || HotelId == "-1") {
                url = "";
            }
            PQLoadGrid("#grid_paging", url, colM, sort, 10, false);

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
            </script>

</head>
<body>
    <form id="form1" runat="server">
        <input runat="server" type="hidden" id="Hdhoteladmin" value="1" />
    <input runat="server" type="hidden" id="hdHotelId" value="-1" />
    <input runat="server" type="hidden" id="hdhoteltype" value="2" />
    <input runat="server" type="hidden" id="htHotelTree" value="true" />
    <div class="tools_bar btnbartitle btnbartitlenr" style="display:block;">
        <div>
            当前位置：<span><a title="系统首页" href="/Frame/HomeIndex.aspx">系统首页</a></span><span> &gt; </span><span>系统设置</span> &gt; </span><span>充值设置</span></span>
        </div>
    </div>

       <div class="gtall gmkf clearfix">
        <div id="HotelTree" runat="server" class="gmkfNav">
            <dl>
                <%=hotelTreeHtml.ToString()%>
            </dl>
        </div>
        <div class="bonusrecord">
            <div class="bonusrecord03">
                <div class="sharesearch">
                    <input type="text" name="name" id="txtSearch" placeholder="请输入关键字..." />
                    <i class="icon-search" onclick="ListGrid()"></i>
                </div>
                <div class="wdyhd" style="padding-right: 12px;">
                    <div class="r">
                       <span onclick="add()">添加</span> <span onclick="edit()">修改</span> <span onclick="de()">删除</span>
                    </div>
                </div>
            </div>
            <div id="grid_paging" style="margin-top: 1px;">
            </div>
        </div>
    </div>
    </form>

    <script type="text/javascript">

        //左边导航
        $('.gmkfNav').on('click', 'b', function () {
            $(this).siblings('ul').slideToggle(120);
            $(this).parents('dd').toggleClass('down');
        });
        //添加
        function add() {
            var url = "/SysSetBase/rechange/addrechange.aspx";
                top.art.dialog.open(url, {
                    id: 'addrechange',
                    title: '充值设置 > 添加',
                    width: 530,
                    height: 400,
                    close: function () {
                        ListGrid();
                    }
                }, false);

            }
            //修改
            function edit() {
                var key = GetPqGridRowValue("#grid_paging", 0);
                if (IsEditdata(key)) {
                    edit2(key);
                }
            }
            function edit2(key) {
                var url = "/SysSetBase/rechange/addrechange.aspx?ID=" + key;
                top.art.dialog.open(url, {
                    id: 'addrechange',
                    title: '充值设置 > 修改',
                    width: 530,
                    height: 400,
                    close: function () {
                        ListGrid();
                    }
                }, false);
            }
            // 删除
            function de() {
                var key = CheckboxValue();
                if (IsDelData(key)) {
                    showTipsMsg("操作失败！", 2000, 5);
                }
            }
            function de2(key) {
                var delparm = 'action=DeleteP&module=会员充值管理&tableName=CardRecharge&pkName=ID&pkVal=' + key;
                delConfig('/Ajax/Common_Ajax.ashx', delparm);
            }
    </script>

</body>
</html>
