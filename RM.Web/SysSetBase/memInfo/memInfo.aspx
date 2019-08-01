<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="memInfo.aspx.cs" Inherits="RM.Web.SysSetBase.memInfo.memInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
        <div class="gmkfNav" style=" display:none">
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
                    万妮达
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
                            <span>会员ID</span> <em>159475</em>
                        </td>
                        <td>
                            <span>手机号码</span> <em>1591235689</em>
                        </td>
                        <td>
                            <span>微信昵称</span> <em>你好，明天</em>
                        </td>
                        <td>
                            <span>性别</span> <em>女</em>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <span>会员级别</span> <em>微会员</em>
                        </td>
                        <td>
                            <span>创建时间</span> <em>2018-01-05 18:56</em>
                        </td>
                        <td>
                            <span>订单数</span> <em>5</em>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="sharetabs">
                <ul class="clearfix">
                    <li>订单记录</li>
                    <li class="act">会员卡记录</li>
                    <li>积分记录</li>
                </ul>
            </div>
            <div class="bonusrecord03">
                <div class="sharedate">
                    <input type="text" placeholder="2018-01-13 00:00" />
                    <input type="text" placeholder="2018-01-14 00:00" />
                </div>
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
                                    订单号
                                </th>
                                <th>
                                    订单来源
                                </th>
                                <th>
                                    预订房型
                                </th>
                                <th>
                                    入住时间
                                </th>
                                <th>
                                    离店时间
                                </th>
                                <th>
                                    房数
                                </th>
                                <th>
                                    联系人
                                </th>
                                <th>
                                    手机号码
                                </th>
                                <th>
                                    下单时间
                                </th>
                                <th>
                                    订单总额
                                </th>
                                <th>
                                    优惠
                                </th>
                                <th>
                                    应付
                                </th>
                                <th>
                                    支付方式
                                </th>
                                <th>
                                    订单状态
                                </th>
                            </tr>
                        </thead>
                        <tbody id="tb">
                            <tr>
                                <td>
                                    1
                                </td>
                                <td>
                                    1654
                                </td>
                                <td>
                                    员工销售
                                </td>
                                <td>
                                    豪华大客房-单早
                                </td>
                                <td>
                                    2017-11-12
                                </td>
                                <td>
                                    2017-11-13
                                </td>
                                <td>
                                    1
                                </td>
                                <td>
                                    何嘉莉
                                </td>
                                <td>
                                    1596435464
                                </td>
                                <td>
                                    2017-11-22 08:00
                                </td>
                                <td>
                                    $253
                                </td>
                                <td>
                                    -￥2
                                </td>
                                <td>
                                    ￥123
                                </td>
                                <td>
                                    支付方式
                                </td>
                                <td>
                                    未确认
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
