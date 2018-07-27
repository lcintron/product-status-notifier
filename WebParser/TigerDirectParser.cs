using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Cintron.LoggerCin;
using AngleSharp.Network.Default;
using AngleSharp;

namespace WebParser.Parsers
{
    public class TigerDirectParser : WebParser
    {
        private String browserUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.108 Safari/537.36 Accept: text / html, application / xhtml + xml, application / xml; q = 0.9,image / webp,image / apng,*/*;q=0.8";
        private String accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
        private String acceptLanguage = "en-US,en;q=0.9,es;q=0.8";
        public override async Task<Product> RequestProduct(String Url)
        {

            Product product = new Product();
            product.Url = Url;
            if (Url.ToLower().Contains("tigerdirect.com"))
            {
                try
                {
                    char[] charsToTrim = { ' ', '\n', '\t' };
                    IDocument productDoc = await this.GetWeb(Url);


                    String name = productDoc.QuerySelector("div.prodName") != null ? productDoc.QuerySelector("div.prodName h1").TextContent.
                        Trim(charsToTrim).Replace("\n", " ").Replace("\t", "") : String.Empty;
                    product.Name = name;

                    String price = productDoc.QuerySelector(".salePrice") != null ? productDoc.QuerySelector("span.salePrice").TextContent.
                        Trim(charsToTrim) : String.Empty;
                    product.Price = price;

                    String availability= productDoc.QuerySelectorAll(".prodMesg") != null ? productDoc.QuerySelector(".prodMesg").TextContent.Trim(charsToTrim): String.Empty;
                    product.Availability = availability.ToLower().Contains("in stock") ? "In-Stock" : "Out-of-Stock";

                    product.Valid = true;
                }
                catch (Exception ex)
                {
                    LoggerCin logger = new LoggerCin("C:\\Development\\ProductStatusNotifier\\Test\\bin\\Debug");
                    logger.Error(ex.ToString());
                    product.Valid = false;
                }

            }
            return product;

        }

        protected override async Task<IDocument> GetWeb(String Uri)
        {
            try
            {
                var requester = new HttpRequester();
                requester.Headers["User-Agent"] = browserUserAgent;
                requester.Headers["Accept-Language"] = acceptLanguage;
                requester.Headers["Accept"] = accept;
                var config = Configuration.Default.WithDefaultLoader(requesters: new[] { requester }).WithCss().WithCookies().WithJavaScript();

                // We create a new context
                var context = BrowsingContext.New(config);
                IDocument response = await context.OpenAsync(Uri);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
