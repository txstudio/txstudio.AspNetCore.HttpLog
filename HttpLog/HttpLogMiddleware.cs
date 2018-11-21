using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace HttpLogExtensions
{
    public class HttpLogMiddleware
    {
        private readonly IHttpLogStore _logStore;
        private readonly RequestDelegate _next;

        public HttpLogMiddleware(RequestDelegate next, IHttpLogStore logStore)
        {
            this._next = next;
            this._logStore = logStore;
        }

        public async Task Invoke(HttpContext context)
        {
            HttpLog _log = new HttpLog();
            Stopwatch _stopwatch = new Stopwatch();

            _stopwatch.Reset();
            _stopwatch.Start();

            Task.WaitAll(new Task[]
            {
                this.SetConnectionInfo(context, _log)
                , this.SetIdentity(context, _log)
                , this.SetHttpRequest(context, _log)
            });

            //此項目有順序性
            await this.SetHttpResponse(context, _log);

            _stopwatch.Stop();

            _log.Elapsed = _stopwatch.ElapsedMilliseconds;

            await this._logStore.Add(_log);
        }

        private bool CaptureRequestBody(string contentType)
        {
            /*
             * multipart/form-data 的傳遞內容不進行 Request Body 紀錄
             */
            if (string.IsNullOrWhiteSpace(contentType) == true)
                return true;

            if (contentType.ToLower().StartsWith("multipart/form-data;") == true)
                return false;

            return true;
        }

        private async Task SetConnectionInfo(HttpContext context, HttpLog log)
        {
            await Task.Run(() =>
            {
                ConnectionInfo _connection = context.Connection;

                if (_connection == null)
                    return;

                //ConnectionInfo
                log.LocalIpAddress = _connection.LocalIpAddress.ToString();
                log.LocalPort = _connection.LocalPort;
                log.RemoteIpAddress = _connection.RemoteIpAddress.ToString();
                log.RemotePort = _connection.RemotePort;
            });
        }

        private async Task SetIdentity(HttpContext context, HttpLog log)
        {
            await Task.Run(() =>
            {
                if (context.User == null)
                    return;

                //Identity
                log.IdentityIsAuthenticated = context.User.Identity.IsAuthenticated;
                log.IdentityName = context.User.Identity.Name;
            });
        }

        private async Task SetHttpRequest(HttpContext context, HttpLog log)
        {
            HttpRequest _request = context.Request;

            if (_request == null)
                return;

            log.RequestMethod = _request.Method;
            log.RequestScheme = _request.Scheme;
            log.RequestPath = _request.Path;
            log.RequestPathBase = _request.PathBase;
            log.RequestQueryString = _request.QueryString.ToString();

            if (_request.ContentType != null)
                log.RequestContentType = _request.ContentType;

            log.RequestContentLength = _request.ContentLength;
            log.RequestHost = _request.Host.Value;

            if (this.CaptureRequestBody(_request.ContentType) == true)
            {
                _request.EnableBuffering();
                _request.Body.Position = 0;

                log.RequestBody = await new StreamReader(_request.Body).ReadToEndAsync();

                _request.Body.Position = 0;
            }
        }

        private async Task SetHttpResponse(HttpContext context, HttpLog log)
        {
            HttpResponse _response = context.Response;

            if (_response == null)
                return;

            //HttpResponse
            Stream _responseStream = _response.Body;
            MemoryStream _stream = new MemoryStream();

            _response.Body = _stream;

            await this._next(context);

            _stream.Position = 0;

            string _Body = await new StreamReader(_stream).ReadToEndAsync();

            log.ResponseStatusCode = _response.StatusCode;

            if (_response.ContentType != null)
                log.ResponseContentType = _response.ContentType;

            log.ResponseContentLength = _response.ContentLength;
            log.ResponseBody = _Body;

            _stream.Position = 0;

            await _stream.CopyToAsync(_responseStream);

            _response.Body = _responseStream;
        }

    }

}
