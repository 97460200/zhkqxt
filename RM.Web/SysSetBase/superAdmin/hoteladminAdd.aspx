<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="hoteladminAdd.aspx.cs"
    Inherits="RM.Web.SysSetBase.superAdmin.hoteladminAdd" %>

<%@ Register Src="~/SysSetBase/superAdmin/colorPage.ascx" TagName="page" TagPrefix="UC" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>酒店管理</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <link href="/Themes/Styles/minicolors.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/js/avalon.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server" style="overflow: hidden;" ms-controller='app' class="ms-controller">
    <div class="crlist">
        <dl class="adifoli" style="width: 470px; float: left; margin-right: -50px;">
            <dd>
                <small>酒店名称</small>
                <div>
                    <input type="text" style="width: 330px;" />
                </div>
            </dd>
            <dd>
                <small>酒店logo</small>
                <div>
                    <span class="btn">上传</span> <i class="photo">
                        <img src="/Themes/Images/Login/logo.png" /></i>
                </div>
            </dd>
            <dd>
                <small>酒店类型</small>
                <div class="radio">
                    <label>
                        单体</label><label class="checked">连锁</label>
                    <div class="tt">
                        共
                        <input type="text" style="width: 100px;" />
                        间
                    </div>
                </div>
            </dd>
            <dd>
                <small>酒店风格</small>
                <div>
                    <select style="width: 255px;" ms-change='selFun'>
                        <option value="'mcol': '#ff9900','micon': '#ff9900','spri': '#ff9900','scop': '#ff9900','bbg': '#ffffff','bfgx': '#e3e3e3','bfq': '#eeeeee','bdeep': '#333333','bmid': '#666666','bligh': '#999999','bicon': '#999999','bbtn': '#b6b6b6'">
                            白色</option>
                        <option value="'mcol': '#ff0000','micon': '#ff0000','spri': '#ff0000','scop': '#ff0000','bbg': '#000000','bfgx': '#666666','bfq': '#444444','bdeep': '#ffffff','bmid': '#b6b6b6','bligh': '#cccccc','bicon': '#cccccc','bbtn': '#666666'">
                            黑色</option>
                    </select>
                    <a class="btn">自定义</a>
                </div>
            </dd>
            <dd>
                <small>酒店状态</small>
                <div class="radio">
                    <label>
                        试用</label><label class="checked">正式</label><label>关闭</label>
                </div>
            </dd>
        </dl>
        <%--展示图--%>
        <UC:page ID="page" runat="server" />
    </div>
    </form>
    <script type="text/javascript">
        avalon.ready(function () {
            var vm = avalon.define({
                $id: 'app',
                mcol: '#ff9900',
                micon: '#ff9900',

                spri: '#ff9900',
                scop: '#ff9900',

                bbg: '#ffffff',
                bfgx: '#e3e3e3',
                bfq: '#eeeeee',
                bdeep: '#333333',
                bmid: '#666666',
                bligh: '#999999',
                bicon: '#999999',
                bbtn: '#b6b6b6',


                selFun: function () {
                    var obj = eval('({' + this.value + '})');

                    vm.mcol = obj.mcol;
                    vm.micon = obj.micon;

                    vm.spri = obj.spri;
                    vm.scop = obj.scop;

                    vm.bbg = obj.bbg;
                    vm.bfgx = obj.bfgx;
                    vm.bfq = obj.bfq;
                    vm.bdeep = obj.bdeep;
                    vm.bmid = obj.bmid;
                    vm.bligh = obj.bligh;
                    vm.bicon = obj.bicon;
                    vm.bbtn = obj.bbtn;
                },



                footIndex: 0,
                chfoot: function (index) {
                    vm.footIndex = index;

                    //                    $('.foot a').css('color', vm.bligh);
                    //                    $('.foot a').eq(index).css('color', vm.micon);
                }
            });

            avalon.scan();
        })
    </script>
</body>
</html>
