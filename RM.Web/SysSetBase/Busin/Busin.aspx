<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Busin.aspx.cs" Inherits="RM.Web.SysSetBase.Busin.Busin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>营业点管理</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <script src="../../WDatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/PqGrid/jquery-ui.min.js"></script>
    <script src="/Themes/Scripts/PqGrid/pqgrid.min.js"></script>
    <link href="/Themes/Scripts/PqGrid/pqgrid.min.css" rel="stylesheet" />
    <link href="../../SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="../../SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        $(function () {
            ListGrid(true);
            typelist();
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
                typelist();
            });
        });


        function typelist() {
            $.ajax({
                url: "Book.ashx",
                data: {
                    Menu: "GetBusinType",
                    hotelid: $("#hdHotelId").val(),
                    Adminhotelid: $("#Hdhoteladmin").val()
                },
                type: "POST",
                dataType: "text",
                success: function (data) {
                    var list = "<option value='-1'>所有营业点</option>";
                    if (data == "") {
                        $("#ddlType").html(list);
                        return;
                    }
                    var json = eval("(" + data + ")");

                    for (var i = 0; i < json.length; i++) {
                        list += "<option value='" + json[i].ID + "'>" + json[i].TYPENAME + "</option>";
                    }
                    $("#ddlType").html(list);
                }
            });
        }


    </script>
    <script type="text/javascript">


        /**加载表格函数**/
        function ListGrid() {
            //url：请求地址
            var strwhere = "";
            var type = $("#ddlType").val();
            var content = $("#txtSearch").val();
            var hotelid = $("#hdHotelId").val();

            if ($.trim(type) != '' && $.trim(type) != '-1') {
                strwhere += "&type=" + type;
            }

            if ($.trim(content) != '') {
                strwhere += "&content=" + content;
            }
            if ($.trim(hotelid) != '') {
                strwhere += "&hotelid=" + hotelid;
            }
            var url = "Book.ashx?Menu=GetInfoList" + strwhere;
            //colM：表头名称
            var colM = [
                { title: "checkbox", width: 60, align: "center", sortable: false,
                    render: function (ui) {
                        var rowData = ui.rowData;
                        return "<div onclick='CheckAllLine3(this)' class='xuanxuan' dateId='" + rowData[0] + "' name='checkbox'></div>";
                    }
                },
                { title: "营业点名称", width: 160, align: "left" },
                { title: "所属分类", width: 80, align: "center" },
          


                {
                    title: "服务电话", width: 180, align: "center",
                    render: function (ui) {
                        var rowData = ui.rowData[3]
                        if (ui.rowData[7] != null && ui.rowData[7] != undefined) {
                            rowData += ui.rowData[7]
                        }
                        return rowData;
                    }
                },


                { title: "服务时间", width: 180, align: "center" },




                { title: "所在位置", width: 300, align: "center" },
                { title: "排序", editable: false, width: 60, align: "center", sortable: false,
                    render: function (ui) {
                        var rowData = ui.rowData;
                        var px = "<div class='paixu'>";
                        var sv = rowData[0] + "|" + rowData[6] + "|1";
                        px += "<img onclick=\"return SetSort('up','" + sv + "')\" title='向上' src='../../Themes/Images/up.png'>"
                        px += "<img onclick=\"return SetSort('down','" + sv + "')\" title='向下' src='../../Themes/Images/down.png'></div>";
                        return px;
                    }
                },
                { title: "操作", editable: false, width: 120, align: "center", sortable: false,
                    render: function (ui) {
                        var rowData = ui.rowData;
                        var px = "";
                        px += "<span   class='icon-edit4'  onclick='editById(" + rowData[0] + ")'>修改</span>"
                        px += "<span   class='icon-lbx' onclick='DeleteById(" + rowData[0] + ")'>删除</span>";
                        return px;
                    }
                }
            ];

            //sort：要显示字段,数组对应
            var sort = [
               "id",
               "id",
               "BusinessName",
               "TypeName",
               "Telephone",
               "BusinessTime",
               "BusinessAddress",
               "Sort",
               "Extension"
            ];


            var HotelId = $("#hdHotelId").val();
            if (HotelId == "" || HotelId == "-1") {
                url = "";
            }
            PQLoadGrid("#grid_paging", url, colM, sort, 10, false);
//            $("#grid_paging").pqGrid({
//                freezeCols: 6, //固定前面列
//                title: false
//            });
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
    <script type="text/javascript">
        //添加
        function add() {
            var HotelId = $("#hdHotelId").val();
            if (HotelId == "" || HotelId == "-1") {
                showTipsMsg('请先选择所属酒店！', '5000', '5');
                return false;
            }
            var url = "/SysSetBase/Busin/addBusin.aspx?HotelId=" + HotelId;
            top.art.dialog.open(url, {
                id: 'AddInfo',
                title: '添加营业点',
                width: 800,
                height: 550,
                close: function () {
                    ListGrid();
                }
            }, false);
        }

        ///管理
        function manage() {
            var HotelId = $("#hdHotelId").val();
            if (HotelId == "" || HotelId == "-1") {
                showTipsMsg('请先选择所属酒店！', '5000', '5');
                return false;
            }
            var url = "/SysSetBase/Busin/Colulist.aspx?HotelId=" + HotelId;
            window.location.href = url;
        }

        //处理
        function edit() {

            var key = GetPqGridRowValue("#grid_paging", 0);
            if (IsEditdata(key)) {
                editById(key);
            }
        }
        function editById(key) {
            var HotelId = $("#hdHotelId").val();
            if (HotelId == "" || HotelId == "-1") {
                showTipsMsg('请先选择所属酒店！', '5000', '5');
                return false;
            }
            var url = "/SysSetBase/Busin/addBusin.aspx?ID=" + key + "&HotelId=" + HotelId;
            top.art.dialog.open(url, {
                id: 'AddInfo',
                title: '处理营业点',
                width: 800,
                height: 550,
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

            var delparm = 'action=ISDelete&module=营业点订单管理系统&tableName=Bookings&pkName=ID&pkVal=' + key;
            delConfig('/Ajax/Common_Ajax.ashx', delparm);
            ListGrid();
        }

        function SetSort(type, id) {
            $.post("Hotel.ashx?Menu=SetSort&type=" + type + "&sv=" + id, function (date) {
                if (date == "1") {
                    showTipsMsg("操作成功！", 2000, 4);
                    ListGrid();
                } else {
                    showTipsMsg("操作失败！", 2000, 5);
                }
            });
            return false;
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <input runat="server" type="hidden" id="Hdhoteladmin" value="1" />
    <input runat="server" type="hidden" id="hdHotelId" value="-1" />
    <input runat="server" type="hidden" id="htHotelTree" value="true" />
    <div class="tools_bar btnbartitle btnbartitlenr" style="display:block;">
        <div>
            当前位置：<span><a title="系统首页" href="/Frame/HomeIndex.aspx">系统首页</a></span><span> &gt; </span><span>酒店管理</span> &gt; </span><span>营业点管理</span></span>
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
                <div class="w120">
                    <select id="ddlType">
                        <option value="-1">所有营业点</option>
                    </select>
                </div>
                <div class="sharesearch">
                    <input type="text" name="name" id="txtSearch" runat="server" placeholder="请输入关键字..." />
                    <i class="icon-search" onclick="ListGrid()"></i>
                </div>
                <div class="wdyhd" style="padding-right: 12px;">
                    <div class="r">
                        <span onclick="manage();">分类管理</span> <span onclick="add();">添加</span> <span onclick="edit();">
                            修改</span> <span onclick="Delete();">删除</span>
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
        $(".gmkfNav").panel({ iWheelStep: 80 });
    </script>
</body>
</html>
