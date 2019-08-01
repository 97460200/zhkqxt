<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="addfpgl.aspx.cs" Inherits="RM.Web.SysSetBase.finance.addfpgl" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>发票管理</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer" />
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/SysSetBase/css/jquery.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <dl class="addfpgl">
        <div class="sharehead">
            <span>发票信息</span><span class="line"></span>
        </div>
        <div class="clearfix">
            <dd>
                <small>发票类型</small>
                <div>
                    <select>
                        <option value="value"></option>
                    </select>
                </div>
            </dd>
            <dd>
                <small>发票抬头</small>
                <div>
                    <select>
                        <option value="value"></option>
                    </select>
                </div>
            </dd>
            <dd>
                <small>发票金额(元)</small>
                <div>
                    <input type="text" class="inputban"/>
                    <p>
                        如有疑问，请联系智订云财务：0755-25787564
                    </p>
                </div>
            </dd>
            <dd>
                <small>发票内容</small>
                <div>
                    <select>
                        <option value="value"></option>
                    </select>
                </div>
            </dd>
        </div>
        
        <div class="sharehead sharehead02 sharehead03">
            <span>补充发票信息</span><span class="line"></span>
        </div>
        <div class="clearfix">
            <dd>
                <small>纳税人识别号</small>
                <div>
                    <input type="text" class="inputban" />
                </div>
            </dd>
            <dd>
                <small>开户行</small>
                <div>
                    <input type="text" class="inputban" />
                </div>
            </dd>
            <dd>
                <small>地址</small>
                <div>
                    <input type="text" class="inputban" />
                </div>
            </dd>
            <dd>
                <small>帐号</small>
                <div>
                    <input type="text" class="inputban" />
                    <p>
                        如需修改，请联系智订云财务：0755-25787564
                    </p>
                </div>
            </dd>
            <dd style="margin-top:-28px;">
                <small>电话</small>
                <div>
                    <input type="text" class="inputban" />
                </div>
            </dd>
        </div>
        <div class="sharehead sharehead02">
            <span>快递信息</span><span class="line"></span>
        </div>
        <div class="clearfix">
            <dd>
                <small>收件人</small>
                <div>
                    <input type="text" class="inputban" />
                </div>
            </dd>
            <dd>
                <small>所在地区</small>
                <div>
                    <div class="w110">
                        <select>
                            <option value="value"></option>
                        </select>
                    </div>
                    <div class="w110">
                        <select>
                            <option value="value"></option>
                        </select>
                    </div>
                    <div class="w110">
                        <select>
                            <option value="value"></option>
                        </select>
                    </div>
                </div>
            </dd>
            <dd>
                <small>联系方式</small>
                <div>
                    <input type="text" class="inputban" />
                </div>
            </dd>
            <dd>
                <small>发票地址</small>
                <div>
                    <input type="text" class="inputban" />
                </div>
            </dd>
            <dd style="width:100%;">
                <small>标题</small>
                <div>
                    <textarea cols="30" rows="10">【柏丽酒店】标准大床房-有早</textarea>
                    <p>
                        开票金额满500元免快递费，低于500元快递费由酒店承担；发票可累计多月合并开具
                    </p>
                </div>
            </dd>
        </div>
    </dl>
    <div class="sharebottombtn">
        <div class="fr">
            <input type="submit" name="btnSumit" value="提交">
            <input type="submit" name="btnSumit" value="重置">
        </div>
    </div>
    </form>
</body>
</html>
