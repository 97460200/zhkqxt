<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="hzbb01.aspx.cs" Inherits="RM.Web.SysSetBase.statement.hzbb01" %>

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
</head>
<body style="overflow:auto;">
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
                    <small>月份</small>
                    <div>
                        <select id="yuefen">
                        <option value="">请选择</option>
                            <option value="2018-01-01">2018年1月</option>
                            <option value="2018-02-01">2018年2月</option>
                            <option value="2018-03-01">2018年3月</option>
                            <option value="2018-04-01">2018年4月</option>
                            <option value="2018-05-01">2018年5月</option>
                            <option value="2018-06-01">2018年6月</option>
                            <option value="2018-07-01">2018年7月</option>
                            <option value="2018-08-01">2018年8月</option>
                            <option value="2018-09-01">2018年9月</option>
                            <option value="2018-10-01">2018年10月</option>
                            <option value="2018-11-01">2018年11月</option>
                            <option value="2018-12-01">2018年12月</option>
                            <option value="2019-01-01" selected="selected">2019年1月</option>
                        </select>
                    </div>
                </dd>
                <dd style=" display:none">
                    <small>关键词</small>
                    <div>
                        <input type="text" name="name" value="" id="gjc" />
                    </div>
                </dd>
                <div class="sharesearchbtn">
                    <input type="submit" name="btnSumit" onclick="ListGrid();return false;" value="查询">
                </div>
            </dl>
            
            
        </div>
        <div class="zhankai">
        </div>
        <div class="shareframer">
            
            <div class="hzbb">
                    汇总报表
                </div>
            <div class="bbgl">
                <div class="d2 clearfix">
                    <div class="d21">
                        酒店数据-汇总
                    </div>
                    <div class="d22" id="Span1">
                        制表时间：2016-04-28 13:00
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
                                    <th width="40">
                                        时间
                                    </th>
                                    <th>
                                        总会员数
                                    </th>
                                    <%--<th>
                                        总有效会员数
                                    </th>--%>
                                    <th>
                                        总销售额
                                    </th>
                                    <th>
                                        总房晚数
                                    </th>
                                   <%-- <th>
                                        其他销售额
                                    </th>--%>
                                    <th>
                                        充值总额
                                    </th>
                                    <th>
                                        会员卡消费总额
                                    </th>
                                </tr>
                            </thead>
                            <tbody id="tb">
                                
                                
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <div class="bbgl" style=" display:none;">
                <div class="d2 clearfix">
                    <div class="d21">
                        酒店数据-分销售渠道
                    </div>
                    <div class="d22" id="Span2">
                        制表时间：2016-04-28 13:00
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
                                        销售渠道
                                    </th>
                                    <th>
                                        客房销售额
                                    </th>
                                    <th>
                                        其他销售额
                                    </th>
                                    <th>
                                        总销售额
                                    </th>
                                    <th>
                                        销售额占比
                                    </th>
                                </tr>
                            </thead>
                            <tbody id="Tbody1">
                                <tr>
                                    <td>
                                        1
                                    </td>
                                    <td>
                                        酒店员工
                                    </td>
                                    <td>
                                        10000.00
                                    </td>
                                    <td>
                                        10000.00
                                    </td>
                                    <td>
                                        10000.00
                                    </td>
                                    <td>
                                        23%
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        2
                                    </td>
                                    <td>
                                        酒店代理
                                    </td>
                                    <td>
                                        11163.00 
                                    </td>
                                    <td>
                                        11163.00 
                                    </td>
                                    <td>
                                        11163.00 
                                    </td>
                                    <td>
                                        66%
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        3
                                    </td>
                                    <td>
                                        智订云代理
                                    </td>
                                    <td>
                                        5646514.00 
                                    </td>
                                    <td>
                                        5646514.00 
                                    </td>
                                    <td>
                                        5646514.00 
                                    </td>
                                    <td>
                                        6%
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        4
                                    </td>
                                    <td>
                                        官微
                                    </td>
                                    <td>
                                        4565.00 
                                    </td>
                                    <td>
                                        4565.00 
                                    </td>
                                    <td>
                                        4565.00 
                                    </td>
                                    <td>
                                        66%
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        5
                                    </td>
                                    <td style="font-weight:bold;">
                                        合计
                                    </td>
                                    <td class="color01">
                                        5646514.00 
                                    </td>
                                    <td class="color01">
                                        5646514.00 
                                    </td>
                                    <td class="color01">
                                        5646514.00 
                                    </td>
                                    <td class="color01">
                                       
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>

            </div>
            <div class="bbgl" style=" display:none;">
                <div class="d2 clearfix">
                    <div class="d21">
                        酒店数据-官微浏览量
                    </div>
                    <div class="d22" id="Span3">
                        制表时间：2016-04-28 13:00
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
                                        时间
                                    </th>
                                    <th>
                                        总浏览量（PV）
                                    </th>
                                    <th>
                                        首页浏览量
                                    </th>
                                    <th>
                                        客房列表浏览量
                                    </th>
                                    <th>
                                        会员中心浏览量
                                    </th>
                                    <th>
                                        酒店简介浏览量
                                    </th>
                                    <th>
                                        实景欣赏浏览量
                                    </th>
                                </tr>
                            </thead>
                            <tbody id="Tbody2">
                                <tr>
                                    <td>
                                        1
                                    </td>
                                    <td>
                                        上月(LM)
                                    </td>
                                    <td>
                                        10000.00
                                    </td>
                                    <td>
                                        10000.00
                                    </td>
                                    <td>
                                        10000.00
                                    </td>
                                    <td>
                                        10000.00
                                    </td>
                                    <td>
                                        10000.00
                                    </td>
                                    <td>
                                        10000.00
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        2
                                    </td>
                                    <td>
                                        上月(LM)
                                    </td>
                                    <td>
                                        10000.00
                                    </td>
                                    <td>
                                        10000.00
                                    </td>
                                    <td>
                                        10000.00
                                    </td>
                                    <td>
                                        10000.00
                                    </td>
                                    <td>
                                        10000.00
                                    </td>
                                    <td>
                                        10000.00
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        3
                                    </td>
                                    <td style="font-weight:bold;">
                                        合计
                                    </td>
                                    <td class="color01">
                                        5646514.00 
                                    </td>
                                    <td class="color01">
                                        5646514.00 
                                    </td>
                                    <td class="color01">
                                        5646514.00 
                                    </td>
                                    <td class="color01">
                                        5646514.00 
                                    </td>
                                    <td class="color01">
                                        5646514.00 
                                    </td>
                                    <td class="color01">
                                        5646514.00 
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>


            </div>
            <div class="bbgl">
                <div class="d2 clearfix">
                    <div class="d21">
                        客房数据-分支付方式
                    </div>
                    <div class="d22" id="Span4">
                        制表时间：2016-04-28 13:00
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
                                        销售渠道
                                    </th>
                                    <th>
                                        人次
                                    </th>
                                    <th>
                                        销售额
                                    </th>
                                </tr>
                            </thead>
                            <tbody id="Tbody4">



                            </tbody>
                        </table>
                    </div>
                </div>

            </div>
            <div class="bbgl">
                <div class="d2 clearfix">
                    <div class="d21">
                        客房数据-分房型销售
                    </div>
                    <div class="d22" id="Span5">
                        制表时间：2016-04-28 13:00
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
                                        房型
                                    </th>
                                    <th>
                                        售房规则
                                    </th>
                                    <th>
                                        房晚数
                                    </th>
                                    <th>
                                        房晚数占比
                                    </th>
                                    <th>
                                        销售额(GMV)
                                    </th>
                                    <th>
                                        销售额(GMV)占比
                                    </th>
                                    <th>
                                        平均房价(ADR)
                                    </th>
                                </tr>
                            </thead>
                            <tbody id="Tbody3">
                                
                                
                                
                            </tbody>
                        </table>
                    </div>
                </div>

            </div>
            <div class="bbgl">
                <div class="d2 clearfix">
                    <div class="d21">
                        客房数据-分支付方式
                    </div>
                    <div class="d22" id="Div1">
                        制表时间：2016-04-28 13:00
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
                                        销售渠道
                                    </th>
                                    <th>
                                        间夜
                                    </th>
                                    <th>
                                        销售额
                                    </th>
                                </tr>
                            </thead>
                            <tbody id="Tbody8">



                            </tbody>
                        </table>
                    </div>
                </div>

            </div>

            <div class="bbgl" style=" display:none">
                <div class="d2 clearfix">
                    <div class="d21">
                        酒店员工销售数据-分部门
                    </div>
                    <div class="d22" id="Span6">
                        制表时间：2016-04-28 13:00
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
                                        部门
                                    </th>
                                    <th>
                                        客房销售量
                                    </th>
                                    <th>
                                        客房销售额
                                    </th>
                                    <th>
                                        客房销售额占比
                                    </th>
                                    <th>
                                        浏览量
                                    </th>
                                    <th>
                                        浏览量占比
                                    </th>
                                    <th>
                                        转化率
                                    </th>
                                </tr>
                            </thead>
                            <tbody id="Tbody5">
                                <tr>
                                    <td>
                                        1
                                    </td>
                                    <td>
                                        前厅部
                                    </td>
                                    <td>
                                        1000
                                    </td>
                                    <td>
                                        1000.00 
                                    </td>
                                    <td>
                                        10% 
                                    </td>
                                    <td>
                                        1000
                                    </td>
                                    <td>
                                        10% 
                                    </td>
                                    <td>
                                        10%
                                    </td>
                                </tr>
                                
                                
                                <tr>
                                    <td>
                                        2
                                    </td>
                                    <td>
                                        前厅部
                                    </td>
                                    <td>
                                        1000
                                    </td>
                                    <td>
                                        1000.00 
                                    </td>
                                    <td>
                                        10% 
                                    </td>
                                    <td>
                                        1000
                                    </td>
                                    <td>
                                        10% 
                                    </td>
                                    <td>
                                        10%
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        3
                                    </td>
                                    <td style="font-weight:bold;">
                                        合计
                                    </td>
                                    <td class="color01">
                                        1000
                                    </td>
                                    <td class="color01">
                                        10000.00  
                                    </td>
                                    <td>
                                    </td>
                                    <td class="color01">
                                        1000
                                    </td>
                                    <td>
                                    </td>
                                    <td > 
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>

            </div>
            <div class="bbgl">
                <div class="d2 clearfix">
                    <div class="d21">
                        酒店员工销售数据-TOP20员工
                    </div>
                    <div class="d22" id="Span7">
                        制表时间：2016-04-28 13:00
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
                                    <%--<th>
                                        部门
                                    </th>--%>
                                    <th>
                                        员工
                                    </th>
                                    <th>
                                        客房销售量
                                    </th>
                                    <th>
                                        客房销售额
                                    </th>
                                    <th>
                                        客房销售额占比
                                    </th>
                                    <%--<th>
                                        浏览量
                                    </th>
                                    <th>
                                        浏览量占比
                                    </th>
                                    <th>
                                        转化率
                                    </th>--%>
                                </tr>
                            </thead>
                            <tbody id="Tbody6">
                                <tr>
                                    <td>
                                        1
                                    </td>
                                    <td>
                                        前厅部
                                    </td>
                                    <td>
                                        里静
                                    </td>
                                    <td>
                                        1000
                                    </td>
                                    <td>
                                        1000.00 
                                    </td>
                                    <td>
                                        10% 
                                    </td>
                                    <td>
                                        1000
                                    </td>
                                    <td>
                                        10% 
                                    </td>
                                    <td>
                                        10%
                                    </td>
                                </tr>
                                
                                
                                <tr>
                                    <td>
                                        2
                                    </td>
                                    <td>
                                        前厅部
                                    </td>
                                    <td>
                                        何思思
                                    </td>
                                    <td>
                                        1000
                                    </td>
                                    <td>
                                        1000.00 
                                    </td>
                                    <td>
                                        10% 
                                    </td>
                                    <td>
                                        1000
                                    </td>
                                    <td>
                                        10% 
                                    </td>
                                    <td>
                                        10%
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        3
                                    </td>
                                    <td style="font-weight:bold;">
                                        合计
                                    </td>
                                    <td>
                                    </td>
                                    <td class="color01">
                                        1000
                                    </td>
                                    <td class="color01">
                                        10000.00  
                                    </td>
                                    <td>
                                    </td>
                                    <td class="color01">
                                        1000
                                    </td>
                                    <td>
                                    </td>
                                    <td > 
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>

            </div>
            <div class="bbgl" style=" display:none">
                <div class="d2 clearfix">
                    <div class="d21">
                        用户分布数据（按手机号归属地）
                    </div>
                    <div class="d22" id="Span8">
                        制表时间：2016-04-28 13:00
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
                                        手机号归属地
                                    </th>
                                    <th>
                                        会员人数
                                    </th>
                                    <th>
                                        会员人数占比
                                    </th>
                                    <th>
                                        购买人数
                                    </th>
                                    <th>
                                        购买人数占比
                                    </th>
                                    <th>
                                        转化率
                                    </th>
                                </tr>
                            </thead>
                            <tbody id="Tbody7">
                                <tr>
                                    <td>
                                        2
                                    </td>
                                    <td>
                                        湖南
                                    </td>
                                    <td>
                                        1000
                                    </td>
                                    <td>
                                        10%
                                    </td>
                                    <td>
                                        1000
                                    </td>
                                    <td>
                                        10% 
                                    </td>
                                    <td>
                                        10%
                                    </td>
                                </tr>
                                
                                
                                <tr>
                                    <td>
                                        3
                                    </td><td>
                                        广东
                                    </td>
                                    <td>
                                        1000
                                    </td>
                                    <td>
                                        10%
                                    </td>
                                    <td>
                                        1000
                                    </td>
                                    <td>
                                        10% 
                                    </td>
                                    <td>
                                        10%
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        3
                                    </td>
                                    <td style="font-weight:bold;">
                                        合计
                                    </td>
                                    <td class="color01">
                                        1000
                                    </td>
                                    <td> 
                                    </td> 
                                    <td class="color01">
                                        1000
                                    </td>
                                    <td>
                                    </td>
                                    <td > 
                                    </td>
                                </tr>
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
            jdlist(); 
            ListGrid();
           
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

        


        function ListGrid() {
            var myDate = new Date(); //获取系统当前时间
            $("#Span1").text("制表时间：" + myDate.getFullYear() + "-" + (myDate.getMonth() * 1 + 1) + "-" + myDate.getDate() + " " + myDate.getHours() + ":" + myDate.getMinutes());
            $("#Span4").text("制表时间：" + myDate.getFullYear() + "-" + (myDate.getMonth() * 1 + 1) + "-" + myDate.getDate() + " " + myDate.getHours() + ":" + myDate.getMinutes());
            $("#Span5").text("制表时间：" + myDate.getFullYear() + "-" + (myDate.getMonth() * 1 + 1) + "-" + myDate.getDate() + " " + myDate.getHours() + ":" + myDate.getMinutes());
            $("#Span7").text("制表时间：" + myDate.getFullYear() + "-" + (myDate.getMonth() * 1 + 1) + "-" + myDate.getDate() + " " + myDate.getHours() + ":" + myDate.getMinutes());
            $("#Div1").text("制表时间：" + myDate.getFullYear() + "-" + (myDate.getMonth() * 1 + 1) + "-" + myDate.getDate() + " " + myDate.getHours() + ":" + myDate.getMinutes());
            $("#tb").html("");
                $.ajax({
                    url: "ajaxbb.ashx",
                    data: {
                        Menu: "Gethztj",
                        AdminHotelid: $("#AdminHotelid").val(),
                        Hotelid: $("#hotelid").val(),
                        StartDate:$("#yuefen").val(),
                        EndDate:$("#yuefen").val()
                    },
                    type: "POST",
                    success: function (data) {
                        
                        if (data == null || data == "") {
                            return;
                        }
                        $("#tb").html(data);
                    }
                });

                $("#Tbody4").html("");
                $.ajax({
                    url: "ajaxbb.ashx",
                    data: {
                        Menu: "Getzftj",
                        AdminHotelid: $("#AdminHotelid").val(),
                        Hotelid: $("#hotelid").val(),
                        StartDate:$("#yuefen").val(),
                        EndDate:$("#yuefen").val()


                    },
                    type: "POST",
                    success: function (data) {
                        
                        if (data == null || data == "") {
                            return;
                        }
                        $("#Tbody4").html(data);
                    }
                });
                $("#Tbody8").html("");
                $.ajax({
                    url: "ajaxbb.ashx",
                    data: {
                        Menu: "Getzfjytj",
                        AdminHotelid: $("#AdminHotelid").val(),
                        Hotelid: $("#hotelid").val(),
                        StartDate:$("#yuefen").val(),
                        EndDate:$("#yuefen").val()
                    },
                    type: "POST",
                    success: function (data) {
                        if (data == null || data == "") {
                            return;
                        }
                        
                        $("#Tbody8").html(data);
                    }
                });
                $("#Tbody3").html("");
                $.ajax({
                    url: "ajaxbb.ashx",
                    data: {
                        Menu: "Getfxtj",
                        AdminHotelid: $("#AdminHotelid").val(),
                        Hotelid: $("#hotelid").val(),
                        StartDate: $("#yuefen").val(),
                        EndDate: $("#yuefen").val()


                    },
                    type: "POST",
                    success: function (data) {
                        if (data == null || data == "") {
                            return;
                        }
                        
                        $("#Tbody3").html(data);
                    }
                });
                $("#Tbody6").html("");
                $.ajax({
                    url: "ajaxbb.ashx",
                    data: {
                        Menu: "Getygtj",
                        AdminHotelid: $("#AdminHotelid").val(),
                        Hotelid: $("#hotelid").val(),
                        StartDate: $("#yuefen").val(),
                        EndDate: $("#yuefen").val()


                    },
                    type: "POST",
                    success: function (data) {
                        if (data == null || data == "") {
                            return;
                        }
                        
                        $("#Tbody6").html(data);
                    }
                });
        }

    </script>
</body>

</html>
