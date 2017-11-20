# SPP Data Loader Deployment Guide

This guide will walk you the process of deploying the Data Loader infrastructure into an Azure subscription.

> __Note__: This guide assumes that you have already created a SPP tenant database (SppDbV2). You will need this database's connection string later in this guide. If using Azure SQL Database, be sure to add a firewall rule to the Azure SQL Database server allowing the API App to access the database. This shouldn't be an issue if the database is running in the same subscription as the Data Loader.

## Prerequisites

* An Azure Subscription
* Visual Studio 2015 or higher
  * Azure SDK 2.9 or higher
* An existing SPP tenant database (SppDbV2)

## Clone the spp-apiandtools Repository

* If you have not done so already, clone the __spp-apiandtools__ repository to your local machine.
* Check out the __master__ branch.
* If you have not done so already, create a new branch from the __master__ branch for the tenant (team) that you are working with. Check out that branch.

## Set up the Data Loader Resource Group

* Navigate to the Azure Portal.
* Create a new or select an existing resource group that will contain the Data Loader components.

### Create a Cosmos DB Account

> Data Loader schema information is stored in Cosmos DB.

* With the data loader resource group, create a new Azure Cosmos DB account.
* Enter an account ID.
* In the API section, select __SQL (DocumentDB)__ from the dropdown list.
* Once the account is created, navigate to it.
* Click Add Collection to add a new collection.
* Set Collection Id to __sppschemas__.
* Set STORAGE CAPACITY to __Fixed (10GB)__.
* Set INITIAL THROUGHPUT CAPACITY (RU/s) to __400__.
* Leave PARTITION KEY blank.
* In the DATABASE section, enter __sppdatabase__ as the database id.

### Create a Service Bus namespace

> When a user uploads a file, the API sends a message to an Azure Service Bus topic. The processor subscribes to an associated Azure Service Bus subscription and asynchronously processes uploaded files in the background.

* Within the data loader resource group, create a new Service Bus namespace.
* Add a new topic to the Service Bus namespace named __spptopic__.
* Add a new subscription to the __spptopic__ named __sppprocessor__.

### Create a Storage Account

> Uploaded files are stored in tenant-specific blob containers. File metadata (file ID, file name, status, etc.) is tracked in an Azure Storage table. Processor events are logged in an Azure Storage table.

* Within the data loader resource group, create a new Storage account.
* Choose __General purpose__ in the Account kind section.
* Choose __Locally-Redundant Storage (LRS)__ for Replication.

### Create an App Service Plan

> The App Service Plan will host the UI Web App, API App and background processor WebJob.

* Within the data loader resource group, create a new App Service Plan.
* Ensure that the App Service Plan __Pricing tier__ is set to __S1 Standard__ or higher.

### Create an API App and Background Processor WebJob

* Within the data loader resource group, create a new API App.
* Ensure that the __App Service plan__ is set to the plan that you created in the __Setup the App Service Plan__ section.

### Create an Web App (Data Loader UI App)

* Within the data loader resource group , create a new Web App.
* Ensure that the App Service Plan is set to the plan that you created in the __Setup the App Service Plan__ section.


## Data Loader Configurations

### Configure the API App

* Once the API App has been created, navigate to the app in the portal.
* Open the __Application settings__ blade.
* In the __General settings__ section:
    * Set __Always On__ to __On__.
    * Set __ARR Affinity__ to __Off__.
* In the __App settings__ section, add the following key-value pairs:

| Key | Value | Description
| :-- | :---- | :------------
| FileAnalyzed.SenderTopicName | spptopic | Service Bus topic name
| FileUploaded.SenderTopicName | spptopic | Service Bus topic name
| DocumentDbEndpointUrl | __[Cosmos DB account URI]__ | Located in Cosmos DB account *Overview* blade
| DocumentDbAuthKey | __[Cosmos DB Access Key]__ | Located Cosmos DB account *Keys* blade
| DocumentDbSchemaDatabaseId | sppdatabase | Cosmos DB database id
| DocumentDbSchemaCollectionId | sppschemas | Cosmos DB collection id
| ReceiverTopicName | spptopic | Service Bus topic name
| ReceiverSubscriptionName | sppprocessor | Service Bus subscription name

* In the __Connection strings__ section, configure the following connection strings:

| Name | Value | Connection String Type | Description
| :--------------------- | :--------------------- | :---------- | :---------
| ServiceBusConnectionString | __[Service Bus Connection String]__ | Custom | Located in Service Bus *Shared access policy* blade, *RootManageSharedAccessKey* policy
| StorageConnectionString | __[Storage Account Connection String]__ | Custom | Located in Storage Account *Access Key* blade
| SqlConnectionString | __[SPP Database Connection String]__ | SQL Server | Your existing SPP Database connection string

* Save the __Application settings__.
* Open the __\src\SportsDataLoaderv2\SportsDataLoader\SportsDataLoader.sln__ solution in Visual Studio, and find the __SportsDataLoader.WebApi__ project in the Solution Explorer.
* Open __Web.config__, replace __[FileUploaded.SenderTopicName]__ with __spptopic__.
* Replace __[ServiceBusConnectionString]__ with the service bus connection string.
* Replace __[StorageConnectionString]__ with the storage connection string.


### Configure the Web App

* Once the web app has been created, navigate to the __Application settings__ blade.
* Set __Always On__ to __On__.
* Set __ARR Affinity__ to __Off__.
* In the __App settings__ section, configure the following key-value pairs:

| Key | Value | Description
| :---- | :-------- | :----
| ApiBaseUrl            | __[Data Loader API Base URL]__ | This is the URL of the API App that was deployed in the __Setup the API App__ section. <br />__Note__: Do not forget the closing slash
| DefaultCultureCode    | __[Default File Culture Code]__<br />(e.g., en-US) |This culture code will be used when parsing uploaded files. The default culture code can be overridden using the __cultureCode__ query string parameter.
| DefaultTenantId       | __[Default Tenant ID]__ <br />(e.g., 516ab912-6b5e-4ae4-83c0-cd4184be8411) | The Data Loader was designed with multi-tenancy in mind. This is in arbitrary GUID that will be associated with files that are uploaded. The default tenant ID can be overridden using the __tenantId__ query string parameter.

* Save the __Application settings__.

* Make a note of the Web App's URL. From the portal, navigate to the API App that you created in the __Create the API App__ section and open the __CORS__ blade.
* From the __CORS__ blade, add a new __Allowed Origin__ using the Web App's URL. Save your changes.

* Open the __\src\SportsDataLoaderv2\SportsDataLoader\SportsDataLoader.sln__ solution in Visual Studio, and find the __SportsDataLoader.WebApp__ project in the Solution Explorer.
* Open __Web.config__, replace __[FileUploaded.SenderTopicName]__ with __spptopic__.
* Replace __[ServiceBusConnectionString]__ with the service bus connection string from the __Configure the Web Api__ section.
* Replace __[StorageConnectionString]__ with the storage connection string from the __Configure the Web Api__ section.


> For more information regarding configuring Azure API/Web App Application settings, refer to https://azure.microsoft.com/en-us/documentation/articles/web-sites-configure/#application-settings.

### Configure the WebJob

* Open the __\src\SportsDataLoaderv2\SportsDataLoader\SportsDataLoader.sln__ solution in Visual Studio, and find the __SportsDataLoader.MessageProcessor.Console__ project in the Solution Explorer.
* Open App.config, in __appSettings__ and __connectionStrings__ sections, replace all values with the ones from the __Configure the Web Api__ section (you may ignore __ApplicationInsightsInstrumentationKey__).


## Deploy Data Loader

### Publish Web Api

* Open the __\src\SportsDataLoaderv2\SportsDataLoader\SportsDataLoader.sln__ solution in Visual Studio, and find the __SportsDataLoader.WebApi__ project in the Solution Explorer.
* Right-click the __SportsDataLoader.WebApi__ in the Solution Explorer project and select __Publish...__ from the menu.
* Select the __Microsoft Azure App Service__ publishing target and, if necessary, provide your Azure credentials to access the appropriate subscription.
* Select the API App that you created earlier then click __OK__.
* Click __Publish__ to publish the API App.
d the __SportsDataLoader.MessageProcessor.Console__ project.
* Right-click on the __SportsDataLoader.MessageProcessor.Console__ project and select __Publish as Azure WebJob...__ from the menu.
* Select the __Microsoft Azure App Service__ publishing target and, if necessary, provide your Azure credentials to access the appropriate Azure subscription.
* Select the API App that you created earlier then click __OK__.
* Click __Publish__ to publish the WebJob.

### Publish WebJob

* Find the __SportsDataLoader.MessageProcessor.Console__ project.
* Right-click on the __SportsDataLoader.MessageProcessor.Console__ project and select __Publish as Azure WebJob...__ from the menu.
* Select the __Microsoft Azure App Service__ publishing target and, if necessary, provide your Azure credentials to access the appropriate Azure subscription.
* Select the API App that you created earlier in this section then click __OK__.
* Click __Publish__ to publish the WebJob.

> For additional information regarding deploying Azure WebJobs, refer to https://azure.microsoft.com/en-us/documentation/articles/websites-webjobs-resources/#deploy.

### Publish Web App

* Locate the __SportsDataLoader.Web__ project in the Solution Explorer.
* Right-click the __SportsDataLoader.Web__ project in the Solution Explorer and select __Publish...__ from the menu.
* Select the __Microsoft Azure App Service__ publishing target and, if necessary, provide your Azure credentials to access the appropriate Azure subscription.
* Select the Web App that you created earlier then click __OK__.
* Click __Publish__ to publish the Web App.

> For more information regarding deploying Azure Web Apps, refer to https://azure.microsoft.com/en-us/documentation/articles/web-sites-dotnet-get-started/#deploy-the-web-project-to-the-azure-we
