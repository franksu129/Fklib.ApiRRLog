using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace Fklib.ApiRRLog.Models
{
    public class LogInfo
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public LogLevel LogLevel { get; set; }

        public long ExecutionTime { get; set; }

        public List<LogicLog> Logics { get; set; }

        public List<ExceptionLog> Exceptions { get; set; }
    }
}