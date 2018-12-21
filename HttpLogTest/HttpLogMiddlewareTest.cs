using HttpLogExtensions;
using Microsoft.AspNetCore.Http;
using Moq;
using System.IO;
using System;
using System.Threading.Tasks;
using Xunit;
using System.Text;
using System.Security.Claims;
using System.Net;

namespace HttpLogTest
{
    public partial class HttpLogMiddlewareTest
    {
        private readonly Encoding _encoding;
        private HttpLog _httpLog;

        public HttpLogMiddlewareTest()
        {
            this._httpLog = null;
            this._encoding = Encoding.GetEncoding("utf-8");
        }

        [Fact]
        public async Task Can_Get_Null_RequestBody_When_ContentType_Is_MultipartFormData()
        {
            var _httpContextMock = await this.GetHttpContextMock();
            var _httpLogStoreMock = await GetHttpLogStoreMock((log) => _httpLog = log);

            var _requestMock = new Mock<HttpRequest>();
            _requestMock.Setup(x => x.ContentType).Returns("multipart/form-data;");
            _httpContextMock.Setup(x => x.Request).Returns(_requestMock.Object);

            await this.InvokeMiddleware(_httpContextMock, _httpLogStoreMock);

            Assert.Null(_httpLog.RequestBody);
        }

        [Fact]
        public async Task Can_Get_ConnectionInfo_Properties()
        {
            //HttpContext
            var _httpContextMock = await this.GetHttpContextMock();
            var _httpLogStoreMock = await this.GetHttpLogStoreMock((log) => _httpLog = log);

            await this.InvokeMiddleware(_httpContextMock, _httpLogStoreMock);

            Assert.Equal("127.0.0.1", _httpLog.LocalIpAddress);
            Assert.Equal(8080, _httpLog.LocalPort);
            Assert.Equal("198.168.0.124", _httpLog.RemoteIpAddress);
            Assert.Equal(80, _httpLog.RemotePort);
        }

        [Fact]
        public async Task Can_Get_Identity_Properties()
        {
            var _httpContextMock = await this.GetHttpContextMock();
            var _httpLogStoreMock = await this.GetHttpLogStoreMock((log) => _httpLog = log);

            await this.InvokeMiddleware(_httpContextMock, _httpLogStoreMock);

            Assert.True(_httpLog.IdentityIsAuthenticated);
            Assert.Equal("txstudio/administrator", _httpLog.IdentityName);
        }

        [Fact]
        public async Task Can_Get_HttpRequest_Properties()
        {
            var _httpContextMock = await this.GetHttpContextMock();
            var _httpLogStoreMock = await this.GetHttpLogStoreMock((log) => _httpLog = log);

            await this.InvokeMiddleware(_httpContextMock, _httpLogStoreMock);

            Assert.Equal("GET", _httpLog.RequestMethod);
            Assert.Equal("https", _httpLog.RequestScheme);
            Assert.Equal("/p/blog-page.html", _httpLog.RequestPath);
            Assert.Equal("/", _httpLog.RequestPathBase);
            Assert.Equal("?apikey=txstudio", _httpLog.RequestQueryString);
            Assert.Equal("application/json", _httpLog.RequestContentType);
            Assert.Equal(32767, _httpLog.RequestContentLength);
            Assert.Equal("blog.txstudio.tw", _httpLog.RequestHost);
            Assert.Equal(@"{""Email"":""boss@blog.txstudio.tw""}", _httpLog.RequestBody);
        }

        [Fact]
        public async Task Can_Get_HttpResponse_Properties()
        {
            var _httpContextMock = await this.GetHttpContextMock();
            var _httpLogStoreMock = await this.GetHttpLogStoreMock((log) => _httpLog = log);

            await this.InvokeMiddleware(_httpContextMock, _httpLogStoreMock);

            Assert.Equal(200, _httpLog.ResponseStatusCode);
            Assert.Equal("text/xml", _httpLog.ResponseContentType);
            Assert.Equal(10, _httpLog.ResponseContentLength);
        }

        [Fact]
        public async Task Can_Get_HttpResponse_Body()
        {
            var _httpLogStoreMock = await this.GetHttpLogStoreMock((log) => _httpLog = log);

            await this.InvokeMiddlewareUseDefaultHttpContext(_httpLogStoreMock);

            Assert.Equal(@"{""Result"":""true""}", _httpLog.ResponseBody);
        }
    }
}
