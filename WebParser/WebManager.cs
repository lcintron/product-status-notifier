using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Web;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;

namespace WebParser.WebManagers
{
    public class WebManager
    {
        private HttpContextBase Context;

        protected String GetRequest(String url)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            StringBuilder htmlContent = new StringBuilder();

            try
            {
                // Creates a web request with specified URL
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);

                webRequest.Method = "GET";
                // Make persistent connection to the remote server
                webRequest.KeepAlive = true;
                webRequest.AllowAutoRedirect = true;
                // Creates the web response for URL
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();

                // Creates new instance of text reader and tells garbage collector to dispose of the resource when complete
                using (StreamReader streamReader = new StreamReader(webResponse.GetResponseStream()))
                {
                    string html = streamReader.ReadToEnd();
                    streamReader.Close();

                    // Convert the HTML (bad) to well-formed XHTML (good) so we can easily get data

                    return html;
                }
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }

        protected String POSTRequest(String post_url, String content_to_post, String contenttype="")
        {
            try
            {
                // create a new web request
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(post_url);
                httpWebRequest.ContentType = contenttype;
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    String content = content_to_post;

                    streamWriter.Write(content);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    return result;
                }

                return String.Empty;

            }
            catch (Exception)
            {
                return String.Empty;
            }
        }
    }
}
