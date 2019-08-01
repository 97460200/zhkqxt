<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="guestroomorder.aspx.cs" Inherits="RM.Web.SysSetBase.guestroomorder.guestroomorder" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>客房订单</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="ptb8">
            <div class="w120">   
                <select>
                    <option value="value">选项01</option>
                    <option value="value">选项01</option>
                    <option value="value">选项01</option>
                </select>
            </div>
            <div class="w120">   
                <select>
                    <option value="value">选项01</option>
                    <option value="value">选项01</option>
                    <option value="value">选项01</option>
                </select>
            </div>
            <div class="sharesearch">
                <input type="text" name="name" value="" placeholder="请输入关键字...">
                <i class="icon-search"></i>
            </div>
            <div class="wdyhd" style="padding-right: 12px;">
                <div class="r">          
                    <span onclick="">导出</span>     
                    <span onclick="">删除</span>
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
                                <th width="40">
                                    <i class="icon-radio6"></i>
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
                                    <i class="icon-radio6"></i>
                                </td>
                                <td>
                                    wx20171122030714 
                                </td>
                                <td>
                                    <span class="ddly ygxs">员工销售</span>
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
                                    何佳丽
                                </td>
                                <td>
                                    13794875444
                                </td>
                                <td>
                                    2017-11-22  08:00 
                                </td>
                                <td>
                                    ￥123
                                </td>
                                <td>
                                    -￥2
                                </td>
                                <td>
                                    ￥123
                                </td>
                                <td>
                                    <span class="zffs wxzf">微信支付</span>
                                </td>
                                <td>
                                    <span class="ddzt wqr">未确认</span>
                                </td>
                                <td>
                                    <i class="icon-edit4"></i>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    2
                                </td>
                                <td>
                                    <i class="icon-radio6"></i>
                                </td>
                                <td>
                                    wx20171122030714 
                                </td>
                                <td>
                                    <span class="ddly ygxs">员工销售</span>
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
                                    何佳丽
                                </td>
                                <td>
                                    13794875444
                                </td>
                                <td>
                                    2017-11-22  08:00 
                                </td>
                                <td>
                                    ￥123
                                </td>
                                <td>
                                    -￥2
                                </td>
                                <td>
                                    ￥123
                                </td>
                                <td>
                                    <span class="zffs hykzf">会员卡支付</span>
                                </td>
                                <td>
                                    <span class="ddzt yqr">已确认</span>
                                </td>
                                <td>
                                    <i class="icon-edit4"></i>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    3
                                </td>
                                <td>
                                    <i class="icon-radio6"></i>
                                </td>
                                <td>
                                    wx20171122030714 
                                </td>
                                <td>
                                    <span class="ddly gw">官微</span>
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
                                    何佳丽
                                </td>
                                <td>
                                    13794875444
                                </td>
                                <td>
                                    2017-11-22  08:00 
                                </td>
                                <td>
                                    ￥123
                                </td>
                                <td>
                                    -￥2
                                </td>
                                <td>
                                    ￥123
                                </td>
                                <td>
                                    <span class="zffs jfzf">积分兑换</span>
                                </td>
                                <td>
                                    <span class="ddzt yqx">已取消</span>
                                </td>
                                <td>
                                    <i class="icon-edit4"></i>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    4
                                </td>
                                <td>
                                    <i class="icon-radio6"></i>
                                </td>
                                <td>
                                    wx20171122030714 
                                </td>
                                <td>
                                    <span class="ddly gw">官微</span>
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
                                    何佳丽
                                </td>
                                <td>
                                    13794875444
                                </td>
                                <td>
                                    2017-11-22  08:00 
                                </td>
                                <td>
                                    ￥123
                                </td>
                                <td>
                                    -￥2
                                </td>
                                <td>
                                    ￥123
                                </td>
                                <td>
                                    <span class="zffs qtzf">到店付款</span>
                                </td>
                                <td>
                                    <span class="ddzt yrz">已入住</span>
                                </td>
                                <td>
                                    <i class="icon-edit4"></i>
                                </td>
                            </tr>
                        </tbody>

                    </table>
                </div>
            </div>

    </form>
    <script src="/SysSetBase/css/jquery.js" type="text/javascript"></script>
    <script type="text/javascript">
        var selected = function (Selector) {
            var htr = function () {
                $(this).parents('tr').hasClass('active') ? $(Selector).find('tr').removeClass('active') : $(Selector).find('tr').addClass('active');
            };
            var btr = function () {
                $(this).parents('tr').hasClass('active') ? $(this).parents('tr').removeClass('active') : $(this).parents('tr').addClass('active');
                isCheckAll() ? $(Selector).find('thead tr').removeClass('active') : $(Selector).find('thead tr').addClass('active');
            };
            var isCheckAll = function () {
                var otr = $(Selector).find('tbody tr');
                for (var i = 0; i < otr.length; i++) {
                    if (!otr.eq(i).hasClass('active')) return true;
                }
                return false;
            };
            $(Selector).on('click', 'thead .icon-radio6', htr);
            $(Selector).on('click', 'tbody .icon-radio6', btr);
        };
        selected('.rgetable');

    </script>
</body>
</html>
