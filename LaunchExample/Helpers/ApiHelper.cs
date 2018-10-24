using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Cyclr.LaunchExample.Helpers
{
    public static class ApiHelper
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private static string CyclrApiDomain = ConfigurationManager.AppSettings["CyclrApiUri"];

        public static async Task<string> GetOrbitUrl()
        {
            var message = new HttpRequestMessage(HttpMethod.Post, $"{CyclrApiDomain}/v1.0/users/orbit")
            {
                Content = new StringContent(JsonConvert.SerializeObject(new
                {
                    Username = ConfigurationManager.AppSettings["CyclrAccountUsername"],
                    Password = ConfigurationManager.AppSettings["CyclrAccountPassword"],
                    AccountId = ConfigurationManager.AppSettings["CyclrAccountId"]
                }))
            };
            message.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await OAuthHelper.GetAccessToken());
            var response = await _httpClient.SendAsync(message);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Failed to get orbit url{Environment.NewLine}{await response.Content.ReadAsStringAsync()}");

            var responseString = await response.Content.ReadAsStringAsync();
            var responseObj = (IDictionary<string, object>)JsonConvert.DeserializeObject<ExpandoObject>(responseString);
            return (string)responseObj["OrbitUrl"];
        }

        public static async Task<string> GetLaunchUrl()
        {
            var message = new HttpRequestMessage(HttpMethod.Post, $"{CyclrApiDomain}/v1.0/users/launch")
            {
                Content = new StringContent(JsonConvert.SerializeObject(new
                {
                    Username = ConfigurationManager.AppSettings["CyclrAccountUsername"],
                    Password = ConfigurationManager.AppSettings["CyclrAccountPassword"],
                    AccountId = ConfigurationManager.AppSettings["CyclrAccountId"]
                }))
            };
            message.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await OAuthHelper.GetAccessToken());
            var response = await _httpClient.SendAsync(message);
            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to get launch url");

            var responseString = await response.Content.ReadAsStringAsync();
            var responseObj = (IDictionary<string, object>)JsonConvert.DeserializeObject<ExpandoObject>(responseString);
            return (string)responseObj["LaunchUrl"];
        }
    }
}