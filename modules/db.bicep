param prefix string

resource backendStorage 'Microsoft.Storage/storageAccounts@2021-04-01' = {
  name: '${prefix}db'
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
  resource blob 'blobServices@2021-04-01' = {
    name: 'default'
    properties: {
      deleteRetentionPolicy:{
        enabled: true
        days: 7
      }
      isVersioningEnabled: true
    }
  }
}

output storageAccountName string = backendStorage.name
