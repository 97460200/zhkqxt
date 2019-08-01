<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="hyqxsz.aspx.cs" Inherits="RM.Web.SysSetBase.xitongcanshu.hyqxsz" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="memr hyqxsz">
        <ul class="memrList">
            
            <li>
                <div class="qytable" style="width: 700px;">
                    <table class="ul" id="tab1">
                        <tbody>
                            <tr>
                                <th>
                                    权益
                                </th>
                                <th width="200">
                                    说明
                                </th>
                                <th class="hylxname">
                                    微会员
                                </th>
                                <th class="hylxname">
                                    银卡会员
                                </th>
                                <th class="hylxname">
                                    金卡会员
                                </th>
                                <th class="hylxname">
                                    砖石会员
                                </th>
                                <th class="hylxname"  width="160">
                                    操作
                                </th>
                            </tr>
                            <tr>
                                <td>
                                    <input type="text" name="name" value="积分倍数"  style="width:80px;"/>
                                </td>
                                <td>
                                    <input type="text" name="name" value="" placeholder="例：1.5倍"/>
                                </td>
                                <td class="active">
                                    <i class="icon-radio6"></i>
                                </td>
                                <td class="active">
                                    <i class="icon-radio6"></i>
                                </td>
                                <td class="active">
                                    <i class="icon-radio6"></i>
                                </td>
                                <td class="active">
                                    <i class="icon-radio6"></i>
                                </td>
                                <td>
                                    <a href="#" class="btn">删除</a>
                                    <a href="#" class="btn">继续添加</a>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="text" name="name" value="延迟退房" style="width:80px;"/>
                                </td>
                                <td>
                                    <input type="text" name="name" value="" placeholder="例：14:00"/>
                                </td>
                                <td class="active">
                                    <i class="icon-radio6"></i>
                                </td>
                                <td class="active">
                                    <i class="icon-radio6"></i>
                                </td>
                                <td class="active">
                                    <i class="icon-radio6"></i>
                                </td>
                                <td class="active">
                                    <i class="icon-radio6"></i>
                                </td>
                                <td>
                                    <a href="#" class="btn">删除</a>
                                    <a href="#" class="btn">继续添加</a>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="text" name="name" value="免费升级" style="width:80px;"/>
                                </td>
                                <td>
                                    <input type="text" name="name" value="" placeholder="例：积分达到10000"/>
                                </td>
                                <td class="active">
                                    <i class="icon-radio6"></i>
                                </td>
                                <td class="active">
                                    <i class="icon-radio6"></i>
                                </td>
                                <td class="active">
                                    <i class="icon-radio6"></i>
                                </td>
                                <td class="active">
                                    <i class="icon-radio6"></i>
                                </td>
                                <td>
                                    <a href="#" class="btn">删除</a>
                                    <a href="#" class="btn">继续添加</a>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="text" name="name" value="忠诚计划" style="width:80px;"/>
                                </td>
                                <td>
                                    <input type="text" name="name" value="" placeholder="例：住10晚送1晚"/>
                                </td>
                                <td class="active">
                                    <i class="icon-radio6"></i>
                                </td>
                                <td class="active">
                                    <i class="icon-radio6"></i>
                                </td>
                                <td class="active">
                                    <i class="icon-radio6"></i>
                                </td>
                                <td class="active">
                                    <i class="icon-radio6"></i>
                                </td>
                                <td>
                                    <a href="#" class="btn">删除</a>
                                    <a href="#" class="btn">继续添加</a>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="text" name="name" value="网络设施" style="width:80px;"/>
                                </td>
                                <td>
                                    <input type="text" name="name" value="" placeholder="例：有线宽带"/>
                                </td>
                                <td class="active">
                                    <i class="icon-radio6"></i>
                                </td>
                                <td class="active">
                                    <i class="icon-radio6"></i>
                                </td>
                                <td class="active">
                                    <i class="icon-radio6"></i>
                                </td>
                                <td class="active">
                                    <i class="icon-radio6"></i>
                                </td>
                                <td>
                                    <a href="#" class="btn">删除</a>
                                    <a href="#" class="btn">继续添加</a>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="text" name="name" value="停车服务" style="width:80px;"/>
                                </td>
                                <td>
                                    <input type="text" name="name" value="" placeholder="例：免费"/>
                                </td>
                                <td class="active">
                                    <i class="icon-radio6"></i>
                                </td>
                                <td class="active">
                                    <i class="icon-radio6"></i>
                                </td>
                                <td class="active">
                                    <i class="icon-radio6"></i>
                                </td>
                                <td class="active">
                                    <i class="icon-radio6"></i>
                                </td>
                                <td>
                                    <a href="#" class="btn">删除</a>
                                    <a href="#" class="btn">继续添加</a>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="text" name="name" value="WiFi账号"  style="width:80px;"/>
                                </td>
                                <td>
                                    <input type="text" name="name" value="" placeholder="例：酒店内WiFi账号"/>
                                </td>
                                <td class="active">
                                    <i class="icon-radio6"></i>
                                </td>
                                <td class="active">
                                    <i class="icon-radio6"></i>
                                </td>
                                <td class="active">
                                    <i class="icon-radio6"></i>
                                </td>
                                <td class="active">
                                    <i class="icon-radio6"></i>
                                </td>
                                <td>
                                    <a href="#" class="btn">删除</a>
                                    <a href="#" class="btn">继续添加</a>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="text" name="name" value="WiFi密码" style="width:80px;"/>
                                </td>
                                <td>
                                    <input type="text" name="name" value="" placeholder="例：酒店内WiFi密码"/>
                                </td>
                                <td class="active">
                                    <i class="icon-radio6"></i>
                                </td>
                                <td class="active">
                                    <i class="icon-radio6"></i>
                                </td>
                                <td class="active">
                                    <i class="icon-radio6"></i>
                                </td>
                                <td class="active">
                                    <i class="icon-radio6"></i>
                                </td>
                                <td>
                                    <a href="#" class="btn">删除</a>
                                    <a href="#" class="btn">继续添加</a>
                                </td>
                            </tr>
                        </tbody>
                    </table>

                     <dl class="addevaluate hyqyjj">
                        <dd>
                            <small>	会员权益简介</small>
                            <div>
                                <textarea>content</textarea>
                            </div>
                        </dd>
                    </dl>
                    <div class="membtn">
                        <a class="button buttonActive" onclick="Submit()">保存</a>
                    </div>
                </div>
            </li>
            
        </ul>
    </div>
    </form>
</body>
</html>
