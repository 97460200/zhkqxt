using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace RM.Common.DotPqGrid
{
    public class PqGridHelper
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int totalRecords { get; set; }
        /// <summary>
        /// 当前页
        /// </summary>
        public int curPage { get; set; }
        /// <summary>
        /// 数据列表
        /// </summary>
        public ArrayList data { get; set; }
    }
}
