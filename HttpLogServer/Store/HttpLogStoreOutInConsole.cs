using HttpLogExtensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HttpLogServer
{
    public class HttpLogStoreOutInConsole : IHttpLogStore
    {
        /* 刻意透過 Exception 方式顯示 HttpLog 物件的內容 */
        public async Task Add(HttpLog log)
        {
            await Task.Run(() => { 
                string _message = JsonConvert.SerializeObject(log, Formatting.Indented);

                Console.WriteLine();
                Console.WriteLine("HttpLogStoreOutInConsole:");
                Console.WriteLine(_message);
                Console.WriteLine();
            });
        }
    }
}
