<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="addlanmu.aspx.cs" Inherits="RM.Web.SysSetBase.live.addlanmu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>添加栏目</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <dl class="addevaluate addlanmu">
            <dd>
                <small>栏目名称</small>
                <div>
                    <input type="text" name="name" value=" " />
                    <p>
                    如外观大堂、客房、会议室、餐厅、休闲娱乐等
                    </p>
                </div>
            </dd>
            <dd>
                <small>栏目类型</small>
                <div class="radio clearfix">
                    <label class="checked"  data-index="1">添加图片</label>
                    <label data-index="2">关联图片</label>
                    <label data-index="3">外部链接</label>
                    <div class="mt33" style="display:block;">
                    </div>
                    <div class="mt33">
                        <select>
                            <option value="value">餐饮服务</option>
                        </select>
                    </div>
                    <div class="mt33">
                        <input type="text" name="name" value="" />
                    </div>
                  
                </div>
            </dd>

        </dl>
    </form>
    <script src="/SysSetBase/css/jquery.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(".radio label").click(function () {
            $(this).addClass('checked').siblings().removeClass('checked');
            var i = $(this).data('index');
            $(".mt33").hide();
            $(".mt33").eq(i - 1).show(); //最低为0
        })
    </script>
</body>
</html>
