using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace HttpLogExtensions
{
    public sealed class HttpLog
    {
        //ConnectionInfo
        public string LocalIpAddress { get; set; }
        public int LocalPort { get; set; }
        public string RemoteIpAddress { get; set; }
        public int RemotePort { get; set; }

        //User.Identity
        public bool IdentityIsAuthenticated { get; set; }
        public string IdentityName { get; set; }

        //Request
        public string RequestMethod { get; set; }
        public string RequestScheme { get; set; }
        public string RequestPath { get; set; }
        public string RequestPathBase { get; set; }
        public string RequestQueryString { get; set; }
        public string RequestContentType { get; set; }
        public long? RequestContentLength { get; set; }
        public string RequestHost { get; set; }
        public string RequestBody { get; set; }

        //Response
        public int ResponseStatusCode { get; set; }
        public string ResponseContentType { get; set; }
        public long? ResponseContentLength { get; set; }
        public string ResponseBody { get; set; }

        public long Elapsed { get; set; }
    }

}
