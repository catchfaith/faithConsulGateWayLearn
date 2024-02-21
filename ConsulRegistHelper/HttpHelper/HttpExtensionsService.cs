using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ConsulRegistHelper.HttpHelper
{
    public class HttpExtensionsService : IHttpExtensionsService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpExtensionsService(IHttpClientFactory _httpClientFactory)
        {
            this._httpClientFactory = _httpClientFactory;
        }

        public async ValueTask<string> GetAsync(string url)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                return await httpClient.GetStringAsync(url);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        public async ValueTask<string> PostAddHeaderAsync(string url, object body,Dictionary<string,string> dictionary)
        {
            var httpClient = _httpClientFactory.CreateClient();
            foreach (var VARIABLE in dictionary)
            {
                httpClient.DefaultRequestHeaders.Add(VARIABLE.Key,VARIABLE.Value);
            }
            var httpResponseMessage = await httpClient.PostAsync(url,new JsonContent(body));
            return await httpResponseMessage.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// post带 body
        /// </summary>
        /// <param name="url"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async ValueTask<string> PostAsync(string url,object body)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.PostAsync(url,new JsonContent(body));
            return await httpResponseMessage.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// post请求无参
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async ValueTask<string> PostNoBodyAsync(string url)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                var repMessage =await httpClient.PostAsync(url, null);
                return await repMessage.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}