<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="newroomstate01.aspx.cs" Inherits="RM.Web.SysSetBase.houseState.newroomstate01" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
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
        <div class="gmkfStion noStrong">
            <div class="wdyhd" style="padding-left: 20px; max-width: 1010px;">
                <div style="float: left;">
                    <div class="evRgt R3">
                        <select id="selYear">
                            <option value="2017">2017</option>
                            <option selected="selected" value="2018">2018</option>
                            <option value="2019">2019</option>
                        </select>
                        <i id="iLastMonth" title="上一月" class="icon-left"></i>
                        <select name="selMonth" id="selMonth">
                            <option value="1">1月</option>
                            <option value="2">2月</option>
                            <option selected="selected" value="3">3月</option>
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
                        <i id="iNextMonth" title="下一月" class="icon-right"></i>
                    </div>
                </div>
                <div class="r setupbtn">
                    <span><a href="###">房价模板管理</a></span><span><a href="###">房价模板应用</a></span><span><a
                        href="###">批量设置房态</a></span>
                </div>
            </div>
            <ul class="wdySetup">
                <li style="display: list-item;">
                    <div class="daytable newdaytable">
                        <table style="width: 1010px;">
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
                            <tbody id="Tbody1">
                                <tr>
                                    <td class="calendar guo">
                                        <b class="date">25</b>
                                        <div class="d1">平日价模板A</div>
                                        <span class="xt">
                                            -20
                                        </span>
                                    </td>
                                    <td class="calendar guo">
                                        <b class="date">26</b>
                                        <div class="d1">平日价模板A</div>
                                    </td>
                                    <td class="calendar guo">
                                        <b class="date">27</b>
                                        <div class="d1">平日价模板A</div>
                                    </td>
                                    <td class="calendar guo">
                                        <b class="date">28</b>
                                        <div class="d1">平日价模板A</div>
                                    </td>
                                    <td class="calendar">
                                        <b class="date">1</b>
                                        
                                        <div class="d1">平日价模板A</div>
                                        <span class="st">
                                            +20
                                        </span>
                                    </td>
                                    <td class="calendar">
                                        <b class="date">2</b>
                                        <div class="d1">周末价模板</div>
                                    </td>
                                    <td class="calendar">
                                        <b class="date">3</b>
                                        <div class="d1">周末价模板</div>
                                        <span class="st">
                                            +20
                                        </span>
                                        <span class="qbmf">全部满房</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="calendar">
                                        <b class="date">4</b>
                                        <div class="d1">周末价模板</div>
                                        <span class="st">
                                            +20
                                        </span>
                                    </td>
                                    <td class="calendar">
                                        <b class="date">5</b>
                                        <div class="d1">周末价模板</div>
                                    </td>
                                    <td class="calendar">
                                        <b class="date">6</b>
                                        <div class="d1">周末价模板</div>
                                    </td>
                                    <td class="calendar">
                                        <b class="date">7</b>
                                        <div class="d1">周末价模板</div>
                                        <span class="st">
                                            +20
                                        </span>
                                    </td>
                                    <td class="calendar">
                                        <b class="date">8</b>
                                        <div class="d1">周末价模板</div>
                                        <span class="st">
                                            +20
                                        </span>
                                    </td>
                                    <td class="calendar">
                                        <b class="date">9</b>
                                        <div class="d1">周末价模板</div>
                                        <span class="st">
                                            +20
                                        </span>
                                    </td>
                                    <td class="calendar">
                                        <b class="date">10</b>
                                        <div class="d1">周末价模板</div>
                                        <span class="st">
                                            +20
                                        </span>
                                        <span class="bfmf">部分满房</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="calendar">
                                        <b class="date">11</b>
                                        <div class="d1">周末价模板</div>
                                        <span class="st">
                                            +20
                                        </span>
                                    </td>
                                    <td class="calendar">
                                        <b class="date">12</b>
                                        <div class="d1">周末价模板</div>
                                        <span class="st">
                                            +20
                                        </span>
                                    </td>
                                    <td class="calendar">
                                        <b class="date">13</b>
                                        <div class="d1">周末价模板</div>
                                        <span class="st">
                                            +20
                                        </span>
                                    </td>
                                    <td class="calendar">
                                        <b class="date">14</b>
                                        <div class="d1">周末价模板</div>
                                        <span class="st">
                                            +20
                                        </span>
                                    </td>
                                    <td class="calendar">
                                        <b class="date">15</b>
                                        <div class="d1">周末价模板</div>
                                        <span class="st">
                                            +20
                                        </span>
                                    </td>
                                    <td class="calendar">
                                        <b class="date">16</b>
                                        <div class="d1">周末价模板</div>
                                        <span class="st">
                                            +20
                                        </span>
                                    </td>
                                    <td class="calendar">
                                        <b class="date">17</b>
                                        <div class="d1">周末价模板</div>
                                        <span class="st">
                                            +20
                                        </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="calendar">
                                        <b class="date">18</b>
                                        <div class="d1">周末价模板</div>
                                        <span class="st">
                                            +20
                                        </span>
                                    </td>
                                    <td class="calendar">
                                        <b class="date">19</b>
                                        <div class="d1">周末价模板</div>
                                        <span class="st">
                                            +20
                                        </span>
                                    </td>
                                    <td class="calendar today">
                                        <b class="date">20</b>
                                        <div class="d1">周末价模板</div>
                                        <span class="st">
                                            +20
                                        </span>
                                    </td>
                                    <td class="calendar">
                                        <b class="date">21</b>
                                        <div class="d1">周末价模板</div>
                                        <span class="st">
                                            +20
                                        </span>
                                    </td>
                                    <td class="calendar">
                                        <b class="date">22</b>
                                        <div class="d1">周末价模板</div>
                                        <span class="st">
                                            +20
                                        </span>
                                    </td>
                                    <td class="calendar">
                                        <b class="date">23</b>
                                        <div class="d1">周末价模板</div>
                                        <span class="st">
                                            +20
                                        </span>
                                    </td>
                                    <td class="calendar">
                                        <b class="date">24</b>
                                        <div class="d1">周末价模板</div>
                                        <span class="st">
                                            +20
                                        </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="calendar">
                                        <b class="date">25</b>
                                        <div class="d1">周末价模板</div>
                                        <span class="st">
                                            +20
                                        </span>
                                    </td>
                                    <td class="calendar">
                                        <b class="date">26</b>
                                        <div class="d1">周末价模板</div>
                                        <span class="st">
                                            +20
                                        </span>
                                    </td>
                                    <td class="calendar">
                                        <b class="date">27</b>
                                        <div class="d1">周末价模板</div>
                                        <span class="st">
                                            +20
                                        </span>
                                    </td>
                                    <td class="calendar">
                                        <b class="date">28</b>
                                        <div class="d1">周末价模板</div>
                                        <span class="st">
                                            +20
                                        </span>
                                    </td>
                                    <td class="calendar">
                                        <b class="date">29</b>
                                        <div class="d1">周末价模板</div>
                                        <span class="st">
                                            +20
                                        </span>
                                    </td>
                                    <td class="calendar">
                                        <b class="date">30</b>
                                        <div class="d1">周末价模板</div>
                                        <span class="st">
                                            +20
                                        </span>
                                    </td>
                                    <td class="calendar">
                                        <b class="date">31</b>
                                        <div class="d1">周末价模板</div>
                                        <span class="st">
                                            +20
                                        </span>
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
</body>
</html>
