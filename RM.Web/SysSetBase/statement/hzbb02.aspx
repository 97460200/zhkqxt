<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="hzbb02.aspx.cs" Inherits="RM.Web.SysSetBase.statement.hzbb02" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>汇总报表</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script src="/WDatePicker/WdatePicker.js" type="text/javascript"></script>
    <style type="text/css">
        .rge .rgetable td
        {
            background-color:#fff;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <input id="AdminHotelid" type="hidden" runat="server" />
    <div class="shareframe">
        <div class="shareframel"> 
            <dl class="addsearch">
                <dd>
                    <small>酒店名称</small>
                    <div>
                        <select id="hotellist">
                            
                        </select>
                    </div>
                </dd>
                <dd>
                    <small>房型</small>
                    <div>
                        <select id="roomlist">
                            
                        </select>
                    </div>
                </dd>
                <dd>
                    <small>支付方式</small>
                    <div>
                        <select id="zffs">
                            <option value="0">全部</option>
                            <option value="4">微信支付</option>
                            <option value="3">会员卡支付</option>
                            <option value="2">积分支付</option>
                        </select>
                    </div>
                </dd>
                <dd>
                    <small>日期类型</small>
                    <div class="radio" id="rqlx">
                        <label class="checked" val="1">下单日期</label>
                        <label val="2">入住日期</label>
                        <label val="3">在店日期</label>
                        <label val="4">离店日期</label>
                        <label val="5">结账日期</label>
                    </div>
                </dd>
                <dd>
                    <small>开始时间</small>
                    <div class="sharedate01">
                    <input type="text"  id="txtStartTime" runat="server" class="Wdate" onfocus="WdatePicker({dateFmt: 'yyyy-MM-dd'})"  placeholder="">
                    </div>
                </dd>
                <dd>
                    <small>结束时间</small>
                    <div class="sharedate01">
                    <input type="text" id="txtEndTime" runat="server" class="Wdate" onfocus="WdatePicker({dateFmt: 'yyyy-MM-dd'})"  placeholder="">
                    </div>
                </dd>
                <dd>
                    <small>关键词</small>
                    <div>
                        <input type="text" name="name" value="" id="gjc" />
                    </div>
                </dd>
                <dd>
                    <small>报表类型</small>
                    <div class="radio" id="bblx">
                        <label class="checked" val="1">统计表</label>
                        <label val="2">明细表</label>
                    </div>
                </dd>

                <div class="sharesearchbtn">
                    <input type="submit" name="btnSumit" value="查询" onclick="ListGrid();return false;">
                </div>
            </dl>            
        </div>
        <div class="zhankai">
        </div>
        <div class="shareframer">
            <div class="hzbb" id="bbmc">
                    客房统计报表
                </div>

                <div class="bbgl" style="" id="kftjbb">
                <div class="d2 clearfix">
                    <div class="d22">
                        <span id="Span1">制表时间：2016-04-28 13:00</span><span class="dc">导出</span>
                    </div>
                </div>
                <div class="rge">
                    <div class="rgetable">
                        <table class="ul" >
                            <thead>
                                <tr>
                                    <th width="40">
                                        序号
                                    </th>
                                    <th>
                                        酒店名称
                                    </th>
                                    <th>
                                        房型
                                    </th>
                                    <th>
                                        售房规则
                                    </th>
                                    <th>
                                        间页数
                                    </th>
                                    <th>
                                        客房销售额
                                    </th>
                                    <th>
                                        平均房价
                                    </th>
                                    <th>
                                        回头房晚数
                                    </th>
                                    <th>
                                        回头率
                                    </th>
                                    <th style=" display:none">
                                        总占比
                                    </th>
                                </tr>
                            </thead>
                            <tbody id="Tbody2">
                                
                               
                                
                                
                                
                                
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>

            <div class="bbgl" id="kftjmxb" style=" display:none">
                <div class="d2 clearfix">
                    <div class="d22">
                        <span id="zbsj">制表时间：2016-04-28 13:00</span><span class="dc">导出</span>
                    </div>
                </div>
                <div class="rge">
                    <div class="rgetable">
                        <table class="ul">
                            <thead>
                                <tr>
                                    <th width="40">
                                        序号
                                    </th>
                                    <th>
                                        酒店名称
                                    </th>
                                    <th>
                                        房型
                                    </th>
                                    <th>
                                        售房规则
                                    </th>
                                    <th>
                                        下单时间
                                    </th>
                                    <th>
                                        订单编号
                                    </th>
                                    <th>
                                        订单状态
                                    </th>
                                    <th>
                                        会员ID
                                    </th>
                                    <th>
                                        会员姓名
                                    </th>
                                    <th>
                                        会员手机号
                                    </th>
                                    <th>
                                        间夜数
                                    </th>
                                    <th>
                                        支付金额
                                    </th>
                                    <th>
                                        支付方式
                                    </th>
                                    <th>
                                        下单联系方式
                                    </th>
                                </tr>
                            </thead>
                            <tbody id="Tbody6">
                                
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            
        </div>
    </div>
    </form>
    <script src="/SysSetBase/css/jquery.js" type="text/javascript"></script>
    <script type="text/javascript">
        $('.radio').on('click', 'label', function () {
            $(this).addClass('checked').siblings().removeClass('checked');
        });


        //        展开收起
        $('.zhankai').on('click', function () {
            $(this).hasClass('act') ? $(this).removeClass('act') : $(this).addClass('act');
            $(".shareframel").toggle();
        });
 
    </script>

    <script type="text/javascript">

        $(function () {
            jdlist(); kflist();
            ListGrid();
            $("#hotellist").change(function () {
                kflist();
            });
        });

        function jdlist() {
            $.ajax({
                url: "/SysSetBase/Ajax/SysAjax.ashx",
                data: {
                    Menu: "GetHotelList",
                    adminhotelid: $("#AdminHotelid").val()
                },
                type: "POST",
                dataType: "text",
                success: function (data) {
                    
                    var list = "<option value='0'>全部酒店</option>";
                    if (data == "") {
                        $("#hotellist").html(list);
                        return;
                    }
                    var json = eval("(" + data + ")");

                    for (var i = 0; i < json.length; i++) {
                        list += "<option value='" + json[i].ID + "'>" + json[i].NAME + "</option>";
                    }
                    $("#hotellist").html(list);
                    
                }
            });
        }

        function kflist() {
            $.ajax({
                url: "/SysSetBase/Ajax/SysAjax.ashx",
                data: {
                    Menu: "GetroomList",
                    hotelid: $("#hotellist").val(),
                    adminhotelid: $("#AdminHotelid").val()
                },
                type: "POST",
                dataType: "text",
                success: function (data) {
                    
                    var list = "<option value='0'>全部房型</option>";
                    if (data == "") {
                        $("#roomlist").html(list);
                        return;
                    }
                    var json = eval("(" + data + ")");

                    for (var i = 0; i < json.length; i++) {
                        list += "<option value='" + json[i].ID + "'>" + json[i].NAME + "</option>";
                    }
                    $("#roomlist").html(list);
                }
            });
        }


        function ListGrid() {
            var myDate = new Date(); //获取系统当前时间
            $("#Span1").text("制表时间：" + myDate.getFullYear() + "-" + (myDate.getMonth() * 1 + 1) + "-" + myDate.getDate() + " " + myDate.getHours() + ":" + myDate.getMinutes());
            $("#zbsj").text("制表时间：" + myDate.getFullYear() + "-" + (myDate.getMonth() * 1 + 1) + "-" + myDate.getDate() + " " + myDate.getHours() + ":" + myDate.getMinutes());
            
            var bblx = $("#bblx").find(".checked").eq(0).attr("val");

            var kfid = $("#roomlist").val();
            //var txtSearch = $.trim($("#txtSearch").val());
            //alert(bblx);

            var rqlx = $("#rqlx").find(".checked").attr("val");
            if (bblx == "1") {
                $("#Tbody2").html("");
                $("#kftjbb").show();
                $("#kftjmxb").hide();
                $("#bbmc").html("客房统计报表");
                $.ajax({
                    url: "ajaxbb.ashx",
                    data: {
                        Menu: "Getkftj",
                        AdminHotelid: $("#AdminHotelid").val(),
                        hotelid: $("#hotellist").val(),
                        zflx: $("#zffs").val(),
                        rqlx: rqlx,
                        kssj: $("#txtStartTime").val(),
                        jssj: $("#txtEndTime").val(),
                        gjc: $("#gjc").val(),
                        kfid: kfid

                    },
                    type: "POST",
                    success: function (data) {
                        if (data == null || data == "") {
                            alert("未找到数据");
                            return;
                        }
                        
                        $("#Tbody2").html(data);
                    }
                });
            } else if (bblx == "2") {
                $("#Tbody6").html("");
                $("#kftjbb").hide();
                $("#kftjmxb").show();
                $("#bbmc").html("客房统计明细表");
                $.ajax({
                    url: "ajaxbb.ashx",
                    data: {
                        Menu: "Getkfmx",
                        AdminHotelid: $("#AdminHotelid").val(),
                        hotelid: $("#hotellist").val(),
                        zflx: $("#zffs").val(),
                        rqlx: rqlx,
                        kssj: $("#txtStartTime").val(),
                        jssj: $("#txtEndTime").val(),
                        gjc: $("#gjc").val(),
                        kfid: kfid

                    },
                    type: "POST",

                    success: function (data) {
                        if (data == null || data == "") {
                            alert("未找到数据");
                            return;
                        }
                        
                        $("#Tbody6").html(data);
                    }
                });
            }
            
        }

    </script>
</body>
</html>
