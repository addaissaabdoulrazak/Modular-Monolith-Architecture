using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables
{
    public class ProductOptionValueEntity
    {
		public int OptionId { get; set; }
		public string Value { get; set; }
		public int ValueId { get; set; }

        public ProductOptionValueEntity() { }

        public ProductOptionValueEntity(DataRow dataRow)
        {
			OptionId = Convert.ToInt32(dataRow["OptionId"]);
			Value = Convert.ToString(dataRow["Value"]);
			ValueId = Convert.ToInt32(dataRow["ValueId"]);
        }
    }
}

