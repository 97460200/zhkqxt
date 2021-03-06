﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberReport.aspx.cs" Inherits="RM.Web.SysSetBase.statement.MemberReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>会员报表</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script src="/WDatePicker/WdatePicker.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <input id="AdminHotelid" type="hidden" runat="server" />
    <div class="shareframe">
        <div class="shareframel"> 
            <dl class="addsearch">
                <dd style=" display:none">
                    <small>酒店名称</small>
                    <div>
                        <select id="hotellist">
                            
                        </select>
                    </div>
                </dd>
                <dd style=" display:none">
                    <small>销售渠道</small>
                    <div>
                        <select id="xsqd">
                            
                        </select>
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
                    <input type="text"  id="txtEndTime" runat="server" class="Wdate" onfocus="WdatePicker({dateFmt: 'yyyy-MM-dd'})"  placeholder="">
                    </div>
                </dd>
                <dd>
                    <small>关键词</small>
                    <div>
                        <input type="text" name="name" value="" />
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
                    会员统计报表
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
                                        会员等级
                                    </th>
                                    <th>
                                        人数
                                    </th>
                                    <th>
                                        人数占比
                                    </th>
                                    <th>
                                        充值总额
                                    </th>
                                    <th>
                                        平均充值
                                    </th>
                                    <th>
                                        消费总额
                                    </th>
                                    <th>
                                        平均消费
                                    </th>
                                    <th>
                                        余额
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
                                        卡券名称
                                    </th>
                                    <th>
                                        获得方式
                                    </th>
                                    <th>
                                        面值
                                    </th>
                                    <th>
                                        使用期限
                                    </th>
                                    <th>
                                        会员名
                                    </th>
                                    <th>
                                        手机号
                                    </th>
                                    <th>
                                        酒店名称
                                    </th>
                                    <th>
                                        房型
                                    </th>
                                    <th>
                                        使用状态
                                    </th>
                                    <th>
                                        使用日期
                                    </th>
                                    <th>
                                        订单号
                                    </th>
                                    <th>
                                        卡券状态
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

                    var list = "<option value='0'>全部</option>";
                    if (data == "") {
                        $("#roomlist").html(list);
                        return;
                    }
                    //                    var json = eval("(" + data + ")");

                    //                    for (var i = 0; i < json.length; i++) {
                    //                        list += "<option value='" + json[i].ID + "'>" + json[i].NAME + "</option>";
                    //                    }
                    //                    $("#roomlist").html(list);
                }
            });
        }


        function ListGrid() {
            var myDate = new Date(); //获取系统当前时间
            $("#Span1").text("制表时间：" + myDate.getFullYear() + "-" + (myDate.getMonth() * 1 + 1) + "-" + myDate.getDate() + " " + myDate.getHours() + ":" + myDate.getMinutes());
            $("#zbsj").text("制表时间：" + myDate.getFullYear() + "-" + (myDate.getMonth() * 1 + 1) + "-" + myDate.getDate() + " " + myDate.getHours() + ":" + myDate.getMinutes());

            var bblx = 1;

            var kfid = $("#roomlist").val();
            //var txtSearch = $.trim($("#txtSearch").val());
            //alert(bblx);
            if (bblx == "1") {
                $("#Tbody2").html("");
                $("#kftjbb").show();
                $("#kftjmxb").hide();
                $("#bbmc").html("会员统计报表");
                $.ajax({
                    url: "ajaxbb.ashx",
                    data: {
                        Menu: "MemberReport",
                        AdminHotelid: $("#AdminHotelid").val(),
                        StartDate: $("#txtStartTime").val(),
                        EndDate: $("#txtEndTime").val()
                        //CouponList: $("#CouponList").val()


                    },
                    type: "POST",
                    success: function (data) {
                        //alert(data);
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
                $("#bbmc").html("卡券统计明细表");
                $.ajax({
                    url: "ajaxbb.ashx",
                    data: {
                        Menu: "MemberReport",
                        AdminHotelid: $("#AdminHotelid").val(),
                        StartDate: $("#txtStartTime").val(),
                        EndDate: $("#txtEndTime").val(),
                        gjc: $("#gjc").val()


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
