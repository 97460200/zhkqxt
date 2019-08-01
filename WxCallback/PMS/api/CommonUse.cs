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
using Common;
using System.Xml;
using System.Collections;

namespace PMS.api
{
    /// <summary>
    /// 通用接口
    /// </summary>
    public class CommonUse
    {
        #region ** 参数值验证 **
        /// <summary>
        /// 参数值验证
        /// </summary>
        /// <param name="jd_data">参数对象</param>
        /// <param name="key">参数</param>
        /// <param name="msg">参数说明</param>
        /// <param name="blMust">是否必传</param>
        /// <param name="blEmpty">是否不能为空</param>
        /// <returns></returns>
        public static string ParamVal(JsonData jd_data, string key, string msg, bool blMust, bool blEmpty)
        {
            string val = "";
            if (blMust)
            {
                try
                {
                    val = jd_data[key].ToString();
                }
                catch (Exception)
                {
                    throw new PMSException("缺少" + msg + "[" + key + "]参数！");
                }
            }
            else
            {
                try
                {
                    val = jd_data[key].ToString();
                }
                catch (Exception)
                {
                    val = "";
                }
            }
            if (blEmpty && val == "")
            {
                throw new PMSException(msg + "[" + key + "]不能为空！");
            }
            return val;
        }
        #endregion

        #region  ** 1. 日期变动 **

        public void DateChange(HttpContext context, string jsonDate)
        {
            #region  ** 参数验证 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }

            DateTime StartDate = DateTime.Now;
            JsonData jd = new JsonData();
            jd["code"] = 1;
            string json = "";
            string day = CommonUse.ParamVal(jd_data, "Day", "当前日期", false, false);
            if (day != "")
            {
                jd["Date"] = StartDate.ToString("yyyy年MM月dd日");
                json = jd.ToJson();

                // 统一输出
                PMSResponse.WirterString(json);
                return;
            }

            string date = CommonUse.ParamVal(jd_data, "Date", "起始日期", true, true);
            string type = CommonUse.ParamVal(jd_data, "Type", "变动类型", true, true);
            int UpDown = CommonHelper.GetInt(CommonUse.ParamVal(jd_data, "UpDown", "调整方式", true, true), 1);

            #endregion

            switch (type)
            {
                case "1":
                    StartDate = Convert.ToDateTime(date).AddDays(UpDown);
                    jd["Date"] = StartDate.ToString("yyyy年MM月dd日");
                    break;
                case "2":
                    StartDate = Convert.ToDateTime(date + "01日").AddMonths(UpDown);
                    jd["Date"] = StartDate.ToString("yyyy年MM月");
                    break;
                case "3":
                    StartDate = Convert.ToDateTime(date + "01月01日").AddYears(UpDown);
                    jd["Date"] = StartDate.ToString("yyyy年");
                    break;
                default:
                    jd["code"] = 0;
                    jd["message"] = "变动类型错误！";
                    break;
            }
            json = jd.ToJson();

            // 统一输出
            PMSResponse.WirterString(json);
        }
        #endregion

        #region  ** 2. 发送短信验证码 **

        public void SmsCode(HttpContext context, string jsonDate)
        {
            #region  ** 参数验证 **

            JsonData jd_data = JsonMapper.ToObject(jsonDate);
            if (jd_data == null || jd_data.Count < 1)
            {
                throw new PMSException("参数不能为空");
            }

            string phone = CommonUse.ParamVal(jd_data, "Phone", "手机号码", true, true);
            int num = CommonHelper.GetInt(CommonUse.ParamVal(jd_data, "SmsType", "短信类型", true, true));//类型 1 登录验证,3 免费注册,5忘记密码

            JsonData jd = new JsonData();
            if (!ValidateUtil.IsValidMobile(phone))
            {
                jd["code"] = 0;
                jd["message"] = "请输入正确的手机号码！";
            }

            Random ran_int = new Random();
            string code = ran_int.Next(1000, 9999).ToString();
            if (SendSms(phone, code, num, "1"))
            {
                jd["code"] = 1;
                jd["data"] = "";
            }
            else
            {
                jd["code"] = 0;
                jd["message"] = "请输入正确的手机号码！";
            }
            #endregion

            string json = jd.ToJson();

            // 统一输出
            PMSResponse.WirterString(json);
        }


        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="Phone"></param>
        /// <param name="Number"></param>
        /// <param name="num"></param>
        /// <param name="AdminHotelid"></param>
        /// <returns></returns>
        public static bool SendSms(string Phone, string Number, int num, string AdminHotelid)
        {
            string content = "";
            string HotelName = "";
            string SendMoney = "0.08";
            string sql_name = string.Format(@"SELECT Name from Hotel_Admin where AdminHotelid='{0}'", AdminHotelid);
            DataTable ds_name = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql_name));
            if (ds_name != null && ds_name.Rows.Count > 0)
            {
                HotelName = ds_name.Rows[0]["Name"].ToString();

            }
            string sql = string.Format(@"SELECT ID , Userid ,  Account ,   Password , URL ,  OrganizationID , Name , SingleMoney , AdminHotelid from SmsParameter where AdminHotelid='{0}'", AdminHotelid);
            DataTable ds = DataFactory.SqlDataBase().GetDataTableBySQL(new StringBuilder(sql));
            if (ds != null && ds.Rows.Count > 0)
            {
                SendMoney = ds.Rows[0]["SingleMoney"].ToString();
                //if (num == 1)
                //{
                //    content = "您正在" + HotelName + "进行免费注册验证，验证码：" + Number + "，请输入验证码完成注册。【" + ds.Rows[0]["name"] + "】";
                //    if (AdminHotelid == "1")
                //    {
                //        content = "您正在" + HotelName + "进行登录，验证码：" + Number + "，请输入验证码完成登录。【" + ds.Rows[0]["name"] + "】";
                //    }
                //}
                //else if (num == 2)
                //{
                //    content = "你在" + HotelName + "进行了修改手机绑定，验证码：" + Number + "，请输入验证码完成修改。【" + ds.Rows[0]["name"] + "】";
                //}
                //else if (num == 3)
                //{
                //    content = "您正在" + HotelName + "进行免费注册验证，验证码：" + Number + "，请输入验证码完成注册。【" + ds.Rows[0]["name"] + "】";
                //}
                //else if (num == 4)
                //{
                //    content = Number;
                //}
                //else if (num == 5)
                //{
                //    content = "您正在智订云进行了忘记密码，验证码：" + Number + "，请输入验证码完成修改。【智订云】";
                //}

                if (num == 1)
                {

                    content = "您正在智订云移动PMS进行免费注册验证，验证码：" + Number + "，请输入验证码完成注册。【智订云】";
                    if (AdminHotelid == "1")
                    {
                        content = "您正在登录智订云移动PMS，验证码：" + Number + "，请按页面提示进行输入。【智订云】";
                    }
                }
                else if (num == 2)
                {
                    content = "你在智订云移动PMS进行了修改手机绑定，验证码：" + Number + "，请输入验证码完成修改。【智订云】";
                }
                else if (num == 3)
                {
                    content = "您正在智订云移动PMS进行免费注册验证，验证码：" + Number + "，请输入验证码完成注册。【智订云】";
                }
                else if (num == 4)
                {
                    content = Number;
                }
                else if (num == 5)
                {
                    content = "您正在智订云进行了忘记密码，验证码：" + Number + "，请输入验证码完成修改。【智订云】";
                }
                string param = "action=send&userid=" + ds.Rows[0]["Userid"] + "&account=" + ds.Rows[0]["account"] + "&password=" + ds.Rows[0]["password"] + "&mobile=" + Phone + "&content=" + EncodeConver(content);
                string url = ds.Rows[0]["url"].ToString();
                string result = TemplateMessage.PostWebRequest(url, param);
                SafeXmlDocument xmlDoc = new SafeXmlDocument();
                try
                {
                    xmlDoc.LoadXml(result);
                    //读取Activity节点下的数据。SelectSingleNode匹配第一个Activity节点  
                    XmlNode root = xmlDoc.SelectSingleNode("//returnsms");//当节点Workflow带有属性是，使用SelectSingleNode无法读取          
                    if (root != null)
                    {
                        string RetureState = (root.SelectSingleNode("returnstatus")).InnerText;
                        string ErrorDescribe = (root.SelectSingleNode("message")).InnerText;
                        string RetureBalance = root.SelectSingleNode("remainpoint").InnerText;
                        string SequenceId = root.SelectSingleNode("taskID").InnerText;
                        string SuccessCounts = root.SelectSingleNode("successCounts").InnerText;
                        if (int.Parse(SuccessCounts) > 0)//发送成功添加记录到短信记录表
                        {
                            Hashtable ht = new Hashtable();
                            ht["MessageType"] = 0;
                            ht["Number"] = "DX" + System.DateTime.Now.ToString("yyMMddHHmmss") + Number;
                            ht["Code"] = Number;//
                            ht["ReceiveType"] = 3;//接收对象类型
                            ht["ReceiveObject"] = "个人（验证码）";//接收对象
                            ht["SendNum"] = 1;//发送短信数量
                            ht["SendUser"] = "智订云";
                            ht["SendContent"] = content;//发送内容
                            ht["SendType"] = 0;//发送类型（0、即时 1、实时）
                            ht["SendMoney"] = SendMoney;
                            ht["DeductionType"] = 1;//扣费类型(0赠送扣除 1营销费扣除 2费用不足够抵扣)
                            ht["SingleMoney"] = SendMoney;//单条短信费用
                            ht["MulTiple"] = 1;//短信倍数
                            ht["State"] = 1;//状态（1审核中、2部分成功、3发送失败、4发送成功）
                            ht["RetureState"] = RetureState;
                            ht["ErrorDescribe"] = ErrorDescribe;
                            ht["RetureBalance"] = RetureBalance;
                            ht["SequenceId"] = SequenceId;
                            ht["SuccessCounts"] = SuccessCounts;
                            ht["PhoneSubmit"] = Phone;
                            ht["HotelName"] = HotelName;
                            ht["AdminHotelid"] = AdminHotelid;
                            bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("SendRecord", "ID", "", ht);
                            return true;
                        }
                        else
                        {//失败记录
                            Hashtable ht = new Hashtable();
                            ht["MessageType"] = 0;
                            ht["Number"] = "DX" + System.DateTime.Now.ToString("yyMMddHHmmss") + Number;
                            ht["Code"] = Number;//
                            ht["ReceiveType"] = 3;//接收对象类型
                            ht["ReceiveObject"] = "个人（验证码）";//接收对象
                            ht["SendNum"] = 1;//发送短信数量
                            ht["SendUser"] = "智订云";
                            ht["SendContent"] = content;//发送内容
                            ht["SendType"] = 0;//发送类型（0、即时 1、实时）
                            ht["SendMoney"] = SendMoney;
                            ht["DeductionType"] = 1;//扣费类型(0赠送扣除 1营销费扣除 2费用不足够抵扣)
                            ht["SingleMoney"] = SendMoney;//单条短信费用
                            ht["MulTiple"] = 1;//短信倍数
                            ht["State"] = 3;//状态（1审核中、2部分成功、3发送失败、4发送成功）
                            ht["PhoneSubmit"] = Phone;
                            ht["HotelName"] = HotelName;
                            ht["AdminHotelid"] = AdminHotelid;
                            bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("SendRecord", "ID", "", ht);
                            return false;
                        }
                    }
                    else
                    {

                        Console.WriteLine("the node  is not existed");
                        return false;
                    }
                }
                catch (Exception e)
                {
                    //显示错误信息  
                    Console.WriteLine(e.Message);
                    return false;
                }

            }
            else
            {
                // 解析 Result  
                return false;
            }

        }


        /// <summary>
        /// 转换格式
        /// </summary>
        /// <param name="instring"></param>
        /// <returns></returns>
        public static string EncodeConver(string instring)
        {
            return HttpUtility.UrlEncode(instring, Encoding.UTF8);
        }

        #endregion

    }
}