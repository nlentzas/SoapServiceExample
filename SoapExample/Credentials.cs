using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Protocols;

namespace SoapExample
{
    public class Credentials : SoapHeader
    {
        public string username;
        public string password;
    }
}