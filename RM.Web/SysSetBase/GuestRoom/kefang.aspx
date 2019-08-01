<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="kefang.aspx.cs" Inherits="RM.Web.SysSetBase.GuestRoom.kefang" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/PqGrid/pqgrid.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <!--除了图标 都不要使用<i>-->
    <div class="gtall gmkf clearfix">
        <div class="gmkfNav">
            <dl id="gmkfNav">
            </dl>
        </div>
        <div class="gmkfStion">
            <div class="wdyhd" style="padding-left: 20px; max-width: 1200px;">
                <div class="l">
                    <a class="active" hid="1">默认设置</a><a hid="2">每日设置</a>
                </div>
                <div class="memGrade">
                    <em>会员等级</em><%=hydjHtml%>
                </div>
                <div class="evRgt">
                    <select id="nian">
                        <%=yearHtml%>
                    </select>
                    <i class="icon-left"></i>
                    <select id="yuefen" runat="server">
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
                    <i class="icon-right"></i>
                </div>
                <em class="zhus">注：<i class="icon-starfull"></i>为自定义客房价格 </em>
            </div>
            <ul class="wdySetup">
                <%--默认设置--%>
                <li style="overflow: auto;">
                    <div class="gmkfList" id="tbhtml" style="width: 800px;">
                    </div>
                    <strong><b>酒店客房预订最长时间设置</b></strong>
                    <div class="gmlong">
                        时长<select id="rili" runat="server">
                            <option value="1">1</option>
                            <option value="2" selected>2</option>
                            <option value="3">3</option>
                            <option value="4">4</option>
                            <option value="5">5</option>
                            <option value="6">6</option>
                            <option value="7">7</option>
                            <option value="8">8</option>
                            <option value="9">9</option>
                            <option value="10">10</option>
                            <option value="11">11</option>
                            <option value="12">12</option>
                        </select>个月
                    </div>
                    <a class="button buttonActive" id="btnsave">保存</a> </li>
                <%--每日设置--%>
                <li>
                    <div class="daytable">
                        <table style="width: 1200px;">
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
                            <tbody id="divdate">
                                <tr>
                                    <td>
                                        <b class="date">26</b>
                                    </td>
                                    <td>
                                        <b class="date">27</b>
                                    </td>
                                    <td>
                                        <b class="date">28</b>
                                    </td>
                                    <td>
                                        <b class="date">29</b>
                                    </td>
                                    <td>
                                        <b class="date">30</b>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small><span class="dz">单早</span></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small><span class="dz">单早</span></span>
                                            <%-- --%><span><small>可订房</small><small>10</small></span>
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td class="today">
                                        <b class="date">31</b>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small><span class="dz">单早</span></span>
                                            <%-- --%><span><small>需积分</small><small>723800</small><span class="dz">单早</span></span>
                                            <%-- --%><span><small>可订房</small><small>10</small></span>
                                        </div>
                                        <i class="icon-kg1 kgClose"></i>
                                    </td>
                                    <td>
                                        <b class="date">1</b>
                                        <div class="currInfo">
                                            <%-- --%><span><small class="star">活动价</small><small>138</small><span class="dz">单早</span></span>
                                            <%-- --%><span><small>需积分</small><small>800</small><span class="dz">单早</span></span>
                                            <%-- --%><span><small>可订房</small><small>10</small></span>
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </li>
            </ul>
        </div>
    </div>
    <div style="display: none" id="hotelnameList">
        <dd>
            <b hotelid="{@hotelid}">{@hotelname}</b>
            <ul>
                {@kfnameList}
            </ul>
        </dd>
    </div>
    <input runat="server" id="adminhotelid" type="hidden" />
    <%-- 弹窗 start--%>
    <div class="Alert">
        <h2 onclick="Alert();">
            客房参数管理 > 编辑客房参数 <i class="icon-boldclose" onclick="$('.Alert').fadeOut(200);"></i>
        </h2>
        <div class="altconX">
            <ul class="editkf">
                <li><small>酒店名称</small><b>柏丽酒店</b></li>
                <li><small>客房名称</small><b>特色大床房</b></li>
                <li><small>日期</small><b>2018-01-12</b></li>
                <li><small>会员等级</small><b>微会员</b></li>
                <li class="inp"><small>价格类型</small><b><select class="half">
                    <option value="0">自定义价格</option>
                </select><input type="text" value="100" class="half" /></b></li>
                <li class="inp"><small>客房价格</small><b><input type="text" value="100" />元</b></li>
                <li class="inp"><small>所需积分</small><b><input type="text" value="100" />分</b></li>
                <li class="inp"><small>可订房数</small><b><input type="text" value="100" />间</b></li>
                <li><small>早餐</small><b><label class="checked">无早</label><label>单早</label><label>双早</label></b></li>
            </ul>
        </div>
        <div class="altBtn">
            <a class="button buttonActive" onclick="Rm.F_alert('sure');">提交</a>
        </div>
    </div>
    <div class="tip">
    </div>
    <div class="load" style="display: none">
    </div>
    <%-- 弹窗 end--%>
    <input runat="server" id="hdVal" type="hidden" value="" />
    <input runat="server" id="GSID" type="hidden" value="" />
    <input runat="server" id="HotelID" type="hidden" value="" />
    <%--1是客房价格2是日历价格--%>
    <input runat="server" id="id_hid" type="hidden" value="1" />
    <%--会员等级--%>
    <input runat="server" id="hydj" type="hidden" value="5" />
    <%--当前年--%>
    <input runat="server" id="year" type="hidden" value="2017" />
    </form>
    <script src="/SysSetBase/css/ScrollBar.js" type="text/javascript"></script>
    <script src="/SysSetBase/css/IE.js" type="text/javascript"></script>
    <script type="text/javascript">
        $('#divdate').on('click', 'td', function () {
            var day = $(this).attr("day");
            var myDate = new Date(); //获取当前时间
            var myday = myDate.getFullYear() + "-" + parseInt(parseInt(myDate.getMonth()) + 1) + "-" + myDate.getDate()
            if (Date.parse(day) >= Date.parse(myday)) {
                var url = "/RMBase/SysCalendar/ProductEdit.aspx?day=" + 1 + "&ddlid=" + 1 + "&hid=" + 1 + "&name=" + 1;
                top.art.dialog.open(url, {
                    id: 'ProductEdit',
                    title: '客房参数管理  > 编辑客房参数',
                    width: 320,
                    height: 410,
                    close: function () {

                    }
                }, false);
            } else {
                Tip("已过去的日期不能选择！");
            }
        })

        //左边导航
        $('.gmkfNav').on('click', 'b', function () {
            $(this).siblings('ul').slideToggle(120);
            $(this).parents('dd').toggleClass('down');
        });
        $(".gmkfNav").panel({ iWheelStep: 80 });



        $(function () {
            hotel();
        })

        function hotel() {
            $.ajax({
                url: "../Ajax/SysAjax.ashx",
                data: {
                    Menu: "GetHotelList",
                    adminhotelid: $("#adminhotelid").val()
                },
                type: "GET",
                datatype: "JSON",
                async: false,
                success: function (data) {
                    if (data == "") {
                        return;
                    }
                    var json = eval("(" + data + ")");
                    var hftr = $("#hotelnameList").html();
                    $("#gmkfNav").empty();
                    for (var i = 0; i < json.length; i++) {
                        var copytr = hftr;
                        copytr = copytr.replace("{@hotelid}", json[i].ID);
                        copytr = copytr.replace("{@hotelname}", json[i].NAME);
                        if (json[i].kf != undefined && json[i].kf != "") {
                            var html = "";
                            for (var s = 0; s < json[i].kf.length; s++) {
                                html += "<li GSID=" + json[i].kf[s].id + "><span>" + json[i].kf[s].name + "</span> </li>";
                            }
                            copytr = copytr.replace("{@kfnameList}", html);
                        } else {
                            copytr = copytr.replace("{@kfnameList}", "");
                        }
                        $("#gmkfNav").append(copytr);
                    }
                }
            })

            $.ajax({
                url: "../Ajax/SysAjax.ashx",
                data: {
                    Menu: "GetMemberType",
                    adminhotelid: $("#adminhotelid").val()
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
            })
        }

        function getTextVal(gsid) {
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
                            $("#" + tvals[0]).val(tvals[1]).next().next().val(val.ZAOCAN);
                            tvals = val.JF.split(',');
                            $("#" + tvals[0]).val(tvals[1]).next().next().val(val.JF_ZAOCAN);
                            tvals = val.FJS.split(',');
                            $("#" + tvals[0]).val(tvals[1]).next().next().val(val.ZAOCAN);
                        });
                    }
                    else {

                    }
                }
            });
        }


        //右边设置tab切换
        $('.wdyhd .l').on('click', 'a', function () {
            if ($(this).hasClass('active')) return;
            $(this).addClass('active').siblings().removeClass('active');
            $("#id_hid").val($(this).attr("hid"));
            var index = $(this).index();
            $('.wdySetup li').eq(index).show().siblings().hide();
            $('.gmkfStion').toggleClass('noStrong');  //修改部分布局 -- 如：每日设置的注释

            if ($("#id_hid").val() == "1") {
                var gsid = $("#GSID").val();
                getTextVal(gsid)
            } else {
                rililist()
            }
        });

        ///客房点击事件
        $("#gmkfNav").on('click', "li", function () {
            $("#gmkfNav li").removeClass("active");
            $(this).addClass("active");
            var gsid = $(this).attr("gsid");
            $("#GSID").val(gsid)
            $("#HotelID").val($(this).parent().siblings().attr("hotelid"));
            if ($("#id_hid").val() == "1") {
                getTextVal(gsid)
            } else {
                rililist()
            }
        });


        //每日设置 -- 会员等级
        $('.evDayhd').on('click', 'b', function () {
            $(this).addClass('active').siblings('b').removeClass('active');
            $("#hydj").val($(this).attr("hydj"));
            rililist();
        });


        //每日设置 -- 日期开关按钮
        $('.daytable').on('click', 'icon-kg1', function () {
            $('.icon-kg1').hasClass('kgClose') ? $('.icon-kg1').removeClass('kgClose') : $('.icon-kg1').addClass('kgClose')
        });

        //年份选择
        $("#nian").change(function () {
            rililist();
        });

        //月份选择
        $("#yuefen").change(function () {
            rililist();
        });
    </script>
    <script>


        $("#btnsave").click(function () {
            checkText();
        });

        function checkText() {
            var arr = [];
            var hdvals = [];
            $("#tbhtml input[type=text]").each(function () {
                var hydj = $(this).attr("hydj");
                var zc = $(this).next().next(); //早餐
                var zc_val = ",无早";
                if (zc.length > 0) {
                    zc_val = "," + zc.val();
                }

                var MemberType = $(this).attr("MemberType");
                var tVal = $(this).val() == "" ? "0" : $(this).val();
                var nameval = $(this).attr("name");
                var ia = $.inArray(hydj + "*" + MemberType, arr);
                if ($.inArray(hydj + "*" + MemberType, arr) < 0) {
                    arr.push(hydj + "*" + MemberType);
                    if (MemberType == "2" || MemberType == "3" || MemberType == "4") {
                        hdvals.push("|" + hydj + "," + MemberType + "," + tVal + ",{jf}" + ",{fjs}" + zc_val + "{jf_zaocan}");
                    } else {
                        hdvals.push("|" + hydj + "," + MemberType + "," + tVal + ",0" + ",0" + zc_val + ",无早");
                    }
                } else {
                    if (nameval.indexOf("jf") >= 0) {
                        hdvals[ia] = hdvals[ia].replace("{jf}", tVal).replace("{jf_zaocan}", zc_val);
                    }
                    if (nameval.indexOf("fjs") >= 0) {
                        hdvals[ia] = hdvals[ia].replace("{fjs}", tVal);
                    }
                }
            });
            $("#hdVal").val(hdvals.toString());

            if ($("#GSID").val() == "") {
                Tip("请选择客房！");
                return false;
            }
            $.ajax({
                url: "../Ajax/SysAjax.ashx",
                data: {
                    Menu: "Save",
                    hid: $("#HotelID").val(),
                    gsid: $("#GSID").val(),
                    vals: hdvals.toString(),
                    adminhotelid: $("#adminhotelid").val()
                },
                type: "GET",
                dataType: "text",
                async: false,
                success: function (response) {
                    Tip(response);
                }
            });

            $.ajax({
                url: "../Ajax/SysAjax.ashx",
                data: {
                    Menu: "Saverili",
                    rili: $("#rili").val(),
                    adminhotelid: $("#adminhotelid").val()
                },
                type: "GET",
                dataType: "text",
                async: false,
                success: function (response) {

                }
            });

        }


        function rililist() {
            $.ajax({
                url: "../Ajax/SysAjax.ashx",
                data: {
                    Menu: "rililist",
                    adminhotelid: $("#adminhotelid").val(),
                    year: $("#year").val(),
                    Month: $("#yuefen").val(),
                    hydj: $("#hydj").val(),
                    gsid: $("#GSID").val(),
                    hotelid: $("#HotelID").val()
                },
                type: "GET",
                dataType: "text",
                async: false,
                success: function (r) {
                    $("#divdate").html(r);
                }
            });
        }
    </script>
</body>
</html>
