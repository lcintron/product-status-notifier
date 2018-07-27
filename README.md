# Product Status Notifier
Scrappes product pages of specific sites and checks for price/availability changes. Sends notification using IFTTT webhooks if changed.

## Features
    - Poll web stores for product info
    - Detect product parameter changes
    - Sends notifications via WebHooks
    - Customizable polling frequency

### Scrapers
    1. Newegg.com
    2. Tigerdirect.com
    3. B&H Photo
    4. SabrePC.com

### Setup Notifications
 - Uses IFTTT webhooks
 
## Build
 - Language: C#
 - Tested on .Net Framework 4.6.1

## Dependencies
- AngleSharp by AngleSharp (NuGet)
- CommandLineParser20 by Giacomo Stelluti Scala(NuGet)
- Jint by Sebastien Ros (NuGet)
- Newtonsoft.Json by James Newton-King (NuGet)

## License
