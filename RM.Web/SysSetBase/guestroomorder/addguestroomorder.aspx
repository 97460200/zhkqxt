<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="addguestroomorder.aspx.cs" Inherits="RM.Web.SysSetBase.guestroomorder.addguestroomorder" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>客房订单</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer" />
    <script src="../../WDatePicker/WdatePicker.js" type="text/javascript"></script>
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="clearfix">
        <dl class="addguestroomorder">
        <dd>
            <small>订单号</small>
            <div>
                wx20171122030714
            </div>
        </dd>
        <dd>
            <small>订单来源</small>
            <div>
                <span class="ddly ygxs">员工销售</span><span class="ddly gw">官微</span>何新
            </div>
        </dd>
        <dd>
            <small>预订房型</small>
            <div>
                豪华大客房-有早 
            </div>
        </dd>
        <dd>
            <small>入住/离店时间</small>
            <div>
                2017-11-12至2017-11-12   共计1天
            </div>
        </dd>
        <dd>
            <small>房数</small>
            <div>
                1
            </div>
        </dd>
        <dd>
            <small>到店时间</small>
            <div>
                2017-11-12 09:00
            </div>
        </dd>
        <dd>
            <small>联系人</small>
            <div>
                何佳丽
            </div>
        </dd>
        <dd>
            <small>手机号码</small>
            <div>
                15986562656
            </div>
        </dd>
        <dd>
            <small>下单时间</small>
            <div>
                2017-11-10 11:52
            </div>
        </dd>
        <dd>
            <small>订单总额</small>
            <div>
                ￥253
            </div>
        </dd>
        <dd>
            <small>优惠</small>
            <div>
                ￥5
            </div>
        </dd>
        <dd>
            <small>应付</small>
            <div>
                <em class="FC8D1F">￥248</em><span class="zfzt yzf">已支付</span><%--<span class="zfzt wzf">未支付</span><span class="zfzt tkz">退款中</span><span class="zfzt ytk">已退款</span>--%>
            </div>
        </dd>
        <dd>
            <small>支付方式</small>
            <div>
                <span class="zffs wxzf">微信支付</span><%--<span class="zffs hykzf">会员卡支付</span><span class="zffs jfzf">积分兑换</span><span class="zffs qtzf">到店付款</span>--%> 订单确认后，客人不可取消
            </div>
        </dd>
        <dd>
            <small>特殊要求</small>
            <div>
                无
            </div>
        </dd>
        <dd>
            <small>订单状态</small>
            <div>
                未确认
            </div>
        </dd>
    </dl>
        <dl class="addguestroomorder">
        <dd>
            <small>预订会员信息</small>
            <div>
                <table border="0" cellpadding="0" cellspacing="0">
                    <thead>
                        <tr>
                            <th>
                                会员ID
                            </th>
                            <th>
                                会员手机号
                            </th>
                            <th>
                                会员姓名
                            </th>
                            <th>
                                会员级别
                            </th>
                            <th>
                                注册时间
                            </th>
                        </tr>
                    </thead>
                    <tr>
                        <td class="c39c">
                            3222124006
                        </td>
                        <td>
                            15965253622
                        </td>
                        <td>
                            李立威
                        </td>
                        
                        <td>
                            钻石会员
                        </td>
                        <td>
                            2018-01-11 17:00
                        </td>

                    </tr>
                </table>
            </div>
        </dd>
        <dd>
            <small>操作日志</small>
            <div>
                <p>
                    2017-11-12 18:00    会员    创建订单
                </p>
                <p>
                    2017-11-12 18:01    会员    支付成功
                </p>
                <p>
                    2017-11-12 18:01    何欣    确认订单
                </p>
                
            </div>
        </dd>
        <dd>
            <small>支付记录</small>
            <div>
                <span>支付单号 <em class="c39c">1236233544</em></span>    <span style="margin-left:5px;">支付时间 <em>2018-01-02 08:00</em></span>
            </div>
        </dd>
    </dl>
    </div>
    
    <%--<div class="adifoliBtn">
        <input type="submit" name="btnSumit" value="提交">
    </div>--%>
        <div class="sharebottombtn">
            <div class="fr">
                <input type="submit" name="btnSumit" value="确认订单" class="w85">
                <input type="submit" name="btnSumit" value="取消订单" class="w85">
                <input type="submit" name="btnSumit" value="结账">
                <input type="submit" name="btnSumit" value="入住">
                <input type="submit" name="btnSumit" value="打印">
            </div>
        </div>
    <div>
        
    </div>
    </form>
</body>
</html>
