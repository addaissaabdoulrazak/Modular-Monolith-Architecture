using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables
{
    public class DeliveryServiceEntity
    {
		public string Contact { get; set; }
		public string CoverageAreas { get; set; }
		public DateTime? CreatedAt { get; set; }	
		public decimal DeliveryFee { get; set; }
		public bool? IsActive { get; set; }
		public string Name { get; set; }
		public int ServiceId { get; set; }

        public DeliveryServiceEntity() { }

        public DeliveryServiceEntity(DataRow dataRow)
        {
			Contact = Convert.ToString(dataRow["Contact"]);
			CoverageAreas = (dataRow["CoverageAreas"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CoverageAreas"]);
			CreatedAt = (dataRow["CreatedAt"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreatedAt"]);
			DeliveryFee = Convert.ToDecimal(dataRow["DeliveryFee"]);
			IsActive = (dataRow["IsActive"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["IsActive"]);
			Name = Convert.ToString(dataRow["Name"]);
			ServiceId = Convert.ToInt32(dataRow["ServiceId"]);
        }
    }
}

