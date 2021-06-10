param prefix string

resource webStorage 'Microsoft.Storage/storageAccounts@2021-04-01' = {
  name: '${prefix}web'
  location: resourceGroup().location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
  properties: {
    supportsHttpsTrafficOnly: true
    minimumTlsVersion: 'TLS1_2'
  }
}

var webStorageHostname = replace(replace(webStorage.properties.primaryEndpoints.web, 'https://', ''), '/', '')

resource webCdn 'Microsoft.Cdn/profiles@2020-09-01' = {
  name: '${prefix}cdn'
  location: resourceGroup().location
  sku: {
    name: 'Standard_Microsoft'
  }
  resource endpoint 'endpoints@2020-09-01' = {
    name: prefix
    location: resourceGroup().location
    properties: {
      originHostHeader: webStorageHostname
      isHttpAllowed: true
      isHttpsAllowed: true
      queryStringCachingBehavior: 'IgnoreQueryString'
      origins: [
        {
          name: webStorage.name
          properties: {
            hostName: webStorageHostname
          }
        }
      ]
      deliveryPolicy: {
        rules:[
          {
            name: 'HttpsRedirect'
            order: 1
            conditions: [
              
              {
                name: 'RequestScheme'
                parameters: {
                  '@odata.type':  '#Microsoft.Azure.Cdn.Models.DeliveryRuleRequestSchemeConditionParameters'
                  operator: 'Equal'
                  matchValues: [
                      'HTTP'
                  ]
                }
              }
            ]
            actions: [
              {
                name:'UrlRedirect'
                parameters:{
                  '@odata.type': '#Microsoft.Azure.Cdn.Models.DeliveryRuleUrlRedirectActionParameters'
                  redirectType: 'Found'
                  destinationProtocol: 'Https'
                }
              }
            ]
          }
        ]
      }
    }
  }
}

output storageAccountName string = webStorage.name
