using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using RM.Web.Lib;
using System.Xml;
using RM.Web.WX_SET;
using System.Net;
using System.Text;
using System.IO;
using System.Collections;
using RM.Busines;

namespace RM.Web
{
    public partial class wx_sample : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //酒店
            Sample sample = new Sample(this);
            sample.ProcessNotify();
        }
    }
}