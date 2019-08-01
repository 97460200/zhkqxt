<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="weixingjcsz.aspx.cs" Inherits="RM.Web.SysSetBase.xitongcanshu.weixingjcsz" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>关注回复</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <dl class="addevaluate weixingjcsz">
            <dd>
                <small>规则名称</small>
                <div>
                    <input type="text" name="name" value="" />
                </div>
            </dd>
            <dd>
                <small>关键字</small>
                <div>
                    <input type="text" name="name" value="" />
                </div>
            </dd>
            <dd>
                <small>回复类型</small>
                <div class="radio">
                    <label class="checked" value='1' >文字</label>
                    <label value='0'>图片</label>
                </div>
            </dd>
            <dd>
                <small>回复内容</small>
                <div>
                    <textarea></textarea>
                </div>
            </dd>
            <dd>
                <small>回复内容</small>
                <div>
                    <div class="add">
                        +
                    </div>
                    <div class="addimg">
                        <img src="../img/ewm.png" alt="Alternate Text" />
                        <div class="addimgclose">
                            <i class="icon-close"></i>
                        </div>      
                    </div>
                </div>
            </dd>
        </dl>
    </form>
</body>
</html>
