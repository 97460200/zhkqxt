using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMS
{
    /// <summary>
    /// PMS自定义异常
    /// </summary>
    public class PMSException : Exception
    {
        public PMSCode ReCode = PMSCode.OK;

        public PMSException()
            : base()
        {

        }
        public PMSException(string message)
            : base(message)
        {
            ReCode = PMSCode.ERROR;
        }

        public PMSException(PMSCode errCode)
        {
            this.ReCode = errCode;

        }
    }
}
