using System.Net;
using System.Xml.Linq;

namespace SharePointRestAPIDemo
{
    public interface ISharePointHelper
    {
        Task UploadDocumentToSharePointAsync(string docLibraryName, string fileName, byte[] fileContent);
    }

    public class SharePointBearerTokenHelper : ISharePointHelper
    {
        private readonly IAdfsTokenProvider adfsTokenProvider;
        private const string siteUrl = "https://xxxxxxxxxxxxxxxxx";

        public SharePointBearerTokenHelper(IAdfsTokenProvider adfsTokenProvider) => this.adfsTokenProvider = adfsTokenProvider;

        public async Task UploadDocumentToSharePointAsync(string docLibraryName, string fileName, byte[] fileContent)
        {
            using HttpClient httpClient = new();
            string accessToken = await adfsTokenProvider.GetAccessTokenAsync();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

            string endpointUrl = $"{siteUrl}/_api/web/lists/getbytitle('{docLibraryName}')/RootFolder/Files/Add(url='{fileName}', overwrite=true)";

            using ByteArrayContent content = new(fileContent);
            HttpResponseMessage response = await httpClient.PostAsync(endpointUrl, content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("File uploaded successfully.");
            }
            else
            {
                Console.WriteLine($"Error uploading the file. Status code: {response.StatusCode}");
            }
        }
    }

    public class SharePointWindowsAuthHelper : ISharePointHelper
    {
        private const string siteUrl = "https://xxxxxxxxxx";
        private const string domain = "xxxxxxxxxx";
        private const string username = "xxxxxxxxxx";
        private const string password = "xxxxxxxxxx";

        public async Task UploadDocumentToSharePointAsync(string docLibraryName, string fileName, byte[] fileContent)
        {
            using HttpClient httpClient = new(new HttpClientHandler() { UseDefaultCredentials = true });

            string formDigest = await GetFormDigestAsync();

            string endpointUrl = $"{siteUrl}/_api/web/lists/getbytitle('{docLibraryName}')/RootFolder/Files/Add(url='{fileName}', overwrite=true)";

            httpClient.DefaultRequestHeaders.Add("X-RequestDigest", formDigest);

            using ByteArrayContent content = new(fileContent);
            HttpResponseMessage response = await httpClient.PostAsync(endpointUrl, content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("File uploaded successfully.");
            }
            else
            {
                Console.WriteLine($"Error uploading the file. Status code: {response.StatusCode}");
            }
        }

        private static async Task<string> GetFormDigestAsync()
        {
            using HttpClient httpClient = new(new HttpClientHandler()
            {
                Credentials = new NetworkCredential(username, password, domain),
                UseDefaultCredentials = true
            });

            string endpointUrl = $"{siteUrl}/_api/contextinfo";

            HttpResponseMessage response = await httpClient.PostAsync(endpointUrl, null);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            XDocument doc = XDocument.Parse(responseBody);
            return doc.Descendants().First(x => x.Name.LocalName == "FormDigestValue").Value;
        }
    }
}
