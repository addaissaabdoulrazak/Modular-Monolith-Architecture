using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables
{
    public class ProductVariantEntity
    {
		public decimal? PriceAdjustment { get; set; }
		public long ProductId { get; set; }
		public string SKU { get; set; }
		public int Stock { get; set; }
		public long VariantId { get; set; }

        public ProductVariantEntity() { }

        public ProductVariantEntity(DataRow dataRow)
        {
			PriceAdjustment = (dataRow["PriceAdjustment"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["PriceAdjustment"]);
			ProductId = Convert.ToInt64(dataRow["ProductId"]);
			SKU = (dataRow["SKU"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SKU"]);
			Stock = Convert.ToInt32(dataRow["Stock"]);
			VariantId = Convert.ToInt64(dataRow["VariantId"]);
        }
    }
}

