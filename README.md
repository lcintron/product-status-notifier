# Product Status Notifier

![Buildkite pass](https://img.shields.io/buildkite/3826789cf8890b426057e6fe1c4e683bdf04fa24d498885489/master.svg)
![GitHub license](https://img.shields.io/github/license/mashape/apistatus.svg)

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
Published under MIT license.

MIT License

Copyright (c) 2018 Luis Cintron

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
