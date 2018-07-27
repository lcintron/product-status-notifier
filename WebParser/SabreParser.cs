using AngleSharp.Dom;
using Cintron.LoggerCin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebParser
{
    public class SabreParser: WebParser
    {
        public override async Task<Product> RequestProduct(String Url)
        {

            Product product = new Product();
            product.Url = Url;
            if (Url.ToLower().Contains("sabrepc.com"))
            {
                try
                {
                    char[] charsToTrim = { ' ', '\n', '\t' };
                    IDocument productDoc = await this.GetWeb(Url);
                    if (productDoc == null)
                    {
                        product.Valid = false;
                        return product;
                    }
                    String name = productDoc.QuerySelector("div.product-name") != null ? productDoc.QuerySelector("div.product-name").FirstElementChild.TextContent.
                        Trim(charsToTrim).Replace("\n", " ").Replace("\t", "").Trim() : String.Empty;
                    product.Name = name;

                    var priceNode = productDoc.QuerySelector("div.product-d").Children.Where(x => x.ClassName.Contains("integral")).FirstOrDefault();
                    String price = priceNode != null ? priceNode.TextContent : String.Empty;
                    product.Price = price;

                    var availabilityNode = productDoc.QuerySelectorAll("span").Where(n => n.ParentElement.ClassName.ToLower().Equals("availability in-stock")).FirstOrDefault();
                    string availability = availabilityNode != null ? availabilityNode.TextContent : String.Empty;
                    product.Availability = availability.Equals("Out Of Stock") ? "Out-of-Stock" : "In-Stock";

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
