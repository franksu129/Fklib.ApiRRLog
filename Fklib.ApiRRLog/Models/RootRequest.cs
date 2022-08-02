using System;

namespace Fklib.ApiRRLog.Models
{
    public class RootRequest
    {
        public DateTime OnTime { get; set; }
        public string Scheme { get; set; }
        public string Host { get; set; }
        public string Body { get; set; }
        public string AbsolutePath { get; set; }
        public string QueryString { get; set; }
    }
}