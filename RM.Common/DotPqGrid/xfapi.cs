using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RM.Common.DotPqGrid
{
    public class xfapi
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 返回描述
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 数据列表
        /// </summary>
        public Dictionary<string, object> data { get; set; }
    }
}
