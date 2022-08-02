using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Fklib.ApiRRLog.Models;

namespace Fklib.ApiRRLog.Service
{
    public class ScopeLoggerService : IScopeLoggerService
    {
        public List<LogicLog> logicLogs;

        public List<ExceptionLog> exceptionLogs;

        public ScopeLoggerService()
        {
            logicLogs = new List<LogicLog>();
            exceptionLogs = new List<ExceptionLog>();
        }

        /// <summary>
        /// 取最嚴重的當作該整體Log等級
        /// </summary>
        private LogLevel ApiLogLevel
        {
            get
            {
                if (exceptionLogs.Any())
                {
                    return LogLevel.Error;
                }
                else
                {
                    if (logicLogs.Any())
                        return logicLogs.Max(x => x.Level);
                    else
                        return LogLevel.None;
                }
            }
        }

        public void LogLogic(LogLevel level, string message, object paras = null, object res = null)
        {
            logicLogs.Add(new LogicLog
            {
                Level = level,
                Message = message,
                Parameters = paras,
                Return = res
            });
        }

        public void LogException(Exception exception)
        {
            this.exceptionLogs.Add(new ExceptionLog
            {
                Message = exception.Message,
                HelpLink = exception.HelpLink,
                Source = exception.Source,
                TargetSiteName = exception.TargetSite.Name,
                StackTrace = exception.StackTrace
            });
        }

        public string LogInfo(long executionTime)
        {
            var jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatString = "yyyy/MM/dd HH:mm:ss.fff",
            };

            var obj = new LogInfo
            {
                LogLevel = ApiLogLevel,
                ExecutionTime = executionTime,
                Logics = logicLogs,
                Exceptions = exceptionLogs,
            };
            return JsonConvert.SerializeObject(obj, jsonSettings);
        }

        public string LogApi(Uri uri, RootRequest rootRequest, RootResponse rootResponse, long executionTime, string clientIP)
        {
            var jsonSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatString = "yyyy/MM/dd HH:mm:ss.fff",
            };

            var obj = new ApiLog
            {
                LogLevel = ApiLogLevel,
                Url = uri?.ToString(),
                ClientIP = clientIP,
                ExecutionTime = executionTime,
                Request = rootRequest,
                Response = rootResponse,
                Logics = logicLogs,
                Exceptions = exceptionLogs,
            };
            return JsonConvert.SerializeObject(obj, jsonSettings);
        }
    }
}