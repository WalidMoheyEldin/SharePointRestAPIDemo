using SharePointRestAPIDemo;

byte[] fileContent = await File.ReadAllBytesAsync("path_to_local_file");
IAdfsTokenProvider adfsTokenProvider = new AdfsTokenProvider();
ISharePointHelper sharePointHelper = new SharePointBearerTokenHelper(adfsTokenProvider);
await sharePointHelper.UploadDocumentToSharePointAsync("docLibName", "fileName", fileContent);