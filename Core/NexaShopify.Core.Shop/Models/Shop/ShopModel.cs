using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexaShopify.Core.Shop.Models.Shop
{
    public class ShopModel
    {
        public string description { get; set; }
        public string email { get; set; }
        public int id { get; set; }
        public string logo { get; set; }
        public string name { get; set; }
        public string phone { get; set; }

        public ShopModel()
        {
            
        }


        //Parameterized Constructor
        public ShopModel(Infrastructure.Data.Entities.Tables.Shop.ShopEntity x )
        {
            description = x.description.ToString();
            logo = x.logo.ToString();
            name = x.name.ToString();
            phone = x.phone.ToString();    
        }


      
        public Infrastructure.Data.Entities.Tables.Shop.ShopEntity ToShopEntity()
        {
            return new Infrastructure.Data.Entities.Tables.Shop.ShopEntity
            {
                description = this.description.ToString(),
                email = email.ToString(),
                id = id,
                logo = logo,
                name = name.ToString(),
                phone = phone.ToString(),
            };
        }

    }
}
