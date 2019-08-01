using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Text;
using RM.Common.DotNetBean;

namespace RM.Web.App_Code
{
    /// <summary>
    /// 基类
    /// </summary>
    /// <summary>
    /// PageBase 的摘要说明
    /// </summary>
    public class PageBase : Page
    {
        public PageBase()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        public void GetHeader()
        {
            StringBuilder sbMeta = new StringBuilder();
            //sbMeta.Append("\n    <meta name=\"Description\" content=\"EXPRESS CARD、USB3.0 EXPRESS CARD、PCI-E EXPRESS CARD、BLUETOOTH MOUSE、2.4G WIRELESS MOUSE(ENCRYPT THE FOLDER)\" />");
            //sbMeta.Append("\n    <meta name=\"Keywords\" content=\"EXPRESS CARD、USB3.0 EXPRESS CARD、PCI-E EXPRESS CARD、BLUETOOTH MOUSE、2.4G WIRELESS MOUSE(ENCRYPT THE FOLDER)\" />");
            sbMeta.Append("\n    <meta content=\"BlendTrans(Duration=0.3)\" http-equiv=\"Page-Exit\" />");
            Literal child = new Literal();
            child.Text = sbMeta.ToString();
            Page.Header.Controls.Add(child);
            //Page.Header.Title = string.Format("{0}", WebTitle);
        }
        public virtual int ID
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Request.QueryString["ID"]))
                {
                    int id;
                    int.TryParse(this.Request.QueryString["ID"], out id);
                    return id;
                }

                return -1;
            }
        }

        /// <summary>
        /// 初始化下拉框数据
        /// </summary>
        /// <param name="objDDL"></param>
        /// <param name="ds"></param>
        /// <param name="FieldName"></param>
        /// <param name="FieldText"></param>
        public void InitDropDownList(DropDownList objDDL, System.Data.DataSet ds, string FieldName, string FieldText)
        {
            objDDL.DataSource = ds.Tables[0].DefaultView;
            objDDL.DataValueField = FieldName;
            objDDL.DataTextField = FieldText;
            objDDL.DataBind();

            if (objDDL.Items.Count == 0) objDDL.Items.Add(new ListItem("NULL", "-1"));
        }

        ///// <summary>
        ///// 记录出错信息
        ///// </summary>
        ///// <param name="strError"></param>
        //public void LogError(string strError)
        //{
        //    log4net.ILog logger = log4net.LogManager.GetLogger(this.GetType());
        //    logger.Info(strError);
        //    logger.Error(strError);
        //    logger = null;
        //}

        /// <summary>
        /// 检测是匿名访问还是经过登陆访问
        /// </summary>
        protected virtual bool CheckSession()
        {
            if (Request.Cookies["LoginUser_CK"] != null)
            {
                if (Request.Cookies["LoginUser_CK"]["User_ID"] == null)
                {
                    this.Response.Write("<script lanuage=javascript>top.location='/Frame/Login.htm'</script>");
                    return false;
                }
                if (this.Session["SESSION_USER"] == null)
                {
                    SessionUser user = new SessionUser();
                    user.UserId = Request.Cookies["LoginUser_CK"]["User_ID"].ToString();
                    user.UserAccount = Request.Cookies["LoginUser_CK"]["User_Account"].ToString();
                    user.AdminHotelid = Request.Cookies["LoginUser_CK"]["AdminHotelid"].ToString();
                    user.Hotelid = Request.Cookies["LoginUser_CK"]["Hotelid"].ToString();
                    user.UserName = Request.Cookies["LoginUser_CK"]["User_Name"].ToString();
                    user.UserPwd = Request.Cookies["LoginUser_CK"]["UserPwd"].ToString();
                    user.HotelListId = Request.Cookies["LoginUser_CK"]["HotelListId"].ToString();
                    user.IsAdmin = Request.Cookies["LoginUser_CK"]["IsAdmin"].ToString();
                    user.RoleName = Request.Cookies["LoginUser_CK"]["RoleName"].ToString();
                    RequestSession.AddSessionUser(user);
                }

            }
            else
            {
                this.Response.Write("<script lanuage=javascript>top.location='/Frame/Login.htm'</script>");
                return false;
            }
            return true;
            //if (RequestSession.GetSessionUser() == null)
            //{
            //    this.Response.Write("<script lanuage=javascript>window.top.location='/Frame/Login.htm'</script>");
            //    return false;
            //}
            //return true;
        }

        protected override void OnInit(EventArgs e)
        {
            //    //检测是匿名访问还是经过登陆访问
            this.CheckSession();

            base.OnInit(e);
        }

        public bool IsAdd
        {
            get
            {
                if (this.Request.QueryString["ID"] == null)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// 字符串转换成日期时间，当出错时
        /// 设置缺省值为1900-01-01
        /// </summary>
        /// <param name="strDateTime"></param>
        /// <returns></returns>
        public static DateTime GetDefaultDateTime(string strDateTime)
        {
            try
            {
                return DateTime.Parse(strDateTime);
            }
            catch { return new DateTime(1900, 1, 1); }
        }
        /// <summary>
        /// 日期时间转换成字符串，当日期是0001-01-01时
        /// 设置缺省值为空字符串
        /// </summary>
        /// <param name="dtDateTime"></param>
        /// <returns></returns>
        public static string GetDefaultStringDateTime(DateTime dtDateTime)
        {
            try
            {
                if (dtDateTime.Equals(new DateTime(0001, 1, 1)))
                    return "";

                return dtDateTime.ToShortDateString();
            }
            catch { return ""; }
        }

        /// <summary>
        /// 防止SQL注入式攻击
        /// </summary>
        /// <param name="strParam"></param>
        /// <returns></returns>
        public static string FilterSQL(string strParam)
        {
            return strParam.Replace(";", "").Replace("=", "").Replace("%", "").Replace("'", "").Replace("--", "");
        }


        private Regex RegNumber = new Regex("^[0-9]+$");
        private Regex RegNumberSign = new Regex("^[+-]?[0-9]+$");
        private Regex RegDecimal = new Regex("^[0-9]\\d*(\\.\\d+)?$");//("^[0-9]+[.]?[0-9]+$");
        private Regex RegDecimalSign = new Regex("^[+-]?[0-9]+[.]?[0-9]+$"); //等价于^[+-]?\d+[.]?\d+$
        private Regex RegEmail = new Regex("^[\\w-]+@[\\w-]+\\.(com|net|org|edu|mil|tv|biz|info)$");//w 英文字母或数字的字符串，和 [a-zA-Z0-9] 语法一样 
        private Regex RegCHZN = new Regex("[\u4e00-\u9fa5]");
        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="script">脚本</param>
        public void ExcuteScript(string script)
        {
            ClientScript.RegisterStartupScript(typeof(Page), "scipt", script);
        }

        #region 显示消息框
        /// <summary>
        /// 显示消息框
        /// </summary>
        /// <param name="AlertMessage">显示的文本</param>
        public void ShowMessage(string strMessage)
        {
            ClientScript.RegisterStartupScript(typeof(Page), "alert", "<script language='javascript'>document.body.onload = function(){ alert(\"" + strMessage + "\");} </script>");
        }
        /// <summary>
        /// 显示消息框
        /// </summary>
        /// <param name="strMessage">显示的文本</param>
        /// <param name="pageName">点击确定后跳转的页面Url</param>
        public void ShowMessage(string strMessage, string pageName)
        {
            ClientScript.RegisterStartupScript(typeof(Page), "alert", string.Format("<script language='javascript'> alert(\"{0}\");window.location.href=\"{1}\";</script>", strMessage, pageName));
        }
        /// <summary>
        /// 显示消息框
        /// </summary>
        /// <param name="strMessage">显示的文本</param>
        /// <param name="IsAskDialog">是否为询问框</param>
        public void ShowMessage(string strMessage, bool IsAskDialog)
        {
            if (IsAskDialog)
                ClientScript.RegisterStartupScript(typeof(Page), "confirm", string.Format("return confirm('{0}')", strMessage));
            else
                ShowMessage(strMessage);
        }
        #endregion
        /// <summary>
        /// 新建立页面
        /// </summary>
        /// <param name="pageName">打开页面的Url</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="scrollbars">是否有滚动条</param>
        public void NewPage(string pageUrl, int width, int height, bool scrollbars)
        {
            ClientScript.RegisterStartupScript(typeof(Page), "confirm", string.Format("<script>window.open(\"{0}\", \"\", \"width={1},height={2},top=\"+(window.screen.height-{2})/2+\",left=\"+(window.screen.width-{1})/2+\",resizable=yes,status=yes,scrollbars={3},menubar=no,Location=no\")</script>", pageUrl, width, height, scrollbars ? "yes" : "no"));
        }

        ///// <summary>
        ///// 生成模式对话框字符串（显示页面）
        ///// </summary>
        ///// <returns></returns>
        //public static string ShowModalDialog(string pageName, int width, int height, bool scrollbars)
        //{
        //    return string.Format("window.showModalDialog('{0}',window,'scroll:{3};center:yes;help:no;status:no;dialogWidth:{1}px;dialogHeight:{2}px;');return false;", pageName, width, height, scrollbars ? "yes" : "no");
        //}

        /// <summary>
        /// 生成模式对话框字符串
        /// </summary>
        /// <param name="pageUrl">模式框里显示页面的Url</param>
        /// <param name="width">模式框宽度</param>
        /// <param name="height">模式框高度</param>
        /// <param name="scrollbars">是否有滚动条</param>
        /// <returns></returns>
        public string ShowModalDialog(string pageUrl, int width, int height, bool scrollbars)
        {
            return string.Format("var returnValue=window.showModalDialog('{0}',window,'scroll:{3};center:yes;help:no;status:no;dialogWidth:{1}px;dialogHeight:{2}px;');if(returnValue!=undefined){ alert(returnValue);} else return false;", pageUrl, width, height, scrollbars ? "yes" : "no");
        }

        /// <summary>
        /// 关闭模式窗口
        /// </summary>
        /// <param name="message">弹出的消息内容</param>
        public void CloseModalDialog(string message)
        {
            if (message != null)
                ExcuteScript(string.Format("window.returnValue='{0}';window.close();", message));
            else
                ExcuteScript("window.close();");
        }
        /// <summary>
        /// 关闭模式框口
        /// </summary>
        public void CloseModalDialog()
        {
            ExcuteScript("window.close();");
        }

        #region 数字字符串检查

        /// <summary>
        /// 检查Request查询字符串的键值，是否是数字，最大长度限制
        /// </summary>
        /// <param name="req">Request</param>
        /// <param name="inputKey">Request的键值</param>
        /// <param name="maxLen">最大长度</param>
        /// <returns>返回Request查询字符串</returns>
        public string FetchInputDigit(HttpRequest req, string inputKey, int maxLen)
        {
            string retVal = string.Empty;
            if (inputKey != null && inputKey != string.Empty)
            {
                retVal = req.QueryString[inputKey];
                if (null == retVal)
                    retVal = req.Form[inputKey];
                if (null != retVal)
                {
                    retVal = SqlText(retVal, maxLen);
                    if (!IsNumber(retVal))
                        retVal = string.Empty;
                }
            }
            if (retVal == null)
                retVal = string.Empty;
            return retVal;
        }
        /// <summary>
        /// 是否数字字符串
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public bool IsNumber(string inputData)
        {
            Match m = RegNumber.Match(inputData);
            return m.Success;
        }
        /// <summary>
        /// 是否数字字符串 可带正负号
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public bool IsNumberSign(string inputData)
        {
            Match m = RegNumberSign.Match(inputData);
            return m.Success;
        }
        /// <summary>
        /// 是否是浮点数
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public bool IsDecimal(string inputData)
        {
            Match m = RegDecimal.Match(inputData);
            return m.Success;
        }
        /// <summary>
        /// 是否是浮点数 可带正负号
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public bool IsDecimalSign(string inputData)
        {
            Match m = RegDecimalSign.Match(inputData);
            return m.Success;
        }

        #endregion

        #region 中文检测

        /// <summary>
        /// 检测是否有中文字符
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public bool IsHasCHZN(string inputData)
        {
            Match m = RegCHZN.Match(inputData);
            return m.Success;
        }

        #endregion

        #region 邮件地址
        /// <summary>
        /// 是否是浮点数 可带正负号
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public bool IsEmail(string inputData)
        {
            Match m = RegEmail.Match(inputData);
            return m.Success;
        }

        #endregion

        #region 其他

        /// <summary>
        /// 检查字符串最大长度，返回指定长度的串
        /// </summary>
        /// <param name="sqlInput">输入字符串</param>
        /// <param name="maxLength">最大长度</param>
        /// <returns></returns>			
        public string SqlText(string sqlInput, int maxLength)
        {
            if (sqlInput != null && sqlInput != string.Empty)
            {
                sqlInput = sqlInput.Trim();
                if (sqlInput.Length > maxLength)//按最大长度截取字符串
                    sqlInput = sqlInput.Substring(0, maxLength);
            }
            return sqlInput;
        }


        /// <summary>
        /// 字符串编码
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public string HtmlEncode(string inputData)
        {
            return HttpUtility.HtmlEncode(inputData);
        }
        /// <summary>
        /// 设置Label显示Encode的字符串
        /// </summary>
        /// <param name="lbl"></param>
        /// <param name="txtInput"></param>
        public void SetLabel(Label lbl, string txtInput)
        {
            lbl.Text = HtmlEncode(txtInput);
        }
        public void SetLabel(Label lbl, object inputObj)
        {
            SetLabel(lbl, inputObj.ToString());
        }

        #endregion

    }
}
