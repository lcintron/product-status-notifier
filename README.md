# Product Status Notifier
Scrappes product pages of specific sites and checks for price/availability changes. Sends notification using IFTTT webhooks if changed.

## Features
- Poll web stores for product info
- Detect product parameter changes
- Sends notifications via WebHooks
- Customizable polling frequency

## Current Scrapers
1. Newegg.com
2. Tigerdirect.com
3. B&H Photo
4. SabrePC.com

## How To Setup Notifications
- Uses IFTTT webhooks
- Create a webhook channel on IFTTT and get a key
- Pass the key using parameter  '-key YourWebHookKey'
 
## Build
 - Language: C#
 - Tested on .Net Framework 4.6.1
 
## Run
- Command Line:
  > .\ProductMonitorConsole.exe\
  
  -k, --key           (Default: ) IFTTT webhook key.
     
  -e, --event         (Default: ) IFTTT webhook event name.
     
  -i, --interval      (Default: 5) Time interval to check products.
     
  -j, --jsonconfig    (Default: ) JSON configuration file to use.
     
  --help              Display this help screen.

- Double click .exe and use commands to set webhooks key, webhook event name, polling interval, or json configuration file.

## Dependencies
- AngleSharp by AngleSharp (NuGet)
- CommandLineParser20 by Giacomo Stelluti Scala(NuGet)
- Jint by Sebastien Ros (NuGet)
- Newtonsoft.Json by James Newton-King (NuGet)

## License
