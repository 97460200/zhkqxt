using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Api
{
    /// <summary>
    /// API自定义异常
    /// </summary>
    public class APIException : Exception
    {
        public APICode ReCode = APICode.OK;

        public APIException()
            : base()
        {

        }
        public APIException(string message)
            : base(message)
        {
            ReCode = APICode.ERROR;
        }

        public APIException(APICode errCode)
        {
            this.ReCode = errCode;

        }
    }
}
