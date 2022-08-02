using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace Fklib.ApiRRLog.Models
{
    public class ApiLog
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public LogLevel LogLevel { get; set; }

        public string Url { get; set; }

        public string ClientIP { get; set; }

        public long ExecutionTime { get; set; }

        public RootRequest Request { get; set; }

        public RootResponse Response { get; set; }

        public List<LogicLog> Logics { get; set; }

        public List<ExceptionLog> Exceptions { get; set; }
    }
}