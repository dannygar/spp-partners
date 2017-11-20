spp-parterners
================================================
> Microsoft Sports Performance Platform for parterners.

SPP is a platform that enables professional and amateur sports teams to collect and analyze sports data in an aggregated way and build predictive analytical reports from this data.

## Deployment

### Prerequisites

The project requires [Visual Studio 2017](https://www.visualstudio.com/) and [Azure PowerShell](https://docs.microsoft.com/en-us/powershell/azure/install-azurerm-ps) installed.

You will also need an active Azure subscription to deploy SPP. If you don't have one yet, go to the [Microsoft Azure](https://azure.microsoft.com) website to sign up.

Make sure to clone this repo your local machine before continuing the following steps.

```bash
git clone git@github.com:Microsoft-Projects/spp-v2.git
```

### Setup Azure AD

TODO - Working in progress

### Deploy SPP Azure Resource Group

Navigate to the [deployment](deployment) folder in your local repo. Run the [Deploy-Spp.ps1](deployment/Deploy-Spp.ps1) deployment script in PowerShell to deploy SPP Azure resource group:

```
.\Deploy-Spp.ps1
```

Once the deployment succeeds, the deployment script will automaticlly inject database connection string into application setting files for you.

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
