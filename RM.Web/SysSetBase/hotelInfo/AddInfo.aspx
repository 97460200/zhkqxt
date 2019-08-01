<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddInfo.aspx.cs" Inherits="RM.Web.SysSetBase.hotelInfo.AddInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>酒店信息</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer" />
    <script src="../../WDatePicker/WdatePicker.js" type="text/javascript"></script>
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/SysSetBase/css/IE.js" type="text/javascript"></script>
    <script src="/SysSetBase/css/jquery.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="clearfix">
        
        <dl class="adifoli addjdxx">
            <dd>
                <small>酒店名称</small>
                <div>
                    <input type="text" />
                </div>
            </dd>
            <dd>
                <small>酒店logo</small>
                <div>
                    <div style="width:156px;float:left;margin-right:20px;">
                        <a class="btn">上传</a> <i class="jy">建议图片上传尺寸为400*400</i>
                    </div>
                    
                    <i class="photo" style="float:left;">
                        <img src="/App_Themes/default/images/logo10.png" alt="photo" /></i>
                </div>
            </dd>
            <dd>
                <small>标签</small>
                <div class="checkbox">
                    <label class="checked">
                        全选</label><label>无线网</label><label>淋浴</label><label>停车场</label><label>餐厅</label>
                </div>
            </dd>
            <dd>
                <small>电话</small>
                <div>
                    <input type="text" />
                </div>
            </dd>
            <dd>
                <small>地址</small>
                <div>
                    <input type="text" />
                </div>
            </dd>
            <dd>
                <small>地址经纬度</small>
                <div>
                    <input type="text" style="width:269px;"/><a class="btn">自动生成</a>
                </div>
            </dd>
            <dd>
                <small>星级</small>
                <div class="radio">
                    <label class="checked">
                        5星/豪华</label><label>准5星/豪华</label><label>4星/高档</label><label>准4星/高档</label><label>3星/舒适</label><label>准3星/舒适</label><label>2星及以下/经济型</label>
                </div>
            </dd>
            <dd>
                <small>开业年份</small>
                <div>
                    <input type="text" />
                </div>
            </dd>
            <dd>
                <small>最近装修</small>
                <div>
                    <input type="text" />
                </div>
            </dd>
            <dd>
                <small>客房总数（间）</small>
                <div>
                    <input type="text" />
                </div>
            </dd>
            <dd>
                <small>最低价</small>
                <div>
                    <input type="text" />
                </div>
            </dd>
        </dl>
        
        <dl class="adifoli addjdxx">
            <dd>
                <small>简介</small>
                <div>
                    <textarea cols="30" rows="10"></textarea>
                </div>
            </dd>
            <dd>
                <small>服务信息</small>
                <div>
                    <textarea cols="30" rows="10"></textarea>
                </div>
            </dd>
            <dd>
                <small>预订说明</small>
                <div>
                    <textarea cols="30" rows="10"></textarea>
                </div>
            </dd>
            <dd>
                <small>周边信息</small><%--
                <div>
                    <div class="zbxx">
                        <select>
                        <option value="">选择类型</option>
                        </select>
                        <div class="d1">
                        <input type="text" value="[娱乐]海上换了世界5000米步行200分钟" />
                        </div>
                        <a class="btn" >删除</a>
                    </div>
                     <a class="btns">继续添加</a>
                        <p class="sl">
                        示例：海上欢乐世界5000米步行200分钟
                        </p>
                </div>--%>
                <div>
                    <div class="sharetabs">
                        <ul class="clearfix">
                            <li class="act"><a href="###">交通</a></li>
                            <li><a href="###">餐饮</a></li>
                            <li><a href="###">购物</a></li>
                            <li><a href="###">娱乐</a></li>
                            <li><a href="###">银行</a></li>
                            <li><a href="###">医疗</a></li>
                        </ul>
                    </div>
                    <div class="zhoubxx">
                        <ul>
                            <li>天虹西乡地铁站旁500米5分钟</li>
                            <li>天虹西乡地铁站旁500米5分钟</li>
                            <li>天虹西乡地铁站旁500米5分钟</li>
                            <li>天虹西乡地铁站旁500米5分钟</li>
                        </ul>
                    </div>
                    <a class="btn">管理</a>
                </div>
            </dd>
            <dd>
                <small>酒店状态</small>
                <div class="radio">
                    <label class="checked">
                        正常营业</label><label>筹备中</label>
                </div>
            </dd>
        </dl>
        
    </div>
    <%--<div class="adifoliBtn">
        <input type="submit" name="btnSumit" value="提交">
    </div>--%>
    <div class="sharebottombtn">
        <div class="fr">
            <input type="submit" name="btnSumit" value="提交">
            <input type="submit" name="btnSumit" value="重置">
        </div>
    </div>
    </form>
    <script type="text/javascript">
        $('.radio').on('click', 'label', function () {
            $(this).addClass('checked').siblings().removeClass('checked');
        });
        $('.checkbox').on('click', 'label', function () {
            $(this).hasClass('checked') ? $(this).removeClass('checked') : $(this).addClass('checked');
        });
    </script>
</body>
</html>
