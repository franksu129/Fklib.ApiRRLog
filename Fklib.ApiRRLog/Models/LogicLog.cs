using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Fklib.ApiRRLog.Models
{
    public class LogicLog
    {
        /// <summary>
        /// Log 訊息
        /// </summary>
        /// <value></value>
        public string Message { get; set; }

        /// <summary>
        /// Log 層級
        /// </summary>
        /// <value></value>
        [JsonConverter(typeof(StringEnumConverter))]
        public LogLevel Level { get; set; }

        /// <summary>
        /// 程式參數
        /// </summary>
        /// <value></value>
        public object Parameters { get; set; }

        /// <summary>
        /// 回傳結果
        /// </summary>
        /// <value></value>
        public object Return { get; set; }
    }
}