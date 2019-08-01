<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="bringcustomers.aspx.cs" Inherits="RM.Web.SysSetBase.sales.bringcustomers" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>带来客户</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="gtall gmkf clearfix">
        <div class="gmkfNav">
            <dl>
                <dd class="">
                    <b>美思柏丽酒店</b>
                    <ul style="display: none;">
                        <li><span>鹤山前进南路店</span> </li>
                        <li><span>佛山文华北路店</span> </li>
                        <li><span>恩平鳌峰广场店</span> </li>
                        <li><span>鹤山中心店</span> </li>
                    </ul>
                </dd>
            </dl>
        </div>
        <div class="bonusrecord">
            <div class="bonusrecord01">
                <div class="xm">
                    王梓薇
                </div>
                <div class="wdyhd" style="padding-right: 12px;">
                    <div class="r">
                        <span onclick="">修改</span> <span onclick="">返回</span>
                    </div>
                </div>
            </div>
            <div class="bonusrecord02">
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <span>手机号码</span>
                            <em>13794875444</em>
                        </td>
                        <td>
                            <span>微信昵称</span>
                            <em>你好，明天</em>
                        </td>
                        <td>
                            <span>部门</span>
                            <em>前厅部员工</em>
                        </td>
                        <td>
                            <span>类型</span>
                            <em>员工销售员</em>
                        </td>
                        <td>
                            <span>创建时间</span>
                            <em>2018-01-05 18:58</em>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>累计销售额</span>
                            <em>￥2000</em>
                        </td>
                        <td>
                            <span>累计获得奖金</span>
                            <em>￥200</em>
                        </td>
                        <td>
                            <span>累计提现金额</span>
                            <em>￥200</em>
                        </td>
                        <td>
                            <span>累计带来客户</span>
                            <em>￥200</em>
                        </td>
                    </tr>
                    
                </table>
            </div>

            <div class="sharetabs">
                <ul class="clearfix">
                    <li>
                        奖金记录
                    </li>
                    <li class="act">
                        提现记录
                    </li>
                    <li>
                        带来客户
                    </li>
                </ul>
            </div>
            <div class="bonusrecord03">
                <div class="sharesearch">
                    <input type="text" name="name" value="" placeholder="请输入关键字..." />
                    <i class="icon-search"></i>
                </div>
                <div class="wdyhd" style="padding-right: 12px;">
                    <div class="r">
                        <span onclick="">导出</span>
                    </div>
                </div>
            </div>
        
            <div class="rge">
                <div class="rgetable">
                    <table class="ul" id="tab">
                        <thead>
                            <tr>
                                <th width="40">
                                </th>
                                <th>
                                    手机号码
                                </th>
                                <th>
                                    姓名
                                </th>
                                <th>
                                    会员级别
                                </th>
                                <th>
                                    入住时间
                                </th>
                                <th>
                                    客房名称
                                </th>
                                <th>
                                    订单金额
                                </th>
                                <th>
                                    支付方式
                                </th>
                            </tr>
                        </thead>
                    
                        <tbody id="tb">
                            <tr>
                                <td>
                                    1
                                </td>
                                <td>
                                    159*****475 
                                </td>
                                <td>
                                    张磊
                                </td>
                                <td>
                                    微会员
                                </td>
                                <td>
                                    2018-01-10 
                                </td>
                                <td>
                                    豪华套房
                                </td>
                                <td>
                                    318
                                </td>
                                <td>
                                    <span class="zffs wxzf">微信支付</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    2
                                </td>
                                <td>
                                    159*****475 
                                </td>
                                <td>
                                    张磊
                                </td>
                                <td>
                                    微会员
                                </td>
                                <td>
                                    2018-01-10 
                                </td>
                                <td>
                                    豪华套房
                                </td>
                                <td>
                                    318
                                </td>
                                <td>
                                    <span class="zffs jfdh">积分兑换</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    3
                                </td>
                                <td>
                                    159*****475 
                                </td>
                                <td>
                                    张磊
                                </td>
                                <td>
                                    微会员
                                </td>
                                <td>
                                    2018-01-10 
                                </td>
                                <td>
                                    豪华套房
                                </td>
                                <td>
                                    318
                                </td>
                                <td>
                                    <span class="zffs kqdh">卡券兑换</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    4
                                </td>
                                <td>
                                    159*****475 
                                </td>
                                <td>
                                    张磊
                                </td>
                                <td>
                                    微会员
                                </td>
                                <td>
                                    2018-01-10 
                                </td>
                                <td>
                                    豪华套房
                                </td>
                                <td>
                                    318
                                </td>
                                <td>
                                    <span class="zffs qtzf">到店付款</span>
                                </td>
                            </tr>
                
                        </tbody>

                    </table>
                </div>
            </div>
        
        </div>
    </div>
    </form>
    <script src="/SysSetBase/css/jquery.js" type="text/javascript"></script>
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
