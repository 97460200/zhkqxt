<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="addBusin.aspx.cs" Inherits="RM.Web.SysSetBase.Busin.addBusin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>营业点管理</title>
    <meta name="viewport" content="width=device-width, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="renderer" content="webkit" />
    <meta content="always" name="referrer">
    <link href="/SysSetBase/css/reset.css" rel="stylesheet" type="text/css" />
    <link href="/SysSetBase/css/backer.css" rel="stylesheet" type="text/css" />
    <script src="/Themes/Scripts/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/Validator/JValidator.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/artDialog.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/artDialog/iframeTools.source.js" type="text/javascript"></script>
    <script src="/Themes/Scripts/FunctionJS.js" type="text/javascript"></script>
        <link href="/SysSetBase/hotelphoto/jquery.dad.css" rel="stylesheet" type="text/css" />
    <script src="/SysSetBase/hotelphoto/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="/SysSetBase/hotelphoto/jquery.dad.min.js" type="text/javascript"></script>
    <link rel="stylesheet" href="/kindeditor/themes/default/default.css" />
    <link rel="stylesheet" href="/kindeditor/plugins/code/prettify.css" />
    <script type="text/javascript" charset="utf-8" src="/kindeditor/kindeditor.js"></script>
    <script type="text/javascript" charset="utf-8" src="/kindeditor/lang/zh_CN.js"></script>
    <script type="text/javascript" charset="utf-8" src="/kindeditor/plugins/code/prettify.js"></script>
    <script type="text/javascript">
        KindEditor.ready(function (K) {
            var editor1 = K.create('#fckContent', {
                cssPath: '/kindeditor/plugins/code/prettify.css',
                uploadJson: '/kindeditor/asp.net/upload_json.ashx',
                fileManagerJson: '/kindeditor/asp.net/file_manager_json.ashx',
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

     <script language="javascript" type="text/javascript">


         function check() {

             var Image = "";
             for (var y = 0; y < $(".demo").find("div").length; y++) {
                 Image += $(".demo").find("div").eq(y).attr("imgname") + ",";
             }

             $("#hfBusinImage").val(Image);

             if ($('#txtBusinessName').val() == '') {
                 showTipsMsg("请输入营业点名称！", 3000, 3);
                 $('#txtBusinessName').focus();
                 return false;
             }

             if ($('#ddlType').val() == null) {
                 showTipsMsg("请先添加所属分类！", 3000, 3);
                 $('#ddlType').focus();
                 return false;
             }


             if ($('#ddlType').val() == '') {
                    showTipsMsg("请选择所属分类！", 3000, 3);
                    $('#ddlType').focus();
                    return false;
             }



             if ($('#txtTelephone').val() == '') {
                 showTipsMsg("请输入服务电话！", 3000, 3);
                 $('#txtTelephone').focus();
                 return false;
             }


             if ($('#txtBusinessTime').val() == '') {
                 showTipsMsg("请输入服务时间！", 3000, 3);
                 $('#txtBusinessTime').focus();
                 return false;
             }


             if ($('#txtBusinessAddress').val() == '') {
                 showTipsMsg("请输入所在位置！", 3000, 3);
                 $('#txtBusinessAddress').focus();
                 return false;
             }


             if (!confirm('您确认要提交此操作吗？')) {
                 return false;
             }

         
         }

  
    </script>

        <script type="text/javascript">
            $(function () {
                hotelphoto();
                $('.demo').dad({
                    draggable: '.dads-children img'
                });

                $('.hotelphoto').on('click', '.gb', function (ev) {
                    ev = ev || window.event;
                    ev.stopPropagation();
                    if (!confirm('您确认要删除当前图片吗？')) {
                        return false;
                    }
                    $(this).parent().remove();
                    $('.demo').dad({
                        draggable: '.dads-children img'
                    });
                });
            })

            //判断最多上传5张图片
            function add(tr) {
                var imglength = $(tr).next().next().find("div").length;
                if (parseInt(imglength) > 4) {
                    showTipsMsg("最多上传5张", 3000, 3);
                    return false;
                }
                else {
                    $(tr).next().click();
                }
            }

            //鼠标移入当前层显示删除按钮
            function toOver(tr) {
                $(tr).find("span").show();
            }
            //鼠标移出当前层显示删除按钮
            function toOut(tr) {
                $(tr).find("span").hide();
            }

            function hotelphoto() {

                //加载酒店图片
                $.ajax({
                    url: "Book.ashx",
                    data: {
                        Menu: "GetBusinImgList",
                        BusinId: $("#hfBusinId").val()
                    },
                    type: "GET",
                    datatype: "JSON",
                    async: false,
                    success: function (data) {

                        if (data == "") {

                            $("#BusinPicture").empty();
                            var html = "";
                            html += "<div class='hotelphotolistb clearfix' style='margin-top:0' onmouseover='toOver(this)' onmouseout='toOut(this)'>";
                            html += " <div class='cfae4ce clearfix' style='padding:0;background-color:#fff;'>";
                            html += "<div class='add' onclick='add(this)'><i class='icon-plus'></i></div>";
                            html += "<input type='file' id='fileElem' name='fileElem'  accept='image/gif,image/jpeg,image/jpg,image/png,image/svgs' onchange='handleFiles(this)' style='display: none' />";
                            html += "<div  typeid='9'  class='addlist demo clearfix' >";
                            html += "</div>";
                            html += "</div>";
                            html += "</div>";
                            $("#BusinPicture").append(html);
                            return;
                        }

                        var json = eval("(" + data + ")");
                        $("#BusinPicture").empty();
                        var html = "";
                        html += "<div class='hotelphotolistb clearfix' style='margin-top:0' onmouseover='toOver(this)' onmouseout='toOut(this)'>";
                        html += " <div class='cfae4ce clearfix' style='padding:0;background-color:#fff;'>";
                        html += "<div class='add' onclick='add(this)'><i class='icon-plus'></i></div>";
                        html += "<input type='file' id='fileElem' name='fileElem'  accept='image/gif,image/jpeg,image/jpg,image/png,image/svgs' onchange='handleFiles(this)' style='display: none' />";
                        html += "<div  typeid='9'  class='addlist demo clearfix' >";
                        for (var i = 0; i < json.length; i++) {
                            if (json[i].IMGFILE != undefined && json[i].IMGFILE != "") {
                                html += "<div imgname='" + json[i].IMGFILE + "'><img src='../../upload/BusinPhoto/SN" + json[i].IMGFILE + "' alt='Alternate Text' /> <span class='gb'><i class='icon-close'></i></span> </div>";
                            }
                        }
                        html += "</div>";
                        html += "</div>";
                        html += "</div>";
                        $("#BusinPicture").append(html);

                    }
                })
            }
    </script>
    <script type="text/javascript">
        //上传图片
        function handleFiles(obj) {

            var files = obj.files,
			img = new Image();
            var src = "";
            if (window.URL) {
                //保存图片
                var formData = new FormData();
                formData.append("type", "add");
                formData.append("path", "BusinPhoto");
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
                        success: function (response) {
                            src = response;
                        }
                    })
                    img.src = window.URL.createObjectURL(files[0]); //创建一个object URL，并不是你的本地路径
                    var newspan = "<div  imgname=" + src + "><img src='" + img.src + "' alt='Alternate Text' /><span class='gb' ><i class='icon-close'></i></span></div>";
                    $(obj).next().append(newspan);
                    $('.demo').dad({
                        draggable: '.dads-children img'
                    });

                }
            }
        }

  

    </script>


</head>
<body>
    <form id="form1" runat="server">
       <input runat="server" type="hidden" id="Hdhoteladmin" value="1" />
    <input runat="server" type="hidden" id="hdHotelId" value="-1" />
        <input runat="server" type="hidden" id="hfBusinId" value="0" />
        <input runat="server" type="hidden" id="hfBusinImage" />
    <dl class="adifoli" style="max-height:500px;">

          <dd class="hotelphoto">
                <small>轮换图</small>
                <div id="BusinPicture">
                </div>
                <div>
                    <i class="jy" style="padding-left: 90px;">建议图片上传尺寸为4:3</i>
                </div>
            </dd>

        <dd>
            <small>营业点名称</small>
            <div>
                <input type="text"  id="txtBusinessName"  runat="server"/>
            </div>
        </dd>
        <dd>
            <small>所属分类</small>
            <div>
              <asp:DropDownList ID="ddlType" runat="server"></asp:DropDownList>
            </div>
        </dd>
        <dd>
            <small>服务电话</small>
            <div>
                <input type="text"   id="txtTelephone"  runat="server"/>
            </div>
           
        </dd>

         <dd>
        
          <small>分机号</small>
            <div>
                <input type="text"   id="txtExtension"  runat="server"/>
            </div>
        </dd>

        <dd>
            <small>服务时间</small>
            <div>
          <input type="text"   id="txtBusinessTime"  runat="server"/>
            </div>
        </dd>
        <dd><small></small>
            <div>
                <input type="text" id="txtBusinessTime1" runat="server" />
            </div>
        </dd>
        <dd><small></small>
            <div>
                <input type="text" id="txtBusinessTime2" runat="server" />
            </div>
        </dd>
        <dd><small></small>
            <div>
                <input type="text" id="txtBusinessTime3" runat="server" />
            </div>
        </dd>
        <dd>
            <small>所在位置</small>
            <div>
          <input type="text"   id="txtBusinessAddress"  runat="server"/>
            </div>
        </dd>
        <dd>
                <small>地址经纬度</small>
                <div>
                    <input type="text" id="txtMap" runat="server" style="width:260px;"/><a class="btn" id="GCoordinate">自动生成</a><span style="color:#c00;">*</span>
                </div>
         </dd>
        <dd style="display:none">
            <small>关联优惠</small>
            <div>
                <select>
                    <option value="0">请选择</option>
                    <option value="1">餐饮服务</option>
                </select>
            </div>
        </dd>
        <dd  style="display:none">
            <small>关联实景</small>
            <div>
                <select>
                    <option value="0">请选择</option>
                    <option value="1">餐饮服务</option>
                </select>
            </div>
        </dd>

         <dd>
            <small>关联餐厅</small>
            <div>
               <asp:DropDownList ID="ddlRestaurant" runat="server"></asp:DropDownList>
            </div>
        </dd>

        <dd>
            <small>简介</small>
            <div>
                <textarea cols="30"  id="txtIntroduction"  runat="server" rows="10" style=" display:none"></textarea>
                <textarea id="fckContent" cols="30" rows="10" 
                            runat="server" style="width:650px;"></textarea>
            </div>
        </dd>
        <dd>
            <small>预订功能</small>
            <div class="radio" id="Reservation">
                <label  class="checked">开启</label><label>关闭</label>
            </div>
        </dd>
        <dd>
            <small>订餐</small>
            <div class="radio" id="OrderMeal">
                <label  class="checked">开启</label><label>关闭</label>
            </div>
        </dd>
        <dd>
            <small>是否展示</small>
            <div class="radio" id="isopen">
                <label  class="checked">是</label><label>否</label>
            </div>
        </dd>

        <dd>
            <small>按钮名称</small>
            <div>
          <input type="text"   id="txtButtonName"  runat="server" value="预订"/>
            </div>
        </dd>

        <dd>
            <small>链接</small>
            <div>
          <input type="text"   id="txtBookUrl"  runat="server"/>
            </div>
        </dd>

        <dd>
            <small>预订须知</small>
            <div>
                <textarea cols="30"  id="txtRecontent"  runat="server" rows="10"></textarea>
            </div>
        </dd>
    </dl>
    <div class="adifoliBtn">
        <div style="float:right;">
             <asp:Button ID="btnSumit" runat="server" Text="提交" OnClientClick="return check()" OnClick="btnSumit_Click" />
            </div>
            <asp:HiddenField ID="hfReservation" runat="server" Value="0" />
            <asp:HiddenField ID="hfOrderMeal" runat="server" Value="0" />
            <asp:HiddenField ID="hfisopen" runat="server" Value="0" />
        </div>
    </form>

      <script type="text/javascript">

          $("#GCoordinate").click(function () {
              if ($.trim($("#txtBusinessAddress").val()) == "") {
                  showTipsMsg("请填写详细地址", 3000, 3);
                  $("#txtBusinessAddress").focus();
              }
              else {
                  $.ajax({
                      type: 'GET',
                      url: "https://apis.map.qq.com/ws/geocoder/v1/?",
                      dataType: 'jsonp',
                      data: {
                          address: $("#txtBusinessAddress").val(),
                          key: 'HZVBZ-IKLAX-4JF4D-7FTGU-ADEHZ-QXBK5',
                          output: "jsonp"
                      },
                      success: function (data, textStatus) {

                          if (data.status == 0) {
                              var address = data.result.location.lat + "," + data.result.location.lng;
                              $("#txtMap").val(address);
                          } else {
                              alert("系统错误，请联系管理员！")
                          }
                      },
                      error: function (fail) {
                          alert("系统错误，请联系管理员！" + fail)
                      }
                  });

              }
          });

          $("#Reservation").on('click', 'label', function () {

              $(this).addClass('checked').siblings().removeClass('checked');

              if ($(this).html() == "开启") {
            
                  $("#hfReservation").val(1);
              }
              if ($(this).html() == "关闭") {
                  $("#hfReservation").val(0);
              }

          });
          $("#OrderMeal").on('click', 'label', function () {

                  $(this).addClass('checked').siblings().removeClass('checked');

                  if ($(this).html() == "开启") {
                      $("#hfOrderMeal").val(1);
                  }
                  if ($(this).html() == "关闭") {
                      $("#hfOrderMeal").val(0);
                  }

              });

              $("#isopen").on('click', 'label', function () {

                  $(this).addClass('checked').siblings().removeClass('checked');

                  if ($(this).html() == "是") {
                      $("#hfisopen").val(1);
                  }
                  if ($(this).html() == "否") {
                      $("#hfisopen").val(0);
                  }

              });

              $(function () {

                  $("#Reservation label").each(function () {
                      var reservation = "";
                      if ('<%=_Reservation %>' == "1") {
                          reservation = "开启";
                      } else if ('<%=_Reservation %>' == "0") {
                          reservation = "关闭";
                      }
                      if (reservation == $(this).html()) {
                          $(this).addClass('checked').siblings().removeClass('checked');
                      }
                  });

                  $("#OrderMeal label").each(function () {
                      var ordermeal = "";
                      if ('<%=_OrderMeal %>' == "1") {
                          ordermeal = "开启";
                      } else if ('<%=_OrderMeal %>' == "0") {
                          ordermeal = "关闭";
                      }
                      if (ordermeal == $(this).html()) {
                          $(this).addClass('checked').siblings().removeClass('checked');
                      }
                  });

                  $("#isopen label").each(function () {
                      var isopen = "";
                      if ('<%=_isopen %>' == "1") {
                          isopen = "是";
                      } else if ('<%=_isopen %>' == "0") {
                          isopen = "否";
                      }
                      if (isopen == $(this).html()) {
                          $(this).addClass('checked').siblings().removeClass('checked');
                      }
                  });

              });
 
    </script>


</body>
</html>
