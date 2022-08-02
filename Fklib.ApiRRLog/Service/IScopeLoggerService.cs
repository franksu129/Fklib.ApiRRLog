using Microsoft.Extensions.Logging;
using System;
using Fklib.ApiRRLog.Models;

namespace Fklib.ApiRRLog.Service
{
    public interface IScopeLoggerService
    {
        string LogApi(Uri uri, RootRequest rootRequest, RootResponse rootResponse, long executionTime, string clientIP);

        void LogException(Exception exception);
        string LogInfo(long executionTime);

        /// <summary>
        /// 加入商業邏輯紀錄
        /// </summary>
        void LogLogic(LogLevel level, string message, object paras = null, object res = null);
    }
}