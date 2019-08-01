<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="couponstypelist.aspx.cs" Inherits="RM.Web.SysSetBase.coupons.couponstypelist" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>信息</title>
    <script src="../../WDatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/PqGrid/jquery-ui.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/PqGrid/pqgrid.min.js" type="text/javascript"></script>
    <link href="/Themes/Scripts/PqGrid/pqgrid.min.css" rel="stylesheet" />
    <link href="../../SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="../../SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            ListGrid();
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
                
                ListGrid(false);
            });
            var HotelId = $("#hdHotelId").val();
            if (HotelId !== "-1") {

                for (var i = 0; i < $(".gmkfNav ul li").length; i++) {
                    if ($(".gmkfNav ul li").eq(i).attr("hotelid") == HotelId) {
                        $(".gmkfNav ul li").eq(i).addClass("active");
                    } else {
                        $(".gmkfNav ul li").eq(i).removeClass("active");
                    }
                }
            }
        });
        function GetWhere() {
            var strwhere = "";
            var Search = "";
            var svl = $("#txtSearch").val();
            if ($.trim(svl) != '') {
                Search += "Name@" + svl + "|";
            }
            
            if (Search != "") {
                strwhere += "&Search=" + Search.substr(0, Search.length - 1);
            }
            return strwhere;
        }
        /**加载表格函数**/
        function ListGrid() {
            //url：请求地址
            var url = "GetCouponList.ashx?Menu=GetCouponType" + GetWhere();
            //colM：表头名称
            var colM = [
                { title: "checkbox", width: 60, align: "center", sortable: false,
                    render: function (ui) {
                        var rowData = ui.rowData;
                        return "<div onclick='CheckAllLine3(this)' class='xuanxuan' dateId='" + rowData[0] + "' name='checkbox'></div>";
                    }
                },
                { title: "类型名称", width: 500, align: "center" },
                { title: "排序", editable: false, width: 60, align: "center", sortable: false,
                    render: function (ui) {
                        var rowData = ui.rowData;
                        var px = "";
                        px += "<div class='paixu'>";
                        px += "<img title='向上'  onclick=\"SetRoomCommoditySort('up','" + rowData[0] + "','" + rowData[2] + "')\"  src='/Themes/Images/up.png'/>";
                        px += "<img title='向下'  onclick=\"SetRoomCommoditySort('down','" + rowData[0] + "','" + rowData[2] + "')\" src='/Themes/Images/down.png'/>";
                        px += "</div>";
                        return px;
                    }
                },
                { title: "操作", editable: false, width: 120, align: "center", sortable: false,
                    render: function (ui) {
                        var rowData = ui.rowData;
                        var px = "<div class='caozuo'>";
                        px += "<a class='icon-edit4' title='' onclick='editById(" + rowData[0] + ")'>修改</a>"
                        px += "<a class='icon-lbx' title='' onclick='DeleteById(" + rowData[0] + ")'>删除</a></div>";
                        return px;
                    }
                }
            ];

            //sort：要显示字段,数组对应
            var sort = [
            "Id",
            "Id",
            "Name",
            "AdminHotelid",
            
            "Sort"
            ];
            PQLoadGrid("#grid_paging", url, colM, sort, 30, false);
            pqGridResize("#grid_paging", -45, 100);
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


            var url = "/SysSetBase/coupons/couponstypeadd.aspx?v=1";
            top.art.dialog.open(url, {
                id: 'couponstypeaddClass',
                title: '添加卡券类型',
                width: 400,
                height: 126,
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


            var url = "/SysSetBase/coupons/couponstypeadd.aspx?ID=" + key;
            top.art.dialog.open(url, {
                id: 'couponstypeaddClass',
                title: '编辑卡券类型',
                width: 400,
                height: 126,
                close: function () {
                    ListGrid();
                }
            }, false);
        }
        // 删除
        function Delete() {
            var key = CheckboxValue();
            if (IsDelData(key)) {
                DeleteById(key)
            }
        }
        function DeleteById(key) {
            $.post("GetCouponList.ashx?Menu=DeleteSetProKind&ProKindId=" + key, function (date) {
                if (date == "1") {
                    var delparm = 'action=DeleteP&module=卡券类型管理系统&tableName=CouponType&pkName=ID&pkVal=' + key;
                    delConfig('/Ajax/Common_Ajax.ashx', delparm);
                } else {
                    showTipsMsg("请先删除该类型的卡券", 2000, 5);
                }
            });
        }

        function SetRoomCommoditySort(type, ProKindId, AdminHotelid) {
            $.post("GetCouponList.ashx?Menu=SetCouponTypeSort&type=" + type + "&ProKindId=" + ProKindId + "&AdminHotelid=" + AdminHotelid, function (date) {
                if (date == "1") {
                    showTipsMsg("操作成功！", 2000, 4);
                    ListGrid();
                } else {
                    showTipsMsg("操作失败！", 2000, 5);
                }
            });
            return false;
        }

        function goBack() {
            javascript: history.back();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <input type="hidden" runat="server" id="pid" value="0" />
    <input runat="server" type="hidden" id="Hdhoteladmin" value="1" />
    <input runat="server" type="hidden" id="hdHotelId" value="-1" />
    <input runat="server" type="hidden" id="htHotelTree" value="true" />
    <div class="tools_bar btnbartitle btnbartitlenr" style="display:block;">
        <div>
            当前位置：<span><a title="系统首页" href="/Frame/HomeIndex.aspx">系统首页</a></span> &gt; <span>卡券管理</span> &gt; <span>卡券类型管理</span>
        </div>
    </div>
    <div class="gtall gmkf clearfix">
        
        <div class="bonusrecord">
            <div class="bonusrecord03">
           
                <div class="sharesearch">
                    <input type="text" name="name" id="txtSearch" runat="server" placeholder="请输入关键字..."/>
                    <i class="icon-search" onclick="ListGrid()"></i>
                </div>
                <div class="wdyhd" style="padding-right: 12px;">
                    <div class="r">
                        <span onclick="add();">添加</span> <span onclick="edit();">
                            修改</span> <span onclick="Delete();">删除</span> <span onclick="goBack();">返回</span>
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
