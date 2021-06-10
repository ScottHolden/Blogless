param storageAccountName string
param principalId string

resource storageAccount 'Microsoft.Storage/storageAccounts@2021-04-01' existing = {
  name: storageAccountName
}

resource backendStorageRoleAssign 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = {
  name: guid(storageAccountName, principalId, 'blobDataOwner')
  scope: storageAccount
  properties: {
    principalId: principalId
    roleDefinitionId: '/subscriptions/${subscription().subscriptionId}/providers/Microsoft.Authorization/roleDefinitions/b7e6dc6d-f1e8-4753-8033-0f276bb0955b'
  }
}
