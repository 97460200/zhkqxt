<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="live.aspx.cs" Inherits="RM.Web.SysSetBase.live.live" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>实景管理</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="gtall gmkf clearfix">
        <div class="gmkfNav">
            <dl>
                <dd class="">
                    <b>美思柏丽酒店</b>
                    <ul style="display: none;">
                        <li><span>鹤山前进南路店</span> </li>
                        <li><span>佛山文华北路店</span> </li>
                        <li><span>恩平鳌峰广场店</span> </li>
                        <li><span>鹤山中心店</span> </li>
                    </ul>
                </dd>
            </dl>
        </div>
        <div class="shareright" style="overflow-y: auto;">
            <div class="ptb8" style="border-bottom:1px solid #eee;">
                <div class="wdyhd" style="padding-right: 12px;">
                    <div class="r">
                        <span onclick="edit3()">栏目管理</span>
                    </div>
                </div>
            </div>
            <div class="addlive">
                <div class="addlivelist">
                    <div class="addlivelistt">
                        <span>房型图片</span>
                        <p>建议上传图片尺寸为4:3，拖动图片可排序。</p>
                    </div>
                    <div class="addlivelistb">
                        <div class="cfae4ce clearfix">
                            <div class="add">
                                <i class="icon-plus"></i>
                            </div>
                            <div class="addlist demo clearfix">
                                <div class="item item1">
                                   <img src="../../upload/image/1.jpg" alt="Alternate Text" />
                                   <span class="gb">
                                        <i class="icon-close"></i>
                                    </span>  
                                    <div class="it">
                                        <input type="text" name="name" value="" placeholder="请输入名称"/>
                                    </div>
                                </div>
                                <div class="item item2">
                                    <img src="../../upload/image/2.jpg" alt="Alternate Text" />
                                    <div class="it">
                                        <input type="text" name="name" value="" placeholder="请输入名称"/>
                                    </div>
                                </div>
                                <div class="item item3">
                                   <img src="../../upload/image/3.jpg" alt="Alternate Text" />
                                   <div class="it">
                                        <input type="text" name="name" value="" placeholder="请输入名称"/>
                                   </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="addlivelist">
                    <div class="addlivelistt">
                        <span>休闲娱乐</span>
                        <p>建议上传图片尺寸为4:3，拖动图片可排序。</p>
                    </div>
                    <div class="addlivelistb">
                        <div class="cfae4ce clearfix">
                            <div class="add">
                                <i class="icon-plus"></i>
                            </div>
                            
                        </div>
                    </div>
                </div>

                <div class="addlivelist">
                    <div class="addlivelistt">
                        <span>会议室</span>
                    </div>
                    <div class="addlivelistb">
                        <div class="cfae4ce clearfix" style="color:#666;font-size:14px;">
                            外部链接：http://oa.sewa-power.com/Frame/MainIndex.aspx
                        </div>
                    </div>
                </div>

                <div class="addlivelist">
                    <div class="addlivelistt">
                        <span>会议室</span>
                    </div>
                    <div class="addlivelistb">
                        <div class="fx">关联“餐饮服务"</div>
                        <div class="cfae4ce clearfix">
                            <div class="addlist demo clearfix">
                                <div class="item item1">
                                   <img src="../../upload/image/1.jpg" alt="Alternate Text" />
                                </div>
                                <div class="item item2">
                                    <img src="../../upload/image/2.jpg" alt="Alternate Text" />
                                </div>
                                <div class="item item3">
                                   <img src="../../upload/image/3.jpg" alt="Alternate Text" />
                                </div>
                            </div>
                        </div>

                    </div>
                </div>

                <div class="addlivelist">
                    <div class="addlivelistt">
                        <span>客房</span>
                        <p>建议上传图片尺寸为4:3，拖动图片可排序。</p>
                    </div>
                    <div class="addlivelistb">
                        <div class="cfae4ce clearfix">
                            <div class="add">
                                <i class="icon-plus"></i>
                            </div>
                            
                        </div>
                    </div>
                </div>



                <div style="height: 50px;">
                </div>
                <div class="sharebottombtn" style="background-color: #F3F3F3;position: fixed;bottom: 0;left: 200px;width: 100%;padding-left: 20px;">
                    <input type="submit" name="btnSumit" value="提交">
                </div>
            </div>
        </div>
    </div>

    </form>
</body>
</html>
<link href="jquery.dad.css" rel="stylesheet" type="text/css" />
<script src="jquery-1.11.3.min.js"></script>
<script src="jquery.dad.min.js"></script>
<script>
    $(function () {
        $('.demo').dad({
            draggable: '.dads-children img'
        });
    });
</script>