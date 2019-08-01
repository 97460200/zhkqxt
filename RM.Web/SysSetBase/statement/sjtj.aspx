<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sjtj.aspx.cs" Inherits="RM.Web.SysSetBase.statement.sjtj" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>数据统计</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/highcharts.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/highcharts-more.js" type="text/javascript"></script>
    <%--<script src="https://img.hcharts.cn/highcharts/highcharts.js"></script>--%>
    <script src="https://img.hcharts.cn/highcharts/modules/exporting.js"></script>
        <script src="https://img.hcharts.cn/highcharts/modules/oldie.js"></script>
        <script src="https://img.hcharts.cn/highcharts-plugins/highcharts-zh_CN.js"></script>
    <script src="/WDatePicker/WdatePicker.js" type="text/javascript"></script>
    <style type="text/css">
        html, body, form
        {
            height: 100%;
            overflow: auto;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <input id="AdminHotelid" type="hidden" runat="server" />
    <div class="tools_bar btnbartitle btnbartitlenr" style="display: block;">
        <div>
            当前位置：<span><a title="系统首页" href="/zdyindex.aspx">系统首页</a></span><span> &gt; </span>
            <span>报表管理</span> &gt; </span><span>数据统计</span></span>
        </div>
    </div>
    <div class="zdyindex sjtj">
        <div class="list">
            <div class="thirdt clearfix">
                <div class="bt">
                    总销售额
                </div>
                <div class="sharedate">
                    <input name="zStartTime" type="text" id="zStartTime" onfocus="WdatePicker({onpicked: function(dp){BindchartSaleMoney($('#zStartTime').val(), $('#zEndTime').val());}})"
                         autocomplete="off">
                    <input name="zEndTime" type="text" id="zEndTime" onfocus="WdatePicker({onpicked: function(dp){BindchartSaleMoney($('#zStartTime').val(), $('#zEndTime').val());}})" 
                         autocomplete="off">
                </div>
                <ul id="yjpgul">
                    <li onclick="GetDate(1,this)">过去90天</li>
                    <li onclick="GetDate(2,this)">过去30天</li>
                    <li onclick="GetDate(3,this)">上月</li>
                    <li onclick="GetDate(4,this)">本月</li>
                </ul>
                
            </div>
            <div class="tb d1b" id="chartSaleMoney">
            </div>
        </div>
        <div class="list" style="float: left; width: 50%;">
            <div class="thirdt clearfix">
                <div class="bt">
                    间夜数、回头间夜数和会员数
                </div>
                <div class="sharedate">
                    <input name="hStartTime" type="text" id="hStartTime" onfocus="WdatePicker({dateFmt: 'yyyy-MM' ,onpicked: function(dp){BindchartSaleNight($('#hStartTime').val(), $('#hEndTime').val());}})"  value="2018-11-01"
                        autocomplete="off">
                    <input name="hEndTime" type="text" id="hEndTime" onfocus="WdatePicker({dateFmt: 'yyyy-MM' ,onpicked: function(dp){BindchartSaleNight($('#hStartTime').val(), $('#hEndTime').val());}})" 
                        value="2018-11-20" autocomplete="off">
                </div>
                <input name="hdyjpg" type="hidden" id="Hidden2" value="6">
            </div>
            <div class="tb tb01" id="chartSaleNight">
            </div>
        </div>
        <div class="list" style="float: left; width: 50%;">
            <div class="thirdt clearfix">
                <div class="bt">
                    预订次数
                </div>
                <div class="sharedate">
                    <input name="tStartTime" type="text" id="tStartTime" onfocus="WdatePicker({onpicked: function(dp){chartOTTCount($('#tStartTime').val(), $('#tEndTime').val());}})"  value="2018-11-01"
                        autocomplete="off">
                    <input name="tEndTime" type="text" id="tEndTime" onfocus="WdatePicker({onpicked: function(dp){BindchartOTTCount($('#tStartTime').val(), $('#tEndTime').val());}})" 
                        value="2018-11-20" autocomplete="off">
                </div>
                <ul id="Ul1">
                    <li onclick="GetDates(1,this)">过去90天</li>
                    <li onclick="GetDates(2,this)">过去30天</li>
                    <li onclick="GetDates(3,this)">上月</li>
                    <li onclick="GetDates(4,this)">本月</li>
                </ul>
                <input name="hdyjpg" type="hidden" id="Hidden1" value="6">
            </div>
            <div class="tb tb01" id="chartOTTCount">
            </div>
        </div>
        <div class="list">
            <div class="thirdt clearfix">
                <div class="bt">
                    下单时段分布
                </div>
                <div class="sharedate">
                    <input name="xStartTime" type="text" id="xStartTime" onfocus="WdatePicker({dateFmt: 'yyyy-MM' ,onpicked: function(dp){BindchartHourCount($('#xStartTime').val(), $('#xEndTime').val());}})"  value="2018-11-01"
                        autocomplete="off">
                    <input name="xEndTime" type="text" id="xEndTime" onfocus="WdatePicker({dateFmt: 'yyyy-MM' ,onpicked: function(dp){BindchartHourCount($('#xStartTime').val(), $('#xEndTime').val());}})" 
                        value="2018-11-20" autocomplete="off">
                </div>
                <input name="hdyjpg" type="hidden" id="Hidden3" value="6">
            </div>
            <div class="tb" id="chartHourCount">
            </div>
        </div>
        <div class="list">
            <div class="thirdt clearfix">
                <div class="bt">
                    订单入住日分布
                </div>
                <div class="sharedate">
                    <input name="dStartTime" type="text" id="dStartTime" onfocus="WdatePicker({onpicked: function(dp){BindchartWeekCount($('#dStartTime').val(), $('#dEndTime').val());}})"  value="2018-11-01"
                        autocomplete="off">
                    <input name="dEndTime" type="text" id="dEndTime" onfocus="WdatePicker({onpicked: function(dp){BindchartWeekCount($('#dStartTime').val(), $('#dEndTime').val());}})" 
                        value="2018-11-20" autocomplete="off">
                </div>
                <input name="hdyjpg" type="hidden" id="Hidden4" value="6">
            </div>
            <div class="tb" id="chartWeekCount">
            </div>
        </div>
        <div class="list" style="float: left; width: 50%;">
            <div class="thirdt clearfix">
                <div class="bt">
                    会员性别比例
                </div>
                <div class="sharedate">
                    <input name="xbStartTime" type="text" id="xbStartTime" onfocus="WdatePicker({onpicked: function(dp){Bindchartperson_sex($('#xbStartTime').val(), $('#xbEndTime').val());}})"  value="2018-11-01"
                        autocomplete="off">
                    <input name="xbEndTime" type="text" id="xbEndTime" onfocus="WdatePicker({onpicked: function(dp){Bindchartperson_sex($('#xbStartTime').val(), $('#xbEndTime').val());}})" 
                        value="2018-11-20" autocomplete="off">
                </div>
                <input name="hdyjpg" type="hidden" id="Hidden5" value="6">
            </div>
            <div class="tb tb01" id="chartperson_sex">
            </div>
        </div>
        <div class="list" style="float: left; width: 50%;">
            <div class="thirdt clearfix">
                <div class="bt">
                    支付方式比例
                </div>
                <div class="sharedate">
                    <input name="zfStartTime" type="text" id="zfStartTime" onfocus="WdatePicker({onpicked: function(dp){Bindchartpay_name($('#zfStartTime').val(), $('#zfEndTime').val());}})"  value="2018-11-01"
                        autocomplete="off">
                    <input name="zfEndTime" type="text" id="zfEndTime" onfocus="WdatePicker({onpicked: function(dp){Bindchartpay_name($('#zfStartTime').val(), $('#zfEndTime').val());}})" 
                        value="2018-11-20" autocomplete="off">
                </div>
                <input name="hdyjpg" type="hidden" id="Hidden6" value="6">
            </div>
            <div class="tb tb01" id="chartpay_name">
            </div>
        </div>
    </div>
    <script>


        var chartSaleMoney = {
            chart: {
            type: 'spline',
                zoomType: 'x'
            },
            title: {
                text: ''
            },
            subtitle: {
                text: document.ontouchstart === undefined ?
            			'鼠标拖动可以进行缩放' : '手势操作进行缩放'
                //text:''
            },
            
            xAxis: {
                categories: null
            },
            yAxis: {
                title: {
                    text: ''
                }
            },
            credits: {  
                enabled: false     //不显示LOGO 
            },
            tooltip: {
                crosshairs: true,
                shared: true
            },
            plotOptions: {
                spline: {
                    marker: {
                        radius: 4,
                        lineColor: '#666666',
                        lineWidth: 1,
                        enabled: false
                    }
                }
            },
            series: [
                        {
                            name: '总销售额',
                            data: null
                        }
                        

                        ]
        }

        function BindchartSaleMoney(StartDate, EndDate) {
            $.post("ajaxbb.ashx?Menu=R_T_SaleMoney&StartDate=" + StartDate + "&EndDate=" + EndDate + "&AdminHotelid=" + $("#AdminHotelid").val(), function (data) {
                if (data == "") {
                    //alert("未找到数据");
                    return;
                }

                var json = eval("(" + data + ")");

                var xAxisdata = new Array();
                var val = new Array();

                for (var i = 0; i < json.length; i++) {
                    xAxisdata[i] = json[i]["every_time"];
                    val[i] = json[i]["val"] * 1;


                }

                chartSaleMoney.xAxis.categories = xAxisdata;
                chartSaleMoney.series[0].data = val;



                $('#chartSaleMoney').highcharts(chartSaleMoney);

            });

        }



        var chartSaleNight = {
            chart: {
                type: 'column'
            },
            title: {
                text: ''
            },
            subtitle: {
                text: ''
            },
            xAxis: {
                categories: null,
                crosshair: true
            },
            yAxis: {
                min: 0,
                title: {
                    text: ''
                }
            },
            credits: {
                enabled: false     //不显示LOGO 
            },
            tooltip: {
                // head + 每个 point + footer 拼接成完整的 table
                headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
		'<td style="padding:0"><b>{point.y:.1f}</b></td></tr>',
                footerFormat: '</table>',
                shared: true,
                useHTML: true
            },
            plotOptions: {
                column: {
                    borderWidth: 0
                }
            },
            series: [{
                name: '间夜',
                data: null
            }, {
                name: '回头间夜',
                data: null
            }, {
                name: '会员数',
                data: null
            }]
        };

        function BindchartSaleNight(StartDate, EndDate) {
            $.post("ajaxbb.ashx?Menu=R_T_SaleNight&StartDate=" + StartDate + "&EndDate=" + EndDate + "&AdminHotelid=" + $("#AdminHotelid").val(), function (data) {
                if (data == "") {
                    //alert("未找到数据");
                    return;
                }
                //alert(data);
                var json = eval("(" + data + ")");

                var xAxisdata = new Array();
                var SaleNight = new Array();
                var ComeBackNight = new Array();
                var Members = new Array();

                for (var i = 0; i < json.length; i++) {
                    xAxisdata[i] = json[i]["every_time"];
                    SaleNight[i] = json[i]["SaleNight"] * 1;
                    ComeBackNight[i] = json[i]["ComeBackNight"] * 1;
                    Members[i] = json[i]["Members"] * 1;


                }

                chartSaleNight.xAxis.categories = xAxisdata;
                chartSaleNight.series[0].data = SaleNight;
                chartSaleNight.series[1].data = ComeBackNight;
                chartSaleNight.series[2].data = Members;



                $('#chartSaleNight').highcharts(chartSaleNight);

            });

        }

        var chartOTTCount = {
            chart: {
                type: 'bar'
            },
            title: {
                text: ''
            },
            subtitle: {
                text: ''
            },
            xAxis: {
                categories: ['三次及以上', '两次', '一次'],
                title: {
                    text: null
                }
            },
            yAxis: {
                min: 0,
                title: {
                    text: '占比数 (%)',
                    align: 'high'
                },
                labels: {
                    overflow: 'justify'
                }
            },
            credits: {
                enabled: false     //不显示LOGO 
            },
            tooltip: {
                valueSuffix: '%'
            },
            plotOptions: {
                bar: {
                    dataLabels: {
                        enabled: true,
                        allowOverlap: true // 允许数据标签重叠
                    }
                }
            },
            legend: {
                layout: 'vertical',
                align: 'right',
                verticalAlign: 'top',
                x: -40,
                y: 100,
                floating: true,
                borderWidth: 1,
                backgroundColor: ((Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF'),
                shadow: true
            },
            series: [{
                name: '占比',
                data: null
            }]
        };

        
        function BindchartOTTCount(StartDate, EndDate) {
            $.post("ajaxbb.ashx?Menu=R_T_OTTCount&StartDate=" + StartDate + "&EndDate=" + EndDate + "&AdminHotelid=" + $("#AdminHotelid").val(), function (data) {
                if (data == "") {
                    //alert("未找到数据");
                    return;
                }
                //alert(data);
                var json = eval("(" + data + ")");

                var xAxisdata = new Array();
                var val = new Array();


                for (var i = 0; i < json.length; i++) {
                    val[0] = json[i]["three"];
                    val[1] = json[i]["two"];
                    val[2] = json[i]["one"];
                }

                //chartOTTCount.xAxis.categories = xAxisdata;
                chartOTTCount.series[0].data = val;


                //alert(chartOTTCount.series[0].data);

                $('#chartOTTCount').highcharts(chartOTTCount);

            });

        }

        var chartHourCount = {
            chart: {
            type: 'spline'
                //zoomType: 'x'
            },
            title: {
                text: ''
            },
            subtitle: {
                text: ''
            },
            
            xAxis: {
                categories: null
            },
            yAxis: {
                title: {
                    text: ''
                }
            },
            credits: {
                enabled: false     //不显示LOGO 
            },
            tooltip: {
                crosshairs: true,
                shared: true
            },
            plotOptions: {
                spline: {
                    marker: {
                        radius: 4,
                        lineColor: '#666666',
                        lineWidth: 1,
                        enabled: false
                    }
                }
            },
            series: [
                        {
                            name: '间夜',
                            data: null
                        }
                        

                        ]
        }
        function BindchartHourCount(StartDate, EndDate) {
            $.post("ajaxbb.ashx?Menu=R_T_HourCount&StartDate=" + StartDate + "&EndDate=" + EndDate + "&AdminHotelid=" + $("#AdminHotelid").val(), function (data) {

                if (data == "") {
                    //alert("未找到数据");
                    return;
                }
                
                var json = eval("(" + data + ")");

                var xAxisdata = new Array();
                var count = new Array();

                for (var i = 0; i < json.length; i++) {
                    xAxisdata[i] = json[i]["every_time"];
                    count[i] = json[i]["count"] * 1;


                }

                chartHourCount.xAxis.categories = xAxisdata;
                chartHourCount.series[0].data = count;



                $('#chartHourCount').highcharts(chartHourCount);

            });

        }



        var chartWeekCount = {
            chart: {
            type: 'spline'
                //zoomType: 'x'
            },
            title: {
                text: ''
            },
            subtitle: {
                text: ''
            },
            
            xAxis: {
                categories: null
            },
            yAxis: {
                title: {
                    text: ''
                }
            },
            credits: {
                enabled: false     //不显示LOGO 
            },
            tooltip: {
                crosshairs: true,
                shared: true
            },
            plotOptions: {
                spline: {
                    marker: {
                        radius: 4,
                        lineColor: '#666666',
                        lineWidth: 1,
                        enabled: false
                    }
                }
            },
            series: [
                        {
                            name: '间夜',
                            data: null
                        }
                        

                        ]
        }
        function BindchartWeekCount(StartDate, EndDate) {
            $.post("ajaxbb.ashx?Menu=R_T_WeekCount&StartDate=" + StartDate + "&EndDate=" + EndDate + "&AdminHotelid=" + $("#AdminHotelid").val(), function (data) {

                if (data == "") {
                    //alert("未找到数据");
                    return;
                }

                var json = eval("(" + data + ")");
                
                var xAxisdata = new Array();
                var count = new Array();

                for (var i = 0; i < json.length; i++) {
                    xAxisdata[i] = json[i]["every_time"];
                    count[i] = json[i]["count"] * 1;


                }

                chartWeekCount.xAxis.categories = xAxisdata;
                chartWeekCount.series[0].data = count;



                $('#chartWeekCount').highcharts(chartWeekCount);

            });

        }

        
        var chartperson_sex = {
            chart: {
                spacing: [20, 10, 20, 10]
            },
            title: {
                floating: true,
                text: ''
            },
            credits: {
                enabled: false     //不显示LOGO 
            },
            tooltip: {
                pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: true,
                        format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                        style: {
                            color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                        }
                    },
                    point: {
                        events: {
                            mouseOver: function (e) {  // 鼠标滑过时动态更新标题
                                // 标题更新函数，API 地址：https://api.hcharts.cn/highcharts#Chart.setTitle
                                chartperson_sex.setTitle({
                                    text: e.target.name + '\t' + e.target.y + ' %'
                                });
                            }
                        }
                    }
                }
            },
            series: [{
                type: 'pie',
                innerSize: '80%',
                name: '市场份额',
                data: null
            }]
        }


        function Bindchartperson_sex(StartDate, EndDate) {
            $.post("ajaxbb.ashx?Menu=person_sex&StartDate=" + StartDate + "&EndDate=" + EndDate + "&AdminHotelid=" + $("#AdminHotelid").val(), function (data) {
                
                if (data == "") {
                    //alert("未找到数据");
                    return;
                }

                var json = eval("(" + data + ")");


                var val = new Array();

                for (var i = 0; i < json.length; i++) {

                    val[i] = [json[i]["name"], json[i]["y"] * 1.0];


                }


                chartperson_sex.series[0].data = val;



                $('#chartperson_sex').highcharts(chartperson_sex
//                ,
//                function(c) { 
//                 //图表初始化完毕后的会掉函数
//	             //环形图圆心
//	                var centerY = c.series[0].center[1],
//		            titleHeight = parseInt(c.title.styles.fontSize);
//	                 //动态设置标题位置
//	                c.setTitle({
//		                y:centerY + titleHeight/2
//	                });
//                }
                );
                

        
        });

        }


        var chartpay_name = {
            chart: {
                spacing: [20, 10, 20, 10]
            },
            title: {
                floating: true,
                text: ''
            },
            credits: {
                enabled: false     //不显示LOGO 
            },
            tooltip: {
                pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: true,
                        format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                        style: {
                            color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                        }
                    },
                    point: {
                        events: {
                            mouseOver: function (e) {  // 鼠标滑过时动态更新标题
                                // 标题更新函数，API 地址：https://api.hcharts.cn/highcharts#Chart.setTitle
                                chart.setTitle({
                                    text: e.target.name + '\t' + e.target.y + ' %'
                                });
                            }
                        }
                    }
                }
            },
            series: [{
                type: 'pie',
                innerSize: '80%',
                name: '市场份额',
                data: null
            }]
        }


        function Bindchartpay_name(StartDate, EndDate) {
            $.post("ajaxbb.ashx?Menu=pay_name&StartDate=" + StartDate + "&EndDate=" + EndDate + "&AdminHotelid=" + $("#AdminHotelid").val(), function (data) {
                //alert(data);
                if (data == "") {
                    //alert("未找到数据");
                    return;
                }

                var json = eval("(" + data + ")");


                var val = new Array();

                for (var i = 0; i < json.length; i++) {

                    val[i] = [json[i]["name"], json[i]["y"] * 1.0];


                }


                chartpay_name.series[0].data = val;



                $('#chartpay_name').highcharts(chartpay_name
                //                ,
                //                function(c) { 
                //                 //图表初始化完毕后的会掉函数
                //	             //环形图圆心
                //	                var centerY = c.series[0].center[1],
                //		            titleHeight = parseInt(c.title.styles.fontSize);
                //	                 //动态设置标题位置
                //	                c.setTitle({
                //		                y:centerY + titleHeight/2
                //	                });
                //                }
                );



            });

        }

    </script>
    <script>

        function getDay(day) {
            var today = new Date();

            var targetday_milliseconds = today.getTime() + 1000 * 60 * 60 * 24 * day;

            today.setTime(targetday_milliseconds);

            var tYear = today.getFullYear();
            var tMonth = today.getMonth();
            var tDate = today.getDate();
            tMonth = doHandleMonth(tMonth + 1);
            tDate = doHandleMonth(tDate);
            return tYear + "-" + tMonth + "-" + tDate;
        }
        function doHandleMonth(month) {
            var m = month;
            if (month.toString().length == 1) {
                m = "0" + month;
            }
            return m;
        }
        bind("1");
        function bind(type, obj) {


            $(obj).addClass('act').siblings().removeClass('act');
            var myDate = new Date();

            //获取当前年
            var year = myDate.getFullYear();

            //获取当前月
            var month = myDate.getMonth() + 1;

            //获取当前日
            var day = myDate.getDate();

            //var StartDate = "2018-01-01";
            var StartDate = year + "-" + month + "-01";
            var EndDate = year + "-" + month + "-" + (myDate.getDate());
            if (type == "1") {
                //StartDate = getDay(-90);
                EndDate = getDay(1);
            }
            $("#zStartTime").val(StartDate);
            $("#zEndTime").val(EndDate);
            $("#hStartTime").val(StartDate);
            $("#hEndTime").val(EndDate);
            $("#xStartTime").val(StartDate);
            $("#xEndTime").val(EndDate);
            $("#dStartTime").val(StartDate);
            $("#dEndTime").val(EndDate);
            $("#xbStartTime").val(StartDate);
            $("#xbEndTime").val(EndDate);
            $("#zfStartTime").val(StartDate);
            $("#zfEndTime").val(EndDate);
            $("#tStartTime").val(StartDate);
            $("#tEndTime").val(EndDate);
            
            BindchartSaleMoney(StartDate, EndDate);
            BindchartSaleNight(StartDate, EndDate);
            BindchartHourCount(StartDate, EndDate);
            BindchartWeekCount(StartDate, EndDate);
            Bindchartperson_sex(StartDate, EndDate);
            Bindchartpay_name(StartDate, EndDate);
            BindchartOTTCount(StartDate, EndDate);


        }



        function GetDate(type, obj) {
            //alert(0);
            $(obj).addClass('active').siblings().removeClass('active');
            var myDate = new Date();
            //获取当前年
            var year = myDate.getFullYear();
            //获取当前月
            var month = myDate.getMonth() + 1;
            //获取当前日
            var day = myDate.getDate();
            var StartDate = "";
            var EndDate = "";
            switch (type * 1) {
                case 1:
                    {
                        //90
                        StartDate = getDay(-90);
                        EndDate = getDay(0);
                        break;
                    }
                case 2:
                    {
                        //30
                        StartDate = getDay(-30);
                        EndDate = getDay(0);
                        break;
                    }
                case 3:
                    {
                        //上个月
                        StartDate = year + "-" + (month - 1) + "-01";
                        //EndDate = new Date(year, month - 1, 0).Format("yyyy-MM-dd");
                        EndDate = year + "-" + month + "-01";
                        break;
                    }
                case 4:
                    {
                        //本月
                        StartDate = year + "-" + month + "-01";
                        EndDate = new Date(year, month, 0).Format("yyyy-MM-dd");
                        break;
                    }
                default:
                    {
                        //今天
                        StartDate = getDay(0);
                        EndDate = getDay(0);
                        
                        break;
                    }
            }
            //alert(StartDate + "," + EndDate);
            $("#zStartTime").val(StartDate);
            $("#zEndTime").val(EndDate);
            BindchartSaleMoney(StartDate, EndDate);
        }

        function GetDates(type, obj) {
            //alert(0);
            $(obj).addClass('active').siblings().removeClass('active');
            var myDate = new Date();
            //获取当前年
            var year = myDate.getFullYear();
            //获取当前月
            var month = myDate.getMonth() + 1;
            //获取当前日
            var day = myDate.getDate();
            var StartDate = "";
            var EndDate = "";
            switch (type * 1) {
                case 1:
                    {
                        //90
                        StartDate = getDay(-90);
                        EndDate = getDay(0);
                        break;
                    }
                case 2:
                    {
                        //30
                        StartDate = getDay(-30);
                        EndDate = getDay(0);
                        break;
                    }
                case 3:
                    {
                        //上个月
                        StartDate = year + "-" + (month - 1) + "-01";
                        //EndDate = new Date(year, month - 1, 0).Format("yyyy-MM-dd");
                        EndDate = year + "-" + month + "-01";
                        break;
                    }
                case 4:
                    {
                        //本月
                        StartDate = year + "-" + month + "-01";
                        EndDate = new Date(year, month, 0).Format("yyyy-MM-dd");
                        break;
                    }
                default:
                    {
                        //今天
                        StartDate = getDay(0);
                        EndDate = getDay(0);

                        break;
                    }
            }
            //alert(StartDate + "," + EndDate);
            $("#tStartTime").val(StartDate);
            $("#tEndTime").val(EndDate);
            BindchartOTTCount(StartDate, EndDate);
        }


        function getDay(day) {
            var today = new Date();

            var targetday_milliseconds = today.getTime() + 1000 * 60 * 60 * 24 * day;

            today.setTime(targetday_milliseconds);

            var tYear = today.getFullYear();
            var tMonth = today.getMonth();
            var tDate = today.getDate();
            tMonth = doHandleMonth(tMonth + 1);
            tDate = doHandleMonth(tDate);
            return tYear + "-" + tMonth + "-" + tDate;
        }
        function doHandleMonth(month) {
            var m = month;
            if (month.toString().length == 1) {
                m = "0" + month;
            }
            return m;
        }


        Date.prototype.Format = function (fmt) { //author: meizz 
            var o = {
                "M+": this.getMonth() + 1, //月份 
                "d+": this.getDate(), //日 
                "H+": this.getHours(), //小时 
                "m+": this.getMinutes(), //分 
                "s+": this.getSeconds(), //秒 
                "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
                "S": this.getMilliseconds() //毫秒 
            };
            if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
            for (var k in o)
                if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
            return fmt;
        }

    </script>
    </form>
</body>
</html>
