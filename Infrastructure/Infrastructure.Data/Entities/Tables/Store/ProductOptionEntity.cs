using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables
{
    public class ProductOptionEntity
    {
		public int OptionId { get; set; }
		public string OptionName { get; set; }
		public long ProductId { get; set; }

        public ProductOptionEntity() { }

        public ProductOptionEntity(DataRow dataRow)
        {
			OptionId = Convert.ToInt32(dataRow["OptionId"]);
			OptionName = Convert.ToString(dataRow["OptionName"]);
			ProductId = Convert.ToInt64(dataRow["ProductId"]);
        }
    }
}

