using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace Common
{
    public class SafeXmlDocument : XmlDocument
    {
        public SafeXmlDocument()
        {
            this.XmlResolver = null;
        }
    }
}