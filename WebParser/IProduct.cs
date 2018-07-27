using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebParser
{
    public enum ProductType { NewEgg, BHPhotoVideo, TigerDirect};
    public class Product
    {
        public String Name { get; set; }
        public String Price { get; set; }
        public String Availability { get; set; }
        private String url;
        public String Url
        {
            get
            {
                return url;
            }
            set
            {
                if (value.Contains("newegg.com"))
                    this.Type = ProductType.NewEgg;
                else if (value.Contains("bhphotovideo.com"))
                    this.Type = ProductType.BHPhotoVideo;
                else if (value.Contains("tigerdirect.com"))
                    this.Type = ProductType.TigerDirect;

                url = value;
            }
        }

        public bool Equals(Product product)
        {
            return this.Name.Equals(product.Name)
            && this.Price.Equals(product.Price)
            && this.Availability.Equals(product.Availability);


        }

        public void CopyFrom(Product product)
        {
            //do nothing
            this.Name = product.Name;
            this.Price = product.Price;
            this.Availability = product.Availability;
            this.Url = product.Url;
            this.Type = product.Type;
            this.Valid = true;
        }

        public bool Valid { get; set; }

        public ProductType Type { get; set; }

        public String StoreName {
            get
            {
                String name = "";
                switch(this.Type)
                {
                    case ProductType.NewEgg:
                        name = "NewEgg";
                        break;
                    case ProductType.BHPhotoVideo:
                        name = "BHPhotoVideo";
                        break;
                    case ProductType.TigerDirect:
                        name = "TigerDirect";
                        break;
                    default:
                        name = String.Empty;
                        break;
                }
                return name;

            }
        }


    }
}
