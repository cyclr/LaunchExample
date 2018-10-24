using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cyclr.LaunchExample.Helpers
{
    public static class OAuthHelper
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private static string CyclrApiDomain = ConfigurationManager.AppSettings["CyclrApiUri"];

        public static async Task<string> GetAccessToken()
        {
            var context = new Models.LaunchExampleContext();
            var token = context.OAuthTokens.OrderByDescending(o => o.Expires).FirstOrDefault();
            if (token == null)
            {
                token = await GetOAuthToken();
                context.OAuthTokens.Add(token);
                await context.SaveChangesAsync();
            }
            else if (token.Expires < DateTime.UtcNow.AddMinutes(-60))
            {
                var newToken = await RefreshOAuthToken(token);
                context.OAuthTokens.Remove(token);
                context.OAuthTokens.Add(newToken);
                await context.SaveChangesAsync();
                token = newToken;
            }

            return token.AccessToken;
        }

        private static async Task<Models.OAuthToken> GetOAuthToken()
        {
            var content = new Dictionary<string, string> {
                { "client_id", ConfigurationManager.AppSettings["CyclrClientId"] },
                { "grant_type", "password" },
                { "username", ConfigurationManager.AppSettings["CyclrPartnerUsername"] },
                { "password", ConfigurationManager.AppSettings["CyclrPartnerPassword"] }
            };
            var message = new HttpRequestMessage(HttpMethod.Post, $"{CyclrApiDomain}/oauth/token")
            {
                Content = new FormUrlEncodedContent(content)
            };
            var response = await _httpClient.SendAsync(message);
            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to get oauth token");

            var responseString = await response.Content.ReadAsStringAsync();
            var responseObj = (IDictionary<string, object>)JsonConvert.DeserializeObject<ExpandoObject>(responseString);
            return new Models.OAuthToken
            {
                AccessToken = (string)responseObj["access_token"],
                RefreshToken = (string)responseObj["refresh_token"],
                Expires = DateTime.Parse((string)responseObj[".expires"])
            };
        }

        private static async Task<Models.OAuthToken> RefreshOAuthToken(Models.OAuthToken token)
        {
            var content = new Dictionary<string, string> {
                { "client_id", ConfigurationManager.AppSettings["CyclrClientId"] },
                { "grant_type", "refresh_token" },
                { "refresh_token", token.RefreshToken }
            };
            var message = new HttpRequestMessage(HttpMethod.Post, $"{CyclrApiDomain}/oauth/token")
            {
                Content = new FormUrlEncodedContent(content)
            };
            var response = await _httpClient.SendAsync(message);
            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to refresh oauth token");
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObj = (IDictionary<string, object>)JsonConvert.DeserializeObject<ExpandoObject>(responseString);
            return new Models.OAuthToken
            {
                AccessToken = (string)responseObj["access_token"],
                RefreshToken = (string)responseObj["refresh_token"],
                Expires = DateTime.Parse((string)responseObj[".expires"])
            };
        }
    }
}