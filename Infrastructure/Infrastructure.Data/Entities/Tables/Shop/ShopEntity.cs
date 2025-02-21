using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Tables.Shop
{
    public class ShopEntity
    {
        public string address { get; set; }
        public string banner { get; set; }
        public DateTime? created_at { get; set; }
        public string custom_url { get; set; }
        public string description { get; set; }
        public string email { get; set; }
        public int id { get; set; }
        public string logo { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public DateTime? updated_at { get; set; }

        public ShopEntity() { }

        public ShopEntity(DataRow dataRow)
        {
            address = (dataRow["address"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["address"]);
            banner = (dataRow["banner"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["banner"]);
            created_at = (dataRow["created_at"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["created_at"]);
            custom_url = (dataRow["custom_url"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["custom_url"]);
            description = (dataRow["description"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["description"]);
            email = Convert.ToString(dataRow["email"]);
            id = Convert.ToInt32(dataRow["id"]);
            logo = (dataRow["logo"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["logo"]);
            name = Convert.ToString(dataRow["name"]);
            phone = (dataRow["phone"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["phone"]);
            updated_at = (dataRow["updated_at"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["updated_at"]);
        }
    }
}
