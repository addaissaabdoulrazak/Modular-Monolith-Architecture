using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables
{
    public class CartEntity
    {
		public long CartId { get; set; }
		public DateTime? CreatedAt { get; set; }
		public long CustomerId { get; set; }

        public CartEntity() { }

        public CartEntity(DataRow dataRow)
        {
			CartId = Convert.ToInt64(dataRow["CartId"]);
			CreatedAt = (dataRow["CreatedAt"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreatedAt"]);
			CustomerId = Convert.ToInt64(dataRow["CustomerId"]);
        }
    }
}

