# SharePoint REST API Demo

This project demonstrates how to upload a document to SharePoint using the REST API.

## Prerequisites

* You need to have a SharePoint 2019 site and know the URL of the site.
* You need to have the `Newtonsoft.Json` and `System.Text` NuGet packages installed.

## Getting Started

1. Clone the repository.
2. Open the `SharePointRestAPIDemo.sln` solution in Visual Studio.
3. In the `AdfsTokenProvider.cs` file, update the following constants with your own values:
    * `ADFSUrl`
    * `ClientId`
    * `ClientSecret`
    * `Username`
    * `Password`
4. In the `SharePointHelper.cs` file, update the following constants with your own values:
    * `siteUrl`
5. Run the project.

## Uploading a Document

To upload a document, follow these steps:

1. In the `Main` method, update the `docLibName` and `fileName` variables with the name of the document library and the name of the file you want to upload.
2. Run the project.

The document will be uploaded to the SharePoint document library.

## Author

This project was created by Walid Moheyeldin.
