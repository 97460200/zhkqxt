using System;
using System.Collections.Generic;
using System.Text;

namespace RM.Common.DotNetBean
{
    /// <summary>
    /// 存 Session对象
    /// </summary>
    public class SessionUser
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public object UserId { get; set; }
        /// <summary>
        /// 登陆账户
        /// </summary>
        public object UserAccount { get; set; }
        /// <summary>
        /// 登陆密码
        /// </summary>
        public object UserPwd { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public object UserName { get; set; }

        /// <summary>
        /// 集团酒店ID
        /// </summary>
        public object AdminHotelid { get; set; }
        /// <summary>
        /// 所属酒店
        /// </summary>
        public object Hotelid { get; set; }
        /// <summary>
        /// 多酒店id
        /// </summary>
        public object HotelListId { get; set; }
        /// <summary>
        /// 0:超级管理员,1:集团管理员,2:酒店员工
        /// </summary>
        public object IsAdmin { get; set; }
        /// <summary>
        /// 用户角色
        /// </summary>
        public object RoleName { get; set; }

        public SessionUser(object userId, object userAccount, object userPwd, object userName, object adminHotelid, object hotelid, object hotelListId, object isAdmin, object roleName)
        {
            this.UserId = userId;
            this.UserAccount = userAccount;
            this.UserName = userName;
            this.UserPwd = userPwd;
            this.AdminHotelid = adminHotelid;
            this.Hotelid = hotelid;
            this.HotelListId = hotelListId;
            this.IsAdmin = isAdmin;
            this.RoleName = roleName;
        }

        public SessionUser()
        {
        }
    }
}
