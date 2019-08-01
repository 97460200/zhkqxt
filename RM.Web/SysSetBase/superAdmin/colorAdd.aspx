<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="colorAdd.aspx.cs" Inherits="RM.Web.SysSetBase.superAdmin.colorAdd" %>

<%@ Register Src="~/SysSetBase/superAdmin/colorPage.ascx" TagName="page" TagPrefix="UC" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>业务管理</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/Themes/Styles/minicolors.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/js/avalon.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server" ms-controller='app' class="ms-controller">
    <div class="crlist clearfix">
        <%--配色选项--%>
        <div class="crul_l">
            <div class="crli block">
                <span class="name">配色名称</span>
                <div class="val">
                    <input type="text" />
                </div>
            </div>
            <div class="crli block">
                <span class="name">复制已有配色</span>
                <div class="val">
                    <select ms-change='selFun'>
                        <option value="'mcol': '#ff9900','micon': '#ff9900','spri': '#ff9900','scop': '#ff9900','bbg': '#ffffff','bfgx': '#e3e3e3','bfq': '#eeeeee','bdeep': '#333333','bmid': '#666666','bligh': '#999999','bicon': '#999999','bbtn': '#b6b6b6'">
                            白底</option>
                        <option value="'mcol': '#ff0000','micon': '#ff0000','spri': '#ff0000','scop': '#ff0000','bbg': '#000000','bfgx': '#666666','bfq': '#444444','bdeep': '#ffffff','bmid': '#b6b6b6','bligh': '#cccccc','bicon': '#cccccc','bbtn': '#666666'">
                            黑底</option>
                    </select>
                </div>
            </div>
            <%--主色--%>
            <strong class="crtitle"><b>主色</b></strong>
            <div class="crli">
                <ul>
                    <li><span class="name">主色调</span>
                        <input type="text" class="colpt" maxlength="7" ms-duplex='mcol' /><label ms-css-background='mcol'></label>
                    </li>
                    <li><span class="name">高亮图标</span>
                        <input type="text" class="colpt" maxlength="7" ms-duplex='micon' /><label ms-css-background='micon'></label>
                    </li>
                </ul>
            </div>
            <%--辅助色--%>
            <strong class="crtitle"><b>辅助色</b></strong>
            <div class="crli">
                <ul>
                    <li><span class="name">价格积分</span>
                        <input type="text" class="colpt" maxlength="7" ms-duplex='spri' /><label ms-css-background='spri'></label>
                    </li>
                    <li><span class="name">高亮卡券底色</span>
                        <input type="text" class="colpt" maxlength="7" ms-duplex='scop' /><label ms-css-background='scop'></label>
                    </li>
                </ul>
            </div>
            <%--基础色--%>
            <strong class="crtitle"><b>基础色</b></strong>
            <div class="crli">
                <ul>
                    <li><span class="name">全局页面底色</span>
                        <input type="text" class="colpt" maxlength="7" ms-duplex='bbg' /><label ms-css-background='bbg'></label>
                    </li>
                    <li><span class="name">分割线、边框</span>
                        <input type="text" class="colpt" maxlength="7" ms-duplex='bfgx' /><label ms-css-background='bfgx'></label>
                    </li>
                    <li><span class="name">分隔区</span>
                        <input type="text" class="colpt" maxlength="7" ms-duplex='bfq' /><label ms-css-background='bfq'></label>
                    </li>
                    <li><span class="name">深色文字</span>
                        <input type="text" class="colpt" maxlength="7" ms-duplex='bdeep' /><label ms-css-background='bdeep'></label>
                    </li>
                    <li><span class="name">中色文字</span>
                        <input type="text" class="colpt" maxlength="7" ms-duplex='bmid' /><label ms-css-background='bmid'></label>
                    </li>
                    <li><span class="name">浅色文字</span>
                        <input type="text" class="colpt" maxlength="7" ms-duplex='bligh' /><label ms-css-background='bligh'></label>
                    </li>
                    <li><span class="name">非高亮图标</span>
                        <input type="text" class="colpt" maxlength="7" ms-duplex='bicon' /><label ms-css-background='bicon'></label>
                    </li>
                    <li><span class="name">禁用按钮</span>
                        <input type="text" class="colpt" maxlength="7" ms-duplex='bbtn' /><label ms-css-background='bbtn'></label>
                    </li>
                </ul>
            </div>
        </div>
        <%--展示图--%>
        <UC:page ID="page" runat="server" />
    </div>
    </form>
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/js/colorpicker.js" type="text/javascript"></script>
    <script type="text/javascript">
        //更改颜色，右边模块跟着改变
        //        var changeCol = function (currCol) {
        //            //console.log(currCol)
        //            if (!(currCol.length == 4 || currCol.length == 7)) return;
        //            console.log(currCol)
        //        };

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
                    console.log('{' + this.value + '}')
                    var obj = eval('({' + this.value + '})');
                    console.log(obj)

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
                    $('.foot a').css('color', vm.bligh);
                    $('.foot a').eq(index).css('color', vm.micon);
                }
            });

            vm.$watch('micon', function (newVal) {
                console.log(newVal);
                $('.foot .active a').css('color', newVal);
            })

            avalon.scan();
        })
    </script>
</body>
</html>
