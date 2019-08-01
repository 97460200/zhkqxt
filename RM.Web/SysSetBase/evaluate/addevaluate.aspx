<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="addevaluate.aspx.cs" Inherits="RM.Web.SysSetBase.evaluate.addevaluate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>评价管理</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer" />
    <script src="../../WDatePicker/WdatePicker.js" type="text/javascript"></script>
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/SysSetBase/css/jquery.js" type="text/javascript"></script>
</head>
<body>
<body>
    <form id="form1" runat="server">
    <dl class="addevaluate">
        <div class="sharehead">
            <span>基本信息</span><span class="line"></span>
        </div>
        <div class="clearfix">
            <div style="width:40%;float:left;">
                <dd>
                    <small>订单号</small>
                    <div style="color:#3399cc">
                        wx20171117104802476
                    </div>
                </dd>
                <dd>
                    <small>入住酒店</small>
                    <div>
                        美思柏丽酒店
                    </div>
                </dd>
                <dd>
                    <small>房型</small>
                    <div>
                        豪华大客房 
                    </div>
                </dd>
                <dd>
                    <small>入住/离店时间</small>
                    <div>
                        2017-11-12至2017-11-12 
                    </div>
                </dd>
            </div>
            <div style="width:60%;float:left;">
                <dd>
                    <small>联系人</small>
                    <div>
                        何佳丽 
                    </div>
                </dd>
                <dd>
                    <small>手机号码</small>
                    <div>
                        13794875454
                    </div>
                </dd>
                <dd>
                    <small>实际支付</small>
                    <div>
                        236 
                    </div>
                </dd>
            </div>
        </div>
        <div class="sharehead sharehead02">
            <span>评价内容</span><span class="line"></span>
        </div>
        <dd>
            <small>评分</small>
            <div>
                <span class="star"><em>服务</em><b>4分</b></span>
                <span class="star"><em>清洁</em><b>4分</b></span>
                <span class="star"><em>设施</em><b>4分</b></span>
                <span class="star"><em>环境</em><b>4分</b></span>
                <span class="star">
                    <b>总分</b> <em class="mr9">4.3分</em>
                    <i class="icon-starfull active"></i><i class="icon-starfull active"></i><i class="icon-starfull active"></i><i class="icon-starfull"></i><i class="icon-starfull"></i>
                </span>
            </div>
        </dd>
        <dd>
            <small>评价内容</small>
            <div>
                酒店的服务态度极好，唯一的不足就是空气不太好~希望日后能够有所改善！
            </div>
        </dd>
        <dd>
            <small>图片</small>
            <div>
                <span class="photobd"><img src="../img/ewm.png" /></span>
                <span class="photobd"><img src="../img/ewm.png" /></span>
            </div>
        </dd>
        <dd>
            <small>评价时间</small>
            <div>
                2017-10-10 18:00
            </div>
        </dd>
        <div class="sharehead sharehead02">
            <span>回复信息</span><span class="line"></span>
        </div>
        <%--
        <dd>
            <small>回复内容</small>
            <div>
                <textarea cols="30" rows="10"></textarea>
            </div>
        </dd>
        <dd>
            <small>是否公开</small>
            <div class="radio">
                <label class="checked">是</label>
                <label>否</label>
            </div>
        </dd>--%>
        <dd>
            <small>回复内容</small>
            <div>
                可能是由于刚刚消毒完，未开窗换气，以后我们将尽量避免这样的疏忽！感谢您对柏丽酒店的支持，欢迎下次光临！
            </div>
        </dd>
        <dd>
            <small>是否公开</small>
            <div>
                <span class="blue">是</span>
            </div>
        </dd>
        <dd>
            <small>回复人</small>
            <div>
                杜先生
            </div>
        </dd>
        <dd>
            <small>回复时间</small>
            <div>
                2017-10-10 18:30
            </div>
        </dd>
    </dl>
    <div class="sharebottombtn">
        <div class="fr">
            <input type="submit" name="btnSumit" value="提交">
            <input type="submit" name="btnSumit" value="重置">
        </div>
    </div>
    </form>
    <script type="text/javascript">
        $('.radio').on('click', 'label', function () {
            $(this).addClass('checked').siblings().removeClass('checked');
        });
        $('.checkbox').on('click', 'label', function () {
            $(this).hasClass('checked') ? $(this).removeClass('checked') : $(this).addClass('checked');
        });
    </script>
</body>
</html>
