<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="houseState.aspx.cs" Inherits="RM.Web.SysSetBase.houseState.houseState" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <link href="/Themes/Scripts/PqGrid/style.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/PqGrid/jquery-ui.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/PqGrid/pqgrid.min.js" type="text/javascript"></script>
    <link href="/Themes/Scripts/PqGrid/pqgrid.min.css" rel="stylesheet" />
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <input runat="server" type="hidden" id="Hdhoteladmin" value="1" />
    <input runat="server" type="hidden" id="hdHotelId" value="-1" />
    <input runat="server" type="hidden" id="htHotelTree" value="true" />
    <input runat="server" type="hidden" id="hdJFState" value="1" />
    <div class="gtall gmkf clearfix">
        <div class="gmkfNav" id="HotelTree" runat="server">
            <dl>
                <%=hotelTreeHtml.ToString()%>
            </dl>
        </div>
        <div class="gmkfStion">
            <div class="wdyhd" style="padding-left: 20px; max-width: 1140px;">
                <div class="l">
                    <a class="active">默认设置</a><a>按月设置</a><a>按周设置</a>
                </div>
                <div class="evRgt R2" style="margin-left: 6px;">
                    <i class="icon-left"></i>
                    <input type="text" class="date" />
                    <i class="icon-right"></i>
                </div>
                <div class="evRgt R3">
                    <select id="nian">
                        <option value="2011">2011年</option>
                        <option value="2012">2012年</option>
                    </select>
                    <i class="icon-left"></i>
                    <select id="yuefen" runat="server">
                        <option value="1">1月</option>
                        <option value="2">2月</option>
                        <option value="3">3月</option>
                        <option value="4">4月</option>
                        <option value="5">5月</option>
                        <option value="6">6月</option>
                        <option value="7">7月</option>
                        <option value="8">8月</option>
                        <option value="9">9月</option>
                        <option value="10">10月</option>
                        <option value="11">11月</option>
                        <option value="12">12月</option>
                    </select>
                    <i class="icon-right"></i>
                </div>
                <div class="r setupbtn">
                    <span>批量设置价格</span><span>批量设置房态</span>
                </div>
            </div>
            <ul class="wdySetup">
                <%--默认设置--%>
                <li style="overflow: auto;">
                    <div class="gmkfList housefj" id="tbhtml" style="width: 430px; border-bottom: 1px solid #eee;">
                        <table>
                            <thead class="cn">
                                <tr>
                                    <th>
                                        客房参数
                                    </th>
                                    <th>
                                        微会员
                                    </th>
                                    <th>
                                        银卡会员
                                    </th>
                                    <th>
                                        金卡会员
                                    </th>
                                    <th>
                                        钻石会员
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr class="line">
                                    <td>
                                        门市价
                                    </td>
                                    <td>
                                        <input type="text">
                                        <i class="dw">元</i>
                                    </td>
                                    <td>
                                        <input type="text">
                                        <i class="dw">元</i>
                                    </td>
                                    <td>
                                        <input type="text">
                                        <i class="dw">元</i>
                                    </td>
                                    <td>
                                        <input type="text">
                                        <i class="dw">元</i>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        平日价
                                    </td>
                                    <td>
                                        <input type="text">
                                        <i class="dw">元</i>
                                    </td>
                                    <td>
                                        <input type="text">
                                        <i class="dw">元</i>
                                    </td>
                                    <td>
                                        <input type="text">
                                        <i class="dw">元</i>
                                    </td>
                                    <td>
                                        <input type="text">
                                        <i class="dw">元</i>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        周末价
                                    </td>
                                    <td>
                                        <input type="text">
                                        <i class="dw">元</i>
                                    </td>
                                    <td>
                                        <input type="text">
                                        <i class="dw">元</i>
                                    </td>
                                    <td>
                                        <input type="text">
                                        <i class="dw">元</i>
                                    </td>
                                    <td>
                                        <input type="text">
                                        <i class="dw">元</i>
                                    </td>
                                </tr>
                                <tr class="line">
                                    <td>
                                        平日兑换客房
                                    </td>
                                    <td>
                                        <input type="text">
                                        <i class="dw">分</i>
                                    </td>
                                    <td>
                                        <input type="text">
                                        <i class="dw">分</i>
                                    </td>
                                    <td>
                                        <input type="text">
                                        <i class="dw">分</i>
                                    </td>
                                    <td>
                                        <input type="text">
                                        <i class="dw">分</i>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        周末兑换客房
                                    </td>
                                    <td>
                                        <input type="text">
                                        <i class="dw">分</i>
                                    </td>
                                    <td>
                                        <input type="text">
                                        <i class="dw">分</i>
                                    </td>
                                    <td>
                                        <input type="text">
                                        <i class="dw">分</i>
                                    </td>
                                    <td>
                                        <input type="text">
                                        <i class="dw">分</i>
                                    </td>
                                </tr>
                                <tr class="line">
                                    <td>
                                        平日可订房数
                                    </td>
                                    <td>
                                        <input type="text">
                                        <i class="dw">间</i>
                                    </td>
                                    <td>
                                        <input type="text">
                                        <i class="dw">间</i>
                                    </td>
                                    <td>
                                        <input type="text">
                                        <i class="dw">间</i>
                                    </td>
                                    <td>
                                        <input type="text">
                                        <i class="dw">间</i>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        周末可订房数
                                    </td>
                                    <td>
                                        <input type="text">
                                        <i class="dw">间</i>
                                    </td>
                                    <td>
                                        <input type="text">
                                        <i class="dw">间</i>
                                    </td>
                                    <td>
                                        <input type="text">
                                        <i class="dw">间</i>
                                    </td>
                                    <td>
                                        <input type="text">
                                        <i class="dw">间</i>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <a class="button buttonActive">保存</a> </li>
                <%--按月设置--%>
                <li>
                    <div class="chkopt">
                        <div class="radio">
                            <em>售房规则</em><b>无早</b><b class="active">有早</b>
                        </div>
                        <div class="radio">
                            <em>会员等级</em><b class="active">微会员</b><b>银卡会员</b><b>金卡会员</b><b>钻石会员</b>
                        </div>
                        <div class="checkbox">
                            <em>显示分类</em><b class="active">房价</b><b>需分</b><b>可定</b>
                        </div>
                    </div>
                    <div class="huste zhou">
                        <table style="width: 1140px;">
                            <thead>
                                <tr>
                                    <th>
                                        一
                                    </th>
                                    <th>
                                        二
                                    </th>
                                    <th>
                                        三
                                    </th>
                                    <th>
                                        四
                                    </th>
                                    <th>
                                        五
                                    </th>
                                    <th>
                                        六
                                    </th>
                                    <th>
                                        日
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>
                                        <div class="optn optn01">
                                            26
                                        </div>
                                    </td>
                                    <td>
                                        <div class="optn optn01">
                                            27
                                        </div>
                                    </td>
                                    <td>
                                        <div class="optn optn01">
                                            28
                                        </div>
                                    </td>
                                    <td>
                                        <div class="optn optn01">
                                            29
                                        </div>
                                    </td>
                                    <td>
                                        <div class="optn optn01">
                                            30
                                        </div>
                                    </td>
                                    <td>
                                        <div class="optn optn01">
                                            31
                                        </div>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>活动价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10</small></span>
                                        </div>
                                        <div class="optn optn01">
                                            1
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <div class="optn optn01">
                                            2
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <div class="optn optn01">
                                            3
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <div class="optn optn01">
                                            4
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <div class="optn optn01">
                                            5
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <div class="optn optn01">
                                            6
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <div class="optn optn01">
                                            7
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <div class="optn optn01">
                                            8
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <div class="optn optn01">
                                            9
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <div class="optn optn01">
                                            10
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <div class="optn optn01">
                                            11
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <div class="optn optn01">
                                            12
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <div class="optn optn01">
                                            13
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <div class="optn optn01">
                                            14
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <div class="optn optn01">
                                            15
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <div class="optn optn01">
                                            16
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <div class="optn optn01">
                                            17
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <div class="optn optn01">
                                            18
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <div class="optn optn01">
                                            19
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td class="today">
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>723800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10</small></span>
                                        </div>
                                        <div class="optn optn01">
                                            20
                                        </div>
                                        <i class="icon-kg1 kgClose"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>活动价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10</small></span>
                                        </div>
                                        <div class="optn optn01">
                                            21
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <div class="optn optn01">
                                            22
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <div class="optn optn01">
                                            23
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <div class="optn optn01">
                                            24
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <div class="optn optn01">
                                            25
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <div class="optn optn01">
                                            26
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <div class="optn optn01">
                                            27
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <div class="optn optn01">
                                            28
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <div class="optn optn01">
                                            29
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <div class="optn optn01">
                                            30
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="optn optn01">
                                            1
                                        </div>
                                    </td>
                                    <td>
                                        <div class="optn optn01">
                                            2
                                        </div>
                                    </td>
                                    <td>
                                        <div class="optn optn01">
                                            3
                                        </div>
                                    </td>
                                    <td>
                                        <div class="optn optn01">
                                            4
                                        </div>
                                    </td>
                                    <td>
                                        <div class="optn optn01">
                                            5
                                        </div>
                                    </td>
                                    <td>
                                        <div class="optn optn01">
                                            6
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </li>
                <%--按周设置--%>
                <li>
                    <div class="chkopt">
                        <div class="radio">
                            <em>售房规则</em><b>无早</b><b class="active">有早</b>
                        </div>
                        <div class="radio">
                            <em>会员等级</em><b class="active">微会员</b><b>银卡会员</b><b>金卡会员</b><b>钻石会员</b>
                        </div>
                        <div class="checkbox">
                            <em>显示分类</em><b class="active">房价</b><b>积分</b><b>房数</b>
                        </div>
                    </div>
                    <div class="huste zhou">
                        <table style="width: 1140px;">
                            <thead>
                                <tr>
                                    <th style="width: 80px;">
                                        房型
                                    </th>
                                    <th style="width: 80px;">
                                        规则名称
                                    </th>
                                    <th>
                                        一
                                    </th>
                                    <th>
                                        二
                                    </th>
                                    <th>
                                        三
                                    </th>
                                    <th>
                                        四
                                    </th>
                                    <th>
                                        五
                                    </th>
                                    <th>
                                        六
                                    </th>
                                    <th>
                                        日
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td rowspan="3">
                                        高级标间
                                    </td>
                                    <td>
                                        无早
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>723800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10</small></span>
                                        </div>
                                        <i class="icon-kg1 kgClose"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>活动价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10</small></span>
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        有早
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        有早
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2">
                                        高级单间
                                    </td>
                                    <td>
                                        无早
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td class="today">
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>723800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10</small></span>
                                        </div>
                                        <i class="icon-kg1 kgClose"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>活动价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10</small></span>
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        有早
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                    <td>
                                        <div class="currInfo">
                                            <%-- --%><span><small>平日价</small><small>138</small></span>
                                            <%-- --%><span><small>需积分</small><small>443800</small></span>
                                            <%-- --%><span><small>可订房</small><small>10/18</small></span>
                                        </div>
                                        <div class="optn">
                                            <i class="dan">单</i><i class="yu">预</i><i class="xian">现</i>
                                        </div>
                                        <i class="icon-kg1"></i>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </li>
            </ul>
        </div>
    </div>
    </form>
    <script type="text/javascript">
        $('.gmkfNav').on('click', 'b', function () {
            $(this).siblings('ul').slideToggle(120);
            $(this).parents('dd').toggleClass('down');
        });
        $(".gmkfNav").panel({ iWheelStep: 80 });


        //右边设置tab切换
        $('.wdyhd .l').on('click', 'a', function () {
            if ($(this).hasClass('active')) return;
            $(this).addClass('active').siblings().removeClass('active');

            var index = $(this).index();
            $('.wdySetup li').eq(index).show().siblings().hide();


            index == 1 ? $('.R2').hide() : $('.R2').show();
            index == 2 ? $('.R3').hide() : $('.R3').show();
            index < 1 ? $('.setupbtn, .R2, .R3').hide() : $('.setupbtn').show();
        });


        $('.radio').on('click', 'b', function () {
            $(this).addClass('active').siblings().removeClass('active');
        });
        $('.checkbox').on('click', 'b', function () {
            $(this).hasClass('active') ? $(this).removeClass('active') : $(this).addClass('active');
        });

        //默认房型价格设置
        function DefaultRoomPrice() {
            $.ajax({
                url: "../Ajax/SysAjax.ashx",
                data: {
                    Menu: "DefaultRoomPrice",
                    adminhotelid: $("#Hdhoteladmin").val()
                },
                type: "GET",
                datatype: "JSON",
                async: false,
                success: function (data) {
                    if (data == "") {
                        return;
                    }
                    $("#tbhtml").html(data);
                }
            })
        }
        DefaultRoomPrice();
        $('#tbhtml').on('keypress', 'input', function (e) {
            if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                return false;
            }
        });
    </script>
</body>
</html>
