using HttpLogData;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HttpLogClient
{
    class Program
    {
        static HttpClient _client = new HttpClient();

        static async Task Main(string[] args)
        {
            var _url = "http://localhost:5000/api/account/login";
            var _encoding = Encoding.GetEncoding("utf-8");

            var _Login = new LoginViewModel();
            _Login.Email = "boss@txstudio.tw";
            _Login.Password = "Pa$$w0rd";

            var _json = JsonConvert.SerializeObject(_Login);

            var _content = new StringContent(_json, _encoding, "application/json");
            var _response = await _client.PostAsync(_url, _content);

            if(_response.IsSuccessStatusCode == true)
            {
                var _responseString = await _response.Content.ReadAsStringAsync();

                Console.WriteLine();
                Console.WriteLine($"{_url} request result is:");
                Console.WriteLine(_responseString);
                Console.WriteLine();
            }

            Console.WriteLine("press any key to exit");
            Console.ReadKey();
        }
    }
}
