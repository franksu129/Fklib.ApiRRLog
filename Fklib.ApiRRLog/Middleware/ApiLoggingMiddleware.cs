using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fklib.ApiRRLog.Models;
using Fklib.ApiRRLog.Service;

namespace Fklib.ApiRRLog.Middleware
{
    public class ApiLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context,
                                 ILogger<ApiLoggingMiddleware> logger,
                                 IScopeLoggerService loggerService)
        {
            CheckMandatoryData(context, logger, loggerService);

            var executionTime = new Stopwatch();
            executionTime.Start();

            var myUrl = new Uri($"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}");
            var request = await FormatRequest(context.Request);
            var rootRequest = new RootRequest
            {
                OnTime = DateTime.Now,
                Scheme = myUrl.Scheme,
                Host = myUrl.Host,
                Body = request,
                AbsolutePath = myUrl.AbsolutePath,
                QueryString = myUrl.Query
            };

            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();

            context.Response.Body = responseBody;
            await _next(context);
            executionTime.Stop();

            var response = await FormatResponse(context.Response);

            var rootResponse = new RootResponse
            {
                OnTime = DateTime.Now,
                Status = context.Response.StatusCode,
                Content = response
            };
            var myLog = loggerService.LogApi(myUrl, rootRequest, rootResponse, executionTime.ElapsedMilliseconds, GetClientIP(context));

            logger.LogInformation(myLog);

            await responseBody.CopyToAsync(originalBodyStream);
        }

        private static void CheckMandatoryData(params dynamic[] objArray)
        {
            for (int i = 0; i < objArray.Length; i++)
            {
                if ((object)objArray[i] == null)
                {
                    throw new ArgumentNullException("obj");
                }
            }
        }

        /// <summary>
        /// 取得 Client IP
        /// </summary>
        private static string GetClientIP(HttpContext context)
        {
            var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.RemoteIpAddress.ToString();
            }

            return ip;
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            //This line allows us to set the reader for the request back at the beginning of its stream.
            request.EnableBuffering();

            var body = request.Body;

            //We now need to read the request stream.  First, we create a new byte[] with the same length as the request stream...
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            //...Then we copy the entire request stream into the new buffer.
            await request.Body.ReadAsync(buffer, 0, buffer.Length);

            //We convert the byte[] into a string using UTF8 encoding...
            var bodyAsText = Encoding.UTF8.GetString(buffer);

            body.Seek(0, SeekOrigin.Begin);
            request.Body.Position = 0;
            //..and finally, assign the read body back to the request body, which is allowed because of EnableRewind()
            request.Body = body;


            return bodyAsText;
            //// check if the Request is a POST call
            //// since we need to read from the body
            //if (request.Method == "GET")
            //{
            //    return null;
            //}
            //else
            //{
            //    request.EnableBuffering();
            //    var body = await new StreamReader(request.Body).ReadToEndAsync();
            //    request.Body.Position = 0;
            //    return body;
            //}
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            //將串流指回開頭，準備讀取回傳內容
            response.Body.Seek(0, SeekOrigin.Begin);

            //讀取回傳內容
            var responseBody = await new StreamReader(response.Body).ReadToEndAsync();

            //在串流指回開頭，預備StreamCopy
            response.Body.Seek(0, SeekOrigin.Begin);

            return responseBody;
        }
    }
}
