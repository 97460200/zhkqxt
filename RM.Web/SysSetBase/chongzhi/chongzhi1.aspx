<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="chongzhi1.aspx.cs" Inherits="RM.Web.SysSetBase.chongzhi.chongzhi1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>充值</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="../../Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    <script type="text/javascript">


        function Gopay() {
            window.location.href = "pay.aspx?Type=" + $("#hdType").val() + "&Money=" + $("#txtMoney").val()
        }
        </script>

</head>
<body>
    <form id="form1" runat="server">   
      <input id="hdType" runat="server"  type="hidden" value="3" />
              <div class="tools_bar btnbartitle btnbartitlenr" style="display: block;">
        <div>
            当前位置：<span><a title="系统首页" href="/Frame/HomeIndex.aspx">系统首页</a></span><span> &gt;
            </span><span>财务管理</span> &gt; <span>奖金账户</span>
        </div>
    </div>
     <div class="pd20">
        <div class="kefangsz">
            <div class="kefangszt">
                 智订云酒店 奖金充值
            </div>
            <dl class="addevaluate kefangszb" style="width:450px;">
                
                <dd>
                    <small>充值类型</small>
                    <div>
                           奖金充值
                    </div>
                </dd>

                <dd>
                    <small>金额</small>
                    <div >
                        <input type="text" name="name" id="txtMoney"   runat="server"  onkeyup="this.value=this.value.replace(/\D/g,'')"/>
                        <p style="color:#666;">
                            金额不能小于100元的整数
                        </p>
                    </div>
                </dd>
            </dl>
        </div>
        <a class="btn" onclick="Gopay()" style="margin-top: 15px;">充值</a>
    </div>

    </form>
</body>
</html>
