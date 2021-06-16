param prefix string
param settings object = {}

var baseSettings = {
  AzureWebJobsStorage__accountName: storage.name
  FUNCTIONS_EXTENSION_VERSION: '~3'
  FUNCTIONS_WORKER_RUNTIME: 'dotnet'
  WEBSITE_RUN_FROM_PACKAGE: '1'
}

resource storage 'Microsoft.Storage/storageAccounts@2021-04-01' = {
  name: '${prefix}func'
  location: resourceGroup().location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
  properties: {
    allowBlobPublicAccess: false
    supportsHttpsTrafficOnly: true
    minimumTlsVersion: 'TLS1_2'
  }
}
resource appServicePlan 'Microsoft.Web/serverfarms@2021-01-01' = {
  name: prefix
  location: resourceGroup().location
  sku: {
    tier: 'Dynamic'
    name: 'Y1'
  }
}
resource appService 'Microsoft.Web/sites@2021-01-01' = {
  name: '${prefix}mgmt'
  location: resourceGroup().location
  kind: 'functionapp'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: appServicePlan.id
    clientAffinityEnabled: false
    httpsOnly: true
  }
  resource appsettings 'config@2021-01-01' = {
    name: 'appsettings'
    properties: union(baseSettings, settings)
  }
}
resource storageRoleAssign 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = {
  name: guid(appService.id, 'blobDataOwner')
  scope: storage
  properties: {
    principalId: appService.identity.principalId
    roleDefinitionId: '/subscriptions/${subscription().subscriptionId}/providers/Microsoft.Authorization/roleDefinitions/b7e6dc6d-f1e8-4753-8033-0f276bb0955b'
  }
}
output principalId string = appService.identity.principalId
