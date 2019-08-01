<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="jueseguanli.aspx.cs" Inherits="RM.Web.SysSetBase.xitongyonghu.jueseguanli" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>角色管理</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(function () {
            $(".icon-kg1").click(function () {
                $(this).hasClass('kgClose') ? $(this).removeClass('kgClose') : $(this).addClass('kgClose');
                if ($(this).hasClass('kgClose') == true) {
                    $(this).next(".sub").children("span").children("a").addClass("icon-kg2 kgClose2");
                } else if ($(this).hasClass('kgClose') == false) {
                    $(this).next(".sub").children("span").children("a").removeClass("kgClose2");
                }
            })

            $(".icon-kg2").click(function () {
                $(this).hasClass('kgClose2') ? $(this).removeClass('kgClose2') : $(this).addClass('kgClose2');
            })

            //二级列表
            $('.useList').on('click', 'b', function () {
                $(this).parent().toggleClass('down');
                $(this).siblings('.sub').slideToggle(140);
            })
        })

       
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <dl class="copnli jsgl">
        <div style="width:25%;float:left;">
            <dd class="useList">
                <small>角色列表</small>
                <div class="checkbox">
                    <label>员工</label><br>
                    <label>经理</label><br>
                    <label>销售员</label><br>
                </div>
            </dd>
        </div>
        <div style="width:75%;float:left;">
            <dd class="useList">
                <small>可用门店</small>
                <ol id="gmkfNav">
                    <li>
                        <b class="icon-kg1">基础功能</b>
                        <div class="sub">
                            <span><a class="icon-kg2">基础功能01</a></span>
                        </div>
                    </li>
                    <li>
                        <b class="icon-kg1">重要功能</b>
                        <div class="sub">
                            <span><a class="icon-kg2">促销维护</a></span>
                            <span><a class="icon-kg2">酒店信息</a></span>
                            <span><a class="icon-kg2">协议客户管理</a></span>
                            <span><a class="icon-kg2">创建客房推广页面</a></span>
                            <span><a class="icon-kg2">公众号认证</a></span>
                        </div>
                    </li>
                    <li>
                        <b class="icon-kg1">核心功能</b>
                        <div class="sub">
                            <span><a class="icon-kg2">核心功能01</a></span>
                        </div>
                    </li>
                    <li>
                        <b class="icon-kg1">现金账户</b>
                        <div class="sub">
                            <span><a class="icon-kg2">现金账户01</a></span>
                        </div>
                    </li>
                    <li>
                        <b class="icon-kg1">会员卡权限</b>
                        <div class="sub">
                            <span><a class="icon-kg2">会员卡权限01</a></span>
                        </div>
                    </li>
                </ol>
            </dd>
            
        </div>
    </dl>
    </form>

</body>
</html>
