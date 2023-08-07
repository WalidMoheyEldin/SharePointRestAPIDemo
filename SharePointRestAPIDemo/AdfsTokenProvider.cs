using Newtonsoft.Json;
using System.Text;

namespace SharePointRestAPIDemo
{
    public interface IAdfsTokenProvider
    {
        Task<string> GetAccessTokenAsync();
    }

    public class AdfsTokenProvider : IAdfsTokenProvider
    {
        private const string adfsUrl = "https://XXXXXXXXXX";
        private const string clientId = "XXXXXXXXX";
        private const string clientSecret = "XXXXXXXXX";
        private const string username = "XXXXXXXX";
        private const string password = "XXXXXXXXX";

        public async Task<string> GetAccessTokenAsync()
        {
            using HttpClient httpClient = new();
            string tokenEndpoint = $"{adfsUrl}/oauth2/token";
            string requestBody = $"grant_type=password&client_id={clientId}&client_secret={clientSecret}&username={Uri.EscapeDataString(username)}&password={Uri.EscapeDataString(password)}&scope=openid";

            HttpResponseMessage response = await httpClient.PostAsync(tokenEndpoint, new StringContent(requestBody, Encoding.UTF8, "application/x-www-form-urlencoded"));

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                ADFSResponse tokenResponse = JsonConvert.DeserializeObject<ADFSResponse>(responseContent);
                return tokenResponse.Access_Token;
            }
            else
            {
                Console.WriteLine($"Error obtaining access token. Status code: {response.StatusCode}");
                return null;
            }
        }
    }

    public class ADFSResponse
    {
        public string Access_Token { get; set; }
    }
}
