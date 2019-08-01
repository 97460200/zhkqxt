<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="kfdddz.aspx.cs" Inherits="RM.Web.SysSetBase.finance.kfdddz" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>客房订单对账</title>
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
        <div class="shareright">
            <div class="ptb8">
                <div class="shareleixing">
                    <div class="radio">
                        <small>筛选类型：</small><label class="checked">5星/豪华</label><label class="">准5星/豪华</label>
                    </div>
                </div>
                <div class="sharedate">
                    <input type="text" placeholder="2018-01-13 00:00">
                    <input type="text" placeholder="2018-01-14 00:00">
                </div>
                <div class="sharesearch">
                    <input type="text" name="name" value="" placeholder="请输入关键字...">
                    <i class="icon-search"></i>
                </div>
                <div class="wdyhd" style="padding-right: 12px;">
                    <div class="r">
                        <span onclick="">下载</span>
                    </div>
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
    <script type="text/javascript">
        $('.radio').on('click', 'label', function () {
            $(this).addClass('checked').siblings().removeClass('checked');
        });
    </script>

</body>
</html>
