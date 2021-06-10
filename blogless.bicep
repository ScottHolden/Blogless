var entropy = uniqueString(resourceGroup().id)
var prefix = 'blgls${entropy}'

module web 'modules/web.bicep' = {
  name: '${prefix}web'
  params: {
    prefix: prefix
  }
}

module webRoleAssign 'modules/blobDataOwner.bicep' = {
  name: '${prefix}webRoleAssign'
  params: {
    principalId: func.outputs.principalId
    storageAccountName: web.outputs.storageAccountName
  }
}

module db 'modules/db.bicep' = {
  name: '${prefix}db'
  params: {
    prefix: prefix
  }
}

module dbRoleAssign 'modules/blobDataOwner.bicep' = {
  name: '${prefix}dbRoleAssign'
  params: {
    principalId: func.outputs.principalId
    storageAccountName: db.outputs.storageAccountName
  }
}

module func 'modules/func.bicep' = {
  name: '${prefix}func'
  params: {
    prefix: prefix
    settings: {
      BloglessDB__accountName: db.outputs.storageAccountName
      BloglessWeb__accountName: web.outputs.storageAccountName
    }
  }
}
