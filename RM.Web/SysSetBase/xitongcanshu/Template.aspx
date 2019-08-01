<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Template.aspx.cs" Inherits="RM.Web.SysSetBase.xitongcanshu.Template" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>提示显示设置</title>
        <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
   <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $('ul li').on('click', 'i', function () {
                var hid = $(this).attr("hid");
                for (var i = 0; i < $("ul li i").length; i++) {
                    if ($("ul li i").eq(i).attr('hid') == hid) {
                        $("ul li i").eq(i).addClass('checked');
                    } else {
                        $("ul li i").eq(i).removeClass('checked');
                    }
                }
            });

            var hasTemplate = $("#hdhasTemplate").val();
            for (var i = 0; i < $("ul li i").length; i++) {
                if ($("ul li i").eq(i).attr('hid') == hasTemplate) {
                    $("ul li i").eq(i).addClass('checked');
                } else {
                    $("ul li i").eq(i).removeClass('checked');
                }
            }

        });
    </script>
      <script type="text/javascript">
        //确定
        function sure() {
            var jsons = "";
            var hasval = "0";
            for (var i = 0; i < $("ul li i").length; i++) {
                if ($("ul li i").eq(i).hasClass('checked') == true) {
                    hasval = "1";
                    jsons = $("ul li i").eq(i).attr("hid") + "," + $("ul li i").eq(i).attr("hinfo") + "," + $("ul li i").eq(i).attr("hdtype") + "," + $("ul li i").eq(i).attr("hdname");
                }
            }
            if (hasval == "0")
            {
                showTipsMsg("请选择模板图片", 3000, 3);
                return;
            }
            
            art.dialog.data('jsons', jsons);
            art.dialog.close();
        }
 </script>
</head>
<body>
    <form id="form1" runat="server">
        <input runat="server" type="hidden" id="hdAdminHotelId" value="-1" />
        <input runat="server" type="hidden" id="hdhasTemplate" value="cz01.png" />
    <div class="template">
        <ul class="clearfix">
            <li>
                <div class="img">
                    <img src="/upload/TipsPhoto/SNcz.png" />
                </div>
                <p>
                    <i class="checked" hid="cz01.png" hinfo="会员充值有礼<br />多充多优惠" hdtype="0"  hdname="[模板]充值">充值</i>
                </p>
            </li>
            <li>
                <div class="img">
                    <img src="/upload/TipsPhoto/SNms.png" />
                </div>
                <p>
                    <i class="" hid="ms01.png" hinfo="火速送达<br />你想要的美食" hdtype="0" hdname="[模板]美食">美食</i>
                </p>
            </li>
   <%--         <li>
                <div class="img">
                    <img src="/upload/TipsPhoto/SNkq.png" />
                </div>
                <p>
                    <i class="" hid="kq01.png" hinfo="优惠卡券" hdtype="1" hdname="[模板]卡券">卡券</i>
                </p>
            </li>--%>
            <li>
                <div class="img">
                    <img src="/upload/TipsPhoto/SNxsqg.png" />
                </div>
                <p>
                    <i class="" hid="xsqg01.png" hinfo="限时抢购<br />超值套票" hdtype="0" hdname="[模板]限时抢购">限时抢购</i>
                </p>
            </li>
            <li>
                <div class="img">
                    <img src="/upload/TipsPhoto/SNwsc.png" />
                </div>
                <p>
                    <i class="" hid="wsc01.png" hinfo="微商城<br />酒店里的便利店" hdtype="0" hdname="[模板]微商城">微商城</i>
                </p>
            </li>
            <li>
                <div class="img">
                    <img src="/upload/TipsPhoto/SNzc.png" />
                </div>
                <p>
                    <i class="" hid="zc01.png" hinfo="注册成为会员<br />尊享会员权益" hdtype="2"  hdname="[模板]注册">注册</i>
                </p>
            </li>
            <li>
                <div class="img">
                    <img src="/upload/TipsPhoto/SNxf.png" />
                </div>
                <p>
                    <i class="" hid="xf01.png" hinfo="消费可获得<br />积分和卡券" hdtype="3" hdname="[模板]消费">消费</i>
                </p>
            </li>
            <li>
                <span>
                    敬请期待更多模板<br/>...
                </span>
            </li>
        </ul>
    </div>

  


     <div class="adifoliBtn">
        <div class="SubmitButton" style="float: right;">
            <a class="bbgg" href="javascript:void(0)" onclick="sure();"><span class="bbgg bbgg1">
                提 交</span></a> <a class="bbgg" href="javascript:void(0)" onclick="OpenClose();"><span>
                    关 闭</span></a>
        </div>
    </div>

    </form>
</body>
</html>
