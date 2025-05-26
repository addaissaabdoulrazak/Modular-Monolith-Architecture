using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables
{
    public class VariantOptionValueEntity
    {
		public int ValueId { get; set; }
		public long VariantId { get; set; }

        public VariantOptionValueEntity() { }

        public VariantOptionValueEntity(DataRow dataRow)
        {
			ValueId = Convert.ToInt32(dataRow["ValueId"]);
			VariantId = Convert.ToInt64(dataRow["VariantId"]);
        }
    }
}

