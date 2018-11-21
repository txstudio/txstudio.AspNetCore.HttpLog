# 在 ASP.NET Core 應用程式取得 HttpLog 的擴充方法

在 ASP.NET Core 應用程式擷取 Request, Response, ConnectionInfo 資訊的 IApplicationBuilder 擴充方法 (Middleware)

## 如何使用

實作 IHttpLogStore 介面定義 HttpLog 事件需要儲存的位置與內容

在 Startup 類別中的 Configure 方法加入 app.UseHttpLog 呼叫，並帶入 IHttpLogStore 實作類別 (選擇項目)

## 參考連結

### Middleware 物件開發參考
[鐵人賽 Day03] ASP.NET Core 2 系列 - Middleware | John Wu's Blog

https://blog.johnwu.cc/article/ironman-day03-asp-net-core-middleware.html

### 進行 Middleware 測試

» ASP.NET Core 2.1 middlewares part 2: Unit test a custom middleware

http://anthonygiretti.com/2018/09/04/asp-net-core-2-1-middlewares-part2-unit-test-a-custom-middleware/

Unit Test ASP.NET Core Middleware - Sul Aga

http://www.sulhome.com/blog/15/unit-test-asp-net-core-middleware

### 使用 Moq 模擬 Middleware 物件的範例程式碼
log-request-response-middleware/LogRequestMiddlewareTest.cs at master · sulhome/log-request-response-middleware

https://github.com/sulhome/log-request-response-middleware/blob/master/src/LogResReqMiddleware.UnitTest/LogRequestMiddlewareTest.cs
