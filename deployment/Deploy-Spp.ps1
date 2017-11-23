<#
.SYNOPSIS
	Deploys SPP template to Azure.
.DESCRIPTION
	Deploys SPP template to Azure.
.PARAMETER subscriptionId
	The subscription id where the template will be deployed.
.PARAMETER resourceGroupName
	The resource group name.
.PARAMETER resourceGroupLocation
	Optional, a resource group location. If specified, will try to create a new resource group in this location.
.PARAMETER deploymentName
	The deployment name.
.PARAMETER templateFilePath
	Optional, path to the template file. Defaults to spp.template.json.
.PARAMETER parametersFilePath
	Optional, path to the parameters file. Defaults to spp.parameters.json. If file is not found, will prompt for parameter values based on template.
.PARAMETER backpacFilePath
	Optional, path to the SppDbV2 bacpac file. Defaults to SppDbV2.bacpac.
#>

param(
    [string]
    $subscriptionId,

    [string]
    $resourceGroupName,

    [string]
    $resourceGroupLocation,

    [string]
    $deploymentName,

    [string]
    $templateFilePath = "deploy-spp.json",

    [string]
    $parametersFilePath = "deploy-spp.parameters.json",

    [string]
    $bacpacFilePath = "SppDbV2.bacpac"
)


#region Helper Functions
function Select-ListItem([array]$list) {
    function Show-List([array]$list, [int]$pos) {
        for ($i = 0; $i -le $list.length; $i++) {
            if ($list[$i] -ne $null) {
                $listItem = $list[$i]
                if ($i -eq $pos) {
                    Write-Host "> $($listItem)" -ForegroundColor Green
                }
                else {
                    Write-Host "  $($listItem)"
                }
            }
        }
    }
	
    if ($list.Length -eq 0) {
        return;
    }

    $pos = 0
    $virtualKeycode = 0

    Write-Host
    $curTop = [System.Console]::CursorTop
    [console]::CursorVisible = $false

    Show-List $list $pos
    While ($virtualKeycode -ne 0x0D) {
        $key = $host.ui.rawui.readkey("NoEcho,IncludeKeyDown")
        $virtualKeycode = $key.virtualkeycode

        If ($virtualKeycode -eq 38 -or $key.Character -eq 'k') { $pos-- }
        If ($virtualKeycode -eq 40 -or $key.Character -eq 'j') { $pos++ }
        if ($pos -ge $list.Length) { $pos = 0 }
        if ($pos -lt 0) { $pos = $list.Length - 1 }
		
        [System.Console]::SetCursorPosition(0, $curTop)
        Show-List $list $pos
    }

    [console]::CursorVisible = $true
    Write-Host

    return $pos
}

function SignIn {
    Write-Host "Logging in..."
    if ((Get-AzureRmContext).Account -eq $null) {
        Login-AzureRmAccount | Out-Null
    }
}

function Register-ResourceProviders([array]$rpNamespaces) {
    if ($rpNamespaces.Length) {
        Write-Host "Registering resource providers..."
        foreach ($rpNamespace in $rpNamespaces) {
            Write-Host "Registering '$rpNamespace'..."
            Register-AzureRmResourceProvider -ProviderNamespace $rpNamespace | Out-Null
        }
    }
}

function Show-ParamHint([string]$param) {
    Write-Host
    Write-Host -NoNewLine "[$param]" -ForegroundColor Blue
    Write-Host " not provided."
}

function CreateBlobContainer([string]$storageAccountName, [string]$storageContainerName) {

    $storageAccountKey = $(Get-AzureRmStorageAccountKey -ResourceGroupName $resourceGroupName `
        -Name $storageAccountName).Value[0]
    $storageContext = New-AzureStorageContext -StorageAccountName $storageAccountName `
        -StorageAccountKey $storageAccountKey
    
    # 4. Create a storage container
    New-AzureStorageContainer -Name $storageContainerName -Context $storageContext | Out-Null
}

#endregion

#region Environment Setup
$ErrorActionPreference = "Stop"

SignIn
#endregion

#region Parameters Supplement
if (!$subscriptionId) {
    Show-ParamHint("subscriptionId")

    Write-Host "Getting available subscriptions for you..."
    $subscriptions = Get-AzureRmSubscription

    Write-Host "Please choose a subscription for the deployment:"
    $pos = Select-ListItem ($subscriptions | ForEach-Object { "$($_.SubscriptionId)    $($_.Name)" })
    $subscriptionId = $subscriptions[$pos].Id
}
Write-Host "Switching to subscription '$subscriptionId'..."
Select-AzureRmSubscription -SubscriptionId $subscriptionId

$resourceGroup = $null
if (!$resourceGroupName) {
    Show-ParamHint("resourceGroupName")

    $message = "Do you want to use an existing resource group?"
    $yes = New-Object System.Management.Automation.Host.ChoiceDescription "&Yes - use existing", `
        "Use an existing resource group"
    $no = New-Object System.Management.Automation.Host.ChoiceDescription "&No - create new", `
        "Create a new resource group"
    $options = [System.Management.Automation.Host.ChoiceDescription[]]($yes, $no)
    $result = $host.UI.PromptForChoice("", $message, $options, 0)
    Write-Host

    if ($result -eq 0) {
        Write-Host "Getting existing resuorce groups..."
        $existingResourceGroups = Get-AzureRmResourceGroup
        if ($existingResourceGroups.Length -eq 0) {
            Write-Host "No existing resource group found in the current subscription."
        }
        else {
            Write-Host "Please choose a resource group:"
            $pos = Select-ListItem $existingResourceGroups.ResourceGroupName
            $resourceGroup = $existingResourceGroups[$pos]
            $resourceGroupName = $resourceGroup.ResourceGroupName
            $resourceGroupLocation = $resourceGroup.Location
        }
    }

    if (!$resourceGroup) {
        $prompt = "Please enter a new resource group name"
        $resourceGroupName = Read-Host -Prompt $prompt
        while ([string]::IsNullOrEmpty($resourceGroupName)) {
            Write-Host "Resource group name cannot be empty."
            $resourceGroupName = Read-Host -Prompt $prompt
        }
    }
}

if (!$resourceGroupLocation) {
    Show-ParamHint("resourceGroupLocation")

    Write-Host "Getting avaiable locations..."
    $locations = (Get-AzureRmLocation).Location

    Write-Host "Please select a location for your resource group:"
    $pos = Select-ListItem $locations
    $resourceGroupLocation = $locations[$pos]
}

if (!$deploymentName) {
    Show-ParamHint("deploymentName")

    $defaultName = "deploy-spp"
    $deploymentName = Read-Host -Prompt "Please enter a deployment name ($defaultName)"
    if ([String]::IsNullOrEmpty($deploymentName)) {
        $deploymentName = $defaultName
    }
}
#endregion

#region Deployment Steps
Write-Host
Write-Host "Starting deployment..."

# 1. Create new resource group if neccessary
if (!$resourceGroup) {
    Write-Host "Creating resource group '$resourceGroupName' in location '$resourceGroupLocation'..."
    New-AzureRmResourceGroup -Name $resourceGroupName -Location $resourceGroupLocation | Out-Null
}

# 2. Register resource providers
$rpNameSpaces = @(
    "microsoft.Web",
    "Microsoft.Sql",
    "Microsoft.Storage",
    "Microsoft.ServiceBus",
    "Microsoft.DocumentDB"
)
Register-ResourceProviders($rpNameSpaces)

$storageAccountName = $null
try {
    # 3. Create a temporary storage account for database import
    Write-Host "Creating a temporary storage account for database import..."
    $storageAccountName = "dbimport$(Get-Random)"
    New-AzureRmStorageAccount -ResourceGroupName $resourceGroupName `
        -AccountName $storageAccountName `
        -Location $resourceGroupLocation `
        -Type "Standard_LRS" | Out-Null
	
    $storageContainerName = "dbimportcontainer$(Get-Random)"
    $storageAccountUri = "http://$storageAccountName.blob.core.windows.net/$storageContainerName/$bacpacFilePath"
    $storageAccountKey = $(Get-AzureRmStorageAccountKey -ResourceGroupName $resourceGroupName `
            -Name $storageAccountName).Value[0]
    $storageContext = New-AzureStorageContext -StorageAccountName $storageAccountName `
        -StorageAccountKey $storageAccountKey

    # 4. Create a storage container 
    Write-Host "Creating blob container..."
    New-AzureStorageContainer -Name $storageContainerName -Context $storageContext | Out-Null

    # 5. Upload sample database into storage container
    Write-Host "Uploading database bacpac file..."
    Set-AzureStorageBlobContent -Container $storageContainerName `
        -File $bacpacFilePath -Context $storageContext | Out-Null
	
    # 5. Test deployment
    Write-Host "Verifying deployment parameters..."
    Test-AzureRmResourceGroupDeployment -ResourceGroupName $resourceGroupName `
        -TemplateFile $templateFilePath `
        -TemplateParameterFile $parametersFilePath `
        -sqlServerAdminLogin "testsqladmin" `
        -sqlServerAdminPassword (ConvertTo-SecureString -String "testsqlpassword123=" -AsPlainText -Force) `
        -dbImportStorageKey (ConvertTo-SecureString -String $storageAccountKey -AsPlainText -Force) `
        -dbImportStorageUri $storageAccountUri

    # 6. Deploy templatey
    Write-Host "Deploying template..."
    New-AzureRmResourceGroupDeployment -ResourceGroupName $resourceGroupName `
        -TemplateFile $templateFilePath `
        -TemplateParameterFile $parametersFilePath `
        -dbImportStorageKey (ConvertTo-SecureString -String $storageAccountKey -AsPlainText -Force)`
        -dbImportStorageUri $storageAccountUri | Out-Null

}
catch {
    Write-Host ""
    Write-Host $_
    Write-Host ""

    # On db import or deployment error, delete the whole resource group
    Write-Host "Error deploying template. Removing resource group..."
    Remove-AzureRmResourceGroup -ResourceGroupName $resourceGroupName | Out-Null

    Write-Host
    Write-Host "Deployment failed."
    exit
}

# Cleanup temporary storage account
try {
    Write-Host "Finishing up deployment..."
    Write-Host "Removing temparory storage account..."
    Remove-AzureRmStorageAccount -ResourceGroupName $resourceGroupName -AccountName $storageAccountName -Force
}
catch {
    Write-Host
    Write-Host $_
    Write-Host
	
    Write-Host "Failed to remove temporary storage acouunt '$storageAccountName'. " `
        "You may manually remove it in the Azure portal."
}

Write-Host
Write-Host "Deployment completed!"
Write-Host
#endregion

#region Configurations Injection
Write-Host "Injecting database connection string..."
$outputs = (Get-AzureRmResourceGroupDeployment -ResourceGroupName $resourceGroupName | Select-Object -Last 1).Outputs
$dbAdoConnString = $outputs.dbAdoConnString.Value
$apiEndpoint = $outputs.apiEndpoint.Value
$apiTokenEndpoint = $apiEndpoint + '/api/v1/auth/b2b/token'
$blobBaseUrl = $outputs.blobBaseUrl.Value
$blobName = $outputs.blobName.Value
Read-Host -Prompt "Press Enter to continue"

# Inject credentials to Web App
$configJson = (Get-Content -Path "../src/Spp/Spp.Presentation.Admin.WebApp/Client/config.json") | ConvertFrom-Json

# Create a storage container to generate blob sas token
$blobContainerName = "content"
$storageAccountKey = $(Get-AzureRmStorageAccountKey -ResourceGroupName $resourceGroupName `
    -Name $blobName).Value[0]
$storageCtxt = New-AzureStorageContext -StorageAccountName $blobName `
    -StorageAccountKey $storageAccountKey

New-AzureStorageContainer -Name $blobContainerName -Context $storageCtxt | Out-Null
$blobSasToken =  New-AzureStorageBlobSASToken -Container $blobContainerName -Blob blobName -Permission rwd -Context $storageCtxt

$configJson.apiEndpoint = $apiEndpoint
$configJson.apiTokenEndpoint = $apiTokenEndpoint
$configJson.blob_baseUrl = $blobBaseUrl + '/' + $blobContainerName
$configJson.blob_sasToken = $blobSasToken

# Collect Azure AD Credentials
Read-Host -Prompt "Now follow the Azure AD deployment steps to provide the necessary Azure credentials. Once complete press Enter to continue this deployment."
$adTenant = Read-Host "Please enter your Azure AD Tenant URL: "
$clientId = Read-Host "Please enter your Azure Client ID: "
$instance = Read-Host "Please enter your Azure AD Instance URL: "
$instanceB2b = Read-Host "Please enter your Azure AD B2B Instance URL: "
$adTenantB2b = Read-Host "Please enter your Azure AD B2B Tenant URL: "
$audienceB2b = Read-Host "Please enter your Azure AD B2B Audience URL: "
$clientKeyB2b = Read-Host "Please enter your Azure AD B2B Client Key: "
$clientIdB2b = Read-Host "Please enter your Azure AD B2B Client ID: "

$configJson.tenant = $adTenant
$configJson.client_id = $clientId
$configJson.instance = $instance
$configJson.aadInstance_b2b = $instanceB2b

$configJson.tenant_b2b = $adTenantB2b
$configJson.audience_b2b = $audienceB2b
$configJson.clientKey_b2b = $clientKeyB2b
$configJson.clientId_b2b = $clientIdB2b

$configJson | ConvertTo-Json | Set-Content -Path "../src/Spp/Spp.Presentation.Admin.WebApp/Client/config.json"
Read-Host -Prompt "Press Enter to continue"

# Inject credentials to API
$settingsJson = (Get-Content -Path "../src/Spp/Spp.Application.Api/appsettings.json") | ConvertFrom-Json
$activeTenant = $settingsJson.DbTenants.ActiveTenant
$settingsJson.DbTenants.$activeTenant.SppDbConnection = $dbAdoConnString
$settingsJson | ConvertTo-Json | Set-Content -Path "../src/Spp/Spp.Application.Api/appsettings.json"

# Inject credentials to Tests
$settingsJson = (Get-Content -Path "../src/Spp/Spp.Application.Services.Tests/testsettings.json") | ConvertFrom-Json
$activeTenant = $settingsJson.DbTenants.ActiveTenant
$settingsJson.DbTenants.$activeTenant.SppDbConnection = $dbAdoConnString
$settingsJson | ConvertTo-Json | Set-Content -Path "../src/Spp/Spp.Application.Services.Tests/testsettings.json"


# TODO: other settings
#endregion

Write-Host "Done!"