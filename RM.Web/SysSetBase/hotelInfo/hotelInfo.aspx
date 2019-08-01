<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="hotelInfo.aspx.cs" Inherits="RM.Web.SysSetBase.hotelInfo.hotelInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>提现记录</title>
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
                <dd class="down">
                    <b>美思柏丽酒店</b>
                    <ul style="display: block;">
                        <li class="li">
                            <dl>
                                <dd>
                                    <b>美思柏丽酒店</b>
                                    <ul style="display: none;">
                                        <li><span>鹤山前进南路店</span> </li>
                                        <li><span>佛山文华北路店</span> </li>
                                        <li><span>恩平鳌峰广场店</span> </li>
                                        <li><span>鹤山中心店</span> </li>
                                    </ul>
                                </dd>
                            </dl>
                        </li>
                        <li class="li">
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
                        </li>
                    </ul>
                </dd>
            </dl>
        </div>
        <div class="bonusrecord">
            <div class="bonusrecord03">
                <div class="sharesearch">
                    <input type="text" name="name" value="" placeholder="请输入关键字..." />
                    <i class="icon-search"></i>
                </div>
                <div class="wdyhd" style="padding-right: 12px;">
                    <div class="r">
                        <span>添加</span><span>修改</span><span>删除</span>
                    </div>
                </div>
            </div>
            <div class="rge">
                <div class="rgetable">
                    <table class="ul">
                        <thead>
                            <tr>
                                <th width="40">
                                </th>
                                <th width="40">
                                </th>
                                <th>
                                    酒店名称
                                </th>
                                <th>
                                    酒店地址
                                </th>
                                <th>
                                    酒店电话
                                </th>
                                <th>
                                    客房总数（间）
                                </th>
                                <th>
                                    已入住（间）
                                </th>
                                <th>
                                    未入住（间）
                                </th>
                                <th>
                                    酒店状态
                                </th>
                                <th>
                                    客房管理
                                </th>
                                <th>
                                    操作
                                </th>
                            </tr>
                        </thead>
                        <tbody id="tb">
                            <tr>
                                <td>
                                    1
                                </td>
                                <td>
                                </td>
                                <td>
                                    湖北宾馆
                                </td>
                                <td>
                                    深圳市罗湖宝安路1236号
                                </td>
                                <td>
                                    0755-2587465
                                </td>
                                <td>
                                    691
                                </td>
                                <td>
                                    691
                                </td>
                                <td>
                                    691
                                </td>
                                <td>
                                    <span>酒店状态</span>
                                </td>
                                <td>
                                    管理（<small>20</small>）
                                </td>
                                <td>
                                    修改 删除
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
    <script src="/SysSetBase/css/ScrollBar.js" type="text/javascript"></script>
    <script type="text/javascript">
        //左边导航
        $('.gmkfNav').on('click', 'b', function () {
            $(this).siblings('ul').slideToggle(120);

            $(this).parents('.li dd').toggleClass('down');
        });
        $(".gmkfNav").panel({ iWheelStep: 80 });
    </script>
</body>
</html>
