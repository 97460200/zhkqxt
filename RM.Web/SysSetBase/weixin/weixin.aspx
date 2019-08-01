<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="weixin.aspx.cs" Inherits="RM.Web.SysSetBase.weixin.weixin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>微信设置</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/SysSetBase/css/IE.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="WX">
        <div class="WXnav">
            <a class="active">微信通知</a><a>菜单管理</a><a>关键词</a>
        </div>
        <dl class="WXnavList">
            <dd style="padding: 0 18px;">
                <%--微信通知客户--%>
                <strong><b>微信通知客户</b></strong> <b class="tx">设置客户关注公众号后，进行以下操作时是否推送微信微信通知给客户</b>
                <div class="wxfig">
                    <span class="yd">订单预订成功</span>
                    <label class="this">
                        是</label>
                    <label>
                        否</label>
                </div>
                <div class="wxfig">
                    <span class="yd">订单支付成功</span>
                    <label class="this">
                        是</label>
                    <label>
                        否</label>
                </div>
                <%--微信通知管理员--%>
                <strong><b>微信通知管理员</b></strong><b class="tx">设置客户关注公众号后，进行以下操作时是否推送微信微信通知给酒店管理人员</b>
                <div class="wxfig">
                    <span class="yd">订单预订成功</span>
                    <label class="this">
                        是</label>
                    <label>
                        否</label>
                </div>
                <ul class="noteList">
                    <li>
                        <input type="text" /><span>删除</span><span>添加</span></li>
                    <li>
                        <input type="text" /><span>删除</span><span>添加</span></li>
                    <li>
                        <input type="text" /><span>删除</span><span>添加</span></li>
                </ul>
                <div class="wxfig">
                    <span class="yd">订单支付成功</span>
                    <label class="this">
                        是</label>
                    <label>
                        否</label>
                </div>
                <ul class="noteList">
                    <li>
                        <input type="text" /><span>删除</span><span>添加</span></li>
                    <li>
                        <input type="text" /><span>删除</span><span>添加</span></li>
                    <li>
                        <input type="text" /><span>删除</span><span>添加</span></li>
                </ul>
                <%--访问关注公众号--%>
                <strong><b>访问关注公众号</b></strong><b class="tx">设置访问公众号关注成为酒店会员参数</b>
                <div class="wxfig">
                    <span class="yd">访问关注公众号</span>
                    <label class="this">
                        是</label>
                    <label>
                        否</label>
                </div>
                <div class="wxfig">
                    <span class="yd">公众号二维码图片</span>
                    <div class="download">
                        <a>下载</a> <span>
                            <img src="/App_Themes/default/images/fx_ewm2.png" alt="code" /></span>
                    </div>
                </div>
                <div class="poBtn">
                    <a class="button buttonActive">保存</a>
                </div>
            </dd>
            <dd>
            </dd>
            <dd>
                <div class="wdyhd" style="padding: 12px;">
                    <div class="l" style="border: none;">
                        <label>
                            <input type="text" placeholder="请输入关键字..."><i class="icon-search"></i></label>
                    </div>
                    <div class="r">
                        <span onclick="Rm.F_alert('#add');">添加</span> <span>修改</span> <span>删除</span>
                    </div>
                </div>
                <div class="wxtable">
                    <table class="ul" style="width: 1150px;">
                        <thead>
                            <tr>
                                <th width="40">
                                </th>
                                <th width="40">
                                    <i class="icon-radio6"></i>
                                </th>
                                <th>
                                    规则名称
                                </th>
                                <th>
                                    关键字
                                </th>
                                <th>
                                    回复内容
                                </th>
                                <th>
                                    使用量
                                </th>
                                <th width="80">
                                    操作
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>
                                    1
                                </td>
                                <td>
                                    <i class="icon-radio6"></i>
                                </td>
                                <td>
                                    阳江
                                </td>
                                <td>
                                    阳江
                                </td>
                                <td>
                                    文字
                                </td>
                                <td>
                                    18
                                </td>
                                <td>
                                    <i class="icon-edit4"></i><i class="icon-lbx"></i>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </dd>
        </dl>
    </div>
    <!--弹窗-->
    <div class="Alert" id="add">
        <h2 onclick="Rm.F_alert();">
            添加自动回复 <i class="icon-boldclose"></i>
        </h2>
        <div class="altconX">
            <div class="reply">
                <div class="repNav clearfix">
                    <span class="active">被关注回复</span><span>消息自动回复</span><span>关键字回复</span>
                </div>
                <div class="repdd">
                    <dl>
                        <%--被关注回复--%>
                        <dd>
                            <div class="reptype">
                                <b>回复类型</b> <span class="this">文字</span> <span>图片</span>
                            </div>
                            <ul class="reptxt">
                                <li class="this"><b>回复内容</b>
                                    <textarea cols="30" rows="10"></textarea>
                                </li>
                                <li><b>回复内容</b>
                                    <div class="photo clearfix">
                                        <a class="icon-plus"></a>
                                        <div>
                                            <a>
                                                <img src="/App_Themes/default/images/zp_banner.png"><i class="icon-close"></i></a>
                                        </div>
                                        <div>
                                            <a>
                                                <img src="/App_Themes/default/images/zp_banner.png"><i class="icon-close"></i></a>
                                        </div>
                                    </div>
                                </li>
                            </ul>
                        </dd>
                        <%--消息自动回复--%>
                        <dd>
                            <div class="reptype">
                                <b>回复类型</b> <span>文字</span> <span class="this">图片</span>
                            </div>
                            <ul class="reptxt">
                                <li><b>回复内容</b>
                                    <textarea cols="30" rows="10"></textarea>
                                </li>
                                <li class="this"><b>回复内容</b>
                                    <div class="photo clearfix">
                                        <a class="icon-plus"></a>
                                        <div>
                                            <a>
                                                <img src="/App_Themes/default/images/zp_banner.png"><i class="icon-close"></i></a>
                                        </div>
                                        <div>
                                            <a>
                                                <img src="/App_Themes/default/images/zp_banner.png"><i class="icon-close"></i></a>
                                        </div>
                                    </div>
                                </li>
                            </ul>
                        </dd>
                        <%--关键字回复--%>
                        <dd>
                            <div class="keyRep">
                                <div class="keytext">
                                    <b>规格名称</b><input type="text" />
                                </div>
                                <div class="keytext">
                                    <b>关键字</b><input type="text" />
                                </div>
                                <span class="zs">多个关键字以中文逗号分隔“，”</span>
                            </div>
                            <div class="reptype">
                                <b>回复类型</b> <span>文字</span> <span class="this">图片</span>
                            </div>
                            <ul class="reptxt">
                                <li><b>回复内容</b>
                                    <textarea cols="30" rows="10"></textarea>
                                </li>
                                <li class="this"><b>回复内容</b>
                                    <div class="photo clearfix">
                                        <a class="icon-plus"></a>
                                        <div>
                                            <a>
                                                <img src="/App_Themes/default/images/zp_banner.png"><i class="icon-close"></i></a>
                                        </div>
                                        <div>
                                            <a>
                                                <img src="/App_Themes/default/images/zp_banner.png"><i class="icon-close"></i></a>
                                        </div>
                                    </div>
                                </li>
                            </ul>
                        </dd>
                    </dl>
                </div>
            </div>
        </div>
        <div class="altBtn">
            <a class="button buttonActive">提交</a>
        </div>
    </div>
    </form>
    <script src="/SysSetBase/css/jquery.js" type="text/javascript"></script>
    <script type="text/javascript">
        //切换显示界面
        $('.WXnav a').on('click', function () {
            $(this).addClass('active').siblings().removeClass('active');
            $('.WXnavList dd').eq($(this).index()).show().siblings().hide();
        });


        //是否单选按钮
        $('.wxfig label').on('click', function () {
            $(this).addClass('this').siblings().removeClass('this');
        });


        //表格 单选按钮
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
        selected('.wxtable');





        //弹窗
        var Rm = {
            curr: '',
            F_alert: function (Selector) {
                if (typeof Selector === 'undefined' || Selector == 'sure') {
                    $('.Alert').fadeOut(200);
                    if (Selector == 'sure') this.F_sure();
                } else {
                    //$(Selector).addClass('AlertShow AlertInit');
                    Rm.curr = Selector;
                    alertAnim(Selector);
                }
            },
            F_sure: function () {
                this.F_tip('当前选择器是' + this.curr);
            },
            F_tip: function (str) {
                $('.tip').html(str).fadeIn(120);
                setTimeout(function () { $('.tip').fadeOut(120); }, 2000);
            },

            F_nav: function () {
                $(this).addClass('active').siblings().removeClass('active');
                var pos = -$('.repdd').outerWidth() * $(this).index();
                $('.repdd dl').css('left', pos);
            },
            F_repradio: function () {
                $(this).addClass('this').siblings().removeClass('this');
                var index = $(this).index() - 1;
                $(this).parent().siblings('.reptxt').find('li').eq(index).fadeIn(400).siblings().hide();
            }
        }
        $('.repNav span').on('click', Rm.F_nav);
        $('.reptype span').on('click', Rm.F_repradio);
    </script>
</body>
</html>
