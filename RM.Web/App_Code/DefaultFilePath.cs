using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace RM.Web.App_Code
{
    /// <summary>
    /// DefaultFilePath 的摘要说明
    /// </summary>

    public static class DefaultFilePath
    {
        static DefaultFilePath()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 出团计划图片目录
        /// </summary>
        public static string GroupPlansImg
        {
            get { return "group"; }
        }

        /// <summary>
        /// 出团计划图片完整路径
        /// </summary>
        public static string GroupPlansImgFullPath
        {
            get { return "/upload/group/"; }
        }

        /// <summary>
        /// 出团计划行程文档目录
        /// </summary>
        public static string GroupPlansDoc
        {
            get { return "attachment"; }
        }

        /// <summary>
        /// 出团计划行程文档完整路径
        /// </summary>
        public static string GroupPlansDocFullPath
        {
            get { return "/upload/attachment/"; }
        }

        /// <summary>
        /// 包团案例图片目录
        /// </summary>
        public static string TeamCaseImg
        {
            get { return "case"; }
        }

        /// <summary>
        /// 包团案例图片完整路径
        /// </summary>
        public static string TeamCaseImgFullPath
        {
            get { return "/upload/case/"; }
        }

        /// <summary>
        /// 酒店图片目录
        /// </summary>
        public static string HotelImg
        {
            get { return "hotel"; }
        }

        /// <summary>
        /// 酒店图片完整路径
        /// </summary>
        public static string HotelImgFullPath
        {
            get { return "/upload/hotel/"; }
        }

        /// <summary>
        /// 景点图片目录
        /// </summary>
        public static string ScenicImg
        {
            get { return "scenic"; }
        }

        /// <summary>
        /// 景点图片完整路径
        /// </summary>
        public static string ScenicImgFullPath
        {
            get { return "/upload/scenic/"; }
        }

        /// <summary>
        /// 线路图片目录
        /// </summary>
        public static string LinesImg
        {
            get { return "lines"; }
        }

        /// <summary>
        /// 线路图片完整路径
        /// </summary>
        public static string LinesImgFullPath
        {
            get { return "/upload/lines/"; }
        }

        /// <summary>
        /// 会员头像目录
        /// </summary>
        public static string MemberPhoto
        {
            get { return "mem"; }
        }

        /// <summary>
        /// 会员头像完整路径
        /// </summary>
        public static string MemberPhotoFullPath
        {
            get { return "/upload/mem/"; }
        }

        /// <summary>
        /// 信息图片目录
        /// </summary>
        public static string InfoImg
        {
            get { return "info"; }
        }

        /// <summary>
        /// 信息图片完整路径
        /// </summary>
        public static string InfoImgFullPath
        {
            get { return "/upload/info/"; }
        }

        /// <summary>
        /// 邮轮图片目录
        /// </summary>
        public static string CruiseImg
        {
            get { return "cruise"; }
        }

        /// <summary>
        /// 邮轮图片完整路径
        /// </summary>
        public static string CruiseImgFullPath
        {
            get { return "/upload/cruise/"; }
        }

        /// <summary>
        /// 相册目录
        /// </summary>
        public static string AlbumImg
        {
            get { return "album"; }
        }

        /// <summary>
        /// 相册完整路径
        /// </summary>
        public static string AlbumImgFullPath
        {
            get { return "/upload/album/"; }
        }

        /// <summary>
        /// 礼品图片存放目录
        /// </summary>
        public static string GiftImg
        {
            get { return "gift"; }
        }

        /// <summary>
        /// 礼品图片完整路径
        /// </summary>
        public static string GiftImgFullPath
        {
            get { return "/upload/gift/"; }
        }

        /// <summary>
        /// 广告图存放目录 
        /// </summary>
        public static string AdvertiseImg
        {
            get { return "/advertise/"; }
        }

        /// <summary>
        /// 广告图存放完整路径
        /// </summary>
        public static string AdvertiseImgFullPath
        {
            get { return "/upload/advertise/"; }
        }

        public static string _version = "1";
        /// <summary>
        /// 设置语言版本
        /// </summary>
        public static string Version
        {
            get { return _version; }
            set { _version = value; }
        }

        public static string _ispcmobile = "1";
        /// <summary>
        /// 设置版本手机和PC
        /// </summary>
        public static string IsPcMobile
        {
            get { return _ispcmobile; }
            set { _ispcmobile = value; }
        }

        /// <summary>
        /// 系统枚举
        /// </summary>
        public enum SystemType : int
        {
            /// <summary>
            /// 单页图文(1)
            /// </summary>
            PageText = 1,

            /// <summary>
            /// 基础业务
            /// </summary>
            Info = 2,

            /// <summary>
            /// 产品展示(3)
            /// </summary>
            Product = 3,

            /// <summary>
            /// 下载分类(4)
            /// </summary>
            DownLoad = 4,

            /// <summary>
            /// 图片分类(5)
            /// </summary>
            Trans = 5,

            /// <summary>
            /// 商标热门分类(6)
            /// </summary>
            HotTrans = 6,

            /// <summary>
            /// 名标展示(7)
            /// </summary>
            MinBiaoType = 7,

            /// <summary>
            /// 酒店轮换图(9)
            /// </summary>
            HotelType = 9,

            /// <summary>
            /// 案例展示图
            /// </summary>
            ItemType = 10,

            /// <summary>
            /// 视频展示图
            /// </summary>
            VideoType = 11,

            /// <summary>
            /// 客户
            /// </summary> 
            Client = 12,

            /// <summary>
            /// 项目
            /// </summary> 
            Project = 13,

            BaseInfo = 14,

            DataInfo = 15,

            /// <summary>
            /// 评价
            /// </summary>
            Comment = 16,
            /// <summary>
            /// 营业点轮换
            /// </summary>
            Business = 17,
            /// <summary>
            /// 服务轮换
            /// </summary>
            Service = 18,
            /// <summary>
            /// 门店图片轮换
            /// </summary>
            StoresImg = 19,
            /// <summary>
            /// 实景欣赏
            /// </summary>
            LiveAppreciate = 20,
            /// <summary>
            /// 菜式轮换
            /// </summary>
            Food = 21,
            /// <summary>
            /// 餐厅轮换
            /// </summary>
            Restaurant = 22,

            /// <summary>
            /// 餐厅评价
            /// </summary>
            RestaurantComm = 23,

            /// <summary>
            /// 营业点轮换
            /// </summary>
            BusinPhoto = 24,

            /// <summary>
            /// 商城评价
            /// </summary>
            ProductComm = 25,


            /// <summary>
            /// 商城轮换
            /// </summary>
            Mall = 26,


            /// <summary>
            /// 房商图片
            /// </summary>
            RoomCommodity = 27,


            /// <summary>
            /// 预售券详情轮换
            /// </summary>
            TicketDetail = 28,

            /// <summary>
            /// 留言反馈图片
            /// </summary>
            FeedBackImg = 29




        }
    }
}