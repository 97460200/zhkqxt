<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tips.aspx.cs" Inherits="RM.Web.SysSetBase.xitongcanshu.Tips" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>提示显示设置</title>
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
 
        .ke-container
        {
            width:320px !important;
            height:72px !important;
            }
        .ke-toolbar {
	        display: none !important;
        }
    </style>
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
    
    <link href="../../kindeditor/themes/default/default.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../../kindeditor/plugins/code/prettify.css" />
    <script charset="utf-8" src="../../kindeditor/kindeditor.js"></script>
    <script charset="utf-8" src="../../kindeditor/lang/zh_CN.js"></script>
    <script charset="utf-8" src="../../kindeditor/plugins/code/prettify.js"></script>
      <script>
        KindEditor.ready(function (K) {
            var editor1 = K.create('#txtNetworkInfo', {
                cssPath: '../../kindeditor/plugins/code/prettify.css',
                uploadJson: '../../kindeditor/asp.net/upload_json.ashx',
                fileManagerJson: '../../kindeditor/asp.net/file_manager_json.ashx',
                allowFileManager: false,
                allowImageRemote: false,
                afterCreate: function () {
                    var self = this;
                    K.ctrl(document, 13, function () {
                        self.sync();
                        K('form[name=example]')[0].submit();
                    });
                    K.ctrl(self.edit.doc, 13, function () {
                        self.sync();
                        K('form[name=example]')[0].submit();
                    });
                }
            });
            prettyPrint();
        });
    </script>
      <script>
          KindEditor.ready(function (K) {
              var editor1 = K.create('#txtRoomInfo', {
                  cssPath: '../../kindeditor/plugins/code/prettify.css',
                  uploadJson: '../../kindeditor/asp.net/upload_json.ashx',
                  fileManagerJson: '../../kindeditor/asp.net/file_manager_json.ashx',
                  allowFileManager: false,
                  allowImageRemote: false,
                  afterCreate: function () {
                      var self = this;
                      K.ctrl(document, 13, function () {
                          self.sync();
                          K('form[name=example]')[0].submit();
                      });
                      K.ctrl(self.edit.doc, 13, function () {
                          self.sync();
                          K('form[name=example]')[0].submit();
                      });
                  }
              });
              prettyPrint();
          });
    </script>
      <script>
          KindEditor.ready(function (K) {
              var editor1 = K.create('#txtCenterInfo', {
                  cssPath: '../../kindeditor/plugins/code/prettify.css',
                  uploadJson: '../../kindeditor/asp.net/upload_json.ashx',
                  fileManagerJson: '../../kindeditor/asp.net/file_manager_json.ashx',
                  allowFileManager: false,
                  allowImageRemote: false,
                  afterCreate: function () {
                      var self = this;
                      K.ctrl(document, 13, function () {
                          self.sync();
                          K('form[name=example]')[0].submit();
                      });
                      K.ctrl(self.edit.doc, 13, function () {
                          self.sync();
                          K('form[name=example]')[0].submit();
                      });
                  }
              });
              prettyPrint();
          });
    </script>
      <script>
          KindEditor.ready(function (K) {
              var editor1 = K.create('#txtBookInfo', {
                  cssPath: '../../kindeditor/plugins/code/prettify.css',
                  uploadJson: '../../kindeditor/asp.net/upload_json.ashx',
                  fileManagerJson: '../../kindeditor/asp.net/file_manager_json.ashx',
                  allowFileManager: false,
                  allowImageRemote: false,
                  afterCreate: function () {
                      var self = this;
                      K.ctrl(document, 13, function () {
                          self.sync();
                          K('form[name=example]')[0].submit();
                      });
                      K.ctrl(self.edit.doc, 13, function () {
                          self.sync();
                          K('form[name=example]')[0].submit();
                      });
                  }
              });
              prettyPrint();
          });
    </script>
    </style>

       <script type="text/javascript">
           $(function () {

           debugger
           var NetworkType = $("#hdNetworkType").val();
               if (NetworkType == "0") {
                   $("#pNetworkInfo").show();
                   $("#divNetworkInfo").show();
                   $("#pNetworkInfo").parent().removeClass('act');
               } else if (NetworkType == "1") {
                   $("#pNetworkInfo").hide();
                   $("#divNetworkInfo").hide();
                   $("#pNetworkInfo").parent().removeClass('act');
               } else if (NetworkType == "2" || NetworkType == "3") {
                   $("#pNetworkInfo").show();
                   $("#divNetworkInfo").show();
                   $("#pNetworkInfo").parent().addClass('act');
               }
     
               $('#IsNetwork').on('click', 'label', function () {
                   $(this).addClass('checked').siblings().removeClass('checked');
                   if ($.trim($(this).html()) == "否") {
                       $("#NetworkMore").hide();
                       $("#DivNetworkMore").hide();
                       $("#hdIsNetwork").val(0);
                   }
                   if ($.trim($(this).html()) == "是") {
                       $("#NetworkMore").show();
                       $("#DivNetworkMore").show();
                       $("#hdIsNetwork").val(1);
                   }

               });

               $("#IsNetwork label").each(function () {
                   var IsNetwork = "";
                   if ($("#hdIsNetwork").val() == "0") {
                       IsNetwork = "否";
                   } else if ($("#hdIsNetwork").val() == "1") {
                       IsNetwork = "是";
                   }
                   if (IsNetwork == $.trim($(this).html())) {
                       $(this).addClass('checked').siblings().removeClass('checked');
                       if ($.trim($(this).html()) == "否") {
                           $("#NetworkMore").hide();
                           $("#DivNetworkMore").hide();
                           $("#hdIsNetwork").val(0);
                       }
                       if ($.trim($(this).html()) == "是") {
                           $("#NetworkMore").show();
                           $("#DivNetworkMore").show();
                           $("#hdIsNetwork").val(1);
                       }
                   }
               });

               var RoomType = $("#hdRoomType").val();
               if (RoomType == "0") {
                   $("#pRoomInfo").show();
                   $("#divRoomInfo").show();
                   $("#pRoomInfo").parent().removeClass('act');
               } else if (RoomType == "1") {
                   $("#pRoomInfo").hide();
                   $("#divRoomInfo").hide();
                   $("#pRoomInfo").parent().removeClass('act');
               } else if (RoomType == "2" || RoomType == "3") {
                   $("#pRoomInfo").show();
                   $("#divRoomInfo").show();
                   $("#pRoomInfo").parent().addClass('act');
               }

               $('#IsRoom').on('click', 'label', function () {
                   $(this).addClass('checked').siblings().removeClass('checked');
                   if ($.trim($(this).html()) == "否") {
                       $("#RoomMore").hide();
                       $("#DivRoomMore").hide();
                       $("#hdIsRoom").val(0);
                   }
                   if ($.trim($(this).html()) == "是") {
                       $("#RoomMore").show();
                       $("#DivRoomMore").show();
                       $("#hdIsRoom").val(1);
                   }

               });

               $("#IsRoom label").each(function () {

                   var IsRoom = "";
                   if ($("#hdIsRoom").val() == "0") {
                       IsRoom = "否";
                   } else if ($("#hdIsRoom").val() == "1") {
                       IsRoom = "是";
                   }
                   if (IsRoom == $.trim($(this).html())) {
                       $(this).addClass('checked').siblings().removeClass('checked');
                       if ($.trim($(this).html()) == "否") {
                           $("#RoomMore").hide();
                           $("#DivRoomMore").hide();
                           $("#hdIsRoom").val(0);
                       }
                       if ($.trim($(this).html()) == "是") {
                           $("#RoomMore").show();
                           $("#DivRoomMore").show();
                           $("#hdIsRoom").val(1);
                       }
                   }
               });


               var CenterType = $("#hdCenterType").val();
               if (CenterType == "0") {
                   $("#pCenterInfo").show();
                   $("#divCenterInfo").show();
                   $("#pCenterInfo").parent().removeClass('act');
               } else if (CenterType == "1") {
                   $("#pCenterInfo").hide();
                   $("#divCenterInfo").hide();
                   $("#pCenterInfo").parent().removeClass('act');
               } else if (CenterType == "2" || CenterType == "3") {
                   $("#pCenterInfo").show();
                   $("#divCenterInfo").show();
                   $("#pCenterInfo").parent().addClass('act');
               }

               $('#IsCenter').on('click', 'label', function () {
                   $(this).addClass('checked').siblings().removeClass('checked');
                   if ($.trim($(this).html()) == "否") {
                       $("#CenterMore").hide();
                       $("#DivCenterMore").hide();
                       $("#hdIsCenter").val(0);
                   }
                   if ($.trim($(this).html()) == "是") {
                       $("#CenterMore").show();
                       $("#DivCenterMore").show();
                       $("#hdIsCenter").val(1);
                   }

               });

               $("#IsCenter label").each(function () {
                   var IsCenter = "";
                   if ($("#hdIsCenter").val() == "0") {
                       IsCenter = "否";
                   } else if ($("#hdIsCenter").val() == "1") {
                       IsCenter = "是";
                   }
                   if (IsCenter == $.trim($(this).html())) {
                       $(this).addClass('checked').siblings().removeClass('checked');
                       if ($.trim($(this).html()) == "否") {
                           $("#CenterMore").hide();
                           $("#DivCenterMore").hide();
                           $("#hdIsCenter").val(0);
                       }
                       if ($.trim($(this).html()) == "是") {
                           $("#CenterMore").show();
                           $("#DivCenterMore").show();
                           $("#hdIsCenter").val(1);
                       }
                   }
               });


               var BookType = $("#hdBookType").val();
               if (BookType == "0") {
                   $("#pBookInfo").show();
                   $("#divBookInfo").show();
                   $("#pBookInfo").parent().removeClass('act');
               } else if (BookType == "1") {
                   $("#pBookInfo").hide();
                   $("#divBookInfo").hide();
                   $("#pBookInfo").parent().removeClass('act');
               } else if (BookType == "2" || BookType == "3") {
                   $("#pBookInfo").show();
                   $("#divBookInfo").show();
                   $("#pBookInfo").parent().addClass('act');
               }


               $('#IsBook').on('click', 'label', function () {
                   $(this).addClass('checked').siblings().removeClass('checked');
                   if ($.trim($(this).html()) == "否") {
                       $("#BookMore").hide();
                       $("#DivBookMore").hide();
                       $("#hdIsBook").val(0);
                   }
                   if ($.trim($(this).html()) == "是") {
                       $("#BookMore").show();
                       $("#DivBookMore").show();
                       $("#hdIsBook").val(1);
                   }

               });

               $("#IsBook label").each(function () {
                   var IsBook = "";
                   if ($("#hdIsBook").val() == "0") {
                       IsBook = "否";
                   } else if ($("#hdIsBook").val() == "1") {
                       IsBook = "是";
                   }
                   if (IsBook == $.trim($(this).html())) {
                       $(this).addClass('checked').siblings().removeClass('checked');
                       if ($.trim($(this).html()) == "否") {
                           $("#BookMore").hide();
                           $("#DivBookMore").hide();
                           $("#hdIsBook").val(0);
                       }
                       if ($.trim($(this).html()) == "是") {
                           $("#BookMore").show();
                           $("#DivBookMore").show();
                           $("#hdIsBook").val(1);
                       }
                   }
               });
           })

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <input runat="server" type="hidden" id="hdAdminHotelId" value="1" />
    <input runat="server" type="hidden" id="hdHotelId" value="-1" />

    <input type="hidden" runat="server" id="hdNetworkType" value="0" />
    <input type="hidden" runat="server" id="hdIsNetwork" value="0" />
    <input type="hidden" runat="server" id="hdNetworkImg" value="" />
  
    <input type="hidden" runat="server" id="hdRoomType" value="0" />
    <input type="hidden" runat="server" id="hdIsRoom" value="0" />
    <input type="hidden" runat="server" id="hdRoomImg" value="" />

    <input type="hidden" runat="server" id="hdCenterType" value="0" />
    <input type="hidden" runat="server" id="hdIsCenter" value="0" />
    <input type="hidden" runat="server" id="hdCenterImg" value="" />

    
    <input type="hidden" runat="server" id="hdBookType" value="0" />
    <input type="hidden" runat="server" id="hdIsBook" value="0" />
    <input type="hidden" runat="server" id="hdBookImg" value="" />
       <div class="tools_bar btnbartitle btnbartitlenr" style="display: block;">
        <div>
            当前位置：<span><a title="系统首页" href="/zdyindex.aspx">系统首页</a></span><span> &gt;
            </span><span>系统设置</span> &gt; <span>基础设置</span>
        </div>
    </div>
    <div class="memr" style="height:auto;">
        <div class="mrNav">
            <a href="kfsz.aspx">基础参数设置</a><a href="/RMBase/SysHotel/SheSiList.aspx">客房设施设置</a><a class="active">提示显示设置</a>
            <a
                href="/RMBase/SysParameter/HotelInformation.aspx">酒店信息</a>
        </div>
    </div>
    <div class="pd20 tipsheight">
        <div class="kefangsz tips">
            <div class="kefangszt">
                设定相关页面的提示
            </div>
            <dl class="addevaluate kefangszb">
                
                <div>
                <dd>
                    <small>首次进入微网是否弹出提示</small>
                    <div id="IsNetwork" class="radio">
                        <label class="checked" value="1">
                            是</label>
                        <label value="0">
                            否</label>
                    </div>
                    <div id="NetworkMore" class="guige yes" style="display: none;">
                    </div>
                </dd>
                <dd style="margin-left:160px;display: none;" id="DivNetworkMore" >
                    <div style="float:left;">
                    <input type='file' id="fileNetwork" accept='image/gif,image/jpeg,image/jpg,image/png,image/svgs'
                    onchange='UploadNetworkImg(this)' style='display: none' />
                        <div class="add"   onclick="addNetworkImg()"><i class="icon-plus"></i></div>
                        <div style="color:#999;margin:5px 0;">
                            图片尺寸必须为500*620px
                        </div>
                        <a class="btn" onclick="SelectNetworkImg()" style="width: auto;padding: 0 10px;max-width: 120px;margin-bottom:5px;">
                            选择模板图片
                        </a>
                    </div>
                    <div class="addimg">
                        <div class="img" id="NetworkImg" runat="server">
                            <img id="iNetworkImg" runat="server"  src="" />
                        </div>

                          <asp:Label ID="lblNetworkName" runat="server" class="span"></asp:Label>
                          <div style="display:none"><input  type="text" id="hdNetworkDay"  runat="server" value="100" /></div>
                           
                        <div class="p" id="pNetworkInfo" runat="server">
                            会员充值有礼多充多优惠
                        </div>
                    </div>
                    <div class="wz" id="divNetworkInfo" runat="server">
                        <p class="p1">
                            文字
                        </p>
                        <p id="aNetworkInfo">
                            <textarea  id="txtNetworkInfo" runat="server"   style="width:380px;"></textarea>
                        </p>
                        <p class="p2">
                            不得超过14个字数
                        </p>
                        <a class="btn" onclick="onSaveNetwork()" >
                            保存
                        </a>
                        <div class="arrow-left arrow-box tb">
                            <b class="left"><i class="left-arrow1"></i><i class="left-arrow2"></i></b>
                        </div>
                    </div>
                </dd>
                </div>

                <div>
                <dd>
                    <small>客房预订成功是否弹出提示</small>
                    <div id="IsRoom" class="radio">
                        <label value="1">
                            是</label>
                        <label class="checked" value="0">
                            否</label>
                    </div>
                    <div id="RoomMore" class="guige yes" style="display: none;">
                    </div>
                </dd>
                <dd style="margin-left:160px;display:none;" id="DivRoomMore">
                    <div style="float:left;">
                   <input type='file' id="fileRoom" accept='image/gif,image/jpeg,image/jpg,image/png,image/svgs'
                    onchange='UploadRoomImg(this)' style='display: none' />
                        <div class="add" onclick="addRoomImg()"><i class="icon-plus"></i></div>
                        <div style="color:#999;margin:5px 0;">
                            图片尺寸必须为500*620px
                        </div>
                        <a class="btn" onclick="SelectRoomImg()"  style="width: auto;padding: 0 10px;max-width: 120px;margin-bottom:5px;">
                            选择模板图片
                        </a>
                    </div>
                    <div class="addimg">
                        <div class="img" id="RoomImg" runat="server" >
                            <img id="iRoomImg" runat="server"  src="" />
                        </div>
                        <asp:Label ID="lblRoomName" runat="server" class="span"></asp:Label>
                        <div style="display:none"><input  type="text" id="hdRoomDay"  runat="server" value="100" /></div>
                           
                        <div class="p" id="pRoomInfo" runat="server">
                            会员充值有礼多充多优惠
                        </div>
                    </div>

                    <div class="wz" id="divRoomInfo" runat="server">
                        <p class="p1">
                            文字
                        </p>
                        <p  id="aRoomInfo">
                            <textarea  id="txtRoomInfo" runat="server"></textarea>
                        </p>
                        <p class="p2">
                            不得超过14个字数
                        </p>
                        <a class="btn"  onclick="onSaveRoom()">
                            保存
                        </a>
                        <div class="arrow-left arrow-box tb">
                            <b class="left"><i class="left-arrow1"></i><i class="left-arrow2"></i></b>
                        </div>
                    </div>
                </dd>
                </div>
              
                <div>
                <dd>
                    <small>进入会员中心是否弹出提示</small>
                    <div id="IsCenter" class="radio">
                        <label value="1">
                            是</label>
                        <label class="checked" value="0">
                            否</label>
                    </div>
                    <div id="CenterMore" class="guige yes" style="display: none;">
                    </div>
                </dd>
                <dd style="margin-left:160px;display:none;" id="DivCenterMore">
                    <div style="float:left;">
                     <input type='file' id="fileCenter" accept='image/gif,image/jpeg,image/jpg,image/png,image/svgs'
                    onchange='UploadCenterImg(this)' style='display: none' />
                        <div class="add" onclick="addCenterImg()"><i class="icon-plus"></i></div>
                        <div style="color:#999;margin:5px 0;">
                            图片尺寸必须为500*620px
                        </div>
                        <a class="btn"  onclick="SelectCenterImg()"  style="width: auto;padding: 0 10px;max-width: 120px;margin-bottom:5px;">
                            选择模板图片
                        </a>
                    </div>
                    
                    <div class="addimg">
                        <div class="img" id="CenterImg" runat="server" >
                            <img id="iCenterImg" runat="server"  src="" />
                        </div>
                           <asp:Label ID="lblCenterName" runat="server"  class="span"></asp:Label>
                        <div style="display:none"><input  type="text" id="hdCenterDay"  runat="server" value="100" /></div>
                           
                        <div class="p" id="pCenterInfo" runat="server">
                            会员充值有礼多充多优惠
                        </div>
                    </div>

                    <div class="wz" id="divCenterInfo" runat="server">
                        <p class="p1">
                            文字
                        </p>
                        <p id="aCenterInfo">
                            <textarea  id="txtCenterInfo" runat="server"></textarea>
                        </p>
                        <p class="p2">
                            不得超过14个字数
                        </p>
                        <a class="btn" onclick="onSaveCenter()">
                            保存
                        </a>
                        <div class="arrow-left arrow-box tb">
                            <b class="left"><i class="left-arrow1"></i><i class="left-arrow2"></i></b>
                        </div>
                    </div>
                </dd>
                </div>

                <div>
                <dd>
                    <small>进入订单页面是否弹出提示</small>
                    <div id="IsBook" class="radio">
                        <label value="1">
                            是</label>
                        <label class="checked" value="0">
                            否</label>
                    </div>
                    <div id="BookMore" class="guige yes" style="display: none;">
                    </div>
                </dd>
                <dd style="margin-left:160px;display:none;" id="DivBookMore">
                    <div style="float:left;">
                          <input type='file' id="fileBook" accept='image/gif,image/jpeg,image/jpg,image/png,image/svgs'
                    onchange='UploadBookImg(this)' style='display: none' />
                        <div class="add"  onclick="addBookImg(this)"><i class="icon-plus"></i></div>
                        <div style="color:#999;margin:5px 0;">
                            图片尺寸必须为500*620px
                        </div>
                        <a class="btn"   onclick="SelectBookImg()"  style="width: auto;padding: 0 10px;max-width: 120px;margin-bottom:5px;">
                            选择模板图片
                        </a>
                    </div>
                    <div class="addimg">
                        <div class="img"  id="BookImg" runat="server">
                            <img id="iBookImg" runat="server"/>
                        </div>
                               <asp:Label ID="lblBookName" runat="server"  class="span"></asp:Label>
                        <div style="display:none"><input  type="text" id="hdBookDay"  runat="server" value="100" /></div>
                           
                        <div class="p"  id="pBookInfo" runat="server">
                            会员充值有礼多充多优惠
                        </div>

                    </div>

                    <div class="wz"  id="divBookInfo" runat="server">
                        <p class="p1">
                            文字
                        </p>
                        <p id="aBookInfo">
                            <textarea  id="txtBookInfo" runat="server"></textarea>
                        </p>
                        <p class="p2">
                            不得超过14个字数
                        </p>
                        <a class="btn" onclick="onSaveBook()">
                            保存
                        </a>
                        <div class="arrow-left arrow-box tb">
                            <b class="left"><i class="left-arrow1"></i><i class="left-arrow2"></i></b>
                        </div>
                    </div>
                </dd>
                </div>

            </dl>
        </div>
        <a class="btn" onclick="Submit()" style="margin-top: 15px;">保存</a>
    </div>
       
    </form>
        <script type="text/javascript">
            function addNetworkImg() {
                $("#fileNetwork").click();
            }
     
            
            function SelectNetworkImg() {
                var url = "/SysSetBase/xitongcanshu/Template.aspx?SelectType=" + $("#hdNetworkImg").val();
                top.art.dialog.open(url, {
                    id: 'Template',
                    title: '选择模板图片',
                    width: 872,
                    height: 628,
                    close: function () {
                   
                        var jsons = art.dialog.data('jsons'); //人员信息
                        if (jsons != undefined && jsons != "") {
                            var jsonval = jsons.split(",");
                            //图片
                            $("#NetworkImg").empty();
                            var newspan = "<img id='iNetworkImg'   src='/upload/TipsPhoto/" + jsonval[0] + "'/>";
                            $("#hdNetworkImg").val(jsonval[0]);
                            $("#NetworkImg").append(newspan);
                   
                            //文本框
                            $("#pNetworkInfo").empty();
                            $("#pNetworkInfo").html(jsonval[1]);
                            $("#txtNetworkInfo").empty();
                            $("#txtNetworkInfo").text(jsonval[1]);
                            $("#aNetworkInfo").find(".ke-edit-iframe").contents().find(".ke-content").html(jsonval[1]);

                            //模板类型
                            $("#hdNetworkType").val(jsonval[2]);
                            if (jsonval[2] == "0") {
                                $("#pNetworkInfo").show();
                                $("#divNetworkInfo").show();
                                $("#pNetworkInfo").parent().removeClass('act');
                            } else if (jsonval[2] == "1") {
                                $("#pNetworkInfo").hide();
                                $("#divNetworkInfo").hide();
                                $("#pNetworkInfo").parent().removeClass('act');
                            } else if (jsonval[2] == "2" || jsonval[2] == "3") {
                                $("#pNetworkInfo").show();
                                $("#divNetworkInfo").show();
                                $("#pNetworkInfo").parent().addClass('act');
                            }

                            //图片名称
                            $("#lblNetworkName").empty();
                            $("#lblNetworkName").text(jsonval[3]);
                            art.dialog.removeData('jsons');
                        }
                    }
                }, false);
            }

           // 上传微网图片
            function UploadNetworkImg(obj) {
                var files = obj.files;
                if (window.URL) {
                    //上传并保存图片
                    var formData = new FormData();
                    formData.append("type", "add");
                    formData.append("path", "TipsPhoto");
                    formData.append("fileElem", files[0]);
                    if (files[0] != null) {
                        $.ajax({
                            url: "../../Ajax/UploadImg1.ashx",
                            data: formData,
                            type: "post",
                            dataType: "text",
                            enctype: 'multipart/form-data',
                            contentType: false,
                            processData: false,
                            async: false,
                            success: function (img_path) {
                                if (img_path == "" || img_path == "0") {
                                    showTipsMsg('图片上传失败！', '3000', '5');
                                    return false;
                                } else {

                                    $("#lblNetworkName").empty();
                                    $("#pNetworkInfo").empty();
                                    $("#pNetworkInfo").parent().removeClass('act');
                                    $("#hdNetworkType").val(0);
                                    $("#txtNetworkInfo").text("");
                                    $("#aNetworkInfo").find(".ke-edit-iframe").contents().find(".ke-content").html("");

                                    $("#pNetworkInfo").hide();
                                    $("#divNetworkInfo").hide();

                                    $("#NetworkImg").empty();
                                    var newspan = "<img id='iNetworkImg'   src='/upload/TipsPhoto/" + img_path + "'/>";
                                    $("#hdNetworkImg").val(img_path);
                                    $("#NetworkImg").append(newspan);
                                }
                            }
                        })
                    }
                }
            }





            function onSaveNetwork() {

                var NetworkInfo = $("#aNetworkInfo").find(".ke-edit-iframe").contents().find(".ke-content").html();
                if (NetworkInfo != null && NetworkInfo != "") {
                    var networklen = 0;
                    for (var i = 0; i < NetworkInfo.length; i++) {
                        var a = NetworkInfo.charAt(i);
                        if (a.match(/[^\x00-\xff]/ig) != null && a.match(/[\u4e00-\u9fa5]/g)) {
                            networklen += 1;
                        }
                    }
                    if (networklen > 14) {
                        showTipsMsg('首次进入微网文本提示不得超过14个字数！', '3000', '5');
                        return;
                    }
                }

                $.ajax({
                    url: "kfsz.ashx",
                    data: {
                        action: "GetNetwork",
                        AdminHotelid: $("#hdAdminHotelId").val(),
                        HotelId: $("#hdHotelId").val(),
                        IsNetwork: $("#hdIsNetwork").val(),
                        NetworkImg: $("#hdNetworkImg").val(),
                        NetworkType: $("#hdNetworkType").val(),
                        NetworkDay: $("#hdNetworkDay").val(),
                        NetworkName: $("#lblNetworkName").text(),
                        NetworkInfo: NetworkInfo
                    },
                    type: "GET",
                    dataType: "text",
                    success: function (response) {
                        if (response == "1") {
                            $("#pNetworkInfo").html(NetworkInfo);
                            showTipsMsg("保存成功！", 3000, 3);
                            return;
                        }else {
     
                            showTipsMsg("保存失败，请稍后再试！", 3000, 3);
                        }
                    }
                })
            }
        </script>

        <script type="text/javascript">
            function addRoomImg() {
                $("#fileRoom").click();
            }

            function SelectRoomImg() {
                var url = "/SysSetBase/xitongcanshu/Template.aspx?SelectType=" + $("#hdRoomImg").val();
                top.art.dialog.open(url, {
                    id: 'Template',
                    title: '选择模板图片',
                    width: 872,
                    height: 628,
                    close: function () {

                        var jsons = art.dialog.data('jsons'); //人员信息
                        if (jsons != undefined && jsons != "") {
                            var jsonval = jsons.split(",");
                            //图片
                            $("#RoomImg").empty();
                            var newspan = "<img id='iRoomImg'   src='/upload/TipsPhoto/" + jsonval[0] + "'/>";
                            $("#hdRoomImg").val(jsonval[0]);
                            $("#RoomImg").append(newspan);

                            //文本框
                            $("#pRoomInfo").empty();
                            $("#pRoomInfo").html(jsonval[1]);
                            $("#txtRoomInfo").empty();
                            $("#txtRoomInfo").text(jsonval[1]);
                            $("#aRoomInfo").find(".ke-edit-iframe").contents().find(".ke-content").html(jsonval[1]);

                            //模板类型
                            $("#hdRoomType").val(jsonval[2]);
                            if (jsonval[2] == "0") {
                                $("#pRoomInfo").show();
                                $("#divRoomInfo").show();
                                $("#pRoomInfo").parent().removeClass('act');
                            } else if (jsonval[2] == "1") {
                                $("#pRoomInfo").hide();
                                $("#divRoomInfo").hide();
                                $("#pRoomInfo").parent().removeClass('act');
                            } else if (jsonval[2] == "2" || jsonval[2] == "3") {
                                $("#pRoomInfo").show();
                                $("#divRoomInfo").show();
                                $("#pRoomInfo").parent().addClass('act');
                            }

                            //图片名称
                            $("#lblRoomName").empty();
                            $("#lblRoomName").text(jsonval[3]);
                            art.dialog.removeData('jsons');
                        }


                    }
                }, false);
            }

            // 上传微网图片
            function UploadRoomImg(obj) {
  
                var files = obj.files;
                if (window.URL) {
                    //上传并保存图片
                    var formData = new FormData();
                    formData.append("type", "add");
                    formData.append("path", "TipsPhoto");
                    formData.append("fileElem", files[0]);
                    if (files[0] != null) {
                        $.ajax({
                            url: "../../Ajax/UploadImg1.ashx",
                            data: formData,
                            type: "post",
                            dataType: "text",
                            enctype: 'multipart/form-data',
                            contentType: false,
                            processData: false,
                            async: false,
                            success: function (img_path) {
                                if (img_path == "" || img_path == "0") {
                                    showTipsMsg('图片上传失败！', '3000', '5');
                                    return false;
                                } else {
       
                                    $("#lblRoomName").empty();
                                    $("#pRoomInfo").empty();
                                    $("#pRoomInfo").parent().removeClass('act');
                                    $("#hdRoomType").val(0);
                                    $("#txtRoomInfo").text("");
                                    $("#aRoomInfo").find(".ke-edit-iframe").contents().find(".ke-content").html("");

                                    $("#pRoomInfo").hide();
                                    $("#divRoomInfo").hide();

                                    $("#RoomImg").empty();
                                    var newspan = "<img id='iRoomImg'   src='/upload/TipsPhoto/" + img_path + "'/>";
                                    $("#hdRoomImg").val(img_path);
                                    $("#RoomImg").append(newspan);
                                }
                            }
                        })
                    }
                }
            }

            function onSaveRoom() {

                var RoomInfo = $("#aRoomInfo").find(".ke-edit-iframe").contents().find(".ke-content").html();

                if (RoomInfo != null && RoomInfo != "") {
                    var roomlen = 0;
                    for (var i = 0; i < RoomInfo.length; i++) {
                        var a = RoomInfo.charAt(i);
                        if (a.match(/[^\x00-\xff]/ig) != null && a.match(/[\u4e00-\u9fa5]/g)) {
                            roomlen += 1;
                        }
                    }
                    if (roomlen > 14) {
                        showTipsMsg('客房预订成功文本提示不得超过14个字数！', '3000', '5');
                        return;
                    } 
                }
                $.ajax({
                    url: "kfsz.ashx",
                    data: {
                        action: "GetRoom",
                        AdminHotelid: $("#hdAdminHotelId").val(),
                        HotelId: $("#hdHotelId").val(),
                        IsRoom: $("#hdIsRoom").val(),
                        RoomImg: $("#hdRoomImg").val(),
                        RoomType: $("#hdRoomType").val(),
                        RoomDay: $("#hdRoomDay").val(),
                        RoomName: $("#lblRoomName").text(),
                        RoomInfo: RoomInfo
                    },
                    type: "GET",
                    dataType: "text",
                    success: function (response) {
                        if (response == "1") {
                            $("#pRoomInfo").html(RoomInfo);
                            showTipsMsg("保存成功！", 3000, 3);
                            return;
                        } else {

                            showTipsMsg("保存失败，请稍后再试！", 3000, 3);
                        }
                    }
                })
            }
        </script>

        <script type="text/javascript">
               function addCenterImg() {
                   $("#fileCenter").click();
               }

               function SelectCenterImg() {
                   var url = "/SysSetBase/xitongcanshu/Template.aspx?SelectType=" + $("#hdCenterImg").val();
                   top.art.dialog.open(url, {
                       id: 'Template',
                       title: '选择模板图片',
                       width: 872,
                       height: 628,
                       close: function () {

                           var jsons = art.dialog.data('jsons'); //人员信息
                           if (jsons != undefined && jsons != "") {
                               var jsonval = jsons.split(",");
                               //图片
                               $("#CenterImg").empty();
                               var newspan = "<img id='iCenterImg'   src='/upload/TipsPhoto/" + jsonval[0] + "'/>";
                               $("#hdCenterImg").val(jsonval[0]);
                               $("#CenterImg").append(newspan);

                               //文本框
                               $("#pCenterInfo").empty();
                               $("#pCenterInfo").html(jsonval[1]);
                               $("#txtCenterInfo").empty();
                               $("#txtCenterInfo").text(jsonval[1]);
                               $("#aCenterInfo").find(".ke-edit-iframe").contents().find(".ke-content").html(jsonval[1]);

                               //模板类型
                               $("#hdCenterType").val(jsonval[2]);
                               if (jsonval[2] == "0") {
                                   $("#pCenterInfo").show();
                                   $("#divCenterInfo").show();
                                   $("#pCenterInfo").parent().removeClass('act');
                               } else if (jsonval[2] == "1") {
                                   $("#pCenterInfo").hide();
                                   $("#divCenterInfo").hide();
                                   $("#pCenterInfo").parent().removeClass('act');
                               } else if (jsonval[2] == "2" || jsonval[2] == "3") {
                                   $("#pCenterInfo").show();
                                   $("#divCenterInfo").show();
                                   $("#pCenterInfo").parent().addClass('act');
                               }

                               //图片名称
                               $("#lblCenterName").empty();
                               $("#lblCenterName").text(jsonval[3]);
                               art.dialog.removeData('jsons');
                           }
                       }
                   }, false);
               }

               // 上传微网图片
               function UploadCenterImg(obj) {
                   var files = obj.files;
                   if (window.URL) {
                       //上传并保存图片
                       var formData = new FormData();
                       formData.append("type", "add");
                       formData.append("path", "TipsPhoto");
                       formData.append("fileElem", files[0]);
                       if (files[0] != null) {
                           $.ajax({
                               url: "../../Ajax/UploadImg1.ashx",
                               data: formData,
                               type: "post",
                               dataType: "text",
                               enctype: 'multipart/form-data',
                               contentType: false,
                               processData: false,
                               async: false,
                               success: function (img_path) {
                                   if (img_path == "" || img_path == "0") {
                                       showTipsMsg('图片上传失败！', '3000', '5');
                                       return false;
                                   } else {

                                       $("#lblCenterName").empty();
                                       $("#pCenterInfo").empty();
                                       $("#pCenterInfo").parent().removeClass('act');
                                       $("#hdCenterType").val(0);
                                       $("#txtCenterInfo").text("");
                                       $("#aCenterInfo").find(".ke-edit-iframe").contents().find(".ke-content").html("");

                                       $("#pCenterInfo").hide();
                                       $("#divCenterInfo").hide();

                                       $("#CenterImg").empty();
                                       var newspan = "<img id='iCenterImg'   src='/upload/TipsPhoto/" + img_path + "'/>";
                                       $("#hdCenterImg").val(img_path);
                                       $("#CenterImg").append(newspan);
                                   }
                               }
                           })
                       }
                   }
               }

               function onSaveCenter() {

                   var CenterInfo = $("#aCenterInfo").find(".ke-edit-iframe").contents().find(".ke-content").html();

                   if (CenterInfo != null && CenterInfo != "") {
                       var centerlen = 0;
                       for (var i = 0; i < CenterInfo.length; i++) {
                           var a = CenterInfo.charAt(i);
                           if (a.match(/[^\x00-\xff]/ig) != null && a.match(/[\u4e00-\u9fa5]/g)) {
                               centerlen += 1;
                           }
                       }

                       if (centerlen > 14) {
                           showTipsMsg('进入会员中心文本提示不得超过14个字数！', '3000', '5');
                           return;
                       }
                   }
                   
                   $.ajax({
                       url: "kfsz.ashx",
                       data: {
                           action: "GetCenter",
                           AdminHotelid: $("#hdAdminHotelId").val(),
                           HotelId: $("#hdHotelId").val(),
                           IsCenter: $("#hdIsCenter").val(),
                           CenterImg: $("#hdCenterImg").val(),
                           CenterType: $("#hdCenterType").val(),
                           CenterDay: $("#hdCenterDay").val(),
                           CenterName: $("#lblCenterName").text(), 
                           CenterInfo: CenterInfo
                       },
                       type: "GET",
                       dataType: "text",
                       success: function (response) {
                           if (response == "1") {
                               $("#pCenterInfo").html(CenterInfo);
                               showTipsMsg("保存成功！", 3000, 3);
                               return;
                           } else {

                               showTipsMsg("保存失败，请稍后再试！", 3000, 3);
                           }
                       }
                   })
               }
        </script>

        <script type="text/javascript">
            function addBookImg() {
                $("#fileBook").click();
             }

             function SelectBookImg() {
                 var url = "/SysSetBase/xitongcanshu/Template.aspx?SelectType=" + $("#hdBookImg").val();
                 top.art.dialog.open(url, {
                     id: 'Template',
                     title: '选择模板图片',
                     width: 872,
                     height: 628,
                     close: function () {

                         var jsons = art.dialog.data('jsons'); //人员信息
                         if (jsons != undefined && jsons != "") {
                             var jsonval = jsons.split(",");
                             //图片
                             $("#BookImg").empty();
                             var newspan = "<img id='iBookImg'   src='/upload/TipsPhoto/" + jsonval[0] + "'/>";
                             $("#hdBookImg").val(jsonval[0]);
                             $("#BookImg").append(newspan);

                             //文本框
                             $("#pBookInfo").empty();
                             $("#pBookInfo").html(jsonval[1]);
                             $("#txtBookInfo").empty();
                             $("#txtBookInfo").text(jsonval[1]);
                             $("#aBookInfo").find(".ke-edit-iframe").contents().find(".ke-content").html(jsonval[1]);

                             //模板类型
                             $("#hdBookType").val(jsonval[2]);
                             if (jsonval[2] == "0") {
                                 $("#pBookInfo").show();
                                 $("#divBookInfo").show();
                                 $("#pBookInfo").parent().removeClass('act');
                             } else if (jsonval[2] == "1") {
                                 $("#pBookInfo").hide();
                                 $("#divBookInfo").hide();
                                 $("#pBookInfo").parent().removeClass('act');
                             } else if (jsonval[2] == "2" || jsonval[2] == "3") {
                                 $("#pBookInfo").show();
                                 $("#divBookInfo").show();
                                 $("#pBookInfo").parent().addClass('act');
                             }

                             //图片名称
                             $("#lblBookName").empty();
                             $("#lblBookName").text(jsonval[3]);
                             art.dialog.removeData('jsons');
                         }
                     }
                 }, false);
             }

             // 上传微网图片
             function UploadBookImg(obj) {
                 var files = obj.files;
                 if (window.URL) {
                     //上传并保存图片
                     var formData = new FormData();
                     formData.append("type", "add");
                     formData.append("path", "TipsPhoto");
                     formData.append("fileElem", files[0]);
                     if (files[0] != null) {
                         $.ajax({
                             url: "../../Ajax/UploadImg1.ashx",
                             data: formData,
                             type: "post",
                             dataType: "text",
                             enctype: 'multipart/form-data',
                             contentType: false,
                             processData: false,
                             async: false,
                             success: function (img_path) {
                                 if (img_path == "" || img_path == "0") {
                                     showTipsMsg('图片上传失败！', '3000', '5');
                                     return false;
                                 } else {
                                     $("#lblBookName").empty();
                                     $("#pBookInfo").empty();
                                     $("#pBookInfo").parent().removeClass('act');
                                     $("#hdBookType").val(0);
                                     $("#txtBookInfo").text("");
                                     $("#aBookInfo").find(".ke-edit-iframe").contents().find(".ke-content").html("");

                                     $("#pBookInfo").hide();
                                     $("#divBookInfo").hide();

                                     $("#BookImg").empty();
                                     var newspan = "<img id='iBookImg'   src='/upload/TipsPhoto/" + img_path + "'/>";
                                     $("#hdBookImg").val(img_path);
                                     $("#BookImg").append(newspan);
                                 }
                             }
                         })
                     }
                 }
             }

             function onSaveBook() {
                 var BookInfo = $("#aBookInfo").find(".ke-edit-iframe").contents().find(".ke-content").html();
                 if (BookInfo != null && BookInfo != "") {
                     var booklen = 0;
                     for (var i = 0; i < BookInfo.length; i++) {
                         var a = BookInfo.charAt(i);
                         if (a.match(/[^\x00-\xff]/ig) != null && a.match(/[\u4e00-\u9fa5]/g)) {
                             booklen += 1;
                         }
                     }
                     if (booklen > 14) {
                         showTipsMsg('进入订单页面文本提示不得超过14个字数！', '3000', '5');
                         return;
                     }
                 }
                 $.ajax({
                     url: "kfsz.ashx",
                     data: {
                         action: "GetBook",
                         AdminHotelid: $("#hdAdminHotelId").val(),
                         HotelId: $("#hdHotelId").val(),
                         IsBook: $("#hdIsBook").val(),
                         BookImg: $("#hdBookImg").val(),
                         BookType: $("#hdBookType").val(),
                         BookDay: $("#hdBookDay").val(),
                         BookName: $("#lblBookName").text(),
                         BookInfo: BookInfo
                     },
                     type: "GET",
                     dataType: "text",
                     success: function (response) {
                         if (response == "1") {
                             $("#pBookInfo").html(BookInfo);
                             showTipsMsg("保存成功！", 3000, 3);
                             return;
                         } else {

                             showTipsMsg("保存失败，请稍后再试！", 3000, 3);
                         }
                     }
                 })
             }
        </script>

        <script type="text/javascript">
            function Submit() {
                var NetworkInfo = $("#aNetworkInfo").find(".ke-edit-iframe").contents().find(".ke-content").html();
                if (NetworkInfo != null && NetworkInfo != "") {
                    var networklen = 0;
                    for (var i = 0; i < NetworkInfo.length; i++) {
                        var a = NetworkInfo.charAt(i);
                        if (a.match(/[^\x00-\xff]/ig) != null && a.match(/[\u4e00-\u9fa5]/g)) {
                            networklen += 1;
                        }
                    }
                    if (networklen > 14) {
                        showTipsMsg('首次进入微网文本提示不得超过14个字数！', '3000', '5');
                        return;
                    }
                }
                var RoomInfo = $("#aRoomInfo").find(".ke-edit-iframe").contents().find(".ke-content").html();
                if (RoomInfo != null && RoomInfo != "") {
                    var roomlen = 0;
                    for (var j = 0; j < RoomInfo.length; j++) {
                        var b = RoomInfo.charAt(j);
                        if (b.match(/[^\x00-\xff]/ig) != null && b.match(/[\u4e00-\u9fa5]/g)) {
                            roomlen += 1;
                        }
                    }
                    if (roomlen > 14) {
                        showTipsMsg('客房预订成功文本提示不得超过14个字数！', '3000', '5');
                        return;
                    }
                }
                var CenterInfo = $("#aCenterInfo").find(".ke-edit-iframe").contents().find(".ke-content").html();
                if (CenterInfo != null && CenterInfo != "") {
                    var centerlen = 0;
                    for (var k = 0; k < CenterInfo.length; k++) {
                        var c = CenterInfo.charAt(k);
                        if (c.match(/[^\x00-\xff]/ig) != null && c.match(/[\u4e00-\u9fa5]/g)) {
                            centerlen += 1;
                        }
                    }
                    if (centerlen > 14) {
                        showTipsMsg('进入会员中心文本提示不得超过14个字数！', '3000', '5');
                        return;
                    }
                }
                var BookInfo = $("#aBookInfo").find(".ke-edit-iframe").contents().find(".ke-content").html();
                if (BookInfo != null && BookInfo != "") {
                    var booklen = 0;
                    for (var g = 0; g < BookInfo.length; g++) {
                        var d = BookInfo.charAt(g);
                        if (d.match(/[^\x00-\xff]/ig) != null && d.match(/[\u4e00-\u9fa5]/g)) {
                            booklen += 1;
                        }
                    }
                    if (booklen > 14) {
                        showTipsMsg('进入订单页面文本提示不得超过14个字数！', '3000', '5');
                        return;
                    }
                }
                $.ajax({
                    url: "kfsz.ashx",
                    data: {
                        action: "GetSubmit",
                        AdminHotelid: $("#hdAdminHotelId").val(),
                        HotelId: $("#hdHotelId").val(),
                        IsNetwork: $("#hdIsNetwork").val(),
                        NetworkImg: $("#hdNetworkImg").val(),
                        NetworkType: $("#hdNetworkType").val(),
                        NetworkDay: $("#hdNetworkDay").val(),
                        NetworkName: $("#lblNetworkName").text(),  
                        NetworkInfo: NetworkInfo,
                        IsRoom: $("#hdIsRoom").val(),
                        RoomImg: $("#hdRoomImg").val(),
                        RoomType: $("#hdRoomType").val(),
                        RoomDay: $("#hdRoomDay").val(),
                        RoomName: $("#lblRoomName").text(), 
                        RoomInfo: RoomInfo,
                        IsCenter: $("#hdIsCenter").val(),
                        CenterImg: $("#hdCenterImg").val(),
                        CenterType: $("#hdCenterType").val(),
                        CenterDay: $("#hdCenterDay").val(),
                        CenterName: $("#lblCenterName").text(),
                        CenterInfo: CenterInfo,
                        IsBook: $("#hdIsBook").val(),
                        BookImg: $("#hdBookImg").val(),
                        BookType: $("#hdBookType").val(),
                        BookDay: $("#hdBookDay").val(),
                        BookName: $("#lblBookName").text(),   
                        BookInfo: BookInfo

                    },
                    type: "GET",
                    dataType: "text",
                    success: function (response) {
                        if (response == "1") {
                            $("#pNetworkInfo").html(NetworkInfo);
                            $("#pRoomInfo").html(RoomInfo);
                            $("#pCenterInfo").html(CenterInfo);
                            $("#pBookInfo").html(BookInfo);
                            showTipsMsg("保存成功！", 3000, 3);
                            return;
                        } else {

                            showTipsMsg("保存失败，请稍后再试！", 3000, 3);
                        }
                    }
                })
            }
        </script>
</body>
</html>
