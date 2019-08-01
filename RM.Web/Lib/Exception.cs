using System;
using System.Collections.Generic;
using System.Web;

namespace RM.Web.Lib
{
    public class WxPayException : Exception 
    {
        public WxPayException(string msg) : base(msg) 
        {

        }
     }
}