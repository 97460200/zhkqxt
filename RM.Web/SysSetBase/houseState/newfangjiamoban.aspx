<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="newfangjiamoban.aspx.cs" Inherits="RM.Web.SysSetBase.houseState.newfangjiamoban" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="gtall gmkf clearfix">
        <div id="HotelTree" class="gmkfNav">
            <dl>
                <dd class="down">
                    <b class="Hotel" adminhotelid="1006203" hotelid="44">湖北宾馆</b><ul style="display: block;">
                        <li datatype="Room" adminhotelid="1006203" hotelid="44" roomid="201"><span>测试房</span></li><li
                            datatype="Room" adminhotelid="1006203" hotelid="44" roomid="200"><span>商务套08</span></li><li
                                datatype="Room" adminhotelid="1006203" hotelid="44" roomid="199"><span>商务套06</span></li><li
                                    datatype="Room" adminhotelid="1006203" hotelid="44" roomid="198"><span>白金单/双人房</span></li><li
                                        datatype="Room" adminhotelid="1006203" hotelid="44" roomid="197"><span>温馨单人房</span></li></ul>
                </dd>
            </dl>
        </div>
        <div class="fangjiamoban">
            <div class="fangjiamobant clearfix">
                <span>房价模板</span><span><img src="/SysSetBase/img/mbadd.png" /></span>
            </div>
            <div class="fangjiamobanb">
                <ul>
                    <li class="clearfix">
                        <span>平日价模板A</span><span><img src="/SysSetBase/img/del10.png" /></span>
                    </li>
                    <li class="clearfix">
                        <span>平日价模板A</span><span><img src="/SysSetBase/img/del10.png" /></span>
                    </li>
                    <li class="clearfix">
                        <span>平日价模板A</span><span><img src="/SysSetBase/img/del10.png" /></span>
                    </li>
                    <li class="clearfix">
                        <span>平日价模板A</span><span><img src="/SysSetBase/img/del10.png" /></span>
                    </li>
                </ul>
            </div>
            
        </div>

        <div class="rul pq-cont fangjiamobanbg" style="width:1072px;">
            <div class="wdyhd" style="margin:10px 0;">
                <div class="r">          
                    <span>返回</span>
                </div>
            </div>
            <table>
                <thead>
                    <tr>
                        <th>
                            房型
                        </th>
                        <th>
                            售房规则
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
                        <th>
                            可订房数
                        </th>
                        <th>
                            价格是否改变
                        </th>
                    </tr>
                </thead>
                <tbody id="rulelist">
                    <tr>
                        <td rowspan="2">
                            高级标间
                        </td>
                        <td>
                            无早
                        </td>
                        <td>
                            <input type="text" name="name" value=" " />元
                        </td>
                        <td>
                            <input type="text" name="name" value=" " />元
                        </td>
                        <td>
                            <input type="text" name="name" value=" " />元
                        </td>
                        <td>
                            <input type="text" name="name" value=" " />元
                        </td>
                        <td>
                            <input type="text" name="name" value=" " />间
                        </td>
                        <td>
                            <div class="radio clearfix">
                                <label class="checked" value="1">展开</label>
                                <label value="0">收起</label>
                            </div>

                        </td>
                    </tr>
                    <tr>
                        <td>
                            有早
                        </td>
                        <td>
                            <input type="text" name="name" value=" " />元
                        </td>
                        <td>
                            <input type="text" name="name" value=" " />元
                        </td>
                        <td>
                            <input type="text" name="name" value=" " />元
                        </td>
                        <td>
                            <input type="text" name="name" value=" " />元
                        </td>
                        <td>
                            <input type="text" name="name" value=" " />间
                        </td>
                        <td class="jgsfkb">
                            <div class="radio clearfix">
                                <label class="checked" value="1">展开</label>
                                <label value="0">收起</label>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="1">
                            标准单人房
                        </td>
                        <td>
                            
                        </td>
                        <td>
                            <input type="text" name="name" value=" " />元
                        </td>
                        <td>
                            <input type="text" name="name" value=" " />元
                        </td>
                        <td>
                            <input type="text" name="name" value=" " />元
                        </td>
                        <td>
                            <input type="text" name="name" value=" " />元
                        </td>
                        <td>
                            <input type="text" name="name" value=" " />间
                        </td>
                        <td>
                            <div class="radio clearfix">
                                <label class="checked" value="1">展开</label>
                                <label value="0">收起</label>
                            </div>

                        </td>
                    </tr>
                    <tr>
                        <td rowspan="2">
                            豪华大床房
                        </td>
                        <td>
                            无早
                        </td>
                        <td>
                            <input type="text" name="name" value=" " />元
                        </td>
                        <td>
                            <input type="text" name="name" value=" " />元
                        </td>
                        <td>
                            <input type="text" name="name" value=" " />元
                        </td>
                        <td>
                            <input type="text" name="name" value=" " />元
                        </td>
                        <td>
                            <input type="text" name="name" value=" " />间
                        </td>
                        <td>
                            <div class="radio clearfix">
                                <label class="checked" value="1">展开</label>
                                <label value="0">收起</label>
                            </div>

                        </td>
                    </tr>
                    <tr>
                        <td>
                            有早
                        </td>
                        <td>
                            <input type="text" name="name" value=" " />元
                        </td>
                        <td>
                            <input type="text" name="name" value=" " />元
                        </td>
                        <td>
                            <input type="text" name="name" value=" " />元
                        </td>
                        <td>
                            <input type="text" name="name" value=" " />元
                        </td>
                        <td>
                            <input type="text" name="name" value=" " />间
                        </td>
                        <td class="jgsfkb">
                            <div class="radio clearfix">
                                <label class="checked" value="1">展开</label>
                                <label value="0">收起</label>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="2">
                            高级标间
                        </td>
                        <td>
                            无早
                        </td>
                        <td>
                            <input type="text" name="name" value=" " />元
                        </td>
                        <td>
                            <input type="text" name="name" value=" " />元
                        </td>
                        <td>
                            <input type="text" name="name" value=" " />元
                        </td>
                        <td>
                            <input type="text" name="name" value=" " />元
                        </td>
                        <td>
                            <input type="text" name="name" value=" " />间
                        </td>
                        <td>
                            <div class="radio clearfix">
                                <label class="checked" value="1">展开</label>
                                <label value="0">收起</label>
                            </div>

                        </td>
                    </tr>
                    <tr>
                        <td>
                            有早
                        </td>
                        <td>
                            <input type="text" name="name" value=" " />元
                        </td>
                        <td>
                            <input type="text" name="name" value=" " />元
                        </td>
                        <td>
                            <input type="text" name="name" value=" " />元
                        </td>
                        <td>
                            <input type="text" name="name" value=" " />元
                        </td>
                        <td>
                            <input type="text" name="name" value=" " />间
                        </td>
                        <td class="jgsfkb">
                            <div class="radio clearfix">
                                <label class="checked" value="1">展开</label>
                                <label value="0">收起</label>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <a class="btn" onclick="Submit()" style="margin-top:15px;">保存</a>
    </div>

    <dl class="addevaluate" style="width:395px;height:90px;display:none;">
        <dd>
            <small>模板名称</small>
            <div>
                <input type="text" name="name" value="" style="width:100%;"/>
            </div>
        </dd>
    </dl>

    </form>
</body>
</html>
