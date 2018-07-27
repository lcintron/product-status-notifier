using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Cintron.LoggerCin;

namespace WebParser.Parsers
{
    public class BHParser:WebParser
    {
        public override async Task<Product> RequestProduct(String Url)
        {

            Product product = new Product();
            product.Url = Url;
            if (Url.ToLower().Contains("bhphotovideo.com"))
            {
                try
                {
                    char[] charsToTrim = { ' ', '\n', '\t' };
                    IDocument productDoc = await this.GetWeb(Url);
                    if(productDoc == null)
                    {
                        product.Valid = false;
                        return product;
                    }
                    String name = productDoc.QuerySelector(".js-main-product-name") != null ? productDoc.QuerySelector(".js-main-product-name").TextContent.
                        Trim(charsToTrim).Replace("\n", " ").Replace("\t", "") : String.Empty;
                    product.Name = name;

                    String price =  productDoc.QuerySelector("span.ypYouPay") != null ? productDoc.QuerySelector("span.ypYouPay").TextContent.
                        Trim(charsToTrim) : String.Empty;
                    product.Price = price;

                    var availabilityNode = productDoc.QuerySelectorAll("span").Where(t => t.HasAttribute("data-selenium") && (t.GetAttribute("data-selenium").
                            Equals("inStock") || t.GetAttribute("data-selenium").Equals("notStock"))).FirstOrDefault();
                    String availability = availabilityNode!=null?availabilityNode.GetAttribute("data-selenium"):String.Empty;
                    product.Availability = availability.Equals("inStock")?"In-Stock":"Out-of-Stock";

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
    }
}
