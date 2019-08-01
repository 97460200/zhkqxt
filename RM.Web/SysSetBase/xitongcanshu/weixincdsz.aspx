<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="weixincdsz.aspx.cs" Inherits="RM.Web.SysSetBase.xitongcanshu.weixincdsz" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>微信菜单设置</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="weixincdsz clearfix">
        <div class="weixincdsz01">
            <img src="/SysSetBase/img/shili01.png" alt="Alternate Text" />
            <div class="cdsz">
                <ul>
                    <li class="act">
                        first
                        
                        <div class="cdsz01">
                            <ul>
                                <li class="act">
                                    first
                                </li>
                                <li>
                                    first
                                </li>
                                <li>
                                    first
                                </li>
                            </ul>
                        </div>
                    </li>
                    <li>
                        first
                    </li>
                    <li>
                        first
                    </li>
                </ul>
            </div>
        </div>
        <div class="weixincdsz02">
            <div class="d1 clearfix">
                <div class="d11">
                    子菜单名称
                </div>
                <div class="d12">
                    <span>删除</span>
                </div>
            </div>
            <dl class="addevaluate weixincdszhi">
                <dd>
                    <small>菜单名称</small>
                    <div>
                        <input type="text" name="name" value=" " />
                        <p>
                        字数不超过4个汉字
                        </p>
                    </div>
                </dd>
                <dd class="cdnr">
                    <small>菜单内容</small>
                    <div class="radio">
                        <label class="checked" value='1' >发送消息</label>
                        <label value='0'>跳转页面</label><i>选择页面链接</i>
                        <div>
                            <textarea>content</textarea>
                        </div>
                    </div>
                </dd>
                <dd class="sharebtn" style="margin-left:65px;">
                    <a href="#">保存</a>
                </dd>
            </dl>

        </div>
    
    </div>
    </form>
</body>
</html>
