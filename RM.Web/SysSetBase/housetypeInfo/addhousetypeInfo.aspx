<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="addhousetypeInfo.aspx.cs" Inherits="RM.Web.SysSetBase.housetypeInfo.addhousetypeInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>房型信息</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer" />
    <script src="../../WDatePicker/WdatePicker.js" type="text/javascript"></script>
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/SysSetBase/css/jquery.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <dl class="addhousetypeInfo">
        <div class="sharehead">
            <span>基本信息</span><span class="line"></span>
        </div><%--
        <dd>
            <small>酒店名称</small>
            <div>
                湖北宾馆（洪湖店）
            </div>
        </dd>--%>
        <div class="clearfix">
        
            <div style="width:50%;float:left;">
        
                <dd>
                    <small>房型</small>
                    <div>
                        <input type="text" />
                    </div>
                </dd>
                <dd>
                    <small>所在位置</small>
                    <div>
                        <input type="text" />
                    </div>
                </dd>
                <dd>
                    <small>面积</small>
                    <div>
                        <input type="text" />
                    </div>
                </dd>
                <dd>
                    <small>床型</small>
                    <div class="radio">
                        <label class="checked">大床</label>
                        <label>双床</label>
                        <label>大/双床</label>
                        <label>三床</label>
                        <label>圆床</label>
                        <label>不显示</label>
                    </div>
                </dd>
                <dd>
                    <small>床宽（米）</small>
                    <div>
                        <input type="text" />
                    </div>
                </dd>
            </div>
            <div style="width:50%;float:left;">
        
                <dd>
                    <small>客房设施</small>
                    <div class="checkbox">
                        <label class="checked">全选</label>
                        <label>免费宽带</label>
                        <label>电话</label>
                        <label>有线电视</label>
                        <label>独立洗手间</label>
                        <label>独立分体空调</label><br />
                        <label>电水壶</label>
                        <label>电子门锁</label>
                        <label>自助吹风机</label>
                        <label>24小时热水供应</label>
                    </div>
                </dd>
                <dd>
                    <small>房型介绍</small>
                    <div>
                        <textarea cols="30" rows="10"></textarea>
                    </div>
                </dd>
            </div>
        </div>
        <div class="sharehead sharehead02">
            <span>推广信息</span><span class="line"></span>
        </div>
        <dd>
            <small>是否推广</small>
            <div class="radio">
                <label class="checked">是</label>
                <label>否</label>
            </div>
        </dd>
        <dd>
            <small>标题</small>
            <div class="w346">
                <textarea cols="30" rows="10">【柏丽酒店】标准大床房-有早</textarea>
            </div>
        </dd>
        <dd>
            <small>广告语</small>
            <div  class="w346">
                <textarea cols="30" rows="10">新优惠！现在预订“标准大床房-有早”只需要￥203，快来预订吧！</textarea>
            </div>
        </dd>
        <dd>
            <small>图片</small>
            <div>
                <a class="btn">上传</a> <i class="jy" style="display: inline-block; padding: 0;">图片尺寸比例为1:1</i>
                <i class="photo">
                <img src="/App_Themes/default/images/logo10.png" alt="photo">
                </i>
            </div>
        </dd>
        <div class="wxfxyl">
            <p class="p1">
                微信分享预览
                <span class="tb">
                    <div class="arrow-left arrow-box tb" >
                        <b class="left"><i class="left-arrow1"></i><i class="left-arrow2"></i></b>
                    </div>
                </span>
                
            </p>
            <p class="p2">
                发送给朋友
            </p>
            <div class="d1">
                <h1>
                【柏丽酒店】标准大床房
                </h1>
                <div class="nrtp">
                    <div class="nr">
                        新优惠！现在预订“标准大床房-有早”只需要￥203，快来预订吧！
                    </div>
                    <div class="tp">
                        <img src="../img/ewm.png" />
                    </div>
                </div>
            </div>
            
            <p class="p3">
                分享到朋友圈
            </p>
            
            <div class="d2">
                <div class="tp">
                    <img src="../img/ewm.png" />
                </div>
                <div class="nr">
                    【柏丽酒店】标准大床房
                </div>
            </div>
            
        </div>
    </dl>
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
