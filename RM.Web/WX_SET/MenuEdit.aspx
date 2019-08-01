
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MenuEdit.aspx.cs" Inherits="RM.Web.WX_SET.MenuEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>自定义菜单</title>
    <meta content="width=device-width,initial-scale=1,user-scalable=no" name="viewport" />
    <link href="/App_Themes/default/css/Total.css" rel="stylesheet" type="text/css" />
    <link href="/App_Themes/default/css/menuEdit.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="SetUseName">
    </div>
    <div class="meEdit">
        <%--   <h2>
            <b>自定义菜单</b> <span>使用说明</span>
        </h2>
        <div class="meCare">
            <span>菜单编辑中</span> <span>菜单未发布，请确认菜单编辑完成后点击“保存并发布”同步到手机。若停用菜单，<a>请点击这里</a></span>
        </div>--%>
        <div class="meMain clearfix">
            <div class="phase">
                <div class="ph_head">
                    <b>商都观景酒店</b>
                </div>
                <div class="ph_foot clearfix">
                    <i class="keybord"></i>

                    <ul class="phfoNav">
                        <%--<li><b>菜单名称</b>
                            <div class="subNav">
                                <a class="icon-plus" title="最多添加5个子菜单名称"></a>
                            </div>
                        </li>
                        <li class="icon-plus" title="最多添加3个菜单名称"></li>--%>
                    </ul>
                </div>
            </div>
            <div class="phtip">
                点击左侧菜单进行编辑操作
            </div>
            <div class="survey">
                <h4>
                    <b id="nemuName">什么名称</b><span onclick="right.F_del('Dete');">删除菜单</span>
                </h4>
                <p class="sm" id="sm">
                    已添加子菜单，仅可设置菜单名称。
                </p>
                <div class="surList">
                    <div class="surSon">
                        <span class="fill">菜单名称</span><span><input type="text" id="newTitle" placeholder="菜单名称"
                            maxlength="4" /><i>字数不超过4个汉字或8个字符</i></span>
                    </div>
                </div>
                <div class="surList" id="submenuContxt">
                    <div class="surSon">
                        <span class="fill">菜单内容</span><span id="exchange"><a class="active">发送消息</a><a>跳转网页</a></span>
                    </div>
                    <ul class="desCxt" id="showList">
                        <li style="display: block">//富文本
                            <textarea name="" id="sendText" cols="30" rows="10"></textarea>
                        </li>
                        <li>
                            <p class="tt">
                                订阅者点击该子菜单会跳到以下链接</p>
                            <div class="ctx_form">
                                <label>
                                    页面地址</label><input type="text" id="Url" placeholder="请输入网址" />
                            </div>
                            <div class="ctx_form">
                                <a class="swap">从公众号图文消息中选择</a>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
        <div class="phBtn">
            <a class="active" onclick="right.F_sure()">保存并发布</a>
        </div>
        <div class="delAlert" id="Dete">
            <div class="delAlr">
                <h3 onclick="right.F_del();">
                    温馨提示
                </h3>
                <div class="delText">
                    <div>
                        <b>删除确认</b> <span>删除后"密码"菜单下设置的内容将被删除</span></div>
                </div>
                <div class="delBtn">
                    <a class="active" onclick="right.F_del('sure');">确定</a><a onclick="right.F_del();">取消</a>
                </div>
            </div>
        </div>
    </div>
    </form>
    <script src="/App_Themes/default/js/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="menu.js" type="text/javascript"></script>
    <script type="text/javascript">
        $.ajax({                            //ajax初始化数据
            method: 'get',
            url: '/Ajax/menu.json',
            success: function (res) {
                var data = eval('(' + res + ')');
                left.F_init(data);
            }
        })
    </script>
</body>
</html>
