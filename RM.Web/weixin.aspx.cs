using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Web.WX_SET;
using System.Threading;

namespace RM.Web
{
    public partial class weixin : System.Web.UI.Page
    {
       // static AutoResetEvent sleepSynchro = new AutoResetEvent(false);
        protected void Page_Load(object sender, EventArgs e)
        {
            //智订云
            Sample sample = new Sample(this);
            sample.ProcessNotify();

            //Thread controlThread = new Thread(threadSample);
            //controlThread.Start(); 
            //if (!sleepSynchro.WaitOne(3000, false))
            //{
            //    Response.Write("error");
            //    Response.End();
            //}
        }

        //protected void threadSample()
        //{
        //    Sample sample = new Sample(this);
        //    sample.ProcessNotify();
        //}
    }
}