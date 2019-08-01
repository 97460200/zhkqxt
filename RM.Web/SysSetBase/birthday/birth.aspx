<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="birth.aspx.cs" Inherits="RM.Web.SysSetBase.birthday.birth" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>生日设置</title>
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
    <div class="brhy">
        <b class="tx">设置会员生日提醒优惠方式参数</b>
        <div class="setion">
            <div class="byfig clearfix">
                <span>生日提醒</span>
                <label class="this">
                    开启</label>
                <label>
                    关闭</label>
            </div>
            <ul class="poCfgList odd">
                <li><b>提醒方式</b><span class="active">手机短信</span><span>微信通知</span><span>进入微网弹出</span></li>
                <li><b>提醒时间</b><span class="active">一个月前</span><span>生日当月</span><span>一周前</span><span>生日当月</span></li>
                <li><b style="padding-top: 5px;">提醒内容</b><textarea cols="30" rows="10"></textarea></li>
            </ul>
        </div>
        <div class="setion">
            <div class="byfig clearfix">
                <span>优惠方式</span>
                <label class="this">
                    开启</label>
                <label>
                    关闭</label>
            </div>
            <ul class="poCfgList even">
                <li><b class="checked">优惠券</b><span class="active">手机短信</span><span>微信通知</span><span>进入微网弹出</span></li>
                <li><b>生日折扣</b><div class="agio">
                    <div>
                        <em>会员等级</em>
                        <label>
                            生日折扣</label>
                    </div>
                    <div>
                        <em>微会员</em>
                        <label>
                            <input type="text" /></label>
                    </div>
                    <div>
                        <em>会员</em>
                        <label>
                            <input type="text" /></label>
                    </div>
                    <div>
                        <em>至尊会员</em>
                        <label>
                            <input type="text" /></label>
                    </div>
                    <div>
                        <em>至尊会员1</em>
                        <label>
                            <input type="text" /></label>
                    </div>
                    <div>
                        <em>至尊会员2</em>
                        <label>
                            <input type="text" /></label>
                    </div>
                </div>
                </li>
                <li><b>赠送礼品</b><div class="photo clearfix">
                    <div>
                        <a>
                            <img src="/App_Themes/default/images/zp_banner.png" /><i class="icon-close"></i></a>
                        <input type="text" placeholder="输入名称" />
                    </div>
                    <a class="icon-plus"></a>
                </div>
                </li>
            </ul>
        </div>
        <div class="poBtn">
            <a class="button buttonActive">保存</a>
        </div>
    </div>
    </form>
    <script src="/SysSetBase/css/jquery.js" type="text/javascript"></script>
    <script type="text/javascript">
        //单选操作
        var checked = {
            isOpen: function () {
                $(this).addClass('this').siblings().removeClass('this');
                if ($.trim($(this).html()) == '关闭') {
                    $(this).parent().siblings('.poCfgList').find('span').removeClass('active');
                    $(this).parent().siblings('.poCfgList').find('b').removeClass('checked');
                    $(this).parent().siblings('.poCfgList').find('b, span').off();
                    $(this).parent().siblings('.poCfgList').find('input, textarea').attr('disabled', true);
                } else {
                    if ($.trim($(this).siblings('span').html()) == '生日提醒') {
                        $('.odd span').on('click', checked.birth);
                        $('.odd textarea').attr('disabled', false);
                    }
                    else {
                        $('.even b, .even span').on('click', checked.discount);
                        $('.even input').attr('disabled', false);
                    }
                }
            },
            birth: function () {
                $(this).hasClass('active') ? $(this).removeClass('active') : $(this).addClass('active');
            },
            discount: function () {
                this.tagName.toLowerCase() == 'b' ?
                $(this).hasClass('checked') ? $(this).removeClass('checked') : $(this).addClass('checked') :
                $(this).addClass('active').siblings().removeClass('active');
            }
        }
        $('.byfig label').on('click', checked.isOpen);
        $('.odd span').on('click', checked.birth);
        $('.even b, .even span').on('click', checked.discount);





        
    </script>
</body>
</html>
