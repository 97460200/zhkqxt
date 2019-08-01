<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="roomstate.aspx.cs" Inherits="RM.Web.SysSetBase.houseState.roomstate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
<%--    <link href="/Themes/Scripts/PqGrid/style.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/PqGrid/jquery-ui.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/PqGrid/pqgrid.min.js" type="text/javascript"></script>
    <link href="/Themes/Scripts/PqGrid/pqgrid.min.css" rel="stylesheet" />--%>
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <input runat="server" type="hidden" id="hdAdminHotelId" value="1" />
    <input runat="server" type="hidden" id="hdHotelId" value="-1" />
    <input runat="server" type="hidden" id="hdRoomId" value="" />
    <input runat="server" type="hidden" id="hdRuleId" value="" />
    <input runat="server" type="hidden" id="hdJFState" value="1" />
    <!--除了图标 都不要使用<i>-->
    <div class="tools_bar btnbartitle btnbartitlenr" style="display:block;">
        <div>
            当前位置：<span><a title="系统首页" href="/Frame/HomeIndex.aspx">系统首页</a></span><span> &gt; </span><span>酒店管理</span> &gt; </span><span>房态房价</span></span>
        </div>
    </div>
    <div class="gtall gmkf clearfix">
        <div class="gmkfNav" id="HotelTree" runat="server">
            <dl>
                <%=hotelTreeHtml.ToString()%>
            </dl>
        </div>
        <div class="gmkfStion">
            <div class="wdyhd" style="padding-left: 20px; max-width: 1010px;">
                <div class="l">
                    <a class="active" hid="1">默认设置</a><a hid="2">按月设置</a><a hid="3">按周设置</a>
                </div>
                <div class="evRgt R3">
                    <select id="selYear">
                        <%=yearHtml%>
                    </select>
                    <i id="iLastMonth" title="上一月" class="icon-left"></i>
                    <select id="selMonth" runat="server">
                        <option value="1">1月</option>
                        <option value="2">2月</option>
                        <option value="3">3月</option>
                        <option value="4">4月</option>
                        <option value="5">5月</option>
                        <option value="6">6月</option>
                        <option value="7">7月</option>
                        <option value="8">8月</option>
                        <option value="9">9月</option>
                        <option value="10">10月</option>
                        <option value="11">11月</option>
                        <option value="12">12月</option>
                    </select>
                    <i id="iNextMonth" title="下一月" class="icon-right"></i>
                </div>
                <div class="evRgt R2" style="margin-left: 6px; display: none;">
                    <i id="iLastWeek" title="前一周" class="icon-left">前一周</i>
                    <input runat="server" id="txtWeekDate" type="text" class="date" />
                    <i id="iNextWeek" title="后一周" class="icon-right">后一周</i>
                </div>
                <div class="r setupbtn">
                    <span><a href="setPrice.aspx">批量设置价格</a> </span><span><a href="setState.aspx">批量设置房态</a></span>
                </div>
            </div>
            <ul class="wdySetup">
                <%--默认设置--%>
                <li style="overflow: auto;">
                    <div class="gmkfList" id="tbhtml" style="width: 320px;">
                    </div>
                    <a class="button buttonActive" onclick="SaveDefaultPrice()">保存</a> </li>
                <%--每月设置--%>
                <li>
                    <div class="chkopt" style="width: 1010px;">
                        <div id="dRule" class="radio">
                            <em>售房规则</em>
                            <%--<b dataid="26">无早</b><b dataid="27" class="active">有早</b>--%>
                        </div>
                        <div id="dVip" class="radio">
                            <em>会员等级</em><%=hydjHtml%>
                        </div>
                        <div id="dShowType" class="checkbox">
                            <em>显示分类</em><b showtype="fj" class="active">房价</b><b showtype="jf" class="active">需分</b><b
                                showtype="kd" class="active">可定</b>
                        </div>
                    </div>
                    <div class="daytable">
                        <table style="width: 1010px;">
                            <thead>
                                <tr>
                                    <th>
                                        一
                                    </th>
                                    <th>
                                        二
                                    </th>
                                    <th>
                                        三
                                    </th>
                                    <th>
                                        四
                                    </th>
                                    <th>
                                        五
                                    </th>
                                    <th>
                                        六
                                    </th>
                                    <th>
                                        日
                                    </th>
                                </tr>
                            </thead>
                            <tbody id="tbMonthList">
                            </tbody>
                        </table>
                    </div>
                </li>
                <%--按周设置--%>
                <li>
                    <div class="chkopt" style="width: 1010px;">
                        <div id="dWeekRule" class="radio">
                            <em>售房规则</em><%--<b>无早</b><b class="active">有早</b>--%>
                        </div>
                        <div id="dWeekVip" class="radio">
                            <em>会员等级</em><%=hydjHtml%>
                        </div>
                        <div id="dWeekShowType" class="checkbox">
                            <em>显示分类</em><b showtype="fj" class="active">房价</b><b showtype="jf" class="active">需分</b><b
                                showtype="kd" class="active">可定</b>
                        </div>
                    </div>
                    <div class="huste zhou">
                        <table style="width: 1010px;">
                            <thead>
                                <tr>
                                    <th style="width: 80px;">
                                        房型
                                    </th>
                                    <th style="width: 80px;">
                                        规则名称
                                    </th>
                                    <th>
                                        一
                                    </th>
                                    <th>
                                        二
                                    </th>
                                    <th>
                                        三
                                    </th>
                                    <th>
                                        四
                                    </th>
                                    <th>
                                        五
                                    </th>
                                    <th>
                                        六
                                    </th>
                                    <th>
                                        日
                                    </th>
                                </tr>
                            </thead>
                            <tbody id="tbWeekList">
                            </tbody>
                        </table>
                    </div>
                </li>
            </ul>
        </div>
        <input runat="server" id="hdVal" type="hidden" value="" />
        <%--1是客房价格2是日历价格--%>
        <input runat="server" id="id_hid" type="hidden" value="1" />
    </form>
    <script type="text/javascript">
        //左边导航
        $('.gmkfNav').on('click', 'b', function () {
            $(this).siblings('ul').slideToggle(120);
            $(this).parents('dd').toggleClass('down');
        });
        //酒店 点击
        $("#HotelTree .li[datatype='Hotel']").on("click", function (e) {
            $(this).find('ul').slideToggle(120);
            $(this).find('dd').toggleClass('down');
            var hotelid = $(this).attr("hotelid");
            $("#hdHotelId").val(hotelid);
            if ($("#id_hid").val() == "3") {
                $("#hdRoomId").val("");
                $("#HotelTree li[datatype='Room']").removeClass("active");
                GetWeekRule();
                GetWeekPriceTable();
            }
            e.stopPropagation();
        });
        //酒店 点击
        $("#HotelTree b.Hotel").on("click", function (e) {
            var hotelid = $(this).attr("hotelid");
            $("#hdHotelId").val(hotelid);
            if ($("#id_hid").val() == "3") {
                $("#hdRoomId").val("");
                $("#HotelTree li[datatype='Room']").removeClass("active");
                GetWeekRule();
                GetWeekPriceTable();
            }
        });
        //客房点击 点击
        $("#HotelTree li[datatype='Room']").on("click", function (e) {
            $("#HotelTree li[datatype='Room']").removeClass("active");
            $(this).addClass("active");
            var hotelid = $(this).attr("hotelid");
            var roomId = $(this).attr("RoomId");
            $("#hdHotelId").val(hotelid);
            $("#hdRoomId").val(roomId);

            if ($("#id_hid").val() == "1") {
                getRoomVal(roomId);
            } else if ($("#id_hid").val() == "2") {
                GetRule();
                GetMonthPriceTable();
            } else if ($("#id_hid").val() == "3") {
                GetWeekRule();
                GetWeekPriceTable();
            }
            e.stopPropagation();
        });

        //右边设置tab切换
        $('.wdyhd .l').on('click', 'a', function () {
            if ($(this).hasClass('active')) return;
            $(this).addClass('active').siblings().removeClass('active');
            $("#id_hid").val($(this).attr("hid"));
            var index = $(this).index();
            $('.wdySetup li').eq(index).show().siblings().hide();
            $('.gmkfStion').toggleClass('noStrong');  //修改部分布局 -- 如：每日设置的注释

            if ($("#id_hid").val() == "1") {
                var roomId = $("#hdRoomId").val();
                getRoomVal(roomId)
            } else if ($("#id_hid").val() == "2") {
                GetRule();
                GetMonthPriceTable();
            } else if ($("#id_hid").val() == "3") {
                GetWeekRule();
                GetWeekPriceTable();
            }

            index == 1 ? $('.R2').hide() : $('.R2').show();
            index == 2 ? $('.R3').hide() : $('.R3').show();
            index < 1 ? $('.setupbtn, .R2, .R3').hide() : $('.setupbtn').show();
        });


    </script>
    <script type="text/javascript">
        //默认设置
        $(function () {
            DefaultRoomPrice();
        });
        //加载默认设置表格
        function DefaultRoomPrice() {
            $.ajax({
                url: "../Ajax/SysAjax.ashx",
                data: {
                    Menu: "DefaultRoomPrice",
                    adminhotelid: $("#hdAdminHotelId").val()
                },
                type: "GET",
                datatype: "JSON",
                async: false,
                success: function (data) {
                    if (data == "") {
                        return;
                    }
                    $("#tbhtml").html(data);
                }
            });
        }
        //默认设置 加载房型值
        function getRoomVal(gsid) {
            $("#tbhtml input[type=text]").val("");
            $.ajax({
                url: "../Ajax/SysAjax.ashx",
                data: {
                    Menu: "GetKFJGList",
                    GSID: gsid
                },
                type: "GET",
                dataType: "JSON",
                async: false,
                success: function (response) {
                    if (response != null) {
                        var obj = eval(response);
                        var str = "";
                        $(obj).each(function (index) {
                            var val = obj[index];
                            var tvals = val.JG.split(',');
                            $("#" + tvals[0]).val(tvals[1]);
                            tvals = val.JF.split(',');
                            $("#" + tvals[0]).val(tvals[1]);
                            tvals = val.FJS.split(',');
                            $("#" + tvals[0]).val(tvals[1]);
                        });
                    }
                    else {

                    }
                }
            });
        }

        //保存默认设置
        function SaveDefaultPrice() {
            if ($("#hdRoomId").val() == "") {
                showTipsMsg('请选择房型！', '5000', '5');
                return false;
            }
            var arr = [];
            var hdvals = [];
            $("#tbhtml input[type=text]").each(function () {
                var hydj = $(this).attr("hydj");
                var MemberType = $(this).attr("MemberType");
                var tVal = $(this).val() == "" ? "0" : $(this).val();
                var nameval = $(this).attr("name");
                var ia = $.inArray(hydj + "*" + MemberType, arr);
                if ($.inArray(hydj + "*" + MemberType, arr) < 0) {
                    arr.push(hydj + "*" + MemberType);
                    if (MemberType == "2" || MemberType == "3" || MemberType == "4") {
                        hdvals.push("|" + hydj + "," + MemberType + "," + tVal + ",{jf}" + ",{fjs}");
                    } else {
                        hdvals.push("|" + hydj + "," + MemberType + "," + tVal + ",0" + ",0");
                    }
                } else {
                    if (nameval.indexOf("jf") >= 0) {
                        hdvals[ia] = hdvals[ia].replace("{jf}", tVal);
                    }
                    if (nameval.indexOf("fjs") >= 0) {
                        hdvals[ia] = hdvals[ia].replace("{fjs}", tVal);
                    }
                }
            });
            var reg = new RegExp("{jf}", "g"); //g,表示全部替换。

            $("#hdVal").val(hdvals.toString().replace(reg, "0"));
            $.ajax({
                url: "../Ajax/SysAjax.ashx",
                data: {
                    Menu: "Save",
                    hid: $("#hdHotelId").val(),
                    gsid: $("#hdRoomId").val(),
                    vals: hdvals.toString().replace(reg, "0"),
                    adminhotelid: $("#hdAdminHotelId").val()
                },
                type: "GET",
                dataType: "text",
                async: false,
                success: function (response) {
                    showTipsMsg(response, '5000', '4');
                }
            });
        }

    </script>
    <script type="text/javascript">
        ////////按月设置////////
        //获取售房规则
        function GetRule() {
            $("#dRule b").remove();
            $.ajax({
                url: "../Ajax/SysAjax.ashx",
                data: {
                    Menu: "GetRule",
                    RoomId: $("#hdRoomId").val(),
                    DisplayType: $("#id_hid").val()
                },
                type: "GET",
                datatype: "text",
                async: false,
                success: function (data) {
                    $("#dRule").append(data);
                }
            });
        }
        //售房规则事件
        $("#dRule").on("click", "b", function () {
            $(this).addClass('active').siblings('b').removeClass('active');
            GetMonthPriceTable();
        });
        //会员等级
        $("#dVip").on("click", "b", function () {
            $(this).addClass('active').siblings('b').removeClass('active');
            GetMonthPriceTable();
        });

        //年份选择
        $("#selYear").change(function () {
            GetMonthPriceTable();
        });

        //月份选择
        $("#selMonth").change(function () {
            GetMonthPriceTable();
        });

        //上一月
        $("#iLastMonth").on("click", function () {
            var sm = parseInt($("#selMonth").val());
            if (sm > 1) {
                sm--;
                $("#selMonth").val(sm);
                GetMonthPriceTable();
            }
        });
        //下一月
        $("#iNextMonth").on("click", function () {
            var sm = parseInt($("#selMonth").val());
            if (sm < 12) {
                sm++;
                $("#selMonth").val(sm);
                GetMonthPriceTable();
            }
        });

        //显示分类
        $('#dShowType').on('click', 'b', function () {
            var st = $(this).attr("ShowType");
            if ($(this).hasClass('active')) {
                $(this).removeClass('active');
                $("#tbMonthList .currInfo").find("." + st).hide();
            } else {
                $(this).addClass('active');
                $("#tbMonthList .currInfo").find("." + st).show();
            }
        });
        //加载 月 展示表格
        function GetMonthPriceTable() {
            $.ajax({
                url: "../Ajax/SysAjax.ashx",
                data: {
                    Menu: "GetMonthPriceTable",
                    HotelId: $("#hdHotelId").val(),
                    RoomId: $("#hdRoomId").val(),
                    RuleId: $("#dRule b.active").attr("dataid"),
                    VipCode: $("#dVip b.active").attr("dataid"),
                    Year: $("#selYear").val(),
                    Month: $("#selMonth").val()
                },
                type: "GET",
                dataType: "text",
                async: false,
                success: function (r) {
                    $("#tbMonthList").html(r);
                    $("#dShowType b").each(function () {
                        var st = $(this).attr("ShowType");
                        if (!$(this).hasClass('active')) {
                            $("#tbMonthList .currInfo").find("." + st).hide();
                        }
                    });
                }
            });
        }

        //开启状态
        $('#tbMonthList').on('click', 'i', function (e) {
            if ($("#hdRoomId").val() == "" || $("#hdRoomId").val() == "0") {
                showTipsMsg('请先选择房型！', '5000', '5');
                return;
            }
            if ($("#dRule b.active").attr("dataid") == undefined || $("#dRule b.active").attr("dataid") == "0") {
                showTipsMsg('请先添加售房规则！', '5000', '5');
                return;
            }

            var day = $(this).attr("day");
            var myDate = new Date(); //获取当前时间
            var myday = myDate.getFullYear() + "-" + parseInt(parseInt(myDate.getMonth()) + 1) + "-" + myDate.getDate()
            if (Date.parse(day) >= Date.parse(myday)) {
                var roomState = "0";
                if ($(this).hasClass('kgClose')) {
                    $(this).removeClass('kgClose');
                    roomState = "1";
                } else {
                    $(this).addClass('kgClose');
                    roomState = "0";
                }
                $.post("../Ajax/SysAjax.ashx", {
                    Menu: "RoomState",
                    HotelId: $("#hdHotelId").val(),
                    RoomId: $("#hdRoomId").val(),
                    RuleId: $("#dRule b.active").attr("dataid"),
                    VipCode: $("#dVip b.active").attr("dataid"),
                    DateRange: day,
                    RoomState: roomState
                }, function (data) {

                });
            } else {
                showTipsMsg('已过去的日期不能选择！', '5000', '5');
            }
            e.stopPropagation();
        });

        //按月表格点击事件
        $('#tbMonthList').on('click', 'td', function () {
            if ($("#hdRoomId").val() == "" || $("#hdRoomId").val() == "0") {
                showTipsMsg('请先选择房型！', '5000', '5');
                return;
            }
            if ($("#dRule b.active").attr("dataid") == undefined || $("#dRule b.active").attr("dataid") == "0") {
                showTipsMsg('请先添加售房规则！', '5000', '5');
                return;
            }
            var day = $(this).attr("day");
            var myDate = new Date(); //获取当前时间
            var myday = myDate.getFullYear() + "-" + parseInt(parseInt(myDate.getMonth()) + 1) + "-" + myDate.getDate()
            if (Date.parse(day) >= Date.parse(myday)) {
                var hotelid = $("#hdHotelId").val();
                var roomid = $("#hdRoomId").val();
                var ruleid = $("#dRule b.active").attr("dataid");
                var vipCode = $("#dVip b.active").attr("dataid");
                var url = "/SysSetBase/houseState/hoSeEdit.aspx?HotelId=" + hotelid + "&RoomId=" + roomid + "&RuleId=" + ruleid + "&VipCode=" + vipCode + "&DateRange=" + day;
                top.art.dialog.open(url, {
                    id: 'hoSeEdit',
                    title: '修改客房设置',
                    width: 600,
                    height: 490,
                    close: function () {
                        closeRefresh();
                    }
                }, false);
            } else {
                showTipsMsg('已过去的日期不能选择！', '5000', '5');
            }
        });
    </script>
    <script type="text/javascript">
        //按周设置
        //获取售房规则
        function GetWeekRule() {
            $("#dWeekRule b").remove();
            $.ajax({
                url: "../Ajax/SysAjax.ashx",
                data: {
                    Menu: "GetRule",
                    RoomId: $("#hdRoomId").val(),
                    DisplayType: $("#id_hid").val()
                },
                type: "GET",
                datatype: "text",
                async: false,
                success: function (data) {
                    $("#dWeekRule").append(data);
                }
            });
        }

        //售房规则事件
        $("#dWeekRule").on("click", "b", function () {
            $(this).addClass('active').siblings('b').removeClass('active');
            GetWeekPriceTable();
        });

        //会员等级
        $("#dWeekVip").on("click", "b", function () {
            $(this).addClass('active').siblings('b').removeClass('active');
            GetWeekPriceTable();
        });

        //显示分类
        $('#dWeekShowType').on('click', 'b', function () {
            var st = $(this).attr("ShowType");
            if ($(this).hasClass('active')) {
                $(this).removeClass('active');
                $("#tbWeekList .currInfo").find("." + st).hide();
            } else {
                $(this).addClass('active');
                $("#tbWeekList .currInfo").find("." + st).show();
            }
        });
        //上一周
        $("#iLastWeek").on("click", function () {
            $.post("../Ajax/SysAjax.ashx", {
                Menu: "WeekDate",
                Type: "LastWeek",
                DateRange: $("#txtWeekDate").val()
            }, function (data) {
                $("#txtWeekDate").val(data);
                GetWeekPriceTable();
            });
        });
        //下一周
        $("#iNextWeek").on("click", function () {
            $.post("../Ajax/SysAjax.ashx", {
                Menu: "WeekDate",
                Type: "NextWeek",
                DateRange: $("#txtWeekDate").val()
            }, function (data) {
                $("#txtWeekDate").val(data);
                GetWeekPriceTable();
            });
        });
        //加载 周 展示表格
        function GetWeekPriceTable() {
            $.ajax({
                url: "../Ajax/SysAjax.ashx",
                data: {
                    Menu: "GetWeekPriceTable",
                    HotelId: $("#hdHotelId").val(),
                    RoomId: $("#hdRoomId").val(),
                    RuleId: $("#dWeekRule b.active").attr("dataid"),
                    VipCode: $("#dWeekVip b.active").attr("dataid"),
                    DateRange: $("#txtWeekDate").val()
                },
                type: "GET",
                dataType: "text",
                async: false,
                success: function (r) {
                    $("#tbWeekList").html(r);
                    $("#dWeekShowType b").each(function () {
                        var st = $(this).attr("ShowType");
                        if (!$(this).hasClass('active')) {
                            $("#tbWeekList .currInfo").find("." + st).hide();
                        }
                    });
                }
            });
        }

        //开启状态
        $('#tbWeekList').on('click', 'i', function (e) {
            if ($("#hdHotelId").val() == "" || $("#hdHotelId").val() == "0") {
                showTipsMsg('请先选择酒店！', '5000', '5');
                return;
            }
            var RoomId = $(this).parent().attr("RoomId");
            var RuleId = $(this).parent().attr("RuleId");
            var day = $(this).parent().attr("day");
            var myDate = new Date(); //获取当前时间
            var myday = myDate.getFullYear() + "-" + parseInt(parseInt(myDate.getMonth()) + 1) + "-" + myDate.getDate()
            if (Date.parse(day) >= Date.parse(myday)) {
                var roomState = "0";
                if ($(this).hasClass('kgClose')) {
                    $(this).removeClass('kgClose');
                    roomState = "1";
                } else {
                    $(this).addClass('kgClose');
                    roomState = "0";
                }

                $.post("../Ajax/SysAjax.ashx", {
                    Menu: "RoomState",
                    HotelId: $("#hdHotelId").val(),
                    RoomId: RoomId,
                    RuleId: RuleId,
                    VipCode: $("#dWeekVip b.active").attr("dataid"),
                    DateRange: day,
                    RoomState: roomState
                }, function (data) {

                });
            } else {
                showTipsMsg('已过去的日期不能选择！', '5000', '5');
            }
            e.stopPropagation();
        });

        //按 周 表格点击事件
        $('#tbWeekList').on('click', 'td', function () {
            if ($("#hdHotelId").val() == "" || $("#hdHotelId").val() == "0") {
                showTipsMsg('请先选择酒店！', '5000', '5');
                return;
            }
            var day = $(this).attr("day");
            var myDate = new Date(); //获取当前时间
            var myday = myDate.getFullYear() + "-" + parseInt(parseInt(myDate.getMonth()) + 1) + "-" + myDate.getDate();
            if (Date.parse(day) >= Date.parse(myday)) {
                var hotelid = $("#hdHotelId").val();
                var roomid = $(this).attr("roomid");
                var ruleid = $(this).attr("ruleid");
                var vipCode = $("#dWeekVip b.active").attr("dataid");
                var url = "/SysSetBase/houseState/hoSeEdit.aspx?HotelId=" + hotelid + "&RoomId=" + roomid + "&RuleId=" + ruleid + "&VipCode=" + vipCode + "&DateRange=" + day;
                top.art.dialog.open(url, {
                    id: 'hoSeEdit',
                    title: '修改客房设置',
                    width: 600,
                    height: 490,
                    close: function () {
                        closeRefresh();
                    }
                }, false);
            } else {
                showTipsMsg('已过去的日期不能选择！', '5000', '5');
            }
        });

        function closeRefresh() {
            if ($("#id_hid").val() == "2") {
                GetMonthPriceTable();
            } else if ($("#id_hid").val() == "3") {
                GetWeekPriceTable();
            }
        }
    </script>
</body>
</html>
