<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cwhz.aspx.cs" Inherits="RM.Web.SysSetBase.finance.cwhz" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>财务汇总</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
</head>
<body  style="overflow:auto;">
    <form id="form1" runat="server">
    <div class="tools_bar btnbartitle btnbartitlenr" style="display: block;">
        <div>
            当前位置：<span><a title="系统首页" href="/Frame/HomeIndex.aspx">系统首页</a></span>
            <span> &gt;</span>
            <span>财务管理</span>
            <span> &gt; </span>
            <span>财务汇总</span>
        </div>
    </div>
    <div class="shareframe">
        <div class="shareframel">   
            <dl class="addsearch">
                <dd>
                    <small>酒店名称</small>
                    <div>
                        <select>
                            <option value="value">美思柏丽酒店（东门店）</option>
                        </select>
                    </div>
                </dd>
                <dd style="float:left;width:55%;">
                    <small>年份</small>
                    <div>
                        <select>
                            <option value="value">2018年</option>
                        </select>
                    </div>
                </dd>
                <dd style="float:left;width:45%;">
                    <small style="width:40px;">月份</small>
                    <div style="width: calc(100% - 58px);">
                        <select>
                            <option value="value">2月</option>
                        </select>
                    </div>
                </dd>
                <div class="sharesearchbtn">
                    <input type="submit" name="btnSumit" value="查询">
                </div>
            </dl>
            
            
        </div>
        <div class="zhankai">
        </div>
        <div class="shareframer">
            
            <div class="hzbb">
                    湖北宾馆财务汇总
                <p style="font-size:18px;color:#666;margin-top:10px;">
                    2017年12月
                </p>
                </div>
            <div class="bbgl">
                <div class="d2 clearfix">
                    <div class="d21">
                        营销数据对账单
                    </div>
                    <div class="d22">
                       <span class="dc">导出</span>
                    </div>
                </div>
                <div class="rge">
                    <div class="rgetable">
                        <table class="ul">
                            <thead>
                                <tr>
                                    <th width="40">
                                        序号
                                    </th>
                                    <th>
                                        酒店名称
                                    </th>
                                    <th>
                                        订单数量
                                    </th>
                                    <th>
                                        销售间夜
                                    </th>
                                    <th>
                                        销售金额
                                    </th>
                                    <th>
                                        注册会员
                                    </th>
                                    <th>
                                        营销费充值
                                    </th>
                                    <th>
                                        员工奖金
                                    </th>
                                    <th>
                                        维护费比例
                                    </th>
                                    <th>
                                        维护费
                                    </th>
                                    <th>
                                        维护费合计
                                    </th>
                                    <th>
                                        营销费余额
                                    </th>
                                </tr>
                            </thead>
                            <tbody id="tb">
                                <tr>
                                    <td>
                                        1
                                    </td>
                                    <td>
                                        湖北宾馆
                                    </td>
                                    <td>
                                        200
                                    </td>
                                    <td>
                                        236
                                    </td>
                                    <td>
                                        10000.00
                                    </td>
                                    <td>
                                        1000
                                    </td>
                                    <td>
                                        10000.00
                                    </td>
                                    <td>
                                        10000.00
                                    </td>
                                    <td>
                                        2.2%
                                    </td>
                                    <td>
                                        10000.00 
                                    </td>
                                    <td>
                                        10000.00 
                                    </td>
                                    <td>
                                        10000.00 
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <div class="bbgl">
                <div class="d2 clearfix">
                    <div class="d21">
                        员工奖金统计
                    </div>
                    <div class="d22">
                        <span class="dc">导出</span>
                    </div>
                </div>
                <div class="rge">
                    <div class="rgetable">
                        <table class="ul">
                            <thead>
                                <tr>
                                    <th width="40">
                                        序号
                                    </th>
                                    <th>
                                        员工姓名
                                    </th>
                                    <th>
                                        获得奖金
                                    </th>
                                    <th>
                                        已提奖金
                                    </th>
                                    <th>
                                        余额
                                    </th>
                                </tr>
                            </thead>
                            <tbody id="Tbody1">
                                <tr>
                                    <td>
                                        1
                                    </td>
                                    <td>
                                        林经理
                                    </td>
                                    <td>
                                        263.00 
                                    </td>
                                    <td>
                                        263.00 
                                    </td>
                                    <td>
                                        0
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        2
                                    </td>
                                    <td>
                                        林经理
                                    </td>
                                    <td>
                                        263.00 
                                    </td>
                                    <td>
                                        263.00 
                                    </td>
                                    <td>
                                        0
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        3
                                    </td>
                                    <td>
                                        林经理
                                    </td>
                                    <td>
                                        263.00 
                                    </td>
                                    <td>
                                        263.00 
                                    </td>
                                    <td>
                                        0
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        4
                                    </td>
                                    <td>
                                        林经理
                                    </td>
                                    <td>
                                        263.00 
                                    </td>
                                    <td>
                                        263.00 
                                    </td>
                                    <td>
                                        0
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        5
                                    </td>
                                    <td style="font-weight:bold;">
                                        总计
                                    </td>
                                    <td style="font-weight:bold;">
                                        263.00 
                                    </td>
                                    <td style="font-weight:bold;">
                                        263.00  
                                    </td>
                                    <td style="font-weight:bold;">
                                        0 
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>

            </div>
      
        </div>
    </div>
    </form>
    
    <script src="/SysSetBase/css/jquery.js" type="text/javascript"></script>
    <script type="text/javascript">
        $('.radio').on('click', 'label', function () {
            $(this).addClass('checked').siblings().removeClass('checked');
        });

        //        展开收起
        $('.zhankai').on('click', function () {
            $(this).hasClass('act') ? $(this).removeClass('act') : $(this).addClass('act');
            $(".shareframel").toggle();
        });

 
    </script>
</body>
</html>
