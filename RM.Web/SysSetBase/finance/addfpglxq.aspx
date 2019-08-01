<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="addfpglxq.aspx.cs" Inherits="RM.Web.SysSetBase.finance.addfpglxq" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>发票管理详情</title>
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
    <dl class="addfpgl addfpglxq">
        <div class="sharehead sharehead04">
            <span>申请人信息</span><span class="line"></span>
        </div>
        <div class="clearfix">
            <dd>
                <small>申请人</small>
                <div>
                    李鑫
                </div>
            </dd>
            <dd>
                <small>申请时间</small>
                <div>
                    2018-11-01 08:00
                </div>
            </dd>
        </div>
        
        <div class="sharehead sharehead02">
            <span>发票信息</span><span class="line"></span>
        </div>
        <div class="clearfix">
            <dd>
                <small>发票类型</small>
                <div>
                    增值税专用发票
                </div>
            </dd>
            <dd>
                <small>发票抬头</small>
                <div>
                    深圳中投柏丽置业有限公司
                </div>
            </dd>
            <dd>
                <small>发票金额(元)</small>
                <div>
                    7896.00
                </div>
            </dd>
            <dd>
                <small>发票内容</small>
                <div>
                    服务费
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
                    6543541321231231X
                </div>
            </dd>
            <dd>
                <small>开户行</small>
                <div>
                    深圳市翠竹中国工商银行分行 
                </div>
            </dd>
            <dd>
                <small>地址</small>
                <div>
                    深圳市罗湖区贝丽南路鹏利阁23G
                </div>
            </dd>
            <dd>
                <small>账号</small>
                <div>
                    20221564564545455
                </div>
            </dd>
            <dd>
                <small>电话</small>
                <div>
                    0755-25787564
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
                    何佳丽
                </div>
            </dd>
            <dd>
                <small>所在地区</small>
                <div>
                    广东省 深圳市 罗湖区 
                </div>
            </dd>
            <dd>
                <small>联系方式</small>
                <div>
                    13794874785
                </div>
            </dd>
            <dd>
                <small>备注</small>
                <div>
                    请尽快...
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
                    何佳丽
                </div>
            </dd>
            <dd>
                <small>所在地区</small>
                <div>
                    广东省 深圳市 罗湖区 
                </div>
            </dd>
            <dd>
                <small>联系方式</small>
                <div>
                    13794874785
                </div>
            </dd>
            <dd>
                <small>备注</small>
                <div>
                    请尽快...
                </div>
            </dd>
            <dd>
                <p>
                提示：开票金额已满500元，可免快递费
                </p>
            </dd>
        </div>
        <div class="sharehead sharehead02">
            <span>申请情况</span><span class="line"></span>
        </div>
        <div class="clearfix">
            <dd class="w100">
                <small>申请状态</small>
                <div class="gray">
                    已退回
                </div>
                <div class="green">
                    申请中
                </div>
                <div class="blue">
                    已通过
                </div>
            </dd>
            <dd class="w100">
                <small>退回原因</small>
                <div>
                    经双方协商，取消此次申请。
                </div>
            </dd>
            <dd class="w100">
                <small>退回时间</small>
                <div>
                    2018-11-01 08:00
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
