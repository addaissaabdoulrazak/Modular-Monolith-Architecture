using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables
{
    public class ProductEntity
    {			public decimal BasePrice { get; set; }
		public int CategoryId { get; set; }
		public DateTime? CreatedAt { get; set; }
		public string Name { get; set; }
		public long ProductId { get; set; }
		public long StoreId { get; set; }
		public DateTime? UpdatedAt { get; set; }

        public ProductEntity() { }

        public ProductEntity(DataRow dataRow)
        {
			BasePrice = Convert.ToDecimal(dataRow["BasePrice"]);
			CategoryId = Convert.ToInt32(dataRow["CategoryId"]);
			CreatedAt = (dataRow["CreatedAt"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreatedAt"]);
			Name = Convert.ToString(dataRow["Name"]);
			ProductId = Convert.ToInt64(dataRow["ProductId"]);
			StoreId = Convert.ToInt64(dataRow["StoreId"]);
			UpdatedAt = (dataRow["UpdatedAt"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["UpdatedAt"]);
        }
    }
}

