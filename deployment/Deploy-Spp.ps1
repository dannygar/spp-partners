<#
.SYNOPSIS
	Deploys SPP template to Azure.
.DESCRIPTION
	Deploys SPP template to Azure.
.PARAMETER subscriptionId
	The subscription id where the template will be deployed.
.PARAMETER deploymentName
	The deployment name.
.PARAMETER resourceGroupName
	The resource group name.
.PARAMETER resourceGroupLocation
	Optional, a resource group location. If specified, will try to create a new resource group in this location.
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
    $templateFilePath = "deploy-spp.json",

    [string]
    $parametersFilePath = "deploy-spp.parameters.json",

    [string]
    $bacpacFilePath = "SppDbV2.bacpac"
)

#############################################################
# Helper Functions
#############################################################

function SignIn {
    Write-Host "Logging in..."
    try {
        Get-AzureRmContext | Out-Null
    }
    catch {
        if ($_ -like "*Login-AzureRmAccount to login*") {
            Login-AzureRmAccount | Out-Null
        }
        else {
            throw
        }
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

#############################################################
# Script Body
#############################################################

$ErrorActionPreference = "Stop"

SignIn

# Select subscription
if (!$subscriptionId) {
    Write-Host "The following subscriptions are available for you:"
    Get-AzureRmSubscription | Format-Table SubscriptionName, SubscriptionId
    $subscriptionId = Read-Host -Prompt "Copy and paste a subscription id from the list above"
}
Select-AzureRmSubscription -SubscriptionId $subscriptionId

# Register resource providers
$rpNameSpaces = @(
    "microsoft.Web",
    "Microsoft.Sql",
    "Microsoft.Storage",
    "Microsoft.ServiceBus",
    "Microsoft.DocumentDB"
)
Register-ResourceProviders($rpNameSpaces)

# Supply resource group name if neccessary
$createNewRg = $false
if (!$resourceGroupName) {
    $existingRgNames = ((Get-AzureRmResourceGroup) | Select-Object ResourceGroupName).ResourceGroupName
    $prompt = "Type a resource group name from the list above or enter a new one"
    if ($existingRgNames.Length) {
        Write-Host "The following resource groups are available for your subscription:"
        write-Host ""
        for ($i = 1; $i -le $existingRgNames.Count; $i++) {
            Write-Host $i'.' $existingRgNames[$i - 1]
        }
        Write-Host ""
    }
    else {
        Write-Host "No existing resource groups found in the current subscription."
        $prompt = "Please enter a new resource group name"
    }

    $resourceGroupName = Read-Host -Prompt $prompt
    while ([string]::IsNullOrEmpty($resourceGroupName)) {
        Write-Host "No resource group specified."
        $resourceGroupName = Read-Host -Prompt $prompt
    }

    $createNewRg = !$existingRgNames.Contains($resourceGroupName)
}

# Supply location if neccessary
if ($createNewRg -and !$resourceGroupLocation) {
    Write-Host "The following locations are available for your resource group:"
    write-Host ""
    $locations = (Get-AzureRmLocation | Select-Object Location).Location
    for ($i = 1; $i -le $locations.Count; $i++) {
        Write-Host $i'. ' $locations[$i - 1]
    }

    $prompt = "Type a location from the list above"
    write-Host ""
    $resourceGroupLocation = Read-Host -Prompt $prompt

    while ([string]::IsNullOrEmpty($resourceGroupLocation)) {
        Write-Host "No resource group location specified."
        $resourceGroupLocation = Read-Host -Prompt $prompt
    }
    while (!$locations.Contains($resourceGroupLocation)) {
        Write-Host "Resource group location is not valid"
        $resourceGroupLocation = Read-Host -Prompt $prompt
    }
}

# Create or check for existing resource group
if ($createNewRg) {
    Write-Host "Creating resource group '$resourceGroupName' in location '$resourceGroupLocation'..."
    New-AzureRmResourceGroup -Name $resourceGroupName -Location $resourceGroupLocation | Out-Null
}
else {
    Write-Host "Using existing resource group '$resourceGroupName'."
    $resourceGroupLocation = (Get-AzureRmResourceGroup -Name $resourceGroupName).Location
}

# Prepare for database import
Write-Host "Creating a temporary storage account for database import..."
$storageAccountName = "dbimport$(Get-Random)"
New-AzureRmStorageAccount -ResourceGroupName $resourceGroupName `
    -AccountName $storageAccountName `
    -Location $resourceGroupLocation `
    -Type "Standard_LRS" | Out-Null

try {
    $storageContainerName = "dbimportcontainer$(Get-Random)"
    $storageAccountUri = "http://$storageAccountName.blob.core.windows.net/$storageContainerName/$bacpacFilePath"
    $storageAccountKey = $(Get-AzureRmStorageAccountKey -ResourceGroupName $resourceGroupName -StorageAccountName $storageAccountName).Value[0]
    $storageContext = $(New-AzureStorageContext -StorageAccountName $storageAccountName -StorageAccountKey $storageAccountKey)

    # Create a storage container 
    Write-Host "Creating blob container..."
    New-AzureStorageContainer -Name $storageContainerName -Context $storageContext | Out-Null

    # Upload sample database into storage container
    Write-Host "Uploading database bacpac file..."
    Set-AzureStorageBlobContent -Container $storageContainerName -File $bacpacFilePath -Context $storageContext | Out-Null 

    # Deploy templatey
    Write-Host "Deploying template..."
    New-AzureRmResourceGroupDeployment -ResourceGroupName $resourceGroupName `
        -TemplateFile $templateFilePath `
        -TemplateParameterFile $parametersFilePath `
        -dbImportStorageKey (ConvertTo-SecureString -String $storageAccountKey -AsPlainText -Force)`
        -dbImportStorageUri $storageAccountUri

}
catch {
    Write-Host ""
    Write-Host $_
    Write-Host ""

    # On db import or deployment error, delete the whole resource group
    Write-Host "Error deploying template. Removing resource group..."
    Remove-AzureRmResourceGroup -Name $resourceGroupName -Force -Confirm | Out-Null

    Write-Host ""
    Write-Host "Deployment failed."
    exit
}

# Cleanup temporary storage account
try {
    Write-Host "Finishing up deployment..."
    Remove-AzureRmStorageAccount -ResourceGroupName $resourceGroupName -AccountName $storageAccountName -Force | Out-Null
}
catch {
    Write-Host ""
    Write-Host $_
    Write-Host ""
	
    Write-Host "Failed to remove temporary storage acouunt '$storageAccountName'."
    Write-Host "You may manually remove it in the Azure portal."
}

Write-Host ""
Write-Host "Deployment completed!"


# Inject application settings.
Write-Host "Injecting database connection string..."
$outputs = (Get-AzureRmResourceGroupDeployment -ResourceGroupName $resourceGroupName | Select-Object -Last 1).Outputs
$dbAdoConnString = $outputs.dbAdoConnString.Value

$settingsJson = (Get-Content -Path "../src/Spp/Spp.Application.Api/appsettings.json") | ConvertFrom-Json
$activeTenant = $settingsJson.DbTenants.ActiveTenant
$settingsJson.DbTenants.$activeTenant.SppDbConnection = $dbAdoConnString
$settingsJson | ConvertTo-Json | Set-Content -Path "../src/Spp/Spp.Application.Api/appsettings.json"

$settingsJson = (Get-Content -Path "../src/Spp/Spp.Application.Services.Tests/testsettings.json") | ConvertFrom-Json
$activeTenant = $settingsJson.DbTenants.ActiveTenant
$settingsJson.DbTenants.$activeTenant.SppDbConnection = $dbAdoConnString
$settingsJson | ConvertTo-Json | Set-Content -Path "../src/Spp/Spp.Application.Services.Tests/testsettings.json"

# TODO: other settings

Write-Host "Done!"