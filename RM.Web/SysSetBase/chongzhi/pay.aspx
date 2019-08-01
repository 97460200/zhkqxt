<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pay.aspx.cs" Inherits="RM.Web.SysSetBase.chongzhi.pay" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>支付</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="../../Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server"> 
    
                <div class="tools_bar btnbartitle btnbartitlenr" style="display: block;">
        <div>
            当前位置：<span><a title="系统首页" href="/Frame/HomeIndex.aspx">系统首页</a></span><span> &gt;
            </span><span>财务管理</span> &gt; <span>账户充值</span>
        </div>
    </div>
       <div class="pd20">
        <div class="kefangsz">
            <div class="kefangszt">
                 智订云酒店 账户充值
            </div>
            <dl class="addevaluate kefangszb" style="width:450px;">
                <div style="width: 50px;height: 50px;border: 1px solid #ff9900;border-radius: 50%;text-align: center;line-height: 50px;font-size: 24px;font-family: ”Microsoft YaHei”;color: #ff9900;margin-left:105px;">
                    √
                </div>
               <dd>
                    <small>充值类型</small>
                    <div  id="divpay" runat="server">
                       <b  id="PayType" runat="server">500</b>
                    </div>
                </dd>


                <dd>
                    <small>订单金额</small>
                    <div  id="divprice" runat="server">
                       <b  id="price" runat="server">500</b>
                    </div>
                </dd>

                <dd>
                    <small>订单编号</small>
                    <div  id="div1" runat="server">
                       <b  id="orders" runat="server"></b>
                    </div>
                </dd>

                <dd>
                    <small>酒店名称</small>
                    <div  id="div2" runat="server">
                       <b  id="hotel" runat="server">智订云酒店</b>
                    </div>
                </dd>

                <dd>
                    <small>下单时间</small>
                    <div  id="div3" runat="server">
                       <b  id="rlrq" runat="server"></b>
                    </div>
                </dd>


            </dl>
        </div>
   
    </div>

    </form>
</body>
</html>
