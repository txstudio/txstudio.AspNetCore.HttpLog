using HttpLogExtensions;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HttpLogTest
{
    public partial class HttpLogMiddlewareTest
    {
        private async Task<Mock<HttpContext>> GetHttpContextMock()
        {
            return await Task.Run(() => {

                //ConnectionInfo
                var _connectionInfoMock = new Mock<ConnectionInfo>();
                _connectionInfoMock.Setup(x => x.LocalIpAddress).Returns(IPAddress.Parse("127.0.0.1"));
                _connectionInfoMock.Setup(x => x.LocalPort).Returns(8080);
                _connectionInfoMock.Setup(x => x.RemoteIpAddress).Returns(IPAddress.Parse("198.168.0.124"));
                _connectionInfoMock.Setup(x => x.RemotePort).Returns(80);

                //Identity
                var _userMock = new Mock<ClaimsPrincipal>();
                _userMock.Setup(x => x.Identity.IsAuthenticated).Returns(true);
                _userMock.Setup(x => x.Identity.Name).Returns("txstudio/administrator");

                //Request
                var _requestBody = @"{""Email"":""boss@blog.txstudio.tw""}";
                var _requestMock = new Mock<HttpRequest>();
                _requestMock.Setup(x => x.Method).Returns("GET");
                _requestMock.Setup(x => x.Scheme).Returns("https");
                _requestMock.Setup(x => x.Path).Returns(new PathString("/p/blog-page.html"));
                _requestMock.Setup(x => x.PathBase).Returns(new PathString("/"));
                _requestMock.Setup(x => x.QueryString).Returns(new QueryString("?apikey=txstudio"));
                _requestMock.Setup(x => x.ContentType).Returns("application/json");
                _requestMock.Setup(x => x.ContentLength).Returns(32767);
                _requestMock.Setup(x => x.Host).Returns(new HostString("blog.txstudio.tw"));
                _requestMock.Setup(x => x.Body).Returns(new MemoryStream(this._encoding.GetBytes(_requestBody)));

                //Response
                var _responseBody = @"{""Result"":""true""}";
                var _responseStream = new MemoryStream(this._encoding.GetBytes(_responseBody));
                var _responseMock = new Mock<HttpResponse>();
                _responseMock.Setup(x => x.StatusCode).Returns(200);
                _responseMock.Setup(x => x.ContentType).Returns("text/xml");
                _responseMock.Setup(x => x.ContentLength).Returns(10);
                _responseMock.Setup(x => x.Body).Returns(_responseStream);

                //HttpContext
                var _httpContextMock = new Mock<HttpContext>();
                _httpContextMock.Setup(x => x.Connection).Returns(_connectionInfoMock.Object);
                _httpContextMock.Setup(x => x.User).Returns(_userMock.Object);
                _httpContextMock.Setup(x => x.Request).Returns(_requestMock.Object);
                _httpContextMock.Setup(x => x.Response).Returns(_responseMock.Object);

                return _httpContextMock;
            });
        }

        private async Task<Mock<IHttpLogStore>> GetHttpLogStoreMock(Action<HttpLog> action)
        {
            return await Task.Run(() => {
                var _logStoreMock = new Mock<IHttpLogStore>();

                _logStoreMock
                    .Setup(x => x.Add(It.IsAny<HttpLog>()))
                    .Callback<HttpLog>(action)
                    .Returns(Task.FromResult(0));

                return _logStoreMock;
            });
        }

        private async Task InvokeMiddleware(Mock<HttpContext> httpContextMock
                                            , Mock<IHttpLogStore> httpLogStoreMock)
        {
            var _middleware
                = new HttpLogMiddleware(next: async (context) => {
                        await context.Response.WriteAsync(@"{""Result"":""true""}");
                    }, logStore: httpLogStoreMock.Object);

            await _middleware.Invoke(httpContextMock.Object);
        }

        private async Task InvokeMiddlewareUseDefaultHttpContext(Mock<IHttpLogStore> httpLogStoreMock)
        {
            var _context = new DefaultHttpContext();

            var _middleware
                = new HttpLogMiddleware(next: async (context) => {
                    await context.Response.WriteAsync(@"{""Result"":""true""}");
                }, logStore: httpLogStoreMock.Object);

            await _middleware.Invoke(_context);
        }

    }
}
