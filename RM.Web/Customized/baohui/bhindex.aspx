<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="bhindex.aspx.cs" Inherits="RM.Web.Customized.baohui.bhindex" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no">
    <meta name="format-detection" content="telephone=no">
    <title>宝晖商务酒店</title>
    <link href="/App_Themes/default/css/Total.css" rel="stylesheet" type="text/css">
    <link href="/App_Themes/default/css/regroup.css" rel="stylesheet" type="text/css">
    <link href="/App_Themes/default/css/baohui.css" rel="stylesheet" type="text/css" />
    <link href="/App_Themes/default/css/swiper.min.css" rel="stylesheet" type="text/css" />
</head>
<body class="bg">
    <form id="form1" runat="server">
    <%--<div class="banner">
        <div class="swiper-container">
            <div class="swiper-wrapper">
                <div class="swiper-slide"><img src="/App_Themes/default/images/baohuiimage/bg02.jpg" alt="Alternate Text" /></div>
                <div class="swiper-slide"><img src="/App_Themes/default/images/baohuiimage/bg03.jpg" alt="Alternate Text" /></div>
            </div>
        </div>
    </div>--%>
    <div class="nav">
        <%--<img src="/App_Themes/default/images/baohuiimage/bgzzc.png"  />
        <a href="http://www.zidinn.com/Servicedisplay/foodservice01.aspx?AdminHotelid=1001587&Type=27" class="a1"></a>
        <a href="http://www.zidinn.com/Servicedisplay/foodservice01.aspx?AdminHotelid=1001587&Type=26" class="a2"></a>
        <a href="http://www.zidinn.com/Servicedisplay/foodservice01.aspx?AdminHotelid=1001587&Type=29" class="a3"></a>
        <a href="http://www.zidinn.com/Servicedisplay/foodservice01.aspx?AdminHotelid=1001587&Type=28" class="a4"></a>
        <a href="http://www.zidinn.com/Servicedisplay/foodservice01.aspx?AdminHotelid=1001587" class="a5"></a>
        <a href="http://www.zidinn.com/Reservation/HotelDetails.aspx?AdminHotelid=1001587&hotelid=97" class="a6"></a>
        <a href="http://www.zidinn.com/HotelCenter/Discount.aspx?AdminHotelid=1001587" class="a7"></a>
        <a href="http://www.zidinn.com/Servicedisplay/foodserviceorderall.aspx?AdminHotelid=1001587" class="a8"></a>
        <a href="http://www.zidinn.com/Members/MemberCantre.aspx?AdminHotelid=1001587" class="a9"></a>
        <a href="http://www.zidinn.com/PageText/DzMap.aspx?AdminHotelid=1001587&hotelid=97" class="a10"></a>--%>
        <ul>
            <li>
                <a href="http://www.zidinn.com/Reservation/HotelIntroduction.aspx?AdminHotelid=1001587&hotelid=97">
                    <div class="d1">  
                        <i class="i1"></i>
                    </div>
                    <p>
                        酒店介绍
                    </p>
                </a>
            </li>
            <li>
                <a href="http://www.zidinn.com/Reservation/HotelDetails.aspx?AdminHotelid=1001587&hotelid=97">
                    <div class="d1">  
                        <i class="i2"></i>
                    </div>
                    <p>
                        一键订房
                    </p>
                </a>
            </li>
            <li>
                <a href="http://www.zidinn.com/Servicedisplay/foodservice01.aspx?AdminHotelid=1001587&Type=26">
                    <div class="d1">  
                        <i class="i3"></i>
                    </div>
                    <p>
                        舌尖美馔
                    </p>
                </a>
            </li>
            <li>
                <a href="http://www.zidinn.com/Servicedisplay/foodservice01.aspx?AdminHotelid=1001587&Type=29">
                    <div class="d1">  
                        <i class="i4"></i>
                    </div>
                    <p>
                        康体娱乐
                    </p>
                </a>
            </li>
            <li>
                <a href="http://www.zidinn.com/Servicedisplay/foodservice01.aspx?AdminHotelid=1001587&Type=28">
                    <div class="d1">  
                        <i class="i5"></i>
                    </div>
                    <p>
                        会议宴会
                    </p>
                </a>
            </li>
            <li>
                <a href="http://www.zidinn.com/HotelCenter/Discount.aspx?AdminHotelid=1001587">
                    <div class="d1">  
                        <i class="i6"></i>
                    </div>
                    <p>
                        精选优惠
                    </p>
                </a>
            </li>
            <li>
                <a href="http://www.zidinn.com/Servicedisplay/foodserviceorderall.aspx?AdminHotelid=1001587">
                    <div class="d1">  
                        <i class="i7"></i>
                    </div>
                    <p>
                        服务预订
                    </p>
                </a>
            </li>
            <li>
                <a href="http://www.zidinn.com/Members/MemberCantre.aspx?AdminHotelid=1001587">
                    <div class="d1">  
                        <i class="i8"></i>
                    </div>
                    <p>
                        会员之家
                    </p>
                </a>
            </li>
            <li>
                <a href="http://www.zidinn.com/PageText/DzMap.aspx?AdminHotelid=1001587&hotelid=97">
                    <div class="d1">  
                        <i class="i9"></i>
                    </div>
                    <p>
                        地址导航
                    </p>
                </a>
            </li>
            <li>
                <a href="http://www.zidinn.com/Servicedisplay/foodservice01.aspx?AdminHotelid=1001587">
                    <div class="d1">  
                        <i class="i10"></i>
                    </div>
                    <p>
                        更多服务
                    </p>
                </a>
            </li>
        </ul>
    </div>
    <script src="/App_Themes/default/js/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="/App_Themes/default/js/swiper.min.js" type="text/javascript"></script>
    <script>        
      var mySwiper = new Swiper ('.swiper-container', {
        direction: 'horizontal',
        speed:1000,
        autoplay: 5000,
        loop: true,
        effect : 'fade',
    
      })        
  </script>
    </form>
</body>
</html>
