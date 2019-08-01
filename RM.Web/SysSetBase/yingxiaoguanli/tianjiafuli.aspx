<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="tianjiafuli.aspx.cs" Inherits="RM.Web.SysSetBase.yingxiaoguanli.tianjiafuli" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml"  style="overflow:auto;">
<head runat="server">
    <title>添加福利</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <dl class="addevaluate tianjiafuli">
        <dd>
            <small>福利类型</small>
            <div>
                <select>
                    <option value="value">现金红包</option>
                </select>
            </div>
        </dd>
        <dd>
            <small>福利名称</small>
            <div>
                <input type="text" name="name" value="现金红包" />
            </div>
        </dd>
        <dd class="cdnr">
            <small>红包类型</small>
            <div class="radio">
                <label class="checked" value='1' >固定</label>
                <label value='0'>随机</label>
                <span class="sharedate">
                    <input name="txtStartTime" type="text" id="txtStartTime" onfocus="WdatePicker()">
                    <input name="txtEndTime" type="text" id="txtEndTime" onfocus="WdatePicker()">
                </span>
                <span>元<i>*最少1元</i></span>
            </div>
        </dd>
        <dd class="cdnr">
            <small>说明</small>
            <div>
                <textarea>content</textarea>
            </div>
        </dd>
    </dl>
    <dl class="addevaluate tianjiafuli" style="display:none">
        <dd>
            <small>福利类型</small>
            <div>
                <select>
                    <option value="value">卡券</option>
                </select>
            </div>
        </dd>
        <dd>
            <small>卡券</small>
            <div class="checkbox">
                <label class="checked">10元卡券</label>
                <label value='0'>首次注册卡券</label>
                <label value='0'>支付卡券</label>
                <label value='0'>评价卡券</label>
                <label value='0'>10卡券</label>
            </div>
        </dd>
        <dd>
            <div>
                <a class="btn" style="margin-left:105px;">上传</a>
            </div>
        </dd>
        <dd>
            <small>说明</small>
            <div>
                <textarea>content</textarea>
            </div>
        </dd>
    </dl>
    <dl class="addevaluate tianjiafuli" style="display:none">
        <dd>
            <small>福利类型</small>
            <div>
                <select>
                    <option value="value">小礼品</option>
                </select>
            </div>
        </dd>
        <dd>
            <small>福利名称</small>
            <div>
                <input type="text" name="name" value="矿泉水（两瓶）" />
            </div>
        </dd>
        <dd>
            <small>上传</small>
            <div>
                <a class="btn">上传</a>
            </div>
        </dd>
        <dd>
            <div>
                <img src="../img/ewm.png" alt="Alternate Text" width="84" height="84"  style="margin-left:105px;border:1px solid #d9dada;border-radius:5px;"/>
            </div>
        </dd>
    </dl>
    <dl class="addevaluate tianjiafuli" style="display:none">
        <dd>
            <small>福利类型</small>
            <div>
                <select>
                    <option value="value">积分</option>
                </select>
            </div>
        </dd>
        <dd>
            <small>福利名称</small>
            <div>
                <input type="text" name="name" value="矿泉水（两瓶）" />
            </div>
        </dd>
        <dd>
            <small>积分</small>
            <div>
                <input type="text" name="name" value="" />
            </div>
        </dd>
        <dd>
            <small>说明</small>
            <div>
                <textarea>content</textarea>
            </div>
        </dd>
    </dl>
    </form>
</body>
</html>
