using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables
{
    public class StoreEntity
    {
		public string Address { get; set; }
		public DateTime? CreatedAt { get; set; }
		public string Description { get; set; }
		public bool? IsActive { get; set; }
		public string Name { get; set; }
		public long OwnerId { get; set; }
		public long StoreId { get; set; }
		public string StoreImageUrl { get; set; }
		public string StoreLogoUrl { get; set; }
		public int SubscriptionPlan { get; set; }

        public StoreEntity() { }

        public StoreEntity(DataRow dataRow)
        {
			Address = (dataRow["Address"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Address"]);
			CreatedAt = (dataRow["CreatedAt"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreatedAt"]);
			Description = (dataRow["Description"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Description"]);
			IsActive = (dataRow["IsActive"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["IsActive"]);
			Name = Convert.ToString(dataRow["Name"]);
			OwnerId = Convert.ToInt64(dataRow["OwnerId"]);
			StoreId = Convert.ToInt64(dataRow["StoreId"]);
			StoreImageUrl = (dataRow["StoreImageUrl"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["StoreImageUrl"]);
			StoreLogoUrl = (dataRow["StoreLogoUrl"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["StoreLogoUrl"]);
			SubscriptionPlan = Convert.ToInt32(dataRow["SubscriptionPlan"]);
        }
    }
}

