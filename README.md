# IonosDnsUpdater
### Description:
IonosDnsUpdater, as its name says, updates the DNS A-Records from your web hosting package, triggered by your FritzBox.

### Prerequisites:
- Fritz!Box
- Ionos WebHosting
- Ionos API-Key
- .Net core 6.0

### How to:
deploy the IonosDnsUpdater anywhere in your network. 
i'm using the dot net kestrel service an *{servername}*:60080 with an reverse nginx proxy to *{servername}*:60081.

test, if your service is reachable:
`wget 'http://{servername}:60081/api/v1/IonosDnsUpdate/SetNewIp?ipAddress=127.0.0.1'`

if so, you'll get an 200 response and you can store your url in your FritzBox! DynDNS Service as 
`http://{servername}:60081/api/v1/IonosDnsUpdate/SetNewIp?ipAddress=<ipaddr>`
for Fritz!Box it is necassary to set domainname, user and password, for this serivice it isn't, so you can use any random dummy values.

Every time the Fritz!Box router gets an new ip address, Fritz!Box calls this service and the service updates the configurated dns values.

### Config
The appsettings.json needs mandatory values:
 
        "IonosConfig": {
          "ApiUrl": "https://api.hosting.ionos.com/dns",
          "ApiKey": "yourKeyFromIonos",
          "domain": "example.com",
          "subdomains": [
            "sub.example.com",
            "sub2.example.com"
          ]
        }
- `ApiUrl` the official URL from Ionos
- `ApiKey` if you dont't have a key, pleas register it at Ionos
- `domain` your domain by ionos
- `subdomain` your subdomain, which ip address you'd like to change
