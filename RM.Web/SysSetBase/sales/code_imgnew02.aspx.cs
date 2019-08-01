using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace RM.Web.SysSetBase.sales
{
    public partial class code_imgnew02 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 编辑按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSumit_Click(object sender, EventArgs e)
        {
            string filename = "智订云.jpg";
            string filePath = "~/SysSetBase/img/智订云.jpg";
            DownloadFile(Server.UrlEncode(filename), Server.MapPath(filePath));
        }

        private void DownloadFile(string fileName, string filePath)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(filePath);
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
                Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                Response.AddHeader("Content-Transfer-Encoding", "binary");
                Response.ContentType = "application/octet-stream";
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
                Response.WriteFile(fileInfo.FullName);
                Response.Flush();
            }
            catch
            {

            }
        }

    }
}