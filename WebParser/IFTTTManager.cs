using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.IO;

namespace WebParser.WebManagers
{
    public class IFTTTWebhookData
    {
        public String value1;
        public String value2;
        public String value3;
    }
    public class IFTTTManager:WebManager
    {
        public bool SendIFTTNotification(String eventName, String webhookKey, String value1 = "", String value2 = "", String value3 = "")
        {
            try
            {
                IFTTTWebhookData data = new IFTTTWebhookData
                {
                    value1 = value1,
                    value2 = value2,
                    value3 = value3
                };

                String content = JsonConvert.SerializeObject(data);
                String getUrl = "https://maker.ifttt.com/trigger/" + eventName + "/with/key/" + webhookKey;
                String response = this.POSTRequest(getUrl, content, "application/json");
                return response.ToLower().Contains("congratulations") ? true : false;
            }
            catch (Exception ex)
            {
                //log
                return false;
            }
        }
    }
}
