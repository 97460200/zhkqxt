<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="addinform.aspx.cs" Inherits="RM.Web.SysSetBase.inform.addinform" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>通知管理</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
      <dl class="addinform">
        <dd>
            <small>接受用户</small>
            <div>
                <select>
                    <option value="value">何佳丽&nbsp;&nbsp;&nbsp;1598654256&nbsp;&nbsp;&nbsp;前厅部经理</option>
                </select>
                <span>
                    <img src="/SysSetBase/img/weixin.png" width="16" height="16" /> 酸酸酸李子
                </span>
                <p style="color:#999;">
                    仅显示已绑定“智订云商户平台”的用户
                </p>
            </div>
        </dd>
        <dd>
            <small>接受分店</small>
            <div class="checkbox checkboxlabel01">
                <label class="checked">全选</label>
                <label>鹤山前进南路店</label>
                <label>佛山文华北路店</label>
                <label>恩平鳌峰广场店</label>
                <label>会鹤山中心店</label>
                <label>佛山文华北路店</label>
                <label>恩平鳌峰广场店</label>
                <label>鹤山前进南路店</label>
                <label>鹤山前进南路店</label>
            </div>
        </dd>
        <dd>
            <small>通知类型</small>
            <div class="checkbox checkboxlabel01">
                <label class="checked">全选</label>
                <label>客房预订通知</label>
                <label>营业点预订通知</label>
                <label>门店预订通知</label>
                <label>会员充值通知</label>
                <label>员工获得奖金通知</label>
                <label>充值账户余额不足通知</label>
                <label>今日营销数据通知</label>
                <label>本月营销数据通知</label>
            </div>
        </dd>
        <dd>
            <small>有效星期</small>
            <div class="checkbox">
                <label class="checked">全选</label>
                <label>星期一</label>
                <label>星期二</label>
                <label>星期三</label>
                <label>星期四</label>
                <label>星期五</label>
                <label>星期六</label>
                <label>星期日</label>
            </div>
        </dd>
        <dd>
            <small>时段</small>
            <div class="checkbox checkboxlabel">
                <label class="checked">全选</label>
                <label>00:00-01:00</label>
                <label>01:00-02:00</label>
                <label>02:00-03:00</label>
                <label>03:00-04:00</label>
                <label>04:00-05:00</label>
                <label>05:00-06:00</label>
                <label>06:00-07:00</label>
                <label>07:00-08:00</label>
                <label>08:00-09:00</label>
                <label>09:00-10:00</label>
                <label>10:00-11:00</label>
                <label>11:00-12:00</label>
                <label>12:00-13:00</label>
                <label>13:00-14:00</label>
                <label>14:00-15:00</label>
                <label>15:00-16:00</label>
                <label>16:00-17:00</label>
                <label>17:00-18:00</label>
                <label>18:00-19:00</label>
                <label>19:00-20:00</label>
                <label>20:00-21:00</label>
                <label>21:00-22:00</label>
                <label>22:00-23:00</label>
                <label>23:00-24:00</label>
            </div>
        </dd>
    </dl>
    </form>
    <script src="/SysSetBase/css/jquery.js" type="text/javascript"></script>
    <script type="text/javascript">
        $('.checkbox').on('click', 'label', function () {
            $(this).hasClass('checked') ? $(this).removeClass('checked') : $(this).addClass('checked');
        });
    </script>
</body>
</html>
