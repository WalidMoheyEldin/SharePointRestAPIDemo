namespace SharePointRestAPIDemo
{
    public interface ISharePointHelper
    {
        Task UploadDocumentToSharePointAsync(string docLibraryName, string fileName, byte[] fileContent);
    }

    public class SharePointHelper : ISharePointHelper
    {
        private readonly IAdfsTokenProvider adfsTokenProvider;
        private readonly string siteUrl = "https://xxxxxxxxxxxxxxxxx";

        public SharePointHelper(IAdfsTokenProvider adfsTokenProvider) => this.adfsTokenProvider = adfsTokenProvider;

        public async Task UploadDocumentToSharePointAsync(string docLibraryName, string fileName, byte[] fileContent)
        {
            using HttpClient httpClient = new();
            string accessToken = await adfsTokenProvider.GetAccessTokenAsync();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

            var endpointUrl = $"{siteUrl}/_api/web/lists/getbytitle('{docLibraryName}')/RootFolder/Files/Add(url='{fileName}', overwrite=true)";

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
}
