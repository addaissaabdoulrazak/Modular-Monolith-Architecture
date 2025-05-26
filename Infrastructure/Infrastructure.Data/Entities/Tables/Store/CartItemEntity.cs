using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables
{
    public class CartItemEntity
    {
		public long CartId { get; set; }
		public long CartItemId { get; set; }
		public long ProductVariantId { get; set; }
		public int Quantity { get; set; }

        public CartItemEntity() { }

        public CartItemEntity(DataRow dataRow)
        {
			CartId = Convert.ToInt64(dataRow["CartId"]);
			CartItemId = Convert.ToInt64(dataRow["CartItemId"]);
			ProductVariantId = Convert.ToInt64(dataRow["ProductVariantId"]);
			Quantity = Convert.ToInt32(dataRow["Quantity"]);
        }
    }
}

