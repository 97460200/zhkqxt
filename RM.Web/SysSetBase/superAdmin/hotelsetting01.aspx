<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="hotelsetting01.aspx.cs" Inherits="RM.Web.SysSetBase.superAdmin.hotelsetting01" %>

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
</head>
<body>
    <form id="form1" runat="server">
    <div class="hotelsetting">
        <div class="hotelname">
            柏丽酒店
        </div>
        <div class="sharetabs">
            <ul class="clearfix">
                <li><a href="###">营销设置</a> </li>
                <li class="act"><a href=="###">微信设置</a> </li>
                <li><a href=="###">短信设置</a> </li>
                <li><a href=="###">功能设置</a> </li>
                <li><a href=="###">系统对接</a> </li>
            </ul>
        </div>

        <div class="sharetabs01">
            <a class="active">微信绑定设置</a><a>微信模板设置</a><a>微信授权文件</a><a>公众号基本信息设置</a>
        </div>

        <dl class="addevaluate weixinshezhi" style="display:none;">
            <div class="d1">
                设置微网站绑定微信公众号相关配置
            </div>
            <dd>
                <small>原始ID</small>
                <div>
                    <input type="text" name="name" value=" " /><span class="bz">公众号注册信息的原始ID</span>
                </div>
            </dd>
            <dd>
                <small>APPID</small>
                <div>
                    <input type="text" name="name" value=" " /><span class="bz">绑定支付的APPID（必须配置）</span>
                </div>
            </dd>
            <dd>
                <small>APPSECRET</small>
                <div>
                    <input type="text" name="name" value=" " /><span class="bz">公众帐号secert（仅JSAPI支付的时候需要配置）</span>
                </div>
            </dd>
            <dd>
                <small>MCHID</small>
                <div>
                    <input type="text" name="name" value=" " /><span class="bz">商户号（必须配置）</span>
                </div>
            </dd>
            <dd>
                <small>KEY</small>
                <div>
                    <input type="text" name="name" value=" " /><span class="bz">商户支付密钥，参考开户邮件设置（必须配置）</span>
                </div>
            </dd>
            <dd>
                <small>微信公众号账号</small>
                <div>
                    <input type="text" name="name" value=" " />
                </div>
            </dd>
            <dd>
                <small>密码</small>
                <div>
                    <input type="text" name="name" value=" " />
                </div>
            </dd>
            <dd>
                <small>商户平台账号</small>
                <div>
                    <input type="text" name="name" value=" " />
                </div>
            </dd>
            <dd>
                <small>密码</small>
                <div>
                    <input type="text" name="name" value=" " />
                </div>
            </dd>
            <div class="membtn">
                <a class="button buttonActive" onclick="Submit()">保存</a>
            </div>
        </dl>

        
        <dl class="addevaluate weixinshezhi"  style="display:none;">
            <div class="d1">
                设置微信手机接收推送通知，填写微信公众号接口地址
            </div>
            <dd>
                <small>订单提交成功通知</small>
                <div>
                    <input type="text" name="name" value=" " />
                </div>
            </dd>
            <dd>
                <small>订单支付成功提示</small>
                <div>
                    <input type="text" name="name" value=" " />
                </div>
            </dd>
            <dd>
                <small>客满取消</small>
                <div>
                    <input type="text" name="name" value=" " />
                </div>
            </dd>
            <dd>
                <small>房间订单取消通知</small>
                <div>
                    <input type="text" name="name" value=" " />
                </div>
            </dd>
            <dd>
                <small>退款申请成功提醒</small>
                <div>
                    <input type="text" name="name" value=" " />
                </div>
            </dd>
            <dd>
                <small>账单通知</small>
                <div>
                    <input type="text" name="name" value=" " />
                </div>
            </dd>
            <div class="membtn">
                <a class="button buttonActive" onclick="Submit()">保存</a>
            </div>
        </dl>

        <dl class="addevaluate weixinshezhi"  style="display:none;">
            <strong style="margin-top:-10px;"><b>微信公众号授权文件</b></strong>
            <dd>
                <small>上传微信验证文件</small>
                <div>
                    <a href="#" class="tjfj">添加附件</a>
                    <div id="div1" class="addfj clearfix" style="">
                         <div class="addfjl">
                             <img src="/SysSetBase/img/jsb.png">
                         </div>
                         
                         <div class="addfjr">
                             <p>
                                 <span id="name1">QQ图片20171107165844.jpg</span><span onclick="scdiv(this)" class="s1">删除</span>
                             </p>
                             <p class="p1"> 
                                 <span class="s2"><i style="width:37%;"></i></span><span class="s3">37%</span><span class="s4">剩余时间：00:00:07</span>
                             </p>
                         </div>
                    </div>
                </div>
            </dd>
            <strong><b>退款需要上传的文件</b></strong>
            <dd>
                <small>pkcs12格式</small>
                <div>
                    <a href="#" class="tjfj">添加附件</a><span>（apiclient_cert.p12）</span>
                    <div class="addfj clearfix">
                         <div class="addfjl">
                             <img src="/SysSetBase/img/jsb.png">
                         </div>
                         
                         <div class="addfjr">
                             <p>
                                 <span id="name2" runat="server">apiclient_cert.p12</span><span onclick="scdiv(this)" class="s1">删除</span>
                             </p>
                             <p class="p1"> 
                                 <span class="s5">62.5k</span class="bz01"><span class="s6">上传成功</span>
                             </p>
                         </div>
                    </div>
                </div>
            </dd>
            <dd>
                <small>CA证书</small>
                <div>
                    <a href="#" class="tjfj">添加附件</a><span class="bz01">（rootca.pem）</span>
                </div>
            </dd>
            <dd>
                <small>证书</small>
                <div>
                    <a href="#" class="tjfj">添加附件</a><span class="bz01">（apiclient_cert.pem）</span>
                </div>
            </dd>
            <dd>
                <small>证书密钥</small>
                <div>
                    <a href="#" class="tjfj">添加附件</a><span class="bz01">（apiclient_key.pem）</span>
                </div>
            </dd>

            <div class="membtn">
                <a class="button buttonActive" onclick="Submit()">保存</a>
            </div>
        </dl>

        <dl class="addevaluate weixinshezhi">
            <div class="d1">
                设置微信公众号的基本信息
            </div>
            <dd>
                <small>微信名称</small>
                <div>
                    <input type="text" name="name" value="美思柏丽酒店" />
                </div>
            </dd>
            <dd>
                <small>主体信息</small>
                <div>
                    <input type="text" name="name" value="广东美思柏丽酒店有限公司" />
                </div>
            </dd>
            <dd>
                <small>功能介绍</small>
                <div>
                    <textarea>广东美思柏丽酒店有限公司成立于2002年，专注于酒店娱乐行业的服务。产品主要覆盖于酒店、餐饮、娱乐、休闲会所、长租公寓等领域</textarea>
                </div>
            </dd>
            <dd>
                <small>公众号管理员</small>
                <div>
                    <input type="text" name="name" value="广东美思柏丽酒店有限公司" />
                </div>
            </dd>
            <dd>
                <small>认证到期时间</small>
                <div>
                    <div class="sharedate">
                        <input name="txtStartTime" type="text" id="txtStartTime" onfocus="WdatePicker()" value="2018-01-01">
                        <input name="txtEndTime" type="text" id="txtEndTime" onfocus="WdatePicker()" value="2018-02-01">
                    </div>
                </div>
            </dd>
            <dd>
                <small>公众号logo</small>
                <div>
                    <a class="btn">上传</a>
                    <div class="gzhlogo">
                        <img src="/SysSetBase/img/ewm.png" />
                    </div>
                </div>
            </dd>
            <dd>
                <small>公众号二维码</small>
                <div>
                    <a class="btn">上传</a>
                    <div class="gzhewm">
                        <img src="/SysSetBase/img/ewm.png">
                    </div>
                </div>
            </dd>

            <div class="membtn">
                <a class="button buttonActive" onclick="Submit()">保存</a>
            </div>
        </dl>


    
    </div>

    </form>
</body>
</html>
