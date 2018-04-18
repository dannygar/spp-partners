spp-partners
================================================
> Microsoft Sports Performance Platform for partners.

SPP is a platform that enables professional and amateur sports teams to collect and analyze sports data in an aggregated way and build predictive analytical reports from this data.

## Deployment

### Prerequisites

The project requires [Visual Studio 2017](https://www.visualstudio.com/) and [Azure PowerShell](https://docs.microsoft.com/en-us/powershell/azure/install-azurerm-ps) (version 5.0.0 or higher) installed.

You will also need an active Azure subscription to deploy SPP. If you don't have one yet, go to the [Microsoft Azure](https://azure.microsoft.com) website to sign up.

Please fork this repo and clone it to your local machine before continuing the following steps.

### Deploy SPP Azure Resource Group

Navigate to the [deployment](deployment) folder in your local repo. Run the [Deploy-Spp.ps1](deployment/Deploy-Spp.ps1) script in PowerShell and follow the prompt to deploy SPP Azure resource group:

```
.\Deploy-Spp.ps1
```

Once completed, you will find the following Azure resources in your SPP resource group:
 - A SQL server.
 - A SQL database containing sample SPP data.
 - An APP Service Plan for Web APIs and APPs.
 - An API APP for SPP.
 - A Web APP for SPP admin portal.
 - A Storage Account for SPP admin portal.
 - A Storage Account for user images.
 - An API APP for SPP data loader.
 - A Web APP for SPP data loader.
 - A Service Bus for SPP data loader.
 - A Cosmos DB for SPP data loader schema files.

The deployment script will also set up continuous integration and publish code for SPP API. However, for data loader and admin portal, you may need to do it manually at this time. Please check these [instructions](src/Spp.DataLoader/README.md) on how to manually set up SPP data loader.

### Setup Azure AD

In the past, to configure SPP Azure Active Directory authentication, you were required to integrate SPP with both Azure Active Directory B2B and Azure Active Directory B2C separate systems. This SPP release uses a new **Azure AD v2.0** endpoint authentication API version that enables you to sign in both types of accounts using one simple integration.

> ***Note:*** If you don’t have your own Azure AD tenant already created, you can create one by using the instructions in this [Azure AD Get Started]( https://docs.microsoft.com/en-us/azure/active-directory/get-started-azure-ad)

To build a client app, whether it is a Surface UWP desktop app, mobile app or a web hosted site, that authenticates your users with SPP, you’ll first need to register SPP APIs with Microsoft Apps registration portal. To register your SPP APIs with the Azure AD v2.0 endpoint you can follow the instructions in this [Active Directory v2 App Registration](https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-v2-app-registration).

During the registration at the portal, make sure you choose both: the Web Platform with the Redirect URL pointing to the SPP API endpoint (e.g. https://sppapi.azurewebsites.net), which you can obtain from the previous step deploying the SPP Azure Resource group, and the Native Application platform to support your native Client UWP app or a new mobile app.
Make copy of the **Application Id** (it will be used in the further steps).

### Add a new user to the SPP Database

Getting through the hoops of user login to Azure AD will get you only a half way through. You may be authenticated with Active Directory, but you'd still need to get the authorization to access the SPP database. This is done as the multi-factor authentication (MFA) to better protect the SPP resources and to allow adding a layer of authorization. As there may be several different roles of users (e.g. coaches, players, administrators), the need for an additional layer of user authorization is obvious. In SPP it is accomplished via SPP Database user roles and user sessions. To add/authorize a new user to SPP database, you should follow the next steps:

- Go to Azure portal - Active Directory blade and navigate to Users and groups. Click Users at the bottom. Select the user you want to grant access permissions to SPP database and then copy the __Object ID__ value from the Essentials properties of that user. It will be presented in the Guid format.

- Open your SPP SQL Database in any suitable editor of your choice. We personally like SQL Management Studio, which is a free product you can download and install from Microsoft public website.

- Navigate to the User table in your SPP database and open it for adding a new record.

- Add a new line to the existing Users records (if any), and enter all user pertaining information in the associated columns. Make sure that the user's email address matches the one was used to sign this user up in the SPP Client application, and that the isEnabled and isActive fields are set to true.

- Use the copied user's UniqueId or Object ID you've copied in the previous steps to paste in the AADId column. Save your new record and make the copy of the record Id.

- Open the UserTeam table and insert a new record with your User Id, Team Id, and the Start and End dates.

- Open the SessionUser table and then insert a new record with the corresponding Session Id (default: 1), and the User ID.

- In the SQL Query window, execute the following two SQL queries and validate that the result renders the User Id from the first query and the Session Id from the second one by replacing the `{User Object ID}` parameter with your own User Object ID:

```SQL
SELECT u.[Id]  
  FROM [dbo].[User] u
  INNER JOIN UserTeam ut on ut.UserId = u.Id
  WHERE u.AADId = '{User Object ID}'

SELECT s.[Id]  
  FROM [dbo].[Session] s
  INNER JOIN SessionUser su on su.SessionId = s.Id
  INNER JOIN [dbo].[User]  u on u.Id = su.UserId
  WHERE u.AADId = '{User Object ID}'
```

### Native Client App Configuration Settings

When you launch for the first time your native Surface client App, it will prompt you to enter the App configuration settings on its settings page. The following is the description of each setting you’d need to provide:

**API Endpoint Url**: the SPP API endpoint Url in the following format: `https://[your azure deployment domain]/api`.

- **Client Id**: the Application Id you’ve copied from the Microsoft Apps Registration portal (e.g. 786b73b7-8f59-45e7-9449-580f45465d90d).

- **Tenant Id**: The Unique Id of the Azure Active Directory tenant – it is your tenant Directory ID, which can be obtain from the Azure Active Directory Properties in Azure Portal.

- **ML Endpoint Url**: this is the Azure Machine Learning endpoint URL pointing to the Azure Machine Learning API web service providing the training load recommendations. Unless you develop your own recommendations, you can choose to use the default one: ` https://ussouthcentral.services.azureml.net/workspaces/320fa3d96f05432184e98eaff316509b/services/8126537b7f1e43d993d968a469907b9f/execute?api-version=2.0&details=true`.

- **ML Client Key**: this is the secret key for the AML Machine Learning Web service API. Use either your own ML Key, or leave the default one: `4vCGbQhsWvt1mlm8RzkvDeJ9vEq1Z6I71v4Wm8wA6vv+SDXIEwqj6Zg8vA76O68fFFb14idf4577LqoQQxWlEg==`.

- **Default Session Date**: For the demo client app it is October 12, 2016, but for your own data, it may be a different date.
Once you’ve satisfied with the entries, save them into your local configuration location by clicking **Save Settings** button.

For the convenience of setting up and running a demo Seahawks client application, please use these demo settings:
**API Endpoint URL**: https://sppapi-dev-test.azurewebsites.net/api
**Client Id**: 436b73b7-8f59-45e7-9449-580fb9a5d90d
**Tenant Id**: 0802107c-ed17-4b41-9f91-96250f0b54d4
**ML Endpoint URL**: https://ussouthcentral.services.azureml.net/workspaces/320fa3d96f05432184e98eaff316509b/services/8126537b7f1e43d993d968a469907b9f/execute?api-version=2.0&details=true
**ML Client Key**: 4vCGbQhsWvt1mlm8RzkvDeJ9vEq1Z6I71v4Wm8wA6vv+SDXIEwqj6Zg8vA76O68fFFb14idf4577LqoQQxWlEg==
**Default Session Date**: October, 12, 2016



## Development

### Project Structure

The project consists of the main SPP solution and the DataLoader tool. You may find all source code under the [src](src) folder, and here is a quick overview of the complete code structure:

    .
    ├── Spp                                      # SPP Main Source Folder
    │   ├── Spp.Presentation.User.Client         # SPP User UWP APP
    │   ├── Spp.Presentation.User.FaceTrainer    # SPP User Face Trainer APP
    │   ├── Spp.Presentation.Admin.WebApp        # SPP Admin Web APP (obsolete)
    │   ├── Spp.Application.Api                  # SPP Web API
    │   ├── Spp.Application.Core                 # SPP Web API DTO and Service Contracts
    │   ├── Spp.Application.Services             # SPP Web API Services Implementation
    │   ├── Spp.Application.Services.Tests       # Test Project for SPP Web API Services
    │   └── Spp.Data                             # SPP Data Entities and DB Context
    └── Spp.DataLoader                           # DataLoader Source Folder (obsolete)
        └── ...

Please note that the admin web app and the DataLoader tool are obsolete and we are currently still working on refactoring them. You may try out and explore the code if you still want to, but there might be breaking changes in the future.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

