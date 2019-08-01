<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberList.aspx.cs" Inherits="RM.Web.RMBase.SysSetBase.sales.MemberList" %>

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
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
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
            ListGrid(true);
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

            $(".pq-cont table").live('dblclick', function () {
                var key = GetPqGridRowValue("#grid_paging", 0);
                lookInfo(key);
            });


        });
        function GetWhere() {
            var strwhere = "";
            var Search = "";
            var svl = $("#ddlSearch").val();
            if (svl != '0') {
                Search += "hylx@" + svl + "|";
            }
            svl = $("#txtSearch").val();

            if ($.trim(svl) != '') {
                Search += "kh@" + svl + "|";
            }

            var hDeleteMark = $("#hDeleteMark").val();
            if (hDeleteMark != "") {
                Search += "DeleteMark@" + hDeleteMark + "|";
            }

            svl = $("#Hdhoteladmin").val();
            if ($.trim(svl) != '') {
                Search += "AdminHotelid@" + svl + "|";
            }


            svl = $("#hdHotelId").val();

            if ($.trim(svl) != '' && $.trim(svl) != '0') {
                Search += "hotelid@" + svl + "|";
            }

            svl = $("#txtStartTime").val();
            if ($.trim(svl) != '' && $.trim(svl) != '0') {
                Search += "CreateDate@" + svl + "|";
            }

            svl = $("#txtEndTime").val();
            if ($.trim(svl) != '' && $.trim(svl) != '0') {
                Search += "CreateDate2@" + svl + "|";
            }


            if (Search != "") {
                strwhere += "&Search=" + Search.substr(0, Search.length - 1);
            }

            return strwhere;
        }
        /**加载表格函数**/
        function ListGrid(load_bl) {
            //url：请求地址
            var url = "salesOrderManager.ashx?Menu=GetInfoList" + GetWhere();
            //colM：表头名称
            var colM = [
                { title: "checkbox", width: 60, align: "center", sortable: false,
                    render: function (ui) {
                        var rowData = ui.rowData;
                        return "<div onclick='CheckAllLine3(this)' class='xuanxuan' dateId='" + rowData[0] + "' name='checkbox'></div>";
                    }
                },
                { title: "真实姓名", width: 80, align: "center" },
                { title: "手机号码", width: 100, align: "center" },
                { title: "部门职位", width: 150, align: "center" },
                { title: "微信昵称", width: 100, align: "center" },
                { title: "角色", width: 100, align: "center" },
                { title: "创建时间", width: 120, align: "center" },
                { title: "销售金额", width: 80, align: "center" },
                { title: "累计奖金", width: 80, align: "center" },
                { title: "提现奖金", width: 80, align: "center" },
                { title: "剩余奖金", width: 80, align: "center" },
                { title: "带来客户", width: 80, align: "center" },
                { title: "推广状态", editable: false, width: 100, align: "center",
                    render: function (ui) {
                        var rowData = ui.rowData;
                        var px = "<div class='caozuo'>";
                        var zt = rowData[12];
                        if (zt == "1") {
                            px += "<i title='点击关闭' class='icon-kg1 kai' style='font-size:32px;' onclick=\"SetPopularizeState(this,'" + rowData[0] + "','" + rowData[1] + "')\"></i>";
                        } else {
                            px += "<i title='点击开启' class='icon-kg1' style='font-size:32px;' onclick=\"SetPopularizeState(this,'" + rowData[0] + "','" + rowData[1] + "')\"></i>";
                        }
                        px += "</div>";
                        return px;
                    }
                },
                { title: "推广二维码", editable: false, width: 100, align: "center", sortable: false,
                    render: function (ui) {
                        var rowData = ui.rowData;
                        var px = "<div class='caozuo'>";
                        px += "<a class='ck' title='查看推广码' onclick=\"lookErWeiMa('" + rowData[0] + "','" + rowData[2] + "')\" style='color:#3299cd;'>查看推广码</a></div>";
                        //                        px += "<a class='c1' title='编辑' onclick='editById(" + rowData[0] + ")'></a>"
                        return px;
                    }
                },
                { title: "操作", editable: false, width: 80, align: "center", sortable: false,
                    render: function (ui) {
                        var rowData = ui.rowData;
                        var px = "<div class='caozuo'>";
                        px += "<a class='icon-xianqing' title='查看' onclick=\"lookInfo('" + rowData[0] + "')\">查看</a></div>";
                        //                        px += "<a class='c1' title='编辑' onclick='editById(" + rowData[0] + ")'></a>"
                        return px;
                    }
                }
            ];

            //sort：要显示字段,数组对应
            var sort = [
            'User_ID',
            'User_Name',
            'User_Phone',
            'OrgName',
            'WX_Nickname',
            'Roles_Name',
            'CreateDate',
            'Sales_Total',
            'Bonus_Total',
            'WithdrawCash',
            'ExtractableMoney',
            'Register_Total',
            'PopularizeState'
            ];
            if (load_bl && $("#htHotelTree").val() == "True") {//首次加载标题栏
                url = "";
            }
            PQLoadGrid("#grid_paging", url, colM, sort, 20, false);
            pqGridResize("#grid_paging", 0, 0);

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
        //修改
        function edit() {
            var key = GetPqGridRowValue("#grid_paging", 0);
            if (IsEditdata(key)) {
                editById(key);
            }
        }

        //查看
        function lookInfo(key) {
            var HotelId = $("#hdHotelId").val();
            window.location.href = "/SysSetBase/sales/bonusrecord.aspx?User_ID=" + key + "&HotelId=" + HotelId;
        }

        //智订云商户平台二维码 
        function MerchantsCode() {
            var url = "/SysSetBase/sales/code_imgnew02.aspx";
            top.art.dialog.open(url, {
                id: 'lookErWeiMa',
                title: '智订云商户平台二维码',
                width: 510,
                height: 560,
                close: function () {

                }
            }, false);
        }

        //设置展牌 
        function Billboard() {
            var AdminHotelid = $("#Hdhoteladmin").val();
            var url = "/SysSetBase/sales/setcard.aspx?AdminHotelid=" + AdminHotelid;
            top.art.dialog.open(url, {
                id: 'setcard',
                title: '展牌设置',
                width: 780,
                height: 450,
                close: function () {

                }
            }, false);
        }

        //查看二维码 
        function lookErWeiMa(key, wxName) {
            var url = "/SysSetBase/sales/code_imgnew01.aspx?id=" + key + "&wxName=" + wxName;
            top.art.dialog.open(url, {
                id: 'code_imgnew01',
                title: '查看推广码',
                width: 740,
                height: 560,
                close: function () {
                    ListGrid();
                }
            }, false);
        }

        function editById(key) {
            var url = "/RMBase/SysMember/MemberEditor.aspx?key=" + key;
            top.art.dialog.open(url, {
                id: 'MemberEditor',
                title: '会员管理系统 > 修改会员',
                width: 330,
                height: 250,
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
        function lzxx(obj) {
            if ($(obj).html() == "离职用户") {
                $("#hDeleteMark").val("0");
                $(obj).html("在职用户");
            } else {
                $("#hDeleteMark").val("1");
                $(obj).html("离职用户");
            }
            ListGrid(false);
        }

        function SetPopularizeState(lb, userid, username) {
            var tc = $(lb).attr("class");
            var state = "";
            var msg = "";
            if (tc == "icon-kg1 kai") {
                state = "0";
                msg = "关闭";
            } else {
                state = "1";
                msg = "开启";
            }
            showConfirmMsg("你确定要" + msg + "[" + username + "]推广状态吗?", function (r) {
                if (r) {
                    $.ajax({
                        url: "salesOrderManager.ashx",
                        data: {
                            Menu: "SetPopularizeState",
                            userid: userid,
                            state: state
                        },
                        type: "POST",
                        dataType: "text",
                        success: function (data) {
                            if (data == "1") {
                                if (state == "1") {
                                    $(lb).attr("class", "icon-kg1 kai");
                                } else {
                                    $(lb).attr("class", "icon-kg1");
                                }
                            } else {
                                showTipsMsg('操作失败!', '5000', '5');
                            }
                        }
                    });
                }
            });
        }

        function AllPopularizeState(lb) {
            var tc = $(lb).attr("class");
            var state = "";
            var msg = "";
            if (tc == "icon-kg1 kai") {
                state = "0";
                msg = "关闭";
            } else {
                state = "1";
                msg = "开启";
            }

            if ($("#hdHotelId").val() == "" || $("#hdHotelId").val() == "-1") {
                showTipsMsg('请选择酒店！', '5000', '5');
                return false;
            }

            showConfirmMsg("你确定要" + msg + "所有员工推广状态吗?", function (r) {
                if (r) {
                    $.ajax({
                        url: "salesOrderManager.ashx",
                        data: {
                            Menu: "AllPopularizeState",
                            AdminHotelId: $("#Hdhoteladmin").val(),
                            HotelId: $("#hdHotelId").val(),
                            state: state
                        },
                        type: "POST",
                        dataType: "text",
                        success: function (data) {
                            if (data == "1") {
                                if (state == "1") {
                                    $(lb).attr("class", "icon-kg1 kai");
                                } else {
                                    $(lb).attr("class", "icon-kg1");
                                }
                                $(".pq-refresh").click();
                            } else {
                                showTipsMsg('操作失败!', '5000', '5');
                            }
                        }
                    });
                }
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <input runat="server" type="hidden" id="Hdhoteladmin" />
    <input runat="server" type="hidden" id="hdHotelId" value="0" />
    <input runat="server" type="hidden" id="htHotelTree" value="true" />
    <input runat="server" type="hidden" id="hdMenus" />
    <input runat="server" type="hidden" id="hDeleteMark" value="1" />
    <div class="tools_bar btnbartitle btnbartitlenr" style="display: block;">
        <div>
            当前位置：<span><a title="系统首页" href="/Frame/HomeIndex.aspx">系统首页</a></span><span> &gt;
            </span><span>营销管理</span> &gt;<span>销售管理</span>
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
                <div class="w120" style="display: none;">
                    <select id="ddlSearch" runat="server" onchange="ListGrid(false)">
                        <option value="value">证件类型</option>
                        <option value="value">选项01</option>
                        <option value="value">选项01</option>
                    </select>
                </div>
                <div class="sharedate">
                    <input type="text" id="txtStartTime" runat="server" onfocus="WdatePicker()" />
                    <input type="text" id="txtEndTime" runat="server" onfocus="WdatePicker()" />
                </div>
                <div class="sharesearch">
                    <input type="text" id="txtSearch" value="" placeholder="请输入关键字..." />
                    <i class="icon-search" onclick="ListGrid(false);"></i>
                </div>
                <div class="wdyhd" style="padding-right: 12px;">
                    <div class="r">
                        <span style="display:flex;">全部推广状态 <i id="iRoomState" class="icon-kg1 kai" style="font-size:32px;margin-left:5px;" onclick="AllPopularizeState(this)"></i></span>
                        <span onclick="MerchantsCode()">智订云商户平台二维码</span>
                        <asp:Button ID="btnSumit" runat="server" Text="批量下载推广码" OnClick="btnSumit_Click"
                            class="span" />
                        <asp:Button ID="btnSumits" runat="server" Text="批量下载展牌" OnClick="btnSumits_Click"
                            class="span" />
                        <span onclick="Billboard()">展牌设置</span> <span onclick="lzxx(this)">离职用户</span>
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
