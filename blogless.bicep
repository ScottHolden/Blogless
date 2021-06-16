var entropy = uniqueString(resourceGroup().id)
var prefix = 'blgls${entropy}'

module web 'modules/web.bicep' = {
  name: '${prefix}webDeploy'
  params: {
    prefix: prefix
  }
}

module db 'modules/db.bicep' = {
  name: '${prefix}dbDeploy'
  params: {
    prefix: prefix
  }
}

module func 'modules/func.bicep' = {
  name: '${prefix}funcDeploy'
  params: {
    prefix: prefix
    settings: {
      BloglessDB__accountName: db.outputs.storageAccountName
      BloglessWeb__accountName: web.outputs.storageAccountName
    }
  }
}

module webRoleAssign 'modules/blobDataOwner.bicep' = {
  name: '${prefix}webRoleAssign'
  params: {
    principalId: func.outputs.principalId
    storageAccountName: web.outputs.storageAccountName
  }
}

module dbRoleAssign 'modules/blobDataOwner.bicep' = {
  name: '${prefix}dbRoleAssign'
  params: {
    principalId: func.outputs.principalId
    storageAccountName: db.outputs.storageAccountName
  }
}
