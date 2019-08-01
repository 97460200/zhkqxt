<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="businadminEdit.aspx.cs"
    Inherits="RM.Web.SysSetBase.superAdmin.businadminEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>业务编辑</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server" style="overflow: hidden;">
    <dl class="adifoli adilen" style="width: 470px; float: left;">
        <dd>
            <small>酒店名称</small>
            <div>
                <select>
                    <option value="value">美思柏丽酒店</option>
                </select>
            </div>
        </dd>
        <dd>
            <small>购买项目</small>
            <div>
                <select>
                    <option value="value">智订云酒店营销系统</option>
                </select>
            </div>
        </dd>
        <dd>
            <small>对接系统</small>
            <div>
                <select>
                    <option value="value">国光酒店管理系统</option>
                </select>
            </div>
        </dd>
        <dd>
            <small>订单来源</small>
            <div>
                <select>
                    <option value="官网">官网</option>
                    <option value="员工">员工</option>
                </select>
            </div>
        </dd>
        <dd>
            <small>员工姓名</small>
            <div>
                <input type="text" value="何佳丽" />
            </div>
        </dd>
        <dd>
            <small>产生时间</small>
            <div>
                <input type="text" class="date" />
            </div>
        </dd>
        <dd class="skuan">
            <small>收款状态</small>
            <div class="radio">
                <label class="checked">
                    未收款</label><label>已收款</label>
            </div>
        </dd>
        <dd yshou>
            <small>销售模式</small>
            <div>
                <select>
                    <option value="value">官网</option>
                </select>
            </div>
        </dd>
        <dd yshou>
            <small>订单金额</small>
            <div>
                <input type="text" />
            </div>
        </dd>
        <dd yshou>
            <small>开始时间</small>
            <div>
                <input type="text" class="date" />
            </div>
        </dd>
        <dd yshou>
            <small>到期时间</small>
            <div>
                <input type="text" class="date" />
            </div>
        </dd>
        <dd class="jindu">
            <small>业务进度</small>
            <div class="radio wsk">
                <label class="checked" wsk>
                    洽谈中</label><label wsk>试用中</label><label ysk>使用中</label><label>已结束</label>
            </div>
        </dd>
        <dd>
            <small>情况说明</small>
            <div>
                <textarea cols="30" rows="10"></textarea>
            </div>
        </dd>
    </dl>
    <div class="frgul">
        <b>操作日志</b>
        <ul>
            <li><span>2017-11-12 18:00</span><span>管理员</span><span>创建订单</span><span class="state">洽谈中</span></li>
            <li><span>2017-11-12 18:01</span><span>管理员</span><span>修改订单</span><span class="state">试用中</span></li>
        </ul>
    </div>
    </form>
    <script type="text/javascript">
        //来源
        $('.come select').on('change', function () {
            $(this).val().replace(/\s+/g, '') == '官网' ?
            $('.come').next('dd').hide() :
            $('.come').next('dd').show()
        })


        $('.radio').on('click', 'label', function () {
            $(this).addClass('checked').siblings().removeClass('checked');

            //是否收款
            if ($(this).parents('dd').hasClass('skuan')) {
                if ($(this).html().replace(/\s+/g, '') == '未收款') {
                    $('.jindu .radio').removeClass('ysk').addClass('wsk');
                    $('.jindu .radio').find('[wsk]:eq(0)').addClass('checked').siblings().removeClass('checked');
                    $('.adifoli dd[yshou]').hide();
                } else {
                    $('.jindu .radio').removeClass('wsk').addClass('ysk');
                    $('.jindu .radio').find('[ysk]:eq(0)').addClass('checked').siblings().removeClass('checked');
                    $('.adifoli dd[yshou]').show();
                }
            }
        });
    </script>
</body>
</html>
