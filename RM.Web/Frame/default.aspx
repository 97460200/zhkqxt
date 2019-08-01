<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="RM.Web.Frame._default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/Themes/Images/Login/logo.png" rel="icon" />
    <link href="/Themes/Styles/accordion.css" rel="stylesheet" />
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
    <div class="default">
        <%--左边信息--%>
        <div class="Census">
            <div class="UserDetail">
                <p>
                    Welcome
                </p>
                <p class="big">
                    <b>陈浪[管理员]</b>
                </p>
                <p>
                    <span class="address">深圳湖北宾馆（东门店）-服务员</span><a>[账户管理]</a><a>[修改密码]</a>
                </p>
            </div>
            <div class="CenData HotelTD">
                <div class="DataHead">
                    <b>酒店当日数据</b> <i class="prevDay"></i>
                    <asp:TextBox ID="TextBox1" runat="server" placeholder="2020-03-09"></asp:TextBox>
                    <i class="nextDay"></i>
                </div>
                <ul class="CenList">
                    <li><a><span class="items"><i></i>营业额 </span><span><b>121</b>元</span></a></li>
                    <li><a><span class="items"><i></i>订单数 </span><span><b>121</b>单 </span></a></li>
                    <li><a><span class="items"><i></i>已订房数 </span><span><b>121</b>间 </span></a></li>
                    <li><a><span class="items"><i></i>可订房数 </span><span><b>121</b>间 </span></a></li>
                    <li><a><span class="items"><i></i>入住率 </span><span><b>121</b>% </span></a></li>
                    <li><a><span class="items"><i></i>登录数 </span><span><b>121</b>人 </span></a></li>
                    <li><a><span class="items"><i></i>注册数 </span><span><b>121</b>人 </span></a></li>
                    <li><a><span class="items"><i></i>访问数 </span><span><b>121</b>人 </span></a></li>
                    <li><a><span class="items"><i></i>领券数 </span><span><b>121</b>张 </span></a></li>
                    <li><a><span class="items"><i></i>用券数 </span><span><b>121</b>张 </span></a></li>
                </ul>
            </div>
            <div class="CenData SofeTD">
                <div class="DataHead">
                    <b>系统统计数据</b>
                </div>
                <ul class="CenList">
                    <li><a><span class="items"><i></i>酒店 </span><span><b>121</b>元</span></a> </li>
                    <li><a><span class="items"><i></i>客房 </span><span><b>121</b>单 </span></a></li>
                    <li><a><span class="items"><i></i>订单 </span><span><b>121</b>间 </span></a></li>
                    <li><a><span class="items"><i></i>会员 </span><span><b>121</b>间 </span></a></li>
                    <li><a><span class="items"><i></i>评价 </span><span><b>121</b>% </span></a></li>
                    <li><a><span class="items"><i></i>广告 </span><span><b>121</b>人 </span></a></li>
                    <li><a><span class="items"><i></i>单页 </span><span><b>121</b>人 </span></a></li>
                    <li><a><span class="items"><i></i>信息 </span><span><b>121</b>人 </span></a></li>
                    <li><a><span class="items"><i></i>用户 </span><span><b>121</b>张 </span></a></li>
                    <li><a><span class="items"><i></i>短信 </span><span><b>121</b>张 </span></a></li>
                </ul>
            </div>
        </div>
        <%--右边数据表--%>
        <div class="StatiTable">
            <div class="TableList">
                <h2>
                    通知通告<i class="AddKnow"></i>
                </h2>
                <ul>
                    <li><a href="#">湖北宾馆(湖北店)将于12月19日正式营业...</a> <span>06-05</span></li>
                    <li><a href="#">四月优惠活动</a> <span>05-02</span></li>
                    <li><a href="#">四月优惠活动</a> <span>05-02</span></li>
                </ul>
            </div>
            <div class="Chart">
                <h2>
                    营业额
                </h2>
                <div class="ChartConn" id="business">
                </div>
            </div>
            <div class="Chart">
                <h2>
                    入住率
                </h2>
                <div class="ChartConn">
                </div>
            </div>
            <div class="Chart">
                <h2>
                    活跃率
                </h2>
                <div class="ChartConn">
                </div>
            </div>
        </div>
    </div>
    </form>
    <script src="../Themes/js/jquery-1.11.0.min.js" type="text/javascript"></script>
    <script src="/Themes/js/echarts.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        (function () {
            var business = echarts.init(document.getElementById('business'));

            var data = [10];    //后台 获取每个间断的数组值
            option = {
                tooltip: {
                    trigger: 'item',
                    formatter: '{a} : {c}'
                },
                grid: {
                    left: "14px",
                    right: "20px",
                    bottom: "12px",
                    top: "24px",
                    containLabel: true
                },
                xAxis: {
                    type: 'category',
                    axisLine: {
                        lineStyle: {
                            color: '#aaa'   //x轴的字、线颜色
                        }
                    },
                    splitLine: { show: false },
                    boundaryGap: false,     //x轴数字向左靠
                    data: ['0', '3', '6', '9', '12', '15', '18', '21', '24']
                },
                yAxis: {
                    type: 'value',
                    axisLine: {
                        lineStyle: {
                            color: '#aaa'   //y轴的字、线颜色
                        }
                    },
                    data: ['50', '100', '200', '300', '400']
                },
                series: {
                    name: '营业额',
                    type: 'line',
                    smooth: true,   //线条变得有平滑度
                    symbolSize: 8,   //线条 当前点的大小
                    itemStyle: {
                        normal: {
                            color: '#5BBDFF'   //更换线条颜色
                        }
                    },
                    data: data
                }
            };


            // 使用刚指定的配置项和数据显示图表。
            business.setOption(option);

            setInterval(function activing() {
                var busVal = Math.floor(Math.random() * 400);
                data.push(busVal);            //下一个时间间断 有多少个人
                option.series.data = data;



                business.setOption(option);



                if (data.length >= 9) data = [busVal];    //到了24小时，切换明天
            }, 3000);


        })();


    </script>
</body>
</html>
