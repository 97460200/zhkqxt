<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Recon.aspx.cs" Inherits="RM.Web.SysSetBase.memInfo.Recon" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>会员卡管理</title>
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
                                    会员卡号
                                </th>
                                <th>
                                    推荐人
                                </th>
                                <th>
                                    真是姓名
                                </th>
                                <th>
                                    手机号码
                                </th>
                                <th>
                                    充值日期
                                </th>
                                <th>
                                    充值金额
                                </th>
                                <th>
                                    现金账户进项
                                </th>
                                <th>
                                    支付手续费
                                </th>
                                <th>
                                    系统维护费
                                </th>
                                <th>
                                    会员销售费
                                </th>
                                <th>
                                    实际结算
                                </th>
                            </tr>
                        </thead>
                        <tbody id="tb">
                            <tr>
                                <td>
                                    1
                                </td>
                                <td>
                                    1656
                                </td>
                                <td>
                                    琉璃
                                </td>
                                <td>
                                    张好
                                </td>
                                <td>
                                    1596574537
                                </td>
                                <td>
                                    2017-11-13 15:12
                                </td>
                                <td>
                                    2000
                                </td>
                                <td>
                                    2000
                                </td>
                                <td>
                                    20
                                </td>
                                <td>
                                    40
                                </td>
                                <td>
                                    10
                                </td>
                                <td>
                                    1930
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
