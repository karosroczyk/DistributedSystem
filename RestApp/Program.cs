using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Json;

namespace ConsoleApp4
{
    class Program
    {
        public class Obj
        {
            public int speed { get; set; }
        }
        static async Task Main(string[] args)
        {
            var httpClient = new HttpClient();
            var url = "https://dziekanat.agh.edu.pl/";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(url);

            if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
            {
                var content = httpResponseMessage.Content;
                var data = await content.ReadAsStringAsync();
                var json1 = JsonConvert.SerializeObject(data);
                Console.WriteLine(json1);
            }
            else
            {
                Console.WriteLine($"Error: " + httpResponseMessage.StatusCode);
            }

            //Obj speed_ = new Obj();
            //speed_.speed = 5;
            //var json = JsonConvert.SerializeObject(speed_);
            //httpClient.PutAsync("https://dziekanat.agh.edu.pl/", new StringContent(json, Encoding.UTF8, "application/json"));
        
/*            async Task<JsonObject> PostAsync(string uri, string data)
            {
                var response = await httpClient.PostAsync(uri, new StringContent(data));

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();
                return await Task.Run(() => JsonObject(content));
            }*/
        }

    }
}