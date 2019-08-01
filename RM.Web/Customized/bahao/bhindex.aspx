<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="bhindex.aspx.cs" Inherits="RM.Web.Customized.bahao.bhindex" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no">
    <meta name="format-detection" content="telephone=no">
    <title>酒店首页</title>
    <link href="/App_Themes/default/css/bh.css" rel="stylesheet" type="text/css" />
    <link href="/App_Themes/default/css/swiper.min.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="banner">
        <div class="swiper-container">
            <div class="swiper-wrapper">
                <div class="swiper-slide"><img src="/App_Themes/default/images/bhimage/bg01.jpg" alt="Alternate Text" /></div>
                <div class="swiper-slide"><img src="/App_Themes/default/images/bhimage/bg02.jpg" alt="Alternate Text" /></div>
                <div class="swiper-slide"><img src="/App_Themes/default/images/bhimage/bg03.jpg" alt="Alternate Text" /></div>
            </div>
        </div>
    </div>
    <div class="logo clearfix">
        <div class="logo01">
            <img src="/App_Themes/default/images/bhimage/logo1.png" alt="Alternate Text" />
        </div>
        <div class="logo02">
            <img src="/App_Themes/default/images/bhimage/logo2.png" alt="Alternate Text" />
        </div>
    </div>
    <ul class="nav clearfix">
        <li><a href="/Reservation/HotelList.aspx?AdminHotelid=1008337">
            <img src="/App_Themes/default/images/bhimage/nav01.png" alt="Alternate Text" /></a>
        </li>
        <li><a href="/Members/MemberCantre.aspx?AdminHotelid=1008337">
            <img src="/App_Themes/default/images/bhimage/nav02.png" alt="Alternate Text" /></a>
        </li>
        <li><a href="/HotelCenter/Discount.aspx?AdminHotelid=1008337">
            <img src="/App_Themes/default/images/bhimage/nav03.png" alt="Alternate Text" /></a>
        </li>
        <li><a href="http://www.zidinn.com/PageText/DzMap.aspx?AdminHotelid=1008337&hotelid=89">
            <img src="/App_Themes/default/images/bhimage/nav04.png" alt="Alternate Text" /></a>
        </li>
    </ul>
    <div class="phone clearfix">
        <div class="phone01">
            <img src="/App_Themes/default/images/bhimage/phone.png" alt="Alternate Text" />
        </div>
        <a style="display: block;" href="tel:400-8310-009">
            <div class="phone02">
                <p>
                    预订热线
                </p>
                <p>
                    400-8310-009
                </p>
            </div>
        </a>
    </div>
    <script src="/App_Themes/default/js/jquery-1.11.3.min.js" type="text/javascript"></script>
    <script src="/App_Themes/default/js/swiper.min.js" type="text/javascript"></script>
    <script>        
      var mySwiper = new Swiper ('.swiper-container', {
        direction: 'horizontal',
        speed:5000,
        autoplay: true,
        loop: true,
        effect : 'fade',
    
      })        
  </script>
    </form>
</body>
</html>
