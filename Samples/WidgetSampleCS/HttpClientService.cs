using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WidgetSampleCS
{
    public class HttpClientService
    {
        private readonly HttpClient _Client = new HttpClient();


        public async Task<HttpResponseMessage> GetAsync(string url, KeyValuePair<string, string>[] headers = null)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, url))
            {
                requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        requestMessage.Headers.Add(header.Key, header.Value);
                    }
                }

                var response = await _Client.SendAsync(requestMessage);

                return response;
            }
        }

        public async Task<HttpResponseMessage> PostAsync(string url, object content, KeyValuePair<string, string>[] headers = null)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, url))
            {
                requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        requestMessage.Headers.Add(header.Key, header.Value);
                    }
                }

                if (content != null)
                {
                    requestMessage.Content = new StringContent(JsonConvert.SerializeObject(content).ToString(), Encoding.UTF8, "application/json");
                }

                var response = await _Client.SendAsync(requestMessage);

                return response;
            }
        }

        public async Task<HttpResponseMessage> PutAsync(string url, object content, KeyValuePair<string, string>[] headers = null)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Put, url))
            {
                requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        requestMessage.Headers.Add(header.Key, header.Value);
                    }
                }

                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

                var response = await _Client.SendAsync(requestMessage);

                return response;
            }
        }

        public async Task<HttpResponseMessage> DeleteAsync(string url, KeyValuePair<string, string>[] headers = null)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Delete, url))
            {
                requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        requestMessage.Headers.Add(header.Key, header.Value);
                    }
                }

                var response = await _Client.SendAsync(requestMessage);

                return response;
            }
        }
    }   
}
