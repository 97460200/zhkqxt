using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LitJson;
using PMS;
using RM.Common.DotNetEncrypt;
using RM.Busines;
using RM.Common.DotNetCode;
using System.Text;
using System.Data;
using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Common.DotNetUI;
using System.Collections;
using System.Net;
using System.Web.Script.Serialization;
using WxCallback.Common;

namespace PMS.api
{
    public class User
    {
        #region  ** 1. 获取小程序Openid **

        public void GetOpenid(HttpContext context, string jsonDate)
        {
            #region  ** 参数验证 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            string code = CommonUse.ParamVal(jd_data, "Code", "用户认证", true, true);

            #endregion

            string postUrl = " https://api.weixin.qq.com/sns/jscode2session?appid={0}&secret={1}&js_code={2}&grant_type=authorization_code";
            postUrl = string.Format(postUrl, "wx1e179ed4b6fe4ab0", "3a42b861d8f512167e60968744fc557d", code);
            string returnJason = GetJson(postUrl);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, object> dtJson = (Dictionary<string, object>)serializer.DeserializeObject(returnJason);

            JsonData jd = new JsonData();//返回JsonData
            if (dtJson != null)
            {
                object value = null;
                JsonData jdData = new JsonData();//DataTable值
                jdData["openid"] = "";
                if (dtJson.TryGetValue("openid", out value))
                {
                    jdData["openid"] = value.ToString();
                }
                //开放平台unionid
                if (dtJson.TryGetValue("unionid", out value))
                {
                    jdData["unionid"] = value.ToString();
                }
                jd["code"] = 1;
                jd["data"] = jdData;
            }
            else
            {
                jd["code"] = 0;
                jd["message"] = "未查询到相关信息!";
            }
            string json = jd.ToJson();
            // 统一输出
            PMSResponse.WirterString(json);
        }

        //访问微信url并返回微信信息
        protected string GetJson(string url)
        {
            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            wc.Encoding = Encoding.UTF8;
            string returnText = wc.DownloadString(url);
            if (returnText.Contains("errcode"))
            {
                //可能发生错误
            }
            return returnText;
        }

        #endregion

        #region  ** 登录信息 **

        private void LoginInfo(HttpContext context, DataTable dtlogin, string openid)
        {
            if (dtlogin == null || dtlogin.Rows.Count == 0)
            {
                throw new PMSException("账户或者密码有错误！");
            }
            RM_UserInfo_IDAO user_idao = new RM_UserInfo_Dal();
            string UserName = dtlogin.Rows[0]["User_Account"].ToString();
            string adminHotelId = dtlogin.Rows[0]["AdminHotelid"].ToString();
            string hotelId = dtlogin.Rows[0]["Hotelid"].ToString();

            JsonData jd = new JsonData();//返回JsonData
            JsonData jdData = new JsonData();//DataTable值
            string uid = dtlogin.Rows[0]["User_ID"].ToString();
            Hashtable ht = new Hashtable();
            ht["pms_openid"] = openid;
            ht["IsDefault"] = 0;//默认登录账号
            DataFactory.SqlDataBase().UpdateByHashtable("Base_UserInfo", "User_ID", uid, ht);

            jdData["User_ID"] = uid;
            jdData["User_Account"] = UserName;
            jdData["AdminHotelid"] = adminHotelId;
            jdData["Hotelid"] = hotelId;
            jdData["User_Name"] = dtlogin.Rows[0]["User_Name"].ToString();
            jdData["UserPwd"] = dtlogin.Rows[0]["User_Pwd"].ToString();
            jdData["HotelListId"] = dtlogin.Rows[0]["HotelListId"].ToString();
            jdData["HotelName"] = dtlogin.Rows[0]["HotelName"].ToString();
            jdData["HotelCode"] = DESEncrypt.Encrypt(adminHotelId + "," + hotelId + "," + uid);
            string User_Phone = dtlogin.Rows[0]["User_Phone"].ToString();
            jdData["User_Phone"] = User_Phone.ToString().Substring(0, 3) + "****" + User_Phone.ToString().Substring(User_Phone.ToString().Length - 4, 4);
            string IsAdmin = dtlogin.Rows[0]["IsAdmin"].ToString();
            string roleName = "";
            if (IsAdmin == "0")
            {
                roleName = "管理员";
            }
            else if (IsAdmin == "1")
            {
                roleName = "管理员";
            }
            else
            {
                roleName = dtlogin.Rows[0]["Roles_Name"].ToString();
            }
            jdData["IsAdmin"] = IsAdmin;
            jdData["RoleName"] = roleName;
            jdData["DataType"] = CheckHotelData(hotelId);

            jd["code"] = 1;
            jd["data"] = jdData;
            string json = jd.ToJson();
            // 统一输出
            PMSResponse.WirterString(json);
        }

        private string CheckHotelData(string hotelId)
        {
            string val = "0";
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
            Select  IsNull(a.sjcx_jdid, '0') sjcx_jdid ,
                    IsNull(b.[Servers], '') SqlCon
            From    dbo.Hotel a
                    Left Join dbo.Set_Association b On a.ID = b.hotelid
            Where   a.ID = @hotelId
            ");

            IList<SqlParam> lParam = new List<SqlParam>();
            lParam.Add(new SqlParam("@hotelId", hotelId));
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, lParam.ToArray());
            if (dt != null && dt.Rows.Count > 0)
            {
                string sjcx_jdid = dt.Rows[0]["sjcx_jdid"].ToString();//酒店ID
                string SqlCon = dt.Rows[0]["SqlCon"].ToString();//数据库连接
                if (SqlCon == "" && sjcx_jdid != "" && sjcx_jdid != "0")
                {
                    val = sjcx_jdid;
                }
            }
            return val;
        }
        #endregion

        #region  ** 10. 用户名登录 **

        public void Login(HttpContext context, string jsonDate)
        {
            #region  ** 参数验证 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            string UserName = CommonUse.ParamVal(jd_data, "UserName", "用户名", true, true);
            string Password = CommonUse.ParamVal(jd_data, "Password", "密码", true, true);
            string openid = CommonUse.ParamVal(jd_data, "openid", "小程序ID", true, true);

            #endregion

            RM_UserInfo_IDAO user_idao = new RM_UserInfo_Dal();
            DataTable dtlogin = user_idao.UserLogin(UserName, Password);
            if (dtlogin != null)
            {
                if (dtlogin.Rows.Count != 0)
                {
                    #region ** 登录日志 **
                    IPScanerHelper objScan = new IPScanerHelper();
                    objScan.DataPath = context.Server.MapPath("/Scripts/IPScaner/QQWry.Dat");
                    objScan.IP = RequestHelper.GetIP();
                    string OWNER_address = objScan.IPLocation();
                    string adminHotelId = dtlogin.Rows[0]["AdminHotelid"].ToString();
                    string hotelId = dtlogin.Rows[0]["Hotelid"].ToString();

                    if (hotelId == "0")
                    {
                        throw new PMSException("账户或者密码有错误！");
                    }
                    user_idao.SysLoginLog(1, 3, UserName, "100", OWNER_address, dtlogin.Rows[0]["User_ID"].ToString(), adminHotelId);
                    #endregion

                    LoginInfo(context, dtlogin, openid);//返回登录信息

                }
                else
                {
                    throw new PMSException("账户或者密码有错误！");
                }
            }
            else
            {
                throw new PMSException("服务连接失败，请稍后再试！");
            }
        }
        #endregion

        #region  ** 11. 手机号登录 **

        public void PhoneLogin(HttpContext context, string jsonDate)
        {
            #region  ** 参数验证 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            string Phone = CommonUse.ParamVal(jd_data, "Phone", "手机号码", true, true);
            string Code = CommonUse.ParamVal(jd_data, "Code", "验证码", true, true);
            string openid = CommonUse.ParamVal(jd_data, "openid", "小程序ID", true, true);

            #endregion
            bool blPC = ComClass.CheckPhoneCode(Phone, Code);
            if (!blPC)
            {
                throw new PMSException("验证码错误或超时！");
            }

            RM_UserInfo_IDAO user_idao = new RM_UserInfo_Dal();
            DataTable dtlogin = user_idao.PhoneLogin(Phone);

            if (dtlogin == null || dtlogin.Rows.Count == 0)
            {
                throw new PMSException("手机号不存在！");
            }

            #region ** 登录日志 **
            IPScanerHelper objScan = new IPScanerHelper();
            objScan.DataPath = context.Server.MapPath("/Scripts/IPScaner/QQWry.Dat");
            objScan.IP = RequestHelper.GetIP();
            string OWNER_address = objScan.IPLocation();
            string adminHotelId = dtlogin.Rows[0]["AdminHotelid"].ToString();
            string hotelId = dtlogin.Rows[0]["Hotelid"].ToString();

            if (hotelId == "0")
            {
                throw new PMSException("账户或者密码有错误！");
            }

            Hashtable ht = new Hashtable();
            ht["pms_openid"] = openid;
            ht["IsDefault"] = 1;
            DataFactory.SqlDataBase().UpdateByHashtable("Base_UserInfo", "User_Phone", Phone, ht);

            user_idao.SysLoginLog(2, 3, "", "1", OWNER_address, dtlogin.Rows[0]["User_ID"].ToString(), adminHotelId);
            #endregion

            LoginInfo(context, dtlogin, openid);//返回登录信息
        }
        #endregion

        #region  ** 12. 找回密码 **
        //第一步 手机验证码
        public void RetrievePwd(HttpContext context, string jsonDate)
        {
            #region  ** 参数验证 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            string Phone = CommonUse.ParamVal(jd_data, "Phone", "手机号码", true, true);
            string Code = CommonUse.ParamVal(jd_data, "Code", "验证码", true, true);

            #endregion
            bool blPC = ComClass.CheckPhoneCode(Phone, Code);
            if (!blPC)
            {
                throw new PMSException("验证码错误或超时！");
            }
            JsonData jd = new JsonData();//返回JsonData

            StringBuilder sql = new StringBuilder();
            sql.Append(@"
            Select  b.name HotelName ,
                    a.hotelid ,
                    a.User_ID ,
                    a.User_Account ,
                    a.User_Phone
            From    Base_UserInfo a
                    Inner Join Hotel b On a.hotelid = b.ID
            Where   a.User_Phone = @User_Phone
            ");
            IList<SqlParam> lParam = new List<SqlParam>();
            lParam.Add(new SqlParam("@User_Phone", Phone));

            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, lParam.ToArray());

            if (dt == null || dt.Rows.Count == 0)
            {
                jd["code"] = 0;
                jd["message"] = "手机账号不存在，请先根据旧用户重置账号或联系管理员添加！";
            }
            else
            {
                JsonData jdData = new JsonData();//DataTable值
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonData jdList = new JsonData();
                    jdList["HotelName"] = dt.Rows[i]["HotelName"].ToString();
                    jdList["HotelId"] = dt.Rows[i]["hotelid"].ToString();
                    jdList["UserId"] = dt.Rows[i]["User_ID"].ToString();
                    jdList["UserAccount"] = dt.Rows[i]["User_Account"].ToString();
                    jdList["UserPhone"] = dt.Rows[i]["User_Phone"].ToString();
                    jdData.Add(jdList);
                }
                jd["code"] = 1;
                jd["data"] = jdData;
            }
            string json = jd.ToJson();
            // 统一输出
            PMSResponse.WirterString(json);
        }
        //第二部 修改密码
        public void UpdatePwd(HttpContext context, string jsonDate)
        {
            #region  ** 参数验证 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            string UserId = CommonUse.ParamVal(jd_data, "UserId", "用户ID", true, true);
            string Password = CommonUse.ParamVal(jd_data, "Password", "密码", true, true);
            string openid = CommonUse.ParamVal(jd_data, "openid", "小程序ID", true, true);
            #endregion

            JsonData jd = new JsonData();//返回JsonData
            Hashtable ht = new Hashtable();
            ht["User_Pwd"] = Md5Helper.MD5(Password, 32);
            ht["IsDefault"] = 1;
            bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("Base_UserInfo", "User_ID", UserId, ht);
            if (!IsOk)
            {
                throw new PMSException("密码修改失败！");
            }

            RM_UserInfo_IDAO user_idao = new RM_UserInfo_Dal();
            DataTable dtlogin = user_idao.UserIdLogin(UserId);

            if (dtlogin == null || dtlogin.Rows.Count == 0)
            {
                throw new PMSException("操作失败！");
            }

            #region ** 登录日志 **
            IPScanerHelper objScan = new IPScanerHelper();
            objScan.DataPath = context.Server.MapPath("/Scripts/IPScaner/QQWry.Dat");
            objScan.IP = RequestHelper.GetIP();
            string OWNER_address = objScan.IPLocation();
            user_idao.SysLoginLog(2, 3, "", "1", OWNER_address, dtlogin.Rows[0]["User_ID"].ToString(), dtlogin.Rows[0]["adminHotelId"].ToString());
            #endregion

            LoginInfo(context, dtlogin, openid);//返回登录信息
        }

        #endregion

        #region  ** 20. 用户级别信息 **

        #endregion

        #region  ** 30. 酒店信息 **

        public void HotelInfo(HttpContext context, string jsonDate)
        {
            #region  ** 解码酒店编号 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            string HotelCode = CommonUse.ParamVal(jd_data, "HotelCode", "酒店编号", true, true);
            string sign = CommonUse.ParamVal(jd_data, "Sign", "签名", true, true);

            string sign_new = Md5Helper.MD5(HotelCode, 32);
            if (sign.ToLower() != sign_new.ToLower())
            {
                throw new PMSException("签名验证失败！");
            }

            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length < 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }
            #endregion

            string adminHotelId = keyValue[0];
            string hotelId = keyValue[1];
            string userId = keyValue[2];


            StringBuilder sql = new StringBuilder();
            sql.Append(@"
            Select  a.name ,
                    a.phone ,
                    a.Address ,
                    a.logo ,
                    b.ExpireTime
            From    dbo.Hotel a
                    Left Join MobilePMS b On a.ID = b.HotelId
            Where   a.ID = @HotelId
            ");

            JsonData jdData = new JsonData();//DataTable值
            IList<SqlParam> lParam = new List<SqlParam>();
            lParam.Add(new SqlParam("@HotelId", hotelId));

            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, lParam.ToArray());
            JsonData jd = new JsonData();//返回JsonData
            if (dt != null && dt.Rows.Count > 0)
            {
                JsonData jdList = new JsonData();
                jdList["HotelName"] = dt.Rows[0]["name"].ToString();//酒店名称
                jdList["HotelPhone"] = dt.Rows[0]["phone"].ToString();//电话
                jdList["Address"] = dt.Rows[0]["Address"].ToString();//地址
                jdList["LoGo"] = dt.Rows[0]["logo"].ToString();//LoGo

                jdList["Edition"] = "VIP标准版";//版本
                jdList["ExpiryDate"] = CommonHelper.GetDateTime(dt.Rows[0]["ExpireTime"]).ToString("yyyy-MM-dd");//有效期
                jdList["Email"] = "service@zidinn.com";//邮箱

                jdData.Add(jdList);

                jd["code"] = 1;
                jd["data"] = jdData;
            }
            else
            {
                jd["code"] = 0;
                jd["message"] = "未查询到相关信息!";
            }
            string json = jd.ToJson();
            // 统一输出
            PMSResponse.WirterString(json);
        }

        #endregion

        #region  ** 40. 用户管理 **

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="jsonDate"></param>
        public void GetUserInfo(HttpContext context, string jsonDate)
        {
            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            string userId = CommonUse.ParamVal(jd_data, "UserId", "用户ID", true, true);
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
            Select  User_Name ,
                    User_Account ,
                    User_Phone ,
                    User_Sex
            From    dbo.Base_UserInfo
            Where   User_Id = @User_Id 
            ");

            IList<SqlParam> lParam = new List<SqlParam>();
            lParam.Add(new SqlParam("@User_Id", userId));

            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, lParam.ToArray());
            JsonData jd = new JsonData();//返回JsonData
            if (dt == null || dt.Rows.Count == 0)
            {
                throw new PMSException("操作失败，用户ID不存在！");
            }
            JsonData jdData = new JsonData();//值
            jdData["User_Account"] = dt.Rows[0]["User_Account"].ToString();
            jdData["User_Name"] = dt.Rows[0]["User_Name"].ToString();
            jdData["User_Sex"] = dt.Rows[0]["User_Sex"].ToString();
            jdData["User_Phone"] = dt.Rows[0]["User_Phone"].ToString();
            jd["code"] = 1;
            jd["data"] = jdData;
            string json = jd.ToJson();
            // 统一输出
            PMSResponse.WirterString(json);
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="jsonDate"></param>
        public void SetUserInfo(HttpContext context, string jsonDate)
        {
            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            string userId = CommonUse.ParamVal(jd_data, "UserId", "用户ID", true, true);
            //string User_Account = CommonUse.ParamVal(jd_data, "User_Account", "用户名", true, true);
            string User_Name = CommonUse.ParamVal(jd_data, "User_Name", "姓名", true, true);
            string User_Sex = CommonUse.ParamVal(jd_data, "User_Sex", "性别", true, true);

            Hashtable ht = new Hashtable();
            //ht["User_Account"] = User_Account;
            ht["User_Name"] = User_Name;
            ht["User_Sex"] = User_Sex;
            int val = DataFactory.SqlDataBase().UpdateByHashtable("Base_UserInfo", "User_ID", userId, ht);
            JsonData jd = new JsonData();//返回JsonData
            if (val > 0)
            {
                jd["code"] = 1;
                jd["data"] = "修改成功！";
            }
            else
            {
                jd["code"] = 0;
                jd["message"] = "操作失败！";
            }
            string json = jd.ToJson();
            // 统一输出
            PMSResponse.WirterString(json);
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="context"></param>
        /// <param name="jsonDate"></param>
        public void UpdateUserPwd(HttpContext context, string jsonDate)
        {
            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            string userId = CommonUse.ParamVal(jd_data, "UserId", "用户ID", true, true);
            string OldPwd = CommonUse.ParamVal(jd_data, "OldPwd", "旧密码", true, true);
            string NewPwd = CommonUse.ParamVal(jd_data, "NewPwd", "新密码", true, true);

            StringBuilder sql = new StringBuilder();
            sql.Append(@"
            Select  User_Pwd 
            From    dbo.Base_UserInfo
            Where   User_Id = @User_Id 
            ");

            IList<SqlParam> lParam = new List<SqlParam>();
            lParam.Add(new SqlParam("@User_Id", userId));

            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, lParam.ToArray());
            JsonData jd = new JsonData();//返回JsonData
            if (dt == null || dt.Rows.Count == 0)
            {
                throw new PMSException("操作失败，用户ID不存在！");
            }
            string up = dt.Rows[0]["User_Pwd"].ToString();
            string op = Md5Helper.MD5(OldPwd, 32);
            if (up != op)
            {
                throw new PMSException("操作失败，旧密码错误！");
            }
            string np = Md5Helper.MD5(NewPwd, 32);
            Hashtable ht = new Hashtable();
            ht["User_Pwd"] = np;
            int val = DataFactory.SqlDataBase().UpdateByHashtable("Base_UserInfo", "User_ID", userId, ht);
            if (val > 0)
            {
                jd["code"] = 1;
                jd["data"] = "修改成功！";
            }
            else
            {
                jd["code"] = 0;
                jd["message"] = "修改失败！";
            }
            string json = jd.ToJson();
            // 统一输出
            PMSResponse.WirterString(json);
        }

        /// <summary>
        /// 修改用户手机号码
        /// </summary>
        /// <param name="context"></param>
        /// <param name="jsonDate"></param>
        public void UpdateUserPhone(HttpContext context, string jsonDate)
        {
            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            string userId = CommonUse.ParamVal(jd_data, "UserId", "用户ID", true, true);
            string Phone = CommonUse.ParamVal(jd_data, "Phone", "手机号码", true, true);
            string Code = CommonUse.ParamVal(jd_data, "Code", "验证码", true, true);

            bool blPC = ComClass.CheckPhoneCode(Phone, Code);
            if (!blPC)
            {
                throw new PMSException("验证码错误或超时！");
            }
            Hashtable ht = new Hashtable();
            ht["User_Phone"] = Phone;
            int val = DataFactory.SqlDataBase().UpdateByHashtable("Base_UserInfo", "User_ID", userId, ht);
            JsonData jd = new JsonData();//返回JsonData
            if (val > 0)
            {
                jd["code"] = 1;
                jd["data"] = "修改成功！";
            }
            else
            {
                jd["code"] = 0;
                jd["message"] = "修改失败！";
            }
            string json = jd.ToJson();
            // 统一输出
            PMSResponse.WirterString(json);
        }

        #endregion

        #region  ** 50. 用户权限设置 **
        #endregion

        #region  ** 60. 版本 **

        //版本列表
        public void EditionList(HttpContext context, string jsonDate)
        {
            #region  ** 解码酒店编号 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            string HotelCode = CommonUse.ParamVal(jd_data, "HotelCode", "酒店编号", true, true);
            string sign = CommonUse.ParamVal(jd_data, "Sign", "签名", true, true);

            //string sign_new = Md5Helper.MD5(HotelCode, 32);
            //if (sign.ToLower() != sign_new.ToLower())
            //{
            //    throw new PMSException("签名验证失败！");
            //}

            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length < 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }
            #endregion

            StringBuilder sql = new StringBuilder();
            sql.Append(@"
            SELECT TOP 10
                    ID ,
                    Title ,
                    Sender ,
                    Summary ,
                    ContentEn ,
                    AddTime
            FROM    dbo.Info
            WHERE   KindId = 1396
                    AND AdminHotelid = '1'
            ORDER BY Sort ASC ,
                    ID DESC
            ");

            JsonData jdData = new JsonData();//DataTable值

            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql);
            JsonData jd = new JsonData();//返回JsonData
            if (dt != null && dt.Rows.Count > 0)
            {
                string EditionCode = "V6.2.23";
                EditionCode = dt.Rows[0]["Sender"].ToString();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonData jdList = new JsonData();
                    jdList["Id"] = dt.Rows[i]["ID"].ToString();//ID
                    jdList["Title"] = dt.Rows[i]["Title"].ToString();//标题
                    jdList["Summary"] = dt.Rows[i]["Summary"].ToString();//简介
                    jdList["Content"] = dt.Rows[i]["ContentEn"].ToString();//详细内容
                    jdList["AddTime"] = CommonHelper.GetDateTime(dt.Rows[i]["AddTime"]).ToString("yyyy-MM-dd");//时间
                    jdData.Add(jdList);
                }
                jd["code"] = 1;
                jd["EditionCode"] = EditionCode;
                jd["data"] = jdData.IsArray ? jdData : false;
            }
            else
            {
                jd["code"] = 0;
                jd["message"] = "未查询到相关信息!";
            }
            string json = jd.ToJson();
            // 统一输出
            PMSResponse.WirterString(json);
        }

        //版本详情
        public void EditionDetail(HttpContext context, string jsonDate)
        {
            #region  ** 解码酒店编号 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            string HotelCode = CommonUse.ParamVal(jd_data, "HotelCode", "酒店编号", true, true);
            string ID = CommonUse.ParamVal(jd_data, "ID", "版本ID", true, true);
            string sign = CommonUse.ParamVal(jd_data, "Sign", "签名", true, true);

            //string sign_new = Md5Helper.MD5(HotelCode, 32);
            //if (sign.ToLower() != sign_new.ToLower())
            //{
            //    throw new PMSException("签名验证失败！");
            //}

            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length < 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }
            #endregion

            StringBuilder sql = new StringBuilder();
            sql.Append(@"
            SELECT TOP 10
                    ID ,
                    Title ,
                    Summary ,
                    ContentEn ,
                    AddTime
            FROM    dbo.Info
            WHERE   KindId = 1396
                    AND AdminHotelid = '1'
                    AND ID = @ID
            ORDER BY Sort ASC ,
                    ID DESC
            ");

            JsonData jdData = new JsonData();//DataTable值

            IList<SqlParam> lParam = new List<SqlParam>();
            lParam.Add(new SqlParam("@ID", ID));

            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, lParam.ToArray());
            JsonData jd = new JsonData();//返回JsonData
            if (dt != null && dt.Rows.Count > 0)
            {
                JsonData jdList = new JsonData();
                jdList["Id"] = dt.Rows[0]["ID"].ToString();//ID
                jdList["Title"] = dt.Rows[0]["Title"].ToString();//标题
                jdList["Summary"] = dt.Rows[0]["Summary"].ToString();//简介
                jdList["Content"] = dt.Rows[0]["ContentEn"].ToString();//详细内容
                jdList["AddTime"] = CommonHelper.GetDateTime(dt.Rows[0]["AddTime"]).ToString("yyyy-MM-dd");//时间
                jdData.Add(jdList);
                jd["code"] = 1;
                jd["data"] = jdData.IsArray ? jdData : false;
            }
            else
            {
                jd["code"] = 0;
                jd["message"] = "未查询到相关信息!";
            }
            string json = jd.ToJson();
            // 统一输出
            PMSResponse.WirterString(json);
        }
        #endregion

        #region  ** 70. 反馈 **
        #endregion

        #region  ** 80. 切换账号 **
        //账号列表
        public void UserList(HttpContext context, string jsonDate)
        {
            #region  ** 参数验证 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            string openid = CommonUse.ParamVal(jd_data, "openid", "小程序ID", true, true);

            #endregion

            StringBuilder sql = new StringBuilder();
            sql.Append(@"
            Select  b.name HotelName ,
                    IsNull(b.Logo, '') Logo ,
                    User_ID ,
                    User_Account ,
                    User_Phone
            From    Base_UserInfo a
                    Left Join dbo.Hotel b On a.hotelid = b.ID
            Where   pms_openid = @pms_openid and a.DeleteMark <> 0 
            ");

            IList<SqlParam> lParam = new List<SqlParam>();
            lParam.Add(new SqlParam("@pms_openid", openid));

            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, lParam.ToArray());

            if (dt != null && dt.Rows.Count > 0)
            {
                JsonData jd = new JsonData();//返回JsonData
                JsonData jdData = new JsonData();//DataTable值
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonData jdList = new JsonData();
                    jdList["HotelName"] = dt.Rows[i]["HotelName"].ToString();
                    jdList["User_ID"] = dt.Rows[i]["User_ID"].ToString();
                    jdList["User_Account"] = dt.Rows[i]["User_Account"].ToString();
                    string User_Phone = dt.Rows[i]["User_Phone"].ToString();
                    jdList["User_Phone"] = User_Phone.ToString().Substring(0, 3) + "****" + User_Phone.ToString().Substring(User_Phone.ToString().Length - 4, 4);
                    string lg = dt.Rows[i]["Logo"].ToString();
                    if (lg.Length > 4)
                    {
                        lg = "http://www.zidinn.com/upload/image/" + lg;
                    }
                    jdList["Logo"] = lg;
                    jdData.Add(jdList);
                }
                jd["code"] = 1;
                jd["data"] = jdData;
                string json = jd.ToJson();
                // 统一输出
                PMSResponse.WirterString(json);
            }
            else
            {
                throw new PMSException("服务连接失败，请稍后再试！");
            }
        }

        //账号切换
        public void UserSwitch(HttpContext context, string jsonDate)
        {
            #region  ** 参数验证 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            string userid = CommonUse.ParamVal(jd_data, "UserId", "用户ID", true, true);
            string openid = CommonUse.ParamVal(jd_data, "openid", "小程序ID", true, true);
            #endregion

            RM_UserInfo_IDAO user_idao = new RM_UserInfo_Dal();
            DataTable dtlogin = user_idao.UserIdLogin(userid);

            if (dtlogin == null || dtlogin.Rows.Count == 0)
            {
                throw new PMSException("切换失败！");
            }

            #region ** 登录日志 **
            IPScanerHelper objScan = new IPScanerHelper();
            objScan.DataPath = context.Server.MapPath("/Scripts/IPScaner/QQWry.Dat");
            objScan.IP = RequestHelper.GetIP();
            string OWNER_address = objScan.IPLocation();
            user_idao.SysLoginLog(2, 3, "", "1", OWNER_address, dtlogin.Rows[0]["User_ID"].ToString(), dtlogin.Rows[0]["adminHotelId"].ToString());
            #endregion

            LoginInfo(context, dtlogin, openid);//返回登录信息
        }


        #endregion

        #region  ** 90. 旧用户账号验证 **
        public void CheckUser(HttpContext context, string jsonDate)
        {
            #region  ** 参数验证 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            string sjcx_jdid = CommonUse.ParamVal(jd_data, "DataId", "酒店id", true, true);
            string UserName = CommonUse.ParamVal(jd_data, "UserName", "用户名", true, true);
            string Password = CommonUse.ParamVal(jd_data, "Password", "密码", true, true);

            #endregion

            StringBuilder sql = new StringBuilder();
            sql.Append(@"
            Select  x1.jdid, x1.[ygh], x1.password, x2.jdname, x2.Status, x1.jid
            From    [XX_JDDLXXB] x1
                    Full Join XX_JDXXB x2 On x1.jdid = x2.jdid
            Where   x1.[jdid] = @jdid And x1.[ygh] = @ygh And x1.[password] = @password
            ");

            IList<SqlParam> lParam = new List<SqlParam>();
            lParam.Add(new SqlParam("@jdid", sjcx_jdid));
            lParam.Add(new SqlParam("@ygh", UserName));
            lParam.Add(new SqlParam("@password", Password));
            string adminHotelId = "1010142";//国光数据查询
            DataTable dt = DataFactory.SqlDataBase(adminHotelId).GetDataTableBySQL(sql, lParam.ToArray());
            JsonData jd = new JsonData();//返回JsonData
            if (dt != null && dt.Rows.Count > 0)
            {
                sql = new StringBuilder();
                sql.Append(" Select Id From dbo.Hotel Where sjcx_jdid=@sjcx_jdid ");
                lParam = new List<SqlParam>();
                lParam.Add(new SqlParam("@sjcx_jdid", sjcx_jdid));
                DataTable dtHotel = DataFactory.SqlDataBase().GetDataTableBySQL(sql, lParam.ToArray());
                if (dtHotel != null && dtHotel.Rows.Count == 1)
                {
                    JsonData jdData = new JsonData();//DataTable值
                    jdData["HotelId"] = dtHotel.Rows[0]["Id"].ToString();//酒店ID
                    jd["code"] = 1;
                    jd["data"] = jdData;
                }
                else
                {
                    jd["code"] = 0;
                    jd["message"] = "未关联相应酒店信息，请联系管理员进行关联！";
                }
            }
            else
            {
                jd["code"] = 0;
                jd["message"] = "账户或者密码有错误！";
            }
            string json = jd.ToJson();
            // 统一输出
            PMSResponse.WirterString(json);
        }
        #endregion

        #region  ** 91. 旧用户 设置新账号 **

        public void SetUser(HttpContext context, string jsonDate)
        {
            #region  ** 参数验证 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            string HotelId = CommonUse.ParamVal(jd_data, "HotelId", "酒店id", true, true);
            string Phone = CommonUse.ParamVal(jd_data, "Phone", "手机号码", true, true).Trim();
            string Code = CommonUse.ParamVal(jd_data, "Code", "验证码", true, true).Trim();
            string Password = CommonUse.ParamVal(jd_data, "Password", "密码", true, true).Trim();
            string openid = CommonUse.ParamVal(jd_data, "openid", "小程序ID", true, true);

            #endregion

            bool blPC = ComClass.CheckPhoneCode(Phone, Code);
            if (!blPC)
            {
                throw new PMSException("验证码错误或超时！");
            }
            string adminHotelId = "";
            string hotelName = "";
            ComClass.GetHotelName(HotelId, out adminHotelId, out hotelName);
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
            Select  User_ID ,
                    User_Account ,
                    User_Phone ,
                    IsNull(pms_openid, '') openid
            From    Base_UserInfo
            Where   HotelId = @HotelId
                    And User_Phone = @User_Phone
            ");

            IList<SqlParam> lParam = new List<SqlParam>();
            lParam.Add(new SqlParam("@HotelId", HotelId));
            lParam.Add(new SqlParam("@User_Phone", Phone));

            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, lParam.ToArray());
            JsonData jd = new JsonData();//返回JsonData

            string userid = CommonHelper.GetGuid;
            if (dt != null && dt.Rows.Count > 0)
            {
                userid = dt.Rows[0]["User_ID"].ToString();
                string ua = dt.Rows[0]["User_Account"].ToString();
                string up = dt.Rows[0]["User_Phone"].ToString();
                string ydlopenid = dt.Rows[0]["openid"].ToString();
                if (ydlopenid.Length > 4)
                {
                    jd["code"] = 0;
                    jd["message"] = "该账号已经存在，如忘记密码请重新找回密码！";
                }
                if (ua == up)//旧用户
                {
                    ua = NewUserName(hotelName, Phone);
                    Hashtable ht = new Hashtable();
                    ht["User_Account"] = ua;
                    ht["pms_openid"] = openid;
                    ht["IsDefault"] = 1;
                    DataFactory.SqlDataBase().UpdateByHashtable("Base_UserInfo", "User_ID", userid, ht);
                }
            }
            else
            {
                string ua = NewUserName(hotelName, Phone);
                Hashtable ht = new Hashtable();
                ht["IsAdmin"] = 2;
                ht["AdminHotelId"] = adminHotelId;
                ht["HotelId"] = HotelId;
                ht["HotelListId"] = HotelId;
                ht["User_Account"] = ua;
                ht["User_Phone"] = Phone;
                ht["User_Pwd"] = Md5Helper.MD5(Password, 32);
                ht["DeleteMark"] = 1;
                ht["User_Sex"] = 1;
                ht["pms_openid"] = openid;
                ht["IsDefault"] = 1;
                ht["User_Name"] = "";
                ht["User_ID"] = userid;
                bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("Base_UserInfo", "User_ID", "", ht);
                if (!IsOk)
                {
                    throw new PMSException("用户添加失败！");
                }
            }

            RM_UserInfo_IDAO user_idao = new RM_UserInfo_Dal();
            DataTable dtlogin = user_idao.UserIdLogin(userid);

            if (dtlogin == null || dtlogin.Rows.Count == 0)
            {
                throw new PMSException("用户添加失败！");
            }

            #region ** 登录日志 **
            IPScanerHelper objScan = new IPScanerHelper();
            objScan.DataPath = context.Server.MapPath("/Scripts/IPScaner/QQWry.Dat");
            objScan.IP = RequestHelper.GetIP();
            string OWNER_address = objScan.IPLocation();
            user_idao.SysLoginLog(2, 3, "", "1", OWNER_address, dtlogin.Rows[0]["User_ID"].ToString(), adminHotelId);
            #endregion

            LoginInfo(context, dtlogin, openid);//返回登录信息

        }

        private string NewUserName(string HotelName, string Phone)
        {
            string hn_py = new PinyinHelper().mkPinyinString(HotelName);
            if (hn_py.Length > 4)
            {
                hn_py = hn_py.Substring(0, 4);
            }
            return hn_py.ToLower() + Phone;
        }
        #endregion

        #region  ** 100. 续费信息 **

        public void RenewFeeInfo(HttpContext context, string jsonDate)
        {
            #region  ** 解码酒店编号 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            string HotelCode = CommonUse.ParamVal(jd_data, "HotelCode", "酒店编号", true, true);
            string sign = CommonUse.ParamVal(jd_data, "Sign", "签名", true, true);

            string sign_new = Md5Helper.MD5(HotelCode, 32);
            if (sign.ToLower() != sign_new.ToLower())
            {
                throw new PMSException("签名验证失败！");
            }

            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length < 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }
            #endregion

            string adminHotelId = keyValue[0];
            string hotelId = keyValue[1];
            string userId = keyValue[2];

            StringBuilder sql = new StringBuilder();
            sql.Append(@"Select ExpireTime,Price,OneYearFee,TwoYearFee,ThreeYearFee From MobilePMS Where HotelId = @HotelId ");

            IList<SqlParam> lParam = new List<SqlParam>();
            lParam.Add(new SqlParam("@HotelId", hotelId));

            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, lParam.ToArray());
            JsonData jd = new JsonData();//返回JsonData
            if (dt == null || dt.Rows.Count == 0)
            {
                jd["code"] = 0;
                jd["message"] = "操作失败！";
            }
            else
            {

                DateTime endDate = CommonHelper.GetDateTime(dt.Rows[0]["ExpireTime"]);
                double price = CommonHelper.GetDouble(dt.Rows[0]["Price"]);
                double OneYearFee = CommonHelper.GetDouble(dt.Rows[0]["OneYearFee"]);
                double TwoYearFee = CommonHelper.GetDouble(dt.Rows[0]["TwoYearFee"]);
                double ThreeYearFee = CommonHelper.GetDouble(dt.Rows[0]["ThreeYearFee"]);

                JsonData jdData = new JsonData();//DataTable值
                JsonData jdList = new JsonData();
                if (OneYearFee > 0)
                {
                    jdList = new JsonData();
                    jdList["Years"] = 1;
                    jdList["StartDate"] = endDate.ToString("yyyy-MM-dd");
                    jdList["EndDate"] = endDate.AddYears(1).ToString("yyyy-MM-dd");
                    jdList["PayMoney"] = CommonHelper.GetInt(price);
                    jdData.Add(jdList);
                }
                if (TwoYearFee > 0)
                {
                    jdList = new JsonData();
                    jdList["Years"] = 2;
                    jdList["StartDate"] = endDate.ToString("yyyy-MM-dd");
                    jdList["EndDate"] = endDate.AddYears(2).ToString("yyyy-MM-dd");
                    jdList["PayMoney"] = CommonHelper.GetInt(price * 2 * 0.95);
                    jdData.Add(jdList);
                }
                if (ThreeYearFee > 0)
                {
                    jdList = new JsonData();
                    jdList["Years"] = 3;
                    jdList["StartDate"] = endDate.ToString("yyyy-MM-dd");
                    jdList["EndDate"] = endDate.AddYears(3).ToString("yyyy-MM-dd");
                    jdList["PayMoney"] = CommonHelper.GetInt(price * 3 * 0.9);
                    jdData.Add(jdList);
                }

                jd["code"] = 1;
                jd["data"] = jdData;
                jd["Edition"] = "VIP标准版";//版本
                jd["ExpireTime"] = endDate.ToString("yyyy-MM-dd");
            }
            string json = jd.ToJson();
            // 统一输出
            PMSResponse.WirterString(json);
        }

        #endregion

        #region  ** 101. 续费支付 **

        public void RenewFeePay(HttpContext context, string jsonDate)
        {
            #region  ** 解码酒店编号 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            string HotelCode = CommonUse.ParamVal(jd_data, "HotelCode", "酒店编号", true, true);
            string Years = CommonUse.ParamVal(jd_data, "Years", "续费年限", true, true);
            string StartDate = CommonUse.ParamVal(jd_data, "StartDate", "当前前到期日", true, true);
            string EndDate = CommonUse.ParamVal(jd_data, "EndDate", "续费后到期日", true, true);
            string PayMoney = CommonUse.ParamVal(jd_data, "PayMoney", "支付金额", true, true);
            string openid = CommonUse.ParamVal(jd_data, "openid", "openid", true, true);
            string sign = CommonUse.ParamVal(jd_data, "Sign", "签名", true, true);

            string sign_new = Md5Helper.MD5(HotelCode + Years + StartDate + EndDate + PayMoney + openid, 32);
            if (sign.ToLower() != sign_new.ToLower())
            {
                throw new PMSException("签名验证失败！");
            }

            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length < 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }
            #endregion

            string adminHotelId = keyValue[0];
            string hotelId = keyValue[1];
            string userId = keyValue[2];

            string ordernum = "XF" + DateTime.Now.ToString("yyMMddHHmmss") + CommonHelper.RndNum(2);
            Hashtable ht = new Hashtable();
            ht["AdminHotelId"] = adminHotelId;
            ht["HotelId"] = hotelId;
            ht["UserId"] = userId;
            ht["FeeType"] = "自主续费";
            ht["Years"] = Years;
            ht["StartDate"] = StartDate;
            ht["EndDate"] = EndDate;
            ht["Openid"] = openid;
            ht["OrderNumber"] = ordernum;
            ht["PayWay"] = "微信支付";
            ht["PayMoney"] = PayMoney;
            ht["PayNumber"] = "";
            ht["Remarks"] = "";
            int val = DataFactory.SqlDataBase().InsertByHashtable("PMS_RenewFeeTemp", ht);

            JsonData jd = new JsonData();//返回JsonData
            if (val > 0)
            {
                string wxJsApiParam = "";
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                //Dictionary<string, object> csInfo = new Dictionary<string, object>();
                //csInfo.Add("action", "RenewFee");//
                //csInfo.Add("PayMoney", PayMoney);
                //csInfo.Add("OrderNumber", ordernum);
                //csInfo.Add("openid", openid);
                //string menuInfo = serializer.Serialize(csInfo);
                string postUrl = "http://zidinn.com/API/pmsapi.ashx?action=RenewFee&PayMoney=" + PayMoney + "&OrderNumber=" + ordernum + "&openid=" + openid;
                wxJsApiParam = ComClass.PostWebRequest(postUrl, "");

                jd["code"] = 1;
                jd["data"] = wxJsApiParam;
            }
            else
            {
                jd["code"] = 0;
                jd["message"] = "操作失败！";
            }

            string json = jd.ToJson();
            // 统一输出
            PMSResponse.WirterString(json);
        }

        #endregion

        #region  ** 102. 续费记录 **

        public void RenewFeeList(HttpContext context, string jsonDate)
        {
            #region  ** 解码酒店编号 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }
            string HotelCode = CommonUse.ParamVal(jd_data, "HotelCode", "酒店编号", true, true);
            string sign = CommonUse.ParamVal(jd_data, "Sign", "签名", true, true);

            string sign_new = Md5Helper.MD5(HotelCode, 32);
            if (sign.ToLower() != sign_new.ToLower())
            {
                throw new PMSException("签名验证失败！");
            }

            string[] keyValue = DESEncrypt.Decrypt(HotelCode).Split(',');
            if (keyValue.Length < 2)
            {
                throw new PMSException("错误的酒店编号,请核对后重试");
            }
            #endregion

            string adminHotelId = keyValue[0];
            string hotelId = keyValue[1];
            string userId = keyValue[2];

            StringBuilder sql = new StringBuilder();
            sql.Append(@"
            Select  IsNull(b.User_Phone, 'admin') UserName ,
                    a.*
            From    dbo.PMS_RenewFee a
                    Left Join dbo.Base_UserInfo b On a.UserId = b.User_ID
            Where   a.HotelId = @HotelId
            ");

            IList<SqlParam> lParam = new List<SqlParam>();
            lParam.Add(new SqlParam("@HotelId", hotelId));

            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sql, lParam.ToArray());
            JsonData jd = new JsonData();//返回JsonData
            if (dt == null || dt.Rows.Count == 0)
            {
                jd["code"] = 0;
                jd["message"] = "未查询到相关信息!";
            }
            else
            {
                JsonData jdData = new JsonData();//DataTable值
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    JsonData jdList = new JsonData();
                    jdList["UserName"] = dt.Rows[i]["UserName"].ToString();
                    jdList["Years"] = dt.Rows[i]["Years"].ToString();
                    jdList["StartDate"] = dt.Rows[i]["StartDate"].ToString();
                    jdList["EndDate"] = dt.Rows[i]["EndDate"].ToString();
                    jdList["PayMoney"] = dt.Rows[i]["PayMoney"].ToString();
                    jdList["AddTime"] = dt.Rows[i]["AddTime"].ToString();
                    jdData.Add(jdList);
                }
                jd["code"] = 1;
                jd["data"] = jdData;
            }
            string json = jd.ToJson();
            // 统一输出
            PMSResponse.WirterString(json);
        }

        #endregion

    }
}