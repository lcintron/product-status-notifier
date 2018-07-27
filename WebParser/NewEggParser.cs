using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WebParser.Parsers
{
    public class NewEggProductInfo
    {
        public String unitPrice { get; set; }
        public bool instock { get; set; }

    }

    public class NewEggJsonObject
    {
        public NewEggProductInfo mainItem { get; set; }
    }
    public class NewEggParser:WebParser
    {

        public override async Task<Product> RequestProduct(String Url)
        {
            Product product = new Product();
            product.Url = Url;
            if (Url.ToLower().Contains("newegg.com"))
            {
                try
                {
                    
                    var queryStrings = HttpUtility.ParseQueryString(Url.Split(new String[] {"?"}, StringSplitOptions.RemoveEmptyEntries).Last());
                    String itemId = String.Empty;//queryStrings["Item"];
                    if (!String.IsNullOrEmpty(itemId))
                    {
                        Url = "https://www.newegg.com/LandingPage/ItemInfo4ProductDetail2016.aspx?Item=" + itemId;
                        IDocument productDoc = await this.GetWeb(Url);
                        String scriptData = productDoc.TextContent;
                        scriptData = scriptData.Replace("var Product={};", "").Replace("var rawItemInfo =", "").Replace("Product=rawItemInfo;","");

                    }
                    else
                    {
                        char[] charsToTrim = { ' ', '\n', '\t' };
                        IDocument productDoc = await this.GetWeb(Url);
                        String scriptData = productDoc.All.Where(t => t.LocalName.Equals("script") && t.TextContent.Contains("utag_data")).FirstOrDefault().TextContent;
                        List<String> productDataFromJS = new List<String>();
                        if (scriptData != null)
                        {
                            String[] splitS = { "\n" };
                            productDataFromJS = scriptData.Split(splitS, StringSplitOptions.RemoveEmptyEntries).ToList();
                        }
                        product.Name = productDoc.QuerySelector("#grpDescrip_h").TextContent.Trim(charsToTrim);
                        if (productDataFromJS.Any())
                        {
                            String availability = GetJavascriptProductTagValue("product_instock", productDataFromJS);
                            product.Availability = availability.Trim().Equals("0") ? "Out-of-Stock" : "In-Stock";
                            product.Price = GetJavascriptProductTagValue("product_sale_price", productDataFromJS);
                        }
                        product.Name = product.Name.Trim(charsToTrim);
                        product.Availability = product.Availability.Trim(charsToTrim);
                        product.Price = product.Price.Trim(charsToTrim);
                        product.Valid = true;
                    }
                }
                catch(Exception ex)
                {
                    product.Name =product.Name?? String.Empty;
                    product.Availability = product.Availability??String.Empty;
                    product.Price = product.Price?? String.Empty;
                    product.Valid = false;
                }
                
            }
            return product;
        }

        private String GetJavascriptProductTagValue(String value, List<String> jsData)
        {
            String exists = jsData.Where(t => t.Contains(value)).FirstOrDefault();
            if(!String.IsNullOrEmpty(exists))
            {
                String[] sep = { "['", "']"};
                String val = exists.Split(sep, StringSplitOptions.RemoveEmptyEntries).ElementAtOrDefault(1);
                return val;
            }

            return String.Empty;
        }

    }
}
