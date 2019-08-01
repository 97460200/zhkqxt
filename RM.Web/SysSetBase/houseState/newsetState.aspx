<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="newsetState.aspx.cs" Inherits="RM.Web.SysSetBase.houseState.newsetState" %>

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
    <div class="gtall gmkf newsetState clearfix">
        <div class="gmkfStion" style="padding-left: 20px; overflow: auto;">
            <div class="plsz">
                <i class="text">当前日期</i>
                <div class="much">
                    2018-03-19 星期一
                </div>
            </div>
            <div class="plsz l">
                <i class="text">房价模板</i>
                <div class="" style="width: 234px; float: left;">
                    <select>
                        <option value="">平日价模板A</option>
                    </select>
                </div>
            </div>
            <div class="plsz l" style="margin-left: 50px;">
                <i class="text">价格调整</i>
                <div id="RoomState" class="radio">
                    <label rs="1" class="checked">
                        上调</label>
                    <label rs="0">
                        下调</label>
                </div>
            </div>
            <div class="plsz">
                <input type="text" name="name" value="" style="width: 50px; margin-right: 5px;" />元
            </div>
            <div class="rul pq-cont">
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
                                增加可订房数
                            </th>
                            <th>
                                价格是否改变
                            </th>
                            <th>
                                房态
                            </th>
                        </tr>
                    </thead>
                    <tbody id="rulelist">
                        <tr>
                            <td rowspan="2">
                                标准单人房
                            </td>
                            <td>
                                无早
                            </td>
                            <td>
                                <span>￥200</span>
                                <span class="xt">-20</span>
                            </td>
                            <td>
                                <span>￥200</span>
                                <span class="xt">-20</span>
                            </td>
                            <td>
                                <span>￥200</span>
                                <span class="xt">-20</span>
                            </td>
                            <td>
                                <span>￥200</span>
                                <span class="xt">-20</span>
                            </td>
                            <td>
                                10/30
                            </td>
                            <td>
                                <input type="text" name="name" value=" ">间
                            </td>
                            <td>
                                是
                            </td>
                            <td>
                                <div class="kqgb">
                                    <label class="icon-kg1 guan"></label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                有早
                            </td>
                            <td>
                                <span>￥200</span>
                                <span class="xt">-20</span>
                            </td>
                            <td>
                                <span>￥200</span>
                                <span class="xt">-20</span>
                            </td>
                            <td>
                                <span>￥200</span>
                                <span class="xt">-20</span>
                            </td>
                            <td>
                                <span>￥200</span>
                                <span class="xt">-20</span>
                            </td>
                            <td>
                                满房
                            </td>
                            <td>
                                <input type="text" name="name" value=" ">间
                            </td>
                            <td>
                                是
                            </td>
                            <td>
                                <div class="kqgb">
                                    <label class="icon-kg1 guan"></label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="2">
                                标准双人房
                            </td>
                            <td>
                                无早
                            </td>
                            <td>
                                <span>￥200</span>
                                <span class="st">+20</span>
                            </td>
                            <td>
                                <span>￥200</span>
                                <span class="st">+20</span>
                            </td>
                            <td>
                                <span>￥200</span>
                                <span class="st">+20</span>
                            </td>
                            <td>
                                <span>￥200</span>
                                <span class="st">+20</span>
                            </td>
                            <td>
                                10/30
                            </td>
                            <td>
                                <input type="text" name="name" value=" ">间
                            </td>
                            <td>
                                是
                            </td>
                            <td>
                                <div class="kqgb">
                                    <label class="icon-kg1 kai"></label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                有早
                            </td>
                            <td>
                                <span>￥200</span>
                                <span class="st">+20</span>
                            </td>
                            <td>
                                <span>￥200</span>
                                <span class="st">+20</span>
                            </td>
                            <td>
                                <span>￥200</span>
                                <span class="st">+20</span>
                            </td>
                            <td>
                                <span>￥200</span>
                                <span class="st">+20</span>
                            </td>
                            <td>
                                满房
                            </td>
                            <td>
                                <input type="text" name="name" value=" ">间
                            </td>
                            <td>
                                是
                            </td>
                            <td>
                                <div class="kqgb">
                                    <label class="icon-kg1 kai"></label>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
