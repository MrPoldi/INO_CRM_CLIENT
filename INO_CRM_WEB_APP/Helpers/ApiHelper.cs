using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace INO_CRM_WEB_APP.Helpers
{
    public static class ApiHelper
    {
        private static string apiUrl;
        private static HttpClient client;

        static ApiHelper()
        {
            apiUrl = "http://localhost:50060/";
            client = new HttpClient();
            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async static Task<HttpResponseMessage> GetAsync(string route, string accessToken = "")
        {
            if(accessToken != "")
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
            return await client.GetAsync(route);
        }

        public async static Task<HttpResponseMessage> PutAsync(string route, object obj, string accessToken = "")
        {
            if (accessToken != "")
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
            return await client.PutAsJsonAsync(route, obj);
        }

        public async static Task<HttpResponseMessage> DeleteAsync(string route, string accessToken = "")
        {
            if (accessToken != "")
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
            return await client.DeleteAsync(route);
        }
    }
}
